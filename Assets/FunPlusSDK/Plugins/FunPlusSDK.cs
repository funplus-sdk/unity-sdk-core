using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FunPlus
{
	public class FunPlusSDK
	{
		#region FunPlusSDK members

		public static string VERSION = "4.0.1";

		#if UNITY_IOS || UNITY_ANDROID
		private static FunPlusSDK instance = null;
		#endif

		#if UNITY_EDITOR
		#elif UNITY_IOS
		private static FunPlusiOS nativeSdk = null;
		#elif UNITY_ANDROID
		private static FunPlusAndroid nativeSdk = null;
		#endif

		#endregion // FunPlusSDK members

		private FunPlusSDK() {}

		public static void Install ()
		{
			#if UNITY_IOS || UNITY_ANDROID
			if (instance != null)
			{
				Debug.LogWarning ("[FunPlusSDK] FunPlus SDK has already been installed.");
				return;
			}

			instance = new FunPlusSDK ();

			string appId = FunPlusSDKConfig.Instance.AppId;
			string appKey = FunPlusSDKConfig.Instance.AppKey;
			string rumTag = FunPlusSDKConfig.Instance.RumTag;
			string rumKey = FunPlusSDKConfig.Instance.RumKey;
			string environment = FunPlusSDKConfig.Instance.Environment;

			#if UNITY_EDITOR
			Debug.LogFormat ("[FunPlusSDK] Installing FunPlus SDK: {{appId={0}, appKey={1}, rumTag={2}, rumKey={3}, environment={4}}}.", appId, appKey, rumTag, rumKey, environment);
			#elif UNITY_IOS
			nativeSdk = new FunPlusiOS ();
			nativeSdk.Install (appId, appKey, rumTag, rumKey, environment);
			#elif UNITY_ANDROID
			nativeSdk = new FunPlusAndroid ();
			nativeSdk.Install (appId, appKey, rumTag, rumKey, environment);
			#endif
			#endif
		}

		#region getters
		public static FunPlusID GetFunPlusID()
		{
			return FunPlusID.GetInstance ();
		}
			
		public static FunPlusRUM GetFunPlusRUM()
		{
			return FunPlusRUM.GetInstance ();
		}

		public static FunPlusData GetFunPlusData()
		{
			return FunPlusData.GetInstance ();
		}
		#endregion // getters

		#region functional classes
		public class FunPlusID
		{
			private static FunPlusID instance;

			private FunPlusID() {}

			public static FunPlusID GetInstance()
			{
				if (instance == null) {
					instance = new FunPlusID ();
				}
				return instance;
			}

			public string GetSessionId() {
				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusID.GetFPID().");
				return "mock_funplus_session_id";
				#elif UNITY_IOS || UNITY_ANDROID
				return nativeSdk.GetSessionId();
				#endif
			}

			public void GetFPID(string externalID, ExternalIDType externalIDType, Action<string> onSuccess, Action<string> onFailure)
			{
				FunPlusEventListener.getFPIDSuccessHandler = onSuccess;
				FunPlusEventListener.getFPIDFailureHandler = onFailure;

				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusID.GetFPID().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.GetFPID(externalID, ExternalIDTypeExtension.ToTypeString (externalIDType));
				#endif
			}

			public void BindFPID(string fpid, string externalID, ExternalIDType externalIDType, Action<string> onSuccess, Action<string> onFailure)
			{
				FunPlusEventListener.bindFPIDSuccessHandler = onSuccess;
				FunPlusEventListener.bindFPIDFailureHandler = onFailure;

				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusID.BindFPID().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.BindFPID(fpid, externalID, ExternalIDTypeExtension.ToTypeString (externalIDType));
				#endif
			}
		}

		public class FunPlusRUM
		{
			private static FunPlusRUM instance;

			private FunPlusRUM() {}

			public static FunPlusRUM GetInstance()
			{
				if (instance == null) {
					instance = new FunPlusRUM ();
				}
				return instance;
			}

			public void TraceServiceMonitoring(string serviceName,
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

				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusRUM.TraceServiceMonitoring().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.TraceRUMServiceMonitoring(
					serviceName, httpUrl, httpStatus, requestSize, responseSize, httpLatency, requestTs,
					responseTs, requestId, targetUserId, gameServerId
				);
				#endif
			}

			public void SetExtraProperty (string key, string value)
			{
				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusRUM.SetExtraProperty().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.SetRUMExtraProperty(key, value);
				#endif
			}

			public void EraseExtraProperty(string key)
			{
				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusRUM.EraseExtraProperty().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.EraseRUMExtraProperty(key);
				#endif
			}
		}

		public class FunPlusData
		{
			private static FunPlusData instance;

			private FunPlusData() {}

			public static FunPlusData GetInstance()
			{
				if (instance == null) {
					instance = new FunPlusData ();
				}
				return instance;
			}

			public void TraceCustom(Dictionary<string, object> dataEvent)
			{
				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusData.TraceCustom().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.TraceDataCustom(dataEvent);
				#endif
			}

			public void TraceCustomEventWithNameAndProperties(string eventName, Dictionary<string, object> properties)
			{
			#if UNITY_EDITOR
			Debug.Log ("[FunPlusSDK] FunPlusData.TraceCustomEventWithNameAndProperties().");
			#elif UNITY_IOS || UNITY_ANDROID
			nativeSdk.TraceCustomEventWithNameAndProperties(eventName, properties);
			#endif
			}

			public void TracePayment(double amount,
				                     string currency,
				                     string productId,
				                     string productName,
				                     string productType,
				                     string transactionId,
				                     string paymentProcessor,
									 string itemsReceived,
				                     string currencyReceived)
			{
				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusData.TracePayment().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.TraceDataPayment(
					amount, currency, productId, productName, productType, transactionId,
					paymentProcessor, itemsReceived, currencyReceived
				);
				#endif
			}

			public void SetExtraProperty (string key, string value)
			{
				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusData.SetExtraProperty().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.SetDataExtraProperty(key, value);
				#endif
			}

			public void EraseExtraProperty(string key)
			{
				#if UNITY_EDITOR
				Debug.Log ("[FunPlusSDK] FunPlusData.EraseExtraProperty().");
				#elif UNITY_IOS || UNITY_ANDROID
				nativeSdk.EraseDataExtraProperty(key);
				#endif
			}
		}
		#endregion // functional classes
	}
}