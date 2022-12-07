using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using AOT;

namespace agora_rtm {
	public sealed class RtmClientEventHandler {
		private int currentIdIndex = 0;
		private static int _id = 0;
		private IntPtr _rtmClientEventHandlerNativePtr = IntPtr.Zero;
		private static Dictionary<int, RtmClientEventHandler> clientEventHandlerHandlerDic = new Dictionary<int, RtmClientEventHandler>();
		/// <summary>
		/// Occurs when a user logs in the Agora RTM system.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		public delegate void OnLoginSuccessHandler(int id);

		/// <summary>
		/// Occurs when a user fails to log in the Agora RTM system.
		/// </summary>
		/// <param name="id">
		/// the id of your engine
		/// </param>
		/// <param name="errorCode">Error codes related to login.</param>
		public delegate void OnLoginFailureHandler(int id, LOGIN_ERR_CODE errorCode);

		/// <summary>
		/// Reports the result of the renewToken method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="token">Your new token.</param>
		/// <param name="errorCode">The error code. </param>
		public delegate void OnRenewTokenResultHandler(int id, string token, RENEW_TOKEN_ERR_CODE errorCode);
		
		/// <summary>
		/// Occurs when the RTM server detects that the RTM token has exceeded the 24-hour validity period and when the SDK is in the CONNECTION_STATE_RECONNECTING state.
		/// This callback occurs only when the SDK is reconnecting to the server. You will not receive this callback when the SDK is in the CONNECTION_STATE_CONNECTED state.
		/// When receiving this callback, generate a new RTM Token on the server and call the renewToken method to pass the new Token on to the server.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		public delegate void OnTokenExpiredHandler(int id);


		/// <summary>
		/// Occurs when the token expires in 30 seconds.
		/// Upon receiving this callback, generate a new token on the server and call the \ref agora::rtm::IRtmService::renewToken "renewToken" method to pass the new token to the SDK.
		/// If the token used in the \ref agora::rtm::IRtmService::login "login" method expires, the user becomes offline and the SDK attempts to reconnect.
		/// </summary>
		public delegate void OnTokenPrivilegeWillExpireHandler(int id);

		/// <summary>
		/// Occurs when a user logs out of the Agora RTM system.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="errorCode">The error code. </param>
		public delegate void OnLogoutHandler(int id, LOGOUT_ERR_CODE errorCode);

		/// <summary>
		/// Occurs when the connection state changes between the SDK and the Agora RTM system.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="state">The new connection state.</param>
		/// <param name="reason">The reason for the connection state change.</param>
		public delegate void OnConnectionStateChangedHandler(int id, CONNECTION_STATE state, CONNECTION_CHANGE_REASON reason);
		
		/// <summary>
		/// Reports the result of the sendMessageToPeer method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="messageId">The ID of the sent message.</param>
		/// <param name="errorCode">The peer-to-peer message state. </param>
		public delegate void OnSendMessageResultHandler(int id, Int64 messageId, PEER_MESSAGE_ERR_CODE errorCode);
		
		/// <summary>
		/// Occurs when receiving a peer-to-peer message.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="peerId">The ID of the message sender.</param>
		/// <param name="message">The received peer-to-peer message.</param>
		public delegate void OnMessageReceivedFromPeerHandler(int id, string peerId, TextMessage message);

		/// <summary>
		/// Reports the result of the queryPeersOnlineStatus method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="peersStatus">The online status of the peer. </param>
		/// <param name="peerCount">The number of the queried peers.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnQueryPeersOnlineStatusResultHandler(int id, Int64 requestId, PeerOnlineStatus[] peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode);
		
		/// <summary>
		/// Returns the result of the subscribePeersOnlineStatus or unsubscribePeersOnlineStatus method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnSubscriptionRequestResultHandler(int id, Int64 requestId, PEER_SUBSCRIPTION_STATUS_ERR errorCode);
		
		/// <summary>
		/// Returns the result of the queryPeersBySubscriptionOption method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="peerIds">A user ID array of the specified users, to whom you subscribe.</param>
		/// <param name="peerCount">Count of the peers.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnQueryPeersBySubscriptionOptionResultHandler(int id, Int64 requestId, string[] peerIds, int peerCount, QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the setLocalUserAttributes method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnSetLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the addOrUpdateLocalUserAttributes method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnAddOrUpdateLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the deleteLocalUserAttributesByKeys method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnDeleteLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the clearLocalUserAttributes method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnClearLocalUserAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the getUserAttributes or getUserAttributesByKeys method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="userId">The user ID of the specified user.</param>
		/// <param name="attributes">An array of the returned attributes. See RtmAttribute.</param>
		/// <param name="numberOfAttributes">The total number of the user's attributes</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnGetUserAttributesResultHandler(int id, Int64 requestId, string userId, RtmAttribute[] attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the setChannelAttributes method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnSetChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the addOrUpdateChannelAttributes method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnAddOrUpdateChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the deleteChannelAttributesByKeys method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnDeleteChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the clearChannelAttributes method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnClearChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the getChannelAttributes or getChannelAttributesByKeys method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="attributes"></param>
		/// <param name="numberOfAttributes">The total number of the attributes.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnGetChannelAttributesResultHandler(int id, Int64 requestId, RtmChannelAttribute[] attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
		
		/// <summary>
		/// Reports the result of the getChannelMemberCount method call.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="requestId">The unique ID of this request.</param>
		/// <param name="channelMemberCounts">An array of the channel member counts.</param>
		/// <param name="channelCount">The total number of the channels.</param>
		/// <param name="errorCode">Error Codes.</param>
		public delegate void OnGetChannelMemberCountResultHandler(int id, Int64 requestId, ChannelMemberCount[] channelMemberCounts , int channelCount, GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode);
		
		/// <summary>
		/// Occurs when the online status of the peers, to whom you subscribe, changes.
		/// When the subscription to the online status of specified peers succeeds, the SDK returns this callback to report the online status of peers, to whom you subscribe.
		/// When the online status of the peers, to whom you subscribe, changes, the SDK returns this callback to report whose online status has changed.
		/// If the online status of the peers, to whom you subscribe, changes when the SDK is reconnecting to the server, the SDK returns this callback to report whose online status has changed when successfully reconnecting to the server.
		/// </summary>
		/// <param name="id">the id of your engine</param>
		/// <param name="peersStatus">An array of peers' online states. See PeerOnlineStatus.</param>
		/// <param name="peerCount">Count of the peers, whose online status changes.</param>
		public delegate void OnPeersOnlineStatusChangedHandler(int id, PeerOnlineStatus[] peersStatus, int peerCount);

		public OnLoginSuccessHandler OnLoginSuccess;
		public OnLoginFailureHandler OnLoginFailure;
		public OnRenewTokenResultHandler OnRenewTokenResult;
		public OnTokenExpiredHandler OnTokenExpired;
		public OnTokenPrivilegeWillExpireHandler OnTokenPrivilegeWillExpire;
		public OnLogoutHandler OnLogout;
		public OnConnectionStateChangedHandler OnConnectionStateChanged;
		public OnSendMessageResultHandler OnSendMessageResult;
		public OnMessageReceivedFromPeerHandler OnMessageReceivedFromPeer;

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

		private CRtmServiceEventHandler rtmServiceEventHandler;
		private CRtmServiceEventHandlerPtr rtmServiceEventHandlerPtr;
		private IntPtr globalPtr = IntPtr.Zero;

		public RtmClientEventHandler() {
			currentIdIndex = _id;
			clientEventHandlerHandlerDic.Add(currentIdIndex, this);
			rtmServiceEventHandler = new CRtmServiceEventHandler {
				onLoginSuccess = OnLoginSuccessCallback,
				onLoginFailure = OnLoginFailureCallback,
				onRenewTokenResult = OnRenewTokenResultCallback,
				onTokenExpired = OnTokenExpiredCallback,
				onTokenPrivilegeWillExpire = OnTokenPrivilegeWillExpireCallback,
				onLogout = OnLogoutCallback,
				onConnectionStateChanged = OnConnectionStateChangedCallback,
				onSendMessageResult = OnSendMessageResultCallback,
				onMessageReceivedFromPeer = OnMessageReceivedFromPeerCallback,
				onQueryPeersOnlineStatusResult = OnQueryPeersOnlineStatusResultCallback, 
				onSubscriptionRequestResult = OnSubscriptionRequestResultCallback,
				onQueryPeersBySubscriptionOptionResult = OnQueryPeersBySubscriptionOptionResultCallback,
				onPeersOnlineStatusChanged = OnPeersOnlineStatusChangedCallback,
				onSetLocalUserAttributesResult = OnSetLocalUserAttributesResultCallback,
				onDeleteLocalUserAttributesResult = OnDeleteLocalUserAttributesResultCallback,
				onClearLocalUserAttributesResult = OnClearLocalUserAttributesResultCallback,
				onGetUserAttributesResult = OnGetUserAttributesResultCallback,
				onSetChannelAttributesResult = OnSetLocalUserAttributesResultCallback,
				onAddOrUpdateLocalUserAttributesResult = OnAddOrUpdateLocalUserAttributesResultCallback,
				onDeleteChannelAttributesResult = OnDeleteChannelAttributesResultCallback,
				onClearChannelAttributesResult = OnClearChannelAttributesResultCallback,
				onGetChannelAttributesResult = OnGetChannelAttributesResultCallback,
				onGetChannelMemberCountResult = OnGetChannelMemberCountResultCallback
			};

			rtmServiceEventHandlerPtr = new CRtmServiceEventHandlerPtr {
				onLoginSuccess = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onLoginSuccess),
				onLoginFailure = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onLoginFailure),
				onRenewTokenResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onRenewTokenResult),
				onTokenExpired = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onTokenExpired),
				onTokenPrivilegeWillExpire = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onTokenPrivilegeWillExpire),
				onLogout = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onLogout),
				onConnectionStateChanged = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onConnectionStateChanged),
				onSendMessageResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onSendMessageResult),
				onMessageReceivedFromPeer = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onMessageReceivedFromPeer),
				onQueryPeersOnlineStatusResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onQueryPeersOnlineStatusResult),
				onSubscriptionRequestResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onSubscriptionRequestResult),
				onQueryPeersBySubscriptionOptionResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onQueryPeersBySubscriptionOptionResult),
				onPeersOnlineStatusChanged = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onPeersOnlineStatusChanged),
				onSetLocalUserAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onSetLocalUserAttributesResult),
				onDeleteLocalUserAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onDeleteLocalUserAttributesResult),
				onClearLocalUserAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onClearLocalUserAttributesResult),
				onGetUserAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onGetUserAttributesResult),
				onSetChannelAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onSetChannelAttributesResult),
				onAddOrUpdateLocalUserAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onAddOrUpdateLocalUserAttributesResult),
				onDeleteChannelAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onDeleteChannelAttributesResult),
				onClearChannelAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onClearChannelAttributesResult),
				onGetChannelAttributesResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onGetChannelAttributesResult),
				onGetChannelMemberCountResult = Marshal.GetFunctionPointerForDelegate(rtmServiceEventHandler.onGetChannelMemberCountResult)
			};

			globalPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CRtmServiceEventHandlerPtr)));
			Marshal.StructureToPtr(rtmServiceEventHandlerPtr, globalPtr, true);
			_rtmClientEventHandlerNativePtr = IRtmApiNative.service_event_handler_createEventHandle(currentIdIndex, globalPtr);
            _id ++;
		}

		public void Release() {
			Debug.Log("RtmClientEventHandler Released");
			if (_rtmClientEventHandlerNativePtr == IntPtr.Zero) {
				return;
			}
			clientEventHandlerHandlerDic.Remove(currentIdIndex);
			IRtmApiNative.service_event_handler_releaseEventHandler(_rtmClientEventHandlerNativePtr);
			_rtmClientEventHandlerNativePtr = IntPtr.Zero;

			Marshal.FreeHGlobal(globalPtr);
			globalPtr = IntPtr.Zero;
		}

		public IntPtr GetPtr() {
			return _rtmClientEventHandlerNativePtr;
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

		[MonoPInvokeCallback(typeof(OnTokenPrivilegeWillExpireHandler))]
		private static void OnTokenPrivilegeWillExpireCallback(int id)
		{
			if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnTokenExpired != null)
			{
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null)
				{
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(() => {
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnTokenPrivilegeWillExpire != null)
						{
							clientEventHandlerHandlerDic[id].OnTokenPrivilegeWillExpire(id);
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
					TextMessage textMessage = new TextMessage(message, MESSAGE_FLAG.SEND);
					TextMessage _textMessage = new TextMessage(textMessage, MESSAGE_FLAG.RECEIVE);
					textMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (clientEventHandlerHandlerDic.ContainsKey(id) && clientEventHandlerHandlerDic[id].OnMessageReceivedFromPeer != null) {
							clientEventHandlerHandlerDic[id].OnMessageReceivedFromPeer(id, peerId, _textMessage);
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
								peerOnlineStatus.isOnline = 1 == int.Parse(sArray[j++]);
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
