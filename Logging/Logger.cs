using log4net;
using System;
using System.Collections.Generic;

namespace Novalia.Logging
{
    public class Logger : ILogger
    {
        private readonly Dictionary<LogType, List<Action<string>>> _eventListeners;
        private readonly ILog _log;

        public Logger(ILog log)
        {
            _eventListeners = new Dictionary<LogType, List<Action<string>>>();
            _log = log;
        }

        public void RegisterEventListener(LogType type, Action<string> listener)
        {
            if (!_eventListeners.ContainsKey(type))
            {
                _eventListeners[type] = new List<Action<string>>();
            }

            _eventListeners[type].Add(listener);
        }

        public void UnregisterEventListener(LogType type, Action<string> listener)
        {
            _eventListeners[type].Remove(listener);
        }

        public void Log(string message, LogType logType)
        {
            switch (logType)
            {
                case LogType.Debug:
                    _log.Debug(message);
                    break;
                case LogType.Error:
                    _log.Error(message);
                    break;
                case LogType.Gameplay:
                    break;
            }

            if (_eventListeners.TryGetValue(logType, out var listeners))
            {
                listeners.ForEach(action => action(message));
            }
        }

        public void Debug(string message) => Log(message, LogType.Debug);
    }
}
