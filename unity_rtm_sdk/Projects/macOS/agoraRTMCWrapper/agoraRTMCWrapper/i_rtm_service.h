//
//  i_agora_rtm_service_c.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once

#include "common.h"
#include "ChannelEventHandler.h"
#include "RtmCallEventHandler.h"
#include "LogHelper.h"

AGORA_API void* createRtmService_();

AGORA_API const char* _getRtmSdkVersion_();

AGORA_API int setLogFileSize(void *rtmInstance, int fileSizeInKBytes);

AGORA_API int setLogFilter(void *rtmInstance, int filter);

AGORA_API int setLogFile(void *rtmInstance, const char* logfile);

AGORA_API int getChannelMemberCount(void *rtmInstance, const char* channelIds[], int channelCount, long long requestId);

AGORA_API int getChannelAttributesByKeys(void *rtmInstance, const char* channelId, const char* attributeKeys[], int numberOfKeys, long long requestId);

AGORA_API int setChannelAttributes(void *rtmInstance, const char* channelId, long long attributes[], int numberOfAttributes, bool enableNotificationToChannelMembers, long long requestId);

AGORA_API int getChannelAttributes(void *rtmInstance, const char* channelId, long long requestId);


AGORA_API int clearChannelAttributes(void *rtmInstance, const char* channelId, bool enableNotificationToChannelMembers, long long requestId);

AGORA_API int deleteChannelAttributesByKeys(void *rtmInstance, const char* channelId, const char* attributeKeys[], int numberOfKeys, bool enableNotificationToChannelMembers, long long requestId);

AGORA_API int getUserAttributesByKeys(void *rtmInstance, const char* userId, const char* attributeKeys[], int numberOfKeys, long long requestId);

AGORA_API int getUserAttributes(void *rtmInstance, const char* userId, long long requestId);

AGORA_API int clearLocalUserAttributes(void *rtmInstance, long long requestId);

AGORA_API int deleteLocalUserAttributesByKeys(void *rtmInstance, const char* attributeKeys[], int numberOfKeys, long long requestId);

AGORA_API int queryPeersOnlineStatus(void *rtmInstance, const char* peerIds[], int peerCount, long long requestId);

AGORA_API int subscribePeersOnlineStatus(void *rtmInstance, const char* peerIds[], int peerCount, long long requestId);

AGORA_API int unsubscribePeersOnlineStatus(void *rtmInstance, const char* peerIds[], int peerCount, long long requestId);

AGORA_API int queryPeersBySubscriptionOption(void *rtmInstance,    agora::rtm::PEER_SUBSCRIPTION_OPTION option, long long requestId);

AGORA_API int setLocalUserAttributes(void *rtmInstance, void* attributes, int numberOfAttributes, long long requestId);

AGORA_API int addOrUpdateLocalUserAttributes(void *rtmInstance, void* attributes, int numberOfAttributes, long long requestId);

AGORA_API int setParameters(void *rtmInstance, const char* parameters);

AGORA_API void *createChannelAttribute(void *rtmInstance);

AGORA_API int createImageMessageByUploading(void *rtmInstance, const char* filePath, long long requestId);

AGORA_API int createFileMessageByUploading(void *rtmInstance, const char* filePath, long long requestId);

AGORA_API void *createImageMessageByMediaId(void *rtmInstance, const char* mediaId);

AGORA_API void *createFileMessageByMediaId(void *rtmInstance, const char* mediaId);

AGORA_API void *createMessage(void *rtmInstance, const uint8_t* rawData, int length, const char* description);

AGORA_API void *createMessage2(void *rtmInstance, const uint8_t* rawData, int length);

AGORA_API void *createMessage3(void *rtmInstance, const char* message);

AGORA_API void *createMessage4(void *rtmInstance);

AGORA_API void *createChannel(void *rtmInstance, const char *channelId, void *channelEventHandlerInstance);

AGORA_API int sendMessageToPeer(void *rtmInstance, const char *peerId, void *message, bool enableOfflineMessaging,
                                    bool enableHistoricalMessaging);

AGORA_API int cancelMediaUpload(void *rtmInstance, long long requestId);

AGORA_API int cancelMediaDownload(void *rtmInstance, long long requestId);

AGORA_API int downloadMediaToFile(void *rtmInstance, const char* mediaId, const char* filePath, long long requestId);

AGORA_API int downloadMediaToMemory(void *rtmInstance, const char* mediaId, long long requestId);

AGORA_API int sendMessageToPeer2(void *rtmInstance, const char *peerId, void *message);

AGORA_API int renewToken(void *rtmInstance, const char* token);

AGORA_API int logout(void *rtmInstance);

AGORA_API int login(void *rtmInstance, const char *token, const char *userId);

AGORA_API void release(void *rtmInstance, bool sync);

AGORA_API void *getRtmCallManager(void *rtmInstance, void *rtmCallEventHandler);
