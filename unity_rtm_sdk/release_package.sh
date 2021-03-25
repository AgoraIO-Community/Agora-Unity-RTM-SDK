#!/bin/bash
PROJDIR=$1
ReleaseDir=$2
WinDllURL=$3

#--------------------------------------
# release package
#--------------------------------------
cd $PROJDIR/

rm -rf $ReleaseDir/libs
mkdir -p $ReleaseDir/libs/Plugins/iOS/           || exit 1
mkdir -p $ReleaseDir/libs/Plugins/Android/	 || exit 1
mkdir -p $ReleaseDir/libs/Plugins/macOS          || exit 1
mkdir -p $ReleaseDir/libs/Plugins/x86/           || exit 1
mkdir -p $ReleaseDir/libs/Plugins/x86_64         || exit 1

#
# c# files
#
cp -r Rtm-Scripts $ReleaseDir/libs   || exit 1

#
# android plugin
#
cp -a Android/sdk/*.plugin $ReleaseDir/libs/Plugins/Android/

echo "copy mac bundle start"
#
# mac bundle
#
cp -a macOS/sdk/*.bundle  $ReleaseDir/libs/Plugins/macOS/
echo "copy mac bundle end"

#
# ios binaries
#
cp -a IOS/sdk/*  $ReleaseDir/libs/Plugins/iOS/

#
# windows binaries
#
# The CI script provides the download location of the zip file
cd $PROJDIR
if [ -n $WinDllURL ]; then
    WinDllZip="ZipDownload/RTM_WinDll.zip"
    if [ -d ZipDownload ]; then
	rm -rf ZipDownload
    fi
    mkdir ZipDownload
    wget -O $WinDllZip $WinDllURL

    cd $PROJDIR/Windows && unzip -f $PROJDIR/$WinDllZip 
    cp unity/x86/* $ReleaseDir/libs/Plugins/x86/
    cp unity/x86_64/* $ReleaseDir/libs/Plugins/x86_64/ 
    cp agoraRTMCWrapper/sdk/x86/dll/agora_rtm_sdk.dll $ReleaseDir/libs/Plugins/x86/
    cp agoraRTMCWrapper/sdk/x64/dll/agora_rtm_sdk.dll $ReleaseDir/libs/Plugins/x86_64/ 
fi

echo "All sdk build done under $ReleaseDir"
