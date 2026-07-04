using Castle.Core.Internal;
using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.MES.Capacitys;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.SchedulingInfReports.Datas;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SIE.MES.TaskManagement.SchedulingInfReports
{

    /// <summary>
    /// 排程状态查询表-控制器
    /// </summary>
    public class SchedulingInfReportController : DomainController
    {




        /// <summary>
        /// 排程状态查询表-查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfReport> Fetch(SchedulingInfReportCriteria criteria)
        {
            var datas = new EntityList<SchedulingInfReport>();

            var q = Query<SIE.MES.WorkOrders.LayoutInfo>();
            //工序工厂为当前工厂的数据:         
            q.Exists<SIE.Rbac.InvOrgs.InvOrg>((layout, invOrg) => invOrg.Where(p => p.ExternalId == layout.Factory && p.Code ==RT.InvOrg));

            //强制过滤：工单状态（为空时 仅要 发放/生产中）            
            if (criteria.State == null) { q.Where(p => p.WorkOrder.State == SIE.Core.WorkOrders.WorkOrderState.Release || p.WorkOrder.State == SIE.Core.WorkOrders.WorkOrderState.Producing); }


            //强制过滤：工序（1、仅要PP05/PP06     2、属性为排程点）       
            q.Where(p => p.Steus == "PP05" || p.Steus == "PP06");
            q.Exists<SIE.MES.ProcessProperty.ProcessPty>((a, b) => b.Where(p => p.Process.Code == a.ProcessCode && p.Scheduling == true));

            //强制过滤：下单时间（为空时 仅要 最近一年）
            if (!criteria.UpdateDate.BeginValue.HasValue && !criteria.UpdateDate.EndValue.HasValue) { q.Where(p => p.WorkOrder.CreateDate >= DateTime.Now.AddDays(-365)); }
            if (criteria.UpdateDate.BeginValue.HasValue) { q.Where(p => p.WorkOrder.CreateDate >= criteria.UpdateDate.BeginValue); }
            if (criteria.UpdateDate.EndValue.HasValue) { q.Where(p => p.WorkOrder.CreateDate <= criteria.UpdateDate.EndValue); }

            //条件过滤：工单号、MRP控制者（车间编码）、产品编码、产品名称、旧料号、工序
            if (!string.IsNullOrEmpty(criteria.WorkOrderNo)) { q.Where(p => p.WorkOrder.No == criteria.WorkOrderNo); }
            if (!string.IsNullOrEmpty(criteria.Mrp)) { q.Where(p => p.WorkOrder.WorkShop.Code == criteria.Mrp); }
            if (!string.IsNullOrEmpty(criteria.ProductCode)) { q.Where(p => p.WorkOrder.Product.Code == criteria.ProductCode); }
            if (!string.IsNullOrEmpty(criteria.ProductName)) { q.Where(p => p.WorkOrder.Product.Name.Contains(criteria.ProductName)); }
            if (!string.IsNullOrEmpty(criteria.ShortDescription)) { q.Where(p => p.WorkOrder.Product.ShortDescription.Contains(criteria.ShortDescription)); }
            if (!string.IsNullOrEmpty(criteria.ProcessCode)) { q.Where(p => p.ProcessCode == criteria.ProcessCode); }  //精确匹配，待同步修改web查询视图为弹窗选择          



            // 获取总数（分页前）
            var totalCount = q.Count();
            datas.SetTotalCount(totalCount);           
            PagingInfo pagingInfo = criteria.PagingInfo;

            //在最后面 datas.Add(schedulingInfReport); 全部完成后，判断 datas.Count<criteria.PagingInfo.PageSize时，
            //继续; 下一页的var layoutInfo_List = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //直到datas.Count==criteria.PagingInfo.PageSize

            var layoutInfo_List = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());


            // 构建组合键列表：WorkOrderId + "_" + ProcessCode          
            var compositeKeys = layoutInfo_List.Select(p => $"{p.WorkOrder.Id}_{p.ProcessCode}").Distinct().ToList();


            // 一次性查询所有 DispatchTask [派工任务]
            //条件过滤：排程导入时间  (待优化：在 q.Count();前过滤)        
            var q_DispatchTask = Query<SIE.MES.TaskManagement.Dispatchs.DispatchTask>().Where(d => compositeKeys.Contains(d.WorkOrderId + "_" + d.Process.Code));           
            if (criteria.ImportTime.BeginValue.HasValue) { q_DispatchTask.Where(p => p.ImportTime >= criteria.ImportTime.BeginValue); }
            if (criteria.ImportTime.EndValue.HasValue) { q_DispatchTask.Where(p => p.ImportTime <= criteria.ImportTime.EndValue); }
            var dispatchTask_list = q_DispatchTask.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            // 按组合键分组，提高查找效率
            var dispatchTaskDict = dispatchTask_list.GroupBy(d => d.WorkOrderId + "_" + d.Process.Code).ToDictionary(g => g.Key, g => g.ToList());


            // 一次性查询所有 DispatchTaskp [MES排程导入中间表]  
            var q_SchedulingInf = Query<SIE.MES.TaskManagement.SchedulingInfs.SchedulingInf>().Where(s => compositeKeys.Contains(s.WorkOrderId + "_" + s.Process.Code));           
            var schedulingInf_list = q_SchedulingInf.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var schedulingInfDict = schedulingInf_list.GroupBy(s => s.WorkOrderId + "_" + s.Process.Code).ToDictionary(g => g.Key, g => g.ToList());


            layoutInfo_List.ForEach(layInfo =>
            {
                var key = layInfo.WorkOrder.Id + "_" + layInfo.ProcessCode;
              
                // 从字典中获取对应的 DispatchTask列表和SchedulingInf列表
                var meetDispatchTask = dispatchTaskDict.ContainsKey(key) ? dispatchTaskDict[key] : new List<SIE.MES.TaskManagement.Dispatchs.DispatchTask>();
                var meetSchedulingInf = schedulingInfDict.ContainsKey(key) ? schedulingInfDict[key] : new List<SIE.MES.TaskManagement.SchedulingInfs.SchedulingInf>();

                //计划数量：工单的 计划数量
                //工序数量：工单与工艺路线的 工序数量
                //工序排程数量：已下发+未下发===[派工任务]的任务数量+[MES排程导入中间表]的数量 
                DispatchTaskInfoTotal infoTotal = new DispatchTaskInfoTotal();
                infoTotal.MachineCode = meetDispatchTask.FirstOrDefault(d => !string.IsNullOrEmpty(d.Resource.Code))?.Resource.Code ?? string.Empty;
                infoTotal.MachineCode = meetDispatchTask.FirstOrDefault(d => !string.IsNullOrEmpty(d.Resource.Name))?.Resource.Name ?? string.Empty;
                infoTotal.YesGenerateTaskQty = meetDispatchTask.Sum(p => p.DispatchQty);
                infoTotal.NoGenerateTaskQty= meetSchedulingInf.FirstOrDefault(s => s.IsCancel != true)?.StandardCapacity ?? 0;                
                infoTotal.SchedulingQty = infoTotal.YesGenerateTaskQty;
                infoTotal.WaitSchedulingQty = layInfo.ProcessQty - infoTotal.SchedulingQty;
                infoTotal.IsImport = (layInfo.ProcessQty - infoTotal.YesGenerateTaskQty - infoTotal.NoGenerateTaskQty) <= 0 ? YesNo.Yes : YesNo.No;
                infoTotal.ReportQty = meetDispatchTask.Sum(p => p.ReportQty);

                //工单+工序的已报工数量合计=0，且任务单状态为已派工、派工中、待派工时，状态为已派工    ③
                //工单+工序的已报工数量合计=0，且任务单状态为派工中、待派工时，则状态为派工中  ②
                //工单+工序的已报工数量合计=0，且任务单状态为待派工时，则状态为待派工  ①
                //工单+工序的已报工数量合计>0<任务数量，则状态为执行中  ④
                //工单+工序的已报工数量合计=任务数量，则状态为已完成  ⑤
                #region 按顺序执行               
                if (infoTotal.ReportQty == 0 && meetDispatchTask.Any(d => d.TaskStatus == DispatchTaskStatus.ToDispatch)) { infoTotal.TaskStatus = DispatchTaskStatus.ToDispatch; }
                if (infoTotal.ReportQty == 0 && meetDispatchTask.Any(d => d.TaskStatus == DispatchTaskStatus.Dispatching)) { infoTotal.TaskStatus = DispatchTaskStatus.Dispatching; }
                if (infoTotal.ReportQty == 0 && meetDispatchTask.Any(d => d.TaskStatus == DispatchTaskStatus.Dispatched)) { infoTotal.TaskStatus = DispatchTaskStatus.Dispatched; }              
                if (infoTotal.ReportQty > 0 && infoTotal.ReportQty < layInfo.ProcessQty) { infoTotal.TaskStatus = DispatchTaskStatus.Executing; }
                if (infoTotal.ReportQty >= layInfo.ProcessQty) { infoTotal.TaskStatus = DispatchTaskStatus.Finished; }
                #endregion   

                infoTotal.ImportQty = infoTotal.SchedulingQty;
                infoTotal.IsSchedulingInfReturn = meetDispatchTask.Any(d => d.IsSchedulingInfReturn == YesNo.Yes) ? YesNo.Yes : YesNo.No;
                infoTotal.SchedulingInfReturnReason = meetDispatchTask.FirstOrDefault(d => !string.IsNullOrEmpty(d.SchedulingInfReturnReason))?.SchedulingInfReturnReason ?? string.Empty;              
                infoTotal.ImportTime = meetDispatchTask.FirstOrDefault()?.CreateDate;
                if (infoTotal.NoGenerateTaskQty != 0) { infoTotal.StandardCapacity = infoTotal.NoGenerateTaskQty; }  //取消界面显示  标准产能：产品+工序+资源 >> SCHEDULING_INF
                infoTotal.IsCheck = !(infoTotal.NoGenerateTaskQty > 0 && (meetSchedulingInf.FirstOrDefault(s => s.IsCancel != true)?.IsCheck != true));  //取消界面显示  检验是否通过：产品+工序+资源 >> SCHEDULING_INF
                infoTotal.IsGenerateTask = infoTotal.YesGenerateTaskQty>0? YesNo.Yes : YesNo.No; //含有未下发的统一是未下发


                //条件过滤：是否已完全导入、是否排程退回   (待优化：在 q.Count();前过滤)                             
                if (criteria.IsImport != null&& infoTotal.IsImport != criteria.IsImport) { return; }
                if (criteria.IsSchedulingInfReturn != null&& infoTotal.IsSchedulingInfReturn != criteria.IsSchedulingInfReturn) { return; }
                if (criteria.IsGenerateTask != null&& infoTotal.IsGenerateTask != criteria.IsGenerateTask) { return; }


                var schedulingInfReport = new SchedulingInfReport
                {
                    IsImport = infoTotal.IsImport,
                    Factory = layInfo.WorkOrder.Factory.Code,
                    WorkOrderNo = layInfo.WorkOrder.No,
                    Mrp = layInfo.WorkOrder.WorkShop.Code,
                    ProcessCode = layInfo.ProcessCode,
                    ProcessName = layInfo.ProcessCode,
                    MachineCode = infoTotal.MachineCode,
                    MachineName = infoTotal.MachineName,
                    StandardCapacity=infoTotal.StandardCapacity,  //取消界面显示  标准产能：产品+工序+资源 >> SCHEDULING_INF
                    ProductCode = layInfo.WorkOrder.Product.Code,
                    ProductName = layInfo.WorkOrder.Product.Name,
                    ShortDescription = layInfo.WorkOrder.Product.ShortDescription,
                    PlanBeginDate = layInfo.WorkOrder.PlanBeginDate,
                    PlanEndDate = layInfo.WorkOrder.PlanEndDate,
                    WorkShop = layInfo.WorkOrder.WorkShop.Name,
                    Type = layInfo.WorkOrder.Type,
                    FinishQty = layInfo.WorkOrder.FinishQty,
                    ScrapQty = layInfo.WorkOrder.ScrapQty,
                    ProcessQty = layInfo.ProcessQty,
                    ReportQty = infoTotal.ReportQty,
                    SchedulingQty = infoTotal.SchedulingQty,
                    WaitSchedulingQty = infoTotal.WaitSchedulingQty,
                    TaskStatus = infoTotal.TaskStatus,
                    PlanQty = layInfo.WorkOrder.PlanQty,
                    ImportQty = infoTotal.YesGenerateTaskQty,
                    State = layInfo.WorkOrder.State,
                    IsSchedulingInfReturn = infoTotal.IsSchedulingInfReturn,
                    SchedulingInfReturnReason = infoTotal.SchedulingInfReturnReason,
                    ImportTime = layInfo.CreateDate,
                    IsCheck = infoTotal.IsCheck, //取消界面显示  检验是否通过：产品+工序+资源 >> SCHEDULING_INF
                    IsGenerateTask = infoTotal.IsGenerateTask,
                    UpdateDate = layInfo.WorkOrder.CreateDate 
                };

                datas.Add(schedulingInfReport);

            });

            //if (datas.Count < criteria.PagingInfo.PageCount) { datas.SetTotalCount(datas.Count); }

            return datas;
        }



        /// <summary>
        /// 排程状态查询表-查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SchedulingInfReport> Fetch_Old(SchedulingInfReportCriteria criteria)
        {
            var datas = new EntityList<SchedulingInfReport>();

            // 1、查询 工单表 WorkOrder
            var q_WorkOrder = Query<SIE.MES.WorkOrders.WorkOrder>();

            //强制过滤：工单状态（发放/生产中）          
            q_WorkOrder.Where(p => p.State == SIE.Core.WorkOrders.WorkOrderState.Release || p.State == SIE.Core.WorkOrders.WorkOrderState.Producing);

            //强制过滤：下单时间（为空时 仅要最近一年）
            if (!criteria.UpdateDate.BeginValue.HasValue && !criteria.UpdateDate.EndValue.HasValue) { q_WorkOrder.Where(p => p.CreateDate >= DateTime.Now.AddDays(-365)); }
            if (criteria.UpdateDate.BeginValue.HasValue) { q_WorkOrder.Where(p => p.CreateDate >= criteria.UpdateDate.BeginValue); }
            if (criteria.UpdateDate.EndValue.HasValue) { q_WorkOrder.Where(p => p.CreateDate <= criteria.UpdateDate.EndValue); }

            //条件过滤：工单号、MRP控制者（车间编码）、产品编码、产品名称、旧料号、工序
            if (!string.IsNullOrEmpty(criteria.WorkOrderNo)) { q_WorkOrder.Where(p => p.No == criteria.WorkOrderNo); }
            if (!string.IsNullOrEmpty(criteria.Mrp)) { q_WorkOrder.Where(p => p.WorkShop.Code == criteria.Mrp); }
            if (!string.IsNullOrEmpty(criteria.ProductCode)) { q_WorkOrder.Where(p => p.Product.Code == criteria.ProductCode); }
            if (!string.IsNullOrEmpty(criteria.ProductName)) { q_WorkOrder.Where(p => p.Product.Name.Contains(criteria.ProductName)); }
            if (!string.IsNullOrEmpty(criteria.ShortDescription)) { q_WorkOrder.Where(p => p.Product.ShortDescription.Contains(criteria.ShortDescription)); }
            if (!string.IsNullOrEmpty(criteria.ProcessCode)) { q_WorkOrder.Where(p => p.LayoutInfoList.Any(l => l.ProcessCode == criteria.ProcessCode)); }  //精确匹配，待同步修改web查询视图为弹窗选择

            
            //执行查询           
            var workOrderList = q_WorkOrder.ToList();
            //var workOrderList = q_WorkOrder.ToList(new PagingInfo(), new EagerLoadOptions().LoadWithViewProperty()); 



            // 2、查询 工艺路线表 LayoutInfo           
            foreach (var workOrder in workOrderList)
            {
                var q_LayoutInfo = Query<SIE.MES.WorkOrders.LayoutInfo>().Where(p => p.WorkOrderId == workOrder.Id);

                //强制过滤：工序（1、仅要PP05/PP06     2、属性为排程点）              
                q_LayoutInfo.Where(p => p.Steus == "PP05" || p.Steus== "PP06");
                q_LayoutInfo.Exists<SIE.MES.ProcessProperty.ProcessPty>((a, b) => b.Where(p => p.Process.Code == a.ProcessCode && p.Scheduling == true));

                //条件过滤（第2次）：工序
                if (!string.IsNullOrEmpty(criteria.ProcessCode)) { q_LayoutInfo.Where(p => p.ProcessCode == criteria.ProcessCode); }


                //执行查询
                var layoutInfoList = q_LayoutInfo.ToList();// new PagingInfo(),new EagerLoadOptions().LoadWithViewProperty()

                foreach (var layoutInfo in layoutInfoList)
                {
                    //3、已导入的排程表 ：工单+工序>>存在派工任务单
                    var q_DispatchTask = Query<SIE.MES.TaskManagement.Dispatchs.DispatchTask>().Where(p => p.WorkOrderId == workOrder.Id && p.Process.Code == layoutInfo.ProcessCode);

                    //条件过滤：排程导入时间 、是否排程退回 
                    if (criteria.ImportTime.BeginValue.HasValue) { q_DispatchTask.Where(p=>p.ImportTime>= criteria.ImportTime.BeginValue); }
                    if (criteria.ImportTime.EndValue.HasValue) { q_DispatchTask.Where(p => p.ImportTime <= criteria.ImportTime.EndValue); }
                    if (criteria.IsSchedulingInfReturn != null) { q_DispatchTask.Where(p => p.IsSchedulingInfReturn == criteria.IsSchedulingInfReturn); }

                    //执行查询
                    var dispatchTask_list = q_DispatchTask.ToList();


                    //计划数量：工单的 计划数量
                    //工序数量：工单与工艺路线的 工序数量
                    //工序排程数量：派工单的 任务数量
                    DispatchTaskInfoTotal infoTotal = new DispatchTaskInfoTotal();
                    var firstDispatchTask = dispatchTask_list.FirstOrDefault();

                    infoTotal.SchedulingQty = dispatchTask_list.Sum(p => p.DispatchQty);
                    infoTotal.WaitSchedulingQty = layoutInfo.ProcessQty - infoTotal.SchedulingQty;
                    infoTotal.IsImport = dispatchTask_list.Any() ? (infoTotal.WaitSchedulingQty <= 0 ? YesNo.Yes : YesNo.No) : YesNo.No;
                    infoTotal.ReportQty = dispatchTask_list.Sum(p => p.ReportQty);
                    infoTotal.TaskStatus = firstDispatchTask?.TaskStatus ?? DispatchTaskStatus.ToDispatch; // 设置默认值
                    infoTotal.ImportQty = infoTotal.SchedulingQty;
                    infoTotal.IsSchedulingInfReturn = firstDispatchTask?.IsSchedulingInfReturn ?? YesNo.No; // 设置默认值
                    infoTotal.SchedulingInfReturnReason = firstDispatchTask?.SchedulingInfReturnReason;
                    infoTotal.ImportTime = firstDispatchTask?.CreateDate;
                    infoTotal.IsCheck = dispatchTask_list.Any();
                    infoTotal.IsGenerateTask = dispatchTask_list.Any() ? YesNo.Yes : YesNo.No;
                    infoTotal.MachineCode= firstDispatchTask?.Resource.Code; 
                    infoTotal.MachineName = firstDispatchTask?.Resource.Name; 

                    //标准产能：产品+工序+资源 （直接取消显示）

                    var report = CreateReportFromEntities(workOrder, layoutInfo, infoTotal);
                    datas.Add(report);


                }              
            }          
            //datas.SetTotalCount(datas.TotalCount);
            return datas;
        }

        private SchedulingInfReport CreateReportFromEntities(SIE.MES.WorkOrders.WorkOrder workOrder, SIE.MES.WorkOrders.LayoutInfo layoutInfo, DispatchTaskInfoTotal infoTotal)
        {
            return new SchedulingInfReport
            {
                IsImport = infoTotal.IsImport,
                Factory = workOrder.Factory.Code,
                WorkOrderNo = workOrder.No,
                Mrp = workOrder.WorkShop.Code,
                ProcessCode = layoutInfo.ProcessCode,
                ProcessName = layoutInfo.ProcessCode,
                MachineCode = infoTotal.MachineCode, 
                MachineName = infoTotal.MachineName, 
                StandardCapacity = 0,  //取消界面显示  标准产能：产品+工序+资源
                ProductCode = workOrder.Product.Code,
                ProductName = workOrder.Product.Name,
                ShortDescription = workOrder.Product.ShortDescription,
                PlanBeginDate = workOrder.PlanBeginDate,
                PlanEndDate = workOrder.PlanEndDate,
                WorkShop = workOrder.WorkShop.Name,
                Type = workOrder.Type,
                FinishQty = workOrder.FinishQty,
                ScrapQty = workOrder.ScrapQty,
                ProcessQty = layoutInfo.ProcessQty,
                ReportQty = infoTotal.ReportQty,
                SchedulingQty = infoTotal.SchedulingQty,
                WaitSchedulingQty = infoTotal.WaitSchedulingQty,
                TaskStatus = infoTotal.TaskStatus,
                PlanQty = workOrder.PlanQty,
                ImportQty = infoTotal.SchedulingQty,
                State = workOrder.State,
                IsSchedulingInfReturn = infoTotal.IsSchedulingInfReturn,
                SchedulingInfReturnReason = infoTotal.SchedulingInfReturnReason,
                ImportTime = infoTotal.ImportTime,
                IsCheck = infoTotal.IsCheck,
                IsGenerateTask = infoTotal.IsGenerateTask,
                UpdateDate = workOrder.CreateDate,
            };
        }


    }









}