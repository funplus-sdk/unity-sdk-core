using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using FunPlus;

public class FunPlusConsoleEndpoint
{
	private const string FunPlusPath = "Assets/FunPlusSDK/";

	public static void ExportPackage()
	{
		Debug.Log("Exporting FunPlus SDK Unity Package...");
		string path = OutputPath;
		Debug.Log ("XXXXX " + path);

		try
		{
			if (!File.Exists(Path.Combine(Application.dataPath, "Temp")))
			{
				AssetDatabase.CreateFolder("Assets", "Temp");
			}

			AssetDatabase.MoveAsset(FunPlusPath + "Resources/FunPlusSDKConfig.asset", "Assets/Temp/FunPlusSDKConfig.asset");

			string[] files = (string[])Directory.GetFiles(FunPlusPath, "*.*", SearchOption.AllDirectories);

			AssetDatabase.ExportPackage(
				files,
				path,
				ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
		}
		finally
		{
			// Move files back no matter what
			AssetDatabase.MoveAsset("Assets/Temp/FunPlusSDKConfig.asset", FunPlusPath + "Resources/FunPlusSDKConfig.asset");
			AssetDatabase.DeleteAsset("Assets/Temp");
		}

		Debug.Log("Finished exporting!");
	}

	private static string OutputPath
	{
		get
		{
			string projectRoot = Directory.GetCurrentDirectory();
			var outputDirectory = new DirectoryInfo(Path.Combine(projectRoot, "Release"));

			// Create the directory if it doesn't exist
			outputDirectory.Create();

			string packageName = string.Format("funplus-unity-sdk-{0}.unitypackage", FunPlusSDK.VERSION);
			return Path.Combine(outputDirectory.FullName, packageName);
		}
	}
}