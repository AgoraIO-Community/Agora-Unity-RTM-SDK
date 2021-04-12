#!/bin/bash
## ==============================================================================
## build script for RTM plugin for Android on Unity
##
## required environmental variables:
##   $RTM_VERSION
## ==============================================================================
PLATFORM="Android"

function download_library {
    DOWNLOAD_URL=$1
    
    if [[ ! -e $DOWNLOAD_FILE ]]; then
        wget $DOWNLOAD_URL
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

function Clean {
    if [ -e prebuilt ]; then
	echo "clean ndk lib build..."
	ndk-build -C jni/ clean 
    fi
    echo "removing Android build intermitten files..."
    rm -rf sdk *.zip Agora_RTM_SDK_for_$PLATFORM 
    rm -rf obj libs bin prebuilt 
}


if [ "$1" == "clean" ]; then
    Clean
    exit 0
fi

# We will require the setting of RTM_VERSION environmental variable
if [ -z ${RTM_VERSION+x} ]; then
    echo "ERROR, environment variable RTM_VERSION (e.g. 'v1_4_2') must be set!"
    exit 1
    else echo "$PLATFORM RTM_VERSION = $RTM_VERSION"
fi

#download
download_library $1
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


