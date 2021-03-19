#!/bin/bash
PROJDIR=$1
ReleaseDir=$2

#--------------------------------------
# release package
#--------------------------------------
cd $PROJDIR/

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
cp -a Windows/sdk/x86/*  $ReleaseDir/libs/Plugins/x86/
cp -a Windows/sdk/x64/*  $ReleaseDir/libs/Plugins/x86_64/

echo "All sdk build done under $ReleaseDir"
