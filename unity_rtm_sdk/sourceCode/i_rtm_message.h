//
//  i_rtm_message.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once
#include "common.h"

AGORA_API long long imessage_getMessageId(void* file_message_instance);

/**
 Retrieves the message type.

 @return The message type. See #MESSAGE_TYPE.
 */
AGORA_API int imessage_getMessageType(void* file_message_instance);

/**
 Sets the content of a text message, or the text description of a raw message.

 @param str The text message to be set. Must not exceed 32 KB in length. If the
 message is a raw message, ensure that the overall size of the text description
 and the raw message data does not exceed 32 KB.
 */
AGORA_API void imessage_setText(void* file_message_instance, const char* str);

/**
 Retrieves the content of a text message, or the text description of a raw
 message.

 @return The content of the received text message, or the text description of
 the received raw message.
 */
AGORA_API const char* imessage_getText(void* file_message_instance);

/**
 Retrieves the starting address of the raw message in the memory.

 @return The starting address of the raw message in the memory.
 */
AGORA_API const char* imessage_getRawMessageData(void* file_message_instance);

/**
 Retrieves the length of the raw message.

 @return The length of the raw message in Bytes.
 */
AGORA_API int imessage_getRawMessageLength(void* file_message_instance);
/**
 Allows the receiver to retrieve the timestamp of when the messaging server
 receives this message.

 @note
 - You can infer from the returned timestamp the *approximate* time as to when
 this message was sent.
 - The returned timestamp is on a millisecond time-scale. It is for
 demonstration purposes only, not for strict ordering of messages.


 @return The timestamp (ms) of when the messaging server receives this message.
 */
AGORA_API long long imessage_getServerReceivedTs(void* file_message_instance);

/**
 Allows the receiver to check whether this message has been cached on the server
 (Applies to peer-to-peer message only).

 @note
 - This method returns false if a message is not cached by the server. Only if
 the sender sends the message as an offline message (sets \ref
 agora::rtm::SendMessageOptions::enableOfflineMessaging "enableOfflineMessaging"
 as true) when the specified user is offline, does the method return true when
 the user is back online.
 - For now we only cache 200 offline messages for up to seven days for each
 message receiver. When the number of the cached messages reaches this limit,
 the newest message overrides the oldest one.

 @return
 - true: This message has been cached on the server (the server caches this
 message and re-sends it to the receiver when he/she is back online).
 - false: This message has not been cached on the server.
 */
AGORA_API bool imessage_isOfflineMessage(void* file_message_instance);

/**
 Releases all resources used by the \ref agora::rtm::IMessage "IMessage"
 instance.

 @note For the message receiver: please access and save the content of the
 IMessage instance when receiving the \ref
 agora::rtm::IChannelEventHandler::onMessageReceived "onMessageReceived" or the
 \ref agora::rtm::IRtmServiceEventHandler::onMessageReceivedFromPeer
 "onMessageReceivedFromPeer" callback. The SDK will release the IMessage
 instance when the callback ends.
 */
AGORA_API void imessage_release(void* file_message_instance);
