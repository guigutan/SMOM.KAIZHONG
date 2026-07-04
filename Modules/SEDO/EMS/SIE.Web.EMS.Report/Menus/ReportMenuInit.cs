using SIE.Common.Menus;
using System.Collections.Generic;
using SIE.EMS.Report.EquipCostAnalyses;
using SIE.EMS.Report.EquipmentIntegrateStatistics;
using SIE.EMS.Report.EquipmentMixReport;
using SIE.EMS.Report.FaultCountReports;
using SIE.EMS.Report.MttrAndMtbfReports;
using SIE.EMS.Report.SparePartMitReports;
using SIE.EMS.Report.WorkOrderExcuteReports;

namespace SIE.Web.EMS.Report
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class ReportMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "设备综合统计报表(年)",
                EntityType = typeof(EquipmentMixReportMonViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "MTTR/MTBF统计报表",
                EntityType = typeof(MttrAndMtbfReportViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "设备综合统计",
                EntityType = typeof(EquipmentIntegrateStatistic)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "设备成本分析",
                EntityType = typeof(EquipCostAnalyse)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "故障统计报表",
                EntityType = typeof(FaultCountReport)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "备件库综合报表",
                EntityType = typeof(SparePartMixtReportViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "工单执行统计报表",
                EntityType = typeof(WorkOrderExcuteReportViewModel)
            });

            return res;
        }

    }
}
