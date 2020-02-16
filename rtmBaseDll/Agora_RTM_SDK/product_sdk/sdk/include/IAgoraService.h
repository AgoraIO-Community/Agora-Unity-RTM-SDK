//  Agora SDK
//
//  Copyright (c) 2018 Agora.io. All rights reserved.
//

#ifndef AGORA_SERVICE_H
#define AGORA_SERVICE_H
#include "AgoraBase.h"

namespace agora {
    namespace rtc {
        class IRtcEngine;
    }
    namespace rtm {
        class IRtmService;
    }
namespace base {

struct AgoraServiceContext
{
};


class IAgoraService
{
public:
    virtual ~IAgoraService() {}
    
    /**
     Releases all resources used by the IAgoraService instance.
     */
    virtual void release() = 0;

	/**
     Initializes the IAgoraService instance.
     
     @param context The Agora service context.
     
     @return
     - 0: Success.
     - < 0: Failure.
     */
    virtual int initialize(const AgoraServiceContext& context) = 0;
    
    /// @cond
    /** Gets the SDK version number.
    * @param build Build number.
    * @return The current SDK version in the sting format. For example, 2.3.0
    */
    virtual const char* getVersion(int* build) = 0;

    virtual rtc::IRtcEngine* createRtcEngine() = 0;
    /// @endcond
    
    /**
     Creates an \ref agora::rtm::IRtmService "IRtmService" instance.
     
     @note When you no longer need an \ref agora::base::IAgoraService "IAgoraService" instance, ensure that you call the \ref agora::rtm::IRtmService::release "release" method to release all resources that it uses.

     @return An \ref agora::rtm::IRtmService "IRtmService" instance.
     */
    virtual rtm::IRtmService* createRtmService() = 0;
};

} //namespace base
} // namespace agora

////////////////////////////////////////////////////////
/** \addtogroup getAgoraSdkVersion
 @{
 */
////////////////////////////////////////////////////////

/**
 Gets the SDK version number.
 
 @param build Set it as null.
 @return The version number of the SDK.
 */
AGORA_API const char* AGORA_CALL getAgoraSdkVersion(int* build);

////////////////////////////////////////////////////////
/** @} */
////////////////////////////////////////////////////////

/// @cond

/**
* Creates the RtcEngine object and returns the pointer.
* @param err Error code
* @return returns Description of the error code
*/
AGORA_API const char* AGORA_CALL getAgoraSdkErrorDescription(int err);
/// @endcond

////////////////////////////////////////////////////////
/** \addtogroup createAgoraService
 @{
 */
////////////////////////////////////////////////////////

/**
 Creates an \ref agora::base::IAgoraService "IAgoraService" instance.
 
 @note
 - The Agora RTM SDK supports creating multiple \ref agora::base::IAgoraService "IAgoraService" instances.
 - When you no longer need an \ref agora::base::IAgoraService "IAgoraService" instance, ensure that you call the \ref agora::base::IAgoraService::release "release" method to release all resources that it uses.
 
 @return An \ref agora::base::IAgoraService "IAgoraService" instance.
*/
AGORA_API agora::base::IAgoraService* AGORA_CALL createAgoraService();

////////////////////////////////////////////////////////
/** @} */
////////////////////////////////////////////////////////

/// @cond
AGORA_API int AGORA_CALL setAgoraSdkExternalSymbolLoader(void* (*func)(const char* symname));
/// @endcond
#endif
