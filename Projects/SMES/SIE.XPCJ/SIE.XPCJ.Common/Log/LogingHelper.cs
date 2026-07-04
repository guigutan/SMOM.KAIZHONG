using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SIE.XPCJ.Common.Snowflakes;

namespace SIE.XPCJ.Common.Log
{
    public class LogingHelper
    {
        public static string ServerTraceId = SnowflakeHelper.NextId().ToString();
        static ILoging loging = LogFactory.GetLogger<LogingHelper>();

        class LogData
        { 
            public string Header { get; set; }
            public object Content { get; set; }
        }

        public static void Info(object message, object data = null)
        {
            if(loging.IsInfoEnabled())
                loging.Info(ServerTraceId, null, null, null, message, data);
        }

        public static void Info(string type, string client, string session, object message, object data=null)
        {
            if (loging.IsInfoEnabled())
                loging.Info(ServerTraceId, type, client, session, message, data);
        }

        public static void Debug(string type, string client, string session, object message, object data=null)
        {
            if (loging.IsDebugEnabled())
                loging.Debug(ServerTraceId, type, client, session, message, data);
        }

        public static void Error(object message, Exception exception = null)
        {
            if (!loging.IsErrorEnabled())
                return;

            loging.Error(ServerTraceId, null, null, null, message, exception);
        }

        public static void Error(string type, string client, string session, object message, Exception exception = null)
        {
            if (!loging.IsErrorEnabled())
                return;
            loging.Error(ServerTraceId, type, client, session, message, exception);
        }

        public static void Warn(object message, Exception exception = null)
        {
            if (!loging.IsWarnEnabled())
                return;
            loging.Warn(ServerTraceId, null, null, null, message, exception);
        }

        public static void Warn(string type, string client, string session, object message, Exception exception = null)
        {
            if (!loging.IsWarnEnabled())
                return;
            loging.Warn(ServerTraceId, type, client, session, message, exception);
        }

        public static void Fatal(object message, Exception exception = null)
        {
            if (!loging.IsFatalEnabled())
                return;
            loging.Fatal(ServerTraceId, null, null, null, message, exception);
        }

        public static void Fatal(string type, string client, string session, object message, Exception exception = null)
        {
            if (!loging.IsFatalEnabled())
                return;
            loging.Fatal(ServerTraceId, type, client, session, message, exception);
        }
    }
}
