using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
    public sealed class FileMessage : IMessage {
		private Int64 _Size {
			get;
			set;
		}

		private string _MediaId {
			get;
			set;
		}

		private byte[] _Thumbnail {
			get;
			set;
		}

		private string _FileName {
			get;
			set;
		}

		public FileMessage(IntPtr MessagePtr) {
			_MessagePtr = MessagePtr;
		}


        public FileMessage(IntPtr MessagePtr, MESSAGE_FLAG MessageFlag) {
            _MessagePtr = MessagePtr;
			_MessageFlag = MessageFlag;
        }

		public FileMessage(FileMessage fileMessage, MESSAGE_FLAG MessageFlag) {
			_MessageFlag = MessageFlag;
			_MessageId = fileMessage.GetMessageId();
			_MessageType = fileMessage.GetMessageType();
			_MessageText = fileMessage.GetText();
			_IsOfflineMessage = fileMessage.IsOfflineMessage();
			_Ts = fileMessage.GetServerReceiveTs();
			_RawMessageData = fileMessage.GetRawMessageData();
			_Length = fileMessage.GetRawMessageLength();
			_Size = fileMessage.GetSize();
			_MediaId = fileMessage.GetMediaId();
			_Thumbnail = fileMessage.GetThumbnailData();
			_FileName = fileMessage.GetFileName();
		}

        ~FileMessage() {
			Debug.Log(" ~FileMessage");
            Release();
        }

		/// <summary>
		/// 获取上传文件的大小。
		/// </summary>
		/// <returns>上传文件的大小，单位为字节。</returns>
        public Int64 GetSize() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Size;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return iFile_message_getSize(_MessagePtr);
        }

		/// <summary>
		/// 获取上传文件的 media ID。
		/// @note
		/// - 文件成功上传到服务器后，SDK 会自动分配一个 media ID。
		/// - media ID 的有效期为 7 天，因为每份上传文件只能在文件服务器保留 7 天。
		/// </summary>
		/// <returns>上传文件的 media ID。</returns>
        public string GetMediaId() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _MediaId;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
            IntPtr mediaIdPtr = iFile_message_getMediaId(_MessagePtr);
            if (!ReferenceEquals(mediaIdPtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(mediaIdPtr);
			} else {
				return "";
			}
        }

		/// <summary>
		/// 指定上传文件的缩略图（二进制文件）。
		/// </summary>
		/// <param name="thumbnail">上传文件的缩略图。必须是二进制文件。`thumbnail` 和 `fileName` 数据长度加起来的大小不得超过 32 KB。</param>
        public void SetThumbnail(byte[] thumbnail) {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
			{
				_Thumbnail = thumbnail;
				return;
			}

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
            iFile_message_setThumbnail(_MessagePtr, thumbnail, thumbnail.Length);
        }

		/// <summary>
		/// 获取上传文件的缩略图。
		/// </summary>
		/// <returns>上传文件的缩略图。</returns>
        public byte[] GetThumbnailData() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Thumbnail;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return _Thumbnail;
			}
			long Length = iFile_message_getThumbnailLength(_MessagePtr);
			byte [] rawData = new byte[Length];
			IntPtr _ThumbnailData = iFile_message_getThumbnailData(_MessagePtr);
			Marshal.Copy(_ThumbnailData, rawData, 0, (int)Length);
            return rawData;
        }

		/// <summary>
		/// 设置上传文件的文件名。
		/// </summary>
		/// <param name="fileName">上传文件的文件名。`thumbnail` 和 `fileName` 加起来的大小不得超过 32 KB。</param>
        public void SetFileName(string fileName) {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
			{
				_FileName = fileName;
				return;
			}

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
            iFile_message_setFileName(_MessagePtr, fileName);
        }

		/// <summary>
		/// 获取上传文件的文件名。
		/// </summary>
		/// <returns>上传文件的文件名。</returns>
        public string GetFileName() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _FileName;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
            IntPtr fileNamePtr = iFile_message_getFileName(_MessagePtr);
            if (!ReferenceEquals(fileNamePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(fileNamePtr);
			} else {
				return "";
			}
        }
    }
}