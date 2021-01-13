*Read this in other language: [English](README.md)*

<br />
<p align="center">
  <h1 align="center">Agora Unity RTM SDK</h1>
</p>
</br>


## 目录
* [内容](#内容)
* [快速开始](#快速开始)
* [SDK链接](#SDK链接)
* [API文档](#API文档)

**该项目包含 Unity RTM SDK 源码和 Demo。请使用 SDK Release 部分下载 Unity package，然后将 .unitypackage 导入你的项目中**

## 内容

 - **API-Example** : 简单的 API 调用示例，显示 Unity RTM SDK API 调用的用法。
 - **Unity-RTM-Demo**: Unity演示，演示了 SDK 的主要功能（登录，频道加入，发送短信，发送图像，邀请用户等）。该演示还包含在 release 的 unitypackage 中。
 - **unity_rtm_sdk**: Unity RTM SDK 的源代码。你可以使用源代码编译 Unity RTM SDK。

## 快速开始
```
// 创建 RtmClient 需要的回调事件的 RtmClientEventHandler 实例
RtmClientEventHandler eventHandler = new RtmClientEventHandler();
eventHandler.OnLoginSuccess = OnLoginSuccessHandler;
eventHandler.OnLogout = OnLogoutHandler;
eventHandler.OnSendMessageResult = OnSendMessageResultHandler;
eventHandler.OnMessageReceivedFromPeer = OnMessageReceivedFromPeerHandler;
eventHandler.OnFileMediaUploadResult = OnFileMediaUploadResultHandler;
.......

// 创建 RtmChannel 需要的回调事件的 RtmChannelEventHandler 实例
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

// 获取 SDK 版本号
string version = RtmClient.GetSdkVersion();

// 创建 RTM 引擎实例，你可以创建多实例。
RtmClient rtmClient = new RtmClient(#YOUR_APPID, eventHandler);
rtmClient.Login("", "100");

// 创建 RtmChannel 实例，你可以创建多实例。
RtmChannel rtmChannel1 = rtmClient.CreateChannel("channel_name", channelEventHandler);
RtmChannel rtmChannel2 = rtmClient.CreateChannel("channel_name2", channelEventHandler2);

rtmChannel1.Join();
rtmChannel2.Join();

// 最后，当你收到 OnJoinSuccess 回调后，你可以通过 RtmChannel 实例向频道里发送消息
//当你收到 OnLoginSuccess 后，可以通过 RtmClient 实例发送点对点消息
	
```

## SDK链接
- Please check the right-hand side in [the Release section](https://github.com/AgoraIO-Community/Unity-RTM/releases).  This Repo does not contain the actual plugins.

## API文档

 - [API Reference](https://docs.agora.io/en/Real-time-Messaging/API%20Reference/RTM_java/index.html)

## License
The MIT License (MIT).


