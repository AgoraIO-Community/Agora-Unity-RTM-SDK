#!/bin/bash
#============================================================================== 
#   copy_demo.sh :  build script for RTM SDK
#
#    This script copies required files to make a demo zipball for user download.
#   
#
#============================================================================== 
AgoraRTMSdk=$1
PROJDIR=`pwd`
SAMPLEDIR=$AgoraRTMSdk/samples/Unity-RTM-Demo/Assets/AgoraEngine
mkdir -p $SAMPLEDIR || exit 1
cd $SAMPLEDIR 
cp -a $PROJDIR/../Unity-RTM-Demo/Assets/AgoraEngine/RTM-Engine .
cp -a $PROJDIR/../Unity-RTM-Demo/Assets/AgoraEngine/RtmDemo .

