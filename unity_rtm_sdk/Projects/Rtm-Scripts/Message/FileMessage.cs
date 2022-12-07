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