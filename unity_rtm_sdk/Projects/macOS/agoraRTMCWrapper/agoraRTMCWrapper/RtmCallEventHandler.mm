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
       RtmCallEventHandler::RtmCallEventHandler(int index, FUNC_onLocalInvitationReceivedByPeer onLocalInvitationReceivedByPeer,
            FUNC_onLocalInvitationCanceled onLocalInvitationCanceled,
            FUNC_onLocalInvitationFailure onLocalInvitationFailure,
            FUNC_onLocalInvitationAccepted onLocalInvitationAccepted,
            FUNC_onLocalInvitationRefused onLocalInvitationRefused,
            FUNC_onRemoteInvitationRefused onRemoteInvitationRefused,
            FUNC_onRemoteInvitationAccepted onRemoteInvitationAccepted,
            FUNC_onRemoteInvitationReceived onRemoteInvitationReceived,
            FUNC_onRemoteInvitationFailure onRemoteInvitationFailure,
                                                FUNC_onRemoteInvitationCanceled onRemoteInvitationCanceled) {
           handlerId = index;
           _onLocalInvitationReceivedByPeer = onLocalInvitationReceivedByPeer;
           _onLocalInvitationCanceled = onLocalInvitationCanceled;
           _onLocalInvitationFailure = onLocalInvitationFailure;
           _onLocalInvitationAccepted = onLocalInvitationAccepted;
           _onLocalInvitationRefused = onLocalInvitationRefused;
           _onRemoteInvitationRefused = onRemoteInvitationRefused;
           _onRemoteInvitationAccepted = onRemoteInvitationAccepted;
           _onRemoteInvitationReceived = onRemoteInvitationReceived;
           _onRemoteInvitationFailure = onRemoteInvitationFailure;
           _onRemoteInvitationCanceled = onRemoteInvitationCanceled;
       }
        
        RtmCallEventHandler::~RtmCallEventHandler() {
            _onLocalInvitationReceivedByPeer = nullptr;
            _onLocalInvitationCanceled = nullptr;
            _onLocalInvitationFailure = nullptr;
            _onLocalInvitationAccepted = nullptr;
            _onLocalInvitationRefused = nullptr;
            _onRemoteInvitationRefused = nullptr;
            _onRemoteInvitationAccepted = nullptr;
            _onRemoteInvitationReceived = nullptr;
            _onRemoteInvitationFailure = nullptr;
            _onRemoteInvitationCanceled = nullptr;
        }

    /**
       Callback to the caller: occurs when the callee receives the call invitation.

       @param localInvitation An ILocalCallInvitation object.
       */
        void RtmCallEventHandler::onLocalInvitationReceivedByPeer(agora::rtm:: ILocalCallInvitation *localInvitation) {
            if (_onLocalInvitationReceivedByPeer)
                _onLocalInvitationReceivedByPeer(handlerId, localInvitation);
        }
            
      /**
       Callback to the caller: occurs when the caller cancels a call invitation.
     
       @param localInvitation An ILocalCallInvitation object.
       */
        void RtmCallEventHandler::onLocalInvitationCanceled(agora::rtm::ILocalCallInvitation *localInvitation) {
            if (_onLocalInvitationCanceled)
                _onLocalInvitationCanceled(handlerId, localInvitation);
        }
            
      /**
       Callback to the caller: occurs when the life cycle of the outgoing call invitation ends in failure.
     
       @param localInvitation An ILocalCallInvitation object.
       @param errorCode The error code. See #LOCAL_INVITATION_ERR_CODE.
       */
        void RtmCallEventHandler::onLocalInvitationFailure(agora::rtm::ILocalCallInvitation *localInvitation, agora::rtm::LOCAL_INVITATION_ERR_CODE errorCode) {
            if (_onLocalInvitationFailure)
                _onLocalInvitationFailure(handlerId, localInvitation, errorCode);
        }


      /**
       Callback to the caller: occurs when the callee accepts the call invitation.
     
       @param localInvitation An ILocalCallInvitation object.
       @param response The callee's response to the call invitation.
       */
        void RtmCallEventHandler::onLocalInvitationAccepted(agora::rtm::ILocalCallInvitation *localInvitation, const char *response) {
            if (_onLocalInvitationAccepted)
                _onLocalInvitationAccepted(handlerId, localInvitation, response);
        }
          
      /**
       Callback to the caller: occurs when the callee refuses the call invitation.

       @param localInvitation An ILocalCallInvitation object.
       @param response The callee's response to the call invitation.
       */
        void RtmCallEventHandler::onLocalInvitationRefused(agora::rtm::ILocalCallInvitation *localInvitation, const char *response) {
            if (_onLocalInvitationRefused)
                _onLocalInvitationRefused(handlerId, localInvitation, response);
        }
            
      /**
       Callback for the callee: occurs when the callee refuses a call invitation.

       @param remoteInvitation An IRemoteCallInvitation object.
       */
        void RtmCallEventHandler::onRemoteInvitationRefused(agora::rtm::IRemoteCallInvitation *remoteInvitation) {
            if (_onRemoteInvitationRefused)
                _onRemoteInvitationRefused(handlerId, remoteInvitation);
        }
        
      /**
       Callback to the callee: occurs when the callee accepts a call invitation.

       @param remoteInvitation An IRemoteCallInvitation object.
       */
        void RtmCallEventHandler::onRemoteInvitationAccepted(agora::rtm::IRemoteCallInvitation *remoteInvitation) {
            if (_onRemoteInvitationAccepted)
                _onRemoteInvitationAccepted(handlerId, remoteInvitation);
        }
        
      /**
       Callback to the callee: occurs when the callee receives a call invitation.

       @param remoteInvitation An IRemoteCallInvitation object.
       */
        void RtmCallEventHandler::onRemoteInvitationReceived(agora::rtm::IRemoteCallInvitation *remoteInvitation) {
            if (_onRemoteInvitationReceived)
                _onRemoteInvitationReceived(handlerId, remoteInvitation);
        }
            
      /**
       Callback to the callee: occurs when the life cycle of the incoming call invitation ends in failure.

       @param remoteInvitation An IRemoteCallInvitation object.
       @param errorCode The error code. See #REMOTE_INVITATION_ERR_CODE.
       */
        void RtmCallEventHandler::onRemoteInvitationFailure(agora::rtm::IRemoteCallInvitation *remoteInvitation, agora::rtm::REMOTE_INVITATION_ERR_CODE errorCode) {
            if (_onRemoteInvitationFailure)
                _onRemoteInvitationFailure(handlerId, remoteInvitation, errorCode);
        }
        
      /**
       Callback to the callee: occurs when the caller cancels the call invitation.

       @param remoteInvitation An IRemoteCallInvitation object.
       */
        void RtmCallEventHandler::onRemoteInvitationCanceled(agora::rtm::IRemoteCallInvitation *remoteInvitation) {
            if (_onRemoteInvitationCanceled)
                _onRemoteInvitationCanceled(handlerId, remoteInvitation);
        }
}
}
