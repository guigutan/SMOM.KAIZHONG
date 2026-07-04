using System;

namespace SIE.XPCJ.Common.Log
{
    public interface ILoging
    {
        void Info(object message);
        void Info(string traceId, string type, string client, string session, object message, object data);
        bool IsInfoEnabled();

        void Debug(object message);
        void Debug(string traceId, string type, string client, string session, object message, object data);
        bool IsDebugEnabled();

        void Error(object message);
        void Error(string traceId, string type, string client, string session, object message, Exception exception);
        bool IsErrorEnabled();

        void Warn(object message);
        void Warn(string traceId, string type, string client, string session, object message, Exception exception);
        bool IsWarnEnabled();

        void Fatal(object message);
        void Fatal(string traceId, string type, string client, string session, object message, Exception exception);
        bool IsFatalEnabled();
    }
}
