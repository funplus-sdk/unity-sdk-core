//
//  FunPlusUnityBridge.mm
//  FunPlusUnityBridge
//
//  Created by Yuankun Zhang on 24/10/2016.
//  Copyright © 2016 FunPlus. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <FunPlusSDK/FunPlusSDK-Swift.h>

extern "C"
{
    void _install(const char *appId, const char *appKey, const char *environment) {
        NSString *sAppId = [NSString stringWithUTF8String:appId];
        NSString *sAppKey = [NSString stringWithUTF8String:appKey];
        NSString *sEnv = [NSString stringWithUTF8String:environment];
        
        [OCExposer installWithAppId:sAppId
                             appKey:sAppKey
                        environment:sEnv];
    }
    
    void _traceRUMServiceMonitoring(const char *serviceName,
                                    const char *httpUrl,
                                    const char *httpStatus,
                                    const int requestSize,
                                    const int responseSize,
                                    const long httpLatency,
                                    const long requestTs,
                                    const long responseTs,
                                    const char *requestId,
                                    const char *targetUserId,
                                    const char *gameServerId)
    {
        
    }
    
    void _setRUMExtraProperty(const char *key, const char *value) {
        
    }
    
    void _traceDataCustom() {
        
    }
    
    void _setDataExtraProperty(const char *key, const char *value) {
        
    }
    
    void _traceDataPayment (const double amount,
                            const char *currency,
                            const char *productId,
                            const char *productName,
                            const char *productType,
                            const char *transactionId,
                            const char *paymentProcessor,
                            const char *currencyReceived,
                            const char *currencyReceivedType,
                            const char *itemsReceived)
    {
//        [OCExposer traceDataPaymentWithAmount:amount
//                                     currency:currency
//                                    productId:productId
//                                  productName:productName
//                                  productType:productType
//                                transactionId:transactionId
//                             paymentProcessor:paymentProcessor
//                                itemsReceived:itemsReceived
//                             currencyReceived:currencyReceived]
    }
}
