using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试控制器-工作计划
    /// </summary>
    public partial class EquipmentSetupController : DomainController
    {
        /// <summary>
        /// 根据id列表获取安装调试工作计划
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>安装调试工作计划</returns>
        public virtual EntityList<EquipmentSetupPlan> GetEquipmentSetupPlansByIds(List<double> ids)
        {
            return ids.SplitContains(id => Query<EquipmentSetupPlan>().Where(p => id.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据安装调试id列表获取安装调试工作计划
        /// </summary>
        /// <param name="setupIds">安装调试id列表</param>
        /// <returns>安装调试工作计划</returns>
        public virtual EntityList<EquipmentSetupPlan> GetPlansBySetupIds(List<double> setupIds)
        {
            return setupIds.SplitContains(ids => Query<EquipmentSetupPlan>().Where(p => ids.Contains(p.EquipmentSetupId)).ToList());
        }

        /// <summary>
        /// 获取安装调试的工作计划
        /// </summary>
        /// <param name="setupId">安装调试id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">工作节点</param>
        /// <returns>工作计划</returns>
        public virtual EntityList<EquipmentSetupPlan> GetPlansBySetupId(double setupId, PagingInfo pagingInfo, string keyword)
        {
            return Query<EquipmentSetupPlan>().Where(p => p.EquipmentSetupId == setupId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.TodoItem.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 查询设备台账
        /// </summary>
        /// <param name="criteria">设备台账查询</param>
        /// <returns>设备台账</returns>
        public virtual EntityList<SelEquipDetailViewModel> CriteriaSelEquipDetails(SelEquipDetailCriteriaViewModel criteria)
        {
            var query = Query<EquipAccount>();
            if (!criteria.Code.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (!criteria.Name.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if ((criteria.ModelCode.IsNotEmpty() || criteria.ModelName.IsNotEmpty()) && criteria.PurchaseOrder.IsNullOrWhiteSpace())
            {
                query.Exists<EquipModel>((x, y) => y.Join<EquipType>((c, d) => c.EquipTypeId == d.Id)
                .Where(p => p.Id == x.EquipModelId)
                .WhereIf(criteria.ModelCode.IsNotEmpty(), c => c.Code.Contains(criteria.ModelCode))
                .WhereIf(criteria.ModelName.IsNotEmpty(), c => c.Name.Contains(criteria.ModelName)));
            }
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.ResourceId.HasValue)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId);
            }
            if (criteria.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criteria.ProcessId);
            }
            if (!criteria.PurchaseOrder.IsNullOrWhiteSpace())
            {
                query.Exists<EquipmentInboundDetail>((x, y) => y.Where(p => p.EquipmentCode == x.Code && p.PurchaseOrder.OrderNo.Contains(criteria.PurchaseOrder)));

                query.NotExists<EquipmentDetail>((a, b) => b.Where(p => p.EquipAccountId == a.Id));
            }
            var eqList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var list = new EntityList<SelEquipDetailViewModel>();
            foreach (var eq in eqList)
            {
                var item = new SelEquipDetailViewModel();
                item.Id = eq.Id;
                item.Code = eq.Code;
                item.Name = eq.Name;
                item.Alias = eq.Alias;
                item.EquipAccountModelCode = eq.ModelCode;
                item.EquipAccountModelName = eq.ModelName;
                item.Specifications = eq.Specifications;
                item.ManageDepartmentName = eq.ManageDepartmentName;
                item.UseDepartmentName = eq.UseDepartmentName;
                item.Manufacturer = eq.Manufacturer;
                item.OriginalSerialNumber = eq.OriginalSerialNumber;
                item.WarrantyPeriod = eq.WarrantyPeriod;
                list.Add(item);
            }
            list.SetTotalCount(list.Count);
            return list;
        }

        /// <summary>
        /// 计划开始
        /// </summary>
        /// <param name="planIds">计划id</param>
        public virtual void PlanStart(List<double> planIds)
        {
            var plans = GetEquipmentSetupPlansByIds(planIds);
            if (!plans.Any())
            {
                throw new ValidationException("数据异常，找不到工作计划".L10N());
            }
            if (plans.Any(p => p.WorkStatus != WorkStatus.NotStarted))
            {
                throw new ValidationException("状态为【未开始】才能开始".L10N());
            }
            var setup = GetById<EquipmentSetup>(plans.FirstOrDefault().EquipmentSetupId);
            if (setup == null)
            {
                throw new ValidationException("数据异常，找不到安装调试单".L10N());
            }
            if (setup.ApprovalStatus != ApprovalStatus.Audited)
            {
                throw new ValidationException("审核状态为【通过】才能开始".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var now = RF.Find<EquipmentSetup>().GetDbTime();
                foreach (var plan in plans)
                {
                    //子表状态更新为【进行中】，更新实际开始时间为当前时间
                    plan.WorkStatus = WorkStatus.InProgress;
                    plan.ActualStartDateTime = now;

                    //生成操作记录，开始工作计划【xxx】
                    SaveSetupLog(new List<double> { setup.Id }, "开始工作计划【{0}】".L10nFormat(plan.TodoItem), now);
                }
                RF.Save(plans);
                //如果主表状态为【待执行】，则更新为执行中
                if (setup.SetupStatus == SetupStatus.ToBe)
                {
                    setup.SetupStatus = SetupStatus.Doing;
                    RF.Save(setup);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 计划完成
        /// </summary>
        /// <param name="planIds">计划id</param>
        public virtual void PlanEnd(List<double> planIds)
        {
            var plans = GetEquipmentSetupPlansByIds(planIds);
            if (!plans.Any())
            {
                throw new ValidationException("数据异常，找不到工作计划".L10N());
            }
            if (plans.Any(p => p.WorkStatus != WorkStatus.InProgress))
            {
                throw new ValidationException("状态为【进行中】才能完成".L10N());
            }
            var setup = GetById<EquipmentSetup>(plans.FirstOrDefault().EquipmentSetupId);
            if (setup == null)
            {
                throw new ValidationException("数据异常，找不到安装调试单".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var now = RF.Find<EquipmentSetup>().GetDbTime();
                foreach (var plan in plans)
                {
                    //子表状态更新为【已完成】，更新实际结束时间为当前时间
                    plan.WorkStatus = WorkStatus.Finish;
                    plan.ActualEndDateTime = now;

                    //生成操作记录，完成工作计划【xxx】
                    SaveSetupLog(new List<double> { setup.Id }, "完成工作计划【{0}】".L10nFormat(plan.TodoItem), now);
                }
                RF.Save(plans);

                //如果全部工作计划状态都为【已完成】，则更新主表状态为【已完成】
                var allPlans = GetPlansBySetupIds(new List<double> { setup.Id });
                if (allPlans.All(p => p.WorkStatus == WorkStatus.Finish))
                {
                    setup.SetupStatus = SetupStatus.Done;
                    RF.Save(setup);
                }
                trans.Complete();
            }
        }
    }
}
