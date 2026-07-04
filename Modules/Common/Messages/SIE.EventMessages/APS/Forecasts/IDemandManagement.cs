using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.Forecasts
{
    /// <summary>
    /// 预测冲销-需求管理接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyDemandManagement))]
    public interface IDemandManagement
    {
        /// <summary>
        /// 获取需求管理信息
        /// </summary>
        Dictionary<double,DemandManegementInfo> GetDemandManagementIdAndInfoDic(List<double> demandIds);
    }

    /// <summary>
    /// 空方法
    /// </summary>
    public class EmptyDemandManagement : IDemandManagement
    {
        /// <summary>
        /// 生成报表相关类 - 空方法
        /// </summary>
        public Dictionary<double, DemandManegementInfo> GetDemandManagementIdAndInfoDic(List<double> demandIds)
        {
            return new Dictionary<double, DemandManegementInfo>();
        }
    }

    /// <summary>
    /// 需求管理信息类
    /// </summary>
    public class DemandManegementInfo
    {
        /// <summary>
        /// 需求管理ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 需求管理编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 需求管理行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 编码和行号
        /// </summary>
        public string CodeAndLineNo
        {
            get
            {
                return Code + (LineNo.IsNullOrEmpty() ? string.Empty : ("_" + LineNo));
            }
        }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
    }

}
