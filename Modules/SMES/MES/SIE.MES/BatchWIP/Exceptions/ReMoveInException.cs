using System;
using System.Runtime.Serialization;

namespace SIE.MES.BatchWIP.Exceptions
{
    /// <summary>
    /// 重复转入异常
    /// </summary>
    [Serializable]
    public class ReMoveInException : PlatformException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReMoveInException() { } 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示信息</param>
        public ReMoveInException(string message) : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="workOrderId">工单Id</param>
        public ReMoveInException(string message, double workOrderId) : base(message)
        {
            this.Data.Add(WorkOrderId, workOrderId);
        }

        /// <summary>
        /// 工单Id
        /// </summary>
        public const string WorkOrderId = "WorkOrderId";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">上下文</param>
        protected ReMoveInException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="innerException">内部异常</param>
        public ReMoveInException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}