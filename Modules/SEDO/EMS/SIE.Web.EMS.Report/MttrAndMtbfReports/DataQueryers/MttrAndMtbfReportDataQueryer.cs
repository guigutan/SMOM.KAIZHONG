using SIE.Domain;
using SIE.EMS.API.APIModels;
using SIE.EMS.API.APIs;
using SIE.EMS.Report.MttrAndMtbfReports;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SIE.Web.EMS.Report.MttrAndMtbfReports.DataQueryers
{
    /// <summary>
    /// MTTR/MTBF统计报表查询器
    /// </summary>
    public class MttrAndMtbfReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <returns></returns>
        public MttrAndMtbfReportInfo GetMttrAndMtbfReportData(MttrAndMtbfReportViewModelCriteria criteria)
        {
            var statiDic = RT.Service.Resolve<MttrAndMtbfReportViewModelController>().GetFaultStatisticsReport(criteria);
            var statiList = statiDic.First().Value;
            var reportInfo = new MttrAndMtbfReportInfo();
            reportInfo.MttrAndMtbfBarReport = new MttrAndMtbfBarReportInfo();
            reportInfo.MttrAndMtbfBarReport.MttrAndMtbfBarDatas = new List<EquipmentFaultStatistics>();
            reportInfo.MttrAndMtbfList = new EntityList<MttrAndMtbfReportViewModel>();
            reportInfo.MttrAndMtbfBarReport.MttrAndMtbfBarDatas.AddRange(statiList);
            reportInfo.EquipmentCount = statiDic.First().Key.ToString();

            EntityList<MttrAndMtbfReportViewModel> reportList = new EntityList<MttrAndMtbfReportViewModel>();

            reportList.Add(new MttrAndMtbfReportViewModel()
            {
                StatiItem = "故障次数".L10N()
            });

            reportList.Add(new MttrAndMtbfReportViewModel()
            {
                StatiItem = "维修时长(小时)".L10N()
            });

            reportList.Add(new MttrAndMtbfReportViewModel()
            {
                StatiItem = "设备故障总时间(小时)".L10N()
            });

            reportList.Add(new MttrAndMtbfReportViewModel()
            {
                StatiItem = "设备运行时长(小时)".L10N()
            });

            reportList.Add(new MttrAndMtbfReportViewModel()
            {
                StatiItem = "MTBF(小时)".L10N()
            });

            reportList.Add(new MttrAndMtbfReportViewModel()
            {
                StatiItem = "MTTR(小时)".L10N()
            });

            foreach (var data in reportList)
            {
                foreach (var item in statiList)
                {
                    //获取对应项目的值
                    var dataValue = GetItemValue(data, item);

                    switch (item.Month.Split('-')[1])
                    {
                        case "1":
                            data.One = dataValue;
                            break;
                        case "2":
                            data.Two = dataValue;
                            break;
                        case "3":
                            data.Three = dataValue;
                            break;
                        case "4":
                            data.Four = dataValue;
                            break;
                        case "5":
                            data.Five = dataValue;
                            break;
                        case "6":
                            data.Six = dataValue;
                            break;
                        case "7":
                            data.Seven = dataValue;
                            break;
                        case "8":
                            data.Eight = dataValue;
                            break;
                        case "9":
                            data.Nine = dataValue;
                            break;
                        case "10":
                            data.Ten = dataValue;
                            break;
                        case "11":
                            data.Eleven = dataValue;
                            break;
                        case "12":
                            data.Twelve = dataValue;
                            break;
                        default:
                            break;
                    }
                }
            }

            reportInfo.MttrAndMtbfList.AddRange(reportList);

            return reportInfo;
        }

        /// <summary>
        /// 获取对应项目的值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private decimal GetItemValue(MttrAndMtbfReportViewModel data, EquipmentFaultStatistics item)
        {
            decimal dataValue = data.StatiItem == "MTBF(小时)" ? (decimal)item.Mtbf : (decimal)item.Mttr;
            dataValue = data.StatiItem == "维修时长(小时)" ? (decimal)item.RepairTimeTotal : dataValue;
            dataValue = data.StatiItem == "故障次数" ? (decimal)item.Count : dataValue;
            dataValue = data.StatiItem == "设备故障总时间(小时)" ? (decimal)item.EquipMentFailureTime : dataValue;
            dataValue = data.StatiItem == "设备运行时长(小时)" ? (decimal)item.RunningTime : dataValue;
            return dataValue;
        }
    }
}
