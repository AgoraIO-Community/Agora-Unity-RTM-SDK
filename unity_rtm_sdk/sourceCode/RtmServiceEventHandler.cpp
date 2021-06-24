//
//  RtmServiceEventHandler.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "RtmServiceEventHandler.h"

namespace agora {
    namespace unity {
    RtmServiceEventHandler::RtmServiceEventHandler(int _id, FUNC_onLoginSuccess onLoginSuccess,
          FUNC_onLoginFailure onLoginFailure,
          FUNC_onRenewTokenResult onRenewTokenResult,
          FUNC_onTokenExpired onTokenExpired,
          FUNC_onLogout onLogout,
          FUNC_onConnectionStateChanged onConnectionStateChanged,
          FUNC_onSendMessageResult onSendMessageResult,
          FUNC_onMessageReceivedFromPeer onMessageReceivedFromPeer,
          FUNC_onImageMessageReceivedFromPeer onImageMessageReceivedFromPeer,
          FUNC_onFileMessageReceivedFromPeer onFileMessageReceivedFromPeer,
          FUNC_onMediaUploadingProgress onMediaUploadingProgress,
          FUNC_onMediaDownloadingProgress onMediaDownloadingProgress,
          FUNC_onFileMediaUploadResult onFileMediaUploadResult,
          FUNC_onImageMediaUploadResult onImageMediaUploadResult,
          FUNC_onMediaDownloadToFileResult onMediaDownloadToFileResult,
          FUNC_onMediaDownloadToMemoryResult onMediaDownloadToMemoryResult,
          FUNC_onMediaCancelResult onMediaCancelResult,
          FUNC_onQueryPeersOnlineStatusResult onQueryPeersOnlineStatusResult,
          FUNC_onSubscriptionRequestResult onSubscriptionRequestResult,
          FUNC_onQueryPeersBySubscriptionOptionResult onQueryPeersBySubscriptionOptionResult,
          FUNC_onPeersOnlineStatusChanged onPeersOnlineStatusChanged,
          FUNC_onSetLocalUserAttributesResult onSetLocalUserAttributesResult,
          FUNC_onDeleteLocalUserAttributesResult onDeleteLocalUserAttributesResult,
          FUNC_onClearLocalUserAttributesResult onClearLocalUserAttributesResult,
          FUNC_onGetUserAttributesResult onGetUserAttributesResult,
          FUNC_onSetChannelAttributesResult onSetChannelAttributesResult,
          FUNC_onAddOrUpdateLocalUserAttributesResult onAddOrUpdateLocalUserAttributesResult,
          FUNC_onDeleteChannelAttributesResult onDeleteChannelAttributesResult,
          FUNC_onClearChannelAttributesResult onClearChannelAttributesResult,
          FUNC_onGetChannelAttributesResult onGetChannelAttributesResult,
          FUNC_onGetChannelMemberCountResult onGetChannelMemberCountResult)
    {
        handlerId = _id;
        _onLoginSuccess = onLoginSuccess;
        _onLoginFailure = onLoginFailure;
        _onRenewTokenResult = onRenewTokenResult;
        _onTokenExpired = onTokenExpired;
        _onLogout = onLogout;
        _onConnectionStateChanged = onConnectionStateChanged;
        _onSendMessageResult = onSendMessageResult;
        _onMessageReceivedFromPeer = onMessageReceivedFromPeer;
        _onImageMessageReceivedFromPeer = onImageMessageReceivedFromPeer;
        _onFileMessageReceivedFromPeer = onFileMessageReceivedFromPeer;
        _onMediaUploadingProgress = onMediaUploadingProgress;
        _onMediaDownloadingProgress = onMediaDownloadingProgress;
        _onFileMediaUploadResult = onFileMediaUploadResult;
        _onImageMediaUploadResult = onImageMediaUploadResult;
        _onMediaDownloadToFileResult = onMediaDownloadToFileResult;
        _onMediaDownloadToMemoryResult = onMediaDownloadToMemoryResult;
        _onMediaCancelResult = onMediaCancelResult;
        _onQueryPeersOnlineStatusResult = onQueryPeersOnlineStatusResult;
        _onSubscriptionRequestResult = onSubscriptionRequestResult;
        _onQueryPeersBySubscriptionOptionResult = onQueryPeersBySubscriptionOptionResult;
        _onPeersOnlineStatusChanged = onPeersOnlineStatusChanged;
        _onSetLocalUserAttributesResult = onSetLocalUserAttributesResult;
        _onDeleteLocalUserAttributesResult = onDeleteLocalUserAttributesResult;
        _onClearLocalUserAttributesResult = onClearLocalUserAttributesResult;
        _onGetUserAttributesResult = onGetUserAttributesResult;
        _onSetChannelAttributesResult = onSetChannelAttributesResult;
        _onAddOrUpdateLocalUserAttributesResult = onAddOrUpdateLocalUserAttributesResult;
        _onDeleteChannelAttributesResult = onDeleteChannelAttributesResult;
        _onClearChannelAttributesResult = onClearChannelAttributesResult;
        _onGetChannelAttributesResult = onGetChannelAttributesResult;
        _onGetChannelMemberCountResult = onGetChannelMemberCountResult;
    }

    RtmServiceEventHandler::~RtmServiceEventHandler()
    {
        _onLoginSuccess = nullptr;
        _onLoginFailure = nullptr;
        _onRenewTokenResult = nullptr;
        _onTokenExpired = nullptr;
        _onLogout = nullptr;
        _onConnectionStateChanged = nullptr;
        _onSendMessageResult = nullptr;
        _onMessageReceivedFromPeer = nullptr;
        _onImageMessageReceivedFromPeer = nullptr;
        _onFileMessageReceivedFromPeer = nullptr;
        _onMediaUploadingProgress = nullptr;
        _onMediaDownloadingProgress = nullptr;
        _onFileMediaUploadResult = nullptr;
        _onImageMediaUploadResult = nullptr;
        _onMediaDownloadToFileResult = nullptr;
        _onMediaDownloadToMemoryResult = nullptr;
        _onMediaCancelResult = nullptr;
        _onQueryPeersOnlineStatusResult = nullptr;
        _onSubscriptionRequestResult = nullptr;
        _onQueryPeersBySubscriptionOptionResult = nullptr;
        _onPeersOnlineStatusChanged = nullptr;
        _onSetLocalUserAttributesResult = nullptr;
        _onDeleteLocalUserAttributesResult = nullptr;
        _onClearLocalUserAttributesResult = nullptr;
        _onGetUserAttributesResult = nullptr;
        _onSetChannelAttributesResult = nullptr;
        _onAddOrUpdateLocalUserAttributesResult = nullptr;
        _onDeleteChannelAttributesResult = nullptr;
        _onClearChannelAttributesResult = nullptr;
        _onGetChannelAttributesResult = nullptr;
        _onGetChannelMemberCountResult = nullptr;
    }

    /**
    Occurs when a user logs in the Agora RTM system.

    The local user receives this callback when the \ref agora::rtm::IRtmService::login "login" method call succeeds.
    */
    void RtmServiceEventHandler::onLoginSuccess()
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onLoginSuccess");
        if (_onLoginSuccess)
            _onLoginSuccess(handlerId);
    }

    /**
    Occurs when a user fails to log in the Agora RTM system.

    The local user receives this callback when the \ref agora::rtm::IRtmService::login "login" method call fails. See \ref agora::rtm::LOGIN_ERR_CODE "LOGIN_ERR_CODE" for the error codes.
    */
    void RtmServiceEventHandler::onLoginFailure(agora::rtm::LOGIN_ERR_CODE errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onLoginFailure");
        if (_onLoginFailure)
            _onLoginFailure(handlerId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::renewToken "renewToken" method call.

    @param token Your new token.
    @param errorCode The error code. See #RENEW_TOKEN_ERR_CODE.
    */
    void RtmServiceEventHandler::onRenewTokenResult(const char* token, agora::rtm::RENEW_TOKEN_ERR_CODE errorCode)
    {
        if (_onRenewTokenResult)
            _onRenewTokenResult(handlerId, token, int(errorCode));
    }

    /**
    Occurs when the RTM server detects that the RTM token has exceeded the 24-hour validity period and when the SDK is in the \ref agora::rtm::CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" state.

    - This callback occurs only when the SDK is reconnecting to the server. You will not receive this callback when the SDK is in the \ref agora::rtm::CONNECTION_STATE_CONNECTED "CONNECTION_STATE_CONNECTED" state.
    - When receiving this callback, generate a new RTM Token on the server and call the \ref agora::rtm::IRtmService::renewToken "renewToken" method to pass the new Token on to the server.
    */
    void RtmServiceEventHandler::onTokenExpired()
    {
        if (_onTokenExpired)
            _onTokenExpired(handlerId);
    }

    /**
    Occurs when a user logs out of the Agora RTM system.

    The local user receives this callback when the SDK calls the \ref agora::rtm::IRtmService::logout "logout" method. See \ref agora::rtm::LOGOUT_ERR_CODE "LOGOUT_ERR_CODE" for the error codes.
    */
    void RtmServiceEventHandler::onLogout(agora::rtm::LOGOUT_ERR_CODE errorCode)
    {
        if (_onLogout)
            _onLogout(handlerId, int(errorCode));
    }

    /**
    Occurs when the connection state changes between the SDK and the Agora RTM system.

    @param state The new connection state. See #CONNECTION_STATE.
    @param reason The reason for the connection state change. See #CONNECTION_CHANGE_REASON.
    */
    void RtmServiceEventHandler::onConnectionStateChanged(agora::rtm::CONNECTION_STATE state, agora::rtm::CONNECTION_CHANGE_REASON reason)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onConnectionStateChanged");
        if (_onConnectionStateChanged)
            _onConnectionStateChanged(handlerId, int(state), int(reason));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::sendMessageToPeer "sendMessageToPeer" method call.

    @param messageId The ID of the sent message.
    @param errorCode The peer-to-peer message state. See #PEER_MESSAGE_ERR_CODE.

    */
    void RtmServiceEventHandler::onSendMessageResult(long long messageId, agora::rtm::PEER_MESSAGE_ERR_CODE errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onSendMessageResult");
        if (_onSendMessageResult)
            _onSendMessageResult(handlerId, messageId, int(errorCode));
    }

    /**
    Occurs when receiving a peer-to-peer message.

    @param peerId The ID of the message sender.
    @param message The received peer-to-peer message. See \ref agora::rtm::IMessage "IMessage".
    */
    void RtmServiceEventHandler::onMessageReceivedFromPeer(const char *peerId, const agora::rtm::IMessage *message)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onMessageReceivedFromPeer");
        if (_onMessageReceivedFromPeer)
            _onMessageReceivedFromPeer(handlerId, peerId, (void *)message);
    }

    /**
    Occurs when receiving a peer-to-peer image message.

    @param peerId The ID of the message sender.
    @param message The received peer-to-peer image message. See \ref agora::rtm::IImageMessage "IImageMessage".
    */
    void RtmServiceEventHandler::onImageMessageReceivedFromPeer(const char *peerId, const   agora::rtm::IImageMessage* message)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onImageMessageReceivedFromPeer");
        if (_onImageMessageReceivedFromPeer)
            _onImageMessageReceivedFromPeer(handlerId, peerId, (void *)message);
    }

    /**
    Occurs when receiving a peer-to-peer file message.

    @param peerId The ID of the message sender.
    @param message The received peer-to-peer file message. See \ref agora::rtm::IFileMessage "IFileMessage".
    */
    void RtmServiceEventHandler::onFileMessageReceivedFromPeer(const char *peerId, const agora::rtm::IFileMessage* message)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onFileMessageReceivedFromPeer");
        if (_onFileMessageReceivedFromPeer)
            _onFileMessageReceivedFromPeer(handlerId, peerId, (void *)message);
    }

    /**
    Reports the progress of an ongoing upload task.

    @note
    - If the upload task is ongoing, the SDK returns this callback once every second.
    - If the upload task comes to a halt, the SDK stops returning this callback until the task is going again.

    @param requestId The unique ID of the upload request.
    @param progress The progress of the ongoing upload task. See \ref agora::rtm::MediaOperationProgress "MediaOperationProgress".
    */
    void RtmServiceEventHandler::onMediaUploadingProgress(long long requestId, const agora::rtm::MediaOperationProgress &progress)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onMediaUploadingProgress");
        if (_onMediaUploadingProgress)
            _onMediaUploadingProgress(handlerId, requestId, progress.totalSize, progress.currentSize);
    }

    /**
    Reports the progress of an ongoing download task.

    @note
    - If the download task is ongoing, the SDK returns this callback once every second.
    - If the download task comes to a halt, the SDK stops returning this callback until the task is going again.

    @param requestId The unique ID of the download request.
    @param progress The progress of the ongoing download task. See \ref agora::rtm::MediaOperationProgress "MediaOperationProgress".
    */
    void RtmServiceEventHandler::onMediaDownloadingProgress(long long requestId, const agora::rtm::MediaOperationProgress &progress)
    {
         agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onMediaDownloadingProgress");
        if (_onMediaDownloadingProgress)
            _onMediaDownloadingProgress(handlerId, requestId, progress.totalSize, progress.currentSize);
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::createFileMessageByUploading "createFileMessageByUploading" method call.

    @param requestId The unique ID of the upload request.
    @param fileMessage An \ref agora::rtm::IFileMessage "IFileMessage" instance.
    @param code Error codes. See #UPLOAD_MEDIA_ERR_CODE.
    */
    void RtmServiceEventHandler::onFileMediaUploadResult(long long requestId, agora::rtm::IFileMessage* fileMessage, agora::rtm::UPLOAD_MEDIA_ERR_CODE code)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onFileMediaUploadResult");
        if (_onFileMediaUploadResult)
            _onFileMediaUploadResult(handlerId, requestId, fileMessage, int(code));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::createImageMessageByUploading "createImageMessageByUploading" method call.

    @param requestId The unique ID of the upload request.
    @param imageMessage An \ref agora::rtm::IImageMessage "IImageMessage" instance.
    @param code Error codes. See #UPLOAD_MEDIA_ERR_CODE.
    */
    void RtmServiceEventHandler::onImageMediaUploadResult(long long requestId, agora::rtm::IImageMessage* imageMessage, agora::rtm::UPLOAD_MEDIA_ERR_CODE code)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onImageMediaUploadResult");
        if (_onImageMediaUploadResult)
            _onImageMediaUploadResult(handlerId, requestId, imageMessage, int(code));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::downloadMediaToFile "downloadMediaToFile" method call.

    @param requestId The unique ID of the download request.
    @param code Error codes. See #DOWNLOAD_MEDIA_ERR_CODE.
    */
    void RtmServiceEventHandler::onMediaDownloadToFileResult(long long requestId, agora::rtm::DOWNLOAD_MEDIA_ERR_CODE code)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onMediaDownloadToFileResult");
        if (_onMediaDownloadToFileResult)
            _onMediaDownloadToFileResult(handlerId, requestId, int(code));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::downloadMediaToMemory "downloadMediaToMemory" method call.

    @note The SDK releases the downloaded file or image immediately after returning this callback.

    @param requestId The unique ID of the download request.
    @param memory The memory address where the downloaded file or image is stored.
    @param length The size of the downloaded file or image.
    @param code Error codes. See #DOWNLOAD_MEDIA_ERR_CODE.
    */
    void RtmServiceEventHandler::onMediaDownloadToMemoryResult(long long requestId, const char* memory, long long length, agora::rtm::DOWNLOAD_MEDIA_ERR_CODE code)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onMediaDownloadToMemoryResult");
        if (_onMediaDownloadToMemoryResult)
            _onMediaDownloadToMemoryResult(handlerId, requestId, memory, length, int(code));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::cancelMediaDownload "cancelMediaDownload" or \ref agora::rtm::IRtmService::cancelMediaUpload "cancelMediaUpload" method call.

    @param requestId The unique ID of the cancel request.
    @param code Error codes. See #CANCEL_MEDIA_ERR_CODE.
    */
    void RtmServiceEventHandler::onMediaCancelResult(long long requestId, agora::rtm::CANCEL_MEDIA_ERR_CODE code)
    {
        if (_onMediaCancelResult)
            _onMediaCancelResult(handlerId, requestId, int(code));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::queryPeersOnlineStatus "queryPeersOnlineStatus" method call.

    @param requestId The unique ID of this request.
    @param peersStatus The online status of the peer. See PeerOnlineStatus.
    @param peerCount The number of the queried peers.
    @param errorCode Error Codes. See #QUERY_PEERS_ONLINE_STATUS_ERR.
    */
    void RtmServiceEventHandler::onQueryPeersOnlineStatusResult(long long requestId, const agora::rtm::PeerOnlineStatus* peersStatus, int peerCount, agora::rtm::QUERY_PEERS_ONLINE_STATUS_ERR errorCode)
    {
        if (_onQueryPeersOnlineStatusResult) {
            char szMsg[520] = {};
             std::string strPostMsg = "";
             for (int i = 0; i < peerCount; i++)
             {
                 sprintf(szMsg, "%s\t%s\t%d\t%d", strPostMsg.data(), peersStatus->peerId, peersStatus->isOnline, peersStatus->onlineState);
                 strPostMsg = szMsg;
                 peersStatus++;
             }
             sprintf(szMsg, "%s", strPostMsg.data());
             _onQueryPeersOnlineStatusResult(handlerId, requestId, szMsg, peerCount, errorCode);
        }
    }

    /**
    Returns the result of the \ref agora::rtm::IRtmService::subscribePeersOnlineStatus "subscribePeersOnlineStatus" or \ref agora::rtm::IRtmService::unsubscribePeersOnlineStatus "unsubscribePeersOnlineStatus" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #PEER_SUBSCRIPTION_STATUS_ERR.
    */
    void RtmServiceEventHandler::onSubscriptionRequestResult(long long requestId, agora::rtm::PEER_SUBSCRIPTION_STATUS_ERR errorCode)
    {
        if (_onSubscriptionRequestResult)
            _onSubscriptionRequestResult(handlerId, requestId, int(errorCode));
    }

    /**
    Returns the result of the \ref agora::rtm::IRtmService::queryPeersBySubscriptionOption "queryPeersBySubscriptionOption" method call.

    @param requestId The unique ID of this request.
    @param peerIds A user ID array of the specified users, to whom you subscribe.
    @param peerCount Count of the peer(s).
    @param errorCode Error Codes. See #QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR.
    */
    void RtmServiceEventHandler::onQueryPeersBySubscriptionOptionResult(long long requestId, const char* peerIds[], int peerCount, agora::rtm::QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode)
    {
        if (_onQueryPeersBySubscriptionOptionResult)
            _onQueryPeersBySubscriptionOptionResult(handlerId, requestId, peerIds, peerCount, int(errorCode));
    }

    /**
    Occurs when the online status of the peers, to whom you subscribe, changes.

    - When the subscription to the online status of specified peer(s) succeeds, the SDK returns this callback to report the online status of peers, to whom you subscribe.
    - When the online status of the peers, to whom you subscribe, changes, the SDK returns this callback to report whose online status has changed.
    - If the online status of the peers, to whom you subscribe, changes when the SDK is reconnecting to the server, the SDK returns this callback to report whose online status has changed when successfully reconnecting to the server.

    @param peersStatus An array of peers' online states. See PeerOnlineStatus.
    @param peerCount Count of the peer(s), whose online status changes.
    */
    void RtmServiceEventHandler::onPeersOnlineStatusChanged(const agora::rtm::PeerOnlineStatus peersStatus[], int peerCount)
    {
        if (_onPeersOnlineStatusChanged) {
            char szMsg[520] = {};
             std::string strPostMsg = "";
             for (int i = 0; i < peerCount; i++)
             {
                 const agora::rtm::PeerOnlineStatus peerOnline = peersStatus[i];
                 sprintf(szMsg, "%s\t%s\t%d\t%d", strPostMsg.data(), peerOnline.peerId, peerOnline.isOnline, peerOnline.onlineState);
                 strPostMsg = szMsg;
             }
             sprintf(szMsg, "%s", strPostMsg.data());
             _onPeersOnlineStatusChanged(handlerId, szMsg, peerCount);
        }
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::setLocalUserAttributes "setLocalUserAttributes" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onSetLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        if (_onSetLocalUserAttributesResult)
            _onSetLocalUserAttributesResult(handlerId, requestId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::addOrUpdateLocalUserAttributes "addOrUpdateLocalUserAttributes" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onAddOrUpdateLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        if (_onAddOrUpdateLocalUserAttributesResult)
            _onAddOrUpdateLocalUserAttributesResult(handlerId, requestId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::deleteLocalUserAttributesByKeys "deleteLocalUserAttributesByKeys" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onDeleteLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        if (_onDeleteLocalUserAttributesResult)
            _onDeleteLocalUserAttributesResult(handlerId, requestId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::clearLocalUserAttributes "clearLocalUserAttributes" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onClearLocalUserAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onClearLocalUserAttributesResult");
        if (_onClearLocalUserAttributesResult)
            _onClearLocalUserAttributesResult(handlerId, requestId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::getUserAttributes "getUserAttributes" or \ref agora::rtm::IRtmService::getUserAttributesByKeys "getUserAttributesByKeys" method call.

    @param requestId The unique ID of this request.
    @param userId The user ID of the specified user.
    @param attributes An array of the returned attributes. See RtmAttribute.
    @param numberOfAttributes The total number of the user's attributes
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onGetUserAttributesResult(long long requestId, const char* userId, const agora::rtm::RtmAttribute* attributes, int numberOfAttributes, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        char szMsg[520] = {};
        std::string strPostMsg = "";
        for (int i = 0; i < numberOfAttributes; i++)
        {
            agora::rtm::RtmAttribute *rtmAttribute = (agora::rtm::RtmAttribute *)(attributes + i);
            sprintf(szMsg, "%s\t%s\t%s", strPostMsg.data(), rtmAttribute->key, rtmAttribute->value);
            strPostMsg = szMsg;
        }
        sprintf(szMsg, "%s", strPostMsg.data());
        
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onGetUserAttributesResult");
        if (_onGetUserAttributesResult)
            _onGetUserAttributesResult(handlerId, requestId, userId, szMsg, numberOfAttributes, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::setChannelAttributes "setChannelAttributes" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onSetChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onSetChannelAttributesResult");
        if (_onSetChannelAttributesResult)
            _onSetChannelAttributesResult(handlerId, requestId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::addOrUpdateChannelAttributes "addOrUpdateChannelAttributes" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onAddOrUpdateChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onAddOrUpdateChannelAttributesResult");
        if (_onAddOrUpdateLocalUserAttributesResult)
            _onAddOrUpdateLocalUserAttributesResult(handlerId, requestId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::deleteChannelAttributesByKeys "deleteChannelAttributesByKeys" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onDeleteChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onDeleteChannelAttributesResult");
        if (_onDeleteChannelAttributesResult)
            _onDeleteChannelAttributesResult(handlerId, requestId, int(errorCode));
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::clearChannelAttributes "clearChannelAttributes" method call.

    @param requestId The unique ID of this request.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onClearChannelAttributesResult(long long requestId, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onClearChannelAttributesResult");
        if (_onClearChannelAttributesResult)
            _onClearChannelAttributesResult(handlerId, requestId, int(errorCode));
    }
    /**
    Reports the result of the \ref agora::rtm::IRtmService::getChannelAttributes "getChannelAttributes" or \ref agora::rtm::IRtmService::getChannelAttributesByKeys "getChannelAttributesByKeys" method call.

    @param requestId The unique ID of this request.
    @param attributes An array of the returned channel attributes.
    @param numberOfAttributes The total number of the attributes.
    @param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
    */
    void RtmServiceEventHandler::onGetChannelAttributesResult(long long requestId, const agora::rtm::IRtmChannelAttribute* attributes[], int numberOfAttributes, agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode)
    {
        if (_onGetChannelAttributesResult)
        {
            char szMsg[520] = {};
            std::string strPostMsg = "";
            for (int i = 0; i < numberOfAttributes; i++)
            {
                const agora::rtm::IRtmChannelAttribute *rtmAttribute = attributes[i];
                if (rtmAttribute && rtmAttribute->getKey() && rtmAttribute->getValue() && rtmAttribute->getLastUpdateUserId()) {
                    sprintf(szMsg, "%s\t%s\t%s\t%lld\t%s", strPostMsg.data(), rtmAttribute->getKey(), rtmAttribute->getValue(), rtmAttribute->getLastUpdateTs(), rtmAttribute->getLastUpdateUserId());
                    strPostMsg = szMsg;
                }
            }
            sprintf(szMsg, "%s", strPostMsg.data());
            _onGetChannelAttributesResult(handlerId, requestId, szMsg, numberOfAttributes, int(errorCode));
        }
    }

    /**
    Reports the result of the \ref agora::rtm::IRtmService::getChannelMemberCount "getChannelMemberCount" method call.

    @param requestId The unique ID of this request.
    @param channelMemberCounts An array of the channel member counts.
    @param channelCount The total number of the channels.
    @param errorCode Error Codes. See #GET_CHANNEL_MEMBER_COUNT_ERR_CODE.
    */
    void RtmServiceEventHandler::onGetChannelMemberCountResult(long long requestId, const agora::rtm::ChannelMemberCount* channelMemberCounts , int channelCount, agora::rtm::GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode)
    {
        agora::unity::LogHelper::getInstance().writeLog("AgoraRtm: RtmServiceEventHandler onGetChannelMemberCountResult");
        if (_onGetChannelMemberCountResult) {
            char szMsg[520] = {};
             std::string strPostMsg = "";
             for (int i = 0; i < channelCount; i++)
             {
                 const agora::rtm::ChannelMemberCount *channelMember = channelMemberCounts++;
                 if (channelMember) {
                     sprintf(szMsg, "%s\t%s\t%d", strPostMsg.data(), channelMember->channelId, channelMember->count);
                     strPostMsg = szMsg;
                 }
             }
             sprintf(szMsg, "%s", strPostMsg.data());
             _onGetChannelAttributesResult(handlerId, requestId, szMsg, channelCount, int(errorCode));
        }
    }
    }
}
