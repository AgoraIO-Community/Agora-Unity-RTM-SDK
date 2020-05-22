using System;
using System.Collections.Generic;
using UnityEngine;

namespace io.agora.rtm
{
#if UNITY_ANDROID
    public class RtmWrapperAndroid : IRtmWrapper
    {
        AndroidJavaClass rtmClientClass;
        static AndroidJavaObject rtmClient;


        public class RtmChannelAndroid : IRtmChannel
        {
            private AndroidJavaObject chHandler;
            private Action<object> successCallback;
            public RtmChannelAndroid(AndroidJavaObject chHandler)
            {
                this.chHandler = chHandler;
            }

            public RtmChannelAndroid(AndroidJavaObject chHandler, Action<object> successCallback)
            {
                this.chHandler = chHandler;
                this.successCallback = successCallback;
            }

            public void Join()
            {
                chHandler.Call("join", new ResultListener("join channel", successCallback, (o) => { }));
            }

            public void Leave()
            {
                chHandler.Call("leave", new ResultListener("leave channel", (res) => { }, (o) => { }));
            }

            public void SendMessage(string msg)
            {
                chHandler.Call("sendMessage", CreateAndroidMessage(msg), new ResultListener("send channel message"));
            }

            public void SendMessage(string msg, SendMessageOptions smo)
            {
            }

            public string GetId()
            {
                return chHandler.Call<string>("getId");
            }

            public void GetMembers()
            {
                chHandler.Call("getMembers", new ResultListener("send channel message", (res) =>
                {
                    var javaClass = new AndroidJavaClass("java.lang.Class");
                    var arrayListClass = javaClass.CallStatic<AndroidJavaObject>("forName", "java.util.ArrayList");
                    var listAO = arrayListClass.Call<AndroidJavaObject>("cast", res);

                    Debug.Log("Channel members:");
                    var channelMembers = new List<ChannelMember>();
                    for (var i = 0; i < listAO.Call<int>("size"); i++)
                    {
                        var value = listAO.Call<AndroidJavaObject>("get", i);
                        var channelId = value.Call<string>("getChannelID");
                        var userId = value.Call<string>("getChannelID");
                        channelMembers.Add(new ChannelMember(value));

                        Debug.Log(channelId + " - " + userId);
                    }
                },
                (err) =>
                {
                }));
            }

            public void Release()
            {
                chHandler.Call("release");
            }
        }


        class ResultListener : AndroidJavaProxy
        {
            private Action<object> OnSuccess;
            private Action<object> OnFailure;
            private string owner;

            public ResultListener(string owner) : base("io.agora.rtm.ResultCallback")
            {
                this.owner = owner;
            }

            public ResultListener(string owner, Action<object> osc, Action<object> ofc) : base("io.agora.rtm.ResultCallback")
            {
                this.owner = owner;
                OnSuccess = osc;
                OnFailure = ofc;
            }

            public void onSuccess(AndroidJavaObject o)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.Log(owner + ": success!");
                });

                if (OnSuccess != null)
                    OnSuccess.Invoke(o);
            }

            public void onFailure(AndroidJavaObject o)
            {
                int errorCode = o.Call<int>("getErrorCode");

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.Log(owner + ": failure: " + errorCode);
                });

                if (OnFailure != null)
                    OnFailure.Invoke(o);
            }
        }

        class ChannelListener : AndroidJavaProxy
        {
            private Action<string, string> OnMessageReceived;
            private Action<string, string, bool> OnMemberChanged;

            public ChannelListener(Action<string, string> omr) : base("io.agora.rtm.RtmChannelListener")
            {
                OnMessageReceived = omr;
            }

            public ChannelListener(Action<string, string> omr, Action<string, string, bool> omc) : base("io.agora.rtm.RtmChannelListener")
            {
                OnMessageReceived = omr;
                OnMemberChanged = omc;
            }

            public void onMemberCountUpdated(int i)
            {
            }

            public void onAttributesUpdated(AndroidJavaObject list)
            {

            }

            public void onMessageReceived(AndroidJavaObject message, AndroidJavaObject member)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    var peerId = member.Call<string>("getUserId");
                    var msg = message.Call<string>("getText");
                    Debug.Log("received message from " + peerId + " saying " + msg);
                    if (OnMessageReceived != null)
                        OnMessageReceived.Invoke(peerId, msg);
                });
            }

            public void onMemberJoined(AndroidJavaObject member)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (OnMemberChanged != null)
                        OnMemberChanged.Invoke(member.Call<string>("getUserId"), member.Call<string>("getChannelId"), true);
                });
            }

            public void onMemberLeft(AndroidJavaObject member)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (OnMemberChanged != null)
                        OnMemberChanged.Invoke(member.Call<string>("getUserId"), member.Call<string>("getChannelId"), false);
                });
            }
        }

        class ServiceListener : AndroidJavaProxy
        {
            private Action<string, string> OnMessageReceived;

            public ServiceListener(Action<string, string> omr) : base("io.agora.rtm.RtmClientListener")
            {
                OnMessageReceived = omr;
            }

            public void onConnectionStateChanged(int state, int reason)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.Log("new connection - state: " + state + " -- reason: " + reason);
                });
            }

            public void onMessageReceived(AndroidJavaObject rtmMessage, String peerId)
            {
                var msg = rtmMessage.Call<string>("getText");
                Debug.Log(peerId + " sent the following direct message: " + msg);

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (OnMessageReceived != null)
                        OnMessageReceived.Invoke(peerId, msg);
                });
            }

            public void onTokenExpired()
            {

            }

        }

        public class ChannelMember
        {
            private AndroidJavaObject chMember;

            public ChannelMember(AndroidJavaObject chMember)
            {
                this.chMember = chMember;
            }

            public string GetUserId()
            {
                return chMember.Call<string>("getUserId");
            }

            public string GetChannelId()
            {
                return chMember.Call<string>("getChannelId");
            }
        }

        public class RtmChannelAttribute
        {
            private AndroidJavaObject chAttr;

            public RtmChannelAttribute(AndroidJavaObject chAttr)
            {
                this.chAttr = chAttr;
            }


            public void setKey(string key)
            {
                chAttr.Call("setKey", CreateAndroidStr(key));
            }

            public string getKey()
            {
                return chAttr.Call<string>("getKey");
            }


            public void setValue(string value)
            {
                chAttr.Call("setValue", CreateAndroidStr(value));
            }

            public string getValue()
            {
                return chAttr.Call<string>("getValue");
            }

            public string getLastUpdateUserId()
            {
                return chAttr.Call<string>("getLastUpdateUserId");
            }

            public long getLastUpdateTs()
            {
                return chAttr.Call<long>("getLastUpdateTs");
            }

        }

        public class ChannelAttributeOptions
        {
            private AndroidJavaObject chAttr;

            public ChannelAttributeOptions(AndroidJavaObject chAttr)
            {
                this.chAttr = chAttr;
            }

            public bool enableNotificationToChannelMembers
            {
                get
                {
                    return chAttr.Call<bool>("getEnableNotificationToChannelMembers");
                }
                set
                {
                    chAttr.Call("setEnableNotificationToChannelMembers", value);
                }
            }

        }


        public static AndroidJavaObject CreateAndroidMessage(string content)
        {
            var message = rtmClient.Call<AndroidJavaObject>("createMessage");
            message.Call("setText", new AndroidJavaObject("java.lang.String", content));

            return message;
        }

        private static AndroidJavaObject CreateAndroidStr(string str)
        {
            return new AndroidJavaObject("java.lang.String", str);
        }

        private void Login(string userName, string token)
        {
            rtmClient.Call("login", CreateAndroidStr(token), CreateAndroidStr(userName), new ResultListener("login", (o) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    LoggedIn = true;
                    OnLoginSuccessCallback();
                });
            }, (o) =>
            {
                int errorCode = ((AndroidJavaObject)o).Call<int>("getErrorCode");
                if (errorCode == 5)
                    Debug.Log("Invalid Token!");
                if (errorCode == 6)
                    Debug.Log("Token Expired!");
            }));
        }

        public override void Logout()
        {
            rtmClient.Call("logout", new ResultListener("logout", (o) => { }, (o) => { }));
        }

        public override void Release()
        {
            rtmClient.Call("release");
        }

        protected IRtmChannel CreateAndJoinChannel(string channelName, Action<object> successCallback)
        {
            var channelJavaObj = rtmClient.Call<AndroidJavaObject>("createChannel", CreateAndroidStr(channelName),
                new ChannelListener(
                    (peerId, msg) => { OnMessageReceivedCallback(peerId, msg); },
                    (peerId, channelId, joined) => { OnMemberChangedCallback(peerId, channelId, joined); }
                    ));

            var channel = new RtmChannelAndroid(channelJavaObj, successCallback);
            channel.Join();

            return channel;
        }

        private void GetChannelMembers()
        {
        }


        public void SubscribePeersOnlineStatus(string[] peerIds)
        {
            var peerIdsSet = new AndroidJavaObject("java.util.HashSet");
            foreach (var peerId in peerIds)
                peerIdsSet.Call<bool>("add", CreateAndroidStr(peerId));


            rtmClient.Call("subscribePeersOnlineStatus", peerIdsSet, new ResultListener("subscribe peers online status"));
        }

        public void UnsubscribePeersOnlineStatus(string[] peerIds)
        {
            var peerIdsSet = new AndroidJavaObject("java.util.HashSet");
            foreach (var peerId in peerIds)
                peerIdsSet.Call<bool>("add", CreateAndroidStr(peerId));


            rtmClient.Call("unsubscribePeersOnlineStatus", peerIdsSet, new ResultListener("unsubscribe peers online status"));
        }

        public void RenewToken(string token, Action<string> onTokenRenewal)
        {
            rtmClient.Call("renewToken", CreateAndroidStr(token), new ResultListener("renew token", (res) =>
            { },
            (err) => { }));
        }

        private void SetLocalUserAttributes()
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            list.Call<bool>("add", "io.agora.rtm.RtmAttribute.RtmAttribute");
            rtmClient.Call("setLocalUserAttributes", list, new ResultListener("renew token", (res) => { }, (err) => { }));
        }

        private void addOrUpdateLocalUserAttributes()
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            rtmClient.Call("addOrUpdateLocalUserAtributesByKeys", list, new ResultListener("add or update local user attributes by keys",
                (o) => { }, (err) => { }));
        }


        private void deleteLocalUserAttributes()
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            rtmClient.Call("addOrUpdateLocalUserAtributesByKeys", list, new ResultListener("add or update local user attributes by keys",
                (o) => { }, (err) => { }));
        }

        private void clearLocalUserAttributes()
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            rtmClient.Call("clearLocalUserAttributes", list, new ResultListener("listener or update local user attributes by keys",
                (o) => { }, (err) => { }));
        }

        private void getUserAttributes(string userId)
        {
            rtmClient.Call("getUserAttributes", CreateAndroidStr(userId), new ResultListener("waterfall", (o) => { }, (err) => { }));
        }

        private void getUserAttributesByKeys(string userId, List<string> keys)
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            foreach (var key in keys)
            {
                list.Call<bool>("add", key);
            }

            rtmClient.Call("getUserAttributesByKey", CreateAndroidStr(userId), list, new ResultListener("Get User Attributes By Keys", (o) => { }, (err) => { }));
        }

        class RtmChannelId
        {

        }

        private void setChannelAttributes(string channelId, List<RtmChannelAttribute> keys, int commandOptions)
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            foreach (var value in keys)
            {
                list.Call<bool>("add", value);
            }

            rtmClient.Call("setChannelAttributes", channelId, list, commandOptions, new ResultListener("set Channel Attributes", (o) => { }, (err) => { }));
        }

        private void addOrUpdateChannelAttributes(string channelId, List<RtmChannelAttribute> keys, int commandOptions)
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            foreach (var value in keys)
            {
                list.Call<bool>("add", value);
            }

            rtmClient.Call("addOrUpdateChannelAttributes", channelId, list, commandOptions, new ResultListener("set Channel Attributes", (o) => { }, (err) => { }));
        }

        private void deleteChannelAttributesByKeys(string channelId, List<string> keys, int commandOptions)
        {
            var list = new AndroidJavaObject("java.util.ArrayList");
            foreach (var value in keys)
            {
                list.Call<bool>("add", value);
            }

            rtmClient.Call("deleteChannelAttributesByKeys", channelId, list, commandOptions, new ResultListener("Delete Channel attributes", (o) => { }, (err) => { }));
        }


        private void clearChannelAttributes(string channelId, int commandOptions)
        {
            var list = new AndroidJavaObject("java.util.ArrayList");

            rtmClient.Call("clearChannelAttributes", channelId, list, commandOptions, new ResultListener("Clear Channel attributes", (o) => { }, (err) => { }));
        }
        private void GetChannelAttributes(string channelId)
        {
            rtmClient.Call("getChannelAttributes", channelId, new ResultListener("Get Channel Attribues", (o) => { }, (err) => { }));
        }


        private void GetChannelAttributesByKeys(string channelId)
        {
            rtmClient.Call("getChannelAttributesByKeys", channelId, new ResultListener("Get Channel Attribues By Keys", (o) => { }, (err) => { }));
        }


        //private void GetChannelMemberCount(List<String> channelIds)
        //{
        //    var list = new AndroidJavaObject("java.util.ArrayList");
        //    foreach(var channelId in channelIds)
        //    {
        //        list.Call<bool>("add", channelId);
        //    }

        //    rtmClient.Call("getChannelMemberCount", list, new ResultListener("Get Channel Member Count"));
        //}

        private int SetParameters(String parameters)
        {
            return rtmClient.Call<int>("setParameters", CreateAndroidStr(parameters));
        }

        private int SetLogFile(String filePath)
        {
            return rtmClient.Call<int>("setLogFile", CreateAndroidStr(filePath));
        }
        private int SetLogFilter(int filter)
        {
            return rtmClient.Call<int>("setLogFilter", filter);
        }
        private int SetLogFileSize(int fileSizeInKBytes)
        {
            return rtmClient.Call<int>("setLogFileSize", fileSizeInKBytes);
        }

        public override void Initialize()
        {
        }


        protected override void CreateRtmServiceAndLogin(string appId, string token, string username)
        {
            if (LoggedIn)
                return;

            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

            rtmClientClass = new AndroidJavaClass("io.agora.rtm.RtmClient");
            rtmClient = rtmClientClass.CallStatic<AndroidJavaObject>("createInstance", currentActivityObject, CreateAndroidStr(appId), new ServiceListener(
                (peerId, msg) => OnMessageReceivedCallback(peerId, msg)));

            Login(username, token);
        }

        protected override void LogoutAndReleaseRtmService()
        {
            if (LoggedIn)
            {
                LoggedIn = false;
                Logout();
                Release();
            }
        }

        public override IRtmChannel JoinChannel(string channelName)
        {
            return CreateAndJoinChannel(channelName, (o) =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    channels.Add(channelName);
                    Debug.Log("joined: " + channelName);
                    OnJoinSuccessCallback();
                });
            });
        }

        public override void LeaveChannel(IRtmChannel channel)
        {
            channel.Leave();
        }

        public override void SendChannelMessage(IRtmChannel channel, string channelName, string msg)
        {
            if (!channels.Contains(channelName))
            {
                Debug.LogError("You need to join the channel before you can send a message to it");
                return;
            }

            Debug.Log("send msg: " + msg);
            channel.SendMessage(msg);
        }

        public override void SendChannelMessageWithOptions(IRtmChannel channel, string channelName, string msg, IRtmWrapper.SendMessageOptions smo)
        {
            Debug.Log("send msg: " + msg);
            channel.SendMessage(msg);
        }

        public override void SendPeerMessage(string peerId, string msg, bool enableOffline)
        {
            if (string.IsNullOrEmpty(peerId))
            {
                Debug.LogError("You need to enter the user ID you want to send the message to");
                return;
            }

            var sendMessageOptions = new AndroidJavaObject("io.agora.rtm.SendMessageOptions");
            sendMessageOptions.Set("enableOfflineMessaging", enableOffline ? true : false);
            rtmClient.Call("sendMessageToPeer", CreateAndroidStr(peerId), CreateAndroidMessage(msg), sendMessageOptions, new ResultListener("send peer message"));
        }


        public override void QueryPeersOnlineStatus(string peerIdsUnformatted, ref long requestId)
        {
            var peerIds = peerIdsUnformatted.Split(' ');
            var set = new AndroidJavaObject("java.util.HashSet");
            foreach (var peerId in peerIds)
                set.Call<bool>("add", CreateAndroidStr(peerId));

            rtmClient.Call("queryPeersOnlineStatus", set, new ResultListener("queryPeersOnlineStatus",
                (res) =>
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Debug.Log("query peers success callback");
                        var javaClass = new AndroidJavaClass("java.lang.Class");
                        var hashMapClass = javaClass.CallStatic<AndroidJavaObject>("forName", "java.util.HashMap");
                        var map = hashMapClass.Call<AndroidJavaObject>("cast", res);

                        foreach (var peerId in peerIds)
                        {
                            var onlineVal = map.Call<AndroidJavaObject>("get", CreateAndroidStr(peerId));
                            var online = onlineVal.Call<string>("toString");
                            var isOnline = online == "true";
                            Debug.Log(peerId + " is " + (isOnline ? "online" : "offline"));
                            OnQueryStatusReceivedCallback(-1, new PeerOnlineStatus() { isOnline = isOnline, onlineState = isOnline ? 0 : 2, peerId = peerId }, -1, -1);
                        }
                    });
                },
                (o) =>
                {
                })
            );
        }

        public int count;
        private void GetChannelMemberCount(string[] channels, long reqIdq)
        {
            var list = new AndroidJavaObject("java.util.ArrayList");

            for (var i = 0; i < channels.Length; i++)
            {
                var channel = channels[i];
                list.Call<bool>("add", CreateAndroidStr(channel));
            }
            rtmClient.Call("getChannelMemberCount", list, new ResultListener("getChannelMemberCount",
                (res) =>
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {

                        var javaClass = new AndroidJavaClass("java.lang.Class");
                        var arrayListClass = javaClass.CallStatic<AndroidJavaObject>("forName", "java.util.ArrayList");
                        var listAO = arrayListClass.Call<AndroidJavaObject>("cast", res);

                        var channelMembers = new List<ChannelMemberCount>();
                        for (var i = 0; i < listAO.Call<int>("size"); i++)
                        {
                            var value = listAO.Call<AndroidJavaObject>("get", i);
                            var channelId = value.Call<string>("getChannelID");
                            Debug.Log(channelId);

                            var count = value.Call<int>("getMemberCount");
                            Debug.Log(count);

                            channelMembers.Add(new ChannelMemberCount() { channelId = channelId, count = count });
                        }

                        OnChannelMemberCountReceivedCallback(-1, channelMembers);

                    });
                },
                (o) => { }
            ));
        }


        public override void GetChannelMemberCount(string[] channelIds, int channelCount, ref long reqId)
        {
            GetChannelMemberCount(channelIds, reqId);
        }

        public class AttributesMap
        {

        }
    }
#endif
} 
