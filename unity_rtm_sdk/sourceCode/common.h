//
//  common.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/11.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once
#include <stdio.h>

#include "rtm_private_api.h"
#if defined(_WIN64)
    #define AGORARTC_EXPORT
    #pragma comment(lib, "../sdk/x64/lib/agora_rtm_sdk.lib")
    #include "../sdk/x64/include/IAgoraRtmCallManager.h"
    #include "../sdk/x64/include/IAgoraRtmService.h"
    #define AGORA_CALL __cedc
#elif defined(_WIN32)
    #define AGORARTC_EXPORT
    #pragma comment(lib, "../sdk/x86/lib/agora_rtm_sdk.lib")
    #include "../sdk/x86/include/IAgoraRtmCallManager.h"
    #include "../sdk/x86/include/IAgoraRtmService.h"
    #define AGORA_CALL __cedc
#elif defined(__APPLE__)
    #include <TargetConditionals.h>
    #if TARGET_IPHONE_SIMULATOR
        #include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmCallManager.h"
        #include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmService.h"
    #elif TARGET_OS_IPHONE
        #include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmCallManager.h"
        #include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmService.h"
    #elif TARGET_OS_MAC
        #include "../Agora_RTM_SDK_for_Mac/libs/AgoraRtmKit.framework/Headers/IAgoraRtmCallManager.h"
        #include "../Agora_RTM_SDK_for_Mac/libs/AgoraRtmKit.framework/Headers/IAgoraRtmService.h"
    #endif
    #define AGORA_CALL
#elif defined(__ANDROID__) || defined(__linux__)
    #include "../prebuilt/include/IAgoraRtmCallManager.h"
    #include "../prebuilt/include/IAgoraRtmService.h"
    #define AGORA_CALL
#endif

typedef void(AGORA_CALL *FUNC_channel_onJoinSuccess)(int _id);
typedef void(AGORA_CALL *FUNC_channel_onJoinFailure)(int _id, int errorCode);
typedef void(AGORA_CALL *FUNC_channel_onLeave)(int _id, int errorCode);
typedef void(AGORA_CALL *FUNC_channel_onMessageReceived)(int _id, const char *userId, const agora::rtm::IMessage *message);
typedef void(AGORA_CALL *FUNC_channel_onImageMessageReceived)(int _id, const char *userId, const agora::rtm::IMessage *message);
typedef void(AGORA_CALL *FUNC_channel_onFileMessageReceived)(int _id, const char *userId, const agora::rtm::IMessage *message);
typedef void(AGORA_CALL *FUNC_channel_onSendMessageResult)(int _id, long long messageId, int state);
typedef void(AGORA_CALL *FUNC_channel_onMemberJoined)(int _id, void *member);
typedef void(AGORA_CALL *FUNC_channel_onMemberLeft)(int _id, void *member);
typedef void(AGORA_CALL *FUNC_channel_onGetMembers)(int _id, const char *members, int userCount, int errorCode);
typedef void(AGORA_CALL *FUNC_channel_onMemberCountUpdated)(int _id, int memberCount);
typedef void(AGORA_CALL *FUNC_channel_onAttributeUpdate)(int _id, const char *attributes, int numberOfAttributes);


typedef struct CChannelEventHandler {
    FUNC_channel_onJoinSuccess onJoinSuccess;
    FUNC_channel_onJoinFailure onJoinFailure;
    FUNC_channel_onLeave onLeave;
    FUNC_channel_onMessageReceived onMessageReceived;
    FUNC_channel_onImageMessageReceived onImageMessageReceived;
    FUNC_channel_onFileMessageReceived onFileMessageReceived;
    FUNC_channel_onSendMessageResult onSendMessageResult;
    FUNC_channel_onMemberJoined onMemberJoined;
    FUNC_channel_onMemberLeft onMemberLeft;
    FUNC_channel_onGetMembers onGetMembers;
    FUNC_channel_onMemberCountUpdated onMemberCountUpdated;
    FUNC_channel_onAttributeUpdate onAttributeUpdate;
} CChannelEventHandler;