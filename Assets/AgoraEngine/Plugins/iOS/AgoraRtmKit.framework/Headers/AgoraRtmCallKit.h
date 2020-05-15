//
//  AgoraRtmKit.h
//  AgoraRtmKit
//
//  Copyright (c) 2019 Agora.io. All rights reserved.
//

#import <Foundation/Foundation.h>

@class AgoraRtmKit;
@class AgoraRtmCallKit;

/**
<b>RETURNED TO THE CALLER.</b> States of an outgoing call invitation.
 */
typedef NS_ENUM(NSInteger, AgoraRtmLocalInvitationState) {
    
  /**
0: <b>RETURNED TO THE CALLER.</b> The initial state of a call invitation (idle).
   */
  AgoraRtmLocalInvitationStateIdle = 0,
    
  /**
1: FOR INTERNAL USE ONLY.
   */
  AgoraRtmLocalInvitationStateSentToRemote = 1,
    
  /**
2: <b>RETURNED TO THE CALLER.</b> The call invitation is received by the callee.
   */
  AgoraRtmLocalInvitationStateReceivedByRemote = 2,
    
  /**
3: <b>RETURNED TO THE CALLER.</b> The call invitation is accepted by the callee.
   */
  AgoraRtmLocalInvitationStateAcceptedByRemote = 3,
    
  /**
4: <b>RETURNED TO THE CALLER.</b> The call invitation is declined by the callee.
   */
  AgoraRtmLocalInvitationStateRefusedByRemote = 4,
    
  /**
5: <b>RETURNED TO THE CALLER.</b> You have canceled the call invitation.
   */
  AgoraRtmLocalInvitationStateCanceled = 5,
    
  /**
6: <b>RETURNED TO THE CALLER.</b> The life cycle of the outgoing call invitation ends in failure.
   */
  AgoraRtmLocalInvitationStateFailure = 6,
};

/**
<b>RETURNED TO THE CALLEE.</b> States of an incoming call invitation.
 */
typedef NS_ENUM(NSInteger, AgoraRtmRemoteInvitationState) {
    
  /**
0: <b>RETURNED TO THE CALLEE.</b> The initial state of a call invitation (idle).
   */
  AgoraRtmRemoteInvitationStateIdle = 0,
    
  /**
1: <b>RETURNED TO THE CALLEE.</b> A call invitation from a remote caller is received.
   */
  AgoraRtmRemoteInvitationStateInvitationReceived = 1,
    
  /**
2: FOR INTERNAL USE ONLY.
   */
  AgoraRtmRemoteInvitationStateAcceptSentToLocal = 2,
    
  /**
3: <b>RETURNED TO THE CALLEE.</b> You have declined the call invitation.
   */
  AgoraRtmRemoteInvitationStateRefused = 3,
    
  /**
4: <b>RETURNED TO THE CALLEE.</b> You have accepted the call invitation.
   */
  AgoraRtmRemoteInvitationStateAccepted = 4,
    
  /**
5: <b>RETURNED TO THE CALLEE.</b> The call invitation is canceled by the remote caller.
   */
  AgoraRtmRemoteInvitationStateCanceled = 5,
    
  /**
6: <b>RETURNED TO THE CALLEE.</b> The life cycle of the incoming call invitation ends in failure.
   */
  AgoraRtmRemoteInvitationStateFailure = 6,
};

/**
<b>RETURNED TO THE CALLER.</b> Error codes of an outgoing call invitation.
 */
typedef NS_ENUM(NSInteger, AgoraRtmLocalInvitationErrorCode) {
    
  /**
0: <b>RETURNED TO THE CALLER.</b> The outgoing call invitation succeeds.
   */
  AgoraRtmLocalInvitationErrorOk = 0,
    
  /**
1: <b>RETURNED TO THE CALLER.</b> The callee is offline. <p> The SDK: <ul><li>Keeps resending the call invitation to the callee, if he or she is offline.</li><li>Returns this error code, if he or she is still offline 30 seconds since the call invitation is sent.</li></ul>
   */
  AgoraRtmLocalInvitationErrorRemoteOffline = 1,
    
  /**
2: <b>RETURNED TO THE CALLER.</b> The callee is online but has not ACKed to the call invitation 30 seconds since it is sent.
   */
  AgoraRtmLocalInvitationErrorRemoteNoResponse = 2,
    
  /**
3: <b>RETURNED TO THE CALLER. SAVED FOR FUTURE USE.</b> The call invitation expires 60 seconds since it is sent, if the callee ACKs to the call invitation but neither the caller or callee takes any further action (cancel, accpet, or decline it).
   */
  AgoraRtmLocalInvitationErrorExpire = 3,
    
  /**
4: <b>RETURNED TO THE CALLER.</b> The caller is not logged in.
   */
  AgoraRtmLocalInvitationErrorNotLoggedIn = 4,
};

/**
<b>RETURNED TO THE CALLEE.</b> Error codes of an incoming call invitation.
 */
typedef NS_ENUM(NSInteger, AgoraRtmRemoteInvitationErrorCode) {
    
  /**
0: <b>RETURNED TO THE CALLEE.</b> The incoming call invitation succeeds.
   */
  AgoraRtmRemoteInvitationErrorOk = 0,
    
  /**
1: <b>RETURNED TO THE CALLEE.</b> The call invitation received by the callee fails: the callee is not online.
   */
  AgoraRtmRemoteInvitationErrorLocalOffline = 1,
    
  /**
2: <b>RETURNED TO THE CALLEE.</b> The call invitation received by callee fails: the acceptance of the call invitation fails.
   */
  AgoraRtmRemoteInvitationErrorAcceptFailure = 2,
    
  /**
3: <b>RETURNED TO THE CALLEE.</b> The call invitation received by the callee fails: the call invitation expires 60 seconds since it is sent, if the callee ACKs to the call invitation but neither the caller or callee takes any further action (cancel, accpet, or decline it).
   */
  AgoraRtmRemoteInvitationErrorExpire = 3,
};

/**
Error codes of the call invitation methods.
 */
typedef NS_ENUM(NSInteger, AgoraRtmInvitationApiCallErrorCode) {
    
  /**
0: The method call succeeds.
   */
  AgoraRtmInvitationApiCallErrorOk = 0,
    
  /**
1: The method call fails. Invalid argument.
   */
  AgoraRtmInvitationApiCallErrorInvalidAugment = 1,
    
  /**
2: The method call fails. The call invitation has not started.
   */
  AgoraRtmInvitationApiCallErrorNotStarted = 2,
    
  /**
3: The method call fails. The call invitation has ended.
   */
  AgoraRtmInvitationApiCallErrorAlreadyEnd = 3,
    
  /**
4: The method call fails. The call invitation is already accepted.
   */
  AgoraRtmInvitationApiCallErrorAlreadyAccept = 4,
    
  /**
5: The method call fails. The call invitation is already sent.
   */
  AgoraRtmInvitationApiCallErrorAlreadySent = 5,
};

/**
 Returns the result of the [sendLocalInvitation]([AgoraRtmCallKit sendLocalInvitation:completion:]) method call. See AgoraRtmInvitationApiCallErrorCode for the error codes.
 */
typedef void (^AgoraRtmLocalInvitationSendBlock)(AgoraRtmInvitationApiCallErrorCode errorCode);

/**
 Returns the result of the [acceptRemoteInvitation]([AgoraRtmCallKit acceptRemoteInvitation:completion:]) method call. See AgoraRtmInvitationApiCallErrorCode for the error codes.
 */
typedef void (^AgoraRtmRemoteInvitationAcceptBlock)(AgoraRtmInvitationApiCallErrorCode errorCode);

/**
 Returns the result of the [refuseRemoteInvitation]([AgoraRtmCallKit refuseRemoteInvitation:completion:]) method call. See AgoraRtmInvitationApiCallErrorCode for the error codes.
 */
typedef void (^AgoraRtmRemoteInvitationRefuseBlock)(AgoraRtmInvitationApiCallErrorCode errorCode);

/**
 Returns the result of the [cancelLocalInvitation]([AgoraRtmCallKit cancelLocalInvitation:completion:]) method call. See AgoraRtmInvitationApiCallErrorCode for the error codes.
 */
typedef void (^AgoraRtmLocalInvitationCancelBlock)(AgoraRtmInvitationApiCallErrorCode errorCode);

/**
 The caller's call invitation object.
 */
__attribute__((visibility("default"))) @interface AgoraRtmLocalInvitation: NSObject

/**
 User ID of the callee.
 */
@property (nonatomic, copy, nonnull) NSString *calleeId;

/**
 The call invitation content set by the caller.
 */
@property (nonatomic, copy, nullable) NSString *content;

/**
 The callee's reponse to the incoming call invitation.
 */
@property (nonatomic, copy, nullable, readonly) NSString *response;

/**
 The channel ID.
 
 **NOTE**
 
 To intercommunicate with the legacy Agora Signaling SDK, you MUST set the channel ID. However, even if the callee successfully accepts the call invitation, the Agora RTM SDK does not join the channel of the specified channel ID.
 */
@property (nonatomic, copy, nullable) NSString *channelId;

/**
 The state of the outgoing call invitation. See AgoraRtmLocalInvitationState.
 */
@property (nonatomic, assign, readonly) AgoraRtmLocalInvitationState state;

/**
 Creates an [AgoraRtmLocalInvitation](AgoraAgoraRtmLocalInvitation) instance.

 @param calleeId uid of the callee.
 @return An [AgoraRtmLocalInvitation](AgoraAgoraRtmLocalInvitation) instance.
 */
- (instancetype _Nonnull)initWithCalleeId: (NSString * _Nonnull) calleeId;
@end

/** The callee's call invitation object. */
__attribute__((visibility("default"))) @interface AgoraRtmRemoteInvitation: NSObject

/** User ID of the caller. */
@property (nonatomic, copy, nonnull, readonly) NSString *callerId;

/** The call invitation content set by the caller. */
@property (nonatomic, copy, nullable, readonly) NSString *content;

/** The callee's reponse to the call invitation. */
@property (nonatomic, copy, nullable) NSString *response;

/**The channel ID. */
@property (nonatomic, copy, nullable, readonly) NSString *channelId;

/** The state of the incoming call invitation. See AgoraRtmRemoteInvitationState. */
@property (nonatomic, assign, readonly) AgoraRtmRemoteInvitationState state;
@end

/**
 The AgoraRtmCallDelegate protocol enables Agora RTM call callback event notifications to your app.
 */
@protocol AgoraRtmCallDelegate  <NSObject>
@optional

/**
 Callback to the caller: occurs when the callee receives the call invitation.

 @param callKit An RtmCallKit object.
 @param localInvitation An AgoraRtmLocalInvitation object.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit localInvitationReceivedByPeer:(AgoraRtmLocalInvitation * _Nonnull)localInvitation;

/**
 Callback to the caller: occurs when the callee accepts the call invitation.

 @param callKit An RtmCallKit object.
 @param localInvitation An AgoraRtmLocalInvitation object.
 @param response The response set by the callee.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit localInvitationAccepted:(AgoraRtmLocalInvitation * _Nonnull)localInvitation withResponse:(NSString * _Nullable) response;

/**
 Callback to the caller: occurs when the callee declines the call invitation.

 @param callKit An RtmCallKit object.
 @param localInvitation An AgoraRtmLocalInvitation object.
 @param response The response set by the callee.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit localInvitationRefused:(AgoraRtmLocalInvitation * _Nonnull)localInvitation withResponse:(NSString * _Nullable) response;

/**
 Callback to the caller: occurs when the caller cancels a call invitation.

 @param callKit An RtmCallKit object.
 @param localInvitation An AgoraRtmLocalInvitation object.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit localInvitationCanceled:(AgoraRtmLocalInvitation * _Nonnull)localInvitation;

/**
 Callback to the caller: occurs when the life cycle of the outgoing call invitation ends in failure.

 @param callKit An RtmCallKit object.
 @param localInvitation An AgoraRtmLocalInvitation object.
 @param errorCode See AgoraRtmLocalInvitationErrorCode for the error codes.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit localInvitationFailure:(AgoraRtmLocalInvitation * _Nonnull)localInvitation errorCode:(AgoraRtmLocalInvitationErrorCode) errorCode;

/**
 Callback to the callee: occurs when the callee receives a call invitation.

 @param callKit An RtmCallKit object.
 @param remoteInvitation An AgoraRtmRemoteInvitation object.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit remoteInvitationReceived:(AgoraRtmRemoteInvitation * _Nonnull)remoteInvitation;

/**
 Callback to the callee: occurs when the callee declines a call invitation.

 @param callKit An RtmCallKit object.
 @param remoteInvitation An AgoraRtmRemoteInvitation object.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit remoteInvitationRefused:(AgoraRtmRemoteInvitation * _Nonnull)remoteInvitation;

/**
 Callback to the callee: occurs when the callee accepts a call invitation.

 @param callKit An RtmCallKit object.
 @param remoteInvitation An AgoraRtmRemoteInvitation object.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit remoteInvitationAccepted:(AgoraRtmRemoteInvitation * _Nonnull)remoteInvitation;

/**
 Callback to the callee: occurs when the caller cancels the call invitation.

 @param callKit An RtmCallKit object.
 @param remoteInvitation An AgoraRtmRemoteInvitation object.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit remoteInvitationCanceled:(AgoraRtmRemoteInvitation * _Nonnull)remoteInvitation;

/**
 Callback to the callee: occurs when the life cycle of the incoming call invitation ends in failure.

 @param callKit An RtmCallKit object.
 @param remoteInvitation An AgoraRtmRemoteInvitation object.
 @param errorCode See AgoraRtmRemoteInvitationErrorCode for the error codes.
 */
- (void)rtmCallKit:(AgoraRtmCallKit * _Nonnull)callKit remoteInvitationFailure:(AgoraRtmRemoteInvitation * _Nonnull)remoteInvitation errorCode:(AgoraRtmRemoteInvitationErrorCode) errorCode;
@end

__attribute__((visibility("default"))) @interface AgoraRtmCallKit: NSObject
/** An AgoraRtmKit instance. */
@property (atomic, readonly, weak, nullable) AgoraRtmKit *rtmKit;
/** AgoraRtmCallDelegate enables Agora RTM call callback event notifications to your app. */
@property (nonatomic, weak, nullable) id<AgoraRtmCallDelegate > callDelegate;  // nonatomic

/**
 Allows the caller to send a call invitation to the callee.

 @param localInvitation An AgoraRtmLocalInvitation object.
 @param completion An AgoraRtmLocalInvitationSendBlock object, which indicates the result of sending a call invitation to a callee. See AgoraRtmInvitationApiCallErrorCode for the error codes.

  - Success: 
    - The caller receives the AgoraRtmLocalInvitationSendBlock object with the `AgoraRtmInvitationApiCallErrorOk` state and the [localInvitationReceivedByPeer]([AgoraRtmCallDelegate rtmCallKit:localInvitationReceivedByPeer:]) callback.
    - The callee receives the [remoteInvitationReceived]([AgoraRtmCallDelegate rtmCallKit:remoteInvitationReceived:]) callback.
 - Failure: The caller receives the AgoraRtmLocalInvitationSendBlock object with an error code. See AgoraRtmInvitationApiCallErrorCode for the error codes.
 */
- (void)sendLocalInvitation:(AgoraRtmLocalInvitation * _Nonnull)localInvitation completion:(AgoraRtmLocalInvitationSendBlock _Nullable)completion;

/**
 Allows the caller to cancel a call invitation.

 @param localInvitation An AgoraRtmLocalInvitation object.
 @param completion An AgoraRtmLocalInvitationCancelBlock object.

 - Success: 
    - The caller receives the AgoraRtmLocalInvitationCancelBlock object with the `AgoraRtmInvitationApiCallErrorOk` state and the [localInvitationCanceled]([AgoraRtmCallDelegate rtmCallKit:localInvitationCanceled:]) callback.
    - The callee receives the [remoteInvitationCanceled]([AgoraRtmCallDelegate rtmCallKit:remoteInvitationCanceled:]) callback.
 - Failure: The caller receives the AgoraRtmLocalInvitationCancelBlock object with an error code. See AgoraRtmInvitationApiCallErrorCode for the error codes.

 */
- (void)cancelLocalInvitation:(AgoraRtmLocalInvitation * _Nonnull)localInvitation completion:(AgoraRtmLocalInvitationCancelBlock _Nullable)completion;

/**
 Allows the callee to accept a call invitation.

 @param remoteInvitation An AgoraRtmRemoteInvitation object.
 @param completion An AgoraRtmRemoteInvitationAcceptBlock object.

 - Success:
    - The caller receives the AgoraRtmRemoteInvitationAcceptBlock object with the `AgoraRtmInvitationApiCallErrorOk` state and the [localInvitationAccepted]([AgoraRtmCallDelegate rtmCallKit:localInvitationAccepted:withResponse:]) callback.
    - The callee receives the [remoteInvitationAccepted]([AgoraRtmCallDelegate rtmCallKit:remoteInvitationAccepted:]) callback.
 - Failure: The caller receives the AgoraRtmRemoteInvitationAcceptBlock object with an error code. See AgoraRtmInvitationApiCallErrorCode for the error codes.
 
 */
- (void)acceptRemoteInvitation:(AgoraRtmRemoteInvitation * _Nonnull)remoteInvitation completion:(AgoraRtmRemoteInvitationAcceptBlock _Nullable)completion;

/**
 Allows the callee to decline a call invitation.

 @param remoteInvitation An AgoraRtmRemoteInvitation object.
 @param completion An AgoraRtmRemoteInvitationRefuseBlock object.

 - Success: 
    - The caller receives the AgoraRtmRemoteInvitationRefuseBlock object with the `AgoraRtmInvitationApiCallErrorOk` state and the [localInvitationRefused]([AgoraRtmCallDelegate rtmCallKit:localInvitationRefused:withResponse:]) callback.
    - The callee receives the [remoteInvitationRefused]([AgoraRtmCallDelegate rtmCallKit:remoteInvitationRefused:]) callback.
 - Failure: The caller receives the AgoraRtmRemoteInvitationRefuseBlock object with an error code. See AgoraRtmInvitationApiCallErrorCode for the error codes.
 */
- (void)refuseRemoteInvitation:(AgoraRtmRemoteInvitation * _Nonnull)remoteInvitation completion:(AgoraRtmRemoteInvitationRefuseBlock _Nullable)completion;
@end
