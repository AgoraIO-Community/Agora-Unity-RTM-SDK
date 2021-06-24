//
//  i_rtm_call_event_handler.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_call_event_handler.h"

extern "C" {
#define RTM_CALL_EVENT_HANDLER_PTR static_cast<agora::unity::RtmCallEventHandler *>(eventHandlerPtr);
}

AGORA_API void* i_rtm_call_event_handler_createEventHandler(int _index, agora::unity::FUNC_onLocalInvitationReceivedByPeer _onLocalInvitationReceivedByPeer,
agora::unity::FUNC_onLocalInvitationCanceled _onLocalInvitationCanceled,
agora::unity::FUNC_onLocalInvitationFailure _onLocalInvitationFailure,
agora::unity::FUNC_onLocalInvitationAccepted _onLocalInvitationAccepted,
agora::unity::FUNC_onLocalInvitationRefused _onLocalInvitationRefused,
agora::unity::FUNC_onRemoteInvitationRefused _onRemoteInvitationRefused,
agora::unity::FUNC_onRemoteInvitationAccepted _onRemoteInvitationAccepted,
agora::unity::FUNC_onRemoteInvitationReceived _onRemoteInvitationReceived,
agora::unity::FUNC_onRemoteInvitationFailure _onRemoteInvitationFailure,
agora::unity::FUNC_onRemoteInvitationCanceled _onRemoteInvitationCanceled)
{
    return new agora::unity::RtmCallEventHandler(_index, _onLocalInvitationReceivedByPeer,
                                                 _onLocalInvitationCanceled,
                                                 _onLocalInvitationFailure,
                                                 _onLocalInvitationAccepted,
                                                 _onLocalInvitationRefused,
                                                 _onRemoteInvitationRefused,
                                                 _onRemoteInvitationAccepted,
                                                 _onRemoteInvitationReceived,
                                                 _onRemoteInvitationFailure,
                                                 _onRemoteInvitationCanceled);
}


AGORA_API void i_rtm_call_event_releaseEventHandler(void *eventHandlerPtr)
{
    delete RTM_CALL_EVENT_HANDLER_PTR;
    eventHandlerPtr = nullptr;
}
