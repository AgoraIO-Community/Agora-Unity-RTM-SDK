//
//  i_rtm_call_event_handler.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/22.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_call_event_handler.h"

extern "C" {
#define RTM_CALL_EVENT_HANDLER_PTR static_cast<agora::unity::rtm::RtmCallEventHandler *>(eventHandlerPtr);
}

AGORA_API void* i_rtm_call_event_handler_createEventHandler(int _index, CRtmCallEventHandler *rtmCallEventHandler)
{
    return new agora::unity::rtm::RtmCallEventHandler(_index, rtmCallEventHandler);
}


AGORA_API void i_rtm_call_event_releaseEventHandler(void *eventHandlerPtr)
{
    delete RTM_CALL_EVENT_HANDLER_PTR;
    eventHandlerPtr = nullptr;
}
