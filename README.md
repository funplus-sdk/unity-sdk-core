# FunPlus SDK for Unity

## Requirements

* Unity 5.3+

## Example Game

There is an example game inside the `FunPlusSDK/Example` directory. You can open it to see an example on how the FunPlusSDK can be integrated and used.

## Integration

### Import FunPlus SDK to Your Project

Import the SDK package named `funplus-unity-sdk-<version>.unitypackage` to your project.

The FunPlus SDK is structured in this way:

```shell
Assets/
└── FunPlusSDK/
    ├── Editor/
    ├── Example/
    ├── Example.meta
    └── Plugins
        ├── Android/
        ├── iOS/
        └── iOS.meta
```

### Configure FunPlus SDK

Click the `FunPlusSDK > Edit Config` menu item from the menu bar, fill in all the empty fields in the right side, and then click the `Save Config` button. Please ask the SDK team to get the values.

* Game ID
* Game Key
* Environment

### Install the SDK

In the `Start()` method of your game scene, put in the following initializing code:

```csharp
FunPlusSDK.Install();
```

### For Exporting Android Project Only

* Before exporting , you need to modify the manifest file by clicking the `FunPlusSDK > Fix AndroidManifest` menu item.
* After exporting, you need to manually add the following the play services library as project dependency.
  * For Eclipse:
  * For Android Studio:

## Usage

### The ID Module

### The RUM Module

To trace a `service_monitoring` event.

```csharp
FunPlusSDK.GetFunPlusRUM().TraceServiceMonitoring(...);
```

Below is the signature of the `TraceServiceMonitoring` method.

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

### The Data Module