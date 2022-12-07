using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RtmChannelAttribute : IRtmApiNative {

		private IntPtr _channelAttributePtr = IntPtr.Zero;
		private MESSAGE_FLAG _flag = MESSAGE_FLAG.SEND;
		private string _key {
			get;
			set;
		}

		private string _value {
			get;
			set;
		}

		private Int64 _lastUpdateTs {
			get;
			set;
		}

		private string _lastUpdateUserId {
			get;
			set;
		}

		public RtmChannelAttribute(MESSAGE_FLAG flag) {
			_flag = flag;
		}

		public RtmChannelAttribute(IntPtr channelAttributePtr) {
			_channelAttributePtr = channelAttributePtr;
		}

		public RtmChannelAttribute(RtmChannelAttribute channelAttribute, MESSAGE_FLAG flag) {
			_flag = flag;
			_key = channelAttribute.GetKey();
			_value = channelAttribute.GetValue();
			_lastUpdateTs = channelAttribute.GetLastUpdateTs();
			_lastUpdateUserId = channelAttribute.GetLastUpdateUserId();
		}

		~RtmChannelAttribute() {
			Debug.Log("~RtmChannelAttribute");
		}

		public void SetKey(string key) {
			if (_flag == MESSAGE_FLAG.RECEIVE) {
				_key = key;
				return;
			}

			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return;
			}
			channelAttribute_setKey(_channelAttributePtr, key);
		}

		public string GetKey() {
			if (_flag == MESSAGE_FLAG.RECEIVE) {
				return _key;
			}

			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr keyPtr = channelAttribute_getKey(_channelAttributePtr);
            if (!ReferenceEquals(keyPtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(keyPtr);
			} else {
				return "";
			}
		}

		public void SetValue(string value) {
			if (_flag == MESSAGE_FLAG.RECEIVE) {
				_value = value;
				return;
			}

			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return;
			}
			channelAttribute_setValue(_channelAttributePtr, value);
		}

		public string GetValue() {
			if (_flag == MESSAGE_FLAG.RECEIVE) {
				return _value;
			}

			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = channelAttribute_getValue(_channelAttributePtr);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}
		}

		public void SetLastUpdateUserId(string lastUpdateUserId) {
			_lastUpdateUserId = lastUpdateUserId;
		}

		public string GetLastUpdateUserId() {
			if (_flag == MESSAGE_FLAG.RECEIVE) {
				return _lastUpdateUserId;
			}

			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr userIdPtr = channelAttribute_getLastUpdateUserId(_channelAttributePtr);
            if (!ReferenceEquals(userIdPtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(userIdPtr);
			} else {
				return "";
			}
		}

		public void SetLastUpdateTs(Int64 ts) {
			_lastUpdateTs = ts;
		}

		public Int64 GetLastUpdateTs() {
			if (_flag == MESSAGE_FLAG.RECEIVE) {
				return _lastUpdateTs;
			}

			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			Int64 ts = channelAttribute_getLastUpdateTs(_channelAttributePtr);
			return ts;
		}

		public IntPtr GetPtr() {
			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return IntPtr.Zero;
			}
			return _channelAttributePtr;
		}

		public void Release() {
			if (_flag == MESSAGE_FLAG.RECEIVE)
				return;

			if (_channelAttributePtr == IntPtr.Zero)
			{
				Debug.LogError("_channelAttributePtr is null");
				return;
			}
			channelAttribute_release(_channelAttributePtr);
			_channelAttributePtr = IntPtr.Zero;
		}
	}
}
