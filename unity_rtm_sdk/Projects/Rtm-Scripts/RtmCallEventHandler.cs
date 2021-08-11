using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using AOT;


namespace agora_rtm {
	public sealed class RtmCallEventHandler {
        private static int id = 0;
        private static Dictionary<int, RtmCallEventHandler> _rtmCallEventHandlerDic = new Dictionary<int, RtmCallEventHandler>();
        private IntPtr _rtmCallEventHandlerPtr = IntPtr.Zero;
        private int _currentIdIndex = 0;

		/// <summary>
		/// Callback to the caller: occurs when the callee receives the call invitation.
		/// </summary>
		/// <param name="localInvitation">An LocalCallInvitation object.</param>
		public delegate void OnLocalInvitationReceivedByPeerHandler(LocalInvitation localInvitation);
		
		/// <summary>
		/// Callback to the caller: occurs when the caller cancels a call invitation.
		/// </summary>
		/// <param name="localInvitation">An LocalCallInvitation object.</param>
		public delegate void OnLocalInvitationCanceledHandler(LocalInvitation localInvitation);
		
		/// <summary>
		/// Callback to the caller: occurs when the life cycle of the outgoing call invitation ends in failure.
		/// </summary>
		/// <param name="localInvitation">An LocalCallInvitation object.</param>
		/// <param name="errorCode">The error code. </param>
		public delegate void OnLocalInvitationFailureHandler(LocalInvitation localInvitation, LOCAL_INVITATION_ERR_CODE errorCode);
		
		/// <summary>
		/// Callback to the caller: occurs when the callee accepts the call invitation.
		/// </summary>
		/// <param name="localInvitation">An LocalCallInvitation object.</param>
		/// <param name="response">	The callee's response to the call invitation.</param>
		public delegate void OnLocalInvitationAcceptedHandler(LocalInvitation localInvitation, string response);
		
		/// <summary>
		/// Callback to the caller: occurs when the callee refuses the call invitation.
		/// </summary>
		/// <param name="localInvitation">	An LocalCallInvitation object.</param>
		/// <param name="response">	The callee's response to the call invitation.</param>
		public delegate void OnLocalInvitationRefusedHandler(LocalInvitation localInvitation, string response);
		
		/// <summary>
		/// Callback for the callee: occurs when the callee refuses a call invitation.
		/// </summary>
		/// <param name="remoteInvitation">An RemoteCallInvitation object.</param>
		public delegate void OnRemoteInvitationRefusedHandler(RemoteInvitation remoteInvitation);
		
		/// <summary>
		/// Callback to the callee: occurs when the callee accepts a call invitation.
		/// </summary>
		/// <param name="remoteInvitation">A RemoteCallInvitation object.</param>
		public delegate void OnRemoteInvitationAcceptedHandler(RemoteInvitation remoteInvitation);
		
		/// <summary>
		/// Callback to the callee: occurs when the callee receives a call invitation.
		/// </summary>
		/// <param name="remoteInvitation">	A RemoteCallInvitation object.</param>
		public delegate void OnRemoteInvitationReceivedHandler(RemoteInvitation remoteInvitation);
		
		/// <summary>
		/// Callback to the callee: occurs when the life cycle of the incoming call invitation ends in failure.
		/// </summary>
		/// <param name="remoteInvitation">	A RemoteCallInvitation object.</param>
		/// <param name="errorCode">The error code. </param>
		public delegate void OnRemoteInvitationFailureHandler(RemoteInvitation remoteInvitation, REMOTE_INVITATION_ERR_CODE errorCode);
		
		/// <summary>
		/// Callback to the callee: occurs when the caller cancels the call invitation.
		/// </summary>
		/// <param name="remoteInvitation">An RemoteCallInvitation object.</param>
		public delegate void OnRemoteInvitationCanceledHandler(RemoteInvitation remoteInvitation);

		public OnLocalInvitationReceivedByPeerHandler OnLocalInvitationReceivedByPeer;
		public OnLocalInvitationCanceledHandler OnLocalInvitationCanceled;
		public OnLocalInvitationFailureHandler OnLocalInvitationFailure;
		public OnLocalInvitationAcceptedHandler OnLocalInvitationAccepted;
		public OnLocalInvitationRefusedHandler OnLocalInvitationRefused;
		public OnRemoteInvitationRefusedHandler OnRemoteInvitationRefused;
		public OnRemoteInvitationAcceptedHandler OnRemoteInvitationAccepted;
		public OnRemoteInvitationReceivedHandler OnRemoteInvitationReceived;
		public OnRemoteInvitationFailureHandler OnRemoteInvitationFailure;
		public OnRemoteInvitationCanceledHandler OnRemoteInvitationCanceled;

		private CRtmCallEventHandler rtmCallEventHandler;
		private CRtmCallEventHandlerPtr rtmCallEventHandlerPtr;
		private IntPtr globalPtr = IntPtr.Zero;

		public RtmCallEventHandler() {
			_currentIdIndex = id;
			_rtmCallEventHandlerDic.Add(_currentIdIndex, this);
			rtmCallEventHandler = new CRtmCallEventHandler {
				_onLocalInvitationReceivedByPeer = OnLocalInvitationReceivedByPeerCallback,
				_onLocalInvitationCanceled = OnLocalInvitationCanceledCallback,
				_onLocalInvitationFailure = OnLocalInvitationFailureCallback,
				_onLocalInvitationAccepted = OnLocalInvitationAcceptedCallback,
				_onLocalInvitationRefused = OnLocalInvitationRefusedCallback,
				_onRemoteInvitationRefused = OnRemoteInvitationRefusedCallback,
				_onRemoteInvitationAccepted = OnRemoteInvitationAcceptedCallback,
				_onRemoteInvitationReceived = OnRemoteInvitationReceivedCallback,
				_onRemoteInvitationFailure = OnRemoteInvitationFailureCallback,
				_onRemoteInvitationCanceled = OnRemoteInvitationCanceledCallback
			};

			rtmCallEventHandlerPtr = new CRtmCallEventHandlerPtr {
				_onLocalInvitationReceivedByPeer = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onLocalInvitationReceivedByPeer),
				_onLocalInvitationCanceled = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onLocalInvitationCanceled),
				_onLocalInvitationFailure = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onLocalInvitationFailure),
				_onLocalInvitationAccepted = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onLocalInvitationAccepted),
				_onLocalInvitationRefused = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onLocalInvitationRefused),
				_onRemoteInvitationRefused = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onRemoteInvitationRefused),
				_onRemoteInvitationAccepted = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onRemoteInvitationAccepted),
				_onRemoteInvitationReceived = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onRemoteInvitationReceived),
				_onRemoteInvitationFailure = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onRemoteInvitationFailure),
				_onRemoteInvitationCanceled = Marshal.GetFunctionPointerForDelegate(rtmCallEventHandler._onRemoteInvitationCanceled)
			};


			globalPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CRtmCallEventHandlerPtr)));
			Marshal.StructureToPtr(rtmCallEventHandlerPtr, globalPtr, true);

			_rtmCallEventHandlerPtr = IRtmApiNative.i_rtm_call_event_handler_createEventHandler(_currentIdIndex, globalPtr);
			id ++;
		}


		public void Release() {
			Debug.Log("_rtmCallEventHandlerPtr Release");
			if (_rtmCallEventHandlerPtr == IntPtr.Zero) {
				return;
			}

			_rtmCallEventHandlerDic.Remove(_currentIdIndex);
			IRtmApiNative.i_rtm_call_event_releaseEventHandler(_rtmCallEventHandlerPtr);
			_rtmCallEventHandlerPtr = IntPtr.Zero;

			Marshal.FreeHGlobal(globalPtr);
			globalPtr = IntPtr.Zero;
		}

		internal IntPtr GetPtr()
        {
			return _rtmCallEventHandlerPtr;
        }

		[MonoPInvokeCallback(typeof(EngineEventOnLocalInvitationReceivedByPeerHandler))]
        private static void OnLocalInvitationReceivedByPeerCallback(int _id, IntPtr localInvitationPtr) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationReceivedByPeer != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationReceivedByPeer != null) {
							LocalInvitation _localInvitation = new LocalInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnLocalInvitationReceivedByPeer(_localInvitation);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnLocalInvitationCanceledHandler))]
        private static void OnLocalInvitationCanceledCallback(int _id, IntPtr localInvitationPtr) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationCanceled != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationCanceled != null) {
							LocalInvitation _localInvitation = new LocalInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnLocalInvitationCanceled(_localInvitation);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnLocalInvitationFailureHandler))]
        private static void OnLocalInvitationFailureCallback(int _id, IntPtr localInvitationPtr, LOCAL_INVITATION_ERR_CODE errorCode) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationFailure != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationFailure != null) {
							LocalInvitation _localInvitation = new LocalInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnLocalInvitationFailure(_localInvitation, errorCode);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnLocalInvitationAcceptedHandler))]
        private static void OnLocalInvitationAcceptedCallback(int _id, IntPtr localInvitationPtr, string response) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationAccepted != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationAccepted != null) {
							LocalInvitation _localInvitation = new LocalInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnLocalInvitationAccepted(_localInvitation, response);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnLocalInvitationRefusedHandler))]
        private static void OnLocalInvitationRefusedCallback(int _id, IntPtr localInvitationPtr, string response) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationRefused != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnLocalInvitationRefused != null) {
							LocalInvitation _localInvitation = new LocalInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnLocalInvitationRefused(_localInvitation, response);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnRemoteInvitationRefusedHandler))]
        private static void OnRemoteInvitationRefusedCallback(int _id, IntPtr localInvitationPtr) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationRefused != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationRefused != null) {
							RemoteInvitation _localInvitation = new RemoteInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnRemoteInvitationRefused(_localInvitation);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnRemoteInvitationAcceptedHandler))]
        private static void OnRemoteInvitationAcceptedCallback(int _id, IntPtr localInvitationPtr) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationAccepted != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationAccepted != null) {
							RemoteInvitation _localInvitation = new RemoteInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnRemoteInvitationAccepted(_localInvitation);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnRemoteInvitationReceivedHandler))]
        private static void OnRemoteInvitationReceivedCallback(int _id, IntPtr localInvitationPtr) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationReceived != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationReceived != null) {
							RemoteInvitation _localInvitation = new RemoteInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnRemoteInvitationReceived(_localInvitation);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnRemoteInvitationFailureHandler))]
        private static void OnRemoteInvitationFailureCallback(int _id, IntPtr localInvitationPtr, REMOTE_INVITATION_ERR_CODE errorCode) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationFailure != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationFailure != null) {
							RemoteInvitation _localInvitation = new RemoteInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnRemoteInvitationFailure(_localInvitation, errorCode);
						}
					});
				}
			}
        }

		[MonoPInvokeCallback(typeof(EngineEventOnRemoteInvitationCanceledHandler))]
        private static void OnRemoteInvitationCanceledCallback(int _id, IntPtr localInvitationPtr) {
			if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationCanceled != null) {
				if (AgoraCallbackObject.GetInstance()._CallbackQueue != null) {
					AgoraCallbackObject.GetInstance()._CallbackQueue.EnQueue(()=>{
						if (_rtmCallEventHandlerDic.ContainsKey(_id) && _rtmCallEventHandlerDic[_id].OnRemoteInvitationCanceled != null) {
							RemoteInvitation _localInvitation = new RemoteInvitation(localInvitationPtr, false);
							_rtmCallEventHandlerDic[_id].OnRemoteInvitationCanceled(_localInvitation);
						}
					});
				}
			}
        }
	}
}
