#ifndef _RTM_BASE_H_
#define _RTM_BASE_H_
#include "IAgoraService.h"
#include "IAgoraRtmService.h"
#include "IAgoraRtmCallManager.h"

#include <fstream>

//#ifdef WINDOWS
#define DllExport __declspec (dllexport)
//#else
//#define DllExport
//#endif

// wrapper logger
static std::ofstream logFile;

std::string gbk2utf8(const char* gbk)
{
	std::string str;

	if (gbk != NULL)
	{
		int len = MultiByteToWideChar(936, 0, gbk, -1, NULL, 0);
		std::wstring strW;

		strW.resize(len);

		MultiByteToWideChar(936, 0, gbk, -1, (LPWSTR)strW.data(), len);

		len = WideCharToMultiByte(CP_UTF8, 0, strW.data(), len - 1, NULL, 0, NULL, NULL);

		str.resize(len);

		WideCharToMultiByte(CP_UTF8, 0, strW.data(), -1, (LPSTR)str.data(), len, NULL, NULL);
	}

	return str;
}

std::string utf82gbk(const char* utf8)
{
	std::string str;

	if (utf8 != NULL)
	{
		int len = MultiByteToWideChar(CP_UTF8, 0, utf8, -1, NULL, 0);
		std::wstring strW;

		strW.resize(len);

		MultiByteToWideChar(CP_UTF8, 0, utf8, -1, (LPWSTR)strW.data(), len);

		len = WideCharToMultiByte(936, 0, strW.data(), len - 1, NULL, 0, NULL, NULL);

		str.resize(len);

		WideCharToMultiByte(936, 0, strW.data(), -1, (LPSTR)str.data(), len, NULL, NULL);
	}

	return str;
}

std::string gbk2utf8(const std::string& gbk)
{
	return gbk2utf8(gbk.c_str());
}


extern "C"
{
	class PeerOnlineStatus
	{
		const char* peerId;
		bool isOnline;
		int onlineState;
	};


	typedef void (*SendMessageReceivedCallback)(long long messageId, int state);
	typedef void (*MessageReceivedCallback)(const char* userId, const char* msg);
	typedef void (*PeerMessageReceivedCallback)(const char* userId, const char* msg);
	typedef void (*LoginSuccessCallback)();
	typedef void (*LoginFailureCallback)(int errorCode);
	typedef void (*JoinSuccessCallback)();
	typedef void (*JoinFailureCallback)(int errorCode);
	typedef void (*LeaveCallback)(int errorCode);
	typedef void (*GetMembersCallback)(agora::rtm::IChannelMember** members, int userCount, agora::rtm::GET_MEMBERS_ERR errorCode);
	typedef void (*GetChannelMemberCountCallback)(long long requestId, const agora::rtm::ChannelMemberCount* channelMemberCounts, int channelCount, agora::rtm::GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode);
	typedef void (*ChannelMessageReceivedCallback)(const char* userId, const char *channelId, const char* msg);
	typedef void (*QueryUserStatusCallback)(long long requestId, const agora::rtm::PeerOnlineStatus* peersStatus, int peerCount, int errorCode);
	typedef void (*SubscriptionRequestCallback)(long long requestId, const agora::rtm::PEER_SUBSCRIPTION_STATUS_ERR err);

	class RtmEventHandler : public agora::rtm::IRtmServiceEventHandler
	{
	private:
		SendMessageReceivedCallback smr;
		PeerMessageReceivedCallback pmr;
		LoginSuccessCallback lsc;
		LoginFailureCallback lfc;
		QueryUserStatusCallback qus;
		SubscriptionRequestCallback src;
		GetChannelMemberCountCallback gcmc;


	public:
		RtmEventHandler() {
			this->smr = nullptr;
			this->pmr = nullptr;
			this->lsc = nullptr;
			this->lfc = nullptr;
			this->qus = nullptr;
			this->src = nullptr;
			this->gcmc = nullptr;

		}

		RtmEventHandler(SendMessageReceivedCallback smr,
			PeerMessageReceivedCallback pmr,
			LoginSuccessCallback lsc,
			LoginFailureCallback lfc,
			QueryUserStatusCallback qus,
			SubscriptionRequestCallback src,
			GetChannelMemberCountCallback gcmc)
		{
			this->smr = smr;
			this->pmr = pmr;
			this->lsc = lsc;
			this->lfc = lfc;
			this->qus = qus;
			this->src = src;
			this->gcmc = gcmc;
		}



	protected:

		void onSendMessageResult(long long messageId, agora::rtm::PEER_MESSAGE_ERR_CODE errorCode) override
		{
			if (smr != nullptr)
				smr(messageId, errorCode);
		}

		void onMessageReceivedFromPeer(const char* peerId, const agora::rtm::IMessage* message) override
		{
			if (pmr != nullptr)
				pmr(peerId, message->getText());
		}

		void onLoginSuccess() override
		{
			if (lsc != nullptr)
				lsc();
		}

		void onLoginFailure(agora::rtm::LOGIN_ERR_CODE errorCode) override
		{
			if (lfc != nullptr)
				lfc(errorCode);
		}

		void onQueryPeersOnlineStatusResult(long long requestId, const agora::rtm::PeerOnlineStatus* peersStatus,
			int peerCount, agora::rtm::QUERY_PEERS_ONLINE_STATUS_ERR errorCode) override
		{
			if (qus != nullptr)
				qus(requestId, peersStatus, peerCount, errorCode);
		}

		void onSubscriptionRequestResult(long long requestId, const agora::rtm::PEER_SUBSCRIPTION_STATUS_ERR err) override
		{
			if (src != nullptr)
			{
				src(requestId, err);
			}
		}

		void onGetChannelMemberCountResult(long long requestId, const agora::rtm::ChannelMemberCount* channelMemberCounts, int channelCount, agora::rtm::GET_CHANNEL_MEMBER_COUNT_ERR_CODE errorCode)
		{
			if (gcmc != nullptr)
			{
				gcmc(requestId, channelMemberCounts, channelCount, errorCode);
			}
		}
	};

	class RtmChannelEventHandler : public agora::rtm::IChannelEventHandler
	{
	private:
		SendMessageReceivedCallback smr;
		JoinSuccessCallback jsc;
		JoinFailureCallback jfc;
		LeaveCallback lc;
		GetMembersCallback gmc;
		ChannelMessageReceivedCallback cmr;


	public:


		RtmChannelEventHandler()
		{
			this->smr = nullptr;
			this->jsc = nullptr;
			this->jfc = nullptr;
			this->lc = nullptr;
			this->gmc = nullptr;
			this->cmr = nullptr;
		}
		RtmChannelEventHandler(SendMessageReceivedCallback smr, JoinSuccessCallback jsc, JoinFailureCallback jfc,
			ChannelMessageReceivedCallback cmr, LeaveCallback lcb, GetMembersCallback gmc)
		{
			this->smr = smr;
			this->jsc = jsc;
			this->jfc = jfc;
			this->cmr = cmr;
			this->lc = lcb;
			this->gmc = gmc;

		}

		void onSendMessageResult(long long messageId, agora::rtm::CHANNEL_MESSAGE_ERR_CODE state) override
		{
			if (smr != nullptr)
				smr(messageId, state);
		}

		void onMessageReceived(const char* userId, const agora::rtm::IMessage* message) override
		{
			//const char *channelId = utf82gbk(m_channeId);
			std::string msg = utf82gbk(message->getText());

			if (cmr != nullptr)
				cmr(userId, "", msg.c_str());
		}

		void onJoinSuccess() override
		{
			if (jsc != nullptr)
				jsc();
		}

		void onJoinFailure(agora::rtm::JOIN_CHANNEL_ERR errorCode) override
		{
			if (jfc != nullptr)
				jfc(errorCode);
		}

		void onLeave(agora::rtm::LEAVE_CHANNEL_ERR errorCode) override
		{
			if (lc != nullptr)
				lc(errorCode);
		}

		void onGetMembers(agora::rtm::IChannelMember** members, int userCount, agora::rtm::GET_MEMBERS_ERR errorCode) override
		{
			if (gmc != nullptr)
				gmc(members, userCount, errorCode);
		}


	};

	static std::unique_ptr<agora::base::IAgoraService> agoraInst;
	static agora::base::AgoraServiceContext agoraContext;
	static std::unique_ptr<agora::rtm::IRtmServiceEventHandler> rtmEventCallback;
	std::unique_ptr<agora::rtm::IChannelEventHandler> channelEventCallback;

	static agora::rtm::IRtmService* rtmService;
	static agora::rtm::IRtmCallManager* rtmCallManager;
	//static agora::rtm::ILocalCallInvitation* localCallInvitation;
	//static agora::rtm::IRemoteCallInvitation* remoteCallInvitation;

	DllExport int initialize()
	{
		agoraInst.reset(createAgoraService());
		if (!agoraInst) {
			return -1;
		}

		if (agoraInst->initialize(agoraContext)) {
			return -2;
		}

		logFile = std::ofstream("internal_log.txt");

		return 0;
	}

	DllExport int createRtmService(const char *appId, SendMessageReceivedCallback smr,
		PeerMessageReceivedCallback pmr, LoginSuccessCallback lsc, LoginFailureCallback lfc,
		QueryUserStatusCallback qus, SubscriptionRequestCallback src,
		GetChannelMemberCountCallback gcmc)
	{
		rtmService = agoraInst->createRtmService();
		if (!rtmService) {
			return -1;
		}

		rtmEventCallback.reset(new RtmEventHandler(smr, pmr, lsc, lfc, qus, src, gcmc));

		int ret = rtmService->initialize(appId, rtmEventCallback.get());
		if (ret)
		{
			return ret;
		}
		
		ret = rtmService->setLogFile("agora_log.txt");
		if (ret)
		{
			return ret;
		}

		return 0;
	}

	DllExport int login(const char* token, const char* uid)
	{
		return rtmService->login(token ? token : "", uid);
	}

	DllExport int logout()
	{
		return rtmService->logout();
	}


	DllExport int sendMessageToPeer(const char* peerID, const char* msg, int offline) {
		agora::rtm::IMessage* rtmMessage = rtmService->createMessage();
		rtmMessage->setText(gbk2utf8(msg).c_str());
		agora::rtm::SendMessageOptions so;
		so.enableOfflineMessaging = offline != 0;
		int ret = rtmService->sendMessageToPeer(peerID, rtmMessage, so);
		rtmMessage->release();

		return ret;
	}

	DllExport agora::rtm::IChannel* createChannel(const char* channel, SendMessageReceivedCallback smr, JoinSuccessCallback jsc, JoinFailureCallback jfc, ChannelMessageReceivedCallback cmr, LeaveCallback lcb, GetMembersCallback gmc)
	{
		channelEventCallback.reset(new RtmChannelEventHandler(smr,jsc, jfc, cmr, lcb, gmc));
		return rtmService->createChannel(channel, channelEventCallback.get());
	}

	// const char * channel not needed
	DllExport int joinChannel(agora::rtm::IChannel* channelHandler, const char* channel, SendMessageReceivedCallback smr, JoinSuccessCallback jsc, JoinFailureCallback jfc, ChannelMessageReceivedCallback cmr, LeaveCallback lcb)
	{
		return channelHandler->join();
	}

	DllExport int leaveChannel(agora::rtm::IChannel* channelHandler)
	{
		return channelHandler->leave();
	}

	DllExport int sendChannelMessageWithOptions(agora::rtm::IChannel* channelHandler, char const* msg, agora::rtm::SendMessageOptions *smo)
	{
		agora::rtm::IMessage* rtmMessage = rtmService->createMessage();
		rtmMessage->setText(gbk2utf8(msg).c_str());
		int res = channelHandler->sendMessage(rtmMessage, *smo);
		rtmMessage->release();

		return res;
	}

	DllExport int sendChannelMessage(agora::rtm::IChannel* channelHandler, char const* msg)
	{
		agora::rtm::SendMessageOptions smo;
		return sendChannelMessageWithOptions(channelHandler, msg, &smo);
	}

	DllExport const char* getChannelId(agora::rtm::IChannel* channelHandler)
	{
		return channelHandler->getId();
	}

	DllExport int getChannelMembers(agora::rtm::IChannel* channelHandler)
	{
		return channelHandler->getMembers();
	}

	DllExport void releaseChannel(agora::rtm::IChannel* channelHandler)
	{
		channelHandler->release();
	}


	DllExport void release()
	{
		rtmService->release();

		logFile.close();
	}

	DllExport int queryPeersOnlineStatus(const char* peerIds[], int peerCount, long long& requestId)
	{
		return rtmService->queryPeersOnlineStatus(peerIds, peerCount, requestId);
	}


	DllExport int subscribePeersOnlineStatus(const char* peerIds[], int peerCount, long long& requestId)
	{
		return rtmService->subscribePeersOnlineStatus(peerIds, peerCount, requestId);
	}

	DllExport int unsubscribePeersOnlineStatus(const char* peerIds[], int peerCount, long long& requestId)
	{
		return rtmService->unsubscribePeersOnlineStatus(peerIds, peerCount, requestId);
	}

	DllExport int queryPeersBySubscriptionOption(int option, long long& requestId)
	{
		return rtmService->queryPeersBySubscriptionOption((agora::rtm::PEER_SUBSCRIPTION_OPTION) option, requestId);
	}

	DllExport int getChannelMemberCount(const char* channelIds[], int channelCount, long long& requestId)
	{
		return rtmService->getChannelMemberCount(channelIds, channelCount, requestId);
	}

	DllExport int setLogFile(const char* logfile)
	{
		return rtmService->setLogFile(logfile);
	}

	DllExport int setLogFilter(agora::rtm::LOG_FILTER_TYPE filter)
	{
		return rtmService->setLogFilter(filter);
	}

	DllExport int setLogFileSize(int fileSizeInKBytes)
	{
		return rtmService->setLogFileSize(fileSizeInKBytes);
	}



	//deleteChannelAttributesByKeys(const char* channelId,
	//	const char* attributeKeys[],
	//	int 	numberOfKeyOptions& options,
	//	long long& requestId
	//	)

	DllExport agora::rtm::IRtmChannelAttribute* createChannelAttribute()
	{
		return rtmService->createChannelAttribute();
	}

	DllExport int getChannelAttributes(const char* channelId, long long& requestId)
	{
		return rtmService->getChannelAttributes(channelId, requestId);
	}

	DllExport const char* getChannelAttribute_getLastUpdateUserId(agora::rtm::IRtmChannelAttribute* rca)
	{
		return rca->getLastUpdateUserId();
	}

	DllExport long long getChannelAttribute_getLastUpdateTs(agora::rtm::IRtmChannelAttribute* rca)
	{
		return rca->getLastUpdateTs();
	}

	DllExport bool ChannelAttrOptions_enableNotificationToChannelMembers(agora::rtm::ChannelAttributeOptions* cao)
	{
		return cao->enableNotificationToChannelMembers;
	}


	DllExport int getChannelAttributesByKeys(const char* channelId,
		const char* attributeKeys[], int numberOfKeys, long long& requestId
	)
	{
		return rtmService->getChannelAttributesByKeys(channelId, attributeKeys, numberOfKeys, requestId);
	}

	DllExport void setChannelAttributeKey(agora::rtm::IRtmChannelAttribute* chAttr, const char* key)
	{
		chAttr->setKey(key);
	}

	DllExport const char* getChannelAttributeKey(agora::rtm::IRtmChannelAttribute* chAttr)
	{
		return chAttr->getKey();
	}

	DllExport void setChannelAttributeValue(agora::rtm::IRtmChannelAttribute* chAttr, const char* value)
	{
		return chAttr->setValue(value);
	}

	DllExport const char* getChannelAttributeValue(agora::rtm::IRtmChannelAttribute* chAttr)
	{
		return chAttr->getValue();
	}

	DllExport const char* getChannelAttributeLastUpdateUserId(agora::rtm::IRtmChannelAttribute* chAttr)
	{
		return chAttr->getLastUpdateUserId();
	}

	DllExport long long getChannelAttributeLastUpdateTs(agora::rtm::IRtmChannelAttribute* chAttr)
	{
		return chAttr->getLastUpdateTs();
	}

	DllExport long long getMessageID(agora::rtm::IMessage* message)
	{
		return message->getMessageId();
	}
	
	DllExport agora::rtm::MESSAGE_TYPE getMessageType(agora::rtm::IMessage* message)
	{
		return message->getMessageType();
	}
	
	DllExport void setMessageText(agora::rtm::IMessage* message, const char* str)
	{
		message->setText(str);
	}

	DllExport const char* getMessageText(agora::rtm::IMessage* message)
	{
		return message->getText();
	}
	
	DllExport const char* getRawMessageData(agora::rtm::IMessage* message)
	{
		return message->getRawMessageData();
	}

	DllExport int getRawMessageLength(agora::rtm::IMessage* message)
	{
		return message->getRawMessageLength();
	}

	DllExport long long getServerReceivedTs(agora::rtm::IMessage* message)
	{
		return message->getServerReceivedTs();
	}

	DllExport bool isOfflineMessage(agora::rtm::IMessage* message)
	{
		return message->isOfflineMessage();
	}
	//


	DllExport const char* getMemberUserId(agora::rtm::IChannelMember* chanMember)
	{
		return chanMember->getUserId();
	}

	DllExport const char* getMemberChannelId(agora::rtm::IChannelMember* chanMember)
	{
		return chanMember->getChannelId();
	}

	DllExport void releaseMember(agora::rtm::IChannelMember* chanMember)
	{
		chanMember->release();
	}


	//IRtmCallManager

	/*
	DllExport int sendLocalInvitation(agora::rtm::ILocalCallInvitation invitation)
	{
		return rtmCallManager ->sendLocalInvitation(invitation);
	}

	DllExport int acceptRemoteInvitiation((agora::rtm:IRemoteCallInvitation* invitation) invitation)
	{
		return rtmCallManager ->acceptRemoteInvitiation(invitation);
	}

	DllExport int refuseRemoteInvitation((agora::rtm:IRemoteCallInvitation*) invitation)
	{
		return rtmCallManager ->refuseRemoteInvitation(invitation);
	}

	DllExport int cancelLocalInvitation((agora::rtm:ILocalCallInvitation*) invitation)
	{
		return rtmCallManager ->cancelLocalInvitation(invitation);
	}
	
	DllExport int createLocalCallInvitation(const char* calleeId)
	{
		return rtmCallManager ->createLocalCallInvitation(calleeId);
	}
	*/



	//ILocalCallInvitation
	DllExport const char* getLocalCallCalleeId(agora::rtm::ILocalCallInvitation* localCallInvitation)
	{
		return localCallInvitation->getCalleeId();
	}
	DllExport void setLocalCallContent(agora::rtm::ILocalCallInvitation* localCallInvitation, const char* content)
	{
		return localCallInvitation->setContent(content);
	}
	DllExport const char* getLocalCallContent(agora::rtm::ILocalCallInvitation* localCallInvitation)
	{
		return localCallInvitation->getContent();
	}
	DllExport void setLocalCallChannelId(agora::rtm::ILocalCallInvitation* localCallInvitation, const char* channelId)
	{
		return localCallInvitation->setChannelId(channelId);
	}
	
	DllExport const char* getLocalCallChannelId(agora::rtm::ILocalCallInvitation* localCallInvitation)
	{
		return localCallInvitation->getChannelId();
	}
	
	DllExport const char* getLocalCallResponse(agora::rtm::ILocalCallInvitation* localCallInvitation)
	{
		return localCallInvitation->getResponse();
	}
	
	DllExport int getLocalCallState(agora::rtm::ILocalCallInvitation* localCallInvitation)
	{
		return localCallInvitation->getState();
	}

	//IRemoteCall invitation

	DllExport const char* getRemoteCallCallId(agora::rtm::IRemoteCallInvitation* remoteCallInvitation)
	{
		return remoteCallInvitation->getCallerId();
	}

	DllExport const char* getRemoteCallContent(agora::rtm::IRemoteCallInvitation* remoteCallInvitation)
	{
		return remoteCallInvitation->getContent();
	}

	DllExport const char* getRemoteCallChannelId(agora::rtm::IRemoteCallInvitation* remoteCallInvitation)
	{
		return remoteCallInvitation->getChannelId();
	}

	DllExport void setRemoteCallResponse(agora::rtm::IRemoteCallInvitation* remoteCallInvitation, const char* response)
	{
		remoteCallInvitation->setResponse(response);
	}

	DllExport const char* getRemoteCallResponse(agora::rtm::IRemoteCallInvitation* remoteCallInvitation)
	{
		return remoteCallInvitation -> getResponse();
	}

	DllExport agora::rtm::REMOTE_INVITATION_STATE getRemoteCallState(agora::rtm::IRemoteCallInvitation* remoteCallInvitation)
	{
		return remoteCallInvitation-> getState();
	}

	DllExport void releaseRemoteCall(agora::rtm::IRemoteCallInvitation* remoteCallInvitation)
	{
		remoteCallInvitation->release();
	}



	DllExport int 	sendLocalInvitation(agora::rtm::IRtmCallManager* manager, agora::rtm::ILocalCallInvitation* invitation)
	{
		return manager->sendLocalInvitation(invitation);
	}
	DllExport int 	acceptRemoteInvitation(agora::rtm::IRtmCallManager* manager, agora::rtm::IRemoteCallInvitation* invitation)
	{
		return manager->acceptRemoteInvitation(invitation);
	}
	DllExport int 	refuseRemoteInvitation(agora::rtm::IRtmCallManager* manager, agora::rtm::IRemoteCallInvitation* invitation)
	{
		return manager->refuseRemoteInvitation(invitation);
	}
	DllExport int 	cancelLocalInvitation(agora::rtm::IRtmCallManager* manager, agora::rtm::ILocalCallInvitation* invitation)
	{
		return manager->cancelLocalInvitation(invitation);
	}
	DllExport agora::rtm::ILocalCallInvitation* createLocalCallInvitation(agora::rtm::IRtmCallManager* manager, const char* calleeId)
	{
		return manager->createLocalCallInvitation(calleeId);
	}
	DllExport void releaseCallManager(agora::rtm::IRtmCallManager* manager)
	{
		manager->release();
	}

	//RTM Attribute
	//key 
	//
}

class rtmBase
{
public:
	rtmBase();
	~rtmBase();
};

#endif