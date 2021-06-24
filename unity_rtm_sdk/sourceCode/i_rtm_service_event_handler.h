//
//  i_rtm_service_event_handler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/12.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once

#include "RtmServiceEventHandler.h"
#include "common.h"

AGORA_API void* service_event_handler_createEventHandle(
    int _id,
    CRtmServiceEventHandler* handler);

AGORA_API void service_event_handler_releaseEventHandler(
    void* channelEventHandlerInstance);
