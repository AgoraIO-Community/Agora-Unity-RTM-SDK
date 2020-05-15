using System;
using System.Runtime.InteropServices;

namespace io.agora.rtm
{
    public interface IRtmChannel
    {
        void Join();
        void Leave();
        void SendMessage(string msg);
        void SendMessage(string msg, IRtmWrapper.SendMessageOptions smo);

        string GetId();
        void GetMembers();

        void Release();
    }

    public interface IRtmMessage
    {
        int GetMsgType();
        void SetText(string text);
        string GetText();

        Int64 GetServerReceivedTs();
        bool IsOfflineMessage();

        string GetRawData();
        void SetRawData(string data);
    }

    public interface IRtmCallKit
    {
        IntPtr GetHandle();
        void SendLocalInvitation(IRtmLocalCallInvitation call, RtmWrapperDll.OnCompletion callback);
        void CancelLocalInvitation(IRtmLocalCallInvitation call, RtmWrapperDll.OnCompletion callback);
        void AcceptRemoteInvitation(IRtmRemoteCallInvitation call, RtmWrapperDll.OnCompletion callback);
        void RefuseRemoteInvitation(IRtmRemoteCallInvitation call, RtmWrapperDll.OnCompletion callback);
    }

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
        //[MarshalAs(UnmanagedType.LPStr)]
        public string peerId;
        //[MarshalAs(UnmanagedType.U4)]
        public bool isOnline;
        //[MarshalAs(UnmanagedType.U4)]
        public int onlineState;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ChannelMemberCount
    {
        [MarshalAs(UnmanagedType.LPStr)]
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