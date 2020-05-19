using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace io.agora.rtm.demo
{
    public class chatManager : MonoBehaviour
    {
        [SerializeField]
        private string appId = "";
        [SerializeField]
        private string token = "";

        private string userName, channelName;

        public int maxMessages = 25;

        public InputField userNameInput, channelNameInput;
        public GameObject chatPanel, textPrefab;
        public InputField chatBox;
        public InputField peerUserBox;
        public InputField peerMessageBox;
        public InputField queryUsersBox;
        public Color playerMessage, info;
        public Text appIdDisplayText;
        public Text tokenDisplayText;
        public int screenWidth;
        public int screenHeight;

        [SerializeField]
        List<Message> messageList = new List<Message>();

        private IRtmWrapper rtm;
        private IRtmChannel channel;

        // Start is called before the first frame update
        void Start()
        {
            rtm = RtmWrapper.Instance;
            rtm.OnLoginSuccess += Rtm_OnLoginSuccess;
            rtm.OnJoinSuccess += Rtm_OnJoinSuccess;
            rtm.OnMessageReceived += Rtm_OnMessageReceived;
            rtm.OnQueryStatusReceived += Rtm_OnQueryStatusReceived;
            rtm.OnChannelMemberCountReceived += Rtm_OnChannelMemberCountReceived;
            rtm.OnMemberChanged += Rtm_OnMemberChanged;

            bool initialized = ShowDisplayTexts();
            if (initialized)
            {
                SendMessageToChat("RTM initialized", Message.MessageType.info);
            }
            else
            {
                SendMessageToChat("RTM not initialized", Message.MessageType.info);
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
            SendMessageToChat(userName + (joined ? " joined" : " left") + " channel " + channelId, Message.MessageType.info);
        }

        private void Rtm_OnChannelMemberCountReceived(long requestId, List<ChannelMemberCount> channelMembers)
        {
            foreach (var ch in channelMembers)
            {
                SendMessageToChat("Channel: " + ch.channelId + " has: " + ch.count + " member(s)", Message.MessageType.info);
            }
        }

        private void Rtm_OnQueryStatusReceived(long requestId, PeerOnlineStatus peersStatus, int peerCount, int errorCode)
        {
            SendMessageToChat("Query users: " + queryUsersBox.text + ": " + (peersStatus.onlineState == 0), Message.MessageType.info);
        }

        private void Rtm_OnMessageReceived(string userName, string msg)
        {
            SendMessageToChat(userName + ": " + msg, Message.MessageType.playerMessage);
        }

        public void Rtm_OnJoinSuccess()
        {
            SendMessageToChat(userName + " joined the " + channelName + " channel", Message.MessageType.info);
        }

        public void Rtm_OnLoginSuccess()
        {
            SendMessageToChat(userName + " logged into the rtm", Message.MessageType.info);
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

            Debug.Log("Before calling rtm");
            rtm.Login(appId, token, userName);
        }

        public void Logout()
        {
            SendMessageToChat(userName + " logged out of the rtm", Message.MessageType.info);
            rtm.Logout();
        }

        public void ChannelMemberCountButtonPressed()
        {
            long reqId = 0;
            rtm.GetChannelMemberCount(new string[] { channelName }, 1, ref reqId);
        }

        public void JoinChannel()
        {
            //Debug.Log("User: " + userName + " joined room: " + channelName);

            channelName = channelNameInput.GetComponent<InputField>().text;


            if (!rtm.LoggedIn)
                Debug.LogError("You need to login before you can join a channel");

            else
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    channel = rtm.JoinChannel(channelName);

                    _currentChannelName();

                    //(value) => { Debug.LogError("Error joining/creating channel"); }
                });
            }
        }

        public void LeaveChannel()
        {
            //TODO: Add ONLeaveSuccess
            SendMessageToChat(userName + " left the chat", Message.MessageType.info);
            rtm.LeaveChannel(channel);

        }

        private void _currentChannelName()
        {
            string newChannelName = channelNameInput.GetComponent<InputField>().text;
            channelName = newChannelName;
            print("channel name is " + channelName);
        }

        //public void SendMessage()
        //{
        //    SendMessageToChat(userName + ": " + chatBox.text, Message.MessageType.playerMessage);

        //    //TODO: CHANNEL NAME
        //    //channel.SendChannelMessageWithOptions channelName, );
        //    SendPeerMessage(chatBox.text, new RtmWrapper.SendMessageOptions() { enableOfflineMessaging = true });

        //    chatBox.text = "";
        //}

        public void SendMessageToChannel()
        {
            SendMessageToChat(userName + ": " + chatBox.text, Message.MessageType.playerMessage);

            //TODO: CHANNEL NAME
            channel.SendMessage(chatBox.text);
            //rtm.SendChannelMessage(channelName, text);
        }
        public void SendMessageToChat(string text, Message.MessageType messageType)
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
            SendMessageToChat(userName + ": " + peerMessageBox.text, Message.MessageType.playerMessage);

            rtm.SendPeerMessage(peerUserBox.text, peerMessageBox.text, true);

            peerMessageBox.text = "";
        }

        public void QueryPeersOnlineStatus()
        {
            long req = 0;
            rtm.QueryPeersOnlineStatus(queryUsersBox.text, ref req);
        }

        Color MessageTypeColor(Message.MessageType messageType)
        {
            Color color = info;

            switch (messageType)
            {
                case Message.MessageType.playerMessage:
                    color = playerMessage;
                    break;
            }

            return color;
        }

        public void SetWidth(int newWidth)
        {
            screenWidth = newWidth;
        }

        public void SetHeight(int newHeight)
        {
            screenHeight = newHeight;
        }

        public void SetScreenRes()
        {
            Screen.SetResolution(screenWidth, screenHeight, false);
        }
    }

    [System.Serializable]
    public class Message
    {
        public string text;
        public Text textObj;
        public MessageType messageType;

        public enum MessageType
        {
            playerMessage,
            info
        }
    }
}
