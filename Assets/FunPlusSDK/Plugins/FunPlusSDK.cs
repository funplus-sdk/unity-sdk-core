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

		public static FunPlusSDK getInstance () {
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

		public void install () {
			string appId = FunPlusSDKConfig.Instance.AppId;
			string appKey = FunPlusSDKConfig.Instance.AppKey;
			string environment = FunPlusSDKConfig.Instance.Environment;

			#if UNITY_IOS || UNITY_ANDROID
			nativeSdk.install(appId, appKey, environment);
			#endif
		}
	}

}