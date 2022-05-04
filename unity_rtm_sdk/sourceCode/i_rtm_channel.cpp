//
//  i_rtm_channel.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_channel.h"

extern "C" {
#define CHANNEL_INSTANCE static_cast<agora::rtm::IChannel*>(channelInstance)
#define IMESSAGE_INSTANCE static_cast<agora::rtm::IMessage*>(message)
}

AGORA_API int channel_join(void* channelInstance) {
  return CHANNEL_INSTANCE->join();
}

AGORA_API int channel_leave(void* channelInstance) {
  return CHANNEL_INSTANCE->leave();
}

AGORA_API int channel_sendMessage(void* channelInstance, void* message) {
  return CHANNEL_INSTANCE->sendMessage(IMESSAGE_INSTANCE);
}

AGORA_API int channel_sendMessage2(void* channelInstance,
                                   void* message,
                                   bool enableOfflineMessaging,
                                   bool enableHistoricalMessaging) {
  agora::rtm::SendMessageOptions option;
  option.enableHistoricalMessaging = enableHistoricalMessaging;
  option.enableOfflineMessaging = enableOfflineMessaging;
  return CHANNEL_INSTANCE->sendMessage(IMESSAGE_INSTANCE, option);
}

AGORA_API const char* channel_getId(void* channelInstance) {
  return CHANNEL_INSTANCE->getId();
}

AGORA_API int channel_getMembers(void* channelInstance) {
  return CHANNEL_INSTANCE->getMembers();
}

AGORA_API int channel_acquireLock(void* channelInstance, const char *lock, bool blocking, long long &requestId) {
  return CHANNEL_INSTANCE->acquireLock(lock, blocking, requestId);
}

AGORA_API int channel_releaseLock(void* channelInstance, const char *lock, long long &requestId) {
  return CHANNEL_INSTANCE->releaseLock(lock, requestId);
}

AGORA_API void channel_release(void* channelInstance) {
  return CHANNEL_INSTANCE->release();
}
