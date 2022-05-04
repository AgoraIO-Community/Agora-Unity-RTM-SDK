//
//  common.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/11.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once
#include <stdio.h>

#include "rtm_private_api.h"
#if defined(_WIN64)
#define AGORARTC_EXPORT
#pragma comment(lib, "../sdk/x64/lib/agora_rtm_sdk.lib")
#include "../sdk/x64/include/IAgoraRtmCallManager.h"
#include "../sdk/x64/include/IAgoraRtmService.h"
#define AGORA_CALL __stdcall
#elif defined(_WIN32)
#define AGORARTC_EXPORT
#pragma comment(lib, "../sdk/x86/lib/agora_rtm_sdk.lib")
#include "../sdk/x86/include/IAgoraRtmCallManager.h"
#include "../sdk/x86/include/IAgoraRtmService.h"
#define AGORA_CALL __stdcall
#elif defined(__APPLE__)
#include <TargetConditionals.h>
#if TARGET_IPHONE_SIMULATOR
#include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmCallManager.h"
#include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmService.h"
#elif TARGET_OS_IPHONE
#include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmCallManager.h"
#include "../Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/AgoraRtmKit.framework/Headers/IAgoraRtmService.h"
#elif TARGET_OS_MAC
#include "../Agora_RTM_SDK_for_Mac/libs/AgoraRtmKit.framework/Headers/IAgoraRtmCallManager.h"
#include "../Agora_RTM_SDK_for_Mac/libs/AgoraRtmKit.framework/Headers/IAgoraRtmService.h"
#endif
#define AGORA_CALL
#elif defined(__ANDROID__) || defined(__linux__)
#include "../prebuilt/include/IAgoraRtmCallManager.h"
#include "../prebuilt/include/IAgoraRtmService.h"
#define AGORA_CALL
#endif

// channel callback function pointer
typedef void(AGORA_CALL* FUNC_channel_onJoinSuccess)(int _id);
typedef void(AGORA_CALL* FUNC_channel_onJoinFailure)(int _id, int errorCode);
typedef void(AGORA_CALL* FUNC_channel_onLeave)(int _id, int errorCode);
typedef void(AGORA_CALL* FUNC_channel_onMessageReceived)(
    int _id,
    const char* userId,
    const agora::rtm::IMessage* message);
typedef void(AGORA_CALL* FUNC_channel_onImageMessageReceived)(
    int _id,
    const char* userId,
    const agora::rtm::IMessage* message);
typedef void(AGORA_CALL* FUNC_channel_onFileMessageReceived)(
    int _id,
    const char* userId,
    const agora::rtm::IMessage* message);
typedef void(AGORA_CALL* FUNC_channel_onSendMessageResult)(int _id,
                                                           long long messageId,
                                                           int state);
typedef void(AGORA_CALL* FUNC_channel_onMemberJoined)(int _id, void* member);
typedef void(AGORA_CALL* FUNC_channel_onMemberLeft)(int _id, void* member);
typedef void(AGORA_CALL* FUNC_channel_onGetMembers)(int _id,
                                                    const char* members,
                                                    int userCount,
                                                    int errorCode);
typedef void(AGORA_CALL* FUNC_channel_onMemberCountUpdated)(int _id,
                                                            int memberCount);
typedef void(AGORA_CALL* FUNC_channel_onAttributeUpdate)(
    int _id,
    const char* attributes,
    int numberOfAttributes);

typedef void(AGORA_CALL* FUNC_channel_onLockAcquired)(int _id, const char *lockName, long long lockRev, long long requestId);

typedef void(AGORA_CALL* FUNC_channel_onLockExpired)(int _id, const char *lockName);

typedef void(AGORA_CALL* FUNC_channel_onLockAcquireFailed)(int _id, const char *lockName, long long requestId);
// call manager function pointer

typedef void(AGORA_CALL* FUNC_onLocalInvitationReceivedByPeer)(
    int index,
    void* localInvitation);
typedef void(AGORA_CALL* FUNC_onLocalInvitationCanceled)(int index,
                                                         void* localInvitation);
typedef void(AGORA_CALL* FUNC_onLocalInvitationFailure)(
    int index,
    void* localInvitation,
    agora::rtm::LOCAL_INVITATION_ERR_CODE errorCode);
typedef void(AGORA_CALL* FUNC_onLocalInvitationAccepted)(int index,
                                                         void* localInvitation,
                                                         const char* response);
typedef void(AGORA_CALL* FUNC_onLocalInvitationRefused)(int index,
                                                        void* localInvitation,
                                                        const char* response);
typedef void(AGORA_CALL* FUNC_onRemoteInvitationRefused)(
    int index,
    void* remoteInvitation);
typedef void(AGORA_CALL* FUNC_onRemoteInvitationAccepted)(
    int index,
    void* remoteInvitation);
typedef void(AGORA_CALL* FUNC_onRemoteInvitationReceived)(
    int index,
    void* remoteInvitation);
typedef void(AGORA_CALL* FUNC_onRemoteInvitationFailure)(
    int index,
    void* remoteInvitation,
    agora::rtm::REMOTE_INVITATION_ERR_CODE errorCode);
typedef void(AGORA_CALL* FUNC_onRemoteInvitationCanceled)(
    int index,
    void* remoteInvitation);

// rtm service function pointer

typedef void(AGORA_CALL* FUNC_onLoginSuccess)(int handlerId);
typedef void(AGORA_CALL* FUNC_onLoginFailure)(int handlerId, int errorCode);
typedef void(AGORA_CALL* FUNC_onRenewTokenResult)(int handlerId,
                                                  const char* token,
                                                  int errorCode);
typedef void(AGORA_CALL* FUNC_onTokenExpired)(int handlerId);
typedef void(AGORA_CALL* FUNC_onLogout)(int handlerId, int errorCode);
typedef void(AGORA_CALL* FUNC_onUserAttributesUpdated)(int handlerId, const char* userId,
                                                        const char* attribute,
                                                        int numberOfAttributes);
typedef void(AGORA_CALL* FUNC_onSubscribeUserAttributesResult)(int handlerId, long long requestId, const char* userId, int errorCode);
typedef void(AGORA_CALL* FUNC_onUnsubscribeUserAttributesResult)(int handlerId, long long requestId, const char* userId, int errorCode);
typedef void(AGORA_CALL* FUNC_onConnectionStateChanged)(int handlerId,
                                                        int state,
                                                        int reason);
typedef void(AGORA_CALL* FUNC_onSendMessageResult)(int handlerId,
                                                   long long messageId,
                                                   int errorCode);
typedef void(AGORA_CALL* FUNC_onMessageReceivedFromPeer)(int handlerId,
                                                         const char* peerId,
                                                         void* message);
typedef void(AGORA_CALL* FUNC_onImageMessageReceivedFromPeer)(
    int handlerId,
    const char* peerId,
    void* message);
typedef void(AGORA_CALL* FUNC_onFileMessageReceivedFromPeer)(int handlerId,
                                                             const char* peerId,
                                                             void* message);
typedef void(AGORA_CALL* FUNC_onMediaUploadingProgress)(int handlerId,
                                                        long long requestId,
                                                        long long totalSize,
                                                        long long currentSize);
typedef void(AGORA_CALL* FUNC_onMediaDownloadingProgress)(
    int handlerId,
    long long requestId,
    long long totalSize,
    long long currentSize);
typedef void(AGORA_CALL* FUNC_onFileMediaUploadResult)(int handlerId,
                                                       long long requestId,
                                                       void* fileMessage,
                                                       int code);
typedef void(AGORA_CALL* FUNC_onImageMediaUploadResult)(int handlerId,
                                                        long long requestId,
                                                        void* imageMessage,
                                                        int code);
typedef void(AGORA_CALL* FUNC_onMediaDownloadToFileResult)(int handlerId,
                                                           long long requestId,
                                                           int code);
typedef void(AGORA_CALL* FUNC_onMediaDownloadToMemoryResult)(
    int handlerId,
    long long requestId,
    const char* memory,
    long long length,
    int code);
typedef void(AGORA_CALL* FUNC_onMediaCancelResult)(int handlerId,
                                                   long long requestId,
                                                   int code);
typedef void(AGORA_CALL* FUNC_onQueryPeersOnlineStatusResult)(
    int handlerId,
    long long requestId,
    void* peersStatus,
    int peerCount,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onSubscriptionRequestResult)(int handlerId,
                                                           long long requestId,
                                                           int errorCode);
typedef void(AGORA_CALL* FUNC_onQueryPeersBySubscriptionOptionResult)(
    int handlerId,
    long long requestId,
    const char* peerIds[],
    int peerCount,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onPeersOnlineStatusChanged)(
    int handlerId,
    const char* peersStatus,
    int peerCount);
typedef void(AGORA_CALL* FUNC_onSetLocalUserAttributesResult)(
    int handlerId,
    long long requestId,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onDeleteLocalUserAttributesResult)(
    int handlerId,
    long long requestId,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onClearLocalUserAttributesResult)(
    int handlerId,
    long long requestId,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onGetUserAttributesResult)(int handlerId,
                                                         long long requestId,
                                                         const char* userId,
                                                         const char* attribute,
                                                         int numberOfAttributes,
                                                         int errorCode);
typedef void(AGORA_CALL* FUNC_onSetChannelAttributesResult)(int handlerId,
                                                            long long requestId,
                                                            int errorCode);
typedef void(AGORA_CALL* FUNC_onAddOrUpdateLocalUserAttributesResult)(
    int handlerId,
    long long requestId,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onDeleteChannelAttributesResult)(
    int handlerId,
    long long requestId,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onClearChannelAttributesResult)(
    int handlerId,
    long long requestId,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onGetChannelAttributesResult)(
    int handlerId,
    long long requestId,
    const char* attributes,
    int numberOfAttributes,
    int errorCode);
typedef void(AGORA_CALL* FUNC_onGetChannelMemberCountResult)(
    int handlerId,
    long long requestId,
    void* channelMemberCounts,
    int channelCount,
    int errorCode);

typedef struct CChannelEventHandler {
  FUNC_channel_onJoinSuccess onJoinSuccess;
  FUNC_channel_onJoinFailure onJoinFailure;
  FUNC_channel_onLeave onLeave;
  FUNC_channel_onMessageReceived onMessageReceived;
  FUNC_channel_onImageMessageReceived onImageMessageReceived;
  FUNC_channel_onFileMessageReceived onFileMessageReceived;
  FUNC_channel_onSendMessageResult onSendMessageResult;
  FUNC_channel_onMemberJoined onMemberJoined;
  FUNC_channel_onMemberLeft onMemberLeft;
  FUNC_channel_onGetMembers onGetMembers;
  FUNC_channel_onMemberCountUpdated onMemberCountUpdated;
  FUNC_channel_onAttributeUpdate onAttributeUpdate;
  FUNC_channel_onLockAcquired onLockAcquired;
  FUNC_channel_onLockExpired onLockExpired;
  FUNC_channel_onLockAcquireFailed onLockAcquireFailed;
} CChannelEventHandler;

typedef struct CRtmServiceEventHandler {
  FUNC_onLoginSuccess _onLoginSuccess;
  FUNC_onLoginFailure _onLoginFailure;
  FUNC_onRenewTokenResult _onRenewTokenResult;
  FUNC_onTokenExpired _onTokenExpired;
  FUNC_onLogout _onLogout;
  FUNC_onUserAttributesUpdated _onUserAttributesUpdated;
  FUNC_onSubscribeUserAttributesResult _onSubscribeUserAttributesResult;
  FUNC_onUnsubscribeUserAttributesResult _onUnsubscribeUserAttributesResult;
  FUNC_onConnectionStateChanged _onConnectionStateChanged;
  FUNC_onSendMessageResult _onSendMessageResult;
  FUNC_onMessageReceivedFromPeer _onMessageReceivedFromPeer;
  FUNC_onImageMessageReceivedFromPeer _onImageMessageReceivedFromPeer;
  FUNC_onFileMessageReceivedFromPeer _onFileMessageReceivedFromPeer;
  FUNC_onMediaUploadingProgress _onMediaUploadingProgress;
  FUNC_onMediaDownloadingProgress _onMediaDownloadingProgress;
  FUNC_onFileMediaUploadResult _onFileMediaUploadResult;
  FUNC_onImageMediaUploadResult _onImageMediaUploadResult;
  FUNC_onMediaDownloadToFileResult _onMediaDownloadToFileResult;
  FUNC_onMediaDownloadToMemoryResult _onMediaDownloadToMemoryResult;
  FUNC_onMediaCancelResult _onMediaCancelResult;
  FUNC_onQueryPeersOnlineStatusResult _onQueryPeersOnlineStatusResult;
  FUNC_onSubscriptionRequestResult _onSubscriptionRequestResult;
  FUNC_onQueryPeersBySubscriptionOptionResult
      _onQueryPeersBySubscriptionOptionResult;
  FUNC_onPeersOnlineStatusChanged _onPeersOnlineStatusChanged;
  FUNC_onSetLocalUserAttributesResult _onSetLocalUserAttributesResult;
  FUNC_onDeleteLocalUserAttributesResult _onDeleteLocalUserAttributesResult;
  FUNC_onClearLocalUserAttributesResult _onClearLocalUserAttributesResult;
  FUNC_onGetUserAttributesResult _onGetUserAttributesResult;
  FUNC_onSetChannelAttributesResult _onSetChannelAttributesResult;
  FUNC_onAddOrUpdateLocalUserAttributesResult
      _onAddOrUpdateLocalUserAttributesResult;
  FUNC_onDeleteChannelAttributesResult _onDeleteChannelAttributesResult;
  FUNC_onClearChannelAttributesResult _onClearChannelAttributesResult;
  FUNC_onGetChannelAttributesResult _onGetChannelAttributesResult;
  FUNC_onGetChannelMemberCountResult _onGetChannelMemberCountResult;
} CRtmServiceEventHandler;

typedef struct CRtmCallEventHandler {
  FUNC_onLocalInvitationReceivedByPeer _onLocalInvitationReceivedByPeer;
  FUNC_onLocalInvitationCanceled _onLocalInvitationCanceled;
  FUNC_onLocalInvitationFailure _onLocalInvitationFailure;
  FUNC_onLocalInvitationAccepted _onLocalInvitationAccepted;
  FUNC_onLocalInvitationRefused _onLocalInvitationRefused;
  FUNC_onRemoteInvitationRefused _onRemoteInvitationRefused;
  FUNC_onRemoteInvitationAccepted _onRemoteInvitationAccepted;
  FUNC_onRemoteInvitationReceived _onRemoteInvitationReceived;
  FUNC_onRemoteInvitationFailure _onRemoteInvitationFailure;
  FUNC_onRemoteInvitationCanceled _onRemoteInvitationCanceled;
} CRtmCallEventHandler;