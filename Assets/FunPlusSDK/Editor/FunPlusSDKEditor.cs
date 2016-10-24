using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FunPlusSDKConfig))]
public class FunPlusSDKEditor : Editor
{
	GUIContent appIdLabel = new GUIContent("App ID [?]:", "FunPlus app ID can be found at:");
	GUIContent appKeyLabel = new GUIContent("App Key [?]:", "FunPlus app key\n For example, 'xxx'");
	GUIContent environmentLabel = new GUIContent("SDK Environment [?]:", "SDK running environment\n Can take two possible values: sandbox or production");

	public override void OnInspectorGUI () {

		FunPlusSDKConfig sdkConfig = FunPlusSDKConfig.Instance;

		EditorGUILayout.LabelField ("Installation Configurations");
		EditorGUILayout.HelpBox ("1) Add the game object which will respond to SDK callbacks", MessageType.None);
		EditorGUILayout.HelpBox ("2)", MessageType.None);
		EditorGUILayout.HelpBox ("3)", MessageType.None);

		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (appIdLabel);
		sdkConfig.AppId = EditorGUILayout.TextField (sdkConfig.AppId);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (appKeyLabel);
		sdkConfig.AppKey = EditorGUILayout.TextField (sdkConfig.AppKey);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (environmentLabel);
		sdkConfig.Environment = EditorGUILayout.TextField (sdkConfig.Environment);
		EditorGUILayout.EndHorizontal ();
	}
}
