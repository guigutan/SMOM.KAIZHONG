using SIE.Domain.Validation;
using System;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志特性拦截基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ApiLogAttribute : Attribute
    {
        /// <summary>
        /// 超过则记录日志的时间
        /// </summary>
        public int FetchOverTime { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        private Type loggerType;
        /// <summary>
        /// 日志类型
        /// </summary>
        public Type LoggerType 
        { 
            get 
            { 
                return loggerType; 
            }
            set 
            { 
                ValidateLoggerType(value);
                loggerType = value;
                Logger = RT.Service.Resolve(value) as IApiLogLogger;
            }
        }
        /// <summary>
        /// 日志记录者
        /// </summary>
        public IApiLogLogger Logger { get; private set; }

        /// <summary>
        /// API日志特性
        /// </summary>
        public ApiLogAttribute()
        {
            FetchOverTime = RT.Config.Get("ApiLog.OverTime", 0);
            var loggerTypeStr = RT.Config.Get("ApiLog.DefaultLogger", typeof(ApiLogDbLogger).FullName);
            var loggerType = Type.GetType(loggerTypeStr);
            if (null == loggerType)
                throw new ValidationException("未找到日志类[{0}]".FormatArgs(loggerTypeStr));
            ValidateLoggerType(loggerType);
            Logger = RT.Service.Resolve(loggerType) as IApiLogLogger;           
        }

        /// <summary>
        /// 验证日志记录者类型
        /// </summary>
        /// <param name="loggerType"></param>
        private void ValidateLoggerType(Type loggerType)
        {
            if (!typeof(IApiLogLogger).IsAssignableFrom(loggerType))
                throw new ValidationException($"{loggerType.Name}须实现IApiLogLogger接口");
        }
    } 

    /// <summary>
    /// 不启用API日志特性拦截基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DisableApiLogAttribute : Attribute
    {
    }
}
