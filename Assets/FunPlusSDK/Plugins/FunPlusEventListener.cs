using UnityEngine;
using System;
using System.Collections;

namespace FunPlus {

	public class FunPlusEventListener : MonoBehaviour
	{

		// Callbacks
		public static Action<string> getFPIDSuccessHandler;
		public static Action<string> getFPIDFailureHandler;
		public static Action<string> bindFPIDSuccessHandler;
		public static Action<string> bindFPIDFailureHandler;

		// Listens to all the events. All event listeners MUST be removed before this object is disposed!
		void OnEnable ()
		{
		}
		
		// Update is called once per frame
		void OnDisable ()
		{
			getFPIDSuccessHandler = null;
			getFPIDFailureHandler = null;
			bindFPIDSuccessHandler = null;
			bindFPIDFailureHandler = null;
		}

		void onGetFPIDSuccess(string fpid)
		{
			Debug.Log ("Get FPID success: " + fpid);

			if (getFPIDSuccessHandler != null)
			{
				getFPIDSuccessHandler (fpid);
			}
		}

		void onGetFPIDFailure(string error)
		{
			Debug.LogError ("Get FPID failed: " + error);

			if (getFPIDFailureHandler != null)
			{
				getFPIDFailureHandler (error);
			}
		}

		void onBindFPIDSuccess(string fpid)
		{
			Debug.Log("Bind FPID success: " + fpid);

			if (bindFPIDSuccessHandler != null)
			{
				bindFPIDSuccessHandler (fpid);
			}
		}

		void onBindFPIDFailure(string error)
		{
			Debug.LogError ("Bind FPID failed: " + error);

			if (bindFPIDFailureHandler != null)
			{
				bindFPIDFailureHandler (error);
			}
		}
	}

}