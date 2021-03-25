//
//  ChannelEventHandler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/11.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once
#include "common.h"
#include "LogHelper.h"

namespace agora {
    namespace unity {
    
        #if defined(_WIN32)
        typedef void(__stdcall *FUNC_channel_onJoinSuccess)(int _id);
        typedef void(__stdcall *FUNC_channel_onJoinFailure)(int _id, int errorCode);
        typedef void(__stdcall *FUNC_channel_onLeave)(int _id, int errorCode);
        typedef void(__stdcall *FUNC_channel_onMessageReceived)(int _id, const char *userId, const rtm::IMessage *message);
        typedef void(__stdcall *FUNC_channel_onImageMessageReceived)(int _id, const char *userId, const rtm::IMessage *message);
        typedef void(__stdcall *FUNC_channel_onFileMessageReceived)(int _id, const char *userId, const rtm::IMessage *message);
        typedef void(__stdcall *FUNC_channel_onSendMessageResult)(int _id, long long messageId, int state);
        typedef void(__stdcall *FUNC_channel_onMemberJoined)(int _id, void *member);
        typedef void(__stdcall *FUNC_channel_onMemberLeft)(int _id, void *member);
        typedef void(__stdcall *FUNC_channel_onGetMembers)(int _id, const char *members, int userCount, int errorCode);
        typedef void(__stdcall *FUNC_channel_onMemberCountUpdated)(int _id, int memberCount);
        typedef void(__stdcall *FUNC_channel_onAttributeUpdate)(const char *attributes, int numberOfAttributes);
        #else
        typedef void(*FUNC_channel_onJoinSuccess)(int _id);
        typedef void(*FUNC_channel_onJoinFailure)(int _id, int errorCode);
        typedef void(*FUNC_channel_onLeave)(int _id, int errorCode);
        typedef void(*FUNC_channel_onMessageReceived)(int _id, const char *userId, const rtm::IMessage *message);
        typedef void(*FUNC_channel_onImageMessageReceived)(int _id, const char *userId, const rtm::IMessage *message);
        typedef void(*FUNC_channel_onFileMessageReceived)(int _id, const char *userId, const rtm::IMessage *message);
        typedef void(*FUNC_channel_onSendMessageResult)(int _id, long long messageId, int state);
        typedef void(*FUNC_channel_onMemberJoined)(int _id, void *member);
        typedef void(*FUNC_channel_onMemberLeft)(int _id, void *member);
        typedef void(*FUNC_channel_onGetMembers)(int _id, const char *members, int userCount, int errorCode);
        typedef void(*FUNC_channel_onMemberCountUpdated)(int _id, int memberCount);
        typedef void(*FUNC_channel_onAttributeUpdate)(int _id, const char *attributes, int numberOfAttributes);
        #endif
    
        class ChannelEventHandler : public rtm::IChannelEventHandler {
        private:
            int handlerId;
            FUNC_channel_onJoinSuccess _joinSuccess = nullptr;
            FUNC_channel_onJoinFailure _joinFailure = nullptr;
            FUNC_channel_onLeave _onLeave = nullptr;
            FUNC_channel_onMessageReceived _onMessageReceived = nullptr;
            FUNC_channel_onImageMessageReceived _onImageMessageReceived = nullptr;
            FUNC_channel_onFileMessageReceived _onFileMessageReceived = nullptr;
            FUNC_channel_onSendMessageResult _onSendMessageResult = nullptr;
            FUNC_channel_onMemberJoined _onMemberJoined = nullptr;
            FUNC_channel_onMemberLeft _onMemberLeft = nullptr;
            FUNC_channel_onGetMembers _onGetMembers = nullptr;
            FUNC_channel_onMemberCountUpdated _onMemberCountUpdated = nullptr;
            FUNC_channel_onAttributeUpdate _onAttributeUpdate = nullptr;
            
        public:
            ChannelEventHandler();
            ChannelEventHandler(int _id, FUNC_channel_onJoinSuccess joinSuccess,
                                FUNC_channel_onJoinFailure joinFailure,
                                FUNC_channel_onLeave onLeave,
                                FUNC_channel_onMessageReceived onMessageReceived,
                                FUNC_channel_onImageMessageReceived onImageMessageReceived,
                                FUNC_channel_onFileMessageReceived onFileMessageReceived,
                                FUNC_channel_onSendMessageResult onSendMessage,
                                FUNC_channel_onMemberJoined onMemberJoined,
                                FUNC_channel_onMemberLeft onMemberLeft,
                                FUNC_channel_onGetMembers onGetMembers,
                                FUNC_channel_onMemberCountUpdated onMemberCountUpdated,
                                FUNC_channel_onAttributeUpdate onAttributeUpdate);
            
            virtual ~ChannelEventHandler();
            
            virtual void onJoinSuccess() override;
               
             /**
              Occurs when failing to join a channel.

              The local user receives this callback when the \ref agora::rtm::IChannel::join "join" method call fails.
              
              @param errorCode The error code. See #JOIN_CHANNEL_ERR.
              */
            virtual void onJoinFailure(rtm::JOIN_CHANNEL_ERR errorCode) override;

             /**
              Returns the result of the \ref agora::rtm::IChannel::leave "leave" method call.
              
              @param errorCode The error code. See #LEAVE_CHANNEL_ERR.
              */
            virtual void onLeave(rtm::LEAVE_CHANNEL_ERR errorCode) override;

             /**
              Occurs when receiving a channel message.

              @param userId The message sender.
              @param message The received channel message. See \ref agora::rtm::IMessage "IMessage".
              */
            virtual void onMessageReceived(const char *userId, const rtm::IMessage *message) override;
               /**
                Occurs when receiving a channel image message.
                
                @param userId The message sender.
                @param message The received channel image message. See \ref agora::rtm::IImageMessage "IImageMessage".
                */
            virtual void onImageMessageReceived(const char *userId, const rtm::IImageMessage* message) override;
               
               /**
                Occurs when receiving a channel file message.
                
                @param userId The message sender.
                @param message The received channel file message. See \ref agora::rtm::IFileMessage "IFileMessage".
                */
            virtual void onFileMessageReceived(const char *userId, const rtm::IFileMessage* message) override;

             /**
              Returns the result of the \ref agora::rtm::IChannel::sendMessage "sendMessage" method call.

              @param messageId The ID of the sent channel message.
              @param state The error codes. See #CHANNEL_MESSAGE_ERR_CODE.
              */
            virtual void onSendMessageResult(long long messageId, rtm::CHANNEL_MESSAGE_ERR_CODE state) override;
               
             /**
              Occurs when a remote user joins the channel.

              When a remote user calls the \ref agora::rtm::IChannel::join "join" method and receives the \ref agora::rtm::IChannelEventHandler::onJoinSuccess "onJoinSuccess" callback (successfully joins the channel), the local user receives this callback.
              
              @note This callback is disabled when the number of the channel members exceeds 512.

              @param member The user joining the channel. See IChannelMember.
              */
            virtual void onMemberJoined(rtm::IChannelMember *member) override;
               
             /**
              Occurs when a remote member leaves the channel.

              When a remote member in the channel calls the \ref agora::rtm::IChannel::leave "leave" method and receives the the \ref agora::rtm::IChannelEventHandler::onLeave "onLeave (LEAVE_CHANNEL_ERR_OK)" callback, the local user receives this callback.
              
              @note This callback is disabled when the number of the channel members exceeds 512.

              @param member The channel member that leaves the channel. See IChannelMember.
              */
            virtual void onMemberLeft(rtm::IChannelMember *member) override;
               
             /**
              Returns the result of the \ref agora::rtm::IChannel::getMembers "getMembers" method call.
              
              When the method call succeeds, the SDK returns the member list of the channel.

              @param members The member list. See IChannelMember.
              @param userCount The number of members.
              @param errorCode Error code. See #GET_MEMBERS_ERR.
              */
            virtual void onGetMembers(rtm::IChannelMember **members, int userCount, rtm::GET_MEMBERS_ERR errorCode) override;
               
//             /**
//              Occurs when channel attributes are updated, and returns all attributes of the channel.
//
//              @note This callback is enabled only when the user, who updates the attributes of the channel, sets \ref agora::rtm::ChannelAttributeOptions::enableNotificationToChannelMembers "enableNotificationToChannelMembers" as true. Also note that this flag is valid only within the current channel attribute method call.
//
//              @param attributes All attribute of this channel.
//              @param numberOfAttributes The total number of the channel attributes.
//              */
            virtual void onAttributesUpdated(const rtm::IRtmChannelAttribute* attributes[], int numberOfAttributes) override;
//
             /**
              Occurs when the number of the channel members changes, and returns the new number.

              @note
              - When the number of channel members &le; 512, the SDK returns this callback when the number changes and at a MAXIMUM speed of once per second.
              - When the number of channel members exceeds 512, the SDK returns this callback when the number changes and at a MAXIMUM speed of once every three seconds.
              - You will receive this callback when successfully joining an RTM channel, so Agore recommends implementing this callback to receive timely updates on the number of the channel members.

              @param memberCount Member count of this channel.
              */
            virtual void onMemberCountUpdated(int memberCount) override;
        };
    }
}
