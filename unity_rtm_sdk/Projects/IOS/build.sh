#!/bin/bash
## build script for RTM plugin for iOS on Unity
## Note: replace the team id for signing on agoraRTMCWrapper.xcodeproj/project.pbxproj
##	 by setting up our environment variable APPLE_TEAM_ID

PLATFORM="iOS"


function download_library {
    DOWNLOAD_URL="http://192.168.99.149:8086/v1.4.3.402/RTMSDK/iOS/"
    DOWNLOAD_FILE="Agora_RTM_SDK_for_iOS_Unity_v1.4.3.402_20210408_24.zip"
    
    if [[ ! -e $DOWNLOAD_FILE ]]; then
        wget $DOWNLOAD_URL/$DOWNLOAD_FILE
    fi
    #unzip
    unzip -o $DOWNLOAD_FILE
}

function make_unity_plugin {
    rm -rf sdk
    cp -a Agora_RTM_SDK_for_iOS/libs/ sdk/
    cp -a output/tmp/Release-iphoneos/ sdk/
}

function Clean {
    echo "removing $PLATFORM build intermitten files..."
    rm -rf sdk *.zip Agora_RTM_SDK_for_$PLATFORM  
    rm -rf obj libs bin output
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

#use environment variable BUILD_CONFIG and BUILD_TARGET to config build

BUILD_CONFIG=${BUILD_CONFIG:=Release}
BUILD_TARGET=${BUILD_TARGET:=clean build}

export build_config=$BUILD_CONFIG
export output_root="output"
export output_build_tmp_path="output/tmp"

# Simulators
export iphonesimulator_config="iphonesimulator"
# iOS Device
export iphoneos_config="iphoneos"

echo ******** Settings ********
echo BUILD_CONFIG=$BUILD_CONFIG
echo BUILD_TARGET=$BUILD_TARGET
echo

# contains clean target?
if [ "${BUILD_TARGET#clean}" != "$BUILD_TARGET" ]; then
    rm -rf output && echo "output/ is deleted"
fi

mkdir -p ./${output_root}/${build_config} || exit 1

download_library

module_name=agoraRTMCWrapper

# git commit number
COMMITNR=`git log --pretty="%h" | head -n 1`
dirty=`[[ $(git diff --shortstat 2> /dev/null | tail -n1) != "" ]] && echo "*"`
if [ "$dirty" == "*" ]; then
    COMMITNR=${COMMITNR}-dirty
fi

EXTRACFLAGS=OTHER_CFLAGS="-DGIT_SRC_VERSION=${COMMITNR}"

# simulator
xcodebuild -project ${module_name}.xcodeproj -target ${module_name} -configuration ${build_config} -sdk ${iphonesimulator_config} ${BUILD_TARGET} SYMROOT=${output_build_tmp_path} ${EXTRACFLAGS} -UseModernBuildSystem=NO || exit 1

# iphone
xcodebuild -project ${module_name}.xcodeproj -target ${module_name}  -configuration ${build_config} -sdk ${iphoneos_config} ${BUILD_TARGET} SYMROOT=${output_build_tmp_path} ${EXTRACFLAGS} -UseModernBuildSystem=NO || exit 1

lipo ./${output_build_tmp_path}/${build_config}-${iphonesimulator_config}/lib${module_name}.a -remove arm64 -output ./${output_build_tmp_path}/${build_config}-${iphonesimulator_config}/lib${module_name}.a

# merge
lipo -create  ./${output_build_tmp_path}/${build_config}-${iphonesimulator_config}/lib${module_name}.a ./${output_build_tmp_path}/${build_config}-${iphoneos_config}/lib${module_name}.a -output ./${output_build_tmp_path}/${build_config}-${iphoneos_config}/lib${module_name}.a || exit 1

make_unity_plugin

echo "------ FINISHED --------"
echo "Created ./${output_build_tmp_path}/${build_config}-${iphoneos_config}/lib${module_name}.a"
echo
echo "Success! => iOS RTM plugin is created in $PWD/sdk"
exit 0
