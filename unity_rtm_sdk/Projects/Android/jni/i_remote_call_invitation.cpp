//
//  i_remote_call_invitation.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_remote_call_invitation.h"


extern "C"
{
#define I_REMOTE_CALL_INVITATION_INSTANCE static_cast<agora::rtm::IRemoteCallInvitation *>(remoteCallInvitationPtr)
}


AGORA_API const char *i_remote_call_manager_getCallerId(void *remoteCallInvitationPtr)
{
    return I_REMOTE_CALL_INVITATION_INSTANCE->getCallerId();
}

AGORA_API const char *i_remote_call_manager_getContent(void *remoteCallInvitationPtr)
{
    return I_REMOTE_CALL_INVITATION_INSTANCE->getContent();
}

AGORA_API void i_remote_call_manager_setResponse(void *remoteCallInvitationPtr, const char *response)
{
    return I_REMOTE_CALL_INVITATION_INSTANCE->setResponse(response);
}

AGORA_API const char *i_remote_call_manager_getResponse(void *remoteCallInvitationPtr)
{
    return I_REMOTE_CALL_INVITATION_INSTANCE->getResponse();
}

AGORA_API const char *i_remote_call_manager_getChannelId(void *remoteCallInvitationPtr)
{
    return I_REMOTE_CALL_INVITATION_INSTANCE->getChannelId();
}

AGORA_API int i_remote_call_manager_getState(void *remoteCallInvitationPtr)
{
    return I_REMOTE_CALL_INVITATION_INSTANCE->getState();
}

AGORA_API void i_remote_call_manager_release(void *remoteCallInvitationPtr)
{
    return I_REMOTE_CALL_INVITATION_INSTANCE->release();
}
