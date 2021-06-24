#!/bin/bash
## build script for RTM plugin for iOS on Unity
## Note: replace the team id for signing on agoraRTMCWrapper.xcodeproj/project.pbxproj
##	 by setting up our environment variable APPLE_TEAM_ID

PLATFORM="iOS"

CURDIR=$(pwd)

function download_library {
    DOWNLOAD_URL=$1
    DOWNLOAD_FILE="IOS_Native.zip"

    if [[ ! -e $DOWNLOAD_FILE ]]; then
        wget $DOWNLOAD_URL -O $DOWNLOAD_FILE
    fi
    #unzip
    unzip -o $DOWNLOAD_FILE
}

function copy_source_code() {
    echo "Copy source code start"
    local SOURCE_CODE_PATH="${CURDIR}/../../sourceCode/"
    local DST_PATH="${CURDIR}/agoraRTMCWrapper/"
    cp -r ${SOURCE_CODE_PATH}* $DST_PATH
    find $DST_PATH -type f -exec rename 's/\.cpp/\.mm/' '{}' \;
    echo "Copy source code end"
}

function make_unity_plugin {
    rm -rf sdk
    mkdir sdk
    cp -PRf Agora_RTM_SDK_for_iOS/libs/AgoraRtmKit.xcframework/ios-arm64_armv7/ sdk/
    cp -PRf output/tmp/Release-iphoneos/ sdk/
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

download_library $1

# Copy sourceCode
copy_source_code

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
