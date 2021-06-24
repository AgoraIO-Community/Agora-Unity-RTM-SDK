//
//  i_rtm_call_manager.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_call_manager.h"

extern "C" {

#define RTM_CALL_MANAGER_INSTANCE \
  static_cast<agora::rtm::IRtmCallManager*>(callManagerInstance)

#define I_LOCAL_CALL_INVITATION_PTR \
  static_cast<agora::rtm::ILocalCallInvitation*>(invitation)

#define I_REMOTE_CALL_INVITATION_PTR \
  static_cast<agora::rtm::IRemoteCallInvitation*>(invitation)
}

AGORA_API int rtm_call_manager_sendLocalInvitation(void* callManagerInstance,
                                                   void* invitation) {
  return RTM_CALL_MANAGER_INSTANCE->sendLocalInvitation(
      I_LOCAL_CALL_INVITATION_PTR);
}

AGORA_API int rtm_call_manager_acceptRemoteInvitation(void* callManagerInstance,
                                                      void* invitation) {
  return RTM_CALL_MANAGER_INSTANCE->acceptRemoteInvitation(
      I_REMOTE_CALL_INVITATION_PTR);
}

AGORA_API int rtm_call_manager_refuseRemoteInvitation(void* callManagerInstance,
                                                      void* invitation) {
  return RTM_CALL_MANAGER_INSTANCE->refuseRemoteInvitation(
      I_REMOTE_CALL_INVITATION_PTR);
}

AGORA_API int rtm_call_manager_cancelLocalInvitation(void* callManagerInstance,
                                                     void* invitation) {
  return RTM_CALL_MANAGER_INSTANCE->cancelLocalInvitation(
      I_LOCAL_CALL_INVITATION_PTR);
}

AGORA_API void* rtm_call_manager_createLocalCallInvitation(
    void* callManagerInstance,
    const char* calleeId) {
  return RTM_CALL_MANAGER_INSTANCE->createLocalCallInvitation(calleeId);
}

AGORA_API void rtm_call_manager_release(void* callManagerInstance) {
  return RTM_CALL_MANAGER_INSTANCE->release();
}
