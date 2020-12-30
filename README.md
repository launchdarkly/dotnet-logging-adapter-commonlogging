# LaunchDarkly Logging API for .NET - Common.Logging and log4net Adapters

[![CircleCI](https://circleci.com/gh/launchdarkly/dotnet-logging-adapters/tree/master.svg?style=svg)](https://circleci.com/gh/launchdarkly/dotnet-logging-adapters/tree/master)
[![Documentation](https://img.shields.io/static/v1?label=GitHub+Pages&message=reference&color=00add8)](https://launchdarkly.github.io/dotnet-logging-adapters)

## Overview

These .NET packages provide integration from the [`LaunchDarkly.Logging`](https://github.com/launchdarkly/dotnet-logging) API that is used by LaunchDarkly [.NET SDK](https://github.com/launchdarkly/dotnet-server-sdk), [Xamarin SDK](https://github.com/launchdarkly/xamarin-client-sdk), and other LaunchDarkly libraries, to the third-party logging frameworks [`Common.Logging`](https://github.com/net-commons/common-logging) and [`log4net`](https://github.com/net-commons/common-logging).

LaunchDarkly tools can run on a variety of .NET platforms, including .NET Core, .NET Framework, and Xamarin. There is no single logging framework that is consistently favored across all of those. For instance, the standard in .NET Core is now `Microsoft.Extensions.Logging`, but in .NET Framework 4.5.x this is not available without bringing in .NET Core assemblies that are normally not used in .NET Framework.

Earlier versions of LaunchDarkly SDKs used the `Common.Logging` framework, which provides adapters to various popular loggers. But writing the LaunchDarkly packages against such a third-party API causes inconvenience for any developer using LaunchDarkly who prefers a different framework, and it is a relatively heavyweight solution for projects that may only have simple logging requirements. The lightweight `LaunchDarkly.Logging` API, whose small feature set is geared toward the needs of LaunchDarkly SDKs, can be integrated with third-party frameworks and also provides several simple logging implementations of its own.

The adapters in this repository are published as separate packages, to avoid unwanted dependencies on `Common.Logging` and `log4net` in the LaunchDarkly SDKs and in applications that do not use those frameworks.

## Usage: `Common.Logging`

Like `LaunchDarkly.Logging`, `Common.Logging` is a facade that can delegate to various adapters, sending the output either to some specific logging framework, or to a simple destination such as the console. Therefore, the LaunchDarkly adapter for `Common.Logging` is a facade for a facade.

`LaunchDarkly.Logging` already has adapters of its own for some of the same destinations that `Common.Logging` can delegate to. For instance, to send log output from LaunchDarkly components to the console, or to a file, or to the .NET Core `Microsoft.Extensions.Logging` API, you do not need to use `Common.Logging`; you can simply use the methods in `LaunchDarkly.Logging.Logs`. This adapter is only useful in two situations:

1. You have an application that is already using `Common.Logging` (such as an application that was built with an older version of the LaunchDarkly .NET SDK or Xamarin SDK, which previously always logged to `Common.Logging`).

2. You want to use some destination that `Common.Logging` already integrates with and `LaunchDarkly.Logging` currently does not.

To use the adapter:

1. Add the NuGet package `LaunchDarkly.Logging.CommonLogging` to your project.

2. Use the property `LaunchDarkly.Logging.LdCommonLogging.Adapter` in any LaunchDarkly library configuration that accepts a `LaunchDarkly.Logging.ILogAdapter` object. For instance, if you are configuring the LaunchDarkly .NET SDK:

```csharp
    using LaunchDarkly.Logging;
    using LaunchDarkly.Sdk.Server;

    var config = Configuration.Builder("my-sdk-key")
        .Logging(LdCommonLogging.Adapter)
        .Build();
    var client = new LdClient(config);
```

`LdCommonLogging.Adapter` does not have any configuration methods of its own; what happens to the log output is entirely determined by the `Common.Logging` configuration.

## Usage: `log4net`

`log4net` has a rich configuration system that allows log behavior to be controlled in many ways. The LaunchDarkly adapter does not define any specific logging behavior itself, so the actual behavior will be determined by how you have configured `log4net`.

To use the adapter:

1. Add the NuGet package `LaunchDarkly.Logging.Log4net` to your project.

2. Use the property `LaunchDarkly.Logging.LdLog4net.Adapter` in any LaunchDarkly library configuration that accepts a `LaunchDarkly.Logging.ILogAdapter` object. For instance, if you are configuring the LaunchDarkly .NET SDK:

```csharp
    using LaunchDarkly.Logging;
    using LaunchDarkly.Sdk.Server;

    var config = Configuration.Builder("my-sdk-key")
        .Logging(LdLog4net.Adapter)
        .Build();
    var client = new LdClient(config);
```

## Supported .NET versions

The adapter packages are built for two target frameworks:

* .NET Framework 4.5.2: runs on .NET Framework 4.5.x and above.
* .NET Standard 2.0: runs on .NET Core 2.0 and above; on other application platforms that are not .NET Framework, such as Xamarin; or within a library that is targeted to .NET Standard 2.x.

The .NET build tools should automatically load the most appropriate build of the library for whatever platform your application or library is targeted to.

## Contributing

See [Contributing](https://github.com/launchdarkly/dotnet-logging-adapters/blob/master/CONTRIBUTING.md).

## Signing

The published versions of these assemblies are digitally signed with Authenticode, and also strong-named. The public key file is in this repo at `LaunchDarkly.Logging.pk` as well as here:

```
Public Key:
2400000080040000009400000206000024000000535231410400000000010001
afcbfe1e33dbb0c823ca71ef053aed35a49a7f1e601d9ee27fe86b78062b1d83
30814ed41ccaf3817ff3f699766e5debb3dd46fd75f7439fc2fe390fcee65465
a8a17f69f1bef56e253fc9166096c907514ab74b812d041faa04712e2bcb243d
1038eed2b0023a35a41782d70c65cb4b51d189576df0b7846e9378a5d0758a39

Public Key Token: d9182e4b0afd33e7
```

Building the code locally in the default Debug configuration does not sign the assembly and does not require a key file. Note that the unit tests can only be run in the Debug configuration.

## About LaunchDarkly
 
* LaunchDarkly is a continuous delivery platform that provides feature flags as a service and allows developers to iterate quickly and safely. We allow you to easily flag your features and manage them from the LaunchDarkly dashboard.  With LaunchDarkly, you can:
    * Roll out a new feature to a subset of your users (like a group of users who opt-in to a beta tester group), gathering feedback and bug reports from real-world use cases.
    * Gradually roll out a feature to an increasing percentage of users, and track the effect that the feature has on key metrics (for instance, how likely is a user to complete a purchase if they have feature A versus feature B?).
    * Turn off a feature that you realize is causing performance problems in production, without needing to re-deploy, or even restart the application with a changed configuration file.
    * Grant access to certain features based on user attributes, like payment plan (eg: users on the ‘gold’ plan get access to more features than users in the ‘silver’ plan). Disable parts of your application to facilitate maintenance, without taking everything offline.
* LaunchDarkly provides feature flag SDKs for a wide variety of languages and technologies. Check out [our documentation](https://docs.launchdarkly.com/docs) for a complete list.
* Explore LaunchDarkly
    * [launchdarkly.com](https://www.launchdarkly.com/ "LaunchDarkly Main Website") for more information
    * [docs.launchdarkly.com](https://docs.launchdarkly.com/  "LaunchDarkly Documentation") for our documentation and SDK reference guides
    * [apidocs.launchdarkly.com](https://apidocs.launchdarkly.com/  "LaunchDarkly API Documentation") for our API documentation
    * [blog.launchdarkly.com](https://blog.launchdarkly.com/  "LaunchDarkly Blog Documentation") for the latest product updates
    * [Feature Flagging Guide](https://github.com/launchdarkly/featureflags/  "Feature Flagging Guide") for best practices and strategies
