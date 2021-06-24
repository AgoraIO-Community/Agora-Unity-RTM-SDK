//
//  i_rtm_channel_event_handler.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/11.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_channel_event_handler.h"

extern "C" {
#define CHANNEL_EVENT_HANDLER_PTR \
  static_cast<agora::unity::ChannelEventHandler*>(channelEventHandlerInstance);
}

AGORA_API void* channel_event_handler_createEventHandler(
    int _id,
    struct CChannelEventHandler* channelEventHandler) {
  return new agora::unity::ChannelEventHandler(_id, channelEventHandler);
}

AGORA_API void channel_event_handler_releaseEventHandler(
    void* channelEventHandlerInstance) {
  auto ptr = CHANNEL_EVENT_HANDLER_PTR;
  ptr->clear();
  delete ptr;
  channelEventHandlerInstance = nullptr;
}
