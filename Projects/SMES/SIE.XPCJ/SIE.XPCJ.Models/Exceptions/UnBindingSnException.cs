using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SIE.XPCJ.Models.Exceptions
{

    /// <summary>
    /// 拼板码未绑定SN异常
    /// </summary>
    [Serializable]
    public class UnBindingSnException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnBindingSnException() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示信息</param>
        public UnBindingSnException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">上下文</param>
        protected UnBindingSnException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="innerException">内部异常</param>
        public UnBindingSnException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
