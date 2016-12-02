using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FunPlus;

public class UIManagerScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		FunPlusSDK.Install ();

		TestRUMTraceServiceMonitoring ();

		TestRUMSetExtraProperty ();

		TestTraceCustomEventWithNameAndProperties ();

		TestGetFPID ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void TestRUMTraceServiceMonitoring()
	{
		Debug.Log ("xxxxx Test RUM.TraceServiceMonitoring xxxxx");

		string serviceName = "httpbin";
		string httpUrl = "https://www.httpbin.org";
		string httpStatus = "200";
		int requestSize = 100;
		int responseSize = 200;
		long httpLatency = 300;
		long requestTs = 0;
		long responseTs = 300;
		string requestId = "test_request_id";
		string targetUserId = "test_user";
		string gameServerId = "test_game_server_id";

		FunPlusSDK.GetFunPlusRUM ().TraceServiceMonitoring (
			serviceName, httpUrl, httpStatus, requestSize, responseSize, httpLatency,
			requestTs, responseTs, requestId, targetUserId, gameServerId
		);

		Debug.Log ("xxxxx End Test xxxxx");
	}

	void TestRUMSetExtraProperty()
	{
		Debug.Log ("xxxxx Test RUM.SetExtraProperty xxxxx");

		FunPlusSDK.GetFunPlusRUM ().SetExtraProperty ("rum_extra_key", "rum_extra_value");

		Debug.Log ("xxxxx End Test xxxxx");
	}

	void TestTraceCustomEventWithNameAndProperties()
	{
		Debug.Log ("xxxxx Test Data.TraceCustomEventWithNameAndProperties xxxxx");

		string eventName = "level_up";

		Dictionary<string, object> info = new Dictionary<string, object> ();
		info.Add ("old", 17);
		info.Add ("new", 23);
		info.Add ("uid", "testuser");

		Dictionary<string, object> properties = new Dictionary<string, object> ();
		properties.Add ("info", info);
		properties.Add ("bonus", "17 gold");

		FunPlusSDK.GetFunPlusData ().TraceCustomEventWithNameAndProperties (eventName, properties);

		Debug.Log ("xxxxx End Test xxxxx");
	}

	void TestGetFPID()
	{
		Debug.Log ("xxxxx Test ID.GetFPID xxxxx");

		FunPlusSDK.GetFunPlusID ().GetFPID ("testuser", ExternalIDType.InAppUserID, OnGetFPIDSuccess, OnGetFPIDFailure);

		string sessionId = FunPlusSDK.GetFunPlusID ().GetSessionId ();
		Debug.Log ("Session ID: " + sessionId);

		Debug.Log ("xxxxx End Test xxxxx");
	}

	void OnGetFPIDSuccess(string fpid)
	{
		Debug.Log ("Get FPID success: " + fpid);
	}

	void OnGetFPIDFailure(string error)
	{
		Debug.Log ("Get FPID failed: " + error);
	}
}
