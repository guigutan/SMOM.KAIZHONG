using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualBasic;
using SIE.Common.Configs;
using SIE.Core.RedisUtil;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.MES.EmpWork;
using SIE.MES.ForWinform;
using SIE.MES.ItemChecker;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemFixture;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SchedulingInfs.Configs;
using SIE.MES.TaskManagement.SchedulingInfs.Reports;
using SIE.MES.TaskManagement.SchedulingInfs.ViewModels;
using SIE.MES.TaskManagement.TaskConfigs;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.Packages.ItemLabels;
using SIE.ProductIntfc.ProductInsps;
using SIE.Rbac.InvOrgs;
using SIE.Resources.PersonnelSkills;
using SIE.Resources.Skills;
using SIE.Resources.WipResources;
using SIE.Resources.WorkCenters;
using SIE.Tech.Processs;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs
{
    public class SchedulingInfController : DomainController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="mrps"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfValue> GetSchedulingInfValue(List<double> productIds, List<string> mrps)
        {
            var schedulingInfs = Query<SchedulingInf>()
                .Exists<Process>((d, p) => p.Where(w => w.Id == d.ProcessId && w.Code == "精加工"))
                 .Where(p => mrps.Contains(p.WorkOrder.WorkShop.Code)).Select(p => p.Id).ToList<double>().ToList();

            var entityList = schedulingInfs.SplitContains(ids =>
            {
                var query = Query<SchedulingInfValue>()
              .Where(y => ids.Contains(y.SchedulingInfId));
                var hour = DateTime.Now.Hour;
                if (hour >= 8)
                    query.Where(p => p.Date >= DateTime.Now.Date && p.Date <= DateTime.Now.AddDays(2));
                else
                    query.Where(p => p.Date >= DateTime.Now.Date && p.Date <= DateTime.Now.AddDays(3));
                return query.ToList(null, new EagerLoadOptions().LoadWith(SchedulingInfValue.SchedulingInfProperty)
                 .LoadWith(SchedulingInf.ItemProperty).LoadWithViewProperty());
            });

            return entityList;
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="viewModel"></param>
        public virtual void SchedulingInfCancel(SchedulingInfCancelViewModel viewModel)
        {
            var ids = viewModel.Ids.Split(',').Select(p => Convert.ToDouble(p)).Distinct().ToList();
            var values = Query<SchedulingInfValue>().Where(p => ids.Contains(p.SchedulingInfId) && (p.Value1 != null || p.Value2 != null)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (values.Any(p => (p.TaskStatus1 != null && p.TaskStatus1 != DispatchTaskStatus.Dispatching && p.TaskStatus1 != DispatchTaskStatus.ToDispatch) || (p.TaskStatus2 != null && p.TaskStatus2 != DispatchTaskStatus.Dispatching && p.TaskStatus2 != DispatchTaskStatus.ToDispatch)))
                throw new ValidationException("存在非待派工、非派工中的任务单，不能作废!".L10N());

            //var DispatchTaskIds = values.Where(p => p.DispatchTask1Id != null).Select(p => p.DispatchTask1Id).Distinct().ToList();
            //DispatchTaskIds.AddRange(values.Where(p => p.DispatchTask2Id != null).Select(p => p.DispatchTask2Id).Distinct().ToList());
            //var tasks = Query<DispatchTask>().Where(p => DispatchTaskIds.Contains(p.Id)).ToList();

            var curTime = DateTime.Now;
            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {

                foreach (var value in values)
                {
                    //待派工状态的，直接排程退回
                    if (value.Value1 != null)
                    {
                        if (value.DispatchTask1Id != null && value.TaskStatus1 != DispatchTaskStatus.ToDispatch && value.TaskStatus1 != DispatchTaskStatus.Dispatching)
                        {
                        }
                        else
                        {
                            var task = value.DispatchTask1;
                            if (task != null)
                            {
                                task.IsSchedulingInfReturn = YesNo.Yes;
                                task.SchedulingInfReturnReason = viewModel.Reason;
                                task.PersistenceStatus = PersistenceStatus.Modified;
                                RF.Save(task);

                                RT.Service.Resolve<DispatchController>().SetPauseTasks(new List<double>() { task.Id });
                                //关闭任务单
                                RT.Service.Resolve<DispatchController>().SetCloseTasks(new List<double>() { task.Id });

                            }

                            value.DispatchTask1Id = null;
                            value.Value1 = null;
                            value.PersistenceStatus = PersistenceStatus.Modified;
                        }
                    }
                    //将数量清空
                    if (value.Value2 != null)
                    {
                        if (value.DispatchTask2Id != null && value.TaskStatus2 != DispatchTaskStatus.ToDispatch && value.TaskStatus2 != DispatchTaskStatus.Dispatching)
                        {
                        }
                        else
                        {
                            var task = value.DispatchTask2;
                            if (task != null)
                            {
                                task.IsSchedulingInfReturn = YesNo.Yes;
                                task.SchedulingInfReturnReason = viewModel.Reason;
                                task.PersistenceStatus = PersistenceStatus.Modified;
                                RF.Save(task);

                                RT.Service.Resolve<DispatchController>().SetPauseTasks(new List<double>() { task.Id });
                                //关闭任务单
                                RT.Service.Resolve<DispatchController>().SetCloseTasks(new List<double>() { task.Id });
                            }

                            value.DispatchTask2Id = null;
                            value.Value2 = null;
                            value.PersistenceStatus = PersistenceStatus.Modified;
                        }
                    }
                    RF.Save(value);
                }

                //将排程作废掉
                DB.Update<SchedulingInf>().Set(p => p.IsCancel, true).Set(p => p.CancelReason, viewModel.Reason).Set(p => p.CancelTime, curTime).Set(p => p.CancerId, RT.IdentityId).Where(p => ids.Contains(p.Id)).Execute();
                tran.Complete();
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="values"></param>
        public virtual void UpdateSchedulingInfValueTaskId(double taskId)
        {
            var values = GetSchedulingInfsByTaskIds(new List<double>() { taskId });
            if (values.Count > 0)
            {
                values.ForEach(p =>
                {
                    if (p.DispatchTask1Id == taskId)
                        p.DispatchTask1Id = null;
                    else
                        p.DispatchTask2Id = null;
                    p.PersistenceStatus = PersistenceStatus.Modified;
                });
                RF.Save(values);
            }
        }

        /// <summary>
        /// 根据工单Id获取排程中间表
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInf> GetSchedulingInfsByWoIds(List<double> workOrderId)
        {
            var list = workOrderId.SplitContains(ids =>
            {
                return Query<SchedulingInf>().Where(p => ids.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据派工任务获取排程导入值
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfValue> GetSchedulingInfsByTaskIds(List<double> taskIds)
        {
            var schedulingInfValues = Query<SchedulingInfValue>().Where(p => taskIds.Contains((double)p.DispatchTask1Id) || taskIds.Contains((double)p.DispatchTask2Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return schedulingInfValues;
        }

        /// <summary>
        /// 排程导入详情查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfReport> CriteriaSchedulingInfReport(SchedulingInfReportCriteria criteria)
        {
            var q = Query<SchedulingInfReport>();
            if (criteria.IsImport != null)
            {
                q.Where(p => p.IsImport == criteria.IsImport);
            }

            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                q.Where(p => p.WorkOrderNo.Contains(criteria.WorkOrderNo));
            if (!criteria.ProductCode.IsNullOrEmpty())
                q.Where(p => p.ProductCode.Contains(criteria.ProductCode));
            if (!criteria.ProductName.IsNullOrEmpty())
                q.Where(p => p.ProductName.Contains(criteria.ProductName));
            if (!criteria.ProcessCode.IsNullOrEmpty())
                q.Where(p => p.ProcessCode.Contains(criteria.ProcessCode));
            if (!criteria.ProcessName.IsNullOrEmpty())
                q.Where(p => p.ProcessName.Contains(criteria.ProcessName));
            if (criteria.State != null)
                q.Where(p => p.State == criteria.State);
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.ShortDescription.Contains(criteria.ShortDescription));
            if (criteria.UpdateDate.BeginValue != null)
                q.Where(p => p.UpdateDate >= criteria.UpdateDate.BeginValue.Value);
            if (criteria.UpdateDate.EndValue != null)
                q.Where(p => p.UpdateDate <= criteria.UpdateDate.EndValue.Value);
            if (criteria.IsSchedulingInfReturn != null)
                q.Where(p => p.IsSchedulingInfReturn == criteria.IsSchedulingInfReturn);
            if (criteria.ImportTime.BeginValue != null)
                q.Where(p => p.ImportTime >= criteria.ImportTime.BeginValue);
            if (criteria.ImportTime.EndValue != null)
                q.Where(p => p.ImportTime <= criteria.ImportTime.EndValue);
            if (criteria.IsCheck != null)
            {
                if (criteria.IsCheck == YesNo.Yes)
                    q.Where(p => p.IsCheck == true);
                else
                    q.Where(p => p.IsCheck == false || p.IsCheck == null);
            }
            if (criteria.IsGenerateTask != null)
            {
                q.Where(p => p.IsGenerateTask == criteria.IsGenerateTask);
            }
            if (!criteria.Mrb.IsNullOrEmpty())
                q.Where(p => p.Mrb.Contains(criteria.Mrb));
            var list = q.Where(p => p.InvOrgId == RT.InvOrg).OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var workOrderNos = list.Select(p => p.WorkOrderNo).Distinct().ToList();
            var processCodes = list.Select(p => p.ProcessCode).Distinct().ToList();

            //根据工单+工序找出全部派工任务单
            var tasks = processCodes.SplitContains(codes => {
                return workOrderNos.SplitContains(nos =>
                {
                    return Query<DispatchTask>().Where(p => nos.Contains(p.WorkOrder.No) && codes.Contains(p.Process.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });      

            foreach (var l in list)
            {      
                l.WaitSchedulingQty = l.PlanQty - (l.SchedulingQty ?? 0);
                var ltasks = tasks.Where(p => p.ProcessCode == l.ProcessCode && p.WorkOrderNo == l.WorkOrderNo).ToList();
                //工单+工序的已报工数量合计=0，且任务单状态为已派工、派工中、待派工时，状态为已派工
                //工单+工序的已报工数量合计=0，且任务单状态为派工中、待派工时，则状态为派工中
                //工单+工序的已报工数量合计=0，且任务单状态为待派工时，则状态为待派工
                //工单+工序的已报工数量合计>0<任务数量，则状态为执行中
                //工单+工序的已报工数量合计=任务数量，则状态为已完成
                if (ltasks.Sum(p => p.ReportQty) == 0)
                {
                    if (ltasks.Any(p => p.TaskStatus == DispatchTaskStatus.Dispatched))
                    {
                        l.TaskStatus = DispatchTaskStatus.Dispatched;
                    }
                    else if (ltasks.Any(p => p.TaskStatus == DispatchTaskStatus.Dispatching))
                    {
                        l.TaskStatus = DispatchTaskStatus.Dispatching;
                    }
                    else
                    {
                        l.TaskStatus = DispatchTaskStatus.ToDispatch;
                    }
                }
                else if (ltasks.Sum(p => p.ReportQty) > 0 && ltasks.Sum(p => p.ReportQty) < ltasks.Sum(p => p.DispatchQty))
                {
                    l.TaskStatus = DispatchTaskStatus.Executing;
                }
                else if(ltasks.Sum(p=>p.ReportQty) == ltasks.Sum(p => p.DispatchQty))
                {
                    l.TaskStatus = DispatchTaskStatus.Finished;
                }
            }
            return list;
        }


        /// <summary>
        /// MES排程导入中间表校验方法
        /// </summary>
        public virtual void SchedulingInfValidJob()
        {
            //找出未校验过的或者校验失败的数据
            var infs = Query<SchedulingInf>().Where(p => p.IsCheck == null || p.IsCheck == false).Where(p => p.IsCancel == null || p.IsCancel == false).OrderByDescending(p => p.UpdateDate).ToList(new PagingInfo() { PageNumber = 1, PageSize = 500 }, new EagerLoadOptions().LoadWithViewProperty());
            //获取它们Id
            var ids = infs.Select(p => p.Id).Distinct().ToList();
            //执行校验
            ValidSchedulingInf(ids);
        }

        /// <summary>
        /// MES排程导入中间表生成派工任务单调度执行方法
        /// </summary>
        /// <returns></returns>
        public virtual string SchedulingInfGenerateTaskJob()
        {
            //获取校验成功的数据
            var infs = Query<SchedulingInf>().Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id && y.State != Core.WorkOrders.WorkOrderState.Close && y.State != Core.WorkOrders.WorkOrderState.Finish && y.IsPause == YesNo.No).Where(p => p.IsCheck == true && (p.IsCancel == false || p.IsCancel == null)).OrderByDescending(p => p.UpdateDate).Exists<SchedulingInfValue>((x, y) => y.Where(p => p.SchedulingInfId == x.Id && p.Date >= DateTime.Now.Date.AddDays(-1) && ((p.Value1 != null && p.DispatchTask1Id == null) || (p.Value2 != null && p.DispatchTask2Id == null)))).ToList(new PagingInfo() { PageNumber = 1, PageSize = 1000 }, new EagerLoadOptions().LoadWithViewProperty());
            //获取Id
            var ids = infs.Select(p => p.Id).Distinct().ToList();
            //执行生成派工单任务
            var str = GenerateTask(ids);
            return str.IsNullOrEmpty() ? string.Empty : $"执行失败，错误信息: {str}";
        }

        /// <summary>
        /// 生成任务单
        /// </summary>
        /// <param name="ids"></param>
        public virtual string GenerateTask(List<double> ids)
        {
            //redis锁，防止并发
            var msg = string.Empty;
            var lockKey = "SchedulingInfController_GenerateTask" + RT.InvOrg;
            RedisUtil.LockToDo(lockKey, lockKey, "排程下发任务单", () =>
            {
                var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

                var config = ConfigService.GetConfig(new SchedulingInfCheckConfig(), typeof(SchedulingInf));
                if (config == null)
                    throw new ValidationException("未找到排程校验规则,请检查规则配置".L10N());
                if (config.DispatchNumber == null || config.DispatchNumber == 0)
                    throw new ValidationException("排程校验规则,未维护下发个数,请维护!".L10N());

                var taskConfig = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
                if (!taskConfig.IsGenerate) throw new ValidationException("工单没有配置生成任务单".L10N());

                var list = GetSchedulingInfsByIds(ids);
                //获取工单
                var workOrderIds = list.Select(p => p.WorkOrderId).Distinct().ToList();
                var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(workOrderIds);

                var closeWos = workOrders.Where(p => p.State == Core.WorkOrders.WorkOrderState.Close || p.State == Core.WorkOrders.WorkOrderState.Finish).ToList();
                if (closeWos.Count > 0)
                {
                    throw new ValidationException("工单[{0}]关闭或已完成，无法生成任务单".L10nFormat(string.Join('、', closeWos.Select(p => p.No).Distinct().ToList())));
                }

                var curDate = DateTime.Now.Date;
                //获取日期值
                EntityList<SchedulingInfValue> schedulingInfValues = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfValuesBySchedulingInfId(ids, curDate.AddDays(-1));

                //查出该工单下的所有报工任务单
                EntityList<Dispatchs.DispatchTask> dispatches = RT.Service.Resolve<Dispatchs.DispatchController>().GetDispatchTasksByWorkOrderIds(workOrderIds);

                foreach (var l in list)
                {
                    try
                    {
                        var workOrder = workOrders.FirstOrDefault(p => p.Id == l.WorkOrderId);
                        //这个工序必须要是在该工厂(当下库存组织下的)，才能生成派工任务单
                        if (!workOrder.LayoutInfoList.Any(p => p.ProcessCode == l.Process.Code && p.Factory == invOrg.ExternalId))
                        {
                            continue;
                        }

                        var info = workOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == l.Process.Code);
                        if (info == null)
                            throw new ValidationException("未找到关于[{0}]的工序的工艺路线信息!".L10nFormat(l.Process.Code));
                        WipResource wipResource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(l.MachineCode/*info.WorkCenterCode*/);
                        if (wipResource == null)
                            throw new ValidationException("未维护[{0}]生产资源信息!".L10nFormat(l.MachineCode));

                        var index = 0;
                        workOrder.IsCommonMode = false;
                        workOrder.IsMainMaterial = false;
                        var sivs = schedulingInfValues.Where(p => p.SchedulingInfId == l.Id).OrderBy(p => p.Date).ToList();

                        RstTaskBillInfo billInfo = RT.Service.Resolve<DispatchController>().IsCanSyntypeReport(workOrder);
                        bool isAccordConfig = true;
                        if (billInfo.OrgIsSyntype == true && billInfo.IsSyntype == false)
                        {
                            isAccordConfig = false;
                        }

                        //计算每天有几个班次要生成(1:白班;2:晚班;3:白班晚班都要生成)
                        var dic = CalculateClasss(DateTime.Now.Date, config.DispatchNumber.Value);
                        foreach (var d in dic)
                        {
                            var siv = sivs.FirstOrDefault(p => p.Date == d.Key);
                            //如果没有当天的就跳过
                            if (siv == null)
                                continue;
                            //当天只生成白班,或者白班晚班都生成
                            if (d.Value == 1 || d.Value == 3)
                            {
                                //判断是否已经创建任务单
                                if (siv.DispatchTask1Id == null)
                                {
                                    //如果数量大于0，就创建任务单，否则不创建
                                    if (siv.Value1 != null && siv.Value1 > 0)
                                    {
                                        //使用框架创建派工单的逻辑(改动少),原来是按工单生成
                                        var tasks = CreateDispatch(workOrder, isAccordConfig, taskConfig, l.ProcessId);
                                        //创建后，再根据自己的需求更改一些字段
                                        tasks.ForEach(p =>
                                        {
                                            //早上八点到晚上八点是早班
                                            p.PlanBeginTime = siv.Date.Date.AddHours(8);
                                            p.PlanEndTime = siv.Date.Date.AddHours(19).AddMinutes(59).AddSeconds(59);
                                            p.Classes = ClassesType.Day;
                                            p.DispatchQty = siv.Value1.Value;
                                            //p.ProcessId = l.ProcessId;
                                            p.SourceType = Dispatchs.SourceType.SchedulingInf;
                                            p.ResourceId = wipResource.Id;
                                            p.ImportTime = siv.ImportTime1;
                                        });
                                        RF.Save(tasks);
                                        siv.DispatchTask1Id = tasks.FirstOrDefault().Id;
                                        RF.Save(siv);
                                    }
                                }
                                else
                                {
                                    //如果已经创建任务单，那么判断是否数量大于0 ，不是就不允许他们修改
                                    if (siv.Value1 == null || siv.Value1 <= 0)
                                    {
                                        throw new ValidationException("已创建派工任务单，数量不能更新为空或者0");
                                    }
                                    var dispatche = dispatches.FirstOrDefault(p => p.Id == siv.DispatchTask1Id);
                                    //如果数量相同，就没必要进行更改
                                    if (dispatche != null && siv.Value1 != dispatche.DispatchQty)
                                    {
                                        dispatche.DispatchQty = siv.Value1.Value;
                                        dispatche.PersistenceStatus = PersistenceStatus.Modified;
                                        dispatche.ImportTime = siv.ImportTime1;
                                        RF.Save(dispatche);
                                    }
                                }
                            }

                            //当天生成晚班或者早班晚班
                            if (d.Value == 2 || d.Value == 3)
                            {
                                //判断是否已经创建任务单
                                if (siv.DispatchTask2Id == null)
                                {
                                    if (siv.Value2 != null && siv.Value2 > 0)
                                    {
                                        //使用框架创建派工单的逻辑(改动少),原来是按工单生成
                                        var tasks = CreateDispatch(workOrder, isAccordConfig, taskConfig, l.ProcessId);
                                        //创建后，再根据自己的需求更改一些字段
                                        tasks.ForEach(p =>
                                        {
                                            //晚上八点到 隔天早上8点是晚班
                                            p.PlanBeginTime = siv.Date.Date.AddHours(20);
                                            p.PlanEndTime = siv.Date.AddDays(1).Date.AddHours(7).AddMinutes(59).AddSeconds(59);
                                            p.Classes = ClassesType.Night;
                                            p.DispatchQty = siv.Value2.Value;
                                            //p.ProcessId = l.ProcessId;
                                            p.SourceType = Dispatchs.SourceType.SchedulingInf;
                                            p.ResourceId = wipResource.Id;
                                            p.ImportTime = siv.ImportTime2;
                                        });
                                        RF.Save(tasks);
                                        siv.DispatchTask2Id = tasks.FirstOrDefault().Id;
                                        RF.Save(siv);
                                    }
                                }
                                else
                                {
                                    if (siv.Value2 == null || siv.Value2 <= 0)
                                    {
                                        throw new ValidationException("已创建派工任务单，数量不能更新为空或者0");
                                    }
                                    var dispatche = dispatches.FirstOrDefault(p => p.Id == siv.DispatchTask2Id);
                                    if (dispatche != null && siv.Value2 != dispatche.DispatchQty)
                                    {
                                        dispatche.DispatchQty = siv.Value2.Value;
                                        dispatche.PersistenceStatus = PersistenceStatus.Modified;
                                        dispatche.ImportTime = siv.ImportTime2;
                                        RF.Save(dispatche);
                                    }
                                }
                            }

                        }

                        #region 旧逻辑

                        //foreach (var siv in sivs)
                        //{
                        //    //如果是今天的，且是在晚班的时候，就参与，直接到Value2，Value2存储晚班数据,Value1是白班数据
                        //    if (siv.Date == curDate && (DateTime.Now.Hour >= 20 || DateTime.Now.Hour < 8))
                        //    { }
                        //    else
                        //    {
                        //        //判断是否已经创建任务单
                        //        if (siv.DispatchTask1Id == null)
                        //        {
                        //            //如果数量大于0，就创建任务单，否则不创建
                        //            if (siv.Value1 != null && siv.Value1 > 0)
                        //            {
                        //                //使用框架创建派工单的逻辑(改动少),原来是按工单生成
                        //                var tasks = CreateDispatch(workOrder, isAccordConfig, taskConfig, l.ProcessId);
                        //                //创建后，再根据自己的需求更改一些字段
                        //                tasks.ForEach(p =>
                        //                {
                        //                    p.PlanBeginTime = siv.Date;
                        //                    p.PlanEndTime = siv.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        //                    p.Classes = ClassesType.Day;
                        //                    p.DispatchQty = siv.Value1.Value;
                        //                    //p.ProcessId = l.ProcessId;
                        //                    p.SourceType = Dispatchs.SourceType.SchedulingInf;
                        //                    p.ResourceId = wipResource.Id;
                        //                });
                        //                RF.Save(tasks);
                        //                siv.DispatchTask1Id = tasks.FirstOrDefault().Id;
                        //                RF.Save(siv);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            //如果已经创建任务单，那么判断是否数量大于0 ，不是就不允许他们修改
                        //            if (siv.Value1 == null || siv.Value1 <= 0)
                        //            {
                        //                throw new ValidationException("已创建派工任务单，数量不能更新为空或者0");
                        //            }
                        //            var dispatche = dispatches.FirstOrDefault(p => p.Id == siv.DispatchTask1Id);
                        //            //如果数量相同，就没必要进行更改
                        //            if (siv.Value1 != dispatche.DispatchQty)
                        //            {
                        //                dispatche.DispatchQty = siv.Value1.Value;
                        //                dispatche.PersistenceStatus = PersistenceStatus.Modified;
                        //                RF.Save(dispatche);
                        //            }
                        //        }
                        //        index++;
                        //        //判断是否已经达到配置项设置的次数
                        //        if (index >= config.DispatchNumber)
                        //            break;
                        //    }

                        //    if (siv.DispatchTask2Id == null)
                        //    {
                        //        if (siv.Value2 != null && siv.Value2 > 0)
                        //        {
                        //            //使用框架创建派工单的逻辑(改动少),原来是按工单生成
                        //            var tasks = CreateDispatch(workOrder, isAccordConfig, taskConfig, l.ProcessId);
                        //            //创建后，再根据自己的需求更改一些字段
                        //            tasks.ForEach(p =>
                        //            {
                        //                p.PlanBeginTime = siv.Date;
                        //                p.PlanEndTime = siv.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        //                p.Classes = ClassesType.Day;
                        //                p.DispatchQty = siv.Value2.Value;
                        //                //p.ProcessId = l.ProcessId;
                        //                p.SourceType = Dispatchs.SourceType.SchedulingInf;
                        //                p.ResourceId = wipResource.Id;
                        //            });
                        //            RF.Save(tasks);
                        //            siv.DispatchTask2Id = tasks.FirstOrDefault().Id;
                        //            RF.Save(siv);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (siv.Value2 == null || siv.Value2 <= 0)
                        //        {
                        //            throw new ValidationException("已创建派工任务单，数量不能更新为空或者0");
                        //        }
                        //        var dispatche = dispatches.FirstOrDefault(p => p.Id == siv.DispatchTask2Id);
                        //        if (siv.Value2 != dispatche.DispatchQty)
                        //        {
                        //            dispatche.DispatchQty = siv.Value2.Value;
                        //            dispatche.PersistenceStatus = PersistenceStatus.Modified;
                        //            RF.Save(dispatche);
                        //        }
                        //    }
                        //    index++;
                        //    //判断是否已经达到配置项设置的次数
                        //    if (index >= config.DispatchNumber)
                        //        break;

                        //    #region 旧逻辑

                        //    ////等于空就代表还没创建过任务单
                        //    //if (siv.DispatchTask1 == null && siv.Value1 != null && siv.Value1 > 0)
                        //    //{
                        //    //    //使用框架创建派工单的逻辑(改动少),原来是按工单生成
                        //    //    var tasks = CreateDispatch(workOrder, isAccordConfig, taskConfig, l.ProcessId);
                        //    //    //创建后，再根据自己的需求更改一些字段
                        //    //    tasks.ForEach(p =>
                        //    //    {
                        //    //        p.PlanBeginTime = siv.Date;
                        //    //        p.PlanEndTime = siv.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        //    //        p.Classes = ClassesType.Day;
                        //    //        p.DispatchQty = siv.Value1.Value;
                        //    //        //p.ProcessId = l.ProcessId;
                        //    //        p.SourceType = Dispatchs.SourceType.SchedulingInf;
                        //    //        p.ResourceId = wipResource.Id;
                        //    //    });
                        //    //    RF.Save(tasks);
                        //    //    siv.DispatchTask1Id = tasks.FirstOrDefault().Id;
                        //    //    RF.Save(siv);
                        //    //    index++;
                        //    //}
                        //    ////判断是否已经达到配置项设置的次数
                        //    //if (index >= config.DispatchNumber)
                        //    //    break;
                        //    //if (siv.DispatchTask2Id == null && siv.Value2 != null && siv.Value2 > 0)
                        //    //{
                        //    //    var tasks = CreateDispatch(workOrder, isAccordConfig, taskConfig, l.ProcessId);
                        //    //    tasks.ForEach(p =>
                        //    //    {
                        //    //        p.PlanBeginTime = siv.Date;
                        //    //        p.PlanEndTime = siv.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        //    //        p.Classes = ClassesType.Night;
                        //    //        p.SourceType = Dispatchs.SourceType.SchedulingInf;
                        //    //        p.DispatchQty = siv.Value2.Value;
                        //    //        p.ResourceId = wipResource.Id;
                        //    //        //p.ProcessId = l.ProcessId;
                        //    //    });
                        //    //    RF.Save(tasks);
                        //    //    siv.DispatchTask2Id = tasks.FirstOrDefault().Id;
                        //    //    RF.Save(siv);
                        //    //    index++;
                        //    //}
                        //    //if (index >= config.DispatchNumber)
                        //    //    break;
                        //    #endregion

                        //}
                        #endregion

                        //RT.Service.Resolve<DispatchController>().GenerateWorkOrderDispatchTasks(workOrder.Id, isAccordConfig, taskConfig);
                    }
                    catch (Exception ex)
                    {
                        msg += "【" + ex.Message + "】";
                    }

                }

            }, 60);

            return msg;
        }

        /// <summary>
        /// 计算每天有几个班次要生成
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual Dictionary<DateTime, int> CalculateClasss(DateTime dateTime,int i )
        {    
            Dictionary<DateTime, int> dic = new Dictionary<DateTime, int>();
            var curTime = DateTime.Now;
            while (i > 0)
            {
                //判断是不是今天的
                if (dateTime == curTime.Date)
                {
                    //判断是不是 今天8点前的，8点前的为昨天的晚班
                    if (curTime.Hour < 8)
                    {
                        if (i > 0)
                        {
                            //只生成晚班
                            //记录为昨天的晚班要生成一条，标记为1
                            dic.Add(dateTime.AddDays(-1), 2);
                            i = i - 1;
                            //判断是否已经达到配置项规定的数量
                            if (i <= 0)
                                break;
                        }
                        else
                            break;
                        //生成昨天的晚班后，再生成今天的
                        //判断还能生成几个班次，如果大于1，就说明还是生成今天白班、晚班，否则就是只能生成今天的白班
                        if (i > 1)
                        {
                            //白班晚班都生成
                            dic.Add(dateTime, 3);
                            i = i - 2;
                        }
                        else
                        {
                            //只生成白班
                            dic.Add(dateTime, 1);
                            i = i - 1;
                        }
                        if (i <= 0)
                            break;
                    }
                    //如果是在早八到晚八，就定义为白班
                    else if (curTime.Hour >= 8 && curTime.Hour < 20)
                    {
                        //判断还能生成几个班次，如果大于1，就说明还是生成今天白班、晚班，否则就是只能生成今天的白班
                        if (i > 1)
                        {
                            //白班晚班都生成
                            dic.Add(dateTime, 3);
                            i = i - 2;
                        }
                        else
                        {
                            //只生成白班
                            dic.Add(dateTime, 1);
                            i = i - 1;
                        }
                        if (i <= 0)
                            break;
                    }
                    else
                    {
                        //如果以上两个if都不满足就说明已经到了晚上八点以后了，定义为晚班
                        //判断还能生成几个班次，如果大于0，就还能生成一个晚班
                        if (i > 0)
                        {
                            dic.Add(dateTime, 2);
                            i = i - 1;
                        }
                        if (i <= 0)
                            break;
                    }
                }
                else
                {
                    //这个时候，已经不是今天的了，是未来的某一天
                    //判断还能生成几个班次，如果大于1，就说明还是生成今天白班、晚班，否则就是只能生成今天的白班
                    if (i > 1)
                    {
                        dic.Add(dateTime, 3);
                        i = i - 2;
                    }
                    else
                    {
                        dic.Add(dateTime, 1);
                        i = i - 1;
                    }
                    if (i <= 0)
                        break;
                }

                dateTime = dateTime.AddDays(1);
            }
            return dic;
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
        public virtual EntityList<DispatchTask> CreateDispatch(WorkOrder workOrder, bool isAccordConfig, DispatchTaskConfigValue taskConfig,double processId)
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
        /// 校验排程
        /// </summary>
        /// <param name="ids"></param>
        public virtual void ValidSchedulingInf(List<double> ids)
        {
            var config = ConfigService.GetConfig(new SchedulingInfCheckConfig(), typeof(SchedulingInf));
            if (config == null)
                throw new ValidationException("未找到排程校验规则,请检查规则配置".L10N());

            var list = GetSchedulingInfsByIds(ids);

            foreach (var l in list)
            {
                l.CheckMsg = string.Empty;
            }

            //校验人员技能
            if (config.PersonnelSkills == true)
            {
                ValidPersonnelSkills(list);
            }
            //校验产线状态
            if (config.LineState == true)
            {
                ValidAndonLines(list);
            }
            //校验模具状态
            if (config.MoldState == true)
            {
                ValidMoldState(list);
            }
            //校验工装状态
            if (config.ToolingState == true)
            {
                ValidToolingState(list);
            }
            //校验检具状态
            if (config.InspEquipState == true)
            {
                ValidInspEquipState(list);
            }
            //校验物料齐套
            if (config.ItemComplete == true)
            {
                ValidItemComplete(list);
            }

            //通过判断异常原因是否有值，决定是否校验成功
            foreach (var l in list)
            {
                if (l.CheckMsg.IsNullOrEmpty())
                {
                    l.IsCheck = true;
                    l.PersistenceStatus = PersistenceStatus.Modified;
                }    
            }
            RF.Save(list);
        }

        /// <summary>
        /// 校验物料齐套
        /// </summary>
        /// <param name="list"></param>
        public virtual void ValidItemComplete(EntityList<SchedulingInf> list)
        {
            var ids = list.Select(p => p.Id).Distinct().ToList();

            var workOrderProcessBoms = ids.SplitContains(i =>
            {
                return Query<WorkOrderProcessBom>().Join<SchedulingInf>((x, y) => x.WorkOrderId == y.WorkOrderId && x.ProcessId == y.ProcessId && i.Contains(y.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            var itemIds = workOrderProcessBoms.Select(p => p.ItemId).Distinct().ToList();

            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(itemIds);

            foreach (var l in list)
            {
                
                var wpbs = workOrderProcessBoms.Where(p => p.WorkOrderId == l.WorkOrderId && p.ProcessId == l.ProcessId).ToList();
                //如果没有工序BOM就直接通过
                if (wpbs.Count < 1)
                    continue;
                var msg = string.Empty;

                foreach (var wpb in wpbs)
                {
                    var ils = itemLabels.Where(p => p.ItemId == wpb.ItemId).ToList();
                    if (ils.Count < 1)
                        msg = "物料齐套校验异常";
                    else if (wpb.SingleQty * l.WoPlanQty > ils.Sum(p => p.Qty))
                        msg = "物料齐套校验异常";
                    if (!msg.IsNullOrEmpty())
                    {
                        if (l.CheckMsg.IsNullOrEmpty())
                            l.CheckMsg = msg;
                        else
                            l.CheckMsg += ";" + msg;
                        l.PersistenceStatus = PersistenceStatus.Modified;
                        break;
                    }
                }

            }

        }

        /// <summary>
        /// 校验检具状态
        /// </summary>
        /// <param name="list"></param>
        public virtual void ValidInspEquipState(EntityList<SchedulingInf> list)
        {
            var ids = list.Select(p => p.Id).Distinct().ToList();
            var checkerItems = Query<CheckerItem>().Join<SchedulingInf>((x, y) => x.ItemId == y.ItemId && x.ProcessId == y.ProcessId && ids.Contains(y.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var l in list)
            {
                var cis = checkerItems.Where(p => p.ItemId == l.ItemId && p.ProcessId == l.ProcessId).ToList();
                var msg = string.Empty;
                if (cis.Count < 1|| cis.All(p=>p.CheckerUpholdEDate<DateTime.Now))//|| cis.All(p => p.CheckerUpholdState == "故障")
                {
                    msg = "检具状态异常";
                }
                if (!msg.IsNullOrEmpty())
                {
                    if (l.CheckMsg.IsNullOrEmpty())
                        l.CheckMsg = msg;
                    else
                        l.CheckMsg += ";" + msg;
                    l.PersistenceStatus = PersistenceStatus.Modified;
                }
            }

        }

        /// <summary>
        /// 校验工装状态
        /// </summary>
        /// <param name="list"></param>
        public virtual void ValidToolingState(EntityList<SchedulingInf> list)
        {
            var ids = list.Select(p => p.Id).Distinct().ToList();
            var fixtureItems = Query<FixtureItem>().Join<SchedulingInf>((x, y) => x.ItemId == y.ItemId && x.ProcessId == y.ProcessId && ids.Contains(y.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var l in list)
            {
                var fis = fixtureItems.Where(p => p.ItemId == l.ItemId && p.ProcessId == l.ProcessId).ToList();
                var msg = string.Empty;
                if (fis.Count < 1 || fis.All(p => p.FixtureUpholdState == "故障"))
                {
                    msg = "工装状态异常";
                }
                if (!msg.IsNullOrEmpty())
                {
                    if (l.CheckMsg.IsNullOrEmpty())
                        l.CheckMsg = msg;
                    else
                        l.CheckMsg += ";" + msg;
                    l.PersistenceStatus = PersistenceStatus.Modified;
                }
            }
        }

        /// <summary>
        /// 校验模具状态
        /// </summary>
        /// <param name="list"></param>
        public virtual void ValidMoldState(EntityList<SchedulingInf> list)
        {
            var ids = list.Select(p => p.Id).Distinct().ToList();
            var equipAccountItems = Query<EquipAccountItem>().Join<SchedulingInf>((x, y) => x.ItemId == y.ItemId && x.ProcessId == y.ProcessId && ids.Contains(y.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var l in list)
            {
                var ei = equipAccountItems.Where(p => p.ItemId == l.ItemId && p.ProcessId == l.ProcessId).ToList();
                var msg = string.Empty;
                if (ei.Count < 1 || ei.All(p=>p.EquipAccountState == Core.Enums.AccountState.Fault))
                {
                    msg = "模具校验异常";
                }

                if (!msg.IsNullOrEmpty())
                {
                    if (l.CheckMsg.IsNullOrEmpty())
                        l.CheckMsg = msg;
                    else
                        l.CheckMsg += ";" + msg;
                    l.PersistenceStatus = PersistenceStatus.Modified;
                }
            }

        }

        /// <summary>
        /// 校验产线
        /// </summary>
        /// <param name="list"></param>
        public virtual void ValidAndonLines(EntityList<SchedulingInf> list)
        {
            //获取设备台账
            var equipmentNos = list.Select(p => p.EquipmentNo).Distinct().ToList();
            var equipAccounts = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(equipmentNos);

            foreach (var l in list)
            {
                var equipAccount = equipAccounts.FirstOrDefault(p => p.Code == l.EquipmentNo);
                var msg = string.Empty;
                if (equipAccount == null)
                {
                    msg = "设备台账[{0}]不存在".L10nFormat(l.EquipmentNo); ;
                }
                else
                {
                    if (equipAccount.State == Core.Enums.AccountState.Fault)
                    {
                        msg = "产线状态异常".L10N();
                    }
                }
                if (!msg.IsNullOrEmpty())
                {
                    l.CheckMsg = msg;
                    l.PersistenceStatus = PersistenceStatus.Modified;
                }
            }
        }

        /// <summary>
        /// 校验人员技能
        /// </summary>
        /// <param name="list"></param>
        public virtual void ValidPersonnelSkills(EntityList<SchedulingInf> list)
        {
            //获取工序技能
            var processIds = list.Select(p => p.ProcessId).Distinct().ToList();
            var processSkills =  RT.Service.Resolve<ProcessController>().GetProcessSkills(processIds);

            //获取人员与技能关系
            var skillIds = processSkills.Select(p => p.SkillId).Distinct().ToList();
            var employeeSkills = RT.Service.Resolve<SkillController>().GetEmployeeSkillsBySkillIds(skillIds);

            //逐条循环校验
            foreach (var l in list)
            {
                //该工序需要具备的技能
                var proSkills = processSkills.Where(p => p.ProcessId == l.ProcessId).ToList();

                var skIds = proSkills.Select(p => p.SkillId).Distinct().ToList();
                //如果在员工维护的技能明细中，存在拥有该工序需要的技能，那么就校验成功
                if (employeeSkills.Any(x => skIds.Contains(x.SkillId)))
                {

                }
                else
                {
                    l.CheckMsg = "人员技能异常".L10N();
                    l.PersistenceStatus = PersistenceStatus.Modified;
                }
            }
        }

        /// <summary>
        /// 获取排程中间表各个日期的值(大于今天的)
        /// </summary>
        /// <param name="SchedulingInfId"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfValue> GetSchedulingInfValuesBySchedulingInfId(List<double> SchedulingInfIds,DateTime dateTime)
        {
            var list = SchedulingInfIds.SplitContains(ids =>
            {
                return Query<SchedulingInfValue>().Where(p => ids.Contains(p.SchedulingInfId) && p.Date >= dateTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedulingInfId"></param>
        /// <param name="woId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfValue> GetSchedulingInfValues(double schedulingInfId, double woId, double processId)
        {
            var list = Query<SchedulingInfValue>().Where(p => p.SchedulingInfId != schedulingInfId && p.SchedulingInf.WorkOrderId == woId && p.SchedulingInf.ProcessId == processId && (p.Value1 > 0 || p.Value2 > 0)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取排程中间表各个日期的值
        /// </summary>
        /// <param name="SchedulingInfId"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfValue> GetSchedulingInfValuesBySchedulingInfId(List<double> SchedulingInfIds)
        {
            var list = SchedulingInfIds.SplitContains(ids =>
            {
                return Query<SchedulingInfValue>().Where(p => ids.Contains(p.SchedulingInfId) && (p.Value1 > 0 || p.Value2 > 0)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据Id获取MES排程
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInf> GetSchedulingInfsByIds(List<double> ids)
        {
            var list = ids.SplitContains(i =>
            {
                return Query<SchedulingInf>().Where(p => i.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 获取排程中间表各个日期的值
        /// </summary>
        /// <param name="SchedulingInfId"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfValue> GetSchedulingInfValues(double SchedulingInfId)
        {
            var list = Query<SchedulingInfValue>().Where(p => p.SchedulingInfId == SchedulingInfId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据工单、工序、产线获取排程中间表
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="processId"></param>
        /// <param name="andonLineId"></param>
        /// <returns></returns>
        public virtual SchedulingInf GetSchedulingInfByWoProcessAndonLine(double woId,double processId,double andonLineId)
        {
            var first = Query<SchedulingInf>().Where(p => p.WorkOrderId == woId && p.ProcessId == processId && p.AndonLineId == andonLineId && (p.IsCancel == false || p.IsCancel == null)).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SaveDispatchTasks(EntityList<DispatchTask> entitys)
        {
            RF.Save(entitys);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SaveSchedulingInfValue(EntityList<SchedulingInfValue> entitys)
        {
            RF.Save(entitys);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SaveSchedulingInf(SchedulingInf entity)
        {
            RF.Save(entity);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInf> CriteriaSchedulingInf(SchedulingInfCriteria criteria)
        {
            var q = DB.Query<SchedulingInf>("inf").Exists<SchedulingInfValue>((x, y) => y.Where(p => p.SchedulingInfId == x.Id && (p.Value1 > 0 || p.Value2 > 0)));
            if (criteria.Factory.IsNotEmpty())
                q.Where(p => p.Factory.Contains(criteria.Factory));
            if (criteria.WorkOrderId != null)
                q.Where(p => p.WorkOrderId == criteria.WorkOrderId);
            if (criteria.ProcessId != null)
                q.Where(p => p.ProcessId == criteria.ProcessId);
            if (criteria.AndonLineId != null)
                q.Where(p => p.AndonLineId == criteria.AndonLineId);
            if (criteria.ItemId != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.ShortDescription.Contains(criteria.ShortDescription));
            if (criteria.InStorageDate.BeginValue != null)
                q.Where(p => p.BeginDate >= criteria.InStorageDate.BeginValue.Value);
            if (criteria.InStorageDate.EndValue != null)
                q.Where(p => p.BeginDate <= criteria.InStorageDate.EndValue.Value);
            if (criteria.BeginDate.BeginValue != null)
                q.Where(p => p.BeginDate >= criteria.BeginDate.BeginValue.Value);
            if (criteria.BeginDate.EndValue != null)
                q.Where(p => p.BeginDate <= criteria.BeginDate.EndValue.Value);
            if (criteria.EndDate.BeginValue != null)
                q.Where(p => p.EndDate >= criteria.EndDate.BeginValue.Value);
            if (criteria.EndDate.EndValue != null)
                q.Where(p => p.EndDate <= criteria.EndDate.EndValue.Value);
            if (criteria.IsCheck != null)
            {
                if (criteria.IsCheck == YesNo.Yes)
                    q.Where(p => p.IsCheck == true);
                else
                    q.Where(p => p.IsCheck == false || p.IsCheck == null);
            }
            if (criteria.IsSchedulingInfReturn != null)
            {
                q.Where(p => p.IsSchedulingInfReturn == criteria.IsSchedulingInfReturn);
            }
            if (criteria.WorkOrderUpdate.BeginValue != null)
                q.Where(p => p.WorkOrder.UpdateDate >= criteria.WorkOrderUpdate.BeginValue.Value);
            if (criteria.WorkOrderUpdate.EndValue != null)
                q.Where(p => p.WorkOrder.UpdateDate <= criteria.WorkOrderUpdate.EndValue.Value);
            if (criteria.IsCancel == YesNo.Yes)
                q.Where(p => p.IsCancel == true);
            if (criteria.IsCancel == YesNo.No)
                q.Where(p => p.IsCancel == null || p.IsCancel == false);
            if (criteria.CancelTime.BeginValue != null)
                q.Where(p => p.CancelTime >= criteria.CancelTime.BeginValue.Value);
            if (criteria.CancelTime.EndValue != null)
                q.Where(p => p.CancelTime <= criteria.CancelTime.EndValue.Value);
            if (criteria.IsGenerateTask != null)
            {
                if (criteria.IsGenerateTask == YesNo.Yes)
                    q.Where(p => p.SQL<bool>("exists(select 1 from SCHEDULING_INF_VALUE where SCHEDULING_INF_VALUE.Scheduling_Inf_Id = inf.id and (SCHEDULING_INF_VALUE.value1 > 0 or SCHEDULING_INF_VALUE.value2 > 0) and (SCHEDULING_INF_VALUE.Dispatch_Task1_Id > 0 or SCHEDULING_INF_VALUE.Dispatch_Task2_Id > 0))"));
                else
                    q.Where(p => p.SQL<bool>("not exists(select 1 from SCHEDULING_INF_VALUE where SCHEDULING_INF_VALUE.Scheduling_Inf_Id = inf.id and (SCHEDULING_INF_VALUE.value1 > 0 or SCHEDULING_INF_VALUE.value2 > 0) and (SCHEDULING_INF_VALUE.Dispatch_Task1_Id > 0 or SCHEDULING_INF_VALUE.Dispatch_Task2_Id > 0))"));
            }
            if (!criteria.Mrb.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.WorkShop.Code.Contains(criteria.Mrb));
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }
    }
}
