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
echo "$0: proj= $PROJDIR AgoraRTMSdk=$AgoraRTMSdk TargetZipball=$TargetZipball"
SAMPLEDIR=$AgoraRTMSdk/samples/Unity-RTM-Demo/Assets/AgoraEngine
mkdir -p $SAMPLEDIR || exit 1
cd $SAMPLEDIR 
cp -PRf $PROJDIR/../Unity-RTM-Demo/Assets/AgoraEngine/RTM-Engine .
cp -PRf $PROJDIR/../Unity-RTM-Demo/Assets/AgoraEngine/RtmDemo .
cd $AgoraRTMSdk

# print the tree
# tree samples; tree libs

zip -ry $TargetZipball samples libs || exit 1

if [[ "${TargetZipball:0:1}" == / || "${TargetZipball:0:2}" == ~[/a-z] ]]
then
    # "Absolute path
    tpath=$TargetZipball
else
    # "Relative"
    tpath="$AgoraRTMSdk/$TargetZipball"

fi
echo "SDK release zip ball is saved in $tpath"
echo ""
