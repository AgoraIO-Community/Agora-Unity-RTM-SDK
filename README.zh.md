# Agora Real Time Messaging SDK for Unity

一个包含SDK的示例应用程序，用于显示登录/注销，登录到/退出频道，获取频道成员计数，发送/接收频道消息，发送对等消息，查询成员和令牌认证

*Read this in other languages: [English](README.md)*

Agora Unity3D Sample App是一个开源演示，它将帮助您使用Agora RTM SDK将消息聊天直接集成到Unity3D应用程序中。

使用此示例应用程序，您可以：

- 登录到RTM服务器
- 发送点对点消息并离线接收点对点消息
- 显示点对点历史消息
- 加入频道
- 发送频道信息，接收频道信息
- 获取频道会员数
- 查询频道中的成员
- 离开频道
- 注销RTM服务器
- 令牌认证


## 开发人员环境要求
- Unity3d 2017或更高版本
- 真实设备（支持Windows，Android，iOS和Mac）
- 有些模拟器功能缺失或存在性能问题，因此使用真实设备是最佳选择

## 快速开始

本节向您展示如何准备，构建和运行示例应用程序。

### 获取应用ID

要构建和运行示例应用程序，请获取一个应用程序ID (AppID)：

1. 在 [agora.io](https://dashboard.agora.io/signin/) 上创建一个开发人员帐户。完成注册过程后，您将被重定向到仪表板。<br />
2. 在左侧的Agora控制台中导航至** Projects **> ** More **> ** Create **> ** Create New Project **。<br/>
3. 从仪表板保存** App ID **，以备后用。


### 运行应用程序
1. 首先克隆或下载此存储库 (或在这个链接直接下载资源包：bit.ly/3grvN9C)  
2. 在Unity3D编辑器中打开Assets> Scenes> MainScene.unity。
3. 接下来进入Heirarchy窗口并选择MessengerManager，在检查器中将您的App ID添加到AppID输入字段

### 在编辑器中测试
1.转到“文件”>“版本”>“平台”，然后根据要使用的设备选择Windows或Mac。
2.按下Unity Play按钮运行示例场景

#### 部署到Windows，Mac，Android
1. 只需在“文件”>“构建设置”中更改平台，然后切换到首选平台，即可部署到Mac，Android和Windows。
2. 点击构建并运行

#### 如果部署到iOS
1. 在“文件”>“构建设置”窗口的“平台”中，将“平台”更改为iOS，然后单击“构建并运行” **。
2. 在Xcode的General选项卡下的Targets> Unity-iPhone> Frameworks，Libaries和Embedded Content下，将AgoraRtmIos.framework更改为Embed＆Sign
3. 按Xcode中的“播放”按钮以构建您的设备

XCode 设定例子（Unity 2017,2018)
![rtm2018-sign](https://user-images.githubusercontent.com/1261195/90598897-c68b6180-e1a8-11ea-841c-12d06cf635d2.png)

XCode 设定例子(Unity 2019.3)
![rtm2019-sign](https://user-images.githubusercontent.com/1261195/90599047-05211c00-e1a9-11ea-9b2b-ac8114101a0d.png)



## 联系我们
- 完整的 API 文档见 [文档中心](https://docs.agora.io/cn/)
- 如果在集成中遇到问题, 你可以到 [开发者社区](https://dev.agora.io/cn/) 提问
- 如果需要售后技术支持, 你可以在 [Agora Dashboard](https://dashboard.agora.io) 提交工单
- 如果发现了示例代码的 bug, 欢迎提交 [issue](https://github.com/jakep84/Unity-RTM/issues)

## 代码许可
The MIT License (MIT).
