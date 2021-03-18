//
//  i_rtm_channel_member.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/11.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_channel_member.h"

extern "C" {
    #define CHANNEL_MEMBER_INSTANCE static_cast<agora::rtm::IChannelMember *>(channel_member_instance)
}

/**
Retrieves the user ID of a user in the channel.

@return User ID of a user in the channel.
*/
AGORA_API const char * channel_member_getUserId(void* channel_member_instance)
{
    return CHANNEL_MEMBER_INSTANCE->getUserId();
}
    
/**
Retrieves the channel ID of the user.

@return Channel ID of the user.
*/
AGORA_API const char * channel_member_getChannelId(void* channel_member_instance)
{
    return CHANNEL_MEMBER_INSTANCE->getChannelId();
}
    
/**
Releases all resources used by the \ref agora::rtm::IChannelMember "IChannelMember" instance.
*/
AGORA_API void channel_member_release(void* channel_member_instance)
{
    return CHANNEL_MEMBER_INSTANCE->release();
}
