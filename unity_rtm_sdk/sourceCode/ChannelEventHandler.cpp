//
//  ChannelEventHandler.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/11.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "ChannelEventHandler.h"

namespace agora {
namespace unity {
namespace rtm {

ChannelEventHandler::ChannelEventHandler()
    : _c_channel_event_handler(nullptr) {}

ChannelEventHandler::ChannelEventHandler(
    int id,
    CChannelEventHandler* channelEventHandler)
    : handlerId(id), _c_channel_event_handler(channelEventHandler) {}

ChannelEventHandler::~ChannelEventHandler() {
  this->clear();
}

void ChannelEventHandler::clear() {
  _c_channel_event_handler = nullptr;
}

void ChannelEventHandler::onJoinSuccess() {
  agora::unity::rtm::LogHelper::getInstance().writeLog("AgoraRtm:  onJoinSuccess");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onJoinSuccess(handlerId);
}

/**
 Occurs when failing to join a channel.

 The local user receives this callback when the \ref agora::rtm::IChannel::join
 "join" method call fails.

 @param errorCode The error code. See #JOIN_CHANNEL_ERR.
 */
void ChannelEventHandler::onJoinFailure(agora::rtm::JOIN_CHANNEL_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog("AgoraRtm:  onJoinFailure");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onJoinFailure(handlerId, int(errorCode));
}

/**
 Returns the result of the \ref agora::rtm::IChannel::leave "leave" method call.

 @param errorCode The error code. See #LEAVE_CHANNEL_ERR.
 */
void ChannelEventHandler::onLeave(agora::rtm::LEAVE_CHANNEL_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog("AgoraRtm:  onLeave");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onLeave(handlerId, int(errorCode));
}

/**
 Occurs when receiving a channel message.

 @param userId The message sender.
 @param message The received channel message. See \ref agora::rtm::IMessage
 "IMessage".
 */
void ChannelEventHandler::onMessageReceived(const char* userId,
                                            const agora::rtm::IMessage* message) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onMessageReceived");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onMessageReceived(handlerId, userId, message);
}
/**
 Occurs when receiving a channel image message.

 @param userId The message sender.
 @param message The received channel image message. See \ref
 agora::rtm::IImageMessage "IImageMessage".
 */
void ChannelEventHandler::onImageMessageReceived(
    const char* userId,
    const agora::rtm::IImageMessage* message) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onImageMessageReceived");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onImageMessageReceived(handlerId, userId,
                                                     message);
}

/**
 Occurs when receiving a channel file message.

 @param userId The message sender.
 @param message The received channel file message. See \ref
 agora::rtm::IFileMessage "IFileMessage".
 */
void ChannelEventHandler::onFileMessageReceived(
    const char* userId,
    const agora::rtm::IFileMessage* message) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onFileMessageReceived");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onFileMessageReceived(handlerId, userId, message);
}

/**
 Returns the result of the \ref agora::rtm::IChannel::sendMessage "sendMessage"
 method call.

 @param messageId The ID of the sent channel message.
 @param state The error codes. See #CHANNEL_MESSAGE_ERR_CODE.
 */
void ChannelEventHandler::onSendMessageResult(
    long long messageId,
    agora::rtm::CHANNEL_MESSAGE_ERR_CODE state) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onSendMessageResult");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onSendMessageResult(handlerId, messageId,
                                                  int(state));
}

/**
 Occurs when a remote user joins the channel.

 When a remote user calls the \ref agora::rtm::IChannel::join "join" method and
 receives the \ref agora::rtm::IChannelEventHandler::onJoinSuccess
 "onJoinSuccess" callback (successfully joins the channel), the local user
 receives this callback.

 @note This callback is disabled when the number of the channel members exceeds
 512.

 @param member The user joining the channel. See IChannelMember.
 */
void ChannelEventHandler::onMemberJoined(agora::rtm::IChannelMember* member) {
  agora::unity::rtm::LogHelper::getInstance().writeLog("AgoraRtm:  onMemberJoined");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onMemberJoined(handlerId, member);
}

/**
 Occurs when a remote member leaves the channel.

 When a remote member in the channel calls the \ref agora::rtm::IChannel::leave
 "leave" method and receives the the \ref
 agora::rtm::IChannelEventHandler::onLeave "onLeave (LEAVE_CHANNEL_ERR_OK)"
 callback, the local user receives this callback.

 @note This callback is disabled when the number of the channel members exceeds
 512.

 @param member The channel member that leaves the channel. See IChannelMember.
 */
void ChannelEventHandler::onMemberLeft(agora::rtm::IChannelMember* member) {
  agora::unity::rtm::LogHelper::getInstance().writeLog("AgoraRtm:  onMemberLeft");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onMemberLeft(handlerId, member);
}

/**
 Returns the result of the \ref agora::rtm::IChannel::getMembers "getMembers"
 method call.

 When the method call succeeds, the SDK returns the member list of the channel.

 @param members The member list. See IChannelMember.
 @param userCount The number of members.
 @param errorCode Error code. See #GET_MEMBERS_ERR.
 */
void ChannelEventHandler::onGetMembers(agora::rtm::IChannelMember** members,
                                       int userCount,
                                       agora::rtm::GET_MEMBERS_ERR errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog("AgoraRtm:  onGetMembers");

  if (_c_channel_event_handler) {
    std::string strPostMsg = "";
    for (int i = 0; i < userCount; i++) {
      agora::rtm::IChannelMember* channelMember = members[i];
      if (channelMember && channelMember->getUserId() && channelMember->getChannelId()) {
        strPostMsg.append("\t");
        strPostMsg.append(channelMember->getUserId());
        strPostMsg.append("\t");
        strPostMsg.append(channelMember->getChannelId());
      }
    }
    _c_channel_event_handler->onGetMembers(handlerId, strPostMsg.c_str(), userCount,
                                           errorCode);
  }
}

//             /**
//              Occurs when channel attributes are updated, and returns all
//              attributes of the channel.
//
//              @note This callback is enabled only when the user, who updates
//              the attributes of the channel, sets \ref
//              agora::rtm::ChannelAttributeOptions::enableNotificationToChannelMembers
//              "enableNotificationToChannelMembers" as true. Also note that
//              this flag is valid only within the current channel attribute
//              method call.
//
//              @param attributes All attribute of this channel.
//              @param numberOfAttributes The total number of the channel
//              attributes.
//              */
void ChannelEventHandler::onAttributesUpdated(
    const agora::rtm::IRtmChannelAttribute* attributes[],
    int numberOfAttributes, long long revision) {
  if (_c_channel_event_handler) {
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

    _c_channel_event_handler->onAttributeUpdate(
        handlerId, szMsg.c_str(), numberOfAttributes, revision);
  }
}
//
/**
 Occurs when the number of the channel members changes, and returns the new
 number.

 @note
 - When the number of channel members &le; 512, the SDK returns this callback
 when the number changes and at a MAXIMUM speed of once per second.
 - When the number of channel members exceeds 512, the SDK returns this callback
 when the number changes and at a MAXIMUM speed of once every three seconds.
 - You will receive this callback when successfully joining an RTM channel, so
 Agore recommends implementing this callback to receive timely updates on the
 number of the channel members.

 @param memberCount Member count of this channel.
 */
void ChannelEventHandler::onMemberCountUpdated(int memberCount) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onMemberCountUpdated");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onMemberCountUpdated(handlerId, memberCount);
}

void ChannelEventHandler::onLockAcquired(const char *lockName, long long requestId) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onLockAcquired");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onLockAcquired(handlerId, lockName, requestId);
}

void ChannelEventHandler::onLockExpired(const char *lockName) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onLockExpired");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onLockExpired(handlerId, lockName);
}

void ChannelEventHandler::onLockAcquireFailed(const char *lockName, const char* reason, long long requestId, agora::rtm::CHANNEL_ATTRIBUTE_LOCK_ERR_CODE errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onLockAcquireFailed");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onLockAcquireFailed(handlerId, lockName, reason, requestId, errorCode);
}

void ChannelEventHandler::onLockReleaseResult(const char* lockName, long long requestId, agora::rtm::CHANNEL_ATTRIBUTE_LOCK_ERR_CODE errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onLockReleaseResult");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onLockReleaseResult(handlerId, lockName, requestId, errorCode);
}

void ChannelEventHandler::onLockDisableResult(long long requestId, agora::rtm::CHANNEL_ATTRIBUTE_LOCK_ERR_CODE errorCode) {
  agora::unity::rtm::LogHelper::getInstance().writeLog(
      "AgoraRtm:  onLockDisableResult");
  if (_c_channel_event_handler)
    _c_channel_event_handler->onLockDisableResult(handlerId, requestId, errorCode);
}


}
}  // namespace unity
}  // namespace agora
