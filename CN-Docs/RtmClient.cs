using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System;
using AOT;

namespace agora_rtm {
    public sealed class RtmClient : IRtmApiNative, IDisposable {
        private Dictionary<string, RtmChannel> channelDic = new Dictionary<string, RtmChannel>();
        private List<RtmChannelAttribute> attributeList = new List<RtmChannelAttribute>();
        private IntPtr _rtmServicePtr = IntPtr.Zero;
        private RtmClientEventHandler _eventHandler;
        private RtmCallManager _rtmCallManager;
        private bool _disposed = false;
        /// <summary>
        /// 创建并返回一个 #RtmClient 实例。
        /// @note Agora RTM SDK 支持创建多个实例。
        /// </summary>
        /// <param name="appId">
        /// 如果你的开发包里没有 App ID，请向声网申请一个新的 App ID。
        /// </param>
        /// <param name="eventHandler">
        /// 一个 \ref agora_rtm.RtmClientEventHandler "RtmClientEventHandler" 对象。
        /// </param>
        public RtmClient(string appId, RtmClientEventHandler eventHandler) {
            if (appId == null || eventHandler == null) {
                Debug.LogError("appId or eventHandler is null");
                return;
            }
            AgoraCallbackObject.GetInstance();
            _rtmServicePtr = createRtmService_rtm();
            _eventHandler = eventHandler;
            int ret = initialize_rtm(_rtmServicePtr, appId, eventHandler.GetRtmClientEventHandlerPtr());
            if (ret != 0) {
                Debug.LogError("RtmClient create fail, error:  " + ret);
            }
        }

        ~RtmClient() {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by the RtmClient instance.
        /// Note: Do not call this method in any of your callbacks.
        /// </summary>
        /// <param name="sync">
        /// wheather release rtm client async.
        /// </param>
        private void Release(bool sync) {
            Debug.Log("RtmClient Release");
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return;
			}

            foreach(KeyValuePair<string, RtmChannel>item in channelDic) {
                RtmChannel channel = item.Value;
                if (channel != null) {
                    channel.Dispose();
                }
            }
            channelDic.Clear();
            channelDic = null;

            foreach(RtmChannelAttribute item in attributeList) {
                if (item != null) {
                    item.Dispose();
                }
            }
            attributeList.Clear();
            attributeList = null;

            if (_rtmCallManager != null) {
                _rtmCallManager.Dispose();
                _rtmCallManager = null;
            }

            release_rtm(_rtmServicePtr, sync);
            _rtmServicePtr = IntPtr.Zero;
            _eventHandler.Release();
            _eventHandler = null;
        }

        /// <summary>
        /// 获取 Agora RTM SDK 的版本信息。
        /// </summary>
        /// <returns>
        /// String 格式的 Agora RTM SDK 的版本信息。比如：1.0.0。
        /// </returns>
        public static string GetSdkVersion() {
            IntPtr sdkVersion = _getRtmSdkVersion_rtm();
            if (!ReferenceEquals(sdkVersion, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(sdkVersion);
			} else {
				return "";
			}
        }

        /// <summary>
        /// 登录 Agora RTM 系统。
        /// - 方法调用成功：本地用户收到回调 \ref agora_rtm.RtmClientEventHandler.OnLoginSuccessHandler "OnLoginSuccessHandler"。
        /// - 方法调用失败：本地用户收到回调 \ref agora_rtm.RtmClientEventHandler.OnLoginFailureHandler "OnLoginFailureHandler"。错误码详见 \ref agora_rtm.LOGIN_ERR_CODE "LOGIN_ERR_CODE"。
        /// @note
        ///   - 异地登录后之前的状态（目前主要是加入的频道）不会保留。
        ///   - 如果你在不同实例中以相同用户 ID 登录，之前的登录将会失效，你会被踢出之前加入的频道。
        ///   - 只有在调用本方法成功加入频道后（即：当收到 \ref agora_rtm.RtmClientEventHandler.OnLoginSuccessHandler "OnLoginSuccessHandler" 回调时）才可以调用 RTM 的核心业务逻辑。以下方法除外：
        ///     - #CreateChannel
        ///     - #CreateMessage
        ///     - \ref agora_rtm.IMessage.SetText "SetText"
        ///     - #GetRtmCallManager
        ///     - \ref agora_rtm.RtmCallManager.CreateLocalCallInvitation "CreateLocalCallInvitation"
        ///     - #CreateChannelAttribute
        /// </summary>
        /// <param name="token">
        /// 用于登录 Agora RTM 系统的动态密钥。开启动态鉴权后可用。集成及测试阶段请将 `token` 设置为 `null`。
        /// </param>
        /// <param name="userId">
        /// 登录 Agora RTM 系统的用户 ID。该字符串不可超过 64 字节。不可设为空、null 或 "null"。以下为支持的字符集范围:
        ///     - 26 个小写英文字母 a-z
        ///     - 26 个大写英文字母 A-Z
        ///     - 10 个数字 0-9
        ///     - 空格
        ///     - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","
        ///  A userId cannot be empty, null or "null".
        /// </param>
        /// <returns>
        ///   - 0: 方法调用成功。
        ///   - ≠0: 方法调用失败。错误码详见 \ref agora_rtm.LOGIN_ERR_CODE "LOGIN_ERR_CODE"。
        /// </returns>
        public int Login(string token, string userId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return login_rtm(_rtmServicePtr, token, userId);
        }

        /// <summary>
        /// 登出 Agora RTM 系统。
        /// 本地用户收到回调 \ref agora_rtm.RtmClientEventHandler.OnLogoutHandler "OnLogoutHandler"。状态信息详见 \ref agora_rtm.LOGIN_ERR_CODE "LOGIN_ERR_CODE"。
        /// </summary>
        /// <returns>
        ///   - 0: 方法调用成功。
        ///   - ≠0: 方法调用失败。详见 #LOGOUT_ERR_CODE 。
        /// </returns>
        public int Logout() {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return logout_rtm(_rtmServicePtr);
        }
        
        /// <summary>
        /// 更新 SDK 的 RTM Token。
        /// - 在收到 \ref agora_rtm.RtmClientEventHandler.OnTokenExpiredHandler "OnTokenExpiredHandler" 回调时你需要调用此方法更新 Token。
        /// - \ref agora_rtm.RtmClientEventHandler.OnRenewTokenResultHandler "OnRenewTokenResultHandler" 回调会返回 Token 更新的结果。
        /// - 该方法的调用频率为 2 次每秒。
        /// </summary>
        /// <param name="token">新的 RTM Token。你需要自行生成 RTM Token。参考《生成 RTM Token》。</param>
        /// <returns>
        ///   - 0: 方法调用成功。
        ///   - ≠0: 方法调用失败。详见 #RENEW_TOKEN_ERR_CODE 。
        /// </returns>
        public int RenewToken(string token) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return renewToken_rtm(_rtmServicePtr, token);
        }

        /// <summary>
        /// 向指定用户（接收者）发送点对点消息或点对点的离线消息。
        /// - \ref agora_rtm.RtmChannelEventHandler.OnSendMessageResultHandler "OnSendMessageResultHandler" 向发送者返回方法调用的结果。
        /// - 消息到达接收方后，接收者收到回调 \ref agora_rtm.RtmClientEventHandler.OnMessageReceivedFromPeerHandler "OnMessageReceivedFromPeerHandler"。
        /// - 该方法允许你向离线用户发送点对点消息。如果指定用户在你发送离线消息时不在线，消息服务器会保存该条消息。
        /// @note 
        ///   - 目前我们只为每个接收端保存 200 条离线消息最长七天。当保存的离线消息超出限制时，最老的信息将会被最新的消息替换。
        ///   - 本方法可与老信令 SDK 的 endCall 方法兼容。你只需在用本方法发送文本消息时将消息头设为 `GORA_RTM_ENDCALL_PREFIX_\<channelId\>_\<your additional information\>` 格式即可。请以 endCall 对应频道的 ID 替换 `\<channelId\>， \<your additional information\>` 为附加文本信息。附加文本信息中不可使用下划线 "_" ，附加文本信息可以设为空字符串 ""。
        ///   - 发送消息（包括点对点消息和频道消息）的调用频率上限为每 3 秒 180 次。
        /// </summary>
        /// <param name="peerId">
        ///   - 接收者的用户 ID。该字符串不可超过 64 字节。不可设为空、null 或 "null"。以下为支持的字符集范围:
        ///   - 26 个小写英文字母 a-z
        ///   - 26 个大写英文字母 A-Z
        ///   - 10 个数字 0-9
        ///   - 空格
        ///   - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","
        /// </param>
        /// <param name="message">
        /// 需要发送的消息。详见 \ref agora_rtm.IMessage "IMessage" 了解如何创建消息。
        /// </param>
        /// <param name="options">
        /// 消息发送选项。详见 \ref agora_rtm.SendMessageOptions "SendMessageOptions"。
        /// </param> 
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #PEER_MESSAGE_ERR_CODE 。
        /// </returns>
        public int SendMessageToPeer(string peerId, IMessage message, SendMessageOptions options) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return sendMessageToPeer_rtm(_rtmServicePtr, peerId, message.GetPtr(), options.enableOfflineMessaging, options.enableHistoricalMessaging);
        }

        /// <summary>
        /// @note
        /// - 我们不推荐使用该方法发送点对点消息。请改用它的重载方法 #SendMessageToPeer 发送点对点消息或点对点的离线消息。
        /// - \ref agora_rtm.RtmClientEventHandler.OnSendMessageResultHandler "OnSendMessageResultHandler" 向发送者返回方法调用的结果。
        /// - 消息到达接收方后，接收者收到回调 \ref agora_rtm.RtmClientEventHandler.OnMessageReceivedFromPeerHandler "OnMessageReceivedFromPeerHandler"。
        /// @note 发送消息（包括点对点消息和频道消息）的调用频率上限为每 3 秒 180 次。
        /// </summary>
        /// <param name="peerId">
        /// 接收者的用户 ID。
        /// </param>
        /// <param name="message">
        /// 需要发送的消息。详见 \ref agora_rtm.IMessage "IMessage"了解如何创建消息。
        /// </param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #PEER_MESSAGE_ERR_CODE 。
        /// </returns>
        public int SendMessageToPeer(string peerId, IMessage message) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return sendMessageToPeer2_rtm(_rtmServicePtr, peerId, message.GetPtr());
        }

        /// <summary>
        /// 通过 media ID 从 Agora 服务器下载文件或图片至本地内存。
        /// 方法调用结果由 SDK 通过 \ref agora_rtm.RtmClientEventHandler.OnMediaDownloadToMemoryResultHandler "OnMediaDownloadToMemoryResultHandler" 回调返回。
        /// @note
        /// - 该方法适用于需要快速读取下载文件或图片的场景。
        /// - SDK 会在 \ref agora_rtm.RtmClientEventHandler.OnMediaDownloadToMemoryResult "OnMediaDownloadToMemoryResult" 回调结束后立即释放下载的文件或图片。
        /// </summary>
        /// <param name="mediaId">
        /// 服务器上待下载的文件或图片对应的 media ID。
        /// </param>
        /// <param name="requestId">
        /// 标识本次下载请求的唯一 ID。
        /// </param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #DOWNLOAD_MEDIA_ERR_CODE.
        /// </returns>
        public int DownloadMediaToMemory(string mediaId, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return downloadMediaToMemory_rtm(_rtmServicePtr, mediaId, requestId);
        }

        /// <summary>
        /// 通过 media ID 从 Agora 服务器下载文件或图片至本地指定地址。
        /// 方法调用结果由 SDK 通过 \ref agora_rtm.RtmClientEventHandler.OnMediaDownloadToFileResultHandler "OnMediaDownloadToFileResultHandler" 回调返回。
        /// </summary>
        /// <param name="mediaId">服务器上待下载的文件或图片对应的 media ID。</param>
        /// <param name="filePath">下载文件或图片在本地存储的完整路径。文件路径必须为 UTF-8 编码格式。</param>
        /// <param name="requestId">标识本次下载请求的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #DOWNLOAD_MEDIA_ERR_CODE 。
        /// </returns>
        public int DownloadMediaToFile(string mediaId, string filePath, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return downloadMediaToFile_rtm(_rtmServicePtr, mediaId, filePath, requestId);
        }

        /// <summary>
        /// 通过 request ID 取消一个正在进行中的文件或图片下载任务。
        /// 方法调用结果由 SDK 通过 \ref agora_rtm.RtmClientEventHandler.OnMediaCancelResultHandler "OnMediaCancelResultHandler" 回调返回。
        /// @note 你只能取消一个正在进行中的下载任务。下载任务完成后则无法取消下载任务，因为相应的 request ID 已不再有效。
        /// </summary>
        /// <param name="requestId">
        /// 标识本次下载请求的唯一 ID。
        /// </param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #CANCEL_MEDIA_ERR_CODE 。
        /// </returns>
        public int CancelMediaDownload(Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return cancelMediaDownload_rtm(_rtmServicePtr, requestId);
        }

        /// <summary>
        /// 通过 request ID 取消一个正在进行中的文件或图片上传任务。
        /// 方法调用结果由 SDK 通过 \ref agora_rtm.RtmClientEventHandler.OnMediaCancelResultHandler "OnMediaCancelResultHandler" 回调返回。
        /// @note 你只能取消一个正在进行中的上传任务。上传任务完成后则无法取消上传任务，因为相应的 request ID 已不再有效。
        /// </summary>
        /// <param name="requestId">
        /// 标识本次上传请求的唯一 ID。
        /// </param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #CANCEL_MEDIA_ERR_CODE 。
        /// </returns>
        public int CancelMediaUpload(Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return cancelMediaUpload_rtm(_rtmServicePtr, requestId);
        }

        /// <summary>
        /// 创建一个 Agora RTM 频道。
        /// @note 一个 #RtmClient 实例中可以创建多个频道。但是同一个用户只能同时加入最多 20 个频道。请在不使用某个频道时，调用 #Dispose 方法销毁频道实例。
        /// </summary>
        /// <param name="channelId">
        /// Agora RTM 频道名称。该字符串长度在 64 字节以内，不能设为空、null，或 "null"。以下为支持的字符集范围:
        ///  - 26 个小写英文字母 a-z
        ///  - 26 个大写英文字母 A-Z
        ///  - 10 个数字 0-9
        ///  - 空格
        ///  - "!", "#", "$", "%", "&", "(", ")", "+", "-", ":", ";", "<", "=", ".", ">", "?", "@", "[", "]", "^", "_", " {", "}", "|", "~", ","
        /// </param>
        /// <param name="rtmChannelEventHandler">See \ref agora_rtm.RtmChannelEventHandler "RtmChannelEventHandler".</param>
        /// <returns>
        ///  - 一个 \ref agora_rtm.RtmChannel "RtmChannel" 频道实例: 方法调用成功。如果具有相同 `channelId` 的频道不存在，此方法会返回已创建的频道实例。如果已经存在具有相同 `channelId` 的频道，此方法会返回已存在的频道实例。
        ///  - 方法调用失败。原因可能是 `channelId` 无效或频道数量超过限制。
        /// </returns>
        public RtmChannel CreateChannel(string channelId, RtmChannelEventHandler rtmChannelEventHandler) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}

            if (rtmChannelEventHandler == null) 
            {
                Debug.LogError("rtmChannelEventHandler is null");
            }
            
            if (channelDic.ContainsKey(channelId)) {
                if (channelDic[channelId] != null) {
                    return channelDic[channelId];
                }
            }
            
            IntPtr _rtmChannelPtr = createChannel_rtm(_rtmServicePtr, channelId, rtmChannelEventHandler.GetChannelEventHandlerPtr());
            RtmChannel channel = new RtmChannel(_rtmChannelPtr, rtmChannelEventHandler);
            channelDic.Add(channelId, channel);
            return channel;
        }

        /// <summary>
        /// 创建并返回一个空文本 \ref agora_rtm.TextMessage "TextMessage" 消息实例。
        /// @note 
        ///  - \ref agora_rtm.TextMessage "TextMessage" 实例可用于频道和点对点消息消息。
        ///  - 请在不需要 \ref agora_rtm.TextMessage "TextMessage" 时调用 \ref agora_rtm.RtmClient.Dispose "Dispose" 方法销毁其占用的资源。
        ///  - 你可以在创建文本消息实例之后调用 \ref agora_rtm.IMessage.SetText "SetText" 方法设置消息内容。不过请确保文本消息长度不超过 32 KB。
        /// </summary>
        /// <returns>
        /// 一个空文本 \ref agora_rtm.TextMessage "TextMessage" 消息实例。
        /// </returns>
        public TextMessage CreateMessage() {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage4_rtm(_rtmServicePtr);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }

        /// <summary>
        /// 创建并返回一个文本 "TextMessage" 消息实例。
        /// @note 
        ///  - \ref agora_rtm.TextMessage "TextMessage" 实例可用于频道和点对点消息消息。
        ///  - 请在不需要 \ref agora_rtm.TextMessage "TextMessage" 时调用 \ref agora_rtm.RtmClient.Dispose "Dispose" 方法销毁其占用的资源。
        /// </summary>
        /// <param name="text">文本消息内容。长度不得超过 32 KB。</param>
        /// <returns>
        /// 一个文本 \ref agora_rtm.TextMessage "TextMessage" 消息实例。
        /// </returns>
        public TextMessage CreateMessage(string text) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage3_rtm(_rtmServicePtr, text);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }

        /// <summary>
        /// 创建并返回一个自定义二进制 \ref agora_rtm.TextMessage "TextMessage" 消息实例。
        /// @note 
        ///  - \ref agora_rtm.TextMessage "TextMessage" 实例可用于频道和点对点消息消息。
        ///  - 请在不需要 \ref agora_rtm.TextMessage "TextMessage" 时调用 \ref agora_rtm.RtmClient.Dispose "Dispose" 方法销毁其占用的资源。
        ///  - 你可以在调用本方法后通过 \ref agora_rtm.IMessage.SetText "SetText" 方法设置自定义二进制消息的文字描述。但是请确保二进制消息和文字描述加起来的大小不超过 32 KB。
        /// </summary>
        /// <param name="rawData">二进制消息在内存中的首地址。</param>
        /// <returns>
        /// 一个自定义二进制 \ref agora_rtm.TextMessage "TextMessage" 消息实例。
        /// </returns>
        public TextMessage CreateMessage(byte[] rawData) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage2_rtm(_rtmServicePtr, rawData, rawData.Length);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }
        
        /// <summary>
        /// 创建并返回一个包含文字描述的自定义二进制 \ref agora_rtm.TextMessage "TextMessage" 消息实例。
        ///  - \ref agora_rtm.TextMessage "TextMessage" 实例可用于频道和点对点消息消息。
        ///  - 请在不需要 \ref agora_rtm.TextMessage "TextMessage" 时调用 \ref agora_rtm.RtmClient.Dispose "Dispose" 方法销毁其占用的资源。
        ///  - 你也可以先将 `description` 设为 "" ，消息创建成功后可以通过调用 \ref agora_rtm.IMessage.SetText "SetText" 方法设置自定义二进制消息的文字描述。但是请确保自定义二进制消息内容和文字描述加起来的大小不超过 32 KB。
        /// </summary>
        /// <param name="rawData">自定义二进制消息在内存中的首地址。</param>
        /// <param name="description">自定义二进制消息的简短文字描述。设置文字描述时，请确保自定义二进制消息内容和文字描述加起来的大小不超过 32 KB。</param>
        /// <returns>
        /// 一个二进制 \ref agora_rtm.TextMessage "TextMessage" 消息实例。
        /// </returns>
        public TextMessage CreateMessage(byte[] rawData, string description) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createMessage_rtm(_rtmServicePtr, rawData, rawData.Length, description);
            return new TextMessage(_MessagePtr, TextMessage.MESSAGE_FLAG.SEND);
        }

        /// <summary>
        /// 通过 media ID 创建一个 \ref agora_rtm.ImageMessage "ImageMessage" 实例。
        /// - 如果你已经有了一个保存在 Agora 服务器上的图片对应的 media ID，你可以调用本方法创建一个 \ref agora_rtm.ImageMessage "ImageMessage" 实例。
        /// - 如果你没有相应的 media ID，那么你必须通过调用 #CreateImageMessageByUploading 方法上传相应的文件到 Agora 服务器来获得一个对应的 \ref agora_rtm.ImageMessage "ImageMessage" 实例。
        /// </summary>
        /// <param name="mediaId">已上传到 Agora 服务器的图片的 media ID。</param>
        /// <returns>
        /// 一个 \ref agora_rtm.ImageMessage "ImageMessage" 实例。
        /// </returns>
        public ImageMessage CreateImageMessageByMediaId(string mediaId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createImageMessageByMediaId_rtm(_rtmServicePtr, mediaId);
            return new ImageMessage(_MessagePtr, ImageMessage.MESSAGE_FLAG.SEND);
        }

        /// <summary>
        /// 上传一个图片到 Agora 服务器以获取一个相应的 \ref agora_rtm.ImageMessage "ImageMessage" 图片消息实例。
        /// SDK 会通过 \ref agora_rtm.RtmClientEventHandler.OnImageMediaUploadResultHandler "OnImageMediaUploadResultHandler" 回调返回方法的调用结果。如果方法调用成功，该回调返回一个对应的 \ref agora_rtm.ImageMessage "ImageMessage" 实例。
        /// - 如果上传图片为 JPEG、JPG、BMP，或 PNG 格式，SDK 会自动计算上传图片的宽和高。你可以通过调用 \ref agora_rtm.ImageMessage.GetWidth "GetWidth" 方法获取计算出的宽。
        /// - 如果上传图片为其他格式，你需要调用 \ref agora_rtm.ImageMessage.SetWidth "SetWidth" 和 \ref agora_rtm.ImageMessage.SetHeight "SetHeight" 方法自行设置上传图片的宽和高。
        /// @note
        ///  - 如果你已经有了一个保存在 Agora 服务器上的对应的 media ID，你可以调用 \ref agora_rtm.RtmClient.CreateImageMessageByMediaId "CreateImageMessageByMediaId" 创建一个 \ref agora_rtm.ImageMessage "ImageMessage" 实例。
        ///  - 如需取消一个正在进行的上传任务，请调用 \ref agora_rtm.RtmClient.CancelMediaUpload "CancelMediaUpload" 方法。
        /// </summary>
        /// <param name="filePath">待上传图片在本地的完整路径。文件路径必须为 UTF-8 编码格式。</param>
        /// <param name="requestId">标识本次上传请求的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。错误码详见 #UPLOAD_MEDIA_ERR_CODE 。
        /// </returns>
        public int CreateImageMessageByUploading(string filePath, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return createImageMessageByUploading_rtm(_rtmServicePtr, filePath, requestId);
        }

        /// <summary>
        /// 通过 media ID 创建一个 \ref agora_rtm.FileMessage "FileMessage" 实例。
        /// - 如果你已经有了一个保存在 Agora 服务器上的文件对应的 media ID，你可以调用本方法创建一个 \ref agora_rtm.FileMessage "FileMessage" 实例。
        /// - 如果你没有相应的 media ID，那么你必须通过调用 #CreateFileMessageByUploading 方法上传相应的文件到 Agora 服务器来获得一个对应的 \ref agora_rtm.FileMessage "FileMessage" 实例。
        /// </summary>
        /// <param name="mediaId">
        /// 已上传到 Agora 服务器的文件的 media ID。
        /// </param>
        /// <returns>
        /// 一个 \ref agora_rtm.FileMessage "FileMessage" 实例。
        /// </returns>
        public FileMessage CreateFileMessageByMediaId(string mediaId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr _MessagePtr = createFileMessageByMediaId_rtm(_rtmServicePtr, mediaId);
            return new FileMessage(_MessagePtr, FileMessage.MESSAGE_FLAG.SEND);
        }

        /// <summary>
        /// 上传一个文件到 Agora 服务器以获取一个相应的 \ref agora_rtm.FileMessage "FileMessage" 文件消息实例。
        /// SDK 会通过 \ref agora_rtm.RtmClientEventHandler.OnFileMediaUploadResultHandler "OnFileMediaUploadResultHandler" 回调返回方法的调用结果。如果方法调用成功，该回调返回一个对应的 \ref agora_rtm.FileMessage "FileMessage" 实例。
        /// </summary>
        /// <param name="filePath">
        /// 待上传文件在本地的完整路径。文件路径必须为 UTF-8 编码格式。
        /// </param>
        /// <param name="requestId">
        /// 标识本次上传请求的唯一 ID。
        /// </param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。错误码详见 #UPLOAD_MEDIA_ERR_CODE 。
        /// </returns>
        public int CreateFileMessageByUploading(string filePath, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return createFileMessageByUploading_rtm(_rtmServicePtr, filePath, requestId);
        }

        /// <summary>
        /// 创建一个 \ref agora_rtm.RtmChannelAttribute "RtmChannelAttribute" 实例。
        /// </summary>
        /// <returns>
        /// 一个 \ref agora_rtm.RtmChannelAttribute "RtmChannelAttribute" 实例。
        /// </returns>
        public RtmChannelAttribute CreateChannelAttribute() {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return null;
			}
            IntPtr channelAttribute = createChannelAttribute_rtm(_rtmServicePtr);
            RtmChannelAttribute attribute = new RtmChannelAttribute(channelAttribute);
            attributeList.Add(attribute);
            return attribute;
        }
        
        /// <summary>
        /// 通过 JSON 配置 SDK 提供技术预览或特别定制功能。
        /// @note JSON 选项默认不公开。声网工程师正在努力寻求以标准化方式公开 JSON 选项。详情请联系 support@agora.io。
        /// </summary>
        /// <param name="parameters">
        /// JSON 格式的 SDK 选项。
        /// </param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败
        /// </returns>
        public int SetParameters(string parameters) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setParameters_rtm(_rtmServicePtr, parameters);
        }
        
        /// <summary>
        /// 查询指定用户的在线状态。
        /// - 在线：用户已登录到 Agora RTM 系统。
        /// - 不在线：用户已登出 Agora RTM 系统或因其他原因与 Agora RTM 系统断开连接。
        /// @note 调用频率上限为每 5 秒 10 次.
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnQueryPeersOnlineStatusResultHandler "OnQueryPeersOnlineStatusResultHandler" 回调返回方法调用结果。
        /// </summary>
        /// <param name="peerIds">用户 ID 列表。支持 ID 数量上限为 256 。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 \ref agora_rtm.QUERY_PEERS_ONLINE_STATUS_ERR "QUERY_PEERS_ONLINE_STATUS_ERR"。
        /// </returns>
        public int QueryPeersOnlineStatus(string [] peerIds, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return queryPeersOnlineStatus_rtm(_rtmServicePtr, peerIds, peerIds.Length, requestId);
        }

        /// <summary>
        /// 订阅指定单个或多个用户的在线状态。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnSubscriptionRequestResultHandler "OnSubscriptionRequestResultHandler" 回调返回方法调用结果。
        /// - 首次订阅成功后，SDK 会通过 \ref agora_rtm.RtmClientEventHandler.OnPeersOnlineStatusChangedHandler "OnPeersOnlineStatusChangedHandler" 回调返回被订阅用户在线状态。
        /// - 每当被订阅用户在线状态发生变化时，SDK 都会通过 \ref agora_rtm.RtmClientEventHandler.OnPeersOnlineStatusChangedHandler "OnPeersOnlineStatusChangedHandler" 回调通知订阅方。
        /// - 如果 SDK 在断线重连过程中有被订阅用户的在线状态发生改变，SDK 会在重连成功时通过 \ref agora_rtm.RtmClientEventHandler.OnPeersOnlineStatusChangedHandler "OnPeersOnlineStatusChangedHandler" 回调通知订阅方。
        /// @note 
        ///  - 用户登出 Agora RTM 系统后，所有之前的订阅内容都会被清空；重新登录后，如需保留之前订阅内容则需重新订阅。
        ///  - SDK 会在网络连接中断时进入断线重连状态。重连成功时 SDK 会自动重新订阅之前订阅用户，无需人为干预。
        /// </summary>
        /// <param name="peerIds">用户 ID 列表。最多不超过 512 个用户 ID。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #PEER_SUBSCRIPTION_STATUS_ERR 。
        /// </returns>
        public int SubscribePeersOnlineStatus(string [] peerIds, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return subscribePeersOnlineStatus_rtm(_rtmServicePtr, peerIds, peerIds.Length, requestId);
        }

        /// <summary>
        /// 退订指定单个或多个用户的在线状态。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnSubscriptionRequestResultHandler "OnSubscriptionRequestResultHandler" 回调返回方法调用结果。
        /// </summary>
        /// <param name="peerIds">用户 ID 列表。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns> 
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #PEER_SUBSCRIPTION_STATUS_ERR 。
        /// </returns>
        public int UnsubscribePeersOnlineStatus(string [] peerIds, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return unsubscribePeersOnlineStatus_rtm(_rtmServicePtr, peerIds, peerIds.Length, requestId);
        }

        /// <summary>
        /// 获取某特定内容被订阅的用户列表。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnQueryPeersBySubscriptionOptionResultHandler "OnQueryPeersBySubscriptionOptionResultHandler" 回调返回方法调用结果。
        /// </summary>
        /// <param name="option">被订阅的类型。详见 \ref agora_rtm.PEER_SUBSCRIPTION_OPTION "PEER_SUBSCRIPTION_OPTION"。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR 。
        /// </returns>
        public int QueryPeersBySubscriptionOption(PEER_SUBSCRIPTION_OPTION option, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return queryPeersBySubscriptionOption_rtm(_rtmServicePtr, option, requestId);
        }

        /// <summary>
        /// 设置 SDK 输出的单个日志文件的大小，单位为 KB。 SDK 设有 2 个大小相同的日志文件。
        /// </summary>
        /// <param name="fileSizeInKBytes">SDK 输出的单个日志文件的大小，单位为 KB。默认值为 10240 (KB)。取值范围为 [512 KB, 1 GB]。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。
        /// </returns>
        public int SetLogFileSize(int fileSizeInKBytes) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setLogFileSize_rtm(_rtmServicePtr, fileSizeInKBytes);
        }

        /// <summary>
        /// 设置日志输出等级。
        /// 设置 SDK 的输出日志输出等级。不同的输出等级可以单独或组合使用。日志级别顺序依次为 `OFF`、`CRITICAL`、`ERROR`、`WARNING` 和 `INFO`。选择一个级别，你就可以看到在该级别之前所有级别的日志信息。例如，你选择 `WARNING` 级别，就可以看到在 `CRITICAL`、`ERROR` 和 `WARNING` 级别上的所有日志信息。
        /// </summary>
        /// <param name="fileter">日志输出等级。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。
        /// </returns>
        public int SetLogFilter(int fileter) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setLogFilter_rtm(_rtmServicePtr, fileter);
        }

        /// <summary>
        /// 设定日志文件的默认地址。
        /// @note 
        ///  - 请确保指定的路径可写。
        ///  - 如需调用本方法，请在调用 \ref agora_rtm.RtmClient.RtmClient "RtmClient" 方法创建 \ref agora_rtm.RtmClient "RtmClient" 实例后立即调用，否则会造成输出日志不完整。
        /// 
        /// </summary>
        /// <param name="logFilePath">
        /// 日志文件的绝对路径。编码格式为 UTF-8。
        /// </param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。
        /// </returns>
        public int SetLogFile(string logFilePath) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return setLogFile_rtm(_rtmServicePtr, logFilePath);
        }

        /// <summary>
        /// 查询单个或多个频道的成员人数。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnGetChannelMemberCountResultHandler "OnGetChannelMemberCountResultHandler" 回调返回方法调用结果。
        /// @note 
        ///  - 该方法的调用频率上限为每秒 1 次。
        ///  - 不支持一次查询超过 32 个频道的成员人数。
        /// </summary>
        /// <param name="channelIds">指定频道名数组。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>        
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #GET_CHANNEL_MEMBER_COUNT_ERR_CODE 。
        /// </returns>
        public int GetChannelMemberCount(string[] channelIds, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getChannelMemberCount_rtm(_rtmServicePtr, channelIds, channelIds.Length, requestId);
        }

        /// <summary>
        /// 查询某指定频道指定属性名的属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnGetChannelAttributesResultHandler "OnGetChannelAttributesResultHandler" 回调返回方法调用结果。
        /// @note 
        ///  - 你无需加入指定频道即可查询该频道的频道属性。
        ///  - #GetChannelAttributes 和 #GetChannelAttributesByKeys 一并计算在内：调用频率上限为每 5 秒 10 次。
        /// </summary>
        /// <param name="channelId">该指定频道的频道 ID。</param>
        /// <param name="attributeKeys">频道属性名数组。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int GetChannelAttributesByKeys(string channelId, string[] attributeKeys, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getChannelAttributesByKeys_rtm(_rtmServicePtr, channelId, attributeKeys, attributeKeys.Length, requestId);
        }

        /// <summary>
        /// 查询某指定频道的全部属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnGetChannelAttributesResultHandler "OnGetChannelAttributesResultHandler" 回调返回方法调用结果。
        /// </summary>
        /// <param name="channelId">该指定频道的频道 ID。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int GetChannelAttributes(string channelId, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmServicePtr is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }
            return getChannelAttributes_rtm(_rtmServicePtr, channelId, requestId);
        }

        /// <summary>
        /// 清空某指定频道的属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnClearChannelAttributesResultHandler "OnClearChannelAttributesResultHandler" 回调返回方法调用结果。
        /// @note
        ///  - 你无需加入指定频道即可删除该频道属性。
        ///  - #DeleteChannelAttributesByKeys 和 #ClearChannelAttributes 一并计算在内：调用频率上限为每 5 秒 10 次。
        /// </summary>
        /// <param name="channelId">该指定频道的频道 ID。</param>
        /// <param name="enableNotificationToChannelMembers">指示是否将通道属性更改通知所有通道成员。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int ClearChannelAttributes(string channelId, bool enableNotificationToChannelMembers, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return clearChannelAttributes_rtm(_rtmServicePtr, channelId, enableNotificationToChannelMembers, requestId);
        }

        /// <summary>
        /// 删除某指定频道的指定属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnDeleteChannelAttributesResultHandler "OnDeleteChannelAttributesResultHandler" 回调返回方法调用结果。
        /// @note
        ///  - 你无需加入指定频道即可删除该频道属性。
        ///  - 当某频道处于空频道状态（无人状态）数分钟后，该频道的频道属性将被清空。
        ///  - 如果存在多个用户有权限修改频道属性，那么我们建议在修改频道属性前先通过调用 #GetChannelAttributes 方法更新本地频道属性缓存。
        ///  - #SetChannelAttributes 、 #DeleteChannelAttributesByKeys 和 #ClearChannelAttributes 一并计算在内：调用频率上限为每 5 秒 10 次。
        /// </summary>
        /// <param name="channelId">该指定频道的频道 ID。</param>
        /// <param name="attributeKeys">频道属性名数组。</param>
        /// <param name="enableNotificationToChannelMembers">指示是否将通道属性更改通知所有通道成员。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int DeleteChannelAttributesByKeys(string channelId, string [] attributeKeys, bool enableNotificationToChannelMembers, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return deleteChannelAttributesByKeys_rtm(_rtmServicePtr, channelId, attributeKeys, attributeKeys.Length, enableNotificationToChannelMembers, requestId);
        }

        /// <summary>
        /// 查询指定用户指定属性名的属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnGetUserAttributesResultHandler "OnGetUserAttributesResultHandler" 回调返回方法调用结果。
        /// @note #GetUserAttributes 和 #GetUserAttributesByKeys 一并计算在内：调用频率上限为每 5 秒 40 次。
        /// </summary>
        /// <param name="userId">指定用户的用户 ID。</param>
        /// <param name="attributeKeys">属性名数组。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int GetUserAttributesByKeys(string userId, string [] attributeKeys, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getUserAttributesByKeys_rtm(_rtmServicePtr, userId, attributeKeys, attributeKeys.Length, requestId);
        }

        /// <summary>
        /// 查询指定用户的全部属性。
        /// SDK 将通过  \ref agora_rtm.RtmClientEventHandler.OnGetUserAttributesResultHandler "OnGetUserAttributesResultHandler" 回调返回方法调用结果。
        /// @note #GetUserAttributes 和 #GetUserAttributesByKeys 一并计算在内：调用频率上限为每 5 秒 40 次。 
        /// </summary>
        /// <param name="userId">指定用户的用户 ID。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int GetUserAttributes(string userId, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return getUserAttributes_rtm(_rtmServicePtr, userId, requestId);
        }

        /// <summary>
        /// 清空本地用户的属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnClearLocalUserAttributesResultHandler "OnClearLocalUserAttributesResultHandler" 回调返回方法调用结果。
        /// @note #DeleteLocalUserAttributesByKeys 和 #ClearLocalUserAttributes 一并计算在内：调用频率上限为每 5 秒 40 次。 
        /// </summary>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int ClearLocalUserAttributes(Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return clearLocalUserAttributes_rtm(_rtmServicePtr, requestId);
        }

        /// <summary>
        /// 删除本地用户的指定属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnDeleteLocalUserAttributesResultHandler "OnDeleteLocalUserAttributesResultHandler" 回调返回方法调用结果。
        /// @note #DeleteLocalUserAttributesByKeys 和 #ClearLocalUserAttributes 一并计算在内：调用频率上限为每 5 秒 10 次。
        /// </summary>
        /// <param name="attributeKeys">属性名数组。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int DeleteLocalUserAttributesByKeys(string [] attributeKeys, Int64 requestId) {
            if (_rtmServicePtr == IntPtr.Zero)
			{
                Debug.LogError("rtmServicePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return deleteLocalUserAttributesByKeys_rtm(_rtmServicePtr, attributeKeys, attributeKeys.Length, requestId);
        }

        /// <summary>
        /// 全量设置某指定频道的属性。
        /// SDK 将通过 \ref agora_rtm.RtmClientEventHandler.OnSetChannelAttributesResultHandler "OnSetChannelAttributesResultHandler" 回调返回方法调用结果。
        /// @note
        ///  - 你无需加入指定频道即可为该频道设置频道属性。
        ///  - 当某频道处于空频道状态（无人状态）数分钟后，该频道的频道属性将被清空。
        ///  - 如果存在多个用户有权限修改频道属性，那么我们建议在修改频道属性前先通过调用 #GetChannelAttributes 方法更新本地频道属性缓存。
        ///  - #SetChannelAttributes， #DeleteLocalUserAttributesByKeys 和 #ClearLocalUserAttributes 一并计算在内：调用频率上限为每 5 秒 10 次。
        /// </summary>
        /// <param name="channelId">该指定频道的频道 ID。</param>
        /// <param name="attributes">频道属性数组。详见 \ref agora_rtm.RtmChannelAttribute "RtmChannelAttribute"。</param>
        /// <param name="options">频道属性操作选项。详见 \ref agora_rtm.ChannelAttributeOptions "ChannelAttributeOptions"。</param>
        /// <param name="requestId">标识本次请求的的唯一 ID。</param>
        /// <returns>
        ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #ATTRIBUTE_OPERATION_ERR 。
        /// </returns>
        public int SetChannelAttributes(string channelId, RtmChannelAttribute [] attributes, ChannelAttributeOptions options, Int64 requestId)
        {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
            }

            if (attributes != null && attributes.Length > 0) {
                Int64 [] attributeLists = new Int64[attributes.Length];
                for (int i = 0; i < attributes.Length; i++) {
                    attributeLists[i] = attributes[i].GetPtr().ToInt64();
                }
                return setChannelAttributes_rtm(_rtmServicePtr, channelId, attributeLists, attributes.Length, options.enableNotificationToChannelMembers, requestId);
            }
            return -7;
        } 

        /// <summary>
        /// 获取 \ref agora_rtm.RtmCallManager "RtmCallManager" 对象。
        /// 每个 #RtmClient 实例都有各自唯一的 \ref agora_rtm.RtmCallManager "RtmCallManager" 实例。属于不同 #RtmClient 实例的 \ref agora_rtm.RtmCallManager "RtmCallManager" 实例各不相同。
        /// @note 如果不再使用 \ref agora_rtm.RtmCallManager "RtmCallManager" ，请调用 \ref agora_rtm.RtmCallManager.Dispose "Dispose" 方法释放其占用资源。
        /// </summary>
        /// <param name="eventHandler">	一个 \ref agora_rtm.RtmCallEventHandler "RtmCallEventHandler" 对象。</param>
        /// <returns>一个 \ref agora_rtm.RtmCallManager "RtmCallManager" 对象。</returns>
        public RtmCallManager GetRtmCallManager(RtmCallEventHandler eventHandler) {
            if (_rtmServicePtr == IntPtr.Zero)
            {
                Debug.LogError("rtmService is null");
                return null;
            }
            if (eventHandler == null) {
                Debug.LogError("eventHandler is null");
                return null;
            }
            IntPtr rtmCallManagerPtr = getRtmCallManager_rtm(_rtmServicePtr, eventHandler.GetPtr());
            if (_rtmCallManager == null) {
                _rtmCallManager = new RtmCallManager(rtmCallManagerPtr, eventHandler);
            }
            return _rtmCallManager;
        }

        /// <summary>
        /// 释放当前 #RtmClient 实例使用的所有资源。
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (_disposed) return;

            if (disposing) {}

            Release(true);
            _disposed = true;
        }
    }
}