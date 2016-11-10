#if UNITY_IPHONE
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;

namespace FunPlus
{
	public class FunPlusiOS
	{
		[DllImport ("__Internal")]
		private static extern void _install (string appId, string appKey, string environment);
		[DllImport ("__Internal")]
		private static extern void _getFPID (string externalID, string externalIDType);
		[DllImport ("__Internal")]
		private static extern void _bindFPID (string fpid, string externalID, string externalIDType);
		[DllImport ("__Internal")]
		private static extern void _traceRUMServiceMonitoring (string serviceName,
															   string httpUrl,
															   string httpStatus,
															   int requestSize,
															   int responseSize,
															   long httpLatency,
															   long requestTs,
															   long responseTs,
															   string requestId,
															   string targetUserId,
															   string gameServerId);
		[DllImport ("__Internal")]
		private static extern void _setRUMExtraProperty (string key, string value);
		[DllImport ("__Internal")]
		private static extern void _eraseRUMExtraProperty (string key);
		[DllImport ("__Internal")]
		private static extern void _traceDataCustom (string jsonEventDict);
		[DllImport ("__Internal")]
		private static extern void _traceDataPayment (double amount,
			                                          string currency,
			                                          string productId,
			                                          string productName,
			                                          string productType,
			                                          string transactionId,
			                                          string paymentProcessor,
			                                          string itemsReceived,
			                                          string currencyReceived,
													  string currencyReceivedType);
		[DllImport ("__Internal")]
		private static extern void _setDataExtraProperty (string key, string value);
		[DllImport ("__Internal")]
		private static extern void _eraseDataExtraProperty (string key);


		public FunPlusiOS () {}

		public void Install (string appId, string appKey, string environment) {
			_install(appId, appKey, environment);
		}

		public void GetFPID(string externalID, string externalIDType)
		{
			_getFPID (externalID, externalIDType);
		}

		public void BindFPID(string fpid, string externalID, string externalIDType)
		{
			_bindFPID (fpid, externalID, externalIDType);
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
			_traceRUMServiceMonitoring (
				serviceName, httpUrl, httpStatus, requestSize, responseSize, httpLatency,
				requestTs, responseTs, requestId, targetUserId, gameServerId
			);
		}

		public void SetRUMExtraProperty (string key, string value)
		{
			_setRUMExtraProperty (key, value);
		}

		public void EraseRUMExtraProperty (string key)
		{
			_eraseRUMExtraProperty (key);
		}

		public void TraceDataCustom(Dictionary<string, object> dataEvent)
		{
			_traceDataCustom (Json.Serialize (dataEvent));
		}

		public void TraceDataPayment(double amount,
			                         string currency,
			                         string productId,
			                         string productName,
			                         string productType,
			                         string transactionId,
			                         string paymentProcessor,
			                         string itemsReceived,
			                         string currencyReceived,
									 string currencyReceivedType)
		{
			_traceDataPayment (
				amount, currency, productId, productName, productType, transactionId,
				paymentProcessor, itemsReceived, currencyReceived, currencyReceivedType
			);
		}

		public void SetDataExtraProperty (string key, string value)
		{
			_setDataExtraProperty (key, value);
		}

		public void EraseDataExtraProperty (string key)
		{
			_eraseDataExtraProperty (key);
		}
	}

}
#endif
