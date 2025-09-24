using log4net;
using log4net.Config;

namespace Shared.Logging
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        static Logger()
        {
            try
            {
                var logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logsDir))
                {
                    Directory.CreateDirectory(logsDir);
                }

                // Koristi se app.config
                XmlConfigurator.Configure();

                log.Info("Logger initialized successfully from app.config");
            }
            catch (Exception ex)
            {
                BasicConfigurator.Configure();
                log.Warn($"Using basic configuration: {ex.Message}");
            }
        }

        public static void Debug(string message) => log.Debug(message);

        public static void Info(string message) => log.Info(message);

        public static void Warn(string message) => log.Warn(message);

        public static void Error(string message) => log.Error(message);

        public static void Fatal(string message) => log.Fatal(message);

        public static void Error(string message, Exception ex) => log.Error($"{message} - {ex.Message}", ex);

        public static void Fatal(string message, Exception ex) => log.Fatal($"{message} - {ex.Message}", ex);
    }
}