using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RtmChannel :  IRtmApiNative{
		private IntPtr _rtmChannelPtr = IntPtr.Zero;
		private RtmChannelEventHandler _channelEventHandler;
		
		public RtmChannel(IntPtr rtmChannelPtr, RtmChannelEventHandler rtmChannelEventHandler) {
			_rtmChannelPtr = rtmChannelPtr;
			_channelEventHandler = rtmChannelEventHandler;
		}

		~RtmChannel() {
			Debug.Log("~ RtmChannel");
		}

		public int Join() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_join(_rtmChannelPtr);
		}

		public int Leave() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_leave(_rtmChannelPtr);
		}

		public int SendMessage(IMessage message) {
			if (_rtmChannelPtr == IntPtr.Zero || message.GetPtr() == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_sendMessage(_rtmChannelPtr, message.GetPtr());
		}

		public int SendMessage(IMessage message, bool enableOfflineMessaging, bool enableHistoricalMessaging)
		{
			if (_rtmChannelPtr == IntPtr.Zero || message.GetPtr() == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_sendMessage2(_rtmChannelPtr, message.GetPtr(), enableOfflineMessaging, enableHistoricalMessaging);
		}

		public int GetId() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_getId(_rtmChannelPtr);
		}

		public int GetMembers() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_getMembers(_rtmChannelPtr);
		}

		public void Release() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				// Debug.LogError("_rtmChannelPtr is null");
				return;
			}
			channel_release(_rtmChannelPtr);
			_rtmChannelPtr = IntPtr.Zero;
			if (_channelEventHandler != null) {
				_channelEventHandler.Release();
			}
		}
	}
}
