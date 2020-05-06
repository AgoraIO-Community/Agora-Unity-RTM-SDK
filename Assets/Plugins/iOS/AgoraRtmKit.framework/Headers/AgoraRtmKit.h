//
//  AgoraRtmKit.h
//  AgoraRtmKit
//
//  Copyright (c) 2019 Agora.io. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "AgoraRtmCallKit.h"

/**
 The `AgoraRtmKit` class is the entry point of the Agora RTM SDK.
 */
@class AgoraRtmKit;
/**
 The AgoraRtmMessage class provides Agora RTM message methods that can be invoked by your app.
 */
@class AgoraRtmMessage;
/**
 The AgoraRtmChannel class provides Agora RTM channel methods that can be invoked by your app.
 */
@class AgoraRtmChannel;
/**
 The AgoraRtmCallKit class provides Agora RTM call methods that can be invoked by your app.
 */
@class AgoraRtmCallKit;

/**
 AgoraRtmPeerOnlineStatus provides the online status of a specified user.
 */
@class AgoraRtmPeerOnlineStatus;

/**
 The prefix for ending a call. You can use it with the [sendMessage]([AgoraRtmKit sendMessage:toPeer:sendMessageOptions:completion:]) method to be compatible with the endCall method of the legacy Agora Signaling SDK.
 */
static NSString *const AgoraRtmEndcallPrefix = @"AgoraRTMLegacyEndcallCompatibleMessagePrefix";

/**
Message types.
 */
typedef NS_ENUM(NSInteger, AgoraRtmMessageType) {
    
  /**
0: Undefined message type.
   */
  AgoraRtmMessageTypeUndefined = 0,
    
  /**
1: A text message.
   */
  AgoraRtmMessageTypeText = 1,
    
  /**
2: A raw message.
   */
  AgoraRtmMessageTypeRaw = 2,
};

/**
 Error codes related to sending a peer-to-peer message.
 */
typedef NS_ENUM(NSInteger, AgoraRtmSendPeerMessageErrorCode) {
    
    /**
0: The specified user receives the peer-to-peer message.
     */
    AgoraRtmSendPeerMessageErrorOk = 0,
    
    /**
1: The user fails to send the peer-to-peer message.
     */
    AgoraRtmSendPeerMessageErrorFailure = 1,
    
    /**
2: A timeout occurs when sending the peer-to-peer message. The current timeout is set as 10 seconds. Possible reasons: The user is in the `AgoraRtmConnectionStateAborted` or `AgoraRtmConnectionStateReconnecting` state.
     */
    AgoraRtmSendPeerMessageErrorTimeout = 2,
    
    /**
3: The user is offline and has not received the peer-to-peer message.
     */
    AgoraRtmSendPeerMessageErrorPeerUnreachable = 3,
    
    /**
4: The specified user is offline and does not receive the peer-to-peer message, but the server has cached the message and will send the message to the specified user when he/she is back online.
     */
    AgoraRtmSendPeerMessageErrorCachedByServer  = 4,
    
    /**
5: The method call frequency exceeds the limit of 60 queries per second (channel and peer messages taken together).
     */
    AgoraRtmSendPeerMessageErrorTooOften = 5,
    
    /**
6: The user ID is invalid.
     */
    AgoraRtmSendPeerMessageErrorInvalidUserId = 6,
    
    /**
7: The message is null or exceeds 32 KB in length.
     */
    AgoraRtmSendPeerMessageErrorInvalidMessage = 7,

    /**
101: The SDK is not initialized.
     */
    AgoraRtmSendPeerMessageErrorNotInitialized = 101,
    
    /**
102: The sender does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before sending the peer-to-peer message.
     */
    AgoraRtmSendPeerMessageErrorNotLoggedIn = 102,
};

/**
Connection states between the SDK and the Agora RTM system.
 */
typedef NS_ENUM(NSInteger, AgoraRtmConnectionState) {
    
/**
1: The initial state. The SDK is disconnected from the Agora RTM system.
<p>When the user calls the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, the SDK starts to log in the Agora RTM system, triggers the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback, and switches to the `AgoraRtmConnectionStateConnecting` state.
 */
    AgoraRtmConnectionStateDisconnected = 1,
    
/**
2: The SDK is logging in the Agora RTM system.
<p><li>Success: The SDK triggers the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback and switches to the `AgoraRtmConnectionStateConnected` state.
<li>Failure: The SDK triggers the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback and switches to the `AgoraRtmConnectionStateDisConnected` state.
*/
    AgoraRtmConnectionStateConnecting = 2,
    
/**
3: The SDK has logged in the Agora RTM system.
<p><li>If the connection between the SDK and the Agora RTM system is interrupted because of network issues, the SDK triggers the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback and switches to the `AgoraRtmConnectionStateReconnecting` state.
<li>If the login is banned by the server because, for example, another instance logs in the Agora RTM system with the same user ID, the SDK triggers the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback and switches to the `AgoraRtmConnectionStateAborted` state.
<li>If the user calls the [logoutWithCompletion]([AgoraRtmKit logoutWithCompletion:]) method to log out of the Agora RTM system and receives `AgoraRtmLogoutErrorOk`, the SDK triggers the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback and switches to the `AgoraRtmConnectionStateDisconnected` state.
*/
    AgoraRtmConnectionStateConnected = 3,

/**
4: The connection state between the SDK and the Agora RTM system is interrupted due to network issues, and the SDK keeps re-logging in the Agora RTM system.
<p><li>If the SDK successfully re-logs in the Agora RTM system, it triggers the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback and switches to the `AgoraRtmConnectionStateConnected` state. The SDK automatically adds the user back to the channel(s) he or she was in when the connection was interrupted, and synchronizes the local user's attributes with the server. 
<li>If the SDK fails to re-log in the Agora RTM system, the SDK stays in the `AgoraRtmConnectionStateReconnecting` state and keeps re-logging in the system.
*/
    AgoraRtmConnectionStateReconnecting = 4,
    
/**
5: The SDK gives up logging in the Agora RTM system, mainly because another instance has logged in the Agora RTM system with the same user ID.
<p>You must call the [logoutWithCompletion]([AgoraRtmKit logoutWithCompletion:]) method before calling the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method to log in the Agora RTM system again.
*/
    AgoraRtmConnectionStateAborted = 5,
};

/**
Reasons for a connection state change.
 */
typedef NS_ENUM(NSInteger, AgoraRtmConnectionChangeReason) {
    
    /**
1: The SDK is logging in the Agora RTM system.
     */
    AgoraRtmConnectionChangeReasonLogin = 1,
    
    /**
2: The SDK has logged in the Agora RTM system.
     */
    AgoraRtmConnectionChangeReasonLoginSuccess = 2,
    
    /**
3: The SDK fails to log in the Agora RTM system.
     */
    AgoraRtmConnectionChangeReasonLoginFailure = 3,
    
    /**
4: The login has timed out for 10 seconds, and the SDK stops logging in.
     */
    AgoraRtmConnectionChangeReasonLoginTimeout = 4,
    
    /**
5: The connection between the SDK and the Agora RTM system is interrupted for more than four seconds.
     */
    AgoraRtmConnectionChangeReasonInterrupted = 5,
    
    /**
6: The user has called the [logoutWithCompletion]([AgoraRtmKit logoutWithCompletion:]) method to log out of the Agora RTM system.
     */
    AgoraRtmConnectionChangeReasonLogout = 6,
    
    /**
7: Login is banned by the Agora RTM server.
     */
    AgoraRtmConnectionChangeReasonBannedByServer = 7,
    
    /**
8: Another instance has logged in the Agora RTM system with the same user ID.
     */
    AgoraRtmConnectionChangeReasonRemoteLogin = 8,
};

/**
Error codes related to login.
 */
typedef NS_ENUM(NSInteger, AgoraRtmLoginErrorCode) {
    
    /**
0: Login succeeds. No error occurs.
     */
    AgoraRtmLoginErrorOk = 0,
    
    /**
1: Login fails for reasons unknown.
     */
    AgoraRtmLoginErrorUnknown = 1,
    
    /**
2: Login is rejected, possibly because the SDK is not initialized or is rejected by the server.
     */
    AgoraRtmLoginErrorRejected = 2,
    
    /**
3: Invalid login arguments.
     */
    AgoraRtmLoginErrorInvalidArgument = 3,
    
    /**
4: The App ID is invalid.
     */
    AgoraRtmLoginErrorInvalidAppId = 4,
    
    /**
5: The token is invalid.
     */
    AgoraRtmLoginErrorInvalidToken = 5,
    
    /**
6: The token has expired, and hence login is rejected.
     */
    AgoraRtmLoginErrorTokenExpired = 6,
    
    /**
7: Unauthorized login.
     */
    AgoraRtmLoginErrorNotAuthorized = 7,
    
    /**
8: The user has already logged in or is logging in the Agora RTM system, or the user has not called the [logoutWithCompletion]([AgoraRtmKit logoutWithCompletion:]) method to leave the `AgoraRtmConnectionStateAborted` state.
     */
    AgoraRtmLoginErrorAlreadyLogin = 8,
    
    /**
9: The login times out. The current timeout is set as six seconds. 
     */
    AgoraRtmLoginErrorTimeout = 9,
    
    /**
10: The call frequency of the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method exceeds the limit of two queries per second.
     */
    AgoraRtmLoginErrorLoginTooOften = 10,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmLoginErrorLoginNotInitialized = 101,
};

/**
Error codes related to logout.
 */
typedef NS_ENUM(NSInteger, AgoraRtmLogoutErrorCode) {
    
  /**
0: Logout succeeds. No error occurs.
   */
  AgoraRtmLogoutErrorOk = 0,
    
  /**
1: Logout fails.
   */
  AgoraRtmLogoutErrorRejected = 1,
    
  /**
101: The SDK is not initialized.
   */
  AgoraRtmLogoutErrorNotInitialized = 101,
    
  /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before the user logs out of the Agora RTM system.
   */
  AgoraRtmLogoutErrorNotLoggedIn = 102,
};

/**
Error codes related to querying the online status of the specified peer(s).
 */
typedef NS_ENUM(NSInteger, AgoraRtmQueryPeersOnlineErrorCode) {
    
    /**
0: The method call succeeds.
     */
    AgoraRtmQueryPeersOnlineErrorOk = 0,
    
    /**
1: The method call fails.
     */
    AgoraRtmQueryPeersOnlineErrorFailure = 1,
    
    /**
2: The method call fails. The argument is invalid.
     */
    AgoraRtmQueryPeersOnlineErrorInvalidArgument = 2,

    /**
3: The method call fails. The peer user has not logged in the Agora RTM system.
     */
    AgoraRtmQueryPeersOnlineErrorRejected = 3,
    
    /**
4: The SDK has not received a response from the server for 10 seconds. The current timeout is set as 10 seconds. Possible reasons: The user is in the `AgoraRtmConnectionStateAborted` or `AgoraRtmConnectionStateReconnecting` state.
     */
    AgoraRtmQueryPeersOnlineErrorTimeout = 4,
    
    /**
5: The method call frequency of this method exceeds the limit of 10 queries every five seconds.
     */
    AgoraRtmQueryPeersOnlineErrorTooOften = 5,

    /**
101: The SDK is not initialized.
     */
    AgoraRtmQueryPeersOnlineErrorNotInitialized = 101,

    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before querying the online status.
     */
    AgoraRtmQueryPeersOnlineErrorNotLoggedIn = 102,
};

/**
Error codes related to attrubute operations.
 */
typedef NS_ENUM(NSInteger, AgoraRtmProcessAttributeErrorCode) {
    
    /**
0: The attribute operation succeeds.
     */
    AgoraRtmAttributeOperationErrorOk = 0,
    
    /**
1: **DEPRECATED**
     */
    AgoraRtmAttributeOperationErrorNotReady = 1,
    
    /**
2: The attribute operation fails.
     */
    AgoraRtmAttributeOperationErrorFailure = 2,
    
    /**
3: The argument you put for this attribute operation is invalid.
     */
    AgoraRtmAttributeOperationErrorInvalidArgument = 3,
    
    /**
4: The attribute size exceeds the limit. <p><li> For user attribute operations: The user's overall attribute size would exceed the limit of 16 KB, one of the user's attributes would exceeds 8 KB in size, or the number of this user's attributes would exceed 32 after this attribute operation.<li> For channel attribute operations: The channel's overall attribute size would exceed the limit of 32 KB, one of the channel attributes would exceed 8 KB in size, or the number of this channel's attributes would exceed 32 after this attribute operation.
     */
    AgoraRtmAttributeOperationErrorSizeOverflow = 4,
    
    /**
5: The method call frequency exceeds the limit.<p><li>For [setLocalUserAttributes]([AgoraRtmKit setLocalUserAttributes:completion:]), [addOrUpdateLocalUserAttributes]([AgoraRtmKit addOrUpdateLocalUserAttributes:completion:]), [deleteLocalUserAttributesByKeys]([AgoraRtmKit deleteLocalUserAttributesByKeys:completion:]) and [clearLocalUserAttributes]([AgoraRtmKit clearLocalUserAttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.<li>For [getUserAttributes]([AgoraRtmKit getUserAllAttributes:completion:]) and [getUserAttributesByKeys]([AgoraRtmKit getUserAttributes:ByKeys:completion:]) taken together, the limit is 40 queries every five seconds.<li>For [setChannelAttributes]([AgoraRtmKit setChannel:Attributes:Options:completion:]), [addOrUpdateChannelAttributes]([AgoraRtmKit addOrUpdateChannel:Attributes:Options:completion:]), [deleteChannelAttributesByKeys]([AgoraRtmKit deleteChannel:AttributesByKeys:Options:completion:]) and [clearChannelAttributes]([AgoraRtmKit clearChannel:Options:AttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.<li>For [getChannelAllAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) and [getChannelAttributesByKeys]([AgoraRtmKit getChannelAttributes:ByKeys:completion:]) taken together, the limit is 10 queries every five seconds.
     */
    AgoraRtmAttributeOperationErrorTooOften = 5,
    
    /**
6: The specified user is not found, either because the user is offline or because the user does not exist.
     */
    AgoraRtmAttributeOperationErrorUserNotFound = 6,
    
    /**
7: A timeout occurs in the attribute-related operation. The current timeout is set as five seconds. Possible reasons: The user is in the `AgoraRtmConnectionStateAborted` or `AgoraRtmConnectionStateReconnecting` state.
     */
    AgoraRtmAttributeOperationErrorTimeout = 7,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmAttributeOperationErrorNotInitialized = 101,
    
    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before the attribute operation.
     */
    AgoraRtmAttributeOperationErrorNotLoggedIn = 102,
};

/**
Error codes related to renewing the token.
 */
typedef NS_ENUM(NSInteger, AgoraRtmRenewTokenErrorCode) {
    
  /**
0: The token-renewing operation succeeds.
   */
  AgoraRtmRenewTokenErrorOk = 0,
    
  /**
1: Common unknown failure.
   */
  AgoraRtmRenewTokenErrorFailure = 1,
    
  /**
2: The method call fails. The argument is invalid.
   */
  AgoraRtmRenewTokenErrorInvalidArgument = 2,
    
  /**
3: The RTM service is not initialized. You are rejected.
   */
  AgoraRtmRenewTokenErrorRejected = 3,
    
  /**
4: The method call frequency of exceeds the limit of two queries per second.
   */
  AgoraRtmRenewTokenErrorTooOften = 4,

  /**
5: The token has expired.
   */
  AgoraRtmRenewTokenErrorTokenExpired = 5,
    
  /**
6: The token is invalid.
   */
  AgoraRtmRenewTokenErrorInvalidToken = 6,
    
  /**
101: The SDK is not initialized.
   */
  AgoraRtmRenewTokenErrorNotInitialized = 101,
    
  /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before renewing the token.
   */
  AgoraRtmRenewTokenErrorNotLoggedIn = 102,
};

/**
Error codes related to retrieving the channel member count of specified channel(s).
 */
typedef NS_ENUM(NSInteger, AgoraRtmChannelMemberCountErrorCode) {
    
    /**
0: The operation succeeds.
     */
    AgoraRtmChannelMemberCountErrorOk = 0,
    
    /**
1: Unknown common failure.
     */
    AgoraRtmChannelMemberCountErrorFailure = 1,
    
    /**
2: One or several of your channel IDs is invalid.
     */
    AgoraRtmChannelMemberCountErrorInvalidArgument = 2,

    /**
3: The method call frequency exceeds the limit of one query per second.
     */
    AgoraRtmChannelMemberCountErrorTooOften = 3,

    /**
 4: A timeout occurs during this operation. The current timeout is set as five seconds.
     */
    AgoraRtmChannelMemberCountErrorTimeout = 4,
    
    /**
5: The number of the channels that you query is greater than 32.
     */
    AgoraRtmChannelMemberCountErrorExceedLimit = 5,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmChannelMemberCountErrorNotInitialized = 101,

    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before this operation.
     */
    AgoraRtmChannelMemberCountErrorNotLoggedIn = 102,
};

/**
Error codes related to subscribing to or unsubscribing from the status of specified peer(s).
 */
typedef NS_ENUM(NSInteger, AgoraRtmPeerSubscriptionStatusErrorCode) {
    
    /**
0: The method call succeeds, or the operation succeeds.
     */
    AgoraRtmPeerSubscriptionStatusErrorOk = 0,
    
    /**
1: Common failure. The user fails to subscribe to or unsubscribe from the status of the specified peer(s).
     */
    AgoraRtmPeerSubscriptionStatusErrorFailure = 1,
    
    /**
2: The method call fails. The argument is invalid.
     */
    AgoraRtmPeerSubscriptionStatusErrorInvalidArgument = 2,
    
    /**
3: **RESERVED FOR FUTURE USE**
     */
    AgoraRtmPeerSubscriptionStatusErrorRejected = 3,
    
    /**
4: The SDK fails to receive a response from the server in 10 seconds. The current timeout is set as 10 seconds. Possible reasons: The user is in the \ref agora::rtm::CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" or \ref agora::rtm::CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" state.
     */
    AgoraRtmPeerSubscriptionStatusErrorTimeout = 4,
    
    /**
5: The method call frequency exceeds the limit of 10 subscribes every five seconds.
     */
    AgoraRtmPeerSubscriptionStatusErrorTooOften = 5,
    
    /**
6: The number of peers, to whom you subscribe, exceeds the limit of 512.
     */
    PEER_SUBSCRIPTION_STATUS_ERR_OVERFLOW = 6,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmPeerSubscriptionStatusErrorNotInitialized = 101,
    
    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before this operation.
     */
    AgoraRtmPeerSubscriptionStatusErrorNotLoggedIn = 102,
};

/**
Error codes related to getting a list of the peer(s) by suscription option type.
 */
typedef NS_ENUM(NSInteger, AgoraRtmQueryPeersBySubscriptionOptionErrorCode) {
    
    /**
0: The method call succeeds, or the operation succeeds.
     */
    AgoraRtmQueryPeersBySubscriptionOptionErrorOk = 0,
    
    /**
1: Common failure. The user fails to query peer(s) by subscription option type.
     */
    AgoraRtmQueryPeersBySubscriptionOptionErrorFailure = 1,
    
    /**
2: The SDK fails to receive a response from the server in 5 seconds. The current timeout is set as 5 seconds. Possible reasons: The user is in the `AgoraRtmConnectionStateAborted` or `AgoraRtmConnectionStateReconnecting` state.
     */
    AgoraRtmQueryPeersBySubscriptionOptionErrorTimeout = 2,
    
    /**
3: The method call frequency exceeds the limit of 10 subscribes every five seconds.
     */
    AgoraRtmQueryPeersBySubscriptionOptionErrorTooOften = 3,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmQueryPeersBySubscriptionOptionErrorNotInitialized = 101,
    
    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before this operation.
     */
    AgoraRtmQueryPeersBySubscriptionOptionErrorNotLoggedIn = 102,
};


/**
The online states of a peer.
 */
typedef NS_ENUM(NSInteger, AgoraRtmPeerOnlineState) {
    /**
0: The peer is online.
     */
    AgoraRtmPeerOnlineStateOnline = 0,
    /**
1: The peer is temporarily unreachable (the server has not received a packet from the SDK for more than six seconds).
     */
    AgoraRtmPeerOnlineStateUnreachable = 1,
    /**
2: The peer is offline (the sdk has not logged in the Agora RTM system, or it has logged out of the system, or the server has not received a packet from the SDK for more than 30 seconds).
     */
    AgoraRtmPeerOnlineStateOffline = 2,
};

/**
Subscription types.
 */
typedef NS_ENUM(NSInteger, AgoraRtmPeerSubscriptionOptions) {
    /**
0: Takes out a subscription to the online status of specified user(s).
     */
    AgoraRtmPeerSubscriptionOnlineStatus = 0,
};

/**
 Returns the result of the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method call. <p><i>errorCode:<i> Login error code. See AgoraRtmLoginErrorCode.
 */
typedef void (^AgoraRtmLoginBlock)(AgoraRtmLoginErrorCode errorCode);

/**
 Indicates the results of calling the [logoutWithCompletion]([AgoraRtmKit logoutWithCompletion:]) method call. <p><i>errorCode:<i> Logout error code. See AgoraRtmLogoutErrorCode.
 */
typedef void (^AgoraRtmLogoutBlock)(AgoraRtmLogoutErrorCode errorCode);

/**
 Returns the result of the [sendMessage]([AgoraRtmKit sendMessage:toPeer:completion:]) method call. <p><i>errorCode:<i> Error code of sending the peer-to-peer message. See AgoraRtmSendPeerMessageErrorCode.
 */
typedef void (^AgoraRtmSendPeerMessageBlock)(AgoraRtmSendPeerMessageErrorCode errorCode);

/**
 Returns the result of the [queryPeersOnlineStatus]([AgoraRtmKit queryPeersOnlineStatus:completion:]) method call. <p><li><i>peerOnlineStatus:</i> A list of the specified users' online status. See AgoraRtmPeerOnlineStatus. <li><i>errorCode:</i> See AgoraRtmQueryPeersOnlineErrorCode.
 */
typedef void (^AgoraRtmQueryPeersOnlineBlock)(NSArray<AgoraRtmPeerOnlineStatus *> * peerOnlineStatus, AgoraRtmQueryPeersOnlineErrorCode errorCode);

/**
 Returns the result of the [renewToken]([AgoraRtmKit renewToken:completion:]) method call. <p><li><i>token</i> Your new Token. <li><i>errorCode:</i> See AgoraRtmRenewTokenErrorCode.
 */
typedef void (^AgoraRtmRenewTokenBlock)(NSString *token, AgoraRtmRenewTokenErrorCode errorCode);

/**
 Returns the result of the [subscribePeersOnlineStatus]([AgoraRtmKit subscribePeersOnlineStatus:completion:]) or [unsubscribePeersOnlineStatus]([AgoraRtmKit unsubscribePeersOnlineStatus:completion:]) method call. <p><i>errorCode:</i> See AgoraRtmPeerSubscriptionStatusErrorCode.
 */
typedef void (^AgoraRtmSubscriptionRequestBlock)(AgoraRtmPeerSubscriptionStatusErrorCode errorCode);

/**
 Returns the result of the [queryPeersBySubscriptionOption]([AgoraRtmKit queryPeersBySubscriptionOption:completion:]) method call. <p><i>errorCode:</i> See AgoraRtmQueryPeersBySubscriptionOptionErrorCode.
 */
typedef void (^AgoraRtmQueryPeersBySubscriptionOptionBlock)(NSArray<NSString *> * peers, AgoraRtmQueryPeersBySubscriptionOptionErrorCode errorCode);

/**
Attributes of a peer-to-peer or channel text message.
 */
__attribute__((visibility("default"))) @interface AgoraRtmMessage: NSObject

/**
 Agora RTM message type. See AgoraRtmMessageType. Text messages only.
 */
@property (nonatomic, assign, readonly) AgoraRtmMessageType type;

/**
 Agora RTM message content. Must not exceed 32 KB in length.
 */
@property (nonatomic, copy, nonnull) NSString *text;

/**
 The timestamp (ms) of when the messaging server receives this message.
 
 **Note**
 
 The returned timestamp is on a millisecond time-scale. It is for demonstration purposes only, not for strict ordering of messages.
 */
@property (nonatomic, assign, readonly) long long serverReceivedTs;

/**
 Whether this message has been cached on the server (Applies to peer-to-peer message only).
 
 - YES: This message has been cached on the server (the server caches this message and resends it to the receiver when he/she is back online).
 - NO: (Default) This message has not been cached on the server.
 
 **Note**
 
 <li> This method returns NO if a message is not cached by the server. Only if the sender sends the message as an offline message (sets [enableOfflineMessaging]([AgoraRtmSendMessageOptions enableOfflineMessaging]) as YES) when the specified user is offline, does the method return YES when the user is back online. <li> For now we only cache 200 offline messages for up to seven days for each message receiver. When the number of the cached messages reaches this limit, the newest message overrides the oldest one.
 */
@property (nonatomic, assign, readonly) BOOL isOfflineMessage;

/**
 Creates and initializes a text message to be sent.

 @param text A text message of less than 32 KB.

 @return An AgoraRtmMessage instance.

 */
- (instancetype _Nonnull)initWithText:(NSString * _Nonnull)text;
@end

/**
 Attributes of a peer-to-peer or channel raw message. Inherited from AgoraRtmMessage.
 */
__attribute__((visibility("default"))) @interface AgoraRtmRawMessage: AgoraRtmMessage

/**
 Agora RTM raw message content. Must not exceed 32 KB in length.
 */
@property (nonatomic, nonnull) NSData *rawData;

/**
 Creates and initializes a raw message to be sent.

 @param rawData A raw message of less than 32 KB.
 @param description A brief text description of the raw message. If you set a text description, ensure that the size of the raw message and the description combined does not exceed 32 KB.
 @return An AgoraRtmMessage instance.
 */
- (instancetype _Nonnull)initWithRawData:(NSData * _Nonnull)rawData description:(NSString * _Nonnull)description;
@end

/**
 Data structure indicating the online status of a user.
 */
__attribute__((visibility("default"))) @interface AgoraRtmPeerOnlineStatus: NSObject

/**
 The user ID of the specified user.
 */
@property (nonatomic, copy, nonnull) NSString *peerId;

/**
 The online status of the peer. **DEPRECATED** as of v1.2.0. Use `state` instead.

 - YES: The user is online (the user has logged in the Agora RTM system).
 - NO: The user is offline (the user has logged out of the Agora RTM system, has not logged in, or has failed to logged in).
*/
@property (nonatomic, assign) BOOL isOnline;

/**
 The online state of the peer. See AgoraRtmPeerOnlineState.
 
 **NOTE**
 
 - The server will never return the `unreachable` state, if you <i>query</i> the online status of specified peer(s) ([queryPeersBySubscriptionOption]([AgoraRtmKit queryPeersBySubscriptionOption:completion:])). See also: [AgoraRtmSubscriptionRequestBlock](AgoraRtmSubscriptionRequestBlock).
 - The server may return the `unreachable` state, if you <i>subscribe to</i> the online status of specified peer(s) ([subscribePeersOnlineStatus]([AgoraRtmKit subscribePeersOnlineStatus:completion:])). See also: [PeersOnlineStatusChanged]([AgoraRtmDelegate rtmKit:PeersOnlineStatusChanged:]).
 */
@property (nonatomic, assign, readonly) AgoraRtmPeerOnlineState state;
@end

/**
 Data structure indicating the member count of a channel.
 */
__attribute__((visibility("default"))) @interface AgoraRtmChannelMemberCount: NSObject

/**
 The ID of the channel.
 */
@property (nonatomic, copy, nonnull) NSString *channelId;

/**
 The current member count of the channel.
 
 **NOTE** `count` is 0 if a channel with the specified `channelId` is not found.
 */
@property (nonatomic, assign) int count;
@end

/**
 The AgoraRtmChannelDelegate protocol enables Agora RTM channel callback event notifications to your app.
 */
@protocol AgoraRtmChannelDelegate;

/**
 The AgoraRtmCallDelegate protocol enables Agora RTM call callback event notifications to your app.
 */
@protocol AgoraRtmCallDelegate ;

/**
 The AgoraRtmDelegate protocol enables Agora RTM callback event notifications to your app.
 */
@protocol AgoraRtmDelegate <NSObject>
@optional

/**
 Occurs when the connection state between the SDK and the Agora RTM system changes.

 @param kit An [AgoraRtmKit](AgoraRtmKit) instance.
 @param state The new connection state. See AgoraRtmConnectionState.
 @param reason The reason for the connection state change. See AgoraRtmConnectionChangeReason.

 */
- (void)rtmKit:(AgoraRtmKit * _Nonnull)kit connectionStateChanged:(AgoraRtmConnectionState)state reason:(AgoraRtmConnectionChangeReason)reason;

/**
 Occurs when the local user receives a peer-to-peer message.

 @param kit An [AgoraRtmKit](AgoraRtmKit) instance.
 @param message The received message.

 **NOTE** Ensure that you check the `type` property when receiving the message instance: If the message type is `AgoraRtmMessageTypeRaw`, you need to downcast the received instance from AgoraRtmMessage to AgoraRtmRawMessage. See AgoraRtmMessageType.
 
 @param peerId The user ID of the sender.
 */
- (void)rtmKit:(AgoraRtmKit * _Nonnull)kit messageReceived:(AgoraRtmMessage * _Nonnull)message fromPeer:(NSString * _Nonnull)peerId;

/**
 Occurs when the online status of the peers, to whom you subscribe, changes.
 
 - When the subscription to the online status of specified peer(s) succeeds, the SDK returns this callback to report the online status of peers, to whom you subscribe.
 - When the online status of the peers, to whom you subscribe, changes, the SDK returns this callback to report whose online status has changed.
 - If the online status of the peers, to whom you subscribe, changes when the SDK is reconnecting to the server, the SDK returns this callback to report whose online status has changed when successfully reconnecting to the server.
 
 @param kit An [AgoraRtmKit](AgoraRtmKit) instance.
 @param onlineStatus An array of peers' online states. See AgoraRtmPeerOnlineStatus.
 */
- (void)rtmKit:(AgoraRtmKit * _Nonnull)kit PeersOnlineStatusChanged:(NSArray< AgoraRtmPeerOnlineStatus *> * _Nonnull)onlineStatus;

/**
 Occurs when the current RTM Token exceeds the 24-hour validity period.
 
 This callback occurs when the current RTM Token exceeds the 24-hour validity period and reminds the user to renew it. When receiving this callback, generate a new RTM Token on the server and call the [renewToken]([AgoraRtmKit renewToken:completion:]) method to pass the new Token on to the server.

 @param kit An AgoraRtmKit instance.
 */
- (void)rtmKitTokenDidExpire:(AgoraRtmKit * _Nonnull)kit;
@end

/**
Log Filter types.
 */
typedef NS_ENUM(NSInteger, AgoraRtmLogFilter) {
    
    /**
0: Do not output any log information.
     */
    AgoraRtmLogFilterOff = 0,
    
    /**
0x000f: Output CRITICAL, ERROR, WARNING, and INFO level log information.
     */
    AgoraRtmLogFilterInfo = 0x000f,
    
    /**
0x000e: Output CRITICAL, ERROR, and WARNING level log information.
     */
    AgoraRtmLogFilterWarn = 0x000e,
    
    /**
0x000c: Output CRITICAL and ERROR level log information.
     */
    AgoraRtmLogFilterError = 0x000c,
    
    /**
0x0008: Output CRITICAL level log information.
     */
    AgoraRtmLogFilterCritical = 0x0008,
    
    /**
0x80f: RESERVED FOR FUTURE USE
     */
    AgoraRtmLogFilterMask = 0x80f,
};

/** 
 A data structure holding an RTM attribute and its value.
 */
__attribute__((visibility("default"))) @interface AgoraRtmAttribute: NSObject

/**
 The attribute name. Must be visible characters and not exceed 32 Bytes in length.
 */
@property (nonatomic, copy, nonnull) NSString *key;

/**
 The attribute value. Must not exceed 8 KB in length.
 */
@property (nonatomic, copy, nonnull) NSString *value;

@end

/**
 A data structure holding an RTM channel attribute and its value.
 */
__attribute__((visibility("default"))) @interface AgoraRtmChannelAttribute: NSObject

/**
 The key of channel attribute. Must be visible characters and not exceed 32 Bytes.
 */
@property (nonatomic, copy, nonnull) NSString *key;

/**
 The value of the channel attribute. Must not exceed 8 KB in length.
 */
@property (nonatomic, copy, nonnull) NSString *value;

/**
 User ID of the user who makes the latest update to the channel attribute.
 */
@property (nonatomic, copy, nonnull) NSString *lastUpdateUserId;

/**
 Timestamp of when the channel attribute was last updated in milliseconds.
 */
@property (nonatomic, assign, readonly) long long lastUpdateTs;
@end

/**
 Returns the result of the [setLocalUserAttributes]([AgoraRtmKit setLocalUserAttributes:completion:]) <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmSetLocalUserAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [addOrUpdateLocalUserAttributes]([AgoraRtmKit addOrUpdateLocalUserAttributes:completion:]) method call. <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmAddOrUpdateLocalUserAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [deleteLocalUserAttributesByKeys]([AgoraRtmKit deleteLocalUserAttributesByKeys:completion:]) method call. <p><li><i>errorCode:</i> AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmDeleteLocalUserAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [clearLocalUserAttributesWithCompletion]([AgoraRtmKit clearLocalUserAttributesWithCompletion:]) method call. <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmClearLocalUserAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [getUserAttributes]([AgoraRtmKit getUserAllAttributes:completion:]) or the [getUserAttributesByKeys]([AgoraRtmKit getUserAttributes:ByKeys:completion:]) method call. <p><li><i>attributes:</i> An array of RtmAttibutes. See AgoraRtmAttribute. <p><li><i>userId:</i> The User ID of the specified user. <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmGetUserAttributesBlock)(NSArray< AgoraRtmAttribute *> * _Nullable attributes, NSString * userId, AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [getChannelMemberCount]([AgoraRtmKit getChannelMemberCount:completion:]) method call. <p><li><i>channelMemberCounts:</i> An array of AgoraRtmChannelMemberCount. See AgoraRtmChannelMemberCount. <p><li><i>errorCode:</i> See AgoraRtmChannelMemberCountErrorCode.
 */
typedef void (^AgoraRtmChannelMemberCountBlock)(NSArray<AgoraRtmChannelMemberCount *> * channelMemberCounts, AgoraRtmChannelMemberCountErrorCode errorCode);

/**
 Returns the result of the [setLocalUserAttributes]([AgoraRtmKit setChannel:Attributes:Options:completion:]) <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmSetChannelAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [addOrUpdateChannelAttributes]([AgoraRtmKit addOrUpdateChannel:Attributes:Options:completion:]) method call. <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmAddOrUpdateChannelAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [deleteChannelAttributesByKeys]([AgoraRtmKit deleteChannel:AttributesByKeys:Options:completion:]) method call. <p><li><i>errorCode:</i> AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmDeleteChannelAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [clearChannelAttributesWithCompletion]([AgoraRtmKit clearChannel:Options:AttributesWithCompletion:]) method call. <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmClearChannelAttributesBlock)(AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Returns the result of the [getChannelAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) or the [getChannelAttributesByKeys]([AgoraRtmKit getChannelAttributes:ByKeys:completion:]) method call. <p><li><i>attributes:</i> An array of AgoraRtmChannelAttibute. See AgoraRtmChannelAttibute. <p><li><i>errorCode:</i> See AgoraRtmProcessAttributeErrorCode.
 */
typedef void (^AgoraRtmGetChannelAttributesBlock)(NSArray< AgoraRtmChannelAttribute *> * _Nullable attributes, AgoraRtmProcessAttributeErrorCode errorCode);

/**
 Message sending options.
 */
__attribute__((visibility("default"))) @interface AgoraRtmSendMessageOptions: NSObject

/**
 Enables offline messaging.
 
 - YES: Enables offline messaging.
 - NO: (default) Disables offline messaging.
 */
@property (nonatomic, assign) BOOL enableOfflineMessaging;

@property (nonatomic, assign) BOOL enableHistoricalMessaging;
@end

/**
 Channel attribute-specific options.
 */
__attribute__((visibility("default"))) @interface AgoraRtmChannelAttributeOptions: NSObject

/**
 Indicates whether or not to notify all channel members of a channel attribute change.
 
 **NOTE**
 
 This flag is valid only within the current method call.
 
 - YES: Notify all channel members of a channel attribute change.
 - NO: (Default) Do not notify all channel members of a channel attribute change.
 */
@property (nonatomic, assign) BOOL enableNotificationToChannelMembers;
@end

/**
 The entry point to the Agora RTM system.
 */
__attribute__((visibility("default"))) @interface AgoraRtmKit: NSObject

/**
 AgoraRtmDelegate enables Agora RTM callback event notifications to your app.
 */
@property (atomic, weak, nullable) id<AgoraRtmDelegate> agoraRtmDelegate;

/**
 **DEPRECATED** The property for managing channels for the local user.
 */
@property (atomic, readonly, nullable) NSMutableDictionary<NSString *, AgoraRtmChannel *> *channels __deprecated;

@property (atomic, strong, nullable) AgoraRtmCallKit *rtmCallKit;

/**
 Creates and initializes an `AgoraRtmKit` instance.

 The Agora RTM SDK supports creating multiple `AgoraRtmKit` instances. All methods in the `AgoraRtmKit` class, except the [destroyChannelWithId](destroyChannelWithId:) method, are executed asynchronously.

 @param appId The App ID issued to you from the Agora Console. Apply for a new App ID from Agora if it is missing from your kit.

 @param delegate AgoraRtmDelegate invokes callbacks to be passed to the app on Agora RTM SDK runtime events.

 @return - An `AgoraRtmKit` instance if this method call succeeds.
 - `nil` if this method call fails for the reason that the length of the `appId` is not 32 characters.

 */
- (instancetype _Nullable)initWithAppId:(NSString * _Nonnull)appId
                              delegate:(id <AgoraRtmDelegate> _Nullable)delegate;

/**
 Logs in the Agora RTM system.
 
 **Note**
 
 - If you log in with the same user ID from a different instance, you will be kicked out of your previous login and removed from previously joined channels.
 - The call frequency limit for this method is two queries per second.
 - Only after you successfully call this method and receives the `AgoraRtmLoginErrorOk` error code, can you call the key RTM methods except:
 
  - [createChannelWithId]([AgoraRtmKit createChannelWithId:delegate:])
  - [initWithText]([AgoraRtmMessage initWithText:])
  - [getRtmCallKit]([AgoraRtmKit getRtmCallKit])
  - [initWithCalleeId]([AgoraRtmLocalInvitation initWithCalleeId:])

 After the app calls this method, the local user receives the [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callback and switches to the `AgoraRtmConnectionStateConnecting` state.

 - Success: The local user receives the [AgoraRtmLoginBlock](AgoraRtmLoginBlock) and [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callbacks, and switches to the `AgoraRtmConnectionStateConnected` state.
 - Failure: The local user receives the [AgoraRtmLoginBlock](AgoraRtmLoginBlock) and [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callbacks, and switches to the `AgoraRtmConnectionStateDisconnected` state.

 @param token A token generated by the app server and used to log in the Agora RTM system. `token` is used when dynamic authentication is enabled. Set `token` as `nil` at the integration and test stages.

 @param userId The user ID of the user logging in the Agora RTM system. The string length must be less than 64 bytes with the following character scope:

 - The 26 lowercase English letters: a to z
 - The 26 uppercase English letters: A to Z
 - The 10 numbers: 0 to 9
 - Space
 - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","
 
 **Note**
 
 A `userId` cannot be empty, nil, or "null".
 
 @param completionBlock [AgoraRtmLoginBlock](AgoraRtmLoginBlock) returns the result of this method call. See AgoraRtmLoginErrorCode for the error codes.

 */
- (void)loginByToken:(NSString * _Nullable)token
                user:(NSString * _Nonnull)userId
          completion:(AgoraRtmLoginBlock _Nullable)completionBlock;

/**
 Logs out of the Agora RTM system.

 - Success: The local user receives the [AgoraRtmLogoutBlock](AgoraRtmLogoutBlock) and [connectionStateChanged]([AgoraRtmDelegate rtmKit:connectionStateChanged:reason:]) callbacks, and switches to the `AgoraRtmConnectionStateDisConnected` state.
 - Failure: The local user receives the [AgoraRtmLogoutBlock](AgoraRtmLogoutBlock) callback.

 @param completionBlock [AgoraRtmLogoutBlock](AgoraRtmLogoutBlock) returns the result of this method call. See AgoraRtmLogoutErrorCode for the error codes.
 */
- (void)logoutWithCompletion:(AgoraRtmLogoutBlock _Nullable )completionBlock;

/**
 Renews the current RTM Token.
 
 You are required to renew your Token when receiving the [rtmKitTokenDidExpire]([AgoraRtmDelegate rtmKitTokenDidExpire:]) callback, and the [AgoraRtmRenewTokenBlock](AgoraRtmRenewTokenBlock) callback returns the result of this method call. The call frequency limit for this method is two queries per second.
 
 @param token Your new RTM Token.
 @param completionBlock [AgoraRtmRenewTokenBlock](AgoraRtmRenewTokenBlock) returns the result of this method call.
 
 - *token:* Your new RTM Token.
 - *errorCode:* See AgoraRtmRenewTokenErrorCode for the error codes.
*/
- (void)renewToken:(NSString * _Nonnull)token completion:(AgoraRtmRenewTokenBlock _Nullable)completionBlock;

/**
 Sends a peer-to-peer message to a specified peer user.
 
 **DEPRECATED**
 
 We do not recommend using this method to send a peer-to-peer message. Use [sendMessage]([AgoraRtmKit sendMessage:toPeer:sendMessageOptions:completion:]) instead.

 - Success:
    - The local user receives the [AgoraRtmSendPeerMessageBlock](AgoraRtmSendPeerMessageBlock) callback.
    - The specified remote user receives the [messageReceived]([AgoraRtmDelegate rtmKit:messageReceived:fromPeer:]) callback.
 - Failure: The local user receives the [AgoraRtmSendPeerMessageBlock](AgoraRtmSendPeerMessageBlock) callback with an error. See AgoraRtmSendPeerMessageErrorCode for the error codes.
 
 **Note**
 
 You can send messages, including peer-to-peer and channel messages, at a maximum speed of 60 queries per second.

 @param message The message to be sent. For information about creating a message, see AgoraRtmMessage.

 @param peerId The user ID of the remote user.

 @param completionBlock [AgoraRtmSendPeerMessageBlock](AgoraRtmSendPeerMessageBlock) returns the result of this method call. See AgoraRtmSendPeerMessageErrorCode for the error codes.
 */
- (void)sendMessage:(AgoraRtmMessage * _Nonnull)message
             toPeer:(NSString * _Nonnull)peerId
         completion:(AgoraRtmSendPeerMessageBlock _Nullable)completionBlock;

/**
 Sends an (offline) peer-to-peer message to a specified peer user.
 
 This method allows you to send a message to a specified user when he/she is offline. If you set a message as an offline message and the specified user is offline when you send it, the RTM server caches it. Please note that for now we only cache 200 offline messages for up to seven days for each receiver. When the number of the cached messages reaches this limit, the newest message overrides the oldest one.
 
 If you use this method to send off a <i>text</i> message that starts off with AGORA_RTM_ENDCALL_PREFIX_<channelId>_<your additional information>, then this method is compatible with the endCall method of the legacy Agora Signaling SDK. Replace <channelId> with the channel ID from which you want to leave (end call), and replace <your additional information> with any additional information. Note that you must not put any "_" (underscore" in your additional information but you can set <your additional information> as empty "".
 
 - Success:
   - The local user receives the [AgoraRtmSendPeerMessageBlock](AgoraRtmSendPeerMessageBlock) callback.
   - The specified remote user receives the [messageReceived]([AgoraRtmDelegate rtmKit:messageReceived:fromPeer:]) callback.
 - Failure: The local user receives the [AgoraRtmSendPeerMessageBlock](AgoraRtmSendPeerMessageBlock) callback with an error. See AgoraRtmSendPeerMessageErrorCode for the error codes.
 
 **Note**
 
 You can send messages, including peer-to-peer and channel messages, at a maximum speed of 60 queries per second.
 
 @param message The message to be sent. For information about creating a message, see AgoraRtmMessage.
 @param peerId The user ID of the remote user. The string length must be less than 64 bytes with the following character scope:
 
 - The 26 lowercase English letters: a to z
 - The 26 uppercase English letters: A to Z
 - The 10 numbers: 0 to 9
 - Space
 - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","
 
 **Note**
 
 A `peerId` cannot be empty, nil, or "null".
 
 @param options Options when sending the message to a peer. See AgoraRtmSendMessageOptions.
 @param completionBlock [AgoraRtmSendPeerMessageBlock](AgoraRtmSendPeerMessageBlock) returns the result of this method call. See AgoraRtmSendPeerMessageErrorCode for the error codes.
 */
- (void)sendMessage:(AgoraRtmMessage * _Nonnull)message
             toPeer:(NSString * _Nonnull)peerId
            sendMessageOptions:(AgoraRtmSendMessageOptions* _Nonnull)options
         completion:(AgoraRtmSendPeerMessageBlock _Nullable)completionBlock;

/**
 Creates an Agora RTM channel.

 **Note**
 
 You can create multiple `AgoraRtmChannel` instances in an `AgoraRtmKit` instance. But you can only join a maximum of 20 channels at the same time. As a good practice, we recommend calling the [destroyChannelWithId]([AgoraRtmKit destroyChannelWithId:]) method to release all resources of an RTM channel that you no longer use.

 @param channelId The unique channel name of the Agora RTM session. The string length must not exceed 64 bytes with the following character scope:
 
 - The 26 lowercase English letters: a to z
 - The 26 uppercase English letters: A to Z
 - The 10 numbers: 0 to 9
 - Space
 - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","

 **Note**
 
 A `channelId` cannot be empty, nil, or "null". 

 @param delegate [AgoraRtmChannelDelegate](AgoraRtmChannelDelegate) invokes callbacks to be passed to the app on Agora RTM SDK runtime events.

 @return - An [AgoraRtmChannel](AgoraRtmChannel) instance if this method call succeeds.
 - `nil` if this method call fails for reasons such as the `channelId` is invalid or a channel with the same `channelId` already exists.

*/
- (AgoraRtmChannel * _Nullable)createChannelWithId:(NSString * _Nonnull)channelId
                                    delegate:(id <AgoraRtmChannelDelegate> _Nullable)delegate;

/**
 Destroys a specified local [AgoraRtmChannel](AgoraRtmChannel) instance.
 
 **Note**
 
 Do not call this method in any of your callbacks.

 @param channelId The channel ID of the channel to be destroyed.
*/
- (BOOL)destroyChannelWithId: (NSString * _Nonnull) channelId;

/**
 Gets the AgoraRtmCallKit instance.

 @return The AgoraRtmCallKit instance.
 */
- (AgoraRtmCallKit * _Nullable)getRtmCallKit;
    
/**
 Queries the online status of the specified user(s).

 - Online: The user has logged in the Agora RTM system.
 - Offline: The user has logged out of the Agora RTM system.

 @param peerIds User IDs of the specified users.
 @param completionBlock [AgoraRtmQueryPeersOnlineBlock](AgoraRtmQueryPeersOnlineBlock) returns the result of this method call.
 
 - *peerOnlineStatus:* A list of the specified users' online status. See AgoraRtmPeerOnlineStatus.
 - *errorCode:* See AgoraRtmQueryPeersOnlineBlock.
*/
- (void)queryPeersOnlineStatus: (NSArray<NSString*> * _Nonnull) peerIds completion:(AgoraRtmQueryPeersOnlineBlock _Nullable)completionBlock;

/**
 Substitutes the local user's attributes with new ones.
 
 For [setLocalUserAttributes]([AgoraRtmKit setLocalUserAttributes:completion:]), [addOrUpdateLocalUserAttributes]([AgoraRtmKit addOrUpdateLocalUserAttributes:completion:]), [deleteLocalUserAttributesByKeys]([AgoraRtmKit deleteLocalUserAttributesByKeys:completion:]) and [clearLocalUserAttributes]([AgoraRtmKit clearLocalUserAttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.
 
 @param attributes The new attributes. See AgoraRtmAttribute.
 @param completionBlock [AgoraRtmSetLocalUserAttributesBlock](AgoraRtmSetLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)setLocalUserAttributes:(NSArray< AgoraRtmAttribute *> * _Nullable) attributes
                            completion:(AgoraRtmSetLocalUserAttributesBlock _Nullable)completionBlock;

/**
 Adds or updates the local user's attribute(s).
 
 For [setLocalUserAttributes]([AgoraRtmKit setLocalUserAttributes:completion:]), [addOrUpdateLocalUserAttributes]([AgoraRtmKit addOrUpdateLocalUserAttributes:completion:]), [deleteLocalUserAttributesByKeys]([AgoraRtmKit deleteLocalUserAttributesByKeys:completion:]) and [clearLocalUserAttributes]([AgoraRtmKit clearLocalUserAttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.
 
 This method updates the local user's attribute(s) if it finds that the attribute(s) has/have the same key(s), or adds attribute(s) to the local user if it does not.
 
 @param attributes The attrubutes to be added or updated. See AgoraRtmAttribute 。
 @param completionBlock [AgoraRtmAddOrUpdateLocalUserAttributesBlock](AgoraRtmAddOrUpdateLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)addOrUpdateLocalUserAttributes:(NSArray< AgoraRtmAttribute *> * _Nullable) attributes
                            completion:(AgoraRtmAddOrUpdateLocalUserAttributesBlock _Nullable)completionBlock;

/**
 Deletes the local user's attributes using attribute keys.
 
 For [setLocalUserAttributes]([AgoraRtmKit setLocalUserAttributes:completion:]), [addOrUpdateLocalUserAttributes]([AgoraRtmKit addOrUpdateLocalUserAttributes:completion:]), [deleteLocalUserAttributesByKeys]([AgoraRtmKit deleteLocalUserAttributesByKeys:completion:]) and [clearLocalUserAttributes]([AgoraRtmKit clearLocalUserAttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.
 
 @param attributeKeys An array of the attribute keys to be deleted.
 @param completionBlock [AgoraRtmDeleteLocalUserAttributesBlock](AgoraRtmDeleteLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)deleteLocalUserAttributesByKeys:(NSArray< NSString *> * _Nullable) attributeKeys
                       completion:(AgoraRtmDeleteLocalUserAttributesBlock _Nullable)completionBlock;

/**
 Clears all attributes of the local user.
 
 For [setLocalUserAttributes]([AgoraRtmKit setLocalUserAttributes:completion:]), [addOrUpdateLocalUserAttributes]([AgoraRtmKit addOrUpdateLocalUserAttributes:completion:]), [deleteLocalUserAttributesByKeys]([AgoraRtmKit deleteLocalUserAttributesByKeys:completion:]) and [clearLocalUserAttributes]([AgoraRtmKit clearLocalUserAttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.
 
 @param completionBlock [AgoraRtmClearLocalUserAttributesBlock](AgoraRtmClearLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)clearLocalUserAttributesWithCompletion:(AgoraRtmClearLocalUserAttributesBlock _Nullable)completionBlock;

/**
 Gets all attributes of a specified user.
 
 For [getUserAttributes]([AgoraRtmKit getUserAllAttributes:completion:]) and [getUserAttributesByKeys]([AgoraRtmKit getUserAttributes:ByKeys:completion:]) taken together, the call frequency limit is 40 queries every five seconds.
 
 @param userId The user ID of the specified user.
 @param completionBlock [AgoraRtmGetUserAttributesBlock](AgoraRtmGetUserAttributesBlock) returns the result of this method call.
 */
- (void)getUserAllAttributes:(NSString* )userId
                  completion:(AgoraRtmGetUserAttributesBlock _Nullable)completionBlock;

/**
 Gets the attributes of a specified user using attribute keys.
 
 For [getUserAttributes]([AgoraRtmKit getUserAllAttributes:completion:]) and [getUserAttributesByKeys]([AgoraRtmKit getUserAttributes:ByKeys:completion:]) taken together, the call frequency limit is 40 queries every five seconds.
 
 @param userId The user ID of the specified user.
 @param attributeKeys An array of the attribute keys.
 @param completionBlock [AgoraRtmGetUserAttributesBlock](AgoraRtmGetUserAttributesBlock) returns the result of this method call.
 */
- (void)getUserAttributes:(NSString* )userId
                   ByKeys:(NSArray< NSString *> * _Nullable)attributeKeys
               completion:(AgoraRtmGetUserAttributesBlock _Nullable)completionBlock;

/**
 Gets the member count of specified channel(s).
 
 **NOTE**
 
 - The call frequency limit for this method is one query per second.
 - We do not support getting the member counts of more than 32 channels in one method call.

 @param channelIds An array of the specified channel ID(s).
 @param completionBlock [AgoraRtmChannelMemberCountBlock](AgoraRtmChannelMemberCountBlock) returns the result of this method call.
 */
- (void)getChannelMemberCount:(NSArray<NSString*> * _Nonnull)channelIds completion:(AgoraRtmChannelMemberCountBlock _Nullable)completionBlock;

/**
 Sets the attributes of a specified channel with new ones.
 
 - If more than one user can update channel attributes, then we recommend calling [getChannelAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) to update the cache before calling this method.
 - For [setChannelAttributes]([AgoraRtmKit setChannel:Attributes:Options:completion:]), [addOrUpdateChannelAttributes]([AgoraRtmKit addOrUpdateChannel:Attributes:Options:completion:]), [deleteChannelAttributesByKeys]([AgoraRtmKit deleteChannel:AttributesByKeys:Options:completion:]) and [clearChannelAttributes]([AgoraRtmKit clearChannel:Options:AttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.

 @param channelId The ID of the specified channel.
 @param attributes An array of the attributes. See AgoraRtmChannelAttribute.
 @param options Options for this attribute operation. See AgoraRtmChannelAttributeOptions.
 @param completionBlock [AgoraRtmSetLocalUserAttributesBlock](AgoraRtmSetLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)setChannel:(NSString* _Nonnull)channelId
        Attributes:(NSArray< AgoraRtmChannelAttribute *> * _Nullable)attributes
        Options:(AgoraRtmChannelAttributeOptions* _Nonnull)options
        completion:(AgoraRtmSetChannelAttributesBlock _Nullable)completionBlock;

/**
 Adds or updates the attribute(s) of a specified channel.
 
 This method updates the specified channel's attribute(s) if it finds that the attribute(s) has/have the same key(s), or adds attribute(s) to the channel if it does not.
 
 - If more than one user can update channel attributes, then we recommend calling [getChannelAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) to update the cache before calling this method.
 - For [setChannelAttributes]([AgoraRtmKit setChannel:Attributes:Options:completion:]), [addOrUpdateChannelAttributes]([AgoraRtmKit addOrUpdateChannel:Attributes:Options:completion:]), [deleteChannelAttributesByKeys]([AgoraRtmKit deleteChannel:AttributesByKeys:Options:completion:]) and [clearChannelAttributes]([AgoraRtmKit clearChannel:Options:AttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.

 @param channelId The ID of the specified channel.
 @param attributes An array of the attributes. See AgoraRtmChannelAttribute.
 @param options Options for this attribute operation. See AgoraRtmChannelAttributeOptions.
 @param completionBlock [AgoraRtmAddOrUpdateLocalUserAttributesBlock](AgoraRtmAddOrUpdateLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)addOrUpdateChannel:(NSString* _Nonnull)channelId
        Attributes:(NSArray< AgoraRtmChannelAttribute *> * _Nullable)attributes
        Options:(AgoraRtmChannelAttributeOptions* _Nonnull)options
        completion:(AgoraRtmAddOrUpdateChannelAttributesBlock _Nullable)completionBlock;

/**
 Deletes the attributes of a specified channel by attribute keys.
 
 - If more than one user can update channel attributes, then we recommend calling [getChannelAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) to update the cache before calling this method.
 - For [setChannelAttributes]([AgoraRtmKit setChannel:Attributes:Options:completion:]), [addOrUpdateChannelAttributes]([AgoraRtmKit addOrUpdateChannel:Attributes:Options:completion:]), [deleteChannelAttributesByKeys]([AgoraRtmKit deleteChannel:AttributesByKeys:Options:completion:]) and [clearChannelAttributes]([AgoraRtmKit clearChannel:Options:AttributesWithCompletion:]) taken together: the limit is 10 queries every five seconds.

 @param channelId The ID of the specified channel.
 @param attributeKeys An array of the attribute keys.
 @param options Options for this attribute operation. See AgoraRtmChannelAttributeOptions.
 @param completionBlock [AgoraRtmDeleteLocalUserAttributesBlock](AgoraRtmDeleteLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)deleteChannel:(NSString* _Nonnull)channelId
        AttributesByKeys:(NSArray< NSString *> * _Nullable)attributeKeys
        Options:(AgoraRtmChannelAttributeOptions* _Nonnull)options
        completion:(AgoraRtmDeleteChannelAttributesBlock _Nullable)completionBlock;

/**
 Clears all attributes of a specified channel.
 
 For [getChannelAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) and [getChannelAttributesByKeys]([AgoraRtmKit getChannelAttributes:ByKeys:completion:]) taken together, the call frequency limit is 10 queries every five seconds.

 @param channelId The ID of the specified channel.
 @param options Options for this attribute operation. See AgoraRtmChannelAttributeOptions.
 @param completionBlock [AgoraRtmClearLocalUserAttributesBlock](AgoraRtmClearLocalUserAttributesBlock) returns the result of this method call.
 */
- (void)clearChannel:(NSString* _Nonnull)channelId
        Options:(AgoraRtmChannelAttributeOptions* _Nonnull)options
        AttributesWithCompletion:(AgoraRtmClearChannelAttributesBlock _Nullable)completionBlock;

/**
 Gets all attributes of a specified channel.
 
 For [getChannelAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) and [getChannelAttributesByKeys]([AgoraRtmKit getChannelAttributes:ByKeys:completion:]) taken together, the call frequency limit is 10 queries every five seconds.

 @param channelId The ID of the specified channel.
 @param completionBlock [AgoraRtmGetUserAttributesBlock](AgoraRtmGetUserAttributesBlock) returns the result of this method call.
 */
- (void)getChannelAllAttributes:(NSString* _Nonnull)channelId
                  completion:(AgoraRtmGetChannelAttributesBlock _Nullable)completionBlock;

/**
 Gets the attributes of a specified channel by attribute keys.
 
 For [getChannelAttributes]([AgoraRtmKit getChannelAllAttributes:completion:]) and [getChannelAttributesByKeys]([AgoraRtmKit getChannelAttributes:ByKeys:completion:]) taken together, the call frequency limit is 10 queries every five seconds.

 @param channelId The ID of the specified channel.
 @param attributeKeys An array of the attribute keys.
 @param completionBlock [AgoraRtmGetUserAttributesBlock](AgoraRtmGetUserAttributesBlock) returns the result of this method call.
 */
- (void)getChannelAttributes:(NSString* _Nonnull)channelId
                   ByKeys:(NSArray< NSString *> * _Nullable) attributeKeys
               completion:(AgoraRtmGetChannelAttributesBlock _Nullable)completionBlock;

/**
 Subscribes to the online status of the specified user(s).
 
 - When the method call succeeds, the SDK returns the [PeersOnlineStatusChanged]([AgoraRtmDelegate rtmKit:PeersOnlineStatusChanged:]) callback to report the online status of peers, to whom you subscribe.
 - When the online status of the peers, to whom you subscribe, changes, the SDK returns the [PeersOnlineStatusChanged]([AgoraRtmDelegate rtmKit:PeersOnlineStatusChanged:]) callback to report whose online status has changed.
 - If the online status of the peers, to whom you subscribe, changes when the SDK is reconnecting to the server, the SDK returns the [PeersOnlineStatusChanged]([AgoraRtmDelegate rtmKit:PeersOnlineStatusChanged:]) callback to report whose online status has changed when successfully reconnecting to the server.
 
 **Note**
 
 - When you log out of the Agora RTM system, all the status that you subscribe to will be cleared. To keep the original subscription after you re-log in the system, you need to redo the whole subscription process.
 - When the SDK reconnects to the server from the state of being interupted, the SDK automatically subscribes to the peers and states before the interruption without human intervention.

 @param peerIds User IDs of the specified users.
 @param completionBlock [AgoraRtmSubscriptionRequestBlock](AgoraRtmSubscriptionRequestBlock) returns the result of this method call.
 */
- (void)subscribePeersOnlineStatus: (NSArray<NSString*> * _Nonnull) peerIds completion:(AgoraRtmSubscriptionRequestBlock _Nullable)completionBlock;

/**
 Unsubscribes from the online status of the specified user(s).
 
 @param peerIds User IDs of the specified users.
 @param completionBlock [AgoraRtmUnsubscriptionRequestBlock](AgoraRtmUnsubscriptionRequestBlock) returns the result of this method call.
 */
- (void)unsubscribePeersOnlineStatus: (NSArray<NSString*> * _Nonnull) peerIds completion:(AgoraRtmSubscriptionRequestBlock _Nullable)completionBlock;

/**
 Gets a list of the peers, to whose specific status you have subscribed.
 
 @param option The status type, to which you have subscribed. See AgoraRtmPeerSubscriptionOptions.
 @param completionBlock [AgoraRtmQueryPeersBySubscriptionOptionBlock](AgoraRtmQueryPeersBySubscriptionOptionBlock) returns the result of this method call.
 */
- (void)queryPeersBySubscriptionOption: (AgoraRtmPeerSubscriptionOptions) option completion:(AgoraRtmQueryPeersBySubscriptionOptionBlock _Nullable)completionBlock;

/**
 Provides the technical preview functionalities or special customizations by configuring the SDK with JSON options.

 **Note**

 The JSON options are not public by default. Agora is working on making commonly used JSON options public in a standard way. Contact [support@agora.io](mailto:support@agora.io) for more information.

 @param parameters SDK options in the JSON format.
 @return
 - 0: Success.
 - &ne;0: Failure.
 */
- (int)setParameters:(NSString * _Nonnull)parameters;

/**
 Specifies the default path to the SDK log file.
 
 **Note**
 
 - Ensure that the directory holding the log file exists and is writable.
 - Ensure that you call this method immediately after calling the [initWithAppId]([AgoraRtmKit initWithAppId:delegate:]) method, otherwise the output log may be incomplete.
 - The default log file location is as follows:
   - iOS: `App Sandbox/Library/caches/agorartm.log`
   - macOS
    - Sandbox enabled: `App Sandbox/Library/Logs/agorartm.log`, for example `/Users/<username>/Library/Containers/<App Bundle Identifier>/Data/Library/Logs/agorartm.log`.
    - Sandbox disabled: `~/Library/Logs/agorartm.log`.

 @param logFile The absolute file path to the log file. The string of the `logFile` is in UTF-8.
 @return - 0: Success.
 - &ne;0: Failure.
 */
- (int)setLogFile:(NSString * _Nonnull)logFile;

/**
 Sets the log file size in KB.
 
 The SDK has two log files, each with a default size of 512 KB. So, if you set fileSizeInBytes as 1024 KB, the SDK outputs log files with a total maximum size of 2 MB.
 
 **Note**
 
 File size settings of less than 512 KB or greater than 10 MB will not take effect.
 
 @param fileSize The SDK log file size (KB).
 @return - 0: Success.
 - &ne;0: Failure.
 */
- (int)setLogFileSize:(int) fileSize;

/**
 Sets the output log level of the SDK.
 
 You can use one or a combination of the filters. The log level follows the sequence of OFF, CRITICAL, ERROR, WARNING, and INFO. Choose a level to see the logs preceding that level. If, for example, you set the log level to WARNING, you see the logs within levels CRITICAL, ERROR, and WARNING.

 @param filter The log filter level. See AgoraRtmLogFilter.
 @return - 0: Success.
 - &ne;0: Failure.
 */
- (int)setLogFilters:(AgoraRtmLogFilter) filter;

/**
 Gets the version of the Agora RTM SDK.

 @return The RTM SDK version.
 */
+ (NSString*) getSDKVersion;

@end


#pragma mark channel

/**
Error codes related to sending a channel message.
 */
typedef NS_ENUM(NSInteger, AgoraRtmSendChannelMessageErrorCode) {
    
  /**
0: The server receives the channel message.
   */
  AgoraRtmSendChannelMessageErrorOk = 0,
    
    /**
1: The user fails to send the channel message state.
     */
  AgoraRtmSendChannelMessageErrorFailure = 1,
    
  /**
2: The SDK does not receive a response from the server in five seconds. The current timeout is set as five seconds. Possible reasons: The user is in the `AgoraRtmConnectionStateAborted` or `AgoraRtmConnectionStateReconnecting` state.
   */
  AgoraRtmSendChannelMessageErrorTimeout = 2,
    
  /**
3: The method call frequency exceeds the limit of 60 queries per second (channel and peer messages taken together).
  */
  AgoraRtmSendChannelMessageTooOften = 3,
    
  /**
4: The message is null or exceeds 32 KB in length.
   */
  AgoraRtmSendChannelMessageInvalidMessage = 4,
    
  /**
101: The SDK is not initialized.
   */
  AgoraRtmSendChannelMessageErrorNotInitialized = 101,
    
  /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before sending out a channel message.
   */
  AgoraRtmSendChannelMessageNotLoggedIn = 102,
};

/**
Error codes related to joining a channel.
 */
typedef NS_ENUM(NSInteger, AgoraRtmJoinChannelErrorCode) {
    
    /**
0: The user joins the channel successfully.
     */
    AgoraRtmJoinChannelErrorOk = 0,
    
    /**
1: The user fails to join the channel.
     */
    AgoraRtmJoinChannelErrorFailure = 1,
    
    /**
2: The user cannot join the channel, possibly because he/she is already in the channel.
     */
    AgoraRtmJoinChannelErrorRejected = 2,
    
    /**
3: The user fails to join the channel because the argument is invalid.
     */
    AgoraRtmJoinChannelErrorInvalidArgument = 3,
    
    /**
4: A timeout occurs when joining the channel. The current timeout is set as five seconds. Possible reasons: The user is in the `AgoraRtmConnectionStateAborted` or `AgoraRtmConnectionStateReconnecting` state.
     */
    AgoraRtmJoinChannelErrorTimeout = 4,
    
    /**
5: The number of the RTM channels you are in exceeds the limit of 20.
     */
    AgoraRtmJoinChannelErrorExceedLimit = 5,
    
    /**
6: The user is joining or has joined the channel.
     */
    AgoraRtmJoinChannelErrorAlreadyJoined = 6,
    
    /**
7: The method call frequency exceeds the limit of 50 queries every three seconds.
     */
    AgoraRtmJoinChannelErrorTooOften = 7,
    
    /**
8: The method call frequency exceeds the limit of 2 queries per 5 seconds for the same channel.
     */
    AgoraRtmJoinSameChannelErrorTooOften = 8,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmJoinChannelErrorNotInitialized = 101,
    
    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before joining the channel.
     */
    AgoraRtmJoinChannelErrorNotLoggedIn = 102,
};

/**
Error codes related to leaving a channel.
 */
typedef NS_ENUM(NSInteger, AgoraRtmLeaveChannelErrorCode) {
    
    /**
0: The user leaves the channel successfully.
     */
    AgoraRtmLeaveChannelErrorOk = 0,
    
    /**
1: The user fails to leave the channel.
     */
    AgoraRtmLeaveChannelErrorFailure = 1,
    
    /**
2: The user cannot leave the channel, possibly because he/she is not in the channel.
     */
    AgoraRtmLeaveChannelErrorRejected = 2,
    
    /**
3: The user is not in the channel.
     */
    AgoraRtmLeaveChannelErrorNotInChannel = 3,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmLeaveChannelErrorNotInitialized = 101,
    
    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before leaving the channel.
     */
    AgoraRtmLeaveChannelErrorNotLoggedIn = 102,
};

/**
Error codes related to retrieving a channel member list.
*/
typedef NS_ENUM(NSInteger, AgoraRtmGetMembersErrorCode) {
    
    /**
0: The user retrieves a member list of the channel successfully.
     */
    AgoraRtmGetMembersErrorOk = 0,
    
    /**
1: The user fails to retrieve a member list of the channel.
     */
    AgoraRtmGetMembersErrorFailure = 1,
    
    /**
2: The user cannot retrieve a member list of the channel.
     */
    AgoraRtmGetMembersErrorRejected = 2,
    
    /**
3: A timeout occurs when retreiving a member list of the channel. The current timeout is set as five seconds. Possible reasons: The user is in the `AgoraRtmConnectionStateAborted` or `AgoraRtmConnectionStateReconnecting` state.
     */
    AgoraRtmGetMembersErrorTimeout = 3,
    
    /**
4: The method call frequency exceeds the limit of five queries every two seconds.
     */
    AgoraRtmGetMembersErrorTooOften = 4,
    
    /**
5: The user is not in channel.
     */
    AgoraRtmGetMembersErrorNotInChannel = 5,
    
    /**
101: The SDK is not initialized.
     */
    AgoraRtmGetMembersErrorNotInitialized = 101,
    
    /**
102: The user does not call the [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) method, or the method call of [loginByToken]([AgoraRtmKit loginByToken:user:completion:]) does not succeed before retrieving a member list.
     */
    AgoraRtmGetMembersErrorNotLoggedIn = 102,
};

/**
Attributes of an Agora RTM channel member.
 */
__attribute__((visibility("default"))) @interface AgoraRtmMember: NSObject

/**
The user ID of the member in the channel.
 */
@property (nonatomic, copy, nonnull) NSString *userId;

/**
The channel ID.
 */
@property (nonatomic, copy, nonnull) NSString *channelId;
@end


/**
 Returns the result of the [joinWithCompletion]([AgoraRtmChannel joinWithCompletion:]) method call. <p><i>errorCode:</i> See AgoraRtmJoinChannelErrorCode for the error codes.
 */
typedef void (^AgoraRtmJoinChannelBlock)(AgoraRtmJoinChannelErrorCode errorCode);

/**
 Returns the result of the [leaveWithCompletion]([AgoraRtmChannel leaveWithCompletion:]) method call. <p><i>errorCode:</i> See AgoraRtmLeaveChannelErrorCode for the error codes.
 */
typedef void (^AgoraRtmLeaveChannelBlock)(AgoraRtmLeaveChannelErrorCode errorCode);

/** Returns the result of the [sendMessage]([AgoraRtmChannel sendMessage:completion:]) method call. <p><i>errorCode:</i> See AgoraRtmSendChannelMessageErrorCode for the error codes.
*/
typedef void (^AgoraRtmSendChannelMessageBlock)(AgoraRtmSendChannelMessageErrorCode errorCode);

/**
 Returns the result of the [getMembersWithCompletion]([AgoraRtmChannel getMembersWithCompletion:]) method call. <p><li><i>members:</i> The member list. See AgoraRtmMember. <li><i>errorCode:</i> See AgoraRtmGetMembersErrorCode for the error codes.
*/
typedef void (^AgoraRtmGetMembersBlock)(NSArray< AgoraRtmMember *> * _Nullable members, AgoraRtmGetMembersErrorCode errorCode);

/**
 The AgoraRtmChannelDelegate protocol enables callback event notifications to your app.

 The SDK uses delegate callbacks in the AgoraRtmChannelDelegate protocol to report AgoraRtmChannelDelegate runtime events to the app.
 */
@protocol AgoraRtmChannelDelegate <NSObject>
@optional

/**
 Occurs when a user joins the channel.
 
 When a remote user calls the [joinWithCompletion]([AgoraRtmChannel joinWithCompletion:]) method and successfully joins the channel, the local user receives this callback.
 
 **Note**
 
 This callback is disabled when the number of the channel members exceeds 512.

 @param channel The channel that the user joins. See AgoraRtmChannel.
 @param member The user joining the channel. See AgoraRtmMember.
 */
- (void)channel:(AgoraRtmChannel * _Nonnull)channel memberJoined:(AgoraRtmMember * _Nonnull)member;

/**
 Occurs when a channel member leaves the channel.
 
 When a remote channel member calls the [leaveWithCompletion]([AgoraRtmChannel leaveWithCompletion:]) method and successfully leaves the channel, the local user receives this callback.
 
 **Note**
 
 This callback is disabled when the number of the channel members exceeds 512.

 @param channel The channel that the user leaves. See AgoraRtmChannel.
 @param member The channel member that leaves the channel. See AgoraRtmMember.
 */
- (void)channel:(AgoraRtmChannel * _Nonnull)channel memberLeft:(AgoraRtmMember * _Nonnull)member;

/**
 Occurs when receiving a channel message.
 
 When a remote channel member calls the [sendMessage]([AgoraRtmChannel sendMessage:completion:]) method and successfully sends out a channel message, the local user receives this callback.

 @param channel The channel, to which the local user belongs. See AgoraRtmChannel.
 @param message The received channel message. See AgoraRtmMessage.
 
 **NOTE** Ensure that you check the `type` property when receiving the message instance: If the message type is `AgoraRtmMessageTypeRaw`, you need to downcast the received instance from AgoraRtmMessage to AgoraRtmRawMessage. See AgoraRtmMessageType.
 
 @param member The message sender. See AgoraRtmMember.
 */
- (void)channel:(AgoraRtmChannel * _Nonnull)channel messageReceived:(AgoraRtmMessage * _Nonnull)message fromMember:(AgoraRtmMember * _Nonnull)member;


/**
 Occurs when channel attributes are updated, and returns all attributes of the channel.
 
 **NOTE**
 
 This callback is enabled only when the user, who updates the attributes of the channel, sets [enableNotificationToChannelMembers]([AgoraRtmChannelAttributeOptions enableNotificationToChannelMembers]) as YES. Also note that this flag is valid only within the current channel attribute method call.

 @param channel The channel, to which the local user belongs. See AgoraRtmChannel.
 @param attributes An array of AgoraRtmChannelAttribute. See AgoraRtmChannelAttribute.
 */
- (void)channel:(AgoraRtmChannel * _Nonnull)channel attributeUpdate:(NSArray< AgoraRtmChannelAttribute *> * _Nonnull)attributes;


/**
 Occurs when the number of the channel members changes, and returns the new number.
 
 **NOTE**
 
 - When the number of channel members &le; 512, the SDK returns this callback when the number changes and at a MAXIMUM speed of once per second.
 - When the number of channel members exceeds 512, the SDK returns this callback when the number changes and at a MAXIMUM speed of once every three seconds.
 - You will receive this callback when successfully joining an RTM channel, so we recommend implementing this callback to receive timely updates on the number of the channel members.

 @param channel The channel, to which the local user belongs. See AgoraRtmChannel.
 @param count Member count of this channel.
 */
- (void)channel:(AgoraRtmChannel * _Nonnull)channel memberCount:(int)count;

@end

/**
 Agora RTM channel methods.
 */
__attribute__((visibility("default"))) @interface AgoraRtmChannel: NSObject

/**
 An [AgoraRtmKit](AgoraRtmKit) instance.
 */
@property (atomic, readonly, nonnull) AgoraRtmKit *kit;

/**
 AgoraRtmChannelDelegate enables Agora RTM channel callback event notifications to your app.
 */
@property (nonatomic, weak, nullable) id<AgoraRtmChannelDelegate> channelDelegate;

/**
 Joins a channel.

 **Note**
 
 You can join a maximum of 20 RTM channels at the same time. When the number of the channels you join exceeds the limit, you receive the `AgoraRtmJoinChannelErrorCodeExceedLimit` error code.

 - The [AgoraRtmJoinChannelBlock](AgoraRtmJoinChannelBlock) callback returns the result of this method call.
 - When the local user successfully joins the channel, all remote users in the channel receive the [memberJoined]([AgoraRtmChannelDelegate channel:memberJoined:]) callback.

@param completionBlock [AgoraRtmJoinChannelBlock](AgoraRtmJoinChannelBlock) returns the result of this method call. See AgoraRtmJoinChannelErrorCode for the error codes.
 */
- (void)joinWithCompletion:(AgoraRtmJoinChannelBlock _Nullable)completionBlock;

/**
 Leaves a channel.

 - The [AgoraRtmLeaveChannelBlock](AgoraRtmLeaveChannelBlock) callback returns the result of this method call.
 - When the local user successfully leaves the channel, all remote users in the channel receive the [memberLeft]([AgoraRtmChannelDelegate channel:memberLeft:]) callback.

 @param completionBlock [AgoraRtmLeaveChannelBlock](AgoraRtmLeaveChannelBlock) returns the result of this method call. See AgoraRtmLeaveChannelErrorCode for the error codes.
 */
- (void)leaveWithCompletion:(AgoraRtmLeaveChannelBlock _Nullable)completionBlock;

/**
 Allows a channel member to send a message to all members in the channel.

 - The [AgoraRtmSendChannelMessageBlock](AgoraRtmSendChannelMessageBlock) callback returns the result of this method call.
 - When the message arrives, all remote members in the channel receive the [messageReceived]([AgoraRtmChannelDelegate channel:messageReceived:fromMember:]) callback.
 
 **Note**
 
 You can send messages, including peer-to-peer and channel messages, at a maximum speed of 60 queries per second.
 
 @param message The message to be sent. See [AgoraRtmMessage](AgoraRtmMessage).
 @param completionBlock [AgoraRtmSendChannelMessageBlock](AgoraRtmSendChannelMessageBlock) returns the result of this method call. See AgoraRtmSendChannelMessageErrorCode for the error codes.
 */
- (void)sendMessage:(AgoraRtmMessage * _Nonnull)message
         completion:(AgoraRtmSendChannelMessageBlock _Nullable)completionBlock;


- (void)sendMessage:(AgoraRtmMessage * _Nonnull)message
         sendMessageOptions:(AgoraRtmSendMessageOptions* _Nonnull)options
         completion:(AgoraRtmSendChannelMessageBlock _Nullable)completionBlock;


/**
 Retrieves a member list of the channel.
 
 **Note**
 
 You can call this method at a maximum speed of five queries every two seconds.

 @param completionBlock [AgoraRtmGetMembersBlock](AgoraRtmGetMembersBlock) returns the result of this method call (the member list if success). See AgoraRtmGetMembersErrorCode for the error codes.
 */
- (void)getMembersWithCompletion:(AgoraRtmGetMembersBlock _Nullable)completionBlock;
@end
