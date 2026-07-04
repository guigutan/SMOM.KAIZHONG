using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.Statistics.Fpy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品直通率ViewModel控制器
    /// </summary>
    public class ProdReportViewModelController : DomainController
    {
        ProductReportViewModelCriteria _criteria;

        /// <summary>
        /// 查询产品报表ViewModel
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>车间报表ViewModel</returns>
        public virtual ProductReportViewModel GetProdReportViewModel(ProductReportViewModelCriteria criteria)
        {
            _criteria = criteria;

            var prodFpyStatistics = RT.Service.Resolve<FpyController>().GetProdcutFpyStatistics(criteria?.ProductModel?.Name, criteria?.Product?.Name, criteria?.CollectDate);

            var viewModel = new ProductReportViewModel();
            viewModel.LayoutFileName = viewModel.GetType().Name;
            prodFpyStatistics.GroupBy(p => p.CollectedDate).ForEach(p =>
            {
                viewModel.ProdDirectRateList.AddRange(CreateModel(p.ToList()));
            });

            //由于采集时并不能提供产品机型Id和名称
            //所以此处给车间赋值
            var dics = RT.Service.Resolve<ItemController>().GetItemIdToModels(viewModel.ProdDirectRateList.Select(p => (double)p.ProductId).Distinct().ToList());
            viewModel.ProdDirectRateList.ForEach(p =>
            {
                p.ProductModelId = dics[(double)p.ProductId] == null ? 0 : dics[(double)p.ProductId].Id;
                p.ProductModel = dics[(double)p.ProductId] == null ? string.Empty : dics[(double)p.ProductId].Name;
            });

            var modelids = viewModel.ProdDirectRateList.Select(p => p.ProductModelId).OfType<double>().ToList();
            var modelSettingList = RT.Service.Resolve<FpySettings.FpySettingController>().GetProductModelFpySettingsByModelIds(modelids);
            viewModel.ProdDirectRateList.ForEach(p =>
            {
                var setting = modelSettingList.FirstOrDefault(t => t.ModelId == p.ProductModelId) ?? new FpySettings.ProductModelFpySetting();
                p.ModelDirectRate = setting;
                p.ProductDirectRate = setting.ProductFpyList.FirstOrDefault(t => t.ProductId == p.ProductId) ?? new FpySettings.ProductFpySetting { Desired = setting.Desired, Alarm = setting.Alarm };
            });

            viewModel.MarkSaved();
            return viewModel;
        }

        /// <summary>
        /// 创建报表数据ViewModel
        /// </summary>
        /// <returns>报表数据ViewModel</returns>
        private List<ProductDirectRateViewModel> CreateModel(List<ProcessFpyStatistics> fpys)
        { 
            List<ProductDirectRateViewModel> models = new List<ProductDirectRateViewModel>();
            fpys.GroupBy(p => "{0}:{1}".FormatArgs(p.ProductId, p.ModelId)).ForEach(p =>
            {
                decimal processRate = 1;
                var tempList = p.ToList();
                var temp = tempList[0];
                tempList.GroupBy(q => q.ProcessId).ForEach(q =>
                {
                    if (q.Sum(x => x.InputQty) > 0)
                    {
                        processRate *= q.Sum(o => o.PassQty) / q.Sum(o => o.InputQty);
                    }
                });

                var viewModel = new ProductDirectRateViewModel()
                {
                    Year = temp.CollectedDate.Year,
                    Month = temp.CollectedDate.ToString("yyyy年MM月"),
                    Week = "{0}年第{1}周".FormatArgs(temp.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(temp.CollectedDate)),
                    Date = temp.CollectedDate.Date,
                    ProductId = temp.ProductId,
                    Product = temp.ProductName,
                    ProductModel = temp.ModelName ?? string.Empty,
                    ProductModelId = temp.ModelId ?? 0,
                    DirectRate = processRate
                };

                viewModel.GenerateId();
                models.Add(viewModel);
            });

            return models;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, decimal> GetCustomSummeries()
        {
            _criteria = _criteria ?? new ProductReportViewModelCriteria();
            var prodFpyStatistics = RT.Service.Resolve<FpyController>().GetProdcutFpyStatistics(_criteria?.ProductModel?.Name, _criteria?.Product?.Name, _criteria?.CollectDate);
            var modelDics = RT.Service.Resolve<ItemController>().GetItemIdToModels(prodFpyStatistics.Select(p => p.ProductId).Distinct().ToList());
            var dics = new Dictionary<string, decimal>();

            FpyStatisticsGroup(dics, prodFpyStatistics,
            p => {
                return modelDics[p.ProductId]?.Name + ":" + p.ProductName;
            },
            q => {
                return q.CollectedDate.Year.ToString();
            });

            FpyStatisticsGroup(dics, prodFpyStatistics,
            p => {
                return modelDics[p.ProductId]?.Name + ":" + p.ProductName;
            },
            q => {
                return q.CollectedDate.ToString("yyyy年MM月");
            });

            FpyStatisticsGroup(dics, prodFpyStatistics,
            p => {
                return modelDics[p.ProductId]?.Name + ":" + p.ProductName;
            },
            q => {
                return "{0}年第{1}周".FormatArgs(q.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(q.CollectedDate));
            });

            FpyStatisticsGroup(dics, prodFpyStatistics,
            p => {
                return modelDics[p.ProductId]?.Name;
            },
            q => {
                return q.CollectedDate.ToString("yyyy-MM-dd HH:mm:ss");
            });

            FpyStatisticsGroup(dics, prodFpyStatistics,
            p => {
                return modelDics[p.ProductId]?.Name;
            },
            q => {
                return q.CollectedDate.Year.ToString();
            });

            FpyStatisticsGroup(dics, prodFpyStatistics,
            p => {
                return modelDics[p.ProductId]?.Name;
            },
            q => {
                return q.CollectedDate.ToString("yyyy年MM月");
            });

            FpyStatisticsGroup(dics, prodFpyStatistics, 
            p =>{
                return modelDics[p.ProductId]?.Name;
            },
            q => {
                return "{0}年第{1}周".FormatArgs(q.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(q.CollectedDate));
            });

            return dics;
        }

        /// <summary>
        /// 直通率Group执行
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="prodFpyStatistics"></param>
        /// <param name="groupSelector"></param>
        /// <param name="nextGroupSelector"></param>
        private void FpyStatisticsGroup(Dictionary<string, decimal> dics, EntityList<ProcessFpyStatistics> prodFpyStatistics, Func<ProcessFpyStatistics, string> groupSelector, Func<ProcessFpyStatistics, string> nextGroupSelector)
        {
            prodFpyStatistics.GroupBy(p => groupSelector(p)).ForEach(p =>
            {
                p.GroupBy(q => nextGroupSelector(q)).ForEach(q =>
                {
                    var passQty = 0M;
                    var inputQty = 0M;
                    q.GroupBy(o => o.CollectedDate + ":" + o.ResourceId /*+ ":" + o.ShiftId*/ + ":" + o.ProductId).ForEach(o =>
                    {
                        Dictionary<string, decimal> failedDic = new Dictionary<string, decimal>();
                        Dictionary<string, decimal> inputDic = new Dictionary<string, decimal>();

                        o.GroupBy(x => x.ProcessName).ForEach(x =>
                        {
                            failedDic[x.Key] = x.Sum(y => y.FailedQty);
                            inputDic[x.Key] = x.Sum(y => y.InputQty);
                        });

                        var failedQty = 0M;
                        //投入数
                        var maxInput = inputDic.Max(z => z.Value);
                        //一次过站失败数
                        failedQty = failedDic.Sum(z => z.Value);
                        inputQty += maxInput;
                        passQty += maxInput - failedQty;
                    });


                    dics[q.Key + ":" + p.Key] = inputQty > 0 ? passQty / inputQty : 0;
                });
            });
        }

        /// <summary>
        /// 获取产品直通率
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>产品直通率列表</returns>
        public virtual EntityList<ProductDirectRateViewModel> GetProdReportList(ProductReportViewModelCriteria criteria)
        {
            return GetProdReportViewModel(criteria).ProdDirectRateList;
        }
    }
}
