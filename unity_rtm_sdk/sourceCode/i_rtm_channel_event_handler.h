//
//  i_rtm_channel_event_handler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/11.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once
#include "ChannelEventHandler.h"
#include "common.h"

AGORA_API void* channel_event_handler_createEventHandler(
    int _id,
    struct CChannelEventHandler* channelEventHandler);

AGORA_API void channel_event_handler_releaseEventHandler(
    void* channelEventHandlerInstance);
