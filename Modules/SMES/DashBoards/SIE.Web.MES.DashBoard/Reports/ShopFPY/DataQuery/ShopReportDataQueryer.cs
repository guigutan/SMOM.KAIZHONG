using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MES.Statistics.Fpy;
using SIE.ObjectModel;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.DashBoard.Reports.ShopFPY.DataQuery
{
    /// <summary>
    /// 产线直通率控制器
    /// </summary>
    public class ShopReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 自定义颜色数组
        /// </summary>
        private readonly string[] _brushes = new string[] { "#FF0000", "#FF4500", "#FFA500", "#FFFF00", "#FFFFE0" };

        /// <summary>
        /// 获取产线直通率及其相关数据列表
        /// </summary>
        /// <param name="criteria">查询对象</param>
        /// <returns>产线直通率列表</returns>
        public List<ShopRateInfo> GetShopRateInfos(ShopReportViewModelCriteria criteria)
        {
            List<string> resourceNames = new List<string>();
            List<ShopRateInfo> shopRateInfos = new List<ShopRateInfo>();
            EntityList<ProcessFpyStatistics> shopFpyStatistics;
            Dictionary<double, string> resNameDics;
            Dictionary<double, Resources.Enterprises.Enterprise> shopDics;
            var shopDirectRates = RT.Service.Resolve<ShopReportViewModelController>()
                .GetShopReportViewModel(criteria, out shopFpyStatistics, out resNameDics, out shopDics)
                .ShopDirectRateList.OrderBy(p => p.Date);

            //创建产线直通率列表
            foreach (var shopDirectRate in shopDirectRates)
            {
                var prodProductRateInfo = new ShopRateInfo()
                {
                    ShopName = shopDirectRate.ShopName,
                    ResourceName = shopDirectRate.LineName,
                    Date = shopDirectRate.Date,
                    DirectRate = shopDirectRate.DirectRate * 100,
                };

                shopRateInfos.Add(prodProductRateInfo);
            }

            var lineNames = shopDirectRates.Select(p => p.LineName).Distinct().ToList();
            var dicLineNameSettings = RT.Service.Resolve<FpySettingController>().GetShopFpySettings(lineNames);

            //创建折线图信息列表
            shopRateInfos.GroupBy(x => new { x.ShopName, x.ResourceName }).ForEach(p =>
            {
                p.ForEach(e =>
                {
                    if (!resourceNames.Contains(e.ResourceName))
                    {
                        resourceNames.Add(e.ResourceName);
                        FpySetting fpySetting = null;
                        if (dicLineNameSettings.TryGetValue(e.ResourceName, out fpySetting))
                        {
                            var shopChartSettingInfo = new ShopChartSettingInfo()
                            {
                                ResourceName = e.ResourceName,
                                Desired = fpySetting.Desired,
                                Alarm = fpySetting.Alarm
                            };

                            e.ShopChartSettingInfo = shopChartSettingInfo;
                        }
                    }

                    var shopChartInfo = new ShopChartInfo()
                    {
                        XDate = e.Date.ToString("yyyy/MM/dd"),
                        YData = Math.Round(e.DirectRate)
                    };

                    e.ShopChartInfoList.Add(shopChartInfo);
                });
            });

            return shopRateInfos;
        }

        /// <summary>
        /// 获取工序相关列表数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="isHasShift">是否包含班次</param>
        /// <param name="prodProductRateInfo">产线直通率信息</param>
        /// <returns>工序相关信息</returns>
        public ProcessRelatedInfo GetProcessRelatedInfo(string date, bool isHasShift, ShopRateInfo prodProductRateInfo)
        {
            var fpyCt = RT.Service.Resolve<FpyController>();
            ProcessRelatedInfo processRelatedInfo = new ProcessRelatedInfo();
            EntityList<DefectStatistics> defectStatistics = new EntityList<DefectStatistics>();
            EntityList<ProcessFpyStatistics> processFpyStatistics = new EntityList<ProcessFpyStatistics>();
            var dateRange = ProcessDateTime(date, prodProductRateInfo);
            if (isHasShift)
            {
                processFpyStatistics.AddRange(fpyCt.GetShopFpyStatistics(prodProductRateInfo.ShopName, prodProductRateInfo.ResourceName, new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 }));
                defectStatistics.AddRange(fpyCt.GetDefectStatisticsForProd(dateRange, prodProductRateInfo.ShopName, prodProductRateInfo.ResourceName));
            }
            else
            {
                processFpyStatistics.AddRange(fpyCt.GetShopFpyStatistics(prodProductRateInfo.ShopName, "", dateRange: new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 }));
                defectStatistics.AddRange(fpyCt.GetDefectStatisticsForProd(dateRange, prodProductRateInfo.ShopName, ""));
            }

            var groups = processFpyStatistics.GroupBy(p => p.ProcessName).ToList();
            for (int i = 0; i < groups.Count; i++)
            {
                var argument = groups[i].Key;
                if (groups[i].Sum(p => p.InputQty) == 0) continue;
                var value = groups[i].Sum(p => p.PassQty) / groups[i].Sum(p => p.InputQty);
                var passQty = groups[i].Sum(p => p.PassQty);
                var failedQty = groups[i].Sum(p => p.FailedQty);

                var processStatisticsInfo = new ProcessStatisticsInfo()
                {
                    ProcessName = argument,
                    PassQty = passQty,
                    FailedQty = failedQty
                };
                processRelatedInfo.ProcessStatisticsInfoList.Add(processStatisticsInfo);

                var processDirectRateInfo = new ProcessDirectRateInfo()
                {
                    ProcessName = argument,
                    PasssRate =Math.Round( value * 100,3)
                };
                processRelatedInfo.ProcessDirectRateInfoList.Add(processDirectRateInfo);
            }
            processRelatedInfo.DefectInfoList.AddRange(GetDefectInfos(defectStatistics));

            return processRelatedInfo;
        }

        /// <summary>
        /// 获取缺陷信息列表
        /// </summary>
        /// <param name="defectStatistics">缺陷统计列表</param>
        /// <returns>缺陷信息列表</returns>
        public List<DefectInfo> GetDefectInfos(EntityList<DefectStatistics> defectStatistics)
        {
            List<DefectInfo> defectInfos = new List<DefectInfo>();
            var defectsOfTopFive = defectStatistics.GroupBy(p => new { p.CategoryId, p.DefectName })
                .Select(p => new { DefectName = p.Key.DefectName, Qty = p.Count(), DetailList = p.OrderByDescending(q => q.DefectName).ToList() })
                .OrderByDescending(p => p.Qty).Take(5)
                .ToList();
            var sumQty = defectsOfTopFive.Sum(p => p.Qty);
            decimal cumQty = 0m;

            for (int i = 0; i < defectsOfTopFive.Count; i++)
            {
                cumQty += defectsOfTopFive[i].Qty;
                var cumPercent = Math.Round((cumQty / sumQty) * 100);
                var defectInfo = new DefectInfo()
                {
                    DefectName = defectsOfTopFive[i].DefectName,
                    Qty = defectsOfTopFive[i].Qty,
                    CumQty = cumQty,
                    CumPercent = cumPercent
                };

                defectInfos.Add(defectInfo);
            }

            return defectInfos;
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
                    if (i == 2)
                        defectCategoryInfo.ColumnWidth = 30;
                    else
                        defectCategoryInfo.ColumnWidth = 35;
                }
                else
                {
                    defectCategoryInfo.ColumnWidth = Math.Round((double)(100 / count), 2);
                }

                var dectGroups = ctgGroups[i].GroupBy(p => p.DefectName).OrderByDescending(p => p.Sum(q => q.Qty)).ToList();
                for (int j = 0; j < dectGroups.Count; j++)
                {
                    var deftQty = dectGroups[j].Sum(q => q.Qty);
                    if (defectCtgQty == 0)
                    {
                        continue;
                    }
                    var tempHeight = Math.Round((double)(deftQty / defectCtgQty), 2);
                    var defectCodeInfo = new DefectCodeInfo();
                    defectCodeInfo.DefectName = dectGroups[j].Key;
                    defectCodeInfo.RowHeight = tempHeight * 100;
                    defectCodeInfo.LineHeight = tempHeight * 250;
                    defectCodeInfo.IsLast = false;

                    if (j == dectGroups.Count - 1)
                        defectCodeInfo.IsLast = true;

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
        /// <param name="prodProductRateInfo">产线直通率信息</param>
        /// <returns>查询日期范围</returns>
        protected Tuple<DateTime, DateTime> ProcessDateTime(string date, ShopRateInfo prodProductRateInfo)
        {
            if (date !=null && date.Contains("年"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddYears(1));
            }
            else if (date != null && date.Contains("月"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddMonths(1));
            }
            else if (date != null && date.Contains("周"))
            {
                int year = prodProductRateInfo.Date.Year;
                int week = int.Parse(date.Split('第', '周')[1]);
                return RT.Service.Resolve<CommonController>().GetFirstEndDayOfWeek(year, week);
            }
            else
            {
                return Tuple.Create(prodProductRateInfo.Date, prodProductRateInfo.Date.AddDays(1));
            }
        }
    }
}
