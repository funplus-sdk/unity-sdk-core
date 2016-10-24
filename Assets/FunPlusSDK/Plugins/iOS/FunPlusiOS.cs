//#if UNITY_IPHONE
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HSMiniJSON;

namespace FunPlusSDK
{
	public class FunPlusiOS
	{
		[DllImport ("__Internal")]
		private static extern void fpInstall (string appId, string appKey, string environment);

		public FunPlusiOS () {}

		public void install (string appId, string appKey, string environment) {
			install(appId, appKey, environment);
		}
	}

}
//#endif
