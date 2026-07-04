using SIE.EventMessages.QMS.Models;
using System;

namespace SIE.EventMessages.QA
{

    /// <summary>
    /// 关闭质量审核不符合项
    /// </summary>
    [Services.Service(FallbackType = typeof(UpdateNonConformanceDefault))]
    public interface IUpdateNonConformance
    {
        /// <summary>
        /// 关闭质量审核不符合项
        /// </summary>
        /// <param name="reportEvent">8D参数</param>
        void CloseNonConformance(UpdateNonConformanceEvent reportEvent);

    }

    /// <summary>
    /// 接口的默认实现
    /// </summary>
    class UpdateNonConformanceDefault : IUpdateNonConformance
    {
        public void CloseNonConformance(UpdateNonConformanceEvent reportEvent)
        {
            throw new NotImplementedException("缺少质量审核模块，无法更新数据");
        }

    }

    /// <summary>
    /// 生成PDCA改善报告参数
    /// </summary>
    [Serializable]
    public class UpdateNonConformanceEvent
    {
        /// <summary>
        /// 改善编码
        /// </summary>
        public string ImproveCode { get; set; }

        /// <summary>
        /// 来源Ids
        /// </summary>
        public string SourceIds { get; set; }

        /// <summary>
        /// 改善状态
        /// </summary>
        public ImproveState ImproveState { get; set; }

    }


}
