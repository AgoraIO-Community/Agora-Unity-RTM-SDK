using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using AOT;

namespace agora_rtm {
	public sealed class RtmClientEventHandler : IRtmApiNative {
		private int currentIdIndex = 0;
		private static int _id = 0;
		private IntPtr _rtmClientEventHandlerPtr = IntPtr.Zero;
		private static Dictionary<int, RtmClientEventHandler> clientEventHandlerHandlerDic = new Dictionary<int, RtmClientEventHandler>();
		/// <summary>
		/// 登录 Agora RTM 系统成功回调。
		/// 当用户调用 \ref agora_rtm.RtmClient.Login "Login" 方法成功加入频道时，本地用户会收到此回调。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		public delegate void OnLoginSuccessHandler(int id);

		/// <summary>
		/// 登录 Agora RTM 系统失败回调。
		/// 当 \ref agora_rtm.RtmClient.Login "Login" 方法调用失败时，本地用户会收到此回调。
		/// </summary>
		/// <param name="id">
		/// 引擎 ID 
		/// </param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.LOGIN_ERR_CODE "LOGIN_ERR_CODE"。</param>
		public delegate void OnLoginFailureHandler(int id, LOGIN_ERR_CODE errorCode);

		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.RenewToken "RenewToken" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="token">新的 Token。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.RENEW_TOKEN_ERR_CODE "RENEW_TOKEN_ERR_CODE"。</param>
		public delegate void OnRenewTokenResultHandler(int id, string token, RENEW_TOKEN_ERR_CODE errorCode);
		
		/// <summary>
		///（SDK 断线重连时触发）当前使用的 RTM Token 已超过 24 小时的签发有效期。
		/// 该回调仅会在 SDK 处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" 状态时因 RTM 后台监测到 Token 签发有效期过期而触发。SDK 处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_CONNECTED "CONNECTION_STATE_CONNECTED" 状态时该回调不会被触发。
		/// 收到该回调时，请尽快在你的业务服务端生成新的 Token 并调用 \ref agora_rtm.RtmClient.RenewToken "RenewToken" 方法把新的 Token 传给 Token 验证服务器。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		public delegate void OnTokenExpiredHandler(int id);

		/// <summary>
		/// 登出 Agora RTM 服务回调。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.LOGOUT_ERR_CODE "LOGOUT_ERR_CODE"。 </param>
		public delegate void OnLogoutHandler(int id, LOGOUT_ERR_CODE errorCode);

		/// <summary>
		/// SDK 与 Agora RTM 系统的连接状态发生改变回调。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="state">新连接状态。详见 \ref agora_rtm.CONNECTION_STATE "CONNECTION_STATE"。</param>
		/// <param name="reason">连接状态改变原因。详见 \ref agora_rtm.CONNECTION_CHANGE_REASON "CONNECTION_CHANGE_REASON"。</param>
		public delegate void OnConnectionStateChangedHandler(int id, CONNECTION_STATE state, CONNECTION_CHANGE_REASON reason);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.SendMessageToPeer "SendMessageToPeer" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="messageId">点对点消息的 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.PEER_MESSAGE_ERR_CODE "PEER_MESSAGE_ERR_CODE"。</param>
		public delegate void OnSendMessageResultHandler(int id, Int64 messageId, PEER_MESSAGE_ERR_CODE errorCode);
		
		/// <summary>
		/// 收到点对点消息回调。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="peerId">发送该消息的对端用户 ID。</param>
		/// <param name="message">接收到的消息。详见 \ref agora_rtm.IMessage "IMessage"。</param>
		public delegate void OnMessageReceivedFromPeerHandler(int id, string peerId, TextMessage message);
		
		/// <summary>
		/// 收到点对点图片消息回调。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="peerId">发送该消息的对端用户 ID。</param>
		/// <param name="message">T接收到的图片消息。 详见 \ref agora_rtm.ImageMessage "ImageMessage"。</param>
		public delegate void OnImageMessageReceivedFromPeerHandler(int id, string peerId, ImageMessage message);
		
		/// <summary>
		/// 收到点对点文件消息回调。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="peerId">发送该消息的对端用户 ID。</param>
		/// <param name="message">接收到的文件消息。详见 \ref agora_rtm.FileMessage "FileMessage"。</param>
		public delegate void OnFileMessageReceivedFromPeerHandler(int id, string peerId, FileMessage message);
		
		/// <summary>
		/// 主动回调：上传任务的上传进度回调。
		/// @note 
		///  - 如果上传任务正在不断进行中，SDK 每秒返回该回调一次。
		///  - 如果上传任务停顿，SDK 会暂停返回该回调直到上传任务重新进行。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次上传请求的的唯一 ID。</param>
		/// <param name="progress">文件或图片的上传进度。详见 \ref agora_rtm.MediaOperationProgress "MediaOperationProgress"。</param>
		public delegate void OnMediaUploadingProgressHandler(int id, Int64 requestId, MediaOperationProgress progress);
		
		/// <summary>
		/// 主动回调：下载任务的下载进度回调。
		/// @note
		///  - 如果下载任务正在不断进行中，SDK 每秒返回该回调一次。
		///  - 如果下载任务停顿，SDK 会暂停返回该回调直到上传任务重新进行。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次下载请求的的唯一 ID。</param>
		/// <param name="progress">文件或图片的下载进度。详见 \ref agora_rtm.MediaOperationProgress "MediaOperationProgress"。</param>
		public delegate void OnMediaDownloadingProgressHandler(int id, Int64 requestId, MediaOperationProgress progress);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.CreateFileMessageByUploading "CreateFileMessageByUploading" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次上传请求的唯一 ID。</param>
		/// <param name="fileMessage">一个 \ref agora_rtm.FileMessage "FileMessage" 实例。</param>
		/// <param name="code">错误码。详见 \ref agora_rtm.UPLOAD_MEDIA_ERR_CODE "UPLOAD_MEDIA_ERR_CODE".</param>
		public delegate void OnFileMediaUploadResultHandler(int id, Int64 requestId, FileMessage fileMessage, UPLOAD_MEDIA_ERR_CODE code);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.CreateImageMessageByUploading "CreateImageMessageByUploading" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次上传请求的唯一 ID。</param>
		/// <param name="imageMessage">是一个 \ref agora_rtm.ImageMessage "ImageMessage" 实例。</param>
		/// <param name="code">错误码。详见 \ref agora_rtm.UPLOAD_MEDIA_ERR_CODE "UPLOAD_MEDIA_ERR_CODE"。</param>
		public delegate void OnImageMediaUploadResultHandler(int id, Int64 requestId, ImageMessage imageMessage, UPLOAD_MEDIA_ERR_CODE code);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.DownloadMediaToFile "DownloadMediaToFile" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次下载请求的唯一 ID。</param>
		/// <param name="code">错误码。详见 \ref agora_rtm.DOWNLOAD_MEDIA_ERR_CODE "DOWNLOAD_MEDIA_ERR_CODE".</param>
		public delegate void OnMediaDownloadToFileResultHandler(int id, Int64 requestId, DOWNLOAD_MEDIA_ERR_CODE code);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.DownloadMediaToMemory "DownloadMediaToMemory" 方法的调用结果。
		/// @note 回调结束后，SDK 会立刻释放下载的图片或文件。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次下载请求的唯一 ID。</param>
		/// <param name="memory">下载文件或图片的内存存放地址。</param>
		/// <param name="length">下载文件或图片的数据长度。</param>
		/// <param name="code">错误码。详见 \ref agora_rtm.DOWNLOAD_MEDIA_ERR_CODE "DOWNLOAD_MEDIA_ERR_CODE"。</param>
		public delegate void OnMediaDownloadToMemoryResultHandler(int id, Int64 requestId, byte[] memory, Int64 length, DOWNLOAD_MEDIA_ERR_CODE code);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.CancelMediaDownload "CancelMediaDownload" 或 \ref agora_rtm.RtmClient.CancelMediaUpload "CancelMediaUpload" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次取消请求的唯一 ID。</param>
		/// <param name="code">错误码。详见 \ref agora_rtm.CANCEL_MEDIA_ERR_CODE "CANCEL_MEDIA_ERR_CODE"。</param>
		public delegate void OnMediaCancelResultHandler(int id, Int64 requestId, CANCEL_MEDIA_ERR_CODE code);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.QueryPeersOnlineStatus "QueryPeersOnlineStatus" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="peersStatus">用户的在线状态。详见 \ref agora_rtm.QUERY_PEERS_ONLINE_STATUS_ERR "QUERY_PEERS_ONLINE_STATUS_ERR"。</param>
		/// <param name="peerCount">指定用户的数量。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.QUERY_PEERS_ONLINE_STATUS_ERR "QUERY_PEERS_ONLINE_STATUS_ERR"。</param>
		public delegate void OnQueryPeersOnlineStatusResultHandler(int id, Int64 requestId, PeerOnlineStatus[] peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.SubscribePeersOnlineStatus "SubscribePeersOnlineStatus" 或 \ref agora_rtm.RtmClient.UnsubscribePeersOnlineStatus	"UnsubscribePeersOnlineStatus" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.PEER_SUBSCRIPTION_STATUS_ERR "PEER_SUBSCRIPTION_STATUS_ERR"。</param>
		public delegate void OnSubscriptionRequestResultHandler(int id, Int64 requestId, PEER_SUBSCRIPTION_STATUS_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.QueryPeersBySubscriptionOption "QueryPeersBySubscriptionOption" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="peerIds">用户 ID 列表。</param>
		/// <param name="peerCount">某订阅类型被订阅的用户人数。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR "QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR"。</param>
		public delegate void OnQueryPeersBySubscriptionOptionResultHandler(int id, Int64 requestId, string[] peerIds, int peerCount, QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode);
		
		/// @cond not-for-doc
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR".</param>
		public delegate void OnSetLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		/// @endcond
		
		/// @cond not-for-doc
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR"。</param>
		public delegate void OnAddOrUpdateLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		/// @endcond
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.DeleteLocalUserAttributesByKeys "DeleteLocalUserAttributesByKeys" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR"。</param>
		public delegate void OnDeleteLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.ClearLocalUserAttributes "ClearLocalUserAttributes" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR"。</param>
		public delegate void OnClearLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.GetUserAttributes "GetUserAttributes" 或 \ref agora_rtm.RtmClient.GetUserAttributesByKeys "GetUserAttributesByKeys" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="userId">指定用户的用户 ID。</param>
		/// <param name="attributes">返回的属性数组。详见 \ref agora_rtm.RtmAttribute "RtmAttribute"。</param>
		/// <param name="numberOfAttributes">用户属性数组的长度。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR"。</param>
		public delegate void OnGetUserAttributesResultHandler(int id, Int64 requestId, string userId, RtmAttribute[] attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.SetChannelAttributes "SetChannelAttributes" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR"。</param>
		public delegate void OnSetChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// @cond not-for-doc
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。</param>
		public delegate void OnAddOrUpdateChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		/// @endcond
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.DeleteChannelAttributesByKeys "DeleteChannelAttributesByKeys" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR"。</param>
		public delegate void OnDeleteChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.ClearChannelAttributes "ClearChannelAttributes" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR"。</param>
		public delegate void OnClearChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.GetChannelAttributes "GetChannelAttributes" 或 "GetChannelAttributesByKeys" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="attributes">频道属性数组。</param>
		/// <param name="numberOfAttributes">频道属性的条数。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.ATTRIBUTE_OPERATION_ERR "ATTRIBUTE_OPERATION_ERR".</param>
		public delegate void OnGetChannelAttributesResultHandler(int id, Int64 requestId, RtmChannelAttribute[] attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// 报告 \ref agora_rtm.RtmClient.GetChannelMemberCount "GetChannelMemberCount" 方法的调用结果。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="requestId">标识本次请求的的唯一 ID。</param>
		/// <param name="channelMemberCounts">频道成员人数数组。</param>
		/// <param name="channelCount">频道数量。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.GET_CHANNEL_MEMBER_COUNT_ERR_CODE "GET_CHANNEL_MEMBER_COUNT_ERR_CODE"。</param>
		public delegate void OnGetChannelMemberCountResultHandler(int id, Int64 requestId, ChannelMemberCount[] channelMemberCounts , int channelCount, GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode);
		
		/// <summary>
		/// 被订阅用户在线状态改变回调。
		/// - 首次订阅在线状态成功时，SDK 也会返回本回调，显示所有被订阅用户的在线状态。
		/// - 每当被订阅用户的在线状态发生改变，SDK 都会通过该回调通知订阅方。
		/// - 如果 SDK 在断线重连过程中有被订阅用户的在线状态发生改变，SDK 会在重连成功时通过该回调通知订阅方。
		/// </summary>
		/// <param name="id">引擎 ID</param>
		/// <param name="peersStatus">用户在线状态列表。详见 \ref agora_rtm.PeerOnlineStatus "PeerOnlineStatus"。</param>
		/// <param name="peerCount">在线状态发生变化的被订阅用户人数。</param>
		public delegate void OnPeersOnlineStatusChangedHandler(int id, PeerOnlineStatus[] peersStatus, int peerCount);

		public OnLoginSuccessHandler OnLoginSuccess;
		public OnLoginFailureHandler OnLoginFailure;
		public OnRenewTokenResultHandler OnRenewTokenResult;
		public OnTokenExpiredHandler OnTokenExpired;
		public OnLogoutHandler OnLogout;
		public OnConnectionStateChangedHandler OnConnectionStateChanged;
		public OnSendMessageResultHandler OnSendMessageResult;
		public OnMessageReceivedFromPeerHandler OnMessageReceivedFromPeer;
		public OnImageMessageReceivedFromPeerHandler OnImageMessageReceivedFromPeer;
		public OnFileMessageReceivedFromPeerHandler OnFileMessageReceivedFromPeer;
		public OnMediaUploadingProgressHandler OnMediaUploadingProgress;
		public OnMediaDownloadingProgressHandler OnMediaDownloadingProgress;
		public OnFileMediaUploadResultHandler OnFileMediaUploadResult;
		public OnImageMediaUploadResultHandler OnImageMediaUploadResult;
		public OnMediaDownloadToFileResultHandler OnMediaDownloadToFileResult;
		public OnMediaDownloadToMemoryResultHandler OnMediaDownloadToMemoryResult;
		public OnMediaCancelResultHandler OnMediaCancelResult;
		public OnQueryPeersOnlineStatusResultHandler OnQueryPeersOnlineStatusResult;
		public OnSubscriptionRequestResultHandler OnSubscriptionRequestResult;
		public OnQueryPeersBySubscriptionOptionResultHandler OnQueryPeersBySubscriptionOptionResult;
		public OnSetLocalUserAttributesResultHandler OnSetLocalUserAttributesResult;
		public OnAddOrUpdateLocalUserAttributesResultHandler OnAddOrUpdateLocalUserAttributesResult;
		public OnDeleteLocalUserAttributesResultHandler OnDeleteLocalUserAttributesResult;
		public OnClearLocalUserAttributesResultHandler OnClearLocalUserAttributesResult;
		public OnGetUserAttributesResultHandler OnGetUserAttributesResult;
		public OnSetChannelAttributesResultHandler OnSetChannelAttributesResult;
		public OnAddOrUpdateChannelAttributesResultHandler OnAddOrUpdateChannelAttributesResult;
		public OnDeleteChannelAttributesResultHandler OnDeleteChannelAttributesResult;
		public OnClearChannelAttributesResultHandler OnClearChannelAttributesResult;
		public OnGetChannelAttributesResultHandler OnGetChannelAttributesResult;
		public OnGetChannelMemberCountResultHandler OnGetChannelMemberCountResult;
		public OnPeersOnlineStatusChangedHandler OnPeersOnlineStatusChanged;


		public RtmClientEventHandler() {
			currentIdIndex = _id;
			clientEventHandlerHandlerDic.Add(currentIdIndex, this);
			_rtmClientEventHandlerPtr = service_event_handler_createEventHandle(currentIdIndex, OnLoginSuccessCallback,
																				OnLoginFailureCallback,
																				OnRenewTokenResultCallback,
																				OnTokenExpiredCallback,
																				OnLogoutCallback,
																				OnConnectionStateChangedCallback,
																				OnSendMessageResultCallback,
																				OnMessageReceivedFromPeerCallback,
																				OnImageMessageReceivedFromPeerCallback,
																				OnFileMessageReceivedFromPeerCallback,
																				OnMediaUploadingProgressCallback,
																				OnMediaDownloadingProgressCallback,
																				OnFileMediaUploadResultCallback,
																				OnImageMediaUploadResultCallback,
																				OnMediaDownloadToFileResultCallback,
																				OnMediaDownloadToMemoryResultCallback,
																				OnMediaCancelResultCallback,
																				OnQueryPeersOnlineStatusResultCallback,
																				OnSubscriptionRequestResultCallback,
																				OnQueryPeersBySubscriptionOptionResultCallback,
																				OnPeersOnlineStatusChangedCallback,
																				OnSetLocalUserAttributesResultCallback,
																				OnDeleteLocalUserAttributesResultCallback,
																				OnClearLocalUserAttributesResultCallback,
																				OnGetUserAttributesResultCallback,
																				OnSetChannelAttributesResultCallback,
																				OnAddOrUpdateLocalUserAttributesResultCallback,
																				OnDeleteChannelAttributesResultCallback,
																				OnClearChannelAttributesResultCallback,
																				OnGetChannelAttributesResultCallback,
																				OnGetChannelMemberCountResultCallback);
			_id ++;
		}

		public void Release() {
			Debug.Log("RtmClientEventHandler Released");
			if (_rtmClientEventHandlerPtr == IntPtr.Zero) {
				return;
			}
			clientEventHandlerHandlerDic.Remove(currentIdIndex);
			service_event_handler_releaseEventHandler(_rtmClientEventHandlerPtr);
			_rtmClientEventHandlerPtr = IntPtr.Zero;
		}

		public IntPtr GetRtmClientEventHandlerPtr() {
			return _rtmClientEventHandlerPtr;
		}
		
		[MonoPInvokeCallback(typeof(OnLoginSuccessHandler))]
		private static void OnLoginSuccessCallback(int id) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnLoginSuccess != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnLoginSuccess != null) {
							clientEventHandlerHandlerDic[id].OnLoginSuccess(id);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnLoginFailureHandler))]
		private static void OnLoginFailureCallback(int id, LOGIN_ERR_CODE errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnLoginFailure != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnLoginFailure != null) {
							clientEventHandlerHandlerDic[id].OnLoginFailure(id, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnRenewTokenResultHandler))]
		private static void OnRenewTokenResultCallback(int id, string token, RENEW_TOKEN_ERR_CODE errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnRenewTokenResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnRenewTokenResult != null) {
							clientEventHandlerHandlerDic[id].OnRenewTokenResult(id, token, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnTokenExpiredHandler))]
		private static void OnTokenExpiredCallback(int id) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnTokenExpired != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnTokenExpired != null) {
							clientEventHandlerHandlerDic[id].OnTokenExpired(id);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnLogoutHandler))]
		private static void OnLogoutCallback(int id, LOGOUT_ERR_CODE errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnLogout != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnLogout != null) {
							clientEventHandlerHandlerDic[id].OnLogout(id, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnConnectionStateChangedHandler))]
		private static void OnConnectionStateChangedCallback(int id, CONNECTION_STATE state, CONNECTION_CHANGE_REASON reason)
		{
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnConnectionStateChanged != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnConnectionStateChanged != null) {
							clientEventHandlerHandlerDic[id].OnConnectionStateChanged(id, state, reason);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnSendMessageResultHandler))]
		private static void OnSendMessageResultCallback(int id, Int64 messageId, PEER_MESSAGE_ERR_CODE errorCode)
		{
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSendMessageResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSendMessageResult != null) {
							clientEventHandlerHandlerDic[id].OnSendMessageResult(id, messageId, errorCode);
						}
					});
				}
			}
		}
		
		[MonoPInvokeCallback(typeof(EngineEventOnMessageReceived))]
		private static void OnMessageReceivedFromPeerCallback(int id, string peerId, IntPtr message) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMessageReceivedFromPeer != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					TextMessage textMessage = new TextMessage(message, TextMessage.MESSAGE_FLAG.SEND);
					TextMessage _textMessage = new TextMessage(textMessage, TextMessage.MESSAGE_FLAG.RECEIVE);
					textMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMessageReceivedFromPeer != null) {
							clientEventHandlerHandlerDic[id].OnMessageReceivedFromPeer(id, peerId, _textMessage);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnImageMessageReceived))]
		private static void OnImageMessageReceivedFromPeerCallback(int id, string peerId, IntPtr message) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnImageMessageReceivedFromPeer != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					ImageMessage imageMessage = new ImageMessage(message, ImageMessage.MESSAGE_FLAG.SEND);
					ImageMessage _imageMessage = new ImageMessage(imageMessage, ImageMessage.MESSAGE_FLAG.RECEIVE);
					imageMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnImageMessageReceivedFromPeer != null) {
							clientEventHandlerHandlerDic[id].OnImageMessageReceivedFromPeer(id, peerId, _imageMessage);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnFileMessageReceived))]
		private static void OnFileMessageReceivedFromPeerCallback(int id, string peerId, IntPtr message) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnFileMessageReceivedFromPeer != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					FileMessage fileMessage = new FileMessage(message, FileMessage.MESSAGE_FLAG.SEND);
					FileMessage _fileMessage = new FileMessage(fileMessage, FileMessage.MESSAGE_FLAG.RECEIVE);
					fileMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnFileMessageReceivedFromPeer != null) {
							clientEventHandlerHandlerDic[id].OnFileMessageReceivedFromPeer(id, peerId, _fileMessage);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnMediaDownloadToFileResultHandler))]
		private static void OnMediaDownloadToFileResultCallback(int id, Int64 requestId, DOWNLOAD_MEDIA_ERR_CODE code) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaDownloadToFileResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaDownloadToFileResult != null) {
							clientEventHandlerHandlerDic[id].OnMediaDownloadToFileResult(id, requestId, code);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnMediaDownloadToMemoryResult))]
		private static void OnMediaDownloadToMemoryResultCallback(int id, Int64 requestId, IntPtr memory, Int64 length, DOWNLOAD_MEDIA_ERR_CODE code) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaDownloadToMemoryResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					byte[] memoryData = new byte[length];
					Marshal.Copy(memory, memoryData, 0, (int)length);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaDownloadToMemoryResult != null) {
							clientEventHandlerHandlerDic[id].OnMediaDownloadToMemoryResult(id, requestId, memoryData, length, code);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnMediaCancelResultHandler))]
		private static void OnMediaCancelResultCallback(int id, Int64 requestId, CANCEL_MEDIA_ERR_CODE code)
		{
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaCancelResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaCancelResult != null) {
							clientEventHandlerHandlerDic[id].OnMediaCancelResult(id, requestId, code);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnQueryPeersOnlineStatusResult))]
		private static void OnQueryPeersOnlineStatusResultCallback(int id, Int64 requestId, string peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnQueryPeersOnlineStatusResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnQueryPeersOnlineStatusResult != null) {
							int j = 1;
							string[] sArray = peersStatus.Split('\t');
							PeerOnlineStatus [] channelAttributes = new PeerOnlineStatus[peerCount];
							for (int i = 0; i < peerCount; i++) {
								PeerOnlineStatus peerOnlineStatus = new PeerOnlineStatus();
								// word 1 = user id string
								peerOnlineStatus.peerId = sArray[j++];
								// word 2 = online/offline (1 or 0)
								peerOnlineStatus.isOnline = 1 == int.Parse(sArray[j++]);
								// word 3 = PEER_ONLINE_STATE mapped from int to enum
								peerOnlineStatus.onlineState = (PEER_ONLINE_STATE)int.Parse(sArray[j++]);
								channelAttributes[i] = peerOnlineStatus;
							}
							clientEventHandlerHandlerDic[id].OnQueryPeersOnlineStatusResult(id, requestId, channelAttributes, peerCount, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnSubscriptionRequestResultHandler))]
		private static void OnSubscriptionRequestResultCallback(int id, Int64 requestId, PEER_SUBSCRIPTION_STATUS_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSubscriptionRequestResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSubscriptionRequestResult != null) {
							clientEventHandlerHandlerDic[id].OnSubscriptionRequestResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnSetLocalUserAttributesResultHandler))]
		private static void OnSetLocalUserAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSetLocalUserAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSetLocalUserAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnSetLocalUserAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnAddOrUpdateLocalUserAttributesResultHandler))]
		private static void OnAddOrUpdateLocalUserAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnAddOrUpdateLocalUserAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnAddOrUpdateLocalUserAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnAddOrUpdateLocalUserAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnDeleteLocalUserAttributesResultHandler))]
		private static void OnDeleteLocalUserAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnDeleteLocalUserAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnDeleteLocalUserAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnDeleteLocalUserAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnClearLocalUserAttributesResultHandler))]
		private static void OnClearLocalUserAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnClearLocalUserAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnClearLocalUserAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnClearLocalUserAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnSetChannelAttributesResultHandler))]
		private static void OnSetChannelAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSetChannelAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnSetChannelAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnSetChannelAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnAddOrUpdateChannelAttributesResultHandler))]
		private static void OnAddOrUpdateChannelAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnAddOrUpdateChannelAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnAddOrUpdateChannelAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnAddOrUpdateChannelAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnDeleteChannelAttributesResultHandler))]
		private static void OnDeleteChannelAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnDeleteChannelAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnDeleteChannelAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnDeleteChannelAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnClearChannelAttributesResultHandler))]
		private static void OnClearChannelAttributesResultCallback(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnClearChannelAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnClearChannelAttributesResult != null) {
							clientEventHandlerHandlerDic[id].OnClearChannelAttributesResult(id, requestId, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnFileMediaUploadResult))]
		private static void OnFileMediaUploadResultCallback(int id, Int64 requestId, IntPtr fileMessagePtr, UPLOAD_MEDIA_ERR_CODE code) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnFileMediaUploadResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
				 	FileMessage fileMessage = new FileMessage(fileMessagePtr, FileMessage.MESSAGE_FLAG.SEND);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnFileMediaUploadResult != null) {
							clientEventHandlerHandlerDic[id].OnFileMediaUploadResult(id, requestId, fileMessage, code);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnImageMediaUploadResult))]
		private static void OnImageMediaUploadResultCallback(int id, Int64 requestId, IntPtr imageMessagePtr, UPLOAD_MEDIA_ERR_CODE code) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnImageMediaUploadResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					Debug.Log("OnImageUploadResutl  result = " + code);
				 	ImageMessage imageMessage = new ImageMessage(imageMessagePtr, ImageMessage.MESSAGE_FLAG.SEND);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnImageMediaUploadResult != null) {
							clientEventHandlerHandlerDic[id].OnImageMediaUploadResult(id, requestId, imageMessage, code);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnMediaUploadingProgress))]
		private static void OnMediaUploadingProgressCallback(int id, Int64 requestId, Int64 totalSize, Int64 currentSize) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaUploadingProgress != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaUploadingProgress != null) {
							MediaOperationProgress mediaOperationProgress = new MediaOperationProgress();
							mediaOperationProgress.totalSize = totalSize;
							mediaOperationProgress.currentSize = currentSize;
							clientEventHandlerHandlerDic[id].OnMediaUploadingProgress(id, requestId, mediaOperationProgress);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(OnMediaDownloadingProgressHandler))]
		private static void OnMediaDownloadingProgressCallback(int id, Int64 requestId, Int64 totalSize, Int64 currentSize) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaDownloadingProgress != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						MediaOperationProgress mediaOperationProgress = new MediaOperationProgress();
						mediaOperationProgress.totalSize = totalSize;
						mediaOperationProgress.currentSize = currentSize;
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMediaDownloadingProgress != null) {
							clientEventHandlerHandlerDic[id].OnMediaDownloadingProgress(id, requestId, mediaOperationProgress);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnGetUserAttributesResultHandler))]
		private static void OnGetUserAttributesResultCallback(int id, Int64 requestId, string userId, string attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnGetUserAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnGetUserAttributesResult != null) {
							int j = 1;
							string[] sArray = attributes.Split('\t');
							RtmAttribute [] attribute = new RtmAttribute[numberOfAttributes];
							for (int i = 0; i < numberOfAttributes; i++) {
								attribute[i].key = sArray[j++];
								attribute[i].value = sArray[j++];
							}
							clientEventHandlerHandlerDic[id].OnGetUserAttributesResult(id, requestId, userId, attribute, numberOfAttributes, errorCode);										
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnGetChannelAttributesResult))]
		private static void OnGetChannelAttributesResultCallback(int id, Int64 requestId, string attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode) {
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnGetChannelAttributesResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnGetChannelAttributesResult != null) {
							int j = 1;
							string[] sArray = attributes.Split('\t');
							RtmChannelAttribute [] channelAttributes = new RtmChannelAttribute[numberOfAttributes];
							for (int i = 0; i < numberOfAttributes; i++) {
								RtmChannelAttribute _attribute = new RtmChannelAttribute(MESSAGE_FLAG.RECEIVE);
								_attribute.SetKey(sArray[j++]);
								_attribute.SetValue(sArray[j++]);
								_attribute.SetLastUpdateTs(Int64.Parse(sArray[j++]));
								_attribute.SetLastUpdateUserId(sArray[j++]);	
								channelAttributes[i] = _attribute;
							}
							clientEventHandlerHandlerDic[id].OnGetChannelAttributesResult(id, requestId, channelAttributes, numberOfAttributes, errorCode);
						}
					});
				}
			}
		}

		[MonoPInvokeCallback(typeof(EngineEventOnGetChannelMemberCountResult))]
		private static void OnGetChannelMemberCountResultCallback(int id, Int64 requestId, string channelMemberCounts , int channelCount, GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode)
		{
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnGetChannelMemberCountResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnGetChannelMemberCountResult != null) {
							int j = 1;
							string[] sArray = channelMemberCounts.Split('\t');
							ChannelMemberCount [] channelAttributes = new ChannelMemberCount[channelCount];
							for (int i = 0; i < channelCount; i++) {
								ChannelMemberCount channelMemeber = new ChannelMemberCount();
								channelMemeber.channelId = sArray[j++];
								channelMemeber.count = int.Parse(sArray[j++]);	
								channelAttributes[i] = channelMemeber;
							}
							clientEventHandlerHandlerDic[id].OnGetChannelMemberCountResult(id, requestId, channelAttributes, channelCount, errorCode);
						}
					});
				}
			}	
		}

		[MonoPInvokeCallback(typeof(EngineEventOnPeersOnlineStatusChanged))]
		private static void OnPeersOnlineStatusChangedCallback(int id, string peersStatusStr, int peerCount)
		{
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnPeersOnlineStatusChanged != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnPeersOnlineStatusChanged != null) {
							int j = 1;
							string[] sArray = peersStatusStr.Split('\t');
							PeerOnlineStatus [] channelAttributes = new PeerOnlineStatus[peerCount];
							for (int i = 0; i < peerCount; i++) {
								PeerOnlineStatus peerOnlineStatus = new PeerOnlineStatus();
								peerOnlineStatus.peerId = sArray[j++];
								peerOnlineStatus.isOnline = bool.Parse(sArray[j++]);	
								peerOnlineStatus.onlineState = (PEER_ONLINE_STATE)int.Parse(sArray[j++]);
								channelAttributes[i] = peerOnlineStatus;
							}
							clientEventHandlerHandlerDic[id].OnPeersOnlineStatusChanged(id, channelAttributes, peerCount);
						}
					});
				}
			}	
		}

		[MonoPInvokeCallback(typeof(OnQueryPeersBySubscriptionOptionResultHandler))]
		private static void OnQueryPeersBySubscriptionOptionResultCallback(int id, Int64 requestId, string[] peerIds, int peerCount, QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode)
		{
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnQueryPeersBySubscriptionOptionResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnQueryPeersBySubscriptionOptionResult != null) {
							clientEventHandlerHandlerDic[id].OnQueryPeersBySubscriptionOptionResult(id, requestId, peerIds, peerCount, errorCode);
						}
					});
				}
			}	
		}
	}
}
