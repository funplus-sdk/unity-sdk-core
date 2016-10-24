//#if UNITY_IPHONE
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using HSMiniJSON;

namespace FunPlusSDK
{
	public class FunPlusiOS
	{
		[DllImport ("__Internal")]
		private static extern void fpInstall (string appId, string appKey, string environment);
		[DllImport ("__Internal")]
		private static extern void fpTraceRUMServiceMonitoring (
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
			string gameServerId
		);
		[DllImport ("__Internal")]
		private static extern void fpSetRUMExtraProperty (string key, string value);
		[DllImport ("__Internal")]
		private static extern void fpTraceDataCustom (string key, string value);
		[DllImport ("__Internal")]
		private static extern void fpTraceDataPayment (
			string productId,
			string productName,
			string productType,
			string transactionId,
			string paymentProcessor,
			string amount,
			string currency,
			string currencyReceived,
			string currencyReceivedType,
			string itemsReceived
		);
		[DllImport ("__Internal")]
		private static extern void fpSetDataExtraProperty (string key, string value);

		public FunPlusiOS () {}

		public void install (string appId, string appKey, string environment) {
			install(appId, appKey, environment);
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
		}

		public void setRUMExtraProperty (string key, string value)
		{
		}

		public void traceDataCustom(IDictionary dataEvent)
		{
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
		}

		public void setDataExtraProperty (string key, string value)
		{
		}
	}

}
//#endif
