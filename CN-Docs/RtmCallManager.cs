using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RtmCallManager : IRtmApiNative, IDisposable {
		private IntPtr _rtmCallManagerPtr = IntPtr.Zero;
		private RtmCallEventHandler _rtmCallEventHandler;
		private bool _disposed = false;

		public RtmCallManager(IntPtr rtmCallManager, RtmCallEventHandler rtmCallEventHandler) {
			_rtmCallManagerPtr = rtmCallManager;
			_rtmCallEventHandler = rtmCallEventHandler;
		}

		~RtmCallManager() {
			Dispose(false);
		}

	
		private void Release() {
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

		/// <summary>
		/// 发送呼叫邀请给被叫。
		/// </summary>
		/// <param name="invitation">一个 \ref agora_rtm.LocalInvitation "LocalInvitation" 对象。</param>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。详见 #INVITATION_API_CALL_ERR_CODE 。
		public int SendLocalInvitation(LocalInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_sendLocalInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}
		
		/// <summary>
		/// 接受来自主叫的呼叫邀请。
		/// </summary>
		/// <param name="invitation">一个 \ref agora_rtm.RemoteInvitation "RemoteInvitation" 对象。</param>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。详见 #INVITATION_API_CALL_ERR_CODE 。
		/// </returns>
		public int AcceptRemoteInvitation(RemoteInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_acceptRemoteInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}

		/// <summary>
		/// Allows the callee to decline an incoming call invitation.
		/// </summary>
		/// <param name="invitation">一个 \ref agora_rtm.RemoteInvitation "RemoteInvitation" 对象。</param>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。详见 #INVITATION_API_CALL_ERR_CODE 。
		/// </returns>
		public int RefuseRemoteInvitation(RemoteInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_refuseRemoteInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}

		/// <summary>
		/// 取消给被叫的呼叫邀请。
		/// </summary>
		/// <param name="invitation">一个 \ref agora_rtm.LocalInvitation "LocalInvitation" 对象。</param>
		/// <returns>
	    ///  - 0: 方法调用成功。
        ///  - ≠0: 方法调用失败。详见 #INVITATION_API_CALL_ERR_CODE 。
		/// </returns>
		public int CancelLocalInvitation(LocalInvitation invitation) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return rtm_call_manager_cancelLocalInvitation(_rtmCallManagerPtr, invitation.GetPtr());
		}

		/// <summary>
		/// 创建一个呼叫邀请实例。
		/// </summary>
		/// <param name="calleeId">	被叫的用户 ID。</param>
		/// <returns>
		/// 一个 \ref agora_rtm.LocalInvitation "LocalInvitation" 对象。
		/// </returns>
		public LocalInvitation CreateLocalCallInvitation(string calleeId) {
			if (_rtmCallManagerPtr == IntPtr.Zero) 
			{
				Debug.LogError("_rtmCallManagerPtr is null");
				return null;
			}
			return new LocalInvitation(rtm_call_manager_createLocalCallInvitation(_rtmCallManagerPtr, calleeId));
		}
        

        /// <summary>
		/// 释放 #RtmCallManager 实例使用的所有资源。
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing) {
			if (_disposed) return;
			if (disposing) {}
			Release();
			_disposed = true;
		}
	}
}
