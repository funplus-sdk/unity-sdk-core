using UnityEngine;
using System.Collections;

namespace FunPlusSDK
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

		public static FunPlusSDK getInstance ()
		{
			#if UNITY_IOS || UNITY_ANDROID
			if(instance == null) {
				instance = new FunPlusSDK();
			#if UNITY_IOS
				nativeSdk = new FunPlusiOS();
			#elif UNITY_ANDROID
				nativeSdk = new FunPlusAndroid();
			#endif
			}
			return instance;
			#else
			return null;
			#endif
		}

		public void install ()
		{
			string appId = FunPlusSDKConfig.Instance.AppId;
			string appKey = FunPlusSDKConfig.Instance.AppKey;
			string environment = FunPlusSDKConfig.Instance.Environment;

			#if UNITY_IOS || UNITY_ANDROID
			nativeSdk.install(appId, appKey, environment);
			#endif
		}

		public FunPlusID getFunPlusID()
		{
			return FunPlusID.getInstance ();
		}
			
		public FunPlusRUM getFunPlusRUM()
		{
			return FunPlusRUM.getInstance ();
		}

		public FunPlusData getFunPlusData()
		{
			return FunPlusData.getInstance ();
		}

		public FunPlusAdjust getFunPlusAdjust()
		{
			return FunPlusAdjust.getInstance ();
		}

		public class FunPlusID
		{
			private static FunPlusID instance;

			private FunPlusID() {}

			public static FunPlusID getInstance()
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

			public static FunPlusRUM getInstance()
			{
				if (instance == null) {
					instance = new FunPlusRUM ();
				}
				return instance;
			}

			public void traceServiceMonitoring(
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
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.traceRUMServiceMonitoring(
					serviceName,
					httpUrl,
					httpStatus,
					requestSize,
					responseSize,
					httpLatency,
					requestTs,
					responseTs,
					requestId,
					targetUserId,
					gameServerId
				);
				#endif
			}

			public void setExtraProperty (string key, string value)
			{
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.setRUMExtraProperty(key, value);
				#endif
			}
		}

		public class FunPlusData
		{
			private static FunPlusData instance;

			private FunPlusData() {}

			public static FunPlusData getInstance()
			{
				if (instance == null) {
					instance = new FunPlusData ();
				}
				return instance;
			}

			public void traceCustom(IDictionary dataEvent)
			{
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.traceDataCustom(dataEvent);
				#endif
			}

			public void tracePayment(
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
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.traceDataPayment(
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
				#endif
			}

			public void setExtraProperty (string key, string value)
			{
				#if UNITY_IOS || UNITY_ANDROID
				nativeSdk.setDataExtraProperty(key, value);
				#endif
			}
		}

		public class FunPlusAdjust
		{
			private static FunPlusAdjust instance;

			private FunPlusAdjust() {}

			public static FunPlusAdjust getInstance()
			{
				if (instance == null) {
					instance = new FunPlusAdjust ();
				}
				return instance;
			}
		}
	}
}