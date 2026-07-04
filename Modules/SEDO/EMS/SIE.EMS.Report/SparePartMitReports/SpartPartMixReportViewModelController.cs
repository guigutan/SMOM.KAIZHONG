using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.Report.SparePartMitReports
{
    /// <summary>
    /// 备件库综合报表控制器
    /// </summary>
    public class SpartPartMixReportViewModelController : DomainController
    {
        /// <summary>
        /// 获取备件库统计列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual SparePartMixReportInfo GetSpartPartMitReport(SparePartMixReportViewModelCriteria criteria)
        {
            var sparePartMixReportInfo = new SparePartMixReportInfo();
            sparePartMixReportInfo.ClounNameList.Add("月份".L10N());
            List<SparePartMixtReportViewModel> results = new List<SparePartMixtReportViewModel>();
            var foreachMonth = new DateTime(criteria.BeginMonth.Value.Year, criteria.BeginMonth.Value.Month, 1);
            var end = criteria.EndMonth.Value.AddMonths(1);
            var endMonth = new DateTime(end.Year, end.Month, 1);
            while (foreachMonth < endMonth)
            {
                //月出库总额/(期初 + 期末) / 2
                //本月第一天到下个月第一天闭开区间
                var monthBegin = foreachMonth.Date;//本月第一天
                var monthEnd = foreachMonth.AddMonths(1).Date;//下个月第一天
                var sparePartMixReportRecordList = Query<SparePartMixReportRecord>().Where(m => m.SchedulingDate >= monthBegin && m.SchedulingDate < monthEnd && m.WarehouseId == criteria.WarehouseId).ToList();
                SparePartMixtReportViewModel resItem;
                if (!sparePartMixReportRecordList.Any())
                {
                    resItem = new SparePartMixtReportViewModel()
                    {
                        SummaryTime = foreachMonth.Date,
                        Month = monthBegin.Month,
                        Year = monthBegin.Year,
                    };
                }
                else
                {

                    var monthEndItme = sparePartMixReportRecordList.OrderByDescending(m => m.SchedulingDate).First();//月末对象
                    var monthStartItme = sparePartMixReportRecordList.OrderByDescending(m => m.SchedulingDate).Last();//月初对象
                    resItem = new SparePartMixtReportViewModel()
                    {
                        SummaryTime = foreachMonth.Date,
                        Month = monthBegin.Month,
                        Year = monthBegin.Year,
                        ExWarehouseAmount = sparePartMixReportRecordList.Sum(m => m.ExWarehouseAmount),
                        ExWarehouseQty = sparePartMixReportRecordList.Sum(m => m.ExWarehouseQty),
                        MonthSurplusAmount = monthEndItme.SurplusAmount,
                        MonthSurplusQty = monthEndItme.SurplusQty,
                        ReceiptAmount = sparePartMixReportRecordList.Sum(m => m.ReceiptAmount),
                        ReceiptQty = sparePartMixReportRecordList.Sum(m => m.ReceiptQty),

                    };
                    resItem.TurnoverRate = (monthEndItme.SurplusAmount + monthStartItme.SurplusAmount)!=0?(resItem.ExWarehouseAmount / (monthEndItme.SurplusAmount + monthStartItme.SurplusAmount) / 2) * 100:0;

                }
                sparePartMixReportInfo.ExWarehouseList.Add(item: new MonthValue(resItem.TimeDispaly, Math.Round(resItem.ExWarehouseAmount, 2)));
                sparePartMixReportInfo.TurnoverRateList.Add(item: new MonthValue(resItem.TimeDispaly,Math.Round(resItem.TurnoverRate,2)));
                results.Add(resItem);

                //构建动态表头列

                sparePartMixReportInfo.ClounNameList.Add(foreachMonth.ToString("yyyy年MM月").L10N());
                foreachMonth = foreachMonth.AddMonths(1);
            }


            List<List<string>> rowResult = new List<List<string>>();//行转为列

            var fields = typeof(SparePartMixtReportViewModel).GetFields();
            var notMatchFields = new List<string>(){
                    "SummaryTimeProperty",
                    "MonthProperty",
                        "YearProperty",
                        "TimeDispalyProperty"
                };//不需要生成列的字段

            var timeList = results.Select(m => m.SummaryTime).Distinct().ToList();

            foreach (var field in fields)
            {
                var fieldName = field.Name;
                if (notMatchFields.Contains(fieldName)) continue;
                var label = field.CustomAttributes.FirstOrDefault(m => m.AttributeType.FullName == typeof(LabelAttribute).FullName);
                var name = label != null ? label.ConstructorArguments.First().Value.ToString().L10N(): "";

                var row = new List<string>() { name };
                foreach (var ti in timeList)
                {
                    var resItme = results.FirstOrDefault(m => m.SummaryTime == ti);
                    if (resItme != null)
                    {
                        var value = AnalyseCommonHelper.GetStatisticProperties(resItme, field.Name);
                        if (fieldName == "TurnoverRateProperty")
                        {

                            row.Add(value != 0 ? Math.Round(value, 2).ToString() + "%" : "0");
                        }
                        else
                        {
                            row.Add(Math.Round(value, 2).ToString());
                        }
                    }

                }
                rowResult.Add(row);

            }
            sparePartMixReportInfo.Datas = rowResult;
            return sparePartMixReportInfo;
        }

    }
}