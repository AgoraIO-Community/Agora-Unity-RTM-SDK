using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
    public abstract class IMessage : IRtmApiNative {

		protected MESSAGE_FLAG _MessageFlag = MESSAGE_FLAG.RECEIVE;

		protected IntPtr _MessagePtr = IntPtr.Zero;
		protected Int64 _MessageId = 0;

		protected MESSAGE_TYPE _MessageType = MESSAGE_TYPE.MESSAGE_TYPE_UNDEFINED;

		protected string _MessageText = "";

		protected bool _IsOfflineMessage = false;

		protected Int64 _Ts = 0;

		protected byte[] _RawMessageData = null;

		public int _Length = 0;

		public Int64 GetMessageId() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE) 
				return _MessageId;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null ptr");
				return _MessageId;
			}
			return imessage_getMessageId(_MessagePtr);
		}

		public MESSAGE_TYPE GetMessageType() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _MessageType;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null ptr");
				return _MessageType;
			}
			return (MESSAGE_TYPE)imessage_getMessageType(_MessagePtr);
		}

		public void SetText(string text) {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
			{
				_MessageText = text;
				return;
			}

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
			imessage_setText(_MessagePtr, text);
		}

		public string GetText() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _MessageText;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return _MessageText;
			}
			IntPtr textPtr = imessage_getText(_MessagePtr);
			if (!ReferenceEquals(textPtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(imessage_getText(_MessagePtr));
			} else {
				return "";
			}
		}

		public byte[] GetRawMessageData() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _RawMessageData;

			if (_MessagePtr == IntPtr.Zero)
			{
				return _RawMessageData;
			}
			_RawMessageData = new byte[GetRawMessageLength()];
			IntPtr _RawMessagePtr = imessage_getRawMessageData(_MessagePtr);
            Marshal.Copy(_RawMessagePtr, _RawMessageData, 0, GetRawMessageLength());
			return _RawMessageData;
		}

		public int GetRawMessageLength() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Length;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return _Length;
			}
			return imessage_getRawMessageLength(_MessagePtr);
		}

		public Int64 GetServerReceiveTs() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Ts;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return imessage_getServerReceivedTs(_MessagePtr);
		}

		public bool IsOfflineMessage() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _IsOfflineMessage;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return false;
			}
			return imessage_isOfflineMessage(_MessagePtr);
		}

		protected void Release() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return;

			if (_MessagePtr == IntPtr.Zero)
				return;

			imessage_release(_MessagePtr);
			_MessagePtr = IntPtr.Zero;
		}

		public IntPtr GetPtr() {
			return _MessagePtr;
		}

		public void SetMessagePtr (IntPtr messagePtr) {
			_MessagePtr = messagePtr;
		}
    }
}