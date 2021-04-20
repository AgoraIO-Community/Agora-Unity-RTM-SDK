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
			_Height = imageMessage.GetHeight();
			_ThumbnailWidth = imageMessage.GetThumbnailWidth();
			_ThumbnailHeight = imageMessage.GetThumbnailHeight();
		}

        ~ImageMessage() {
            Release();
        }

		/// <summary>
		/// 获取上传图片的大小。
		/// </summary>
		/// <returns>上传图片的大小，单位为字节。</returns>
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

		/// <summary>
		/// 获取上传图片的 media ID。
		/// @note
		///  - 图片成功上传到服务器后，SDK 会自动分配一个 media ID。
		///  - media ID 的有效期为 7 天，因为每份上传图片只能在文件服务器保留 7 天。
		/// </summary>
		/// <returns>上传图片的 media ID。</returns>
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
		/// 指定上传图片的缩略图。
		/// </summary>
		/// <param name="thumbnail">上传图片的缩略图。</param>
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

		/// <summary>
		/// 获取上传图片的缩略图。
		/// </summary>
		/// <returns>上传图片的缩略图。</returns>
        public byte[] GetThumbnail() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Thumbnail;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return _Thumbnail;
			}
			long Length = iImage_message_getThumbnailLength(_MessagePtr);
			byte [] rawData = new byte[Length];
			IntPtr _ThumbnailData = iImage_message_getThumbnailData(_MessagePtr);
			Marshal.Copy(_ThumbnailData, rawData, 0, (int)Length);
            return rawData;
        }

		/// <summary>
		/// 设置上传图片的文件名称。
		/// </summary>
		/// <param name="fileName">	上传图片的文件名称。`thumbnail` 和 `fileName` 总体大小不能超过 32 KB.</param>
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

		/// <summary>
		/// 获取上传图片的文件名称。
		/// </summary>
		/// <returns>上传图片的文件名称。</returns>
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

		/// <summary>
		/// 设置上传图片的宽度。
		/// @note
		/// - 如果上传图片的格式为 JPG、JPEG、BMP，或 PNG，SDK 会自动计算图片的宽和高。你可以通过调用 #GetWidth 方法直接获取图片的宽度。
		/// - 用户自行设置的图片宽度会覆盖由 SDK 计算得出的图片宽度。
		/// </summary>
		/// <param name="width">上传图片的宽度。</param>
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

		/// <summary>
		/// 获取上传图片的宽度。
		/// @note
		/// - 如果上传图片的格式为 JPG、JPEG、BMP，或 PNG，SDK 会自动计算图片的宽和高。你可以通过调用本方法直接获取图片的宽度。
		/// - 用户通过调用 #SetWidth 方法自行设置的图片宽度会覆盖由 SDK 计算得出的图片宽度。
		/// </summary>
		/// <returns>上传图片的宽度。如果SDK不支持上传图像的格式，则返回0。</returns>
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

		/// <summary>
		/// 设置上传图片的高度。
		/// @note 
		/// - 如果上传图片的格式为 JPG、JPEG、BMP，或 PNG，SDK 会自动计算图片的宽和高。你可以通过调用 #GetHeight 方法直接获取图片的高度。
		/// - 用户自行设置的图片高度会覆盖由 SDK 计算得出的图片高度。
		/// </summary>
		/// <param name="height">上传图片的高度。</param>
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

		/// <summary>
		/// 获取上传图片的高度。
		/// @note
		/// - 如果上传图片的格式为 JPG、JPEG、BMP，或 PNG，SDK 会自动计算图片的宽和高。你可以通过调用本方法直接获取图片的高度。
		/// - 用户通过调用 #SetHeight 方法自行设置的图片高度会覆盖由 SDK 计算得出的图片高度。
		/// </summary>
		/// <returns>上传图片的高度。</returns>
        public int GetHeight() {
			if (_MessageFlag == MESSAGE_FLAG.RECEIVE)
				return _Height;

			if (_MessagePtr == IntPtr.Zero)
			{
				Debug.LogError("_MessagePtr is null");
				return (int)COMMON_ERR_CODE.ERROR_NULL_PTR;
			}
            return iImage_message_getHeight(_MessagePtr);
        }

		/// <summary>
		/// 设置缩略图的宽度。
		/// @note 须自行计算，SDK 不会计算缩略图的宽度。
		/// </summary>
		/// <param name="thumbnailWidth">缩略图的宽度。</param>
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

		/// <summary>
		/// 获取缩略图的宽度。
		/// </summary>
		/// <returns>缩略图的宽度。</returns>
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

		/// <summary>
		/// 设置缩略图的高度。
		/// @note 须自行计算，SDK 不会计算缩略图的高度。
		/// </summary>
		/// <param name="height">缩略图的高度。</param>
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

		/// <summary>
		/// 获取缩略图的高度。
		/// </summary>
		/// <returns>缩略图的高度。</returns>
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