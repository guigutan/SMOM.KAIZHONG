using SIE.Domain.Validation;
using SIE.EMS.Report.SparePartMitReports;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Report.SparePartMitReports.DataQueryers
{
    /// <summary>
    /// MTTR/MTBF统计报表查询器
    /// </summary>
    public class SparePartMixReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public SparePartMixReportInfo GetSparePartMixReportData(SparePartMixReportViewModelCriteria criteria)
        {
            SparePartMixReportInfo sparePartMixReportInfo = new SparePartMixReportInfo();
            if (!criteria.BeginMonth.HasValue)
            {
                sparePartMixReportInfo.err = "查询条件【开始月份】不能为空，请选择！".L10N();
                sparePartMixReportInfo.IsSuccess = false;
                return sparePartMixReportInfo;
            }
            if (!criteria.EndMonth.HasValue)
            {
                sparePartMixReportInfo.err = "查询条件【结束月份】不能为空，请选择！".L10N();
                sparePartMixReportInfo.IsSuccess = false;
                return sparePartMixReportInfo;
            }
            if (!criteria.WarehouseId.HasValue)
            {
                sparePartMixReportInfo.err = "查询条件【仓库】不能为空，请选择！".L10N();
                sparePartMixReportInfo.IsSuccess = false;
                return sparePartMixReportInfo;
            }
            if(criteria.EndMonth< criteria.BeginMonth)
            {
                sparePartMixReportInfo.err = "【开始月份】须小于【结束月份】，请确认！".L10N();
                sparePartMixReportInfo.IsSuccess = false;
                return sparePartMixReportInfo;
            }


            return RT.Service.Resolve<SpartPartMixReportViewModelController>().GetSpartPartMitReport(criteria);


        }
    }
}
