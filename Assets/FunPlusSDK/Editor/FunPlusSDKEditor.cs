using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEditor.Callbacks;

[CustomEditor(typeof(FunPlusSDKConfig))]
public class FunPlusSDKEditor : Editor
{
	GUIContent appIdLabel = new GUIContent("App ID [?]:", "FunPlus app ID can be found at:");
	GUIContent appKeyLabel = new GUIContent("App Key [?]:", "FunPlus app key\n For example, 'xxx'");
	GUIContent environmentLabel = new GUIContent("SDK Environment [?]:", "SDK running environment\n Can take two possible values: sandbox or production");

	public override void OnInspectorGUI () {

		FunPlusSDKConfig sdkConfig = FunPlusSDKConfig.Instance;

		EditorGUILayout.LabelField ("FunPlus SDK Configurations");
		EditorGUILayout.HelpBox ("1) Fill in the following fields.", MessageType.None);
		EditorGUILayout.HelpBox ("2) Click the `Save Config` button.", MessageType.None);
		EditorGUILayout.HelpBox ("3) Run from menu: FunPlusSDK/Fix AndroidManifest.", MessageType.None);
		EditorGUILayout.HelpBox ("4) And that's it!", MessageType.None);

		EditorGUILayout.Space ();
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

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button ("Save Config")) {
			sdkConfig.SaveConfig();
		}
		EditorGUILayout.EndHorizontal();
	}

	[MenuItem ("FunPlusSDK/Fix AndroidManifest")]
	static void FixAndroidManifest ()
	{
		#if UNITY_ANDROID
		var exitCode = RunPostBuildScript (preBuild: true);

		if (exitCode == 1)
		{
			EditorUtility.DisplayDialog (
				"FunPlusSDK", 
				string.Format("AndroidManifest.xml changed or created at {0}/Plugins/Android/ .", Application.dataPath),
				"OK"
			);
		}
		else if (exitCode == 0)
		{
			EditorUtility.DisplayDialog ("FunPlusSDK", "AndroidManifest.xml did not needed to be changed.", "OK");
		}
		else
		{
			EditorUtility.DisplayDialog ("FunPlusSDK", GenerateErrorScriptMessage (exitCode), "OK");
		}
		#else
		EditorUtility.DisplayDialog ("FunPlusSDK", "Option only valid for the Android platform.", "OK");
		#endif
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject)
	{
		var exitCode = RunPostBuildScript (preBuild: false, pathToBuiltProject: pathToBuiltProject);

		if (exitCode == -1)
		{
			return;
		}

		if (exitCode != 0)
		{
			var errorMessage = GenerateErrorScriptMessage (exitCode);
			UnityEngine.Debug.LogError ("FunPlus: " + errorMessage);
		}
	}

	static int RunPostBuildScript(bool preBuild, string pathToBuiltProject = "")
	{
		string resultContent;
		string arguments = null;
		string pathToScript = null;

		string filePath = Path.Combine (Environment.CurrentDirectory, "Assets/FunPlusSDK/Editor/FunPlusPostBuildiOS.py");

		// Check if Unity is running on Windows operating system.
		// If yes - fix line endings in python scripts.
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			UnityEngine.Debug.Log ("Windows platform");

			using (System.IO.StreamReader streamReader = new System.IO.StreamReader (filePath))
			{
				string fileContent = streamReader.ReadToEnd ();
				resultContent = Regex.Replace (fileContent, @"\r\n|\n\r|\n|\r", "\r\n");
			}

			if (File.Exists (filePath))
			{
				File.WriteAllText (filePath, resultContent);
			}
		}
		else
		{
			UnityEngine.Debug.Log ("Unix platform");

			using (System.IO.StreamReader streamReader = new System.IO.StreamReader (filePath))
			{
				string replaceWith = "\n";
				string fileContent = streamReader.ReadToEnd ();

				resultContent = fileContent.Replace ("\r\n", replaceWith);
			}

			if (File.Exists (filePath))
			{
				File.WriteAllText (filePath, resultContent);
			}
		}

		#if UNITY_ANDROID
		pathToScript = Path.Combine (Application.dataPath, "FunPlusSDK/Editor/FunPlusPostBuildAndroid.py");
		arguments = "\"" + Application.dataPath + "\"";

		if (preBuild)
		{
			arguments = "--pre-build " + arguments;
		}
		#elif UNITY_IOS
		string configFile = Path.Combine(Application.dataPath, "FunPlusSDK/Plugins/iOS/funsdk-default-config.plist");
		pathToScript = Path.Combine(Application.dataPath, "FunPlusSDK/Editor/FunPlusPostBuildiOS.py");
		arguments = "\"" + pathToBuiltProject + "\" \"" + configFile + "\"";
		#else
		return -1;
		#endif

		Process proc = new Process ();
		proc.EnableRaisingEvents = false; 
		proc.StartInfo.FileName = "python";
		proc.StartInfo.Arguments = pathToScript + " " + arguments;
		proc.StartInfo.UseShellExecute = false;
		proc.StartInfo.RedirectStandardOutput = true;
		proc.Start ();
		proc.WaitForExit ();

		return proc.ExitCode;
	}

	static string GenerateErrorScriptMessage (int exitCode)
	{
	#if UNITY_ANDROID
	if (exitCode == 1)
	{
		return "The AndroidManifest.xml file was only changed or created after building the package. " +
			"PLease build again the Android Unity package so it can use the new file";
		}  
	#endif

	if (exitCode != 0)
	{
		var message = "Build script exited with error." +
			" Please check the FunPlus log file for more information at {0}";
		string projectPath = Application.dataPath.Substring (0, Application.dataPath.Length - 7);
		string logFile = null;

		#if UNITY_ANDROID
		logFile = projectPath + "/FunPlusPostBuildAndroidLog.txt";
		#elif UNITY_IOS
		logFile = projectPath + "/FunPluPostBuildiOSLog.txt";
		#else
		return null;
		#endif
		return string.Format (message, logFile);
		} 

		return null;
	}
}