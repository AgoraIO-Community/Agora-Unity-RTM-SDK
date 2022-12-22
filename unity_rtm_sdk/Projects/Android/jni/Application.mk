APP_ABI := armeabi-v7a x86 arm64-v8a x86_64
APP_OPTIM := release
APP_PLATFORM := android-20
APP_CPPFLAGS += -std=c++11
APP_CPPFLAGS += -fno-exceptions
APP_CPPFLAGS += -fno-rtti -fPIC -fpic
APP_ALLOW_MISSING_DEPS=true
APP_CPPFLAGS += -DBUILD_AGORA_GAMING_SDK
APP_CFLAGS += -DBUILD_AGORA_GAMING_SDK

#gnustl_static c++_static
APP_STL := c++_static
