using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using static RtmWrapperDll;


[StructLayout(LayoutKind.Sequential)]
public struct PeerOnlineStatus
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string peerId;
    public bool isOnline;
    public int onlineState;
}


public class RtmWrapper : MonoBehaviour
{
    private List<string> channels = new List<string>();

    public delegate void LoginSuccess();
    public event LoginSuccess OnLoginSuccess;

    public delegate void JoinSuccess();
    public event JoinSuccess OnJoinSuccess;

    public delegate void MessageReceived(string userName, string msg);
    public event MessageReceived OnMessageReceived;

    public delegate void QueryStatusReceived(long requestId, PeerOnlineStatus peersStatus, int peerCount, int errorCode);
    public event QueryStatusReceived OnQueryStatusReceived;

    public delegate void ChannelMemberCountReceived(long requestId, List<ChannelMemberCount> members);
    public event ChannelMemberCountReceived OnChannelMemberCountReceived;


    public bool LoggedIn { get; set; }
    private RtmChannel ch;

    //private string Marshal
    void Start()
    {
        print("init: " + initialize());
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        print("Release RTM Service");
        if (LoggedIn)
            release();
    }

    public void CreateRtmServiceAndLogin(string appId, string token, string username, Action onLoginSuccess, Action<string> onLoginFail)
    {
        if (LoggedIn)
            return;

        loginCallback = new LoginSuccessHandler(LoginHandler);
        peerMessageCallback = new PeerMessageReceivedHandler(PeerMessageHandler);
        queryUserStatusCallback = new QueryStatusReceivedHandler(QueryUserStatusHandler);
        memberCountCallback = new GetChannelMembersCountHandler(ChannelMemberCountHandler);
        //queryUserStatusCallback = new QueryStatusReceivedSimpHandler(QueryUserStatusSimpHandler);

        print("create rtm service: " + createRtmService(appId,
                        (In64, state) => { print("STATE: " + state); },
            peerMessageCallback,
            loginCallback,
            //() => { print("logged in"); joinChannel("testsdfq", null, null, () => { print("worked"); }, null); },
            //() => { print("logged in"); joinChannel("testsdfq", null, null, () => { print("worked"); }, null); },
            //() => { print("logged in");  },
            (errCode) => { print("Error: " + errCode); },
            queryUserStatusCallback,
            null,
            memberCountCallback
        ));

        print("login: " + login(token, username));
    }

    public void LogoutAndReleaseRtmService()
    {
        if (LoggedIn)
        {
            LoggedIn = false;
            logout();
            release();
        }
    }

    public void Join(string channel, Action onJoinSuccess, Action onJoinFailure)
    {
        channelMessageCallback = new ChannelMessageReceivedHandler(ChannelMessageHandler);
        leaveCallback = new LeaveHandler(LeaveChannelHandler);
        getMembersCallback = new GetMembersHandler(GetChannelMembersHandler);

        ch = createChannel(channel,
            (mid, status) => { print("message " + mid + " sent with status: " + status); },
            () => { UnityMainThreadDispatcher.Instance().Enqueue(() => { channels.Add(channel); print("joined: " + ch.getId());  OnJoinSuccess?.Invoke(); }); },
            (errorCode) => { print("error joining channel: " + errorCode); },
            channelMessageCallback,
            leaveCallback,
            getMembersCallback
        );
        
        //print("send msg " + sendMessageToPeer("whatever", "testing to see if it works."));
        print("join: " + ch.join());
    }

    public void Leave()
    {
        ch.leave();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SendMessageOptions
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool enableOfflineMessaging;
        [MarshalAs(UnmanagedType.I1)]
        public bool enableHistoricalMessaging;
    }

    public void sendChannelMessage(string channel, string msg)
    {
        if (!channels.Contains(channel))
        {
            Debug.LogError("You need to join the channel before you can send a message to it");
            return;
        }

        print("send msg:" + ch.sendMessage(msg));
    }

    public void sendChannelMessageWithOptions(string channel, string msg, SendMessageOptions smo)
    {
        //ch.getMembers();

        if (!channels.Contains(channel))
        {
            Debug.LogError("You need to join the channel before you can send a message to it");
            return;
        }

        print("send msg:" + ch.sendMessage(msg, smo));
    }

    public void sendPeerMessage(string peerId, string msg, int offline)
    {
        if (string.IsNullOrEmpty(peerId))
        {
            Debug.LogError("You need to enter the user ID you want to send the message to");
            return;
        }

        print("send peer msg:" + sendMessageToPeer(peerId, msg, offline));
    }

    /**
     * Test
     */
    public void QueryPeersOnlineStatus(string peerIdsUnformatted, ref long requestId)
    {
        var peerIds = peerIdsUnformatted.Split(' ');
        print("QUERY:  " + queryPeersOnlineStatus(peerIds, peerIds.Length, ref requestId)); ;
    }




    LoginSuccessHandler loginCallback;
    void LoginHandler()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            LoggedIn = true;
            OnLoginSuccess?.Invoke();
        });
    }

    PeerMessageReceivedHandler peerMessageCallback;
    void PeerMessageHandler(string uid, string msg)
    {
        print(uid + " sent the following direct message: " + msg);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            OnMessageReceived?.Invoke(uid, msg);
        });
    }

    ChannelMessageReceivedHandler channelMessageCallback;
    void ChannelMessageHandler(string uid, string channelId, string msg)
    {
        print(uid + " sent the following message: " + msg);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            OnMessageReceived?.Invoke(uid, msg);
        });
    }

    QueryStatusReceivedHandler queryUserStatusCallback;
    //QueryStatusReceivedSimpHandler queryUserStatusCallback;
    //void QueryUserStatusSimpHandler(long requestId, bool pos, int count, int errorCode)
    void QueryUserStatusHandler(long requestId, PeerOnlineStatus pos, int count, int errorCode)
    {
        if (errorCode == 0)
        {
            //print(pos[0].peerId + " is " + (pos[0].isOnline ? "connected" : "disconnected" ));
           // print(pos + " is " + (pos.isOnline? "disconnected" : "Connected" ));
            print(pos.onlineState == 0);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                //OnQueryStatusReceived?.Invoke(requestId, pos, count, errorCode);
                OnQueryStatusReceived?.Invoke(requestId, pos, count, errorCode);
            });

        }
        else
        {
            Debug.LogError("Error sending message: " + errorCode);
        }
    }

    GetChannelMembersCountHandler memberCountCallback;
    void ChannelMemberCountHandler(long requestId, IntPtr channelMember, int channelMemberCount, int errorCode)
    {
        print(channelMemberCount);
        int offset = 0;
        List<ChannelMemberCount> channelMembers = new List<ChannelMemberCount>();
        for(var i = 0; i < channelMemberCount; i++)
        {
            var ptr = new IntPtr(channelMember.ToInt64() + offset);
            var cmc = (ChannelMemberCount)Marshal.PtrToStructure(ptr, typeof(ChannelMemberCount));
            channelMembers.Add(cmc);
            //offset += cmc.channelId.Length + sizeof(int); // size of our structure is: length of the string (1 byte per character + the null terminator) + 32 bits (int size) (assuming 32 bit)
            print(cmc.channelId + ": " + cmc.count);
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            OnChannelMemberCountReceived(requestId, channelMembers);
        });
    }

    LeaveHandler leaveCallback;
    void LeaveChannelHandler(int errorCode)
    {
        if (errorCode == 0)
        {
            print("You left the channel");
        }
        else
        {
            Debug.LogError("Error leaving channel: " + errorCode);
        }
    }


    GetMembersHandler getMembersCallback;
    void GetChannelMembersHandler([Out] IntPtr members, int userCount, int errorCode)
    {
        int a = 0;
        //var allMembersList = new List<RtmChannelMember>();
        //for (int i = 0; i < userCount; i++)
        //{
        //    allMembersList.Add(new RtmChannelMember(members[i]));
        //}

        //allMembersList.ForEach(m => print(m.getUserId()));
    }

    public void GetChannelMemberCount(string[] channelIds, int channelCount, ref long reqId)
    {
        RtmWrapperDll.getChannelMemberCount(channels.ToArray(), channels.Count, ref reqId);
    }

    public class AttributesMap
    {

    }

       [DllImport(dll, CharSet = CharSet.Ansi)]
        protected static extern int setLocalUserAttributes(RtmAttribute[] attributes, int numberOfAttributes, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern int addOrUpdateLocalUserAttributes(RtmAttribute[] attributes, int numberOfAttributes, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern int deleteLocalUserAttributesByKeys(RtmAttribute[] attributes, int numberOfAttributes, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern void clearLocalUserAttributes(ref long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern void getUserAttributes(string userId, ref long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern void getUserAttributesByKeys(char userId, char[] attributeKeys, int numberOfKeys, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern int setChannelAttributes(char channelId, RtmChannelAttribute[] attributes,  int numberOfAttributes, ChannelAttributeOptions options, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern void deleteChannelAttributesByKeys(char[] attributesKeys, int numberOfKeys, ChannelAttributeOptions options, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern void clearChannelAttributes(string channelId, ChannelAttributeOptions options, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern void getChannelAttributes(string channelId, long requestId);

       [DllImport(dll, CharSet = CharSet.Ansi)]
       protected static extern void getChannelAttributesByKeys(string channelId, char[] attributesKey, int numberOfKeys, ChannelAttributeOptions options, long requestId);

}




public class RtmWrapperDll
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    //const string dll = "AgoraRTMTutorial";
    //public const string dll = "agora_rtm_sdk";
    public const string dll = "rtmBaseDll";

#elif UNITY_STANDALONE_OSX
        const string dll = "rtmBaseBUNDLE";
#elif UNITY_IOS
        const string dll = "__Internal";
#else
        const string dll = "";
#endif



    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int initialize();

    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int createRtmService(string appId, SendMessageReceivedHandler smr, PeerMessageReceivedHandler pmr, LoginSuccessHandler lsc, LoginFailureHandler lfc, QueryStatusReceivedHandler qus, SubscriptionRequestHandler src, GetChannelMembersCountHandler gcmc);

    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int login(string token, string uid);

    [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int logout();

    public static RtmChannel createChannel(string channel, SendMessageReceivedHandler smr, JoinSuccessHandler jsc, JoinFailureHandler jfc, ChannelMessageReceivedHandler cmr, LeaveHandler lcb, GetMembersHandler gmc)
    {
        return new RtmChannel (createChannelDll(channel, smr, jsc, jfc, cmr, lcb));
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
    //[field: MarshalAs(UnmanagedType.ByValArray)]
    //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
    public delegate void QueryStatusReceivedHandler(long requestId, PeerOnlineStatus peers, int peerCount, int errorCode);
    //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
    //delegate void QueryStatusReceivedSimpHandler(long requestId, bool peers, int peerCount, int errorCode);

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

    [StructLayout(LayoutKind.Sequential)]
    public struct ChannelMemberCount
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string channelId;
        [MarshalAs(UnmanagedType.U4)]
        public int count;
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetChannelMembersCountHandler(Int64 reqId, IntPtr membersCount, int count, int errorCode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetMembersHandler([Out] IntPtr members, int userCount, int errorCode);

    public class RtmChannel
    {
        private IntPtr chHandler;

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int joinChannel(IntPtr chHandler);

        [DllImport(dll, CharSet = CharSet.Ansi)]
        public static extern int leaveChannel(IntPtr chHandler);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendChannelMessage(IntPtr chHandler, string msg);
        
        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sendChannelMessageWithOptions(IntPtr chHandler, string msg, ref RtmWrapper.SendMessageOptions smo);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getChannelId(IntPtr chHandler);

        [DllImport(dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getChannelMembers(IntPtr chHandler);
        


        public RtmChannel(IntPtr chHandler)
        {
            this.chHandler = chHandler;
        }

        public int join()
        {
            return joinChannel(chHandler);
        }

        public int leave()
        {
            return leaveChannel(chHandler);
        }

        public int sendMessage(string msg)
        {
            return sendChannelMessage(chHandler, msg);
        }
        //sendMessageWChannelOptions
        public int sendMessage(string msg, RtmWrapper.SendMessageOptions smo)
        {
            return sendChannelMessageWithOptions(chHandler, msg, ref smo);
        }

        public string getId()
        {
            return Marshal.PtrToStringAnsi(getChannelId(chHandler));
        }

        public int getMembers()
        {
            return getChannelMembers(chHandler);
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