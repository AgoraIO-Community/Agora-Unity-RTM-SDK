//
//  i_rtm_file_message.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/9/13.
//  Copyright © 2020 张涛. All rights reserved.
//
#pragma once

#include "common.h"

AGORA_API long long iFile_message_getSize(void* file_message_instance);

/**
Gets the media ID of the uploaded file.

@note
- The media ID is automatically populated once the file is uploaded to the file
server.
- The media ID is valid for 7 days because the file server keeps all uploaded
files for 7 days only.

@return The media ID of the uploaded file.
*/
AGORA_API const char* iFile_message_getMediaId(void* file_message_instance);

/**
Sets the thumbnail of the uploaded file.

@param thumbnail The thumbnail of the uploaded file. Must be binary data.
@param length The length of the thumbnail. The size of @p thumbnail and @p
fileName combined must not exceed 32 KB.
*/
AGORA_API void iFile_message_setThumbnail(void* file_message_instance,
                                          const uint8_t* thumbnail,
                                          int length);

/**
Gets the thumbnail of the uploaded file.

@return The thumbnail of the uploaded file.
*/
AGORA_API const char* iFile_message_getThumbnailData(
    void* file_message_instance);

/**
Gets the length of the thumbnail.

@return The length of the thumbnail.
*/
AGORA_API const long long iFile_message_getThumbnailLength(
    void* file_message_instance);

/**
Sets the name of the uploaded file.

@param fileName The name of the uploaded file. The size of @p thumbnail and @p
fileName combined must not exceed 32 KB.
*/
AGORA_API void iFile_message_setFileName(void* file_message_instance,
                                         const char* fileName);

/**
Gets the name of the uploaded file.

@return The name of the uploaded file.
*/
AGORA_API const char* iFile_message_getFileName(void* file_message_instance);
