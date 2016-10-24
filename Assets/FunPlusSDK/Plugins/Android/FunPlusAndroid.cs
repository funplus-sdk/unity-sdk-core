//#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HSMiniJSON;
using System.Linq;
using System.Collections;
using System.Threading;

namespace FunPlusSDK
{
	public class FunPlusAndroid : IWorkerMethodDispacther
	{
		private AndroidJavaClass jc;
		private AndroidJavaObject currentActivity, application;
		private AndroidJavaObject sdkBridgeClass;
		private AndroidJavaClass sdkAPIDelegate;

		public FunPlusAndroid ()
		{
			this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			this.currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			this.application = currentActivity.Call<AndroidJavaObject>("getApplication");
			this.sdkAPIDelegate = new AndroidJavaClass("com.funplus.sdk.UnityAPIDelegate");
			FunPlusWorker.getInstance ().registerClient ("support", this);
		}

		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		void sdkApiCall(string api, params object[] args) {
			addSdkApiCallToQueue ("sdkApiCallWithArgs", api, args);
		}

		void sdkApiCall(string api) {
			addSdkApiCallToQueue ("sdkApiCall", api, null);
		}

		void addSdkApiCallToQueue(String methodIdentifier, String api, object[] args) {
			FunPlusWorker.getInstance ().enqueueApiCall ("support", methodIdentifier, api, args);
		}

		public void install(string appId, string appKey, string environment)
		{
		}

		public void traceRUMServiceMonitoring(
			string serviceName,
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
			sdkApiCall (
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

		public void setRUMExtraProperty (string key, string value)
		{
			sdkApiCall ("setRUMExtraProperty", key, value);
		}

		public void traceDataCustom(IDictionary dataEvent)
		{
			sdkApiCall ("traceDataCustom", dataEvent);
		}

		public void traceDataPayment(
			string productId,
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
			sdkApiCall (
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

		public void setDataExtraProperty (string key, string value)
		{
			sdkApiCall ("setDataExtraProperty", key, value);
		}
	}
}
//#endif