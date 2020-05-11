using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

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


public class RtmWrapper : MonoBehaviour
{
    public static IRtmWrapper Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

#if UNITY_ANDROID
        Instance = new RtmWrapperAndroid();
#elif UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        Instance = new RtmWrapperIOS();
#else
        Instance = new RtmWrapperWindows();
#endif
    }

    public void Start()
    {
        print("rtm wrapper start");
        Instance.Initialize();
    }


    public virtual void OnDestroy()
    {
        Debug.Log("Release RTM Service");
        if (Instance.LoggedIn)
            Instance.Release();
    }
}

public abstract class IRtmWrapper
{
    protected List<string> channels = new List<string>();

    #region Events
    public delegate void LoginSuccess();
    public event LoginSuccess OnLoginSuccess;
    public void OnLoginSuccessCallback()
    {
        if (OnLoginSuccess != null)
            OnLoginSuccess.Invoke();
    }

    public delegate void JoinSuccess();
    public event JoinSuccess OnJoinSuccess;
    protected void OnJoinSuccessCallback()
    {
        if (OnJoinSuccess != null)
            OnJoinSuccess.Invoke();
    }

    public delegate void MessageReceived(string userName, string msg);
    public event MessageReceived OnMessageReceived;
    public void OnMessageReceivedCallback(string userName, string msg)
    {
        if (OnMessageReceived != null)
            OnMessageReceived.Invoke(userName, msg);
    }

    public delegate void MemberChanged(string userName, string channelId, bool joined);
    public event MemberChanged OnMemberChanged;
    public void OnMemberChangedCallback(string userName, string channelId, bool joined)
    {
        if (OnMemberChanged != null)
            OnMemberChanged(userName, channelId, joined);
    }

    public delegate void QueryStatusReceived(long requestId, PeerOnlineStatus peersStatus, int peerCount, int errorCode);
    public event QueryStatusReceived OnQueryStatusReceived;
    public void OnQueryStatusReceivedCallback(long requestId, PeerOnlineStatus peersStatus, int peerCount, int errorCode)
    {
        if (OnQueryStatusReceived != null)
            OnQueryStatusReceived(requestId, peersStatus, peerCount, errorCode);
    }

    public delegate void ChannelMemberCountReceived(long requestId, List<ChannelMemberCount> members);
    public event ChannelMemberCountReceived OnChannelMemberCountReceived;
    public void OnChannelMemberCountReceivedCallback(long requestId, List<ChannelMemberCount> members)
    {
        if (OnChannelMemberCountReceived != null)
            OnChannelMemberCountReceived(requestId, members);
    }
    #endregion

    public bool LoggedIn { get; set; }


    public abstract void Initialize();
    public abstract void Release();

    public void Login(string appId, string token, string username) { CreateRtmServiceAndLogin(appId, token, username); }
    protected abstract void CreateRtmServiceAndLogin(string appId, string token, string username);

    public virtual void Logout() { LogoutAndReleaseRtmService(); }
    protected abstract void LogoutAndReleaseRtmService();

    public abstract IRtmChannel JoinChannel(string channel);

    public abstract void LeaveChannel(IRtmChannel channel);

    [StructLayout(LayoutKind.Sequential)]
    public struct SendMessageOptions
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool enableOfflineMessaging;
        [MarshalAs(UnmanagedType.I1)]
        public bool enableHistoricalMessaging;
    }

    public abstract void SendChannelMessage(IRtmChannel channel, string channeNamel, string msg);

    public abstract void SendChannelMessageWithOptions(IRtmChannel channel, string channelName, string msg, SendMessageOptions smo);

    public abstract void SendPeerMessage(string peerId, string msg, bool enableOffline);

    /**
     * Test
     */
    public abstract void QueryPeersOnlineStatus(string peerIdsUnformatted, ref long requestId);

    public abstract void GetChannelMemberCount(string[] channelIds, int channelCount, ref long reqId);

}
