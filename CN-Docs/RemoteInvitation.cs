using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RemoteInvitation : IRtmApiNative, IDisposable {
		private IntPtr _remoteInvitationPrt = IntPtr.Zero;
		private bool _needDispose = true;
		private bool _disposed = false;
		public RemoteInvitation(IntPtr remoteInvitationPtr, bool needDispose = true) {
			_remoteInvitationPrt = remoteInvitationPtr;
			_needDispose = needDispose;
		}

		~RemoteInvitation() {
			if (_needDispose) {
				Dispose(false);
			}
		}

		/// <summary>
		/// 供被叫获取主叫的用户 ID。
		/// </summary>
		/// <returns>
		/// CallId
		/// </returns>
		public string GetCallerId() {
			if (_remoteInvitationPrt == IntPtr.Zero)
			{
				Debug.LogError("_remoteInvitationPrt is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_remote_call_manager_getCallerId(_remoteInvitationPrt);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}	
		}

		/// <summary>
		/// 供被叫获取主叫通过 \ref agora_rtm.LocalInvitation.SetContent "SetContent" 方法设置的呼叫邀请内容。
		/// </summary>
		/// <returns>The content.</returns>
		public string GetContent() {
			if (_remoteInvitationPrt == IntPtr.Zero)
			{
				Debug.LogError("_remoteInvitationPrt is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_remote_call_manager_getContent(_remoteInvitationPrt);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}	
		}

		/// <summary>
		/// 供被叫设置呼叫邀请响应。
		/// </summary>
		/// <param name="response">邀请响应。若编码为 UTF-8， `response` 的对应的字节长度不得超过 8 KB。</param>
		public void SetResponse(string response) {
			if (_remoteInvitationPrt == IntPtr.Zero)
			{
				Debug.LogError("_remoteInvitationPrt is null");
				return;
			}
			i_remote_call_manager_setResponse(_remoteInvitationPrt, response);	
		}

		/// <summary>
		/// 供被叫获取自己通过 #SetResponse 方法设置的呼叫邀请响应。
		/// </summary>
		public string GetResponse() {
			if (_remoteInvitationPrt == IntPtr.Zero)
			{
				Debug.LogError("_remoteInvitationPrt is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_remote_call_manager_getResponse(_remoteInvitationPrt);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}		
		}

		/// <summary>
		/// 获得频道 ID。
		/// </summary>
		public string GetChannelId() {
			if (_remoteInvitationPrt == IntPtr.Zero)
			{
				Debug.LogError("_remoteInvitationPrt is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
			IntPtr valuePtr = i_remote_call_manager_getChannelId(_remoteInvitationPrt);
            if (!ReferenceEquals(valuePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(valuePtr);
			} else {
				return "";
			}		
		}

		/// <summary>
		/// 供被叫获取呼叫邀请状态。
		/// </summary>
		/// <returns>呼叫邀请状态。详见 \ref agora_rtm.REMOTE_INVITATION_STATE "REMOTE_INVITATION_STATE"。</returns>
		public REMOTE_INVITATION_STATE GetState() {
			if (_remoteInvitationPrt == IntPtr.Zero)
			{
				Debug.LogError("_remoteInvitationPrt is null");
				return REMOTE_INVITATION_STATE.REMOTE_INVITATION_STATE_FAILURE;
			}
			return (REMOTE_INVITATION_STATE)i_remote_call_manager_getState(_remoteInvitationPrt);		
		}

		public IntPtr GetPtr() {
			return _remoteInvitationPrt;
		}


		private void Release() {
			if (_remoteInvitationPrt == IntPtr.Zero)
				return;

			i_remote_call_manager_release(_remoteInvitationPrt);
			_remoteInvitationPrt = IntPtr.Zero;
		}

		/// <summary>
		/// 释放当前 #RemoteInvitation 实例使用的所有资源。
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
				_remoteInvitationPrt = IntPtr.Zero;
			}
			_disposed = true;
		}
	}
}
