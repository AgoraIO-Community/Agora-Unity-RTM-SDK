LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)
LOCAL_MODULE := agora-rtc
LOCAL_SRC_FILES := ../prebuilt/$(TARGET_ARCH_ABI)/libagora-rtm-sdk-jni.so
include $(PREBUILT_SHARED_LIBRARY)

include $(CLEAR_VARS)   
LOCAL_MODULE_CLASS := SHARED_LIBRARIES
LOCAL_MODULE       := libagoraRTMCWrapper
LOCAL_MODULE_TAGS  := optional
LOCAL_SRC_FILES := \
                ChannelEventHandler.cpp\
				i_local_call_invitation.cpp\
				i_remote_call_invitation.cpp\
				i_rtm_call_event_handler.cpp\
				i_rtm_call_manager.cpp\
				i_rtm_channel_attribute.cpp\
				i_rtm_channel_event_handler.cpp\
				i_rtm_channel_member.cpp\
				i_rtm_channel.cpp\
				i_rtm_message.cpp\
				i_rtm_service_event_handler.cpp\
				i_rtm_service.cpp\
				LogHelper.cpp\
				RtmCallEventHandler.cpp\
				RtmServiceEventHandler.cpp


LOCAL_LDLIBS := -llog -landroid
LOCAL_SHARED_LIBRARIES := agora-rtc
include $(BUILD_SHARED_LIBRARY)
#-lGLESv2
