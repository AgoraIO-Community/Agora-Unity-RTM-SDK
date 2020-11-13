//
//  RtmServiceEventHandler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//

#ifndef RtmServiceEventHandler_hpp
#define RtmServiceEventHandler_hpp

#include "common.h"
#include "LogHelper.h"

namespace agora {
    namespace unity {
    
    #if defined(_WIN32)
    typedef void(__stdcall *FUNC_onLoginSuccess)(int handlerId);
    typedef void(__stdcall *FUNC_onLoginFailure)(int handlerId, int errorCode);
    typedef void(__stdcall *FUNC_onRenewTokenResult)(int handlerId, const char* token, int errorCode);
    typedef void(__stdcall *FUNC_onTokenExpired)(int handlerId);
    typedef void(__stdcall *FUNC_onLogout)(int handlerId, int errorCode);
    typedef void(__stdcall *FUNC_onConnectionStateChanged)(int handlerId, int state, int reason);
    typedef void(__stdcall *FUNC_onSendMessageResult)(int handlerId, long long messageId, int errorCode);
    typedef void(__stdcall *FUNC_onMessageReceivedFromPeer)(int handlerId, const char *peerId, void *message);
    typedef void(__stdcall *FUNC_onImageMessageReceivedFromPeer)(int handlerId, const char *peerId, void* message);
    typedef void(__stdcall *FUNC_onFileMessageReceivedFromPeer)(int handlerId, const char *peerId, void* message);
    typedef void(__stdcall *FUNC_onMediaUploadingProgress)(int handlerId, long long requestId, long long totalSize, long long currentSize);
    typedef void(__stdcall *FUNC_onMediaDownloadingProgress)(int handlerId, long long requestId, long long totalSize, long long currentSize);
    typedef void(__stdcall *FUNC_onFileMediaUploadResult)(int handlerId, long long requestId, void* fileMessage, int code);
    typedef void(__stdcall *FUNC_onImageMediaUploadResult)(int handlerId, long long requestId, void* imageMessage, int code);
    typedef void(__stdcall *FUNC_onMediaDownloadToFileResult)(int handlerId, long long requestId, int code);
    typedef void(__stdcall *FUNC_onMediaDownloadToMemoryResult)(int handlerId, long long requestId, const char* memory, long long length, int code);
    typedef void(__stdcall *FUNC_onMediaCancelResult)(int handlerId, long long requestId, int code);
    typedef void(__stdcall *FUNC_onQueryPeersOnlineStatusResult)(int handlerId, long long requestId, void* peersStatus, int peerCount, int errorCode);
    typedef void(__stdcall *FUNC_onSubscriptionRequestResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onQueryPeersBySubscriptionOptionResult)(int handlerId, long long requestId, const char* peerIds[], int peerCount, int errorCode);
    typedef void(__stdcall *FUNC_onPeersOnlineStatusChanged)(int handlerId, void *peersStatus, int peerCount);
    typedef void(__stdcall *FUNC_onSetLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onDeleteLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onClearLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onGetUserAttributesResult)(int handlerId, long long requestId, const char* userId, void* attributes, int numberOfAttributes, int errorCode);
    typedef void(__stdcall *FUNC_onSetChannelAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onAddOrUpdateLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onDeleteChannelAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onClearChannelAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(__stdcall *FUNC_onGetChannelAttributesResult)(int handlerId, long long requestId, const char *attributes, int numberOfAttributes, int errorCode);
    typedef void(__stdcall *FUNC_onGetChannelMemberCountResult)(int handlerId, long long requestId, void* channelMemberCounts , int channelCount, int errorCode);
    #else
    typedef void(*FUNC_onLoginSuccess)(int handlerId);
    typedef void(*FUNC_onLoginFailure)(int handlerId, int errorCode);
    typedef void(*FUNC_onRenewTokenResult)(int handlerId, const char* token, int errorCode);
    typedef void(*FUNC_onTokenExpired)(int handlerId);
    typedef void(*FUNC_onLogout)(int handlerId, int errorCode);
    typedef void( *FUNC_onConnectionStateChanged)(int handlerId, int state, int reason);
    typedef void(*FUNC_onSendMessageResult)(int handlerId, long long messageId, int errorCode);
    typedef void(*FUNC_onMessageReceivedFromPeer)(int handlerId, const char *peerId, void *message);
    typedef void(*FUNC_onImageMessageReceivedFromPeer)(int handlerId, const char *peerId, void* message);
    typedef void(*FUNC_onFileMessageReceivedFromPeer)(int handlerId, const char *peerId, void* message);
    typedef void(*FUNC_onMediaUploadingProgress)(int handlerId, long long requestId, long long totalSize, long long currentSize);
    typedef void(*FUNC_onMediaDownloadingProgress)(int handlerId, long long requestId, long long totalSize, long long currentSize);
    typedef void(*FUNC_onFileMediaUploadResult)(int handlerId, long long requestId, void* fileMessage, int code);
    typedef void(*FUNC_onImageMediaUploadResult)(int handlerId, long long requestId, void* imageMessage, int code);
    typedef void(*FUNC_onMediaDownloadToFileResult)(int handlerId, long long requestId, int code);
    typedef void(*FUNC_onMediaDownloadToMemoryResult)(int handlerId, long long requestId, const char* memory, long long length, int code);
    typedef void(*FUNC_onMediaCancelResult)(int handlerId, long long requestId, int code);
    typedef void(*FUNC_onQueryPeersOnlineStatusResult)(int handlerId, long long requestId, void* peersStatus, int peerCount, int errorCode);
    typedef void(*FUNC_onSubscriptionRequestResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onQueryPeersBySubscriptionOptionResult)(int handlerId, long long requestId, const char* peerIds[], int peerCount, int errorCode);
    typedef void(*FUNC_onPeersOnlineStatusChanged)(int handlerId, const char *peersStatus, int peerCount);
    typedef void(*FUNC_onSetLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onDeleteLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onClearLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onGetUserAttributesResult)(int handlerId, long long requestId, const char* userId, const char *attribute, int numberOfAttributes, int errorCode);
    typedef void(*FUNC_onSetChannelAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onAddOrUpdateLocalUserAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onDeleteChannelAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onClearChannelAttributesResult)(int handlerId, long long requestId, int errorCode);
    typedef void(*FUNC_onGetChannelAttributesResult)(int handlerId, long long requestId, const char *attributes, int numberOfAttributes, int errorCode);
    typedef void(*FUNC_onGetChannelMemberCountResult)(int handlerId, long long requestId, void* channelMemberCounts , int channelCount, int errorCode);
    #endif
    
    class RtmServiceEventHandler : public agora::rtm::IRtmServiceEventHandler
        {
        private:
            int handlerId = 0;
            FUNC_onLoginSuccess _onLoginSuccess = nullptr;
            FUNC_onLoginFailure _onLoginFailure = nullptr;
            FUNC_onRenewTokenResult _onRenewTokenResult = nullptr;
            FUNC_onTokenExpired _onTokenExpired = nullptr;
            FUNC_onLogout _onLogout = nullptr;
            FUNC_onConnectionStateChanged _onConnectionStateChanged = nullptr;
            FUNC_onSendMessageResult _onSendMessageResult = nullptr;
            FUNC_onMessageReceivedFromPeer _onMessageReceivedFromPeer = nullptr;
            FUNC_onImageMessageReceivedFromPeer _onImageMessageReceivedFromPeer = nullptr;
            FUNC_onFileMessageReceivedFromPeer _onFileMessageReceivedFromPeer = nullptr;
            FUNC_onMediaUploadingProgress _onMediaUploadingProgress = nullptr;
            FUNC_onMediaDownloadingProgress _onMediaDownloadingProgress = nullptr;
            FUNC_onFileMediaUploadResult _onFileMediaUploadResult = nullptr;
            FUNC_onImageMediaUploadResult _onImageMediaUploadResult = nullptr;
            FUNC_onMediaDownloadToFileResult _onMediaDownloadToFileResult = nullptr;
            FUNC_onMediaDownloadToMemoryResult _onMediaDownloadToMemoryResult = nullptr;
            FUNC_onMediaCancelResult _onMediaCancelResult = nullptr;
            FUNC_onQueryPeersOnlineStatusResult _onQueryPeersOnlineStatusResult = nullptr;
            FUNC_onSubscriptionRequestResult _onSubscriptionRequestResult = nullptr;
            FUNC_onQueryPeersBySubscriptionOptionResult _onQueryPeersBySubscriptionOptionResult = nullptr;
            FUNC_onPeersOnlineStatusChanged _onPeersOnlineStatusChanged = nullptr;
            FUNC_onSetLocalUserAttributesResult _onSetLocalUserAttributesResult = nullptr;
            FUNC_onDeleteLocalUserAttributesResult _onDeleteLocalUserAttributesResult = nullptr;
            FUNC_onClearLocalUserAttributesResult _onClearLocalUserAttributesResult = nullptr;
            FUNC_onGetUserAttributesResult _onGetUserAttributesResult = nullptr;
            FUNC_onSetChannelAttributesResult _onSetChannelAttributesResult = nullptr;
            FUNC_onAddOrUpdateLocalUserAttributesResult _onAddOrUpdateLocalUserAttributesResult = nullptr;
            FUNC_onDeleteChannelAttributesResult _onDeleteChannelAttributesResult = nullptr;
            FUNC_onClearChannelAttributesResult _onClearChannelAttributesResult = nullptr;
            FUNC_onGetChannelAttributesResult _onGetChannelAttributesResult = nullptr;
            FUNC_onGetChannelMemberCountResult _onGetChannelMemberCountResult = nullptr;
            
            
        public:
            RtmServiceEventHandler(int _id, FUNC_onLoginSuccess _onLoginSuccess,
                   FUNC_onLoginFailure _onLoginFailure,
                   FUNC_onRenewTokenResult _onRenewTokenResult,
                   FUNC_onTokenExpired _onTokenExpired,
                   FUNC_onLogout _onLogout,
                   FUNC_onConnectionStateChanged _onConnectionStateChanged,
                   FUNC_onSendMessageResult _onSendMessageResult,
                   FUNC_onMessageReceivedFromPeer _onMessageReceivedFromPeer,
                   FUNC_onImageMessageReceivedFromPeer _onImageMessageReceivedFromPeer,
                   FUNC_onFileMessageReceivedFromPeer _onFileMessageReceivedFromPeer,
                   FUNC_onMediaUploadingProgress _onMediaUploadingProgress,
                   FUNC_onMediaDownloadingProgress _onMediaDownloadingProgress,
                   FUNC_onFileMediaUploadResult _onFileMediaUploadResult,
                   FUNC_onImageMediaUploadResult _onImageMediaUploadResult,
                   FUNC_onMediaDownloadToFileResult _onMediaDownloadToFileResult,
                   FUNC_onMediaDownloadToMemoryResult _onMediaDownloadToMemoryResult,
                   FUNC_onMediaCancelResult _onMediaCancelResult,
                   FUNC_onQueryPeersOnlineStatusResult _onQueryPeersOnlineStatusResult,
                   FUNC_onSubscriptionRequestResult _onSubscriptionRequestResult,
                   FUNC_onQueryPeersBySubscriptionOptionResult _onQueryPeersBySubscriptionOptionResult,
                   FUNC_onPeersOnlineStatusChanged _onPeersOnlineStatusChanged,
                   FUNC_onSetLocalUserAttributesResult _onSetLocalUserAttributesResult,
                   FUNC_onDeleteLocalUserAttributesResult _onDeleteLocalUserAttributesResult,
                   FUNC_onClearLocalUserAttributesResult _onClearLocalUserAttributesResult,
                   FUNC_onGetUserAttributesResult _onGetUserAttributesResult,
                   FUNC_onSetChannelAttributesResult _onSetChannelAttributesResult,
                   FUNC_onAddOrUpdateLocalUserAttributesResult _onAddOrUpdateLocalUserAttributesResult,
                   FUNC_onDeleteChannelAttributesResult _onDeleteChannelAttributesResult,
                   FUNC_onClearChannelAttributesResult _onClearChannelAttributesResult,
                   FUNC_onGetChannelAttributesResult _onGetChannelAttributesResult,
                   FUNC_onGetChannelMemberCountResult _onGetChannelMemberCountResult);
            
            virtual ~RtmServiceEventHandler();

          /**
           Occurs when a user logs in the Agora RTM system.

           The local user receives this callback when the \ref agora::rtm::IRtmService::login "login" method call succeeds.
           */
            virtual void onLoginSuccess() override;
            
          /**
           Occurs when a user fails to log in the Agora RTM system.

           The local user receives this callback when the \ref agora::rtm::IRtmService::login "login" method call fails. See \ref agora::rtm::LOGIN_ERR_CODE "LOGIN_ERR_CODE" for the error codes.
           */
            virtual void onLoginFailure(agora::rtm::LOGIN_ERR_CODE errorCode) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::renewToken "renewToken" method call.
           
           @param token Your new token.
           @param errorCode The error code. See #RENEW_TOKEN_ERR_CODE.
           */
            virtual void onRenewTokenResult(const char* token, agora::rtm::RENEW_TOKEN_ERR_CODE errorCode) override;
     
          /**
           Occurs when the RTM server detects that the RTM token has exceeded the 24-hour validity period and when the SDK is in the \ref agora::rtm::CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" state.
           
           - This callback occurs only when the SDK is reconnecting to the server. You will not receive this callback when the SDK is in the \ref agora::rtm::CONNECTION_STATE_CONNECTED "CONNECTION_STATE_CONNECTED" state.
           - When receiving this callback, generate a new RTM Token on the server and call the \ref agora::rtm::IRtmService::renewToken "renewToken" method to pass the new Token on to the server.
           */
            virtual void onTokenExpired() override;

          /**
           Occurs when a user logs out of the Agora RTM system.

           The local user receives this callback when the SDK calls the \ref agora::rtm::IRtmService::logout "logout" method. See \ref agora::rtm::LOGOUT_ERR_CODE "LOGOUT_ERR_CODE" for the error codes.
           */
            virtual void onLogout(agora::rtm::LOGOUT_ERR_CODE errorCode) override;
            
          /**
           Occurs when the connection state changes between the SDK and the Agora RTM system.

           @param state The new connection state. See #CONNECTION_STATE.
           @param reason The reason for the connection state change. See #CONNECTION_CHANGE_REASON.
           */
            virtual void onConnectionStateChanged(agora::rtm::CONNECTION_STATE state,    agora::rtm::CONNECTION_CHANGE_REASON reason) override;
            
          /**
           Reports the result of the \ref agora::rtm::IRtmService::sendMessageToPeer "sendMessageToPeer" method call.

           @param messageId The ID of the sent message.
           @param errorCode The peer-to-peer message state. See #PEER_MESSAGE_ERR_CODE.

           */
            virtual void onSendMessageResult(long long messageId, agora::rtm::PEER_MESSAGE_ERR_CODE errorCode) override;
        
          /**
           Occurs when receiving a peer-to-peer message.

           @param peerId The ID of the message sender.
           @param message The received peer-to-peer message. See \ref agora::rtm::IMessage "IMessage".
           */
            virtual void onMessageReceivedFromPeer(const char *peerId, const agora::rtm::IMessage *message) override;
            
          /**
           Occurs when receiving a peer-to-peer image message.
           
           @param peerId The ID of the message sender.
           @param message The received peer-to-peer image message. See \ref agora::rtm::IImageMessage "IImageMessage".
           */
            virtual void onImageMessageReceivedFromPeer(const char *peerId, const   agora::rtm::IImageMessage* message) override;
            
          /**
           Occurs when receiving a peer-to-peer file message.
         
           @param peerId The ID of the message sender.
           @param message The received peer-to-peer file message. See \ref agora::rtm::IFileMessage "IFileMessage".
           */
            virtual void onFileMessageReceivedFromPeer(const char *peerId, const agora::rtm::IFileMessage* message) override;
            
          /**
           Reports the progress of an ongoing upload task.
           
           @note
           - If the upload task is ongoing, the SDK returns this callback once every second.
           - If the upload task comes to a halt, the SDK stops returning this callback until the task is going again.

           @param requestId The unique ID of the upload request.
           @param progress The progress of the ongoing upload task. See \ref agora::rtm::MediaOperationProgress "MediaOperationProgress".
           */
            virtual void onMediaUploadingProgress(long long requestId, const agora::rtm::MediaOperationProgress &progress) override;
            
          /**
           Reports the progress of an ongoing download task.
           
           @note
           - If the download task is ongoing, the SDK returns this callback once every second.
           - If the download task comes to a halt, the SDK stops returning this callback until the task is going again.
         
           @param requestId The unique ID of the download request.
           @param progress The progress of the ongoing download task. See \ref agora::rtm::MediaOperationProgress "MediaOperationProgress".
           */
            virtual void onMediaDownloadingProgress(long long requestId, const agora::rtm::MediaOperationProgress &progress) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::createFileMessageByUploading "createFileMessageByUploading" method call.
         
           @param requestId The unique ID of the upload request.
           @param fileMessage An \ref agora::rtm::IFileMessage "IFileMessage" instance.
           @param code Error codes. See #UPLOAD_MEDIA_ERR_CODE.
           */
            virtual void onFileMediaUploadResult(long long requestId, agora::rtm::IFileMessage* fileMessage, agora::rtm::UPLOAD_MEDIA_ERR_CODE code) override;
       
          /**
           Reports the result of the \ref agora::rtm::IRtmService::createImageMessageByUploading "createImageMessageByUploading" method call.

           @param requestId The unique ID of the upload request.
           @param imageMessage An \ref agora::rtm::IImageMessage "IImageMessage" instance.
           @param code Error codes. See #UPLOAD_MEDIA_ERR_CODE.
           */
            virtual void onImageMediaUploadResult(long long requestId, agora::rtm::IImageMessage* imageMessage, agora::rtm::UPLOAD_MEDIA_ERR_CODE code) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::downloadMediaToFile "downloadMediaToFile" method call.
         
           @param requestId The unique ID of the download request.
           @param code Error codes. See #DOWNLOAD_MEDIA_ERR_CODE.
           */
            virtual void onMediaDownloadToFileResult(long long requestId, agora::rtm::DOWNLOAD_MEDIA_ERR_CODE code);
            
          /**
           Reports the result of the \ref agora::rtm::IRtmService::downloadMediaToMemory "downloadMediaToMemory" method call.
           
           @note The SDK releases the downloaded file or image immediately after returning this callback.
         
           @param requestId The unique ID of the download request.
           @param memory The memory address where the downloaded file or image is stored.
           @param length The size of the downloaded file or image.
           @param code Error codes. See #DOWNLOAD_MEDIA_ERR_CODE.
           */
            virtual void onMediaDownloadToMemoryResult(long long requestId, const char* memory, long long length, agora::rtm::DOWNLOAD_MEDIA_ERR_CODE code) override;
            
          /**
           Reports the result of the \ref agora::rtm::IRtmService::cancelMediaDownload "cancelMediaDownload" or \ref agora::rtm::IRtmService::cancelMediaUpload "cancelMediaUpload" method call.
         
           @param requestId The unique ID of the cancel request.
           @param code Error codes. See #CANCEL_MEDIA_ERR_CODE.
           */
            virtual void onMediaCancelResult(long long requestId, agora::rtm::CANCEL_MEDIA_ERR_CODE code) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::queryPeersOnlineStatus "queryPeersOnlineStatus" method call.

           @param requestId The unique ID of this request.
           @param peersStatus The online status of the peer. See PeerOnlineStatus.
           @param peerCount The number of the queried peers.
           @param errorCode Error Codes. See #QUERY_PEERS_ONLINE_STATUS_ERR.
           */
            virtual void onQueryPeersOnlineStatusResult(long long requestId, const agora::rtm::PeerOnlineStatus* peersStatus, int peerCount, agora::rtm::QUERY_PEERS_ONLINE_STATUS_ERR errorCode) override;

          /**
           Returns the result of the \ref agora::rtm::IRtmService::subscribePeersOnlineStatus "subscribePeersOnlineStatus" or \ref agora::rtm::IRtmService::unsubscribePeersOnlineStatus "unsubscribePeersOnlineStatus" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #PEER_SUBSCRIPTION_STATUS_ERR.
           */
            virtual void onSubscriptionRequestResult(long long requestId, agora::rtm::PEER_SUBSCRIPTION_STATUS_ERR errorCode) override;

          /**
           Returns the result of the \ref agora::rtm::IRtmService::queryPeersBySubscriptionOption "queryPeersBySubscriptionOption" method call.

           @param requestId The unique ID of this request.
           @param peerIds A user ID array of the specified users, to whom you subscribe.
           @param peerCount Count of the peer(s).
           @param errorCode Error Codes. See #QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR.
           */
            virtual void onQueryPeersBySubscriptionOptionResult(long long requestId, const char* peerIds[], int peerCount, agora::rtm::QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode) override;

          /**
           Occurs when the online status of the peers, to whom you subscribe, changes.
           
           - When the subscription to the online status of specified peer(s) succeeds, the SDK returns this callback to report the online status of peers, to whom you subscribe.
           - When the online status of the peers, to whom you subscribe, changes, the SDK returns this callback to report whose online status has changed.
           - If the online status of the peers, to whom you subscribe, changes when the SDK is reconnecting to the server, the SDK returns this callback to report whose online status has changed when successfully reconnecting to the server.
           
           @param peersStatus An array of peers' online states. See PeerOnlineStatus.
           @param peerCount Count of the peer(s), whose online status changes.
           */
            virtual void onPeersOnlineStatusChanged(const agora::rtm::PeerOnlineStatus peersStatus[], int peerCount) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::setLocalUserAttributes "setLocalUserAttributes" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onSetLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::addOrUpdateLocalUserAttributes "addOrUpdateLocalUserAttributes" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onAddOrUpdateLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::deleteLocalUserAttributesByKeys "deleteLocalUserAttributesByKeys" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onDeleteLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::clearLocalUserAttributes "clearLocalUserAttributes" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onClearLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;

          /**
           Reports the result of the \ref agora::rtm::IRtmService::getUserAttributes "getUserAttributes" or \ref agora::rtm::IRtmService::getUserAttributesByKeys "getUserAttributesByKeys" method call.

           @param requestId The unique ID of this request.
           @param userId The user ID of the specified user.
           @param attributes An array of the returned attributes. See RtmAttribute.
           @param numberOfAttributes The total number of the user's attributes
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onGetUserAttributesResult(long long requestId, const char* userId, const agora::rtm::RtmAttribute* attributes, int numberOfAttributes, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;
          
          /**
           Reports the result of the \ref agora::rtm::IRtmService::setChannelAttributes "setChannelAttributes" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onSetChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;
          
          /**
           Reports the result of the \ref agora::rtm::IRtmService::addOrUpdateChannelAttributes "addOrUpdateChannelAttributes" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onAddOrUpdateChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;
          
          /**
           Reports the result of the \ref agora::rtm::IRtmService::deleteChannelAttributesByKeys "deleteChannelAttributesByKeys" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onDeleteChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;
          
          /**
           Reports the result of the \ref agora::rtm::IRtmService::clearChannelAttributes "clearChannelAttributes" method call.

           @param requestId The unique ID of this request.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onClearChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;
          /**
           Reports the result of the \ref agora::rtm::IRtmService::getChannelAttributes "getChannelAttributes" or \ref agora::rtm::IRtmService::getChannelAttributesByKeys "getChannelAttributesByKeys" method call.

           @param requestId The unique ID of this request.
           @param attributes An array of the returned channel attributes.
           @param numberOfAttributes The total number of the attributes.
           @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
           */
            virtual void onGetChannelAttributesResult(long long requestId, const agora::rtm::IRtmChannelAttribute* attributes[], int numberOfAttributes, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) override;
           
          /**
           Reports the result of the \ref agora::rtm::IRtmService::getChannelMemberCount "getChannelMemberCount" method call.

           @param requestId The unique ID of this request.
           @param channelMemberCounts An array of the channel member counts.
           @param channelCount The total number of the channels.
           @param errorCode Error Codes. See #GET_CHANNEL_MEMBER_COUNT_ERR_CODE.
           */
            virtual void onGetChannelMemberCountResult(long long requestId, const agora::rtm::ChannelMemberCount* channelMemberCounts , int channelCount, agora::rtm::GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode) override;
        };
    }
}

#endif /* RtmServiceEventHandler_hpp */
