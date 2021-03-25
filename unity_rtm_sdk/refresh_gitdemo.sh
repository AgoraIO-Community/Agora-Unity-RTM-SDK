#!/bin/bash
#============================================================================== 
#    refresh_gitdemo.sh :  refresh the demo project with the release files
#============================================================================== 
SDK=$1
DEMO=../Unity-RTM-Demo/Assets/AgoraEngine/RTM-Engine
if [ -d $SDK/libs/Plugins ]; then
    echo "Replacing $DEMO plugins with files from $SDK"
    rm -rf $DEMO/Plugins
    cp -a $SDK/libs/Plugins $DEMO
fi

if [ -d $SDK/libs/Rtm-Scripts ]; then
    echo "Replacing $DEMO Rtm-Scripts with files from $SDK"
    rm -rf $DEMO/Rtm-Scripts 
    cp -r $SDK/libs/Rtm-Scripts $DEMO
fi
echo ">>> refresh_gitdemo done."
