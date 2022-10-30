using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Helpers
{
    public class ListLogger : ILogger
    {
        public IList<string> Logs;


        public bool IsEnabled(LogLevel logLevel) => false;
        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
        public ListLogger()
        {
            this.Logs = new List<string>();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);
            this.Logs.Add(message);
        }

  
    }
}
