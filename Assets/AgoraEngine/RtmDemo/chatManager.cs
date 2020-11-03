using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using agora_rtm;

namespace io.agora.rtm.demo
{
    public class chatManager : MonoBehaviour
    {
        [Header("Agora Properties")]
        [SerializeField]
        private string appId = "";
        [SerializeField]
        private string token = "";

        private string userName, channelName;

        [Header("Application Properties")]
        public int maxMessages = 25;

        public InputField userNameInput, channelNameInput;
        public GameObject chatPanel, textPrefab;
        public InputField channelMsgInputBox;
        public InputField peerUserBox;
        public InputField peerMessageBox;
        public InputField queryUsersBox;
        public MessageColorStruct MessageColors;
        public Text appIdDisplayText;
        public Text tokenDisplayText;

        [SerializeField]
        List<Message> messageList = new List<Message>();

        private RtmClient rtmClient = null;
        private RtmChannel channel;
        private RtmClientEventHandler clientEventHandler;
        private RtmChannelEventHandler channelEventHandler;

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
                AddTextToDisplay("RTM initialized", Message.MessageType.Info);
            }
            else
            {
                AddTextToDisplay("RTM not initialized", Message.MessageType.Info);
            }
        }

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

        private void Rtm_OnMemberChanged(string userName, string channelId, bool joined)
        {
            AddTextToDisplay(userName + (joined ? " joined" : " left") + " channel " + channelId, Message.MessageType.Info);
        }

        private void Rtm_OnChannelMemberCountReceived(long requestId, List<ChannelMemberCount> channelMembers)
        {
            foreach (var ch in channelMembers)
            {
                AddTextToDisplay("Channel: " + ch.channelId + " has: " + ch.count + " member(s)", Message.MessageType.Info);
            }
        }

        private void Rtm_OnQueryStatusReceived(long requestId, PeerOnlineStatus peersStatus, int peerCount, int errorCode)
        {
            AddTextToDisplay("Query users: " + queryUsersBox.text + ": " + (peersStatus.onlineState == 0), Message.MessageType.Info);
        }

        private void Rtm_OnMessageReceived(string userName, string msg)
        {
            AddTextToDisplay(userName + ": " + msg, Message.MessageType.PlayerMessage);
        }

        public void Rtm_OnJoinSuccess()
        {
            AddTextToDisplay(userName + " joined the " + channelName + " channel", Message.MessageType.Info);
        }

        public void Rtm_OnLoginSuccess()
        {
            AddTextToDisplay(userName + " logged into the rtm", Message.MessageType.Info);
        }

        public void Login()
        {
            Debug.Log("Pressed login button");
            userName = userNameInput.text;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(appId))
            {
                Debug.LogError("We need a username and appId to login");
                return;
            }

            Debug.Log("Before calling rtm login");
            rtmClient.Login(token, userName);
        }

        public void Logout()
        {
            AddTextToDisplay(userName + " logged out of the rtm", Message.MessageType.Info);
            rtmClient.Logout();
        }

        public void ChannelMemberCountButtonPressed()
        {
            int totalMemebers = channel.GetMembers();
            AddTextToDisplay("Total members = " + totalMemebers, Message.MessageType.Info);
        }

        public void JoinChannel()
        {
            //Debug.Log("User: " + userName + " joined room: " + channelName);
            channelName = channelNameInput.GetComponent<InputField>().text;
            channel = rtmClient.CreateChannel(channelName, channelEventHandler);
            ShowCurrentChannelName();
            channel.Join();
        }

        public void LeaveChannel()
        {
            //TODO: Add ONLeaveSuccess
            AddTextToDisplay(userName + " left the chat", Message.MessageType.Info);
            channel.Leave();
        }

        private void ShowCurrentChannelName()
        {
            string newChannelName = channelNameInput.GetComponent<InputField>().text;
            channelName = newChannelName;
            print("channel name is " + channelName);
        }

        public void SendMessageToChannel()
        {
            string msg = channelMsgInputBox.text;
            string peer = "[channel:"+channelName+"]";

            string displayMsg = string.Format("{0}->{1}: {2}", userName, peer, msg);

            AddTextToDisplay(displayMsg, Message.MessageType.PlayerMessage);
            channel.SendMessage(rtmClient.CreateMessage(msg));
        }

        public void AddTextToDisplay(string text, Message.MessageType messageType)
        {
            if (messageList.Count >= maxMessages)
            {
                Destroy(messageList[0].textObj.gameObject);
                messageList.Remove(messageList[0]);
            }

            Message newMessage = new Message();
            newMessage.text = text;

            GameObject newText = Instantiate(textPrefab, chatPanel.transform);
            newMessage.textObj = newText.GetComponent<Text>();
            newMessage.textObj.text = newMessage.text;
            newMessage.textObj.color = MessageTypeColor(messageType);
            messageList.Add(newMessage);
        }

        public void SendPeerMessage()
        {
            string msg = peerMessageBox.text;
            string peer = peerUserBox.text;

            string displayMsg = string.Format("{0}->{1}: {2}", userName, peer, msg);
            AddTextToDisplay(displayMsg, Message.MessageType.PlayerMessage);

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

        Color MessageTypeColor(Message.MessageType messageType)
        {
            Color color = MessageColors.infoColor;

            switch (messageType)
            {
                case Message.MessageType.PlayerMessage:
                    color = MessageColors.playerColor;
                    break;
                case Message.MessageType.ChannelMessage:
                    color = MessageColors.channelColor;
                    break;
                case Message.MessageType.PeerMessage:
                    color = MessageColors.peerColor;
                    break;
                case Message.MessageType.Error:
                    color = MessageColors.errorColor;
                    break;
            }

            return color;
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

        #region EventHandlers

        void OnQueryPeersOnlineStatusResultHandler(int id, long requestId, PeerOnlineStatus[] peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode)
        {
            if (peersStatus.Length > 0)
            {
                Debug.Log("OnQueryPeersOnlineStatusResultHandler requestId = " + requestId +
                " peersStatus: peerId=" + peersStatus[0].peerId +
                " online=" + peersStatus[0].isOnline +
                " onlinestate=" + peersStatus[0].onlineState);
                AddTextToDisplay("User " + peersStatus[0].peerId + " online status = " + peersStatus[0].onlineState, Message.MessageType.Info);
             }
        }

        void OnJoinSuccessHandler(int id)
        {
            string msg = "channel:" + channelName + " OnJoinSuccess id = " + id;
            Debug.Log(msg);
            AddTextToDisplay(msg, Message.MessageType.Info);
        }

        void OnJoinFailureHandler(int id, JOIN_CHANNEL_ERR errorCode)
        {
            string msg = "channel OnJoinFailure  id = " + id + " errorCode = " + errorCode;
            Debug.Log(msg);
            AddTextToDisplay(msg, Message.MessageType.Error);
        }

        void OnClientLoginSuccessHandler(int id) 
	    {
            string msg = "client login successful! id = " + id;
            Debug.Log(msg);
            AddTextToDisplay(msg, Message.MessageType.Info);
	    }

        void OnClientLoginFailureHandler(int id, LOGIN_ERR_CODE errorCode) 
	    { 
            string msg = "client login unsuccessful! id = " + id + " errorCode = " + errorCode;
            Debug.Log(msg);
            AddTextToDisplay(msg, Message.MessageType.Error);
	    }

        void OnLeaveHandler(int id, LEAVE_CHANNEL_ERR errorCode)
        { 
            string msg = "client onleave id = " + id + " errorCode = " + errorCode;
            Debug.Log(msg);
            AddTextToDisplay(msg, Message.MessageType.Info);
	    }

        void OnChannelMessageReceivedHandler(int id, string userId, TextMessage message)
        { 
            Debug.Log("client OnChannelMessageReceived id = " + id + ", from user:" + userId + " text:" + message.GetText());
            AddTextToDisplay(userId + ": " + message.GetText(), Message.MessageType.ChannelMessage);
	    }

        void OnMessageReceivedFromPeerHandler(int id, string peerId, TextMessage message)
        { 
            Debug.Log("client OnMessageReceivedFromPeer id = " + id + ", from user:" + peerId + " text:" + message.GetText());
            AddTextToDisplay(peerId + ": " + message.GetText(), Message.MessageType.PeerMessage);
        }

        void OnMemberCountUpdatedHandler(int id, int memberCount)
        {
            Debug.Log("Member count changed to:" + memberCount);
	    }
        void OnMemberJoinedHandler(int id, RtmChannelMember member)
        {
            string msg ="channel OnMemberJoinedHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
            Debug.Log(msg);
            AddTextToDisplay(msg, Message.MessageType.Info);
        }

        void OnMemberLeftHandler(int id, RtmChannelMember member)
        {
            string msg = "channel OnMemberLeftHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
            Debug.Log(msg);
            AddTextToDisplay(msg, Message.MessageType.Info);
        }

        #endregion
    }

    [System.Serializable]
    public class Message
    {
        public string text;
        public Text textObj;
        public MessageType messageType;

        public enum MessageType
        {
            Info,
            Error,
            PlayerMessage,
            ChannelMessage,
            PeerMessage
        }
    }

    [System.Serializable]
    public struct MessageColorStruct
    {
        public Color infoColor,errorColor, playerColor, peerColor, channelColor;
    }
}
