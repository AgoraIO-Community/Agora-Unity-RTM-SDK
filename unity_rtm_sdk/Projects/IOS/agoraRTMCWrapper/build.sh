#!/bin/bash

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

# merge
lipo -create  ./${output_build_tmp_path}/${build_config}-${iphonesimulator_config}/lib${module_name}.a ./${output_build_tmp_path}/${build_config}-${iphoneos_config}/lib${module_name}.a -output ./${output_build_tmp_path}/${build_config}-${iphoneos_config}/lib${module_name}.a || exit 1

echo "------ FINISHED --------"
echo "Created ./${output_build_tmp_path}/${build_config}-${iphoneos_config}/lib${module_name}.a"
exit 0
