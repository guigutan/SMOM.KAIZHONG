using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.TeamManagement;
using SIE.MES.Statistics.Fpy;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.Reports.ShopFPY
{
    /// <summary>
    /// 车间直通率报表控制器
    /// </summary>
    public class ShopReportViewModelController : DomainController
    {
        ShopReportViewModelCriteria _criteria;

        /// <summary>
        /// 查询车间报表ViewModel
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <param name="shopFpyStatistics">直通率采集数据</param>
        /// <param name="resNameDicsIn">资源名称字典</param>
        /// <param name="shopDicsIn">车间字典</param>
        /// <returns>车间报表ViewModel</returns>
        public virtual ShopReportViewModel GetShopReportViewModel(ShopReportViewModelCriteria criteria, out EntityList<ProcessFpyStatistics> shopFpyStatistics, out Dictionary<double, string> resNameDicsIn, out Dictionary<double, Enterprise> shopDicsIn)
        {
            _criteria = criteria;
            shopFpyStatistics = RT.Service.Resolve<FpyController>().GetShopFpyStatistics(shopName: criteria?.Shop?.Name, dateRange: criteria?.CollectDate);

            //获取资源Id与资源名称的字典，因为名称可能被修改
            var resIds = shopFpyStatistics.Select(p => p.ResourceId).Distinct().ToList();
            var resNameDics = RT.Service.Resolve<WipResourceController>().GetResourceList(resIds).ToDictionary(k => k.Id, v => v.Name);

            var viewModel = new ShopReportViewModel();
            viewModel.LayoutFileName = viewModel.GetType().Name;
            shopFpyStatistics.GroupBy(p => p.CollectedDate).ForEach(p =>
            {
                viewModel.ShopDirectRateList.AddRange(CreateModel(p.ToList(), resNameDics));
            });
            resNameDicsIn = resNameDics;

            //由于采集时并不能提供车间Id和名称
            //所以此处给车间赋值
            var shopDics = RT.Service.Resolve<EnterpriseController>().GetLineToShopDics(resIds.ToList());
            if (shopDics.Count > 0)
            {
                viewModel.ShopDirectRateList.ForEach(p =>
                {
                    if (shopDics.ContainsKey((double)p.LineId))
                    {
                        p.ShopId = shopDics[(double)p.LineId]?.Id;
                        p.ShopName = shopDics[(double)p.LineId]?.Name;
                    }
                });
            }
            shopDicsIn = shopDics;
            var shopids = viewModel.ShopDirectRateList.Select(p => p.ShopId).OfType<double>().ToList();
            var shopSettingList = RT.Service.Resolve<FpySettings.FpySettingController>().GetShopFpySettingsByShopIds(shopids);
            viewModel.ShopDirectRateList.ForEach(p =>
            {
                var setting = shopSettingList.FirstOrDefault(t => t.ShopId == p.ShopId) ?? new FpySettings.ShopFpySetting();
                p.ShopDirectRate = setting;
                p.LineDirectRate = setting.LineFpyList.FirstOrDefault(t => t.ResourceId == p.LineId) ?? new FpySettings.LineFpySetting { Desired = setting.Desired, Alarm = setting.Alarm };
            });

            viewModel.MarkSaved();
            return viewModel;
        }

        /// <summary>
        /// 创建报表数据ViewModel
        /// </summary>
        /// <param name="fpys">直通率统计</param>
        /// <param name="dics"></param>
        /// <returns>报表数据ViewModel</returns>
        private List<ShopDirectRateViewModel> CreateModel(List<ProcessFpyStatistics> fpys, Dictionary<double, string> dics)
        {
            List<ShopDirectRateViewModel> models = new List<ShopDirectRateViewModel>();
            fpys.GroupBy(p => p.ResourceId).ForEach(p =>
            {
                var temp = p.ToList()[0];
                decimal processRate = 1;
                p.GroupBy(q => q.ResourceId).ForEach(q =>
                {
                    q.GroupBy(o => o.ProcessId).ForEach(o =>
                    {
                        if (o.Sum(x => x.InputQty) > 0)
                        {
                            processRate *= o.Sum(x => x.PassQty) / o.Sum(x => x.InputQty);
                        }
                    });
                });


                var viewModel = new ShopDirectRateViewModel()
                {
                    Year = temp.CollectedDate.Year,
                    Month = temp.CollectedDate.ToString("yyyy年MM月"),
                    Week = "{0}年第{1}周".FormatArgs(temp.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(temp.CollectedDate)),
                    Date = temp.CollectedDate.Date,
                    LineId = temp.ResourceId,
                    LineName = dics.ContainsKey(temp.ResourceId) ? dics[temp.ResourceId] : temp.ResourceName,
                    //LineName = temp.ResourceName,
                    DirectRate = processRate
                };

                viewModel.GenerateId();
                models.Add(viewModel);
            });

            return models;
        }

        /// <summary>
        /// 获取自定义求和值
        /// </summary>
        /// <returns>根据行列属性值对应的字典 Key：行列属性值得字符串， Value：求和值</returns>
        public virtual Dictionary<string, decimal> GetCustomSummeries()
        {
            _criteria = _criteria ?? new ShopReportViewModelCriteria();
            var shopFpyStatistics = RT.Service.Resolve<FpyController>().GetShopFpyStatistics(shopName: _criteria?.Shop?.Name, dateRange: _criteria?.CollectDate);

            //获取资源Id与资源名称的字典，因为名称可能被修改
            var resIds = shopFpyStatistics.Select(p => p.ResourceId).Distinct().ToList();
            var resNameDics = RT.Service.Resolve<WipResourceController>().GetResourceList(resIds).ToDictionary(k => k.Id, v => v.Name);

            var shopDics = RT.Service.Resolve<EnterpriseController>().GetLineToShopDics(resIds.ToList());
            var dics = new Dictionary<string, decimal>();

            if (shopDics.Count > 0)
            {
                shopFpyStatistics.GroupBy(p =>
                {
                    var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                    return p.CollectedDate.Year + ":" + shopDics[p.ResourceId]?.Name + ":" + resName;
                }).ForEach(p =>
                {
                    decimal processRate = 1M;
                    p.GroupBy(q => q.ProcessId).ForEach(q =>
                    {
                        if (q.Sum(x => x.InputQty) > 0)
                        {
                            processRate *= q.Sum(x => x.PassQty) / q.Sum(x => x.InputQty);
                        }
                    });

                    dics[p.Key] = processRate;
                });

                shopFpyStatistics.GroupBy(p =>
                {
                    var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                    return p.CollectedDate.ToString("yyyy年MM月") + ":" + shopDics[p.ResourceId]?.Name + ":" + resName;
                }).ForEach(p =>
                {
                    decimal processRate = 1M;
                    p.GroupBy(q => q.ProcessId).ForEach(q =>
                    {
                        if (q.Sum(x => x.InputQty) > 0)
                        {
                            processRate *= q.Sum(x => x.PassQty) / q.Sum(x => x.InputQty);
                        }
                    });

                    dics[p.Key] = processRate;
                });

                shopFpyStatistics.GroupBy(p =>
                {
                    var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                    return "{0}年第{1}周".FormatArgs(p.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(p.CollectedDate)) + ":" + shopDics[p.ResourceId]?.Name + ":" + resName;
                }).ForEach(p =>
                {
                    decimal processRate = 1M;
                    p.GroupBy(q => q.ProcessId).ForEach(q =>
                    {
                        if (q.Sum(x => x.InputQty) > 0)
                        {
                            processRate *= q.Sum(x => x.PassQty) / q.Sum(x => x.InputQty);
                        }
                    });

                    dics[p.Key] = processRate;
                });
            }

            return dics;
        }

        /// <summary>
        /// BS获取车间直通率
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>直通率数据</returns>
        public virtual EntityList<ShopDirectRateViewModel> GetShopDirectRateViewModel(ShopReportViewModelCriteria criteria)
        {
            EntityList<ProcessFpyStatistics> shopFpyStatistics;
            Dictionary<double, string> resNameDics;
            Dictionary<double, Enterprise> shopDics;
            var criModel = GetShopReportViewModel(criteria, out shopFpyStatistics, out resNameDics, out shopDics);
            if (criModel == null || criModel.ShopDirectRateList.Count == 0)
            {
                return new EntityList<ShopDirectRateViewModel>();
            }
            var rateModel = criModel.ShopDirectRateList.Where(p => p.LineId > 0).ToList();
            EntityList<ShopDirectRateViewModel> rstList = new EntityList<ShopDirectRateViewModel>();
            var dics = new Dictionary<string, decimal>();
            SetDic(shopFpyStatistics, criteria.DateType, dics, resNameDics, shopDics);
            if (criteria.DateType == DateType.Year)
            {
                rateModel.GroupBy(p => p.LineId).ForEach(p =>
                {
                    p.GroupBy(e => e.Date.Year).ForEach(e =>
                    {
                        rstList.Add(setModel(e.FirstOrDefault(), criteria, dics));
                    });
                });
            }
            else if (criteria.DateType == DateType.Month)
            {
                rateModel.GroupBy(p => p.LineId).ForEach(p =>
                {
                    p.GroupBy(e => e.Month).ForEach(e =>
                    {
                        rstList.Add(setModel(e.FirstOrDefault(), criteria, dics));
                    });
                });
            }
            else if (criteria.DateType == DateType.Week)
            {
                rateModel.GroupBy(p => p.LineId).ForEach(p =>
                {
                    p.GroupBy(e => e.Week).ForEach(e =>
                    {
                        rstList.Add(setModel(e.FirstOrDefault(), criteria, dics));
                    });
                });
            }
            else
            {
                rstList.AddRange(rateModel);
            }

            return rstList;
        }

        /// <summary>
        /// 字典存放年月周的直通率
        /// </summary>
        /// <param name="shopFpyStatistics">数据源</param>
        /// <param name="dtype">时间类型</param>
        /// <param name="dics">字典</param>
        /// <param name="resNameDics">资源数据</param>
        /// <param name="shopDics">车间数据</param>
        private void SetDic(EntityList<ProcessFpyStatistics> shopFpyStatistics, DateType dtype, Dictionary<string, decimal> dics, Dictionary<double, string> resNameDics, Dictionary<double, Enterprise> shopDics)
        {
            if (shopDics.Count == 0)
            {
                return;
            }

            if (dtype != DateType.Day)
            {
                shopFpyStatistics.GroupBy(p =>
                {
                    var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                    string head = "";
                    switch (dtype)
                    {
                        case DateType.Year:
                            head = p.CollectedDate.Year.ToString();
                            break;
                        case DateType.Month:
                            head = p.CollectedDate.ToString("yyyy年MM月");
                            break;
                        case DateType.Week:
                            head = "{0}年第{1}周".FormatArgs(p.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(p.CollectedDate));
                            break;
                        default:
                            break;
                    }
                    return head + ":" + shopDics[p.ResourceId]?.Name + ":" + resName;
                }).ForEach(p =>
                {
                    decimal processRate = 1M;
                    p.GroupBy(q => q.ProcessId).ForEach(q =>
                    {
                        if (q.Sum(x => x.InputQty) > 0)
                        {
                            processRate *= q.Sum(x => x.PassQty) / q.Sum(x => x.InputQty);
                        }
                    });

                    dics[p.Key] = processRate;
                });
            }
        }

        /// <summary>
        /// 创建新车间直通率数据
        /// </summary>
        /// <param name="first">数据源</param>
        /// <param name="criteria">查询</param>
        /// <param name="dics">字典</param>
        /// <returns>直通率数据</returns>
        private ShopDirectRateViewModel setModel(ShopDirectRateViewModel first, ShopReportViewModelCriteria criteria, Dictionary<string, decimal> dics)
        {
            ShopDirectRateViewModel model = new ShopDirectRateViewModel();
            model.ShopName = first.ShopName;
            model.ShopId = first.ShopId;
            model.LineId = first.LineId;
            model.LineName = first.LineName;
            if (criteria.DateType == DateType.Year)
            {
                model.Date = DateTime.Parse(first.Date.Year + "-01-01");
                model.Year = first.Year;
                decimal rate = 0;
                dics.TryGetValue(model.Year + ":" + model.ShopName + ":" + model.LineName, out rate);
                model.DirectRate = rate;
            }
            else if (criteria.DateType == DateType.Month)
            {
                model.Date = DateTime.Parse(first.Date.Year + "-" + first.Date.Month + "-01");
                model.Month = criteria.CollectDate.BeginValue.Value.Year != criteria.CollectDate.EndValue.Value.Year ? 
                    first.Date.Year + "年" + first.Date.Month + "月" : first.Date.Month + "月";
                decimal rate = 0;
                dics.TryGetValue(model.Month + ":" + model.ShopName + ":" + model.LineName, out rate);
                model.DirectRate = rate;
            }
            else if (criteria.DateType == DateType.Week)
            {
                model.Date = first.Date;
                model.Week = criteria.CollectDate.BeginValue.Value.Year != criteria.CollectDate.EndValue.Value.Year ? first.Week : first.Week.Remove(0, 6);
                decimal rate = 0;
                dics.TryGetValue(model.Week + ":" + model.ShopName + ":" + model.LineName, out rate);
                model.DirectRate = rate;
            }
            else
            {
                //
            }
            model.LineDirectRate = new FpySettings.LineFpySetting();
            model.ShopDirectRate = new FpySettings.ShopFpySetting();
            model.LineDirectRate = first.LineDirectRate;
            model.ShopDirectRate = first.ShopDirectRate;
            return model;
        }
    }
}
