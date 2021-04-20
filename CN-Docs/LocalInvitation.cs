using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;


namespace agora_rtm {
	public sealed class LocalInvitation : IRtmApiNative, IDisposable {
		private IntPtr _localInvitationPtr = IntPtr.Zero;
		private bool _disposed = false;
		private bool _needDispose = true;
		public LocalInvitation(IntPtr localInvitationPtr, bool needRelease = true) {
			_localInvitationPtr = localInvitationPtr;
			_needDispose = needRelease;
		}

		~LocalInvitation() {
			if (_needDispose) {
				Dispose(false);
			}
		}
        
		/// <summary>
		/// 供主叫获取被叫的用户 ID。
		/// </summary>
		public string GetCalleeId() {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_remote_call_manager_getCallerId(_localInvitationPtr);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}
		}

		/// <summary>
		/// 供主叫设置呼叫邀请内容。
		/// </summary>
		/// <param name="content">邀请响应。若编码为 UTF-8， `content` 的对应的字节长度不得超过 8 KB。</param>
		public void SetContent(string content) {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return;
			}
			i_local_call_invitation_setContent(_localInvitationPtr, content);
		}

		/// <summary>
		/// 供主叫获取自己通过 #SetContent 方法设置的呼叫邀请内容
		/// </summary>
		public string GetContent() {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_local_call_invitation_getContent(_localInvitationPtr);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}	
		}

		/// <summary>
		/// 设置频道 ID。
		/// @note 与老信令 SDK 互通时你必须设置频道 ID。不过即使在被叫成功接受呼叫邀请后，Agora RTM SDK 也不会把主叫加入指定频道。
		/// </summary>
		/// <param name="channelId">频道 ID。</param>
		public void SetChannelId(string channelId) {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return;
			}
			i_local_call_invitation_setChannelId(_localInvitationPtr, channelId);
		}

		/// <summary>
		/// 获取频道 ID。
		/// </summary>
		public string GetChannelId() {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_local_call_invitation_getChannelId(_localInvitationPtr);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}
		}

		/// <summary>
		/// 供主叫获取被叫通过 \ref agora_rtm.RemoteInvitation.SetResponse "SetResponse" 方法设置的呼叫邀请响应。
		/// </summary>
		public string GetResponse() {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_local_call_invitation_getResponse(_localInvitationPtr);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}	
		}

		public IntPtr GetPtr() {
			return _localInvitationPtr;
		}

		/// <summary>
		/// 供主叫获取呼叫邀请状态。
		/// </summary>
		/// <returns>呼叫邀请状态。详见 \ref agora_rtm.LOCAL_INVITATION_STATE "LOCAL_INVITATION_STATE"。</returns>
		public LOCAL_INVITATION_STATE GetState() {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return LOCAL_INVITATION_STATE.LOCAL_INVITATION_STATE_FAILURE;
			}
			return (LOCAL_INVITATION_STATE)i_local_call_invitation_getState(_localInvitationPtr);
		}

		private void Release() {
			if (_localInvitationPtr == IntPtr.Zero) {
				Debug.LogError("_localInvitationPtr is null");
				return;
			}
			i_local_call_invitation_release(_localInvitationPtr);
			_localInvitationPtr = IntPtr.Zero;
		}

		/// <summary>
		/// 释放当前 #LocalInvitation 实例使用的所有资源。
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing) {
			if (_needDispose) {
				if (_disposed) return;
				if (disposing) {}
				Release();
			} else {
				_localInvitationPtr = IntPtr.Zero;
			}
			_disposed = true;
			Debug.Log("~LocalInvitation Dispose");
		}
	}
}