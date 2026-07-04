using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC
{
    /// <summary>
    /// 包装记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("包装记录查询实体")]
    public class PackingReportRecordCriterial : Criteria
    {
        #region 蓝标 BlueLabel
        /// <summary>
        /// 蓝标
        /// </summary>
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<PackingReportRecordCriterial >.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 包装报工类型 Report
        /// <summary>
        /// 包装报工类型
        /// </summary>
        [Label("包装报工类型")]
        public static readonly Property<ReportType?> ReportProperty = P<PackingReportRecordCriterial>.Register(e => e.Report);

        /// <summary>
        /// 包装报工类型
        /// </summary>
        public ReportType? Report
        {
            get { return this.GetProperty(ReportProperty); }
            set { this.SetProperty(ReportProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PackingQcController>().CriterialPackingReportRecord(this);
        }
    }
}
