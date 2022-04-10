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
namespace rtm {
class RtmCallEventHandler : public agora::rtm::IRtmCallEventHandler {
 private:
  int handlerId;
  CRtmCallEventHandler* _c_rtm_call_event_handler;

 public:
  RtmCallEventHandler(int index,
                      CRtmCallEventHandler* cRtmCallEventHandler);

  virtual ~RtmCallEventHandler();

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
}
}  // namespace unity
}  // namespace agora
