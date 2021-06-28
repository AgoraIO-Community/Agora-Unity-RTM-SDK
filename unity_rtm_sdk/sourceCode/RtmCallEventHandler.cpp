//
//  RtmCallEventHandler.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "RtmCallEventHandler.h"

namespace agora {
namespace unity {
RtmCallEventHandler::RtmCallEventHandler(
    int index,
    CRtmCallEventHandler* cRtmCallEventHandler) {
  handlerId = index;
  _c_rtm_call_event_handler = cRtmCallEventHandler;
}

RtmCallEventHandler::~RtmCallEventHandler() {
  _c_rtm_call_event_handler = nullptr;
}

/**
   Callback to the caller: occurs when the callee receives the call invitation.

   @param localInvitation An ILocalCallInvitation object.
   */
void RtmCallEventHandler::onLocalInvitationReceivedByPeer(
    agora::rtm::ILocalCallInvitation* localInvitation) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onLocalInvitationReceivedByPeer(
        handlerId, localInvitation);
}

/**
 Callback to the caller: occurs when the caller cancels a call invitation.

 @param localInvitation An ILocalCallInvitation object.
 */
void RtmCallEventHandler::onLocalInvitationCanceled(
    agora::rtm::ILocalCallInvitation* localInvitation) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onLocalInvitationCanceled(handlerId,
                                                          localInvitation);
}

/**
 Callback to the caller: occurs when the life cycle of the outgoing call
 invitation ends in failure.

 @param localInvitation An ILocalCallInvitation object.
 @param errorCode The error code. See #LOCAL_INVITATION_ERR_CODE.
 */
void RtmCallEventHandler::onLocalInvitationFailure(
    agora::rtm::ILocalCallInvitation* localInvitation,
    agora::rtm::LOCAL_INVITATION_ERR_CODE errorCode) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onLocalInvitationFailure(
        handlerId, localInvitation, errorCode);
}

/**
 Callback to the caller: occurs when the callee accepts the call invitation.

 @param localInvitation An ILocalCallInvitation object.
 @param response The callee's response to the call invitation.
 */
void RtmCallEventHandler::onLocalInvitationAccepted(
    agora::rtm::ILocalCallInvitation* localInvitation,
    const char* response) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onLocalInvitationAccepted(
        handlerId, localInvitation, response);
}

/**
 Callback to the caller: occurs when the callee refuses the call invitation.

 @param localInvitation An ILocalCallInvitation object.
 @param response The callee's response to the call invitation.
 */
void RtmCallEventHandler::onLocalInvitationRefused(
    agora::rtm::ILocalCallInvitation* localInvitation,
    const char* response) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onLocalInvitationRefused(
        handlerId, localInvitation, response);
}

/**
 Callback for the callee: occurs when the callee refuses a call invitation.

 @param remoteInvitation An IRemoteCallInvitation object.
 */
void RtmCallEventHandler::onRemoteInvitationRefused(
    agora::rtm::IRemoteCallInvitation* remoteInvitation) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onRemoteInvitationRefused(handlerId,
                                                          remoteInvitation);
}

/**
 Callback to the callee: occurs when the callee accepts a call invitation.

 @param remoteInvitation An IRemoteCallInvitation object.
 */
void RtmCallEventHandler::onRemoteInvitationAccepted(
    agora::rtm::IRemoteCallInvitation* remoteInvitation) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onRemoteInvitationAccepted(handlerId,
                                                           remoteInvitation);
}

/**
 Callback to the callee: occurs when the callee receives a call invitation.

 @param remoteInvitation An IRemoteCallInvitation object.
 */
void RtmCallEventHandler::onRemoteInvitationReceived(
    agora::rtm::IRemoteCallInvitation* remoteInvitation) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onRemoteInvitationReceived(handlerId,
                                                           remoteInvitation);
}

/**
 Callback to the callee: occurs when the life cycle of the incoming call
 invitation ends in failure.

 @param remoteInvitation An IRemoteCallInvitation object.
 @param errorCode The error code. See #REMOTE_INVITATION_ERR_CODE.
 */
void RtmCallEventHandler::onRemoteInvitationFailure(
    agora::rtm::IRemoteCallInvitation* remoteInvitation,
    agora::rtm::REMOTE_INVITATION_ERR_CODE errorCode) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onRemoteInvitationFailure(
        handlerId, remoteInvitation, errorCode);
}

/**
 Callback to the callee: occurs when the caller cancels the call invitation.

 @param remoteInvitation An IRemoteCallInvitation object.
 */
void RtmCallEventHandler::onRemoteInvitationCanceled(
    agora::rtm::IRemoteCallInvitation* remoteInvitation) {
  if (_c_rtm_call_event_handler)
    _c_rtm_call_event_handler->_onRemoteInvitationCanceled(handlerId,
                                                           remoteInvitation);
}
}  // namespace unity
}  // namespace agora
