using SIE.EMS.MeteringEquipment.Calibrations.Criterias;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.Criterias
{
    /// <summary>
    /// 计量设备定检查询
    /// </summary>
    public class CalibrationCriteriaViewConfig : WebViewConfig<CalibrationCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.InspectionNo).Show(ShowInWhere.All);
            View.Property(p => p.InspectionStatus).Show(ShowInWhere.All);
            View.Property(p => p.InspectionResult).Show(ShowInWhere.All);
            View.Property(p => p.AgencyId).HasLabel("检验机构").Show(ShowInWhere.All);
            View.Property(p => p.PlanInspectionDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.Month;
            }).Show(ShowInWhere.All);
            View.Property(p => p.ActualInspectionDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.All;
            }).Show(ShowInWhere.All);
        }
    }
}
