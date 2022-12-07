using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RemoteInvitation : IRtmApiNative {
		private IntPtr _remoteInvitationPrt = IntPtr.Zero;

		public RemoteInvitation(IntPtr remoteInvitationPtr) {
			_remoteInvitationPrt = remoteInvitationPtr;
		}

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

		public void SetResponse(string response) {
			if (_remoteInvitationPrt == IntPtr.Zero)
			{
				Debug.LogError("_remoteInvitationPrt is null");
				return;
			}
			i_remote_call_manager_setResponse(_remoteInvitationPrt, response);	
		}

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

		public void Release() {
			if (_remoteInvitationPrt == IntPtr.Zero)
				return;

			i_remote_call_manager_release(_remoteInvitationPrt);
		}
	}
}
