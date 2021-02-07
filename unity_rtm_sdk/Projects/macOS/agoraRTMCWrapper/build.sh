#!/bin/bash

#use environment variable BUILD_CONFIG and BUILD_TARGET to config build

wget https://download.agora.io/rtmsdk/release/Agora_RTM_SDK_for_Mac_Unity_v1_4_2.zip
#unzip
unzip -o Agora_RTM_SDK_for_Mac_Unity_v1_4_2.zip


BUILD_CONFIG=${BUILD_CONFIG:=Release}
BUILD_TARGET=${BUILD_TARGET:=clean build}

export build_config=$BUILD_CONFIG
export output_root="output"
export output_build_tmp_path="output/tmp"

# MAC
export macosx_config="macosx"


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

# MAC 
xcodebuild -project ${module_name}.xcodeproj -target ${module_name} -configuration ${build_config} -sdk ${macosx_config} ${BUILD_TARGET} SYMROOT=${output_build_tmp_path} ${EXTRACFLAGS} || exit 1

rm -rf sdk/

cp -r output/tmp/Release/ sdk/
# cp -r Agora_RTM_SDK_for_Mac/libs/libagora_rtm_sdk.dylib sdk/

echo "------ FINISHED --------"
echo "Created ./${output_build_tmp_path}/${build_config}/${module_name}.bundle"
exit 0
