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

#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            Instance = new RtmWrapperIOS();
#elif UNITY_ANDROID
        Instance = new RtmWrapperAndroid();
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
        /**
       * Occurs when a user logs in the Agora RTM system.
       */
        public delegate void LoginSuccess();
        public event LoginSuccess OnLoginSuccess;
        public void OnLoginSuccessCallback()
        {
            if (OnLoginSuccess != null)
                OnLoginSuccess.Invoke();
        }

        /**
        * Occurs when a user joins a channel.
        */
        public delegate void JoinSuccess();
        public event JoinSuccess OnJoinSuccess;
        protected void OnJoinSuccessCallback()
        {
            if (OnJoinSuccess != null)
                OnJoinSuccess.Invoke();
        }

        /**
         * This callback is triggered when the user has received a DTMF message from the other user.
         */
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


        /**
         * Initializes an IRtmService instance.
         */
        public abstract void Initialize();

        /**
         * Releases all resources used by the IRtmService instance.
         */
        public abstract void Release();

        /**
         * Creates the RTM service and logs in to the RTM system
         */
        public void Login(string appId, string token, string username) { CreateRtmServiceAndLogin(appId, token, username); }
        protected abstract void CreateRtmServiceAndLogin(string appId, string token, string username);

        /**
         * Logs out of the RTM releases the service 
         */
        public virtual void Logout() { LogoutAndReleaseRtmService(); }
        protected abstract void LogoutAndReleaseRtmService();

        /**
         * Allows user to join a channel
         */
        public abstract IRtmChannel JoinChannel(string channel);

        /**
         * Allows a user to leave a channel
         */
        public abstract void LeaveChannel(IRtmChannel channel);

        [StructLayout(LayoutKind.Sequential)]
        public struct SendMessageOptions
        {
            /**
             * Enables offline messaging 
             */
            [MarshalAs(UnmanagedType.I1)]
            public bool enableOfflineMessaging;

            /**
             * Enables whether to save to message history
             */
            [MarshalAs(UnmanagedType.I1)]
            public bool enableHistoricalMessaging;
        }

        /**
         * Sends a channel message.
         **
         * Note
         * * You can send messages, including peer-to-peer and channel messages, at a maximum speed of 60 queries per second.
         * */
            public abstract void SendChannelMessage(IRtmChannel channel, string channeNamel, string msg);

        /**
         * Allows a channel member to send a message to all members in the channel.
         **
         * Note
         * *You can send messages, including peer-to-peer and channel messages, at a maximum speed of 60 queries per second.
            **/
        public abstract void SendChannelMessageWithOptions(IRtmChannel channel, string channelName, string msg, SendMessageOptions smo);
        /**
        * Sends a one to one peer message.
        **
        * Note
         * * You can send messages, including peer-to-peer and channel messages, at a maximum speed of 60 queries per second.
         * */
        public abstract void SendPeerMessage(string peerId, string msg, bool enableOffline);
        /**
         * Queries the online status of the specified user(s).
         * */
        public abstract void QueryPeersOnlineStatus(string peerIdsUnformatted, ref long requestId);
        /**
         * Gets the member count of specified channel(s).
         */
        public abstract void GetChannelMemberCount(string[] channelIds, int channelCount, ref long reqId);

    }
} 
