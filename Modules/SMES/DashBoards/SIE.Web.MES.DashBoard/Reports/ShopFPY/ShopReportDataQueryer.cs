using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MES.DashBoard.TeamManagement;
using SIE.MES.Statistics.Fpy;
using SIE.ObjectModel;
using SIE.Web.Data;
using SIE.Web.Json;
using SIE.Web.MES.DashBoard.Reports.ShopFPY;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefectCategoryInfo = SIE.Web.MES.DashBoard.Reports.ShopFPY.DefectCategoryInfo;
using DefectCodeInfo = SIE.Web.MES.DashBoard.Reports.ShopFPY.DefectCodeInfo;

namespace SIE.Web.MES.DashBoard.Reports
{
    /// <summary>
    /// 客制查询数据处理
    /// </summary>
    public class ShopReportDataQueryer : DataQueryer
    {
        private const string DATE_FORMAT = "yyyyMMdd";
        private const string TD_TAG = "<td>";
        private const string TD_END = "</td>";

        /// <summary>
        /// 自定义颜色数组
        /// </summary>
        private readonly string[] _brushes = new string[] { "#FF0000", "#FF4500", "#FFA500", "#FFFF00", "#FFFFE0" };


        /// <summary>
        /// 获取评分记录数据
        /// </summary>
        /// <param name="criter">查询实体</param>
        /// <returns>评分记录信息</returns>
        public EntityJson GetShopReportData(ShopReportViewModelCriteria criter)
        {
            var stores = RT.Service.Resolve<ShopReportViewModelController>().GetShopDirectRateViewModel(criter);
            if (stores == null || stores.Count == 0)
            {
                EntityJson nullNode = new EntityJson();
                nullNode.SetProperty("gridData", string.Empty);
                nullNode.SetProperty("chartData", string.Empty);
                nullNode.SetProperty("chartIds", string.Empty);
                nullNode.SetProperty("chartNames", string.Empty);
                nullNode.SetProperty("columnDataIndexList", string.Empty);
                return nullNode;
            }

            stores.ForEach(p => { p.DirectRate = p.DirectRate * 100; });

            string nameList = string.Empty;
            string idList = string.Empty;
            ArrayList monthArrayList = new ArrayList();
            DateDataHandle(criter.DateType, stores, monthArrayList);
            int rowCount = monthArrayList.Count;
            int columnCount = stores.Select(p => p.LineId).Distinct().ToList().Count;
            int shopCount = stores.Select(p => p.ShopId).Distinct().ToList().Count;
            ////定义一个二维数组，ExtChart数据格式使用纵向数据，数据组成，行：时间轴+每一项的数据，列：第一列时间轴，后面每列代表每一项在时间轴的数据
            string[,] chartarr = new string[rowCount, columnCount + 1];
            string[,] settingAChartarr = new string[rowCount, columnCount + 1];
            string[,] settingDChartarr = new string[rowCount, columnCount + 1];
            string[,] shopChartarr = new string[rowCount, shopCount + 1];
            string[,] shopAChartarr = new string[rowCount, shopCount + 1];
            string[,] shopDChartarr = new string[rowCount, shopCount + 1];
            for (int i = 0; i < rowCount; i++)
            {
                ////第一列存时间轴
                chartarr[i, 0] = monthArrayList[i].ToString();
                settingAChartarr[i, 0] = monthArrayList[i].ToString();
                settingDChartarr[i, 0] = monthArrayList[i].ToString();
                shopChartarr[i, 0] = monthArrayList[i].ToString();
                shopAChartarr[i, 0] = monthArrayList[i].ToString();
                shopDChartarr[i, 0] = monthArrayList[i].ToString();
            }

            var dateList = stores.Select(p => p.Date).OrderBy(p => p).Distinct().ToList();
            string columnDataIndexList = InitColumnList(stores, criter.DateType, dateList, criter);
            ArrayList rowNameList = new ArrayList();
            List<EntityJson> res = GetResData(stores, ref nameList, ref idList, rowCount, chartarr, settingAChartarr, settingDChartarr, dateList, rowNameList);
            ArrayList shopNameList = GetShopNames(stores, rowCount, shopChartarr, shopAChartarr, shopDChartarr, dateList, res);

            List<EntityJson> chartData = new List<EntityJson>();
            for (int i = 0; i < rowCount; i++)
            {
                EntityJson chartempnode = new EntityJson();
                chartempnode.SetProperty("monthDay", chartarr[i, 0]);
                for (int k = 1; k <= columnCount; k++)
                {
                    chartempnode.SetProperty(rowNameList[k - 1].ToString(), chartarr[i, k]);
                    chartempnode.SetProperty("Alarm" + rowNameList[k - 1], settingAChartarr[i, k] == string.Empty ? null : settingAChartarr[i, k]);
                    chartempnode.SetProperty("Desired" + rowNameList[k - 1], settingDChartarr[i, k] == string.Empty ? null : settingDChartarr[i, k]);
                }
                for (int k = 1; k <= shopCount; k++)
                {
                    chartempnode.SetProperty(shopNameList[k - 1].ToString(), shopChartarr[i, k] == string.Empty ? null : shopChartarr[i, k]);
                    chartempnode.SetProperty("Alarm" + shopNameList[k - 1], shopAChartarr[i, k] == string.Empty ? null : shopAChartarr[i, k]);
                    chartempnode.SetProperty("Desired" + shopNameList[k - 1], shopDChartarr[i, k] == string.Empty ? null : shopDChartarr[i, k]);
                }

                chartData.Add(chartempnode);
            }

            EntityJson resNode = new EntityJson();
            resNode.SetProperty("gridData", res);
            resNode.SetProperty("chartData", chartData);
            resNode.SetProperty("chartIds", idList.TrimEnd(','));
            resNode.SetProperty("chartNames", nameList.TrimEnd(','));
            resNode.SetProperty("columnDataIndexList", columnDataIndexList.TrimEnd(','));
            return resNode;
        }

        /// <summary>
        /// 取车间信息
        /// </summary>
        /// <param name="stores"></param>
        /// <param name="rowCount"></param>
        /// <param name="shopChartarr"></param>
        /// <param name="shopAChartarr"></param>
        /// <param name="shopDChartarr"></param>
        /// <param name="dateList"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private static ArrayList GetShopNames(EntityList<ShopDirectRateViewModel> stores, int rowCount, string[,] shopChartarr, string[,] shopAChartarr, string[,] shopDChartarr, List<DateTime> dateList, List<EntityJson> res)
        {
            int j = 1;
            ArrayList shopNameList = new ArrayList();
            stores.GroupBy(p => p.ShopName).ForEach(shop =>
            {
                EntityJson node = new EntityJson();
                var first = shop.FirstOrDefault();
                node.SetProperty("lineName", "合计");
                node.SetProperty("lineId", -1);
                node.SetProperty("shopName", shop.Key);
                node.SetProperty("shopId", first.ShopId);
                shopNameList.Add("shop" + first.ShopId);
                var linelist = shop.ToList();
                string alarmValue = first.ShopDirectRate.Alarm.ToString("0.00");
                string desiredValue = first.ShopDirectRate.Desired.ToString("0.00");
                ArrayList dataArrList = new ArrayList();
                dateList.ForEach(e =>
                {
                    var lineDateData = linelist.Where(f => f.Date == e).ToList();
                    if (lineDateData.Count > 0)
                    {
                        var rate = (lineDateData.Sum(f => f.DirectRate) / lineDateData.Count);
                        node.SetProperty("Date" + e.Date.ToString(DATE_FORMAT), rate.ToString("###0.##"));
                        dataArrList.Add(rate);
                    }
                    else
                    {
                        dataArrList.Add(null);
                    }
                });
                res.Add(node);
                for (int i = 0; i < rowCount; i++)
                {
                    ////每一个人占一列数据
                    if (dataArrList[i] != null)
                    {
                        shopChartarr[i, j] = dataArrList[i].ToString();
                    }

                    shopAChartarr[i, j] = alarmValue;
                    shopDChartarr[i, j] = desiredValue;
                }
                j++;
            });
            return shopNameList;
        }

        /// <summary>
        /// 取员工分数信息
        /// </summary>
        /// <param name="stores"></param>
        /// <param name="nameList"></param>
        /// <param name="idList"></param>
        /// <param name="rowCount"></param>
        /// <param name="chartarr"></param>
        /// <param name="settingAChartarr"></param>
        /// <param name="settingDChartarr"></param>
        /// <param name="dateList"></param>
        /// <param name="rowNameList"></param>
        /// <returns></returns>
        private static List<EntityJson> GetResData(EntityList<ShopDirectRateViewModel> stores,  ref string nameList, ref string idList, int rowCount, string[,] chartarr, string[,] settingAChartarr, string[,] settingDChartarr, List<DateTime> dateList, ArrayList rowNameList)
        {
            List<EntityJson> res = new List<EntityJson>();
            int j = 1;
            StringBuilder sbName = new StringBuilder();
            StringBuilder sbId = new StringBuilder();
            stores.GroupBy(p => p.LineName).ForEach(store =>
            {
                EntityJson node = new EntityJson();
                var list = store.OrderBy(e => e.Date).ToList();
                var itemone = list.FirstOrDefault();
                node.SetProperty("lineName", store.Key);
                node.SetProperty("lineId", itemone.LineId);
                node.SetProperty("shopName", itemone.ShopName);
                node.SetProperty("shopId", itemone.ShopId);
                rowNameList.Add("line" + itemone.LineId);
                sbName.Append( store.Key + ",");
                sbId.Append(itemone.LineId + ",");
                string alarmValue = itemone.LineDirectRate.Alarm.ToString("0.00");
                string desiredValue = itemone.LineDirectRate.Desired.ToString("0.00");
                ArrayList dataArrList = new ArrayList();
                ////按时间排序，确保数据顺序一致，dataArrList按日期顺序存放当前员工分数
                List<string> listDate = new List<string>() { };
                dateList.ForEach(e =>
                {
                    var lineDateData = list.FirstOrDefault(f => f.Date == e);

                    if (lineDateData == null)
                    {
                        dataArrList.Add(null);
                        listDate.Add(null);
                    }
                    else
                    {
                        node.SetProperty("Date" + e.Date.ToString(DATE_FORMAT), (lineDateData.DirectRate).ToString("###0.##"));
                        listDate.Add(e.Date.ToShortDateString());
                        dataArrList.Add(lineDateData.DirectRate);
                    }

                });
                node.SetProperty("dateRange", listDate);
                res.Add(node);

                for (int i = 0; i < rowCount; i++)
                {
                    ////每一个人占一列数据
                    if (dataArrList[i] != null)
                    {
                        chartarr[i, j] = dataArrList[i].ToString();
                    }

                    settingAChartarr[i, j] = alarmValue;
                    settingDChartarr[i, j] = desiredValue;
                }
                j++;
            });
            nameList = sbName.ToString();
            idList = sbId.ToString();
            return res;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="criter">查询</param>
        /// <returns>table数据</returns>
        public string ExportShopReportRecords(ShopReportViewModelCriteria criter)
        {
            var stores = RT.Service.Resolve<ShopReportViewModelController>().GetShopDirectRateViewModel(criter);
            if (stores == null||stores.Count == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            string head = "<table><tr><td>车间</td><td>资源</td>";

            if (stores.Count > 0)
            {
                var dateList = stores.Select(p => p.Date).Distinct().ToList();
                dateList.OrderBy(p => p.Date).ForEach(f =>
                {
                    switch (criter.DateType)
                    {
                        case DateType.Year:
                            head += TD_TAG + f.Year + "年</td>";
                            break;
                        case DateType.Month:
                            head += TD_TAG + f.ToString("yyyy年MM月") + TD_END;
                            break;
                        case DateType.Week:
                            {
                                var week = stores.FirstOrDefault(p => p.Date == f);
                                head += TD_TAG + week.Week + TD_END;
                            }
                            break;
                        case DateType.Day:
                            head += TD_TAG + f.ToShortDateString() + TD_END;
                            break;
                    }
                });
                List<string> arr= stores.Select(p => p.ShopName).Distinct().ToList();
                arr.ForEach(s =>
                {
                    var lineList = stores.Where(f => f.ShopName == s).ToList();
                    lineList.ForEach(p =>
                    {
                        var list = stores.Where(e => e.LineName == p.LineName).ToList();
                        sb.Append("<tr><td>" + s + "</td><td>" + p.LineName + TD_END);
                        dateList.OrderBy(e => e.Date).ForEach(f =>
                        {
                            var data = list.FirstOrDefault(e => e.Date == f);
                            sb.Append(data == null ? "<td>&nbsp;</td>" : TD_TAG + data.DirectRate.ToString("P2") + TD_END);
                        });
                        sb.Append("</tr>");
                    });
                    sb.Append("<tr><td colspan='2'>车间合计</td>");
                    dateList.OrderBy(e => e.Date).ForEach(f =>
                    {
                        var workData = lineList.Where(p => p.Date == f).ToList();
                        string rate = "&nbsp;";
                        if (workData.Count != 0)
                        {
                            rate = (workData.Sum(p => p.DirectRate) / workData.Count).ToString("P2");
                        }

                        sb.Append(TD_TAG + rate + TD_END);
                    });
                    sb.Append("</tr>");
                });
                sb.Append("</tr></table>");
                sb = new StringBuilder(head + "</tr>" + sb);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 时间范围数据处理
        /// </summary>
        /// <param name="dT">查询类型</param>
        /// <param name="WorkOrderReachList"></param>
        /// <param name="monthArrayList">包含每个时间轴的数组</param>
        private void DateDataHandle(DateType dT, EntityList<ShopDirectRateViewModel> WorkOrderReachList, ArrayList monthArrayList)
        {
            if (dT == DateType.Day)
            {
                var dateList = WorkOrderReachList.Select(p => p.Date).Distinct().ToList();
                dateList.OrderBy(p => p).ForEach(p =>
                  {
                      string d = p.Year.ToString().Substring(2, 2) + "/" + p.Month + "/" + p.Day;
                      monthArrayList.Add(d);
                  });
            }
            else if (dT == DateType.Month)
            {
                var yearList = WorkOrderReachList.OrderBy(p => p.Date).Select(p => p.Month).ToList();
                yearList = yearList.Distinct().ToList();
                yearList.OrderBy(p => p).ForEach(p => monthArrayList.Add(p));
            }
            else if (dT == DateType.Year)
            {
                var yearList = WorkOrderReachList.OrderBy(p => p.Date).Select(p => p.Year).ToList();
                yearList = yearList.Distinct().ToList();
                yearList.OrderBy(p => p).ForEach(p => monthArrayList.Add(p));
            }
            else if (dT == DateType.Week)
            {
                var weekList = WorkOrderReachList.OrderBy(p => p.Date).Select(p => p.Week).ToList();
                weekList = weekList.Distinct().ToList();
                weekList.ForEach(p => monthArrayList.Add(p));
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 定义chart表头的内容
        /// </summary>
        /// <returns>表头内容</returns>
        private string InitColumnList(EntityList<ShopDirectRateViewModel> stores, DateType dtpye, List<DateTime> dateList, ShopReportViewModelCriteria criter)
        {
            string columnDataIndexList = string.Empty;
            dateList.ForEach(e =>
            {
                switch (dtpye)
                {
                    case DateType.Year:
                        columnDataIndexList += (e.Year + "年^Date" + e.Date.ToString(DATE_FORMAT) + ",");
                        break;
                    case DateType.Month:
                        {
                            string m = e.Month + "月";
                            if (criter.CollectDate.BeginValue.Value.Year != criter.CollectDate.EndValue.Value.Year)
                            {
                                m = e.ToString("yyyy年MM月");
                            }

                            columnDataIndexList += (m + "^Date" + e.Date.ToString(DATE_FORMAT) + ",");
                            break;
                        }
                    case DateType.Week:
                        {
                            var week = stores.FirstOrDefault(p => p.Date == e);
                            columnDataIndexList += week.Week + "^Date" + e.Date.ToString(DATE_FORMAT) + ",";
                        }
                        break;
                    case DateType.Day:
                        {
                            string m = e.Month + "月";
                            if (criter.CollectDate.BeginValue.Value.Year != criter.CollectDate.EndValue.Value.Year)
                            {
                                m = e.ToString("yyyy年MM月");
                            }

                            columnDataIndexList += (e.Date.Day + "号^Date" + e.Date.ToString(DATE_FORMAT) + "^" + m + ",");
                        }
                        break;
                    default:
                        break;
                }
            });
            return columnDataIndexList;
        }


        /// <summary>
        /// 获取车间报表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ShopFypInfo GetShopRates(ShopReportViewModelCriteria criteria)
        {
            ShopFypInfo lineVM = new ShopFypInfo();
            List<ShopDirectRateInfo> directRateVMs = new List<ShopDirectRateInfo>();
            EntityList<ProcessFpyStatistics> shopFpyStatistics;
            Dictionary<double, string> resNameDics;
            Dictionary<double, Resources.Enterprises.Enterprise> shopDics;
            var shopModel = RT.Service.Resolve<ShopReportViewModelController>().GetShopReportViewModel(criteria, out shopFpyStatistics, out resNameDics, out shopDics);
            if (shopModel == null)
            {
                return lineVM;
            }

            var lineDirectRates = shopModel.ShopDirectRateList;
            List<ShopFpySettingInfo> settings = new List<ShopFpySettingInfo>();
            foreach (var lineDirectRate in lineDirectRates)
            {
                directRateVMs.Add(CreateRateModel(lineDirectRate.LineName, lineDirectRate.ShopName, lineDirectRate.DirectRate, lineDirectRate.Date, false));

                if (settings.FirstOrDefault(p => p.Name == lineDirectRate.ShopName && p.IsShop) == null)
                {
                    settings.Add(CreateSettingModel(lineDirectRate.ShopName, true));
                }

                settings.Add(CreateSettingModel(lineDirectRate.LineName, false));

            }
            if (directRateVMs.Count > 0)
            {
                lineVM.DirectRateVMList.AddRange(directRateVMs);
            }

            if (settings.Count > 0)
            {
                lineVM.ShopFpySettingList.AddRange(settings);
            }

            return lineVM;
        }

        /// <summary>
        /// 取车间报表
        /// </summary>
        /// <param name="lineName"></param>
        /// <param name="shopName"></param>
        /// <param name="directRateVMs"></param>
        /// <param name="isShop"></param>
        /// <returns></returns>
        public List<ReportRateInfo> GetShopReport(string lineName, string shopName, List<ShopDirectRateInfo> directRateVMs, bool isShop)
        {
            List<ReportRateInfo> lineReportVMs = new List<ReportRateInfo>();
            var filterRatesVMs = directRateVMs.Where(p => p.LineName == lineName && p.ShopName == shopName);
            var lineNames = filterRatesVMs.Select(p => p.LineName).Distinct().ToList();
            var setting = RT.Service.Resolve<FpySettingController>().GetLineFpySettingByLineName(lineNames).FirstOrDefault();

            foreach (var filterRatesVM in filterRatesVMs)
            {
                var lineReport = new ReportRateInfo()
                {
                    XDate = filterRatesVM.Date.ToString("yyyy/MM/dd"),
                    YData = Math.Round(filterRatesVM.DirectRate)
                };

                if (setting != null)
                {
                    lineReport.YDesired = setting.Desired;
                    lineReport.YAlarm = setting.Alarm;
                }

                lineReportVMs.Add(lineReport);
            }

            return lineReportVMs;
        }

        /// <summary>
        /// 获取车间明细ViewModel
        /// </summary>
        /// <param name="date"></param>
        /// <param name="directRateVM"></param>
        /// <returns></returns>
        public ShopFypDetailInfo GetShopDetailViewModel(string date, ShopDirectRateInfo directRateVM)
        {
            ShopFypDetailInfo shopModel = new ShopFypDetailInfo();
            List<BarBasicInfo> barBasicVMs = new List<BarBasicInfo>();
            List<ColumnStackedInfo> columnStackedVMs = new List<ColumnStackedInfo>();
            var dateRange = ProcessDateTime(date, directRateVM.Date);
            var fpySouece = RT.Service.Resolve<FpyController>().GetShopFpyStatistics(directRateVM.ShopName, directRateVM.LineName, new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
            var defectStatistics = RT.Service.Resolve<FpyController>().GetDefectStatisticsForLine(dateRange, directRateVM.LineName);
            var groups = fpySouece.GroupBy(p => p.ProcessName).ToList();
            for (int i = 0; i < groups.Count; i++)
            {
                var argument = groups[i].Key;
                if (groups[i].Sum(p => p.InputQty) == 0)
                {
                    continue;
                }
                var value = groups[i].Sum(p => p.PassQty) / groups[i].Sum(p => p.InputQty);
                var passQty = groups[i].Sum(p => p.PassQty);
                var failedQty = groups[i].Sum(p => p.FailedQty);
                var barBasicVM = new BarBasicInfo()
                {
                    ProcessName = argument,
                    PasssRate = value * 100
                };

                barBasicVMs.Add(barBasicVM);
                var columnStackedVM = new ColumnStackedInfo()
                {
                    ProcessName = argument,
                    PassQty = passQty,
                    FailedQty = failedQty
                };

                columnStackedVMs.Add(columnStackedVM);
            }

            shopModel.BarBasicViewModel.AddRange(barBasicVMs);
            shopModel.ColumnStackedViewModel.AddRange(columnStackedVMs);
            var defect = GetDefectCategoryInfos(defectStatistics);

            shopModel.DefectCategory.AddRange(defect);
            return shopModel;
        }

        private ShopFpySettingInfo CreateSettingModel(string name, bool isShop)
        {
            ShopFpySettingInfo LineSet = new ShopFpySettingInfo();
            LineSet.Name = name;
            LineSet.IsShop = isShop;
            return LineSet;
        }

        private ShopDirectRateInfo CreateRateModel(string lineName, string shopName, decimal rate, DateTime dt, bool isShop)
        {
            var directRateVM = new ShopDirectRateInfo()
            {
                LineName = lineName,
                ShopName = shopName,
                DirectRate = rate * 100,
                Date = dt,
                IsShop = isShop
            };
            return directRateVM;
        }

        /// <summary>
        /// 获取缺陷分类及其相关信息列表
        /// </summary>
        /// <param name="defectStatistics">缺陷统计列表</param>
        /// <returns>缺陷分类及其相关信息列表</returns>
        public List<DefectCategoryInfo> GetDefectCategoryInfos(EntityList<DefectStatistics> defectStatistics)
        {
            List<DefectCategoryInfo> defectCategoryInfos = new List<DefectCategoryInfo>();
            var defectCategorysOfTopFive = defectStatistics.OrderByDescending(p => p.Qty).Take(5).ToList();
            var ctgGroups = defectCategorysOfTopFive.GroupBy(p => p.CategoryName).ToList();
            int count = ctgGroups.Count;

            for (int i = 0; i < count; i++)
            {
                var defectCategoryInfo = new DefectCategoryInfo();
                defectCategoryInfo.CategoryNo = i;
                defectCategoryInfo.CategoryName = ctgGroups[i].Key;
                defectCategoryInfo.ColorName = _brushes[i];
                var defectCtgQty = ctgGroups[i].Sum(q => q.Qty);
                if (count == 3)
                {
                    defectCategoryInfo.ColumnWidth = i == 2 ? 30 : 35;
                }
                else
                {
                    defectCategoryInfo.ColumnWidth = Math.Round((double)(100 / count), 2);
                }

                var dectGroups = ctgGroups[i].GroupBy(p => p.DefectName).OrderByDescending(p => p.Sum(q => q.Qty)).ToList();
                for (int j = 0; j < dectGroups.Count; j++)
                {
                    var deftQty = dectGroups[j].Sum(q => q.Qty);
                    var tempHeight = Math.Round((double)(deftQty / defectCtgQty), 2);
                    var defectCodeInfo = new DefectCodeInfo();
                    defectCodeInfo.DefectName = dectGroups[j].Key;
                    defectCodeInfo.RowHeight = tempHeight * 100;
                    defectCodeInfo.LineHeight = tempHeight * 250;
                    defectCodeInfo.IsLast = false;

                    if (j == dectGroups.Count - 1)
                    {
                        defectCodeInfo.IsLast = true;
                    }

                    defectCategoryInfo.DefectCodeList.Add(defectCodeInfo);
                }

                defectCategoryInfos.Add(defectCategoryInfo);
            }

            return defectCategoryInfos;
        }


        /// <summary>
        /// 获取查询日期范围
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="dt">来源日期</param>
        /// <returns>查询日期范围</returns>
        public Tuple<DateTime, DateTime> ProcessDateTime(string date, DateTime dt)
        {
            if (date.Contains("年"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddYears(1));
            }
            else if (date.Contains("月"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddMonths(1));
            }
            else if (date.Contains("周"))
            {
                int year = dt.Year;
                int week = int.Parse(date.Split('第', '周')[1]);
                return RT.Service.Resolve<CommonController>().GetFirstEndDayOfWeek(year, week);
            }
            else
            {
                return Tuple.Create(dt.Date, dt.Date.AddDays(1));
            }
        }

    }
}
