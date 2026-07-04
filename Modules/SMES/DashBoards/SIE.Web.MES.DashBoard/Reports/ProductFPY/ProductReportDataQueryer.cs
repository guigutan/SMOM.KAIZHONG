using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MES.DashBoard.TeamManagement;
using SIE.MES.Statistics.Fpy;
using SIE.ObjectModel;
using SIE.Web.Data;
using SIE.Web.Json;
using SIE.Web.MES.DashBoard.Reports.ProductFPY;
using SIE.Web.MES.DashBoard.Reports.ShopFPY;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.DashBoard.Reports
{
    /// <summary>
    /// 客制查询数据处理
    /// </summary>
    public class ProductReportDataQueryer : DataQueryer
    {
        private const string FORMAT_DATE = "yyyyMMdd";
        private const string END_TD = "</td>";
        private const string TAG_TD = "<td>";

        /// <summary>
        /// 自定义颜色数组
        /// </summary>
        protected readonly string[] _brushes = new string[] { "#FF0000", "#FF4500", "#FFA500", "#FFFF00", "#FFFFE0" };


        /// <summary>
        /// 获取评分记录数据
        /// </summary>
        /// <param name="criter">查询实体</param>
        /// <returns>评分记录信息</returns>
        public EntityJson GetProductReportData(ProductReportViewModelCriteria criter)
        {
            var stores = RT.Service.Resolve<ProdReportViewModelController>().GetProdReportList(criter);
            if (stores == null || stores.Count == 0 || criter == null)
            {
                EntityJson nullNode = new EntityJson();
                nullNode.SetProperty("gridData", string.Empty);
                nullNode.SetProperty("chartData", string.Empty);
                nullNode.SetProperty("chartIds", string.Empty);
                nullNode.SetProperty("chartNames", string.Empty);
                nullNode.SetProperty("columnDataIndexList", string.Empty);
                return nullNode;
            }

            stores.ForEach(p =>
            {
                p.DirectRate = p.DirectRate * 100;
            });
            List<EntityJson> res = new List<EntityJson>();
            List<EntityJson> chartData = new List<EntityJson>();
            string nameList = string.Empty;
            string idList = string.Empty;
            ArrayList monthArrayList = new ArrayList();
            DateDataHandle(criter.DateType, stores, monthArrayList);
            int rowCount = monthArrayList.Count;
            int columnCount = stores.Select(p => p.ProductModelId).Distinct().ToList().Count;
            int productCount = stores.Select(p => p.ProductId).Distinct().ToList().Count;
            ////定义一个二维数组，ExtChart数据格式使用纵向数据，数据组成，行：时间轴+每一项的数据，列：第一列时间轴，后面每列代表每一项在时间轴的数据
            string[,] chartarr = new string[rowCount, columnCount + 1];
            string[,] settingAChartarr = new string[rowCount, columnCount + 1];
            string[,] settingDChartarr = new string[rowCount, columnCount + 1];
            string[,] productChartarr = new string[rowCount, productCount + 1];
            string[,] productAChartarr = new string[rowCount, productCount + 1];
            string[,] productDChartarr = new string[rowCount, productCount + 1];
            for (int i = 0; i < rowCount; i++)
            {
                ////第一列存时间轴
                chartarr[i, 0] = monthArrayList[i].ToString();
                settingAChartarr[i, 0] = monthArrayList[i].ToString();
                settingDChartarr[i, 0] = monthArrayList[i].ToString();
                productChartarr[i, 0] = monthArrayList[i].ToString();
                productAChartarr[i, 0] = monthArrayList[i].ToString();
                productDChartarr[i, 0] = monthArrayList[i].ToString();
            }

            var dateList = stores.Select(p => p.Date).OrderBy(p => p).Distinct().ToList();
            string columnDataIndexList = InitColumnList(stores, criter.DateType, dateList, criter);
            ArrayList rowNameList = GetNameList(stores, res, ref nameList, ref idList, rowCount, chartarr, settingAChartarr, settingDChartarr, dateList);
            ArrayList productNameList = GetProductNames(stores, res, rowCount, productChartarr, productAChartarr, productDChartarr, dateList);

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

                for (int k = 1; k <= productCount; k++)
                {
                    chartempnode.SetProperty(productNameList[k - 1].ToString(), productChartarr[i, k] == string.Empty ? null : productChartarr[i, k]);
                    chartempnode.SetProperty("Alarm" + productNameList[k - 1], productAChartarr[i, k] == string.Empty ? null : productAChartarr[i, k]);
                    chartempnode.SetProperty("Desired" + productNameList[k - 1], productDChartarr[i, k] == string.Empty ? null : productDChartarr[i, k]);
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
        /// 取产品名称信息
        /// </summary>
        /// <param name="stores"></param>
        /// <param name="res"></param>
        /// <param name="rowCount"></param>
        /// <param name="productChartarr"></param>
        /// <param name="productAChartarr"></param>
        /// <param name="productDChartarr"></param>
        /// <param name="dateList"></param>
        /// <returns></returns>
        private static ArrayList GetProductNames(EntityList<ProductDirectRateViewModel> stores, List<EntityJson> res, int rowCount, string[,] productChartarr, string[,] productAChartarr, string[,] productDChartarr, List<DateTime> dateList)
        {
            int j = 1;
            ArrayList productNameList = new ArrayList();
            stores.GroupBy(p => p.ProductModel).ForEach(product =>
            {
                EntityJson node = new EntityJson();
                var first = product.FirstOrDefault();
                node.SetProperty("lineName", "合计");
                node.SetProperty("lineId", -1);
                node.SetProperty("productName", product.Key);
                node.SetProperty("productId", first.ProductId);
                productNameList.Add("product" + first.ProductId);
                var linelist = product.ToList();
                string alarmValue = first.ProductDirectRate.Alarm.ToString("0.00");
                string desiredValue = first.ProductDirectRate.Desired.ToString("0.00");
                ArrayList dataArrList = new ArrayList();
                dateList.ForEach(e =>
                {
                    var lineDateData = linelist.Where(f => f.Date == e).ToList();
                    if (lineDateData.Count > 0)
                    {
                        var rate = (lineDateData.Sum(f => f.DirectRate) / lineDateData.Count);
                        node.SetProperty("Date" + e.Date.ToString(FORMAT_DATE), rate.ToString("###0.##"));
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
                        productChartarr[i, j] = dataArrList[i].ToString();
                    }

                    productAChartarr[i, j] = alarmValue;
                    productDChartarr[i, j] = desiredValue;
                }

                j++;
            });
            return productNameList;
        }

        /// <summary>
        /// 获取员工分数等信息
        /// </summary>
        /// <param name="stores"></param>
        /// <param name="res"></param>
        /// <param name="nameList"></param>
        /// <param name="idList"></param>
        /// <param name="rowCount"></param>
        /// <param name="chartarr"></param>
        /// <param name="settingAChartarr"></param>
        /// <param name="settingDChartarr"></param>
        /// <param name="dateList"></param>
        /// <returns></returns>
        private static ArrayList GetNameList(EntityList<ProductDirectRateViewModel> stores, List<EntityJson> res, ref string nameList, ref string idList, int rowCount, string[,] chartarr, string[,] settingAChartarr, string[,] settingDChartarr, List<DateTime> dateList)
        {
            var rowNameList = new ArrayList();
            int j = 1;
            StringBuilder sbName = new StringBuilder();
            StringBuilder sbId = new StringBuilder();
            stores.GroupBy(p => p.ProductModelId).ForEach(store =>
            {
                EntityJson node = new EntityJson();
                var list = store.OrderBy(e => e.Date).ToList();
                var itemone = list.FirstOrDefault();
                node.SetProperty("lineName", store.Key);
                node.SetProperty("lineId", itemone.ProductModelId);
                node.SetProperty("productName", itemone.ProductModel);
                node.SetProperty("productId", itemone.ProductId);
                rowNameList.Add("line" + itemone.ProductModelId);
                sbName.Append(store.Key + ",");
                sbId.Append(itemone.ProductModelId + ",");
                string alarmValue = itemone.ProductDirectRate.Alarm.ToString("0.00");
                string desiredValue = itemone.ProductDirectRate.Desired.ToString("0.00");
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
                        node.SetProperty("Date" + e.Date.ToString(FORMAT_DATE), (lineDateData.DirectRate).ToString("###0.##"));
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
            return rowNameList;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="criter">查询</param>
        /// <returns>table数据</returns>
        public string ExportProductReportRecords(ProductReportViewModelCriteria criter)
        {
            var stores = RT.Service.Resolve<ProdReportViewModelController>().GetProdReportList(criter);
            if (stores == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            string head = "<table><tr><td>机型</td><td>产品</td>";

            if (stores.Count > 0)
            {
                var dateList = stores.Select(p => p.Date).Distinct().ToList();
                dateList.OrderBy(p => p.Date).ForEach(f =>
                {
                    switch (criter.DateType)
                    {
                        case DateType.Year:
                            head += TAG_TD + f.Year + "年</td>";
                            break;
                        case DateType.Month:
                            head += TAG_TD + f.ToString("yyyy年MM月") + END_TD;
                            break;
                        case DateType.Week:
                            {
                                var week = stores.FirstOrDefault(p => p.Date == f);
                                head += TAG_TD + week.Week + END_TD;
                            }

                            break;
                        case DateType.Day:
                            head += TAG_TD + f.ToShortDateString() + END_TD;
                            break;
                        default:
                            break;
                    }
                });
                List<string> arr = stores.Select(p => p.Product).Distinct().ToList();
                arr.ForEach(s =>
                {
                    var lineList = stores.Where(f => f.Product == s).ToList();
                    lineList.ForEach(p =>
                    {
                        var list = stores.Where(e => e.ProductModel == p.ProductModel).ToList();
                        sb.Append("<tr><td>" + s + "</td><td>" + p.ProductModel + END_TD);
                        dateList.OrderBy(e => e.Date).ForEach(f =>
                        {
                            var data = list.FirstOrDefault(e => e.Date == f);
                            sb.Append(data == null ? "<td>&nbsp;</td>" : TAG_TD + data.DirectRate.ToString("P2") + END_TD);
                        });
                        sb.Append("</tr>");
                    });
                    sb.Append("<tr><td colspan='2'>机型合计</td>");
                    dateList.OrderBy(e => e.Date).ForEach(f =>
                    {
                        var workData = lineList.Where(p => p.Date == f).ToList();
                        string rate = "&nbsp;";
                        if (workData.Count != 0)
                        {
                            rate = (workData.Sum(p => p.DirectRate) / workData.Count).ToString("P2");
                        }

                        sb.Append(TAG_TD + rate + END_TD);
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
        /// <param name="dT">日期</param>
        /// <param name="workOrderReachList">数据列表</param>
        /// <param name="monthArrayList">返回数据</param>
        private void DateDataHandle(DateType dT, EntityList<ProductDirectRateViewModel> workOrderReachList, ArrayList monthArrayList)
        {
            if (dT == DateType.Day)
            {
                var dateList = workOrderReachList.Select(p => p.Date).Distinct().ToList();
                dateList.OrderBy(p => p).ForEach(p =>
                  {
                      string d = p.Year.ToString().Substring(2, 2) + "/" + p.Month + "/" + p.Day;
                      monthArrayList.Add(d);
                  });
            }
            else if (dT == DateType.Month)
            {
                var yearList = workOrderReachList.OrderBy(p => p.Date).Select(p => p.Month).ToList();
                yearList = yearList.Distinct().ToList();
                yearList.OrderBy(p => p).ForEach(p => monthArrayList.Add(p));
            }
            else if (dT == DateType.Year)
            {
                var yearList = workOrderReachList.OrderBy(p => p.Date).Select(p => p.Year).ToList();
                yearList = yearList.Distinct().ToList();
                yearList.OrderBy(p => p).ForEach(p => monthArrayList.Add(p));
            }
            else if (dT == DateType.Week)
            {
                var weekList = workOrderReachList.OrderBy(p => p.Date).Select(p => p.Week).ToList();
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
        /// <param name="stores">数据列表</param>
        /// <param name="dtpye">时间类型</param>
        /// <param name="dateList">日期列表</param>
        /// <param name="criter">查询条件实体</param>
        /// <returns>返回时间格式</returns>
        private string InitColumnList(EntityList<ProductDirectRateViewModel> stores, DateType dtpye, List<DateTime> dateList, ProductReportViewModelCriteria criter)
        {
            string columnDataIndexList = string.Empty;
            dateList.ForEach(e =>
            {
                switch (dtpye)
                {
                    case DateType.Year:
                        columnDataIndexList += (e.Year + "年^Date" + e.Date.ToString(FORMAT_DATE) + ",");
                        break;
                    case DateType.Month:
                        {
                            string m = e.Month + "月";
                            if (criter.CollectDate.BeginValue.Value.Year != criter.CollectDate.EndValue.Value.Year)
                            {
                                m = e.ToString("yyyy年MM月");
                            }

                            columnDataIndexList += (m + "^Date" + e.Date.ToString(FORMAT_DATE) + ",");
                        }

                        break;
                    case DateType.Week:
                        {
                            var week = stores.FirstOrDefault(p => p.Date == e);
                            columnDataIndexList += (week.Week + "^Date" + e.Date.ToString(FORMAT_DATE) + ",");
                        }

                        break;
                    case DateType.Day:
                        {
                            string m = e.Month + "月";
                            if (criter.CollectDate.BeginValue.Value.Year != criter.CollectDate.EndValue.Value.Year)
                            {
                                m = e.ToString("yyyy年MM月");
                            }

                            columnDataIndexList += (e.Date.Day + "号^Date" + e.Date.ToString(FORMAT_DATE) + "^" + m + ",");
                        }

                        break;
                    default:
                        break;
                }
            });
            return columnDataIndexList;
        }

        /// <summary>
        /// 获取产品直通率
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>产品直通率实体</returns>
        public ProductFypInfo GetProductRates(ProductReportViewModelCriteria criteria)
        {
            ProductFypInfo lineVM = new ProductFypInfo();
            List<ProductDirectRateInfo> directRateVMs = new List<ProductDirectRateInfo>();
            var productModel = RT.Service.Resolve<ProdReportViewModelController>().GetProdReportViewModel(criteria);
            if (productModel == null)
            {
                return lineVM;
            }

            var productDirectRates = productModel.ProdDirectRateList;
            List<ProductFpySettingInfo> settings = new List<ProductFpySettingInfo>();
            foreach (var productDirectRate in productDirectRates)
            {
                directRateVMs.Add(CreateRateModel(productDirectRate.ProductModel, productDirectRate.Product, productDirectRate.DirectRate, productDirectRate.Date, false));

                if (settings.FirstOrDefault(p => p.Name == productDirectRate.Product && p.IsProductModel) == null)
                {
                    settings.Add(CreateSettingModel(productDirectRate.Product, true));
                }

                settings.Add(CreateSettingModel(productDirectRate.ProductModel, false));
            }

            if (directRateVMs.Count > 0)
            {
                lineVM.DirectRateVMList.AddRange(directRateVMs);
            }

            if (settings.Count > 0)
            {
                lineVM.ProductFpySettingList.AddRange(settings);
            }

            return lineVM;
        }

        /// <summary>
        /// 获取产品直通率报表
        /// </summary>
        /// <param name="productModelName">机型</param>
        /// <param name="productName">产品</param>
        /// <param name="directRateVMs">直通率列表</param>
        /// <param name="isProduct">是否机型</param>
        /// <returns>产品直通率报表</returns>
        public List<ReportRateInfo> GetProductReport(string productModelName, string productName, List<ProductDirectRateInfo> directRateVMs, bool isProduct)
        {
            List<ReportRateInfo> lineReportVMs = new List<ReportRateInfo>();
            var filterRatesVMs = directRateVMs.Where(p => p.ProductModelName == productModelName && p.ProductName == productName);
            var lineNames = filterRatesVMs.Select(p => p.ProductModelName).Distinct().ToList();
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
        /// 获取工序图表数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="directRateVM">直通率数据列表</param>
        /// <returns>工序图表数据</returns>
        public ProductFpyDetailInfo GetProductDetailViewModel(string date, ProductDirectRateInfo directRateVM)
        {
            ProductFpyDetailInfo productModel = new ProductFpyDetailInfo();
            if(directRateVM == null)
            {
                return productModel;
            }
            List<BarBasicInfo> barBasicVMs = new List<BarBasicInfo>();
            List<ColumnStackedInfo> columnStackedVMs = new List<ColumnStackedInfo>();
            var dateRange = ProcessDateTime(date, directRateVM.Date);
            var fpySouece = RT.Service.Resolve<FpyController>().GetProdcutFpyStatistics(directRateVM.ProductModelName, directRateVM.ProductName, new DateRange { BeginValue = dateRange.Item1, EndValue = dateRange.Item2 });
            var defectStatistics = RT.Service.Resolve<FpyController>().GetDefectStatisticsForProd(dateRange, directRateVM.ProductModelName, directRateVM.ProductName);
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

            productModel.BarBasicViewModel.AddRange(barBasicVMs);
            productModel.ColumnStackedViewModel.AddRange(columnStackedVMs);
            var defect = GetDefectCategoryInfos(defectStatistics);
            productModel.DefectCategory.AddRange(defect);
            return productModel;
        }

        /// <summary>
        /// 创建设置产品实体
        /// </summary>
        /// <param name="name">产品</param>
        /// <param name="isProduct">是否产品</param>
        /// <returns>产品实体</returns>
        private ProductFpySettingInfo CreateSettingModel(string name, bool isProduct)
        {
            ProductFpySettingInfo productSet = new ProductFpySettingInfo
            {
                Name = name,
                IsProductModel = isProduct
            };
            return productSet;
        }

        /// <summary>
        /// 创建直通率模型
        /// </summary>
        /// <param name="productModelName">机型</param>
        /// <param name="productName">产品</param>
        /// <param name="rate">直通率</param>
        /// <param name="dt">日期</param>
        /// <param name="isProduct">是否产品</param>
        /// <returns>直通率模型</returns>
        private ProductDirectRateInfo CreateRateModel(string productModelName, string productName, decimal rate, DateTime dt, bool isProduct)
        {
            var directRateVM = new ProductDirectRateInfo()
            {
                ProductModelName = productModelName,
                ProductName = productName,
                DirectRate = rate * 100,
                Date = dt
            };
            return directRateVM;
        }

        /// <summary>
        /// 获取缺陷分类及其相关信息列表
        /// </summary>
        /// <param name="defectStatistics">缺陷统计列表</param>
        /// <returns>缺陷分类及其相关信息列表</returns>
        public List<ProductFPY.DefectCategoryInfo> GetDefectCategoryInfos(EntityList<DefectStatistics> defectStatistics)
        {
            List<ProductFPY.DefectCategoryInfo> defectCategoryInfos = new List<ProductFPY.DefectCategoryInfo>();
            var defectCategorysOfTopFive = defectStatistics.OrderByDescending(p => p.Qty).Take(5).ToList();
            var ctgGroups = defectCategorysOfTopFive.GroupBy(p => p.CategoryName).ToList();
            int count = ctgGroups.Count;

            for (int i = 0; i < count; i++)
            {
                var defectCategoryInfo = new ProductFPY.DefectCategoryInfo();
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
                    var defectCodeInfo = new ProductFPY.DefectCodeInfo();
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
            if (!string.IsNullOrWhiteSpace(date) && date.Contains("年"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddYears(1));
            }
            else if (!string.IsNullOrWhiteSpace(date) && date.Contains("月"))
            {
                return Tuple.Create(DateTime.Parse(date), DateTime.Parse(date).AddMonths(1));
            }
            else if (!string.IsNullOrWhiteSpace(date) && date.Contains("周"))
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
