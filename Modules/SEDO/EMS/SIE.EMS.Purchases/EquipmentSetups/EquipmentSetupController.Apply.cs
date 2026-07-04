using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试控制器-备件申请
    /// </summary>
    public partial class EquipmentSetupController : DomainController
    {
        /// <summary>
        /// 根据安装调试id列表获取安装调试备件申请
        /// </summary>
        /// <param name="setupIds">安装调试id列表</param>
        /// <returns>安装调试备件申请</returns>
        public virtual EntityList<SetupSparePartApply> GetApplysBySetupIds(List<double> setupIds)
        {
            return setupIds.SplitContains(ids => Query<SetupSparePartApply>().Where(p => ids.Contains(p.EquipmentSetupId)).ToList());
        }

        /// <summary>
        /// 获取安装调试备件申请
        /// </summary>
        /// <param name="setup">安装调试</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="sortInfo">排序</param>
        /// <returns>安装调试备件申请</returns>
        public virtual EntityList<SetupSparePartApply> GetSetupSparePartApplys(EquipmentSetup setup, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var list = new EntityList<SetupSparePartApply>();
            var sparePartApplys = Query<SetupSparePartApply>().Where(p => p.EquipmentSetupId == setup.Id)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            var allSpareParts = GetSparePartsBySetupIds(new List<double> { setup.Id });

            //关联备件申请单中来源类型是【安装调试】，来源单号是当前安装调试单号的数据中，该备件的【出库数量-退回数量】，多条数据时进行汇总
            var allApplyDetails = Query<ApplyDetail>().Exists<SparePartApp>((a, b) => b.Where(p => p.Id == a.SparePartAppId
            && p.FromType == FromType.Setup && p.FromNo == setup.SetupNo)).ToList();
            foreach (var apply in sparePartApplys)
            {
                apply.IssueQty = allApplyDetails.Where(p => p.SparePartId == apply.SparePartId).Sum(p => p.OutDepotAmount);
                //汇总【备件使用】页签该备件编码的数量
                apply.ConsumeQty = allSpareParts.Where(p => p.SparePartId == apply.SparePartId).Sum(p => p.UseQty);
                list.Add(apply);
            }
            return list;
        }

        /// <summary>
        /// 验证工时、备件申请
        /// </summary>
        /// <param name="setup">安装调试</param>
        /// <param name="oldApplys">原备件申请</param>
        /// <param name="logIds">新增工时操作</param>
        private void CheckWorkHourApply(EquipmentSetup setup, EntityList<SetupSparePartApply> oldApplys, List<double> logIds)
        {
            //验证工时
            var setupWorkHourList = setup.SetupWorkHourList.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged);
            if (setupWorkHourList.Any(p => p.EquipmentSetupPlanId <= 0))
            {
                throw new ValidationException("安装调试的工时的【工作节点】不能为空".L10N());
            }
            if (setupWorkHourList.Any(p => p.Hours <= 0))
            {
                throw new ValidationException("工时(h)必须大于0".L10N());
            }
            if (setupWorkHourList.Any(p => p.PersistenceStatus == PersistenceStatus.New))
            {
                logIds.Add(setup.Id);
            }
            foreach (var setupWorkHour in setupWorkHourList)
            {
                var hour = (decimal)Math.Round((setupWorkHour.EndDateTime - setupWorkHour.StartDateTime).TotalMilliseconds / 1000 / 3600, 1);
                if (setupWorkHour.Hours > hour)
                {
                    throw new ValidationException("工时(h)不能大于【结束时间-开始时间】".L10N());
                }
            }
            //验证备件申请
            var applyList = setup.SetupSparePartApplyList.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged);
            foreach (var apply in applyList)
            {
                var oldApply = oldApplys.FirstOrDefault(p => p.Id == apply.Id);
                if (oldApply != null)
                {
                    var max = Math.Max(oldApply.ApplyQty, apply.ConsumeQty ?? 0);
                    if (apply.ApplyQty < max)
                    {
                        throw new ValidationException("申请数量不能小于max{申请数量，消耗数量}".L10N());
                    }
                }
            }
        }

        /// <summary>
        /// 领料申请
        /// </summary>
        /// <param name="model">领料申请</param>
        /// <param name="list">领料申请明细</param>
        public virtual void MaterialApply(MaterialApplyViewModel model, List<MaterialApplyDetailViewModel> list)
        {
            if (list == null || model == null || !list.Any())
            {
                throw new ValidationException("领料数据不能为空".L10N());
            }
            var groups = list.GroupBy(p => new { p.SparePartId, p.WarehouseId });
            if (groups.Count() != list.Count)
            {
                throw new ValidationException("【备件编码+出库仓库】不能重复".L10N());
            }
            //校验本次申请数量和发料仓库都输入
            if (list.Any(p => p.ApplyQty <= 0))
            {
                throw new ValidationException("本次申请数量必须大于0".L10N());
            }
            if (list.Any(p => p.WarehouseId <= 0))
            {
                throw new ValidationException("发料仓库不能为空".L10N());
            }
            var setup = GetById<EquipmentSetup>(model.EquipmentSetupId);
            if (setup == null)
            {
                throw new ValidationException("数据异常，找不到安装调试单".L10N());
            }
            var applys = GetApplysBySetupIds(new List<double> { setup.Id });
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //申请的备件合并到备件申请子表，备件编码已存在的，更新申请数量，备件编码不存在的新增数据；
                foreach (var item in list)
                {
                    var oldApply = applys.FirstOrDefault(p => p.SparePartId == item.SparePartId);
                    if (oldApply != null)
                    {
                        oldApply.ApplyQty += item.ApplyQty;
                        RF.Save(oldApply);
                    }
                    else
                    {
                        var newApply = new SetupSparePartApply();
                        newApply.EquipmentSetupId = setup.Id;
                        newApply.ApplyQty = item.ApplyQty;
                        newApply.Remark = model.Remark;
                        newApply.RequireDateTime = model.DemandTime;
                        newApply.WarehouseId = item.WarehouseId;
                        newApply.SparePartId = item.SparePartId;
                        RF.Save(newApply);
                    }
                }
                //生成【领料申请】的操作记录；
                var now = RF.Find<EquipmentSetup>().GetDbTime();
                SaveSetupLog(new List<double> { setup.Id }, "领料申请".L10N(), now);
                //生成备件申请单
                SaveSparePartApp(setup, model, list);
                trans.Complete();
            }
        }

        /// <summary>
        /// 生成备件申请单
        /// </summary>
        /// <param name="setup">安装调试</param>
        /// <param name="model">领料申请</param>
        /// <param name="list">领料申请明细</param>
        private void SaveSparePartApp(EquipmentSetup setup, MaterialApplyViewModel model, List<MaterialApplyDetailViewModel> list)
        {
            var sparePartApp = new SparePartApp();
            sparePartApp.No = RT.Service.Resolve<SparePartAppController>().GetNo();
            sparePartApp.FromNo = setup.SetupNo;
            sparePartApp.FromType = FromType.Setup;
            sparePartApp.DemandDate = model.DemandTime;
            sparePartApp.AuditState = AuditState.StandAudit;
            sparePartApp.QualityStatus = QualityStatus.Good;

            if (!setup.DepartmentId.HasValue)
            {
                throw new ValidationException("安装调试单【{0}】的部门为空，不能生成备件申请单。".L10nFormat(setup.SetupNo));
            }

            sparePartApp.GetDepartmentId = setup.DepartmentId.Value;

            sparePartApp.Remark = model.Remark;
            RF.Save(sparePartApp);

            //申请单明细
            foreach (var item in list)
            {
                var applyDetail = new ApplyDetail();
                applyDetail.SparePartAppId = sparePartApp.Id;
                applyDetail.SparePartId = item.SparePartId;
                applyDetail.ApplyAmount = (int)item.ApplyQty;
                applyDetail.WarehouseId = item.WarehouseId;
                RF.Save(applyDetail);
            }
        }
    }
}
