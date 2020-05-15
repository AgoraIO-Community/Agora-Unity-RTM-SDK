using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace io.agora.rtm
{
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
} // namespace
