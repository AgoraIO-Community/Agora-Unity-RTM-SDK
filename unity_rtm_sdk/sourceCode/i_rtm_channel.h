//
//  i_rtm_channel.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once

#include "common.h"

AGORA_API int channel_join(void* channelInstance);

AGORA_API int channel_leave(void* channelInstance);

AGORA_API int channel_sendMessage(void* channelInstance, void* message);

AGORA_API int channel_sendMessage2(void* channelInstance,
                                   void* message,
                                   bool enableOfflineMessaging,
                                   bool enableHistoricalMessaging);

AGORA_API const char* channel_getId(void* channelInstance);

AGORA_API int channel_getMembers(void* channelInstance);

AGORA_API int channel_acquireLock(void* channelInstance, const char *lock, bool blocking, long long ttl, long long &requestId);

AGORA_API int channel_disableLock(void* channelInstance, const char *lock, const char *userId, long long &requestId);

AGORA_API int channel_releaseLock(void* channelInstance, const char *lock, long long &requestId);

AGORA_API void channel_release(void* channelInstance);
