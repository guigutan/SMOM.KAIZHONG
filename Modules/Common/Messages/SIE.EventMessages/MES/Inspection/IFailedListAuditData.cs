using SIE.EventMessages.MES.Inspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EventMessages.MES.Inspection
{
    /// <summary>
    /// 默认不合格审核接口实现
    /// </summary>
    public class DefaultFailedListAuditData
    {
        /// <summary>
        /// 不合格审核方法类集合
        /// </summary>
        public static Dictionary<int, IFailedListAuditData> DicHelper { get; set; } = new Dictionary<int, IFailedListAuditData>();

        /// <summary>
        /// 获取单据对应的处理方式
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public List<ProcessMode> GetProcessModeEnumList(FailedBillEventArgs eventArgs)
        {
            var processModeList = DicHelper.FirstOrDefault(p => p.Key == eventArgs.InspectionTypeInt).Value?.GetProcessModeEnumList();
            if (processModeList != null)
                return processModeList;
            return new List<ProcessMode>();
        }

        /// <summary>
        /// 获取不合格审核缺陷信息
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public List<FailedBillDetailInfo> GetFailedBillDetailInfos(FailedBillEventArgs eventArgs)
        {
            var res = DicHelper.FirstOrDefault(p => p.Key == eventArgs.InspectionTypeInt).Value?.GetFailedBillDetailInfos(eventArgs.BillId);
            if (res != null)
                return res;

            return new List<FailedBillDetailInfo>();
        }
    }

    /// <summary>
    /// 不合格审核数据方法类接口定义
    /// </summary>
    public interface IFailedListAuditData
    {
        /// <summary>
        /// 获取单据对应的处理方式
        /// </summary>
        /// <returns></returns>
        List<ProcessMode> GetProcessModeEnumList();

        /// <summary>
        /// 获取单据不合格项目信息
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        List<FailedBillDetailInfo> GetFailedBillDetailInfos(double billId);
    }

    /// <summary>
    /// 不合格审核单据抽象类,用于定义接口时指定类型
    /// </summary>
    [Serializable]
    public class InspBillAuditInfoBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected InspBillAuditInfoBase() { }
    }

    /// <summary>
    /// 不合格审核接口参数
    /// </summary>
    [Serializable]
    public class FailedBillEventArgs
    {
        /// <summary>
        /// 检验类型枚举对应值，整型格式
        /// </summary>
        public int InspectionTypeInt { get; set; }

        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }
    }

}
