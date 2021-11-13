using System.Runtime.InteropServices;
using System;

namespace agora_rtm {

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnMessageReceived(int _id, string userId, IntPtr messagePtr);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnImageMessageReceived(int _id, string userId, IntPtr messagePtr);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnFileMessageReceived(int _id, string userId, IntPtr messagePtr);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnMemberJoined(int _id, IntPtr channelMemberPtr);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnMemberLeft(int _id, IntPtr channelMemberPtr);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnAttributesUpdated(int _id, string attributesListPtr, int numberOfAttributes);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnGetMember(int _id, string membersPtr, int userCount, GET_MEMBERS_ERR errorCode);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnQueryPeersOnlineStatusResult(int _id, Int64 requestId, string peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnMediaUploadingProgress(int _id, Int64 requestId, Int64 totalSize, Int64 currentSize);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnFileMediaUploadResult(int _id, Int64 requestId, IntPtr fileMessage, UPLOAD_MEDIA_ERR_CODE code);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnImageMediaUploadResult(int _id, Int64 requestId, IntPtr fileMessage, UPLOAD_MEDIA_ERR_CODE code);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnMediaDownloadingProgress(int _id, Int64 requestId, Int64 totalSize, Int64 currentSize);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnMediaDownloadToMemoryResult(int _id, Int64 requestId, IntPtr memory, Int64 length, DOWNLOAD_MEDIA_ERR_CODE code);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnGetUserAttributesResultHandler(int _id, Int64 requestId, string userId, string attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnGetChannelAttributesResult(int _id, Int64 requestId, string attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnGetChannelMemberCountResult(int _id, Int64 requestId, string channelMemberCounts, int channelCount, GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnPeersOnlineStatusChanged(int _id, string peersStatus, int peerCount);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnLocalInvitationReceivedByPeerHandler(int _id, IntPtr localInvitation);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnLocalInvitationCanceledHandler(int _id, IntPtr localInvitation);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnLocalInvitationFailureHandler(int _id, IntPtr localInvitation, LOCAL_INVITATION_ERR_CODE errorCode);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnLocalInvitationAcceptedHandler(int _id, IntPtr localInvitation, string response);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnLocalInvitationRefusedHandler(int _id, IntPtr localInvitation, string response);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnRemoteInvitationRefusedHandler(int _id, IntPtr remoteInvitation);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnRemoteInvitationAcceptedHandler(int _id, IntPtr remoteInvitation);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnRemoteInvitationReceivedHandler(int _id, IntPtr remoteInvitation);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnRemoteInvitationFailureHandler(int _id, IntPtr remoteInvitation, REMOTE_INVITATION_ERR_CODE errorCode);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void EngineEventOnRemoteInvitationCanceledHandler(int _id, IntPtr remoteInvitation);

	[StructLayout(LayoutKind.Sequential)]
	internal struct CChannelEvent
	{
		internal RtmChannelEventHandler.OnJoinSuccessHandler onJoinSuccess;
		internal RtmChannelEventHandler.OnJoinFailureHandler onJoinFailure;
		internal RtmChannelEventHandler.OnLeaveHandler onLeave;
		internal EngineEventOnMessageReceived onMessageReceived;
		internal EngineEventOnImageMessageReceived onImageMessageReceived;
		internal EngineEventOnFileMessageReceived onFileMessageReceived;
		internal RtmChannelEventHandler.OnSendMessageResultHandler onSendMessageResult;
		internal EngineEventOnMemberJoined onMemberJoined;
		internal EngineEventOnMemberLeft onMemberLeft;
		internal EngineEventOnGetMember onGetMember;
		internal RtmChannelEventHandler.OnMemberCountUpdatedHandler onMemberCountUpdated;
		internal EngineEventOnAttributesUpdated onAttributesUpdated;
	};

	[StructLayout(LayoutKind.Sequential)]
	internal struct CChannelEventPtr
	{
		internal IntPtr onJoinSuccess;
		internal IntPtr onJoinFailure;
		internal IntPtr onLeave;
		internal IntPtr onMessageReceived;
		internal IntPtr onImageMessageReceived;
		internal IntPtr onFileMessageReceived;
		internal IntPtr onSendMessageResult;
		internal IntPtr onMemberJoined;
		internal IntPtr onMemberLeft;
		internal IntPtr onGetMember;
		internal IntPtr onMemberCountUpdated;
		internal IntPtr onAttributesUpdated;
	};

	[StructLayout(LayoutKind.Sequential)]
	internal struct CRtmServiceEventHandlerPtr
	{
		internal IntPtr onLoginSuccess;
		internal IntPtr onLoginFailure;
		internal IntPtr onRenewTokenResult;
		internal IntPtr onTokenExpired;
		internal IntPtr onLogout;
		internal IntPtr onConnectionStateChanged;
		internal IntPtr onSendMessageResult;
		internal IntPtr onMessageReceivedFromPeer;
		internal IntPtr onImageMessageReceivedFromPeer;
		internal IntPtr onFileMessageReceivedFromPeer;
		internal IntPtr onMediaUploadingProgress;
		internal IntPtr onMediaDownloadingProgress;
		internal IntPtr onFileMediaUploadResult;
		internal IntPtr onImageMediaUploadResult;
		internal IntPtr onMediaDownloadToFileResult;
		internal IntPtr onMediaDownloadToMemoryResult;
		internal IntPtr onMediaCancelResult;
		internal IntPtr onQueryPeersOnlineStatusResult;
		internal IntPtr onSubscriptionRequestResult;
		internal IntPtr onQueryPeersBySubscriptionOptionResult;
		internal IntPtr onPeersOnlineStatusChanged;
		internal IntPtr onSetLocalUserAttributesResult;
		internal IntPtr onDeleteLocalUserAttributesResult;
		internal IntPtr onClearLocalUserAttributesResult;
		internal IntPtr onGetUserAttributesResult;
		internal IntPtr onSetChannelAttributesResult;
		internal IntPtr onAddOrUpdateLocalUserAttributesResult;
		internal IntPtr onDeleteChannelAttributesResult;
		internal IntPtr onClearChannelAttributesResult;
		internal IntPtr onGetChannelAttributesResult;
		internal IntPtr onGetChannelMemberCountResult;
	}

    [StructLayout(LayoutKind.Sequential)]
    internal struct CRtmServiceEventHandler
    {
        internal RtmClientEventHandler.OnLoginSuccessHandler onLoginSuccess;
        internal RtmClientEventHandler.OnLoginFailureHandler onLoginFailure;
        internal RtmClientEventHandler.OnRenewTokenResultHandler onRenewTokenResult;
        internal RtmClientEventHandler.OnTokenExpiredHandler onTokenExpired;
        internal RtmClientEventHandler.OnLogoutHandler onLogout;
        internal RtmClientEventHandler.OnConnectionStateChangedHandler onConnectionStateChanged;
        internal RtmClientEventHandler.OnSendMessageResultHandler onSendMessageResult;
        internal EngineEventOnMessageReceived onMessageReceivedFromPeer;
        internal EngineEventOnImageMessageReceived onImageMessageReceivedFromPeer;
        internal EngineEventOnFileMessageReceived onFileMessageReceivedFromPeer;
        internal EngineEventOnMediaUploadingProgress onMediaUploadingProgress;
        internal EngineEventOnMediaDownloadingProgress onMediaDownloadingProgress;
        internal EngineEventOnFileMediaUploadResult onFileMediaUploadResult;
        internal EngineEventOnImageMediaUploadResult onImageMediaUploadResult;
        internal RtmClientEventHandler.OnMediaDownloadToFileResultHandler onMediaDownloadToFileResult;
        internal EngineEventOnMediaDownloadToMemoryResult onMediaDownloadToMemoryResult;
        internal RtmClientEventHandler.OnMediaCancelResultHandler onMediaCancelResult;
        internal EngineEventOnQueryPeersOnlineStatusResult onQueryPeersOnlineStatusResult;
        internal RtmClientEventHandler.OnSubscriptionRequestResultHandler onSubscriptionRequestResult;
        internal RtmClientEventHandler.OnQueryPeersBySubscriptionOptionResultHandler onQueryPeersBySubscriptionOptionResult;
        internal EngineEventOnPeersOnlineStatusChanged onPeersOnlineStatusChanged;
        internal RtmClientEventHandler.OnSetLocalUserAttributesResultHandler onSetLocalUserAttributesResult;
        internal RtmClientEventHandler.OnDeleteLocalUserAttributesResultHandler onDeleteLocalUserAttributesResult;
        internal RtmClientEventHandler.OnClearLocalUserAttributesResultHandler onClearLocalUserAttributesResult;
        internal EngineEventOnGetUserAttributesResultHandler onGetUserAttributesResult;
        internal RtmClientEventHandler.OnSetChannelAttributesResultHandler onSetChannelAttributesResult;
        internal RtmClientEventHandler.OnAddOrUpdateLocalUserAttributesResultHandler onAddOrUpdateLocalUserAttributesResult;
        internal RtmClientEventHandler.OnDeleteChannelAttributesResultHandler onDeleteChannelAttributesResult;
        internal RtmClientEventHandler.OnClearChannelAttributesResultHandler onClearChannelAttributesResult;
        internal EngineEventOnGetChannelAttributesResult onGetChannelAttributesResult;
        internal EngineEventOnGetChannelMemberCountResult onGetChannelMemberCountResult;
    }

    [StructLayout(LayoutKind.Sequential)]
	internal struct CRtmCallEventHandler {
		internal EngineEventOnLocalInvitationReceivedByPeerHandler _onLocalInvitationReceivedByPeer;
		internal EngineEventOnLocalInvitationCanceledHandler _onLocalInvitationCanceled;
		internal EngineEventOnLocalInvitationFailureHandler _onLocalInvitationFailure;
		internal EngineEventOnLocalInvitationAcceptedHandler _onLocalInvitationAccepted;
		internal EngineEventOnLocalInvitationRefusedHandler _onLocalInvitationRefused;
		internal EngineEventOnRemoteInvitationRefusedHandler _onRemoteInvitationRefused;
		internal EngineEventOnRemoteInvitationAcceptedHandler _onRemoteInvitationAccepted;
		internal EngineEventOnRemoteInvitationReceivedHandler _onRemoteInvitationReceived;
		internal EngineEventOnRemoteInvitationFailureHandler _onRemoteInvitationFailure;
		internal EngineEventOnRemoteInvitationCanceledHandler _onRemoteInvitationCanceled;
	};

	[StructLayout(LayoutKind.Sequential)]
	internal struct CRtmCallEventHandlerPtr
	{
		internal IntPtr _onLocalInvitationReceivedByPeer;
		internal IntPtr _onLocalInvitationCanceled;
		internal IntPtr _onLocalInvitationFailure;
		internal IntPtr _onLocalInvitationAccepted;
		internal IntPtr _onLocalInvitationRefused;
		internal IntPtr _onRemoteInvitationRefused;
		internal IntPtr _onRemoteInvitationAccepted;
		internal IntPtr _onRemoteInvitationReceived;
		internal IntPtr _onRemoteInvitationFailure;
		internal IntPtr _onRemoteInvitationCanceled;
	};

	internal class IRtmApiNative {

		#region DllImport
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		internal const string MyLibName = "agoraRTMCWrapper";
#else

#if UNITY_IPHONE
		internal const string MyLibName = "__Internal";
#else
        internal const string MyLibName = "agoraRTMCWrapper";
#endif
#endif
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createRtmService_rtm();

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr _getRtmSdkVersion_rtm();

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int initialize_rtm(IntPtr rtmServiceInstance, string appId, IntPtr serviceEventHandler);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int setLogFileSize_rtm(IntPtr rtmServiceInstance, int fileSizeInKBytes);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int setLogFile_rtm(IntPtr rtmServiceInstance, string logFile);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int setLogFilter_rtm(IntPtr rtmServiceInstance, int filter);
	
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int getChannelMemberCount_rtm(IntPtr rtmServiceInstance, string [] channelIds, int channelCount, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int getChannelAttributesByKeys_rtm(IntPtr rtmServiceInstance, string channelId, string [] attributeKeys, int numberOfKeys, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int getChannelAttributes_rtm(IntPtr rtmServiceInstance, string channelId, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int clearChannelAttributes_rtm(IntPtr rtmServiceInstance, string channelId, bool enableNotificationToChannelMembers, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int deleteChannelAttributesByKeys_rtm(IntPtr rtmServiceInstance, string channelId, string [] attributeKeys, int numberOfKeys, bool enableNotificationToChannelMembers, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int getUserAttributesByKeys_rtm(IntPtr rtmServiceInstance, string userId, string [] attributeKeys, int numberOfKeys, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int getUserAttributes_rtm(IntPtr rtmServiceInstance, string userId, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int clearLocalUserAttributes_rtm(IntPtr rtmServiceInstance, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int deleteLocalUserAttributesByKeys_rtm(IntPtr rtmServiceInstance, string [] attributeKeys, int numberOfKeys, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int queryPeersOnlineStatus_rtm(IntPtr rtmServiceInstance, string [] peerIds, int peerCount, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int subscribePeersOnlineStatus_rtm(IntPtr rtmServiceInstance, string [] peerIds, int peerCount, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int unsubscribePeersOnlineStatus_rtm(IntPtr rtmServiceInstance, string [] peerIds, int peerCount, Int64 requestId);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int queryPeersBySubscriptionOption_rtm(IntPtr rtmServiceInstance, PEER_SUBSCRIPTION_OPTION option, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int setParameters_rtm(IntPtr rtmServiceInstance, string parameters);
			
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createChannelAttribute_rtm(IntPtr rtmServiceInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int createImageMessageByUploading_rtm(IntPtr rtmServiceInstance, string filePath, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int createFileMessageByUploading_rtm(IntPtr rtmServiceInstance, string filePath, Int64 requestId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createImageMessageByMediaId_rtm(IntPtr rtmServiceInstance, string mediaId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createFileMessageByMediaId_rtm(IntPtr rtmServiceInstance, string mediaId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createMessage_rtm(IntPtr rtmServiceInstance, byte[] rawData, int length, string description);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createMessage2_rtm(IntPtr rtmServiceInstance, byte[] rawData, int length);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createMessage3_rtm(IntPtr rtmServiceInstance, string message);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createMessage4_rtm(IntPtr rtmServiceInstance);
	
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr createChannel_rtm(IntPtr rtmServiceInstance, string channelId, IntPtr channelEventHandlerPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int sendMessageToPeer_rtm(IntPtr rtmServiceInstance, string peerId, IntPtr message, bool enableOfflineMessaging,
                                    bool enableHistoricalMessaging);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int cancelMediaUpload_rtm(IntPtr rtmServiceInstance, Int64 requestId);						

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int cancelMediaDownload_rtm(IntPtr rtmServiceInstance, Int64 requestId);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int downloadMediaToFile_rtm(IntPtr rtmServiceInstance, string mediaId, string filePath, Int64 requestId);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int downloadMediaToMemory_rtm(IntPtr rtmServiceInstance, string mediaId, Int64 requestId);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int sendMessageToPeer2_rtm(IntPtr rtmServiceInstance, string peerId, IntPtr message);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int renewToken_rtm(IntPtr rtmServiceInstance, string token);		

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int logout_rtm(IntPtr rtmServiceInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int login_rtm(IntPtr rtmServiceInstance, string token, string userId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void release_rtm(IntPtr rtmServiceInstance, bool sync);

		/// Channel api
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int channel_join(IntPtr channelInstance);
	
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int channel_leave(IntPtr channelInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int channel_sendMessage(IntPtr channelInstance, IntPtr message);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int channel_sendMessage2(IntPtr channelInstance, IntPtr message,  bool enableOfflineMessaging, bool enableHistoricalMessaging);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int channel_getId(IntPtr channelInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int channel_getMembers(IntPtr channelInstance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int channel_release(IntPtr channelInstance);

		/// ChannelAttribute
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void channelAttribute_setKey(IntPtr channel_attribute_instance, string key);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr channelAttribute_getKey(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void channelAttribute_setValue(IntPtr channel_attribute_instance, string value);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr channelAttribute_getValue(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr channelAttribute_getLastUpdateUserId(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int64 channelAttribute_getLastUpdateTs(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void channelAttribute_release(IntPtr channel_attribute_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int setChannelAttributes_rtm(IntPtr rtmInstance, string channelId, Int64 [] attributes, int numberOfAttributes, bool enableNotificationToChannelMembers, Int64 requestId);
		/// Message api
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int64 imessage_getMessageId(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int imessage_getMessageType(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void imessage_setText(IntPtr file_message_instance, string text);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr imessage_getText(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr imessage_getRawMessageData(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int imessage_getRawMessageLength(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int64 imessage_getServerReceivedTs(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool imessage_isOfflineMessage(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void imessage_release(IntPtr file_message_instance);


		/// Image Message
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int64 iImage_message_getSize(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr iImage_message_getMediaId(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iImage_message_setThumbnail(IntPtr image_message_instance, byte[] thumbnail, Int64 length);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr iImage_message_getThumbnailData(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int64 iImage_message_getThumbnailLength(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iImage_message_setFileName(IntPtr image_message_instance, string fileName);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr iImage_message_getFileName(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iImage_message_setWidth(IntPtr image_message_instance, int width);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int iImage_message_getWidth(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iImage_message_setHeight(IntPtr image_message_instance, int height);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int iImage_message_getHeight(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iImage_message_setThumbnailWidth(IntPtr image_message_instance, int width);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int iImage_message_getThumbnailWidth(IntPtr image_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iImage_message_setThumbnailHeight(IntPtr image_message_instance, int height);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int iImage_message_getThumbnailHeight(IntPtr image_message_instance);

		/// File Message

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int64 iFile_message_getSize(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr iFile_message_getMediaId(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iFile_message_setThumbnail(IntPtr file_message_instance, byte[] thumbnail, int length);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr iFile_message_getThumbnailData(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int64 iFile_message_getThumbnailLength(IntPtr file_message_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void iFile_message_setFileName(IntPtr file_message_instance, string fileName);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr iFile_message_getFileName(IntPtr file_message_instance);

		// Channel Member
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr channel_member_getUserId(IntPtr channel_member_instance);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr channel_member_getChannelId(IntPtr channel_member_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr channel_member_release(IntPtr channel_member_instance);

		[DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr channel_event_handler_createEventHandler(int currenEventHandlerIndex, IntPtr cChannelEvent);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern void channel_event_handler_releaseEventHandler(IntPtr channel_eventHandler_ptr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr service_event_handler_createEventHandle(int id, IntPtr cRtmServiceEventHandler);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr service_event_handler_releaseEventHandler(IntPtr service_eventHandler_ptr);

		//rtm call event manager
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr getRtmCallManager_rtm(IntPtr service_eventHandler_ptr, IntPtr rtmCallEventHandler);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern int rtm_call_manager_sendLocalInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern int rtm_call_manager_acceptRemoteInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern int rtm_call_manager_refuseRemoteInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern int rtm_call_manager_cancelLocalInvitation(IntPtr callManagerInstance, IntPtr invitation);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr rtm_call_manager_createLocalCallInvitation(IntPtr callManagerInstance, string calleeId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr rtm_call_manager_release(IntPtr callManagerInstance);

		//local invitation
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_local_call_invitation_getCalleeId(IntPtr localCallInvitationPtr);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern void i_local_call_invitation_setContent(IntPtr localCallInvitationPtr, string content);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_local_call_invitation_getContent(IntPtr localCallInvitationPtr);	

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern void i_local_call_invitation_setChannelId(IntPtr localCallInvitationPtr, string channelId);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_local_call_invitation_getChannelId(IntPtr localCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_local_call_invitation_getResponse(IntPtr localCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern int i_local_call_invitation_getState(IntPtr localCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern void i_local_call_invitation_release(IntPtr localCallInvitationPtr);

		//remote invitation
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_remote_call_manager_getCallerId(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_remote_call_manager_getContent(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern void i_remote_call_manager_setResponse(IntPtr remoteCallInvitationPtr, string response);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_remote_call_manager_getResponse(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_remote_call_manager_getChannelId(IntPtr remoteCallInvitationPtr);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern int i_remote_call_manager_getState(IntPtr remoteCallInvitationPtr);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern void i_remote_call_manager_release(IntPtr remoteCallInvitationPtr);
		
		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern IntPtr i_rtm_call_event_handler_createEventHandler(int id, IntPtr cRtmCallEventHandler);

		[DllImport(MyLibName, CharSet = CharSet.Ansi)]
		internal static extern void i_rtm_call_event_releaseEventHandler(IntPtr remoteCallInvitationPtr);

#endregion engine callbacks
	}
}
