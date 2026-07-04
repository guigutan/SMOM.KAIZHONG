using System;
using System.Runtime.Serialization;

namespace SIE.KZ.Print
{
    /// <summary>
    /// 打印异常类
    /// </summary>
    [Serializable]
    public class PrintException : PlatformException
    {
        /// <summary>
        /// 默认无参的构造函数
        /// </summary>
        public PrintException()
        {
        }

        /// <summary>
        /// 有参的构造函数
        /// </summary>
        /// <param name="message">异常说明信息.</param>
        public PrintException(string message) : base(message) { }

        /// <summary>
        /// 有参的构造函数
        /// </summary>
        /// <param name="message">异常说明信息.</param>
        /// <param name="innerException"><see cref="Exception"/>异常实例</param>
        public PrintException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// 有参的构造函数.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/>序列化信息.</param>
        /// <param name="context"><see cref="StreamingContext"/>序列化上下文</param>
        protected PrintException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

    }
}
