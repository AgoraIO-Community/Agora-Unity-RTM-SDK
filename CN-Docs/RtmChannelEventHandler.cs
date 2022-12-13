using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using AOT;



namespace agora_rtm {
    public sealed class RtmChannelEventHandler : IRtmApiNative { 
        private static int _id = 0;
        private static Dictionary<int, RtmChannelEventHandler> channelEventHandlerDic = new Dictionary<int, RtmChannelEventHandler>();
        private IntPtr channelEventHandlerPtr = IntPtr.Zero;
        private int currentIdIndex = 0;

		/// <summary>
		/// 加入频道成功回调。
		/// 本地用户调用 \ref agora_rtm.RtmChannel.Join "Join" 方法成功加入频道后：
		/// - SDK 触发此回调；
		/// - 频道内所有远端用户收到 \ref agora_rtm.RtmChannelEventHandler.OnMemberJoinedHandler "OnMemberJoinedHandler" 回调。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
        public delegate void OnJoinSuccessHandler(int id);

		/// <summary>
		/// 加入频道失败回调。
		/// SDK 会在用户 \ref agora_rtm.RtmChannel.Join "Join" 方法失败后触发此回调。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.JOIN_CHANNEL_ERR "JOIN_CHANNEL_ERR"。</param>
        public delegate void OnJoinFailureHandler(int id, JOIN_CHANNEL_ERR errorCode);
        
		/// <summary>
		/// 离开频道回调。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.LEAVE_CHANNEL_ERR "LEAVE_CHANNEL_ERR"。</param>
		public delegate void OnLeaveHandler(int id, LEAVE_CHANNEL_ERR errorCode);
        
		/// <summary>
		/// 收到频道消息回调。
		/// 当远端用户调用 \ref agora_rtm.RtmChannel.SendMessage "SendMessage" 方法成功发送频道消息后，在相同频道的本地用户会收到此回调。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="userId">消息发送者的用户 ID。</param>
		/// <param name="message">接收到的频道消息内容。详见 \ref agora_rtm.IMessage "IMessage"。</param>
		public delegate void OnMessageReceivedHandler(int id, string userId, TextMessage message);
        
		/// <summary>
		/// 报告 \ref agora_rtm.RtmChannel.SendMessage "SendMessage" 方法的调用结果。
	    /// 本地用户调用 \ref agora_rtm.RtmChannel.SendMessage "SendMessage" 方法成功发送频道消息后：
		/// - SDK 触发此回调；
		/// - 频道内所有远端用户收到 #OnMessageReceived 回调。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="messageId">频道消息的 ID。</param>
		/// <param name="errorCode">错误码。详见 \ref agora_rtm.CHANNEL_MESSAGE_ERR_CODE "CHANNEL_MESSAGE_ERR_CODE"。</param>
		public delegate void OnSendMessageResultHandler(int id, Int64 messageId, CHANNEL_MESSAGE_ERR_CODE errorCode);
        
		/// <summary>
		/// 远端用户加入频道回调。
		/// 当有远端用户调用 \ref agora_rtm.RtmChannel.Join "Join" 方法加入频道并收到 #OnJoinSuccessHandler 回调时，在相同频道的本地用户会收到此回调。
		/// note 频道人数超过 512 人时后台会关闭上下线通知。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="member">加入频道的远端用户。详见 \ref agora_rtm.ChannelMemberCount "ChannelMember"。</param>
		public delegate void OnMemberJoinedHandler(int id, RtmChannelMember member);
        
		/// <summary>
		/// 频道成员离开频道回调。
		/// 当有频道成员调用 \ref agora_rtm.RtmChannel.Leave "Leave" 方法离开频道并收到 #OnLeaveHandler (LEAVE_CHANNEL_ERR_OK) 回调时，在相同频道的本地用户会收到此回调。
		/// @note 频道人数超过 512 人时后台会关闭上下线通知。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="member">离开频道的频道成员。详见 \ref agora_rtm.RtmChannelMember "ChannelMember"。</param>
		public delegate void OnMemberLeftHandler(int id, RtmChannelMember member);
        
		/// <summary>
		/// 报告 \ref agora_rtm.RtmChannel.GetMembers "GetMembers" 方法的调用结果。
		/// 当方法调用成功时，SDK 会返回 \ref agora_rtm.RtmChannel "RtmChannel" 频道成员列表。
		/// @note 该方法的调用频率上限为每 2 秒 5 次。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="members">频道成员列表。详见 \ref agora_rtm.RtmChannel "RtmChannel"。</param>
		/// <param name="userCount">频道成员数量。</param>
		/// <param name="errorCode">错误代码。详见 \ref agora_rtm.GET_MEMBERS_ERR "GET_MEMBERS_ERR"。</param>
		public delegate void OnGetMembersHandler(int id, RtmChannelMember[] members, int userCount, GET_MEMBERS_ERR errorCode);
        
		/// <summary>
		/// 频道属性更新回调。报告所在频道的所有属性。
		/// @note 只有当频道属性更新者将 \ref agora_rtm.ChannelAttributeOptions.enableNotificationToChannelMembers "enableNotificationToChannelMembers" 设为 `true` 后，该回调才会被触发。请注意：该标志位为一次性标志位，仅对当前频道属性操作有效。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="attributesList">当前频道的所有属性。</param>
		/// <param name="numberOfAttributes">频道属性的条数。</param>
		public delegate void OnAttributesUpdatedHandler(int id, RtmChannelAttribute[] attributesList, int numberOfAttributes);
        
		/// <summary>
		/// 频道成员人数更新回调。报告最新频道成员人数。
		/// @note SDK 会在频道成员人数更新时返回该回调：
		/// - 频道成员人数 ≤ 512 时，回调触发频率为每秒 1 次。
		/// - 频道成员人数超过 512 时，回调触发频率为每 3 秒 1 次。
		/// - 用户在成功加入频道时会收到该回调。你可以通过监听该回调获取加入频道时的频道成员人数和后继人数更新。
		/// </summary>
		/// <param name="id">#RtmChannelEventHandler ID</param>
		/// <param name="memberCount">最新频道成员人数。</param>
		public delegate void OnMemberCountUpdatedHandler(int id, int memberCount);

        public OnJoinSuccessHandler OnJoinSuccess;
        public OnJoinFailureHandler OnJoinFailure;
        public OnLeaveHandler OnLeave;
        public OnMessageReceivedHandler OnMessageReceived;
        public OnSendMessageResultHandler OnSendMessageResult;
        public OnMemberJoinedHandler OnMemberJoined;
        public OnMemberLeftHandler OnMemberLeft;
        public OnGetMembersHandler OnGetMembers;
        public OnAttributesUpdatedHandler OnAttributesUpdated;
        public OnMemberCountUpdatedHandler OnMemberCountUpdated;

        public RtmChannelEventHandler() {
            currentIdIndex = _id;

			cChannelEvent = new CChannelEvent
			{
				onJoinSuccess = OnJoinSuccessCallback,
				onJoinFailure = OnJoinFailureCallback,
				onLeave = OnLeaveCallback,
				onMessageReceived = OnMessageReceivedCallback,
				onSendMessageResult = OnSendMessageResultCallback,
				onMemberJoined = OnMemberJoinedCallback,
				onMemberLeft = OnMemberLeftCallback,
				onGetMember = OnGetMemberCallback,
				onAttributesUpdated = OnAttributesUpdatedCallback,
				onMemberCountUpdated = OnMemberCountUpdatedCallback
			};

			cChannelEventPtr = new CChannelEventPtr {
				onJoinSuccess = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onJoinSuccess),
				onJoinFailure = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onJoinFailure),
				onLeave = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onLeave),
				onMessageReceived = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onMessageReceived),
				onSendMessageResult = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onSendMessageResult),
				onMemberJoined = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onMemberJoined),
				onMemberLeft = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onMemberLeft),
				onGetMember = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onGetMember),
				onAttributesUpdated = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onAttributesUpdated),
				onMemberCountUpdated = Marshal.GetFunctionPointerForDelegate(cChannelEvent.onMemberCountUpdated)
			};
			globalPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CChannelEventPtr)));
			Marshal.StructureToPtr(cChannelEventPtr, globalPtr, true);
			channelEventHandlerDic.Add(currentIdIndex, this);
			channelEventHandlerNativePtr = IRtmApiNative.channel_event_handler_createEventHandler(currentIdIndex, globalPtr);
            _id ++;
        }

        internal IntPtr GetPtr() {
            return channelEventHandlerNativePtr;
        }

        [MonoPInvokeCallback(typeof(OnLeaveHandler))]
        private static void OnLeaveCallback(int id, LEAVE_CHANNEL_ERR errorCode) {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnLeave != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnLeave != null) {
							channelEventHandlerDic[id].OnLeave(id, errorCode);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(OnJoinFailureHandler))]
        private static void OnJoinFailureCallback(int id, JOIN_CHANNEL_ERR errorCode) {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnJoinFailure != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnJoinFailure != null) {
							channelEventHandlerDic[id].OnJoinFailure(id, errorCode);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(OnJoinSuccessHandler))]
        private static void OnJoinSuccessCallback(int id) {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnJoinSuccess != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnJoinSuccess != null) {
							channelEventHandlerDic[id].OnJoinSuccess(id);
						}
					});
				}
			}
        }
        
        [MonoPInvokeCallback(typeof(EngineEventOnMessageReceived))]
        private static void OnMessageReceivedCallback(int id, string userId, IntPtr messagePtr)
        {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMessageReceived != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
                    TextMessage textMessage = new TextMessage(messagePtr, MESSAGE_FLAG.SEND);
					TextMessage _textMessage = new TextMessage(textMessage, MESSAGE_FLAG.RECEIVE);
					textMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMessageReceived != null) {
							channelEventHandlerDic[id].OnMessageReceived(id, userId, _textMessage);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(OnSendMessageResultHandler))]
        private static void OnSendMessageResultCallback(int id, Int64 messageId, CHANNEL_MESSAGE_ERR_CODE errorCode)
        {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnSendMessageResult != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnSendMessageResult != null) {
							channelEventHandlerDic[id].OnSendMessageResult(id, messageId, errorCode);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(EngineEventOnMemberJoined))]
        private static void OnMemberJoinedCallback(int id, IntPtr channelMemberPtr)
        {
            Debug.Log("OnMemberJoinedCallback");
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMemberJoined != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
                    string userId = Marshal.PtrToStringAnsi(IRtmApiNative.channel_member_getUserId(channelMemberPtr));
                    string channelId = Marshal.PtrToStringAnsi(IRtmApiNative.channel_member_getChannelId(channelMemberPtr));
                    RtmChannelMember rtmChannelMember = new RtmChannelMember(userId, channelId);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMemberJoined != null) {
							channelEventHandlerDic[id].OnMemberJoined(id, rtmChannelMember);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(EngineEventOnMemberLeft))]
        private static void OnMemberLeftCallback(int id, IntPtr channelMemberPtr)
        {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMemberLeft != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
                    string userId = Marshal.PtrToStringAnsi(IRtmApiNative.channel_member_getUserId(channelMemberPtr));
                    string channelId = Marshal.PtrToStringAnsi(IRtmApiNative.channel_member_getChannelId(channelMemberPtr));
                    RtmChannelMember rtmChannelMember = new RtmChannelMember(userId, channelId);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMemberLeft != null) {
							channelEventHandlerDic[id].OnMemberLeft(id, rtmChannelMember);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(EngineEventOnAttributesUpdated))]
        private static void OnAttributesUpdatedCallback(int id, string attributesListPtr, int numberOfAttributes)
        {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnAttributesUpdated != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnAttributesUpdated != null) {
							int j = 1;
							string[] sArray = attributesListPtr.Split('\t');
							RtmChannelAttribute [] channelAttributes = new RtmChannelAttribute[numberOfAttributes];
							for (int i = 0; i < numberOfAttributes; i++) {
								RtmChannelAttribute _attribute = new RtmChannelAttribute(MESSAGE_FLAG.RECEIVE);
								_attribute.SetKey(sArray[j++]);
								_attribute.SetValue(sArray[j++]);
								_attribute.SetLastUpdateTs(Int64.Parse(sArray[j++]));
								_attribute.SetLastUpdateUserId(sArray[j++]);
								channelAttributes[i] = _attribute;
							}
							channelEventHandlerDic[id].OnAttributesUpdated(id, channelAttributes, numberOfAttributes);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(OnMemberCountUpdatedHandler))]
        private static void OnMemberCountUpdatedCallback(int id, int memberCount)
        {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMemberCountUpdated != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMemberCountUpdated != null) {
							channelEventHandlerDic[id].OnMemberCountUpdated(id, memberCount);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(EngineEventOnGetMember))]
        private static void OnGetMemberCallback(int id, string membersStr, int userCount, GET_MEMBERS_ERR errorCode)
        {
		    if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnGetMembers != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnGetMembers != null) {
							int j = 1;
							string[] sArray = membersStr.Split('\t');
							RtmChannelMember [] membersList = new RtmChannelMember[userCount];
							for (int i = 0; i < userCount; i++) {
								RtmChannelMember member = new RtmChannelMember(sArray[j++], sArray[j++]);
								membersList[i] = member;
							}
							channelEventHandlerDic[id].OnGetMembers(id, membersList, userCount, errorCode);
						}
					});
				}
			}
        }

        public void Release() {
			Debug.Log("RtmChannelEventHandler Released");
            if (channelEventHandlerNativePtr == IntPtr.Zero) {
                return;
            }
            channelEventHandlerDic.Remove(currentIdIndex);
			IRtmApiNative.channel_event_handler_releaseEventHandler(channelEventHandlerNativePtr);
			channelEventHandlerNativePtr = IntPtr.Zero;
			Marshal.FreeHGlobal(globalPtr);
			globalPtr = IntPtr.Zero;
		}
	}
}