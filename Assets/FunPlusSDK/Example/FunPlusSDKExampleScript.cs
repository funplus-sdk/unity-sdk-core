using UnityEngine;
using System.Collections;
using FunPlus;

public class FunPlusSDKExampleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FunPlusSDK.Install ();

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

		FunPlusSDK.GetFunPlusRUM ().SetExtraProperty ("rum_extra_key", "rum_extra_value");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
