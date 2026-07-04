using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 工单准时达成率报表控制器
    /// </summary>
    public class WorkOrderReachController : DomainController
    {
        /// <summary>
        /// 查询工单准时达成率报表记录
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>实体列表</returns>
        public virtual WoReachReportViewModel GetWorkOrderReach(WorkOrderReachCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
            var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrderReachReportData(criteria.WorkShopId, criteria.ResourceId, criteria.Shift, criteria.ModelId, criteria.PlanDate);
            var viewModel = new WoReachReportViewModel();
            viewModel.LayoutFileName = viewModel.GetType().Name;
            List<string> arr = new List<string>() { "工单总数".L10N(), "准时完工数".L10N(), "准时达成率".L10N(), "工单完工数".L10N(), "工单完工率".L10N() };

            workOrders.GroupBy(p => p.PlanEndDate.Date).ForEach(p =>
            {
                foreach (var item in arr)
                {
                    viewModel.WorkOrderReachList.Add(CreateModel(p.ToList(), item));
                }
            });

            viewModel.WorkOrderReachList.ForEach(w => w.RowOrder = arr.IndexOf(w.RowName));
            viewModel.Criteria = criteria;
            viewModel.MarkSaved();
            return viewModel;
        }

        /// <summary>
        /// 创建报表数据ViewModel
        /// </summary>
        /// <param name="workorders">workorders</param>
        /// <param name="rowname">rowname</param>
        /// <returns>model</returns>
        private WorkOrderReachViewModel CreateModel(List<WorkOrder> workorders, string rowname)
        {
            WorkOrderReachViewModel model = new WorkOrderReachViewModel();
            var temp = workorders[0];
            model.RowName = rowname;
            model.Year = temp.PlanEndDate.Year.ToString();
            model.Month = temp.PlanEndDate.ToString("yyyy年MM月".L10N());
            model.Week = "{0}年第{1}周".FormatArgs(temp.PlanEndDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(temp.PlanEndDate));
            model.PlanDate = temp.PlanEndDate.Date;
            model.TotalQty = workorders.Count;
            model.CompleteQty = workorders.Where(p => p.ActuFinishDate != null && p.ActuFinishDate <= p.PlanEndDate).ToList().Count;
            model.ReachRate = (model.CompleteQty * 1.0 / model.TotalQty);
            model.ClosedQty = workorders.Where(p => p.State == Core.WorkOrders.WorkOrderState.Finish).ToList().Count;
            model.ClosedRate = (model.ClosedQty * 1.0 / model.TotalQty);
            if (rowname == "工单总数".L10N())
            {
                model.Data = model.TotalQty;
            }
            else if (rowname == "准时完工数".L10N())
            {
                model.Data = model.CompleteQty;
            }
            else if (rowname == "准时达成率".L10N())
            {
                model.Data = model.ReachRate;
            }
            else if (rowname == "工单完工数".L10N())
            {
                model.Data = model.ClosedQty;
            }
            else if (rowname == "工单完工率".L10N())
            {
                model.Data = model.ClosedRate;
            }
            //switch (rowname)
            //{
            //    case "工单总数":
            //        model.Data = model.TotalQty;
            //        break;
            //    case "准时完工数":
            //        model.Data = model.CompleteQty;
            //        break;
            //    case "准时达成率":
            //        model.Data = model.ReachRate;
            //        break;
            //    case "工单完工数":
            //        model.Data = model.ClosedQty;
            //        break;
            //    case "工单完工率":
            //        model.Data = model.ClosedRate;
            //        break;
            //}

            return model;
        }

        /// <summary>
        /// 获取达成率工单明细
        /// </summary>
        /// <param name="criteria">查询</param>
        /// <returns>工单明细</returns>
        public virtual EntityList<WoReachDetailViewModel> GetWoReachDetailList(WorkOrderReachCriteria criteria, PagingInfo pageinfo = null)
        {
            var pt = ProcessDateTime(criteria);
            DateRange dr = new DateRange() { BeginValue = pt.Item1, EndValue = pt.Item2 };
            var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrderReachReportData(criteria.WorkShopId, criteria.ResourceId, criteria.Shift, criteria.ModelId, criteria.PlanDate, pageinfo, dr);
            EntityList<WoReachDetailViewModel> list = new EntityList<WoReachDetailViewModel>();
            var now = RF.Find<WorkOrder>().GetDbTime();
            foreach (var item in workOrders)
            {
                WoReachDetailViewModel entity = new WoReachDetailViewModel();
                entity.No = item.No;
                entity.ActuFinishDate = item.ActuFinishDate;
                entity.ActuStartDate = item.ActuStartDate;
                entity.FinishQty = item.FinishQty;
                entity.PlanBeginDate = item.PlanBeginDate;
                entity.PlanEndDate = item.PlanEndDate;
                entity.PlanQty = item.PlanQty;
                entity.ProductId = item.ProductId;
                entity.ProductCode = item.Product.Code;
                entity.State = item.State;
                entity.WorkShopName = item.WorkShopName;
                entity.WorkShopId = item.WorkShopId;
                entity.ResourceName = item.ResourceName;
                entity.ResourceId = item.ResourceId;
                if (now > item.PlanEndDate && !item.ActuFinishDate.HasValue || item.ActuFinishDate.HasValue && item.ActuFinishDate.Value > item.PlanEndDate)
                {
                    entity.ReachState = ReachState.IsLate;
                }
                else if (now <= item.PlanEndDate || item.ActuFinishDate.Value <= item.PlanEndDate)
                {
                    entity.ReachState = ReachState.IsNotLate;
                }
                else 
                { 
                    //
                }

                list.Add(entity);
            }
            list.SetTotalCount(workOrders.TotalCount);
            return list;
        }

        /// <summary>
        /// 求日期属性(年、月、 周、日)的范围
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <returns>日期组（开始时间，结束日期时间）</returns>
        private Tuple<DateTime, DateTime> ProcessDateTime(WorkOrderReachCriteria criteria)
        {
            switch (criteria.ColumnFieldName)
            {
                case nameof(WorkOrderReachViewModel.Year):
                    return Tuple.Create(DateTime.Parse(criteria.ColumnFieldValue + "年"), DateTime.Parse(criteria.ColumnFieldValue + "年").AddYears(1));

                case nameof(WorkOrderReachViewModel.Month):
                    return Tuple.Create(DateTime.Parse(criteria.ColumnFieldValue), DateTime.Parse(criteria.ColumnFieldValue).AddMonths(1));

                case nameof(WorkOrderReachViewModel.Week):
                    {
                        int year = int.Parse(criteria.ColumnFieldValue.Substring(0, 4));
                        int week = int.Parse(criteria.ColumnFieldValue.Substring(6, 2));
                        return RT.Service.Resolve<CommonController>().GetFirstEndDayOfWeek(year, week);
                    }

                case nameof(WorkOrderReachViewModel.PlanDate):
                default:
                    return Tuple.Create(DateTime.Parse(criteria.ColumnFieldValue), DateTime.Parse(criteria.ColumnFieldValue).AddDays(1));
            }
        }

        /// <summary>
        /// get viewmodel
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderReachViewModel> GetWorkOrderReachViewModel(WorkOrderReachCriteria criteria)
        {
            var criModel = GetWorkOrderReach(criteria);
            if (criModel == null || criModel.WorkOrderReachList.Count == 0)
            {
                return new EntityList<WorkOrderReachViewModel>();
            }
            return criModel.WorkOrderReachList;
        }

        ////private void SetModelData(WorkOrderReachViewModel model, EntityList<WorkOrderReachViewModel> e)
        ////{
        ////    model.TotalQty = e.Sum(f => f.TotalQty);
        ////    model.CompleteQty = e.Sum(f => f.CompleteQty);
        ////    model.ClosedQty = e.Sum(f => f.ClosedQty);
        ////    model.ReachRate = model.CompleteQty * 1.0 / model.TotalQty;
        ////    model.ClosedRate = model.ClosedQty * 1.0 / model.TotalQty;
        ////    switch (model.RowName)
        ////    {
        ////        case "工单总数":
        ////            model.Data = model.TotalQty;
        ////            break;
        ////        case "准时完工数":
        ////            model.Data = model.CompleteQty;
        ////            break;
        ////        case "准时达成率":
        ////            model.Data = model.ReachRate;
        ////            break;
        ////        case "工单完工数":
        ////            model.Data = model.ClosedQty;
        ////            break;
        ////        case "工单完工率":
        ////            model.Data = model.ClosedRate;
        ////            break;
        ////    }
        ////}
    }
}
