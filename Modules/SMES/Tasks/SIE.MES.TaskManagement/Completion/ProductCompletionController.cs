using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Capacitys;
using SIE.MES.TaskManagement.Dispatchs;
using System;
using System.Linq;

namespace SIE.MES.TaskManagement.Completion
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class ProductCompletionController : DomainController
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ProductCompletion> GetProductCompletions(ProductCompletionCriteria criteria)
        {
            if (criteria.PlanBeginTime.BeginValue == null || criteria.PlanBeginTime.EndValue == null)
                throw new ValidationException("计划生产时间不能为空");

            var datas = new EntityList<ProductCompletion>();
            var q = Query<DispatchTask>();

            if (criteria.ResourceCode.IsNotEmpty())
                q.Where(p => p.Resource.Code.Contains(criteria.ResourceCode));
            if (criteria.ResourceName.IsNotEmpty())
                q.Where(p => p.Resource.Name.Contains(criteria.ResourceName));
            if (criteria.ProductCode.IsNotEmpty())
                q.Where(p => p.Product.Code.Contains(criteria.ProductCode));
            if (criteria.ProductName.IsNotEmpty())
                q.Where(p => p.Product.Name.Contains(criteria.ProductName));
            if (criteria.MrpController.IsNotEmpty())
                q.Where(p => p.Product.MrpController.Contains(criteria.MrpController));

            if (criteria.PlanBeginTime.BeginValue.HasValue)
                q.Where(p => p.PlanBeginTime >= criteria.PlanBeginTime.BeginValue);
            if (criteria.PlanBeginTime.EndValue.HasValue)
                q.Where(p => p.PlanEndTime <= criteria.PlanBeginTime.EndValue);
            if (criteria.ProcessId > 0)
                q.Where(p => p.ProcessId == criteria.ProcessId);
            if (criteria.WorkOrderId > 0)
                q.Where(p => p.WorkOrderId == criteria.WorkOrderId);
            if (criteria.Classes.HasValue)
                q.Where(p => p.Classes == criteria.Classes.Value);
            var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var scheduleStartTime = criteria.PlanBeginTime.BeginValue;
            var scheduleEndTime = criteria.PlanBeginTime.EndValue;
            var details = GetProductCompletionDetails(list, scheduleStartTime, scheduleEndTime);
            var productCodes = details.Select(p => p.ProductCode).Distinct().ToList();
            var stdCapacitys = Query<StandardCapacity>().Where(p => productCodes.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            details.GroupBy(p => new
            {
                p.ProductCode,
                p.ProductName,
                p.ResourceCode,
                p.ResourceName,
                p.ProcessCode,
                p.ProcessName,
                p.MrpController,

            }).ForEach(dtl =>
            {
                //班次产能；取标准产能维护基础表的值（取值逻辑：按优先级从高到低的维度获取一个值，优先级高-资源+物料编码+工序，优先级中-物料编码+工序，优先级低-物料编码）
                var stdCapacity = stdCapacitys.FirstOrDefault(p => p.ItemCode == dtl.Key.ProductCode && p.ProcessCode == dtl.Key.ProcessCode && p.ResourceCode == dtl.Key.ResourceCode);
                if (stdCapacity == null)
                    stdCapacity = stdCapacitys.FirstOrDefault(p => p.ItemCode == dtl.Key.ProductCode && p.ProcessCode == dtl.Key.ProcessCode && p.ResourceId == null);
                if (stdCapacity == null)
                    stdCapacity = stdCapacitys.FirstOrDefault(p => p.ItemCode == dtl.Key.ProductCode && p.ProcessId == null && p.ResourceId == null);
                var completion = new ProductCompletion
                {
                    ProductCode = dtl.Key.ProductCode,
                    ProductName = dtl.Key.ProductName,
                    ResourceCode = dtl.Key.ResourceCode,
                    ResourceName = dtl.Key.ResourceName,
                    ProcessCode = dtl.Key.ProcessCode,
                    ProcessName = dtl.Key.ProcessName,
                    MrpController = dtl.Key.MrpController,
                    TaskQty = dtl.Sum(x => x.TaskQty),
                    ReportedQty = dtl.Sum(x => x.ReportedQty),
                    OkQty = dtl.Sum(x => x.OkQty),
                    NgQty = dtl.Sum(x => x.NgQty),
                    ReworkQty = dtl.Sum(x => x.ReworkQty),
                    SuspectQty = dtl.Sum(x => x.SuspectQty),
                    ShiftCount = dtl.Sum(x => x.ShiftCount),
                    ShiftCapacity = stdCapacity?.ShiftCapacity ?? 0,
                    ScheduleStartTime = scheduleStartTime,
                    ScheduleEndTime = scheduleEndTime,
                    Classes = criteria.Classes
                };
                datas.Add(completion);
            });
            datas.SetTotalCount(datas.Count);
            return datas;
        }

        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="completion"></param>
        /// <returns></returns>
        public virtual EntityList<ProductCompletionDetail> GetProductCompletionDetails(ProductCompletion completion)
        {
            var q = Query<DispatchTask>();

            if (completion.ResourceCode.IsNotEmpty())
                q.Where(p => p.Resource.Code.Contains(completion.ResourceCode));
            
            if (completion.ProductCode.IsNotEmpty())
                q.Where(p => p.Product.Code.Contains(completion.ProductCode));
            

            if (completion.ScheduleStartTime.HasValue)
                q.Where(p => p.PlanBeginTime >= completion.ScheduleStartTime);
            if (completion.ScheduleEndTime.HasValue)
                q.Where(p => p.PlanEndTime <= completion.ScheduleEndTime);

            if (completion.ProcessCode.IsNotEmpty())
                q.Where(p => p.Process.Code.Contains(completion.ProcessCode));
            
            if (completion.Classes.HasValue)
                q.Where(p => p.Classes == completion.Classes.Value);
            var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var scheduleStartTime = completion.ScheduleStartTime;
            var scheduleEndTime = completion.ScheduleEndTime;
            var details = GetProductCompletionDetails(list, scheduleStartTime, scheduleEndTime);

            return details;
        }

        /// <summary>
        /// 转换 ProductCompletionDetail
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        EntityList<ProductCompletionDetail> GetProductCompletionDetails(EntityList<DispatchTask> tasks, DateTime? scheduleStartTime = null, DateTime? scheduleEndTime = null)
        {
            var list = new EntityList<ProductCompletionDetail>();
            tasks.ForEach(p =>
            {
                var dtl = new ProductCompletionDetail()
                {
                    TaskNo = p.No,
                    WorkOrderNo = p.WorkOrderNo,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    ResourceCode = p.ResourceCode,
                    ResourceName = p.ResourceName,
                    ProcessCode = p.ProcessCode,
                    ProcessName = p.ProcessName,
                    TaskQty = p.DispatchQty,
                    ReportedQty = p.ReportQty,
                    OkQty = p.OkQty,
                    NgQty = p.NgQty,
                    ReworkQty = p.ReworkQty,
                    SuspectQty = p.SuspectQty,
                    Classes = p.Classes,
                    MrpController = p.MrpController,
                    ScheduleStartTime = scheduleStartTime,
                    ScheduleEndTime = scheduleEndTime
                };
                dtl.ShiftCount = p.Classes == null ? 2 : 1;
                if (scheduleEndTime != null && scheduleStartTime != null)
                {
                    TimeSpan timeSpan = scheduleEndTime.Value.Subtract(scheduleStartTime.Value);
                    int days = timeSpan.Days + 1;
                    dtl.ShiftCount = dtl.ShiftCount * days;
                }
                list.Add(dtl);
            });
            return list;
        }
    }
}
