using SIE.EMS.SMDC.Equipments.ErrorInfo;
using System;

namespace SIE.EMS.SMDC.Equipments.Infos
{
    /// <summary>
    /// MDC接口返回数据结构
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    [Serializable]
    public class MdcReturn<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        public double RequistId { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public EquipEapError Error { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
