using SIE.Domain;
using SIE.EMS.API.APIModels;
using SIE.EMS.Report.MttrAndMtbfReports;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Report.MttrAndMtbfReports.DataQueryers
{
    /// <summary>
    /// MTTR/MTBF统计报表数据
    /// </summary>
    [Serializable]
    public class MttrAndMtbfReportInfo
    {
        /// <summary>
        /// 设备数
        /// </summary>
        public string EquipmentCount { get; set; }

        /// <summary>
        /// MTTR/MTBF报表数据
        /// </summary>
        public MttrAndMtbfBarReportInfo MttrAndMtbfBarReport { get; set; }

        /// <summary>
        /// MTTR/MTBF统计明细
        /// </summary>
        public EntityList<MttrAndMtbfReportViewModel> MttrAndMtbfList { get; set; }
    }

    #region MTTR/MTBF报表
    /// <summary>
    /// MTTR/MTBF统计报表数据
    /// </summary>
    [Serializable]
    public class MttrAndMtbfBarReportInfo
    {
        /// <summary>
        /// 报表数据
        /// </summary>
        public List<EquipmentFaultStatistics> MttrAndMtbfBarDatas { get; set; }
    }

    #endregion

}
