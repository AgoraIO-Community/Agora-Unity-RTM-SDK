//
//  i_rtm_call_event_handler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once
#include "common.h"
#include "RtmCallEventHandler.h"

AGORA_API void* i_rtm_call_event_handler_createEventHandler(int _index, agora::unity::FUNC_onLocalInvitationReceivedByPeer _onLocalInvitationReceivedByPeer,
agora::unity::FUNC_onLocalInvitationCanceled _onLocalInvitationCanceled,
agora::unity::FUNC_onLocalInvitationFailure _onLocalInvitationFailure,
agora::unity::FUNC_onLocalInvitationAccepted _onLocalInvitationAccepted,
agora::unity::FUNC_onLocalInvitationRefused _onLocalInvitationRefused,
agora::unity::FUNC_onRemoteInvitationRefused _onRemoteInvitationRefused,
agora::unity::FUNC_onRemoteInvitationAccepted _onRemoteInvitationAccepted,
agora::unity::FUNC_onRemoteInvitationReceived _onRemoteInvitationReceived,
agora::unity::FUNC_onRemoteInvitationFailure _onRemoteInvitationFailure,
agora::unity::FUNC_onRemoteInvitationCanceled _onRemoteInvitationCanceled);


AGORA_API void i_rtm_call_event_releaseEventHandler(void *eventHandlerPtr);
