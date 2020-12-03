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
        public delegate void OnJoinSuccessHandler(int id);
        public delegate void OnJoinFailureHandler(int id, JOIN_CHANNEL_ERR errorCode);
        public delegate void OnLeaveHandler(int id, LEAVE_CHANNEL_ERR errorCode);
        public delegate void OnMessageReceivedHandler(int id, string userId, TextMessage message);
        public delegate void OnImageMessageReceivedHandler(int id, string userId, ImageMessage message);
        public delegate void OnFileMessageReceivedHandler(int id, string userId, FileMessage message);
        public delegate void OnSendMessageResultHandler(int id, Int64 messageId, CHANNEL_MESSAGE_ERR_CODE errorCode);
        public delegate void OnMemberJoinedHandler(int id, RtmChannelMember member);
        public delegate void OnMemberLeftHandler(int id, RtmChannelMember member);
        public delegate void OnGetMembersHandler(int id, RtmChannelMember[] members, int userCount, GET_MEMBERS_ERR errorCode);
        public delegate void OnAttributesUpdatedHandler(int id, RtmChannelAttribute[] attributesList, int numberOfAttributes);
        public delegate void OnMemberCountUpdatedHandler(int id, int memberCount);

        public OnJoinSuccessHandler OnJoinSuccess;
        public OnJoinFailureHandler OnJoinFailure;
        public OnLeaveHandler OnLeave;
        public OnMessageReceivedHandler OnMessageReceived;
        public OnImageMessageReceivedHandler OnImageMessageReceived;
        public OnFileMessageReceivedHandler OnFileMessageReceived;
        public OnSendMessageResultHandler OnSendMessageResult;
        public OnMemberJoinedHandler OnMemberJoined;
        public OnMemberLeftHandler OnMemberLeft;
        public OnAttributesUpdatedHandler OnAttributesUpdated;
        public OnMemberCountUpdatedHandler OnMemberCountUpdated;
        public OnGetMembersHandler OnGetMembers;

        public RtmChannelEventHandler() {
            currentIdIndex = _id;
			channelEventHandlerDic.Add(currentIdIndex, this);
            channelEventHandlerPtr = channel_event_handler_createEventHandler(currentIdIndex, OnJoinSuccessCallback,
                                                                                              OnJoinFailureCallback,
                                                                                              OnLeaveCallback,
                                                                                              OnMessageReceivedCallback,
                                                                                              OnImageMessageReceivedCallback,
                                                                                              OnFileMessageReceivedCallback,
                                                                                              OnSendMessageResultCallback,
                                                                                              OnMemberJoinedCallback,
                                                                                              OnMemberLeftCallback,
                                                                                              OnGetMemberCallback,
                                                                                              OnMemberCountUpdatedCallback,
                                                                                              OnAttributesUpdatedCallback);
            _id ++;
        }

        public IntPtr GetChannelEventHandlerPtr() {
            return channelEventHandlerPtr;
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
                    TextMessage textMessage = new TextMessage(messagePtr, TextMessage.MESSAGE_FLAG.SEND);
					TextMessage _textMessage = new TextMessage(textMessage, TextMessage.MESSAGE_FLAG.RECEIVE);
					textMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnMessageReceived != null) {
							channelEventHandlerDic[id].OnMessageReceived(id, userId, _textMessage);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(EngineEventOnImageMessageReceived))]
        private static void OnImageMessageReceivedCallback(int id, string userId, IntPtr messagePtr) 
        {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnImageMessageReceived != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
                    ImageMessage imageMessage = new ImageMessage(messagePtr, ImageMessage.MESSAGE_FLAG.SEND);
					ImageMessage _imageMessage = new ImageMessage(imageMessage, ImageMessage.MESSAGE_FLAG.RECEIVE);
					imageMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnImageMessageReceived != null) {
							channelEventHandlerDic[id].OnImageMessageReceived(id, userId, _imageMessage);
						}
					});
				}
			}
        }

        [MonoPInvokeCallback(typeof(EngineEventOnFileMessageReceived))]
        private static void OnFileMessageReceivedCallback(int id, string userId, IntPtr messagePtr)
        {
			if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnFileMessageReceived != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
                    FileMessage fileMessage = new FileMessage(messagePtr, FileMessage.MESSAGE_FLAG.SEND);
					FileMessage _fileMessage = new FileMessage(fileMessage, FileMessage.MESSAGE_FLAG.RECEIVE);
					fileMessage.SetMessagePtr(IntPtr.Zero);
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (channelEventHandlerDic.ContainsKey(id) && channelEventHandlerDic[id].OnFileMessageReceived != null) {
							channelEventHandlerDic[id].OnFileMessageReceived(id, userId, _fileMessage);
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
                    string userId = Marshal.PtrToStringAnsi(channel_member_getUserId(channelMemberPtr));
                    string channelId = Marshal.PtrToStringAnsi(channel_member_getChannelId(channelMemberPtr));
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
                    string userId = Marshal.PtrToStringAnsi(channel_member_getUserId(channelMemberPtr));
                    string channelId = Marshal.PtrToStringAnsi(channel_member_getChannelId(channelMemberPtr));
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
            if (channelEventHandlerPtr == IntPtr.Zero) {
                return;
            }
            channelEventHandlerDic.Remove(currentIdIndex);
            channel_event_handler_releaseEventHandler(channelEventHandlerPtr);
            channelEventHandlerPtr = IntPtr.Zero;
		}
	}
}