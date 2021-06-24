//
//  i_local_call_invitation.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once
#include "common.h"

AGORA_API const char* i_local_call_invitation_getCalleeId(
    void* localCallInvitationPtr);

AGORA_API void i_local_call_invitation_setContent(void* localCallInvitationPtr,
                                                  const char* content);

AGORA_API const char* i_local_call_invitation_getContent(
    void* localCallInvitationPtr);

AGORA_API void i_local_call_invitation_setChannelId(
    void* localCallInvitationPtr,
    const char* channelId);

AGORA_API const char* i_local_call_invitation_getChannelId(
    void* localCallInvitationPtr);

AGORA_API const char* i_local_call_invitation_getResponse(
    void* localCallInvitationPtr);

AGORA_API int i_local_call_invitation_getState(void* localCallInvitationPtr);

AGORA_API void i_local_call_invitation_release(void* localCallInvitationPtr);
