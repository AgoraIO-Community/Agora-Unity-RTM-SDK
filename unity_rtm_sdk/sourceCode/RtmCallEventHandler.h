//
//  RtmCallEventHandler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once

#include "common.h"

namespace agora {
namespace unity {

#if defined(_WIN32)
typedef void(__stdcall* FUNC_onLocalInvitationReceivedByPeer)(
    int index,
    void* localInvitation);
typedef void(__stdcall* FUNC_onLocalInvitationCanceled)(int index,
                                                        void* localInvitation);
typedef void(__stdcall* FUNC_onLocalInvitationFailure)(
    int index,
    void* localInvitation,
    rtm::LOCAL_INVITATION_ERR_CODE errorCode);
typedef void(__stdcall* FUNC_onLocalInvitationAccepted)(int index,
                                                        void* localInvitation,
                                                        const char* response);
typedef void(__stdcall* FUNC_onLocalInvitationRefused)(int index,
                                                       void* localInvitation,
                                                       const char* response);
typedef void(__stdcall* FUNC_onRemoteInvitationRefused)(int index,
                                                        void* remoteInvitation);
typedef void(__stdcall* FUNC_onRemoteInvitationAccepted)(
    int index,
    void* remoteInvitation);
typedef void(__stdcall* FUNC_onRemoteInvitationReceived)(
    int index,
    void* remoteInvitation);
typedef void(__stdcall* FUNC_onRemoteInvitationFailure)(
    int index,
    void* remoteInvitation,
    rtm::REMOTE_INVITATION_ERR_CODE errorCode);
typedef void(__stdcall* FUNC_onRemoteInvitationCanceled)(
    int index,
    void* remoteInvitation);
#else
typedef void (*FUNC_onLocalInvitationReceivedByPeer)(int index,
                                                     void* localInvitation);
typedef void (*FUNC_onLocalInvitationCanceled)(int index,
                                               void* localInvitation);
typedef void (*FUNC_onLocalInvitationFailure)(
    int index,
    void* localInvitation,
    rtm::LOCAL_INVITATION_ERR_CODE errorCode);
typedef void (*FUNC_onLocalInvitationAccepted)(int index,
                                               void* localInvitation,
                                               const char* response);
typedef void (*FUNC_onLocalInvitationRefused)(int index,
                                              void* localInvitation,
                                              const char* response);
typedef void (*FUNC_onRemoteInvitationRefused)(int index,
                                               void* remoteInvitation);
typedef void (*FUNC_onRemoteInvitationAccepted)(int index,
                                                void* remoteInvitation);
typedef void (*FUNC_onRemoteInvitationReceived)(int index,
                                                void* remoteInvitation);
typedef void (*FUNC_onRemoteInvitationFailure)(
    int index,
    void* remoteInvitation,
    rtm::REMOTE_INVITATION_ERR_CODE errorCode);
typedef void (*FUNC_onRemoteInvitationCanceled)(int index,
                                                void* remoteInvitation);
#endif

class RtmCallEventHandler : public agora::rtm::IRtmCallEventHandler {
 private:
  int handlerId;
  FUNC_onLocalInvitationReceivedByPeer _onLocalInvitationReceivedByPeer =
      nullptr;
  FUNC_onLocalInvitationCanceled _onLocalInvitationCanceled = nullptr;
  FUNC_onLocalInvitationFailure _onLocalInvitationFailure = nullptr;
  FUNC_onLocalInvitationAccepted _onLocalInvitationAccepted = nullptr;
  FUNC_onLocalInvitationRefused _onLocalInvitationRefused = nullptr;

  FUNC_onRemoteInvitationRefused _onRemoteInvitationRefused = nullptr;
  FUNC_onRemoteInvitationAccepted _onRemoteInvitationAccepted = nullptr;
  FUNC_onRemoteInvitationReceived _onRemoteInvitationReceived = nullptr;
  FUNC_onRemoteInvitationFailure _onRemoteInvitationFailure = nullptr;
  FUNC_onRemoteInvitationCanceled _onRemoteInvitationCanceled = nullptr;

 public:
  RtmCallEventHandler(
      int index,
      FUNC_onLocalInvitationReceivedByPeer _onLocalInvitationReceivedByPeer,
      FUNC_onLocalInvitationCanceled _onLocalInvitationCanceled,
      FUNC_onLocalInvitationFailure _onLocalInvitationFailure,
      FUNC_onLocalInvitationAccepted _onLocalInvitationAccepted,
      FUNC_onLocalInvitationRefused _onLocalInvitationRefused,
      FUNC_onRemoteInvitationRefused _onRemoteInvitationRefused,
      FUNC_onRemoteInvitationAccepted _onRemoteInvitationAccepted,
      FUNC_onRemoteInvitationReceived _onRemoteInvitationReceived,
      FUNC_onRemoteInvitationFailure _onRemoteInvitationFailure,
      FUNC_onRemoteInvitationCanceled _onRemoteInvitationCanceled);

  ~RtmCallEventHandler();

  /**
     Callback to the caller: occurs when the callee receives the call
     invitation.

     @param localInvitation An ILocalCallInvitation object.
     */
  virtual void onLocalInvitationReceivedByPeer(
      agora::rtm::ILocalCallInvitation* localInvitation) override;

  /**
   Callback to the caller: occurs when the caller cancels a call invitation.

   @param localInvitation An ILocalCallInvitation object.
   */
  virtual void onLocalInvitationCanceled(
      agora::rtm::ILocalCallInvitation* localInvitation) override;

  /**
   Callback to the caller: occurs when the life cycle of the outgoing call
   invitation ends in failure.

   @param localInvitation An ILocalCallInvitation object.
   @param errorCode The error code. See #LOCAL_INVITATION_ERR_CODE.
   */
  virtual void onLocalInvitationFailure(
      agora::rtm::ILocalCallInvitation* localInvitation,
      agora::rtm::LOCAL_INVITATION_ERR_CODE errorCode) override;

  /**
   Callback to the caller: occurs when the callee accepts the call invitation.

   @param localInvitation An ILocalCallInvitation object.
   @param response The callee's response to the call invitation.
   */
  virtual void onLocalInvitationAccepted(
      agora::rtm::ILocalCallInvitation* localInvitation,
      const char* response) override;

  /**
   Callback to the caller: occurs when the callee refuses the call invitation.

   @param localInvitation An ILocalCallInvitation object.
   @param response The callee's response to the call invitation.
   */
  virtual void onLocalInvitationRefused(
      agora::rtm::ILocalCallInvitation* localInvitation,
      const char* response) override;

  /**
   Callback for the callee: occurs when the callee refuses a call invitation.

   @param remoteInvitation An IRemoteCallInvitation object.
   */
  virtual void onRemoteInvitationRefused(
      agora::rtm::IRemoteCallInvitation* remoteInvitation) override;

  /**
   Callback to the callee: occurs when the callee accepts a call invitation.

   @param remoteInvitation An IRemoteCallInvitation object.
   */
  virtual void onRemoteInvitationAccepted(
      agora::rtm::IRemoteCallInvitation* remoteInvitation) override;

  /**
   Callback to the callee: occurs when the callee receives a call invitation.

   @param remoteInvitation An IRemoteCallInvitation object.
   */
  virtual void onRemoteInvitationReceived(
      agora::rtm::IRemoteCallInvitation* remoteInvitation) override;

  /**
   Callback to the callee: occurs when the life cycle of the incoming call
   invitation ends in failure.

   @param remoteInvitation An IRemoteCallInvitation object.
   @param errorCode The error code. See #REMOTE_INVITATION_ERR_CODE.
   */
  virtual void onRemoteInvitationFailure(
      agora::rtm::IRemoteCallInvitation* remoteInvitation,
      agora::rtm::REMOTE_INVITATION_ERR_CODE errorCode) override;

  /**
   Callback to the callee: occurs when the caller cancels the call invitation.

   @param remoteInvitation An IRemoteCallInvitation object.
   */
  virtual void onRemoteInvitationCanceled(
      agora::rtm::IRemoteCallInvitation* remoteInvitation) override;
};
}  // namespace unity
}  // namespace agora
