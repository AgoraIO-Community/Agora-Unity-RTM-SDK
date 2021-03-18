//
//  i_rtm_channel_event_handler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/11.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once
#include "common.h"
#include "ChannelEventHandler.h"

AGORA_API void* channel_event_handler_createEventHandler(int _id, agora::unity::FUNC_channel_onJoinSuccess joinSuccess,
                                        agora::unity::FUNC_channel_onJoinFailure joinFailure,
                                        agora::unity::FUNC_channel_onLeave onLeave,
                                        agora::unity::FUNC_channel_onMessageReceived onMessageReceived,
                                        agora::unity::FUNC_channel_onImageMessageReceived onImageMessageReceived,
                                        agora::unity::FUNC_channel_onFileMessageReceived onFileMessageReceived,
                                        agora::unity::FUNC_channel_onSendMessageResult onSendMessage,
                                        agora::unity::FUNC_channel_onMemberJoined onMemberJoined,
                                        agora::unity::FUNC_channel_onMemberLeft onMemberLeft,
                                        agora::unity::FUNC_channel_onGetMembers onGetMembers,
                                        agora::unity::FUNC_channel_onMemberCountUpdated onMemberCountUpdated,
                                        agora::unity::FUNC_channel_onAttributeUpdate onAttributeUpdate);

AGORA_API void channel_event_handler_releaseEventHandler(void *channelEventHandlerInstance);
