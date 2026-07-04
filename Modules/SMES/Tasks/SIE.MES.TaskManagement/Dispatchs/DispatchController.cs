using Castle.Core.Logging;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using IronPython.Compiler.Ast;
using NPOI.SS.Formula.PTG;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Common;
using SIE.Core.Equipments;
using SIE.Core.WorkOrders;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.WorkOrders;
using SIE.EventMessages.MES.Dispatchs;
using SIE.Items;
using SIE.Items.Items;
using SIE.Items.KzItemCategorys;
using SIE.KZ.Base.SmomControl;
using SIE.MES.LineAndon;
using SIE.MES.PackingQC;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MES.TaskManagement.Events;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.IOT;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.MES.TaskManagement.ShowBoards.ViewModels;
using SIE.MES.TaskManagement.Specifications;
using SIE.MES.TaskManagement.StandardWorkHours;
using SIE.MES.TaskManagement.TaskConfigs;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.Skills;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Stations;
using SIE.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Station = SIE.Tech.Stations.Station;
using StationController = SIE.Tech.Stations.StationController;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;
using WorkOrderController = SIE.MES.WorkOrders.WorkOrderController;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工控制器
    /// </summary>
    public partial class DispatchController : DomainController, IDispatchs
    {
        /// <summary>
        /// dispatchTask字符
        /// </summary>
        private readonly string _dispatchTaskString = "dispatchTask";

        #region 新增加逻辑 - 2026 - 6- 4号
        /// <summary>
        /// 更新派工明细下的工位名称
        /// </summary>
        /// <param name="disInfoList"></param>
        public virtual void UpateDispatchTaskDetail(List<DispatchTaskDetailInfo> disInfoList)
        {
            foreach (var info in disInfoList)
            {
                DB.Update<DispatchTaskDetail>()
                    .Set(x => x.AdoName, info.StationName)
                    .Where(x => x.AdoId == info.StationId && x.AdoType == AdoType.Station)
                    .Execute();
            }
        }
        #endregion

        /// <summary>
        /// 更新任务单的首末工序
        /// </summary>
        /// <param name="startProcessId"></param>
        /// <param name="endProcessId"></param>
        /// <param name="workOrderId"></param>
        public virtual void UpdateTaskStartEndProcess(double? startProcessId, double? endProcessId, double workOrderId)
        {

            var tasks = GetDispatchTasks(workOrderId, null);
            foreach (var task in tasks)
            {
                task.StartProcess = null;
                task.EndProcess = null;

                if (startProcessId != null && task.ProcessId == startProcessId)
                {
                    task.StartProcess = true;
                }
                if (endProcessId != null && task.ProcessId == endProcessId)
                {
                    task.EndProcess = true;
                }
            }
            if (tasks.Count > 0)
                RF.Save(tasks);
        }


        /// <summary>
        /// 最大报工数和剩余可报工数
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <returns></returns>
        public virtual (decimal, decimal) MaxReportQtyAndMaxRemainQty(DispatchTask dispatchTask, SIE.MES.TaskManagement.Reports.Enums.SourceType? SourceType = null)
        {
            var MaxReportQty = this.MaxReportQty(dispatchTask);
            //var q = MaxReportQty - dispatchTask.ReportQty - dispatchTask.SuspectQty;
            decimal uebto = 0;
            decimal.TryParse(dispatchTask.Uebto, out uebto);

            EntityList<MtartZtflRelation> ztflRelations = RT.Service.Resolve<SmomBaseController>().GetMtartZtflRelations(new List<string>() { dispatchTask.Product.Mtart });
            /*
            1.如果只勾了制卡数量就是任务单的数量/工单计划数量*制卡数量-当前任务单已报工数量-当前任务单可疑品数；
            2.都没勾或没维护就是取任务单的数量-当前任务单已报工数-当前任务单可疑品数
            3.只要勾上了启用容差，就是取任务单数*（1+容差%）-当前任务单的已报工数-当前任务单的可疑品数；
             */
            decimal q = 0;
            decimal? canReportQty = null;
            if (ztflRelations.All(p => (p.IsUebto == null || p.IsUebto == false) && (p.IsZtfl == null || p.IsZtfl == false)))
            {
                q = dispatchTask.DispatchQty - dispatchTask.ReportQty - dispatchTask.SuspectQty;
            }
            else if (ztflRelations.Any(p => p.IsUebto == true))
            {
                q = dispatchTask.DispatchQty * (1 + uebto / 100) - dispatchTask.ReportQty - dispatchTask.SuspectQty;
            }
            else
            {
                q = (dispatchTask.DispatchQty * (dispatchTask.Ztfl ?? 0)) / dispatchTask.WorkOrderPlanQty - dispatchTask.ReportQty - dispatchTask.SuspectQty;

                var despatchTasks = Query<DispatchTask>().Where(p => p.WorkOrderId == dispatchTask.WorkOrderId && p.ProcessId == dispatchTask.ProcessId).ToList();
                canReportQty = dispatchTask.Ztfl - despatchTasks.Sum(p => p.ReportQty + p.SuspectQty);
            }
            //单位非PCS的不需要取整，单位为PCS的向上取整
            var qty = string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(q) : q;

            if (canReportQty.HasValue)
                qty = Math.Min(qty, canReportQty.Value);

            var config = GetDispatchConfig();
            //没有配置项就用原逻辑
            if (config.IsValidScanQty == true)
            {
                //扫码报工逻辑要单独计算
                if (IsReportScanRemainQty(dispatchTask, SourceType, ztflRelations))
                {
                    var pTasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == dispatchTask.WorkOrderId && p.ProcessId == dispatchTask.ProcessId, null);
                    if (ztflRelations.Any(p => p.IsUebto == true))
                    {
                        //q = dispatchTask.DispatchQty * (1 + uebto / 100) - dispatchTask.ReportQty - dispatchTask.SuspectQty;
                        q = (dispatchTask.WorkOrderPlanQty * (1 + uebto / 100) - pTasks.Sum(p => p.ReportQty + p.SuspectQty));
                    }
                    else
                    {
                        //q = (decimal)(dispatchTask.DispatchQty / dispatchTask.WorkOrderPlanQty * dispatchTask.Ztfl) - dispatchTask.ReportQty - dispatchTask.SuspectQty;
                        q = ((dispatchTask.Ztfl ?? 0) - pTasks.Sum(p => p.ReportQty + p.SuspectQty));
                    }
                    qty = string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(q) : q;

                    q = (dispatchTask.ReportQty + qty + dispatchTask.SuspectQty);
                    MaxReportQty = string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(q) : q;
                }
            }
            if (qty < 0) qty = 0;
            return (MaxReportQty, qty);
        }

        /// <summary>
        /// 最大报工数
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <returns></returns>
        public virtual decimal MaxReportQty(DispatchTask dispatchTask)
        {
            //通过接口获取集团【是否启用制卡数量维护】数据
            EntityList<MtartZtflRelation> ztflRelations = RT.Service.Resolve<SmomBaseController>().GetMtartZtflRelations(new List<string>() { dispatchTask.ProductMtart });

            decimal MaxReportQty = 0;
            //1.当都没有勾上的时候，就直接用的任务单数量
            //2.当勾上了容差(不管有没有勾上制卡数量)，就直接用容差去算
            //3.其他情况(只剩单独勾上制卡数量的情况)，就用制卡数量去算
            //单位非PCS的不需要取整，单位为PCS的向上取整
            if (ztflRelations.All(p => (p.IsUebto == null || p.IsUebto == false) && (p.IsZtfl == null || p.IsZtfl == false)))
            {
                MaxReportQty = string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(dispatchTask.DispatchQty) : dispatchTask.DispatchQty;
            }
            else if (ztflRelations.Any(p => p.IsUebto == true))
            {
                MaxReportQty = dispatchTask.MaxReportQty;
            }
            else
            {
                var qty = (dispatchTask.DispatchQty * (dispatchTask.Ztfl ?? 0)) / dispatchTask.WorkOrderPlanQty;
                MaxReportQty = string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(qty) : qty;
            }


            ////当启用了容差或者制卡数量为空、为0 的时候
            //if (ztflRelations.Any(p => p.Mtart == dispatchTask.ProductMtart && p.IsUebto == true) || dispatchTask.Ztfl == null || dispatchTask.Ztfl == 0)
            //    MaxReportQty = dispatchTask.MaxReportQty;//workOrder.PlanQty * (1 + Uebto / 100);
            //else
            //    MaxReportQty = (int)Math.Ceiling((dispatchTask.DispatchQty * (dispatchTask.Ztfl ?? 0)) / dispatchTask.WorkOrderPlanQty);
            return MaxReportQty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="SourceType"></param>
        /// <returns></returns>
        public virtual bool IsReportScanRemainQty(DispatchTask dispatchTask, SIE.MES.TaskManagement.Reports.Enums.SourceType? SourceType = null, EntityList<MtartZtflRelation> ztflRelations = null)
        {
            /*扫码报工时
 在“是否启用制卡数量”功能中启用了制卡数量或容差时:
1首先校验该工序的任务单数量之和是否=工单的计划数量
1.1若等于，则启用校验：若有多个任务单时，存在唯一的任务单状态不是完工，其余任务单状态均为完工或关闭，则当前任务单的剩余可报工数为工单的制卡数量或计划数量*（1+容差%）-（当前工单当前工序的已报工数+可疑品数）;
1.2若不等于(排程工序可能未排程完)，或存在多个任务单状态不是完工或关闭，则剩余可报工数按原逻辑即可
 */

            if (ztflRelations == null)
                ztflRelations = RT.Service.Resolve<SmomBaseController>().GetMtartZtflRelations(new List<string>() { dispatchTask.Product.Mtart });
            //获取工单下全部任务单
            var allTasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == dispatchTask.WorkOrderId, null);
            //获取相同工序任务单
            var pTasks = allTasks.Where(p => p.ProcessId == dispatchTask.ProcessId).ToList();
            //获取可操作的任务单
            var canTasks = pTasks.Where(p => p.TaskStatus != DispatchTaskStatus.Finished && p.TaskStatus != DispatchTaskStatus.Closed).ToList();
            //扫码报工类型，启用了制卡数量或者容差，相同工序全部任务单任务单=工单计划数，相同工序只剩当前任务单未完成
            if (SourceType == SIE.MES.TaskManagement.Reports.Enums.SourceType.Report_Scan && ztflRelations.Any(p => p.IsUebto == true || p.IsZtfl == true) && pTasks.Sum(p => p.DispatchQty) == dispatchTask.WorkOrderPlanQty && canTasks.Count == 1 && canTasks.FirstOrDefault().Id == dispatchTask.Id)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 获取工序最大剩余可报工数
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <returns></returns>
        public virtual decimal GetProcessMaxRemainQty(DispatchTask dispatchTask)
        {
            var tasks = Query<DispatchTask>().Where(p => p.WorkOrderId == dispatchTask.WorkOrderId && p.ProcessId == dispatchTask.ProcessId && p.TaskStatus != DispatchTaskStatus.Closed).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //通过接口获取集团【是否启用制卡数量维护】数据
            EntityList<MtartZtflRelation> ztflRelations = RT.Service.Resolve<SmomBaseController>().GetMtartZtflRelations(new List<string>() { dispatchTask.ProductMtart });

            decimal MaxReportQty = 0;
            var DispatchQty = tasks.Sum(p => p.DispatchQty);
            var ReportQty = tasks.Sum(p => p.ReportQty);
            var SuspectQty = tasks.Sum(p => p.SuspectQty);
            /*
             1.如果只勾了制卡数量就是制卡数量-当前工序已报工数量-当前工序可疑品数；
             2.都没勾或没维护就是取工单的计划数量-当前工序已报工数-当前工序可疑品数
             3.只要勾上了启用容差，就是取工单计划数*（1+容差%）-当前工序的已报工数-当前工序的可疑品数；
             */
            if (ztflRelations.Count < 1 || ztflRelations.All(p => (p.IsUebto == null || p.IsUebto == false) && (p.IsZtfl == null || p.IsZtfl == false)))
            {
                //var pTasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByExpression(p => p.WorkOrderId == dispatchTask.WorkOrderId && p.ProcessId == dispatchTask.ProcessId, null);
                MaxReportQty = dispatchTask.WorkOrderPlanQty;//- tasks.Sum(p => p.ReportQty - p.SuspectQty);
            }
            else if (ztflRelations.Any(p => p.IsUebto == true))
            {
                decimal uebto = 0;
                decimal.TryParse(dispatchTask.Uebto, out uebto);

                MaxReportQty = dispatchTask.WorkOrderPlanQty * (1 + uebto / 100);//tasks.Sum(p => p.MaxReportQty);
            }
            else
            {
                var q = dispatchTask.Ztfl ?? 0;//(DispatchQty * (dispatchTask.Ztfl ?? 0)) / dispatchTask.WorkOrderPlanQty;
                MaxReportQty = string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(q) : q;
            }

            //if (ztflRelations.Any(p => p.Mtart == dispatchTask.ProductMtart && p.IsUebto == true) || dispatchTask.Ztfl == null || dispatchTask.Ztfl == 0)
            //    MaxReportQty = tasks.Sum(p => p.MaxReportQty);
            //else
            //{
            //    var q = (DispatchQty * (dispatchTask.Ztfl ?? 0)) / dispatchTask.WorkOrderPlanQty;
            //    MaxReportQty = string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(q) : q;
            //}

            var rq = MaxReportQty - ReportQty - SuspectQty;
            return string.Equals(dispatchTask.UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(rq) : rq;

        }

        #region 任务单自动完工调度

        /// <summary>
        /// 任务单自动完工调度方法
        /// </summary>
        public virtual int AutoTaskFinishJob()
        {
            //按照工单，获取任务单

            //获取生产中的工单
            var workOrders = Query<WorkOrder>().Where(p => p.State != WorkOrderState.Close && p.State != WorkOrderState.Finish && p.IsPause == YesNo.No).Exists<DispatchTask>((x, y) => y.Where(p => p.WorkOrderId == x.Id && p.TaskStatus == DispatchTaskStatus.Executing)).OrderByDescending(p => p.CreateDate).ToList(new PagingInfo(1, 50000), new EagerLoadOptions().LoadWithViewProperty());

            var workOrderIds = workOrders.Select(p => p.Id).Distinct().ToList();
            //根据工单获取任务单,且工序属性中没勾上报工校验
            //var tasks = GetDispatchTasksByWorkOrderIds(workOrderIds);
            var tasks = workOrderIds.SplitContains(ids =>
            {
                return Query<DispatchTask>().Where(p => ids.Contains((double)p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();
            //var processPtys = processIds.SplitContains(ids =>
            //{
            //    return Query<ProcessPty>().Where(p => ids.Contains(p.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //});

            var curTime = RF.Find<WorkOrder>().GetDbTime();

            int num = 0;
            var productMtarts = workOrders.Select(p => p.ProductMtart).Distinct().ToList();
            //【是否启用制卡数量维护】数据
            EntityList<MtartZtflRelation> ztflRelations = RT.Service.Resolve<SmomBaseController>().GetMtartZtflRelations(productMtarts);

            foreach (var workOrder in workOrders)
            {
                using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
                {
                    var tks = tasks.Where(p => p.WorkOrderId == workOrder.Id).OrderBy(p => p.Seq).ToList();
                    foreach (var current in tks)
                    {
                        var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { current.ProcessId.Value }, current.ProductId);

                        var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(current.ProductId);
                        var pps = new List<ProcessPty>();
                        if (kzItemCategory != null)
                        {
                            pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                        }
                        ////当找得到分类得时候，优先找到分类的，然后再找工序的
                        if (pps.Count == 0)
                            pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                        var processPty = pps.FirstOrDefault();
                        if (processPty != null && processPty.IsReportValid == true)
                            continue;

                        //只有执行中，且非暂停的时候，才可以继续跑下去
                        if (current.TaskStatus != DispatchTaskStatus.Executing || current.IsPause != YesNo.No)
                            continue;

                        //是否完工
                        var isFinish = false;

                        var dic = tks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());

                        //首工序计算方式
                        if (!dic.Any(p => p.Key < current.Seq))
                        {
                            if (current.ReportQty + current.SuspectQty >= current.DispatchQty)
                            {
                                isFinish = true;
                            }
                        }
                        //非首工序时候
                        else
                        {
                            //获取前工序的全部任务单
                            var lastTasks = dic.Where(p => p.Key < current.Seq).OrderByDescending(p => p.Key).FirstOrDefault().Value;
                            //获取全部前工序
                            var allLastTasks = dic.Where(p => p.Key < current.Seq).SelectMany(p => p.Value).ToList();
                            //获取相同工序的其他任务单
                            var proTasks = tks.Where(p => p.ProcessId == current.ProcessId && p.Id != current.Id).ToList();

                            /*
                             非首工序：
                                如果当前任务单为执行中且前工序有不合格且前工序可疑品为0且前工序全部完工或关闭；那么再校验当前工序除当前任务单外，当前工序其他任务单必须是完成或关闭的，才根据以下逻辑进行判断：
                                当前任务单已报工数+当前任务单可疑品数 + 前工序全部任务单的不合格数 大于等于 当前任务单任务数，大于等于则完工，反之不完工。
                             */
                            if (current.TaskStatus == DispatchTaskStatus.Executing && allLastTasks.Sum(p => p.NgQty) != 0 && lastTasks.Sum(p => p.SuspectQty) == 0 && lastTasks.All(p => p.TaskStatus == DispatchTaskStatus.Finished || p.TaskStatus == DispatchTaskStatus.Closed) && (proTasks.Count == 0 || (proTasks.Count > 0 && proTasks.All(p => p.TaskStatus == DispatchTaskStatus.Finished || p.TaskStatus == DispatchTaskStatus.Closed))) && current.ReportQty + current.SuspectQty + allLastTasks.Sum(p => p.NgQty) >= current.DispatchQty)
                            {
                                isFinish = true;
                            }
                        }

                        #region 

                        //var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
                        //var newMaterialPro = new List<string>();
                        //if (!config.NewMaterialProValid.IsNullOrEmpty())
                        //{
                        //    newMaterialPro = config.NewMaterialProValid.Split(',').ToList();
                        //}
                        ////新材料的工序按照原来的方式去做
                        //if (newMaterialPro.Count > 0 && newMaterialPro.Contains(current.ProcessCode))
                        //{
                        //    var lastTasks = tks.Where(p => p.Seq < current.Seq).ToList();
                        //    //首工序计算方式
                        //    if (current.Seq == 10)
                        //    {
                        //        if (current.ReportQty + current.SuspectQty >= current.DispatchQty)
                        //        {
                        //            isFinish = true;
                        //        }
                        //    }
                        //    //非首工序计算方式
                        //    else if (lastTasks.Count >= 1)
                        //    {
                        //        //如果前工序没有不合格数，则不校验多个同工序的任务单状态
                        //        if (lastTasks.Sum(p => p.NgQty) != 0)
                        //        {
                        //            //获取相同工序的任务单
                        //            var proTasks = tks.Where(p => p.ProcessId == current.ProcessId && p.Id != current.Id).ToList();
                        //            //当任务单存在非完成的，就跳过，相同的工序任务单，其他必须是完成的，否则就跳过(这样会出现一个问题就是，当相同工序多个任务单的时候，只要有两条以上的是非完成的，那么就永远都无法完成，已提出，但是仍然要改)
                        //            if (proTasks.Count > 0 && proTasks.Any(p => p.TaskStatus != DispatchTaskStatus.Finished))
                        //                continue;
                        //        }
                        //        if (current.ReportQty + current.SuspectQty + lastTasks.Sum(p => p.NgQty) >= current.DispatchQty)
                        //        {
                        //            isFinish = true;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (RT.Service.Resolve<ReportController>().ValidQtyUpdateTaskState(current, 0, 0) || RT.Service.Resolve<ReportController>().ValidFirstProcessTaskState(current))
                        //    {
                        //        isFinish = true;
                        //    }
                        //}
                        #endregion

                        if (isFinish == true)
                        {
                            current.TaskStatus = DispatchTaskStatus.Finished;
                            current.EndTime = curTime;
                            num += 1;
                            RF.Save(current);
                        }
                    }
                    //当所有任务单都是完成或者关闭的时候，就更新为已完成
                    if (tks.All(p => p.TaskStatus == DispatchTaskStatus.Finished || p.TaskStatus == DispatchTaskStatus.Closed))
                    {
                        decimal Uebto = 0;
                        decimal.TryParse(workOrder.Uebto, out Uebto);

                        decimal qty = 0;
                        //当启用了容差或者制卡数量为空、为0 的时候
                        if (ztflRelations.Any(p => p.Mtart == workOrder.ProductMtart && p.IsUebto == true) || workOrder.Ztfl == null || workOrder.Ztfl == 0)
                            qty = workOrder.PlanQty * (1 + Uebto / 100);
                        else
                            qty = workOrder.Ztfl.Value;

                        //数量达到了，才能完工(当工单不达到这个指，那么工单永远都不会关闭，已讨论，仍然做)
                        if (workOrder.FinishQty >= (int)Math.Ceiling(qty))
                        {
                            workOrder.State = WorkOrderState.Finish;
                            workOrder.ActuFinishDate = curTime;
                            RF.Save(workOrder);
                        }

                    }
                    tran.Complete();
                }
            }
            return num;
        }

        #endregion

        #region 打开任务单

        /// <summary>
        /// 完工/打开
        /// </summary>
        /// <param name="taskIds"></param>
        public virtual void FinishOrOpenTasks(List<double> taskIds)
        {
            var tasks = taskIds.SplitContains(ids =>
            {
                return GetDispatchTasks(ids.ToList());
            });

            var errTaskNos = tasks.Where(p => p.TaskStatus != DispatchTaskStatus.Finished && p.TaskStatus != DispatchTaskStatus.Executing).Select(p => p.No).Distinct().ToList();
            if (errTaskNos.Count > 0)
            {
                throw new ValidationException("任务单【{0}】状态不为完工/执行中，不可修改".L10nFormat(string.Join("、", errTaskNos)));
            }
            foreach (var task in tasks)
            {
                //切记只有完工或者执行中的，才可进行操作
                if (task.TaskStatus == DispatchTaskStatus.Finished)
                {
                    task.TaskStatus = DispatchTaskStatus.Executing;
                }
                else
                {
                    task.TaskStatus = DispatchTaskStatus.Finished;
                }
            }
            if (tasks.Count > 0)
                RF.Save(tasks);
        }

        /// <summary>
        /// 打开任务单
        /// </summary>
        /// <param name="workOrderId"></param>
        public virtual void OpenTasks(double workOrderId)
        {

            var dispatchTasks = GetDispatchTasks(workOrderId);

            foreach (var dispatchTask in dispatchTasks)
            {
                //排程退回的，就一直是关闭，什么情况都不打开，不改变状态
                if (dispatchTask.IsSchedulingInfReturn == YesNo.Yes)
                    continue;

                if (dispatchTask.OldTaskStatus != null)
                {
                    dispatchTask.TaskStatus = dispatchTask.OldTaskStatus.Value;
                    dispatchTask.OldTaskStatus = null;
                    dispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                }
            }

            // 恢复成执行中的数据创建日志
            var executings = dispatchTasks.Where(p => p.TaskStatus == DispatchTaskStatus.Executing).AsEntityList();
            EntityList<ReportOperateLog> logs = new EntityList<ReportOperateLog>();
            if (executings.Count > 0)
            {
                logs = RT.Service.Resolve<ReportController>().GenerateTaskOptStartLog(executings);
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(dispatchTasks);
                if (logs.Count > 0)
                {
                    RF.Save(logs);
                }
                tran.Complete();
            }
        }

        #endregion

        /// <summary>
        /// 根据工单创建任务单
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void GenerateTaskByWorkOrderIds(List<double> workOrderIds)
        {
            var wos = Query<WorkOrder>().Where(p => workOrderIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            foreach (var workOrder in wos)
            {
                var taskConfig = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
                if (!taskConfig.IsGenerate)
                {
                }
                else
                {
                    RstTaskBillInfo billInfo = RT.Service.Resolve<DispatchController>().IsCanSyntypeReport(workOrder);
                    bool isAccordConfig = true;
                    if (billInfo.OrgIsSyntype == true && billInfo.IsSyntype == false)
                    {
                        isAccordConfig = false;
                    }

                    var tasks = RT.Service.Resolve<DispatchController>().CreateDispatch(workOrder, isAccordConfig, taskConfig, true, true);
                    //获取工序属性
                    var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

                    //获取首工序
                    var processId = workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault()?.StartProcess;
                    if (processId == null)
                        processId = workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault()?.ProcessId;

                    foreach (var task in tasks)
                    {
                        if (task.ProcessId == null)
                            throw new ValidationException("任务单未获取到工序，无法生成任务单!".L10N());
                        //派工管理的资源字段取值修改：由原来取工单的资源，改成取工单工艺路线中工序对应的工作中心字段
                        if (task.ProcessId != null)
                        {
                            //这个工序必须要是在该工厂(当下库存组织下的)
                            var info = workOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == task.Process.Code && p.Factory == invOrg.ExternalId);
                            if (info == null)
                                throw new ValidationException("未找到工序[{0}]工厂[{1}]的工艺路线信息,无法生成任务单".L10nFormat(task.Process.Code, invOrg.ExternalId));
                            if (info != null)
                            {
                                WipResource wipResource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(info.WorkCenterCode);
                                if (wipResource == null)
                                    throw new ValidationException("生产资源未维护编码[{0}],无法生成任务单".L10nFormat(info.WorkCenterCode));
                                if (wipResource != null)
                                {
                                    //将任务单的来源状态改为MES生成
                                    task.SourceType = MES.TaskManagement.Dispatchs.SourceType.Mes;
                                    task.ResourceId = wipResource.Id;
                                    //task.PersistenceStatus = PersistenceStatus.Modified;
                                    RF.Save(task);
                                }

                            }
                        }
                        EntityList<ProcessPty> processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { task.ProcessId.Value }, workOrder.ProductId);
                        var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(workOrder.ProductId);
                        var pps = new List<ProcessPty>();
                        if (kzItemCategory != null)
                        {
                            pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                        }
                        ////当找得到分类得时候，优先找到分类的，然后再找工序的
                        if (pps.Count == 0)
                            pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                        if (task.ProcessId != null && pps.Any(p => p.ProcessId == task.ProcessId && p.IsAutoDispatch == true))
                        {
                            ////当找得到物料得时候，优先找到物料的，然后再找工序的
                            //var pps = processPtys.Where(p => p.CategoryItemCode == task.Product.Code).ToList();
                            //if (pps.Count == 0)
                            //    pps = processPtys.ToList();
                            //当工序属性中维护了自动派工且资源类型必须是产线，那么生成派工单后，自动进行派工
                            if (task.Resource.SourceType == SyncSourceType.LineAndon)
                            {
                                var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new List<double>() { task.Id });
                                if (errMsg.Length == 0)
                                { }
                                else
                                    throw new ValidationException(errMsg);
                            }
                            //当资源类型不为产线时候，除了满足非排程点 + 首工序的任务之外，其他都可以自动派工
                            if (task.Resource.SourceType != SyncSourceType.LineAndon)
                            {
                                //非排程点 + 首工序 任务,不要自动派工(因为他们要选择资源产线后才可以派工)
                                if (processId == task.ProcessId && (pps.Any(p => p.ProcessId == task.ProcessId && p.Scheduling == false) || pps.All(p => p.ProcessId != task.ProcessId)))
                                { }
                                else
                                {
                                    var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new List<double>() { task.Id });
                                    if (errMsg.Length == 0)
                                    { }
                                    else
                                        throw new ValidationException(errMsg);
                                }
                            }
                        }
                    }
                }
            }
        }


        public virtual List<double> GetWorkOrderRoutingProcessesByTaskIds(List<double> taskIds)
        {
            var tasks = GetDispatchTasks(taskIds);
            var list = Query<WorkOrderRoutingProcess>().Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id).Join<WorkOrder, DispatchTask>((x, y) => x.Id == y.WorkOrderId && taskIds.Contains(y.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

            var workOrderIds = tasks.Select(p => p.WorkOrderId.Value).Distinct().ToList();
            var allTasks = GetDispatchTasksByWorkOrderIds(workOrderIds);

            var TaskWipResourceList = new List<double>();

            //根据工单工艺路线校验当前工序是否为首工序
            foreach (var task in tasks)
            {
                var lastProcessIds = list.Where(p => p.WorkOrderId == task.WorkOrderId && p.Index < task.Seq).Select(p => p.ProcessId ?? 0).Distinct().ToList();

                var processIds = new List<double>();
                if (lastProcessIds.Count > 0)
                    processIds.AddRange(lastProcessIds);

                processIds.Add(task.ProcessId.Value);

                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds, task.ProductId);

                //var processId = list.Where(p => p.WorkOrderId == task.WorkOrderId).OrderBy(p => p.Index).ToList();

                var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(task.ProductId);
                var pps = new List<ProcessPty>();
                if (kzItemCategory != null)
                {
                    pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId && task.ProcessId == p.ProcessId).ToList();
                }
                ////当找得到分类得时候，优先找到分类的，然后再找工序的
                if (pps.Count == 0)
                    pps = processPtys.Where(p => p.KzCategoryId == null && task.ProcessId == p.ProcessId).ToList();

                var Seq = allTasks.Where(p => p.WorkOrderId == task.WorkOrderId).OrderBy(p => p.Seq).FirstOrDefault().Seq;


                //前面工序必须都不是排程点(防止现场有人忘记排程或者慢排程)，且当前任务单必须是最先的任务单，且当前任务单是非排程点
                if (!processPtys.Any(p => lastProcessIds.Contains(p.ProcessId) && p.Scheduling == true) && task.Seq == Seq && (pps.Any(p => p.ProcessId == task.ProcessId && p.Scheduling == false) || pps.All(p => p.ProcessId != task.ProcessId)))
                {
                    //需要派工到产线的任务单
                    TaskWipResourceList.Add(task.Id);
                }
            }
            return TaskWipResourceList;
        }

        /// <summary>
        /// 更新任务单资源
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="resourceId"></param>
        public virtual void UpdateTaskResource(List<double> taskId, double resourceId)
        {
            var list = Query<DispatchTask>().Where(p => taskId.Contains(p.Id)).ToList();
            foreach (var l in list)
            {
                l.PersistenceStatus = PersistenceStatus.Modified;
                //记录派工原工作中心资源，后面撤销派工的时候，可以还原回去
                l.OldResourceId = l.ResourceId;
                l.ResourceId = resourceId;
            }
            RF.Save(list);
        }


        /// <summary>
        /// 根据任务单Id获取生产批次打印实体的轴号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetPrintAxisNumberByTaskId(double id)
        {
            //var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            //var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            //if (dispatchConfig == null)
            //    throw new ValidationException("派工管理配置项不存在.".L10N());
            //var dispatchTask = RF.GetById<DispatchTask>(id);
            //string batchNo = "";
            ////2810用的是其他绕包和非绕包，以后上新系统需要改
            //if (invOrg.ExternalId == "2810")
            //{
            //    NumberRuleDetail detail = null;
            //    if (dispatchTask.Product.Zmc.Contains("绕包"))
            //    {
            //        if (!dispatchConfig.EntangleNumberRuleId.HasValue)
            //            throw new ValidationException("派工管理绕包线编码规则配置项未配置.".L10N());
            //        detail = dispatchConfig.EntangleNumberRule.DetailList.FirstOrDefault(p => p.GetType() == typeof(WoAxisNumberAlgorithm));
            //        if (detail == null)
            //            throw new ValidationException("派工管理绕包线编码规则配置项明细中，未配置轴号编码算法.".L10N());
            //    }
            //    else
            //    {
            //        if (!dispatchConfig.UnEntangleNumberRuleId.HasValue)
            //            throw new ValidationException("派工管理非绕包线编码规则配置项未配置.".L10N());
            //        detail = dispatchConfig.UnEntangleNumberRule.DetailList.FirstOrDefault(p => p.GetType() == typeof(WoAxisNumberAlgorithm));
            //        if(detail == null)
            //            throw new ValidationException("派工管理非绕包线编码规则配置项明细中，未配置轴号编码算法.".L10N());
            //    }

            //    var Shift = RT.Service.Resolve<ItemCusotmerDataController>().ShiftAlgorithmGetCode();
            //    var Resource = string.Empty;
            //    if (dispatchTask.ResourceId != null)
            //        Resource = RT.Service.Resolve<IWipResources>().WipResourcesName(dispatchTask.ResourceId.Value);
            //    var sequenc = RT.Service.Resolve<AlgorithmController>().GetSequence(detail.Id, 1, Shift + "-WoAxisNumberAlgorithm-" + Resource);

            //}
            return string.Empty;
        }

        /// <summary>
        /// 根据任务单Id获取资源名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetResourceNameByTaskId(double id)
        {
            var task = RF.GetById<DispatchTask>(id);
            if (task == null || task.Resource == null)
                return string.Empty;
            return task.Resource.Name;
        }

        /// <summary>
        /// 根据任务单Id获取工序的分单数量
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual decimal GetProcessZcodeByTaskId(double Id)
        {
            var layoutInfos = Query<LayoutInfo>().Join<Process>((x, y) => x.ProcessCode == y.Code).Join<Process, DispatchTask>((x, y) => x.Id == y.ProcessId && y.Id == Id).ToList();
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            var layoutInfo = layoutInfos.FirstOrDefault(p => p.Factory == invOrg.ExternalId);
            if (layoutInfo == null)
                layoutInfo = layoutInfos.FirstOrDefault();
            return layoutInfo?.Zcode ?? 0;
        }

        /// <summary>
        /// 打开上料
        /// </summary>
        public virtual void OpenFeedingClose(List<double> taskIds)
        {
            //打开
            DB.Update<DispatchTask>().Set(p => p.IsFeedingClose, false).Where(p => taskIds.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 排程退回
        /// </summary>
        /// <param name="taskId"></param>
        public virtual void SchedulingInfReturn(List<double> taskIds, string ReturnReason)
        {
            var tasks = new EntityList<DispatchTask>();
            var schedulingInfValues = new EntityList<SchedulingInfValue>();
            var schedulingInfs = new EntityList<SchedulingInf>();

            foreach (var taskId in taskIds)
            {
                //当集合不为0且查出来的任务单含有taskId，就跳过(因为我们校验、退回的时候，是会把同一个排程任务生成的多个任务单一起校验和退回，判断是为了防止，你在选择多条的时候，这多条中可能包含同一个排程任务的，这样可能会导致重复校验，以及返回重复的任务单)(下面方法也会将同一个排程任务的任务单都查出来)
                if (tasks.Count > 0 && tasks.Any(p => p.Id == taskId))
                    continue;
                //获取要操作的同一个排程任务单的全部任务单
                var tks = SchedulingInfReturnValid(taskId);
                var tkIds = tks.Select(p => p.Id).Distinct().ToList();
                //获取这些任务单的全部排程中间表各个日期的数值
                var sivs = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfsByTaskIds(tkIds);
                var schedulingInfIds = sivs.Select(p => p.SchedulingInfId).Distinct().ToList();
                //获取各个任务单的排程中间表数据
                var sis = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfsByIds(schedulingInfIds);
                foreach (var task in tks)
                {
                    //改变任务单状态为待派工、是否排程退回为是，输入退回原因
                    task.IsSchedulingInfReturn = YesNo.Yes;
                    task.SchedulingInfReturnReason = ReturnReason;
                    task.TaskStatus = DispatchTaskStatus.ToDispatch;
                    task.PersistenceStatus = PersistenceStatus.Modified;

                    sivs.Where(p => p.DispatchTask1Id == task.Id).ForEach(p =>
                    {
                        p.PersistenceStatus = PersistenceStatus.Modified;
                        p.Value1 = null;
                        p.DispatchTask1Id = null;
                    });
                    sivs.Where(p => p.DispatchTask2Id == task.Id).ForEach(p =>
                    {
                        p.PersistenceStatus = PersistenceStatus.Modified;
                        p.Value2 = null;
                        p.DispatchTask2Id = null;
                    });
                }
                //是否排程退回为是，输入退回原因
                sis.ForEach(p =>
                {
                    p.IsSchedulingInfReturn = YesNo.Yes;
                    p.SchedulingInfReturnReason = ReturnReason;
                    p.PersistenceStatus = PersistenceStatus.Modified;

                });

                tasks.AddRange(tks);
                schedulingInfValues.AddRange(sivs);
                schedulingInfs.AddRange(sis);
            }
            if (tasks.Count > 0)
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(tasks);
                    RF.Save(schedulingInfValues);
                    RF.Save(schedulingInfs);

                    //先暂停
                    var selectedIds = tasks.Where(p => p.TaskStatus != DispatchTaskStatus.Pause).Select(p => p.Id).Distinct().ToList();
                    RT.Service.Resolve<DispatchController>().SetPauseTasks(selectedIds);
                    //关闭任务单
                    RT.Service.Resolve<DispatchController>().SetCloseTasks(tasks.Select(p => p.Id).ToList());
                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 排程退回校验(返回同一个排程中间表的多个任务单)
        /// </summary>
        /// <param name="taskId"></param>
        public virtual EntityList<DispatchTask> SchedulingInfReturnValid(double taskId)
        {
            var task = RF.GetById<DispatchTask>(taskId, new EagerLoadOptions().LoadWithViewProperty());

            if (task.SourceType != SourceType.SchedulingInf)
            {
                throw new ValidationException("任务单{0}非排程导入，无法退回".L10nFormat(task.No));
            }

            var tasks = Query<DispatchTask>().Where(p => p.WorkOrderId == task.WorkOrderId && p.ProcessId == task.ProcessId && p.SourceType == SourceType.SchedulingInf && (p.IsSchedulingInfReturn == null || p.IsSchedulingInfReturn == YesNo.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //排程退回时通过工单、生产资源查找包装QC确认记录，有则不允许退回
            var resourceIds = tasks.Select(p => p.ResourceId).Distinct().ToList();
            var count = Query<PackingQc>().Where(p => resourceIds.Contains(p.ResourceId))
                .Exists<PackingDetail>((a, b) => b.Where(p => p.PackingQcId == a.Id && p.WorkOrderNo.Contains(task.WorkOrderNo)))
                .Count();
            if (count > 0)
                throw new ValidationException("工单[{0}]存在包装QC确认记录，无法退回".L10nFormat(task.WorkOrderNo));

            if (tasks.Any(p => p.TaskStatus != DispatchTaskStatus.Dispatching && p.TaskStatus != DispatchTaskStatus.ToDispatch))
            {
                var taskNos = tasks.Where(p => p.TaskStatus != DispatchTaskStatus.Dispatching && p.TaskStatus != DispatchTaskStatus.ToDispatch).Select(p => p.No).Distinct().ToList();
                throw new ValidationException("任务单{0}非派工中、非待派工，无法退回".L10nFormat(string.Join("、", taskNos)));
            }
            //获取工序属性
            var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();
            //var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds, task.ProductId);

            var nos = new List<string>();
            foreach (var p in tasks)
            {
                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { p.ProcessId.Value }, task.ProductId);
                var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(task.ProductId);
                var pps = new List<ProcessPty>();
                if (kzItemCategory != null)
                {
                    pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                }
                ////当找得到分类得时候，优先找到分类的，然后再找工序的
                if (pps.Count == 0)
                    pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                if (pps.Any(a => a.ProcessId == p.ProcessId && a.Scheduling == false))
                {
                    nos.Add(p.No);
                }
            }
            if (nos.Count > 0)
            {
                throw new ValidationException("任务单{0}的工序未勾选排程点，不可退回".L10nFormat(string.Join("、", nos)));
            }

            //if (tasks.Any(p =>
            //{
            //    //当找得到物料得时候，优先找到物料的,找不到就直接查全部，然后再找工序的
            //    var pps = processPtys.Where(f => f.CategoryItemCode == p.ProductCode).ToList();
            //    if (pps.Count == 0)
            //        pps = processPtys.ToList();
            //    return pps.Any(a => a.ProcessId == p.ProcessId && a.Scheduling == false);
            //}
            //))
            //{
            //    var taskNos = tasks.Where(p =>
            //    {
            //        //当找得到物料得时候，优先找到物料的,找不到就直接查全部，然后再找工序的
            //        var pps = processPtys.Where(f => f.CategoryItemCode == p.ProductCode).ToList();
            //        if (pps.Count == 0)
            //            pps = processPtys.ToList();
            //        return pps.Any(a => a.ProcessId == p.ProcessId && a.Scheduling == false);

            //    }).Select(p => p.No).Distinct().ToList();
            //    throw new ValidationException("任务单{0}的工序未勾选排程点，不可退回".L10nFormat(string.Join("、", taskNos)));
            //}
            return tasks;
        }

        /// <summary>
        /// 根据任务单Id获取任务单
        /// </summary>
        /// <param name="dispatchTaskId">任务单Id</param>
        /// <returns>任务单</returns>
        public virtual DispatchTask GetDispatchTask(double dispatchTaskId)
        {
            return Query<DispatchTask>().Where(p => p.Id == dispatchTaskId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据任务单No获取任务单
        /// </summary>
        /// <param name="no">任务单No</param>
        /// <returns>任务单</returns>
        public virtual DispatchTask GetDispatchTask(string no)
        {
            return Query<DispatchTask>().Where(p => p.No == no).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务列表
        /// Expression不支持序列号，前端不允许调用
        /// </summary>
        /// <param name="exp">条件</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<DispatchTask> GetDispatchTasksByExpression(Expression<Func<DispatchTask, bool>> exp, PagingInfo pagingInfo = null)
        {
            var query = Query<DispatchTask>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务单
        /// Expression不支持序列号，前端不允许调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="ascExp">升序排序条件</param>
        /// <param name="descExp">降序排序条件</param>
        /// <returns>任务单</returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskByExpression(Expression<Func<DispatchTask, bool>> exp, Expression<Func<DispatchTask, object>> ascExp = null, Expression<Func<DispatchTask, object>> descExp = null)
        {
            var query = Query<DispatchTask>();
            if (exp != null)
                query.Where(exp);
            if (ascExp != null)
                query.OrderBy(ascExp);
            if (descExp != null)
                query.OrderByDescending(descExp);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取员工关联的任务单
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="retrospectType">产品追溯方式</param>
        /// <param name="processId">工序ID</param>
        /// <param name="reportMode">报工方式</param>
        /// <returns>关联的任务单集合</returns>
        public virtual EntityList<DispatchTask> GetEmployeeRefDispatchTasks(double employeeId, Core.Items.RetrospectType retrospectType, double processId = 0, ReportMode reportMode = ReportMode.Auto)
        {
            var employee = RF.GetById<Employee>(employeeId);
            if (employee == null)
                throw new EntityNotFoundException(typeof(Employee), employeeId);
            var query = Query<DispatchTask>()
                .Join<Core.Items.ItemBatchRule>((d, i) => d.ProductId == i.ItemId && i.RetrospectType == retrospectType)
                .Exists<DispatchTaskDetail>((d, p) => p.Where(f => f.DispatchTaskId == d.Id
                && ((f.AdoType == AdoType.Employee && f.AdoId == employeeId)
                || (f.AdoType == AdoType.WorkGroup && f.AdoId == employee.WorkGroupId)
                || (f.AdoType == AdoType.EmployeeGroup && f.AdoId == employee.EmployeeGroupId))))
                .Where(p => p.ReportMode == reportMode && (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing));
            if (processId > 0)
                query.Where(p => (p.ProcessId != null && p.ProcessId == processId) || p.ProcessId == null);
            var tasks = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return tasks;
        }

        /// <summary>
        /// 获取员工/班组的已派工数量
        /// </summary>
        /// <param name="adoIds">对象类型Id列表</param>
        /// <param name="adoType">对象类型</param>
        /// <returns>员工/班组的已派工数量字典</returns>
        private Dictionary<double, int> GetGroupSendQtys(List<double> adoIds, AdoType adoType)
        {
            var groupSendQtys = new Dictionary<double, int>();
            foreach (var adoId in adoIds)
            {
                var tasks = GetDispatchTasksByAdoType(adoId, adoType);
                if (!groupSendQtys.ContainsKey(adoId))
                    groupSendQtys.Add(adoId, tasks.Distinct().Count());
            }

            return groupSendQtys;
        }

        /// <summary>
        /// 根据对象类型和对象Id列表获取已派工和执行中的派工任务列表
        /// </summary>
        /// <param name="adoIds">对象Id列表</param>
        /// <param name="adoType">对象类型</param>
        /// <returns>派工任务列表</returns>
        private EntityList<DispatchTask> GetDispatchTasksByAdoType(List<double> adoIds, AdoType adoType)
        {
            return Query<DispatchTask>().Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id && adoIds.Contains(p.AdoId) && p.AdoType == adoType && (x.TaskStatus == DispatchTaskStatus.Dispatched || x.TaskStatus == DispatchTaskStatus.Executing))).ToList();
        }

        /// <summary>
        /// 根据对象类型和对象Id获取派工任务列表
        /// </summary>
        /// <param name="adoId">对象Id</param>
        /// <param name="adoType">对象类型</param>
        /// <returns>派工任务列表</returns>
        private EntityList<DispatchTask> GetDispatchTasksByAdoType(double adoId, AdoType adoType)
        {
            var dispatchTasks = new EntityList<DispatchTask>();
            var employeeCt = RT.Service.Resolve<EmployeeController>();

            if (adoType == AdoType.WorkGroup)
            {
                var employees = employeeCt.GetEmployeeListByWorkGroup(new List<double?>() { adoId });
                var employeeIds = employees.Select(p => p.Id).Distinct().ToList();
                dispatchTasks.AddRange(GetDispatchTasksByAdoType(new List<double>() { adoId }, adoType));
                dispatchTasks.AddRange(GetDispatchTasksByAdoType(employeeIds, AdoType.Employee));

            }
            else if (adoType == AdoType.EmployeeGroup)
            {
                var employees = employeeCt.GetEmployeeListByEmployeeGroup(new List<double?>() { adoId });
                var employeeIds = employees.Select(p => p.Id).Distinct().ToList();
                dispatchTasks.AddRange(GetDispatchTasksByAdoType(new List<double>() { adoId }, adoType));
                dispatchTasks.AddRange(GetDispatchTasksByAdoType(employeeIds, AdoType.Employee));
            }
            else
                dispatchTasks.AddRange(GetDispatchTasksByAdoType(new List<double>() { adoId }, adoType));

            return dispatchTasks;
        }

        /// <summary>
        /// 创建任务详情列表
        /// </summary>
        /// <param name="adoId">对象ID</param>
        /// <param name="adoType">对象类型</param>
        /// <returns>任务详情列表</returns>
        public virtual EntityList<TaskDetailViewModel> CreateTaskDetailViewModels(double adoId, AdoType adoType)
        {
            var taskDetailVMs = new EntityList<TaskDetailViewModel>();
            var dispatchTasks = GetDispatchTasksByAdoType(adoId, adoType);

            #region 加载数据
            var workOrderIds = dispatchTasks.Where(p => p.WorkOrder != null).Select(p => p.WorkOrderId.Value).Distinct().ToList();
            var productIds = dispatchTasks.Where(p => p.Product != null).Select(p => p.ProductId).Distinct().ToList();
            var processIds = dispatchTasks.Where(p => p.Process != null).Select(p => p.ProcessId.Value).Distinct().ToList();
            var resourceIds = dispatchTasks.Where(p => p.Resource != null).Select(p => p.ResourceId.Value).Distinct().ToList();
            var specificationIds = dispatchTasks.Where(p => p.Specification != null).Select(p => p.SpecificationId.Value).Distinct().ToList();

            var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByWoIds(workOrderIds);
            var dicWorkOrders = workOrders.ToDictionary(p => p.Id);
            var products = RT.Service.Resolve<ItemController>().GetItemList(productIds);
            var dicProducts = products.ToDictionary(p => p.Id);
            var processs = RT.Service.Resolve<ProcessController>().GetProcessByIds(processIds);
            var dicProcesss = processs.ToDictionary(p => p.Id);
            var resources = RT.Service.Resolve<WipResourceController>().GetResourceList(resourceIds);
            var dicResources = resources.ToDictionary(p => p.Id);
            var specifications = RT.Service.Resolve<ProductSpecificationController>().GetSpecifications(specificationIds);
            var dicSpecifications = specifications.ToDictionary(p => p.Id);
            #endregion

            foreach (var task in dispatchTasks)
            {
                TaskDetailViewModel taskDetailVM = GetTaskDetailVM(task);

                SetTaskDetailVM(dicWorkOrders, dicProducts, dicProcesss, dicResources, dicSpecifications, task, taskDetailVM);

                taskDetailVMs.Add(taskDetailVM);
            }

            return taskDetailVMs;
        }

        /// <summary>
        /// 设置任务详细ViewModel
        /// </summary>
        /// <param name="dicWorkOrders">工单字典</param>
        /// <param name="dicProducts">产品字典</param>
        /// <param name="dicProcesss">工序字典</param>
        /// <param name="dicResources">资源字典</param>
        /// <param name="dicSpecifications">规格件字典</param>
        /// <param name="task">任务单</param>
        /// <param name="taskDetailVM">任务详细ViewModel</param>
        private void SetTaskDetailVM(Dictionary<double, WorkOrder> dicWorkOrders, Dictionary<double, Item> dicProducts, Dictionary<double, Process> dicProcesss, Dictionary<double, WipResource> dicResources, Dictionary<double, Specification> dicSpecifications, DispatchTask task, TaskDetailViewModel taskDetailVM)
        {
            if (task.WorkOrderId.HasValue && dicWorkOrders.TryGetValue(task.WorkOrderId.Value, out WorkOrder wo))
                taskDetailVM.WorkOrderNo = wo.No;

            if (dicProducts.TryGetValue(task.ProductId, out Item product))
                taskDetailVM.ProductCode = product.Code;

            if (task.ProcessId.HasValue && dicProcesss.TryGetValue(task.ProcessId.Value, out Process process))
                taskDetailVM.ProcessName = process.Name;

            if (task.ResourceId.HasValue && dicResources.TryGetValue(task.ResourceId.Value, out WipResource resource))
                taskDetailVM.ResourceName = resource.Name;

            if (task.SpecificationId.HasValue && dicSpecifications.TryGetValue(task.SpecificationId.Value, out Specification specification))
            {
                taskDetailVM.SpecificationCode = specification.Code;
                taskDetailVM.SpecificationName = specification.Name;
            }
        }

        /// <summary>
        /// 创建任务详细ViewModel
        /// </summary>
        /// <param name="task">任务单</param>
        /// <returns>任务详细ViewModel</returns>
        private TaskDetailViewModel GetTaskDetailVM(DispatchTask task)
        {
            return new TaskDetailViewModel()
            {
                No = task.No,
                DispatchQty = task.DispatchQty,
                WorkOrderNo = string.Empty,
                ProductCode = string.Empty,
                Priority = task.Priority,
                ProcessName = string.Empty,
                ResourceName = string.Empty,
                ReportQty = task.ReportQty,
                TaskStatus = task.TaskStatus,
                SpecificationCode = string.Empty,
                SpecificationName = string.Empty,
                ReportMode = task.ReportMode,
                BeginTime = task.BeginTime,
                EndTime = task.EndTime,
                PlanBeginTime = task.PlanBeginTime,
                PlanEndTime = task.PlanEndTime
            };
        }

        /// <summary>
        /// 获取工单任务报工方式
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>工单没有生成任务单，返回null,否则返回报工方式</returns>
        public virtual ReportMode? GetTaskReportModel(double workOrderId)
        {
            return Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId).FirstOrDefault()?.ReportMode;
        }

        /// <summary>
        /// 判断是否存在任务单
        /// Expression不支持序列号，前端不允许调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public virtual bool IsExistDispatchTask(Expression<Func<DispatchTask, bool>> exp)
        {
            var query = Query<DispatchTask>();
            if (exp != null)
                query.Where(exp);
            return query.Count() > 0;
        }

        /// <summary>
        /// 共模计划单对应的辅料工单是否存在已生成的任务单
        /// </summary>
        /// <param name="planNo">计划单号</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistDispatchTask(string planNo)
        {
            return Query<DispatchTask>().Exists<WorkOrder>((a, b) => b.Where(f => a.WorkOrderId != null && f.Id == a.WorkOrderId && f.PlanNo == planNo && f.IsCommonMode && !f.IsMainMaterial && f.State != Core.WorkOrders.WorkOrderState.CancelRelease)).Count() > 0;
        }

        /// <summary>
        /// 判断共模主料工单是否按共模比报工
        /// </summary>
        /// <param name="wo">工单</param>
        /// <returns>true/false</returns>
        public virtual RstTaskBillInfo IsCanSyntypeReport(WorkOrder wo)
        {
            var rstInfo = new RstTaskBillInfo();
            rstInfo.ErrMsg = string.Empty;
            rstInfo.IsSyntype = false;
            rstInfo.OrgIsSyntype = false;
            if (wo.IsCommonMode && wo.IsMainMaterial)
            {
                try
                {
                    var reportRule = RT.Service.Resolve<ReportController>().GetReportRuleConfig(wo.Product.ProductFamilyId ?? 0);
                    if (reportRule == null)
                        throw new ValidationException("未找到产品[{0}]报工规则配置，请在产品族中维护".L10nFormat(wo.ProductCode));
                    if (reportRule.IsSyntype)
                    {
                        rstInfo.IsSyntype = true;
                        rstInfo.OrgIsSyntype = true;

                        if (IsExistDispatchTask(wo.PlanNo))
                            rstInfo.IsSyntype = false;
                    }
                }
                catch (Exception ex)
                {
                    rstInfo.ErrMsg = ex.Message;
                }
            }

            return rstInfo;
        }

        /// <summary>
        /// 根据工单Id获取任务单
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> GetDispatchTasksByWorkOrderIds(List<double> workOrderIds)
        {
            var list = workOrderIds.SplitContains(ids =>
            {
                return Query<DispatchTask>().Where(p => ids.Contains((double)p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 通过派工任务单Id列表获取派工任务单列表（贪婪加载工单和任务单明细列表）
        /// </summary>
        /// <param name="dispatchTaskIds">派工任务单Id列表</param>
        /// <returns>派工任务单列表</returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskList(List<double> dispatchTaskIds)
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                return dispatchTaskIds.SplitContains((tempTaskIds) =>
                {
                    return Query<DispatchTask>().Where(p => tempTaskIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty).LoadWith(DispatchTask.DetailsProperty).LoadWithViewProperty());
                });
            }
        }
        /// <summary>
        /// 通过派工任务单号列表获取派工任务单列表（贪婪加载工单和任务单明细列表）
        /// </summary>
        /// <param name="taskNos">派工任务单Id列表</param>
        /// <returns>派工任务单列表</returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskList(List<string> taskNos)
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                return taskNos.SplitContains((tempTaskNos) =>
                {
                    return Query<DispatchTask>().Where(p => tempTaskNos.Contains(p.No)).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty).LoadWith(DispatchTask.DetailsProperty).LoadWithViewProperty());
                });
            }
        }
        /// <summary>
        /// 通过查询条件获取派工任务列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>派工任务列表</returns>
        /* public virtual EntityList<DispatchTask> GetDispatchTaskList(DispatchTaskCriteria criteria)
         {
             var query = Query<DispatchTask>();
             if (criteria.No.IsNotEmpty())
                 query.Where(p => p.No.Contains(criteria.No));

             if (criteria.TaskStatus.IsNotEmpty())
             {
                 var stateList = new List<int>();
                 criteria.TaskStatus.Split(',').ForEach(s =>
                 {
                     stateList.Add(int.Parse(s));
                 });

                 query.Where(p => (stateList.Contains((int)p.TaskStatus)));

             }

             if (criteria.ProductCode.IsNotEmpty() || criteria.ProductName.IsNotEmpty())
             {
                 query.Exists<Item>((a, b) => b.Where(p => p.Id == a.ProductId)
                  .WhereIf(criteria.ProductCode.IsNotEmpty(), p => p.Code.Contains(criteria.ProductCode))
                  .WhereIf(criteria.ProductName.IsNotEmpty(), p => p.Name.Contains(criteria.ProductName)));
             }

             if (criteria.WorkShop.IsNotEmpty())
                 query.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkShopId).WhereIf(criteria.WorkShop.IsNotEmpty(), p => p.Code == criteria.WorkShop));
             //if (criteria.ResourceId.HasValue)
             //    query.Exists<WipResource>((a, b) => b.Where(p => p.Id == a.ResourceId).WhereIf(criteria.ResourceId.HasValue, p => p.Id == criteria.ResourceId));
             if (!criteria.ResourceCode.IsNullOrEmpty())
                 query.Where(p => p.Resource.Code.Contains(criteria.ResourceCode));
             if (criteria.ProcessId.HasValue)
                 query.Exists<Process>((a, b) => b.Where(p => p.Id == a.ProcessId).WhereIf(criteria.ProcessId.HasValue, p => p.Id == criteria.ProcessId));

             if (criteria.AdoName.IsNotEmpty())
             {
                 query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id && p.AdoName.Contains(criteria.AdoName)));
             }

             if (criteria.IsShowDispatchTask)
                 query.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatching || p.TaskStatus == DispatchTaskStatus.ToDispatch);

             if (criteria.ReportMode.HasValue)
                 query.Where(p => p.ReportMode == criteria.ReportMode);

             if (criteria.WorkOrderId.HasValue)
             {
                 var dispatchTaskIds = GetDispatchTaskIds(criteria.WorkOrderId.Value);
                 query.Where(p => dispatchTaskIds.Contains(p.Id));
             }

             if (criteria.PlanBeginTime.BeginValue.HasValue)
                 query.Where(p => p.PlanBeginTime >= criteria.PlanBeginTime.BeginValue);
             if (criteria.PlanBeginTime.EndValue.HasValue)
                 query.Where(p => p.PlanBeginTime <= criteria.PlanBeginTime.EndValue);
             if (criteria.PlanEndTime.BeginValue.HasValue)
                 query.Where(p => p.PlanEndTime >= criteria.PlanEndTime.BeginValue);
             if (criteria.PlanEndTime.EndValue.HasValue)
                 query.Where(p => p.PlanEndTime <= criteria.PlanEndTime.EndValue);

             if (criteria.IsSyntype)
                 query.Where(p => p.IsSyntype == criteria.IsSyntype);
             if (criteria.IsVirtualPart)
                 query.Where(p => p.IsVirtualPart == criteria.IsVirtualPart);
             if (criteria.IsSchedulingInfReturn != null && criteria.IsSchedulingInfReturn == true)
             {
                 query.Where(p => p.IsSchedulingInfReturn == YesNo.Yes);
             }
             if (!criteria.Fevor.IsNullOrEmpty())
                 query.Where(p => p.WorkOrder.Fevor.Contains(criteria.Fevor));
             if (criteria.IsClose == false || criteria.IsClose == null)
                 query.Where(p => p.TaskStatus != DispatchTaskStatus.Closed);
             if (criteria.ImportTime.BeginValue != null)
                 query.Where(p => p.ImportTime >= criteria.ImportTime.BeginValue.Value);
             if (criteria.ImportTime.EndValue != null)
                 query.Where(p => p.ImportTime <= criteria.ImportTime.EndValue.Value);
             query.Where(p => p.IsMainTask);  //主任务单、共模辅料工单非共模比报工的任务单
             var result = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
             return result;
         }
        */

        /// <summary>
        /// 通过查询条件获取派工任务列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>派工任务列表</returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskList(DispatchTaskCriteria criteria)
        {
            #region 
            //List<string> nos = new List<string>() { "XHD202512230015","XHD202512250004","XHD202512250008","XHD202512290008","XHD202512290011" };
            //var list = Query<DispatchTask>().Where(p => p.TaskStatus == DispatchTaskStatus.Executing && p.Process.Code.Contains("%成品包装%")).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //foreach (var l in list)
            //{
            //    try
            //    {
            //        SplitDispatchTask(l.Id, l.DispatchQty - l.ReportQty - l.SuspectQty, l.ResourceId.Value);
            //    }
            //    catch (Exception ex)
            //    { }
            //}

            //            var workOrders = Query<WorkOrder>()
            //                //.Exists<WorkOrderRoutingProcess>((x, y) => y.Where(p => p.WorkOrderId == x.Id && (p.StartProcess == null || p.EndProcess == null)))
            //                //.NotExists<DispatchTask>((x, y) => y.Where(p => p.WorkOrderId == x.Id && (p.StartProcess == true || p.EndProcess == true)))
            //                .Where(p=>p.No == "100000694318")
            //                .Where(p => p.State == WorkOrderState.Release || p.State == WorkOrderState.Producing)
            //                .ToList(new PagingInfo(1, 50000), new EagerLoadOptions().LoadWithViewProperty());
            //            int index = 0;

            //            var versionIds = workOrders.Select(p => p.VersionId.Value).Distinct().ToList();

            //            var routingProcesses1 = AppRuntime.Service.Resolve<RoutingController>()
            //.GetRoutingProcessList(versionIds);

            //            var ProcessIds = routingProcesses1.Select(p => p.ProcessId.Value).Distinct().ToList();
            //            var processPtys1 = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(ProcessIds);

            //            var tasks = workOrders.Select(p => p.Id).Distinct().ToList().SplitContains(ids =>
            //            {
            //                return Query<DispatchTask>().Where(p => ids.Contains((double)p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //            });

            //            var LayoutInfos = workOrders.Select(p => p.Id).Distinct().ToList().SplitContains(ids =>
            //            {
            //                return Query<LayoutInfo>().Where(p => ids.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //            });

            //            foreach (var workOrder in workOrders)
            //            {
            //                index += 1;
            //                System.Diagnostics.Trace.WriteLine(index);

            //                if (workOrder.State != WorkOrderState.Release && workOrder.State != WorkOrderState.Producing)
            //                    continue;
            //                double? startProcess = null;
            //                double? endProcess = null;
            //                var layoutInfos = LayoutInfos.Where(p => p.WorkOrderId == workOrder.Id).ToList();//workOrder.LayoutInfoList;

            //                var routingProcesses = routingProcesses1.Where(p => p.VersionId == workOrder.VersionId.Value).ToList();//AppRuntime.Service.Resolve<RoutingController>().GetRoutingProcessList(new List<double> { workOrder.VersionId.Value });


            //                //获取工序属性维护
            //                var routingProcessesOfCurrentVersion = routingProcesses
            //        .Where(x => x.VersionId == workOrder.VersionId).ToList();

            //                var processIds = routingProcessesOfCurrentVersion.Select(p => p.ProcessId.Value).Distinct().ToList();
            //                var processPtys = processPtys1.Where(p => processIds.Contains(p.ProcessId)).ToList();//RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds);

            //                //判断首工序
            //                foreach (var layoutInfo in layoutInfos.OrderBy(p => Convert.ToDecimal(p.Vornr)))
            //                {
            //                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
            //                    if (processPty != null)
            //                    {
            //                        startProcess = processPty.ProcessId;
            //                        break;
            //                    }
            //                }
            //                //判断尾工序
            //                foreach (var layoutInfo in layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)))
            //                {
            //                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
            //                    if (processPty != null)
            //                    {
            //                        endProcess = processPty.ProcessId;
            //                        break;
            //                    }
            //                }
            //                foreach (var item in workOrder.RoutingProcessList)
            //                {
            //                    item.StartProcess = startProcess;
            //                    item.EndProcess = endProcess;
            //                }
            //                RF.Save(workOrder.RoutingProcessList);

            //                var taskList = tasks.Where(p => p.WorkOrderId == workOrder.Id && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.Finished).ToList();
            //                if (taskList.Any(p => p.StartProcess == true))
            //                    continue;
            //                ////按工序生成任务单            
            //                //var processList = workOrder.RoutingProcessList.Where(p => p.IsGenerateTask && p.Process.Type != ProcessType.Fix && p.Process.Type != ProcessType.BatchFix).OrderBy(p => p.Index).AsEntityList();
            //                ////设置首末工序标识
            //                //startProcess = processList.FirstOrDefault().StartProcess;
            //                //endProcess = processList.FirstOrDefault().EndProcess;
            //                if (startProcess != null)
            //                {
            //                    var startProcessTasks = taskList.Where(p => p.ProcessId == startProcess);
            //                    startProcessTasks.ForEach(p =>
            //                    {
            //                        p.StartProcess = true;
            //                        RF.Save(p);
            //                    });
            //                }
            //                if (endProcess != null)
            //                {
            //                    var endProcessTasks = taskList.Where(p => p.ProcessId == endProcess);
            //                    endProcessTasks.ForEach(p =>
            //                    {
            //                        p.EndProcess = true;
            //                        RF.Save(p);
            //                    });
            //                }
            //            }



            //List<string> woNos = new List<string>() { };
            //var workOrders = Query<WorkOrder>().Where(p => woNos.Contains(p.No) && p.State != WorkOrderState.Close).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //foreach (var workOrder in workOrders)
            //{
            //    if (workOrder.State != WorkOrderState.Close)
            //    {
            //        //获取这张工单下的所有任务单，然后关闭(原工单关闭逻辑是，必须先关闭对应的任务单，才能关闭工单)
            //        var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(workOrder.Id).Where(p => p.TaskStatus != DispatchTaskStatus.Closed).ToList();

            //        //先暂停
            //        var selectedIds = tasks.Where(p => p.TaskStatus != DispatchTaskStatus.Pause).Select(p => p.Id).Distinct().ToList();
            //        RT.Service.Resolve<DispatchController>().SetPauseTasks(selectedIds);
            //        selectedIds = tasks.Select(p => p.Id).Distinct().ToList();
            //        //关闭任务单
            //        RT.Service.Resolve<DispatchController>().SetCloseTasks(selectedIds);
            //        //先暂停工单
            //        if (workOrder.IsPause == YesNo.No && workOrder.State != WorkOrderState.Finish)
            //            RT.Service.Resolve<WorkOrderController>().Pause(workOrder.Id, "SAP关闭工单");
            //        //关闭工单
            //        RT.Service.Resolve<WorkOrderController>().Close(workOrder.Id, "SAP关闭工单");
            //    }
            //}

            #endregion

            var query = Query<DispatchTask>();
            if (criteria.No.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.No));

            if (criteria.TaskStatus.IsNotEmpty())
            {
                var stateList = new List<int>();
                criteria.TaskStatus.Split(',').ForEach(s =>
                {
                    stateList.Add(int.Parse(s));
                });

                query.Where(p => (stateList.Contains((int)p.TaskStatus)));

            }

            if (criteria.ProductCode.IsNotEmpty() || criteria.ProductName.IsNotEmpty() || criteria.OldItem.IsNotEmpty())
            {
                query.Exists<Item>((a, b) => b.Where(p => p.Id == a.ProductId)
                 .WhereIf(criteria.ProductCode.IsNotEmpty(), p => p.Code.Contains(criteria.ProductCode))
                 .WhereIf(criteria.ProductName.IsNotEmpty(), p => p.Name.Contains(criteria.ProductName))
                 .WhereIf(criteria.OldItem.IsNotEmpty(), p => p.ShortDescription.Contains(criteria.OldItem)));
            }

            if (criteria.WorkShop.IsNotEmpty())
                query.Exists<Enterprise>((a, b) => b.Where(p => p.Id == a.WorkShopId).WhereIf(criteria.WorkShop.IsNotEmpty(), p => p.Code == criteria.WorkShop));
            //if (criteria.ResourceId.HasValue)
            //    query.Exists<WipResource>((a, b) => b.Where(p => p.Id == a.ResourceId).WhereIf(criteria.ResourceId.HasValue, p => p.Id == criteria.ResourceId));
            if (!criteria.ResourceCode.IsNullOrEmpty())
                query.Where(p => p.Resource.Code.Contains(criteria.ResourceCode));
            if (!criteria.ResourceName.IsNullOrEmpty())
                query.Where(p => p.Resource.Name.Contains(criteria.ResourceName));
            //if (!criteria.ResourceCode.IsNullOrEmpty() || !criteria.ResourceName.IsNullOrEmpty())
            //    query.Where(p => p.Resource.Code.Contains(criteria.ResourceCode) || p.Resource.Name.Contains(criteria.ResourceName));
            if (criteria.ProcessId.HasValue)
                query.Exists<Process>((a, b) => b.Where(p => p.Id == a.ProcessId).WhereIf(criteria.ProcessId.HasValue, p => p.Id == criteria.ProcessId));

            if (criteria.AdoName.IsNotEmpty())
            {
                query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id && p.AdoName.Contains(criteria.AdoName)));
            }

            if (criteria.IsShowDispatchTask)
                query.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatching || p.TaskStatus == DispatchTaskStatus.ToDispatch);

            if (criteria.ReportMode.HasValue)
                query.Where(p => p.ReportMode == criteria.ReportMode);

            if (criteria.WorkOrderId.HasValue)
            {
                var dispatchTaskIds = GetDispatchTaskIds(criteria.WorkOrderId.Value);
                query.Where(p => dispatchTaskIds.Contains(p.Id));
            }

            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));

            if (criteria.PlanBeginTime.BeginValue.HasValue)
                query.Where(p => p.PlanBeginTime >= criteria.PlanBeginTime.BeginValue);
            if (criteria.PlanBeginTime.EndValue.HasValue)
                query.Where(p => p.PlanBeginTime <= criteria.PlanBeginTime.EndValue);
            if (criteria.PlanEndTime.BeginValue.HasValue)
                query.Where(p => p.PlanEndTime >= criteria.PlanEndTime.BeginValue);
            if (criteria.PlanEndTime.EndValue.HasValue)
                query.Where(p => p.PlanEndTime <= criteria.PlanEndTime.EndValue);

            if (criteria.IsSyntype)
                query.Where(p => p.IsSyntype == criteria.IsSyntype);
            if (criteria.IsVirtualPart)
                query.Where(p => p.IsVirtualPart == criteria.IsVirtualPart);
            if (criteria.IsSchedulingInfReturn != null && criteria.IsSchedulingInfReturn == true)
            {
                query.Where(p => p.IsSchedulingInfReturn == YesNo.Yes);
            }
            if (!criteria.Fevor.IsNullOrEmpty())
                query.Where(p => p.WorkOrder.Fevor.Contains(criteria.Fevor));
            if (criteria.IsClose == false || criteria.IsClose == null)
                query.Where(p => p.TaskStatus != DispatchTaskStatus.Closed);
            if (criteria.ImportTime.BeginValue != null)
                query.Where(p => p.ImportTime >= criteria.ImportTime.BeginValue.Value);
            if (criteria.ImportTime.EndValue != null)
                query.Where(p => p.ImportTime <= criteria.ImportTime.EndValue.Value);
            query.Where(p => p.IsMainTask);  //主任务单、共模辅料工单非共模比报工的任务单

            if (criteria.OrderInfoList == null || criteria.OrderInfoList.Count == 0)
            {
                criteria.OrderInfoList.Add(new OrderInfo() { Property = "AssociatedWorkOrder", SortIndex = 1, SortOrder = System.ComponentModel.ListSortDirection.Ascending });
                criteria.OrderInfoList.Add(new OrderInfo() { Property = "Seq", SortIndex = 2, SortOrder = System.ComponentModel.ListSortDirection.Ascending });
            }

            var result = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //获取父级旧料号
            var productIds = result.Select(p => p.ProductId).Distinct().ToList();
            var parentItems = productIds.SplitContains(ids =>
            {
                return Query<ParentItem>().Where(p => ids.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            foreach (var r in result)
            {
                var parentItem = parentItems.FirstOrDefault(p => p.ItemId == r.ProductId);
                if (parentItem != null)
                    r.ParShortDescription = parentItem.Bismt;
            }

            return result;
        }

        /// <summary>
        /// 通过派工任务单Id列表获取派工任务单列表
        /// </summary>
        /// <param name="dispatchTaskIds">派工任务单Id列表</param>
        /// <returns>派工任务单列表</returns>
        public virtual EntityList<DispatchTask> GetDispatchTasks(List<double> dispatchTaskIds)
        {
            return Query<DispatchTask>().Where(p => dispatchTaskIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取派工单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>派工单</returns>
        public virtual EntityList<DispatchTask> GetDispatchTasks(double workOrderId, PagingInfo pagingInfo = null)
        {
            return Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取近3日车间产线任务计划信息（计划开始日期为当前日期及后两日的所有任务）
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>近3日车间产线任务计划信息</returns>
        public virtual EntityList<DispatchTask> GetDispatchTasksByDate(double workShopId, double? resourceId)
        {
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            DateTime startDate = new DateTime(now.Year, now.Month, now.Day);
            var endDate = startDate.AddDays(3).AddSeconds(-1);

            if (resourceId.HasValue)
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && p.ResourceId == resourceId && p.PlanBeginTime <= endDate && p.PlanEndTime >= startDate).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
            else
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && p.PlanBeginTime <= endDate && p.PlanEndTime >= startDate).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
        }

        /// <summary>
        /// 获取近3日车间产线任务计划信息（计划开始日期为当前日期及后两日的所有任务）
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>近3日车间产线任务计划信息</returns>
        public virtual List<PlanTaskInfo> GetDispatchTasksOf3Day(double workShopId, double? resourceId)
        {
            List<PlanTaskInfo> planTaskInfoList = new List<PlanTaskInfo>();
            var now = RF.Find<DispatchTask>().GetDbTime();
            var startDate = new DateTime(now.Year, now.Month, now.Day);
            var endDate = startDate.AddDays(3).AddSeconds(-1);
            IList<PlanTaskInfo> normalTasks = null;
            IList<PlanTaskInfo> mergeRowTasks = null;

            //未合并的任务单数据
            if (resourceId.HasValue)
                normalTasks = GetNormalTasksByResource(workShopId, resourceId.Value, startDate, endDate);
            else
                normalTasks = GetNormalTasks(workShopId, startDate, endDate);

            if (normalTasks.Count > 0)
                planTaskInfoList.AddRange(normalTasks);

            //已合并的任务单的数据 
            if (resourceId.HasValue)
                mergeRowTasks = GetMergeRowTasksByResource(workShopId, resourceId.Value, startDate, endDate);
            else
                mergeRowTasks = GetMergeRowTasks(workShopId, startDate, endDate);

            if (mergeRowTasks.Any())
                planTaskInfoList.AddRange(mergeRowTasks);

            return planTaskInfoList;
        }

        /// <summary>
        /// 获取已合并车间任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="now">开始时间</param>
        /// <param name="sDate">开始时间</param>
        /// <param name="eDate">结束时间</param>
        /// <returns>已合并车间任务计划信息</returns>
        private IList<PlanTaskInfo> GetMergeRowTasks(double workShopId, DateTime now, DateTime sDate, DateTime eDate)
        {
            return Query<DispatchTask>().As(_dispatchTaskString)
                               .Where(dispatchTask => dispatchTask.WorkShopId == workShopId)
                              .Join<AssociatedTask>((dispatchTask, associatedTask) => dispatchTask.Id == associatedTask.DispatchTaskId && dispatchTask.MergedStatus == MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                              .Join<AssociatedTask, DispatchTask>("task", (associatedTask, task) => associatedTask.TaskId == task.Id)
                              .Join<DispatchTask, WorkOrder>((task, wo) => task.WorkOrderId == wo.Id && task.WorkOrderId != null && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && (task.PlanEndTime < now || (task.PlanBeginTime <= eDate && task.PlanEndTime >= sDate)))
                              .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
                              .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                              .Select<AssociatedTask, DispatchTask, WorkOrder, Item, DispatchTaskDetail>(
                              (dispatchTask, associatedTask, task, wo, product, taskDetail) => new
                              {
                                  DispatchTaskId = dispatchTask.Id,
                                  DispatchTaskNo = dispatchTask.No,
                                  PlanBeginTime = dispatchTask.PlanBeginTime,
                                  PlanEndTime = dispatchTask.PlanEndTime,
                                  DispatchQty = dispatchTask.DispatchQty,
                                  ReportQty = dispatchTask.ReportQty,
                                  TaskStatus = dispatchTask.TaskStatus,
                                  ProductName = product.Name,
                                  WorkOrderNo = wo.No,
                                  WorkOrderType = wo.Type,
                                  AdoId = taskDetail.AdoId,
                                  AdoType = taskDetail.AdoType,
                                  AdoGroup = taskDetail.AdoGroup
                              }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取已合并车间任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>已合并车间任务计划信息</returns>
        private IList<PlanTaskInfo> GetMergeRowTasks(double workShopId, DateTime startDate, DateTime endDate)
        {
            return Query<DispatchTask>().As(_dispatchTaskString)
                               .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.PlanBeginTime <= endDate && dispatchTask.PlanEndTime >= startDate)
                              .Join<AssociatedTask>((dispatchTask, associatedTask) => dispatchTask.Id == associatedTask.DispatchTaskId && dispatchTask.MergedStatus == MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                              .Join<AssociatedTask, DispatchTask>("task", (associatedTask, task) => associatedTask.TaskId == task.Id)
                              .Join<DispatchTask, WorkOrder>((task, wo) => task.WorkOrderId == wo.Id && task.WorkOrderId != null && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish)
                              .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                              .Select<AssociatedTask, DispatchTask, WorkOrder, DispatchTaskDetail>(
                              (dispatchTask, associatedTask, task, wo, taskDetail) => new
                              {
                                  DispatchTaskId = dispatchTask.Id,
                                  DispatchTaskNo = dispatchTask.No,
                                  PlanBeginTime = dispatchTask.PlanBeginTime,
                                  PlanEndTime = dispatchTask.PlanEndTime,
                                  DispatchQty = dispatchTask.DispatchQty,
                                  ReportQty = dispatchTask.ReportQty,
                                  TaskStatus = dispatchTask.TaskStatus,
                                  WorkOrderNo = wo.No,
                                  WorkOrderType = wo.Type,
                                  AdoId = taskDetail.AdoId,
                                  AdoType = taskDetail.AdoType,
                                  AdoGroup = taskDetail.AdoGroup
                              }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取超期已合并车间任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="now">结束时间</param>
        /// <returns>超期已合并车间任务计划信息</returns>
        private IList<PlanTaskInfo> GetMergeRowTasks(double workShopId, DateTime now)
        {
            return Query<DispatchTask>().As(_dispatchTaskString)
                               .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.PlanEndTime <= now)
                              .Join<AssociatedTask>((dispatchTask, associatedTask) => dispatchTask.Id == associatedTask.DispatchTaskId && dispatchTask.MergedStatus == MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                              .Join<AssociatedTask, DispatchTask>("task", (associatedTask, task) => associatedTask.TaskId == task.Id)
                              .Join<DispatchTask, WorkOrder>((task, wo) => task.WorkOrderId == wo.Id && task.WorkOrderId != null && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish)
                              .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
                              .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                              .Select<AssociatedTask, DispatchTask, WorkOrder, Item, DispatchTaskDetail>(
                              (dispatchTask, associatedTask, task, wo, product, taskDetail) => new
                              {
                                  DispatchTaskId = dispatchTask.Id,
                                  DispatchTaskNo = dispatchTask.No,
                                  PlanBeginTime = dispatchTask.PlanBeginTime,
                                  PlanEndTime = dispatchTask.PlanEndTime,
                                  DispatchQty = dispatchTask.DispatchQty,
                                  ReportQty = dispatchTask.ReportQty,
                                  TaskStatus = dispatchTask.TaskStatus,
                                  ProductName = product.Name,
                                  WorkOrderNo = wo.No,
                                  WorkOrderType = wo.Type,
                                  AdoId = taskDetail.AdoId,
                                  AdoType = taskDetail.AdoType,
                                  AdoGroup = taskDetail.AdoGroup
                              }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取已合并车间产线任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="now">开始时间</param>
        /// <param name="sDate">开始时间</param>
        /// <param name="eDate">结束时间</param>
        /// <returns>已合并车间产线任务计划信息</returns>
        private IList<PlanTaskInfo> GetMergeRowTasksByResource(double workShopId, double resourceId, DateTime now, DateTime sDate, DateTime eDate)
        {
            return Query<DispatchTask>().As(_dispatchTaskString)
            .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.ResourceId == resourceId)
           .Join<AssociatedTask>((dispatchTask, associatedTask) => dispatchTask.Id == associatedTask.DispatchTaskId && dispatchTask.MergedStatus == MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
           .Join<AssociatedTask, DispatchTask>("task", (associatedTask, task) => associatedTask.TaskId == task.Id)
           .Join<DispatchTask, WorkOrder>((task, wo) => task.WorkOrderId == wo.Id && task.WorkOrderId != null && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && (task.PlanEndTime < now || (task.PlanBeginTime <= eDate && task.PlanEndTime >= sDate)))
           .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
           .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
           .Select<AssociatedTask, DispatchTask, WorkOrder, Item, DispatchTaskDetail>(
           (dispatchTask, associatedTask, task, wo, product, taskDetail) => new
           {
               DispatchTaskId = dispatchTask.Id,
               DispatchTaskNo = dispatchTask.No,
               PlanBeginTime = dispatchTask.PlanBeginTime,
               PlanEndTime = dispatchTask.PlanEndTime,
               DispatchQty = dispatchTask.DispatchQty,
               ReportQty = dispatchTask.ReportQty,
               TaskStatus = dispatchTask.TaskStatus,
               ProductName = product.Name,
               WorkOrderNo = wo.No,
               WorkOrderType = wo.Type,
               AdoId = taskDetail.AdoId,
               AdoType = taskDetail.AdoType,
               AdoGroup = taskDetail.AdoGroup
           }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取已合并车间产线任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>已合并车间产线任务计划信息</returns>
        private IList<PlanTaskInfo> GetMergeRowTasksByResource(double workShopId, double resourceId, DateTime startDate, DateTime endDate)
        {
            return Query<DispatchTask>().As(_dispatchTaskString)
            .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.ResourceId == resourceId && dispatchTask.PlanBeginTime <= endDate && dispatchTask.PlanEndTime >= startDate)
           .Join<AssociatedTask>((dispatchTask, associatedTask) => dispatchTask.Id == associatedTask.DispatchTaskId && dispatchTask.MergedStatus == MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
           .Join<AssociatedTask, DispatchTask>("task", (associatedTask, task) => associatedTask.TaskId == task.Id)
           .Join<DispatchTask, WorkOrder>((task, wo) => task.WorkOrderId == wo.Id && task.WorkOrderId != null && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish)
           .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
           .Select<AssociatedTask, DispatchTask, WorkOrder, DispatchTaskDetail>(
           (dispatchTask, associatedTask, task, wo, taskDetail) => new
           {
               DispatchTaskId = dispatchTask.Id,
               DispatchTaskNo = dispatchTask.No,
               PlanBeginTime = dispatchTask.PlanBeginTime,
               PlanEndTime = dispatchTask.PlanEndTime,
               DispatchQty = dispatchTask.DispatchQty,
               ReportQty = dispatchTask.ReportQty,
               TaskStatus = dispatchTask.TaskStatus,
               WorkOrderNo = wo.No,
               WorkOrderType = wo.Type,
               AdoId = taskDetail.AdoId,
               AdoType = taskDetail.AdoType,
               AdoGroup = taskDetail.AdoGroup
           }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取超期已合并车间产线任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="now">当前时间</param>
        /// <returns>超期已合并车间产线任务计划信息</returns>AssemblyGetWipResources
        private IList<PlanTaskInfo> GetMergeRowTasksByResource(double workShopId, double resourceId, DateTime now)
        {
            return Query<DispatchTask>().As(_dispatchTaskString)
            .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.ResourceId == resourceId && dispatchTask.PlanEndTime <= now)
           .Join<AssociatedTask>((dispatchTask, associatedTask) => dispatchTask.Id == associatedTask.DispatchTaskId && dispatchTask.MergedStatus == MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
           .Join<AssociatedTask, DispatchTask>("task", (associatedTask, task) => associatedTask.TaskId == task.Id)
           .Join<DispatchTask, WorkOrder>((task, wo) => task.WorkOrderId == wo.Id && task.WorkOrderId != null && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish)
           .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
           .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
           .Select<AssociatedTask, DispatchTask, WorkOrder, Item, DispatchTaskDetail>(
           (dispatchTask, associatedTask, task, wo, product, taskDetail) => new
           {
               DispatchTaskId = dispatchTask.Id,
               DispatchTaskNo = dispatchTask.No,
               PlanBeginTime = dispatchTask.PlanBeginTime,
               PlanEndTime = dispatchTask.PlanEndTime,
               DispatchQty = dispatchTask.DispatchQty,
               ReportQty = dispatchTask.ReportQty,
               TaskStatus = dispatchTask.TaskStatus,
               ProductName = product.Name,
               WorkOrderNo = wo.No,
               WorkOrderType = wo.Type,
               AdoId = taskDetail.AdoId,
               AdoType = taskDetail.AdoType,
               AdoGroup = taskDetail.AdoGroup
           }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取未合并车间任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="now">开始时间</param>
        /// <param name="sDate">开始时间</param>
        /// <param name="eDate">结束时间</param>
        /// <returns>未合并车间任务计划信息</returns>
        private IList<PlanTaskInfo> GetNormalTasks(double workShopId, DateTime now, DateTime sDate, DateTime eDate)
        {
            return Query<DispatchTask>()
                               .Where(dispatchTask => dispatchTask.WorkShopId == workShopId)
                                .Join<WorkOrder>((dispatchTask, wo) => dispatchTask.WorkOrderId != null && dispatchTask.WorkOrderId == wo.Id && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && (dispatchTask.PlanEndTime < now || (dispatchTask.PlanBeginTime <= eDate && dispatchTask.PlanEndTime >= sDate)) && dispatchTask.MergedStatus != MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                                .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
                                .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                                 .Select<WorkOrder, Item, DispatchTaskDetail>(
                              (dispatchTask, wo, product, taskDetail) => new
                              {
                                  DispatchTaskId = dispatchTask.Id,
                                  DispatchTaskNo = dispatchTask.No,
                                  PlanBeginTime = dispatchTask.PlanBeginTime,
                                  PlanEndTime = dispatchTask.PlanEndTime,
                                  DispatchQty = dispatchTask.DispatchQty,
                                  ReportQty = dispatchTask.ReportQty,
                                  TaskStatus = dispatchTask.TaskStatus,
                                  ProductName = product.Name,
                                  WorkOrderNo = wo.No,
                                  WorkOrderType = wo.Type,
                                  AdoId = taskDetail.AdoId,
                                  AdoType = taskDetail.AdoType,
                                  AdoGroup = taskDetail.AdoGroup
                              }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取未合并车间任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>未合并车间任务计划信息</returns>
        private IList<PlanTaskInfo> GetNormalTasks(double workShopId, DateTime startDate, DateTime endDate)
        {
            return Query<DispatchTask>()
                               .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.PlanBeginTime <= endDate && dispatchTask.PlanEndTime >= startDate)
                                .Join<WorkOrder>((dispatchTask, wo) => dispatchTask.WorkOrderId != null && dispatchTask.WorkOrderId == wo.Id && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && dispatchTask.MergedStatus != MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                                .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                                 .Select<WorkOrder, DispatchTaskDetail>(
                              (dispatchTask, wo, taskDetail) => new
                              {
                                  DispatchTaskId = dispatchTask.Id,
                                  DispatchTaskNo = dispatchTask.No,
                                  PlanBeginTime = dispatchTask.PlanBeginTime,
                                  PlanEndTime = dispatchTask.PlanEndTime,
                                  DispatchQty = dispatchTask.DispatchQty,
                                  ReportQty = dispatchTask.ReportQty,
                                  TaskStatus = dispatchTask.TaskStatus,
                                  WorkOrderNo = wo.No,
                                  WorkOrderType = wo.Type,
                                  AdoId = taskDetail.AdoId,
                                  AdoType = taskDetail.AdoType,
                                  AdoGroup = taskDetail.AdoGroup
                              }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取超期未合并车间任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="now">当前时间</param>
        /// <returns>超期未合并车间任务计划信息</returns>
        private IList<PlanTaskInfo> GetNormalTasks(double workShopId, DateTime now)
        {
            return Query<DispatchTask>()
                               .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.PlanEndTime <= now)
                                .Join<WorkOrder>((dispatchTask, wo) => dispatchTask.WorkOrderId != null && dispatchTask.WorkOrderId == wo.Id && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && dispatchTask.MergedStatus != MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                                .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
                                .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                                 .Select<WorkOrder, Item, DispatchTaskDetail>(
                              (dispatchTask, wo, product, taskDetail) => new
                              {
                                  DispatchTaskId = dispatchTask.Id,
                                  DispatchTaskNo = dispatchTask.No,
                                  PlanBeginTime = dispatchTask.PlanBeginTime,
                                  PlanEndTime = dispatchTask.PlanEndTime,
                                  DispatchQty = dispatchTask.DispatchQty,
                                  ReportQty = dispatchTask.ReportQty,
                                  TaskStatus = dispatchTask.TaskStatus,
                                  ProductName = product.Name,
                                  WorkOrderNo = wo.No,
                                  WorkOrderType = wo.Type,
                                  AdoId = taskDetail.AdoId,
                                  AdoType = taskDetail.AdoType,
                                  AdoGroup = taskDetail.AdoGroup
                              }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取未合并车间产线任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="now">开始时间</param>
        /// <param name="sDate">开始时间</param>
        /// <param name="eDate">结束时间</param>
        /// <returns>未合并车间产线任务计划信息</returns>
        private IList<PlanTaskInfo> GetNormalTasksByResource(double workShopId, double resourceId, DateTime now, DateTime sDate, DateTime eDate)
        {
            return Query<DispatchTask>()
                .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.ResourceId == resourceId)
                 .Join<WorkOrder>((dispatchTask, wo) => dispatchTask.WorkOrderId != null && dispatchTask.WorkOrderId == wo.Id && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && (dispatchTask.PlanEndTime < now || (dispatchTask.PlanBeginTime <= eDate && dispatchTask.PlanEndTime >= sDate)) && dispatchTask.MergedStatus != MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                 .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
                 .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                  .Select<WorkOrder, Item, DispatchTaskDetail>(
               (dispatchTask, wo, product, taskDetail) => new
               {
                   DispatchTaskId = dispatchTask.Id,
                   DispatchTaskNo = dispatchTask.No,
                   PlanBeginTime = dispatchTask.PlanBeginTime,
                   PlanEndTime = dispatchTask.PlanEndTime,
                   DispatchQty = dispatchTask.DispatchQty,
                   ReportQty = dispatchTask.ReportQty,
                   TaskStatus = dispatchTask.TaskStatus,
                   ProductName = product.Name,
                   WorkOrderNo = wo.No,
                   WorkOrderType = wo.Type,
                   AdoId = taskDetail.AdoId,
                   AdoType = taskDetail.AdoType,
                   AdoGroup = taskDetail.AdoGroup
               }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取未合并车间产线任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>未合并车间产线任务计划信息</returns>
        private IList<PlanTaskInfo> GetNormalTasksByResource(double workShopId, double resourceId, DateTime startDate, DateTime endDate)
        {
            return Query<DispatchTask>()
                .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.ResourceId == resourceId && dispatchTask.PlanBeginTime <= endDate && dispatchTask.PlanEndTime >= startDate)
                 .Join<WorkOrder>((dispatchTask, wo) => dispatchTask.WorkOrderId != null && dispatchTask.WorkOrderId == wo.Id && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && dispatchTask.MergedStatus != MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                 .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                  .Select<WorkOrder, DispatchTaskDetail>(
               (dispatchTask, wo, taskDetail) => new
               {
                   DispatchTaskId = dispatchTask.Id,
                   DispatchTaskNo = dispatchTask.No,
                   PlanBeginTime = dispatchTask.PlanBeginTime,
                   PlanEndTime = dispatchTask.PlanEndTime,
                   DispatchQty = dispatchTask.DispatchQty,
                   ReportQty = dispatchTask.ReportQty,
                   TaskStatus = dispatchTask.TaskStatus,
                   WorkOrderNo = wo.No,
                   WorkOrderType = wo.Type,
                   AdoId = taskDetail.AdoId,
                   AdoType = taskDetail.AdoType,
                   AdoGroup = taskDetail.AdoGroup
               }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取超期未合并车间产线任务计划信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="now">当前时间</param>
        /// <returns>超期未合并车间产线任务计划信息</returns>
        private IList<PlanTaskInfo> GetNormalTasksByResource(double workShopId, double resourceId, DateTime now)
        {
            return Query<DispatchTask>()
                            .Where(dispatchTask => dispatchTask.WorkShopId == workShopId && dispatchTask.ResourceId == resourceId && dispatchTask.PlanEndTime <= now)
                             .Join<WorkOrder>((dispatchTask, wo) => dispatchTask.WorkOrderId != null && dispatchTask.WorkOrderId == wo.Id && wo.State != Core.WorkOrders.WorkOrderState.Close && wo.State != Core.WorkOrders.WorkOrderState.Finish && dispatchTask.MergedStatus != MergedStatus.MergeRows && dispatchTask.IsMainTask && dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch)
                             .Join<Item>((dispatchTask, product) => dispatchTask.ProductId == product.Id)
                             .Join<DispatchTaskDetail>((dispatchTask, taskDetail) => dispatchTask.Id == taskDetail.DispatchTaskId)
                              .Select<WorkOrder, Item, DispatchTaskDetail>(
                           (dispatchTask, wo, product, taskDetail) => new
                           {
                               DispatchTaskId = dispatchTask.Id,
                               DispatchTaskNo = dispatchTask.No,
                               PlanBeginTime = dispatchTask.PlanBeginTime,
                               PlanEndTime = dispatchTask.PlanEndTime,
                               DispatchQty = dispatchTask.DispatchQty,
                               ReportQty = dispatchTask.ReportQty,
                               TaskStatus = dispatchTask.TaskStatus,
                               ProductName = product.Name,
                               WorkOrderNo = wo.No,
                               WorkOrderType = wo.Type,
                               AdoId = taskDetail.AdoId,
                               AdoType = taskDetail.AdoType,
                               AdoGroup = taskDetail.AdoGroup
                           }).ToList<PlanTaskInfo>();
        }

        /// <summary>
        /// 获取当月任务计划列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="start">开始日期</param>
        /// <param name="end">结束日期</param>
        /// <returns>当月任务计划列表</returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskOfMonth(double workShopId, double? resourceId, DateTime start, DateTime end)
        {
            if (resourceId.HasValue)
            {
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && p.ResourceId == resourceId && p.PlanBeginTime < end && p.PlanEndTime >= start).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
            }
            else
            {
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && p.PlanBeginTime < end && p.PlanEndTime >= start).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
            }
        }

        /// <summary>
        /// 获取当月任务计划列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>当月任务计划列表</returns>
        public virtual List<PlanTaskInfo> GetMonthlyTaskInfos(double workShopId, double? resourceId, DateTime startDate, DateTime endDate)
        {
            List<PlanTaskInfo> planTaskInfoList = new List<PlanTaskInfo>();
            IList<PlanTaskInfo> normalTasks = null;
            IList<PlanTaskInfo> mergeRowTasks = null;

            //未合并的任务单数据
            if (resourceId.HasValue)
                normalTasks = GetNormalTasksByResource(workShopId, resourceId.Value, startDate, endDate);
            else
                normalTasks = GetNormalTasks(workShopId, startDate, endDate);

            if (normalTasks.Count > 0)
                planTaskInfoList.AddRange(normalTasks);

            //已合并的任务单的数据 
            if (resourceId.HasValue)
                mergeRowTasks = GetMergeRowTasksByResource(workShopId, resourceId.Value, startDate, endDate);
            else
                mergeRowTasks = GetMergeRowTasks(workShopId, startDate, endDate);

            if (mergeRowTasks.Any())
                planTaskInfoList.AddRange(mergeRowTasks);

            return planTaskInfoList;
        }

        /// <summary>
        /// 获取异常任务列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>异常任务列表</returns>
        public virtual EntityList<DispatchTask> GetAbnormalTasks(double workShopId, double? resourceId)
        {
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            if (resourceId.HasValue)
            {
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && p.ResourceId == resourceId && p.PlanEndTime <= now).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
            }
            else
            {
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && p.PlanEndTime <= now).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
            }
        }

        /// <summary>
        /// 获取异常任务列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>异常任务列表</returns>
        public virtual List<PlanTaskInfo> GetAbnormalTasks1(double workShopId, double? resourceId)
        {
            List<PlanTaskInfo> planTaskInfoList = new List<PlanTaskInfo>();
            IList<PlanTaskInfo> normalTasks = null;
            IList<PlanTaskInfo> mergeRowTasks = null;
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            //未合并的任务单数据
            if (resourceId.HasValue)
                normalTasks = GetNormalTasksByResource(workShopId, resourceId.Value, now);
            else
                normalTasks = GetNormalTasks(workShopId, now);

            if (normalTasks.Count > 0)
                planTaskInfoList.AddRange(normalTasks);

            //已合并的任务单的数据 
            if (resourceId.HasValue)
                mergeRowTasks = GetMergeRowTasksByResource(workShopId, resourceId.Value, now);
            else
                mergeRowTasks = GetMergeRowTasks(workShopId, now);

            if (mergeRowTasks.Any())
                planTaskInfoList.AddRange(mergeRowTasks);

            return planTaskInfoList;
        }

        /// <summary>
        /// 获取日生产任务列表（包括当天和计划结束时间小于当天，但是没有做完）
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>日生产任务列表</returns>
        public virtual EntityList<DispatchTask> GetDayProduceTasks(double workShopId, double? resourceId)
        {
            //当天(0时0分0秒和23时59分59秒)
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            DateTime sToday = new DateTime(now.Year, now.Month, now.Day);
            DateTime eToday = sToday.AddDays(1).AddSeconds(-1);
            if (resourceId.HasValue)
            {
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && p.ResourceId == resourceId && ((p.PlanEndTime < DateTime.Now && p.TaskStatus != DispatchTaskStatus.Finished) || (p.PlanBeginTime <= eToday && p.PlanEndTime >= sToday))).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
            }
            else
            {
                return Query<DispatchTask>().Where(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.IsMainTask && p.WorkShopId == workShopId && ((p.PlanEndTime < DateTime.Now && p.TaskStatus != DispatchTaskStatus.Finished) || (p.PlanBeginTime <= eToday && p.PlanEndTime >= sToday))).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.WorkOrderProperty));
            }
        }

        /// <summary>
        /// 获取日生产任务列表（包括当天和计划结束时间小于当天，但是没有做完）
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>日生产任务列表</returns>
        public virtual List<PlanTaskInfo> GetDayProduceTaskInfos(double workShopId, double? resourceId)
        {
            List<PlanTaskInfo> planTaskInfoList = new List<PlanTaskInfo>();
            IList<PlanTaskInfo> normalTasks = null;
            IList<PlanTaskInfo> mergeRowTasks = null;

            //当天(0时0分0秒和23时59分59秒)
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            DateTime sDate = new DateTime(now.Year, now.Month, now.Day);
            DateTime eDate = sDate.AddDays(1).AddSeconds(-1);

            //未合并的任务单数据
            normalTasks = resourceId.HasValue ? GetNormalTasksByResource(workShopId, resourceId.Value, now, sDate, eDate) : GetNormalTasks(workShopId, now, sDate, eDate);

            if (normalTasks.Count > 0)
            {
                planTaskInfoList.AddRange(normalTasks);
            }

            //已合并的任务单的数据 
            if (resourceId.HasValue)
            {
                mergeRowTasks = GetMergeRowTasksByResource(workShopId, resourceId.Value, now, sDate, eDate);
            }
            else
            {
                mergeRowTasks = GetMergeRowTasks(workShopId, now, sDate, eDate);
            }

            if (mergeRowTasks.Any())
            {
                planTaskInfoList.AddRange(mergeRowTasks);
            }

            return planTaskInfoList;
        }

        /// <summary>
        /// 根据主任务单Id列表获取任务工序BOM列表
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>任务工序BOM列表</returns>
        public virtual EntityList<TaskProcessBom> GetTaskProcessBomList(List<double> dispatchTaskIds)
        {
            return Query<TaskProcessBom>().Where(p => dispatchTaskIds.Contains(p.DispatchTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取技能清单列表
        /// </summary>
        /// <param name="workOrderIds">工单Id列表</param>
        /// <returns>技能清单列表</returns>
        private EntityList<Skill> GetSkillListByWorkOrder(List<double> workOrderIds)
        {
            return Query<Skill>().Exists<ProcessSkill>(
                    (x, y) => y.Join<Process>((c, d) => c.ProcessId == d.Id && c.IsCheck)
                        .Join<Process, RoutingProcess>((c, d) => c.Id == d.ProcessId)
                         .Join<RoutingProcess, RoutingVersion>((c, d) => c.VersionId == d.Id)
                          .Join<RoutingVersion, WorkOrder>((c, d) => c.Id == d.VersionId && workOrderIds.Contains(d.Id))
                        .Where(p => p.SkillId == x.Id)).ToList();
        }

        /// <summary>
        /// 获取工序列表
        /// </summary>
        /// <param name="workOrderIds">工单Id列表</param>
        /// <returns>工序列表</returns>
        private EntityList<Process> GetProcessListByWorkOrder(List<double> workOrderIds)
        {
            return Query<Process>().Exists<RoutingProcess>(
                    (x, y) => y.Join<RoutingVersion>((c, d) => c.VersionId == d.Id)
                        .Join<RoutingVersion, WorkOrder>((c, d) => c.Id == d.VersionId && workOrderIds.Contains(d.Id))
                        .Where(p => p.ProcessId == x.Id)).ToList();
        }

        /// <summary>
        /// 获取任务单号
        /// </summary>
        /// <param name="config">任务配置</param>
        /// <returns></returns>
        public virtual string GetTaskNo(DispatchTaskConfigValue config)
        {
            if (config == null)
            {
                return string.Empty;
            }
            return RT.Service.Resolve<NumberRuleController>()
                   .GenerateSegment(config.NumberRuleId.Value, 1)
                   .FirstOrDefault();
        }

        /// <summary>
        /// 获取单号列表
        /// </summary>
        /// <param name="config">配置值</param>
        /// <param name="qty">数量</param>
        /// <returns>单号列表</returns>
        public virtual List<string> GetTaskNo(DispatchTaskConfigValue config, int qty)
        {
            if (config == null)
                return new List<string>();
            return RT.Service.Resolve<NumberRuleController>()
                   .GenerateSegment(config.NumberRuleId.Value, qty)
                   .ToList();
        }

        /// <summary>
        /// 获取工单任务单生成
        /// </summary>
        /// <returns>任务单配置</returns>
        public virtual DispatchTaskConfigValue GetDispatchTaskConfig()
        {
            return ConfigService.GetConfig(new DispatchTaskConfig());
        }

        /// <summary>
        /// 获取工单Id的任务单Id列表
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>任务单Id列表</returns>
        private List<double> GetDispatchTaskIds(double workOrderId)
        {
            var dispatchTaskIds = new List<double>();
            var dispatchTasks = Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId).ToList();
            if (dispatchTasks.Count > 0)
            {
                dispatchTaskIds.AddRange(dispatchTasks.Select(p => p.Id).Distinct());
            }
            var associatedTasks = Query<AssociatedTask>().Exists<DispatchTask>((x, y) => y.Where(f => f.Id == x.TaskId && f.WorkOrderId == workOrderId)).ToList();
            if (associatedTasks.Count > 0)
            {
                dispatchTaskIds.AddRange(associatedTasks.Select(p => p.DispatchTaskId).Distinct());
            }
            dispatchTaskIds = dispatchTaskIds.Distinct().ToList();
            return dispatchTaskIds;
        }

        /// <summary>
        /// 获取手动报工的派工任务单
        /// 关联对象为员工或者员工所属班组或者员工所在员工组都查询
        /// </summary>
        /// <param name="info">报工任务查询信息</param>
        /// <param name="status">任务单状态列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="firstProcess">是否当前工单已生成的第一个工序的任务单(第一张任务单并不一定是首工序)</param>
        /// <param name="type">类型(0:PDA手动报工)</param>
        /// <returns>派工任务单列表</returns>
        public virtual EntityList<DispatchTask> GetDispatchTasksByEmployee(TaskQueryInfo info, List<DispatchTaskStatus> status, PagingInfo pagingInfo = null, bool? startProcess = null, bool? firstProcess = null, int? type = null)
        {
            List<int> intStatus = status.Select(p => (int)p).ToList();
            var query = DB.Query<DispatchTask>("T").Where(p => p.ReportMode == ReportMode.Manual)
                .LeftJoin<ProcessPty>("pty", (x, y) => x.ProcessId == y.ProcessId);
            var query2 = DB.Query<DispatchTask>("T").Where(p => p.ReportMode == ReportMode.Manual)
            .LeftJoin<ProcessPty>("pty", (x, y) => x.ProcessId == y.ProcessId);//用于查询未派工 派工中数据

            if (type == 0 && startProcess == true)
            {
                query = query.Where(p => p.SQL<bool>(new FormattedSql(" (( (( (pty.is_report_valid = 0 or pty.is_report_valid is null)) or pty.is_report_valid = 1 ) AND T.Task_Status != 60) OR T.Task_Status = 60)")));
                query2 = query2.Where(p => p.SQL<bool>(new FormattedSql(" (( (( (pty.is_report_valid = 0 or pty.is_report_valid is null)) or pty.is_report_valid = 1 ) AND T.Task_Status != 60) OR T.Task_Status = 60)")));
            }
            else
            {
                query = query.Where(p => p.SQL<bool>(new FormattedSql(" (( (((T.DISPATCH_QTY-T.REPORT_QTY-T.SUSPECT_QTY) > 0 and (pty.is_report_valid = 0 or pty.is_report_valid is null)) or pty.is_report_valid = 1 ) AND T.Task_Status != 60) OR T.Task_Status = 60)")));
                query2 = query2.Where(p => p.SQL<bool>(new FormattedSql(" (( (((T.DISPATCH_QTY-T.REPORT_QTY-T.SUSPECT_QTY) > 0 and (pty.is_report_valid = 0 or pty.is_report_valid is null)) or pty.is_report_valid = 1 ) AND T.Task_Status != 60) OR T.Task_Status = 60)")));
            }
            if (startProcess == true)
            {
                query = query.Where(p => p.StartProcess == true);
                query2 = query2.Where(p => p.StartProcess == true);
            }

            //当前工单已生成的第一个工序的任务单
            if (firstProcess == true)
            {
                query.Where(p => p.Seq == p.SQL<int>("(select min(TM_DISP_TASK.seq) from TM_DISP_TASK where TM_DISP_TASK.WORK_ORDER_ID = T.WORK_ORDER_ID AND TM_DISP_TASK.IS_PHANTOM = 0 GROUP BY TM_DISP_TASK.WORK_ORDER_ID )"));
                query2.Where(p => p.Seq == p.SQL<int>("(select min(TM_DISP_TASK.seq) from TM_DISP_TASK where TM_DISP_TASK.WORK_ORDER_ID = T.WORK_ORDER_ID AND TM_DISP_TASK.IS_PHANTOM = 0 GROUP BY TM_DISP_TASK.WORK_ORDER_ID )"));
            }

            ////.Where(p => p.ResourceId == info.ResourceId);
            ////if (info.ProcessId.HasValue)
            ////    query.Where(p => p.ProcessId == info.ProcessId);
            if (info.ProcessArray.IsNotEmpty() && info.ProcessArray.Trim() != "")
            {
                string[] val = info.ProcessArray.Split(',');
                if (val.Length > 0)
                {
                    double processid = Convert.ToDouble(val[0]);
                    Expression<Func<DispatchTask, bool>> exp = p => p.ProcessId == processid;
                    ////var predicate = Common.Utils.PredicateBuilder.True<DispatchTask>();
                    for (int i = 1; i < val.Length; i++)
                    {
                        if (val[i].Trim() != "")
                        {
                            double processOtherId = Convert.ToDouble(val[i]);
                            exp = exp.Or(p => p.ProcessId == processOtherId);
                        }
                    }
                    query.Where(exp);
                    query2.Where(exp);
                }

            }
            if (info.Priority.HasValue)
            {
                query.Where(p => p.Priority == (DispatchTaskPriority)info.Priority.Value);
                query2.Where(p => p.Priority == (DispatchTaskPriority)info.Priority.Value);
            }
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            switch (info.QueryDate)
            {
                case 0:   //未来三天
                    query.Where(p => p.PlanBeginTime >= now && p.PlanBeginTime <= now.AddDays(3));
                    query2.Where(p => p.PlanBeginTime >= now && p.PlanBeginTime <= now.AddDays(3));
                    break;
                case 1:  //最近一周
                    query.Where(p => p.PlanBeginTime >= now.AddDays(-7) && p.PlanBeginTime <= now);
                    query2.Where(p => p.PlanBeginTime >= now.AddDays(-7) && p.PlanBeginTime <= now);
                    break;
                case 2:  //本月
                    var firstDate = new DateTime(now.Year, now.Month, 1);
                    query.Where(p => p.PlanBeginTime >= firstDate && p.PlanBeginTime <= firstDate.AddMonths(1).AddSeconds(-1));
                    query2.Where(p => p.PlanBeginTime >= firstDate && p.PlanBeginTime <= firstDate.AddMonths(1).AddSeconds(-1));
                    break;
                default:
                    break;
            }
            switch (info.TaskType)
            {
                case -1:
                    query2.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatching || p.TaskStatus == DispatchTaskStatus.ToDispatch);
                    break;
                case 0:
                    query2.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatching || p.TaskStatus == DispatchTaskStatus.ToDispatch);
                    query.Where(p => p.TaskStatus == DispatchTaskStatus.Dispatched);
                    break;
                case 1:
                    query.Where(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Pause);
                    break;
                case 2:
                    query.Where(p => p.TaskStatus == DispatchTaskStatus.Finished);
                    break;
                default:
                    break;
            }
            if (info.TaskType != -2 && info.KeyWord.IsNotEmpty() && info.KeyWord.Trim() != "")
            {
                query.Where(p => p.No.Contains(info.KeyWord) || p.Product.Code.Contains(info.KeyWord) || p.Product.Name.Contains(info.KeyWord) || p.AssociatedWorkOrder.Contains(info.KeyWord));
            }
            if (status != null && status.Count > 0)
            {
                query.Where(p => intStatus.Contains((int)p.TaskStatus));
            }
            if (info.ResourceId.HasValue)
            {
                var resource = RF.GetById<WipResource>(info.ResourceId.Value);
                if (resource != null)
                {
                    //查询产线对应工作中心
                    var andonLine = Query<AndonLine>().Where(p => p.MachineCode == resource.Code).FirstOrDefault();
                    var workCenterCode = andonLine?.WorkCenter?.Code;
                    if (!workCenterCode.IsNullOrEmpty())
                    {
                        var resourceCodes = new List<string>() { resource.Code, workCenterCode };
                        query.Where(p => resourceCodes.Contains(p.Resource.Code));
                    }
                    else
                    {
                        query.Where(p => p.ResourceId == info.ResourceId);
                    }
                }
                else
                {
                    query.Where(p => p.ResourceId == info.ResourceId);
                }
            }
            var tasks = query.Exists<DispatchTaskDetail>((x, y) => y.Where(p => p.DispatchTaskId == x.Id)
                             //.Join<Employee>((d, e) => e.Id == info.EmployeeId
                             //&& ((e.WorkGroupId == d.AdoId && d.AdoType == AdoType.WorkGroup)
                             //|| (e.EmployeeGroupId == d.AdoId && d.AdoType == AdoType.EmployeeGroup)
                             //|| (e.Id == d.AdoId && d.AdoType == AdoType.Employee)))
                             )
                             .Distinct().OrderByDescending(p => p.UpdateDate)
                             .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            if (info.TaskType == 0 || info.TaskType == -1 && info.Visiable)
            {
                var dispatchingTasks = query2.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                if (dispatchingTasks.TotalCount > 0)
                {
                    tasks.SetTotalCount(tasks.TotalCount + dispatchingTasks.TotalCount);
                    tasks.AddRange(dispatchingTasks);
                }
            }

            var productIds = tasks.Select(p => p.ProductId).Distinct().ToList();
            var parentItems = RT.Service.Resolve<ItemController>().GetParentItemsByItemIds(productIds);
            foreach (var task in tasks)
            {
                var parentItem = parentItems.FirstOrDefault(p => p.ItemId == task.ProductId);
                if (parentItem != null)
                    task.ParShortDescription = parentItem.Bismt;
            }

            return tasks;
        }

        /// <summary>
        /// 获取派工任务的No
        /// </summary>
        /// <returns>派工任务No字符串</returns>
        public virtual string GetDispatchTaskNo()
        {
            var config = GetDispatchTaskConfigValue();

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取派工任务单配置项
        /// </summary>
        /// <returns>派工任务单配置项</returns>
        public virtual DispatchTaskConfigValue GetDispatchTaskConfigValue()
        {
            var config = ConfigService.GetConfig(new DispatchTaskConfig());
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到派工任务编码配置规则，请检查规则配置".L10N());
            }

            return config;
        }

        /// <summary>
        /// 获取派工任务配置项
        /// </summary>
        /// <returns>派工任务配置项</returns>
        public virtual DispatchConfigValue GetDispatchConfig()
        {
            return ConfigService.GetConfig(new DispatchConfig(), typeof(DispatchTask));
        }

        /// <summary>
        /// 根据编码规则获取打印模板
        /// </summary>
        /// <param name="numberRuleId">编码规则Id</param>
        /// <param name="printEntityType">实体类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>返回打印模板集合</returns>
        public virtual EntityList<PrintTemplate> GetPrintTemplates(double numberRuleId, string printEntityType, string keyword, PagingInfo pagingInfo)
        {
            return Query<PrintTemplate>().Exists<NumberRuleInTemplate>((a, b) => b.Where(p => p.TemplateId == a.Id && a.State == State.Enable && a.EntityType == printEntityType && p.RuleId == numberRuleId)).WhereIf(keyword.IsNotEmpty(), p => p.FileName.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 删除某个工单Id下所有任务单
        /// </summary>
        /// <param name="woId">某个工单Id</param>
        public virtual void DeleteCancelDispatchTasks(double woId)
        {
            //取消下达的任务单不存在合并的
            var tasks = GetDispatchTasks(woId);
            tasks.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(tasks);
        }

        /// <summary>
        /// 根据任务单单号获取任务单，并做校验
        /// </summary>
        /// <param name="barcode">任务单单号</param>
        /// <param name="reportEmployee">报工人员</param>
        /// <returns>任务单</returns>
        public virtual DispatchTask GetDispatchTaskByBarcode(string barcode, Employee reportEmployee)
        {
            var task = GetDispatchTasksByExpression(p => p.No == barcode).FirstOrDefault();
            if (task == null)
                throw new ValidationException("任务单号【{0}】不存在".L10nFormat(barcode));
            else if (task.ReportMode == ReportMode.Auto)
                throw new ValidationException("任务单【{0}】报工方式为自动，不允许手动报工".L10nFormat(barcode));
            //else if (task.TaskStatus == DispatchTaskStatus.ToDispatch || task.TaskStatus == DispatchTaskStatus.Dispatching)
            //    throw new ValidationException("任务单号【{0}】未派工".L10nFormat(barcode));
            else if (task.TaskStatus == DispatchTaskStatus.Pause || task.TaskStatus == DispatchTaskStatus.Closed || task.TaskStatus == DispatchTaskStatus.Finished)
                throw new ValidationException("任务单【{0}】状态为【{1}】，不允许报工".L10nFormat(barcode, task.TaskStatus.ToLabel()));
            else
            {
                if (task.TaskStatus != DispatchTaskStatus.ToDispatch && task.TaskStatus != DispatchTaskStatus.Dispatching)
                {
                    var batchRule = RT.Service.Resolve<Items.ItemController>().GetBatchRule(task.ProductId);
                    var taskIds = GetEmployeeRefDispatchTasks(reportEmployee.Id, batchRule.RetrospectType, task.ProcessId ?? 0, ReportMode.Manual).Select(p => p.Id).ToList();
                    if (!taskIds.Contains(task.Id))
                        throw new ValidationException("员工【{0}】没有任务单【{1}】的报工权限".L10nFormat(reportEmployee.Code, task.No));
                }
            }
            return task;
        }

        /// <summary>
        /// 根据产品Id查询机台设备列表
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(double productId, PagingInfo pagingInfo, string keyword)
        {
            return Query<EquipAccount>()
                .Exists<Equipments.EquipAccounts.EquipAccountProduct>((a, b) => b.Where(p => p.EquipAccountId == a.Id && p.ProductId == productId))
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据生产资源Id查询任务单列表
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskByResourceId(double ResourceId, PagingInfo pagingInfo, string keyword, List<DispatchTaskStatus> statuses, bool? isStartProcess = null)
        {
            var query = Query<DispatchTask>()
                .Where(p => p.ResourceId == ResourceId)
                .WhereIf(statuses != null, p => statuses.Contains(p.TaskStatus))
                .WhereIf(isStartProcess != null, p => p.StartProcess == true)
                .WhereIf(keyword.IsNotEmpty(), p => p.No.Contains(keyword) || p.WorkOrder.No.Contains(keyword));
            return query.OrderBy(p => p.PlanBeginTime).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据生产资源Id查询任务单列表
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskByResourceId(double ResourceId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<DispatchTask>()
                .Where(p => (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing) && p.ResourceId == ResourceId)
                  .WhereIf(keyword.IsNotEmpty(), p => p.No.Contains(keyword) || p.WorkOrder.No.Contains(keyword));
            return query.OrderBy(p => p.PlanBeginTime).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据生产资源Id查询带已完成任务单列表
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskByResourceFinishedId(double ResourceId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<DispatchTask>()
                .Where(p => (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Finished) && p.ResourceId == ResourceId)
                  .WhereIf(keyword.IsNotEmpty(), p => p.No.Contains(keyword) || p.WorkOrder.No.Contains(keyword));
            return query.OrderBy(p => p.PlanBeginTime).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }



        /// <summary>
        /// 工单某工序是否末工序
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual bool IsEndProcess(double workOrderId, double processId)
        {
            var task = Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId && p.ProcessId == processId).FirstOrDefault();
            return task?.EndProcess == true;
        }



        /// <summary>
        /// 根据工单工序获取派工任务单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual DispatchTask GetDispatchTaskByWoProcess(double workOrderId, double processId)
        {
            var task = Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId && p.ProcessId == processId).FirstOrDefault();
            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> GetDispatchTaskByWoPacking(double workOrderId, double resourceId)
        {
            var task = Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId && p.Process.Code.Contains("%包装%") && p.ResourceId == resourceId && (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return task;
        }

        /// <summary>
        /// 保存QcC确认
        /// </summary>
        /// <returns></returns>
        public virtual string SavePackingQcC(WorkOrder workOrder, double resourceId, string resourceName, string barcode, ItemLabel itemLabel, WipBatch wipBatch, ref PackingQc packingQc, PackingDetail packingDetail, int exceed)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var dispatchTask = GetDispatchTaskByWoPacking(workOrder.Id, resourceId).FirstOrDefault();

                if (dispatchTask == null)
                {
                    return "工单【" + workOrder.No + "】,资源【" + resourceName + "】,工序【包装】在派工单中不存在!";
                }
                if (exceed == 1)
                {
                    ItemLabel newiItemLabel1 = new ItemLabel();

                    //批次标签等于null， 在去物料标签查询。
                    var itemLabel1 = RT.Service.Resolve<ItemLabelController>().GetPackingItemLabel(barcode);
                    if (itemLabel1 == null)
                    {
                        if (!SaveItemLabel(itemLabel, barcode, packingDetail.PackingNum))
                        {
                            return "物料标签保存失败!";
                        }
                    }

                    itemLabel.Qty = itemLabel.Qty - packingDetail.PackingNum;
                    if (itemLabel.InitialQty == null || itemLabel.InitialQty == 0)
                        itemLabel.InitialQty = itemLabel.Qty;
                    //修改物料标签是否已经使用
                    itemLabel.Isuse = false;
                    RF.Save(itemLabel);
                    //修改批次生成 工序标签是否已经使用
                    wipBatch.Isuse = false;
                    RF.Save(wipBatch);
                    var newiItemLabel = RT.Service.Resolve<ItemLabelController>().GetPackingItemLabel(barcode);
                    //是否存在如果存在提示，不存在 新增
                    var feedingRecord = RT.Service.Resolve<FeedingRecordController>().GetFeedingRecord(barcode);

                    if (!SaveFeedingRecord(barcode, newiItemLabel.Id, dispatchTask, newiItemLabel))
                    {
                        return "上料记录保存失败!";
                    }

                }
                else
                {
                    itemLabel.ItemLabelState = ItemLabelState.Feeding;
                    //修改物料标签是否已经使用
                    itemLabel.Isuse = true;
                    RF.Save(itemLabel);
                    //修改批次生成 工序标签是否已经使用
                    wipBatch.Isuse = true;
                    RF.Save(wipBatch);
                    var feedingRecord = RT.Service.Resolve<FeedingRecordController>().GetFeedingRecord(barcode);
                    if (feedingRecord == null)
                    {
                        if (!SaveFeedingRecord(itemLabel.Label, itemLabel.Id, dispatchTask, itemLabel))
                        {
                            return "上料记录保存失败!";
                        }
                    }
                }
                barcode = barcode + "-BG";
                RF.Save(packingQc);
                //存入包装QC确认明细表
                //barcode = barcode + "-BG";
                packingDetail.ProductLabel = barcode;
                packingDetail.PackingQcId = packingQc.Id;
                packingDetail.Confirm = ConfirmEnum.YES;
                packingDetail.BatchLabel = "";
                packingDetail.WorkOrderNo = workOrder.No;

                packingDetail.LabelType = LabelTypeEnum.BatchLabel;

                packingDetail.ReportsType = ReportsTypeEnum.NO;
                RF.Save(packingDetail);
                tran.Complete();
            }
            return "";
        }

        /// <summary>
        /// 保存上料记录
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveFeedingRecord(string label, double itemLabelId, DispatchTask dispatchTask, ItemLabel itemLabel)
        {
            FeedingRecord record = new FeedingRecord();
            try
            {
                record.DispatchTaskId = dispatchTask.Id;
                record.ResourceId = dispatchTask.ResourceId;
                record.ItemLabelId = itemLabelId;
                record.FeedingItemLabel = label;
                record.ItemId = itemLabel?.ItemId;
                record.FeedingQty = itemLabel?.Qty;
                record.RemainingQty = itemLabel?.Qty;
            }
            catch (Exception)
            {

                return false;
            }
            return RT.Service.Resolve<FeedingRecordController>().SaveFeedingRecord(record);
        }

        /// <summary>
        /// 保存物料标签
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <returns></returns>
        public virtual bool SaveItemLabel(ItemLabel itemLabel, string itemLabelName, decimal qty)
        {
            ItemLabel item = new ItemLabel();
            item.AsnNo = itemLabel.AsnNo;
            item.FactoryId = itemLabel.FactoryId;
            item.IsSerialNumber = itemLabel.IsSerialNumber;
            item.ItemExtProp = itemLabel.ItemExtProp;
            item.ItemExtPropName = itemLabel.ItemExtPropName;
            item.ItemId = itemLabel.ItemId;
            item.Label = itemLabelName;
            item.LongLived = itemLabel.LongLived;
            item.Lot = itemLabel.Lot;
            item.NgQty = itemLabel.NgQty;
            item.OriginalLabel = itemLabel.OriginalLabel;
            item.PnlHeight = itemLabel.PnlHeight;
            item.PnlWidth = itemLabel.PnlWidth;
            item.ProductBatch = itemLabel.ProductBatch;
            item.ProductionDate = itemLabel.ProductionDate;
            item.Qty = qty;
            item.InitialQty = qty;
            item.RelationId = itemLabel.RelationId;
            item.RemainLongLived = itemLabel.RemainLongLived;
            item.SourceType = itemLabel.SourceType;
            item.StorageLocationId = itemLabel.StorageLocationId;
            item.SupplierId = itemLabel.SupplierId;
            item.UnitId = itemLabel.UnitId;
            item.ValidityEnd = itemLabel.ValidityEnd;
            item.ValidityStart = itemLabel.ValidityStart;
            item.WarehouseId = itemLabel.WarehouseId;
            item.NgReturnQtyInTransit = itemLabel.NgReturnQtyInTransit;
            item.ProjectNo = itemLabel.ProjectNo;
            item.ReturnQtyInTransit = itemLabel.ReturnQtyInTransit;
            item.ItemLabelState = ItemLabelState.Feeding;
            item.Lgort = itemLabel.Lgort;
            item.Exidv = itemLabel.Exidv;
            item.Exidv2 = itemLabel.Exidv2;
            item.WorkOrderId = itemLabel.WorkOrderId;
            item.Isuse = true;
            try
            {
                RF.Save(item);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
            //return RT.Service.Resolve<ItemLabelController>().SaveItemLabel(item);

        }

        #region 合并任务记录
        /// <summary>
        /// 获取合并任务记录
        /// </summary>
        /// <param name="taskId">派工任务ID</param>
        /// <returns>合并任务记录</returns>
        private MergeTaskRecord GetMergeTaskRecord(double taskId)
        {
            return Query<MergeTaskRecord>().Where(p => p.TaskId == taskId).FirstOrDefault();
        }

        /// <summary>
        /// 判断工单任务单及关联的工单任务单是否存在合并
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>存在合并返回true，否则返回false</returns>
        public virtual bool IsWorkOrderTaskMerge(string workOrder)
        {
            return Query<MergeTaskRecord>().Where(p => p.MergeWorkOrder.Contains($"%{workOrder}%")).Count() > 0;
        }

        /// <summary>
        /// 删除合并任务记录
        /// </summary>
        /// <param name="taskId">派工任务ID</param>
        private void DeleteTaskMergeRecord(double taskId)
        {
            var record = GetMergeTaskRecord(taskId);
            if (record == null)
            {
                return;
            }
            record.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(record);
        }

        /// <summary>
        /// 添加合并任务记录
        /// example：
        /// 1、工单G1的任务001和工单G2任务002合并，则需添加记录：G1-001-G1;G2、G2-002-G1;G2（对应方法参数）
        /// 1、工单G1的任务001和工单G2任务002和工单G3任务单003合并，则需添加记录：G1-001-G1;G2;G3、G2-002-G1;G2;G3、G3-003-G1;G2;G3
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="taskId">派工任务ID</param>
        /// <param name="mergeWorkOrder">合并关联的工单</param>
        private void AddMergeTaskRecord(double workOrderId, double taskId, string mergeWorkOrder)
        {
            var record = GetMergeTaskRecord(taskId);
            if (record == null)
            {
                RF.Save(new MergeTaskRecord()
                {
                    WorkOrderId = workOrderId,
                    TaskId = taskId,
                    MergeWorkOrder = mergeWorkOrder
                });
                return;
            }
            record.WorkOrderId = workOrderId;
            record.MergeWorkOrder = mergeWorkOrder;
            RF.Save(record);
        }
        #endregion

        #region 关联任务单
        /// <summary>
        /// 获取关联任务单列表
        /// </summary>
        /// Expression不支持序列号，前端不允许调用
        /// <param name="exp">条件</param>
        /// <returns>任务单列表</returns>
        public virtual EntityList<AssociatedTask> GetAssociatedTasks(Expression<Func<AssociatedTask, bool>> exp)
        {
            var query = Query<AssociatedTask>();
            if (exp != null)
            {
                query.Where(exp);
            }
            return query.ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取合并子任务单列表
        /// </summary>
        /// <param name="dispatchTaskId">派工任务单ID</param>
        /// <returns>任务单列表</returns>
        public virtual EntityList<AssociatedTask> GetMergeChildDispatchTask(double dispatchTaskId)
        {
            return Query<AssociatedTask>()
                .Join<DispatchTask>((a, d) => a.TaskId == d.Id && d.MergedStatus == MergedStatus.Merged)
                .Where(p => p.DispatchTaskId == dispatchTaskId).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据主任务单Id列表获取关联任务单列表
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>关联任务单列表</returns>
        public virtual EntityList<AssociatedTask> GetAssociatedTaskList(List<double> dispatchTaskIds)
        {
            return Query<AssociatedTask>().Where(p => dispatchTaskIds.Contains(p.DispatchTaskId)).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据辅单任务单Id获取主任务单列表
        /// </summary>
        /// <param name="taskId">任务单Id</param>
        /// <returns>关联任务单列表</returns>
        public virtual EntityList<AssociatedTask> GetAssociatedTaskListByTaskId(double taskId)
        {
            return Query<AssociatedTask>().Where(p => p.TaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取关联
        /// </summary>
        /// <param name="dispatchTaskIds">关联任务单Id列表</param>
        /// <returns>关联任务单列表</returns>
        public virtual EntityList<AssociatedTask> GetAssociatedTaskListByTask(List<double> dispatchTaskIds)
        {
            return Query<AssociatedTask>().Where(p => dispatchTaskIds.Contains(p.TaskId)).ToList(null, new EagerLoadOptions().LoadWith(AssociatedTask.DispatchTaskProperty));
        }

        /// <summary>
        /// 根据已合并任务单Id获取主任务单
        /// </summary>
        /// <param name="taskId">任务单Id</param>
        /// <returns>关联任务单列表</returns>
        public virtual AssociatedTask GetAssociatedTaskByMergeTaskId(double taskId)
        {
            return Query<AssociatedTask>().Where(p => p.TaskId == taskId).FirstOrDefault();
        }

        /// <summary>
        /// 通过派工任务获取关联任务单列表
        /// </summary>
        /// <param name="dispatchTaskId">派工任务Id</param>
        /// <returns>关联任务单列表</returns>
        public virtual EntityList<AssociatedTask> GetAssociatedDispatchTaskList(double dispatchTaskId)
        {
            return Query<AssociatedTask>().Where(p => p.DispatchTaskId == dispatchTaskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据合并行任务单Id列表获取关联任务单列表
        /// </summary>
        /// <param name="dispatchTaskIds">并行任务单Id列表</param>
        /// <returns>关联任务单列表</returns>
        public virtual EntityList<AssociatedTask> GetAssociatedTasksOfMergedTask(List<double> dispatchTaskIds)
        {
            return dispatchTaskIds.SplitContains((tempTaskIds) =>
            {
                return Query<AssociatedTask>().Exists<DispatchTask>((a, b) => b.Where(p => p.Id == a.DispatchTaskId && p.MergedStatus == MergedStatus.MergeRows && tempTaskIds.Contains(p.Id))).ToList(null, new EagerLoadOptions().LoadWith(AssociatedTask.TaskProperty));
            });
        }

        #endregion

        #region 生成任务单 
        /// <summary>
        /// 工单修改重新生成任务单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="isNew">工单是否新增</param>
        public virtual void WorkOrderGenerateDispathTask(double workOrderId, bool isNew = false)
        {
            try
            {
                var taskConfig = GetDispatchTaskConfig();
                if (!taskConfig.IsGenerate)
                {
                    return;
                }

                if (isNew && taskConfig.GenerateMode != ReportMode.Auto)
                {
                    return;
                }

                //是否重新生成任务单
                bool isReGenerated = false;

                if (!isNew)
                {
                    var task = GetDispatchTasks(workOrderId);

                    if (task.Count > 0)
                    {
                        //这样可以同时删除子列表吗？
                        task.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                        RF.Save(task);

                        isReGenerated = true;
                    }
                }

                //自动生成或已经生成时，再生成任务单
                if (taskConfig.GenerateMode == ReportMode.Auto || isReGenerated)
                {
                    GenerateWorkOrderDispatchTasks(workOrderId, true, taskConfig);
                }
            }
            catch
            {
                if (!isNew)
                {
                    throw;
                }
                RT.Logger.Error("{0}工单[{1}]生成任务单失败".L10nFormat(isNew ? "新增".L10N() : "修改".L10N(), workOrderId));
            }
        }

        /// <summary>
        /// 工单修改更新任务单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="processBomDirty">工序BOM是否脏数据</param>
        public virtual void WorkOrderUpdateDispathTask(double workOrderId, bool processBomDirty = false)
        {
            var taskConfig = GetDispatchTaskConfig();
            if (!taskConfig.IsGenerate) return;

            var task = GetDispatchTasks(workOrderId);

            var workOrder = RF.GetById<WorkOrder>(workOrderId);

            // PMS单号B0035139 修改工单时，判断如果工单还未生成任务单，则修改时不生成任务。
            if (task.Any())
            {
                task.ForEach(p =>
                {
                    //p.WorkShopId = workOrder.WorkShopId;
                    //排程导入类型和MES生成的的时候，不要更新这些字段信息
                    //if (p.SourceType != SourceType.SchedulingInf && p.SourceType != SourceType.Mes)
                    //{
                    //    p.ResourceId = workOrder.ResourceId;
                    //    p.PlanBeginTime = workOrder.PlanBeginDate;
                    //    p.PlanEndTime = workOrder.PlanEndDate;
                    //}
                    if (processBomDirty)
                    {
                        p.Boms.Clear();
                        if (p.ProcessId.HasValue || p.ProcessId > 0)
                        {
                            //按工序生成任务单                          
                            workOrder.ProcessBomList.Where(f => f.ProcessId == p.ProcessId).ForEach(f =>
                            {
                                TaskProcessBom taskProcessBom = new TaskProcessBom();
                                taskProcessBom.ItemId = f.ItemId;
                                taskProcessBom.Qty = f.SingleQty;
                                taskProcessBom.ProcessId = f.ProcessId.Value;
                                p.Boms.Add(taskProcessBom);
                            });
                        }
                        else
                        {
                            workOrder.ProcessBomList.Where(f => f.ProcessId.HasValue).ForEach(f =>
                            {
                                TaskProcessBom taskProcessBom = new TaskProcessBom();
                                taskProcessBom.ItemId = f.ItemId;
                                taskProcessBom.Qty = f.SingleQty;
                                taskProcessBom.ProcessId = f.ProcessId.Value;
                                p.Boms.Add(taskProcessBom);
                            });
                        }
                    }
                });

                RF.Save(task);
            }
        }

        /// <summary>
        /// 按照工序产生任务单
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="isAccordConfig"></param>
        /// <param name="taskConfig"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<DispatchTask> CreateDispatchByProcessId(WorkOrder workOrder, bool isAccordConfig, DispatchTaskConfigValue taskConfig, double processId)
        {
            if (taskConfig.NumberRule == null)
                throw new ValidationException("没有配置生成任务单单号规则".L10N());
            if (workOrder == null)
                throw new ValidationException("工单为空".L10N());
            if (workOrder.Product.ProductFamilyId == null)
                throw new ValidationException("产品[{0}]未维护产品族，请维护产品族及对应报工规则".L10nFormat(workOrder.ProductCode));
            var reportCt = RT.Service.Resolve<ReportController>();
            bool isSyntype = false;
            var reportRule = reportCt.GetReportRuleConfig(workOrder.Product.ProductFamilyId ?? 0);
            if (reportRule == null)
                throw new ValidationException("未找到产品[{0}]报工规则配置，请在产品族中维护".L10nFormat(workOrder.ProductCode));

            isSyntype = reportRule.IsSyntype;
            bool isCommonGenerate = workOrder.IsCommonMode && workOrder.IsMainMaterial;  //共模主工单生成任务单
            var mainTasks = RT.Service.Resolve<DispatchController>().GenerateDispathTasksByProcess(workOrder, taskConfig, isCommonGenerate, isSyntype, processId);
            return mainTasks;
        }

        /// <summary>
        /// 按照工序产生任务单
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="isAccordConfig"></param>
        /// <param name="taskConfig"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        /// <param name="isValidProTask">是否校验已经生成过相同工序任务单</param>
        public virtual EntityList<DispatchTask> CreateDispatch(WorkOrder workOrder, bool isAccordConfig, DispatchTaskConfigValue taskConfig, bool? isProcessPty = null, bool? isValidProTask = null)
        {
            if (taskConfig.NumberRule == null)
                throw new ValidationException("没有配置生成任务单单号规则".L10N());
            if (workOrder == null)
                throw new ValidationException("工单为空".L10N());
            if (workOrder.Product.ProductFamilyId == null)
                throw new ValidationException("产品[{0}]未维护产品族，请维护产品族及对应报工规则".L10nFormat(workOrder.ProductCode));
            var reportCt = RT.Service.Resolve<ReportController>();
            bool isSyntype = false;
            var reportRule = reportCt.GetReportRuleConfig(workOrder.Product.ProductFamilyId ?? 0);
            if (reportRule == null)
                throw new ValidationException("未找到产品[{0}]报工规则配置，请在产品族中维护".L10nFormat(workOrder.ProductCode));

            isSyntype = reportRule.IsSyntype;
            bool isCommonGenerate = workOrder.IsCommonMode && workOrder.IsMainMaterial;  //共模主工单生成任务单OM
            var mainTasks = RT.Service.Resolve<DispatchController>().GenerateDispathTasks(workOrder, taskConfig, isCommonGenerate, isSyntype, isProcessPty, isValidProTask);
            return mainTasks;
        }

        /// <summary>
        /// 生成工单任务单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="isAccordConfig">是否按照配置条件</param>
        /// <param name="taskConfig">任务单配置</param>  
        public virtual void GenerateWorkOrderDispatchTasks(double workOrderId, bool isAccordConfig, DispatchTaskConfigValue taskConfig)
        {
            if (taskConfig.NumberRule == null)
                throw new ValidationException("没有配置生成任务单单号规则".L10N());
            var workOrder = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWithViewProperty());
            if (workOrder == null)
                throw new ValidationException("工单为空".L10N());
            if (workOrder.Product.ProductFamilyId == null)
                throw new ValidationException("产品[{0}]未维护产品族，请维护产品族及对应报工规则".L10nFormat(workOrder.ProductCode));
            var reportCt = RT.Service.Resolve<ReportController>();
            bool isSyntype = false;
            var reportRule = reportCt.GetReportRuleConfig(workOrder.Product.ProductFamilyId ?? 0);
            if (reportRule == null)
                throw new ValidationException("未找到产品[{0}]报工规则配置，请在产品族中维护".L10nFormat(workOrder.ProductCode));

            isSyntype = reportRule.IsSyntype;

            //共模工单
            if (workOrder.IsCommonMode)
            {
                if (!workOrder.IsMainMaterial)
                {
                    //共模非主料的工单
                    var mainWo = RT.Service.Resolve<WorkOrderController>().GetMainWorkOrder(workOrder.PlanNo);
                    var mainReportRule = reportCt.GetReportRuleConfig(mainWo.Product.ProductFamilyId ?? 0);
                    if (mainReportRule == null)
                        throw new ValidationException("共模辅料的工单未找到共模主料产品[{0}]报工规则配置，请在产品族中维护".L10nFormat(mainWo.ProductCode));
                    if (mainReportRule.IsSyntype)
                        isSyntype = mainReportRule.IsSyntype;
                }
                else
                {
                    //主料
                    if (reportRule.IsSyntype && !isAccordConfig)
                        isSyntype = isAccordConfig;
                }
            }

            bool isCommonGenerate = workOrder.IsCommonMode && workOrder.IsMainMaterial;  //共模主工单生成任务单
            var mainTasks = GenerateDispathTasks(workOrder, taskConfig, isCommonGenerate, isSyntype);
            var commonModeTasks = GenerateCommonModeDispatchTasks(workOrder, taskConfig, isCommonGenerate, isSyntype);
            var associatedTasks = AssociatedTasks(isSyntype, mainTasks, commonModeTasks);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(mainTasks);
                RF.Save(commonModeTasks);
                RF.Save(associatedTasks);
                tran.Complete();
            }
        }

        /// <summary>
        /// 关联共模任务单
        /// </summary>
        /// <param name="isSyntype">是否工模比报工</param>
        /// <param name="mainTasks">主料工单生成任务单</param>
        /// <param name="commonModeTasks">辅料工单生成任务单</param>
        /// <returns>共模任务单关联关系列表</returns>
        private EntityList<AssociatedTask> AssociatedTasks(bool isSyntype, EntityList<DispatchTask> mainTasks, EntityList<DispatchTask> commonModeTasks)
        {
            EntityList<AssociatedTask> associatedTasks = new EntityList<AssociatedTask>();
            if (commonModeTasks.Count <= 0 || !isSyntype)  //非共模比报工任务单不做关联，单独报工
                return associatedTasks;
            if (mainTasks.All(p => p.ProcessId == null))
                throw new ValidationException("任务按共模比报工时，只能按工序任务拆分。请确认主料工单与辅料工单的工艺路线至少有设置一个相同的任务工序！".L10N());
            mainTasks.Where(p => p.ProcessId != null).ForEach(mainTask =>
            {
                var result = commonModeTasks.Where(p => p.ProcessId == mainTask.ProcessId);
                result.ForEach(task =>
                {
                    associatedTasks.Add(new AssociatedTask()
                    {
                        DispatchTask = mainTask,
                        Task = task
                    });
                });
                var noList = result.Select(p => p.WorkOrder.No).ToList();
                if (!mainTask.AssociatedWorkOrder.IsNullOrEmpty())
                    noList.Add(mainTask.AssociatedWorkOrder);
                mainTask.AssociatedWorkOrder = string.Join(";", noList.Distinct());

            });

            if (!associatedTasks.Any())
                throw new ValidationException("任务按共模比报工时，只能按工序任务拆分。请确认主料工单与辅料工单的工艺路线至少有设置一个相同的任务工序！".L10N());

            return associatedTasks;
        }

        /// <summary>
        /// 生成共模辅料工单任务单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="taskConfig">派工任务单配置</param>
        /// <param name="isCommonGenerate">共模主工单生成任务单</param>
        /// <param name="isSyntypeReport">是否工模比报工</param>
        /// <returns>派工任务列表</returns>
        private EntityList<DispatchTask> GenerateCommonModeDispatchTasks(WorkOrder workOrder, DispatchTaskConfigValue taskConfig, bool isCommonGenerate, bool isSyntypeReport)
        {
            EntityList<DispatchTask> tasks = new EntityList<DispatchTask>();
            //共模工单、共模比报工、主料才生成辅料工单任务单
            if (workOrder.IsCommonMode && workOrder.IsMainMaterial && isSyntypeReport)
            {
                //共模主料工单--创建辅料工单任务单
                var woList = RT.Service.Resolve<WorkOrderController>().GetCommonModeWorkOrders(workOrder.PlanNo);
                var list = woList.Where(p => p.State != Core.WorkOrders.WorkOrderState.Release).ToList();
                if (list.Count > 0)
                    throw new ValidationException("计划单号[{0}]相关联的工单[{1}]非发放状态".L10nFormat(workOrder?.PlanNo, string.Join(",", list.Select(p => p.No))));
                EntityList<WorkOrder> workOrders = new EntityList<WorkOrder>();
                woList.ForEach(wo =>
                {
                    if (!IsExistDispatchTask(p => p.WorkOrderId == wo.Id))
                        workOrders.Add(wo);
                });
                workOrders.ForEach(p =>
                {
                    tasks.AddRange(GenerateDispathTasks(p, taskConfig, isCommonGenerate, isSyntypeReport));
                });
            }
            return tasks;
        }

        /// <summary>
        /// 按照某个工序生成任务单
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="taskConfig"></param>
        /// <param name="isCommonGenerate"></param>
        /// <param name="isSyntypeReport"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTask> GenerateDispathTasksByProcess(WorkOrder workOrder, DispatchTaskConfigValue taskConfig, bool isCommonGenerate, bool isSyntypeReport, double processId)
        {
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

            EntityList<DispatchTask> taskList = new EntityList<DispatchTask>();
            var tuple = ValidationWorkOrder(workOrder, isCommonGenerate, isSyntypeReport);
            if (tuple == null)
            {
                return taskList;
            }
            var familyConfig = tuple.Item1;
            var reportConfig = tuple.Item2;
            var reportMode = reportConfig.ReportMode;
            var processList = new EntityList<WorkOrderRoutingProcess>();
            if ((int)reportMode == -1)
            {
                throw new ValidationException("请在产品族中配置报工方式".L10N());
            }
            EntityList<ProductSpecificationDetail> specificationDetail = null;
            if (familyConfig.BySpecification)
            {
                specificationDetail = RT.Service.Resolve<ProductSpecificationController>().GetProductSpecification(workOrder.ProductId)?.Details;
            }
            //按工序生成任务单            
            processList = workOrder.RoutingProcessList.Where(p => p.IsGenerateTask && p.Process.Type != ProcessType.Fix && p.Process.Type != ProcessType.BatchFix).OrderBy(p => p.Index).AsEntityList();
            if (!processList.Any())
            {
                throw new ValidationException("生成任务单失败。该产品族配置了【按工序生成任务单】，但未存在任何工序勾选【是否生成任务单】,请检查".L10N());
            }
            //获取工序属性(下面用于判断是否按照工序BOM总和*工单计划数量=任务数量)
            var processIds = processList.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

            //获取首工序，用于下方判断
            //var firstProcess = workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault();

            //在工单工序清单中，指定某个工序去生成任务单
            //processList.Where(p => p.ProcessId == processId).ForEach(p =>
            foreach (var p in processList.Where(p => p.ProcessId == processId))
            {
                //只获取当前工厂+工序
                var layoutInfo = workOrder.LayoutInfoList.Where(item => item.Factory == invOrg.ExternalId && item.ProcessCode == p.Process.Code).FirstOrDefault();
                if (layoutInfo == null)
                    continue;

                var taskBomList = new EntityList<TaskProcessBom>();
                workOrder.ProcessBomList.Where(f => f.ProcessId == p.ProcessId).ForEach(f =>
                {
                    TaskProcessBom taskProcessBom = new TaskProcessBom();
                    taskProcessBom.ItemId = f.ItemId;
                    taskProcessBom.Qty = f.SingleQty;
                    taskProcessBom.ProcessId = f.ProcessId.Value;
                    taskBomList.Add(taskProcessBom);
                });
                EntityList<TaskProcessBom> boms = new EntityList<TaskProcessBom>();
                boms.Clone(taskBomList, new CloneOptions(CloneActions.IdProperty | CloneActions.NormalProperties | CloneActions.RefEntities));
                var task = InitDispatchTask(workOrder, taskConfig, reportMode, isSyntypeReport, true);
                task.ProcessId = p.ProcessId;
                task.RoutingProcessId = p.Id;
                task.Seq = p.Index;
                //当工单主表信息和当前的库存组织不同的时候，就是认为是委外任务单
                if (workOrder.Factory.Code != invOrg.ExternalId)
                    task.IsOutsourcing = true;
                task.Boms.AddRange(boms);
                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { p.ProcessId.Value }, workOrder.ProductId);
                if (processPtys.Count < 1)
                    continue;
                var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(workOrder.ProductId);
                var pps = new List<ProcessPty>();
                if (kzItemCategory != null)
                {
                    pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                }
                ////当找得到分类得时候，优先找到分类的，然后再找工序的
                if (pps.Count == 0)
                    pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                var processPty = pps.FirstOrDefault(f => f.ProcessId == p.ProcessId);
                //判断是否按照工序BOM总和*工单计划数量=任务数量
                if (processPty != null && processPty.IsPbcd == true)
                {
                    var itemIds = task.Boms.Select(p => p.ItemId).Distinct().ToList();
                    var requireQty = task.WorkOrder.BomList.Where(p => itemIds.Contains(p.ItemId) && p.IsVritualItem == false).Sum(p => p.RequireQty);
                    task.DispatchQty = requireQty; //task.Boms.Sum(p => p.Qty) /** workOrder.PlanQty*/;
                }
                else
                    task.DispatchQty = workOrder.PlanQty;
                taskList.Add(task);
            }
            ;

            //设置首末工序标识
            var startProcess = processList.FirstOrDefault().StartProcess;
            var endProcess = processList.FirstOrDefault().EndProcess;
            if (startProcess != null)
            {
                var startProcessTasks = taskList.Where(p => p.ProcessId == startProcess);
                startProcessTasks.ForEach(p =>
                {
                    p.StartProcess = true;
                });
            }
            if (endProcess != null)
            {
                var endProcessTasks = taskList.Where(p => p.ProcessId == endProcess);
                endProcessTasks.ForEach(p =>
                {
                    p.EndProcess = true;
                });
            }
            //var startProcess = processList.FirstOrDefault();
            //var endProcess = processList.LastOrDefault();
            //if (startProcess != null)
            //{
            //    var startProcessTasks = taskList.Where(p => p.ProcessId == startProcess.ProcessId);
            //    startProcessTasks.ForEach(p =>
            //    {
            //        p.StartProcess = true;
            //    });
            //}
            //if (endProcess != null)
            //{
            //    var endProcessTasks = taskList.Where(p => p.ProcessId == endProcess.ProcessId);
            //    endProcessTasks.ForEach(p =>
            //    {
            //        p.EndProcess = true;
            //    });
            //}
            foreach (var task in taskList)
            {
                task.FactoryId = workOrder.FactoryId;//赋值工厂ID
            }
            return taskList;
        }

        /// <summary>
        /// 生成工单任务单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="taskConfig">派工任务单配置</param>
        /// <param name="isCommonGenerate">共模主工单生成任务单</param>
        /// <param name="isSyntypeReport">是否工模比报工</param>
        /// <returns>派工任务列表</returns>
        /// <param name="isValidProTask">是否校验已经生成过相同工序任务单</param>
        public virtual EntityList<DispatchTask> GenerateDispathTasks(WorkOrder workOrder, DispatchTaskConfigValue taskConfig, bool isCommonGenerate, bool isSyntypeReport, bool? isProcessPty = null, bool? isValidProTask = null)
        {
            EntityList<DispatchTask> taskList = new EntityList<DispatchTask>();
            var tuple = ValidationWorkOrder(workOrder, isCommonGenerate, isSyntypeReport);
            if (tuple == null)
            {
                return taskList;
            }
            var familyConfig = tuple.Item1;
            var reportConfig = tuple.Item2;
            var reportMode = reportConfig.ReportMode;
            var processList = new EntityList<WorkOrderRoutingProcess>();
            if ((int)reportMode == -1)
            {
                throw new ValidationException("请在产品族中配置报工方式".L10N());
            }
            EntityList<ProductSpecificationDetail> specificationDetail = null;
            if (familyConfig.BySpecification)
            {
                specificationDetail = RT.Service.Resolve<ProductSpecificationController>().GetProductSpecification(workOrder.ProductId)?.Details;
            }
            if (familyConfig.ByProcess)
            {
                //按工序生成任务单            
                processList = workOrder.RoutingProcessList.Where(p => p.IsGenerateTask && p.Process.Type != ProcessType.Fix && p.Process.Type != ProcessType.BatchFix).OrderBy(p => p.Index).AsEntityList();
                if (!processList.Any())
                {
                    throw new ValidationException("生成任务单失败。该产品族配置了【按工序生成任务单】，但未存在任何工序勾选【是否生成任务单】,请检查".L10N());
                }
                GenerateByProcess(processList, workOrder, taskConfig, reportMode, familyConfig, specificationDetail, taskList, isSyntypeReport, isProcessPty, isValidProTask);
            }
            else if (familyConfig.ByQty)
            {
                //按固定值
                GenerateByQty(workOrder, taskConfig, reportMode, familyConfig, specificationDetail, taskList, isSyntypeReport);
            }
            else if (familyConfig.BySpecification && specificationDetail != null && specificationDetail.Count > 0)
            {
                //按规格件                              
                GenerateBySpecification(workOrder, taskConfig, reportMode, familyConfig, specificationDetail, taskList, workOrder.PlanQty, isSyntypeReport);
            }
            else
            {
                //什么都没有勾选，默认生成一张任务单
                var task = InitDispatchTask(workOrder, taskConfig, reportMode, isSyntypeReport, familyConfig.ByProcess);
                task.DispatchQty = workOrder.PlanQty;
                task.FactoryId = workOrder.FactoryId;
                taskList.Add(task);
            }
            if (familyConfig.ByVirtualPart)
            {
                //按虚拟件                
                workOrder.BomList.Where(f => f.IsVritualItem).ForEach(f =>
                {
                    var task = InitDispatchTask(workOrder, taskConfig, ReportMode.Manual, isSyntypeReport); //虚拟件都是手动报工
                    task.DispatchQty = f.RequireQty;
                    task.SingleQty = f.SingleQty;
                    task.IsVirtualPart = true;
                    task.VirtualPartCode = f.Item.Code;
                    task.VirtualPartName = f.Item.Name;
                    task.FactoryId = workOrder.FactoryId;
                    taskList.Add(task);
                });
            }

            //设置首末工序标识
            var startProcess = processList.FirstOrDefault().StartProcess;
            var endProcess = processList.FirstOrDefault().EndProcess;
            if (startProcess != null)
            {
                var startProcessTasks = taskList.Where(p => p.ProcessId == startProcess);
                startProcessTasks.ForEach(p =>
                {
                    p.StartProcess = true;
                });
            }
            if (endProcess != null)
            {
                var endProcessTasks = taskList.Where(p => p.ProcessId == endProcess);
                endProcessTasks.ForEach(p =>
                {
                    p.EndProcess = true;
                });
            }
            //设置首末工序标识
            //var startProcess = processList.FirstOrDefault();
            //var endProcess = processList.LastOrDefault();
            //if (startProcess != null)
            //{
            //    var startProcessTasks = taskList.Where(p => p.ProcessId == startProcess.ProcessId);
            //    startProcessTasks.ForEach(p =>
            //    {
            //        p.StartProcess = true;
            //    });
            //}
            //if (endProcess != null)
            //{
            //    var endProcessTasks = taskList.Where(p => p.ProcessId == endProcess.ProcessId);
            //    endProcessTasks.ForEach(p =>
            //    {
            //        p.EndProcess = true;
            //    });
            //}
            foreach (var task in taskList)
            {
                task.FactoryId = workOrder.FactoryId;//赋值工厂ID
            }
            return taskList;
        }

        /// <summary>
        /// 判断工单是否已有任务单
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>bool</returns>
        public virtual bool IsExistsDispatchTask(double workOrderId)
        {
            return Query<DispatchTask>().Where(p => p.WorkOrderId == workOrderId).Count() > 0;
        }

        /// <summary>
        /// 按工序生成任务单
        /// </summary>
        /// <param name="processList">工序清单</param>
        /// <param name="workOrder">工单</param>
        /// <param name="taskConfig">派工任务单配置值</param>
        /// <param name="reportMode">报工方式</param>
        /// <param name="familyConfig">产品族任务单配置项</param>
        /// <param name="specificationDetail">产品规格件清单</param>
        /// <param name="taskList">要保存的任务单列表</param>
        /// <param name="isSyntypeReport">是否工模比报工</param>
        /// <param name="isValidProTask">是否校验已经生成过相同工序任务单</param>
        private void GenerateByProcess(EntityList<WorkOrderRoutingProcess> processList, WorkOrder workOrder, DispatchTaskConfigValue taskConfig, ReportMode reportMode, TaskConfig familyConfig, EntityList<ProductSpecificationDetail> specificationDetail, EntityList<DispatchTask> taskList, bool isSyntypeReport, bool? isProcessPty = null, bool? isValidProTask = null)
        {
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

            var processIds = processList.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

            var tasks = Query<DispatchTask>().Where(p => p.WorkOrderId == workOrder.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //获取首工序
            var firstRoutingProcess = workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault();

            foreach (var p in processList)
            {
                var pps = new List<ProcessPty>();
                //返工工单只有最后一道工序生成任务单，只有开始工序等于当前工单的时候，就是
                if (workOrder.Type == WorkOrderType.Rework)
                {
                    if (p.ProcessId != p.StartProcess)
                    {
                        continue;
                    }
                }
                else
                {
                    //是否校验，同一个工序不能生成多个任务单
                    if (isValidProTask == true && tasks.Any(f => f.ProcessId == p.ProcessId))
                        continue;
                    if (taskList.Any(f => f.ProcessId == p.ProcessId))
                        continue;

                    //这个工序必须要是在该工厂(当下库存组织下的)，如果该工序的工厂不是这个工厂的，就认定为委外的，需要去其他工厂(库存组织)创建任务单去生产
                    LayoutInfo info = null;
                    //防止旧数据，LayoutInfoId字段为空
                    if (p.LayoutInfoId != null)
                        info = workOrder.LayoutInfoList.FirstOrDefault(item => item.Id == p.LayoutInfoId && item.Factory == invOrg.ExternalId);
                    else
                        info = workOrder.LayoutInfoList.FirstOrDefault(item => item.ProcessCode == p.Process.Code && item.Factory == invOrg.ExternalId);

                    if (info == null)
                        continue;
                    EntityList<ProcessPty> processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { p.ProcessId.Value }, workOrder.ProductId);
                    if (processPtys.Count < 1)
                        continue;

                    var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(workOrder.ProductId);
                    if (kzItemCategory != null)
                    {
                        pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                    }
                    ////当找得到分类得时候，优先找到分类的，然后再找工序的
                    if (pps.Count == 0)
                        pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                    //当需要校验【维护工序属性】的时候，当工序属性中派工点为是且排程点为否的工序任务单，才能生成派工单，这个只有在特定场景才会触发
                    //当工单为返工时，工艺路线中所有的工序不管有没有勾上派工点，都要自动生成任务单；
                    if (isProcessPty == true && workOrder.Type != WorkOrderType.Rework)
                    {
                        var processPty = pps.FirstOrDefault(f => f.ProcessId == p.ProcessId);
                        if (processPty == null)
                            continue;
                        //工序属性中派工点为是且排程点为否的工序任务单，才能生成派工单
                        if (!(processPty.DispatchWork == true && processPty.Scheduling == false))
                            continue;
                        //当工艺路线的控制码为PP04时，不用生成任务单，不管工序属性有没有勾选派工点；
                        if (info.Steus == "PP04")
                            continue;
                        //1.当工序为非PP01，只要勾上了工序属性中的派工点，都要生成任务单；
                        if (info.Steus != "PP01" && processPty.DispatchWork == true)
                        {

                        }
                        //2.当工序为PP01，且是首工序，且勾上了派工点，就要生成任务单；
                        else if (info.Steus == "PP01" && firstRoutingProcess.Id == p.Id && processPty.DispatchWork == true)
                        {

                        }
                        //3.当工序为PP01且非首工序，则必须勾上派工点和非首工序生成任务单，才生成任务单
                        else if (info.Steus == "PP01" && firstRoutingProcess.Id != p.Id && processPty.DispatchWork == true && processPty.IsUnFirstGenerateTask == true)
                        { }
                        else
                            continue;
                    }
                }

                var taskBomList = new EntityList<TaskProcessBom>();
                workOrder.ProcessBomList.Where(f => f.ProcessId == p.ProcessId).ForEach(f =>
                {
                    TaskProcessBom taskProcessBom = new TaskProcessBom();
                    taskProcessBom.ItemId = f.ItemId;
                    taskProcessBom.Qty = f.SingleQty;
                    taskProcessBom.ProcessId = f.ProcessId.Value;
                    taskBomList.Add(taskProcessBom);
                });
                if (familyConfig.ByQty)
                {
                    GenerateByQty(workOrder, taskConfig, reportMode, familyConfig, specificationDetail, taskList, isSyntypeReport, p.ProcessId, p.Index, taskBomList);
                }
                else
                {
                    if (familyConfig.BySpecification && specificationDetail != null && specificationDetail.Count > 0)
                    {
                        GenerateBySpecification(workOrder, taskConfig, reportMode, familyConfig, specificationDetail, taskList, workOrder.PlanQty, isSyntypeReport, p.ProcessId, p.Index, taskBomList);
                    }
                    else
                    {
                        EntityList<TaskProcessBom> boms = new EntityList<TaskProcessBom>();
                        boms.Clone(taskBomList, new CloneOptions(CloneActions.IdProperty | CloneActions.NormalProperties | CloneActions.RefEntities));
                        var task = InitDispatchTask(workOrder, taskConfig, reportMode, isSyntypeReport, true);
                        task.ProcessId = p.ProcessId;
                        task.RoutingProcessId = p.Id;
                        //当工单主表工厂和当前库存组织不同就认为是委外任务单
                        if (workOrder.Factory.Code != invOrg.ExternalId)
                            task.IsOutsourcing = true;
                        task.Seq = p.Index;
                        task.Boms.AddRange(boms);
                        var processPty = pps.FirstOrDefault(f => f.ProcessId == p.ProcessId);
                        if (processPty != null && processPty.IsPbcd == true)
                        {
                            var itemIds = task.Boms.Select(p => p.ItemId).Distinct().ToList();
                            var requireQty = task.WorkOrder.BomList.Where(p => itemIds.Contains(p.ItemId) && p.IsVritualItem == false).Sum(p => p.RequireQty);
                            task.DispatchQty = requireQty;// Math.Floor(task.Boms.Sum(p => p.Qty) /** workOrder.PlanQty*/);
                        }
                        else
                            task.DispatchQty = workOrder.PlanQty;


                        var standardHourSet = Query<StandardHourSet>().Where(m => m.ProcessId == p.ProcessId && m.WipResourceId == task.ResourceId && m.ProductId == task.ProductId).FirstOrDefault();
                        ComputTime(task, standardHourSet);
                        taskList.Add(task);
                    }
                }
            }
        }
        /// <summary>
        /// 计算时间
        /// </summary>
        /// <param name="task"></param>
        /// <param name="standardHourSet"></param>
        public virtual void ComputTime(DispatchTask task, StandardHourSet standardHourSet)
        {
            if (standardHourSet != null)
            {
                if (standardHourSet.StandardMin != 0)
                {
                    var processHourSum = (!standardHourSet.AttachMin.HasValue || standardHourSet.AttachMin == 0) ? standardHourSet.StandardMin : standardHourSet.AttachMin;
                    task.ProcessHourSum = processHourSum;
                    task.ProcessStandardHour = standardHourSet.StandardMin;
                    task.ExpectedProductionTime = ((task.DispatchQty * task.ProcessStandardHour) - standardHourSet.StandardMin + processHourSum) / 60;
                }
                else
                {
                    task.ProcessHourSum = 0;
                    task.ProcessStandardHour = 0;
                    task.ExpectedProductionTime = 0;

                }
            }
        }

        /// <summary>
        /// 按数量生成任务单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="taskConfig">派工任务单配置值</param>
        /// <param name="reportMode">报工方式</param>
        /// <param name="familyConfig">产品族任务单配置项</param>
        /// <param name="specificationDetail">产品规格件清单</param>
        /// <param name="taskList">要保存的任务单列表</param>     
        /// <param name="processId">工序Id</param>
        /// <param name="index">任务顺序索引</param>
        /// <param name="taskBomList">任务工序BOM</param>
        private void GenerateByQty(WorkOrder workOrder, DispatchTaskConfigValue taskConfig, ReportMode reportMode, TaskConfig familyConfig, EntityList<ProductSpecificationDetail> specificationDetail, EntityList<DispatchTask> taskList, bool isSyntypeReport, double? processId = 0, int index = 0, EntityList<TaskProcessBom> taskBomList = null)
        {
            var planQty = workOrder.PlanQty;
            var setQty = familyConfig.Qty;
            if (setQty <= 0)
            {
                throw new ValidationException("产品族配置项固定数量必须大于0".L10N());
            }
            int length = (int)Math.Ceiling(planQty / setQty);
            List<string> taskNos = null;
            for (int i = 1; i <= length; i++)
            {
                if (i == length)
                {
                    setQty = planQty - (length - 1) * setQty;
                }

                if (familyConfig.BySpecification && specificationDetail != null && specificationDetail.Count > 0)
                {
                    GenerateBySpecification(workOrder, taskConfig, reportMode, familyConfig, specificationDetail, taskList, setQty, isSyntypeReport, processId, index, taskBomList);
                }
                else
                {
                    if (taskNos == null)
                        taskNos = GetTaskNo(taskConfig, length);    //一次性生成多个单号，提高性能
                    var task = InitDispatchTask(workOrder, taskConfig, reportMode, isSyntypeReport, familyConfig.ByProcess, taskNos[i - 1]);
                    if (familyConfig.ByProcess)
                    {
                        task.ProcessId = processId;
                        task.Seq = index;
                        if (taskBomList != null)
                        {
                            EntityList<TaskProcessBom> boms = new EntityList<TaskProcessBom>();
                            boms.Clone(taskBomList, new CloneOptions(CloneActions.IdProperty | CloneActions.NormalProperties | CloneActions.RefEntities));
                            task.Boms.AddRange(boms);
                        }
                    }
                    task.DispatchQty = setQty;
                    taskList.Add(task);
                }
            }
        }

        /// <summary>
        /// 按规格件生成任务单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="taskConfig">派工任务单配置值</param>
        /// <param name="reportMode">报工方式</param>
        /// <param name="familyConfig">产品族任务单配置项</param>
        /// <param name="specificationDetail">产品规格件清单</param>
        /// <param name="taskList">要保存的任务单列表</param>
        /// <param name="planQty">计划数量</param>       
        /// <param name="processId">工序ID</param>
        /// <param name="index">任务顺序索引</param>
        /// <param name="taskBomList">任务工序BOM</param>
        private void GenerateBySpecification(WorkOrder workOrder, DispatchTaskConfigValue taskConfig, ReportMode reportMode, TaskConfig familyConfig,
            EntityList<ProductSpecificationDetail> specificationDetail, EntityList<DispatchTask> taskList, decimal planQty, bool isSyntypeReport, double? processId = 0, int index = 0, EntityList<TaskProcessBom> taskBomList = null)
        {
            specificationDetail.ForEach(e =>
            {
                var task = InitDispatchTask(workOrder, taskConfig, reportMode, isSyntypeReport, familyConfig.ByProcess);
                if (familyConfig.ByProcess && processId != 0)
                {
                    task.ProcessId = processId;
                    task.Seq = index;
                    if (taskBomList != null)
                    {
                        EntityList<TaskProcessBom> boms = new EntityList<TaskProcessBom>();
                        boms.Clone(taskBomList, new CloneOptions(CloneActions.IdProperty | CloneActions.NormalProperties | CloneActions.RefEntities));
                        task.Boms.AddRange(boms);
                    }
                }
                task.SingleQty = e.Qty;
                task.SpecificationId = e.SpecificationId;
                task.DispatchQty = planQty * e.Qty;
                taskList.Add(task);
            });
        }

        /// <summary>
        /// 生成任务单验证
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="isCommonGenerate">共模主工单生成任务单</param>
        /// <param name="isSyntypeReport">是否工模比报工</param>
        private Tuple<TaskConfig, ReportRuleConfig> ValidationWorkOrder(WorkOrder workOrder, bool isCommonGenerate, bool isSyntypeReport)
        {

            if (workOrder == null || workOrder.State == Core.WorkOrders.WorkOrderState.Finish || workOrder.State == Core.WorkOrders.WorkOrderState.Close)
            {
                throw new ValidationException("工单[{0}]完工、关闭状态无法创建派工任务单".L10nFormat(workOrder.No));
            }

            if (workOrder.IsCommonMode && !workOrder.IsMainMaterial && isSyntypeReport && !isCommonGenerate)
            {
                throw new ValidationException("共模比报工的共模辅料工单不允许手动生成任务单".L10N());
            }
            var productFamilyId = workOrder.Product.ProductFamilyId;
            if (!productFamilyId.HasValue)
            {
                throw new ValidationException("产品[{0}]未维护产品族".L10nFormat(workOrder.Product.Code));
            }
            var familyConfig = RT.Service.Resolve<TaskConfigController>().GetTaskConfig(productFamilyId.Value);
            if (familyConfig == null)
            {
                throw new ValidationException("产品[{0}]关联产品族未配置任务单生成规则".L10nFormat(workOrder.Product.Code));
            }
            var reportConfig = RT.Service.Resolve<ReportController>().GetReportRuleConfig(productFamilyId.Value);
            if (reportConfig == null)
                throw new ValidationException("产品[{0}]关联产品族未配置报工规则".L10nFormat(workOrder.Product.Code));
            if (workOrder.IsCommonMode)
            {
                familyConfig.ByVirtualPart = false;
                familyConfig.BySpecification = false;   //共模不考虑虚拟件 
                                                        //共模比报工且通过主单生成任务单，辅单不考虑数量
                familyConfig.ByQty = !(!workOrder.IsMainMaterial && isSyntypeReport && isCommonGenerate) && familyConfig.ByQty;
            }
            return new Tuple<TaskConfig, ReportRuleConfig>(familyConfig, reportConfig);

            //if (workOrder != null && workOrder.State == Core.WorkOrders.WorkOrderState.Release)
            //{
            //    if (workOrder.IsCommonMode && !workOrder.IsMainMaterial && isSyntypeReport && !isCommonGenerate)
            //    {
            //        throw new ValidationException("共模比报工的共模辅料工单不允许手动生成任务单".L10N());
            //    }
            //    var productFamilyId = workOrder.Product.ProductFamilyId;
            //    if (!productFamilyId.HasValue)
            //    {
            //        throw new ValidationException("产品[{0}]未维护产品族".L10nFormat(workOrder.Product.Code));
            //    }
            //    var familyConfig = RT.Service.Resolve<TaskConfigController>().GetTaskConfig(productFamilyId.Value);
            //    if (familyConfig == null)
            //    {
            //        throw new ValidationException("产品[{0}]关联产品族未配置任务单生成规则".L10nFormat(workOrder.Product.Code));
            //    }
            //    var reportConfig = RT.Service.Resolve<ReportController>().GetReportRuleConfig(productFamilyId.Value);
            //    if (reportConfig == null)
            //        throw new ValidationException("产品[{0}]关联产品族未配置报工规则".L10nFormat(workOrder.Product.Code));
            //    if (workOrder.IsCommonMode)
            //    {
            //        familyConfig.ByVirtualPart = false;
            //        familyConfig.BySpecification = false;   //共模不考虑虚拟件 
            //                                                //共模比报工且通过主单生成任务单，辅单不考虑数量
            //        familyConfig.ByQty = !(!workOrder.IsMainMaterial && isSyntypeReport && isCommonGenerate) && familyConfig.ByQty;
            //    }
            //    return new Tuple<TaskConfig, ReportRuleConfig>(familyConfig, reportConfig);
            //}
            //else
            //{
            //    throw new ValidationException("工单非发放状态".L10N());
            //}
        }

        /// <summary>
        /// 生成任务单（公共部分）
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="taskConfig">任务单配置</param>
        /// <param name="reportMode">报工方式</param>
        /// <param name="isSyntypeReport">是否共模报工</param>
        /// <param name="isProcess">是否工序任务</param>
        /// <param name="taskNo">任务单号</param>
        /// <returns>任务单</returns>
        private DispatchTask InitDispatchTask(WorkOrder workOrder, DispatchTaskConfigValue taskConfig, ReportMode reportMode, bool isSyntypeReport, bool isProcess = false, string taskNo = "")
        {
            var no = taskNo.IsNullOrEmpty() ? GetTaskNo(taskConfig) : taskNo;
            var task = new DispatchTask()
            {
                No = no,
                SendQty = 0,
                ReportQty = 0,
                OkQty = 0,
                NgQty = 0,
                PlanBeginTime = workOrder.PlanBeginDate,
                PlanEndTime = workOrder.PlanEndDate,
                AssociatedWorkOrder = workOrder.No,
                IsVirtualPart = false,
                IsSyntype = workOrder.IsCommonMode,
                TechNo = workOrder.ProcessTechOrderCode,
                Proportion = workOrder.Proportion,
                ReportMode = reportMode,
                Priority = DispatchTaskPriority.Normal,
                TaskStatus = DispatchTaskStatus.ToDispatch,
                ProductId = workOrder.ProductId,
                WorkOrderId = workOrder.Id,
                WorkOrder = workOrder,
                ResourceId = workOrder.ResourceId,
                WorkShopId = workOrder.WorkShopId,
                MergedStatus = MergedStatus.Normal,
                IsMainTask = true,
                IsSyntypeReport = workOrder.IsCommonMode && isSyntypeReport
            };
            if (workOrder.IsCommonMode)
            {
                task.Associated = Associated.Syntype;
                task.IsMainTask = !isSyntypeReport || workOrder.IsMainMaterial;
            }
            task.GenerateId();

            if (!isProcess)
            {
                workOrder.ProcessBomList.Where(p => p.ProcessId.HasValue).ForEach(f =>
                {
                    TaskProcessBom bom = new TaskProcessBom()
                    {
                        DispatchTaskId = task.Id,
                        ItemId = f.ItemId,
                        Qty = f.SingleQty,
                        UseQty = 0,
                        ProcessId = f.ProcessId.Value
                    };
                    task.Boms.Add(bom);
                });
            }
            return task;
        }
        #endregion

        #region 选择执行对象 

        /// <summary>
        /// 获取可执行对象数据
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <param name="dispatchTaskId">当前任务单Id</param>
        /// <param name="adoType">可执行对象类型</param>
        /// <param name="adoName">可执行对象名称</param>
        /// <returns>可执行对象数据</returns>
        public virtual TaskPerformerInfo GetTaskPerformerInfo(List<double> dispatchTaskIds, double dispatchTaskId, string adoType, string adoName)
        {
            var dispatchTask = GetDispatchTask(dispatchTaskId);
            if (dispatchTask == null)
                throw new ValidationException("当前任务单已被取消，请重新查询数据后再操作！".L10N());
            TaskPerformerInfo taskPerformerInfo = new TaskPerformerInfo();
            taskPerformerInfo.IsSelectedTaskPerformer = IsSelectedTaskPerformer(dispatchTask, dispatchTaskIds);
            var selectedAdoInfos = GetSelectedAdoInfoList(dispatchTask);
            taskPerformerInfo.SelectedAdoInfos.AddRange(selectedAdoInfos);
            var dicAdoInfos = selectedAdoInfos.GroupBy(p => p.AdoType).Distinct().ToDictionary(p => p.Key, p => p.ToList());
            if (adoType == AdoGroup.WorkGroup.ToLabel())
            {
                if (adoName.Length == 0)
                    GetWorkGroupInfos(dispatchTask, adoType, taskPerformerInfo, dicAdoInfos);
                else
                    GetEmployeeInfosOfWorkGroup(dispatchTask, adoName, AdoGroup.WorkGroup, taskPerformerInfo, dicAdoInfos);

            }
            else if (adoType == AdoGroup.EmployeeGroup.ToLabel())
            {
                if (adoName.Length == 0)
                    GetEmployeeGroupInfos(dispatchTask, adoType, taskPerformerInfo, dicAdoInfos);
                else
                    GetEmployeesInfosOfEmployeeGroup(dispatchTask, adoName, AdoGroup.EmployeeGroup, taskPerformerInfo, dicAdoInfos);
            }
            else if (adoType == AdoGroup.Station.ToLabel() && dispatchTask.ProcessId > 0 && dispatchTaskIds.Count == 1)
            {
                if (adoName.Length == 0)
                    GetStationGroupInfos(dispatchTask, adoType, taskPerformerInfo, dicAdoInfos);
                else
                    GetEmployeeInfosOfStationGroup(dispatchTask, adoName, AdoGroup.Station, taskPerformerInfo, dicAdoInfos);
            }

            foreach (var item in taskPerformerInfo.AdoInfos)
            {
                item.AdoType = item.AdoType.L10N();
            }

            return taskPerformerInfo;
        }

        /// <summary>
        /// 获取员工组的员工对象信息
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoName">可执行对象名称</param>
        /// <param name="adoGroup">组类</param>
        /// <param name="taskPerformerInfo">可执行对象数据</param>
        /// <param name="dicAdoInfos">已存在的对象信息字典</param>
        private void GetEmployeesInfosOfEmployeeGroup(DispatchTask dispatchTask, string adoName, AdoGroup adoGroup, TaskPerformerInfo taskPerformerInfo, Dictionary<string, List<AdoInfo>> dicAdoInfos)
        {
            var employeesOfEmployee = GetEmployeesByEmployeeGroup(adoName);
            GetEmployeeInfos(dispatchTask, adoGroup, taskPerformerInfo, dicAdoInfos, employeesOfEmployee);
        }

        /// <summary>
        /// 获取员工组对象信息
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoType">可执行对象类型</param>
        /// <param name="taskPerformerInfo">可执行对象数据</param>
        /// <param name="dicAdoInfos">已存在的对象信息字典</param>
        private void GetEmployeeGroupInfos(DispatchTask dispatchTask, string adoType, TaskPerformerInfo taskPerformerInfo, Dictionary<string, List<AdoInfo>> dicAdoInfos)
        {
            List<AdoInfo> adoInfos = null;
            dicAdoInfos.TryGetValue(adoType, out adoInfos);
            if (adoInfos == null)
                adoInfos = new List<AdoInfo>();
            var dicAdoInfosOfEmployeeGroup = adoInfos.ToDictionary(p => p.AdoId);
            var allEmployeeGroups = RT.Service.Resolve<EmployeeController>().GetEmployeeGroupList();
            var employeeGroupIds = allEmployeeGroups.Select(p => p.Id).Distinct().ToList();
            var groupSendQtys = GetGroupSendQtys(employeeGroupIds, AdoType.EmployeeGroup);
            if (allEmployeeGroups.Count > 0)
                taskPerformerInfo.ShiftEmployeeInfos.Add(new ShiftEmployeeInfo() { AdoName = "", AdoValue = "" });
            allEmployeeGroups.ForEach(p =>
            {
                taskPerformerInfo.ShiftEmployeeInfos.Add(CreateShiftEmployeeInfo(p.Name));
            });

            foreach (var employeeGroup in allEmployeeGroups)
            {
                if (dicAdoInfosOfEmployeeGroup.ContainsKey(employeeGroup.Id))
                    continue;
                AdoInfo adoInfo = CreateAdoInfo(dispatchTask, employeeGroup.Id, employeeGroup.Name, adoType, null);
                if (groupSendQtys.TryGetValue(employeeGroup.Id, out int sendQty))
                    adoInfo.SendQty = sendQty;
                taskPerformerInfo.AdoInfos.Add(adoInfo);
            }
        }

        /// <summary>
        /// 获取班组的员工对象信息
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoName">可执行对象名称</param>
        /// <param name="adoGroup">组类</param>
        /// <param name="taskPerformerInfo">可执行对象数据</param>
        /// <param name="dicAdoInfos">已存在的对象信息字典</param>
        private void GetEmployeeInfosOfWorkGroup(DispatchTask dispatchTask, string adoName, AdoGroup adoGroup, TaskPerformerInfo taskPerformerInfo, Dictionary<string, List<AdoInfo>> dicAdoInfos)
        {
            var employeesOfWorkGroup = GetEmployeesByWorkGroup(adoName);
            GetEmployeeInfos(dispatchTask, adoGroup, taskPerformerInfo, dicAdoInfos, employeesOfWorkGroup);
        }

        /// <summary>
        /// 获取工位的员工对象信息
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoName">可执行对象名称</param>
        /// <param name="adoGroup">组类</param>
        /// <param name="taskPerformerInfo">可执行对象数据</param>
        /// <param name="dicAdoInfos">已存在的对象信息字典</param>
        private void GetEmployeeInfosOfStationGroup(DispatchTask dispatchTask, string adoName, AdoGroup adoGroup, TaskPerformerInfo taskPerformerInfo, Dictionary<string, List<AdoInfo>> dicAdoInfos)
        {
            var employeesOfStationGroup = GetEmployeesByStationGroup(adoName, dispatchTask.ProcessId);
            GetEmployeeInfos(dispatchTask, adoGroup, taskPerformerInfo, dicAdoInfos, employeesOfStationGroup);
        }

        /// <summary>
        /// 获取员工对象信息
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoGroup">组类</param>
        /// <param name="taskPerformerInfo">可执行对象数据</param>
        /// <param name="dicAdoInfos">已存在的对象信息字典</param>
        /// <param name="employees">员工列表</param>
        private void GetEmployeeInfos(DispatchTask dispatchTask, AdoGroup adoGroup, TaskPerformerInfo taskPerformerInfo, Dictionary<string, List<AdoInfo>> dicAdoInfos, EntityList<Employee> employees)
        {
            List<AdoInfo> adoInfos = null;
            dicAdoInfos.TryGetValue(AdoType.Employee.ToLabel(), out adoInfos);
            if (adoInfos == null)
                adoInfos = new List<AdoInfo>();
            var dicAdoInfosOfEmployee = adoInfos.ToDictionary(p => p.AdoId);
            var employeesIds = employees.Select(p => p.Id).Distinct().ToList();
            var taskDetails = GetDispatchTaskDetailsByAdoType(employeesIds, AdoType.Employee);
            var dicTaskDetails = taskDetails.GroupBy(p => p.AdoId).ToDictionary(p => p.Key, p => p.ToList());
            if (employees.Count > 0)
                taskPerformerInfo.ShiftEmployeeInfos.Add(new ShiftEmployeeInfo() { AdoName = "", AdoValue = "" });
            employees.ForEach(p =>
            {
                taskPerformerInfo.ShiftEmployeeInfos.Add(CreateShiftEmployeeInfo(p.Name));
            });

            foreach (var employee in employees)
            {
                if (dicAdoInfosOfEmployee.ContainsKey(employee.Id))
                    continue;
                AdoInfo adoInfo = CreateAdoInfo(dispatchTask, employee.Id, employee.Name, AdoType.Employee.ToLabel(), adoGroup);
                if (dicTaskDetails.TryGetValue(employee.Id, out List<DispatchTaskDetail> tmpTaskDetails))
                    adoInfo.SendQty = tmpTaskDetails.Select(p => p.DispatchTaskId).Distinct().Count();
                taskPerformerInfo.AdoInfos.Add(adoInfo);
            }
        }

        /// <summary>
        /// 获取班组对象信息
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoType">可执行对象类型</param>
        /// <param name="taskPerformerInfo">可执行对象数据</param>
        /// <param name="dicAdoInfos">已存在的对象信息字典</param>
        private void GetWorkGroupInfos(DispatchTask dispatchTask, string adoType, TaskPerformerInfo taskPerformerInfo, Dictionary<string, List<AdoInfo>> dicAdoInfos)
        {
            List<AdoInfo> adoInfos = null;
            dicAdoInfos.TryGetValue(adoType, out adoInfos);
            if (adoInfos == null)
                adoInfos = new List<AdoInfo>();
            var dicAdoInfosOfWorkGroup = adoInfos.ToDictionary(p => p.AdoId);

            var allWorkGroups = RT.Service.Resolve<EmployeeController>().GetWorkGroupsList();
            var workGroupIds = allWorkGroups.Select(p => p.Id).Distinct().ToList();
            var groupSendQtys = GetGroupSendQtys(workGroupIds, AdoType.WorkGroup);
            if (allWorkGroups.Count > 0)
                taskPerformerInfo.ShiftEmployeeInfos.Add(new ShiftEmployeeInfo() { AdoName = "", AdoValue = "" });
            allWorkGroups.ForEach(p =>
            {
                taskPerformerInfo.ShiftEmployeeInfos.Add(CreateShiftEmployeeInfo(p.Name));
            });

            foreach (var workGroup in allWorkGroups)
            {
                if (dicAdoInfosOfWorkGroup.ContainsKey(workGroup.Id))
                    continue;
                AdoInfo adoInfo = CreateAdoInfo(dispatchTask, workGroup.Id, workGroup.Name, adoType, null);
                if (groupSendQtys.TryGetValue(workGroup.Id, out int sendQty))
                    adoInfo.SendQty = sendQty;
                taskPerformerInfo.AdoInfos.Add(adoInfo);
            }
        }

        /// <summary>
        /// 获取工位对象信息
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="adoType"></param>
        /// <param name="taskPerformerInfo"></param>
        /// <param name="dicAdoInfos"></param>
        private void GetStationGroupInfos(DispatchTask dispatchTask, string adoType, TaskPerformerInfo taskPerformerInfo, Dictionary<string, List<AdoInfo>> dicAdoInfos)
        {
            if (dispatchTask.ProcessId == null || dispatchTask.ProcessId == 0)
                return;
            List<AdoInfo> adoInfos = null;
            dicAdoInfos.TryGetValue(adoType, out adoInfos);
            if (adoInfos == null)
                adoInfos = new List<AdoInfo>();
            var dicAdoInfosOfStation = adoInfos.ToDictionary(p => p.AdoId);

            var allStations = RT.Service.Resolve<StationController>().GetStationProcessByProcessId(dispatchTask.ProcessId.Value, new EagerLoadOptions().LoadWith(StationProcess.StationProperty))
                .Select(p => p.Station).DistinctBy(p => p.Id).ToList();
            var stationIds = allStations.Select(p => p.Id).Distinct().ToList();
            var groupSendQtys = GetGroupSendQtys(stationIds, AdoType.Station);
            if (allStations.Count > 0)
                taskPerformerInfo.ShiftEmployeeInfos.Add(new ShiftEmployeeInfo() { AdoName = "", AdoValue = "" });
            allStations.ForEach(p =>
            {
                taskPerformerInfo.ShiftEmployeeInfos.Add(CreateShiftEmployeeInfo(p.Name));
            });

            foreach (var station in allStations)
            {
                if (dicAdoInfosOfStation.ContainsKey(station.Id))
                    continue;
                AdoInfo adoInfo = CreateAdoInfo(dispatchTask, station.Id, station.Name, adoType, null);
                if (groupSendQtys.TryGetValue(station.Id, out int sendQty))
                    adoInfo.SendQty = sendQty;
                taskPerformerInfo.AdoInfos.Add(adoInfo);
            }
        }

        /// <summary>
        /// 创建班组/员工组/员工对象
        /// </summary>
        /// <param name="name">班组员工组员工名称</param>
        /// <returns>班组/员工组/员工对象</returns>
        private ShiftEmployeeInfo CreateShiftEmployeeInfo(string name)
        {
            return new ShiftEmployeeInfo()
            {
                AdoName = name,
                AdoValue = name
            };
        }

        /// <summary>
        /// 获取已选中的对象信息列表
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <returns>已选中的对象信息列表</returns>
        private List<AdoInfo> GetSelectedAdoInfoList(DispatchTask dispatchTask)
        {
            List<AdoInfo> selectedAdoInfoList = new List<AdoInfo>();
            var dispatchTaskDetails = GetDispatchTaskDetails(dispatchTask.Id);
            var dicDispatchTaskDetails = dispatchTaskDetails.GroupBy(p => p.AdoType).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var dicDispatchTaskDetail in dicDispatchTaskDetails)
            {
                var taskDetails = dicDispatchTaskDetail.Value;
                var adoIds = taskDetails.Select(p => p.AdoId).Distinct().ToList();
                if (dicDispatchTaskDetail.Key == AdoType.WorkGroup)
                {
                    var reportRecordByAdos = GetReportRecordsByWorkGroupIds(adoIds);
                    var dicReportRecordByAdos = reportRecordByAdos.Where(p => p.Principal.WorkGroup != null).GroupBy(p => p.Principal.WorkGroupId.Value).Select(p => new WorkEmployeeGroup { WorkEmployeeId = p.Key, SendQty = p.Count() }).ToDictionary(p => p.WorkEmployeeId);

                    selectedAdoInfoList.AddRange(GetSelectedAdoInfos(AdoType.WorkGroup.ToLabel(), dispatchTask, taskDetails, dicReportRecordByAdos));
                }
                else if (dicDispatchTaskDetail.Key == AdoType.EmployeeGroup)
                {
                    var reportRecordByAdos = GetReportRecordsByEmployeeGroupIds(adoIds);
                    var dicReportRecordByAdos = reportRecordByAdos.Where(p => p.Principal.EmployeeGroup != null).GroupBy(p => p.Principal.EmployeeGroupId.Value).Select(p => new WorkEmployeeGroup { WorkEmployeeId = p.Key, SendQty = p.Count() }).ToDictionary(p => p.WorkEmployeeId);
                    selectedAdoInfoList.AddRange(GetSelectedAdoInfos(AdoType.EmployeeGroup.ToLabel(), dispatchTask, taskDetails, dicReportRecordByAdos));
                }
                else if (dicDispatchTaskDetail.Key == AdoType.Employee)
                {
                    var reportRecordByAdos = GetReportRecordsByEmployeeIds(adoIds);
                    var dicReportRecordByAdos = reportRecordByAdos.GroupBy(p => p.Principal.Id).Select(p => new WorkEmployeeGroup { WorkEmployeeId = p.Key, SendQty = p.Count() }).ToDictionary(p => p.WorkEmployeeId);
                    selectedAdoInfoList.AddRange(GetSelectedAdoInfos(AdoType.Employee.ToLabel(), dispatchTask, taskDetails, dicReportRecordByAdos));
                }
                else if (dicDispatchTaskDetail.Key == AdoType.Station)
                {
                    var reportRecordByAdos = GetReportRecordsByStationIds(adoIds);
                    var dicReportRecordByAdos = reportRecordByAdos.Where(p => p.StationId != null).GroupBy(p => p.StationId.Value).Select(p => new WorkEmployeeGroup { WorkEmployeeId = p.Key, SendQty = p.Count() }).ToDictionary(p => p.WorkEmployeeId);
                    selectedAdoInfoList.AddRange(GetSelectedAdoInfos(AdoType.Station.ToLabel(), dispatchTask, taskDetails, dicReportRecordByAdos));

                }
            }

            return selectedAdoInfoList;
        }

        /// <summary>
        /// 获取已选中的对象信息列表
        /// </summary>
        /// <param name="adoType">可执行对象类型</param>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="taskDetails">任务单明细列表</param>
        /// <param name="dicReportRecordByAdos">班组员工组员工字典</param>
        /// <returns>已选中的对象信息列表</returns>
        private List<AdoInfo> GetSelectedAdoInfos(string adoType, DispatchTask dispatchTask, List<DispatchTaskDetail> taskDetails, Dictionary<double, WorkEmployeeGroup> dicReportRecordByAdos)
        {
            List<AdoInfo> selectedAdoInfos = new List<AdoInfo>();
            taskDetails.ForEach(p =>
            {
                AdoInfo selectedAdoInfo = CreateAdoInfo(dispatchTask, p.AdoId, p.AdoName, adoType, p.AdoGroup);
                WorkEmployeeGroup workEmployeeGroup = null;
                if (dicReportRecordByAdos.TryGetValue(p.AdoId, out workEmployeeGroup))
                {
                    selectedAdoInfo.SendQty = workEmployeeGroup.SendQty;
                }

                selectedAdoInfos.Add(selectedAdoInfo);
            });

            return selectedAdoInfos;
        }

        /// <summary>
        /// 创建可执行对象
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoId">可执行对象Id</param>
        /// <param name="adoName">可执行对象名称</param>
        /// <param name="adoType">可执行对象类型</param>
        /// <param name="adoGroup">组类</param>
        /// <returns>可执行对象</returns>
        public virtual AdoInfo CreateAdoInfo(DispatchTask dispatchTask, double adoId, string adoName, string adoType, AdoGroup? adoGroup)
        {
            var adoInfo = new AdoInfo()
            {
                DispatchTaskId = dispatchTask.Id,
                TaskStatus = dispatchTask.TaskStatus,
                AdoId = adoId,
                AdoName = adoName,
                AdoType = adoType,
                AdoGroup = null,
                SendQty = 0
            };

            if (adoGroup != null)
            {
                adoInfo.AdoGroup = EnumViewModel.EnumToLabel(adoGroup).L10N();
            }

            return adoInfo;
        }

        /// <summary>
        /// 通过班组名称获取对应的员工列表
        /// </summary>
        /// <param name="workGroupName">班组名称</param>
        /// <returns>员工列表</returns>
        private EntityList<Employee> GetEmployeesByWorkGroup(string workGroupName)
        {
            return Query<Employee>().Exists<WorkGroup>((a, b) => b.Where(p => p.Id == a.WorkGroupId && p.Name.Contains(workGroupName))).ToList();
        }

        /// <summary>
        /// 通过员工组名称获取员工列表
        /// </summary>
        /// <param name="employeeGroupName">员工组名称</param>
        /// <returns>员工列表</returns>
        private EntityList<Employee> GetEmployeesByEmployeeGroup(string employeeGroupName)
        {
            return Query<Employee>().Exists<EmployeeGroup>((a, b) => b.Where(p => p.Id == a.EmployeeGroupId && p.Name.Contains(employeeGroupName))).ToList();
        }

        /// <summary>
        /// 通过工位Ids获取员工列表
        /// </summary>
        /// <param name="stationIds">工位Ids</param>
        /// <returns>员工列表</returns>
        private EntityList<Employee> GetEmployeesByStationIds(List<double> stationIds)
        {
            return Query<Employee>()
                .Join<ProcessEmployee>((a, b) => b.EmployeeId == a.Id)
                .Join<ProcessEmployee, StationProcess>((c, d) => c.ProcessId == d.ProcessId)
                .Join<StationProcess, Station>((e, f) => e.StationId == f.Id)
                .Where<Station>((x, p) => stationIds.Contains(p.Id))
                .Distinct()
            .ToList();
        }

        /// <summary>
        /// 通过工位名称获取员工列表
        /// </summary>
        /// <param name="stationName">工位名称</param>
        /// <param name="processId">工位名称</param>
        /// <returns>员工列表</returns>
        private EntityList<Employee> GetEmployeesByStationGroup(string stationName, double? processId)
        {
            return Query<Employee>()
                .Join<ProcessEmployee>((a, b) => b.EmployeeId == a.Id && b.ProcessId == processId)
                .Join<ProcessEmployee, StationProcess>((c, d) => c.ProcessId == d.ProcessId)
                .Join<StationProcess, Station>((e, f) => e.StationId == f.Id)
                .Where<Station>((x, p) => p.Name.Contains(stationName))
                .Distinct()
            .ToList();
        }

        /// <summary>
        /// 通过班组Id列表获取报工列表
        /// </summary>
        /// <param name="workGroupIds">班组Id列表</param>
        /// <returns>报工列表</returns>
        private EntityList<ReportRecord> GetReportRecordsByWorkGroupIds(List<double> workGroupIds)
        {
            return Query<ReportRecord>().Exists<Employee>(
                 (x, y) => y.Join<WorkGroup>((c, d) =>
                         c.WorkGroupId == d.Id && workGroupIds.Contains(d.Id))
                     .Where(p => p.Id == x.PrincipalId)).ToList(null, new EagerLoadOptions().LoadWith(ReportRecord.PrincipalProperty));
        }

        /// <summary>
        /// 通过员工组Id列表获取报工列表
        /// </summary>
        /// <param name="employeeGroupIds">员工组Id列表</param>
        /// <returns>报工列表</returns>
        private EntityList<ReportRecord> GetReportRecordsByEmployeeGroupIds(List<double> employeeGroupIds)
        {
            return Query<ReportRecord>().Exists<Employee>(
                 (x, y) => y.Join<EmployeeGroup>((c, d) =>
                         c.WorkGroupId == d.Id && employeeGroupIds.Contains(d.Id))
                     .Where(p => p.Id == x.PrincipalId)).ToList(null, new EagerLoadOptions().LoadWith(ReportRecord.PrincipalProperty));
        }

        /// <summary>
        /// 通过工位Id列表获取报工列表
        /// </summary>
        /// <param name="stationIds">员工组Id列表</param>
        /// <returns>报工列表</returns>
        private EntityList<ReportRecord> GetReportRecordsByStationIds(List<double> stationIds)
        {
            return Query<ReportRecord>().Where(p => stationIds.Cast<double?>().ToList().Contains(p.StationId))
                .ToList(null, new EagerLoadOptions().LoadWith(ReportRecord.PrincipalProperty));
        }

        /// <summary>
        /// 通过员工Id列表获取报工列表
        /// </summary>
        /// <param name="employeeIds">员工Id列表</param>
        /// <returns>报工列表</returns>
        private EntityList<ReportRecord> GetReportRecordsByEmployeeIds(List<double> employeeIds)
        {
            return Query<ReportRecord>().Exists<Employee>((a, b) => b.Where(p => p.Id == a.WorkGroupId && employeeIds.Contains(p.Id))).ToList(null, new EagerLoadOptions().LoadWith(ReportRecord.PrincipalProperty));
        }

        /// <summary>
        /// 通过派工任务id获取派工任务明细列表
        /// </summary>
        /// <param name="dispatchTaskId">派工任务id</param>
        /// <returns>派工任务明细列表</returns>
        public virtual EntityList<DispatchTaskDetail> GetDispatchTaskDetails(double dispatchTaskId)
        {
            return Query<DispatchTaskDetail>().Where(p => p.DispatchTaskId == dispatchTaskId).ToList();
        }

        /// <summary>
        /// 通过派工任务id列表获取派工任务明细列表
        /// </summary>
        /// <param name="dispatchTaskIds">派工任务id列表</param>
        /// <returns>派工任务明细列表</returns>
        public virtual EntityList<DispatchTaskDetail> GetDispatchTaskDetails(List<double> dispatchTaskIds)
        {
            return dispatchTaskIds.SplitContains((tempTaskIds) =>
            {
                return Query<DispatchTaskDetail>().Where(p => tempTaskIds.Contains(p.DispatchTaskId)).ToList();
            });
        }

        /// <summary>
        /// 根据对象Id列表和对象类型获取派工任务明细列表
        /// </summary>
        /// <param name="adoIds">对象Id列表</param>
        /// <param name="adoType">对象类型</param>
        /// <returns>派工任务明细列表</returns>
        public virtual EntityList<DispatchTaskDetail> GetDispatchTaskDetailsByAdoType(List<double> adoIds, AdoType adoType)
        {
            return Query<DispatchTaskDetail>().Exists<DispatchTask>((a, b) => b.Where(f => f.Id == a.DispatchTaskId && (f.TaskStatus == DispatchTaskStatus.Dispatched || f.TaskStatus == DispatchTaskStatus.Executing) && a.AdoType == adoType && adoIds.Contains(a.AdoId))).ToList();
        }

        /// <summary>
        /// 批量保存可选对象
        /// </summary>
        /// <param name="adoInfo">对象信息</param>
        public virtual string SaveTaskPerformer(AdoInfo adoInfo)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTaskList(adoInfo.SelectedTaskIds);
                var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(adoInfo.SelectedTaskIds);
                var dispatchTaskDetails = GetDispatchTaskDetails(adoInfo.SelectedTaskIds);
                var dicTaskDetails = dispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    foreach (var dispatchTask in dispatchTasks)
                    {
                        List<DispatchTaskDetail> taskDetails = null;
                        dicTaskDetails.TryGetValue(dispatchTask.Id, out taskDetails);
                        SaveTaskPerformer(dispatchTask, adoInfo, taskDetails);
                    }

                    EntityList<DispatchTask> relateDispatchTasks = new EntityList<DispatchTask>();
                    var dicOrgDispatchTasks = dispatchTasks.ToDictionary(p => p.Id);
                    foreach (var orgDispatchTask in dispatchTasks)
                    {
                        List<DispatchTask> notSyntypeDispatchTasks = null;
                        if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out notSyntypeDispatchTasks) && notSyntypeDispatchTasks.Any())
                        {
                            notSyntypeDispatchTasks.ForEach(p =>
                            {
                                p.TaskStatus = orgDispatchTask.TaskStatus;
                                p.PersistenceStatus = PersistenceStatus.Modified;
                            });
                            relateDispatchTasks.AddRange(notSyntypeDispatchTasks);
                        }

                        List<DispatchTask> syntypeDispatchTasks = null;
                        if (!associatedDispatchTaskInfo.DicSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out syntypeDispatchTasks))
                            continue;

                        foreach (var syntypeDispatchTask in syntypeDispatchTasks)
                        {
                            List<DispatchTask> syntypeMainDispatchTasks = null;
                            if (!associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.TryGetValue(syntypeDispatchTask.Id, out syntypeMainDispatchTasks))
                            {
                                continue;
                            }
                            syntypeMainDispatchTasks.ForEach(p =>
                            {
                                DispatchTask dispatchTask = null;
                                if (dicOrgDispatchTasks.TryGetValue(p.Id, out dispatchTask))
                                {
                                    p.TaskStatus = dispatchTask.TaskStatus;
                                }
                            });

                            ////syntypeMainDispatchTasks.ForEach(p =>
                            ////{
                            ////    if (p.TaskStatus == DispatchTaskStatus.Pause && (p.OldTaskStatus == DispatchTaskStatus.Dispatched || p.OldTaskStatus == DispatchTaskStatus.Executing))
                            ////        return;
                            ////});

                            if (syntypeMainDispatchTasks.Any(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched))
                            {
                                continue;
                            }

                            if (syntypeMainDispatchTasks.All(p => p.TaskStatus == DispatchTaskStatus.ToDispatch))
                            {
                                syntypeDispatchTask.TaskStatus = DispatchTaskStatus.ToDispatch;
                            }
                            else
                            {
                                syntypeDispatchTask.TaskStatus = DispatchTaskStatus.Dispatching;
                            }
                            syntypeDispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                            relateDispatchTasks.Add(syntypeDispatchTask);
                        }
                    }
                    RF.Save(relateDispatchTasks);
                    tran.Complete();
                }
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 保存可选对象
        /// </summary>
        /// <param name="dispatchTask">派工单</param>
        /// <param name="adoInfo">对象信息</param>
        /// <param name="taskDetails">任务单明细列表</param>
        private void SaveTaskPerformer(DispatchTask dispatchTask, AdoInfo adoInfo, List<DispatchTaskDetail> taskDetails)
        {
            DispatchTaskDetail dispatchTaskDetail = null;
            var adoType = (AdoType)EnumViewModel.LabelToEnum(adoInfo.AdoType, typeof(AdoType));
            dispatchTaskDetail = GetDispatchTaskDetail(dispatchTask, adoInfo, adoType);

            if (dispatchTaskDetail.PersistenceStatus == PersistenceStatus.Deleted && !taskDetails.Any(p => p.Id != dispatchTaskDetail.Id))
            {
                dispatchTask.TaskStatus = DispatchTaskStatus.ToDispatch;
                dispatchTask.PersistenceStatus = PersistenceStatus.Modified;
            }
            else if (dispatchTaskDetail.PersistenceStatus == PersistenceStatus.New || taskDetails.Any(p => p.Id != dispatchTaskDetail.Id))
            {
                dispatchTask.TaskStatus = DispatchTaskStatus.Dispatching;
                dispatchTask.PersistenceStatus = PersistenceStatus.Modified;
            }
            else
            {
                //
            }

            RF.Save(dispatchTask);
            RF.Save(dispatchTaskDetail);
        }

        /// <summary>
        /// 获取任务单明细
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoInfo">可选对象信息</param>
        /// <param name="adoType">可选对象类型</param>
        /// <returns>任务单明细</returns>
        private DispatchTaskDetail GetDispatchTaskDetail(DispatchTask dispatchTask, AdoInfo adoInfo, AdoType adoType)
        {
            DispatchTaskDetail dispatchTaskDetail;
            if (adoInfo.Status)
            {
                dispatchTaskDetail = GetDispatchTaskDetail(dispatchTask.Id, adoInfo.AdoId, adoType);
                dispatchTaskDetail.PersistenceStatus = PersistenceStatus.Deleted;
            }
            else
            {
                dispatchTaskDetail = CreateDispatchTaskDetail(dispatchTask, adoInfo, adoType);
            }

            return dispatchTaskDetail;
        }

        /// <summary>
        /// 创建任务单明细
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="adoInfo">可选对象信息</param>
        /// <param name="adoType">可选对象类型</param>
        /// <returns>任务单明细</returns>
        public virtual DispatchTaskDetail CreateDispatchTaskDetail(DispatchTask dispatchTask, AdoInfo adoInfo, AdoType adoType)
        {
            var taskDetail = new DispatchTaskDetail()
            {
                DispatchTaskId = dispatchTask.Id,
                AdoId = adoInfo.AdoId,
                AdoName = adoInfo.AdoName,
                AdoType = adoType,
                AdoGroup = (AdoGroup?)EnumViewModel.LabelToEnum(adoInfo.AdoGroup, typeof(AdoGroup)),
                ProcessMatchDegree = adoInfo.MatchDegree,
                PersistenceStatus = PersistenceStatus.New
            };

            taskDetail.GenerateId();
            return taskDetail;
        }

        /// <summary>
        /// 获取派工任务明细列表
        /// </summary>
        /// <param name="dispatchTaskId">派工任务Id</param>
        /// <param name="adoId">对象Id</param>
        /// <param name="adoType">对象类型</param>
        /// <returns>派工任务明细列表</returns>
        public virtual DispatchTaskDetail GetDispatchTaskDetail(double dispatchTaskId, double adoId, AdoType adoType)
        {
            return Query<DispatchTaskDetail>().Where(p => p.DispatchTaskId == dispatchTaskId && p.AdoId == adoId && p.AdoType == adoType).FirstOrDefault();
        }

        /// <summary>
        /// 选择两笔或者两笔以上的任务单，判断是否已选对象
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>true/false</returns>
        private bool IsSelectedTaskPerformer(DispatchTask dispatchTask, List<double> dispatchTaskIds)
        {
            if (!dispatchTaskIds.Contains(dispatchTask.Id))
                return true;
            if (dispatchTaskIds.Count > 1)
                return Query<DispatchTaskDetail>().Exists<DispatchTask>((a, b) => b.Where(p => p.Id == a.DispatchTaskId && dispatchTaskIds.Contains(p.Id))).Count() > 0;
            return false;
        }

        /// <summary>
        /// 选择两笔或者两笔以上的任务单，判断是否已选对象
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns></returns>
        public virtual bool IsSelectedTaskPerformer(List<double> dispatchTaskIds)
        {
            if (dispatchTaskIds.Count > 1)
                return Query<DispatchTaskDetail>().Exists<DispatchTask>((a, b) => b.Where(p => p.Id == a.DispatchTaskId && dispatchTaskIds.Contains(p.Id))).Count() > 0;
            return false;
        }
        #endregion

        #region 界面按钮功能
        /// <summary>
        /// 派工任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>派工结果</returns>
        public virtual string DispatchTasks(List<double> dispatchTaskIds)
        {
            StringBuilder errMsg = new StringBuilder();
            var processCt = RT.Service.Resolve<ProcessController>();
            var employeeCt = RT.Service.Resolve<EmployeeController>();
            var skillCt = RT.Service.Resolve<SkillController>();
            var allEmployeeIds = new List<double>();
            var toBeDispatchTasks = new EntityList<DispatchTask>();
            var emoloyees = new List<Employee>();
            var processEmployees = new EntityList<ProcessEmployee>();
            var employeeResources = new EntityList<EmployeeResource>();
            var employeeSkills = new EntityList<EmployeeSkill>();
            var dicSkillList = new Dictionary<double, List<Skill>>();
            var dicProcessList = new Dictionary<double, List<Process>>();
            try
            {
                #region 获取数据
                var dispatchConfig = GetDispatchConfig();
                var dispatchTasks = GetDispatchTaskList(dispatchTaskIds);
                foreach (var item in dispatchTasks)
                {
                    if (item.TaskStatus != DispatchTaskStatus.ToDispatch && item.TaskStatus != DispatchTaskStatus.Dispatching)
                        throw new ValidationException("派工单[{0}]状态为[{1}]，不允许操作派工！".L10nFormat(item.No, item.TaskStatus.ToLabel()));
                    if (item.IsNeedEquipment == true && item.EquipAccountId == null)
                        throw new ValidationException("派工单[{0}]工序[{1}]需要选择辅助设备，请检查！".L10nFormat(item.No, item.ProcessName));

                }
                var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(dispatchTaskIds);
                var dicMergedDispatchTasks = GetMergedTasks(dispatchTaskIds);
                var dispatchTaskDetails = GetDispatchTaskDetails(dispatchTaskIds);
                if (dispatchConfig.IsCheckEmployeeSkill || dispatchConfig.IsCheckPersonnelPermission)
                {
                    emoloyees.AddRange(GetEmployees(dispatchTaskDetails));
                    allEmployeeIds.AddRange(emoloyees.Select(p => p.Id).Distinct());
                }
                if (dispatchConfig.IsCheckPersonnelPermission)
                {
                    dicProcessList = GetProcessListOfTask(dispatchTasks, dicMergedDispatchTasks);
                    employeeResources.AddRange(employeeCt.GetEmployeeResources(allEmployeeIds));
                    processEmployees.AddRange(processCt.GetProcessEmployees(allEmployeeIds));
                }

                if (dispatchConfig.IsCheckEmployeeSkill)
                {
                    dicSkillList = GetSkillListOfTask(dispatchTasks, dicMergedDispatchTasks);
                    employeeSkills.AddRange(skillCt.GetEmployeeSkills(allEmployeeIds));
                }

                var dicTaskDetails = dispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());

                //根据资源获取工位
                var resourceCodes = dispatchTasks.Where(p => p.ResourceCode != null).Select(p => p.ResourceCode).Distinct().ToList();
                var stations = RT.Service.Resolve<StationController>().GetStations(resourceCodes);

                //如果是工作中心，就要把这个工作中心下所有产线都拿出来，创建工位，并且放到对应的任务单执行对象中去
                EntityList<AndonLine> andonLines = RT.Service.Resolve<AndonLineController>().GetAndonLinesByName(resourceCodes);
                var dicAndonLine = andonLines.GroupBy(p => p.WorkCenterCode).ToDictionary(p => p.Key, p => p.ToList());
                //获取资源
                var machineCodes = andonLines.Select(p => p.MachineCode).Distinct().ToList();
                EntityList<WipResource> wipResources = RT.Service.Resolve<WipResourceController>().GetWipResourceByCodeList(machineCodes);

                //把通过工作中心查找出来的产线，单独拿出来查找工位
                stations.AddRange(RT.Service.Resolve<StationController>().GetStations(andonLines.Select(p => p.MachineCode).Distinct().ToList()));

                var stationCodes = stations.Select(p => p.Code).Distinct().ToList();
                //求两者的差集，查找不存在的工位，然后准备创建新的
                var differences = resourceCodes.Except(stationCodes);

                if (dispatchTasks.Any(p => p.ResourceId == null || p.ProcessId == null))
                    throw new ValidationException("资源和工序不能为空".L10N());
                //用派工任务单对应的资源名称去找工位
                //如果没有工位，就用资源名称去创建工位，且创建工序明细
                foreach (var resourceCode in resourceCodes)
                {
                    var tasks = dispatchTasks.Where(p => p.ResourceCode == resourceCode).ToList();
                    var station = stations.FirstOrDefault(p => p.Code == resourceCode);
                    var processIds = tasks.Select(p => p.ProcessId).Distinct().ToList();
                    CreateStation(station, tasks.FirstOrDefault().ResourceName, resourceCode, tasks.FirstOrDefault().ResourceId.Value, processIds, stations);
                    //如果是这个工作中心下的产线，也要创建工位
                    if (dicAndonLine.ContainsKey(resourceCode))
                    {
                        foreach (var rn in dicAndonLine[resourceCode])
                        {
                            var s = stations.FirstOrDefault(p => p.Code == rn.MachineCode);
                            var wipResource = wipResources.FirstOrDefault(p => p.Code == rn.MachineCode);
                            if (wipResource == null)
                                throw new ValidationException("未找到资源{0}!".L10nFormat(rn.MachineCode));
                            CreateStation(s, rn.MachineName, rn.MachineCode, wipResource.Id, processIds, stations);
                        }
                    }

                    #region 旧逻辑

                    //if (station == null)
                    //{
                    //    if (dicAndonLine.ContainsKey(resourceName))
                    //    { 

                    //    }

                    //    station = RT.Service.Resolve<StationController>().CreateStation(resourceName, tasks.FirstOrDefault().ResourceId.Value, processIds);
                    //    stations.Add(station);
                    //}
                    //else
                    //{
                    //    //校验工序是否在这个工位的工序关联明细存在，不存在就插入一条工序
                    //    foreach (var processId in processIds)
                    //    {
                    //        var sp = station.StationProcessList.FirstOrDefault(p => p.ProcessId == processId);
                    //        //如果不存在就插入一条
                    //        if (sp == null)
                    //        {
                    //            station.StationProcessList.Add(new StationProcess()
                    //            {
                    //                ProcessId = processId.Value,
                    //                PersistenceStatus = PersistenceStatus.New
                    //            });
                    //            station.PersistenceStatus = PersistenceStatus.Modified;
                    //            RF.Save(station);
                    //        }
                    //    }

                    //}
                    #endregion

                }

                #endregion

                #region 执行派工
                foreach (var dispatchTask in dispatchTasks)
                {
                    var sts = stations.Where(p => p.Code == dispatchTask.ResourceCode).ToList();
                    //如果是工作中心，就把这些工作中心下的产线也都加到执行对象中去
                    if (dicAndonLine.ContainsKey(dispatchTask.ResourceCode))
                    {
                        sts.AddRange(stations.Where(p => dicAndonLine[dispatchTask.ResourceCode].Select(p => p.MachineCode).Contains(p.Code)).ToList());
                    }
                    //自动创建一个对象
                    foreach (var st in sts)
                    {
                        if (!dispatchTask.Details.Any(p => p.AdoType == AdoType.Station && p.AdoId == st.Id))
                        {
                            var detail = new DispatchTaskDetail()
                            {
                                DispatchTaskId = dispatchTask.Id,
                                AdoId = st.Id,
                                AdoName = st.Name,
                                AdoType = AdoType.Station,
                                AdoGroup = AdoGroup.Station,
                                PersistenceStatus = PersistenceStatus.New
                            };
                            dispatchTask.Details.Add(detail);
                            //将执行对象写到dic里面，方便下面给执行任务对象赋值
                            if (dicTaskDetails.ContainsKey(dispatchTask.Id))
                                dicTaskDetails[dispatchTask.Id].Add(detail);
                            else
                            {
                                dicTaskDetails.Add(dispatchTask.Id, dispatchTask.Details.ToList());
                            }
                        }
                    }

                    #region 旧逻辑

                    //自动创建一个对象
                    //var station = stations.FirstOrDefault(p => p.Name == dispatchTask.ResourceName);
                    //if (!dispatchTask.Details.Any(p => p.AdoType == AdoType.Station && p.AdoId == station.Id))
                    //{
                    //    var detail = new DispatchTaskDetail()
                    //    {
                    //        DispatchTaskId = dispatchTask.Id,
                    //        AdoId = station.Id,
                    //        AdoName = station.Name,
                    //        AdoType = AdoType.Station,
                    //        AdoGroup = AdoGroup.Station,
                    //        PersistenceStatus = PersistenceStatus.New
                    //    };
                    //    dispatchTask.Details.Add(detail);
                    //    //将执行对象写到dic里面，方便下面给执行任务对象赋值
                    //    if (dicTaskDetails.ContainsKey(dispatchTask.Id))
                    //        dicTaskDetails[dispatchTask.Id].Add(detail);
                    //    else
                    //    {
                    //        dicTaskDetails.Add(dispatchTask.Id, dispatchTask.Details.ToList());
                    //    }
                    //}
                    #endregion

                    List<DispatchTaskDetail> taskDetails = null;
                    dicTaskDetails.TryGetValue(dispatchTask.Id, out taskDetails);
                    List<Process> processList = null;
                    dicProcessList.TryGetValue(dispatchTask.Id, out processList);
                    List<Skill> skillList = null;
                    dicSkillList.TryGetValue(dispatchTask.Id, out skillList);
                    var errMsgOfTask = ValidateDispatchTask(dispatchTask, taskDetails, dicMergedDispatchTasks, dispatchConfig, emoloyees, processEmployees, employeeResources, employeeSkills, processList, skillList);
                    if (errMsgOfTask.Length > 0)
                    {
                        errMsg.Append(errMsgOfTask);
                    }
                    else
                    {
                        if (taskDetails != null)
                        {
                            dispatchTask.TaskPerformer = taskDetails.Select(p => p.AdoName).Distinct().Concat(";");
                        }

                        dispatchTask.TaskStatus = DispatchTaskStatus.Dispatched;
                        dispatchTask.SendQty = dispatchTask.DispatchQty;
                        dispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                        toBeDispatchTasks.Add(dispatchTask);
                    }
                }

                if (toBeDispatchTasks.Count <= 0)
                {
                    return errMsg.ToString();
                }
                else
                {
                    //更新选中任务单关联的任务单的状态和可执行对象
                    DealTaskAssociatedTasks(toBeDispatchTasks, associatedDispatchTaskInfo, dicTaskDetails);

                    RF.Save(toBeDispatchTasks);
                    RT.EventBus.Publish(new DispatchTaskDispatched(toBeDispatchTasks));

                    //派工成功，创建工序产前准备记录
                    var dtIds = toBeDispatchTasks.Select(p => p.Id).Distinct().ToList();
                    RT.Service.Resolve<ProcessPrepareRecordsController>().CreateProcessPrepareRecord(dtIds);
                }

                #endregion
            }
            catch (Exception exc)
            {
                errMsg.Clear();
                errMsg.Append(exc.Message);
            }

            return errMsg.ToString();
        }

        public virtual void CreateStation(Station station, string resourceName, string resourceCode, double ResourceId, List<double?> processIds, EntityList<Station> stations)
        {
            if (station == null)
            {
                station = RT.Service.Resolve<StationController>().CreateStation(resourceCode, resourceName, ResourceId, processIds);
                stations.Add(station);
            }
            else
            {
                //校验工序是否在这个工位的工序关联明细存在，不存在就插入一条工序
                foreach (var processId in processIds)
                {
                    var sp = station.StationProcessList.FirstOrDefault(p => p.ProcessId == processId);
                    //如果不存在就插入一条
                    if (sp == null)
                    {
                        station.StationProcessList.Add(new StationProcess()
                        {
                            ProcessId = processId.Value,
                            PersistenceStatus = PersistenceStatus.New
                        });
                        station.PersistenceStatus = PersistenceStatus.Modified;
                        RF.Save(station);
                    }
                }

            }
        }

        /// <summary>
        /// 更新选中任务单关联的任务单的状态和可执行对象
        /// </summary>
        /// <param name="toBeDispatchTasks">可执行派工的任务单列表</param>
        /// <param name="associatedDispatchTaskInfo">可执行派工的任务单的关联任务单信息</param>
        /// <param name="dicTaskDetails">可执行派工的任务单的派工任务明细字典</param>
        /// <returns>可执行派工的任务单列表</returns>
        private void DealTaskAssociatedTasks(EntityList<DispatchTask> toBeDispatchTasks, AssociatedDispatchTaskInfo associatedDispatchTaskInfo, Dictionary<double, List<DispatchTaskDetail>> dicTaskDetails)
        {
            //备份一份MES回传数据(因后续操作会更改列表数据)
            List<DispatchTask> orgDispatchTasks = DeepCopy.Clone<DispatchTask>(toBeDispatchTasks.ToList());
            var dicOrgDispatchTasks = orgDispatchTasks.ToDictionary(p => p.Id);
            var notSynTaskIds = associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.SelectMany(p => p.Value).Select(p => p.Id).Distinct().ToList();
            var dispatchTaskDetails = GetDispatchTaskDetails(notSynTaskIds);
            foreach (var orgDispatchTask in orgDispatchTasks)
            {
                if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out List<DispatchTask> notSyntypeDispatchTasks))
                    toBeDispatchTasks.AddRange(DealUnSyntypeDispatchTasks(dicTaskDetails, orgDispatchTask, notSyntypeDispatchTasks, dispatchTaskDetails));

                if (associatedDispatchTaskInfo.DicSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out List<DispatchTask> syntypeDispatchTasks))
                    toBeDispatchTasks.AddRange(DealSyntypeDispatchTasks(associatedDispatchTaskInfo, dicOrgDispatchTasks, syntypeDispatchTasks));
            }
        }

        /// <summary>
        /// 处理共模关联任务单列表
        /// </summary>
        /// <param name="associatedDispatchTaskInfo">可执行派工的任务单的关联任务单信息</param>
        /// <param name="dicOrgDispatchTasks">当前选中可执行派工的任务单字典</param>
        /// <param name="syntypeDispatchTasks">共模关联任务单列表</param>
        /// <returns>可执行派工的任务单</returns>
        private EntityList<DispatchTask> DealSyntypeDispatchTasks(AssociatedDispatchTaskInfo associatedDispatchTaskInfo, Dictionary<double, DispatchTask> dicOrgDispatchTasks, List<DispatchTask> syntypeDispatchTasks)
        {
            var toBeDispatchTasks = new EntityList<DispatchTask>();
            foreach (var syntypeDispatchTask in syntypeDispatchTasks)
            {
                associatedDispatchTaskInfo.DicSyntypeDispatchTaskDetails.TryGetValue(syntypeDispatchTask.Id, out List<DispatchTaskDetail> syntypeDispatchTaskDetails);
                string strTaskPerfomer = string.Empty;
                if (syntypeDispatchTaskDetails != null)
                    strTaskPerfomer = syntypeDispatchTaskDetails.Select(p => p.AdoName).Distinct().Concat(";");
                if (associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.TryGetValue(syntypeDispatchTask.Id, out List<DispatchTask> syntypeMainDispatchTasks))
                {
                    SetMainDispatchTasks(dicOrgDispatchTasks, syntypeMainDispatchTasks);
                    toBeDispatchTasks.AddRange(UpdateSyntypeDispatchTasks(syntypeDispatchTask, strTaskPerfomer, syntypeMainDispatchTasks));
                }
            }

            return toBeDispatchTasks;
        }

        /// <summary>
        /// 设置共模主任务单的状态和任务执行对象（因派工后，部分主任务单状态和可执行对象已变更，但是未保存数据库）
        /// </summary>
        /// <param name="dicOrgDispatchTasks">原任务单字典</param>
        /// <param name="syntypeMainDispatchTasks">共模主任务单列表</param>
        private void SetMainDispatchTasks(Dictionary<double, DispatchTask> dicOrgDispatchTasks, List<DispatchTask> syntypeMainDispatchTasks)
        {
            syntypeMainDispatchTasks.ForEach(p =>
            {
                if (dicOrgDispatchTasks.TryGetValue(p.Id, out DispatchTask dispatchTask))
                {
                    p.TaskStatus = dispatchTask.TaskStatus;
                    p.TaskPerformer = dispatchTask.TaskPerformer;
                }
            });
        }

        /// <summary>
        /// 更新共模关联任务单列表
        /// </summary>
        /// <param name="syntypeDispatchTask">共模关联任务单</param>
        /// <param name="strTaskPerfomer">可执行对象</param>
        /// <param name="syntypeMainDispatchTasks">共模关联任务单的所有主料任务单列表（共模辅料任务单与共模主料任务单是1对多）</param>
        /// <returns>可执行派工的任务单</returns>
        private EntityList<DispatchTask> UpdateSyntypeDispatchTasks(DispatchTask syntypeDispatchTask, string strTaskPerfomer, List<DispatchTask> syntypeMainDispatchTasks)
        {
            var toBeDispatchTasks = new EntityList<DispatchTask>();
            if (!(syntypeMainDispatchTasks.Any(p => p.TaskStatus == DispatchTaskStatus.Executing)))
            {
                if (syntypeMainDispatchTasks.Any(p => p.TaskStatus == DispatchTaskStatus.Dispatched))
                {
                    syntypeDispatchTask.TaskStatus = DispatchTaskStatus.Dispatched;
                    syntypeDispatchTask.SendQty = syntypeDispatchTask.DispatchQty;
                    syntypeDispatchTask.TaskPerformer = strTaskPerfomer;
                    syntypeDispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                    toBeDispatchTasks.Add(syntypeDispatchTask);
                }
            }

            return toBeDispatchTasks;
        }

        /// <summary>
        /// 更新非共模的可执行派工的任务单列表
        /// </summary>
        /// <param name="dicTaskDetails">可执行派工的任务单的派工任务明细字典</param>
        /// <param name="orgDispatchTask">当前某选中可派工任务单</param>
        /// <param name="notSyntypeDispatchTasks">当前某选中可派工任务单的非共模关联任务单列表</param>
        /// <param name="dispatchTaskDetails">非共模关联任务单明细列表</param>
        /// <returns>可执行派工的任务单</returns>
        private EntityList<DispatchTask> DealUnSyntypeDispatchTasks(Dictionary<double, List<DispatchTaskDetail>> dicTaskDetails, DispatchTask orgDispatchTask, List<DispatchTask> notSyntypeDispatchTasks, EntityList<DispatchTaskDetail> dispatchTaskDetails)
        {
            var toBeDispatchTasks = new EntityList<DispatchTask>();
            if (notSyntypeDispatchTasks.Any())
            {
                notSyntypeDispatchTasks.ForEach(p => { p.TaskStatus = DispatchTaskStatus.Dispatched; p.TaskPerformer = orgDispatchTask.TaskPerformer; p.SendQty = p.DispatchQty; p.PersistenceStatus = PersistenceStatus.Modified; });
                DealSplitDispatchTasks(dicTaskDetails, orgDispatchTask, notSyntypeDispatchTasks, dispatchTaskDetails);
                toBeDispatchTasks.AddRange(notSyntypeDispatchTasks);
            }

            return toBeDispatchTasks;
        }

        /// <summary>
        /// 对非共模关联任务单中拆分的任务单赋值可执行对象（因拆分的任务单是主单，已派工后必须要给予可执行对象）
        /// </summary>
        /// <param name="dicTaskDetails">派工任务明细字典</param>
        /// <param name="orgDispatchTask">原派工任务</param>
        /// <param name="notSyntypeDispatchTasks">非共模派工任务单列表</param>
        /// <param name="dispatchTaskDetails">非共模关联任务单明细列表</param>
        private void DealSplitDispatchTasks(Dictionary<double, List<DispatchTaskDetail>> dicTaskDetails, DispatchTask orgDispatchTask, List<DispatchTask> notSyntypeDispatchTasks, EntityList<DispatchTaskDetail> dispatchTaskDetails)
        {
            var dicNotSynTaskDetails = dispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var notSyntypeDispatchTask in notSyntypeDispatchTasks)
            {
                if (notSyntypeDispatchTask.Associated == Associated.Split)
                {
                    dicNotSynTaskDetails.TryGetValue(notSyntypeDispatchTask.Id, out List<DispatchTaskDetail> notSynTaskDetails);
                    if (dicTaskDetails.TryGetValue(orgDispatchTask.Id, out List<DispatchTaskDetail> taskDetails))
                    {
                        foreach (var taskDetail in taskDetails)
                        {
                            if (notSynTaskDetails != null && notSynTaskDetails.Any(p => p.AdoId == taskDetail.AdoId && p.AdoName == taskDetail.AdoName && p.AdoType == taskDetail.AdoType))
                                continue;
                            var newTaskDetail = CreateTaskDetail(notSyntypeDispatchTask, taskDetail);
                            notSyntypeDispatchTask.Details.Add(newTaskDetail);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建派工任务明细
        /// </summary>
        /// <param name="dispatchTask">派工任务</param>
        /// <param name="taskDetail">派工任务明细</param>
        /// <returns>新派工任务明细</returns>
        private DispatchTaskDetail CreateTaskDetail(DispatchTask dispatchTask, DispatchTaskDetail taskDetail)
        {
            return new DispatchTaskDetail()
            {
                DispatchTaskId = dispatchTask.Id,
                AdoId = taskDetail.AdoId,
                AdoGroup = taskDetail.AdoGroup,
                AdoName = taskDetail.AdoName,
                AdoType = taskDetail.AdoType,
                ProcessMatchDegree = taskDetail.ProcessMatchDegree,
                PersistenceStatus = PersistenceStatus.New
            };
        }

        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <param name="dispatchTaskDetails">任务明细列表</param>
        /// <returns>员工列表</returns>
        private List<Employee> GetEmployees(EntityList<DispatchTaskDetail> dispatchTaskDetails)
        {
            var employeeCt = RT.Service.Resolve<EmployeeController>();
            List<Employee> emoloyees = new List<Employee>();
            var dicTaskDetails = dispatchTaskDetails.GroupBy(p => p.AdoType).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var dicTaskDetail in dicTaskDetails)
            {
                if (dicTaskDetail.Key == AdoType.WorkGroup)
                {
                    var workGroupIds = dicTaskDetail.Value.Select(p => p.AdoId).Distinct().Cast<double?>().ToList();
                    emoloyees.AddRange(employeeCt.GetEmployeeListByWorkGroup(workGroupIds));
                }
                else if (dicTaskDetail.Key == AdoType.EmployeeGroup)
                {
                    var employeeGroupIds = dicTaskDetail.Value.Select(p => p.AdoId).Distinct().Cast<double?>().ToList();
                    emoloyees.AddRange(employeeCt.GetEmployeeListByEmployeeGroup(employeeGroupIds));
                }
                else if (dicTaskDetail.Key == AdoType.Employee)
                {
                    var employeeIds = dicTaskDetail.Value.Select(p => p.AdoId).Distinct().ToList();
                    emoloyees.AddRange(employeeCt.GetEmployeeList(employeeIds));
                }
                else if (dicTaskDetail.Key == AdoType.Station)
                {
                    var stationIds = dicTaskDetail.Value.Select(p => p.AdoId).Distinct().ToList();
                    emoloyees.AddRange(GetEmployeesByStationIds(stationIds));
                }
            }

            return emoloyees.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
        }

        /// <summary>
        /// 获取技能清单字典
        /// </summary>
        /// <param name="dispatchTasks">任务单列表</param>
        /// <param name="dicMergedDispatchTasks">已合并的任务单字典</param>
        /// <returns>技能清单字典</returns>
        private Dictionary<double, List<Skill>> GetSkillListOfTask(EntityList<DispatchTask> dispatchTasks, Dictionary<double, List<DispatchTask>> dicMergedDispatchTasks)
        {
            var dicSkillList = new Dictionary<double, List<Skill>>();
            var dispatchTaskOfProcess = dispatchTasks.Where(p => p.Process != null);
            var processIds = dispatchTaskOfProcess.Select(p => p.ProcessId.Value).Distinct().ToList();
            var dispatchTaskOfNullProcess = dispatchTasks.Where(p => p.Process == null);
            var processSkills = RT.Service.Resolve<ProcessController>().GetProcessSkills(processIds);

            foreach (var dispatchTask in dispatchTaskOfNullProcess)
            {
                var workOrderIds = new List<double>();
                if (!dicSkillList.ContainsKey(dispatchTask.Id))
                    dicSkillList.Add(dispatchTask.Id, new List<Skill>());
                if (dispatchTask.MergedStatus == MergedStatus.MergeRows)
                {
                    List<DispatchTask> mergedTasks = null;
                    if (dicMergedDispatchTasks.TryGetValue(dispatchTask.Id, out mergedTasks))
                    {
                        workOrderIds.AddRange(mergedTasks.Where(p => p.WorkOrder != null).Select(p => p.WorkOrderId.Value).Distinct());
                    }
                }
                else
                {
                    workOrderIds.Add(dispatchTask.WorkOrderId.Value);
                }

                var skills = GetSkillListByWorkOrder(workOrderIds);
                dicSkillList[dispatchTask.Id].AddRange(skills);
            }
            var dicProcessSkills = processSkills.GroupBy(p => p.ProcessId).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var dispatchTask in dispatchTaskOfProcess)
            {
                if (!dicSkillList.ContainsKey(dispatchTask.Id))
                    dicSkillList.Add(dispatchTask.Id, new List<Skill>());
                List<ProcessSkill> processSkill = null;
                if (dicProcessSkills.TryGetValue(dispatchTask.ProcessId.Value, out processSkill))
                {
                    var skills = processSkill.Select(p => p.Skill);
                    if (skills.Any())
                    {
                        dicSkillList[dispatchTask.Id].AddRange(skills);
                    }
                }
            }

            return dicSkillList;
        }

        /// <summary>
        /// 获取工序字典
        /// </summary>
        /// <param name="dispatchTasks">任务单列表</param>
        /// <param name="dicMergedDispatchTasks">已合并的任务单字典</param>
        /// <returns>工序字典</returns>
        private Dictionary<double, List<Process>> GetProcessListOfTask(EntityList<DispatchTask> dispatchTasks, Dictionary<double, List<DispatchTask>> dicMergedDispatchTasks)
        {
            var dicProcessList = new Dictionary<double, List<Process>>();
            var dispatchTaskOfProcess = dispatchTasks.Where(p => p.Process != null);
            var dispatchTaskOfNullProcess = dispatchTasks.Where(p => p.Process == null);
            foreach (var dispatchTask in dispatchTaskOfNullProcess)
            {
                var workOrderIds = new List<double>();
                if (!dicProcessList.ContainsKey(dispatchTask.Id))
                    dicProcessList.Add(dispatchTask.Id, new List<Process>());
                if (dispatchTask.MergedStatus == MergedStatus.MergeRows)
                {
                    List<DispatchTask> mergedTasks = null;
                    if (dicMergedDispatchTasks.TryGetValue(dispatchTask.Id, out mergedTasks))
                    {
                        workOrderIds.AddRange(mergedTasks.Where(p => p.WorkOrder != null).Select(p => p.WorkOrderId.Value).Distinct());
                    }
                }
                else
                {
                    workOrderIds.Add(dispatchTask.WorkOrderId.Value);
                }

                var processs = GetProcessListByWorkOrder(workOrderIds);
                dicProcessList[dispatchTask.Id].AddRange(processs);
            }

            foreach (var dispatchTask in dispatchTaskOfProcess)
            {
                if (!dicProcessList.ContainsKey(dispatchTask.Id))
                    dicProcessList.Add(dispatchTask.Id, new List<Process>());
                dicProcessList[dispatchTask.Id].Add(dispatchTask.Process);
            }

            return dicProcessList;
        }

        /// <summary>
        /// 验证派工的任务单
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="dispatchTaskDetails">任务单明细列表</param>
        /// <param name="dicMergedDispatchTasks">任务单的合并任务单字典</param>
        /// <param name="dispatchConfig">任务单配置值</param>
        /// <param name="employees">员工列表</param>
        /// <param name="processEmployees">工序员工列表</param>
        /// <param name="employeeResources">员工资源列表</param>
        /// <param name="employeeSkills">员工技能列表</param>
        /// <param name="processList">工序列表</param>
        /// <param name="skillList">技能清单</param>
        private string ValidateDispatchTask(DispatchTask dispatchTask, List<DispatchTaskDetail> dispatchTaskDetails, Dictionary<double, List<DispatchTask>> dicMergedDispatchTasks, DispatchConfigValue dispatchConfig, List<Employee> employees, EntityList<ProcessEmployee> processEmployees, EntityList<EmployeeResource> employeeResources, EntityList<EmployeeSkill> employeeSkills, List<Process> processList, List<Skill> skillList)
        {
            string errMsg = string.Empty;
            var employeesOfTaskDetail = new List<Employee>();
            if (dispatchConfig.IsCheckPersonnelPermission || dispatchConfig.IsCheckEmployeeSkill)
            {
                errMsg = CreateTaskEmployees(dispatchTask, dispatchTaskDetails, employees, employeesOfTaskDetail);
                if (errMsg.Length > 0)
                    return errMsg;
            }

            if (dispatchTask.TaskStatus != DispatchTaskStatus.ToDispatch && dispatchTask.TaskStatus != DispatchTaskStatus.Dispatching)
                return "任务单[{0}]的状态为：{1},派工失败！".L10nFormat(dispatchTask.No,
                    EnumViewModel.EnumToLabel(dispatchTask.TaskStatus).L10N()) + "\n";
            if (!dispatchTask.ResourceId.HasValue)
                return "任务单[{0}]的资源为空,派工失败！请在工单维护资源".L10nFormat(dispatchTask.No) + "\n";
            if (dispatchTask.MergedStatus != MergedStatus.MergeRows)
            {
                var workOrder = dispatchTask.WorkOrder;
                if (workOrder.State != Core.WorkOrders.WorkOrderState.Release && workOrder.State != Core.WorkOrders.WorkOrderState.Producing)
                    return "任务单[{0}]的关联工单[{1}]状态为：{2},派工失败！".L10nFormat(dispatchTask.No, dispatchTask.WorkOrder.No,
                        EnumViewModel.EnumToLabel(workOrder.State).L10N()) + "\n";
                if (workOrder.IsPause == YesNo.Yes && (workOrder.State == Core.WorkOrders.WorkOrderState.Release || workOrder.State == Core.WorkOrders.WorkOrderState.Producing))
                    return "任务单[{0}]的关联工单[{1}]状态为：{2},派工失败！".L10nFormat(dispatchTask.No, dispatchTask.WorkOrder.No,
                        EnumViewModel.EnumToLabel(workOrder.State).L10N() + "暂停".L10N()) + "\n";
            }
            else
            {
                List<DispatchTask> mergedTasks = null;
                if (dicMergedDispatchTasks.TryGetValue(dispatchTask.Id, out mergedTasks))
                {
                    if (mergedTasks.Any(p => p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Release && p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Producing))
                        return "任务单[{0}]的关联工单中存在非发放/生产中的状态,派工失败！".L10nFormat(dispatchTask.No) + "\n";
                    if (mergedTasks.Any(p => (p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Release || p.WorkOrder.State == Core.WorkOrders.WorkOrderState.Producing) && p.WorkOrder.IsPause == YesNo.Yes))
                        return "任务单[{0}]的关联工单中存在非发放/生产中(暂停)的状态,派工失败！".L10nFormat(dispatchTask.No) + "\n";
                }
            }

            if (dispatchTask.Details == null || !dispatchTask.Details.Any())
            {
                return "任务单[{0}]没有可选执行对象,派工失败！".L10nFormat(dispatchTask.No) + "\n";
            }

            if (dispatchConfig.IsCheckPersonnelPermission)
            {
                errMsg = ValidatePersonnelPermission(dispatchTask, processEmployees, employeeResources, employeesOfTaskDetail, processList);
                if (errMsg.Length > 0)
                    return errMsg;
            }

            if (dispatchConfig.IsCheckEmployeeSkill)
            {
                errMsg = ValidateEmployeeSkill(dispatchTask, employeeSkills, employeesOfTaskDetail, skillList);
                if (errMsg.Length > 0)
                {
                    return errMsg;
                }
            }

            return errMsg;
        }

        /// <summary>
        /// 创建员工列表
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="dispatchTaskDetails">任务单明细列表</param>
        /// <param name="employees">员工列表</param>
        /// <param name="employeesOfTaskDetail">员工列表</param>
        /// <returns>结果信息</returns>
        private string CreateTaskEmployees(DispatchTask dispatchTask, List<DispatchTaskDetail> dispatchTaskDetails, List<Employee> employees, List<Employee> employeesOfTaskDetail)
        {
            string errMsg = string.Empty;
            var dicEmployeesOfEmployeeGroup = employees.Where(p => p.EmployeeGroup != null).GroupBy(p => p.EmployeeGroupId.Value).ToDictionary(p => p.Key, p => p.ToList());
            var dicEmployeesOfWorkGroup = employees.Where(p => p.WorkGroup != null).GroupBy(p => p.WorkGroupId.Value).ToDictionary(p => p.Key, p => p.ToList());

            var dicEmployees = employees.ToDictionary(p => p.Id);
            if (dispatchTaskDetails == null)
            {
                return "任务单[{0}]已选员工组员工数据为空,派工失败！".L10nFormat(dispatchTask.No) + "\n";
            }
            foreach (var taskDetail in dispatchTaskDetails)
            {
                if (taskDetail.AdoType == AdoType.Employee)
                {
                    Employee employee = null;
                    if (dicEmployees.TryGetValue(taskDetail.AdoId, out employee))
                    {
                        employeesOfTaskDetail.Add(employee);
                    }
                }
                else if (taskDetail.AdoType == AdoType.EmployeeGroup)
                {
                    List<Employee> employeesOfEmployeeGroup = null;
                    if (dicEmployeesOfEmployeeGroup.TryGetValue(taskDetail.AdoId, out employeesOfEmployeeGroup))
                    {
                        employeesOfTaskDetail.AddRange(employeesOfEmployeeGroup);
                    }
                    else
                    {
                        return "任务单[{0}]已选员工组[{1}]员工数据为空,派工失败！".L10nFormat(dispatchTask.No, taskDetail.AdoName) + "\n";
                    }
                }
                else if (taskDetail.AdoType == AdoType.WorkGroup)
                {
                    List<Employee> employeesOfWorkGroup = null;
                    if (dicEmployeesOfWorkGroup.TryGetValue(taskDetail.AdoId, out employeesOfWorkGroup))
                    {
                        employeesOfTaskDetail.AddRange(employeesOfWorkGroup);
                    }
                    else
                    {
                        return "任务单[{0}]已选班组[{1}]员工数据为空,派工失败！".L10nFormat(dispatchTask.No, taskDetail.AdoName) + "\n";
                    }
                }
                else if (taskDetail.AdoType == AdoType.Station)
                {
                    //
                }
            }

            return errMsg;
        }

        /// <summary>
        /// 验证个人权限
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="processEmployees">工序与员工列表</param>
        /// <param name="employeeResources">员工与资源列表</param>
        /// <param name="employeesOfTaskDetail">员工列表</param>
        /// <param name="processList">工序列表</param>
        /// <returns>结果信息</returns>
        private string ValidatePersonnelPermission(DispatchTask dispatchTask, EntityList<ProcessEmployee> processEmployees, EntityList<EmployeeResource> employeeResources, List<Employee> employeesOfTaskDetail, List<Process> processList)
        {
            string errMsg = string.Empty;
            var dicProcessEmployees = processEmployees.GroupBy(p => p.EmployeeId.Value).ToDictionary(p => p.Key, p => p.ToList());
            var dicEmployeeResources = employeeResources.GroupBy(p => p.EmployeeId).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var employee in employeesOfTaskDetail)
            {
                foreach (var process in processList)
                {
                    if (!dicProcessEmployees.TryGetValue(employee.Id, out List<ProcessEmployee> processEmployeeList))
                    {
                        return "任务单[{0}]的已选员工[{1}]没有[{2}]工序权限,派工失败！".L10nFormat(dispatchTask.No, employee.Name, process.Name) + "\n";
                    }
                    var processIds = processEmployeeList.Select(p => p.ProcessId.Value).Distinct();
                    if (!processIds.Contains(process.Id))
                    {
                        return "任务单[{0}]的已选员工[{1}]没有[{2}]工序权限,派工失败！".L10nFormat(dispatchTask.No, employee.Name, process.Name) + "\n";
                    }

                }

                if (!dicEmployeeResources.TryGetValue(employee.Id, out List<EmployeeResource> employeeResourceList))
                {
                    return "任务单[{0}]的已选员工[{1}]没有[{2}]资源权限,派工失败！".L10nFormat(dispatchTask.No, employee.Name, dispatchTask.Resource.Name) + "\n";
                }
                var resourceIds = employeeResourceList.Select(p => p.ResourceId).Distinct();
                if (!resourceIds.Contains(dispatchTask.ResourceId.Value))
                {
                    return "任务单[{0}]的已选员工[{1}]没有[{2}]资源权限,派工失败！".L10nFormat(dispatchTask.No, employee.Name, dispatchTask.Resource.Name) + "\n";
                }
            }

            return errMsg;
        }

        /// <summary>
        /// 验证员工技能
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        /// <param name="employeeSkills">员工技能列表</param>
        /// <param name="employeesOfTaskDetail">员工列表</param>
        /// <param name="skillList">技能清单列表</param>
        /// <returns>结果信息</returns>
        private string ValidateEmployeeSkill(DispatchTask dispatchTask, EntityList<EmployeeSkill> employeeSkills, List<Employee> employeesOfTaskDetail, List<Skill> skillList)
        {
            string errMsg = string.Empty;
            var dicEmployeeSkills = employeeSkills.GroupBy(p => p.EmployeeId).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var employee in employeesOfTaskDetail)
            {
                foreach (var skill in skillList)
                {
                    List<EmployeeSkill> employeeSkillList = null;
                    if (dicEmployeeSkills.TryGetValue(employee.Id, out employeeSkillList))
                    {
                        var skillIds = employeeSkillList.Select(p => p.SkillId).Distinct();
                        if (!skillIds.Contains(skill.Id))
                        {
                            return "任务单[{0}]的已选员工[{1}]没有[{2}]工序技能要求,派工失败！".L10nFormat(dispatchTask.No, employee.Name, skill.Name) + "\n";
                        }
                    }
                    else
                    {
                        return "任务单[{0}]的已选员工[{1}]没有[{2}]工序技能要求,派工失败！".L10nFormat(dispatchTask.No, employee.Name, skill.Name) + "\n";
                    }
                }
            }

            return errMsg;
        }

        /// <summary>
        /// 撤销派工任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>撤销派工结果</returns>
        public virtual string CancelDispatchTasks(List<double> dispatchTaskIds)
        {
            StringBuilder errMsg = new StringBuilder();
            var toBeCancelDispatchTasks = new EntityList<DispatchTask>();
            try
            {
                var dispatchTasks = GetDispatchTaskList(dispatchTaskIds);
                var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(dispatchTaskIds);

                foreach (var dispatchTask in dispatchTasks)
                {
                    var errMsgOfTask = ValidateCancelDispatchTask(dispatchTask);
                    if (errMsgOfTask.Length > 0)
                    {
                        errMsg.Append(errMsgOfTask);
                    }
                    else
                    {
                        dispatchTask.TaskStatus = DispatchTaskStatus.Dispatching;
                        dispatchTask.SendQty = 0;
                        dispatchTask.TaskPerformer = string.Empty;
                        dispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                        if (dispatchTask.OldResourceId != null)
                            dispatchTask.ResourceId = dispatchTask.OldResourceId;
                        dispatchTask.OldResourceId = null;
                        toBeCancelDispatchTasks.Add(dispatchTask);
                    }
                }

                if (toBeCancelDispatchTasks.Count <= 0)
                {
                    return errMsg.ToString();
                }

                //更新选中可撤销派工的任务单关联的任务单的状态和可执行对象
                CancelAssociatedDispatchTasks(toBeCancelDispatchTasks, associatedDispatchTaskInfo);

                RF.Save(toBeCancelDispatchTasks);

                //删除工序产前准备记录
                var dtsIds = toBeCancelDispatchTasks.Select(p => p.Id).Distinct().ToList();
                RT.Service.Resolve<ProcessPrepareRecordsController>().DeleteProcessPrepareRecordByDispatchTaskIds(dtsIds);
            }
            catch (Exception exc)
            {
                errMsg.Clear();
                errMsg.Append(exc.Message);
            }

            return errMsg.ToString();
        }

        /// <summary>
        /// 更新选中任务单关联的任务单的状态和可执行对象
        /// </summary>
        /// <param name="toBeCancelDispatchTasks">可执行撤销派工的任务单列表</param>
        /// <param name="associatedDispatchTaskInfo">可执行撤销派工的任务单的关联任务单信息</param>
        private void CancelAssociatedDispatchTasks(EntityList<DispatchTask> toBeCancelDispatchTasks, AssociatedDispatchTaskInfo associatedDispatchTaskInfo)
        {
            //备份一份MES回传数据(因后续操作会更改列表数据)
            List<DispatchTask> orgDispatchTasks = DeepCopy.Clone<DispatchTask>(toBeCancelDispatchTasks.ToList());
            var dicOrgDispatchTasks = orgDispatchTasks.ToDictionary(p => p.Id);
            foreach (var orgDispatchTask in orgDispatchTasks)
            {
                if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out List<DispatchTask> notSyntypeDispatchTasks))
                {
                    toBeCancelDispatchTasks.AddRange(CancelUnSyntypeDispatchTasks(orgDispatchTask, notSyntypeDispatchTasks));
                }

                if (associatedDispatchTaskInfo.DicSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out List<DispatchTask> syntypeDispatchTasks))
                {
                    toBeCancelDispatchTasks.AddRange(CancelSyntypeDispatchTasks(associatedDispatchTaskInfo, dicOrgDispatchTasks, syntypeDispatchTasks));
                }
            }
        }

        /// <summary>
        /// 撤销共模关联的任务单列表
        /// </summary>
        /// <param name="associatedDispatchTaskInfo">可执行撤销派工的任务单的关联任务单信息</param>
        /// <param name="dicOrgDispatchTasks">当前选中可撤销派工的任务单列表</param>
        /// <param name="syntypeDispatchTasks">当前选中可撤销派工的共模关联任务单列表</param>
        /// <returns>可撤销的派工任务列表</returns>
        private EntityList<DispatchTask> CancelSyntypeDispatchTasks(AssociatedDispatchTaskInfo associatedDispatchTaskInfo, Dictionary<double, DispatchTask> dicOrgDispatchTasks, List<DispatchTask> syntypeDispatchTasks)
        {
            var toBeCancelDispatchTasks = new EntityList<DispatchTask>();
            foreach (var syntypeDispatchTask in syntypeDispatchTasks)
            {
                associatedDispatchTaskInfo.DicSyntypeDispatchTaskDetails.TryGetValue(syntypeDispatchTask.Id, out List<DispatchTaskDetail> syntypeDispatchTaskDetails);
                string strTaskPerfomer = string.Empty;
                if (syntypeDispatchTaskDetails != null)
                    strTaskPerfomer = syntypeDispatchTaskDetails.Select(p => p.AdoName).Distinct().Concat(";");
                if (associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.TryGetValue(syntypeDispatchTask.Id, out List<DispatchTask> syntypeMainDispatchTasks))
                {
                    SetMainDispatchTasks(dicOrgDispatchTasks, syntypeMainDispatchTasks);

                    // 毫无作用的代码，不知干嘛用的，去掉！
                    ////syntypeMainDispatchTasks.ForEach(p =>
                    ////{
                    ////    if (p.TaskStatus == DispatchTaskStatus.Pause && (p.OldTaskStatus == DispatchTaskStatus.Dispatched || p.OldTaskStatus == DispatchTaskStatus.Executing))
                    ////    {
                    ////        return;
                    ////    }
                    ////});
                    toBeCancelDispatchTasks.AddRange(UpdateCancelSyntypeDispatchTask(syntypeDispatchTask, strTaskPerfomer, syntypeMainDispatchTasks));
                }
            }

            return toBeCancelDispatchTasks;
        }

        /// <summary>
        /// 更新共模关联任务单状态、已派工数量和可执行对象
        /// </summary>
        /// <param name="syntypeDispatchTask">共模关联任务单</param>
        /// <param name="strTaskPerfomer">可执行对象</param>
        /// <param name="syntypeMainDispatchTasks">某共模关联任务单的所有共模主料任务单列表</param>
        /// <returns>可派工的任务单列表</returns>
        private EntityList<DispatchTask> UpdateCancelSyntypeDispatchTask(DispatchTask syntypeDispatchTask, string strTaskPerfomer, List<DispatchTask> syntypeMainDispatchTasks)
        {
            var toBeCancelDispatchTasks = new EntityList<DispatchTask>();
            if (!syntypeMainDispatchTasks.Any(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched))
            {
                syntypeDispatchTask.TaskStatus = DispatchTaskStatus.Dispatching;
                syntypeDispatchTask.SendQty = 0;
                syntypeDispatchTask.TaskPerformer = strTaskPerfomer;
                syntypeDispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                toBeCancelDispatchTasks.Add(syntypeDispatchTask);
            }

            return toBeCancelDispatchTasks;
        }

        /// <summary>
        /// 更新非共模关联任务单列表
        /// </summary>
        /// <param name="orgDispatchTask">当前选中可派工任务单</param>
        /// <param name="notSyntypeDispatchTasks">当前选中可派工任务单</param>
        /// <returns>可派工的任务单列表</returns>
        private EntityList<DispatchTask> CancelUnSyntypeDispatchTasks(DispatchTask orgDispatchTask, List<DispatchTask> notSyntypeDispatchTasks)
        {
            var toBeCancelDispatchTasks = new EntityList<DispatchTask>();
            if (notSyntypeDispatchTasks.Any())
            {
                notSyntypeDispatchTasks.ForEach(p =>
                {
                    p.TaskStatus = DispatchTaskStatus.Dispatching;
                    p.TaskPerformer = orgDispatchTask.TaskPerformer;
                    p.SendQty = 0;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });
                toBeCancelDispatchTasks.AddRange(notSyntypeDispatchTasks);
            }

            return toBeCancelDispatchTasks;
        }

        /// <summary>
        /// 验证撤销派工的任务单
        /// </summary>
        /// <param name="dispatchTask">任务单</param>
        private string ValidateCancelDispatchTask(DispatchTask dispatchTask)
        {
            string errMsg = string.Empty;
            if (dispatchTask.TaskStatus != DispatchTaskStatus.Dispatched)
                return "任务单[{0}]的状态为：{1},撤销派工失败！\n".L10nFormat(dispatchTask.No,
                    EnumViewModel.EnumToLabel(dispatchTask.TaskStatus).L10N());

            return errMsg;
        }

        /// <summary>
        /// 拆分任务单
        /// </summary>
        /// <param name="splitTaskVM">拆分任务ViewModel</param>
        /// <returns>拆分结果</returns>
        public virtual string SplitDispatchTask(SplitTaskViewModel splitTaskVM)
        {
            string errMsg = string.Empty;
            if (splitTaskVM == null)
            {
                return errMsg;
            }
            try
            {
                SplitDispatchTask(splitTaskVM.DispatchTaskId, splitTaskVM.Qty, splitTaskVM.WipResourceId);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 拆分任务单
        /// </summary>
        /// <param name="dispatchTaskId">被拆分的任务单Id</param>
        /// <param name="splitQty">拆分的数量</param>
        /// <param name="wipResourceId">生产资源Id</param>
        private void SplitDispatchTask(double dispatchTaskId, decimal splitQty, double wipResourceId)
        {
            List<double> dispatchTaskIds = new List<double>();
            EntityList<AssociatedTask> associatedTaskList = new EntityList<AssociatedTask>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (Query<DispatchTaskQueue>().Where(p => p.DispatchTaskId == dispatchTaskId && p.IsFinished == false).Count() > 0)
                    throw new ValidationException("任务单在生产队列中，且未完成，不能参与拆分!".L10N());

                dispatchTaskIds.Add(dispatchTaskId);
                var dispatchTask = GetDispatchTaskList(dispatchTaskIds).FirstOrDefault();
                var reportConfig = RT.Service.Resolve<ReportController>().GetReportRuleConfigByProduct(dispatchTask.ProductId);
                if (reportConfig == null)
                {
                    throw new ValidationException("未找到报工配置规则，请检查规则配置".L10N());
                }
                var associatedTasks = GetAssociatedTaskList(dispatchTaskIds);
                var splitTask = new DispatchTask()
                {
                    No = GetDispatchTaskNo(),
                    DispatchQty = splitQty,
                    SendQty = 0,
                    ReportQty = 0,
                    OkQty = 0,
                    NgQty = 0,
                    TaskPerformer = string.Empty,
                    PlanBeginTime = dispatchTask.PlanBeginTime,
                    PlanEndTime = dispatchTask.PlanEndTime,
                    AssociatedWorkOrder = dispatchTask.AssociatedWorkOrder,
                    IsVirtualPart = dispatchTask.IsVirtualPart,
                    VirtualPartCode = dispatchTask.VirtualPartCode,
                    VirtualPartName = dispatchTask.VirtualPartName,
                    IsSyntype = dispatchTask.IsSyntype,
                    TechNo = dispatchTask.TechNo,
                    Proportion = dispatchTask.Proportion,
                    Associated = Associated.Split,
                    ReportMode = reportConfig.ReportMode,
                    Priority = DispatchTaskPriority.Normal,
                    IsMainTask = true,
                    MergedStatus = MergedStatus.Normal,
                    TaskStatus = DispatchTaskStatus.ToDispatch,
                    ProcessId = dispatchTask.ProcessId,
                    ProductId = dispatchTask.ProductId,
                    SpecificationId = dispatchTask.SpecificationId,
                    ResourceId = wipResourceId,//dispatchTask.ResourceId,
                    WorkShopId = dispatchTask.WorkShopId,
                    WorkOrderId = dispatchTask.WorkOrderId,
                    RoutingProcessId = dispatchTask.RoutingProcessId,
                    Seq = dispatchTask.Seq,
                    StartProcess = dispatchTask.StartProcess,
                    EndProcess = dispatchTask.EndProcess,
                    PersistenceStatus = PersistenceStatus.New,
                    FactoryId = dispatchTask.FactoryId,
                    SourceType = SourceType.Split,
                    IsOutsourcing = dispatchTask.IsOutsourcing
                };
                splitTask.GenerateId();

                foreach (var f in dispatchTask.Boms)
                {
                    splitTask.Boms.Add(new TaskProcessBom
                    {
                        ItemId = f.ItemId,
                        Qty = f.Qty,
                        ProcessId = f.ProcessId,
                        DispatchTaskId = splitTask.Id
                    });
                }

                foreach (var associatedTask in associatedTasks)
                {
                    var task = new AssociatedTask()
                    {
                        DispatchTaskId = splitTask.Id,
                        TaskId = associatedTask.TaskId,
                        PersistenceStatus = PersistenceStatus.New
                    };

                    associatedTaskList.Add(task);
                }

                var taskOfSplit = new AssociatedTask()
                {
                    DispatchTaskId = dispatchTask.Id,
                    TaskId = splitTask.Id,
                    PersistenceStatus = PersistenceStatus.New
                };

                associatedTaskList.Add(taskOfSplit);

                dispatchTask.DispatchQty = dispatchTask.DispatchQty - splitQty;
                dispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                //如果被拆的任务单的已报工数+可疑品数在拆分后等于任务单数量，需要在拆分后将该任务单状态改成已完成
                if (dispatchTask.ReportQty + dispatchTask.SuspectQty == dispatchTask.DispatchQty)
                    dispatchTask.TaskStatus = DispatchTaskStatus.Finished;

                RF.Save(dispatchTask);
                RF.Save(splitTask);
                RF.Save(associatedTaskList);

                //增加拆分任务的自动派工逻辑
                var selectedIds = new List<double>();
                selectedIds.Add(splitTask.Id);
                RT.Service.Resolve<DispatchController>().DispatchTasks(selectedIds);

                tran.Complete();
            }
        }

        /// <summary>
        /// 取消合并任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>取消合并结果</returns>
        public virtual string CancelMergeDispatchTasks(List<double> dispatchTaskIds)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTaskList(dispatchTaskIds);
                ValidateCancelMergeDispatchTasks(dispatchTasks);
                CancelMergeDispatchTask(dispatchTasks);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 验证取消合并的派工任务单
        /// </summary>
        /// <param name="dispatchTasks">派工任务单</param>
        private void ValidateCancelMergeDispatchTasks(EntityList<DispatchTask> dispatchTasks)
        {
            if (dispatchTasks.Any(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.TaskStatus != DispatchTaskStatus.Dispatching))
            {
                throw new ValidationException("取消合并失败，存在非待派工/派工中状态的任务单!".L10N());
            }

            if (dispatchTasks.Any(p => p.MergedStatus != MergedStatus.MergeRows))
            {
                throw new ValidationException("取消合并失败，存在非合并关系的任务单!".L10N());
            }
        }

        /// <summary>
        /// 取消合并任务单
        /// </summary>
        /// <param name="dispatchTasks">任务单列表</param>
        private void CancelMergeDispatchTask(EntityList<DispatchTask> dispatchTasks)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                dispatchTasks.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                var dispatchTaskIds = dispatchTasks.Select(p => p.Id).Distinct().ToList();
                var dispatchTaskDetails = GetDispatchTaskDetails(dispatchTaskIds);
                dispatchTaskDetails.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                var associatedTasks = GetAssociatedTaskList(dispatchTaskIds);
                var associatedTaskIds = associatedTasks.Select(p => p.TaskId).Distinct().ToList();
                var mergedAssociatedTasks = GetMergedDispatchTasks(associatedTaskIds);
                mergedAssociatedTasks.ForEach(p =>
                {
                    p.IsMainTask = true;
                    p.MergedStatus = MergedStatus.Normal;
                    p.Associated = p.OldAssociated;
                    p.OldAssociated = null;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });
                associatedTasks.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                var taskProcessBoms = GetTaskProcessBomList(dispatchTaskIds);
                taskProcessBoms.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                //删除合并记录
                mergedAssociatedTasks.ForEach(task => DeleteTaskMergeRecord(task.Id));
                RF.Save(dispatchTaskDetails);
                RF.Save(mergedAssociatedTasks);
                RF.Save(associatedTasks);
                RF.Save(taskProcessBoms);
                RF.Save(dispatchTasks);
                tran.Complete();
            }
        }

        /// <summary>
        /// 合并派工任务单
        /// </summary>
        /// <param name="dispatchTaskIds">选中的派工任务单Id列表</param>
        public virtual string MergeDispatchTasks(List<double> dispatchTaskIds)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTaskList(dispatchTaskIds);
                foreach (var item in dispatchTasks)
                {
                    if (item.MergedStatus != MergedStatus.Normal)
                    {
                        throw new ValidationException("派工单[{0}]合并状态为[{1}]，不允许操作合并！".L10nFormat(item.No, item.MergedStatus.ToLabel()));
                    }
                    if (item.TaskStatus != DispatchTaskStatus.ToDispatch && item.TaskStatus != DispatchTaskStatus.Dispatching)
                    {
                        throw new ValidationException("派工单[{0}]状态为[{1}]，不允许操作合并！".L10nFormat(item.No, item.TaskStatus.ToLabel()));
                    }
                }
                ValidateMergeDispatchTasks(dispatchTasks);
                CreateMergeDispatchTask(dispatchTasks);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 验证合并的派工任务单
        /// </summary>
        /// <param name="dispatchTasks">派工任务单</param>
        private void ValidateMergeDispatchTasks(EntityList<DispatchTask> dispatchTasks)
        {
            if (dispatchTasks.Any(p => p.TaskStatus != DispatchTaskStatus.ToDispatch && p.TaskStatus != DispatchTaskStatus.Dispatching))
                throw new ValidationException("合并失败，存在非待派工/派工中状态的任务单!".L10N());

            if (dispatchTasks.Any(p => p.MergedStatus == MergedStatus.MergeRows))
                throw new ValidationException("合并失败，存在已合并的任务单!".L10N());

            if (dispatchTasks.Any(p => p.IsSyntype))
                throw new ValidationException("合并失败，存在共模任务单".L10N());

            if (dispatchTasks.Any(p => p.ReportMode == ReportMode.Auto))
                throw new ValidationException("合并失败，存在自动报工的任务单".L10N());

            if (dispatchTasks.Select(p => p.WorkOrder.Type).Distinct().Count() > 1)
                throw new ValidationException("合并失败，任务单工单工单类型不一致".L10N());

            if (dispatchTasks.Select(p => p.ProductId).Distinct().Count() > 1)
                throw new ValidationException("合并失败，任务单产品编码不一致".L10N());

            if (dispatchTasks.Select(p => p.ProcessId).Distinct().Count() > 1)
                throw new ValidationException("合并失败，任务单关联工序不一致".L10N());

            if (dispatchTasks.Any(p => !p.WorkShopId.HasValue))
                throw new ValidationException("合并失败，任务单车间不能为空".L10N());

            if (dispatchTasks.Select(p => p.WorkShopId).Distinct().Count() > 1)
                throw new ValidationException("合并失败，任务单车间不一致".L10N());

            if (dispatchTasks.Any(p => !p.ResourceId.HasValue))
                throw new ValidationException("合并失败，任务单资源不能为空".L10N());

            if (dispatchTasks.Select(p => p.ResourceId).Distinct().Count() > 1)
                throw new ValidationException("合并失败，任务单资源不一致".L10N());

            if (dispatchTasks.Select(p => p.SpecificationId).Distinct().Count() > 1)
                throw new ValidationException("合并失败，任务单规格件编码不一致".L10N());

            if (dispatchTasks.Select(p => p.VirtualPartCode).Distinct().Count() > 1)
                throw new ValidationException("合并失败，任务单虚拟件编码不一致".L10N());

            if (!(dispatchTasks.All(p => p.IsVirtualPart) || dispatchTasks.All(p => !p.IsVirtualPart)))
                throw new ValidationException("合并失败，任务单是否虚拟件不一致".L10N());
        }

        /// <summary>
        /// 创建合并任务单
        /// </summary>
        /// <param name="dispatchTasks">任务单列表</param>
        private void CreateMergeDispatchTask(EntityList<DispatchTask> dispatchTasks)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                EntityList<AssociatedTask> associatedTasks = new EntityList<AssociatedTask>();
                EntityList<TaskProcessBom> taskProcessBoms = new EntityList<TaskProcessBom>();
                var dispatchTaskIds = dispatchTasks.Select(p => p.Id).Distinct().ToList();
                var dispatchTaskDetails = GetDispatchTaskDetails(dispatchTaskIds);
                dispatchTaskDetails.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                dispatchTasks.ForEach(p =>
                {
                    p.MergedStatus = MergedStatus.Merged;
                    p.IsMainTask = false;
                    p.OldAssociated = p.Associated;
                    p.Associated = Associated.Combine;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });
                var firstDispatchTask = dispatchTasks.FirstOrDefault();
                var reportConfig = RT.Service.Resolve<ReportController>().GetReportRuleConfigByProduct(firstDispatchTask.ProductId);
                if (reportConfig == null)
                {
                    throw new ValidationException("未找到报工配置规则，请检查规则配置".L10N());
                }
                var mergeDispatchTask = new DispatchTask()
                {
                    No = GetDispatchTaskNo(),
                    DispatchQty = dispatchTasks.Sum(p => p.DispatchQty),
                    SendQty = 0,
                    OkQty = 0,
                    ReportQty = 0,
                    NgQty = 0,
                    TaskPerformer = string.Empty,
                    PlanBeginTime = dispatchTasks.Min(p => p.PlanBeginTime),
                    PlanEndTime = dispatchTasks.Max(p => p.PlanEndTime),
                    AssociatedWorkOrder = dispatchTasks.Where(x => x.WorkOrder != null).Select(x => x.WorkOrder.No).Distinct().Concat(";"),
                    IsVirtualPart = firstDispatchTask.IsVirtualPart,
                    VirtualPartCode = firstDispatchTask.VirtualPartCode,
                    VirtualPartName = firstDispatchTask.VirtualPartName,
                    IsSyntype = firstDispatchTask.IsSyntype,
                    Proportion = firstDispatchTask.Proportion,
                    TechNo = firstDispatchTask.TechNo,
                    Associated = null,
                    ReportMode = reportConfig.ReportMode,
                    Priority = firstDispatchTask.Priority,
                    IsMainTask = true,
                    MergedStatus = MergedStatus.MergeRows,
                    TaskStatus = DispatchTaskStatus.ToDispatch,
                    ProcessId = firstDispatchTask.ProcessId,
                    ProductId = firstDispatchTask.ProductId,
                    SpecificationId = firstDispatchTask.SpecificationId,
                    ResourceId = firstDispatchTask.ResourceId,
                    WorkShopId = firstDispatchTask.WorkShopId,
                    WorkOrder = null,
                    StartProcess = firstDispatchTask.StartProcess,
                    EndProcess = firstDispatchTask.EndProcess,
                    PersistenceStatus = PersistenceStatus.New,
                    FactoryId = firstDispatchTask.FactoryId,
                    RoutingProcessId = firstDispatchTask.RoutingProcessId
                };

                mergeDispatchTask.GenerateId();
                associatedTasks.AddRange(GetAssociatedTasks(dispatchTasks, mergeDispatchTask));
                //添加任务单合并记录
                string mergeWorkOrder = string.Join(";", dispatchTasks.Where(p => !p.WorkOrderNo.IsNullOrEmpty()).Select(p => p.WorkOrderNo));
                dispatchTasks.Where(p => p.WorkOrderId != null).ForEach(task => AddMergeTaskRecord(task.WorkOrderId.Value, task.Id, mergeWorkOrder));
                RF.Save(dispatchTaskDetails);
                RF.Save(dispatchTasks);
                RF.Save(mergeDispatchTask);
                RF.Save(associatedTasks);
                RF.Save(taskProcessBoms);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取关联任务单列表
        /// </summary>
        /// <param name="dispatchTasks">任务单Id列表</param>
        /// <param name="mergeDispatchTask">合并后的任务单</param>
        /// <returns>关联任务单列表</returns>
        private EntityList<AssociatedTask> GetAssociatedTasks(EntityList<DispatchTask> dispatchTasks, DispatchTask mergeDispatchTask)
        {
            EntityList<AssociatedTask> associatedTasks = new EntityList<AssociatedTask>();

            //合并后，原任务单为合并单的关联任务单
            foreach (var dispatchTask in dispatchTasks)
            {
                var associatedTask = new AssociatedTask()
                {
                    DispatchTaskId = mergeDispatchTask.Id,
                    TaskId = dispatchTask.Id,
                    PersistenceStatus = PersistenceStatus.New
                };

                associatedTasks.Add(associatedTask);
            }

            return associatedTasks;
        }

        /// <summary>
        /// 根据任务Id列表获取已合并的任务单列表
        /// </summary>
        /// <param name="dispatchTaskIds">任务Id列表</param>
        /// <returns>已合并的任务单列表</returns>
        public virtual EntityList<DispatchTask> GetMergedDispatchTasks(List<double> dispatchTaskIds)
        {
            return Query<DispatchTask>().Where(p => dispatchTaskIds.Contains(p.Id) && p.MergedStatus == MergedStatus.Merged && !p.IsMainTask).ToList();
        }

        /// <summary>
        /// 获取已合并的任务单字典
        /// </summary>
        /// <param name="dispatchTaskIds">已选任务单Id列表</param>
        /// <returns>已合并的任务单字典</returns>
        public virtual Dictionary<double, List<DispatchTask>> GetMergedTasks(List<double> dispatchTaskIds)
        {
            var dicMergedDispatchTasks = new Dictionary<double, List<DispatchTask>>();
            var associatedTasks = GetAssociatedTasksOfMergedTask(dispatchTaskIds);
            var groupAssociatedTasks = associatedTasks.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());
            var taskIds = associatedTasks.Select(p => p.TaskId).Distinct().ToList();
            var mergedTasks = GetDispatchTaskList(taskIds);
            var dicMergedTasks = mergedTasks.ToDictionary(p => p.Id);

            foreach (var groupAssociatedTask in groupAssociatedTasks)
            {
                var associatedTaskList = groupAssociatedTask.Value;
                if (!dicMergedDispatchTasks.ContainsKey(groupAssociatedTask.Key))
                    dicMergedDispatchTasks.Add(groupAssociatedTask.Key, new List<DispatchTask>());
                foreach (var associatedTask in associatedTaskList)
                {
                    if (associatedTask.Task.Associated == Associated.Syntype)
                        continue;
                    DispatchTask task = null;
                    if (dicMergedTasks.TryGetValue(associatedTask.TaskId, out task))
                        dicMergedDispatchTasks[groupAssociatedTask.Key].Add(task);
                }
            }

            return dicMergedDispatchTasks;
        }

        /// <summary>
        /// 根据任务单Id列表获取关联任务单信息
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>关联任务单信息</returns>
        private AssociatedDispatchTaskInfo GetAssociatedDispatchTaskInfo(List<double> dispatchTaskIds)
        {
            var associatedDispatchTaskInfo = new AssociatedDispatchTaskInfo();
            var associatedTasks = GetAssociatedTaskList(dispatchTaskIds);
            var groupAssociatedTasks = associatedTasks.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());
            var taskIds = associatedTasks.Select(p => p.TaskId).Distinct().ToList();
            var tasks = GetDispatchTaskList(taskIds);
            var dicTasks = tasks.ToDictionary(p => p.Id);
            var associatedTasksOfTask = GetAssociatedTaskListByTask(taskIds);
            var groupAssociatedTasksOfTask = associatedTasksOfTask.GroupBy(p => p.TaskId).ToDictionary(p => p.Key, p => p.ToList());

            //所有关联任务单的主任务单Id列表
            var allDispatchTaskIds = associatedTasksOfTask.Select(p => p.DispatchTaskId).Distinct().ToList();
            var allDispatchTasks = GetDispatchTaskList(allDispatchTaskIds);
            var dicAllDispatchTasks = allDispatchTasks.ToDictionary(p => p.Id);
            var allDispatchTaskDetails = GetDispatchTaskDetails(allDispatchTaskIds);
            var groupDispatchTaskDetails = allDispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());

            foreach (var groupAssociatedTask in groupAssociatedTasks)
            {
                var associatedTaskList = groupAssociatedTask.Value;
                if (!associatedDispatchTaskInfo.DicSyntypeDispatchTasks.ContainsKey(groupAssociatedTask.Key))
                    associatedDispatchTaskInfo.DicSyntypeDispatchTasks.Add(groupAssociatedTask.Key, new List<DispatchTask>());
                if (!associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.ContainsKey(groupAssociatedTask.Key))
                    associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.Add(groupAssociatedTask.Key, new List<DispatchTask>());

                foreach (var associatedTask in associatedTaskList)
                {
                    if (dicTasks.TryGetValue(associatedTask.TaskId, out DispatchTask task))
                    {
                        if (task.Associated != Associated.Syntype)
                            associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks[groupAssociatedTask.Key].Add(task);
                        else
                            DealSyntypeTasks(associatedDispatchTaskInfo, groupAssociatedTasksOfTask, dicAllDispatchTasks, groupDispatchTaskDetails, groupAssociatedTask, task);
                    }
                }
            }

            return associatedDispatchTaskInfo;
        }

        /// <summary>
        /// 处理共模的任务单列表
        /// </summary>
        /// <param name="associatedDispatchTaskInfo">关联任务单信息</param>
        /// <param name="groupAssociatedTasksOfTask">关联任务单关联任务单列表字典</param>
        /// <param name="dicAllDispatchTasks">所有任务单列表</param>
        /// <param name="groupDispatchTaskDetails">任务单派工任务明细字典</param>
        /// <param name="groupAssociatedTask">当前选中主料任务单关联任务单列表字典</param>
        /// <param name="task">关联任务单</param>
        private void DealSyntypeTasks(AssociatedDispatchTaskInfo associatedDispatchTaskInfo, Dictionary<double, List<AssociatedTask>> groupAssociatedTasksOfTask, Dictionary<double, DispatchTask> dicAllDispatchTasks, Dictionary<double, List<DispatchTaskDetail>> groupDispatchTaskDetails, KeyValuePair<double, List<AssociatedTask>> groupAssociatedTask, DispatchTask task)
        {
            associatedDispatchTaskInfo.DicSyntypeDispatchTasks[groupAssociatedTask.Key].Add(task);
            if (groupAssociatedTasksOfTask.TryGetValue(task.Id, out List<AssociatedTask> tmpAssociatedTasks))
            {
                if (!associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.ContainsKey(task.Id))
                    associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.Add(task.Id, new List<DispatchTask>());
                if (!associatedDispatchTaskInfo.DicSyntypeDispatchTaskDetails.ContainsKey(task.Id))
                    associatedDispatchTaskInfo.DicSyntypeDispatchTaskDetails.Add(task.Id, new List<DispatchTaskDetail>());
                foreach (var tmpAssociatedTask in tmpAssociatedTasks)
                {
                    if (dicAllDispatchTasks.TryGetValue(tmpAssociatedTask.DispatchTaskId, out DispatchTask dispatchTask))
                        associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks[task.Id].Add(dispatchTask);
                    if (groupDispatchTaskDetails.TryGetValue(tmpAssociatedTask.DispatchTaskId, out List<DispatchTaskDetail> taskDetails))
                        associatedDispatchTaskInfo.DicSyntypeDispatchTaskDetails[task.Id].AddRange(taskDetails);
                }
            }
        }

        /// <summary>
        /// 批量设置暂停任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>暂停错误信息</returns>
        public virtual string SetPauseTasks(List<double> dispatchTaskIds)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTasks(dispatchTaskIds);
                if (dispatchTasks.Any(p => p.TaskStatus == DispatchTaskStatus.Pause))
                {
                    throw new ValidationException("任务单已暂停，请刷新界面".L10N());
                }
                //var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(dispatchTaskIds);
                dispatchTasks.ForEach(p =>
                {
                    p.OldTaskStatus = p.TaskStatus;
                    p.TaskStatus = DispatchTaskStatus.Pause;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });

                //备份一份MES回传数据(因后续操作会更改列表数据)
                //List<DispatchTask> orgDispatchTasks = DeepCopy.Clone<DispatchTask>(dispatchTasks.ToList());
                //var dicOrgDispatchTasks = orgDispatchTasks.ToDictionary(p => p.Id);
                //foreach (var orgDispatchTask in orgDispatchTasks)
                //{
                //    List<DispatchTask> notSyntypeDispatchTasks = null;
                //    if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out notSyntypeDispatchTasks))
                //    {
                //        if (notSyntypeDispatchTasks.Any())
                //        {
                //            notSyntypeDispatchTasks.ForEach(p =>
                //            {
                //                p.OldTaskStatus = p.TaskStatus;
                //                p.TaskStatus = DispatchTaskStatus.Pause;
                //                p.PersistenceStatus = PersistenceStatus.Modified;
                //            });
                //            dispatchTasks.AddRange(notSyntypeDispatchTasks);
                //        }
                //    }

                //    List<DispatchTask> syntypeDispatchTasks = null;
                //    if (associatedDispatchTaskInfo.DicSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out syntypeDispatchTasks))
                //    {
                //        foreach (var syntypeDispatchTask in syntypeDispatchTasks)
                //        {
                //            List<DispatchTask> syntypeMainDispatchTasks = null;
                //            if (associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.TryGetValue(syntypeDispatchTask.Id, out syntypeMainDispatchTasks))
                //            {
                //                syntypeMainDispatchTasks.ForEach(p =>
                //                {
                //                    DispatchTask dispatchTask = null;
                //                    if (dicOrgDispatchTasks.TryGetValue(p.Id, out dispatchTask))
                //                    {
                //                        p.TaskStatus = dispatchTask.TaskStatus;
                //                        p.OldTaskStatus = dispatchTask.OldTaskStatus;
                //                    }
                //                });

                //                if (syntypeMainDispatchTasks.All(p => p.TaskStatus == DispatchTaskStatus.Pause))
                //                {
                //                    syntypeDispatchTask.OldTaskStatus = syntypeDispatchTask.TaskStatus;
                //                    syntypeDispatchTask.TaskStatus = DispatchTaskStatus.Pause;
                //                    syntypeDispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                //                    dispatchTasks.Add(syntypeDispatchTask);
                //                }
                //            }
                //        }
                //    }
                //}

                // 更新操作日志
                var log = RT.Service.Resolve<ReportController>().UpdateTaskOptStopLog(dispatchTasks);

                using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(dispatchTasks);
                    RF.Save(log);
                    tran.Complete();
                }
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 批量设置恢复任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>恢复错误信息</returns>
        public virtual string SetResumeTasks(List<double> dispatchTaskIds)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTasks(dispatchTaskIds);
                if (dispatchTasks.Any(p => p.OldTaskStatus == null))
                {
                    throw new ValidationException("任务单已恢复，请刷新界面".L10N());
                }
                var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(dispatchTaskIds);
                dispatchTasks.ForEach(p =>
                {
                    p.TaskStatus = p.OldTaskStatus.Value;
                    p.OldTaskStatus = null;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });

                //恢复关联任务单列表的状态
                ResumeAssociatedDispatchTasks(dispatchTasks, associatedDispatchTaskInfo);

                // 恢复成执行中的数据创建日志
                var executings = dispatchTasks.Where(p => p.TaskStatus == DispatchTaskStatus.Executing).AsEntityList();
                EntityList<ReportOperateLog> logs = new EntityList<ReportOperateLog>();
                if (executings.Count > 0)
                {
                    logs = RT.Service.Resolve<ReportController>().GenerateTaskOptStartLog(executings);
                }
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(dispatchTasks);
                    if (logs.Count > 0)
                    {
                        RF.Save(logs);
                    }
                    tran.Complete();
                }

            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 恢复关联任务单列表的状态
        /// </summary>
        /// <param name="dispatchTasks">可恢复的任务单列表</param>
        /// <param name="associatedDispatchTaskInfo">当前选中恢复的任务单的所有关联任务单信息</param>
        private void ResumeAssociatedDispatchTasks(EntityList<DispatchTask> dispatchTasks, AssociatedDispatchTaskInfo associatedDispatchTaskInfo)
        {
            //备份一份MES回传数据(因后续操作会更改列表数据)
            List<DispatchTask> orgDispatchTasks = DeepCopy.Clone<DispatchTask>(dispatchTasks.ToList());
            var dicOrgDispatchTasks = orgDispatchTasks.ToDictionary(p => p.Id);
            foreach (var orgDispatchTask in orgDispatchTasks)
            {
                if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out List<DispatchTask> notSyntypeDispatchTasks))
                    dispatchTasks.AddRange(ResumeUnSyntypeDispatchTasks(notSyntypeDispatchTasks));
                if (associatedDispatchTaskInfo.DicSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out List<DispatchTask> syntypeDispatchTasks))
                    dispatchTasks.AddRange(ResumeSyntypeDispatchTasks(associatedDispatchTaskInfo, dicOrgDispatchTasks, syntypeDispatchTasks));
            }
        }

        /// <summary>
        /// 恢复共模关联任务单列表
        /// </summary>
        /// <param name="associatedDispatchTaskInfo">当前选中恢复的任务单的所有关联任务单信息</param>
        /// <param name="dicOrgDispatchTasks">当前选中恢复的任务单字典</param>
        /// <param name="syntypeDispatchTasks">共模关联任务单列表</param>
        /// <returns>可恢复的任务单列表</returns>
        private EntityList<DispatchTask> ResumeSyntypeDispatchTasks(AssociatedDispatchTaskInfo associatedDispatchTaskInfo, Dictionary<double, DispatchTask> dicOrgDispatchTasks, List<DispatchTask> syntypeDispatchTasks)
        {
            var dispatchTasks = new EntityList<DispatchTask>();
            foreach (var syntypeDispatchTask in syntypeDispatchTasks)
            {
                if (associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.TryGetValue(syntypeDispatchTask.Id, out List<DispatchTask> syntypeMainDispatchTasks))
                {
                    ResumeMainDispatchTasks(dicOrgDispatchTasks, syntypeMainDispatchTasks);
                    dispatchTasks.AddRange(ResumeDispatchTasks(syntypeDispatchTask));
                }
            }

            return dispatchTasks;
        }

        /// <summary>
        /// 设置共模关联任务单的主料任务单新旧状态
        /// </summary>
        /// <param name="dicOrgDispatchTasks">当前选中恢复的任务单字典</param>
        /// <param name="syntypeMainDispatchTasks">共模关联任务单对应的所有主料任务单列表</param>
        private void ResumeMainDispatchTasks(Dictionary<double, DispatchTask> dicOrgDispatchTasks, List<DispatchTask> syntypeMainDispatchTasks)
        {
            syntypeMainDispatchTasks.ForEach(p =>
            {
                if (dicOrgDispatchTasks.TryGetValue(p.Id, out DispatchTask dispatchTask))
                {
                    p.TaskStatus = dispatchTask.TaskStatus;
                    p.OldTaskStatus = dispatchTask.OldTaskStatus;
                }
            });
        }

        /// <summary>
        /// 恢复共模关联任务单列表
        /// </summary>
        /// <param name="syntypeDispatchTask">共模关联任务单列表</param>
        /// <returns>可恢复的任务单列表</returns>
        private EntityList<DispatchTask> ResumeDispatchTasks(DispatchTask syntypeDispatchTask)
        {
            var dispatchTasks = new EntityList<DispatchTask>();
            if (syntypeDispatchTask.TaskStatus == DispatchTaskStatus.Pause)
            {
                if (syntypeDispatchTask.OldTaskStatus.HasValue)
                    syntypeDispatchTask.TaskStatus = syntypeDispatchTask.OldTaskStatus.Value;
                syntypeDispatchTask.OldTaskStatus = null;
                syntypeDispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                dispatchTasks.Add(syntypeDispatchTask);
            }

            return dispatchTasks;
        }

        /// <summary>
        /// 恢复非共模关联任务单列表
        /// </summary>
        /// <param name="notSyntypeDispatchTasks">非共模关联任务单列表</param>
        /// <returns>可恢复的任务单列表</returns>
        private EntityList<DispatchTask> ResumeUnSyntypeDispatchTasks(List<DispatchTask> notSyntypeDispatchTasks)
        {
            var dispatchTasks = new EntityList<DispatchTask>();
            notSyntypeDispatchTasks.Where(p => p.OldTaskStatus != null).ForEach(p =>
            {
                p.TaskStatus = p.OldTaskStatus.Value;
                p.OldTaskStatus = null;
                p.PersistenceStatus = PersistenceStatus.Modified;
            });
            dispatchTasks.AddRange(notSyntypeDispatchTasks);

            return dispatchTasks;
        }

        /// <summary>
        /// 批量设置强制关闭任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>强制关闭错误信息</returns>
        public virtual string SetCloseTasks(List<double> dispatchTaskIds)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTasks(dispatchTaskIds);
                var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(dispatchTaskIds);

                dispatchTasks.ForEach(p =>
                {
                    p.TaskStatus = DispatchTaskStatus.Closed;
                    //sp.OldTaskStatus = null;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });

                //备份一份MES回传数据(因后续操作会更改列表数据)
                List<DispatchTask> orgDispatchTasks = DeepCopy.Clone<DispatchTask>(dispatchTasks.ToList());
                var dicOrgDispatchTasks = orgDispatchTasks.ToDictionary(p => p.Id);
                foreach (var orgDispatchTask in orgDispatchTasks)
                {
                    List<DispatchTask> notSyntypeDispatchTasks = null;
                    if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out notSyntypeDispatchTasks))
                    {
                        if (notSyntypeDispatchTasks.Any())
                        {
                            notSyntypeDispatchTasks.ForEach(p =>
                            {
                                p.TaskStatus = DispatchTaskStatus.Closed;
                                //p.OldTaskStatus = null;
                                p.PersistenceStatus = PersistenceStatus.Modified;
                            });
                            dispatchTasks.AddRange(notSyntypeDispatchTasks);
                        }
                    }

                    List<DispatchTask> syntypeDispatchTasks = null;
                    if (associatedDispatchTaskInfo.DicSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out syntypeDispatchTasks))
                    {
                        foreach (var syntypeDispatchTask in syntypeDispatchTasks)
                        {
                            List<DispatchTask> syntypeMainDispatchTasks = null;
                            if (associatedDispatchTaskInfo.DicSyntypeMainDispatchTasks.TryGetValue(syntypeDispatchTask.Id, out syntypeMainDispatchTasks))
                            {
                                syntypeMainDispatchTasks.ForEach(p =>
                                {
                                    DispatchTask dispatchTask = null;
                                    if (dicOrgDispatchTasks.TryGetValue(p.Id, out dispatchTask))
                                    {
                                        p.TaskStatus = dispatchTask.TaskStatus;
                                        //p.OldTaskStatus = null;
                                    }
                                });

                                if (syntypeMainDispatchTasks.All(p => p.TaskStatus == DispatchTaskStatus.Closed))
                                {
                                    syntypeDispatchTask.TaskStatus = DispatchTaskStatus.Closed;
                                    //syntypeDispatchTask.OldTaskStatus = null;
                                    syntypeDispatchTask.PersistenceStatus = PersistenceStatus.Modified;
                                    dispatchTasks.Add(syntypeDispatchTask);
                                }
                            }
                        }
                    }
                }

                RF.Save(dispatchTasks);
                RT.EventBus.Publish(new DispatchTaskClose(dispatchTasks));
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 批量设置普通任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>设置普通错误信息</returns>
        public virtual string SetNormalTasks(List<double> dispatchTaskIds)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTasks(dispatchTaskIds);
                var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(dispatchTaskIds);
                dispatchTasks.ForEach(p =>
                {
                    p.Priority = DispatchTaskPriority.Normal;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });

                //备份一份MES回传数据(因后续操作会更改列表数据)
                List<DispatchTask> orgDispatchTasks = DeepCopy.Clone<DispatchTask>(dispatchTasks.ToList());
                foreach (var orgDispatchTask in orgDispatchTasks)
                {
                    List<DispatchTask> notSyntypeDispatchTasks = null;
                    if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out notSyntypeDispatchTasks))
                    {
                        notSyntypeDispatchTasks.ForEach(p =>
                        {
                            p.Priority = DispatchTaskPriority.Normal;
                            p.PersistenceStatus = PersistenceStatus.Modified;
                        });
                        dispatchTasks.AddRange(notSyntypeDispatchTasks);
                    }
                }

                RF.Save(dispatchTasks);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 批量设置紧急任务单
        /// </summary>
        /// <param name="dispatchTaskIds">任务单Id列表</param>
        /// <returns>设置紧急错误信息</returns>
        public virtual string SetUrgentTasks(List<double> dispatchTaskIds)
        {
            string errMsg = string.Empty;
            try
            {
                var dispatchTasks = GetDispatchTasks(dispatchTaskIds);
                var associatedDispatchTaskInfo = GetAssociatedDispatchTaskInfo(dispatchTaskIds);
                dispatchTasks.ForEach(p =>
                {
                    p.Priority = DispatchTaskPriority.Urgency;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });

                //备份一份MES回传数据(因后续操作会更改列表数据)
                List<DispatchTask> orgDispatchTasks = DeepCopy.Clone<DispatchTask>(dispatchTasks.ToList());
                foreach (var orgDispatchTask in orgDispatchTasks)
                {
                    List<DispatchTask> notSyntypeDispatchTasks = null;
                    if (associatedDispatchTaskInfo.DicNotSyntypeDispatchTasks.TryGetValue(orgDispatchTask.Id, out notSyntypeDispatchTasks))
                    {
                        notSyntypeDispatchTasks.ForEach(p =>
                        {
                            p.Priority = DispatchTaskPriority.Urgency;
                            p.PersistenceStatus = PersistenceStatus.Modified;
                        });
                        dispatchTasks.AddRange(notSyntypeDispatchTasks);
                    }
                }

                RF.Save(dispatchTasks);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }
        /// <summary>
        /// 删除派工数据
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void DeleteDispatch(double[] selectedIds)
        {
            var selectEntitys = Query<DispatchTask>().Where(m => selectedIds.Contains(m.Id)).ToList();
            if (selectEntitys.Count == 0)
            {
                throw new ValidationException("系统已不存在提交的派工数据，请检查".L10N());
            }
            //“已报工数”为“0”、列字段“任务单状态”为“派工中/待派工”
            if (selectEntitys.Any(m => m.ReportQty != 0 || (m.TaskStatus != DispatchTaskStatus.Dispatching && m.TaskStatus != DispatchTaskStatus.ToDispatch)))
            {
                throw new ValidationException("已有报工数或任务单状态不适用，不能进行删除，请核查".L10N());
            }
            selectEntitys.ForEach(m => m.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(selectEntitys);
        }
        #endregion

        #region 生产任务队列

        /// <summary>
        /// 获取生产队列任务
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isFinish"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTaskQueue> GetDispatchTaskQueueList(List<double> ids, bool? isFinish = null, EagerLoadOptions eagerLoad = null)
        {
            var q = Query<DispatchTaskQueue>().Where(p => ids.Contains(p.Id));
            //q.Exists<DispatchTask>((x, y) => y.Where(p => p.Id == x.DispatchTaskId && p.TaskStatus != DispatchTaskStatus.Closed));
            if (isFinish != null)
                q.Where(p => p.IsFinished == isFinish);
            var list = q.OrderBy(p => p.Seq).ToList(null, eagerLoad);

            //获取父物料
            var productIds = list.Select(p => p.ProductId).Distinct().ToList();
            var parentItems = RT.Service.Resolve<ItemController>().GetParentItemsByItemIds(productIds);

            //获取分单数量
            var woIds = list.Select(p => p.WorkOrderId).Distinct().ToList();
            var layoutInfos = Query<LayoutInfo>().Where(p => woIds.Contains(p.WorkOrderId)).ToList();

            foreach (var task in list)
            {
                var parentItem = parentItems.FirstOrDefault(p => p.ItemId == task.ProductId);
                if (parentItem != null)
                    task.ParShortDescription = parentItem.Bismt;

                var layoutInfo = layoutInfos.FirstOrDefault(p => p.WorkOrderId == task.WorkOrderId && p.ProcessCode == task.ProcessCode);
                if (layoutInfo != null)
                    task.Zcode = layoutInfo.Zcode;
            }

            return list;
        }
        /// <summary>
        /// 获取生产队列任务
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="isFinish"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual EntityList<DispatchTaskQueue> GetDispatchTaskQueueList(double resourceId, bool? isFinish = null, EagerLoadOptions eagerLoad = null)
        {
            //移除已经删除的任务单
            var noExistTasks = Query<DispatchTaskQueue>().Where(p => p.ResourceId == resourceId)
                .NotExists<DispatchTask>((x, y) => y.Where(p => p.Id == x.DispatchTaskId))
                .ToList();
            if (noExistTasks.Count > 0)
            {
                var ids = noExistTasks.Select(p => p.Id).ToList();
                DB.Delete<DispatchTaskQueue>().Where(p => ids.Contains(p.Id)).Execute();
            }

            //移除已经关闭的任务单
            var closeTasks = Query<DispatchTaskQueue>().Where(p => p.ResourceId == resourceId && !p.IsFinished)
                .Exists<DispatchTask>((x, y) => y.Where(p => p.Id == x.DispatchTaskId && p.TaskStatus == DispatchTaskStatus.Closed))
                .ToList();
            if (closeTasks.Count > 0)
            {
                var ids = closeTasks.Select(p => p.Id).ToList();
                DB.Delete<DispatchTaskQueue>().Where(p => ids.Contains(p.Id)).Execute();
            }
            //标识已经完成的任务单
            var finishTasks = Query<DispatchTaskQueue>().Where(p => p.ResourceId == resourceId && !p.IsFinished)
                .Exists<DispatchTask>((x, y) => y.Where(p => p.Id == x.DispatchTaskId && p.TaskStatus == DispatchTaskStatus.Finished))
                .ToList();
            if (finishTasks.Count > 0)
            {
                var ids = finishTasks.Select(p => p.Id).ToList();
                FinishQueueTask(ids);
            }

            var q = Query<DispatchTaskQueue>().Where(p => p.ResourceId == resourceId);
            //q.Exists<DispatchTask>((x, y) => y.Where(p => p.Id == x.DispatchTaskId && p.TaskStatus != DispatchTaskStatus.Closed));
            if (isFinish != null)
                q.Where(p => p.IsFinished == isFinish);
            var list = q.OrderBy(p => p.Seq).ToList(null, eagerLoad);

            var productIds = list.Select(p => p.ProductId).Distinct().ToList();
            var parentItems = RT.Service.Resolve<ItemController>().GetParentItemsByItemIds(productIds);
            foreach (var task in list)
            {
                var parentItem = parentItems.FirstOrDefault(p => p.ItemId == task.ProductId);
                if (parentItem != null)
                    task.ParShortDescription = parentItem.Bismt;
            }

            return list;
        }

        /// <summary>
        /// 完成任务队列
        /// </summary>
        /// <param name="ids"></param>
        public virtual int FinishQueueTask(List<double> ids)
        {
            return DB.Update<DispatchTaskQueue>().Set(p => p.IsFinished, true).Where(p => ids.Contains(p.Id) && !p.IsFinished).Execute();
        }
        /// <summary>
        /// 获取生产队列最大顺序
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public virtual int GetMaxDispatchTaskQueueSeq(double resourceId)
        {
            var queue = Query<DispatchTaskQueue>().Where(p => p.ResourceId == resourceId && !p.IsFinished)
                .OrderByDescending(p => p.Seq).FirstOrDefault();
            return queue?.Seq ?? 0;
        }
        #endregion

        #region 工序标签队列

        /// <summary>
        /// 获取标签队列任务
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isFinish"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual EntityList<WipBatchQueue> GetWipBatchQueueList(List<double> ids, bool? isFinish = null, EagerLoadOptions eagerLoad = null)
        {
            var q = Query<WipBatchQueue>().Where(p => ids.Contains(p.Id));

            if (isFinish != null)
                q.Where(p => p.IsFinished == isFinish);
            var list = q.OrderBy(p => p.Seq).ToList(null, eagerLoad);

            return list;
        }
        /// <summary>
        /// 获取标签队列任务
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="isFinish"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual EntityList<WipBatchQueue> GetWipBatchQueueList(double resourceId, double processId, bool? isFinish = null, EagerLoadOptions eagerLoad = null)
        {
            //移除已删除的数据
            var noExistTasks = Query<WipBatchQueue>().Where(p => p.ResourceId == resourceId && p.ProcessId == processId)
                .NotExists<WipBatch>((x, y) => y.Where(p => p.Id == x.WipBatchId))
                .ToList();
            if (noExistTasks.Count > 0)
            {
                var ids = noExistTasks.Select(p => p.Id).ToList();
                DB.Delete<WipBatchQueue>().Where(p => ids.Contains(p.Id)).Execute();
            }

            //移除报废或者可疑品的数据
            var closeTasks = Query<WipBatchQueue>().Where(p => p.ResourceId == resourceId && p.ProcessId == processId && !p.IsFinished)
                .Exists<WipBatch>((x, y) => y.Where(p => p.Id == x.WipBatchId && (p.Qty == 0 || p.IsScraped == true || p.IsSuspectProduct == YesNo.Yes)))
                .ToList();
            if (closeTasks.Count > 0)
            {
                var ids = closeTasks.Select(p => p.Id).ToList();
                DB.Delete<WipBatchQueue>().Where(p => ids.Contains(p.Id)).Execute();
            }

            var q = Query<WipBatchQueue>().Where(p => p.ResourceId == resourceId && p.ProcessId == processId);

            if (isFinish != null)
                q.Where(p => p.IsFinished == isFinish);
            var list = q.OrderBy(p => p.Seq).ToList(null, eagerLoad);


            return list;
        }
        /// <summary>
        /// 添加标签队列任务
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public virtual WipBatchQueue AddQueueWipBatch(double resourceId, double processId, string batchNo)
        {
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(batchNo);
            if (wipBatch == null)
                throw new ValidationException("标签[{0}]不存在".L10nFormat(batchNo));
            if (wipBatch.Qty == 0)
                throw new ValidationException("标签[{0}]批次数量为0,请确认".L10nFormat(batchNo));
            if (wipBatch.IsScraped)
                throw new ValidationException("标签[{0}]已报废,请确认".L10nFormat(batchNo));
            if (wipBatch.IsSuspectProduct == YesNo.Yes)
                throw new ValidationException("标签[{0}]为可疑品标签,请确认".L10nFormat(batchNo));

            //校验当前工序是否已报工
            var reportWibatch = Query<ReportWipBatch>().Where(p => p.WipBatchId == wipBatch.Id && p.ReportRecord.ProcessId == processId).FirstOrDefault();
            if (reportWibatch != null)
            {
                throw new ValidationException("标签[{0}]已存在当前工序任务的报工记录,请确认".L10nFormat(wipBatch.BatchNo));
            }

            var queue = new WipBatchQueue
            {
                ResourceId = resourceId,
                ProcessId = processId,
                WipBatchId = wipBatch.Id,
                Seq = GetMaxWipBatchQueueSeq(resourceId, processId) + 10,
                IsFinished = false
            };
            RF.Save(queue);

            queue = RF.GetById<WipBatchQueue>(queue.Id, new EagerLoadOptions().LoadWithViewProperty());
            return queue;
        }

        /// <summary>
        /// 删除队列数据
        /// </summary>
        /// <param name="queueIds"></param>
        /// <returns></returns>
        public virtual int RemoveQueueWipBatch(List<double> queueIds)
        {
            return DB.Delete<WipBatchQueue>().Where(p => queueIds.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 检查是否已报工
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public virtual bool CheckIsReport(WipBatchQueue queue)
        {
            if (queue == null)
                throw new ValidationException("标签队列数据异常,请确认".L10N());
            queue = RF.GetById<WipBatchQueue>(queue.Id);
            if (queue.IsFinished)
                return true;
            var reportWibatch = Query<ReportWipBatch>().Where(p => p.WipBatchId == queue.WipBatchId && p.ReportRecord.ProcessId == queue.ProcessId).FirstOrDefault();
            if (reportWibatch != null)
            {
                FinishWipBatchQueue(new List<double>() { queue.Id });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 完成任务队列
        /// </summary>
        /// <param name="ids"></param>
        public virtual int FinishWipBatchQueue(List<double> ids)
        {
            return DB.Update<WipBatchQueue>().Set(p => p.IsFinished, true).Where(p => ids.Contains(p.Id) && !p.IsFinished).Execute();
        }
        /// <summary>
        /// 获取生产队列最大顺序
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual int GetMaxWipBatchQueueSeq(double resourceId, double processId)
        {
            var queue = Query<WipBatchQueue>().Where(p => p.ResourceId == resourceId && p.ProcessId == processId && !p.IsFinished)
                .OrderByDescending(p => p.Seq).FirstOrDefault();
            return queue?.Seq ?? 0;
        }
        #endregion

        /// <summary>
        /// 获取任务单换轴记录
        /// </summary>
        /// <param name="taskNo"></param>
        /// <param name="iotEntity"></param>
        /// <param name="isReport"></param>
        /// <returns></returns>
        public virtual EntityList<AxisChangeRecord> GetAxisChangeRecords(string taskNo, string iotEntity, bool? isReport = null)
        {
            //过滤换轴米数为0的数据
            var ret = DB.Update<AxisChangeRecord>().Set(p => p.IsReport, true)
                .Set(p => p.Remark, "换轴米数异常,不做处理")
                .Where(p => p.IotEntity == iotEntity && p.ChangeFlag == true && p.IsReport == false && (p.AxisQty == 0 || p.AxisQty == null))
                .Execute();

            var q = Query<AxisChangeRecord>().Where(p => p.TaskNo == taskNo)
                .WhereIf(iotEntity.IsNotEmpty(), p => p.IotEntity == iotEntity)
                .WhereIf(isReport != null, p => p.IsReport == isReport);

            return q.OrderBy(p => p.CollectionTime).ToList();
        }

        /// <summary>
        /// 增加IOT累计数量
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public virtual int AddTaskIotQty(double taskId, decimal qty)
        {
            return DB.Update<DispatchTask>().Set(p => p.IotQty, p => p.IotQty + qty).Where(p => p.Id == taskId).Execute();
        }

        /// <summary>
        /// 设置产品穴位
        /// </summary>
        /// <param name="modelCavityCount"></param>
        /// <param name="queues"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<DispatchTaskQueue> SetCavityCount(int modelCavityCount, EntityList<DispatchTaskQueue> queues)
        {
            if (modelCavityCount == 0 || queues.Count == 0)
                return queues;
            var cavityCount = (int)Math.Ceiling((decimal)modelCavityCount / queues.Count);
            foreach (var queue in queues)
            {
                //if (queue.CavityCount == 0)
                {
                    DB.Update<DispatchTask>().Set(p => p.CavityCount, cavityCount).Where(p => p.Id == queue.DispatchTaskId).Execute();
                    queue.DispatchTask.CavityCount = cavityCount;
                    queue.DispatchTask.MarkSaved();
                    queue.LoadProperty(DispatchTaskQueue.CavityCountProperty, cavityCount);
                }
            }

            return queues;
        }
    }
}