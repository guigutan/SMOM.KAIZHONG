using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.SparePartAcceptances.ViewModels;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收控制器
    /// </summary>
    public partial class SparePartAcceptanceController : DomainController
    {
        /// <summary>
        /// 查询备件验收信息
        /// </summary>
        /// <param name="criteria">备件验收查询实体</param>
        /// <returns>备件验收信息</returns>
        public virtual EntityList<SparePartAcceptance> CriteriaSparePartAcceptances(SparePartAcceptanceCriteria criteria)
        {
            var query = Query<SparePartAcceptance>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }

            if (!criteria.AcceptanceNo.IsNullOrEmpty())
            {
                query.Where(p => p.AcceptanceNo.Contains(criteria.AcceptanceNo));
            }

            if (!criteria.ReceiveNo.IsNullOrEmpty())
            {
                query.Where(x => x.SparePartReceive.ReceiveNo.Contains(criteria.ReceiveNo));
            }

            if (criteria.ReceiveType.HasValue)
            {
                query.Where(x => x.SparePartReceive.ReceiveType == criteria.ReceiveType.Value);
            }

            if (!criteria.SparePartCode.IsNullOrEmpty())
            {
                query.Where(p => p.SparePart.SparePartCode.Contains(criteria.SparePartCode));
            }

            if (!criteria.SparePartName.IsNullOrEmpty())
            {
                query.Where(p => p.SparePart.SparePartName.Contains(criteria.SparePartName));
            }

            if (criteria.ControlMethod.HasValue)
            {
                query.Where(p => p.SparePart.ControlMethod == criteria.ControlMethod.Value);
            }

            if (!criteria.PurchaseOrderNo.IsNullOrWhiteSpace())
            {
                query.Exists<SparePartAcceptanceDetail>((a, b) => b.Where(p => p.SparePartAcceptanceId == a.Id
                    && p.PurchaseOrder.OrderNo.Contains(criteria.PurchaseOrderNo)));
            }
            if (criteria.SupplierId.HasValue)
            {
                query.Where(p => p.SupplierId == criteria.SupplierId.Value);
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
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取备件验收信息
        /// </summary>
        /// <param name="acceptIds">id列表</param>
        /// <returns>备件验收信息</returns>
        public virtual EntityList<SparePartAcceptance> GetSparePartAcceptsByIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<SparePartAcceptance>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据备件验收id列表获取验收明细列表
        /// </summary>
        /// <param name="acceptIds">备件验收id列表</param>
        /// <returns>备件验收明细列表</returns>
        public virtual EntityList<SparePartAcceptanceDetail> GetDetailsByAcceptIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<SparePartAcceptanceDetail>().Where(p => ids.Contains(p.SparePartAcceptanceId))
            .ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取批次明细
        /// </summary>
        /// <param name="acceptId">验收id</param>        
        /// <returns>批次明细</returns>
        public virtual EntityList<SparePartAcceptanceLotViewModel> GetSparePartAcceptanceLotViewModels(double acceptId)
        {
            EntityList<SparePartAcceptanceLotViewModel> sparePartAcceptanceLotViewModels = new EntityList<SparePartAcceptanceLotViewModel>();

            var lots = Query<SparePartAcceptanceLot>()
               .Where(p => p.AccepDtl.SparePartAcceptanceId == acceptId)
               .ToList();

            foreach (var lot in lots)
            {
                sparePartAcceptanceLotViewModels.Add(new SparePartAcceptanceLotViewModel()
                {
                    Id = lot.Id,
                    LotNo = lot.LotNo,
                });
            }

            return sparePartAcceptanceLotViewModels;
        }

        /// <summary>
        /// 获取批次明细
        /// </summary>
        /// <param name="acceptId">验收id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>批次明细</returns>
        public virtual EntityList<SparePartAcceptanceLot> GetAcceptanceLotInfo(double acceptId, PagingInfo pagingInfo)
        {
            return Query<SparePartAcceptanceLot>()
                .Exists<SparePartAcceptanceDetail>((a, b) => b.Where(p => p.Id == a.AccepDtlId && p.SparePartAcceptanceId == acceptId))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次明细
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <returns>批次明细</returns>
        public virtual EntityList<SparePartAcceptanceLot> GetLotsByAcceptIds(List<double> acceptIds)
        {
            return Query<SparePartAcceptanceLot>().Exists<SparePartAcceptanceDetail>((a, b) => b.Where(p => p.Id == a.AccepDtlId
            && acceptIds.Contains(p.SparePartAcceptanceId))).ToList();
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="acceptId">验收id</param>        
        /// <returns>序列号明细</returns>
        public virtual EntityList<SparePartAcceptanceSnViewModel> GetSparePartAcceptanceSnViewModels(double acceptId)
        {
            EntityList<SparePartAcceptanceSnViewModel> sparePartAcceptanceSnViewModels = new EntityList<SparePartAcceptanceSnViewModel>();

            var snViewModels = Query<SparePartAcceptanceSn>()
               .Where(p => p.AcceptDtl.SparePartAcceptanceId == acceptId)
               .ToList<SparePartAcceptanceSnViewModel>();

            foreach (var sn in snViewModels)
            {
                sparePartAcceptanceSnViewModels.Add(new SparePartAcceptanceSnViewModel()
                {   
                    Id = sn.Id,
                    Sn = sn.Sn,
                });
            }

            return sparePartAcceptanceSnViewModels;
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="acceptId">验收id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<SparePartAcceptanceSn> GetAcceptanceSnInfo(double acceptId, PagingInfo pagingInfo)
        {
            return Query<SparePartAcceptanceSn>().Exists<SparePartAcceptanceDetail>((a, b) => b.Where(p => p.Id == a.AcceptDtlId && p.SparePartAcceptanceId == acceptId))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取序列号明细
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <returns>序列号明细</returns>
        public virtual EntityList<SparePartAcceptanceSn> GetSnsByAcceptIds(List<double> acceptIds)
        {
            return Query<SparePartAcceptanceSn>().Exists<SparePartAcceptanceDetail>((a, b) => b.Where(p => p.Id == a.AcceptDtlId
            && acceptIds.Contains(p.SparePartAcceptanceId))).ToList();
        }

        /// <summary>
        /// 获取备件验收项目
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <returns>验收项目</returns>
        public virtual EntityList<SparePartAcceptanceItem> GetItemsByAcceptIds(List<double> acceptIds)
        {
            return acceptIds.SplitContains(ids => Query<SparePartAcceptanceItem>().Where(p => ids.Contains(p.SparePartAcceptanceId)).ToList());
        }

        /// <summary>
        /// 获取备件附件列表
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        /// <returns>备件附件列表</returns>
        public virtual EntityList<SparePartAcceptanceAttachment> GetAttachmentByAcceptIds(List<double> acceptIds)
        {
            var nullableIds = acceptIds.Cast<double?>();
            return nullableIds
                .SplitContains(ids => Query<SparePartAcceptanceAttachment>().Where(p => ids.Contains(p.OwnerId)).ToList());
        }

        /// <summary>
        /// 保存备件验收
        /// </summary>
        /// <param name="accepts">备件验收</param>
        public virtual void SaveSparePartAcceptance(EntityList<SparePartAcceptance> accepts)
        {
            if (accepts == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("状态为【待提交】、【驳回】时才能修改".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存验收数据
                SaveAcceptance(accepts);

                //保存后再更新合格数量
                var acceptIds = accepts.Select(p => p.Id).ToList();
                var newAccepts = GetSparePartAcceptsByIds(acceptIds);
                var allDetails = GetDetailsByAcceptIds(acceptIds);
                var allLots = GetLotsByAcceptIds(acceptIds);
                var allSns = GetSnsByAcceptIds(acceptIds);
                foreach (var accept in newAccepts)
                {
                    var details = allDetails.Where(p => p.SparePartAcceptanceId == accept.Id).ToList();
                    var detailIds = details.Select(p => p.Id).ToList();
                    var lots = allLots.Where(p => detailIds.Contains(p.AccepDtlId)).ToList();
                    var sns = allSns.Where(p => detailIds.Contains(p.AcceptDtlId)).ToList();
                    if (accept.ControlMethod != ControlMethod.ItemCode)
                    {
                        foreach (var detail in details)
                        {
                            if (accept.ControlMethod == ControlMethod.Batch)
                            {
                                detail.PassQty = lots.Sum(p => p.PassQty);
                                detail.UnqualifiedQty = lots.Sum(p => p.UnqualifiedQty);
                            }
                            else
                            {
                                detail.PassQty = sns.Count(p => p.AcceptanceResult == InspectionResult.Pass);
                                detail.UnqualifiedQty = sns.Count(p => p.AcceptanceResult == InspectionResult.Fail);
                            }
                            RF.Save(detail);
                        }
                    }
                    accept.PassQty = details.Sum(p => p.PassQty);
                    accept.UnqualifiedQty = details.Sum(p => p.UnqualifiedQty);
                }
                RF.Save(newAccepts);
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存验收数据
        /// </summary>
        /// <param name="accepts">验收数据</param>
        private void SaveAcceptance(EntityList<SparePartAcceptance> accepts)
        {
            RF.Save(accepts);
            var toSaveLot = new EntityList<SparePartAcceptanceLot>();
            var toSaveSn = new EntityList<SparePartAcceptanceSn>();
            foreach (var accept in accepts)
            {
                var values = accept.ExtValues.Values;
                foreach (var value in values)
                {
                    var lotList = value as EntityList<SparePartAcceptanceLot>;
                    if (lotList != null)
                    {
                        if (lotList.Any(p => p.PassQty > p.Qty || p.UnqualifiedQty > p.Qty))
                        {
                            throw new ValidationException("合格数量和不合格数量不能大于批次数量".L10N());
                        }
                        if (lotList.Any(p => p.AcceptanceResult == InspectionResult.Pass && p.PassQty <= 0))
                        {
                            throw new ValidationException("验收状态为合格时，合格数量必须大于0".L10N());
                        }
                        if (lotList.Any(p => p.AcceptanceResult == InspectionResult.Pass && p.UnqualifiedQty >= p.Qty))
                        {
                            throw new ValidationException("验收状态为合格时，不合格数量不能大于等于批次数量".L10N());
                        }
                        toSaveLot.AddRange(lotList);
                    }
                    var snList = value as EntityList<SparePartAcceptanceSn>;
                    if (snList != null)
                    {
                        toSaveSn.AddRange(snList);
                    }
                }
            }
            RF.Save(toSaveLot);
            RF.Save(toSaveSn);
        }

        /// <summary>
        /// 提交备件验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        public virtual void SubmitSparePartAcceptance(List<double> acceptIds)
        {
            var config = RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(typeof(SparePartAcceptance));
            var accepts = GetSparePartAcceptsByIds(acceptIds);
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var allDetails = GetDetailsByAcceptIds(acceptIds);
            var allLots = GetLotsByAcceptIds(acceptIds);
            var allSns = GetSnsByAcceptIds(acceptIds);
            var allItems = GetItemsByAcceptIds(acceptIds);
            var allAttachments = GetAttachmentByAcceptIds(acceptIds);
            foreach (var accept in accepts)
            {
                var details = allDetails.Where(p => p.SparePartAcceptanceId == accept.Id).ToList();
                if (!details.Any())
                {
                    throw new ValidationException("备件明细必须有数据".L10N());
                }
                if (details.Any(p => p.PassQty <= 0 && p.UnqualifiedQty <= 0))
                {
                    throw new ValidationException("备件明细所有行的合格数量和不合格数量必须有值".L10N());
                }
                var detailIds = details.Select(p => p.Id).ToList();
                var lots = allLots.Where(p => detailIds.Contains(p.AccepDtlId)).ToList();
                var sns = allSns.Where(p => detailIds.Contains(p.AcceptDtlId)).ToList();
                if (lots.Any(p => p.AcceptanceResult == null) || sns.Any(p => p.AcceptanceResult == null))
                {
                    throw new ValidationException("请填写序列号明细或批次明细的验收状态".L10N());
                }
                var items = allItems.Where(p => p.SparePartAcceptanceId == accept.Id).ToList();
                var attachments = allAttachments.Where(p => p.OwnerId == accept.Id).ToList();
                if (!items.Any() && !attachments.Any())
                {
                    throw new ValidationException("验收项目和附件至少有一个有值".L10N());
                }
                accept.ApprovalStatus = ApprovalStatus.PendingReview;
            }
            var now = RF.Find<SparePartAcceptance>().GetDbTime();
            //保存
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(accepts);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(SparePartAcceptance).FullName, ApprovalResult.Submit, now, "");

                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    RT.Service.Resolve<SparePartAcceptExamineController>().ExamineSparePartAcceptInner(acceptIds, ApprovalResult.Pass, "通过".L10N(), accepts);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回备件验收
        /// </summary>
        /// <param name="acceptIds">验收id</param>
        public virtual void CancelSparePartAcceptance(List<double> acceptIds)
        {
            var accepts = GetSparePartAcceptsByIds(acceptIds);
            if (accepts.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                accepts.ForEach(p => p.ApprovalStatus = ApprovalStatus.Draft);
                RF.Save(accepts);
                var now = RF.Find<SparePartAcceptance>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(acceptIds, typeof(SparePartAcceptance).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }
    }
}
