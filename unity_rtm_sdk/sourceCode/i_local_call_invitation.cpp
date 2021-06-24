//
//  i_local_call_invitation.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_local_call_invitation.h"

extern "C" {
#define I_LOCAL_CALL_INVITATION_INSTANCE \
  static_cast<agora::rtm::ILocalCallInvitation*>(localCallInvitationPtr)
}

AGORA_API const char* i_local_call_invitation_getCalleeId(
    void* localCallInvitationPtr) {
  return I_LOCAL_CALL_INVITATION_INSTANCE->getCalleeId();
}

AGORA_API void i_local_call_invitation_setContent(void* localCallInvitationPtr,
                                                  const char* content) {
  return I_LOCAL_CALL_INVITATION_INSTANCE->setContent(content);
}

AGORA_API const char* i_local_call_invitation_getContent(
    void* localCallInvitationPtr) {
  return I_LOCAL_CALL_INVITATION_INSTANCE->getContent();
}

AGORA_API void i_local_call_invitation_setChannelId(
    void* localCallInvitationPtr,
    const char* channelId) {
  I_LOCAL_CALL_INVITATION_INSTANCE->setChannelId(channelId);
}

AGORA_API const char* i_local_call_invitation_getChannelId(
    void* localCallInvitationPtr) {
  return I_LOCAL_CALL_INVITATION_INSTANCE->getChannelId();
}

AGORA_API const char* i_local_call_invitation_getResponse(
    void* localCallInvitationPtr) {
  return I_LOCAL_CALL_INVITATION_INSTANCE->getResponse();
}

AGORA_API int i_local_call_invitation_getState(void* localCallInvitationPtr) {
  return I_LOCAL_CALL_INVITATION_INSTANCE->getState();
}

AGORA_API void i_local_call_invitation_release(void* localCallInvitationPtr) {
  return I_LOCAL_CALL_INVITATION_INSTANCE->release();
}
