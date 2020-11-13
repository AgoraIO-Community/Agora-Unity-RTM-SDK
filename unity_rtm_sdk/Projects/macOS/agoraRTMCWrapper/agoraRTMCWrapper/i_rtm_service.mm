//
//  i_agora_rtm_service_c.cpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//

#include "i_rtm_service.h"
#include "RtmServiceEventHandler.h"
#include <vector>

extern "C" {
#define RTM_SERVICE_INSTANCE static_cast<agora::rtm::IRtmService *>(rtmInstance)
#define IMESSAGE_INSTANCE static_cast<agora::rtm::IMessage *>(message)
}

AGORA_API void* createRtmService_()
{
    return agora::rtm::createRtmService();
}

AGORA_API const char* _getRtmSdkVersion_()
{
    return agora::rtm::getRtmSdkVersion();
}

AGORA_API int setLogFileSize(void *rtmInstance, int fileSizeInKBytes)
{
    return RTM_SERVICE_INSTANCE->setLogFileSize(fileSizeInKBytes);
}

AGORA_API int setLogFilter(void *rtmInstance, int filter)
{
    return RTM_SERVICE_INSTANCE->setLogFilter(agora::rtm::LOG_FILTER_TYPE(filter));
}

AGORA_API int setLogFile(void *rtmInstance, const char* logfile)
{
    std::string filePath(logfile);
    filePath.append("_unity_plugin_log.txt");
    agora::unity::LogHelper::getInstance().startLogService(filePath.c_str());
    return RTM_SERVICE_INSTANCE->setLogFile(logfile);
}

AGORA_API int getChannelMemberCount(void *rtmInstance, const char* channelIds[], int channelCount, long long requestId)
{
    return RTM_SERVICE_INSTANCE->getChannelMemberCount(channelIds, channelCount, requestId);
}

AGORA_API int getChannelAttributesByKeys(void *rtmInstance, const char* channelId, const char* attributeKeys[], int numberOfKeys, long long requestId)
{
    return RTM_SERVICE_INSTANCE->getChannelAttributesByKeys(channelId, attributeKeys, numberOfKeys, requestId);
}

AGORA_API int getChannelAttributes(void *rtmInstance, const char* channelId, long long requestId)
{
    return RTM_SERVICE_INSTANCE->getChannelAttributes(channelId, requestId);
}


AGORA_API int clearChannelAttributes(void *rtmInstance, const char* channelId, bool enableNotificationToChannelMembers, long long requestId)
{
    agora::rtm::ChannelAttributeOptions option;
    option.enableNotificationToChannelMembers = enableNotificationToChannelMembers;
    return RTM_SERVICE_INSTANCE->clearChannelAttributes(channelId, option, requestId);
}

AGORA_API int deleteChannelAttributesByKeys(void *rtmInstance, const char* channelId, const char* attributeKeys[], int numberOfKeys, bool enableNotificationToChannelMembers, long long requestId)
{
    agora::rtm::ChannelAttributeOptions option;
    option.enableNotificationToChannelMembers = enableNotificationToChannelMembers;
    return RTM_SERVICE_INSTANCE->deleteChannelAttributesByKeys(channelId, attributeKeys, numberOfKeys, option, requestId);
}

AGORA_API int getUserAttributesByKeys(void *rtmInstance, const char* userId, const char* attributeKeys[], int numberOfKeys, long long requestId)
{
    return RTM_SERVICE_INSTANCE->getUserAttributesByKeys(userId, attributeKeys, numberOfKeys, requestId);
}

AGORA_API int getUserAttributes(void *rtmInstance, const char* userId, long long requestId)
{
    return RTM_SERVICE_INSTANCE->getUserAttributes(userId, requestId);
}

AGORA_API int clearLocalUserAttributes(void *rtmInstance, long long requestId)
{
    return RTM_SERVICE_INSTANCE->clearLocalUserAttributes(requestId);
}

AGORA_API int deleteLocalUserAttributesByKeys(void *rtmInstance, const char* attributeKeys[], int numberOfKeys, long long requestId)
{
    return RTM_SERVICE_INSTANCE->deleteLocalUserAttributesByKeys(attributeKeys, numberOfKeys, requestId);
}

AGORA_API int queryPeersOnlineStatus(void *rtmInstance, const char* peerIds[], int peerCount, long long requestId)
{
    return RTM_SERVICE_INSTANCE->queryPeersOnlineStatus(peerIds, peerCount, requestId);
}

AGORA_API int subscribePeersOnlineStatus(void *rtmInstance, const char* peerIds[], int peerCount, long long requestId)
{
    return RTM_SERVICE_INSTANCE->subscribePeersOnlineStatus(peerIds, peerCount, requestId);
}

AGORA_API int unsubscribePeersOnlineStatus(void *rtmInstance, const char* peerIds[], int peerCount, long long requestId)
{
    return RTM_SERVICE_INSTANCE->unsubscribePeersOnlineStatus(peerIds, peerCount, requestId);
}

AGORA_API int queryPeersBySubscriptionOption(void *rtmInstance,    agora::rtm::PEER_SUBSCRIPTION_OPTION option, long long requestId)
{
    return RTM_SERVICE_INSTANCE->queryPeersBySubscriptionOption(option, requestId);
}

AGORA_API int setLocalUserAttributes(void *rtmInstance, void* attributes, int numberOfAttributes, long long requestId)
{
    return RTM_SERVICE_INSTANCE->setLocalUserAttributes((agora::rtm::RtmAttribute *)attributes, numberOfAttributes, requestId);
}

AGORA_API int setChannelAttributes(void *rtmInstance, const char* channelId, long long attributes[], int numberOfAttributes, bool enableNotificationToChannelMembers, long long requestId)
{
    agora::rtm::ChannelAttributeOptions channelAttributeOptions;
    channelAttributeOptions.enableNotificationToChannelMembers = enableNotificationToChannelMembers;
    
    const agora::rtm::IRtmChannelAttribute *channelAttributeList[numberOfAttributes];
    for(int i = 0; i < numberOfAttributes; i++) {
        channelAttributeList[i] = reinterpret_cast<agora::rtm::IRtmChannelAttribute *>(attributes[i]);
    }
    return RTM_SERVICE_INSTANCE->setChannelAttributes(channelId, channelAttributeList, numberOfAttributes, channelAttributeOptions, requestId);
}

AGORA_API int addOrUpdateLocalUserAttributes(void *rtmInstance, void* attributes, int numberOfAttributes, long long requestId)
{
    return RTM_SERVICE_INSTANCE->addOrUpdateLocalUserAttributes((agora::rtm::RtmAttribute *)attributes, numberOfAttributes, requestId);
}

AGORA_API int setParameters(void *rtmInstance, const char* parameters)
{
    return RTM_SERVICE_INSTANCE->setParameters(parameters);
}

AGORA_API void *createChannelAttribute(void *rtmInstance)
{
    return RTM_SERVICE_INSTANCE->createChannelAttribute();
}

AGORA_API int createImageMessageByUploading(void *rtmInstance, const char* filePath, long long requestId)
{
    return RTM_SERVICE_INSTANCE->createImageMessageByUploading(filePath, requestId);
}

AGORA_API int createFileMessageByUploading(void *rtmInstance, const char* filePath, long long requestId)
{
    return RTM_SERVICE_INSTANCE->createFileMessageByUploading(filePath, requestId);
}

AGORA_API void *createImageMessageByMediaId(void *rtmInstance, const char* mediaId)
{
    return RTM_SERVICE_INSTANCE->createImageMessageByMediaId(mediaId);
}

AGORA_API void *createFileMessageByMediaId(void *rtmInstance, const char* mediaId)
{
    return RTM_SERVICE_INSTANCE->createFileMessageByMediaId(mediaId);
}

AGORA_API void *createMessage(void *rtmInstance, const uint8_t* rawData, int length, const char* description)
{
    return RTM_SERVICE_INSTANCE->createMessage(rawData, length, description);
}

AGORA_API void *createMessage2(void *rtmInstance, const uint8_t* rawData, int length)
{
    return RTM_SERVICE_INSTANCE->createMessage(rawData, length);
}

AGORA_API void *createMessage3(void *rtmInstance, const char* message)
{
    return RTM_SERVICE_INSTANCE->createMessage(message);
}

AGORA_API void *createMessage4(void *rtmInstance)
{
    return RTM_SERVICE_INSTANCE->createMessage();
}

AGORA_API void *createChannel(void *rtmInstance, const char *channelId, void *channelEventHandlerInstance)
{
    agora::unity::ChannelEventHandler *channelEventHandlerPtr= static_cast<agora::unity::ChannelEventHandler *>(channelEventHandlerInstance);
    return RTM_SERVICE_INSTANCE->createChannel(channelId, channelEventHandlerPtr);
}

AGORA_API int sendMessageToPeer(void *rtmInstance, const char *peerId, void *message, bool enableOfflineMessaging,
                                    bool enableHistoricalMessaging)
{
    agora::rtm::SendMessageOptions option;
    option.enableHistoricalMessaging = enableHistoricalMessaging;
    option.enableOfflineMessaging = enableOfflineMessaging;
    return RTM_SERVICE_INSTANCE->sendMessageToPeer(peerId, IMESSAGE_INSTANCE, option);
}

AGORA_API int cancelMediaUpload(void *rtmInstance, long long requestId)
{
    return RTM_SERVICE_INSTANCE->cancelMediaUpload(requestId);
}

AGORA_API int cancelMediaDownload(void *rtmInstance, long long requestId)
{
    return RTM_SERVICE_INSTANCE->cancelMediaDownload(requestId);
}

AGORA_API int downloadMediaToFile(void *rtmInstance, const char* mediaId, const char* filePath, long long requestId)
{
    return RTM_SERVICE_INSTANCE->downloadMediaToFile(mediaId, filePath, requestId);
}

AGORA_API int downloadMediaToMemory(void *rtmInstance, const char* mediaId, long long requestId)
{
    return RTM_SERVICE_INSTANCE->downloadMediaToMemory(mediaId, requestId);
}

AGORA_API int sendMessageToPeer2(void *rtmInstance, const char *peerId, void *message)
{
    return RTM_SERVICE_INSTANCE->sendMessageToPeer(peerId, IMESSAGE_INSTANCE);
}

AGORA_API int renewToken(void *rtmInstance, const char* token)
{
    return RTM_SERVICE_INSTANCE->renewToken(token);
}

AGORA_API int logout(void *rtmInstance)
{
    return RTM_SERVICE_INSTANCE->logout();
}

AGORA_API int login(void *rtmInstance, const char *token, const char *userId)
{
    return RTM_SERVICE_INSTANCE->login(token, userId);
}

AGORA_API void release(void *rtmInstance, bool sync)
{
    return RTM_SERVICE_INSTANCE->release(sync);
}

AGORA_API int initialize(void *rtmInstance, const char *appId, void *eventHandler)
{
    return RTM_SERVICE_INSTANCE->initialize(appId, static_cast<agora::unity::RtmServiceEventHandler *>(eventHandler));
}

AGORA_API void *getRtmCallManager(void *rtmInstance, void *rtmCallEventHandler)
{
    return RTM_SERVICE_INSTANCE->getRtmCallManager(static_cast<agora::unity::RtmCallEventHandler *>(rtmCallEventHandler));
}
