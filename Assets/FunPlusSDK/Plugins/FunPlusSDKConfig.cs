﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using HSMiniJSON;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
[System.Serializable]
public class FunPlusSDKConfig : ScriptableObject
{
	private static FunPlusSDKConfig instance;
	private const string configAssetName = "FunPlusSDKConfig";
	private const string configPath = "FunPlusSDK/Resources";

	[SerializeField]
	private string appId;
	[SerializeField]
	private string appKey;
	[SerializeField]
	private string environment;

	public static FunPlusSDKConfig Instance
	{
		get
		{
			instance = Resources.Load(configAssetName) as FunPlusSDKConfig;
			if (instance == null) {
				instance = CreateInstance<FunPlusSDKConfig>();
				#if UNITY_EDITOR
				string properPath = Path.Combine(Application.dataPath, configPath);
				if (!Directory.Exists(properPath))
				{
					AssetDatabase.CreateFolder("Assets/FunPlusSDK", "Resources");
				}

				string fullPath = Path.Combine(Path.Combine("Assets", configPath), configAssetName + ".asset");
				AssetDatabase.CreateAsset(instance, fullPath);
				#endif
			}
			return instance;
		}
	}

	public string AppId
	{
		get { return appId; }
		set
		{
			if (appId != value)
			{
				appId = value;
			}
		}
	}

	public string AppKey
	{
		get { return appKey; }
		set
		{
			if (appKey != value)
			{
				appKey = value;
			}
		}
	}

	public string Environment
	{
		get { return environment; }
		set
		{
			if (environment != value)
			{
				environment = value;
			}
		}
	}

	#if UNITY_EDITOR
	[MenuItem("FunPlusSDK/Edit Config")]
	public static void Edit()
	{
		Selection.activeObject = Instance;
	}

	[MenuItem("FunPlusSDK/Developers Page")]
	public static void OpenAppPage()
	{
		string url = "https://developer.funplus.com/";
		Application.OpenURL(url);
	}

	[MenuItem("FunPlusSDK/Report an SDK Bug")]
	public static void ReportABug()
	{
		string url = "mailto:dis.red@funplus.com";
		Application.OpenURL(url);
	}
	#endif
}