using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.CallMaterials
{
    /// <summary>
    /// 叫料接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultCallMaterial))]
    public interface ICallMaterial
    {
        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="labelNo">标签号</param>
        /// <returns>标签信息</returns>
        PacingLabelEvent GetPackingLabel(string labelNo);
    }

    /// <summary>
    /// 默认实现叫料接口
    /// </summary>
    public class DefaultCallMaterial : ICallMaterial
    {
        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="labelNo">标签号</param>
        /// <returns>标签信息</returns>
        public PacingLabelEvent GetPackingLabel(string labelNo)
        {
            return null;
        }
    }

    /// <summary>
    /// 标签信息
    /// </summary>
    [Serializable]
    public class PacingLabelEvent
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 标签下所有序列号
        /// </summary>
        public List<double> SnIdList { get; set; } = new List<double>();
    }
}
