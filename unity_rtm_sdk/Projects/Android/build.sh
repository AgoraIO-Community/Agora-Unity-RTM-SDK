#!/bin/bash
#download
wget https://download.agora.io/rtmsdk/release/Agora_RTM_SDK_for_Android_Unity_v1_4_2.zip
#unzip
unzip -o Agora_RTM_SDK_for_Android_Unity_v1_4_2.zip
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

rm -rf sdk/
cp -r bin/arm64-v8a/ sdk/arm64-v8a/
cp -r bin/armeabi-v7a/ sdk/armeabi-v7a/
cp -r bin/x86/ sdk/x86/

echo "------ FINISHED --------"
echo "#cp -r bin/ /Users/Shared/Agora/apps/Unity3D/VideoTexture/Assets/Plugins/Android/libs"
exit 0


