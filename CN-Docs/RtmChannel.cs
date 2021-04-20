using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RtmChannel :  IRtmApiNative, IDisposable{
		private IntPtr _rtmChannelPtr = IntPtr.Zero;
		private bool _disposed = false;
		private RtmChannelEventHandler _channelEventHandler;
		
		public RtmChannel(IntPtr rtmChannelPtr, RtmChannelEventHandler rtmChannelEventHandler) {
			_rtmChannelPtr = rtmChannelPtr;
			_channelEventHandler = rtmChannelEventHandler;
		}

		~RtmChannel() {
			Dispose(false);
		}

		/// <summary>
		/// 加入频道。
		/// @note 同一用户只能同时加入最多 20 个频道。加入频道超限时用户会收到错误码 \ref agora_rtm.LEAVE_CHANNEL_ERR.JOIN_CHANNEL_ERR_FAILURE "JOIN_CHANNEL_ERR_FAILURE"。
		/// - 方法调用成功：
	    ///   - 本地用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnJoinSuccessHandler "OnJoinSuccessHandler"。
		///   - 所有远端用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnMemberJoinedHandler "OnMemberJoinedHandler"。
		/// - 方法调用失败：本地用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnJoinFailureHandler "OnJoinFailureHandler"。 错误代码信息详见 \ref agora_rtm.JOIN_CHANNEL_ERR "JOIN_CHANNEL_ERR"。
		/// </summary>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。错误码详见 \ref agora_rtm.LEAVE_CHANNEL_ERR "JOIN_CHANNEL_ERR"。
		/// </returns>
		public int Join() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_join(_rtmChannelPtr);
		}

		/// <summary>
		/// 离开频道。
		/// - 方法调用成功：
		///   - 本地用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnLeaveHandler "OnLeaveHandler" 的 `LEAVE_CHANNEL_ERR_OK` 状态。
		///   - 所有远端用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnMemberLeftHandler "OnMemberLeftHandler" callback。
		/// - 方法调用失败：本地用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnLeaveHandler "OnLeaveHandler" 的错误代码。错误代码信息详见 \ref agora_rtm.LEAVE_CHANNEL_ERR "LEAVE_CHANNEL_ERR"。
		/// </summary>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。错误码详见 \ref agora_rtm.LEAVE_CHANNEL_ERR "LEAVE_CHANNEL_ERR"。
		/// </returns>
		public int Leave() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_leave(_rtmChannelPtr);
		}

		/// <summary>
		/// 我们不推荐使用该方法发送频道消息。请改用它的重载方法 \ref agora_rtm.RtmChannel.SendMessage(IMessage message,SendMessageOptions options) "SendMessage"。
		/// 方法调用成功：
		/// - 本地用户收到回调 \ref agora_rtm.RtmClientEventHandler.OnSendMessageResultHandler "OnSendMessageResultHandler"。
		/// - 所有远端用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnMessageReceivedHandler "OnMessageReceivedHandler"。
		/// </summary>
		/// <param name="message">发送的消息。 详见 \ref agora_rtm.IMessage "IMessage"。</param>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。错误码详见 \ref agora_rtm.CHANNEL_MESSAGE_ERR_CODE "CHANNEL_MESSAGE_ERR_CODE"。
		/// </returns>
		public int SendMessage(IMessage message) {
			if (_rtmChannelPtr == IntPtr.Zero || message.GetPtr() == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_sendMessage(_rtmChannelPtr, message.GetPtr());
		}

		/// <summary>
		/// 供频道成员向所在频道发送频道消息。
		/// 方法调用成功：
		/// - 本地用户收到回调 \ref agora_rtm.RtmClientEventHandler.OnSendMessageResultHandler "OnSendMessageResultHandler"。
		/// - 所有远端用户收到回调 \ref agora_rtm.RtmChannelEventHandler.OnMessageReceivedHandler "OnMessageReceivedHandler"。
		/// @note 发送消息（包括点对点消息和频道消息）的调用频率上限为每 3 秒 180 次。
		/// </summary>
		/// <param name="message">发送的消息。详见 \ref agora_rtm.IMessage "IMessage"。</param>
		/// <param name="options">消息发送选项。详见 \ref agora_rtm.SendMessageOptions "SendMessageOptions"。</param>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。错误码详见 \ref agora_rtm.CHANNEL_MESSAGE_ERR_CODE "CHANNEL_MESSAGE_ERR_CODE"。
		/// </returns>
		public int SendMessage(IMessage message, SendMessageOptions options)
		{
			if (_rtmChannelPtr == IntPtr.Zero || message.GetPtr() == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_sendMessage2(_rtmChannelPtr, message.GetPtr(), options.enableOfflineMessaging, options.enableHistoricalMessaging);
		}

		/// <summary>
		/// 获取当前频道 ID。
		/// </summary>
		/// <returns>当前频道 ID。</returns>
		public int GetId() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_getId(_rtmChannelPtr);
		}

		/// <summary>
		/// 获取频道成员列表。
		/// SDK 通过 \ref agora_rtm.RtmChannelEventHandler.OnGetMembersHandler "OnGetMembersHandler" 回调返回该方法的调用结果（频道成员列表）。	
		/// @note 该方法的调用频率上限为每 2 秒 5 次。该方法最多获取 512 个频道成员。如果频道成员数量超过 512，该方法会随机返回其中的 512 个成员。
		/// </summary>
		/// <returns>
		///  - 0: 方法调用成功。
		///  - ≠0: 方法调用失败。错误码详见 \ref agora_rtm.GET_MEMBERS_ERR "GET_MEMBERS_ERR" 。
		/// </returns>
		public int GetMembers() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
				Debug.LogError("_rtmChannelPtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
			return channel_getMembers(_rtmChannelPtr);
		}

 		/// <summary>
		/// 释放当前 #RtmChannel 实例使用的所有资源。
		/// </summary>
		public void Dispose() {
			 Dispose(true);
			 GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing) {
			if (_disposed) return;
			if (disposing){}
			Release();
			_disposed = true;
			Debug.Log("RtmChannel Dispose");
		}

		private void Release() {
			if (_rtmChannelPtr == IntPtr.Zero)
			{
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
