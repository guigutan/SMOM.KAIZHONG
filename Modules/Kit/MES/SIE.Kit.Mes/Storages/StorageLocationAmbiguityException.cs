using SIE.Domain.Validation;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 异常类
    /// </summary>
    [Serializable]
    public class StorageLocationAmbiguityException : ValidationException
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public StorageLocationAmbiguityException() { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="message">异常描述</param>
        public StorageLocationAmbiguityException(string message) : base(message) { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="message">异常描述</param>
        /// <param name="innerException">异常</param>
        public StorageLocationAmbiguityException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">序列化上下文</param>
        protected StorageLocationAmbiguityException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}