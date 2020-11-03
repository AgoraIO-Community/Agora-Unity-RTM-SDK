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

		public string GetUserId() {
			return _UserId;
		}

		public string GetChannelId() {
			return _ChannelId;
		}
	}
}
