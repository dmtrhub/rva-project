using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Shared.Logging
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        static Logger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public static void Debug(string message) => log.Debug(message);

        public static void Info(string message) => log.Info(message);

        public static void Warn(string message) => log.Warn(message);

        public static void Error(string message) => log.Error(message);

        public static void Fatal(string message) => log.Fatal(message);
    }
}