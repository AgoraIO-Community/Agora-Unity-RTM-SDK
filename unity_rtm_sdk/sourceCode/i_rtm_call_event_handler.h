//
//  i_rtm_call_event_handler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once
#include "RtmCallEventHandler.h"
#include "common.h"

AGORA_API void* i_rtm_call_event_handler_createEventHandler(
    int _index,
    CRtmCallEventHandler *rtmCallEventHandler);

AGORA_API void i_rtm_call_event_releaseEventHandler(void* eventHandlerPtr);
