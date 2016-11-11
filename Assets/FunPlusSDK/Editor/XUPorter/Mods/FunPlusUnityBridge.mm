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
    extern void UnitySendMessage(const char *, const char *, const char *);

    void _install(const char *appId, const char *appKey, const char *rumTag, const char *rumKey, const char *environment) {
        NSString *sAppId = [NSString stringWithUTF8String:appId];
        NSString *sAppKey = [NSString stringWithUTF8String:appKey];
        NSString *sRumTag = [NSString stringWithUTF8String:rumTag];
        NSString *sRumKey = [NSString stringWithUTF8String:rumKey];
        NSString *sEnv = [NSString stringWithUTF8String:environment];
        
        [OCExposer installWithAppId:sAppId
                             appKey:sAppKey
                             rumTag:sRumTag
                             rumKey:sRumKey
                        environment:sEnv];
    }
    
    void _getFPID(const char *externalID, const char *externalIDType) {
        NSString *sExternalID = [NSString stringWithUTF8String:externalID];
        NSString *sExternalIDType = [NSString stringWithUTF8String:externalIDType];
        
        [OCExposer getFPIDWithExternalID:sExternalID
                    externalIDTypeString:sExternalIDType
                               onSuccess:^(NSString * _Nonnull fpid) {
                                   UnitySendMessage("FunPlusEventListener",
                                                    "onGetFPIDSuccess",
                                                    [fpid cStringUsingEncoding:NSUTF8StringEncoding]);
                               }
                               onFailure:^(NSString * _Nonnull error) {
                                   UnitySendMessage("FunPlusEventListener",
                                                    "onGetFPIDFailure",
                                                    [error cStringUsingEncoding:NSUTF8StringEncoding]);
                               }];
    }
    
    void _bindFPID(const char *fpid, const char *externalID, const char *externalIDType) {
        NSString *sFpid = [NSString stringWithUTF8String:fpid];
        NSString *sExternalID = [NSString stringWithUTF8String:externalID];
        NSString *sExternalIDType = [NSString stringWithUTF8String:externalIDType];
        
        [OCExposer bindFPIDWithFpid:sFpid
                         externalID:sExternalID
               externalIDTypeString:sExternalIDType
                          onSuccess:^(NSString * _Nonnull fpid) {
                              UnitySendMessage("FunPlusEventListener",
                                               "onBindFPIDSuccess",
                                               [fpid cStringUsingEncoding:NSUTF8StringEncoding]);
                          }
                          onFailure:^(NSString * _Nonnull error) {
                              UnitySendMessage("FunPlusEventListener",
                                               "onBindFPIDFailure",
                                               [error cStringUsingEncoding:NSUTF8StringEncoding]);
                          }];
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
        NSString *sServiceName = [NSString stringWithUTF8String:serviceName];
        NSString *sHttpUrl = [NSString stringWithUTF8String:httpUrl];
        NSString *sHttpStatus = [NSString stringWithUTF8String:httpStatus];
        NSString *sRequestId = [NSString stringWithUTF8String:requestId];
        NSString *sTargetUserId = [NSString stringWithUTF8String:targetUserId];
        NSString *sGameServerId = [NSString stringWithUTF8String:gameServerId];
        
        [OCExposer traceRUMServiceMonitoringWithServiceName:sServiceName
                                                    httpUrl:sHttpUrl
                                                 httpStatus:sHttpStatus
                                                requestSize:requestSize
                                               responseSize:responseSize
                                                httpLatency:httpLatency
                                                  requestTs:requestTs
                                                 responseTs:responseTs
                                                  requestId:sRequestId
                                               targetUserId:sTargetUserId
                                               gameServerId:sGameServerId];
    }
    
    void _setRUMExtraProperty(const char *key, const char *value) {
        NSString *sKey = [NSString stringWithUTF8String:key];
        NSString *sValue = [NSString stringWithUTF8String:value];
        
        [OCExposer setRUMExtraPropertyWithKey:sKey value:sValue];
    }
    
    void _eraseRUMExtraProperty(const char *key) {
        NSString *sKey = [NSString stringWithUTF8String:key];
        
        [OCExposer eraseRUMExtraPropertyWithKey:sKey];
    }
    
    void _traceDataCustom(const char *eventString) {
        NSString *sEventString = [NSString stringWithUTF8String:eventString];
        
        [OCExposer traceDataCustomWithEventString:sEventString];
    }
    
    void _setDataExtraProperty(const char *key, const char *value) {
        NSString *sKey = [NSString stringWithUTF8String:key];
        NSString *sValue = [NSString stringWithUTF8String:value];
        
        [OCExposer setDataExtraPropertyWithKey:sKey value:sValue];
    }
    
    void _traceDataPayment (const double amount,
                            const char *currency,
                            const char *productId,
                            const char *productName,
                            const char *productType,
                            const char *transactionId,
                            const char *paymentProcessor,
                            const char *itemsReceived,
                            const char *currencyReceived,
                            const char *currencyReceivedType)
    {
        NSString *sCurrency = [NSString stringWithUTF8String:currency];
        NSString *sProductId = [NSString stringWithUTF8String:productId];
        NSString *sProductName = [NSString stringWithUTF8String:productName];
        NSString *sProductType = [NSString stringWithUTF8String:productType];
        NSString *sTransactionId = [NSString stringWithUTF8String:transactionId];
        NSString *sPaymentProcessor = [NSString stringWithUTF8String:paymentProcessor];
        NSString *sItemsReceived = [NSString stringWithUTF8String:itemsReceived];
        NSString *sCurrencyReceived = [NSString stringWithUTF8String:currencyReceived];
        NSString *sCurrencyReceivedType = [NSString stringWithUTF8String:currencyReceivedType];
        
        [OCExposer traceDataPaymentWithAmount:amount
                                     currency:sCurrency
                                    productId:sProductId
                                  productName:sProductName
                                  productType:sProductType
                                transactionId:sTransactionId
                             paymentProcessor:sPaymentProcessor
                                itemsReceived:sItemsReceived
                             currencyReceived:sCurrencyReceived
                         currencyReceivedType:sCurrencyReceivedType];
    }
    
    void _eraseDataExtraProperty(const char *key) {
        NSString *sKey = [NSString stringWithUTF8String:key];
        
        [OCExposer eraseDataExtraPropertyWithKey:sKey];
    }
}
