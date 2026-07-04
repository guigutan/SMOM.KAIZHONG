using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MES.Statistics.Fpy;
using SIE.ObjectModel;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.DashBoard.Reports.ProductFPY.DataQuery
{
    /// <summary>
    /// 产线直通率控制器
    /// </summary>
    public class ProductReportDataQueryer : DataQueryer
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
        public List<ProductionProductRateInfo> GetProductionProductRateInfos(ProductReportViewModelCriteria criteria)
        {
            List<string> ProductNames = new List<string>();
            List<ProductionProductRateInfo> prodProductRateInfos = new List<ProductionProductRateInfo>();
            var productDirectRates = RT.Service.Resolve<ProdReportViewModelController>().GetProdReportViewModel(criteria).ProdDirectRateList.OrderBy(p => p.Date);

            //创建产线直通率列表
            foreach (var productDirectRate in productDirectRates)
            {
                var prodProductRateInfo = new ProductionProductRateInfo()
                {
                    ProductModelName = productDirectRate.ProductModel,
                    ProductName = productDirectRate.Product,
                    Date = productDirectRate.Date,
                    DirectRate =Math.Round( productDirectRate.DirectRate * 100,3),
                };

                prodProductRateInfos.Add(prodProductRateInfo);
            }

            var productNames = productDirectRates.Select(p => p.Product).Distinct().ToList();
            var dicProductNameSettings = RT.Service.Resolve<FpySettingController>().GetAllProductFpySetting(productNames);
            //创建折线图信息列表
            prodProductRateInfos.GroupBy(x => new { x.ProductModelName, x.ProductName }).ForEach(p =>
            {
                p.ForEach(e =>
                {
                    if (!ProductNames.Contains(e.ProductName))
                    {
                        productNames.Add(e.ProductName);
                        FpySetting productFpySets = null;
                        if (dicProductNameSettings.TryGetValue(e.ProductName, out productFpySets))
                        {
                            var productChartSettingInfo = new ProductChartSettingInfo()
                            {
                                ProductName = e.ProductName,
                                Desired = productFpySets.Desired,
                                Alarm = productFpySets.Alarm
                            };
                            e.ProductChartSettingInfo = productChartSettingInfo;
                        }
                    }

                    var productChartInfo = new ProductChartInfo()
                    {
                        XDate = e.Date.ToString("yyyy/MM/dd"),
                        YData = Math.Round(e.DirectRate)
                    };

                    e.ProductChartInfoList.Add(productChartInfo);
                });
            });

            return prodProductRateInfos;
        }

        /// <summary>
        /// 获取工序相关列表数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="isHasShift">是否包含班次</param>
        /// <param name="prodProductRateInfo">产线直通率信息</param>
        /// <returns>工序相关信息</returns>
        public ProcessRelatedInfo GetProcessRelatedInfo(string date, bool isHasShift, ProductionProductRateInfo prodProductRateInfo)
        {
            var fpyCt = RT.Service.Resolve<FpyController>();
            ProcessRelatedInfo processRelatedInfo = new ProcessRelatedInfo();
            EntityList<DefectStatistics> defectStatistics = new EntityList<DefectStatistics>();
            EntityList<ProcessFpyStatistics> processFpyStatistics = new EntityList<ProcessFpyStatistics>();
            var dateRange = ProcessDateTime(date, prodProductRateInfo);
            if (isHasShift)
            {
                processFpyStatistics.AddRange(fpyCt.GetProdcutFpyStatistics(prodProductRateInfo.ProductModelName, prodProductRateInfo.ProductName, new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 }));
                defectStatistics.AddRange(fpyCt.GetDefectStatisticsForProd(dateRange, prodProductRateInfo.ProductModelName, prodProductRateInfo.ProductName));
            }
            else
            {
                processFpyStatistics.AddRange(fpyCt.GetProdcutFpyStatistics(prodProductRateInfo.ProductModelName, "", dateRange: new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 }));
                defectStatistics.AddRange(fpyCt.GetDefectStatisticsForProd(dateRange, prodProductRateInfo.ProductModelName, ""));
            }

            var groups = processFpyStatistics.GroupBy(p => p.ProcessName).ToList();
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
                    PasssRate =Math.Round( value * 100,2)
                };
                processRelatedInfo.ProcessDirectRateInfoList.Add(processDirectRateInfo);
            }
            processRelatedInfo.DefectInfoList.AddRange(GetDefectInfos(defectStatistics));

            return processRelatedInfo;
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
        /// 获取查询日期范围
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="prodProductRateInfo">产线直通率信息</param>
        /// <returns>查询日期范围</returns>
        protected Tuple<DateTime, DateTime> ProcessDateTime(string date, ProductionProductRateInfo prodProductRateInfo)
        {
            if (date != null && date.Contains("年"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddYears(1));
            }
            else if (date != null && date.Contains("月"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddMonths(1));
            }
            else if (date != null && prodProductRateInfo != null && date.Contains("周"))
            {
                int year = prodProductRateInfo.Date.Year;
                int week = int.Parse(date.Split('第', '周')[1]);
                return RT.Service.Resolve<CommonController>().GetFirstEndDayOfWeek(year, week);
            }
            else if(prodProductRateInfo!=null)
            {
                return Tuple.Create(prodProductRateInfo.Date, prodProductRateInfo.Date.AddDays(1));
            }
            else
            {
                return new Tuple<DateTime, DateTime>(DateTime.Now,DateTime.Now);
            }
        }
    }
}
