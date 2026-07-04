using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Report.EquipmentMixReport
{
    /// <summary>
    /// ESD报表数据
    /// </summary>
    [Serializable]
    public class EmsMixReportInfo
    {
        /// <summary>
        /// ESD检测通过趋势率报表数据
        /// </summary>
        public EsdPassRateReportInfo EsdPassRateReport { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 设备数
        /// </summary>
        public string EquipmentCount { get; set; }

        /// <summary>
        /// 表格数据
        /// </summary>
        public EntityList<EquipmentMixReportMonViewModel> EsdNgList { get; set; }
    }

    #region 通过趋势率报表
    /// <summary>
    /// ESD检测通过趋势率报表数据
    /// </summary>
    [Serializable]
    public class EsdPassRateReportInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EsdPassRateReportInfo()
        {
            EsdPassRateDatas = new List<EsdPassRateDataInfo>();
        }
        /// <summary>
        /// 报表数据
        /// </summary>
        public List<EsdPassRateDataInfo> EsdPassRateDatas { get; set; }
    }

    /// <summary>
    /// 利用率信息
    /// </summary>
    [Serializable]
    public class EsdPassRateDataInfo
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 标准利用率
        /// </summary>
        public decimal UtilizationStandards { get; set; }


        /// <summary>
        /// 利用率
        /// </summary>
        public decimal UtilizationRate { get; set; }
    }
    #endregion
}
