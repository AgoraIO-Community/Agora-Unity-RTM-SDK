*其他语言版本： [简体中文](README.zh.md)*

<br />
<p align="center">
  <h1 align="center">Agora Unity RTM SDK</h1>
</p>
</br>

## Table of Contents
* [Contents](#Contents)
* [QuickStart](#QuickStart)
* [SDK Release](#SDK_Release)
* [API_Reference](#API_Reference)

**This project contains source code and demo of  Agora Unity RTM SDK.   Please use the release section to download the Unity package for the SDK import.  The repository contains three subfolders that provide further description in them.**

## Contents

 - **API-Example** : Simple API call example that shows the usage of the RTM API calls.
 - **Unity-RTM-Demo**: Unity demo that illustrates main features of the SDK (login, channel joining, sending text message, ending image, invite user, etc.). The demo is also included in the release package.
 - **unity_rtm_sdk**: Source code of Unity RTM SDK. You can build Unity RTM SDK by source code.
 


## QuickStart
```
// new instance of client event handler and init delegate of event.
RtmClientEventHandler eventHandler = new RtmClientEventHandler();
eventHandler.OnLoginSuccess = OnLoginSuccessHandler;
eventHandler.OnLogout = OnLogoutHandler;
eventHandler.OnSendMessageResult = OnSendMessageResultHandler;
eventHandler.OnMessageReceivedFromPeer = OnMessageReceivedFromPeerHandler;
eventHandler.OnFileMediaUploadResult = OnFileMediaUploadResultHandler;
.......

// new instance of channel event handler and init delegate of event.
RtmChannelEventHandler channelEventHandler = new RtmChannelEventHandler();
channelEventHandler.OnJoinSuccess = OnJoinSuccessHandler1;
channelEventHandler.OnJoinFailure = OnJoinFailureHandler1;
channelEventHandler.OnLeave = OnLeaveHandler;
channelEventHandler.OnMessageReceived = OnMessageReceivedFromChannelHandler;
channelEventHandler.OnMemberCountUpdated = OnMemberCountUpdatedHandler;
channelEventHandler.OnMemberJoined = OnMemberJoinedHandler1;
channelEventHandler.OnMemberLeft = OnMemberLeftHandler1;

RtmChannelEventHandler channelEventHandler2 = new RtmChannelEventHandler();
channelEventHandler2.OnJoinSuccess = OnJoinSuccessHandler2;
channelEventHandler2.OnJoinFailure = OnJoinFailureHandler2;
channelEventHandler2.OnLeave = OnLeaveHandler2;
channelEventHandler2.OnMessageReceived = OnMessageReceivedFromChannelHandler2;
channelEventHandler2.OnMemberCountUpdated = OnMemberCountUpdatedHandler2;
channelEventHandler2.OnMemberJoined = OnMemberJoinedHandler2;
channelEventHandler2.OnMemberLeft = OnMemberLeftHandler2;

// Get SDK Version
string version = RtmClient.GetSdkVersion();

// Create RTM Engine instance,you can create multi instance of RTM Engine
RtmClient rtmClient = new RtmClient(#YOUR_APPID, eventHandler);
rtmClient.Login("", "100");

// Create instance of RtmChannel, you can create multi instance of RtmChannel.
RtmChannel rtmChannel1 = rtmClient.CreateChannel("channel_name", channelEventHandler);
RtmChannel rtmChannel2 = rtmClient.CreateChannel("channel_name2", channelEventHandler2);

rtmChannel1.Join();
rtmChannel2.Join();

// At last, you can send message  by RtmChannel  after you get OnJoinSuccess callback instance.
//you can also send message by RtmClient instance after you get OnLoginSuccess callback.
	
```

## SDK_Release
- Please check the right-hand side in [the Release section](https://github.com/AgoraIO-Community/Unity-RTM/releases).  This Repo does not contain the actual plugins.

## API_Reference

 - [API Reference](https://docs.agora.io/en/Real-time-Messaging/API%20Reference/RTM_java/index.html)

## License
The MIT License (MIT).