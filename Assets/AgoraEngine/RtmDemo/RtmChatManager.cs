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
        [SerializeField] string ImageURL = "/Users/chengr/Downloads/mono-boad.jpg";

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
#if UNITY_EDITOR
            rtmClient.SetLogFile("./rtm_log.txt");
#endif

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

            // image
            clientEventHandler.OnImageMessageReceivedFromPeer = OnImageMessageReceivedFromPeerHandler;
            clientEventHandler.OnImageMediaUploadResult = OnImageMediaUploadResultHandler;
            clientEventHandler.OnSendMessageResult = OnSendMessageResultHandler;
            clientEventHandler.OnMediaDownloadToFileResult = OnMediaDownloadToFileResultHandler;
            clientEventHandler.OnMediaDownloadToMemoryResult = OnMediaDownloadToMemoryResultHandler;


            bool initialized = ShowDisplayTexts();
            if (initialized)
            {
                string ver = RtmClient.GetSdkVersion();
                messageDisplay.AddTextToDisplay("RTM version " + ver + " initialized.", Message.MessageType.Info);
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

        public void SendMessageToChannel()
        {
            string msg = channelMsgInputBox.text;
            string peer = "[channel:" + ChannelName + "]";

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
                message: rtmClient.CreateMessage(msg),
                enableOfflineMessaging: true,
                enableHistoricalMessaging: true);

            peerMessageBox.text = "";
        }

        public void QueryPeersOnlineStatus()
        {
            long req = 222222;
            rtmClient.QueryPeersOnlineStatus(new string[] { queryUsersBox.text }, req);
        }

        #region  --Image Send / Receive ---------------------------
        string ImageMediaId { get; set; }
        // Sender will get this assign in callback
        ImageMessage RcvImageMessage { get; set; }
        
        public void UploadImageToPeer()
        {
            long requestId = 10002;
            int rc = rtmClient.CreateImageMessageByUploading(ImageURL, requestId);

            Debug.LogFormat("Sending image {0} ---> rc={1}", ImageURL, rc);
        }

        public void GetImageByMediaId()
        {
            //string mediaID = "0d535b2f651da537d7eb0975e806e308deb796076be2501cb72d3901e66c55ad12z2z62737z512z508";
            string mediaID = RcvImageMessage.GetMediaId();
            // int rc = rtmClient.DownloadMediaToFile(mediaID, "/Users/chengr/Downloads/UnityDownload.jpg", 100022);
            int rc = rtmClient.DownloadMediaToMemory(mediaID, 100023);


            Debug.LogFormat("Download image {0} ---> rc={1}", mediaID, rc);
        }

        public void SendImageToPeer()
        { 
            ImageMessage message = rtmClient.CreateImageMessageByMediaId(ImageMediaId);
            string peer = peerUserBox.text;
            if (string.IsNullOrEmpty(peer))
            {
                Debug.LogError("You must enter peer id in the input textfield!");
                return;
            }
            rtmClient.SendMessageToPeer(
                peerId: peer,
                message: message,
                enableOfflineMessaging: true,
                enableHistoricalMessaging: true);
        }

        #endregion
        #endregion


        void ShowCurrentChannelName()
        {
            ChannelName = channelNameInput.GetComponent<InputField>().text;
            Debug.Log("Channel name is " + ChannelName);
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
            string msg = "channel OnMemberJoinedHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
        }

        void OnMemberLeftHandler(int id, RtmChannelMember member)
        {
            string msg = "channel OnMemberLeftHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
        }


        void OnSendMessageResultHandler(int id, long messageId, PEER_MESSAGE_ERR_CODE errorCode)
        {
            Debug.LogFormat("Sent message with id:{0} MessageId:{1} errorCode:{2}", id, messageId, errorCode);
	    }

        void OnImageMediaUploadResultHandler(int id, long requestId, ImageMessage imageMessage, UPLOAD_MEDIA_ERR_CODE errorCode)
        {
            string msg = string.Format("Upload image with id:{0} MessageId:{1} errorCode:{2} MediaID:{3}", id, requestId, errorCode, imageMessage.GetMediaId());
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
            ImageMediaId = imageMessage.GetMediaId();
	    }

        void OnImageMessageReceivedFromPeerHandler(int id, string peerId, ImageMessage imageMessage)
        {
            string msg = string.Format("received image message with id:{0} peer:{1} mediaID:{2}", id, peerId, imageMessage.GetMediaId());
            Debug.Log(msg);
            messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
            RcvImageMessage = imageMessage;
        }

        void OnMediaDownloadToFileResultHandler(int id, long requestId, DOWNLOAD_MEDIA_ERR_CODE code)
        { 
            Debug.LogFormat("Download id:{0} requestId:{1} errorCode:{2}", id, requestId, code);
	    }

        void OnMediaDownloadToMemoryResultHandler(int id, long requestId, byte[] memory, long length, DOWNLOAD_MEDIA_ERR_CODE code)
        {
            Debug.Log("OnMediaDownloadToMemoryResultHandler requestId = " + requestId + " ,length = " + length);
            //messageDisplay.AddImageToDisplay(memory, RcvImageMessage.GetWidth(), RcvImageMessage.GetHight());
            messageDisplay.AddImageToDisplay(memory);
        }
        #endregion
    }

}
