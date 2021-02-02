
namespace LaunchDarkly.Logging
{
    /// <summary>
    /// Provides integration between the LaunchDarkly SDK's logging framework and
    /// the <c>NLog</c> framework.
    /// </summary>
    public static class LdNLog
    {
        /// <summary>
        /// Returns an adapter for directing <c>LaunchDarkly.Logging</c> output to
        /// <c>NLog</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Using this adapter will cause <c>LaunchDarkly.Logging</c> to delegate each
        /// logger it creates to a corresponding logger created with
        /// <c>NLog.LogManager.GetLogger</c>. What happens to the log output then is
        /// entirely determined by the <c>NLog</c> configuration; there are no
        /// configuration methods on the <c>Adapter</c> itself. The logger names that are
        /// used within the <c>LaunchDarkly.Logging</c> framework are passed along as
        /// logger names to <c>NLog</c>, so they can be used in filtering rules, etc.
        /// </para>
        /// <para>
        /// The example code below shows how to configure the LaunchDarkly SDK to
        /// use <c>NLog</c>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        ///     using LaunchDarkly.Logging;
        ///     using LaunchDarkly.Sdk.Server;
        ///
        ///     var config = Configuration.Builder("my-sdk-key")
        ///         .Logging(LdNLog.Adapter)
        ///         .Build();
        ///     var client = new LdClient(config);
        /// </code>
        /// </example>
        /// <returns>an <c>ILogAdapter</c> that delegates to <c>NLog</c></returns>
        public static ILogAdapter Adapter => NLogAdapter.Instance;
    }

    internal sealed class NLogAdapter : ILogAdapter
    {
        internal static readonly NLogAdapter Instance = new NLogAdapter();

        public IChannel NewChannel(string name) => new NLogChannel(name);
    }

    internal sealed class NLogChannel : IChannel
    {
        private readonly NLog.Logger _log;

        internal NLogChannel(string name)
        {
            _log = NLog.LogManager.GetLogger(name);
        }

        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;
                case LogLevel.Info:
                    return _log.IsInfoEnabled;
                case LogLevel.Warn:
                    return _log.IsWarnEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                default:
                    return false;
            }
        }

        public void Log(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _log.Debug(message);
                    break;
                case LogLevel.Info:
                    _log.Info(message);
                    break;
                case LogLevel.Warn:
                    _log.Warn(message);
                    break;
                case LogLevel.Error:
                    _log.Error(message);
                    break;
            }
        }

        public void Log(LogLevel level, string format, object param)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _log.Debug(format, param);
                    break;
                case LogLevel.Info:
                    _log.Info(format, param);
                    break;
                case LogLevel.Warn:
                    _log.Warn(format, param);
                    break;
                case LogLevel.Error:
                    _log.Error(format, param);
                    break;
            }
        }

        public void Log(LogLevel level, string format, object param1, object param2)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _log.Debug(format, param1, param2);
                    break;
                case LogLevel.Info:
                    _log.Info(format, param1, param2);
                    break;
                case LogLevel.Warn:
                    _log.Warn(format, param1, param2);
                    break;
                case LogLevel.Error:
                    _log.Error(format, param1, param2);
                    break;
            }
        }

        public void Log(LogLevel level, string format, params object[] allParams)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _log.Debug(format, allParams);
                    break;
                case LogLevel.Info:
                    _log.Info(format, allParams);
                    break;
                case LogLevel.Warn:
                    _log.Warn(format, allParams);
                    break;
                case LogLevel.Error:
                    _log.Error(format, allParams);
                    break;
            }
        }
    }
}
