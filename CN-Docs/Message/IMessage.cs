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

        /// <summary>
		/// 获取消息 ID。
		/// </summary>
		/// <returns>消息的唯一 ID，在消息对象创建时自动生成。</returns>
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

		/// <summary>
		/// 获取消息类型。
		/// </summary>
		/// <returns>消息类型。详见 #MESSAGE_TYPE 。</returns>
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

		/// <summary>
		/// 设置文本消息正文或自定义二进制消息的文字描述。
		/// @note 最大长度为 32 KB。如果消息为自定义二进制消息，请确保文字描述和二进制消息的总大小不超过 32 KB。
		/// </summary>
		/// <param name="text">待设置的消息正文。</param>
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

		/// <summary>
		/// 获取文本消息正文或自定义二进制消息的文字描述。
		/// </summary>
		/// <returns>文本消息正文或自定义二进制消息的文字描述。</returns>
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

		/// <summary>
		/// 获取自定义消息在内存中的首地址。
		/// </summary>
		/// <returns>自定义消息在内存中的首地址。</returns>
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

		/// <summary>
		/// 获取自定义消息的长度。
		/// </summary>
		/// <returns>自定义消息的长度（字节）。</returns>
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

		/// <summary>
		/// 供消息接收者获取消息服务器接收到消息的时间戳。
		/// @note 
		/// - 你不能设置时间戳，但是你可以从该时间戳推断出消息的大致发送时间。
		/// - 时间戳仅用于展示，不建议用于消息的严格排序。
		/// </summary>
		/// <returns>消息服务器接收到消息的时间戳（毫秒）。</returns>
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

		/// <summary>
		/// 供消息接收者检查消息是否在服务端被保存过（仅适用于点对点消息）。
		/// @note
		/// - 如果消息没有被消息服务器保存过，该方法将返回 false。也就是说：只有当消息发送者通过设置 \ref agora_rtm.SendMessageOptions.enableOfflineMessaging "enableOfflineMessaging=true" 发送离线消息且在发送离线消息时对端不在线，对端重新上线后调用该方法会返回 true。
		/// - 目前我们只为每个接收端保存 200 条离线消息最长七天。当保存的离线消息超出限制时，最老的信息将会被最新的消息替换。
		/// </summary>
		/// <returns>
		///  - true: 被保存过（消息服务器保存了该条消息且在对端重新上线后重新发送成功）。
		///  - false: 未被保存过。
		/// </returns>
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
        
		/// <summary>
		/// 释放当前 IMessage 实例使用的所有资源。
		/// </summary>
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