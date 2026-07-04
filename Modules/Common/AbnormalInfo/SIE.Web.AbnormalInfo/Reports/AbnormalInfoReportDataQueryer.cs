using SIE.AbnormalInfo.Reports;
using SIE.Web.Data;
using SIE.Web.AbnormalInfo.Reports.ViewModels;
using SIE.Web.Json;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.AbnormalInfo.Reports
{
    /// <summary>
    /// 异常信息报表查询器
    /// </summary>
    public class AbnormalInfoReportDataQueryer : DataQueryer
    { /// <summary>
      /// 获取异常信息关闭率
      /// </summary> 
      /// <param name="criter">查询实体</param>
      /// <returns>异常信息关闭率</returns>
        public ReportViewModel GetAbnormalInfoReportData(AbnormalInfoReportCriteria criter)
        {
            ReportViewModel iqcreportviewmodel = new ReportViewModel();
            var stores = RT.Service.Resolve<AbnormalInfoReportController>().GetAbnormalReportRecord(criter);

            if (stores == null || stores.Count == 0)
            {
                return iqcreportviewmodel;
            }

            List<EntityJson> chartData = new List<EntityJson>();

            var data = stores.OrderBy(p => p.Date);

            string[] rowNameList = new string[] { "异常发生数", "异常关闭数", "异常关闭率" };
            foreach (var i in data)
            {
                EntityJson chartempnode = new EntityJson();
                chartempnode.SetProperty("monthDay", i.Date);
                chartempnode.SetProperty("totalQty", i.TotalQty);
                chartempnode.SetProperty("closeQty", i.CloseQty);
                chartempnode.SetProperty("closeRate", i.Rate);
                chartData.Add(chartempnode);

                foreach (var rowname in rowNameList)
                {
                    ProcessDataViewModel processdataviewmodel = new ProcessDataViewModel();
                    processdataviewmodel.Date = i.Date;
                    if (rowname == "异常关闭率")
                        processdataviewmodel.CloseData = decimal.Parse((i.Rate * 100).ToString("F2"));
                    else if (rowname == "异常关闭数")
                        processdataviewmodel.CloseData = i.CloseQty;
                    else if (rowname == "异常发生数")
                        processdataviewmodel.CloseData = i.TotalQty;
                    processdataviewmodel.ReportInfo = rowname;
                    iqcreportviewmodel.ProcessDataList.Add(processdataviewmodel);
                }
            }
            iqcreportviewmodel.ChartJsonData.AddRange(chartData);

            return iqcreportviewmodel;
        }
    }
}
