using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RtmChannelAttribute : IRtmApiNative, IDisposable {

		private IntPtr _channelAttributePtr = IntPtr.Zero;
		private MESSAGE_FLAG _flag = MESSAGE_FLAG.SEND;
		private bool _disposed = false;
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
			Dispose(false);
		}

		/// <summary>
		/// 设置频道属性的属性名。
		/// </summary>
		/// <param name="key">频道属性的属性名。必须为可见字符且长度不得超过 32 字节。</param>
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

		/// <summary>
		/// 获取频道属性的属性名。
		/// </summary>
		/// <returns>频道属性的属性名。</returns>
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

		/// <summary>
		/// 设置频道属性的属性值。
		/// </summary>
		/// <param name="value">频道属性的属性值。长度不得超过 8 KB。</param>
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

		/// <summary>
		/// 获取频道属性的属性值。
		/// </summary>
		/// <returns>频道属性的属性值。</returns>
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

		/// @cond not-for-doc
		public void SetLastUpdateUserId(string lastUpdateUserId) {
			_lastUpdateUserId = lastUpdateUserId;
		}
		/// @endcond

		/// <summary>
		/// 获取最近一次更新频道属性用户的 ID。
		/// </summary>
		/// <returns>最近一次更新频道属性用户的 ID。</returns>
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
 
        /// @cond not-for-doc
		public void SetLastUpdateTs(Int64 ts) {
			_lastUpdateTs = ts;
		}
		/// @endcond

		/// <summary>
		/// 获取频道属性最近一次更新的时间戳。
		/// </summary>
		/// <returns>频道属性最近一次更新的时间戳（毫秒）。</returns>
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

		private void Release() {
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

        /// @cond not-for-doc
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// @endcond

		/// <summary>
		/// 释放 #RtmChannelAttribute 实例使用的所有资源。
		/// </summary>
		public void Dispose(bool disposing) {
			if (_disposed) return;
			if (disposing) {}
			Release();
			_disposed = true;
		}
	}
}
