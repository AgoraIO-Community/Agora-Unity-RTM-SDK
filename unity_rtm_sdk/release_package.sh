#!/bin/bash
PROJDIR=$1
ReleaseDir=$2

echo "Release package PROJDIR:$PROJDIR ReleaseDir:$ReleaseDir"
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
cp -PRf Android/sdk/*.plugin $ReleaseDir/libs/Plugins/Android/

echo "copy mac bundle start"
#
# mac bundle
#
cp -PRf macOS/sdk/  $ReleaseDir/libs/Plugins/macOS/
echo "copy mac bundle end"

#
# ios binaries
#
cp -PRf IOS/sdk/*  $ReleaseDir/libs/Plugins/iOS/

#
# windows binaries
#
# cd $PROJDIR
# if [ -n $WinDllURL ]; then
if [ -d ZipDownload ]; then
rm -rf ZipDownload
fi
WinDllZip="RTM_WinDll.zip"
# mkdir ZipDownload && cd ZipDownload
# # wget -O $PROJDIR/ZipDownload/$WinDllZip $WinDllURL
if [ ! $? == 0 ]; then
echo "error downloading $WinDllURL"
exit 1
fi
pwd
unzip $WinDllZip 
#unzip -f $PROJDIR/ZipDownload/$WinDllZip -d $PROJDIR/ZipDownload/
cp unity/x86/* $ReleaseDir/libs/Plugins/x86/
cp unity/x86_64/* $ReleaseDir/libs/Plugins/x86_64/ 
# fi

echo "All sdk build done under $ReleaseDir"
