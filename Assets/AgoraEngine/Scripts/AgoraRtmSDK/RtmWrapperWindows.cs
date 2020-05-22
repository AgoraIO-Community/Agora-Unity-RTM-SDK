using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace io.agora.rtm
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

public class RtmWrapperWindows : IRtmWrapper
{

    public override void Initialize()
    {
        Debug.Log("init: " + RtmWrapperDll.initialize());
    }


    protected override void CreateRtmServiceAndLogin(string appId, string token, string username)
    {
        if (LoggedIn)
            return;

        loginCallback = new RtmWrapperDll.LoginSuccessHandler(LoginHandler);
        peerMessageCallback = new RtmWrapperDll.PeerMessageReceivedHandler(PeerMessageHandler);
        queryUserStatusCallback = new RtmWrapperDll.QueryStatusReceivedHandler(QueryUserStatusHandler);
        memberCountCallback = new RtmWrapperDll.GetChannelMembersCountHandler(ChannelMemberCountHandler);

        Debug.Log("create rtm service: " + RtmWrapperDll.createRtmService(appId,
                        (In64, state) => { Debug.Log("STATE: " + state); },
            peerMessageCallback,
            loginCallback,
            (errCode) => { Debug.Log("Error: " + errCode); },
            queryUserStatusCallback,
            null,
            memberCountCallback
        ));

        Debug.Log("login: " + RtmWrapperDll.login(token, username));
    }

    protected override void LogoutAndReleaseRtmService()
    {
        if (LoggedIn)
        {
            LoggedIn = false;
            RtmWrapperDll.logout();
            RtmWrapperDll.release();
        }
    }

    public override void Release()
    {
        RtmWrapperDll.release();
    }


    public override IRtmChannel JoinChannel(string channelName)
    {
        channelMessageCallback = new RtmWrapperDll.ChannelMessageReceivedHandler(ChannelMessageHandler);
        leaveCallback = new RtmWrapperDll.LeaveHandler(LeaveChannelHandler);
        getMembersCallback = new RtmWrapperDll.GetMembersHandler(GetChannelMembersHandler);

        var channel = RtmWrapperDll.createChannel(channelName,
            (mid, status) => { Debug.Log("message " + mid + " sent with status: " + status); },
            () => { UnityMainThreadDispatcher.Instance().Enqueue(() => { channels.Add(channelName); Debug.Log("joined: " + channelName);  OnJoinSuccessCallback(); }); },
            (errorCode) => { Debug.Log("error joining channel: " + errorCode); },
            channelMessageCallback,
            leaveCallback,
            getMembersCallback
        );

        channel.Join();

        return channel;
    }

    public override void LeaveChannel(IRtmChannel channel)
    {
        channel.Leave();
    }

    public override void SendChannelMessage(IRtmChannel channel, string channelName, string msg)
    {
        if (!channels.Contains(channelName))
        {
            Debug.LogError("You need to join the channel before you can send a message to it");
            return;
        }

        channel.SendMessage(msg);
    }

    public override void SendChannelMessageWithOptions(IRtmChannel channel, string channelName, string msg, SendMessageOptions smo)
    {
        if (!channels.Contains(channelName))
        {
            Debug.LogError("You need to join the channel before you can send a message to it");
            return;
        }

        channel.SendMessage(msg, smo);
    }

    public override void SendPeerMessage(string peerId, string msg, bool enableOffline)
    {
        if (string.IsNullOrEmpty(peerId))
        {
            Debug.LogError("You need to enter the user ID you want to send the message to");
            return;
        }

        Debug.Log("send peer msg: " + msg);
        RtmWrapperDll.sendMessageToPeer(peerId, msg, enableOffline ? 1 : 0);
    }

    public override void QueryPeersOnlineStatus(string peerIdsUnformatted, ref long requestId)
    {
        var peerIds = peerIdsUnformatted.Split(' ');
        Debug.Log("QUERY:  " + RtmWrapperDll.queryPeersOnlineStatus(peerIds, peerIds.Length, ref requestId)); ;
    }

    public int count;
    RtmWrapperDll.LoginSuccessHandler loginCallback;
    void LoginHandler()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            LoggedIn = true;
            OnLoginSuccessCallback();
        });
    }

    RtmWrapperDll.PeerMessageReceivedHandler peerMessageCallback;
    void PeerMessageHandler(string uid, string msg)
    {
        Debug.Log(uid + " sent the following direct message: " + msg);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            OnMessageReceivedCallback(uid, msg);
        });
    }

    RtmWrapperDll.ChannelMessageReceivedHandler channelMessageCallback;
    void ChannelMessageHandler(string uid, string channelId, string msg)
    {
        Debug.Log(uid + " sent the following message: " + msg);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            OnMessageReceivedCallback(uid, msg);
        });
    }

    RtmWrapperDll.QueryStatusReceivedHandler queryUserStatusCallback;

    void QueryUserStatusHandler(long requestId, PeerOnlineStatus pos, int count, int errorCode)
    {
        if (errorCode == 0)
        {

            Debug.Log(pos.onlineState == 0);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                OnQueryStatusReceivedCallback(requestId, pos, count, errorCode);
            });

        }
        else
        {
            Debug.LogError("Error sending message: " + errorCode);
        }
    }

    RtmWrapperDll.GetChannelMembersCountHandler memberCountCallback;
    void ChannelMemberCountHandler(long requestId, IntPtr channelMember, int channelMemberCount, int errorCode)
    {
        Debug.Log(channelMemberCount);
        int offset = 0;
        List<ChannelMemberCount> channelMembers = new List<ChannelMemberCount>();
        for(var i = 0; i < channelMemberCount; i++)
        {
            var ptr = new IntPtr(channelMember.ToInt64() + offset);
            var cmc = (ChannelMemberCount)Marshal.PtrToStructure(ptr, typeof(ChannelMemberCount));
            channelMembers.Add(cmc);
            Debug.Log(cmc.channelId + ": " + cmc.count);
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            OnChannelMemberCountReceivedCallback(requestId, channelMembers);
        });
    }

    RtmWrapperDll.LeaveHandler leaveCallback;
    void LeaveChannelHandler(int errorCode)
    {
        if (errorCode == 0)
        {
            Debug.Log("You left the channel");
        }
        else
        {
            Debug.LogError("Error leaving channel: " + errorCode);
        }
    }


    RtmWrapperDll.GetMembersHandler getMembersCallback;
    void GetChannelMembersHandler([Out] IntPtr members, int userCount, int errorCode)
    {
        int a = 0;
    }

    public override void GetChannelMemberCount(string[] channelIds, int channelCount, ref long reqId)
    {
        RtmWrapperDll.getChannelMemberCount(channels.ToArray(), channels.Count, ref reqId);
    }

    public class AttributesMap
    {

    }

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
        protected static extern int setLocalUserAttributes(RtmWrapperDll.RtmAttribute[] attributes, int numberOfAttributes, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern int addOrUpdateLocalUserAttributes(RtmWrapperDll.RtmAttribute[] attributes, int numberOfAttributes, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern int deleteLocalUserAttributesByKeys(RtmWrapperDll.RtmAttribute[] attributes, int numberOfAttributes, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern void clearLocalUserAttributes(ref long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern void getUserAttributes(string userId, ref long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern void getUserAttributesByKeys(char userId, char[] attributeKeys, int numberOfKeys, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern int setChannelAttributes(char channelId, RtmWrapperDll.RtmChannelAttribute[] attributes,  int numberOfAttributes, RtmWrapperDll.ChannelAttributeOptions options, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern void deleteChannelAttributesByKeys(char[] attributesKeys, int numberOfKeys, RtmWrapperDll.ChannelAttributeOptions options, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern void clearChannelAttributes(string channelId, RtmWrapperDll.ChannelAttributeOptions options, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern void getChannelAttributes(string channelId, long requestId);

       [DllImport(RtmWrapperDll.dll, CharSet = CharSet.Ansi)]
       protected static extern void getChannelAttributesByKeys(string channelId, char[] attributesKey, int numberOfKeys, RtmWrapperDll.ChannelAttributeOptions options, long requestId);

}




public class RtmWrapperDll
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    //const string dll = "AgoraRTMTutorial";
    //public const string dll = "agora_rtm_sdk";
    public const string dll = "rtmBaseDll";

#elif UNITY_STANDALONE_OSX
        public const string dll = "rtmBaseBUNDLE";
#elif UNITY_IOS
        public const string dll = "__Internal";
#elif UNITY_ANDROID
    public const string dll = "agora-rtm-sdk-jni";
#else
        const string dll = "";
#endif

    public delegate void OnCompletion(int errorCode);
    public delegate void OnQueryStatus(IntPtr peerStatus);
    public delegate void OnChannelMemberCount(ChannelMemberCount[] channelMemberCounts, int errorCode);
    public delegate void OnMemberJoined(IntPtr channel, string userId);
    public delegate void OnMemberLeft(IntPtr channel, string userId);
    public delegate void OnChannelMessageReceived(IntPtr channel, string message, string userId);
    public delegate void OnMemberCount(IntPtr channel, int count);
    public delegate void OnQueryPeersBySubscriptionOption([MarshalAs(UnmanagedType.LPStr)] string[] peerIds, int errorCode);
    public delegate void OnRenewToken(string token, int errorCode);
    public delegate void OnConnectionStateChanged(IntPtr kit, IntPtr state, IntPtr reason);
    public delegate void OnMessageReceived(IntPtr kit, string msg, string peerId);
    public delegate void OnPeersOnlineStatusChanged(IntPtr kit, int[] status);
    public delegate void OnRtmKitTokenDidExpire(IntPtr kit);


    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int initialize();

    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int createRtmService(string appId, SendMessageReceivedHandler smr, PeerMessageReceivedHandler pmr, LoginSuccessHandler lsc, LoginFailureHandler lfc, QueryStatusReceivedHandler qus, SubscriptionRequestHandler src, GetChannelMembersCountHandler gcmc);

    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int login(string token, string uid);

    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int logout();

    public static IRtmChannel createChannel(string channel, SendMessageReceivedHandler smr, JoinSuccessHandler jsc, JoinFailureHandler jfc, ChannelMessageReceivedHandler cmr, LeaveHandler lcb, GetMembersHandler gmc)
    {
        return new RtmChannelWindows (createChannelDll(channel, smr, jsc, jfc, cmr, lcb));
    }

    [DllImport(dll, EntryPoint = "createChannel")]
    private static extern IntPtr createChannelDll(string channel, SendMessageReceivedHandler smr, JoinSuccessHandler jsc, JoinFailureHandler jfc, ChannelMessageReceivedHandler cmr, LeaveHandler lcb);

    [DllImport(dll, CharSet = CharSet.Ansi)]
    public static extern int queryPeersOnlineStatus(string[] peerIds, int peerCount, ref long requestId);

    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int sendMessageToPeer(string peerId, string msg, int offline);

    [DllImport(dll, CharSet = CharSet.Ansi)]
    public static extern void release();

    [DllImport(dll, CharSet = CharSet.Ansi)]
    public static extern void subscribePeersOnlineStatus(string[] peerIds, int peerCount, ref long requestId);

    public static RtmChannelAttribute createChannelAttribute()
    {
        return new RtmChannelAttribute(createChannelAttributeDll());
    }

    [DllImport(dll, EntryPoint = "createChannelAttribute")]
    private static extern IntPtr createChannelAttributeDll();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SendMessageReceivedHandler(Int64 messageId, int state);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PeerMessageReceivedHandler(string userId, string msg);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void QueryStatusReceivedHandler(long requestId, PeerOnlineStatus peers, int peerCount, int errorCode);

    [DllImport(dll, CharSet = CharSet.Ansi)]
    public static extern void getChannelMemberCount(string[] channelId, int channelCount, ref long requestId);



    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void LoginSuccessHandler();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void LoginFailureHandler(int errorCode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void JoinSuccessHandler();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void JoinFailureHandler(int errorCode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ChannelMessageReceivedHandler(string userId, string channelId, string msg);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void LeaveHandler(int errorCode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SubscriptionRequestHandler([Out] Int64 reqId, int errorCode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetChannelMembersCountHandler(Int64 reqId, IntPtr membersCount, int count, int errorCode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetMembersHandler([Out] IntPtr members, int userCount, int errorCode);

    public class RtmChannelWindows : IRtmChannel
    {
        private IntPtr chHandler;

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int joinChannel(IntPtr chHandler);

        [DllImport(dll, CharSet = CharSet.Ansi)]
        public static extern int leaveChannel(IntPtr chHandler);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int releaseChannel(IntPtr chHandler);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendChannelMessage(IntPtr chHandler, string msg);
        
        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendChannelMessageWithOptions(IntPtr chHandler, string msg, ref IRtmWrapper.SendMessageOptions smo);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getChannelId(IntPtr chHandler);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getChannelMembers(IntPtr chHandler);





        public RtmChannelWindows(IntPtr chHandler)
        {
            this.chHandler = chHandler;
        }

        public void Join()
        {
            var result = joinChannel(chHandler);
        }

        public void Leave()
        {
            var result = leaveChannel(chHandler);
        }

        public void Release()
        {
            var result = releaseChannel(chHandler);
        }

        public void SendMessage(string msg)
        {
            var result = sendChannelMessage(chHandler, msg);
        }
        public void SendMessage(string msg, IRtmWrapper.SendMessageOptions smo)
        {
            var result = sendChannelMessageWithOptions(chHandler, msg, ref smo);
        }

        public string GetId()
        {
            return Marshal.PtrToStringAnsi(getChannelId(chHandler));
        }

        public void GetMembers()
        {
            var result = getChannelMembers(chHandler);
        }
    }

    public class RtmAttribute
    {
        private IntPtr rtmAttr;

        public RtmAttribute(IntPtr rtmAttr)
        {
            this.rtmAttr = rtmAttr;
        }
    }

    public class RtmChannelAttribute
    {
        private IntPtr chAttr;

        [DllImport(dll)]
        private static extern void setChannelAttributeKey(IntPtr rtmChannelAttribute, string key);

        [DllImport(dll)]
        private static extern IntPtr getChannelAttributeKey(IntPtr rtmChannelAttribute);

        [DllImport(dll)]
        private static extern void setChannelAttributeValue(IntPtr rtmChannelAttribute, string value);

        [DllImport(dll)]
        private static extern string getChannelAttributeValue(IntPtr rtmChannelAttribute);

        [DllImport(dll)]
        private static extern IntPtr getChannelAttribute_getLastUpdateUserId(IntPtr rtmChannelAttribute);

        [DllImport(dll)]
        private static extern long getChannelAttribute_getLastUpdateTs(IntPtr rtmChannelAttribute);


        public RtmChannelAttribute(IntPtr chAttr)
        {
            this.chAttr = chAttr;
        }


        public void setKey(string key) {
            setChannelAttributeKey(chAttr, key);
        }

        public string getKey()
        {
            var str = getChannelAttributeKey(chAttr);
            return Marshal.PtrToStringAnsi(str);
        }


        public void setValue(string key)
        {
            setChannelAttributeValue(chAttr, key);
        }

        public string getValue()
        {
            return getChannelAttributeValue(chAttr);
        }

        public string getLastUpdateUserId()
        {
            var str = getChannelAttribute_getLastUpdateUserId(chAttr);
            return Marshal.PtrToStringAnsi(str);
        }

        public long getLastUpdateTs()
        {
            return getChannelAttribute_getLastUpdateTs(chAttr);
        }

    }

    public class ChannelAttributeOptions
    {
        private IntPtr chAttr;

        [DllImport(dll)]
        private static extern bool ChannelAttrOptions_enableNotificationToChannelMembers(IntPtr rtmChannelAttribute);

        public ChannelAttributeOptions(IntPtr chAttr)
        {
            this.chAttr = chAttr;
        }

        public bool enableNotificationToChannelMembers
        {
            get
            {
                return ChannelAttrOptions_enableNotificationToChannelMembers(chAttr);
            }
        }

    }

    public class RtmChannelMember
    {
        private IntPtr chMemberHandler;

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getMemberUserId(IntPtr chMemberHandler);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getMemberChannelId(IntPtr chMemberHandler);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void releaseMember(IntPtr chMemberHandler);

        public RtmChannelMember(IntPtr handler)
        {
            chMemberHandler = handler;
        }

        public string getUserId()
        {
            return Marshal.PtrToStringAnsi(getMemberUserId(chMemberHandler));
        }

        public string getChannelId()
        {
            return Marshal.PtrToStringAnsi(getMemberChannelId(chMemberHandler));
        }

        public void release()
        {
            releaseMember(chMemberHandler);
        }
    }

    public class RtmMessage
    {
        private IntPtr messageHandler;

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 getMessageId(IntPtr message);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getMessageType")]
        public static extern int getMessageTypeDll(IntPtr message);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setMessageText(IntPtr message, string text);
        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getMessageText(IntPtr message);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getRawMessageData")]
        public static extern IntPtr getRawMessageDataDll(IntPtr message);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getRawMessageLength")]
        public static extern int getRawMessageLengthDll(IntPtr message);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint ="getServerReceivedTs")]
        public static extern Int64 getServerReceivedTsDll(IntPtr message);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "isOfflineMessage")]
        public static extern bool isOfflineMessageDll(IntPtr message);


        public RtmMessage(IntPtr message)
        {
            this.messageHandler = message;
        }

        public Int64 getMessageID()
        {
            return getMessageId(messageHandler);
        }

        public int getMessageType()
        {
            return getMessageTypeDll(messageHandler);
        }

        public void setText(IntPtr message, string text)
        {
            setMessageText(message, text);
        }

        public string getText()
        {
            return Marshal.PtrToStringAnsi(getMessageText(messageHandler));
        }

        public string getRawMessageData()
        {
            return Marshal.PtrToStringAnsi(getRawMessageDataDll(messageHandler));
        }

        public int getRawMessageLength()
        {
            return getRawMessageLengthDll(messageHandler);
        }


        public Int64 getServerReceivedTs()
        {
            return getServerReceivedTsDll(messageHandler);
        }

        public bool isOfflineMessage()
        {
            return isOfflineMessageDll(messageHandler);
        }
    }

}
#endif
}