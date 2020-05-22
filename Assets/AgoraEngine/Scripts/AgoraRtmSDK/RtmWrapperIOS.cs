using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace io.agora.rtm
{
#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    public class RtmWrapperIOS : IRtmWrapper
    {
        void HandleOnMemberLeft(IntPtr channel, IntPtr member)
        {
        }

        public RtmChannelIOS curChannel;

        public class RtmChannelIOS : IRtmChannel
        {

            private IntPtr chHandler;
            private string channelId;
            public Action<object> successCallback;

            [DllImport("__Internal")]
            public static extern void joinWithCompletion(IntPtr ptr, RtmWrapperDll.OnCompletion onDone);


            [DllImport("__Internal")]
            public static extern void leaveWithCompletion(IntPtr ptr, RtmWrapperDll.OnCompletion onDone);


            [DllImport("__Internal")]
            public static extern void sendChannelMessage(IntPtr ptr, string msg, RtmWrapperDll.OnCompletion onDone);



            [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
            public static void onJoinCompleted(int code)
            {
                ((RtmWrapperIOS)RtmWrapper.Instance).curChannel.successCallback(code);
            }

            [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
            public static void onLeaveChannel(int code)
            {
                Debug.Log("unity on leave channel");
            }


            [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
            public static void onSendChannelMessage(int code)
            {
                Debug.Log("unity on send channel msg: " + code);
            }


            public RtmChannelIOS(string id, IntPtr chHandler, Action<object> successCallback)
            {
                this.channelId = id;
                this.chHandler = chHandler;
                this.successCallback = successCallback;
            }

            public string GetId()
            {
                return channelId;
            }

            public void GetMembers()
            {
                throw new NotImplementedException();
            }

            public void Join()
            {
                joinWithCompletion(chHandler, onJoinCompleted);
            }

            public void Leave()
            {
                leaveWithCompletion(chHandler, onLeaveChannel);
            }

            public void Release()
            {

            }

            public void SendMessage(string msg)
            {
                sendChannelMessage(chHandler, msg, onSendChannelMessage);
            }

            public void SendMessage(string msg, SendMessageOptions smo)
            {
                //TODO: ADD MESSAGE OPTIONS
                sendChannelMessage(chHandler, msg, onSendChannelMessage);
            }
        }

        public override void Release()
        {
        }


        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
        public static void onMemberJoined(IntPtr channel, string uid)
        {
            var chId = ((RtmWrapperIOS)RtmWrapper.Instance).curChannel.GetId();
            Debug.Log(uid + " joined " + chId);
            RtmWrapper.Instance.OnMemberChangedCallback(uid, chId, true);
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
        public static void onMemberLeft(IntPtr channel, string uid)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                var chId = ((RtmWrapperIOS)RtmWrapper.Instance).curChannel.GetId();
                Debug.Log(uid + " left " + chId);
                RtmWrapper.Instance.OnMemberChangedCallback(uid, chId, false);
            });
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
        public static void onChannelMessageReceived(IntPtr channel, string msg, string uid)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log(uid + " sent a message: " + msg);
                RtmWrapper.Instance.OnMessageReceivedCallback(uid, msg);
            });
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
        public static void onMemberCount(IntPtr channel, int count)
        {

        }

        protected IRtmChannel CreateAndJoinChannel(string channelName, Action<object> successCallback)
        {
            var chHandle = RtmWrapperDll.createChannelWithId(channelName,
                                                               onMemberJoined, onMemberLeft, onChannelMessageReceived, onMemberCount);

            var channel = new RtmChannelIOS(channelName, chHandle, successCallback);
            curChannel = channel;
            channel.Join();

            return channel;
        }

        private void GetChannelMembers()
        {
        }


        public void SubscribePeersOnlineStatus(string[] peerIds)
        {
        }

        public void UnsubscribePeersOnlineStatus(string[] peerIds)
        {
        }

        public void RenewToken(string token, Action<string> onTokenRenewal)
        {
        }

        private void SetLocalUserAttributes()
        {
        }

        private void addOrUpdateLocalUserAttributes()
        {
        }


        private void deleteLocalUserAttributes()
        {
        }

        private void clearLocalUserAttributes()
        {
        }

        private void getUserAttributes(string userId)
        {
        }

        private void getUserAttributesByKeys(string userId, List<string> keys)
        {
        }


        public override void Initialize()
        {

            Debug.Log(RtmWrapperDll.getSDKVersion());

        }


        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
        public static void onSendPeerMessage(int code)
        {
            Debug.Log("unity on send peer msg: " + code);
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnCompletion))]
        public static void onLogin(int errorCode)
        {
            if (errorCode == 0)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    RtmWrapper.Instance.LoggedIn = true;
                    RtmWrapper.Instance.OnLoginSuccessCallback();
                });
            }
            else if (errorCode == 5)
                Debug.Log("Invalid Token!");
            else if (errorCode == 6)
                Debug.Log("Token Expired!");
            else
                Debug.Log("Error logging in: " + errorCode);

        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnConnectionStateChanged))]
        public static void onConnectionStateChanged(IntPtr kit, IntPtr state, IntPtr reason)
        {
            Debug.Log("On connection state changed: ");
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnMessageReceived))]
        public static void onMessageReceived(IntPtr kit, string msg, string peerId)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log(peerId + " sent a message: " + msg);
                RtmWrapper.Instance.OnMessageReceivedCallback(peerId, msg);
            });
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnPeersOnlineStatusChanged))]
        public static void onPeersOnlineStatusChanged(IntPtr kit, int[] status)
        {
            Debug.Log("OnPeersOnlineStatusChanged");
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnRtmKitTokenDidExpire))]
        public static void onRtmKitTokenDidExpire(IntPtr kit)
        {
            Debug.Log("OnRtmKitTokenDidExpire");
        }

        protected override void CreateRtmServiceAndLogin(string appId, string token, string username)
        {
            if (LoggedIn)
                return;

            RtmWrapperDll.init(appId, onConnectionStateChanged, onMessageReceived, onPeersOnlineStatusChanged, onRtmKitTokenDidExpire);
            Debug.Log(RtmWrapperDll.getSDKVersion());
            Debug.Log("token: " + token + " || username: " + username);
            RtmWrapperDll.loginByToken(token, username, onLogin);
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
            channel.SendMessage(msg);
        }

        public override void SendChannelMessageWithOptions(IRtmChannel channel, string channelName, string msg, IRtmWrapper.SendMessageOptions smo)
        {
            channel.SendMessage(msg);
        }

        public override void SendPeerMessage(string peerId, string msg, bool enableOffline)
        {
            RtmWrapperDll.sendMessageToPeer(msg, peerId, onSendPeerMessage);
        }

        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnQueryStatus))]
        public static void QueryPeersOnlineStatusCallback(IntPtr peerOnlineStatus)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                var pos = (PeerOnlineStatus)Marshal.PtrToStructure(peerOnlineStatus, typeof(PeerOnlineStatus));
                Debug.Log("peer: " + pos.peerId + " -- " + pos.isOnline);
                Debug.Log("peer: " + (int)pos.onlineState);
                RtmWrapper.Instance.OnQueryStatusReceivedCallback(0, pos, 1, 0);
            });
        }

        public override void QueryPeersOnlineStatus(string peerIds, ref long requestId)
        {
            var fPeerIds = peerIds.Split(' ');
            RtmWrapperDll.queryPeersOnlineStatus(fPeerIds, fPeerIds.Length, QueryPeersOnlineStatusCallback);
        }

        public int count;
        private void GetChannelMemberCount(string[] channels, long reqIdq)
        {

        }


        [MonoPInvokeCallback(typeof(RtmWrapperDll.OnChannelMemberCount))]
        public static void ChannelMemberCountCallback(IntPtr channelMemberCounts, int count)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log("channel member count callback: " + count);
                if (count > 0)
                {

                    var cmc = new ChannelMemberCount[count];
                    int typeSize = Marshal.SizeOf(typeof(ChannelMemberCount));
                    Debug.Log("before for");
                    for (int i = 0; i < count; i++)
                    {
                        cmc[i] = (ChannelMemberCount)Marshal.PtrToStructure(new IntPtr(channelMemberCounts.ToInt64() + (i * typeSize)), typeof(ChannelMemberCount));
                        Debug.Log("after marshal");
                    }

                    RtmWrapper.Instance.OnChannelMemberCountReceivedCallback(0, cmc.ToList());
                    RtmWrapperDll.freeChannelMemberCount(channelMemberCounts, cmc.Length);
                }
                else
                    Debug.Log("Query Peers: No results!");
            });
        }

        public override void GetChannelMemberCount(string[] channelIds, int channelCount, ref long reqId)
        {
            RtmWrapperDll.getChannelMemberCount(channelIds, channelIds.Length, ChannelMemberCountCallback);
        }



        public class RtmMessage : IRtmMessage
        {
            private IntPtr messageHandle;


            [DllImport("__Internal")]
            public static extern int getMessageTypeDll(IntPtr message);

            [DllImport("__Internal")]
            public static extern void setMessageText(IntPtr message, string text);
            [DllImport("__Internal")]
            public static extern IntPtr getMessageText(IntPtr message);

            [DllImport("__Internal")]
            public static extern Int64 getServerReceivedTsDll(IntPtr message);

            [DllImport("__Internal")]
            public static extern bool isOfflineMessageDll(IntPtr message);

            [DllImport("__Internal")]
            public static extern void setRawData(IntPtr message, string text);

            [DllImport("__Internal")]
            public static extern IntPtr getRawData(IntPtr message);


            public RtmMessage(IntPtr message)
            {
                this.messageHandle = message;
            }


            public int GetMsgType()
            {
                return getMessageTypeDll(messageHandle);
            }

            public void SetText(string text)
            {
                setMessageText(messageHandle, text);
            }

            public string GetText()
            {
                return Marshal.PtrToStringAnsi(getMessageText(messageHandle));
            }

            public Int64 GetServerReceivedTs()
            {
                return getServerReceivedTsDll(messageHandle);
            }

            public bool IsOfflineMessage()
            {
                return isOfflineMessageDll(messageHandle);
            }

            public void SetRawData(string text)
            {
                setRawData(messageHandle, text);
            }

            public string GetRawData()
            {
                return Marshal.PtrToStringAnsi(getRawData(messageHandle));
            }
        }


        public class AgoraRtmCallKitIOS : IRtmCallKit
        {
            private IntPtr callHandle = IntPtr.Zero;
            public IntPtr GetHandle() { return callHandle; }

            [DllImport("__Internal")]
            public static extern void sendLocalInvitation(IntPtr callKit, IntPtr call, RtmWrapperDll.OnCompletion callback);
            [DllImport("__Internal")]
            public static extern void cancelLocalInvitation(IntPtr callKit, IntPtr call, RtmWrapperDll.OnCompletion callback);
            [DllImport("__Internal")]
            public static extern void acceptRemoteInvitation(IntPtr callKit, IntPtr call, RtmWrapperDll.OnCompletion callback);
            [DllImport("__Internal")]
            public static extern void refuseRemoteInvitation(IntPtr callKit, IntPtr call, RtmWrapperDll.OnCompletion callback);


            public void SendLocalInvitation(IRtmLocalCallInvitation call, RtmWrapperDll.OnCompletion callback)
            {
                sendLocalInvitation(callHandle, call.GetHandle(), callback);
            }

            public void CancelLocalInvitation(IRtmLocalCallInvitation call, RtmWrapperDll.OnCompletion callback)
            {
                cancelLocalInvitation(callHandle, call.GetHandle(), callback);
            }

            public void AcceptRemoteInvitation(IRtmRemoteCallInvitation call, RtmWrapperDll.OnCompletion callback)
            {
                acceptRemoteInvitation(callHandle, call.GetHandle(), callback);
            }

            public void RefuseRemoteInvitation(IRtmRemoteCallInvitation call, RtmWrapperDll.OnCompletion callback)
            {
                refuseRemoteInvitation(callHandle, call.GetHandle(), callback);
            }
        }


        public class AgoraRtmLocalCallInvitationIOS : IRtmLocalCallInvitation
        {
            private IntPtr callHandle = IntPtr.Zero;
            public IntPtr GetHandle() { return callHandle; }

            public AgoraRtmLocalCallInvitationIOS(IntPtr callHandle)
            {
                this.callHandle = callHandle;
            }

            [DllImport("__Internal")]
            public static extern string getLocalCallCalleeId(IntPtr call);
            [DllImport("__Internal")]
            public static extern void setLocalCallContent(IntPtr call, string content);
            [DllImport("__Internal")]
            public static extern string getLocalCallContent(IntPtr call);
            [DllImport("__Internal")]
            public static extern void setLocalCallChannelId(IntPtr call, string channelId);
            [DllImport("__Internal")]
            public static extern string getLocalCallChannelId(IntPtr call);
            [DllImport("__Internal")]
            public static extern string getLocalCallResponse(IntPtr call);
            [DllImport("__Internal")]
            public static extern int getLocalCallState(IntPtr call);

            public string GetCalleeId()
            {
                return getLocalCallCalleeId(callHandle);
            }

            public string GetChannelId()
            {
                return getLocalCallChannelId(callHandle);
            }

            public string GetContent()
            {
                return getLocalCallContent(callHandle);
            }

            public string GetResponse()
            {
                return getLocalCallResponse(callHandle);
            }

            public int GetState()
            {
                return getLocalCallState(callHandle);
            }

            public void SetChannelId(string channelId)
            {
                setLocalCallChannelId(callHandle, channelId);
            }

            public void SetContent(string content)
            {
                setLocalCallContent(callHandle, content);
            }
        }

        public class AgoraRtmRemoteCallInvitationIOS : IRtmRemoteCallInvitation
        {
            private IntPtr callHandle;
            public IntPtr GetHandle() { return callHandle; }

            public AgoraRtmRemoteCallInvitationIOS(IntPtr callHandle)
            {
                this.callHandle = callHandle;
            }

            [DllImport("__Internal")]
            public static extern string getRemoteCallCallerId(IntPtr call);
            [DllImport("__Internal")]
            public static extern string getRemoteCallContent(IntPtr call);
            [DllImport("__Internal")]
            public static extern void setRemoteCallResponse(IntPtr call, string response);
            [DllImport("__Internal")]
            public static extern string getRemoteCallResponse(IntPtr call);
            [DllImport("__Internal")]
            public static extern string getRemoteCallChannelId(IntPtr call);
            [DllImport("__Internal")]
            public static extern int getRemoteCallState(IntPtr call);

            public string GetCallerId()
            {
                return getRemoteCallCallerId(callHandle);
            }

            public string GetChannelId()
            {
                return getRemoteCallChannelId(callHandle);
            }

            public string GetContent()
            {
                return getRemoteCallContent(callHandle);
            }

            public string GetResponse()
            {
                return getRemoteCallResponse(callHandle);
            }

            public int GetState()
            {
                return getRemoteCallState(callHandle);
            }


            public void SetResponse(string content)
            {
                setRemoteCallResponse(callHandle, content);
            }
        }

    }

    public class RtmWrapperDll
    {

        public delegate void OnCompletion(int errorCode);
        public delegate void OnQueryStatus(IntPtr peerStatus);
        public delegate void OnChannelMemberCount(IntPtr channelMemberCounts, int errorCode);
        public delegate void OnMemberJoined(IntPtr channel, string userId);
        public delegate void OnMemberLeft(IntPtr channel, string userId);
        public delegate void OnChannelMessageReceived(IntPtr channel, string message, string userId);
        public delegate void OnMemberCount(IntPtr channel, int count);
        public delegate void OnQueryPeersBySubscriptionOption([MarshalAs(UnmanagedType.LPStr)] string[] peerIds, int errorCode);
        public delegate void OnRenewToken(string token, int errorCode);
        public delegate void OnConnectionStateChanged(IntPtr kit, IntPtr state, IntPtr reason);
        public delegate void OnMessageReceived(IntPtr kit, string msg, string peerId);
        public delegate void OnPeersOnlineStatusChanged(IntPtr kit, int[] status);
        public delegate void OnRtmKitTokenDidExpire(IntPtr kit);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void init(string appId,
                                       OnConnectionStateChanged connectionStateChanged,
                                       OnMessageReceived onMessageReceived,
                                      OnPeersOnlineStatusChanged onPeersOnlineStatusChanged,
                                      OnRtmKitTokenDidExpire onTokenExpired);


#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void loginByToken(string token, string userId, OnCompletion onDone);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void logout(OnCompletion onDone);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void sendMessageToPeer(string msg, string userId, OnCompletion onDone);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void sendMessageToPeerWithOptions(string msg, string userId, SendMessageOptions options, OnCompletion callback);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void renewToken(string token, OnRenewToken callback);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void queryPeersOnlineStatus(string[] users, int count, OnQueryStatus onDone);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void getChannelMemberCount(string[] channelIds, int count, OnChannelMemberCount onDone);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern IntPtr createChannelWithId(string channlID, OnMemberJoined omj, OnMemberLeft oml, OnChannelMessageReceived omr, OnMemberCount omc);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void destroyChannelWithId(string channelId);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void freeQueryPeersOnlineStatus(PeerOnlineStatus[] users, int count);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern void freeChannelMemberCount(IntPtr data, int count);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern IntPtr getRtmCallKit();

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern string subscribePeersOnlineStatus(string[] peerIds, int count, OnCompletion callback);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern string unsubscribePeersOnlineStatus(string[] peerIds, int count, OnCompletion callback);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern string setParameters(string parameters);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern string setLogFile(string logFile);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern string setLogFileSize(int fileSize);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern string setLogFileFilters(int filter);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
        [DllImport("AgoraRtmBundle")]
#endif
        public static extern string getSDKVersion();
    }
#endif

}