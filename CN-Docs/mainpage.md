Agora Real-time Messaging (RTM) SDK 提供了稳定可靠、低延时、高并发的全球消息云服务，帮助你快速构建实时场景。

> 如无特别说明，本页列出的大多数 RTM 核心业务方法都应在调用 \ref agora_rtm.RtmClient.Login "login" 方法成功收到 \ref agora_rtm.RtmClientEventHandler.OnLoginSuccessHandler "OnLoginSuccessHandler" 回调后调用。

Agora RTM SDK 提供以下功能：

- [登录登出](#登录登出相关)
- [点对点消息](#peermessage)
- [查询单个或多个用户的在线状态](#onlinestatus)
- [订阅或退订单个或多个指定用户的在线状态](#subscribe)
- [用户属性增删改查](#attributes)
- [频道属性增删改查](#channelattributes)
- [查询单个或多个频道的成员人数](#channelmembercount)
- [加入离开频道相关](#joinorleavechannel)
- [频道消息](#channelmessage)
- [获取频道成员列表](#memberlist)
- [呼叫邀请管理](#callinvitation)
- [更新当前的 RTM Token](#renewtoken)
- [日志设置与版本查询](#logfile)
- [定制方法](#customization)

<a name="登录登出"></a>
## 登录登出相关

> SDK 与 RTM 服务器的连接状态是 RTM 开发过程需要理解的核心概念，详见[连接状态管理](https://docs.agora.io/cn/Real-time-Messaging/reconnecting_cpp?platform=Linux%20CPP).

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.RtmClient "RtmClient"</td>
<td>创建并返回一个 <code>RtmClient</code> 实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.Login "Login"</td>
<td>登录 Agora RTM 系统。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.Logout "Logout"</td>
<td>登出 Agora RTM 系统。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.Dispose "Dispose"</td>
<td>释放当前 <code>RtmClient</code> 实例使用的所有资源。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnLoginSuccessHandler "OnLoginSuccessHandler"</td>
<td>登录 Agora RTM 系统成功回调。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnLoginFailureHandler "OnLoginFailureHandler"</td>
<td>登录 Agora RTM 系统失败回调。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnLogoutHandler "OnLogoutHandler"</td>
<td>登出 Agora RTM 系统回调。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnConnectionStateChangedHandler "OnConnectionStateChangedHandler"</td>
<td>SDK 与 Agora RTM 系统连接状态发生改变回调。</td>
</tr>
</table>

<a name="点对点消息"></a>
## 点对点消息

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage() "CreateMessage"</td>
<td>创建并返回一个空文本 <code>TextMessage</code> 消息实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage(string text) "CreateMessage"</td>
<td>创建并返回一个文本 <code>TextMessage</code> 消息实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage(byte[] rawData) "CreateMessage"</td>
<td>创建并返回一个自定义二进制 <code>TextMessage</code> 消息实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage(byte[] rawData, string description ) "CreateMessage"</td>
<td>创建并返回一个包含文字描述的自定义二进制 <code>TextMessage</code> 消息实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.SendMessageToPeer(string peerId, IMessage message, SendMessageOptions options) "SendMessageToPeer"</td>
<td>向指定用户发送点对点消息或点对点的离线消息。当带有前缀时，该方法类似于 Agora 信令 SDK 中的 <code>endCall</code> 方法.</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.Dispose "Dispose"</td>
<td>释放当前 <code>TextMessage</code> 实例使用的所有资源。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnSendMessageResultHandler "OnSendMessageResultHandler"</td>
<td>报告 <code>SendMessageToPeer(string peerId, IMessage message, SendMessageOptions options)</code> 或 <code>SendMessageToPeer(string peerId, IMessage message)</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnMessageReceivedFromPeerHandler "OnMessageReceivedFromPeerHandler"</td>
<td>收到来自对端的点对点消息回调。</td>
</tr>
</table>


<a name="用户在线状态"></a>
## 查询单个或多个用户的在线状态

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.QueryPeersOnlineStatus "QueryPeersOnlineStatus"</td>
<td>查询单个或多个指定用户的在线状态。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnQueryPeersOnlineStatusResultHandler "OnQueryPeersOnlineStatusResultHandler"</td>
<td>报告 <code>QueryPeersOnlineStatus</code> 方法的调用结果。</td>
</tr>
</table>

<a name="订阅"></a>
## 订阅或取消订阅单个或多个指定用户的在线状态

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.SubscribePeersOnlineStatus "SubscribePeersOnlineStatus"</td>
<td>订阅指定单个或多个用户的在线状态。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.UnsubscribePeersOnlineStatus "UnsubscribePeersOnlineStatus"</td>
<td>取消订阅指定单个或多个用户的在线状态。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.QueryPeersBySubscriptionOption "QueryPeersBySubscriptionOption"</td>
<td>获取某特定内容被订阅的用户列表。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnSubscriptionRequestResultHandler "OnSubscriptionRequestResultHandler"</td>
<td>报告 <code>SubscribePeersOnlineStatus</code> 或 <code>UnsubscribePeersOnlineStatus</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnPeersOnlineStatusChangedHandler "OnPeersOnlineStatusChangedHandler"</td>
<td>被订阅用户在线状态改变回调。查看指定用户的在线状态，详见 <code>PeerOnlineStatus</code>。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnQueryPeersBySubscriptionOptionResultHandler "OnQueryPeersBySubscriptionOptionResultHandler"</td>
<td>报告 <code>QueryPeersBySubscriptionOption</code> 方法的调用结果。</td>
</tr>
</table>


<a name="属性"></a>
## 用户属性增删改查

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.DeleteLocalUserAttributesByKeys "DeleteLocalUserAttributesByKeys"</td>
<td>删除本地用户的指定属性。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.ClearLocalUserAttributes "ClearLocalUserAttributes"</td>
<td>清空本地用户的属性。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.GetUserAttributes "GetUserAttributes"</td>
<td>查询指定用户的全部属性。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.GetUserAttributesByKeys "GetUserAttributesByKeys"</td>
<td>查询指定用户指定属性名的属性。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnDeleteLocalUserAttributesResultHandler "OnDeleteLocalUserAttributesResultHandler"</td>
<td> 报告 <code>DeleteLocalUserAttributesByKeys</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnClearLocalUserAttributesResultHandler "OnClearLocalUserAttributesResultHandler"</td>
<td> 报告 <code>ClearLocalUserAttributes</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnGetUserAttributesResultHandler "OnGetUserAttributesResultHandler"</td>
<td> 报告 <code>GetUserAttributes</code> 方法或 <code>GetUserAttributesByKeys</code> 方法的调用结果。</td>
</tr>
</table>

<a name="频道属性"></a>
## 频道属性增删改查

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateChannelAttribute "CreateChannelAttribute"</td>
<td>创建并返回一个 <code>RtmChannelAttribute</code> 实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.SetChannelAttributes "SetChannelAttributes"</td>
<td>全量设置某指定频道的属性。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.DeleteChannelAttributesByKeys "DeleteChannelAttributesByKeys"</td>
<td>删除某指定频道的指定属性。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.ClearChannelAttributes "ClearChannelAttributes"</td>
<td>清空某指定频道的属性。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.GetChannelAttributes "GetChannelAttributes"</td>
<td>查询某指定频道的全部属性。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.GetChannelAttributesByKeys "GetChannelAttributesByKeys"</td>
<td>查询某指定频道指定属性名的属性。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnSetChannelAttributesResultHandler "OnSetChannelAttributesResultHandler"</td>
<td> 报告 <code>SetChannelAttributes</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnDeleteChannelAttributesResultHandler "OnDeleteChannelAttributesResultHandler"</td>
<td> 报告 <code>DeleteChannelAttributesByKeys</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnClearChannelAttributesResultHandler "OnClearChannelAttributesResultHandler"</td>
<td> 报告 <code>ClearChannelAttributes</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnGetChannelAttributesResultHandler "OnGetChannelAttributesResultHandler"</td>
<td> 报告 <code>GetChannelAttributes</code> or <code>GetChannelAttributesByKeys</code> 方法的调用结果。</td>
</tr>
</table>

<table>
<tr>
<th>频道属性更新事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnAttributesUpdatedHandler "OnAttributesUpdatedHandler"</td>
<td>当频道属性更新时返回当前频道的所有属性。</td>
</tr>
</table>


<a name="频道成员人数"></a>
## 查询单个或多个频道的成员人数

<table>
<tr>
<th>方法</th>
<th>事件</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.GetChannelMemberCount "GetChannelMemberCount"</td>
<td>查询单个或多个频道的成员人数。用户无需加入指定频道即可调用该方法。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnGetChannelMemberCountResultHandler "OnGetChannelMemberCountResultHandler"</td>
<td>返回 <code>GetChannelMemberCount</code> 方法的调用结果。</td>
</tr>
</table>


<a name="加入离开频道"></a>
## 加入离开频道相关

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateChannel "CreateChannel"</td>
<td>创建一个 <code>RtmChannel</code> 对象。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannel.Join "Join"</td>
<td>加入频道。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannel.Leave "Leave"</td>
<td>离开频道。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannel.Dispose "Dispose"</td>
<td>释放当前 <code>RtmChannel</code> 频道实例的所有资源。</td>
</tr>
</table>


<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnJoinSuccessHandler "OnJoinSuccessHandler"</td>
<td>成功加入频道回调。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnJoinFailureHandler "OnJoinFailureHandler"</td>
<td>加入频道失败回调。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnLeaveHandler "OnLeaveHandler"</td>
<td>报告 <code>Leave</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnMemberJoinedHandler "OnMemberJoinedHandler"</td>
<td>远端用户加入频道回调。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnMemberLeftHandler "OnMemberLeftHandler"</td>
<td>远端用户离开频道回调。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnMemberCountUpdatedHandler "OnMemberCountUpdatedHandler"</td>
<td>频道成员人数更新回调。返回最新频道成员人数。</td>
</tr>
</table>


<a name="频道消息"></a>
## 频道消息

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage() "CreateMessage"</td>
<td>创建并返回一个空文本 <code>TextMessage</code> 实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage(string text) "CreateMessage"</td>
<td>创建并返回一个文本 <code>IMessage</code> 实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage(byte[] rawData) "CreateMessage"</td>
<td>创建并返回一个自定义二进制 <code>IMessage</code> 实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.CreateMessage(byte[] rawData, string description) "CreateMessage"</td>
<td>创建并返回一个包含文字描述的自定义二进制 <code>IMessage</code> 消息实例。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannel.SendMessage(const IMessage message, const SendMessageOptions options) "sendMessage"</td>
<td>供频道成员向所在频道发送频道消息。</td>
</tr>
<tr>
<td>\ref agora_rtm.IMessage.Release "Release"</td>
<td>释放当前 <code>IMessage</code> 消息实例使用的所有资源。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnSendMessageResultHandler "OnSendMessageResultHandler"</td>
<td>报告 <code>SendMessage</code> 方法的调用结果。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnMessageReceivedHandler "OnMessageReceivedHandler"</td>
<td>收到频道消息回调。</td>
</tr>
</table>


<a name="成员列表"></a>
## 获取频道成员列表

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannel.GetMembers "GetMembers"</td>
<td>获取当前频道成员列表。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmChannelEventHandler.OnGetMembersHandler "OnGetMembersHandler"</td>
<td>获取频道成员列表回调。</td>
</tr>
</table>



<a name="呼叫邀请"></a>
## 呼叫邀请管理

<table>
<tr>
<th>呼叫管理器获取与释放方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.GetRtmCallManager "GetRtmCallManager"</td>
<td>获取呼叫邀请管理器。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallManager.Dispose "Dispose"</td>
<td>释放当前 <code>RtmCallManager</code> 实例的所有资源。</td>
</tr>
</table>

<table>
<tr>
<th>供主叫调用的方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallManager.CreateLocalCallInvitation "CreateLocalCallInvitation"</td>
<td>供主叫创建、管理一个 <code>LocalInvitation</code> 呼叫邀请对象。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallManager.SendLocalInvitation "SendLocalInvitation"</td>
<td>供主叫向指定用户发送呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallManager.CancelLocalInvitation "CancelLocalInvitation"</td>
<td>供主叫取消一个呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.LocalInvitation.Dispose "Dispose"</td>
<td>释放当前 <code>LocalInvitation</code> 本地呼叫邀请实例的所有资源。</td>
</tr>
</table>

<table>
<tr>
<th>供被叫调用的方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallManager.AcceptRemoteInvitation "AcceptRemoteInvitation"</td>
<td>供被叫接受一个呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallManager.RefuseRemoteInvitation "RefuseRemoteInvitation"</td>
<td>供被叫拒绝一个呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RemoteInvitation.Dispose "Dispose"</td>
<td>释放当前 <code>RemoteInvitation</code> 呼叫邀请实例的所有资源。</td>
</tr>
</table>


<table>
<tr>
<th>Caller Event</th>
<th>Description</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnLocalInvitationReceivedByPeerHandler "OnLocalInvitationReceivedByPeerHandler"</td>
<td>报告给主叫的回调：被叫已收到呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnLocalInvitationCanceledHandler "OnLocalInvitationCanceledHandler"</td>
<td>报告给主叫的回调：呼叫邀请已被成功取消。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnLocalInvitationAcceptedHandler "OnLocalInvitationAcceptedHandler"</td>
<td>报告给主叫的回调：被叫已接受呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnLocalInvitationRefusedHandler "OnLocalInvitationRefusedHandler"</td>
<td>报告给主叫的回调：被叫已拒绝呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnLocalInvitationFailureHandler "OnLocalInvitationFailureHandler"</td>
<td>报告给主叫的回调：呼叫邀请过程失败。</td>
</tr>
</table>

<table>
<tr>
<th>被叫接收的事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnRemoteInvitationReceivedHandler "OnRemoteInvitationReceivedHandler"</td>
<td>报告给被叫的回调：收到一条呼叫邀请。SDK 会同时返回一个 <code>RemoteInvitation</code> 对象供被叫管理。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnRemoteInvitationAcceptedHandler "OnRemoteInvitationAcceptedHandler"</td>
<td>报告给被叫的回调：已接受呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnRemoteInvitationRefusedHandler "OnRemoteInvitationRefusedHandler"</td>
<td>报告给被叫的回调：已拒绝呼叫邀请。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnRemoteInvitationCanceledHandler "OnRemoteInvitationCanceledHandler"</td>
<td>报告给被叫的回调：呼叫邀请已被主叫取消。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmCallEventHandler.OnRemoteInvitationFailureHandler "OnRemoteInvitationFailureHandler"</td>
<td>报告给被叫的回调：呼叫邀请进程失败。</td>
</tr>
</table>



<a name="renewtoken"></a>
## 更新 Token


<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.RenewToken "RenewToken"</td>
<td>更新 SDK 的 RTM Token。</td>
</tr>
</table>

<table>
<tr>
<th>事件</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnTokenExpiredHandler "OnTokenExpiredHandler"</td>
<td>（SDK 断线重连时触发）当前使用的 RTM Token 已超过 24 小时的签发有效期。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClientEventHandler.OnRenewTokenResultHandler "OnRenewTokenResultHandler"</td>
<td> 报告 <code>RenewToken</code> 方法的调用结果。</td>
</tr>
</table>

<a name="logfile"></a>
## 日志设置与版本查询

- 日志相关操作在调用 \ref agora_rtm.RtmClient.RtmClient "RtmClient" 方法创建 \ref agora_rtm.RtmClient "RtmClient" 实例后即可进行，无需等到调用 \ref agora_rtm.RtmClient.Login "Login" 方法成功。
- 版本查询操作为全局方法，可在创建 \ref agora_rtm.RtmClient "RtmClient" 实例前进行。

<table>
<tr>
<th>方法</th>
<th>描述</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.SetLogFile "SetLogFile"</td>
<td>设定日志文件的默认地址。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.SetLogFilter "SetLogFilter"</td>
<td>设置日志输出等级。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.SetLogFileSize "SetLogFileSize"</td>
<td>设置 SDK 输出的单个日志文件的大小，单位为 KB。 SDK 设有 2 个大小相同的日志文件。</td>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.GetSdkVersion "GetSdkVersion"</td>
<td>获取 Agora RTM SDK 的版本信息。</td>
</tr>
</table>


<a name="customization"></a>
## 定制方法

<table>
<tr>
<th>Method</th>
<th>Description</th>
</tr>
<tr>
<td>\ref agora_rtm.RtmClient.SetParameters "SetParameters"</td>
<td>通过 JSON 配置 SDK 提供技术预览或特别定制功能。</td>
</tr>
</table>