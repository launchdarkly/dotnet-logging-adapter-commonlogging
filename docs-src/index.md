These .NET packages provide integration from the [`LaunchDarkly.Logging`](https://launchdarkly.github.io/dotnet-logging) API that is used by the LaunchDarkly [.NET SDK](https://github.com/launchdarkly/dotnet-server-sdk), [Xamarin SDK](https://github.com/launchdarkly/xamarin-client-sdk), and other LaunchDarkly libraries, to the third-party logging frameworks [`Common.Logging`](https://github.com/net-commons/common-logging), [`log4net`](https://logging.apache.org/log4net/), and [`NLog`](https://nlog-project.org/).

The adapters in this repository are published as separate NuGet packages, to avoid unwanted dependencies on `Common.Logging`, `log4net`, or `NLog` in the LaunchDarkly SDKs and in applications that do not use those frameworks.

## Using **Common.Logging**

The **<xref:LaunchDarkly.Logging.LdCommonLogging>** adapter is provided by the NuGet package [**`LaunchDarkly.Logging.CommonLogging`**](https://nuget.org/packages/LaunchDarkly.Logging.CommonLogging). It provides integration with [`Common.Logging`](https://github.com/net-commons/common-logging) version 3.4.0 and higher.

Like `LaunchDarkly.Logging`, `Common.Logging` is a facade that can delegate to various adapters, sending the output either to some specific logging framework, or to a simple destination such as the console. Therefore, the LaunchDarkly adapter for `Common.Logging` is a facade for a facade.

`LaunchDarkly.Logging` already has adapters of its own for some of the same destinations that `Common.Logging` can delegate to. For instance, to send log output from LaunchDarkly components to the console, or to a file, or to the .NET Core `Microsoft.Extensions.Logging` API, you do not need to use `Common.Logging`; you can simply use the methods in `LaunchDarkly.Logging.Logs`. This adapter is only useful in two situations:

* You have an application that is already using `Common.Logging` (such as an application that was built with an older version of the LaunchDarkly .NET SDK or Xamarin SDK, which previously always logged to `Common.Logging`).
* Or, you want to use some destination that `Common.Logging` already integrates with and `LaunchDarkly.Logging` currently does not.

To use the adapter:

1. Add the NuGet package `LaunchDarkly.Logging.CommonLogging` to your project. Make sure you also have a dependency on a compatible version of [`Common.Logging`](https://nuget.org/packages/Common.Logging).

2. Use the property [**LdCommonLogging.Adapter**](xref:LaunchDarkly.Logging.LdCommonLogging.Adapter) in any LaunchDarkly library configuration that accepts a `LaunchDarkly.Logging.ILogAdapter` object. For instance, if you are configuring the LaunchDarkly .NET SDK:

```csharp
    using LaunchDarkly.Logging;
    using LaunchDarkly.Sdk.Server;

    var config = Configuration.Builder("my-sdk-key")
        .Logging(LdCommonLogging.Adapter)
        .Build();
    var client = new LdClient(config);
```

`LdCommonLogging.Adapter` does not have any configuration methods of its own; what happens to the log output is entirely determined by the `Common.Logging` configuration.

## Using **log4net**

The **<xref:LaunchDarkly.Logging.LdLog4net>** adapter is provided by the NuGet package [**`LaunchDarkly.Logging.Log4net`**](https://nuget.org/packages/LaunchDarkly.Logging.Log4net). It provides integration with [`log4net`](https://logging.apache.org/log4net/) version 2.0.6 and higher.

`log4net` has a rich configuration system that allows log behavior to be controlled in many ways. The LaunchDarkly adapter does not define any specific logging behavior itself, so the actual behavior will be determined by how you have configured `log4net`.

To use the adapter:

1. Add the NuGet package `LaunchDarkly.Logging.Log4net` to your project. Make sure you also have a dependency on a compatible version of [`log4net`](https://nuget.org/packages/log4net).

2. Use the property [**LdLog4net.Adapter**](xref:LaunchDarkly.Logging.LdLog4net.Adapter) in any LaunchDarkly library configuration that accepts a `LaunchDarkly.Logging.ILogAdapter` object. For instance, if you are configuring the LaunchDarkly .NET SDK:

```csharp
    using LaunchDarkly.Logging;
    using LaunchDarkly.Sdk.Server;

    var config = Configuration.Builder("my-sdk-key")
        .Logging(LdLog4net.Adapter)
        .Build();
    var client = new LdClient(config);
```

## Using **NLog**

The **<xref:LaunchDarkly.Logging.LdNLog>** adapter is provided by the NuGet package [**`LaunchDarkly.Logging.NLog`**](https://nuget.org/packages/LaunchDarkly.LoggingNLog). It provides integration with [`NLog`](https://nlog-project.org/) version 4.5 and higher.

`NLog` has a rich configuration system that allows log behavior to be controlled in many ways. The LaunchDarkly adapter does not define any specific logging behavior itself, so the actual behavior will be determined by how you have configured `NLog`.

To use the adapter:

1. Add the NuGet package `LaunchDarkly.Logging.NLog` to your project. Make sure you also have a dependency on a compatible version of [`NLog`](https://nuget.org/packages/NLog).

2. Use the property [**LdNLog.Adapter**](xref:LaunchDarkly.Logging.LdNLog.Adapter) in any LaunchDarkly library configuration that accepts a `LaunchDarkly.Logging.ILogAdapter` object. For instance, if you are configuring the LaunchDarkly .NET SDK:

```csharp
    using LaunchDarkly.Logging;
    using LaunchDarkly.Sdk.Server;

    var config = Configuration.Builder("my-sdk-key")
        .Logging(LdNLog.Adapter)
        .Build();
    var client = new LdClient(config);
```
