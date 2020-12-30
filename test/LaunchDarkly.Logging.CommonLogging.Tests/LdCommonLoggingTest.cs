using System;
using Common.Logging;
using Common.Logging.Simple;
using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class LdCommonLoggingTest
    {
        [Fact]
        public void TestAdapter()
        {
            var commonLoggingCapture = new CapturingLoggerFactoryAdapter();
            LogManager.Adapter = commonLoggingCapture;

            var ourAdapter = LdCommonLogging.Adapter;
            var logger1 = ourAdapter.Logger("things");
            var logger2 = logger1.SubLogger("stuff");

            logger1.Debug("d0");
            logger1.Debug("d1,{0}", "a");
            logger1.Debug("d2,{0},{1}", "a", "b");
            logger1.Debug("d3,{0},{1},{2}", "a", "b", "c");
            logger1.Info("i0");
            logger1.Info("i1,{0}", "a");
            logger1.Info("i2,{0},{1}", "a", "b");
            logger1.Info("i3,{0},{1},{2}", "a", "b", "c");
            logger1.Warn("w0");
            logger1.Warn("w1,{0}", "a");
            logger1.Warn("w2,{0},{1}", "a", "b");
            logger1.Warn("w3,{0},{1},{2}", "a", "b", "c");
            logger1.Error("e0");
            logger1.Error("e1,{0}", "a");
            logger1.Error("e2,{0},{1}", "a", "b");
            logger1.Error("e3,{0},{1},{2}", "a", "b", "c");
            logger2.Warn("goodbye");

            Assert.Collection(commonLoggingCapture.LoggerEvents,
                ExpectEvent("things", Common.Logging.LogLevel.Debug, "d0"),
                ExpectEvent("things", Common.Logging.LogLevel.Debug, "d1,a"),
                ExpectEvent("things", Common.Logging.LogLevel.Debug, "d2,a,b"),
                ExpectEvent("things", Common.Logging.LogLevel.Debug, "d3,a,b,c"),
                ExpectEvent("things", Common.Logging.LogLevel.Info, "i0"),
                ExpectEvent("things", Common.Logging.LogLevel.Info, "i1,a"),
                ExpectEvent("things", Common.Logging.LogLevel.Info, "i2,a,b"),
                ExpectEvent("things", Common.Logging.LogLevel.Info, "i3,a,b,c"),
                ExpectEvent("things", Common.Logging.LogLevel.Warn, "w0"),
                ExpectEvent("things", Common.Logging.LogLevel.Warn, "w1,a"),
                ExpectEvent("things", Common.Logging.LogLevel.Warn, "w2,a,b"),
                ExpectEvent("things", Common.Logging.LogLevel.Warn, "w3,a,b,c"),
                ExpectEvent("things", Common.Logging.LogLevel.Error, "e0"),
                ExpectEvent("things", Common.Logging.LogLevel.Error, "e1,a"),
                ExpectEvent("things", Common.Logging.LogLevel.Error, "e2,a,b"),
                ExpectEvent("things", Common.Logging.LogLevel.Error, "e3,a,b,c"),
                ExpectEvent("things.stuff", Common.Logging.LogLevel.Warn, "goodbye")
                );
        }

        private Action<CapturingLoggerEvent> ExpectEvent(string loggerName,
            Common.Logging.LogLevel level, string message)
        {
            return e =>
            {
                Assert.Equal(level, e.Level);
                Assert.Equal(loggerName, e.Source.Name);
                Assert.Equal(message, e.RenderedMessage);
            };
        }
    }
}
