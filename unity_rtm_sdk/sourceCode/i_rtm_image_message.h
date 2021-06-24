//
//  i_rtm_image_message.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//

#pragma once

#include "common.h"

AGORA_API long long iImage_message_getSize(void* image_message_instance);

/**
Gets the media ID of the uploaded image.

@note
- The media ID is automatically populated once the image is uploaded to the file
server.
- The media ID is valid for 7 days because the file server keeps all uploaded
files for 7 days only.

@return The media ID of the uploaded image.
*/
AGORA_API const char* iImage_message_getMediaId(void* image_message_instance);

/**
Sets the thumbnail of the uploaded image.

@param thumbnail The thumbnail of the uploaded image.
@param length The length of the thumbnail in bytes. The size of @p thumbnail and
@p fileName combined must not exceed 32 KB.
*/
AGORA_API void iImage_message_setThumbnail(void* image_message_instance,
                                           const uint8_t* thumbnail,
                                           long long length);

/**
Gets the thumbnail data of the uploaded image.

@return The thumbnail data of the uploaded image.
*/
AGORA_API const char* iImage_message_getThumbnailData(
    void* image_message_instance);

/**
Gets the length of the thumbnail data.

@return The length of the thumbnail data.
*/
AGORA_API const long long iImage_message_getThumbnailLength(
    void* image_message_instance);

/**
Sets the file name of the uploaded image.

@param fileName The file name of the uploaded image. The size of @p thumbnail
and @p fileName combined must not exceed 32 KB.
*/
AGORA_API void iImage_message_setFileName(void* image_message_instance,
                                          const char* fileName);

/**
Gets the file name of the uploaded image.

@return The file name of the uploaded image.
*/
AGORA_API const char* iImage_message_getFileName(void* image_message_instance);

/**
Sets the width of the uploaded image.

@note
- If the uploaded image is in JPG, JPEG, BMP, or PNG format, the SDK
automatically calculates the width and height of the image. You can call \ref
agora::rtm::IImageMessage::getWidth() "getWidth" directly to get the width of
the image.
- Image width that is set by calling this method overrides the width calculated
by the SDK.

@param width The width of the uploaded image.
*/
AGORA_API void iImage_message_setWidth(void* image_message_instance, int width);

/**
Gets the width of the uploaded image.

@note
- If the uploaded image is in JPG, JPEG, BMP, or PNG format, the SDK
automatically calculates the width and height of the image. You can call this
method directly to get the width of the image.
- Image width that is set by calling \ref agora::rtm::IImageMessage::setWidth()
"setWidth" overrides the width calculated by the SDK.

@return The width of the uploaded image. Is 0 if the SDK does not support the
format of the uploaded image.
*/
AGORA_API int iImage_message_getWidth(void* image_message_instance);

/**
Sets the height of the uploaded image.

@note
- If the uploaded image is in JPG, JPEG, BMP, or PNG format, the SDK
automatically calculates the width and height of the image. You can call \ref
agora::rtm::IImageMessage::getHeight() "getHeight" directly to get the height of
the image.
- Image height that is set by calling this method overrides the height
calculated by the SDK.

@param height The height of the uploaded image. Is 0 if the SDK does not support
the format of the uploaded image.
*/
AGORA_API void iImage_message_setHeight(void* image_message_instance,
                                        int height);

/**
Gets the height of the uploaded image.

@note
- If the uploaded image is in JPG, JPEG, BMP, or PNG format, the SDK
automatically calculates the width and height of the image. You can call this
method directly to get the height of the image.
- Image height that is set by calling \ref
agora::rtm::IImageMessage::setHeight() "setHeight" overrides the height
calculated by the SDK.

@return The height of the uploaded image.
*/
AGORA_API int iImage_message_getHeight(void* image_message_instance);

/**
Sets the width of the thumbnail.

@note You need to work out the width of the thumbnail by yourself, because the
SDK does not work out the value for you.

@param width The width of the thumbnail.
*/
AGORA_API void iImage_message_setThumbnailWidth(void* image_message_instance,
                                                int width);

/**
Gets the width of the thumbnail.

@return The width of the thumbnail.
*/
AGORA_API int iImage_message_getThumbnailWidth(void* image_message_instance);

/**
Sets the height of the thumbnail.

@note You need to work out the height of the thumbnail by yourself, because the
SDK does not work out the value for you.

@param height The height of the thumbnail.
*/
AGORA_API void iImage_message_setThumbnailHeight(void* image_message_instance,
                                                 int height);

/**
Gets the height of the thumbnail.

@return The height of the thumbnail.
*/
AGORA_API int iImage_message_getThumbnailHeight(void* image_message_instance);
