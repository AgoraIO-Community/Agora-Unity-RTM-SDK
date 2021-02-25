#!/bin/bash
## build script for RTM plugin for Android on Unity

function download_library {
    DOWNLOAD_URL="https://download.agora.io/rtmsdk/release"
    DOWNLOAD_FILE="Agora_RTM_SDK_for_Android_Unity_v1_4_2.zip"
    
    if [[ ! -e $DOWNLOAD_FILE ]]; then
        wget $DOWNLOAD_URL/$DOWNLOAD_FILE
    fi
    #unzip
    unzip -o $DOWNLOAD_FILE
}

function make_unity_plugin {
    rm -rf sdk

    SDKDIR="sdk/AgoraRtmEngineKit.plugin"
    mkdir -p $SDKDIR/libs
    cp AndroidManifest.xml $SDKDIR
    cp project.properties $SDKDIR
    cp -a bin/arm64-v8a $SDKDIR/libs
    cp -a bin/armeabi-v7a $SDKDIR/libs
    cp -a bin/x86 $SDKDIR/libs
}

#download
download_library
mkdir prebuilt
cp -r Agora_RTM_SDK_for_Android/libs/* prebuilt/

COMMITNR=`git log --pretty="%h" | head -n 1`
dirty=`[[ $(git diff --shortstat 2> /dev/null | tail -n1) != "" ]] && echo "*"`
if [ "$dirty" == "*" ]; then
    COMMITNR=${COMMITNR}-dirty
fi

EXTRACFLAGS=APP_CFLAGS=-DGIT_SRC_VERSION=${COMMITNR}
# clean
#rm -r libs/ || exit 1
ndk-build -C jni/ clean || exit 1

# create shared library
ndk-build V=1 $EXTRACFLAGS -C jni/ || exit 1


rm -rf bin
mkdir bin || exit 1
cp -r prebuilt/ bin/ || exit 1
cp -r libs/ bin/ || exit 1
cp AndroidManifest.xml bin/example-AndroidManifest.xml || exit 1

# sdk
make_unity_plugin

echo "------ FINISHED --------"
echo "#cp -r bin/ /Users/Shared/Agora/apps/Unity3D/VideoTexture/Assets/Plugins/Android/libs"
echo
echo "Success! => Android RTM plugin is created in $PWD/sdk"
exit 0


