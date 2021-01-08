
namespace LaunchDarkly.Logging
{
    /// <summary>
    /// Provides integration between the LaunchDarkly SDK's logging framework and
    /// the <c>Common.Logging</c> facade.
    /// </summary>
    public static class LdCommonLogging
    {
        /// <summary>
        /// Returns an adapter for directing <c>LaunchDarkly.Logging</c> output to
        /// <c>Common.Logging</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Using this adapter will cause <c>LaunchDarkly.Logging</c> to delegate each
        /// logger it creates to a corresponding logger created with
        /// <c>Common.Logging.LogManager.GetLogger</c>. What happens to the log output
        /// then is entirely determined by the <c>Common.Logging</c> configuration; there
        /// are no configuration methods on the <c>Adapter</c> itself. The logger names
        /// that are used within the <c>LaunchDarkly.Logging</c> framework are passed along
        /// as logger names to <c>Common.Logging</c>, so they can be used in filtering
        /// rules, etc.
        /// </para>
        /// <para>
        /// The example code below shows how to configure the LaunchDarkly SDK to
        /// use <c>Common.Logging</c>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        ///     using LaunchDarkly.Logging;
        ///     using LaunchDarkly.Sdk.Server;
        ///
        ///     var config = Configuration.Builder("my-sdk-key")
        ///         .Logging(LdCommonLogging.Adapter)
        ///         .Build();
        ///     var client = new LdClient(config);
        /// </code>
        /// </example>
        /// <returns>an <c>ILogAdapter</c> that delegates to <c>Common.Logging</c></returns>
        public static ILogAdapter Adapter => CommonLoggingAdapter.Instance;
    }

    internal sealed class CommonLoggingAdapter : ILogAdapter
    {
        internal static readonly CommonLoggingAdapter Instance = new CommonLoggingAdapter();

        public IChannel NewChannel(string name) => new CommonLoggingChannel(name);
    }

    internal sealed class CommonLoggingChannel : IChannel
    {
        private readonly Common.Logging.ILog _log;

        internal CommonLoggingChannel(string name)
        {
            _log = Common.Logging.LogManager.GetLogger(name);
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
                    _log.DebugFormat(format, param);
                    break;
                case LogLevel.Info:
                    _log.InfoFormat(format, param);
                    break;
                case LogLevel.Warn:
                    _log.WarnFormat(format, param);
                    break;
                case LogLevel.Error:
                    _log.ErrorFormat(format, param);
                    break;
            }
        }

        public void Log(LogLevel level, string format, object param1, object param2)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _log.DebugFormat(format, param1, param2);
                    break;
                case LogLevel.Info:
                    _log.InfoFormat(format, param1, param2);
                    break;
                case LogLevel.Warn:
                    _log.WarnFormat(format, param1, param2);
                    break;
                case LogLevel.Error:
                    _log.ErrorFormat(format, param1, param2);
                    break;
            }
        }

        public void Log(LogLevel level, string format, params object[] allParams)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _log.DebugFormat(format, allParams);
                    break;
                case LogLevel.Info:
                    _log.InfoFormat(format, allParams);
                    break;
                case LogLevel.Warn:
                    _log.WarnFormat(format, allParams);
                    break;
                case LogLevel.Error:
                    _log.ErrorFormat(format, allParams);
                    break;
            }
        }
    }
}
