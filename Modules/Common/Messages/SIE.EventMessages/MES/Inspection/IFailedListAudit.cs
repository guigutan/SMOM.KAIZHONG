using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EventMessages.Inspection
{
    /// <summary>
    /// 默认不合格审核接口实现
    /// </summary>
    public class DefaultFailedListAudit
    {
        /// <summary>
        /// 不合格审核方法类集合
        /// </summary>
        public static Dictionary<int, IFailedListAuditHelper> DicHelper { get; set; } = new Dictionary<int, IFailedListAuditHelper>();

        /// <summary>
        /// 获得对应单据ViewGroup和EntityType
        /// </summary>
        /// <returns>FailedBill</returns>
        public FailedBill GetFailedBillwDataByInspType(int InspectionTypeInt, double billId)
        {
            var viewData = DicHelper.FirstOrDefault(p => p.Key == InspectionTypeInt).Value?.GetFailedBillwDataByInspType(billId);
            if (viewData != null)
                return viewData;
            throw new InvalidOperationException("无法获取不合格审核视图信息。".L10N());
        }

        /// <summary>
        /// 获取单据相关信息
        /// </summary>
        /// <returns>FailedBill</returns>
        public object GetFailedBill(int InspectionTypeInt, double billId)
        {
            var viewData = DicHelper.FirstOrDefault(p => p.Key == InspectionTypeInt).Value?.GetFailedBill(billId);
            FailedListAuditDatas failedListAudit = new FailedListAuditDatas();
            failedListAudit.FailedBill = viewData;
            failedListAudit.VoDownloadRootUrl = RT.Config.Get("client.attachmentDownloadUrl");
            return failedListAudit;
        }
    }

    /// <summary>
    /// 不合格审核方法类接口定义
    /// </summary>
    public interface IFailedListAuditHelper
    {
        /// <summary>
        /// 获得对应单据ViewGroup和EntityType
        /// </summary>
        /// <returns></returns>
        FailedBill GetFailedBillwDataByInspType(double billId);

        /// <summary>
        /// 获取单据相关信息
        /// </summary>
        /// <returns></returns>
        object GetFailedBill(double billId);
    }

    /// <summary>
    /// 不合格审核Arg
    /// </summary>
    [Serializable]
    public class FailedBill
    {
        /// <summary>
        /// 对应不合格审核的ViewGroup
        /// </summary>
        public string ViewGroup { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }
    }

    /// <summary>
    /// 不合格审核数据处理
    /// </summary>
    public class FailedListAuditDatas
    {
        /// <summary>
        /// 不合格审核数据
        /// </summary>
        public object FailedBill { get; set; }

        /// <summary>
        /// 下载路径
        /// </summary>
        public string VoDownloadRootUrl { get; set; }
    }
}
