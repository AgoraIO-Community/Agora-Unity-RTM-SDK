using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using agora_rtm;

namespace io.agora.rtm.demo
{
    public class RtmChatManager : MonoBehaviour
    {
        [Header("Agora Properties")]
        [SerializeField]
        private string appId = "";
        [SerializeField]
        private string token = "";


        [Header("Application Properties")]

        [SerializeField] InputField userNameInput, channelNameInput;
        [SerializeField] InputField channelMsgInputBox;
        [SerializeField] InputField peerUserBox;
        [SerializeField] InputField peerMessageBox;
        [SerializeField] InputField queryUsersBox;
        [SerializeField] Text appIdDisplayText;
        [SerializeField] Text tokenDisplayText;

        [SerializeField] MessageDisplay messageDisplay;

        private RtmClient rtmClient = null;
        private RtmChannel channel;

        private RtmClientEventHandler clientEventHandler;
        private RtmChannelEventHandler channelEventHandler;

        string _userName = "";
        string UserName {
            get { return _userName; }
            set {
                _userName = value;
                PlayerPrefs.SetString("RTM_USER", _userName);
                PlayerPrefs.Save();
            }
        }

        string _channelName = "";
        string ChannelName
        {
            get { return _channelName; }
            set { 
		        _channelName = value;
                PlayerPrefs.SetString("RTM_CHANNEL", _channelName);
                PlayerPrefs.Save();
	        }
        }

        private void Awake()
        {
            userNameInput.text = PlayerPrefs.GetString("RTM_USER", "");
            channelNameInput.text = PlayerPrefs.GetString("RTM_CHANNEL", "");
        }

        // Start is called before the first frame update
        void Start()
        {
            clientEventHandler = new RtmClientEventHandler();
            channelEventHandler = new RtmChannelEventHandler();

            rtmClient = new RtmClient(appId, clientEventHandler);
            rtmClient.SetLogFile("./rtm_log.txt");

            clientEventHandler.OnQueryPeersOnlineStatusResult = OnQueryPeersOnlineStatusResultHandler;
            clientEventHandler.OnLoginSuccess = OnClientLoginSuccessHandler;
            clientEventHandler.OnLoginFailure = OnClientLoginFailureHandler;
            clientEventHandler.OnMessageReceivedFromPeer = OnMessageReceivedFromPeerHandler;

            channelEventHandler.OnJoinSuccess = OnJoinSuccessHandler;
            channelEventHandler.OnJoinFailure = OnJoinFailureHandler;
            channelEventHandler.OnLeave = OnLeaveHandler;
            channelEventHandler.OnMessageReceived = OnChannelMessageReceivedHandler;

            // Optional, tracking members
            channelEventHandler.OnMemberCountUpdated = OnMemberCountUpdatedHandler;
            channelEventHandler.OnMemberJoined = OnMemberJoinedHandler;
            channelEventHandler.OnMemberLeft = OnMemberLeftHandler;


            bool initialized = ShowDisplayTexts();
            if (initialized)
            {
                messageDisplay.AddTextToDisplay("RTM initialized", Message.MessageType.Info);
            }
            else
            {
                messageDisplay.AddTextToDisplay("RTM not initialized", Message.MessageType.Info);
            }
        }

        void OnApplicationQuit()
        {
            if (channel != null)
            {
                channel.Release();
                channel = null;
            }
            if (rtmClient != null)
            {
                rtmClient.Release(true);
                rtmClient = null;
            }
        }

        #region Button Events
        public void Login()
        {
            UserName = userNameInput.text;

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(appId))
            {
                Debug.LogError("We need a username and appId to login");
                return;
            }

            rtmClient.Login(token, UserName);
        }

        public void Logout()
        {
            messageDisplay.AddTextToDisplay(UserName + " logged out of the rtm", Message.MessageType.Info);
            rtmClient.Logout();
        }

        public void ChannelMemberCountButtonPressed()
        {
            int totalMemebers = channel.GetMembers();
            messageDisplay.AddTextToDisplay("Total members = " + totalMemebers, Message.MessageType.Info);
        }

        public void JoinChannel()
        {
            ChannelName = channelNameInput.GetComponent<InputField>().text;
            channel = rtmClient.CreateChannel(ChannelName, channelEventHandler);
            ShowCurrentChannelName();
            channel.Join();
        }

        public void LeaveChannel()
        {
            messageDisplay.AddTextToDisplay(UserName + " left the chat", Message.MessageType.Info);
            channel.Leave();
        }

        private void ShowCurrentChannelName()
        {
            ChannelName = channelNameInput.GetComponent<InputField>().text;
            Debug.Log("Channel name is " + ChannelName);
        }

        public void SendMessageToChannel()
        {
            string msg = channelMsgInputBox.text;
            string peer = "[channel:"+ChannelName+"]";

            string displayMsg = string.Format("{0}->{1}: {2}", UserName, peer, msg);

            messageDisplay.AddTextToDisplay(displayMsg, Message.MessageType.PlayerMessage);
            channel.SendMessage(rtmClient.CreateMessage(msg));
        }

        public void SendPeerMessage()
        {
            string msg = peerMessageBox.text;
            string peer = peerUserBox.text;

            string displayMsg = string.Format("{0}->{1}: {2}", UserName, peer, msg);
            messageDisplay.AddTextToDisplay(displayMsg, Message.MessageType.PlayerMessage);

            rtmClient.SendMessageToPeer(
		        peerId: peer,
		        message:rtmClient.CreateMessage(msg),
		        enableOfflineMessaging: true, 
		        enableHistoricalMessaging: true);

            peerMessageBox.text = "";
        }

        public void QueryPeersOnlineStatus()
        {
            long req = 222222;
            rtmClient.QueryPeersOnlineStatus( new string[] { queryUsersBox.text }, req);
        }


        #endregion

        bool ShowDisplayTexts()
        {
            int showLength = 6;
            if (string.IsNullOrEmpty(appId) || appId.Length < showLength)
            {
                Debug.LogError("App ID is not set, please set it in " + gameObject.name);
                appIdDisplayText.text = "APP ID NOT SET";
                appIdDisplayText.color = Color.red;
                return false;
            }
            else
            {
                appIdDisplayText.text = "appid = ********" + appId.Substring(appId.Length - showLength, showLength);
            }

            if (string.IsNullOrEmpty(token) || token.Length < showLength)
            {
                tokenDisplayText.text = "token = null";
            }
            else
            {
                tokenDisplayText.text = "token = ********" + token.Substring(token.Length - showLength, showLength);

            }
            return true;
        }

        #region EventHandlers

        void OnQueryPeersOnlineStatusResultHandler(int id, long requestId, PeerOnlineStatus[] peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode)
        {
            if (peersStatus.Length > 0)
            {
                Debug.Log("OnQueryPeersOnlineStatusResultHandler requestId = " + requestId +
                " peersStatus: peerId=" + peersStatus[0].peerId +
                " online=" + peersStatus[0].isOnline +
                " onlinestate=" + peersStatus[0].onlineState);
                messageDisplay.AddTextToDisplay("User " + peersStatus[0].peerId + " online status = " + peersStatus[0].onlineState, Message.MessageType.Info);
             }
        }

        void OnJoinSuccessHandler(int id)
        {
            string msg = "channel:" + ChannelName + " OnJoinSuccess id = " + id;
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
        }

        void OnJoinFailureHandler(int id, JOIN_CHANNEL_ERR errorCode)
        {
            string msg = "channel OnJoinFailure  id = " + id + " errorCode = " + errorCode;
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Error);
        }

        void OnClientLoginSuccessHandler(int id) 
	    {
            string msg = "client login successful! id = " + id;
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
	    }

        void OnClientLoginFailureHandler(int id, LOGIN_ERR_CODE errorCode) 
	    { 
            string msg = "client login unsuccessful! id = " + id + " errorCode = " + errorCode;
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Error);
	    }

        void OnLeaveHandler(int id, LEAVE_CHANNEL_ERR errorCode)
        { 
            string msg = "client onleave id = " + id + " errorCode = " + errorCode;
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
	    }

        void OnChannelMessageReceivedHandler(int id, string userId, TextMessage message)
        { 
            Debug.Log("client OnChannelMessageReceived id = " + id + ", from user:" + userId + " text:" + message.GetText());
            messageDisplay.AddTextToDisplay(userId + ": " + message.GetText(), Message.MessageType.ChannelMessage);
	    }

        void OnMessageReceivedFromPeerHandler(int id, string peerId, TextMessage message)
        { 
            Debug.Log("client OnMessageReceivedFromPeer id = " + id + ", from user:" + peerId + " text:" + message.GetText());
            messageDisplay.AddTextToDisplay(peerId + ": " + message.GetText(), Message.MessageType.PeerMessage);
        }

        void OnMemberCountUpdatedHandler(int id, int memberCount)
        {
            Debug.Log("Member count changed to:" + memberCount);
	    }
        void OnMemberJoinedHandler(int id, RtmChannelMember member)
        {
            string msg ="channel OnMemberJoinedHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
        }

        void OnMemberLeftHandler(int id, RtmChannelMember member)
        {
            string msg = "channel OnMemberLeftHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
        }

        #endregion
    }

}
