# FunPlus SDK for Unity

## Requirements

* Unity 5.3+
* iOS 8.0+
* Android API level 16+

## Example Game

There is an example game inside the `FunPlusSDK/Example` directory. You can open it to see an example on how the FunPlusSDK can be integrated and used.

When you're importing the FunPlus SDK Unity package, you can uncheck the `FunPlusSDK/Example` directory if you don't want to import files under this directory.

## Integration

### Import FunPlus SDK to Your Project

Import the SDK package named `funplus-unity-sdk-<version>.unitypackage` to your project.

The FunPlus SDK is structured in this way:

```shell
Assets/
└── FunPlusSDK/
    ├── Editor/
    ├── Example/
    └── Plugins
        ├── Android/
        └── iOS/
```

### Configure FunPlus SDK

Click the `FunPlusSDK > Edit Config` menu item from the menu bar, fill in all the empty fields in the right side, and then click the `Save Config` button. Please ask the SDK team to get the values.

* Game ID
* Game Key
* RUM Tag
* RUM Key
* Environment

### Install the SDK

First, add the prefab located at `Assets/FunPlusSDK/FunPlusEventListener.prefab` to the first scene of your game.

Then, in the `Start()` method of your game scene, put in the following initializing code:

```csharp
using FunPlus;

FunPlusSDK.Install();
```

### For Exporting Android Project Only

Before exporting, click the `FunPlusSDK > Android Prebuild` menu item from the menu bar.

After exporting, you need to manually add the following the latest Play Services library as project dependency.

* For Android Studio project, it's nothing more than just add a line in the `dependencies` block of project's `build.gradle` file: `compile 'com.google.android.gms:play-services-analytics:9.4.0'`.
* For Eclipse project, you should add the `google-play-services_lib` folder as project dependency. `google-play-services_lib` is part of the Android SDK, which you may already have installed.

## Usage

### The ID Module

The objective of the ID module is to provide a unified ID for each unique user and consequently make it possible to identify users across all FunPlus services (marketing, payment, etc). Note that the ID module can not be treated as an account module, therefore you cannot use this module to complete common account functionalities such as registration and logging in.

Before using its APIs, make sure that you've added `Assets/FunPlusSDK/FunPlusEventListener.prefab` to your game scene.

**Get an FPID based on a given user ID**

```csharp
FunPlusSDK.getFunPlusID().GetFPID("{userid}", ExternalIDType.InAppUserID, onGetFPIDSuccess, onGetFPIDFailure);
```

Here is the definition of `ExternalIDType` and the `Get()` method:

```csharp
namespace FunPlus
{
	public enum ExternalIDType
	{
		InAppUserID,
		Email,
		FacebookID
	}
  
    public class FunPlusID
    {
        public void GetFPID(string externalID,
                            ExternalIDType externalIDType,
                            Action<string> onSuccess,
                            Action<string> onFailure);
    }
}
```

**Bind a new user ID to an existing FPID**

```csharp
FunPlusSDK.getFunPlusID().BindFPID("{fpid}", "{userid}", ExternalIDType.InAppUserID, onBindFPIDSuccess, onBindFPIDFailure);
```

Here is the definition of the `Bind()` method:

```csharp
namespace
{
    public class FunPlusID
    {
        public void BindFPID(string fpid,
                             string externalID,
                             ExternalIDType externalIDType,
                             Action<string> onSuccess,
                             Action<string> onFailure);
    }
}
```

**Get current session ID**

```csharp
string sessionId = FunPlusSDK.GetFunPlusID().GetSessionID();
```

### The RUM Module

The RUM module monitors user's actions in real-time and uploads collected data to Log Agent.

**Trace a `service_monitoring` event**

```csharp
FunPlusSDK.GetFunPlusRUM().TraceServiceMonitoring(...);
```

The `TraceServiceMonitoring()` method is defined as below:

```csharp
/**
    params serviceName:		Name of the service.
    params httpUrl:			Requesting URL of the service.
    params httpStatus:		The response status (can be a string).
    params requestSize:		Size of the request body.
    params responseSize:	Size of the response body.
    params httpLatency:		The request duration (in milliseconds).
    params requestTs:		Requesting timestamp.
    params responseTs:		Responding timestamp.
    params requestId:		Identifier of current request.
    params targetUserId:	User ID.
    params gameServerId:	Game server ID.
 */
public void TraceServiceMonitoring(string serviceName,
                                   string httpUrl,
                                   string httpStatus,
                                   int requestSize,
                                   int responseSize,
                                   long httpLatency,
                                   long requestTs,
                                   long responseTs,
                                   string requestId,
                                   string targetUserId,
                                   string gameServerId)
```

**Set extra properties to RUM events**

Sometimes you might want to attach extra properties to RUM events. You can set string properties by calling the `setExtraProperty()` method. Note that you can set more than one extra property by calling this method multiple times. Once set, these properties will be stored and attached to every RUM events. You can call the `eraseExtraProperty()` to erase one property.

```csharp
FunPlusSDK.GetFunPlusRUM().SetExtraProperty(key, value);
FunPlusSDK.GetFunPlusRUM().EraseExtraProperty(key);
```

### The Data Module

The Data module traces client events and uploads them to FunPlus BI System.

The SDK traces following KPI events automatically:

* session_start
* session_end
* new_user
* payment

**Trace custom events**

```csharp
FunPlusSDK.GetFunPlusData().TraceCustom(event)
```

Besides those four KPI events, you might want to trace some custom events. Call the `TraceCustom()` method to achieve this task.

The event you're passing in to this method is a dictionary. Below is an example:

```json
{
    "app_id": "{YourAppId}",
    "data_version": "2.0",
    "event": "level_up",
    "user_id": "{UserId}",
    "session_id": "{SessionId}",
    "ts": "{Timestamp(millisecond)}",
    "properties": {
        "app_version": "{YourAppId}",
        "os": "{android or ios}",
        "os_version": "{OsVersion}",
        "device": "{DeviceName}",
        "lang": "{LanguageCode, for example: 'en'}",
        "install_ts": "{Timestamp(millisecond)}",
        // Other custom properties.
    }
```

It might not be easy for Unity developers to retrieve system and hardware information. In such a case, you can pick an alternative method:

```csharp
FunPlusSDK.GetFunPlusData().TraceCustomEventWithNameAndProperties(string eventName, Dictionary<string, object> properties);
```

**Set extra properties to Data events**

```csharp
FunPlusSDK.GetFunPlusData().SetExtraProperty(key, value);
FunPlusSDK.GetFunPlusData().EraseExtraProperty(key);
```

## FAQ

**Q: Why the hell is the parameter list of  `TraceServiceMonitoring()` so long?**

A: Please consult RUM team on that :)