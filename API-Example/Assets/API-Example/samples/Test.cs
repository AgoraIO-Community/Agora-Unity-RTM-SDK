using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using agora_rtm;

public class Test : MonoBehaviour {
	private RtmClient rtmClient;
	private RtmClient rtmClient2;
	private RtmClientEventHandler eventHandler; 
	private RtmClientEventHandler eventHandler2;
	private RtmClientEventHandler eventHandler3;
	private RtmClientEventHandler eventHandler4;
	private RtmClientEventHandler eventHandler5;
	private RtmChannelEventHandler channelEventHandler1;
	private RtmChannelEventHandler channelEventHandler2;
	private RtmChannel rtmChannel1;
	private RtmChannel rtmChannel2;  

	private RtmChannelAttribute[] channelAttributes = new RtmChannelAttribute[2];

	void Awake() {
		eventHandler = new RtmClientEventHandler();
		eventHandler2 = new RtmClientEventHandler();

		channelEventHandler1 = new RtmChannelEventHandler();
		channelEventHandler2 = new RtmChannelEventHandler();
	}

	// Use this for initialization
	void Start () {	
		string version = RtmClient.GetSdkVersion();

		rtmClient2 = new RtmClient(#YOUR_APPID, eventHandler2);
		rtmClient = new RtmClient(#YOUR_APPID, eventHandler);
		rtmClient.SetLogFile("./rtm_log.txt");
		rtmClient.Login("", "100");
		rtmClient2.Login("", "111");

		rtmChannel1 = rtmClient.CreateChannel("222", channelEventHandler1);
		rtmChannel2 = rtmClient.CreateChannel("444", channelEventHandler2);

	    // RtmChannelAttribute channelAttribute = rtmClient.CreateChannelAttribute();
		// channelAttribute.SetKey("2323");
		// channelAttribute.SetValue("5454");

		// RtmChannelAttribute channelAttribute2 = rtmClient.CreateChannelAttribute();
		// channelAttribute2.SetKey("2323233");
		// channelAttribute2.SetValue("5454434");

		// channelAttributes[0] = channelAttribute;
		// channelAttributes[1] = channelAttribute2;
		// ChannelAttributeOptions options = new ChannelAttributeOptions();
		// options.enableNotificationToChannelMembers = false;
		// rtmClient.SetChannelAttributes("123", channelAttributes, options, 2838938);
		
		// channelAttribute = rtmClient.CreateChannelAttribute();
		// channelAttribute.SetKey("287383");
		// channelAttribute.SetValue("kjkkk");


		// int m1 = rtmClient.GetChannelAttributes("2323", 232323);
		// Debug.Log("GetChannelAttributes = " + m1);

		// int m2 = rtmClient.ClearChannelAttributes("232k32", true, 232323);
		// Debug.Log("ClearChannelAttributes = " + m2);

		// int m3 = rtmClient.DeleteChannelAttributesByKeys("32323", new string[]{"23232", "439e"}, true, 2323);
		// Debug.Log("DeleteChannelAttributesByKeys  m3 = " + m3);

		// int m4 = rtmClient.GetUserAttributesByKeys("23232", new string[]{"2323", "4343"}, 2323);
		// Debug.Log("GetUserAttributesByKeys m4 =" + m4);

		// int m5 = rtmClient.GetUserAttributes("2323", 323232);
		// Debug.Log("GetUsetAttribute = "+m5);

		// int m6 = rtmClient.ClearLocalUserAttributes(23232);
		// Debug.Log("ClearLocalUserAttribute = " + m6);

		// int m7 = rtmClient.DeleteLocalUserAttributesByKeys(new string[]{"232", "3232"}, 3232);
		// Debug.Log("DeleteLocalUserAttribute m7 = " + m7);

		rtmChannel1.Join();
		rtmChannel2.Join();

		channelEventHandler1.OnJoinSuccess = OnJoinSuccessHandler1;
		channelEventHandler2.OnJoinSuccess = OnJoinSuccessHandler2;

		channelEventHandler1.OnJoinFailure = OnJoinFailureHandler1;
		channelEventHandler2.OnJoinFailure = OnJoinFailureHandler2;

		channelEventHandler1.OnLeave = OnLeaveHandler;
		channelEventHandler2.OnLeave = OnLeaveHandler2;

		channelEventHandler1.OnMessageReceived = OnMessageReceivedFromChannelHandler;
		channelEventHandler2.OnMessageReceived = OnMessageReceivedFromChannelHandler2;

		channelEventHandler1.OnAttributesUpdated = OnAttributesUpdatedHandler;
		channelEventHandler2.OnAttributesUpdated = OnAttributesUpdatedHandler2;

		channelEventHandler1.OnMemberCountUpdated = OnMemberCountUpdatedHandler;
		channelEventHandler2.OnMemberCountUpdated = OnMemberCountUpdatedHandler2;

		channelEventHandler1.OnMemberJoined = OnMemberJoinedHandler1;
		channelEventHandler2.OnMemberJoined = OnMemberJoinedHandler2;

		channelEventHandler1.OnMemberLeft = OnMemberLeftHandler1;
		channelEventHandler2.OnMemberLeft = OnMemberLeftHandler2;

		channelEventHandler1.OnSendMessageResult = OnSendMessageResultHandler1;

		eventHandler.OnLoginSuccess = OnLoginSuccessHandler;
		eventHandler2.OnLoginSuccess = OnLoginSuccessHandler2;

		eventHandler.OnLogout = OnLogoutHandler;
		eventHandler2.OnLogout = OnLogoutHandler2;

		eventHandler.OnSendMessageResult = OnSendMessageResultHandler;
		eventHandler2.OnSendMessageResult = OnSendMessageResultHandler2;

		eventHandler.OnMessageReceivedFromPeer = OnMessageReceivedFromPeerHandler;
		eventHandler2.OnMessageReceivedFromPeer = OnMessageReceivedFromPeerHandler2;

		eventHandler.OnFileMediaUploadResult = OnFileMediaUploadResultHandler;
		eventHandler2.OnFileMediaUploadResult = OnFileMediaUploadResultHandler2;
		
		eventHandler.OnFileMessageReceivedFromPeer = OnFileMessageReceivedFromPeerHandler;
		eventHandler2.OnFileMessageReceivedFromPeer = OnFileMessageReceivedFromPeerHandler2;

		eventHandler.OnImageMessageReceivedFromPeer = OnImageMessageReceivedFromPeerHandler;
		eventHandler2.OnImageMessageReceivedFromPeer = OnImageMessageReceivedFromPeerHandler2;

		eventHandler.OnImageMediaUploadResult = OnImageMediaUploadResultHandler;
		eventHandler2.OnImageMediaUploadResult = OnImageMediaUploadResultHandler2;

		eventHandler.OnMediaUploadingProgress = OnMediaUploadingProgressHandler;
		eventHandler2.OnMediaUploadingProgress = OnMediaUploadingProgressHandler2;

		eventHandler.OnMediaDownloadingProgress = OnMediaDownloadingProgressHandler;
		eventHandler2.OnMediaDownloadingProgress = OnMediaDownloadingProgressHandler2;
		
		eventHandler.OnMediaDownloadToFileResult = OnMediaDownloadToFileResultHandler;
		eventHandler2.OnMediaDownloadToFileResult = OnMediaDownloadToFileResultHandler2;

		eventHandler.OnMediaDownloadToMemoryResult = OnMediaDownloadToMemoryResultHandler;
		eventHandler2.OnMediaDownloadToMemoryResult = OnMediaDownloadToMemoryResultHandler2;

		eventHandler.OnGetChannelAttributesResult = OnGetChannelAttributesResultHandler;
		eventHandler2.OnGetChannelAttributesResult = OnGetChannelAttributesResultHandler2;

		eventHandler.OnSetChannelAttributesResult = OnSetChannelAttributesResultHandler;
		eventHandler2.OnSetChannelAttributesResult = OnSetChannelAttributesResultHandler2;

		eventHandler.OnQueryPeersBySubscriptionOptionResult = OnQueryPeersBySubscriptionOptionResultHandler;
		eventHandler2.OnQueryPeersBySubscriptionOptionResult = OnQueryPeersBySubscriptionOptionResultHandler2;

		eventHandler.OnQueryPeersOnlineStatusResult = OnQueryPeersOnlineStatusResultHandler;
		eventHandler2.OnQueryPeersOnlineStatusResult = OnQueryPeersOnlineStatusResultHandler2;

		eventHandler.OnSubscriptionRequestResult = OnSubscriptionRequestResultHandler;
		eventHandler2.OnSubscriptionRequestResult = OnSubscriptionRequestResultHandler2;
	}

	void Update() {

	}

	void OnApplicationQuit() {
		Debug.Log("OnApplicationQuit");
		rtmClient.Release(true);
		rtmClient2.Release(true);
	}
	RtmCallManager rtmCallManager;
	RtmCallEventHandler rtmCallEventHandler;
	LocalInvitation localInvitation;
	void OnLoginSuccessHandler(int id) {
		// TextMessage message = rtmClient.CreateMessage();
		// message.SetText("Hello unity rtm");
		// int ret = rtmClient.SendMessageToPeer("111", message);
		// int ret1 = rtmClient.CreateFileMessageByUploading("/Users/zhangtao/Documents/work/Unitywork/unity_rtm/API-Example/API-Example.sln", 2323);
		// int ret2 = rtmClient.CreateImageMessageByUploading("/Users/zhangtao/Documents/work/Unitywork/unity_rtm/API-Example/test.jpg", 342343);

	}

	void OnLocalInvitationReceivedByPeerHandler(LocalInvitation localInvitation) {
		Debug.Log("rtmCallManager  OnLocalInvitationReceivedByPeerHandler " + localInvitation.GetCalleeId() + "  " + localInvitation.GetChannelId());
	}
	void OnLoginSuccessHandler2(int id){
		
		Debug.Log("client2 OnLoginSuccess id = " + id);
	}

	void OnJoinSuccessHandler1(int id){
		TextMessage message3 = rtmClient.CreateMessage();
		message3.SetText("Hello unity rtm3");
		rtmChannel1.SendMessage(message3);
		Debug.Log("client  OnJoinSuccess id = " + id);

		// rtmClient.QueryPeersOnlineStatus(new string[] {"100", "111", "222", "333", "444"}, 23323);
		// rtmClient.QueryPeersBySubscriptionOption(PEER_SUBSCRIPTION_OPTION.PEER_SUBSCRIPTION_OPTION_ONLINE_STATUS, 23234324);

		// rtmClient.SubscribePeersOnlineStatus(new string[] {"100", "111", "222", "333", "444"}, 3, 3434);

	}

	void OnRemoteInvitationReceivedHandler(RemoteInvitation remoteInvitation) {
		Debug.Log("OnRemoteInvitationReceivedHandler  " + remoteInvitation.GetCallerId() + "  " + remoteInvitation.GetChannelId());
	}

	void OnJoinFailureHandler1(int id, JOIN_CHANNEL_ERR errorCode) {
		Debug.Log("client OnJoinFailure id = " + id);
	}

	void OnJoinSuccessHandler2(int id) {
		Debug.Log("client2  OnJoinSuccess --- id = " + id);
		rtmCallEventHandler = new RtmCallEventHandler();
		rtmCallEventHandler.OnLocalInvitationReceivedByPeer = OnLocalInvitationReceivedByPeerHandler;
		rtmCallEventHandler.OnRemoteInvitationReceived = OnRemoteInvitationReceivedHandler;
		rtmCallManager = rtmClient.GetRtmCallManager(rtmCallEventHandler);
		localInvitation = rtmCallManager.CreateLocalCallInvitation("222");
		rtmCallManager.SendLocalInvitation(localInvitation);
		localInvitation.SetChannelId("1212");
		localInvitation.SetContent("23232");
	}
	void OnJoinFailureHandler2(int id, JOIN_CHANNEL_ERR errorCode) {
		Debug.Log("client2 OnJoinFailure id = " + id + "  error: " + errorCode);
	}

	void OnLeaveHandler(int id, LEAVE_CHANNEL_ERR errorCode) {
		Debug.Log("client  OnLeave id = " + id + "  error:" + errorCode);
	}

	void OnLeaveHandler2(int id, LEAVE_CHANNEL_ERR errorCode) {
		Debug.Log("client2  OnLeave id = " + id + "  error:" + errorCode);
	}

	void OnLogoutHandler(int id, LOGOUT_ERR_CODE errorCode) {
		Debug.Log("client OnLogout id = " + id + "  error: " + errorCode);
	}

	void OnLogoutHandler2(int id, LOGOUT_ERR_CODE errorCode) {
		Debug.Log("client2 OnLogout id = " + id + "  error: " + errorCode);
	}

	void OnSendMessageResultHandler(int id, Int64 messageId, PEER_MESSAGE_ERR_CODE errorCode) {
		Debug.Log("client  OnSendMessageResultHandler id = " + id + " ,messageId = " + messageId + " ,errorCode = " + errorCode);
	}

	void OnSendMessageResultHandler2(int id, Int64 messageId, PEER_MESSAGE_ERR_CODE errorCode) {
		Debug.Log("client2  OnSendMessageResultHandler id = " + id + " ,messageId = " + messageId + " ,errorCode = " + errorCode);
	}

	void OnMessageReceivedFromPeerHandler(int id, string peerId, TextMessage message) {
		Debug.Log("client  OnMessageReceivedFromPeerHandler id = " + id + " ,peerId = " + peerId  + " ,message = " + message.GetText() + " ,MessageId = " + message.GetMessageId() + " ,_IsOfflineMessage" + message.IsOfflineMessage() + " ,_Ts = " + message.GetServerReceiveTs() + " ,length = " + message.GetRawMessageLength());
	}

	void OnMessageReceivedFromPeerHandler2(int id, string peerId, TextMessage message) {
		Debug.Log("client  OnMessageReceivedFromPeerHandler id = " + id + " ,peerId = " + peerId  + " ,message = " + message.GetText() + " ,MessageId = " + message.GetMessageId() + " ,_IsOfflineMessage" + message.IsOfflineMessage() + " ,_Ts = " + message.GetServerReceiveTs() + " ,length = " + message.GetRawMessageLength());
	}

	void OnMessageReceivedFromChannelHandler(int id, string userId, TextMessage message) {
		Debug.Log("client  OnMessageReceivedFromPeerHandler id = " + id + " ,userId = " + userId  + " ,message = " + message.GetText() + " ,MessageId = " + message.GetMessageId() + " ,_IsOfflineMessage" + message.IsOfflineMessage() + " ,_Ts = " + message.GetServerReceiveTs() + " ,length = " + message.GetRawMessageLength());
	}

	void OnMessageReceivedFromChannelHandler2(int id, string userId, TextMessage message) {
		Debug.Log("client  OnMessageReceivedFromPeerHandler id = " + id + " ,userId = " + userId  + " ,message = " + message.GetText() + " ,MessageId = " + message.GetMessageId() + " ,_IsOfflineMessage" + message.IsOfflineMessage() + " ,_Ts = " + message.GetServerReceiveTs() + " ,length = " + message.GetRawMessageLength());
	}

	void OnMemberJoinedHandler1(int id, RtmChannelMember member) {
		Debug.Log("channel OnMemberJoinedHandler1 member = userID =" + member.GetUserId() + " channelId = " + member.GetChannelId());
	}

	void OnMemberJoinedHandler2(int id, RtmChannelMember member) {
		Debug.Log("channel OnMemberJoinedHandler2 member = userID =" + member.GetUserId() + " channelId = " + member.GetChannelId());
	}

	void OnMemberLeftHandler1(int id, RtmChannelMember member) {
		Debug.Log("channel OnMemberLeftHandler1 member = userID =" + member.GetUserId() + " channelId = " + member.GetChannelId());
	}

	void OnMemberLeftHandler2(int id, RtmChannelMember member) {
		Debug.Log("channel OnMemberLeftHandler2 member = userID =" + member.GetUserId() + " channelId = " + member.GetChannelId());
	}

	void OnSendMessageResultHandler1(int id, Int64 messageId, CHANNEL_MESSAGE_ERR_CODE errorCode) {
		Debug.Log("channel OnSendMessageResultHandler1 messageId = " +messageId + " ,errorCode = " + errorCode);
	}

	void OnFileMediaUploadResultHandler(int id, Int64 requestId, FileMessage fileMessage, UPLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("client OnFileMediaUploadResultHandler id = " + id + " fileMessage = " + fileMessage.GetFileName() + " messageId = " + fileMessage.GetMessageId() + " error: " + code);
		fileMessage.SetFileName("hehe.txt");
		byte [] data = new byte[1024];
		fileMessage.SetThumbnail(data);
		fileMessage.SetText("jhjehekek");
		rtmClient.SendMessageToPeer("111", fileMessage);	

		fileMessage.SetFileName("hehe1.txt");
		byte [] data1 = new byte[1024];
		fileMessage.SetThumbnail(data1);
		fileMessage.SetText("jhjehekek22");
		rtmClient.SendMessageToPeer("111", fileMessage);	
	}

	void OnFileMediaUploadResultHandler2(int id, Int64 requestId, FileMessage fileMessage, UPLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("client OnFileMediaUploadResultHandler id = " + id + " fileMessage = " + fileMessage.GetFileName() + " messageId = " + fileMessage.GetMessageId() + " error: " + code);
		fileMessage.SetFileName("hehe.txt");
		byte [] data = new byte[1024];
		fileMessage.SetThumbnail(data);
		fileMessage.SetText("jhjehekek");
		rtmClient.SendMessageToPeer("100", fileMessage);	

		fileMessage.SetFileName("hehe1.txt");
		byte [] data1 = new byte[1024];
		fileMessage.SetThumbnail(data1);
		fileMessage.SetText("jhjehekek22");
		rtmClient.SendMessageToPeer("100", fileMessage);	
	}

	void OnFileMessageReceivedFromPeerHandler(int id, string peerId, FileMessage message) {
		Debug.Log("OnFileMessageReceivedFromPeerHandler id = " + id + " ,peerId = " + peerId + " fileMessage, = " + message.GetFileName() + "   " + message.GetMediaId() + "  " + message.GetRawMessageLength() + "  " + message.GetServerReceiveTs() + "  " + message.IsOfflineMessage());
		// FileMessage message =
		// FileMessage fileMessage = rtmClient.CreateFileMessageByMediaId(message.GetMediaId());
		// Debug.Log("CreateFileMessageByMediaId" + " fileMessage, = " + fileMessage.GetFileName() + "   " + fileMessage.GetMediaId() + "  " + fileMessage.GetRawMessageLength() + "  " + fileMessage.GetServerReceiveTs() + "  " + fileMessage.IsOfflineMessage());
		rtmClient.DownloadMediaToMemory(message.GetMediaId(), 1982939);
		rtmClient.DownloadMediaToFile(message.GetMediaId(), "/Users/zhangtao/Documents/work/Unitywork/unity_rtm/API-Example/test_download_file.txt", 1322323);
		rtmClient.CancelMediaDownload(20203030);
		rtmClient.CancelMediaUpload(3992383);
	}

	
	void OnFileMessageReceivedFromPeerHandler2(int id, string peerId, FileMessage message) {
		Debug.Log("----------- id = " + id + " ,peerId = " + peerId + " fileMessage, = " + message.GetFileName() + "   " + message.GetMediaId() + "  " + message.GetRawMessageLength() + "  " + message.GetServerReceiveTs() + "  " + message.IsOfflineMessage());
		//fileMessage = rtmClient.CreateFileMessageByMediaId(message.GetMediaId());
		//Debug.Log("------------" + " fileMessage, = " + fileMessage.GetFileName() + "   " + fileMessage.GetMediaId() + "  " + fileMessage.GetRawMessageLength() + "  " + fileMessage.GetServerReceiveTs() + "  " + fileMessage.IsOfflineMessage());
		rtmClient.DownloadMediaToMemory(message.GetMediaId(), 1982939);
		rtmClient.DownloadMediaToFile(message.GetMediaId(), "/Users/zhangtao/Documents/work/Unitywork/unity_rtm/API-Example/test_download_file.txt", 1322323);
	}



	void OnImageMediaUploadResultHandler(int id, Int64 requestId, ImageMessage imageMessage, UPLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("-----OnImageMediaUploadResultHandler------ id = " + id + " ,requestId = " + requestId + " imageMessage, = " + imageMessage.GetFileName() + "   " + imageMessage.GetMediaId() + "  " + imageMessage.GetRawMessageLength() + "  " + imageMessage.GetServerReceiveTs() + "  " + imageMessage.IsOfflineMessage() + "  ,errorCode = " + code);
	}

	void OnImageMediaUploadResultHandler2(int id, Int64 requestId, ImageMessage imageMessage, UPLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("-----OnImageMediaUploadResultHandler2------ id = " + id + " ,requestId = " + requestId + " imageMessage, = " + imageMessage.GetFileName() + "   " + imageMessage.GetMediaId() + "  " + imageMessage.GetRawMessageLength() + "  " + imageMessage.GetServerReceiveTs() + "  " + imageMessage.IsOfflineMessage() + "  ,errorCode = " + code);
	}

	void OnMediaUploadingProgressHandler(int id, Int64 requestId, MediaOperationProgress progress) {
		Debug.Log("OnMediaUploadingProgressHandler requestId = " + requestId + " totalSize = " + progress.totalSize + " currentSize = " + progress.currentSize); 
	}

	void OnMediaUploadingProgressHandler2(int id, Int64 requestId, MediaOperationProgress progress) {
		Debug.Log("OnMediaUploadingProgressHandler2 requestId = " + requestId + " totalSize = " + progress.totalSize + " currentSize = " + progress.currentSize); 
	}	

	void OnMediaDownloadingProgressHandler(int id, Int64 requestId, MediaOperationProgress progress) {
		Debug.Log("OnMediaDownloadingProgressHandler requestId = " + requestId + " totalSize = " + progress.totalSize + " currentSize = " + progress.currentSize); 
	}

	void OnMediaDownloadingProgressHandler2(int id, Int64 requestId, MediaOperationProgress progress) {
		Debug.Log("OnMediaDownloadingProgressHandler2 requestId = " + requestId + " totalSize = " + progress.totalSize + " currentSize = " + progress.currentSize); 
	}


	void OnMediaDownloadToFileResultHandler(int id, Int64 requestId, DOWNLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("OnMediaDownloadToFileResultHandler requestId = " + requestId + " error = " + code);
	}


	void OnMediaDownloadToFileResultHandler2(int id, Int64 requestId, DOWNLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("OnMediaDownloadToFileResultHandler2 requestId = " + requestId + " error = " + code);
	}

	void OnMediaDownloadToMemoryResultHandler(int id, Int64 requestId, byte[] memory, Int64 length, DOWNLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("OnMediaDownloadToMemoryResultHandler requestId = " + requestId + " ,length = " + length);
		
	}

	void OnMediaDownloadToMemoryResultHandler2(int id, Int64 requestId, byte[] memory, Int64 length, DOWNLOAD_MEDIA_ERR_CODE code) {
		Debug.Log("OnMediaDownloadToMemoryResultHandler2 requestId = " + requestId + " ,length = " + length);
	}

	void OnImageMessageReceivedFromPeerHandler(int id, string peerId, ImageMessage message) {
		Debug.Log("OnImageMessageReceivedFromPeerHandler  peerId = " + peerId);
		rtmClient.DownloadMediaToMemory(message.GetMediaId(), 232312);
		rtmClient.DownloadMediaToFile(message.GetMediaId(), "/Users/zhangtao/Documents/work/Unitywork/unity_rtm/API-Example/test_download_image.jpg", 342323);
	}

	void OnImageMessageReceivedFromPeerHandler2(int id, string peerId, ImageMessage message) {
		Debug.Log("OnImageMessageReceivedFromPeerHandler  peerId = " + peerId);
			rtmClient.DownloadMediaToMemory(message.GetMediaId(), 232312);
		rtmClient.DownloadMediaToFile(message.GetMediaId(), "/Users/zhangtao/Documents/work/Unitywork/unity_rtm/API-Example/test_download_image.jpg", 342323);
	}

	void OnGetChannelAttributesResultHandler(int id, Int64 requestId, RtmChannelAttribute[] attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode) {
		for(int i = 0; i < numberOfAttributes; i ++) {
			Debug.Log("OnGetChannelAttributesResultHandler " + attributes[i].GetValue());
		}
	}

	void OnGetChannelAttributesResultHandler2(int id, Int64 requestId, RtmChannelAttribute[] attributes, int numberOfAttributes, ATTRIBUTE_OPERATION_ERR errorCode) {
		for(int i = 0; i < numberOfAttributes; i ++) {
			Debug.Log("OnGetChannelAttributesResultHandler " + attributes[i].GetValue());
		}
	}

	void OnSetChannelAttributesResultHandler(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
		Debug.Log("OnSetChannelAttributesResultHandler error: " + errorCode);
		int m = rtmClient.GetChannelAttributesByKeys("222", new string[] {"2323", "234"}, 2333);
		Debug.Log("GetChannelAttributesByKeys m = " + m);
	}

	void OnSetChannelAttributesResultHandler2(int id, Int64 requestId, ATTRIBUTE_OPERATION_ERR errorCode) {
		Debug.Log("OnSetChannelAttributesResultHandler error: " + errorCode);
		int m = rtmClient.GetChannelAttributesByKeys("444", new string[] {"2323", "234"}, 2333);
		Debug.Log("GetChannelAttributesByKeys m = " + m);
	}

	void OnQueryPeersBySubscriptionOptionResultHandler(int id, Int64 requestId, string[] peerIds, int peerCount, QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode) {
		Debug.Log("OnQueryPeersBySubscriptionOptionResultHandler requestId = " + requestId + " ,peerId = " + peerIds[0] + "  ,error = " + errorCode);
	}

	void OnQueryPeersBySubscriptionOptionResultHandler2(int id, Int64 requestId, string[] peerIds, int peerCount, QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR errorCode) {
		Debug.Log("OnQueryPeersBySubscriptionOptionResultHandler requestId = " + requestId + " ,peerId = " + peerIds[0] + "  ,error = " + errorCode);
	}

	void OnQueryPeersOnlineStatusResultHandler(int id, Int64 requestId, PeerOnlineStatus[] peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode) {
		Debug.Log("OnQueryPeersOnlineStatusResultHandler requestId = " + requestId + " peersStatus " + peersStatus[0].peerId + "  " + peersStatus[0].isOnline + "  " + peersStatus[0].peerId);
	}

	void OnQueryPeersOnlineStatusResultHandler2(int id, Int64 requestId, PeerOnlineStatus[] peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode) {
		Debug.Log("OnQueryPeersOnlineStatusResultHandler requestId = " + requestId + " peersStatus " + peersStatus[0].peerId + "  " + peersStatus[0].isOnline + "  " + peersStatus[0].peerId);
	}

	void OnSubscriptionRequestResultHandler(int id, Int64 requestId, PEER_SUBSCRIPTION_STATUS_ERR errorCode) {
		Debug.Log("OnSubscriptionRequestResultHandler  requestId = " + requestId);
	}

	void OnSubscriptionRequestResultHandler2(int id, Int64 requestId, PEER_SUBSCRIPTION_STATUS_ERR errorCode) {
		Debug.Log("OnSubscriptionRequestResultHandler  requestId = " + requestId);
	}

	void OnAttributesUpdatedHandler(int id, RtmChannelAttribute[] attributesList, int numberOfAttributes) {
		Debug.Log("channel OnAttributesUpdatedHandler key = " + attributesList[0].GetKey() + "  ,value = " + attributesList[0].GetValue());
	}

	void OnAttributesUpdatedHandler2(int id, RtmChannelAttribute[] attributesList, int numberOfAttributes) {
		Debug.Log("channel OnAttributesUpdatedHandler key = " + attributesList[0].GetKey() + "  ,value = " + attributesList[0].GetValue());
	}

	void OnMemberCountUpdatedHandler(int id, int memberCount) {
		Debug.Log("OnMemberCountUpdatedHandler memberCount = " + memberCount);
	}

	void OnMemberCountUpdatedHandler2(int id, int memberCount) {
		Debug.Log("OnMemberCountUpdatedHandler2 memberCount = " + memberCount);
	}
}
