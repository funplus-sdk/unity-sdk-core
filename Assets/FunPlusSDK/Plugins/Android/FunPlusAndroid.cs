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
	public class FunPlusAndroid : IWorkerMethodDispacther {

		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
		}

		public void install(string appId, string appKey, string environment)
		{
		}
	}
}
//#endif