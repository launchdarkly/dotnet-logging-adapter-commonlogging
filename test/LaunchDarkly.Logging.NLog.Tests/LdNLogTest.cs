using System;
using Xunit;

namespace LaunchDarkly.Logging.Tests
{
    public class LdNLogTest
    {
        [Fact]
        public void TestAdapter()
        {
            var nLogCapture = new NLog.Targets.MemoryTarget();
            nLogCapture.Layout = "${level}: ${message}";
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(nLogCapture, NLog.LogLevel.Debug);

            var ourAdapter = LdNLog.Adapter;
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

            Assert.Collection(nLogCapture.Logs,
                ExpectEvent("things", LogLevel.Debug, "d0"),
                ExpectEvent("things", LogLevel.Debug, "d1,a"),
                ExpectEvent("things", LogLevel.Debug, "d2,a,b"),
                ExpectEvent("things", LogLevel.Debug, "d3,a,b,c"),
                ExpectEvent("things", LogLevel.Info, "i0"),
                ExpectEvent("things", LogLevel.Info, "i1,a"),
                ExpectEvent("things", LogLevel.Info, "i2,a,b"),
                ExpectEvent("things", LogLevel.Info, "i3,a,b,c"),
                ExpectEvent("things", LogLevel.Warn, "w0"),
                ExpectEvent("things", LogLevel.Warn, "w1,a"),
                ExpectEvent("things", LogLevel.Warn, "w2,a,b"),
                ExpectEvent("things", LogLevel.Warn, "w3,a,b,c"),
                ExpectEvent("things", LogLevel.Error, "e0"),
                ExpectEvent("things", LogLevel.Error, "e1,a"),
                ExpectEvent("things", LogLevel.Error, "e2,a,b"),
                ExpectEvent("things", LogLevel.Error, "e3,a,b,c"),
                ExpectEvent("things.stuff", LogLevel.Warn, "goodbye")
                );
        }

        private Action<string> ExpectEvent(string loggerName,
            LogLevel level, string message)
        {
            return line =>
            {
                Assert.Equal(string.Format("{0}: {1}", level, message), line);
            };
        }
    }
}
