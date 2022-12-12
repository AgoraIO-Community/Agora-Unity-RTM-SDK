using System;

namespace agora_rtm {

    public enum COMMON_ERR_CODE {
          ERROR_NULL_PTR = -7
    }
    /// @cond not-for-doc
    public enum INIT_ERR_CODE {
          
          /**
           0: Initialization succeeds.
           */
          INIT_ERR_OK = 0,
          
          /**
           1: A common failure occurs during initialization.
           */
          INIT_ERR_FAILURE = 1,
          
          /**
           2: The SDK is already initialized.
           */
          INIT_ERR_ALREADY_INITIALIZED = 2,
          
          /**
           3: The App ID is invalid.
           */
          INIT_ERR_INVALID_APP_ID = 3,
          
          /**
           4: The event handler is empty.
           */
          INIT_ERR_INVALID_ARGUMENT = 4,
    };
    /// @endcond  
      
    /**
     @brief 登录相关错误码。
     */
    public enum LOGIN_ERR_CODE {
      
      /**
       0: 方法调用成功或登录成功。
       */
      LOGIN_ERR_OK = 0,
        
      /**
       1: 登录失败。原因未知。
       */
      LOGIN_ERR_UNKNOWN = 1,
      
      /**
       2: 登录被服务器拒绝。
       */
      LOGIN_ERR_REJECTED = 2, // Occurs when not initialized or server reject
        
      /**
       3: 无效的登录参数。
       */
      LOGIN_ERR_INVALID_ARGUMENT = 3,
        
      /**
       4: 无效的 App ID。
       */
      LOGIN_ERR_INVALID_APP_ID = 4,
        
      /**
       5: 无效的 Token。
       */
      LOGIN_ERR_INVALID_TOKEN = 5,
       
      /**
       6: Token 已过期，登录被拒绝。
       */
      LOGIN_ERR_TOKEN_EXPIRED = 6,
       
      /**
       7: **预留错误码** 未经授权的登录。
       */
      LOGIN_ERR_NOT_AUTHORIZED = 7,
       
      /**
       8: 用户已登录，或正在登录 Agora RTM 系统，或未调用 \ref agora_rtm.RtmClient.Logout "Logout" 方法退出 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态。
       */
      LOGIN_ERR_ALREADY_LOGGED_IN = 8,
       
      /**
       9: 登录超时。目前的超时设置为 10 秒。你需要再次登录。
       */
      LOGIN_ERR_TIMEOUT = 9,
       
      /**
       10: 登录过于频繁。超过 2 次每秒的上限。
       */
      LOGIN_ERR_TOO_OFTEN = 10,

      /// @cond  
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
      */
      LOGIN_ERR_NOT_INITIALIZED = 101,
      /// @endcond
    };
      
    /**
     @brief 登出相关错误码。
     */
    public enum LOGOUT_ERR_CODE {
       
      /**
       0: 登出成功。没有错误。
       */
      LOGOUT_ERR_OK = 0,
      
      /**
       1: **预留错误码**
       */
      LOGOUT_ERR_REJECTED = 1,

      /// @cond 
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      LOGOUT_ERR_NOT_INITIALIZED = 101,
      /// @endcond
        
      /**
       102: 登出 Agora RTM 系统前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      LOGOUT_ERR_USER_NOT_LOGGED_IN = 102,
    };

    /**
     @brief 更新 RTM Token 相关错误码。
     */
    public enum RENEW_TOKEN_ERR_CODE {
        
      /**
       0: 方法调用成功，或更新 Token 成功。
       */
      RENEW_TOKEN_ERR_OK = 0,
        
      /**
       1: 通用错误。更新 Token 失败。
       */
      RENEW_TOKEN_ERR_FAILURE = 1,
 
      /**
       2: 无效的输入参数。
       */
      RENEW_TOKEN_ERR_INVALID_ARGUMENT = 2,
        
      /**
       3: **预留错误码**
       */
      RENEW_TOKEN_ERR_REJECTED = 3,
 
      /**
       4: 方法调用过于频繁。超过 2 次每秒的上限。
       */
      RENEW_TOKEN_ERR_TOO_OFTEN = 4,
  
      /**
       5: 输入 Token 已过期。
       */
      RENEW_TOKEN_ERR_TOKEN_EXPIRED = 5,
  
      /**
       6: 输入 Token 无效。
       */
      RENEW_TOKEN_ERR_INVALID_TOKEN = 6,
      
      /// @cond
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      RENEW_TOKEN_ERR_NOT_INITIALIZED = 101,
      /// @endcond
        
      /**
       102: 更新 Token 前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      RENEW_TOKEN_ERR_USER_NOT_LOGGED_IN = 102,
    };
 
    /**
     @brief SDK 与 Agora RTM 系统的连接状态。
     */
    public enum CONNECTION_STATE {
        
      /**
       1: 初始状态。SDK 未连接到 Agora RTM 系统。
       
       App 调用方法 \ref agora_rtm.RtmClient.Login "Login" 时, SDK 开始登录 Agora RTM 系统，触发回调 \ref agora_rtm.RtmClientEventHandler.OnConnectionStateChangedHandler "OnConnectionStateChangedHandler" 并切换到 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_CONNECTING "CONNECTION_STATE_CONNECTING" 状态。
       */
      CONNECTION_STATE_DISCONNECTED = 1,
        
      /**
       2: SDK 正在登录 Agora RTM 系统。

       - 方法调用成功时，SDK 会触发回调 \ref agora_rtm.RtmClientEventHandler.OnLoginFailureHandler "OnLoginFailureHandler" ，并切换到 \ref agora_rtm.RtmClientEventHandler.OnConnectionStateChangedHandler "OnConnectionStateChangedHandler" callback and switches to the \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_DISCONNECTED "CONNECTION_STATE_DISCONNECTED" 状态。
       */
      CONNECTION_STATE_CONNECTING = 2,
        
      /**
       3: SDK 已登录 Agora RTM 系统。

       - 如果 SDK 与 Agora RTM 系统的连接由于网络问题中断，SDK 会触发回调 \ref agora_rtm.RtmClientEventHandler.OnConnectionStateChangedHandler "OnConnectionStateChangedHandler" ，并切换到 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" 状态。
       - 如果 SDK 因为相同 ID 已在其他实例或设备中登录等原因被服务器禁止登录，会触发回调 \ref agora_rtm.RtmClientEventHandler.OnConnectionStateChangedHandler "OnConnectionStateChangedHandler" 并切换到 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态。
       - 如果 App 调用方法 \ref agora_rtm.RtmClient.Logout "Logout" ，SDK 登出 Agora RTM 系统成功，会触发回调 \ref agora_rtm.RtmClientEventHandler.OnLogoutHandler "OnLogoutHandler" 并切换到 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_DISCONNECTED "CONNECTION_STATE_DISCONNECTED" 状态。
       */
      CONNECTION_STATE_CONNECTED = 3,
        
      /**
       4: SDK 与 Agora RTM 系统连接由于网络原因出现中断，SDK 正在尝试自动重连 Agora RTM 系统。

       - 如果 SDK 重新登录 Agora RTM 系统成功，会触发回调 \ref agora_rtm.RtmClientEventHandler.OnConnectionStateChangedHandler "OnConnectionStateChangedHandler"，并切换到 \ref agora_rtm.RtmClientEventHandler.CONNECTION_STATE_CONNECTED "CONNECTION_STATE_CONNECTED" 状态。SDK 会自动加入中断时用户所在频道，并自动将本地用户属性同步到服务端。
       - 如果 SDK 重新登录 Agora RTM 系统失败，会保持 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态并自动重连 Agora RTM 系统。
       */
      CONNECTION_STATE_RECONNECTING = 4,
        
      /**
       5: SDK 停止登录 Agora RTM 系统。
       可能原因：另一实例已经以同一用户 ID 登录 Agora RTM 系统。
       请先调用 \ref agora_rtm.RtmClient.Logout "Logout" ，再视情况调用 \ref agora_rtm.RtmClient.Login "Login" 方法重新登录 Agora RTM 系统。
       */
      CONNECTION_STATE_ABORTED = 5,
    };

    /**
     @brief 连接状态改变原因。
     */
    public enum CONNECTION_CHANGE_REASON {
        
      /**
       1: SDK 正在登录 Agora RTM 系统。
       */
      CONNECTION_CHANGE_REASON_LOGIN = 1,
        
      /**
       2: SDK 登录 Agora RTM 系统成功。
       */
      CONNECTION_CHANGE_REASON_LOGIN_SUCCESS = 2,
        
      /**
       3: SDK 登录 Agora RTM 系统失败。
       */
      CONNECTION_CHANGE_REASON_LOGIN_FAILURE = 3,
        
      /**
       4: SDK 无法登录 Agora RTM 系统超过 6 秒，停止登录。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" 状态。
       */
      CONNECTION_CHANGE_REASON_LOGIN_TIMEOUT = 4,
        
      /**
       5: SDK 与 Agora RTM 系统的连接被中断。
       */
      CONNECTION_CHANGE_REASON_INTERRUPTED = 5,
        
      /**
       6: 用户调用了 \ref agora_rtm.RtmClient.Logout "Logout" 方法登出 Agora RTM 系统。
       */
      CONNECTION_CHANGE_REASON_LOGOUT = 6,
        
      /**
       7: SDK 被服务器禁止登录 Agora RTM 系统。
       */
      CONNECTION_CHANGE_REASON_BANNED_BY_SERVER = 7,
        
      /**
       8: 另一个用户正以相同的 User ID 登陆 Agora RTM 系统。
       */
      CONNECTION_CHANGE_REASON_REMOTE_LOGIN = 8,
      
      /**
      9: Token 过期失效。
      */
      CONNECTION_CHANGE_REASON_TOKEN_EXPIRED = 9,
    };

    /**
     @brief 点对点消息发送相关错误码。
     */
    public enum PEER_MESSAGE_ERR_CODE {
        
      /**
       0: 方法调用成功，或对端已成功收到点对点消息。
       */
      PEER_MESSAGE_ERR_OK = 0,

      /**
       1: 发送点对点消息失败。
       */
      PEER_MESSAGE_ERR_FAILURE = 1,
        
      /**
       2: 发送点对点消息超时。当前的超时时间设置为 10 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING" state。
       */
      PEER_MESSAGE_ERR_SENT_TIMEOUT = 2,
        
      /**
       3: 对方不在线，发出的点对点消息未被收到。
       */
      PEER_MESSAGE_ERR_PEER_UNREACHABLE = 3,
        
     /**
      4: 对方不在线，发出的离线点对点消息未被收到。但是服务器已经保存这条消息并将在用户上线后重新发送。
      */
      PEER_MESSAGE_ERR_CACHED_BY_SERVER = 4,
        
     /**
      5: 发送消息（点对点消息和频道消息一并计算在内）超过每 3 秒 180 次的上限。
      */
      PEER_MESSAGE_ERR_TOO_OFTEN = 5,
        
     /**
      6: 用户 ID 无效。
      */
      PEER_MESSAGE_ERR_INVALID_USERID = 6,
        
     /**
      7: 消息为 null 或超出 32 KB 的长度限制。
	  */
      PEER_MESSAGE_ERR_INVALID_MESSAGE = 7,
      
      /**
       8: 消息接收方的 SDK 版本较老，无法识别本消息。
       */
      PEER_MESSAGE_ERR_IMCOMPATIBLE_MESSAGE = 8,
     
     /// @cond   
     /**
      101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
      */
      PEER_MESSAGE_ERR_NOT_INITIALIZED = 101,
      /// @endcond
    
     /**
      102: 发送点对点消息前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
      */
      PEER_MESSAGE_ERR_USER_NOT_LOGGED_IN = 102,
    };

    /**
     @brief 加入频道相关错误码。
     */
    public enum JOIN_CHANNEL_ERR {
        
      /**
       0: 方法调用成功，或用户加入频道成功。
       */
      JOIN_CHANNEL_ERR_OK = 0,

      /**
       1: 通用错误。用户加入频道失败。
       */
      JOIN_CHANNEL_ERR_FAILURE = 1,
        
      /**
       2: 不会返回此错误码。
       */
      JOIN_CHANNEL_ERR_REJECTED = 2, // Usually occurs when the user is already in the channel
        
      /**
       3: 用户加入频道失败。输入参数无效。
       */
      JOIN_CHANNEL_ERR_INVALID_ARGUMENT = 3,
        
      /**
       4: 用户加入频道超时。目前的超时设置为 5 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态。
       */
      JOIN_CHANNEL_TIMEOUT = 4,
        
     /**
      5: 同时加入的频道数超过 20 上限。
      */
      JOIN_CHANNEL_ERR_EXCEED_LIMIT = 5,
        
     /**
      6: 用户正在加入频道或已成功加入频道。
      */
      JOIN_CHANNEL_ERR_ALREADY_JOINED = 6,
        
      /**
      7: 方法调用超过每 3 秒 50 次的上限。
      */
      JOIN_CHANNEL_ERR_TOO_OFTEN = 7,

      /**
       8: 加入相同频道的频率超过每 5 秒 2 次的上限。
       */
      JOIN_CHANNEL_ERR_JOIN_SAME_CHANNEL_TOO_OFTEN = 8,

     /// @cond   
     /**
      101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
      */
      JOIN_CHANNEL_ERR_NOT_INITIALIZED = 101,
      /// @endcond
        
     /**
      102: 用户加入频道前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
      */
      JOIN_CHANNEL_ERR_USER_NOT_LOGGED_IN = 102,
    };

    /**
     @brief 离开频道相关错误码。
     */
    public enum LEAVE_CHANNEL_ERR {
      
      /**
       0: 方法调用成功，或用户离开频道成功。
       */
      LEAVE_CHANNEL_ERR_OK = 0,
        
      /**
       1: 通用错误。用户离开频道失败。
       */
      LEAVE_CHANNEL_ERR_FAILURE = 1,
        
      /**
       2: **预留错误码**
       */
      LEAVE_CHANNEL_ERR_REJECTED = 2,
        
      /**
       3: 用户已不在频道内。
       */
      LEAVE_CHANNEL_ERR_NOT_IN_CHANNEL = 3,

      /**
        4: 用户被禁止加入频道。
        */
      LEAVE_CHANNEL_ERR_KICKED = 4,

      /// @cond  
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      LEAVE_CHANNEL_ERR_NOT_INITIALIZED = 101,
      /// @endcond
    
      /**
       102: 用户在调用 \ref agora_rtm.RtmChannel.Leave "Leave" 方法前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      LEAVE_CHANNEL_ERR_USER_NOT_LOGGED_IN = 102,
    };
      
    /**
     @brief 离开频道原因。
     */
    public enum LEAVE_CHANNEL_REASON {
        
      /**
       1: 用户已主动调用 \ref agora_rtm.RtmChannel.Leave "Leave" 方法离开频道。
       */
      LEAVE_CHANNEL_REASON_QUIT = 1,
        
      /**
       2: 用户被服务器踢出。可能因为另一个实例用了相同的 uid 登录 Agora RTM 系统。
       */
      LEAVE_CHANNEL_REASON_KICKED = 2,
    };

    /**
     @brief 频道消息发送相关错误码。
     */
    public enum CHANNEL_MESSAGE_ERR_CODE {
        
      /**
       0: 方法调用成功，或服务端已收到频道消息。
       */
      CHANNEL_MESSAGE_ERR_OK = 0,

      /**
       1: 通用错误。发送频道消息失败。
       */
      CHANNEL_MESSAGE_ERR_FAILURE = 1,
             
      /**
       2: 服务器未收到频道消息或者 SDK 未在 10 秒内收到服务器响应。当前的超时设置为 10 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态。
       */
      CHANNEL_MESSAGE_ERR_SENT_TIMEOUT = 2,
        
      /**
       3: 发送消息（点对点消息和频道消息一并计算在内）超过每 3 秒 180 次的上限。
      CHANNEL_MESSAGE_ERR_TOO_OFTEN = 3,
       */
        
      /**
       4: 消息为 null 或超出 32 KB 的长度限制。
       */
      CHANNEL_MESSAGE_ERR_INVALID_MESSAGE = 4,

      /// @cond  
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      CHANNEL_MESSAGE_ERR_NOT_INITIALIZED = 101,
      /// @endcond
    
      /**
       102: 发送频道消息前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      CHANNEL_MESSAGE_ERR_USER_NOT_LOGGED_IN = 102,
    };

    /**
     @brief 获取所在频道成员列表相关错误码。
     */
    public enum GET_MEMBERS_ERR {
  
      /**
       0: 方法调用成功，或获取所在频道成员列表成功。
       */
      GET_MEMBERS_ERR_OK = 0,
    
      /**
       1: 通用错误。获取所在频道成员列表失败。
       */
      GET_MEMBERS_ERR_FAILURE = 1,
        
      /**
       2: **预留错误码**
       */
      GET_MEMBERS_ERR_REJECTED = 2,
        
      /**
       3: 获取所在频道成员列表超时。目前的超时设置为 5 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态。
       */
      GET_MEMBERS_ERR_TIMEOUT = 3,
        
      /**
       4: 方法调用频率超过 5 次每 2 秒的上限。
       */
      GET_MEMBERS_ERR_TOO_OFTEN = 4,
    
      /**
       5: 用户不在频道内。
       */
      GET_MEMBERS_ERR_NOT_IN_CHANNEL = 5,

      /// @cond  
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      GET_MEMBERS_ERR_NOT_INITIALIZED = 101,
      /// @endcond
        
      /**
       102: 获取所在频道成员列表前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      GET_MEMBERS_ERR_USER_NOT_LOGGED_IN = 102,
    };

    /**
     @brief 查询用户在线状态相关的错误码。
     */
    public enum QUERY_PEERS_ONLINE_STATUS_ERR {
      
      /**
       0: 方法调用成功，或查询用户在线状态成功。
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_OK = 0,

      /**
       1: 通用错误。查询用户在线状态失败。
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_FAILURE = 1,
        
      /**
       2: The method call fails. The argument is invalid. 
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_INVALID_ARGUMENT = 2,
        
      /**
       3: **无效的输入参数。**
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_REJECTED = 3,
        
      /**
       4: 服务器响应超时。当前的超时设置为 10 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态。
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_TIMEOUT = 4,
        
      /**
       5: 方法调用过于频繁。超过每 5 秒 10 次的限制。
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_TOO_OFTEN = 5,

      /// @cond  
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_NOT_INITIALIZED = 101,
      /// @endcond
    
      /**
       102: 查询指定用户在线状态前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      QUERY_PEERS_ONLINE_STATUS_ERR_USER_NOT_LOGGED_IN = 102,
    };

    /**
     @brief 用户在线状态类型。
     */
    public enum PEER_ONLINE_STATE {

      /**
       0: 用户在线。
       */
      PEER_ONLINE_STATE_ONLINE = 0,

      /**
       1: 连接状态不稳定（服务器连续 6 秒未收到来自 SDK 的数据包）。
       */
      PEER_ONLINE_STATE_UNREACHABLE = 1,

      /**
       2: 用户不在线（用户未登录或已登出 Agora RTM 系统，或服务器连续 30 秒未收到来自 SDK 的数据包）。
       */
      PEER_ONLINE_STATE_OFFLINE = 2,
    };
      
    /**
     @brief 订阅类型。
     */
    public enum PEER_SUBSCRIPTION_OPTION {
      /**
       0: 订阅指定用户的在线状态。
       */
      PEER_SUBSCRIPTION_OPTION_ONLINE_STATUS = 0,
    };

    /**
     @brief 订阅或退订指定用户状态相关错误码。
     */
    public enum PEER_SUBSCRIPTION_STATUS_ERR {

      /**
       0: 方法调用成功，或订阅退订操作成功。
       */
      PEER_SUBSCRIPTION_STATUS_ERR_OK = 0,

      /**
       1: 通用错误。订阅或退订操作失败。
       */
      PEER_SUBSCRIPTION_STATUS_ERR_FAILURE = 1,

      /**
       2: 无效的输入参数。
       */
      PEER_SUBSCRIPTION_STATUS_ERR_INVALID_ARGUMENT = 2,

      /**
       3: **预留错误码**
       */
      PEER_SUBSCRIPTION_STATUS_ERR_REJECTED = 3,

      /**
       4: 服务器响应超时。当前的超时设置为 10 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态。
       */
      PEER_SUBSCRIPTION_STATUS_ERR_TIMEOUT = 4,

      /**
       5: 方法调用过于频繁。超过 10 次每 5 秒的限制。
       */
      PEER_SUBSCRIPTION_STATUS_ERR_TOO_OFTEN = 5,

      /**
       6: 订阅人数超过 512 人的上限。
       */
      PEER_SUBSCRIPTION_STATUS_ERR_OVERFLOW = 6,
      
      /// @cond
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      PEER_SUBSCRIPTION_STATUS_ERR_NOT_INITIALIZED = 101,
      /// @endcond

      /**
       102: 订阅或退订操作前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      PEER_SUBSCRIPTION_STATUS_ERR_USER_NOT_LOGGED_IN = 102,
    };

    /**
     @brief 根据订阅类型获取被订阅用户列表相关的错误码。
     */
    public enum QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR {

      /**
       0: 方法调用成功，或根据订阅类型获取被订阅用户列表成功。
       */
      QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR_OK = 0,

      /**
       1: 通用错误。根据订阅类型获取被订阅用户列表失败。
       */
      QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR_FAILURE = 1,

      /**
       2: 服务器响应超时。当前的超时设置为 5 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态。
       */
      QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR_TIMEOUT = 2,

      /**
       3: 方法调用过于频繁。超过 10 次每 5 秒的限制。
       */
      QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR_TOO_OFTEN = 3,
      
      /// @cond
      /**
       101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
       */
      QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR_NOT_INITIALIZED = 101,
      /// @endcond

      /**
       102: 根据订阅类型获取被订阅用户列表前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
       */
      QUERY_PEERS_BY_SUBSCRIPTION_OPTION_ERR_USER_NOT_LOGGED_IN = 102,
    };
      
    /**
     @brief 属性相关操作错误码。
     */
    public enum ATTRIBUTE_OPERATION_ERR {
        
        /**
         0: 方法调用成功，或属性操作成功。
         */
        ATTRIBUTE_OPERATION_ERR_OK = 0,
        
        /**
         1: @deprecated
         */
        ATTRIBUTE_OPERATION_ERR_NOT_READY = 1,
        
        /**
         2: 通用错误。属性相关操作失败。
         */
        ATTRIBUTE_OPERATION_ERR_FAILURE = 2,
        
        /**
         3: 无效的输入参数。比如，你不可以把用户属性或频道属性设为 ""。
         */
        ATTRIBUTE_OPERATION_ERR_INVALID_ARGUMENT = 3,
        
        /**
         4: 本次操作后，用户属性或频道属性超过上限。
         
         - 用户属性操作：在本次属性操作后，用户属性总大小超过 16 KB 长度限制，或单条用户属性超过 8 KB 长度限制，或用户属性个数超过 32 个的条目上限。
         - 频道属性操作：在本次属性操作后，频道属性总大小超过 32 KB 长度限制，或单条频道属性超过 8 KB 长度限制，或频道属性个数超过 32 个的条目上限。
         */
        ATTRIBUTE_OPERATION_ERR_SIZE_OVERFLOW = 4,
        
        /**
         5: 方法调用频率超过限制。
         
         - \ref agora_rtm.RtmClient.GetUserAttributes "GetUserAttributes" 、 \ref agora_rtm.RtmClient.GetUserAttributesByKeys "GetUserAttributesByKeys" 一并计算在内：调用频率上限为每 5 秒 10 次
         - \ref agora_rtm.RtmClient.SetChannelAttributes "SetChannelAttributes" 、 \ref agora_rtm.RtmClient.DeleteChannelAttributesByKeys "DeleteChannelAttributesByKeys" 和 \ref agora_rtm.RtmClient.ClearChannelAttributes "ClearChannelAttributes" 一并计算在内：调用频率上限为每 5 秒 40 次。
         - \ref agora_rtm.RtmClient.GetChannelAttributes "GetChannelAttributes" 、 \ref agora_rtm.RtmClient.GetChannelAttributesByKeys "GetChannelAttributesByKeys" 一并计算在内：调用频率上限为每 5 秒 10 次。
         */
        ATTRIBUTE_OPERATION_ERR_TOO_OFTEN = 5,
        
        /**
         6: 未找到指定用户。该用户或者处于离线状态或者并不存在。
         */
        ATTRIBUTE_OPERATION_ERR_USER_NOT_FOUND = 6,
        
        /**
         7: 属性操作超时。当前的超时设定为 5 秒。可能原因：用户正处于 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_ABORTED "CONNECTION_STATE_ABORTED" 状态或 \ref agora_rtm.CONNECTION_STATE.CONNECTION_STATE_RECONNECTING "CONNECTION_STATE_RECONNECTING " 状态。
         */
        ATTRIBUTE_OPERATION_ERR_TIMEOUT = 7,
        
        /// @cond
        /**
         101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
         */
        ATTRIBUTE_OPERATION_ERR_NOT_INITIALIZED = 101,
        /// @endcond
        
        /**
         102: 执行属性相关操作前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
         */
        ATTRIBUTE_OPERATION_ERR_USER_NOT_LOGGED_IN = 102,
    };
     
     /**
      @brief 查询单个或多个指定频道成员人数的相关错误码。
      */
    public enum GET_CHANNEL_MEMBER_COUNT_ERR_CODE {
         
        /**
         0: 方法调用成功，或获取指定频道成员人数成功。
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_OK = 0,
         
        /**
         1: 通用未知错误。
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_FAILURE = 1,
        
        /**
         2: 你的频道 ID 无效或者 `channelCount` < 0.
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_INVALID_ARGUMENT = 2,
        
        /**
         3: 方法调用过于频繁。超过 1 次每秒的限制。
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_TOO_OFTEN = 3,
         
        /**
         4: 服务器响应超时。当前的超时设定为 5 秒。
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_TIMEOUT = 4,
        
        /**
         5: `channelCount` 大于 32。
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_EXCEED_LIMIT = 5,

        /// @cond 
        /**
         101: \ref agora::rtm::IRtmService "IRtmService" is not initialized.
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_NOT_INITIALIZED = 101,
        /// @endcond
        
        /**
         102: 本次操作前未调用 \ref agora_rtm.RtmClient.Login "Login" 方法或者 \ref agora_rtm.RtmClient.Login "Login" 方法调用未成功。
         */
        GET_CHANNEL_MEMBER_COUNT_ERR_USER_NOT_LOGGED_IN = 102,
    };
        
       /**
       @brief 消息类型。
       */
      	public enum MESSAGE_TYPE {
        
        /**
        0: 消息类型未定义。
        */
        MESSAGE_TYPE_UNDEFINED = 0,

        /**
        1: 文本消息。
        */
        MESSAGE_TYPE_TEXT = 1,
        
        /**
        2: 自定义二进制消息。
        */
        MESSAGE_TYPE_RAW = 2,
        
        /**
         3: 文件消息。大小不得超过 32 KB。
         */
        MESSAGE_TYPE_FILE = 3,
        
        /**
         4: 图片消息。大小不得超过 32 KB。
         */
        MESSAGE_TYPE_IMAGE = 4,
    };

     /**
     @brief 指定用户的在线状态。
     */
    public struct PeerOnlineStatus
    {
        
      /**
       指定用户的用户 ID。
       */
      public string peerId;
        
      /**
       @deprecated 请改用变量 \ref agora_rtm.PeerOnlineStatus.onlineState "onlineState"。
       
       指定用户的在线状态。
       
       - true: 在线（用户已登录 Agora RTM 系统）。
       - false: 不在线（用户未登录或未成功登录 Agora RTM 系统，或已登出）。
       */
      public bool isOnline;

      /**
       指定用户的在线状态。详见 \ref agora_rtm.PEER_ONLINE_STATE "PEER_ONLINE_STATE"。
       
       @note
       - 如果你是查询指定用户的在线状态（ \ref agora_rtm.RtmClient.QueryPeersOnlineStatus "QueryPeersOnlineStatus" ），我们的后台服务器并不会返回 `unreachable` 状态。详见 \ref agora_rtm.RtmClientEventHandler.OnQueryPeersOnlineStatusResultHandler "OnQueryPeersOnlineStatusResultHandler" 回调。
       - 如果你是订阅指定用户的在线状态（ \ref agora_rtm.RtmClient.SubscribePeersOnlineStatus "SubscribePeersOnlineStatus" ），我们的后台服务器则可能返回 `unreachable` 状态。详见 \ref agora_rtm.RtmClientEventHandler.OnPeersOnlineStatusChangedHandler "OnPeersOnlineStatusChangedHandler" 回调。
       */
      public PEER_ONLINE_STATE onlineState;
    };

    
    public struct ChannelAttributeOptions{  
        /**
         是否通知所有频道成员本次频道属性变更。
         
         @note 该标志位仅对本次 API 调用有效。
         
         - true: 通知所有频道成员本次频道属性变更。
         - false: (默认) 不通知所有频道成员本次频道属性变更。
         */
        public bool enableNotificationToChannelMembers;
    };


    /**
     @brief 用户属性
     */
    public struct RtmAttribute
    {
    
        /**
         用户属性的属性名。必须为可见字符且长度不得超过 32 字节。
         */
        public string key;
  
        /**
         用户属性的属性值。长度不得超过 8 KB。
         */
        public string value;
    };

     /**
     @brief 通道成员计数。
     */  
    public struct ChannelMemberCount
    {
      /**
       频道 ID（频道名）。
       */
      public string channelId;
      /**
       频道最新成员人数。

       @note 如果不能找到指定的 `channelId` ，返回的 `count` 将显示为 0。
       */ 
      public int count;
    };

  /**
     @brief 返回给主叫的呼叫邀请状态码。
     */
    public enum LOCAL_INVITATION_STATE {
        
      /**
       0: 返回给主叫的呼叫邀请状态码：初始状态。
       */
      LOCAL_INVITATION_STATE_IDLE = 0,
        
      /**
       1: 返回给主叫的呼叫邀请状态码：呼叫邀请已发送给被叫。
       */
      LOCAL_INVITATION_STATE_SENT_TO_REMOTE = 1,
        
      /**
       2: 返回给主叫的呼叫邀请状态码：被叫已收到呼叫邀请。
       */
      LOCAL_INVITATION_STATE_RECEIVED_BY_REMOTE = 2,
        
      /**
       3: 返回给主叫的呼叫邀请状态码：被叫已接受呼叫邀请。
       */
      LOCAL_INVITATION_STATE_ACCEPTED_BY_REMOTE = 3,
        
      /**
       4: 返回给主叫的呼叫邀请状态码：被叫已拒绝呼叫邀请。
       */
      LOCAL_INVITATION_STATE_REFUSED_BY_REMOTE = 4,
        
      /**
       5: 返回给主叫的呼叫邀请状态码：已成功取消呼叫邀请。
       */
      LOCAL_INVITATION_STATE_CANCELED = 5,
        
      /**
       6: 返回给主叫的呼叫邀请状态码：呼叫邀请过程失败。
       */
      LOCAL_INVITATION_STATE_FAILURE = 6,
    };

    /**
     @brief 返回给被叫的呼叫邀请状态码。
     */
    public enum REMOTE_INVITATION_STATE {
        
      /**
       0: 返回给被叫的呼叫邀请状态码：被叫发出的邀请的初始状态。
       */
      REMOTE_INVITATION_STATE_IDLE = 0,
        
      /**
       1: 返回给被叫的呼叫邀请状态码：收到了来自主叫的呼叫邀请。
       */
      REMOTE_INVITATION_STATE_INVITATION_RECEIVED = 1,
        
      /**
       2: 返回给被叫的呼叫邀请状态码：接受呼叫邀请的消息已成功发回给主叫。
       */
      REMOTE_INVITATION_STATE_ACCEPT_SENT_TO_LOCAL = 2,
        
      /**
       3: 返回给被叫的呼叫邀请状态码：已拒绝来自主叫的呼叫邀请。
       */
      REMOTE_INVITATION_STATE_REFUSED = 3,
        
      /**
       4: 返回给被叫的呼叫邀请状态码：已接受来自主叫的呼叫邀请。
       */
      REMOTE_INVITATION_STATE_ACCEPTED = 4,
        
      /**
       5: 返回给被叫的呼叫邀请状态码：主叫已取消呼叫邀请。
       */
      REMOTE_INVITATION_STATE_CANCELED = 5,
        
      /**
       6: 返回给被叫的呼叫邀请状态码：呼叫邀请过程失败。
       */
      REMOTE_INVITATION_STATE_FAILURE = 6,
    };

    /**
     @brief 返回给主叫的呼叫邀请错误码。
     */
    public enum LOCAL_INVITATION_ERR_CODE {
        
      /**
       0: 返回给主叫的呼叫邀请错误码：呼叫邀请成功。
       */
      LOCAL_INVITATION_ERR_OK = 0,
        
      /**
       1: 返回给主叫的呼叫邀请错误码：被叫不在线。
        
       The SDK performs the following:
       - SDK 会在被叫不在线时不断重发呼叫邀请。
       - 若消息发送 30 秒后被叫仍未上线，SDK 会返回此错误码。
       */
      LOCAL_INVITATION_ERR_PEER_OFFLINE = 1,
        
      /**
       2: 返回给主叫的呼叫邀请错误码：被叫在呼叫邀请发出后 30 秒无 ACK 响应。
       */
      LOCAL_INVITATION_ERR_PEER_NO_RESPONSE = 2,
        
      /**
       3: 返回给主叫的呼叫邀请错误码：呼叫邀请过期。被叫 ACK 响应呼叫邀请后 60 秒呼叫邀请未被取消、接受、拒绝，则呼叫邀请过期。
       */
      LOCAL_INVITATION_ERR_INVITATION_EXPIRE = 3,
        
      /**
       4: 返回给主叫的呼叫邀请错误码：主叫未登录。
       */
      LOCAL_INVITATION_ERR_NOT_LOGGEDIN = 4,
    };
      
    /**
     @brief 返回给被叫的呼叫邀请错误码
     */
    public enum REMOTE_INVITATION_ERR_CODE {
        
      /**
       0: 返回给被叫的呼叫邀请错误码：呼叫邀请成功。
       */
      REMOTE_INVITATION_ERR_OK = 0,
        
      /**
       1: 返回给被叫的呼叫邀请错误码：被叫不在线，呼叫邀请失败。
       */
      REMOTE_INVITATION_ERR_PEER_OFFLINE = 1,
        
      /**
       2: 返回给被叫的呼叫邀请错误码：被叫接受呼叫邀请后未收到主叫的 ACK 响应导致呼叫邀请失败，一般由于网络中断造成。
       */
      REMOTE_INVITATION_ERR_ACCEPT_FAILURE = 2,
        
      /**
       3: 返回给被叫的呼叫邀请错误码：呼叫邀请过期。被叫 ACK 响应呼叫邀请后 60 秒呼叫邀请未被取消、接受、拒绝，则呼叫邀请过期。
       */
      REMOTE_INVITATION_ERR_INVITATION_EXPIRE = 3,
    };

    /**
     @brief 呼叫邀请的相关 API 调用的错误码。
     */
    public enum INVITATION_API_CALL_ERR_CODE {
        
      /**
       0: 呼叫邀请相关 API 调用成功。
       */
      INVITATION_API_CALL_ERR_OK = 0,
        
      /**
       1: 呼叫邀请相关 API 调用失败：参数无效，比如参数 `content` 的值超过最大限制长度 8K 字节。
       */
      INVITATION_API_CALL_ERR_INVALID_ARGUMENT = 1,
        
      /**
       2: 呼叫邀请相关 API 调用失败：呼叫邀请未开始。
       */
      INVITATION_API_CALL_ERR_NOT_STARTED = 2,
        
      /**
       3: 呼叫邀请相关 API 调用结果：呼叫邀请已结束。
       */
      INVITATION_API_CALL_ERR_ALREADY_END = 3, // accepted, failure, canceled, refused
        
      /**
       4: 呼叫邀请相关 API 调用结果：已接受邀请。
       */
      INVITATION_API_CALL_ERR_ALREADY_ACCEPT = 4,   // more details
        
      /**
       5: 呼叫邀请相关 API 调用结果：呼叫邀请已发送。
       */
      INVITATION_API_CALL_ERR_ALREADY_SENT = 5,
    };
    
    public struct SendMessageOptions {
      
      /// <summary>
		  /// 是否设置为离线消息。
		  /// @note 本设置仅适用于点对点消息，不适用于频道消息。
		  /// </summary>
		  /// <returns>
      ///  - true: 将该消息设为离线消息。
      ///  - false: （默认）不将该消息设为离线消息。
      /// </returns>
      public bool enableOfflineMessaging;

      /// <summary>
		  /// **PRIVATE BETA** 保存为历史消息。
		  /// @note 本设置仅适用于点对点消息，不适用于频道消息。
		  /// </summary>
		  /// <returns>
      ///  - true: 将该消息保存为历史消息。
      ///  - false:（默认）不将该消息保存为历史消息。
      /// </returns>
      public bool enableHistoricalMessaging;
    };
}