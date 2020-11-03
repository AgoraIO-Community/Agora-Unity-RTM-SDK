using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public class IRtmApiNative {
		public enum MESSAGE_FLAG {
			RECEIVE = 0,
			SEND = 1
		}

#region DllImport
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		public const string MyLibName = "agoraRTMCWrapper";
#else

#if UNITY_IPHONE
		public const string MyLibName = "__Internal";
#else
        public const string MyLibName = "agoraRTMCWrapper";
#endif
#endif
		protected delegate void EngineEventOnMessageReceived(int _id, string userId, IntPtr messagePtr);
		protected delegate void EngineEventOnImageMessageReceived(int _id, string userId, IntPtr messagePtr);
		protected delegate void EngineEventOnFileMessageReceived(int _id, string userId, IntPtr messagePtr);
		protected delegate void EngineEventOnMemberJoined(int _id, IntPtr channelMemberPtr);
		protected delegate void EngineEventOnMemberLeft(int _id, IntPtr channelMemberPtr);
		protected delegate void EngineEventOnAttributesUpdated(int _id, string attributesListPtr, int numberOfAttributes);
		protected delegate void EngineEventOnGetMember(int _id, string membersPtr, int userCount, GET_MEMBERS_ERR errorCode);
		protected delegate void EngineEventOnQueryPeersOnlineStatusResult(int _id, Int64 requestId, string peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode);
		protected delegate void EngineEventOnMediaUploadingProgress(int _id, Int64 requestId, Int64 totalSize, Int64 currentSize);
		protected delegate void EngineEventOnFileMediaUploadResult(int _id, Int64 requestId, IntPtr fileMessage, UPLOAD_MEDIA_ERR_CODE code);
		protected delegate void EngineEventOnImageMediaUploadResult(int _id, Int64 requestId, IntPtr fileMessage, UPLOAD_MEDIA_ERR_CODE code);
		protected delegate void EngineEventOnMediaDownloadingProgress(int _id, Int64 requestId, Int64 totalSize, Int64 currentSize);
		protected delegate void EngineEventOnMediaDownloadToMemoryResult(int _id, Int64 requestId, IntPtr memory, Int64 length, DOWNLOAD_MEDIA_ERR_CODE code);
		protected delegate void EngineEventOnGetUserAttributesResultHandler(int _id, Int64 requestId, string userId, string attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
		protected delegate void EngineEventOnGetChannelAttributesResult(int _id, Int64 requestId, string attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
		protected delegate void EngineEventOnGetChannelMemberCountResult(int _id, Int64 requestId, string channelMemberCounts , int channelCount, GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode);
		protected delegate void EngineEventOnPeersOnlineStatusChanged(int _id, string peersStatus, int peerCount);

		protected delegate void EngineEventOnLocalInvitationReceivedByPeerHandler(IntPtr localInvitation);
		protected delegate void EngineEventOnLocalInvitationCanceledHandler(IntPtr localInvitation);
		protected delegate void EngineEventOnLocalInvitationFailureHandler(IntPtr localInvitation, LOCAL_INVITATION_ERR_CODE errorCode);
		protected delegate void EngineEventOnLocalInvitationAcceptedHandler(IntPtr localInvitation, string response);
		protected delegate void EngineEventOnLocalInvitationRefusedHandler(IntPtr localInvitation, string response);
		protected delegate void EngineEventOnRemoteInvitationRefusedHandler(IntPtr remoteInvitation);
		protected delegate void EngineEventOnRemoteInvitationAcceptedHandler(IntPtr remoteInvitation);
		protected delegate void EngineEventOnRemoteInvitationReceivedHandler(IntPtr remoteInvitation);
		protected delegate void EngineEventOnRemoteInvitationFailureHandler(IntPtr remoteInvitation, REMOTE_INVITATION_ERR_CODE errorCode);
		protected delegate void EngineEventOnRemoteInvitationCanceledHandler(IntPtr remoteInvitation);
		
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createRtmService_();

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr _getRtmSdkVersion_();

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int initialize(IntPtr rtmServiceInstance, string appId, IntPtr serviceEventHandler);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFileSize(IntPtr rtmServiceInstance, int fileSizeInKBytes);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFile(IntPtr rtmServiceInstance, string logFile);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setLogFilter(IntPtr rtmServiceInstance, int filter);
	
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getChannelMemberCount(IntPtr rtmServiceInstance, string [] channelIds, int channelCount, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getChannelAttributesByKeys(IntPtr rtmServiceInstance, string channelId, string [] attributeKeys, int numberOfKeys, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getChannelAttributes(IntPtr rtmServiceInstance, string channelId, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int clearChannelAttributes(IntPtr rtmServiceInstance, string channelId, bool enableNotificationToChannelMembers, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int deleteChannelAttributesByKeys(IntPtr rtmServiceInstance, string channelId, string [] attributeKeys, int numberOfKeys, bool enableNotificationToChannelMembers, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getUserAttributesByKeys(IntPtr rtmServiceInstance, string userId, string [] attributeKeys, int numberOfKeys, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int getUserAttributes(IntPtr rtmServiceInstance, string userId, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int clearLocalUserAttributes(IntPtr rtmServiceInstance, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int deleteLocalUserAttributesByKeys(IntPtr rtmServiceInstance, string [] attributeKeys, int numberOfKeys, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int queryPeersOnlineStatus(IntPtr rtmServiceInstance, string [] peerIds, int peerCount, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int subscribePeersOnlineStatus(IntPtr rtmServiceInstance, string [] peerIds, int peerCount, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int unsubscribePeersOnlineStatus(IntPtr rtmServiceInstance, string [] peerIds, int peerCount, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int queryPeersBySubscriptionOption(IntPtr rtmServiceInstance, PEER_SUBSCRIPTION_OPTION option, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setParameters(IntPtr rtmServiceInstance, string parameters);
			
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createChannelAttribute(IntPtr rtmServiceInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createImageMessageByUploading(IntPtr rtmServiceInstance, string filePath, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int createFileMessageByUploading(IntPtr rtmServiceInstance, string filePath, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createImageMessageByMediaId(IntPtr rtmServiceInstance, string mediaId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createFileMessageByMediaId(IntPtr rtmServiceInstance, string mediaId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createMessage(IntPtr rtmServiceInstance, byte[] rawData, int length, string description);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createMessage2(IntPtr rtmServiceInstance, byte[] rawData, int length);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createMessage3(IntPtr rtmServiceInstance, string message);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createMessage4(IntPtr rtmServiceInstance);
	
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr createChannel(IntPtr rtmServiceInstance, string channelId, IntPtr channelEventHandlerPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendMessageToPeer(IntPtr rtmServiceInstance, string peerId, IntPtr message, bool enableOfflineMessaging,
                                    bool enableHistoricalMessaging);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int cancelMediaUpload(IntPtr rtmServiceInstance, Int64 requestId);						

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int cancelMediaDownload(IntPtr rtmServiceInstance, Int64 requestId);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int downloadMediaToFile(IntPtr rtmServiceInstance, string mediaId, string filePath, Int64 requestId);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int downloadMediaToMemory(IntPtr rtmServiceInstance, string mediaId, Int64 requestId);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int sendMessageToPeer2(IntPtr rtmServiceInstance, string peerId, IntPtr message);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int renewToken(IntPtr rtmServiceInstance, string token);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int logout(IntPtr rtmServiceInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int login(IntPtr rtmServiceInstance, string token, string userId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void release(IntPtr rtmServiceInstance, bool sync);

		/// Channel api
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int channel_join(IntPtr channelInstance);
	
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int channel_leave(IntPtr channelInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int channel_sendMessage(IntPtr channelInstance, IntPtr message);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int channel_sendMessage2(IntPtr channelInstance, IntPtr message,  bool enableOfflineMessaging, bool enableHistoricalMessaging);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int channel_getId(IntPtr channelInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int channel_getMembers(IntPtr channelInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int channel_release(IntPtr channelInstance);

		/// ChannelAttribute
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void channelAttribute_setKey(IntPtr channel_attribute_instance, string key);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channelAttribute_getKey(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void channelAttribute_setValue(IntPtr channel_attribute_instance, string value);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channelAttribute_getValue(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channelAttribute_getLastUpdateUserId(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern Int64 channelAttribute_getLastUpdateTs(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void channelAttribute_release(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int setChannelAttributes(IntPtr rtmInstance, string channelId, Int64 [] attributes, int numberOfAttributes, bool enableNotificationToChannelMembers, Int64 requestId);
		/// Message api
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern Int64 imessage_getMessageId(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int imessage_getMessageType(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void imessage_setText(IntPtr file_message_instance, string text);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr imessage_getText(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr imessage_getRawMessageData(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int imessage_getRawMessageLength(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern Int64 imessage_getServerReceivedTs(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern bool imessage_isOfflineMessage(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void imessage_release(IntPtr file_message_instance);


		/// Image Message
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern Int64 iImage_message_getSize(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr iImage_message_getMediaId(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iImage_message_setThumbnail(IntPtr image_message_instance, byte[] thumbnail, Int64 length);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr iImage_message_getThumbnailData(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern Int64 iImage_message_getThumbnailLength(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iImage_message_setFileName(IntPtr image_message_instance, string fileName);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr iImage_message_getFileName(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iImage_message_setWidth(IntPtr image_message_instance, int width);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int iImage_message_getWidth(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iImage_message_setHeight(IntPtr image_message_instance, int height);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int iImage_message_getHeight(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iImage_message_setThumbnailWidth(IntPtr image_message_instance, int width);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int iImage_message_getThumbnailWidth(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iImage_message_setThumbnailHeight(IntPtr image_message_instance, int height);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern int iImage_message_getThumbnailHeight(IntPtr image_message_instance);

		/// File Message

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern Int64 iFile_message_getSize(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr iFile_message_getMediaId(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iFile_message_setThumbnail(IntPtr file_message_instance, byte[] thumbnail, int length);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr iFile_message_getThumbnailData(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern Int64 iFile_message_getThumbnailLength(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void iFile_message_setFileName(IntPtr file_message_instance, string fileName);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr iFile_message_getFileName(IntPtr file_message_instance);

		// Channel Member
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channel_member_getUserId(IntPtr channel_member_instance);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channel_member_getChannelId(IntPtr channel_member_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channel_member_release(IntPtr channel_member_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern IntPtr channel_event_handler_createEventHandler(int currenEventHandlerIndex, RtmChannelEventHandler.OnJoinSuccessHandler onJoinSuccess,
																				RtmChannelEventHandler.OnJoinFailureHandler onJoinFailure,
																				RtmChannelEventHandler.OnLeaveHandler onLeave,
																				EngineEventOnMessageReceived onMessageReceived,
																				EngineEventOnImageMessageReceived onImageMessageReceived,
																				EngineEventOnFileMessageReceived onFileMessageReceived,
																				RtmChannelEventHandler.OnSendMessageResultHandler onSendMessageResult,
																				EngineEventOnMemberJoined onMemberJoined,
																				EngineEventOnMemberLeft onMemberLeft,
																				EngineEventOnGetMember onGetMember,
																				RtmChannelEventHandler.OnMemberCountUpdatedHandler onMemberCountUpdated,
																				EngineEventOnAttributesUpdated onAttributesUpdated);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
        protected static extern void channel_event_handler_releaseEventHandler(IntPtr channel_eventHandler_ptr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr service_event_handler_createEventHandle(int id, RtmClientEventHandler.OnLoginSuccessHandler onLoginSuccess,
																				RtmClientEventHandler.OnLoginFailureHandler onLoginFailure,
																				RtmClientEventHandler.OnRenewTokenResultHandler onRenewTokenResult,
																				RtmClientEventHandler.OnTokenExpiredHandler onTokenExpired,
																				RtmClientEventHandler.OnLogoutHandler onLogout,
																				RtmClientEventHandler.OnConnectionStateChangedHandler onConnectionStateChanged,
																				RtmClientEventHandler.OnSendMessageResultHandler onSendMessageResult,
																				EngineEventOnMessageReceived onMessageReceivedFromPeer,
																				EngineEventOnImageMessageReceived onImageMessageReceivedFromPeer,
																				EngineEventOnFileMessageReceived onFileMessageReceivedFromPeer,
																				EngineEventOnMediaUploadingProgress onMediaUploadingProgress,
																				EngineEventOnMediaDownloadingProgress onMediaDownloadingProgress,
																				EngineEventOnFileMediaUploadResult onFileMediaUploadResult,
																				EngineEventOnImageMediaUploadResult onImageMessageUploadResult,
																				RtmClientEventHandler.OnMediaDownloadToFileResultHandler onMediaDownloadToFileResult,
																				EngineEventOnMediaDownloadToMemoryResult onMediaDownloadToMemoryResult,
																				RtmClientEventHandler.OnMediaCancelResultHandler onMediaCancelResult,
																				EngineEventOnQueryPeersOnlineStatusResult onQueryPeersOnlineStatus,
																				RtmClientEventHandler.OnSubscriptionRequestResultHandler onSubscriptionRequestResult,
																				RtmClientEventHandler.OnQueryPeersBySubscriptionOptionResultHandler onQueryPeersBySubscriptionOptionResult,
																				EngineEventOnPeersOnlineStatusChanged onPeersOnlineStatusChanged,
																				RtmClientEventHandler.OnSetLocalUserAttributesResultHandler onSetLocalUserAttributesResult,
																				RtmClientEventHandler.OnDeleteLocalUserAttributesResultHandler onDeleteLocalUserAttributesResult,
																				RtmClientEventHandler.OnClearLocalUserAttributesResultHandler onClearLocalUserAttributesResult,
																				EngineEventOnGetUserAttributesResultHandler onGetUserAttributesResult,
																				RtmClientEventHandler.OnSetChannelAttributesResultHandler onSetChannelAttributesResult,
																				RtmClientEventHandler.OnAddOrUpdateLocalUserAttributesResultHandler onAddOrUpdateLocalUserAttributesResult,
																				RtmClientEventHandler.OnDeleteChannelAttributesResultHandler onDeleteChannelAttributesResult,
																				RtmClientEventHandler.OnClearChannelAttributesResultHandler onClearChannelAttributesResult,
																				EngineEventOnGetChannelAttributesResult onGetChannelAttributesResult,
																				EngineEventOnGetChannelMemberCountResult onGetChannelMemberCountResult);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr service_event_handler_releaseEventHandler(IntPtr service_eventHandler_ptr);

		//rtm call event manager
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr getRtmCallManager(IntPtr service_eventHandler_ptr, IntPtr rtmCallEventHandler);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern int rtm_call_manager_sendLocalInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern int rtm_call_manager_acceptRemoteInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern int rtm_call_manager_refuseRemoteInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern int rtm_call_manager_cancelLocalInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr rtm_call_manager_createLocalCallInvitation(IntPtr callManagerInstance, string calleeId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr rtm_call_manager_release(IntPtr callManagerInstance);

		//local invitation
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_local_call_invitation_getCalleeId(IntPtr localCallInvitationPtr);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern void i_local_call_invitation_setContent(IntPtr localCallInvitationPtr, string content);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_local_call_invitation_getContent(IntPtr localCallInvitationPtr);	

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern void i_local_call_invitation_setChannelId(IntPtr localCallInvitationPtr, string channelId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_local_call_invitation_getChannelId(IntPtr localCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_local_call_invitation_getResponse(IntPtr localCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern int i_local_call_invitation_getState(IntPtr localCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern void i_local_call_invitation_release(IntPtr localCallInvitationPtr);

		//remote invitation
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_remote_call_manager_getCallerId(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_remote_call_manager_getContent(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern void i_remote_call_manager_setResponse(IntPtr remoteCallInvitationPtr, string response);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_remote_call_manager_getResponse(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_remote_call_manager_getChannelId(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern int i_remote_call_manager_getState(IntPtr remoteCallInvitationPtr);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern void i_remote_call_manager_release(IntPtr remoteCallInvitationPtr);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern IntPtr i_rtm_call_event_handler_createEventHandler(EngineEventOnLocalInvitationReceivedByPeerHandler onLocalInvitationReceivedByPeerHandler,
																				EngineEventOnLocalInvitationCanceledHandler onLocalInvitationCanceledHandler,
																				EngineEventOnLocalInvitationFailureHandler onLocalInvitationFailureHandler,
																				EngineEventOnLocalInvitationAcceptedHandler onLocalInvitationAcceptedHandler,
																				EngineEventOnLocalInvitationRefusedHandler onLocalInvitationRefusedHandler,
																				EngineEventOnRemoteInvitationRefusedHandler onRemoteInvitationRefusedHandler,
																				EngineEventOnRemoteInvitationAcceptedHandler onRemoteInvitationAcceptedHandler,
																				EngineEventOnRemoteInvitationReceivedHandler onRemoteInvitationReceivedHandler,
																				EngineEventOnRemoteInvitationFailureHandler onRemoteInvitationFailureHandler,
																				EngineEventOnRemoteInvitationCanceledHandler onRemoteInvitationCanceledHandler);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		protected static extern void i_rtm_call_event_releaseEventHandler(IntPtr remoteCallInvitationPtr);



#endregion engine callbacks
	}
}
