using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chatManager : MonoBehaviour
{
    private string appId, token, userName, channelName;

    public int maxMessages = 25;

    public GameObject appIdInput, userNameInput, tokenInput, channelNameInput, chatPanel, textObj;
    public InputField chatBox;
    public InputField peerUserBox;
    public InputField peerMessageBox;
    public InputField queryUsersBox;
    public Color playerMessage, info;
    public Text channelTitleText;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    public RtmWrapper rtm;

    // Start is called before the first frame update
    void Start()
    {
        rtm.OnLoginSuccess += Rtm_OnLoginSuccess;
        rtm.OnJoinSuccess += Rtm_OnJoinSuccess;
        rtm.OnMessageReceived += Rtm_OnMessageReceived;
        rtm.OnQueryStatusReceived += Rtm_OnQueryStatusReceived;
        rtm.OnChannelMemberCountReceived += Rtm_OnChannelMemberCountReceived; ;
        SendMessageToChat("RTM initialized", Message.MessageType.info);
    }

    private void Rtm_OnChannelMemberCountReceived(long requestId, List<RtmWrapperDll.ChannelMemberCount> channelMembers)
    {
        foreach (var ch in channelMembers)
        {
            SendMessageToChat("Channel: " + ch.channelId+ " has: " + ch.count + " member(s)", Message.MessageType.info);
        }
    }

    private void Rtm_OnQueryStatusReceived(long requestId, PeerOnlineStatus peersStatus, int peerCount, int errorCode)
    {
        SendMessageToChat("Query users: " + queryUsersBox.text + ": " + (peersStatus.onlineState == 0), Message.MessageType.info);
    }

    // Update is called once per frame
    void Update()
    {

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
        appId = appIdInput.GetComponent<InputField>().text;
        userName = userNameInput.GetComponent<InputField>().text;
        token = tokenInput.GetComponent<InputField>().text;

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(appId))
        {
            Debug.LogError("We need a username and appId to login");
            return;
        }

        rtm.CreateRtmServiceAndLogin(appId, token, userName, null, null);
    }

    public void Logout()
    {
        SendMessageToChat(userName + " logged out of the rtm", Message.MessageType.info);
        rtm.LogoutAndReleaseRtmService();
    }

    public void ChannelMemberCountButtonPressed()
    {
        long reqId = 0;
        rtm.GetChannelMemberCount(new string[] { channelName }, 1, ref reqId);
    }

    public void JoinChannel ()
    {
        //Debug.Log("User: " + userName + " joined room: " + channelName);

        channelName = channelNameInput.GetComponent<InputField>().text;


        if (!rtm.LoggedIn)
            Debug.LogError("You need to login before you can join a channel");

        else
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                rtm.Join(channelName,
                () => { },
                () => { });

                _currentChannelName();

                //(value) => { Debug.LogError("Error joining/creating channel"); }
            });
        }

    }

    public void LeaveChannel()
    {
        //TODO: Add ONLeaveSuccess
        SendMessageToChat( userName +" left the chat", Message.MessageType.info);
        rtm.Leave();
       
    }

    private void _currentChannelName ()
    {
        string newChannelName = channelNameInput.GetComponent<InputField>().text;
        channelName = newChannelName;
        print("channel name is " + channelName);
    }

    public void SendMessage()
    {
        SendMessageToChat(userName + ": " + chatBox.text, Message.MessageType.playerMessage);

        //TODO: CHANNEL NAME
        rtm.sendChannelMessageWithOptions(channelName, chatBox.text, new RtmWrapper.SendMessageOptions() { enableOfflineMessaging = true });

        chatBox.text = "";
    }

    public void SendMessageToChannel(string text, Message.MessageType messageType)
    {
        SendMessageToChat(userName + ": " + text, messageType);

        //TODO: CHANNEL NAME
        rtm.sendChannelMessage(channelName, text);
    }
    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObj.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObj, chatPanel.transform);
        newMessage.textObj = newText.GetComponent<Text>();
        newMessage.textObj.text = newMessage.text;
        newMessage.textObj.color = MessageTypeColor(messageType);
        messageList.Add(newMessage);
    }

    public void SendPeerMessage()
    {
        SendMessageToChat(userName + ": " + peerMessageBox.text, Message.MessageType.playerMessage);

        rtm.sendPeerMessage(peerUserBox.text, peerMessageBox.text, 1);

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

        switch(messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }

        return color;
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
