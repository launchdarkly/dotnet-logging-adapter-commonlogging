This .NET package provides integration from the [`LaunchDarkly.Logging`](https://launchdarkly.github.io/dotnet-logging) API that is used by the LaunchDarkly [.NET SDK](https://github.com/launchdarkly/dotnet-server-sdk), [Xamarin SDK](https://github.com/launchdarkly/xamarin-client-sdk), and other LaunchDarkly libraries, to the [`Common.Logging`](https://github.com/net-commons/common-logging) framework.

This adapter is published as a separate NuGet package to avoid unwanted dependencies on `Common.Logging` in the LaunchDarkly SDKs and in applications that do not use that framework.

## Usage

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

