using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_rtm {
    public sealed class ImageMessage : IMessage {
		private Int64 _Size = 0;
		private string _MediaId = "";
		private byte[] _Thumbnail = null;
		private string _FileName = "";
		private int _Width = 0;
		private int _Height = 0;
		private int _ThumbnailWidth = 0;
		private int _ThumbnailHeight = 0;

        public ImageMessage(IntPtr MessagePtr) {
            _MessagePtr = MessagePtr;
        }

		public ImageMessage(IntPtr MessagePtr, MESSAGE_FLAG MessageFlag) {
			_MessageFlag = MessageFlag;
			_MessagePtr = MessagePtr;
		}

		public ImageMessage(ImageMessage imageMessage, MESSAGE_FLAG MessageFlag) {
			_MessageFlag = MessageFlag;
			_MessageId = imageMessage.GetMessageId();
			_MessageType = imageMessage.GetMessageType();
			_MessageText = imageMessage.GetText();
			_IsOfflineMessage = imageMessage.IsOfflineMessage();
			_Ts = imageMessage.GetServerReceiveTs();
			_RawMessageData = imageMessage.GetRawMessageData();
			_Length = imageMessage.GetRawMessageLength();
			_Size = imageMessage.GetSize();
			_MediaId = imageMessage.GetMediaId();
			_FileName = imageMessage.GetFileName();
			_Width = imageMessage.GetWidth();
			_Height = imageMessage.GetHight();
			_ThumbnailWidth = imageMessage.GetThumbnailWidth();
			_ThumbnailHeight = imageMessage.GetThumbnailHeight();
		}
        ~ImageMessage() {
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
            return iImage_message_getSize(_MessagePtr);
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
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE) {
				_Thumbnail = thumbnail;
				return;
			}

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
            iImage_message_setThumbnail(_MessagePtr, thumbnail, thumbnail.Length);
        }

        public IntPtr GetThumbnail() {
			// if (_MessageFlag == MESSAGE_FLAG.RECEIVE) {
			// 	return _Thumbnail;
			// }

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return IntPtr.Zero;
			}
            return iImage_message_getThumbnailData(_MessagePtr);
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
            iImage_message_setFileName(_MessagePtr, fileName);
        }

        public string GetFileName() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _FileName;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR + "";
			}
            IntPtr fileNamePtr = iImage_message_getFileName(_MessagePtr);
            if (!ReferenceEquals(fileNamePtr, IntPtr.Zero)) {
				return Marshal.PtrToStringAnsi(fileNamePtr);
			} else {
				return "";
			}
        }

        public void SetWidth(int width) {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
			{
				_Width = width;
				return;
			}

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
            iImage_message_setWidth(_MessagePtr, width);
        }

        public int GetWidth() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Width;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return iImage_message_getWidth(_MessagePtr);
        }

        public void SetHeight(int height) {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
			{
				_Height = height;
				return;
			}

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
            iImage_message_setHeight(_MessagePtr, height);
        }

        public int GetHight() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Height;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return iImage_message_getHeight(_MessagePtr);
        }

        public void SetThumbnailWidth(int thumbnailWidth) {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
			{
				_ThumbnailWidth = thumbnailWidth;
				return;
			}


			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
            iImage_message_setThumbnailWidth(_MessagePtr, thumbnailWidth);
        }

        public int GetThumbnailWidth() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _ThumbnailWidth;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return iImage_message_getThumbnailWidth(_MessagePtr);
        }

        public void SetThumbnailHeight(int height) {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
			{
				_ThumbnailHeight = height;
				return;
			}

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return;
			}
            iImage_message_setThumbnailHeight(_MessagePtr, height);
        }

        public int GetThumbnailHeight() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _ThumbnailHeight;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return iImage_message_getThumbnailHeight(_MessagePtr);
        }
    }
}