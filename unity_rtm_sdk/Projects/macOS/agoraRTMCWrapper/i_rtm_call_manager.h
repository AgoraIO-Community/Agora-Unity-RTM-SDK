//
//  i_rtm_call_manager.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once
#include "common.h"

AGORA_API int rtm_call_manager_sendLocalInvitation(void *callManagerInstance, void *invitation);

AGORA_API int rtm_call_manager_acceptRemoteInvitation(void *callManagerInstance, void *invitation);

AGORA_API int rtm_call_manager_refuseRemoteInvitation(void *callManagerInstance, void *invitation);

AGORA_API int rtm_call_manager_cancelLocalInvitation(void *callManagerInstance, void *invitation);

AGORA_API void* rtm_call_manager_createLocalCallInvitation(void *callManagerInstance, const char *calleeId);

AGORA_API void rtm_call_manager_release(void *callManagerInstance);
