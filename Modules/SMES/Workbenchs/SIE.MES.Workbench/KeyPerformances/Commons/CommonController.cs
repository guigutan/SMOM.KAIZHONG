using SIE.Domain;
using SIE.MES.Statistics.Fpy;
using SIE.MES.Statistics.WIP;
using SIE.MES.Workbench.KeyPerformances.PlanReachedPercentages;
using SIE.MES.Workbench.KeyPerformances.ProductFpy;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SIE.MES.Workbench.KeyPerformances.Commons
{
    /// <summary>
    /// 效率统计公共控制器
    /// </summary>
    public class CommonController : DomainController
    {
        /// <summary>
        /// 获取产线达成率信息列表(时间范围是当前日期前7天)
        /// </summary>
        /// <param name="shopId">车间Id</param>
        /// <returns>产线达成率信息列表</returns>
        public virtual EntityList<LinePlanReachedViewModel> GetLinePRInfos(double shopId, DateTime _dbtime)
        {
            DateRange dr = new DateRange() { BeginValue = _dbtime.AddDays(-7).Date, EndValue = _dbtime.Date };
            var planWoList = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByDateRange(shopId, dr);
            var finishWoList = RT.Service.Resolve<WipStatisticsController>().GetMonthStatics(shopId, dr);
            EntityList<LinePlanReachedViewModel> rstList = new EntityList<LinePlanReachedViewModel>();
            planWoList.GroupBy(p => p.PlanBeginDate.Date).ForEach(p =>
            {
                var curDate = p.Key;
                p.GroupBy(e => e.Resource).ForEach(f =>
                {
                    LinePlanReachedViewModel item = new LinePlanReachedViewModel();
                    item.Date = curDate;
                    item.LineId = f.Key.Id;
                    item.LineName = f.Key.Name;
                    item.PlanQty = f.Sum(g => g.PlanQty);
                    item.ActualQty = 0;
                    rstList.Add(item);
                });
            });

            finishWoList.GroupBy(p => p.CollectDate.Date).ForEach(p =>
            {
                var curDate = p.Key;
                p.GroupBy(e => e.ResourceId).ForEach(f =>
                {
                    var rst = rstList.FirstOrDefault(m => m.LineId == f.Key && m.Date == curDate);
                    var actQty = f.Sum(g => g.QtyPass);
                    if (actQty > 0)
                    {
                        if (rst != null)
                        {
                            rst.ActualQty = f.Sum(g => g.QtyPass);
                        }
                        else
                        {
                            LinePlanReachedViewModel item = new LinePlanReachedViewModel();
                            item.Date = curDate;
                            item.LineId = f.Key;
                            item.LineName = f.FirstOrDefault().ResourceName;
                            item.PlanQty = 0;
                            item.ActualQty = f.Sum(g => g.QtyPass);
                            rstList.Add(item);
                        }
                    }
                });
            });

            return rstList;
        }

        /// <summary>
        /// 构建车间达成率数据Model
        /// </summary>
        /// <returns>ChartCommonViewModel</returns>
        public virtual ChartCommonViewModel BuildShopPRModel(string title, double shopId, DateTime _dbtime)
        {
            var linePRInfos = GetLinePRInfos(shopId, _dbtime);
            var model = new ChartCommonViewModel() { Title = title };
            model.BaseValues = new ObservableCollection<BaseValueViewModel>();

            linePRInfos.OrderBy(p => p.Date).GroupBy(p => p.Date).ForEach(p =>
              {
                  var dayValues = p.ToList();
                  model.BaseValues.Add(new BaseValueViewModel
                  {
                      Date = p.Key,
                      Efficiency = dayValues.Count > 0 ? dayValues.Sum(q => q.Percentage) / dayValues.Count : 0
                  });
              });

            return model;
        }

        /// <summary>
        /// 获取产线直通率信息列表(时间范围是当前日期前7天)
        /// </summary>
        /// <param name="shopId">车间Id</param>
        /// <returns>产线直通率信息列表</returns>
        public virtual EntityList<LineDataViewModel> GetLineFpyInfos(Enterprise workshop, DateTime dbTime)
        {
            DateRange dr = new DateRange() { BeginValue = dbTime.AddDays(-6).Date, EndValue = dbTime.AddDays(1).Date };
            var shopFpyStatistics = RT.Service.Resolve<FpyController>().GetShopProductFpyStatistics(workshop.Name, null, dr);
            var rstList = new EntityList<LineDataViewModel>();
            shopFpyStatistics.OrderBy(p => p.CollectedDate.Date).GroupBy(p => p.CollectedDate.Date).ForEach(p =>
              {
                  var lineList = p.ToList();
                  DateTime curDate = p.Key;
                  lineList.GroupBy(e => e.ResourceId).ForEach(e =>
                  {
                      var itemlist = e.ToList();
                      var sumInput = itemlist.Sum(f => f.InputQty);
                      LineDataViewModel item = new LineDataViewModel()
                      {
                          Date = curDate,
                          LineId = e.Key,
                          LineName = e.FirstOrDefault(f => f.ResourceId == e.Key).ResourceName,
                          Percentage = sumInput > 0 ? (double)((sumInput - itemlist.Sum(f => f.FailedQty)) / sumInput) : 0
                      };
                      rstList.Add(item);
                  });
              });

            return rstList;
        }

        /// <summary>
        /// 构建车间直通率数据Model
        /// </summary>
        /// <returns>ChartCommonViewModel</returns>
        public virtual ChartCommonViewModel BuildLineFpyModel(string title, Enterprise workshop, DateTime dbTime)
        {
            DateRange dr = new DateRange() { BeginValue = dbTime.AddDays(-6).Date, EndValue = dbTime.AddDays(1).Date };
            var shopFpyStatistics = RT.Service.Resolve<FpyController>().GetShopProductFpyStatistics(workshop.Name, null, dr);

            var model = new ChartCommonViewModel() { Title = title };
            model.BaseValues = new ObservableCollection<BaseValueViewModel>();

            shopFpyStatistics.OrderBy(p => p.CollectedDate.Date).GroupBy(p => p.CollectedDate).ForEach(p =>
            {
                var sumInput = p.Sum(x => x.InputQty);
                model.BaseValues.Add(new BaseValueViewModel
                {
                    Date = p.Key,
                    Efficiency = sumInput > 0 ? (double)((sumInput - p.Sum(x => x.FailedQty)) / sumInput) : 0
                });
            });

            return model;
        }

        ///// <summary>
        ///// 获取OEE效率列表(时间范围是当前日期前7天)
        ///// </summary>
        ///// <param name="shopId">车间Id</param>
        ///// <returns>OEE效率列表</returns>
        //public virtual EntityList<OEEfficiency> GetOEEfficiencys(double? shopId = null)
        //{
        //    var query = Query<OEEfficiency>();
        //    if (shopId != null)
        //        query.Where(p => p.WorkShopId == shopId);
        //    query.Where(p => p.Date < DateTime.Now.Date && p.Date >= DateTime.Now.AddDays(-7).Date);
        //    query.Where(p => p.Efficiency > 0);
        //    return query.ToList();
        //}

        ///// <summary>
        ///// 构建车间OEE效率数据Model
        ///// </summary>
        ///// <returns>ChartCommonViewModel</returns>
        //public virtual ChartCommonViewModel BuildOEEfficiencyModel(string title, double shopId)
        //{
        //    var linePRInfos = GetOEEfficiencys(shopId);
        //    var model = new ChartCommonViewModel() { Title = title };
        //    model.BaseValues = new ObservableCollection<BaseValueViewModel>();
        //    var setting = RT.Service.Resolve<TargetSettingController>().GetShopTargetSettings(shopId, TargetSettingType.OEEfficiency);

        //    linePRInfos.GroupBy(p => p.Date).ForEach(p =>
        //    {
        //        var dayValues = p.ToList();
        //        model.BaseValues.Add(new BaseValueViewModel
        //        {
        //            Date = p.Key,
        //            Efficiency = dayValues.Sum(q => q.Efficiency) / dayValues.Count
        //        });
        //    });

        //    SetAlertLevel(model, setting);
        //    return model;
        //}

        ///// <summary>
        ///// 获取车间OPE效率列表(时间范围是当前日期前7天)
        ///// </summary>
        ///// <param name="shopId">车间Id</param>
        ///// <returns>OPE效率列表</returns>
        //public virtual EntityList<OPEfficiency> GetOPEfficiencys(double? shopId = null)
        //{
        //    var query = Query<OPEfficiency>();
        //    if (shopId != null)
        //        query.Where(p => p.WorkShopId == shopId);
        //    query.Where(p => p.Date < DateTime.Now.Date && p.Date >= DateTime.Now.AddDays(-7).Date);
        //    query.Where(p => p.Efficiency > 0);
        //    return query.ToList();
        //}

        ///// <summary>
        ///// 构建OPE效率数据Model
        ///// </summary>
        ///// <returns>ChartCommonViewModel</returns>
        //public virtual ChartCommonViewModel BuildOPEfficiencyModel(string title, double shopId)
        //{
        //    var linePRInfos = GetOPEfficiencys(shopId);
        //    var model = new ChartCommonViewModel() { Title = title };
        //    model.BaseValues = new ObservableCollection<BaseValueViewModel>();
        //    var setting = RT.Service.Resolve<TargetSettingController>().GetShopTargetSettings(shopId, TargetSettingType.OPEfficiency);

        //    linePRInfos.GroupBy(p => p.Date).ForEach(p =>
        //    {
        //        var dayValues = p.ToList();
        //        model.BaseValues.Add(new BaseValueViewModel
        //        {
        //            Date = p.Key,
        //            Efficiency = dayValues.Sum(q => q.Efficiency) / dayValues.Count
        //        });
        //    });

        //    SetAlertLevel(model, setting);
        //    return model;
        //}

        /// <summary>
        /// 构建车间工单完工率
        /// </summary>
        /// <returns>ChartCommonViewModel</returns>
        public virtual ChartCommonViewModel BuildShopCompletedModel(string title, double shopId, DateTime dbtime)
        {
            var model = new ChartCommonViewModel() { Title = title };
            model.BaseValues = new ObservableCollection<BaseValueViewModel>();
            DateRange dr = new DateRange() { BeginValue = dbtime.AddDays(-6).Date, EndValue = dbtime.AddDays(1).Date };
            var datas = RT.Service.Resolve<WorkOrderController>().GetFinishWorkOrders(shopId, null, dr);
            datas.OrderBy(p => p.PlanEndDate.Date).GroupBy(p => p.PlanEndDate.Date).ForEach(p =>
              {
                  var list = p.ToList();
                  model.BaseValues.Add(new BaseValueViewModel
                  {
                      Date = p.Key,
                      Efficiency = list.Count > 0 ? list.Where(e => e.State == Core.WorkOrders.WorkOrderState.Finish).ToList().Count / (list.Count * 1.0) : 0
                  });
              });

            return model;
        }

        /// <summary>
        /// 获取产线直通率信息列表(时间范围是当前日期前7天)
        /// </summary>
        /// <param name="shopId">车间Id</param>
        /// <returns>产线直通率信息列表</returns>
        public virtual EntityList<LineDataViewModel> GetLineCompleteInfos(double shopId, DateTime dbTime)
        {
            DateRange dr = new DateRange() { BeginValue = dbTime.AddDays(-6).Date, EndValue = dbTime.AddDays(1).Date };
            var datas = RT.Service.Resolve<WorkOrderController>().GetFinishWorkOrders(shopId, null, dr);
            var rstList = new EntityList<LineDataViewModel>();
            datas.Where(p => p.ResourceId.HasValue).OrderBy(p => p.PlanEndDate.Date).GroupBy(p => p.PlanEndDate.Date).ForEach(p =>
              {
                  var lineList = p.ToList();
                  DateTime curDate = p.Key;
                  lineList.GroupBy(e => e.ResourceId.Value).ForEach(e =>
                  {
                      var itemlist = e.ToList();
                      LineDataViewModel item = new LineDataViewModel()
                      {
                          Date = curDate,
                          LineId = e.Key,
                          LineName = e.FirstOrDefault(f => f.ResourceId == e.Key).Resource.Name,
                          Percentage = itemlist.Count > 0 ? itemlist.Where(f => f.State == Core.WorkOrders.WorkOrderState.Finish).ToList().Count / (itemlist.Count * 1.0) : 0
                      };
                      rstList.Add(item);
                  });
              });

            return rstList;
        }


        ///// <summary>
        ///// 设置预警颜色
        ///// </summary>
        ///// <param name="model">数据Model</param>
        ///// <param name="setting">车间目标设置</param>
        //private void SetAlertLevel(ChartCommonViewModel model, EntityList<ShopTargetSetting> setting)
        //{
        //    model.ChartAlertLevel = WorkBenchChartBase.Commons.ChartAlertLevel.Green;
        //    if (setting == null || setting.Count <= 0) return;
        //    var avgValue = model.BaseValues.Sum(p => p.Efficiency) / model.BaseValues.Count;
        //    var yellowAlert = setting.FirstOrDefault().YellowAlert;
        //    var redAlert = setting.FirstOrDefault().RedAlert;
        //    if (avgValue <= yellowAlert)
        //        model.ChartAlertLevel = WorkBenchChartBase.Commons.ChartAlertLevel.Yellow;
        //    if (avgValue <= redAlert)
        //        model.ChartAlertLevel = WorkBenchChartBase.Commons.ChartAlertLevel.Red;
        //}
    }
}
