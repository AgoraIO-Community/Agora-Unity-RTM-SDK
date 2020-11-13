using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RtmCallManager : IRtmApiNative {
		private IntPtr _rtmCallManagerPtr = IntPtr.Zero;
		private RtmCallEventHandler _rtmCallEventHandler;

		public RtmCallManager(IntPtr rtmCallManager, RtmCallEventHandler rtmCallEventHandler) {
			_rtmCallManagerPtr = rtmCallManager;
			_rtmCallEventHandler = rtmCallEventHandler;
		}

		public void Release() {
			if (_rtmCallManagerPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return;
			}
			rtm_call_manager_release(_rtmCallManagerPtr);
			_rtmCallManagerPtr = IntPtr.Zero;
			if (_rtmCallEventHandler != null) {
				_rtmCallEventHandler.Release();
			}
		}

		public int SendLocalInvitation(LocalInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_sendLocalInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}
		
		public int AcceptRemoteInvitation(RemoteInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_acceptRemoteInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}

		public int RefuseRemoteInvitation(RemoteInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_refuseRemoteInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}

		public int CancelLocalInvitation(RemoteInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_cancelLocalInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}

		public LocalInvitation CreateLocalCallInvitation(string calleeId) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return null;
			}
			return new LocalInvitation(rtm_call_manager_createLocalCallInvitation(_rtmCallManagerPtr, calleeId));
		}
	}
}
