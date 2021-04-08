//
//  Agora Rtm SDK
//
//  Copyright (c) 2018 Agora IO. All rights reserved.
//

#pragma once

#include <cinttypes>
#if defined(_WIN32)
#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#include <cstdint>
#define AGORA_CALL __cdecl
#if defined(AGORARTC_EXPORT)
#define AGORA_API extern "C" __declspec(dllexport)
#else
#define AGORA_API extern "C" __declspec(dllimport)
#endif
#define _AGORA_CPP_API

#elif defined(__APPLE__)
#define AGORA_API __attribute__((visibility("default"))) extern "C"
#define AGORA_CALL
#define _AGORA_CPP_API

#elif defined(__ANDROID__) || defined(__linux__)
#if defined(__ANDROID__) && defined(FEATURE_RTM_STANDALONE_SDK)
#define AGORA_API extern "C"
#define _AGORA_CPP_API
#else
#define AGORA_API extern "C" __attribute__((visibility("default")))
#define _AGORA_CPP_API __attribute__((visibility("default")))
#endif
#define AGORA_CALL

#else
#define AGORA_API extern "C"
#define AGORA_CALL
#define _AGORA_CPP_API
#endif
namespace agora {
namespace rtm {
class IRtmService;
class IMessage;
namespace internal {
AGORA_API IMessage* AGORA_CALL
createMessage(IRtmService* service, int message_type, const uint8_t* raw_data,
              int length, const char* description, const char* attribtues);

// Test whether the error code indicates that a feature is enabled.
// The error code is supposed to be converted from enum PEER_MESSAGE_ERR_CODE.
AGORA_API bool AGORA_CALL
testFeatureEnabled(int peer_message_error_code);

// Whether enable same thread check, if enabled and some object is used in wrong
// thread, sdk will abort immediately
AGORA_API void AGORA_CALL
enableSameThreadCheck(bool enable);
// Set app type,this API must be called before all rtm instance created and only
// once, otherwise function will return false, app type default is 0
AGORA_API bool AGORA_CALL setRtmAppType(int32_t app_type);
// Set client type,this API must be called before all rtm instance created and only
// once, otherwise function will return false,client type default is 39
AGORA_API bool AGORA_CALL setRtmClientType(int32_t client_type);
}  // namespace internal
}  // namespace rtm
}  // namespace agora
