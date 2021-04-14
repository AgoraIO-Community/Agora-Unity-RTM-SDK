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
#if defined(_WIN32)
#define AGORARTC_EXPORT
#pragma comment(lib, "../sdk/lib/agora_rtm_sdk.lib")
#include "../../../include/IAgoraRtmCallManager.h"
#include "../../../include/IAgoraRtmService.h"
#elif defined(__APPLE__)
#include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmCallManager.h"
#include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmService.h"
#elif defined(__ANDROID__) || defined(__linux__)
#include "../prebuilt/include/IAgoraRtmCallManager.h"
#include "../prebuilt/include/IAgoraRtmService.h"
#endif

