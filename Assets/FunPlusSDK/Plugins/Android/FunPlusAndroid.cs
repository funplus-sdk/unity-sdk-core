#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HSMiniJSON;
using System.Linq;
using System.Collections;
using System.Threading;

namespace FunPlus
{
	public class FunPlusAndroid : IWorkerMethodDispacther
	{
		private AndroidJavaClass jc;
		private AndroidJavaObject currentActivity, application;
		private AndroidJavaObject sdkBridgeClass;

		public FunPlusAndroid ()
		{
			this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			this.currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			this.application = currentActivity.Call<AndroidJavaObject>("getApplication");
			FunPlusWorker.GetInstance ().RegisterClient ("support", this);
		}

		public void ResolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		void SdkApiCall(string api, params object[] args) {
			AddSdkApiCallToQueue ("SdkApiCallWithArgs", api, args);
		}

		void SdkApiCall(string api) {
			AddSdkApiCallToQueue ("SdkApiCall", api, null);
		}

		void AddSdkApiCallToQueue(String methodIdentifier, String api, object[] args) {
			FunPlusWorker.GetInstance ().EnqueueApiCall ("support", methodIdentifier, api, args);
		}

		public void Install(string appId, string appKey, string environment)
		{
		}

		public void TraceRUMServiceMonitoring(string serviceName,
			                                  string httpUrl,
			                                  string httpStatus,
			                                  int requestSize,
			                                  int responseSize,
			                                  long httpLatency,
			                                  long requestTs,
			                                  long responseTs,
			                                  string requestId,
			                                  string targetUserId,
			                                  string gameServerId)
		{
			SdkApiCall (
				"traceRUMServiceMonitoring",
				serviceName,
				httpUrl,
				httpStatus,
				requestSize,
				responseSize,
				httpLatency,
				requestTs,
				requestId,
				targetUserId,
				gameServerId
			);
		}

		public void SetRUMExtraProperty (string key, string value)
		{
			SdkApiCall ("setRUMExtraProperty", key, value);
		}

		public void TraceDataCustom(IDictionary dataEvent)
		{
			SdkApiCall ("traceDataCustom", dataEvent);
		}

		public void TraceDataPayment(string productId,
			                         string productName,
			                         string productType,
			                         string transactionId,
			                         string paymentProcessor,
			                         string amount,
			                         string currency,
			                         string currencyReceived,
			                         string currencyReceivedType,
			                         string itemsReceived)
		{
			SdkApiCall (
				"traceDataPayment", 
				productId,
				productName,
				productType,
				transactionId,
				paymentProcessor,
				amount,
				currency,
				currencyReceived,
				currencyReceivedType,
				itemsReceived
			);
		}

		public void SetDataExtraProperty (string key, string value)
		{
			SdkApiCall ("setDataExtraProperty", key, value);
		}
	}
}
#endif