//
//  i_rtm_service_event_handler.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/12.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once

#include "RtmServiceEventHandler.h"
#include "common.h"

AGORA_API void* service_event_handler_createEventHandle(
    int _id,
    agora::unity::FUNC_onLoginSuccess _onLoginSuccess,
    agora::unity::FUNC_onLoginFailure _onLoginFailure,
    agora::unity::FUNC_onRenewTokenResult _onRenewTokenResult,
    agora::unity::FUNC_onTokenExpired _onTokenExpired,
    agora::unity::FUNC_onLogout _onLogout,
    agora::unity::FUNC_onConnectionStateChanged _onConnectionStateChanged,
    agora::unity::FUNC_onSendMessageResult _onSendMessageResult,
    agora::unity::FUNC_onMessageReceivedFromPeer _onMessageReceivedFromPeer,
    agora::unity::FUNC_onImageMessageReceivedFromPeer
        _onImageMessageReceivedFromPeer,
    agora::unity::FUNC_onFileMessageReceivedFromPeer
        _onFileMessageReceivedFromPeer,
    agora::unity::FUNC_onMediaUploadingProgress _onMediaUploadingProgress,
    agora::unity::FUNC_onMediaDownloadingProgress _onMediaDownloadingProgress,
    agora::unity::FUNC_onFileMediaUploadResult _onFileMediaUploadResult,
    agora::unity::FUNC_onImageMediaUploadResult _onImageMediaUploadResult,
    agora::unity::FUNC_onMediaDownloadToFileResult _onMediaDownloadToFileResult,
    agora::unity::FUNC_onMediaDownloadToMemoryResult
        _onMediaDownloadToMemoryResult,
    agora::unity::FUNC_onMediaCancelResult _onMediaCancelResult,
    agora::unity::FUNC_onQueryPeersOnlineStatusResult
        _onQueryPeersOnlineStatusResult,
    agora::unity::FUNC_onSubscriptionRequestResult _onSubscriptionRequestResult,
    agora::unity::FUNC_onQueryPeersBySubscriptionOptionResult
        _onQueryPeersBySubscriptionOptionResult,
    agora::unity::FUNC_onPeersOnlineStatusChanged _onPeersOnlineStatusChanged,
    agora::unity::FUNC_onSetLocalUserAttributesResult
        _onSetLocalUserAttributesResult,
    agora::unity::FUNC_onDeleteLocalUserAttributesResult
        _onDeleteLocalUserAttributesResult,
    agora::unity::FUNC_onClearLocalUserAttributesResult
        _onClearLocalUserAttributesResult,
    agora::unity::FUNC_onGetUserAttributesResult _onGetUserAttributesResult,
    agora::unity::FUNC_onSetChannelAttributesResult
        _onSetChannelAttributesResult,
    agora::unity::FUNC_onAddOrUpdateLocalUserAttributesResult
        _onAddOrUpdateLocalUserAttributesResult,
    agora::unity::FUNC_onDeleteChannelAttributesResult
        _onDeleteChannelAttributesResult,
    agora::unity::FUNC_onClearChannelAttributesResult
        _onClearChannelAttributesResult,
    agora::unity::FUNC_onGetChannelAttributesResult
        _onGetChannelAttributesResult,
    agora::unity::FUNC_onGetChannelMemberCountResult
        _onGetChannelMemberCountResult);

AGORA_API void service_event_handler_releaseEventHandler(
    void* channelEventHandlerInstance);
