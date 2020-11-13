//
//  i_remote_call_invitation.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once

#include "common.h"



AGORA_API const char *i_remote_call_manager_getCallerId(void *remoteCallInvitationPtr);

AGORA_API const char *i_remote_call_manager_getContent(void *remoteCallInvitationPtr);

AGORA_API void i_remote_call_manager_setResponse(void *remoteCallInvitationPtr, const char *response);

AGORA_API const char *i_remote_call_manager_getResponse(void *remoteCallInvitationPtr);

AGORA_API const char *i_remote_call_manager_getChannelId(void *remoteCallInvitationPtr);

AGORA_API int i_remote_call_manager_getState(void *remoteCallInvitationPtr);

AGORA_API void i_remote_call_manager_release(void *remoteCallInvitationPtr);
