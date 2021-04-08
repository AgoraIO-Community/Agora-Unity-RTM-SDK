#!/bin/bash
## ==============================================================================
## build script for RTM plugin for Windows on Unity
## this build script doesn't support building on Mac environment.
## Please use the python build script and bat files on Windows PC to build the target.
## required environmental variables:
##   $RTM_VERSION
## ==============================================================================

PLATFORM="Windows"

function download_library {
    ARCH=$1
    DOWNLOAD_VERSION=$2
    DOWNLOAD_URL="http://192.168.99.149:8086/v1.4.3.402/RTMSDK/Windows/${ARCH}"
    ZIP_FILE="Agora_RTM_SDK_for_Windows_${ARCH}_Unity_${DOWNLOAD_VERSION}.zip"

    if [[ ! -e $ZIP_FILE ]]; then
        wget $DOWNLOAD_URL/$ZIP_FILE
	status=$?
	if [ ! $status == 0 ]; then
	    echo "Status of wget:$status"
	    # change from v1_4_2 to v1.4.2
	    VERSION_FORMAT2="${DOWNLOAD_VERSION//_/.}"
	    if [ $DOWNLOAD_VERSION == $VERSION_FORMAT2 ]; then
		# we've tried the two formats!
		echo "$ZIP_FILE Fail!" && exit 1
	    else 
		download_library $ARCH $VERSION_FORMAT2
	    fi
	fi
    fi

    #unzip
    (cd sdk && unzip -o ../$ZIP_FILE product_sdk/sdk/* && mv product_sdk/sdk $ARCH)
    rm -rf sdk/product_sdk
}

function package_windows_project {
    # move the sdk into the directory stucture
    mv sdk agoraRTMCWrapper/
    
    zip -r $1 agoraRTMCWrapper
}

function Clean {
    echo "removing $PLATFORM build intermitten files..."
    rm -rf sdk *.zip Agora_RTM_SDK_for_${PLATFORM}_* 
}

if [ "$1" == "clean" ]; then
    Clean
    exit 0
fi

# We will require the setting of RTM_VERSION environmental variable
if [ -z ${RTM_VERSION+x} ]; then
    echo "ERROR, environment variable RTM_VERSION (e.g. 'v1_4_2') must be set!"
    exit 1
    else echo "$PLATFORM RTM_VERSION = $RTM_VERSION"
fi

DOWNLOAD_VERSION=$RTM_VERSION

OUTPUT_PACKAGE="RTM_windows.zip"

rm -f $OUTPUT_PACKAGE
rm -rf sdk agoraRTMCWrapper/sdk
mkdir sdk

download_library "x86" $DOWNLOAD_VERSION
download_library "x64" $DOWNLOAD_VERSION

package_windows_project $OUTPUT_PACKAGE

echo
echo "Windows project file is prepared in $PWD/$OUTPUT_PACKAGE"

