#!/bin/bash
#============================================================================== 
#   copy_demo.sh :  build script for RTM SDK
#
#    This script copies required files to make a demo zipball for user download.
#   
#
#============================================================================== 
AgoraRTMSdk=$1
TargetZipball=$2

PROJDIR=`pwd`
echo proj= $PROJDIR
SAMPLEDIR=$AgoraRTMSdk/samples/Unity-RTM-Demo/Assets/AgoraEngine
mkdir -p $SAMPLEDIR || exit 1
cd $SAMPLEDIR 
cp -a $PROJDIR/../Unity-RTM-Demo/Assets/AgoraEngine/RTM-Engine .
cp -a $PROJDIR/../Unity-RTM-Demo/Assets/AgoraEngine/RtmDemo .
cd $AgoraRTMSdk

# print the tree
tree samples; tree libs

zip -r $TargetZipball samples libs
echo "SDK release zip ball is saved in $TargetZipball"
echo ""
