using System;
using System.Collections;
using ExtremelySimpleLogger;
using Logger = ExtremelySimpleLogger.Logger;

namespace Core.Unity.Utils
{
    public class UnitySink : Sink
    {
        protected override void Log(Logger logger, LogLevel level, string s)
        {
            if (!UnityMainThreadDispatcher.Exists() || UnityMainThreadDispatcher.IsOnMainThread)
            {
                DoLog(logger, level, s);
            }
            else
            {
                UnityMainThreadDispatcher.Instance().Enqueue(ThreadDispatchedLog(logger, level, s));
            }
        }

        private IEnumerator ThreadDispatchedLog(Logger logger, LogLevel level, string s)
        {
            DoLog(logger, level, s);
            yield break;
        }

        private void DoLog(Logger logger, LogLevel level, string s)
        {
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Info:
                    UnityEngine.Debug.Log(s);
                    break;
                case LogLevel.Warn:
                    UnityEngine.Debug.LogWarning(s);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(s);
                    break;
                case LogLevel.Fatal:
                    UnityEngine.Debug.LogException(new Exception(s));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}