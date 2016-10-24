using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FunPlus
{
	public class FunPlusSDK
	{
		#if UNITY_IOS || UNITY_ANDROID
		private static FunPlusSDK instance = null;
		#endif

		#if UNITY_IOS
		private static FunPlusiOS nativeSdk = null;
		#elif UNITY_ANDROID
		private static FunPlusAndroid nativeSdk = null;
		#endif

		private FunPlusSDK() {}

		public static void Install ()
		{
			#if UNITY_IOS || UNITY_ANDROID
			if (instance != null)
			{
				return;
			}

			instance = new FunPlusSDK ();

			#if UNITY_IOS
			nativeSdk = new FunPlusiOS ();
			#elif UNITY_ANDROID
			nativeSdk = new FunPlusAndroid ();
			#endif

			string appId = FunPlusSDKConfig.Instance.AppId;
			string appKey = FunPlusSDKConfig.Instance.AppKey;
			string environment = FunPlusSDKConfig.Instance.Environment;

			nativeSdk.Install(appId, appKey, environment);
			#endif
		}

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

		public static FunPlusAdjust GetFunPlusAdjust()
		{
			return FunPlusAdjust.GetInstance ();
		}

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
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.TraceRUMServiceMonitoring(
					serviceName, httpUrl, httpStatus, requestSize, responseSize, httpLatency, requestTs,
					responseTs, requestId, targetUserId, gameServerId
				);
				#endif
			}

			public void SetExtraProperty (string key, string value)
			{
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.SetRUMExtraProperty(key, value);
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
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.TraceDataCustom(dataEvent);
				#endif
			}

//			public void TracePayment(string amount,
//				                     string currency,
//				                     string productId,
//				                     string productName,
//				                     string productType,
//				                     string transactionId,
//				                     string paymentProcessor,
//									 string itemsReceived,
//				                     string currencyReceived)
//			{
//				#if UNITY_IOS || UNITY_ANDROID
//				nativeSdk.TraceDataPayment(
//					amount, currency, productId, productName, productType, transactionId,
//					paymentProcessor, itemsReceived, currencyReceived
//				);
//				#endif
//			}

			public void SetExtraProperty (string key, string value)
			{
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.SetDataExtraProperty(key, value);
				#endif
			}
		}

		public class FunPlusAdjust
		{
			private static FunPlusAdjust instance;

			private FunPlusAdjust() {}

			public static FunPlusAdjust GetInstance()
			{
				if (instance == null) {
					instance = new FunPlusAdjust ();
				}
				return instance;
			}
		}
	}
}