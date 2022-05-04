//
//  i_rtm_channel_attribute.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_channel_attribute.h"

extern "C" {
#define CHANNEL_ATTRIBUTE_INSTANCE \
  static_cast<agora::rtm::IRtmChannelAttribute*>(channel_attribute_instance)
}

AGORA_API void channelAttribute_setKey(void* channel_attribute_instance,
                                       const char* key) {
  CHANNEL_ATTRIBUTE_INSTANCE->setKey(key);
}

/**
 Gets the key of the channel attribute.

 @return Key of the channel attribute.
 */
AGORA_API const char* channelAttribute_getKey(
    void* channel_attribute_instance) {
  return CHANNEL_ATTRIBUTE_INSTANCE->getKey();
}

/**
 Sets the value of the channel attribute.

 @param value Value of the channel attribute. Must not exceed 8 KB in length.
 */
AGORA_API void channelAttribute_setValue(void* channel_attribute_instance,
                                         const char* value) {
  return CHANNEL_ATTRIBUTE_INSTANCE->setValue(value);
}

/**
 Gets the value of the channel attribute.

 @return Value of the channel attribute.
 */
AGORA_API const char* channelAttribute_getValue(
    void* channel_attribute_instance) {
  return CHANNEL_ATTRIBUTE_INSTANCE->getValue();
}

/**
 Gets the User ID of the user who makes the latest update to the channel
 attribute.

 @return User ID of the user who makes the latest update to the channel
 attribute.
 */
AGORA_API const char* channelAttribute_getLastUpdateUserId(
    void* channel_attribute_instance) {
  return CHANNEL_ATTRIBUTE_INSTANCE->getLastUpdateUserId();
}

/**
 Gets the timestamp of when the channel attribute was last updated.

 @return Timestamp of when the channel attribute was last updated in
 milliseconds.
 */
AGORA_API long long channelAttribute_getLastUpdateTs(
    void* channel_attribute_instance) {
  return CHANNEL_ATTRIBUTE_INSTANCE->getLastUpdateTs();
}

/*get attribute revision
*/
AGORA_API long long channelAttribute_getRevision(void* channel_attribute_instance) {
  return CHANNEL_ATTRIBUTE_INSTANCE->getRevision();
}

/*set attribute based on revision
*/
AGORA_API void channelAttribute_setRevision(void* channel_attribute_instance, long long revision) {
  return CHANNEL_ATTRIBUTE_INSTANCE->setRevision(revision);
}

/**
 Set the lock of the channel attribute.
*/
AGORA_API void channelAttribute_setLockName(void* channel_attribute_instance, const char *lockName) {
  return CHANNEL_ATTRIBUTE_INSTANCE->setLockName(lockName);
}

/**
 Get the lock of the channel attribute.
*/
AGORA_API const char* channelAttribute_getLockName(void* channel_attribute_instance) {
  return CHANNEL_ATTRIBUTE_INSTANCE->getLockName();
}

/**
 Release all resources used by the \ref agora::rtm::IRtmChannelAttribute
 "IRtmChannelAttribute" instance.
 */
AGORA_API void channelAttribute_release(void* channel_attribute_instance) {
  return CHANNEL_ATTRIBUTE_INSTANCE->release();
}
