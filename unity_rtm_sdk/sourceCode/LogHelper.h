//
//  LogHelper.hpp
//  agoraRTMCWrapper
//
//  Created by 张涛 on 2020/10/15.
//  Copyright © 2020 张涛. All rights reserved.
//

#ifndef AGORA_LOG_HELPER_H
#define AGORA_LOG_HELPER_H

#include <stdarg.h>
#include <fstream>
#include <iostream>

namespace agora {
namespace unity {
class LogHelper {
 private:
  FILE* fileStream = nullptr;

 public:
  LogHelper() {}

  ~LogHelper() { stopLogService(); }

  static LogHelper& getInstance() {
    static LogHelper logHelper;
    return logHelper;
  }

 public:
  void startLogService(const char* filePath) {
    if (fileStream)
      return;

    if (filePath)
      fileStream = fopen(filePath, "ab+");
  }

  void stopLogService() {
    if (fileStream) {
      fflush(fileStream);
      fclose(fileStream);
      fileStream = nullptr;
    }
  }

  void writeLog(const char* format, ...) {
    va_list la;
    va_start(la, format);

    if (!fileStream)
      return;

    vfprintf(fileStream, format, la);
    va_end(la);
    fprintf(fileStream, "\n");
    fflush(fileStream);
  }
};
}  // namespace unity
}  // namespace agora

#endif
