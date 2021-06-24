//
//  i_rtm_channel_member.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/11.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once

#include "common.h"

/**
Retrieves the user ID of a user in the channel.

@return User ID of a user in the channel.
*/
AGORA_API const char* channel_member_getUserId(void* channel_member_instance);

/**
Retrieves the channel ID of the user.

@return Channel ID of the user.
*/
AGORA_API const char* channel_member_getChannelId(
    void* channel_member_instance);

/**
Releases all resources used by the \ref agora::rtm::IChannelMember
"IChannelMember" instance.
*/
AGORA_API void channel_member_release(void* channel_member_instance);
