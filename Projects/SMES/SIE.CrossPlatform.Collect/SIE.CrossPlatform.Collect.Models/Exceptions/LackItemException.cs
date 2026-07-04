using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.Exceptions
{
    [Serializable]
    public class LackItemException : ValidationException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LackItemException() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示信息</param>
        public LackItemException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">上下文</param>
        protected LackItemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="innerException">内部异常</param>
        public LackItemException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
