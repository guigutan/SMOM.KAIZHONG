using log4net.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIE.XPCJ.Common.Snowflakes;

namespace SIE.XPCJ.Common.Log
{
    /// <summary>
    /// 链路追踪日志
    /// </summary>
    public class Loging : log4net.Core.LogImpl, ILoging
    {
        static JsonSerializerSettings _JsonSerializerSettings = new JsonSerializerSettings { 
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public Loging(ILogger logger) : base(logger)
        {
        }
        /// <summary>
        /// 处理日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private object HandleMessage(string traceId, string type, string client, string session, Level logLevel, object message, Exception exception = null, object data = null)
        {
            object r = null;
            var me = new LogBaseMessage()
            {
                Id = SnowflakeHelper.NextId(),
                LogLevel = logLevel.ToString(),
                Data = data,
                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                TraceId = traceId,
                Message = message?.ToString(),
                Client = client,
                Session = session,
                Type = type,
            };
            if (exception != null)
                me.Error = exception.ToString();
            r = JsonConvert.SerializeObject(me, _JsonSerializerSettings);

            return r;
        }

        public void Info(string traceId, string type, string client, string session, object message, object data=null)
        {
            if (base.IsInfoEnabled)
                base.Info(HandleMessage(traceId, type, client, session, Level.Info, message, data: data));
        }

        public void Debug(string traceId, string type, string client, string session, object message, object data = null)
        {
            if (base.IsDebugEnabled)
                base.Debug(HandleMessage(traceId, type, client, session, Level.Debug, message, data: data));
        }

        public void Error(string traceId, string type, string client, string session, object message, Exception exception=null)
        {
            if (base.IsErrorEnabled)
                base.Error(HandleMessage(traceId, type, client, session, Level.Error, message, exception));
        }

        public void Warn(string traceId, string type, string client, string session, object message, Exception exception=null)
        {
            if (base.IsWarnEnabled)
                base.Warn(HandleMessage(traceId, type, client, session, Level.Warn, message, exception));
        }

        public void Fatal(string traceId, string type, string client, string session, object message, Exception exception = null)
        {
            if (base.IsFatalEnabled)
                base.Fatal(HandleMessage(traceId, type, client, session, Level.Fatal, message, exception));
        }

        bool ILoging.IsInfoEnabled()
        {
            return base.IsInfoEnabled;
        }

        bool ILoging.IsDebugEnabled()
        {
            return base.IsDebugEnabled;
        }

        bool ILoging.IsErrorEnabled()
        {
            return base.IsErrorEnabled;
        }

        bool ILoging.IsWarnEnabled()
        {
            return base.IsWarnEnabled;
        }

        bool ILoging.IsFatalEnabled()
        {
            return base.IsFatalEnabled;
        }
    }
}
