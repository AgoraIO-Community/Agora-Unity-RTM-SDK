//
//  i_agora_rtm_service_c.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once

#include <string.h>
#include "ChannelEventHandler.h"
#include "LogHelper.h"
#include "RtmCallEventHandler.h"
#include "common.h"

AGORA_API void* createRtmService_rtm();

AGORA_API const char* _getRtmSdkVersion_rtm();

AGORA_API int setLogFileSize_rtm(void* rtmInstance, int fileSizeInKBytes);

AGORA_API int setLogFilter_rtm(void* rtmInstance, int filter);

AGORA_API int setLogFile_rtm(void* rtmInstance, const char* logfile);

AGORA_API int getChannelMemberCount_rtm(void* rtmInstance,
                                        const char* channelIds[],
                                        int channelCount,
                                        long long& requestId);

AGORA_API int getChannelAttributesByKeys_rtm(void* rtmInstance,
                                             const char* channelId,
                                             const char* attributeKeys[],
                                             int numberOfKeys,
                                             long long& requestId);

AGORA_API int setChannelAttributes_rtm(void* rtmInstance,
                                       const char* channelId,
                                       long long attributes[],
                                       const int numberOfAttributes,
                                       bool enableNotificationToChannelMembers,
                                       long long& requestId);

AGORA_API int addOrUpdateChannelAttributes_rtm(void* rtmInstance, 
                                            const char* channelId, 
                                            long long attributes[], 
                                            const int numberOfAttributes, 
                                            bool enableNotificationToChannelMembers,
                                            long long &requestId);

AGORA_API int getChannelAttributes_rtm(void* rtmInstance,
                                       const char* channelId,
                                       long long& requestId);

AGORA_API int clearChannelAttributes_rtm(
    void* rtmInstance,
    const char* channelId,
    bool enableNotificationToChannelMembers,
    long long& requestId);

AGORA_API int deleteChannelAttributesByKeys_rtm(
    void* rtmInstance,
    const char* channelId,
    const char* attributeKeys[],
    int numberOfKeys,
    bool enableNotificationToChannelMembers,
    long long& requestId);

AGORA_API int getUserAttributesByKeys_rtm(void* rtmInstance,
                                          const char* userId,
                                          const char* attributeKeys[],
                                          int numberOfKeys,
                                          long long& requestId);

AGORA_API int getUserAttributes_rtm(void* rtmInstance,
                                    const char* userId,
                                    long long& requestId);

AGORA_API int clearLocalUserAttributes_rtm(void* rtmInstance,
                                           long long& requestId);

AGORA_API int deleteLocalUserAttributesByKeys_rtm(void* rtmInstance,
                                                  const char* attributeKeys[],
                                                  int numberOfKeys,
                                                  long long& requestId);

AGORA_API int queryPeersOnlineStatus_rtm(void* rtmInstance,
                                         const char* peerIds[],
                                         int peerCount,
                                         long long& requestId);

AGORA_API int subscribePeersOnlineStatus_rtm(void* rtmInstance,
                                             const char* peerIds[],
                                             int peerCount,
                                             long long& requestId);

AGORA_API int unsubscribePeersOnlineStatus_rtm(void* rtmInstance,
                                               const char* peerIds[],
                                               int peerCount,
                                               long long& requestId);

AGORA_API int queryPeersBySubscriptionOption_rtm(
    void* rtmInstance,
    agora::rtm::PEER_SUBSCRIPTION_OPTION option,
    long long& requestId);

AGORA_API int setLocalUserAttributes_rtm(void* rtmInstance,
                                         const char* attributesInfo,
                                         int numberOfAttributes,
                                         long long& requestId);

AGORA_API int addOrUpdateLocalUserAttributes_rtm(void* rtmInstance,
                                                 const char* attributesInfo,
                                                 int numberOfAttributes,
                                                 long long& requestId);

AGORA_API int setParameters_rtm_rtm(void* rtmInstance, const char* parameters);

AGORA_API void* createChannelAttribute_rtm(void* rtmInstance);

AGORA_API int createImageMessageByUploading_rtm(void* rtmInstance,
                                                const char* filePath,
                                                long long& requestId);

AGORA_API int createFileMessageByUploading_rtm(void* rtmInstance,
                                               const char* filePath,
                                               long long& requestId);

AGORA_API void* createImageMessageByMediaId_rtm(void* rtmInstance,
                                                const char* mediaId);

AGORA_API void* createFileMessageByMediaId_rtm(void* rtmInstance,
                                               const char* mediaId);

AGORA_API void* createMessage_rtm(void* rtmInstance,
                                  const uint8_t* rawData,
                                  int length,
                                  const char* description);

AGORA_API void* createMessage2_rtm(void* rtmInstance,
                                   const uint8_t* rawData,
                                   int length);

AGORA_API void* createMessage3_rtm(void* rtmInstance, const char* message);

AGORA_API void* createMessage4_rtm(void* rtmInstance);

AGORA_API void* createChannel_rtm(void* rtmInstance,
                                  const char* channelId,
                                  void* channelEventHandlerInstance);

AGORA_API int sendMessageToPeer_rtm(void* rtmInstance,
                                    const char* peerId,
                                    void* message,
                                    bool enableOfflineMessaging,
                                    bool enableHistoricalMessaging);

AGORA_API int cancelMediaUpload_rtm(void* rtmInstance, long long requestId);

AGORA_API int cancelMediaDownload_rtm(void* rtmInstance, long long requestId);

AGORA_API int downloadMediaToFile_rtm(void* rtmInstance,
                                      const char* mediaId,
                                      const char* filePath,
                                      long long& requestId);

AGORA_API int downloadMediaToMemory_rtm(void* rtmInstance,
                                        const char* mediaId,
                                        long long& requestId);

AGORA_API int sendMessageToPeer2_rtm(void* rtmInstance,
                                     const char* peerId,
                                     void* message);

AGORA_API int renewToken_rtm(void* rtmInstance, const char* token);

AGORA_API int logout_rtm(void* rtmInstance);

AGORA_API int login_rtm(void* rtmInstance,
                        const char* token,
                        const char* userId);
                                         
AGORA_API int subscribeUserAttributes_rtm(void* rtmInstance, const char* userId,
                                    long long& requestId);

AGORA_API int unsubscribeUserAttributes_rtm(void* rtmInstance, const char* userId,
                                      long long& requestId);

AGORA_API void release_rtm(void* rtmInstance, bool sync);

AGORA_API void* getRtmCallManager_rtm(void* rtmInstance,
                                      void* rtmCallEventHandler);
