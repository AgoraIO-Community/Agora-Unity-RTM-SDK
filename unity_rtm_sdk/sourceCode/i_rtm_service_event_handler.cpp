//
//  i_rtm_service_event_handler.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/12.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_service_event_handler.h"

extern "C" {
#define Service_EVENT_HANDLER_PTR                     \
  static_cast<agora::unity::RtmServiceEventHandler*>( \
      channelEventHandlerInstance);
}

AGORA_API void* service_event_handler_createEventHandle(
    int _id,
    CRtmServiceEventHandler* handler) {
  return new agora::unity::RtmServiceEventHandler(
      _id, handler);
}

AGORA_API void service_event_handler_releaseEventHandler(
    void* channelEventHandlerInstance) {
  delete Service_EVENT_HANDLER_PTR;
  channelEventHandlerInstance = nullptr;
}
