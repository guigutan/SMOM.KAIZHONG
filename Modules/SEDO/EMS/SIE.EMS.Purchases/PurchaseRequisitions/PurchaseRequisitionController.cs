using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Projects;
using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.EventMessages.EMS.MeteringEquipments;
using SIE.EventMessages.EMS.SpecialEquipments;
using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请控制器
    /// </summary>
    public partial class PurchaseRequisitionController : DomainController
    {
        /// <summary>
        /// 获取工作流服务
        /// </summary>
        /// <returns></returns>
        private PurchaseRequisitionWorkFlowService GetPurchaseRequisitionWorkFlowService()
        {
            var purchaseRequisitionConfigValue = RT.Service.Resolve<PurchasesApprovalController>()
                .GetApprovalConfigValue(typeof(PurchaseRequisition));

            if (purchaseRequisitionConfigValue.EnableApproval)
            {
                return new PurchaseRequisitionInternalWorkFlowService();
            }
            else
            {
                return new PurchaseRequisitionNoWorkFlowService();
            }
        }

        /// <summary>
        /// 查询采购申请
        /// </summary>
        /// <param name="criteria">采购申请查询实体</param>
        /// <returns>采购申请</returns>
        public virtual EntityList<PurchaseRequisition> CriteriaPurchaseRequisitions(PurchaseRequisitionCriteria criteria)
        {
            var query = Query<PurchaseRequisition>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (!criteria.No.IsNullOrWhiteSpace())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (criteria.PurchaseType.HasValue)
            {
                query.Where(p => p.PurchaseType == criteria.PurchaseType.Value);
            }
            if (criteria.PurchaseObjectType.HasValue)
            {
                query.Where(p => p.PurchaseObjectType == criteria.PurchaseObjectType.Value);
            }
            if (criteria.ProjectId.HasValue)
            {
                query.Where(p => p.ProjectId == criteria.ProjectId.Value);
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            var enumList = RT.Service.Resolve<DevicePurController>().GetUserPurchaseObjects(RT.Identity.UserId);
            query.Where(p => enumList.Contains(p.PurchaseObjectType));
            return query.Distinct().OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取采购申请列表
        /// </summary>
        /// <param name="purIds">id列表</param>
        /// <returns>采购申请列表</returns>
        public virtual EntityList<PurchaseRequisition> GetPurchaseRequisitionsByIds(List<double> purIds)
        {
            return purIds.SplitContains(ids => Query<PurchaseRequisition>().Where(p => ids.Contains(p.Id))
            .ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取项目的关键事项
        /// </summary>
        /// <param name="proId">项目id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">事项说明</param>
        /// <returns>关键事项</returns>
        public virtual EntityList<ProjectKeyItem> GetPurKeyItemsByProId(double proId, PagingInfo pagingInfo, string keyword)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ProjectKeyItem.BudgetProperty);
            elo.LoadWithViewProperty();
            return Query<ProjectKeyItem>().Where(p => p.ProjectId == proId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Description.Contains(keyword)||p.Budget.BudgetName.Contains(keyword)||p.Budget.BudgetNo.Contains(keyword))
                .ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 根据id列表查询关键事项
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>关键事项</returns>
        public virtual EntityList<ProjectKeyItem> GetProjectKeyItemsByIds(List<double?> ids)
        {
            return Query<ProjectKeyItem>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取采购申请明细
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>采购申请明细</returns>
        public virtual EntityList<PurchaseRequisitionItem> GetPurDetailsByIds(List<double> ids)
        {
            return Query<PurchaseRequisitionItem>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据采购申请id列表获取采购申请明细
        /// </summary>
        /// <param name="purIds">采购申请id列表</param>
        /// <returns>采购申请明细</returns>
        public virtual EntityList<PurchaseRequisitionItem> GetPurDetailsByPurIds(List<double> purIds)
        {
            return Query<PurchaseRequisitionItem>().Where(p => purIds.Contains(p.PurchaseRequisitionId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工厂+部门获取审核状态为【通过】且采购对象符合的采购申请行
        /// </summary>
        /// <param name="factoryId">工厂</param>
        /// <param name="departmentId">部门</param>
        /// <param name="type">采购对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>采购申请行</returns>
        public virtual EntityList<PurchaseRequisitionItem> GetPagingPurDetailByPurId(double factoryId, double? departmentId, PurchaseObjectType type, PagingInfo pagingInfo, string keyword)
        {
            return Query<PurchaseRequisitionItem>()
                .Join<PurchaseRequisition>((a, b) => a.PurchaseRequisitionId == b.Id
                    && b.PurchaseObjectType == type
                    && b.FactoryId == factoryId
                    && b.DepartmentId == departmentId
                    && b.ApprovalStatus == ApprovalStatus.Audited)
                .WhereIf(!keyword.IsNullOrWhiteSpace(),
                    p => p.PurchaseRequisition.No.Contains(keyword)
                        || p.Description.Contains(keyword)
                        || p.ObjectCode.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取采购对象编码
        /// </summary>
        /// <param name="type">采购对象</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>采购对象编码</returns>
        public virtual EntityList<ObjectCodeInfo> GetObjectCodeInfos(PurchaseObjectType type, PagingInfo pagingInfo, string keyword)
        {
            var list = new EntityList<ObjectCodeInfo>();
            switch (type)
            {
                case PurchaseObjectType.Equipment:
                    AddEquipModelInfo(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.SparePart:
                    AddSparePartInfo(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.Excipients:
                    AddExcipientInfo(list, SparePartType.Consumables, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.Mold:
                    //模具基础数据（此类型等模具模块开发后再开发）
                    break;
                case PurchaseObjectType.Fixture:
                    AddFixtureInfo(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.Tool:
                    AddExcipientInfo(list, SparePartType.Tool, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.OutsourcedRepair:
                    AddRepairInfo(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.OutsourcedMaintainance:
                    AddMaintainanceInfo(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.OutsourcedRegularInspection:
                    //特种设备定检
                    AddInspectionInfo(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.OutsourcedCalibration:
                    //计量设备定检
                    AddCalibrationInfo(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.OutsourcedInstall:
                    //可选委外的安装调试单号，后续补充
                    AddOutsourcedInstall(list, pagingInfo, keyword);
                    break;
                case PurchaseObjectType.Engineering:
                    AddEquipModelInfo(list, pagingInfo, keyword);
                    break;
                default:
                    break;
            }
            return list;
        }
       
        /// <summary>
        /// 添加设备型号基础数据
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddEquipModelInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            var models = Query<EquipModel>().WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)||p.Name.Contains(keyword)).ToList(pagingInfo);
            foreach (var model in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = model.Code,
                    Value = model.Code,
                    Name = model.Name,
                    Specification = model.Specifications
                });
            }
            list.SetTotalCount(models.TotalCount);
        }

        /// <summary>
        /// 添加备件基础数据中，状态为可用，且关联的备件类型的类型不等于【耗材类】、【工具类】的数据
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddSparePartInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SparePart.UnitProperty);
            var models = Query<SparePart>().Where(p => p.State == State.Enable && p.SpartType != SparePartType.Consumables && p.SpartType != SparePartType.Tool).WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.SparePartCode.Contains(keyword)).ToList(pagingInfo, elo);
            foreach (var model in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = model.SparePartCode,
                    Value = model.SparePartCode,
                    Name = model.SparePartName,
                    Specification = model.Specification,
                    ItemUnitId = model.UnitId,
                    ItemUnitNmae = model.Unit == null ? "" : model.Unit.Name
                });
            }
            list.SetTotalCount(models.TotalCount);
        }
        /// <summary>
        /// 添加所有备件基础数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        private void AddAllSparePartInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SparePart.UnitProperty);
            var models = Query<SparePart>().WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.SparePartCode.Contains(keyword)).ToList(pagingInfo, elo);
            foreach (var model in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = model.SparePartCode,
                    Value = model.SparePartCode,
                    Name = model.SparePartName,
                    Specification = model.Specification,
                    ItemUnitId = model.UnitId,
                    ItemUnitNmae = model.Unit == null ? "" : model.Unit.Name
                });
            }
            list.SetTotalCount(models.TotalCount);
        }
        /// <summary>
        /// 添加备件基础数据中，状态为可用，且关联的备件类型的类型等于【耗材类】/【工具类】
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="type">备件类型</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddExcipientInfo(EntityList<ObjectCodeInfo> list, SparePartType type, PagingInfo pagingInfo, string keyword)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SparePart.UnitProperty);
            var models = Query<SparePart>().Where(p => p.State == State.Enable && p.SpartType == type)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.SparePartCode.Contains(keyword)).ToList(pagingInfo, elo);
            foreach (var model in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = model.SparePartCode,
                    Value = model.SparePartCode,
                    Name = model.SparePartName,
                    Specification = model.Specification,
                    ItemUnitId = model.UnitId,
                    ItemUnitNmae = model.Unit == null ? "" : model.Unit.Name
                });
            }
            list.SetTotalCount(models.TotalCount);
        }

        /// <summary>
        /// 添加工治具基础数据
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddFixtureInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            var models = Query<FixtureEncode>().WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.Code.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var model in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = model.Code,
                    Value = model.Code,
                    ModelCode = model.ModelCode,
                    Name = model.ModelName,
                    ItemUnitId = model.UnitId,
                    ItemUnit = model.FixtureModel.Unit,
                    ItemUnitNmae = model.FixtureModel.Unit == null ? "" : model.FixtureModel.Unit.Name
                }) ;
            }
            list.SetTotalCount(models.TotalCount);
        }

        /// <summary>
        /// 添加维修状态为报修、待维修、维修中、暂停中的维修单号
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddRepairInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            var models = Query<EquipRepairBill>().Where(p => p.RepairState == EquipRepairState.ApplyRepair || p.RepairState == EquipRepairState.WaitRepair || p.RepairState == EquipRepairState.Repairing || p.RepairState == EquipRepairState.Suspending)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.RepairNo.Contains(keyword)).ToList(pagingInfo,new EagerLoadOptions().LoadWithViewProperty());
            foreach (var repair in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = repair.RepairNo,
                    Value = repair.RepairNo,
                    Name = repair.EquipAccountName,
                    ModelCode = repair.EquipModelCode
                });
            }
            list.SetTotalCount(models.TotalCount);
        }

        /// <summary>
        /// 添加保养状态为未执行、超期、执行中的保养单号
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddMaintainanceInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            var models = Query<MaintainPlan>().Where(p => p.ExeState == MaintExeState.NotPerformed || p.ExeState == MaintExeState.Overdue
                || p.ExeState == MaintExeState.Performing)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.MaintainNo.Contains(keyword)).ToList(pagingInfo);
            foreach (var maintainNo in models.Select(x => x.MaintainNo))
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = maintainNo,
                    Value = maintainNo
                });
            }
            list.SetTotalCount(models.TotalCount);
        }

        /// <summary>
        /// 可选检验状态为待检验、检验中且检验类型是【外检】的定检任务
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddInspectionInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            var models = RT.Service.Resolve<IRegularInspection>().PurchaseGetRegularInspectionNo(pagingInfo, keyword);
            foreach (var no in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = no,
                    Value = no
                });
            }
            list.SetTotalCount(models.Count);
        }

        /// <summary>
        /// 可选检验状态为待检验、检验中且检验类型是【外检】的计量任务
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddCalibrationInfo(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            var models = RT.Service.Resolve<ICalibration>().PurchaseGetCalibrationNo(pagingInfo, keyword);
            foreach (var Calibration in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = Calibration.InspectionNo,
                    Value = Calibration.InspectionNo,
                    Name = Calibration.PlanNmae
                });
            }
            list.SetTotalCount(models.Count);
        }

        /// <summary>
        /// 可选状态为待执行、执行中、审核状态为已审批且类型是委外的单据
        /// </summary>
        /// <param name="list">采购对象编码列表</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        private void AddOutsourcedInstall(EntityList<ObjectCodeInfo> list, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<EquipmentSetup>().Where(p => p.OutSource && p.ApprovalStatus == ApprovalStatus.Audited && (p.SetupStatus == Enums.SetupStatus.ToBe || p.SetupStatus == Enums.SetupStatus.Doing));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.SetupNo.Contains(keyword));
            }
            var models = query.ToList(pagingInfo);

            foreach (var no in models)
            {
                list.Add(new ObjectCodeInfo()
                {
                    Id = no.Id.ToString(),
                    Value = no.SetupNo
                });
            }
            list.SetTotalCount(models.Count);
        }

        /// <summary>
        /// 创建一个新的采购申请
        /// </summary>
        /// <returns>新的采购申请</returns>
        public virtual PurchaseRequisition GetNewPurchaseRequisition()
        {
            var pur = new PurchaseRequisition();
            pur.No = RT.Service.Resolve<CommonController>().GetNo<PurchaseRequisition>("采购申请");
            pur.Currency = Currency.CNY;
            pur.ApprovalStatus = ApprovalStatus.Draft;
            pur.PurchaseType = PurchaseType.ByProject;
            var enumList = RT.Service.Resolve<DevicePurController>().GetUserPurchaseObjects(RT.Identity.UserId);
            if (enumList.Any())
            {
                pur.PurchaseObjectType = enumList.FirstOrDefault();
            }
            return pur;
        }

        /// <summary>
        /// 保存采购申请
        /// </summary>
        /// <param name="pur">采购申请</param>
        public virtual void SavePurchaseRequisition(PurchaseRequisition pur)
        {
            if (pur == null)
            {
                throw new ValidationException("保存采购申请失败，数据异常".L10N());
            }
            if (pur.PersistenceStatus != PersistenceStatus.New)
            {
                var oldPur = GetById<PurchaseRequisition>(pur.Id);
                if (oldPur == null)
                {
                    throw new ValidationException("保存采购申请失败，数据异常".L10N());
                }
                if (oldPur.ApprovalStatus != ApprovalStatus.Draft && oldPur.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("保存采购申请失败，审核状态已不是【待提交】、【驳回】".L10N());
                }
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(pur);

                //保存后再更新品种数和总数量
                var details = GetPurDetailsByPurIds(new List<double> { pur.Id });
                if (details.Any(p => p.ObjectCode.IsNullOrWhiteSpace() && p.Description.IsNullOrWhiteSpace()))
                {
                    throw new ValidationException("采购编码没有选择时,采购对象描述不能为空".L10N());
                }
                if (pur.PurchaseType == PurchaseType.ByProject)
                {
                    if (pur.ProjectId == null)
                    {
                        throw new ValidationException("采购类型为【项目采购】时，项目不能为空".L10N());
                    }
                    if (details.Any(p => p.ProjectKeyItemId == null))
                    {
                        throw new ValidationException("采购类型为【项目采购】时，项目事项必输".L10N());
                    }
                }
                if (pur.PurchaseObjectType != PurchaseObjectType.OutsourcedRepair && pur.PurchaseObjectType != PurchaseObjectType.OutsourcedMaintainance &&
                    pur.PurchaseObjectType != PurchaseObjectType.OutsourcedRegularInspection && pur.PurchaseObjectType != PurchaseObjectType.OutsourcedCalibration)
                {
                    if (details.Any(p => p.ItemUnitId == null))
                    {
                        throw new ValidationException("采购对象不是委外时单位必输".L10N());
                    }
                }
                else
                {
                    if (details.Any(p => p.ItemUnitId != null))
                    {
                        throw new ValidationException("采购对象是委外时单位必须为空".L10N());
                    }
                }

                //保存采购申请明细
                details.ForEach(p => p.PurchaseRequisitionNoLine = pur.No + "-" + p.LineNo.ToString());
                RF.Save(details);

                pur.VarietyQuantity = details.GroupBy(p => new { p.ObjectCode, p.Description }).Count();
                pur.TotalAmount = details.Sum(p => p.Qty);
                RF.Save(pur);

                trans.Complete();
            }
        }

        /// <summary>
        /// 删除前校验最新状态
        /// </summary>
        /// <param name="ids">实体id</param>
        public virtual void DeletePurchaseRequisition(List<double> ids)
        {
            var purs = GetPurchaseRequisitionsByIds(ids);
            if (purs.Any(p => p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("只有审核状态为【待提交】的数据才能删除".L10N());
            }
        }

        /// <summary>
        /// 提交采购
        /// </summary>
        /// <param name="purIds">采购id</param>
        public virtual void SubmitPurRequire(List<double> purIds)
        {
            var config = RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(typeof(PurchaseRequisition));
            //验证
            var purs = GetPurchaseRequisitionsByIds(purIds);

            //验证是否有明细
            var allDetails = GetPurDetailsByPurIds(purIds);

            foreach (var pur in purs)
            {
                if (!allDetails.Any(p => p.PurchaseRequisitionId == pur.Id))
                {
                    throw new ValidationException("采购明细不能为空".L10N());
                }
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //提交
                GetPurchaseRequisitionWorkFlowService().Submit(purs);

                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExaminePurRequire(purIds, ApprovalResult.Pass, "通过".L10N());
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回采购
        /// </summary>
        /// <param name="purIds">采购id</param>
        public virtual void Withdraw(List<double> purIds)
        {
            var purs = GetPurchaseRequisitionsByIds(purIds);

            //撤回
            GetPurchaseRequisitionWorkFlowService().Withdraw(purs);
        }

        /// <summary>
        /// 审核采购
        /// </summary>
        /// <param name="purIds">采购id</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">备注</param>
        public virtual void ExaminePurRequire(List<double> purIds, ApprovalResult value, string remark)
        {
            var purs = GetPurchaseRequisitionsByIds(purIds);

            if (value == ApprovalResult.Pass)
            {
                //提交
                GetPurchaseRequisitionWorkFlowService().Approved(purs, remark);
            }
            else
            {
                //提交
                GetPurchaseRequisitionWorkFlowService().Reject(purs, remark);
            }

        }
    }
}