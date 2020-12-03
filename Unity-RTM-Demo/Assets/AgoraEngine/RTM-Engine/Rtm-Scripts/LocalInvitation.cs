using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;


namespace agora_rtm {
	public sealed class LocalInvitation : IRtmApiNative {
		private IntPtr _localInvitationPtr = IntPtr.Zero;
        
		public LocalInvitation(IntPtr localInvitationPtr) {
			_localInvitationPtr = localInvitationPtr;
		}
        
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

		public void SetContent(string content) {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return;
			}
			i_local_call_invitation_setContent(_localInvitationPtr, content);
		}

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

		public void SetChannelId(string channelId) {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return;
			}
			i_local_call_invitation_setChannelId(_localInvitationPtr, channelId);
		}

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

		public REMOTE_INVITATION_STATE GetState() {
			if (_localInvitationPtr == IntPtr.Zero)
			{
				Debug.LogError("_localInvitationPtr is null");
				return REMOTE_INVITATION_STATE.REMOTE_INVITATION_STATE_FAILURE;
			}
			return (REMOTE_INVITATION_STATE)i_local_call_invitation_getState(_localInvitationPtr);
		}
	}
}