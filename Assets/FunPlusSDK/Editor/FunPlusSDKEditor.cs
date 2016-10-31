using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using System.Xml;
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
		EditorGUILayout.HelpBox ("3) And that's it!", MessageType.None);

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

	#if UNITY_EDITOR
	[MenuItem("FunPlusSDK/Android Prebuild")]
	public static void AndroidPrebuild()
	{
		RunPreBuildTasksAndroid ();
		EditorUtility.DisplayDialog ("FunPlusSDK", "Android pre-build process completed.", "OK");
	}
	#endif

	private static void RunPreBuildTasksAndroid() {
		bool isFunPlusManifestUsed = false;
		string androidPluginsPath = Path.Combine (Application.dataPath, "Plugins/Android");
		string appManifestPath = Path.Combine (Application.dataPath, "Plugins/Android/AndroidManifest.xml");
		string sdkManifestPath = Path.Combine (Application.dataPath, "FunPlusSDK/Plugins/Android/FunPlusAndroidManifest.xml");
		string appAssetsPath = Path.Combine (Application.dataPath, "Plugins/Android/assets");
		string appConfigPath = Path.Combine (Application.dataPath, "Plugins/Android/assets/funsdk-default-config.json");
		string sdkConfigPath = Path.Combine (Application.dataPath, "FunPlusSDK/Plugins/Android/funsdk-default-config.json");

		// Check if user has already created AndroidManifest.xml file in its location.
		// If not, use already predefined FunPlusAndroidManifest.xml as default one.
		if (!File.Exists(appManifestPath)) {
			if (!Directory.Exists(androidPluginsPath)) {
				Directory.CreateDirectory(androidPluginsPath);
			}

			isFunPlusManifestUsed = true;
			File.Copy(sdkManifestPath, appManifestPath);

			UnityEngine.Debug.Log("[FunPlusSDK] User defined AndroidManifest.xml file not found in Plugins/Android folder.");
			UnityEngine.Debug.Log("[FunPlusSDK] Creating default app's AndroidManifest.xml from FunPlusAndroidManifest.xml file.");
		} else {
			UnityEngine.Debug.Log("[FunPlusSDK] User defined AndroidManifest.xml file located in Plugins/Android folder.");
		}
			
		// Copy the funsdk-default-config.json to project's assets folder.
		if (!Directory.Exists (appAssetsPath)) {
			Directory.CreateDirectory (appAssetsPath);

			UnityEngine.Debug.Log ("[FunPlusSDK] Createing app's assets folder because it does not exist");
		}

		if (File.Exists (appConfigPath)) {
			UnityEngine.Debug.Log ("[FunPlusSDK] Not copy the default config file to app's assets folder because app already has one");
		} else {
			File.Copy (sdkConfigPath, appConfigPath);

			UnityEngine.Debug.Log ("[FunPlusSDK] Copying the default config file to app's assets folder");
		}

		// If FunPlus manifest is used, we have already set up everything in it so that 
		// our native Android SDK can be used properly.
		if (!isFunPlusManifestUsed) {
			// However, if you already had your own AndroidManifest.xml, we'll now run
			// some checks on it and tweak it a bit if needed to add some stuff which
			// our native Android SDK needs so that it can run properly.

			// Let's open the app's AndroidManifest.xml file.
			XmlDocument manifestFile = new XmlDocument();
			manifestFile.Load(appManifestPath);

			// Add needed permissions if they are missing.
			AddPermissions(manifestFile);

			// Add intent filter to main activity if it is missing.
			AddBroadcastReceiver(manifestFile);

			// Save the changes.
			manifestFile.Save(appManifestPath);

			// Clean the manifest file.
			CleanManifestFile(appManifestPath);

			UnityEngine.Debug.Log("[FunPlusSDK] App's AndroidManifest.xml file check and potential modification completed.");
			UnityEngine.Debug.Log("[FunPlusSDK] Please check if any error message was displayed during this process " 
				+ "and make sure to fix all issues in order to properly use the FunPlus SDK in your app.");
		}
	}

	private static void AddPermissions(XmlDocument manifest) {
		// The FunPlus SDK needs three permissions to be added to you app's manifest file:
		// <uses-permission android:name="android.permission.INTERNET" />
		// <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
		// <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE"/>

		UnityEngine.Debug.Log("[FunPlusSDK] Checking if all permissions needed for the FunPlus SDK are present in the app's AndroidManifest.xml file.");

		bool hasInternetPermission = false;
		bool hasAccessWifiStatePermission = false;
		bool hasAccessNetworkStatePermission = false;

		XmlElement manifestRoot = manifest.DocumentElement;

		// Check if permissions are already there.
		foreach (XmlNode node in manifestRoot.ChildNodes) {
			if (node.Name == "uses-permission") {
				foreach (XmlAttribute attribute in node.Attributes) {
					if (attribute.Value.Contains ("android.permission.INTERNET")) {
						hasInternetPermission = true;
					} else if (attribute.Value.Contains ("android.permission.ACCESS_WIFI_STATE")) {
						hasAccessWifiStatePermission = true;
					} else if (attribute.Value.Contains ("android.permission.ACCESS_NETWORK_STATE")) {
						hasAccessNetworkStatePermission = true;
					}
				}
			}
		}

		// If android.permission.INTERNET permission is missing, add it.
		if (!hasInternetPermission) {
			XmlElement element = manifest.CreateElement("uses-permission");
			element.SetAttribute("android__name", "android.permission.INTERNET");
			manifestRoot.AppendChild(element);

			UnityEngine.Debug.Log("[FunPlusSDK] android.permission.INTERNET permission successfully added to your app's AndroidManifest.xml file.");
		} else {
			UnityEngine.Debug.Log("[FunPlusSDK] Your app's AndroidManifest.xml file already contains android.permission.INTERNET permission.");
		}

		// If android.permission.ACCESS_WIFI_STATE permission is missing, add it.
		if (!hasAccessWifiStatePermission) {
			XmlElement element = manifest.CreateElement("uses-permission");
			element.SetAttribute("android__name", "android.permission.ACCESS_WIFI_STATE");
			manifestRoot.AppendChild(element);

			UnityEngine.Debug.Log("[FunPlusSDK] android.permission.ACCESS_WIFI_STATE permission successfully added to your app's AndroidManifest.xml file.");
		} else {
			UnityEngine.Debug.Log("[FunPlusSDK] Your app's AndroidManifest.xml file already contains android.permission.ACCESS_WIFI_STATE permission.");
		}

		// If android.permission.ACCESS_NETWORK_STATE permission is missing, add it.
		if (!hasAccessNetworkStatePermission) {
			XmlElement element = manifest.CreateElement("uses-permission");
			element.SetAttribute("android__name", "android.permission.ACCESS_NETWORK_STATE");
			manifestRoot.AppendChild(element);

			UnityEngine.Debug.Log("[FunPlusSDK] android.permission.ACCESS_NETWORK_STATE permission successfully added to your app's AndroidManifest.xml file.");
		} else {
			UnityEngine.Debug.Log("[FunPlusSDK] Your app's AndroidManifest.xml file already contains android.permission.ACCESS_NETWORK_STATE permission.");
		}
	}

	private static void AddBroadcastReceiver(XmlDocument manifest) {
		// We're looking for existance of broadcast receiver in the AndroidManifest.xml
		// Check out the example below how that usually looks like:

		// <manifest
		//     <!-- ... -->>
		// 
		//     <supports-screens
		//         <!-- ... -->/>
		// 
		//     <application
		//         <!-- ... -->>
		//         <receiver
		//             android:name="com.adjust.sdk.AdjustReferrerReceiver"
		//             android:exported="true" >
		//             
		//             <intent-filter>
		//                 <action android:name="com.android.vending.INSTALL_REFERRER" />
		//             </intent-filter>
		//         </receiver>
		//
		//		   <receiver android:name="com.funplus.sdk.ConnectionChangeReceiver" android:label="NetworkConnection">
		//			   <intent-filter>
		//			       <action android:name="android.net.conn.CONNECTIVITY_CHANGE"/>
		//			   </intent-filter>
		//		   </receiver>
		//         
		//         <activity android:name="com.unity3d.player.UnityPlayerActivity"
		//             <!-- ... -->
		//         </activity>
		//     </application>
		//
		//	   <!-- ... -->> 
		// </manifest>

		UnityEngine.Debug.Log("[FunPlusSDK] Checking if app's AndroidManifest.xml file contains receiver for INSTALL_REFERRER intent.");

		XmlElement manifestRoot = manifest.DocumentElement;
		XmlNode applicationNode = null;

		// Let's find the application node.
		foreach(XmlNode node in manifestRoot.ChildNodes) {
			if (node.Name == "application") {
				applicationNode = node;
				break;
			}
		}

		// If there's no applicatio node, something is really wrong with your AndroidManifest.xml.
		if (applicationNode == null) {
			UnityEngine.Debug.LogError("[FunPlusSDK] Your app's AndroidManifest.xml file does not contain \"<application>\" node.");
			UnityEngine.Debug.LogError("[FunPlusSDK] Unable to add the adjust broadcast receiver to AndroidManifest.xml.");

			return;
		}

		// Okay, there's an application node in the AndroidManifest.xml file.
		// Let's now check if user has already defined a receiver which is listening to INSTALL_REFERRER intent.
		// If that is already defined, don't force the adjust broadcast receiver to the manifest file.
		// If not, add the adjust broadcast receiver to the manifest file.
		bool isThereAnyInstallReferrerBroadcastReiver = false;

		foreach (XmlNode node in applicationNode.ChildNodes) {
			if (node.Name == "receiver") {
				foreach (XmlNode subnode in node.ChildNodes) {
					if (subnode.Name == "intent-filter") {
						foreach (XmlNode subsubnode in subnode.ChildNodes) {
							if (subsubnode.Name == "action") {
								foreach(XmlAttribute attribute in subsubnode.Attributes) {
									if (attribute.Value.Contains("INSTALL_REFERRER")) {
										isThereAnyInstallReferrerBroadcastReiver = true;
										break;
									}
								}
							}

							if (isThereAnyInstallReferrerBroadcastReiver) {
								break;
							}
						}
					}

					if (isThereAnyInstallReferrerBroadcastReiver) {
						break;
					}
				}
			}

			if (isThereAnyInstallReferrerBroadcastReiver) {
				break;
			}
		}

		// Let's see what we have found so far.
		if (isThereAnyInstallReferrerBroadcastReiver) {
			UnityEngine.Debug.Log("[FunPlusSDK] It seems like you are using your own install referrer broadcast receiver.");
			UnityEngine.Debug.Log("[FunPlusSDK] Please, add the calls to the adjust install referrer broadcast receiver like described in here: https://github.com/adjust/android_sdk/blob/master/doc/english/referrer.md");
		} else {
			// Generate adjust broadcast receiver entry and add it to the application node.
			XmlElement receiverElement = manifest.CreateElement("receiver");
			receiverElement.SetAttribute("android__name", "com.adjust.sdk.AdjustReferrerReceiver");
			receiverElement.SetAttribute("android__exported", "true");

			XmlElement intentFilterElement = manifest.CreateElement("intent-filter");
			XmlElement actionElement = manifest.CreateElement("action");
			actionElement.SetAttribute("android__name", "com.android.vending.INSTALL_REFERRER");

			intentFilterElement.AppendChild(actionElement);
			receiverElement.AppendChild(intentFilterElement);
			applicationNode.AppendChild(receiverElement);

			UnityEngine.Debug.Log("[FunPlusSDK] Adjust install referrer broadcast receiver successfully added to your app's AndroidManifest.xml file.");
		}

		// Let's now move forward and check if user has already defined a receiver which is listening
		// to CONNECTIVITY_CHANGE intent. If that is already defined, don't force the FunPlus broadcast
		// receiver to the manifest file. If not, add the FunPlus broadcast receiver to the manifest file.
		bool isThereAnyConnectivityChangeBroadcastReiver = false;

		foreach (XmlNode node in applicationNode.ChildNodes) {
			if (node.Name == "receiver") {
				foreach (XmlNode subnode in node.ChildNodes) {
					if (subnode.Name == "intent-filter") {
						foreach (XmlNode subsubnode in subnode.ChildNodes) {
							if (subsubnode.Name == "action") {
								foreach(XmlAttribute attribute in subsubnode.Attributes) {
									if (attribute.Value.Contains("CONNECTIVITY_CHANGE")) {
										isThereAnyInstallReferrerBroadcastReiver = true;
										break;
									}
								}
							}

							if (isThereAnyConnectivityChangeBroadcastReiver) {
								break;
							}
						}
					}

					if (isThereAnyConnectivityChangeBroadcastReiver) {
						break;
					}
				}
			}

			if (isThereAnyConnectivityChangeBroadcastReiver) {
				break;
			}
		}

		// Let's see what we have found so far.
		if (isThereAnyConnectivityChangeBroadcastReiver) {
			UnityEngine.Debug.Log("[FunPlusSDK] It seems like you are using your own connectivity change broadcast receiver.");
			UnityEngine.Debug.Log("[FunPlusSDK] Please, add the calls to the FunPlus connectivity change broadcast receiver.");
		} else {
			// Generate adjust broadcast receiver entry and add it to the application node.
			XmlElement receiverElement = manifest.CreateElement("receiver");
			receiverElement.SetAttribute("android__name", "com.adjust.sdk.com.funplus.sdk.ConnectionChangeReceiver");
			receiverElement.SetAttribute("android__label", "NetworkConnection");

			XmlElement intentFilterElement = manifest.CreateElement("intent-filter");
			XmlElement actionElement = manifest.CreateElement("action");
			actionElement.SetAttribute("android__name", "com.android.vending.CONNECTIVITY_CHANGE");

			intentFilterElement.AppendChild(actionElement);
			receiverElement.AppendChild(intentFilterElement);
			applicationNode.AppendChild(receiverElement);

			UnityEngine.Debug.Log("[FunPlusSDK] FunPlus connectivity change broadcast receiver successfully added to your app's AndroidManifest.xml file.");
		}
	}

	private static void CleanManifestFile(String manifestPath) {
		// Due to XML writing issue with XmlElement methods which are unable
		// to write "android:[param]" string, we have wrote "android__[param]" string instead.
		// Now make the replacement: "android:[param]" -> "android__[param]"

		TextReader manifestReader = new StreamReader(manifestPath);
		string manifestContent = manifestReader.ReadToEnd();
		manifestReader.Close();

		Regex regex = new Regex("android__");
		manifestContent = regex.Replace(manifestContent, "android:");

		TextWriter manifestWriter = new StreamWriter(manifestPath);
		manifestWriter.Write(manifestContent);
		manifestWriter.Close();
	}
}