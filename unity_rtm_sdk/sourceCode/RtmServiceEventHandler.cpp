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
namespace rtm {
RtmServiceEventHandler::RtmServiceEventHandler(
    int _id,
    CRtmServiceEventHandler* handler) {
  handlerId = _id;
  _c_rtm_service_event_handler = handler;
}

RtmServiceEventHandler::~RtmServiceEventHandler() {
  _c_rtm_service_event_handler = nullptr;
}

/**
Occurs when a user logs in the Agora RTM system.

The local user receives this callback when the \ref
agora::rtm::IRtmService::login "login" method call succeeds.
*/
void RtmServiceEventHandler::onLoginSuccess() {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onLoginSuccess");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onLoginSuccess(handlerId);
}

/**
Occurs when a user fails to log in the Agora RTM system.

The local user receives this callback when the \ref
agora::rtm::IRtmService::login "login" method call fails. See \ref
agora::rtm::LOGIN_ERR_CODE "LOGIN_ERR_CODE" for the error codes.
*/
void RtmServiceEventHandler::onLoginFailure(
    agora::rtm::LOGIN_ERR_CODE errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onLoginFailure");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onLoginFailure(handlerId, int(errorCode));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::renewToken "renewToken"
method call.

@param token Your new token.
@param errorCode The error code. See #RENEW_TOKEN_ERR_CODE.
*/
void RtmServiceEventHandler::onRenewTokenResult(
    const char* token,
    agora::rtm::RENEW_TOKEN_ERR_CODE errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onRenewTokenResult(handlerId, token,
                                                      int(errorCode));
}

/**
Occurs when the RTM server detects that the RTM token has exceeded the 24-hour
validity period and when the SDK is in the \ref
agora::rtm::CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" state.

- This callback occurs only when the SDK is reconnecting to the server. You will
not receive this callback when the SDK is in the \ref
agora::rtm::CONNECTION_STATE_CONNECTED "CONNECTION_STATE_CONNECTED" state.
- When receiving this callback, generate a new RTM Token on the server and call
the \ref agora::rtm::IRtmService::renewToken "renewToken" method to pass the new
Token on to the server.
*/
void RtmServiceEventHandler::onTokenExpired() {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onTokenExpired(handlerId);
}

/**
Occurs when a user logs out of the Agora RTM system.

The local user receives this callback when the SDK calls the \ref
agora::rtm::IRtmService::logout "logout" method. See \ref
agora::rtm::LOGOUT_ERR_CODE "LOGOUT_ERR_CODE" for the error codes.
*/
void RtmServiceEventHandler::onLogout(agora::rtm::LOGOUT_ERR_CODE errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onLogout(handlerId, int(errorCode));
}

void RtmServiceEventHandler::onUserAttributesUpdated(const char* userId,
                                           const agora::rtm::RtmAttribute* attributes,
                                           int numberOfAttributes, long long revision) {
  std::string szMsg;
  for (int i = 0; i < numberOfAttributes; i++) {
    agora::rtm::RtmAttribute* rtmAttribute =
        (agora::rtm::RtmAttribute*)(attributes + i);
    szMsg.append("\t");
    szMsg.append(rtmAttribute->key);
    szMsg.append("\t");
    szMsg.append(rtmAttribute->value);
    szMsg.append("\t");
    szMsg.append(std::to_string(rtmAttribute->revision));
    szMsg.append("\t");
    szMsg.append(std::to_string(rtmAttribute->lastUpdateTs));
  }

  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onUserAttributesUpdated");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onUserAttributesUpdated(handlerId, userId, szMsg.c_str(), numberOfAttributes, revision);                                           
}

void RtmServiceEventHandler::onSubscribeUserAttributesResult(
              long long requestId, const char* userId,
              agora::rtm::RTM_SUBSCRIBE_ATTRIBUTE_OPERATION_ERR errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onSubscribeUserAttributesResult(handlerId, requestId, userId, int(errorCode));
}

void RtmServiceEventHandler::onUnsubscribeUserAttributesResult(
              long long requestId, const char* userId,
              agora::rtm::RTM_SUBSCRIBE_ATTRIBUTE_OPERATION_ERR errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onUnsubscribeUserAttributesResult(handlerId, requestId, userId, int(errorCode));
}

/**
Occurs when the connection state changes between the SDK and the Agora RTM
system.

@param state The new connection state. See #CONNECTION_STATE.
@param reason The reason for the connection state change. See
#CONNECTION_CHANGE_REASON.
*/
void RtmServiceEventHandler::onConnectionStateChanged(
    agora::rtm::CONNECTION_STATE state,
    agora::rtm::CONNECTION_CHANGE_REASON reason) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onConnectionStateChanged");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onConnectionStateChanged(
        handlerId, int(state), int(reason));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::sendMessageToPeer
"sendMessageToPeer" method call.

@param messageId The ID of the sent message.
@param errorCode The peer-to-peer message state. See #PEER_MESSAGE_ERR_CODE.

*/
void RtmServiceEventHandler::onSendMessageResult(
    long long messageId,
    agora::rtm::PEER_MESSAGE_ERR_CODE errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onSendMessageResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onSendMessageResult(handlerId, messageId,
                                                       int(errorCode));
}

/**
Occurs when receiving a peer-to-peer message.

@param peerId The ID of the message sender.
@param message The received peer-to-peer message. See \ref agora::rtm::IMessage
"IMessage".
*/
void RtmServiceEventHandler::onMessageReceivedFromPeer(
    const char* peerId,
    const agora::rtm::IMessage* message) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onMessageReceivedFromPeer");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onMessageReceivedFromPeer(handlerId, peerId,
                                                             (void*)message);
}

/**
Occurs when receiving a peer-to-peer image message.

@param peerId The ID of the message sender.
@param message The received peer-to-peer image message. See \ref
agora::rtm::IImageMessage "IImageMessage".
*/
void RtmServiceEventHandler::onImageMessageReceivedFromPeer(
    const char* peerId,
    const agora::rtm::IImageMessage* message) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onImageMessageReceivedFromPeer");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onImageMessageReceivedFromPeer(
        handlerId, peerId, (void*)message);
}

/**
Occurs when receiving a peer-to-peer file message.

@param peerId The ID of the message sender.
@param message The received peer-to-peer file message. See \ref
agora::rtm::IFileMessage "IFileMessage".
*/
void RtmServiceEventHandler::onFileMessageReceivedFromPeer(
    const char* peerId,
    const agora::rtm::IFileMessage* message) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onFileMessageReceivedFromPeer");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onFileMessageReceivedFromPeer(
        handlerId, peerId, (void*)message);
}

/**
Reports the progress of an ongoing upload task.

@note
- If the upload task is ongoing, the SDK returns this callback once every
second.
- If the upload task comes to a halt, the SDK stops returning this callback
until the task is going again.

@param requestId The unique ID of the upload request.
@param progress The progress of the ongoing upload task. See \ref
agora::rtm::MediaOperationProgress "MediaOperationProgress".
*/
void RtmServiceEventHandler::onMediaUploadingProgress(
    long long requestId,
    const agora::rtm::MediaOperationProgress& progress) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onMediaUploadingProgress");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onMediaUploadingProgress(
        handlerId, requestId, progress.totalSize, progress.currentSize);
}

/**
Reports the progress of an ongoing download task.

@note
- If the download task is ongoing, the SDK returns this callback once every
second.
- If the download task comes to a halt, the SDK stops returning this callback
until the task is going again.

@param requestId The unique ID of the download request.
@param progress The progress of the ongoing download task. See \ref
agora::rtm::MediaOperationProgress "MediaOperationProgress".
*/
void RtmServiceEventHandler::onMediaDownloadingProgress(
    long long requestId,
    const agora::rtm::MediaOperationProgress& progress) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onMediaDownloadingProgress");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onMediaDownloadingProgress(
        handlerId, requestId, progress.totalSize, progress.currentSize);
}

/**
Reports the result of the \ref
agora::rtm::IRtmService::createFileMessageByUploading
"createFileMessageByUploading" method call.

@param requestId The unique ID of the upload request.
@param fileMessage An \ref agora::rtm::IFileMessage "IFileMessage" instance.
@param code Error codes. See #UPLOAD_MEDIA_ERR_CODE.
*/
void RtmServiceEventHandler::onFileMediaUploadResult(
    long long requestId,
    agora::rtm::IFileMessage* fileMessage,
    agora::rtm::UPLOAD_MEDIA_ERR_CODE code) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onFileMediaUploadResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onFileMediaUploadResult(
        handlerId, requestId, fileMessage, int(code));
}

/**
Reports the result of the \ref
agora::rtm::IRtmService::createImageMessageByUploading
"createImageMessageByUploading" method call.

@param requestId The unique ID of the upload request.
@param imageMessage An \ref agora::rtm::IImageMessage "IImageMessage" instance.
@param code Error codes. See #UPLOAD_MEDIA_ERR_CODE.
*/
void RtmServiceEventHandler::onImageMediaUploadResult(
    long long requestId,
    agora::rtm::IImageMessage* imageMessage,
    agora::rtm::UPLOAD_MEDIA_ERR_CODE code) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onImageMediaUploadResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onImageMediaUploadResult(
        handlerId, requestId, imageMessage, int(code));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::downloadMediaToFile
"downloadMediaToFile" method call.

@param requestId The unique ID of the download request.
@param code Error codes. See #DOWNLOAD_MEDIA_ERR_CODE.
*/
void RtmServiceEventHandler::onMediaDownloadToFileResult(
    long long requestId,
    agora::rtm::DOWNLOAD_MEDIA_ERR_CODE code) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onMediaDownloadToFileResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onMediaDownloadToFileResult(
        handlerId, requestId, int(code));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::downloadMediaToMemory
"downloadMediaToMemory" method call.

@note The SDK releases the downloaded file or image immediately after returning
this callback.

@param requestId The unique ID of the download request.
@param memory The memory address where the downloaded file or image is stored.
@param length The size of the downloaded file or image.
@param code Error codes. See #DOWNLOAD_MEDIA_ERR_CODE.
*/
void RtmServiceEventHandler::onMediaDownloadToMemoryResult(
    long long requestId,
    const char* memory,
    long long length,
    agora::rtm::DOWNLOAD_MEDIA_ERR_CODE code) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onMediaDownloadToMemoryResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onMediaDownloadToMemoryResult(
        handlerId, requestId, memory, length, int(code));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::cancelMediaDownload
"cancelMediaDownload" or \ref agora::rtm::IRtmService::cancelMediaUpload
"cancelMediaUpload" method call.

@param requestId The unique ID of the cancel request.
@param code Error codes. See #CANCEL_MEDIA_ERR_CODE.
*/
void RtmServiceEventHandler::onMediaCancelResult(
    long long requestId,
    agora::rtm::CANCEL_MEDIA_ERR_CODE code) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onMediaCancelResult(handlerId, requestId,
                                                       int(code));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::queryPeersOnlineStatus
"queryPeersOnlineStatus" method call.

@param requestId The unique ID of this request.
@param peersStatus The online status of the peer. See PeerOnlineStatus.
@param peerCount The number of the queried peers.
@param errorCode Error Codes. See #QUERY_PEERS_ONLINE_STATUS_ERR.
*/
void RtmServiceEventHandler::onQueryPeersOnlineStatusResult(
    long long requestId,
    const agora::rtm::PeerOnlineStatus* peersStatus,
    int peerCount,
    agora::rtm::QUERY_PEERS_ONLINE_STATUS_ERR errorCode) {
  if (_c_rtm_service_event_handler) {
    char szMsg[520] = {};
    std::string strPostMsg = "";
    for (int i = 0; i < peerCount; i++) {
      sprintf(szMsg, "%s\t%s\t%d\t%d", strPostMsg.data(), peersStatus->peerId,
              peersStatus->isOnline, peersStatus->onlineState);
      strPostMsg = szMsg;
      peersStatus++;
    }
    sprintf(szMsg, "%s", strPostMsg.data());
    _c_rtm_service_event_handler->_onQueryPeersOnlineStatusResult(
        handlerId, requestId, szMsg, peerCount, errorCode);
  }
}

/**
Returns the result of the \ref
agora::rtm::IRtmService::subscribePeersOnlineStatus "subscribePeersOnlineStatus"
or \ref agora::rtm::IRtmService::unsubscribePeersOnlineStatus
"unsubscribePeersOnlineStatus" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #PEER_SUBSCRIPTION_STATUS_ERR.
*/
void RtmServiceEventHandler::onSubscriptionRequestResult(
    long long requestId,
    agora::rtm::PEER_SUBSCRIPTION_STATUS_ERR errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onSubscriptionRequestResult(
        handlerId, requestId, int(errorCode));
}

/**
Returns the result of the \ref
agora::rtm::IRtmService::queryPeersBySubscriptionOption
"queryPeersBySubscriptionOption" method call.

@param requestId The unique ID of this request.
@param peerIds A user ID array of the specified users, to whom you subscribe.
@param peerCount Count of the peer(s).
@param errorCode Error Codes. See #QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR.
*/
void RtmServiceEventHandler::onQueryPeersBySubscriptionOptionResult(
    long long requestId,
    const char* peerIds[],
    int peerCount,
    agora::rtm::QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onQueryPeersBySubscriptionOptionResult(
        handlerId, requestId, peerIds, peerCount, int(errorCode));
}

/**
Occurs when the online status of the peers, to whom you subscribe, changes.

- When the subscription to the online status of specified peer(s) succeeds, the
SDK returns this callback to report the online status of peers, to whom you
subscribe.
- When the online status of the peers, to whom you subscribe, changes, the SDK
returns this callback to report whose online status has changed.
- If the online status of the peers, to whom you subscribe, changes when the SDK
is reconnecting to the server, the SDK returns this callback to report whose
online status has changed when successfully reconnecting to the server.

@param peersStatus An array of peers' online states. See PeerOnlineStatus.
@param peerCount Count of the peer(s), whose online status changes.
*/
void RtmServiceEventHandler::onPeersOnlineStatusChanged(
    const agora::rtm::PeerOnlineStatus peersStatus[],
    int peerCount) {
  if (_c_rtm_service_event_handler) {
    char szMsg[32768] = {};
    std::string strPostMsg = "";
    for (int i = 0; i < peerCount; i++) {
      const agora::rtm::PeerOnlineStatus peerOnline = peersStatus[i];
      sprintf(szMsg, "%s\t%s\t%d\t%d", strPostMsg.data(), peerOnline.peerId,
              peerOnline.isOnline, peerOnline.onlineState);
      strPostMsg = szMsg;
    }
    sprintf(szMsg, "%s", strPostMsg.data());
    _c_rtm_service_event_handler->_onPeersOnlineStatusChanged(handlerId, szMsg,
                                                              peerCount);
  }
}

/**
Reports the result of the \ref agora::rtm::IRtmService::setLocalUserAttributes
"setLocalUserAttributes" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onSetLocalUserAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onSetLocalUserAttributesResult(
        handlerId, requestId, int(errorCode));
}

/**
Reports the result of the \ref
agora::rtm::IRtmService::addOrUpdateLocalUserAttributes
"addOrUpdateLocalUserAttributes" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onAddOrUpdateLocalUserAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onAddOrUpdateLocalUserAttributesResult(
        handlerId, requestId, int(errorCode));
}

/**
Reports the result of the \ref
agora::rtm::IRtmService::deleteLocalUserAttributesByKeys
"deleteLocalUserAttributesByKeys" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onDeleteLocalUserAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onDeleteLocalUserAttributesResult(
        handlerId, requestId, int(errorCode));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::clearLocalUserAttributes
"clearLocalUserAttributes" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onClearLocalUserAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onClearLocalUserAttributesResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onClearLocalUserAttributesResult(
        handlerId, requestId, int(errorCode));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::getUserAttributes
"getUserAttributes" or \ref agora::rtm::IRtmService::getUserAttributesByKeys
"getUserAttributesByKeys" method call.

@param requestId The unique ID of this request.
@param userId The user ID of the specified user.
@param attributes An array of the returned attributes. See RtmAttribute.
@param numberOfAttributes The total number of the user's attributes
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onGetUserAttributesResult(
    long long requestId,
    const char* userId,
    const agora::rtm::RtmAttribute* attributes,
    int numberOfAttributes,
    long long revision,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  std::string szMsg;
  for (int i = 0; i < numberOfAttributes; i++) {
    agora::rtm::RtmAttribute* rtmAttribute =
        (agora::rtm::RtmAttribute*)(attributes + i);
    szMsg.append("\t");
    szMsg.append(rtmAttribute->key);
    szMsg.append("\t");
    szMsg.append(rtmAttribute->value);
    szMsg.append("\t");
    szMsg.append(std::to_string(rtmAttribute->revision));
    szMsg.append("\t");
    szMsg.append(std::to_string(rtmAttribute->lastUpdateTs));
  }

  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onGetUserAttributesResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onGetUserAttributesResult(
        handlerId, requestId, userId, szMsg.c_str(), numberOfAttributes, revision,
        int(errorCode));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::setChannelAttributes
"setChannelAttributes" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onSetChannelAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onSetChannelAttributesResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onSetChannelAttributesResult(
        handlerId, requestId, int(errorCode));
}

/**
Reports the result of the \ref
agora::rtm::IRtmService::addOrUpdateChannelAttributes
"addOrUpdateChannelAttributes" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onAddOrUpdateChannelAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onAddOrUpdateChannelAttributesResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onAddOrUpdateChannelAttributesResult(
        handlerId, requestId, int(errorCode));
}

/**
Reports the result of the \ref
agora::rtm::IRtmService::deleteChannelAttributesByKeys
"deleteChannelAttributesByKeys" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onDeleteChannelAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onDeleteChannelAttributesResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onDeleteChannelAttributesResult(
        handlerId, requestId, int(errorCode));
}

/**
Reports the result of the \ref agora::rtm::IRtmService::clearChannelAttributes
"clearChannelAttributes" method call.

@param requestId The unique ID of this request.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onClearChannelAttributesResult(
    long long requestId,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onClearChannelAttributesResult");
  if (_c_rtm_service_event_handler)
    _c_rtm_service_event_handler->_onClearChannelAttributesResult(
        handlerId, requestId, int(errorCode));
}
/**
Reports the result of the \ref agora::rtm::IRtmService::getChannelAttributes
"getChannelAttributes" or \ref
agora::rtm::IRtmService::getChannelAttributesByKeys "getChannelAttributesByKeys"
method call.

@param requestId The unique ID of this request.
@param attributes An array of the returned channel attributes.
@param numberOfAttributes The total number of the attributes.
@param errorCode Error Codes. See #ATTRIBUTE_OPERATION_ERR.
*/
void RtmServiceEventHandler::onGetChannelAttributesResult(
    long long requestId,
    const agora::rtm::IRtmChannelAttribute* attributes[],
    int numberOfAttributes,
    long long revision,
    agora::rtm::ATTRIBUTE_OPERATION_ERR errorCode) {
    if (_c_rtm_service_event_handler) {
      std::string szMsg;
      for (int i = 0; i < numberOfAttributes; i++) {
        const agora::rtm::IRtmChannelAttribute* rtmAttribute = attributes[i];
        if (rtmAttribute && rtmAttribute->getKey() && rtmAttribute->getValue() &&
            rtmAttribute->getLastUpdateUserId()) {
          szMsg.append("\t");
          szMsg.append(rtmAttribute->getKey());
          szMsg.append("\t");
          szMsg.append(rtmAttribute->getValue());
          szMsg.append("\t");
          szMsg.append(std::to_string(rtmAttribute->getLastUpdateTs()));
          szMsg.append("\t");
          szMsg.append(rtmAttribute->getLastUpdateUserId());
          szMsg.append("\t");
          szMsg.append(std::to_string(rtmAttribute->getRevision()));
        }
      }

    _c_rtm_service_event_handler->_onGetChannelAttributesResult(
        handlerId, requestId, szMsg.c_str(), numberOfAttributes, revision, int(errorCode));
  }
}

/**
Reports the result of the \ref agora::rtm::IRtmService::getChannelMemberCount
"getChannelMemberCount" method call.

@param requestId The unique ID of this request.
@param channelMemberCounts An array of the channel member counts.
@param channelCount The total number of the channels.
@param errorCode Error Codes. See #GET_CHANNEL_MEMBER_COUNT_ERR_CODE.
*/
void RtmServiceEventHandler::onGetChannelMemberCountResult(
    long long requestId,
    const agora::rtm::ChannelMemberCount* channelMemberCounts,
    int channelCount,
    agora::rtm::GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm: RtmServiceEventHandler onGetChannelMemberCountResult");
  if (_c_rtm_service_event_handler) {
    char szMsg[520] = {};
    std::string strPostMsg = "";
    for (int i = 0; i < channelCount; i++) {
      const agora::rtm::ChannelMemberCount* channelMember =
          channelMemberCounts++;
      if (channelMember) {
        sprintf(szMsg, "%s\t%s\t%d", strPostMsg.data(),
                channelMember->channelId, channelMember->count);
        strPostMsg = szMsg;
      }
    }
    sprintf(szMsg, "%s", strPostMsg.data());
    _c_rtm_service_event_handler->_onGetChannelMemberCountResult(
        handlerId, requestId, szMsg, channelCount, int(errorCode));
  }
}
}
}  // namespace unity
}  // namespace agora
