using System;
using System.Drawing;
using ExtremelySimpleLogger;

namespace Core
{
    public class Log : Singleton<Log>
    {
        private readonly Logger m_logger;

        private Log()
        {
            m_logger = new Logger();
            m_logger.Sinks.Add(new ConsoleSink());
        }

        public static void Trace(string message)
        {
            Instance.m_logger.Trace($"{message}"); //.Pastel(Color.DimGray));
        }

        public static void Trace(string category, string message)
        {
            Instance.m_logger.Trace($"[{category}] {message}"); //.Pastel(Color.DimGray));
        }
        
        public static void Trace(string category, string message, Color logColor)
        {
            Instance.m_logger.Trace($"[{category}] {message}"); //.Pastel(logColor));
        }

        public static void Debug(string message)
        {
            Instance.m_logger.Debug($"{message}"); //.Pastel(Color.DimGray));
        }

        public static void Debug(string category, string message)
        {
            Instance.m_logger.Debug($"[{category}] {message}"); //.Pastel(Color.DimGray));
        }
        
        public static void Debug(string category, string message, Color logColor)
        {
            Instance.m_logger.Debug($"[{category}] {message}"); //.Pastel(logColor));
        }

        public static void Info(string message)
        {
            Instance.m_logger.Info($"{message}"); //.Pastel(Color.White));
        }

        public static void Info(string category, string message)
        {
            Instance.m_logger.Info($"[{category}] {message}"); //.Pastel(Color.White));
        }
        
        public static void Info(string category, string message, Color logColor)
        {
            Instance.m_logger.Info($"[{category}] {message}"); //.Pastel(logColor));
        }

        public static void Warning(string message)
        {
            Instance.m_logger.Warn($"{message}"); //.Pastel(Color.Yellow));
        }

        public static void Warning(string category, string message)
        {
            Instance.m_logger.Warn($"[{category}] {message}"); //.Pastel(Color.Yellow));
        }
        
        public static void Warning(string category, string message, Color logColor)
        {
            Instance.m_logger.Warn($"[{category}] {message}"); //.Pastel(logColor));
        }

        public static void Error(string message)
        {
            Instance.m_logger.Error($"{message}"); //.Pastel(Color.Orange));
        }

        public static void Error(string category, string message)
        {
            Instance.m_logger.Error($"[{category}] {message}"); //.Pastel(Color.Orange));
        }
        
        public static void Error(string category, string message, Color logColor)
        {
            Instance.m_logger.Error($"[{category}] {message}"); //.Pastel(logColor));
        }

        public static void Exception(string message)
        {
            Instance.m_logger.Fatal($"{message}"); //.Pastel(Color.Red));
        }

        public static void Exception(string category, string message)
        {
            Instance.m_logger
                    .Fatal($"[{category}] {message}");  //.Pastel(Color.Red));
        }

        public static void Exception(string category, string message, Color logColor)
        {
            Instance.m_logger.Fatal($"[{category}] {message}"); //.Pastel(logColor));
        }

        public static void RegisterSink(Sink logSink)
        {
            Instance.m_logger.Sinks.Add(logSink);
        }

        public static void RegisterSink<T>() where T : Sink, new()
        {
            Instance.m_logger.Sinks.Add(new T());
        }
    }

    public static class LogCategory
    {
        public const string IO = "Input/Output";
        public const string UI = "UI";
    }
}