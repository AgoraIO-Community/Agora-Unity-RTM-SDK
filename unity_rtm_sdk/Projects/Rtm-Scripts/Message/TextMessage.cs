using UnityEngine;
using System;

namespace agora_rtm {
	public sealed class TextMessage : IMessage {

		public TextMessage(TextMessage textMessage, MESSAGE_FLAG flag) {
			_MessageFlag = flag;
			_MessageId = textMessage.GetMessageId();
			_MessageType = textMessage.GetMessageType();
			_MessageText = textMessage.GetText();
			_IsOfflineMessage = textMessage.IsOfflineMessage();
			_Ts = textMessage.GetServerReceiveTs();
			_RawMessageData = textMessage.GetRawMessageData();
			_Length = textMessage.GetRawMessageLength();
		}

		public TextMessage(IntPtr MessagePtr) {
			_MessagePtr = MessagePtr;
		}

		public TextMessage(IntPtr MessagePtr, MESSAGE_FLAG flag) {
			_MessagePtr = MessagePtr;
			_MessageFlag = flag;
		}

		~TextMessage() {
			Debug.Log("~TextMessage  called");
			Release();
		}
	}
}