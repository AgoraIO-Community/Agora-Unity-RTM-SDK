using System;
using System.Runtime.InteropServices;

namespace io.agora.rtm
{
    public interface IRtmChannel
    {
        /**
         * Joins a channel
         */
        void Join();

        /**
         * Leaves a channel
         */
        void Leave();
        /**
         * Sends a message
         */
        void SendMessage(string msg);

        /**
         * Sends a message with options
         */
        void SendMessage(string msg, IRtmWrapper.SendMessageOptions smo);

        /**
         * Gets the ID of the channel
         */
        string GetId();

        /**
         * Retrieves a member list of channel
         */
        void GetMembers();
        /**
         * Releases all resources used by the IChannel instance.
         */
        void Release();
    }

    public interface IRtmMessage
    {
        /**
         * Retrieves the message type.
         */
        int GetMsgType();

        /**
         * Sets the content of the text message or the text description of the raw message.
         * * Note 
         * The maximum length is 32 KB.
          */
        void SetText(string text);

        /**
         * Retrieves the content of the text message or the text description of the raw message.
         */
        string GetText();

        /**
         * Allows the receiver to retrieve the timestamp of when the messaging server receives this message.
         * * Note 
         * The returned timestamp is on a millisecond time-scale. It is for demonstration purposes only, not for strict ordering of messages.
       */
        Int64 GetServerReceivedTs();

        /**
         * Allows the receiver to check whether this message has been cached on the server (Applies to peer-to-peer message only).
        */
        bool IsOfflineMessage();

        string GetRawData();
        void SetRawData(string data);
    }

#if UNITY_IOS || UNITY_STANDALONE_OSX|| UNITY_EDITOR_OSX
    public interface IRtmCallKit
    {
        IntPtr GetHandle();
        void SendLocalInvitation(IRtmLocalCallInvitation call, RtmWrapperDll.OnCompletion callback);
        void CancelLocalInvitation(IRtmLocalCallInvitation call, RtmWrapperDll.OnCompletion callback);
        void AcceptRemoteInvitation(IRtmRemoteCallInvitation call, RtmWrapperDll.OnCompletion callback);
        void RefuseRemoteInvitation(IRtmRemoteCallInvitation call, RtmWrapperDll.OnCompletion callback);
    }
#endif

    public interface IRtmLocalCallInvitation
    {
        IntPtr GetHandle();
        string GetCalleeId();
        void SetContent(string content);
        string GetContent();
        void SetChannelId(string channelId);
        string GetChannelId();
        string GetResponse();
        int GetState();
    }

    public interface IRtmRemoteCallInvitation
    {
        IntPtr GetHandle();
        string GetCallerId();
        string GetContent();
        void SetResponse(string response);
        string GetResponse();
        string GetChannelId();
        int GetState();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RtmAttribute
    {
        public string key;
        public string value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RtmChannelAttribute
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string key;
        [MarshalAs(UnmanagedType.LPStr)]
        public string value;

        public string lastUpdateUserId;
        public long lastUpdateTs;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RtmChanneAttributeOptions
    {
        public bool enableNotificationToChannelMembers;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PeerOnlineStatus
    {
        public string peerId;
        public bool isOnline;
        public int onlineState;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ChannelMemberCount
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string channelId;
        [MarshalAs(UnmanagedType.U4)]
        public int count;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SendMessageOptions
    {
        public bool enableOfflineMessaging;
        public bool enableHistoricalMessaging;
    }

}
