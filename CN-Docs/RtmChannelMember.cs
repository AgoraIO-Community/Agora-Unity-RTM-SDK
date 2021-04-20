using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
	public sealed class RtmChannelMember : IRtmApiNative {
		private string _UserId {
			get;
			set;
		}

		private string _ChannelId {
			get;
			set;
		}
		public RtmChannelMember(string userId, string channelId) {
			_UserId = userId;
			_ChannelId = channelId;
		}

		/// <summary>
		/// 获取频道内指定用户的 ID。
		/// </summary>
		/// <returns>频道内指定用户的 ID。</returns>
		public string GetUserId() {
			return _UserId;
		}

		/// <summary>
		/// 获取指定用户所在频道的 ID。
		/// </summary>
		/// <returns>指定用户所在频道的 ID。</returns>
		public string GetChannelId() {
			return _ChannelId;
		}
	}
}
