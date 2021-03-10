#!/bin/bash
## build script for RTM plugin for Android on Unity


function download_library {
    DOWNLOAD_URL="https://download.agora.io/rtmsdk/release"
    x86_FILE="Agora_RTM_SDK_for_Windows_x86_Unity_v1.4.2.zip"
    x64_FILE="Agora_RTM_SDK_for_Windows_x64_Unity_v1.4.2.zip"

    if [[ ! -e $x86_FILE ]]; then
        wget $DOWNLOAD_URL/$x86_FILE
    fi
    if [[ ! -e $x64_FILE ]]; then
        wget $DOWNLOAD_URL/$x64_FILE
    fi
    #unzip
    rm -rf sdk agoraRTMCWrapper/sdk
    mkdir sdk
    (cd sdk && unzip -o ../$x86_FILE product_sdk/sdk/* && mv product_sdk/sdk x86)
    (cd sdk && unzip -o ../$x64_FILE product_sdk/sdk/* && mv product_sdk/sdk x64)
    rm -rf sdk/product_sdk
}

function package_windows_project {
    # move the sdk into the directory stucture
    mv sdk agoraRTMCWrapper/
    
    zip -r $1 agoraRTMCWrapper
}

OUTPUT_PACKAGE="RTM_windows.zip"

rm -f $OUTPUT_PACKAGE
download_library
package_windows_project $OUTPUT_PACKAGE

echo
echo "Windows project file is prepared in $PWD/$OUTPUT_PACKAGE"

