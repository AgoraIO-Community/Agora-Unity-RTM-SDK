using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System;
using AOT;

namespace agora_rtm {
    public sealed class RtmClient : IRtmApiNative {
        private Dictionary<string, RtmChannel> channelDic = new Dictionary<string, RtmChannel>();
        private List<RtmChannelAttribute> attributeList = new List<RtmChannelAttribute>();
        private IntPtr _rtmServicePtr = IntPtr.Zero;
        private RtmClientEventHandler _eventHandler;
        public RtmClient(string appId, RtmClientEventHandler eventHandler) {
            if (appId == null || eventHandler == null) {
                Debug.LogError("appId or eventHandler is null");
                return;
            }
            AgoraCallbackObject.GetInstance();
            _rtmServicePtr = createRtmService_();
            _eventHandler = eventHandler;
            int ret = initialize(_rtmServicePtr, appId, eventHandler.GetRtmClientEventHandlerPtr());
            if (ret != 0) {
                Debug.LogError("RtmClient create fail, error:  " + ret);
            }
        }

        ~RtmClient() {
            Debug.Log("~RtmClient");
        }

        public void Release(bool sync) {
            Debug.Log("RtmClient Release");
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return;
			}

            foreach(KeyValuePair<string, RtmChannel>item in channelDic) {
                RtmChannel channel = item.Value;
                if (channel != null) {
                    channel.Release();

                }
            }
            channelDic.Clear();

            foreach(RtmChannelAttribute item in attributeList) {
                if (item != null) {
                    item.Release();
                }
            }
            attributeList.Clear();

            release(_rtmServicePtr, sync);
            _rtmServicePtr = IntPtr.Zero;
            _eventHandler.Release();
        }

        public static string GetSdkVersion() {
            IntPtr sdkVersion = _getRtmSdkVersion_();
            if (!ReferenceEquals(sdkVersion, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(sdkVersion);
			} else {
				return "";
			}
        }

        public int Login(string token, string userId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return login(_rtmServicePtr, token, userId);
        }

        public int Logout() {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return logout(_rtmServicePtr);
        }
        
        public int RenewToken(string token) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return renewToken(_rtmServicePtr, token);
        }

        public int SendMessageToPeer(string peerId, IMessage message, bool enableOfflineMessaging,
                                    bool enableHistoricalMessaging) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return sendMessageToPeer(_rtmServicePtr, peerId, message.GetPtr(), enableOfflineMessaging, enableHistoricalMessaging);
        }

        public int SendMessageToPeer(string peerId, IMessage message) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return sendMessageToPeer2(_rtmServicePtr, peerId, message.GetPtr());
        }

        public int DownloadMediaToMemory(string mediaId, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return downloadMediaToMemory(_rtmServicePtr, mediaId, requestId);
        }

        public int DownloadMediaToFile(string mediaId, string filePath, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return downloadMediaToFile(_rtmServicePtr, mediaId, filePath, requestId);
        }

        public int CancelMediaDownload(Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return cancelMediaDownload(_rtmServicePtr, requestId);
        }

        public int CancelMediaUpload(Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return cancelMediaUpload(_rtmServicePtr, requestId);
        }

        public RtmChannel CreateChannel(string channelId, RtmChannelEventHandler rtmChannelEventHandler) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}

            if (rtmChannelEventHandler == null) 
            {
                Debug.LogError("rtmChannelEventHandler is null");
            }
            
            if (channelDic.ContainsKey(channelId)) {
                if (channelDic[channelId] != null) {
                    return channelDic[channelId];
                }
            }
            
            IntPtr _rtmChannelPtr = createChannel(_rtmServicePtr, channelId, rtmChannelEventHandler.GetChannelEventHandlerPtr());
            RtmChannel channel = new RtmChannel(_rtmChannelPtr, rtmChannelEventHandler);
            channelDic.Add(channelId, channel);
            return channel;
        }

        public TextMessage CreateMessage() {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage4(_rtmServicePtr);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }

        public TextMessage CreateMessage(string text) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage3(_rtmServicePtr, text);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }

        public TextMessage CreateMessage(byte[] rawData) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage2(_rtmServicePtr, rawData, rawData.Length);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }

        public TextMessage CreateMessage(byte[] rawData, string description) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage(_rtmServicePtr, rawData, rawData.Length, description);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }

        public ImageMessage CreateImageMessageByMediaId(string mediaId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createImageMessageByMediaId(_rtmServicePtr, mediaId);
            return new ImageMessage(_MessagePtr, ImageMessage.MESSAGE_FLAG.SEND);
        }

        public int CreateImageMessageByUploading(string filePath, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return createImageMessageByUploading(_rtmServicePtr, filePath, requestId);
        }

        public FileMessage CreateFileMessageByMediaId(string mediaId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createFileMessageByMediaId(_rtmServicePtr, mediaId);
            return new FileMessage(_MessagePtr, FileMessage.MESSAGE_FLAG.SEND);
        }

        public int CreateFileMessageByUploading(string filePath, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return createFileMessageByUploading(_rtmServicePtr, filePath, requestId);
        }

        public RtmChannelAttribute CreateChannelAttribute() {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr channelAttribute = createChannelAttribute(_rtmServicePtr);
            RtmChannelAttribute attribute = new RtmChannelAttribute(channelAttribute);
            attributeList.Add(attribute);
            return attribute;
        }
    
        public int SetParameters(string parameters) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setParameters(_rtmServicePtr, parameters);
        }

        public int QueryPeersOnlineStatus(string [] peerIds, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return queryPeersOnlineStatus(_rtmServicePtr, peerIds, peerIds.Length, requestId);
        }

        public int SubscribePeersOnlineStatus(string [] peerIds, int peerCount, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return subscribePeersOnlineStatus(_rtmServicePtr, peerIds, peerCount, requestId);
        }

        public int UnsubscribePeersOnlineStatus(string [] peerIds, int peerCount, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return unsubscribePeersOnlineStatus(_rtmServicePtr, peerIds, peerCount, requestId);
        }

        public int QueryPeersBySubscriptionOption(PEER_SUBSCRIPTION_OPTION option, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return queryPeersBySubscriptionOption(_rtmServicePtr, option, requestId);
        }

        public int SetLogFileSize(int fileSizeInKBytes) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setLogFileSize(_rtmServicePtr, fileSizeInKBytes);
        }

        public int SetLogFileter(int fileter) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setLogFilter(_rtmServicePtr, fileter);
        }

        public int SetLogFile(string logFilePath) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setLogFile(_rtmServicePtr, logFilePath);
        }

        public int GetChannelMemberCount(string[] channelIds, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getChannelMemberCount(_rtmServicePtr, channelIds, channelIds.Length, requestId);
        }

        public int GetChannelAttributesByKeys(string channelId, string[] attributeKeys, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getChannelAttributesByKeys(_rtmServicePtr, channelId, attributeKeys, attributeKeys.Length, requestId);
        }

        public int GetChannelAttributes(string channelId, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmServicePtr is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return getChannelAttributes(_rtmServicePtr, channelId, requestId);
        }

        public int ClearChannelAttributes(string channelId, bool enableNotificationToChannelMembers, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return clearChannelAttributes(_rtmServicePtr, channelId, enableNotificationToChannelMembers, requestId);
        }

        public int DeleteChannelAttributesByKeys(string channelId, string [] attributeKeys, bool enableNotificationToChannelMembers, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return deleteChannelAttributesByKeys(_rtmServicePtr, channelId, attributeKeys, attributeKeys.Length, enableNotificationToChannelMembers, requestId);
        }

        public int GetUserAttributesByKeys(string userId, string [] attributeKeys, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getUserAttributesByKeys(_rtmServicePtr, userId, attributeKeys, attributeKeys.Length, requestId);
        }

        public int GetUserAttributes(string userId, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getUserAttributes(_rtmServicePtr, userId, requestId);
        }

        public int ClearLocalUserAttributes(Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return clearLocalUserAttributes(_rtmServicePtr, requestId);
        }

        public int DeleteLocalUserAttributesByKeys(string [] attributeKeys, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return deleteLocalUserAttributesByKeys(_rtmServicePtr, attributeKeys, attributeKeys.Length, requestId);
        }

        public int SetChannelAttributes(string channelId, RtmChannelAttribute [] attributes, ChannelAttributeOptions options, Int64 requestId)
        {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }

            if (attributes != null && attributes.Length > 0) {
                Int64 [] attributeList = new Int64[attributes.Length];
                for (int i = 0; i < attributes.Length; i++) {
                    attributeList[i] = attributes[i].GetPtr().ToInt64();
                }
                return setChannelAttributes(_rtmServicePtr, channelId, attributeList, attributes.Length, options.enableNotificationToChannelMembers, requestId);
            }
            return -7;
        } 

        public RtmCallManager GetRtmCallManager(RtmCallEventHandler eventHandler) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return null;
            }
            if (eventHandler == null) {
                Debug.LogError("eventHandler is null");
                return null;
            }
            IntPtr rtmCallManagerPtr = getRtmCallManager(_rtmServicePtr, eventHandler.GetPtr());
            return new RtmCallManager(rtmCallManagerPtr, eventHandler);
        }

    }
}