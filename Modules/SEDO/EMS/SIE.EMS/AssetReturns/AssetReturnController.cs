using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.EMS.AssetIssues;
using SIE.Fixtures;
using SIE.Equipments.EquipAccounts;
using SIE.EMS.AssetRequisitions;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Equipments.EquipmentCards;
using SIE.Fixtures.Models;
using SIE.Fixtures.MaintainTasks;
using SIE.Core.Common.Controllers;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.EMS.Common.Utils;
using SIE.Core.Enums;

namespace SIE.EMS.AssetReturns
{
    /// <summary>
    /// 资产归还单控制器
    /// </summary>
    public class AssetReturnController : DomainController
    {
        #region 资产归还单号生成规则
        /// <summary>
        /// 获取自动生成资产归还单号No
        /// </summary>
        /// <returns>编号</returns>
        public virtual string GetNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AssetReturn));

            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到资产归还单号生成规则,请检查规则配置".L10N());
            var code = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();

            return code;
        }
        #endregion

        #region 获取审批流配置信息
        /// <summary>
        /// 获取审批流配置信息
        /// </summary>
        /// <returns>审批流配置信息</returns>
        public virtual ApprovalConfigValue GetApprovalFlowConfigValue()
        {
            var configValue = ConfigService.GetConfig<ApprovalConfigValue>(new ApprovalConfig(), typeof(AssetReturn));

            if (configValue == null)
                throw new ValidationException("未找到审批流配置规则,请检查规则配置".L10N());

            return configValue;
        }
        #endregion

        /// <summary>
        /// 查询资产归还单
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>资产归还列表</returns>
        public virtual EntityList<AssetReturn> GetAssetReturnList(AssetReturnCriteria criteria)
        {
            var q = Query<AssetReturn>();

            if (criteria.ReturnNo.IsNotEmpty())
            {
                q.Where(p => p.ReturnNo.Contains(criteria.ReturnNo));
            }
            if (criteria.RequisitionNo.IsNotEmpty())
            {
                q.Where(p => p.AssetRequisition.RequisitionNo.Contains(criteria.RequisitionNo));
            }
            if (criteria.QureyFactoryId != null && criteria.QureyFactoryId != 0)
            {
                q.Where(p => p.FactoryId == criteria.QureyFactoryId);
            }
            if (criteria.AssetObject != null)
            {
                q.Where(p => p.AssetRequisition.AssetObject == criteria.AssetObject);
            }
            if (criteria.ApplyDepartmentId != null && criteria.ApplyDepartmentId != 0)
            {
                q.Where(p => p.ApplyDepartmentId == criteria.ApplyDepartmentId);
            }
            if (criteria.LendingDepartmentId != null && criteria.LendingDepartmentId != 0)
            {
                q.Where(p => p.LendingDepartmentId == criteria.LendingDepartmentId);
            }
            if (criteria.WarehouseId != null && criteria.WarehouseId != 0)
            {
                q.Where(p => p.WarehouseId == criteria.WarehouseId);
            }
            if (criteria.ApprovalStatus != null)
            {
                q.Where(p => p.ApprovalStatus == criteria.ApprovalStatus);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可归还的设备清单
        /// </summary>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>设备清单集合</returns>
        public virtual EntityList<AssetReturnEquipment> GetAssetReturnEquipmentsById(double returnId, double requisitionId)
        {
            var returnList = Query<AssetReturnEquipment>().Where(p => p.AssetReturnId == returnId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var issueEquipList = Query<AssetIssueEquipment>().Where(p => p.AssetRequisitionEquipment.AssetRequisitionId == requisitionId && p.AssetIssue.ApprovalStatus == SIE.Equipments.Enums.ApprovalStatus.Audited)
                                                             .NotExists<AssetReturnEquipment>((x, y) => y.Where(p => p.EquipAccountId == x.EquipAccountId && p.AssetIssueEquipmentId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var issueEquip in issueEquipList)
            {
                var returnEquip = new AssetReturnEquipment();
                returnEquip.AssetIssueEquipmentId = issueEquip.Id;
                returnEquip.AssetRequisitionEquipmentId = issueEquip.AssetRequisitionEquipmentId;
                returnEquip.LineNo = issueEquip.LineNo;
                returnEquip.EquipAccountId = (double)issueEquip.EquipAccountId;
                returnEquip.EquipAccountCode = issueEquip.EquipAccountCode;
                returnEquip.EquipAccountName = issueEquip.EquipAccountName;
                returnEquip.Alias = issueEquip.Alias;
                returnEquip.EquipModelCode = issueEquip.EquipModelCode;
                returnEquip.EquipModelName = issueEquip.EquipModelName;
                returnEquip.Specifications = issueEquip.Specifications;
                returnList.Add(returnEquip);
            }

            var list = new EntityList<AssetReturnEquipment>();
            list.AddRange(returnList.OrderBy(p => p.LineNo));
            return list;
        }

        /// <summary>
        /// 获取已归还的设备清单
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>已归还的设备清单集合</returns>
        public virtual EntityList<AssetReturnEquipment> GetExistAssetReturnEquipmentsById(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, double returnId, double requisitionId)
        {
            if (!sortInfo.Any())
            {
                OrderInfo orderInfo = new OrderInfo();
                orderInfo.Property = "LineNo";
                orderInfo.SortOrder = System.ComponentModel.ListSortDirection.Ascending;
                orderInfo.SortIndex = 1;
                sortInfo.Add(orderInfo);
            }

            var returnList = Query<AssetReturnEquipment>()
                .Where(p => p.AssetReturnId != returnId && p.AssetRequisitionEquipment.AssetRequisitionId == requisitionId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetReturnEquipment.AssetReturnProperty));

            foreach (var retEquip in returnList)
            {
                retEquip.ReturnNo = retEquip.AssetReturn.ReturnNo;
                retEquip.ApprovalStatus = retEquip.AssetReturn.ApprovalStatus;
            }

            return returnList;
        }

        /// <summary>
        /// 获取已归还的工治具清单
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>已归还的工治具清单集合</returns>
        public virtual EntityList<AssetReturnFixture> GetExistAssetReturnFixturesById(IList<OrderInfo> sortInfo, PagingInfo pagingInfo, double returnId, double requisitionId)
        {
            if (!sortInfo.Any())
            {
                OrderInfo orderInfo = new OrderInfo();
                orderInfo.Property = "LineNo";
                orderInfo.SortOrder = System.ComponentModel.ListSortDirection.Ascending;
                orderInfo.SortIndex = 1;
                sortInfo.Add(orderInfo);
            }

            var returnList = Query<AssetReturnFixture>()
                .Where(p => p.AssetReturnId != returnId && p.AssetRequisitionFixture.AssetRequisitionId == requisitionId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetReturnFixture.AssetReturnProperty).LoadWith(AssetReturnFixture.AssetRequisitionFixtureProperty));

            foreach (var retFixture in returnList)
            {
                retFixture.ReturnNo = retFixture.AssetReturn.ReturnNo;
                retFixture.ApprovalStatus = retFixture.AssetReturn.ApprovalStatus;
                retFixture.NotReturnQty = retFixture.AssetRequisitionFixture.IssuedQty - retFixture.AssetRequisitionFixture.ReturnQty - retFixture.AssetRequisitionFixture.NoGoodsReturnQty;
            }

            return returnList;
        }

        /// <summary>
        /// 获取可归还的工治具清单
        /// </summary>
        /// <param name="returnId">归还单Id</param>
        /// <param name="requisitionId">领用单Id</param>
        /// <returns>工治具清单集合</returns>
        public virtual EntityList<AssetReturnFixture> GetAssetReturnFixturesById(double returnId, double requisitionId)
        {
            var returnList = Query<AssetReturnFixture>().Where(p => p.AssetReturnId == returnId).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetReturnFixture.AssetRequisitionFixtureProperty));

            returnList.ForEach(p =>
            {
                if (p.ManageMode == ManageMode.Number)
                {
                    p.NotReturnQty = 1;
                }
                else
                {
                    //todo:借用单行号的【发放数量-汇总与借用单行号关联的归还清单的数量】
                    p.NotReturnQty = p.AssetRequisitionFixture.IssuedQty - p.AssetRequisitionFixture.ReturnQty - p.AssetRequisitionFixture.NoGoodsReturnQty;
                }
            });
            var assetReqIds = returnList.Where(p => p.ManageMode == ManageMode.Code).Select(p => p.AssetRequisitionFixtureId);

            //编码管控时，只显示【实物归还数量+无实物归还数量】小于发放数量的借用单行，一行显示一条数据】；
            var reqFixtureList = Query<AssetRequisitionFixture>().Where(p => p.FixtureEncode.FixtureModel.ManageMode == ManageMode.Code
                                                                     && p.AssetRequisitionId == requisitionId && p.IssuedQty > (p.ReturnQty + p.NoGoodsReturnQty)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var reqFixture in reqFixtureList)
            {
                if (!assetReqIds.Contains(reqFixture.Id))
                {
                    int notReturnQty = reqFixture.IssuedQty - reqFixture.ReturnQty - reqFixture.NoGoodsReturnQty;
                    var returnFixture = new AssetReturnFixture();

                    returnFixture.AssetIssueFixtureId = null;
                    returnFixture.AssetRequisitionFixtureId = reqFixture.Id;
                    returnFixture.LineNo = reqFixture.LineNo;
                    returnFixture.FixtureEncode = reqFixture.FixtureEncodeCode;
                    returnFixture.ModelCode = reqFixture.ModelCode;
                    returnFixture.ModelName = reqFixture.ModelName;
                    returnFixture.FixtureType = reqFixture.FixtureType;
                    returnFixture.ManageMode = ManageMode.Code;
                    returnFixture.NotReturnQty = notReturnQty;
                    returnFixture.Qty = 0;
                    returnFixture.UnitName = reqFixture.UnitName;
                    returnList.Add(returnFixture);
                }
            }

            //ID管控时获取所有发放单和发放清单的序列号，展示所有未关联归还清单的序列号；修改界面时，在新增界面的基础上增加归还清单已有的数据；
            var issueFixtureList = Query<AssetIssueFixture>().Where(p => p.FixtureAccountId != null && p.AssetRequisitionFixture.AssetRequisitionId == requisitionId && p.AssetIssue.ApprovalStatus == SIE.Equipments.Enums.ApprovalStatus.Audited)
                                                             .NotExists<AssetReturnFixture>((x, y) => y.Where(p => p.FixtureAccountId != null && p.FixtureAccountId == x.FixtureAccountId && p.AssetIssueFixtureId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetReturnFixture.FixtureAccountProperty));

            foreach (var issueFixture in issueFixtureList)
            {
                var returnFixture = new AssetReturnFixture();

                returnFixture.AssetIssueFixtureId = issueFixture.Id;
                returnFixture.AssetRequisitionFixtureId = issueFixture.AssetRequisitionFixtureId;
                returnFixture.LineNo = issueFixture.LineNo;
                returnFixture.FixtureEncode = issueFixture.FixtureEncode;
                returnFixture.ModelCode = issueFixture.ModelCode;
                returnFixture.ModelName = issueFixture.ModelName;
                returnFixture.FixtureType = issueFixture.FixtureType;
                returnFixture.ManageMode = ManageMode.Number;
                returnFixture.FixtureAccountId = issueFixture.FixtureAccountId;
                returnFixture.Sn = issueFixture.FixtureAccount.Code;
                returnFixture.NotReturnQty = 1;
                returnFixture.Qty = 0;
                returnFixture.UnitName = issueFixture.UnitName;
                returnList.Add(returnFixture);
            }

            var list = new EntityList<AssetReturnFixture>();
            list.AddRange(returnList.OrderBy(p => p.LineNo));
            return list;
        }

        /// <summary>
        /// 保存资产归还单设备明细
        /// </summary>
        /// <param name="assetReturn">含设备明细的资产归还单</param>
        public virtual void SaveAssetReturnEquipment(AssetReturn assetReturn)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var returnEquipDtl in assetReturn.AssetReturnEquipmentList)
                {
                    if (returnEquipDtl.IsSelected)
                    {
                        returnEquipDtl.PersistenceStatus = returnEquipDtl.CreateBy == 0 ? PersistenceStatus.New : PersistenceStatus.Modified;
                    }
                    else
                    {
                        returnEquipDtl.PersistenceStatus = returnEquipDtl.CreateBy == 0 ? PersistenceStatus.Unchanged : PersistenceStatus.Deleted;
                    }
                }

                if (assetReturn.AssetReturnAttachmentList.Any()) 
                {                    
                    var attachList = new EntityList<AssetReturnAttachment>();

                    //已上传的附件不能更新，所以改用删除再新增的方式
                    foreach (var item in assetReturn.AssetReturnAttachmentList)
                    {
                        var attach = new AssetReturnAttachment();
                        attach.FileName = item.FileName;
                        attach.FileExtesion = item.FileExtesion;
                        attach.FilePath = item.FilePath;
                        attach.FileSize = item.FileSize;
                        attach.Content = Convert.FromBase64String(FileUrlHelper.GetAttachmentBase64StringData(attach.FilePath, attach.FileName));
                        attach.EquipmentCodes = item.EquipmentCodes;
                        attach.FixtureCodes = item.FixtureCodes;
                        attachList.Add(attach);
                        item.PersistenceStatus = PersistenceStatus.Deleted;
                    }
                    assetReturn.AssetReturnAttachmentList.AddRange(attachList);
                }

                RF.Save(assetReturn);

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存资产归还单工治具明细
        /// </summary>
        /// <param name="assetReturn">含工治具明细的资产归还单</param>
        public virtual void SaveAssetReturnFixture(AssetReturn assetReturn)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var returnEquipDtl in assetReturn.AssetReturnFixtureList)
                {
                    if (returnEquipDtl.IsSelected)
                    {
                        returnEquipDtl.FixtureEncode = returnEquipDtl.AssetRequisitionFixture.FixtureEncode.Code;
                        if (assetReturn.AssetReturnFixtureList.Count(p => p.Sn.IsNullOrEmpty()
                                    && p.FixtureEncode == returnEquipDtl.FixtureEncode
                                    && p.QualityStatus == returnEquipDtl.QualityStatus && p.ReturnType == returnEquipDtl.ReturnType) > 1)
                        {
                            throw new ValidationException("行号【{0}】工治具的归还类型和质量状态的数据重复，请确认".L10nFormat(returnEquipDtl.AssetRequisitionFixture.LineNo));
                        }
                        returnEquipDtl.PersistenceStatus = returnEquipDtl.CreateBy == 0 ? PersistenceStatus.New : PersistenceStatus.Modified;
                    }
                    else
                    {
                        returnEquipDtl.PersistenceStatus = returnEquipDtl.CreateBy == 0 ? PersistenceStatus.Unchanged : PersistenceStatus.Deleted;
                    }
                }

                if (assetReturn.AssetReturnAttachmentList.Any())
                {                    
                    var attachList = new EntityList<AssetReturnAttachment>();

                    //已上传的附件不能更新，所以改用删除再新增的方式
                    foreach (var item in assetReturn.AssetReturnAttachmentList)
                    {
                        var attach = new AssetReturnAttachment();
                        attach.FileName = item.FileName;
                        attach.FileExtesion = item.FileExtesion;
                        attach.FilePath = item.FilePath;
                        attach.FileSize = item.FileSize;
                        attach.Content = Convert.FromBase64String(FileUrlHelper.GetAttachmentBase64StringData(attach.FilePath, attach.FileName));
                        attach.EquipmentCodes = item.EquipmentCodes;
                        attach.FixtureCodes = item.FixtureCodes;
                        attachList.Add(attach);
                        item.PersistenceStatus = PersistenceStatus.Deleted;
                    }
                    assetReturn.AssetReturnAttachmentList.AddRange(attachList);
                }

                RF.Save(assetReturn);

                trans.Complete();
            }
        }

        /// <summary>
        /// 获取资产归还单集合
        /// </summary>
        /// <param name="idList">归还单Id集合</param>
        /// <returns>资产归还单集合</returns>
        public virtual EntityList<AssetReturn> GetAssetReturnListByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<AssetReturn>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetReturn.AssetReturnEquipmentListProperty).LoadWith(AssetReturn.AssetReturnFixtureListProperty));
            });

        }

        /// <summary>
        /// 提交资产归还单
        /// </summary>
        /// <param name="selectedIds">归还单Id集合</param>
        public virtual void SumbitAssetReturns(List<double> selectedIds)
        {
            var configValue = GetApprovalFlowConfigValue();
            var assetReturns = GetAssetReturnListByIds(selectedIds);
            if (assetReturns.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }

            foreach (var assetReturn in assetReturns)
            {
                if (assetReturn.AssetObject == Enums.AssetObject.Equipment && (assetReturn.AssetReturnEquipmentList == null || !assetReturn.AssetReturnEquipmentList.Any()))
                {
                    throw new ValidationException("归还单号【{0}】的设备清单不能为空".L10nFormat(assetReturn.ReturnNo));
                }
                if (assetReturn.AssetObject == Enums.AssetObject.Fixture && (assetReturn.AssetReturnFixtureList == null || !assetReturn.AssetReturnFixtureList.Any()))
                {
                    throw new ValidationException("归还单号【{0}】的工治具清单不能为空".L10nFormat(assetReturn.ReturnNo));
                }
            }
            var nowDate = RF.Find<AssetReturn>().GetDbTime();
            var recordIds = new List<double>();
            foreach (var item in assetReturns)
            {
                item.ApprovalStatus = ApprovalStatus.PendingReview;
                recordIds.Add(item.Id);
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(assetReturns);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(AssetReturn).FullName, ApprovalResult.Submit, nowDate, "");
                //是否启用审批为false时提交后自动审批
                if (!configValue.EnableAudit)
                {
                    ApprovalAssetReturnsInner(selectedIds, ApprovalResult.Pass, "通过".L10N(), assetReturns);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回资产归还单
        /// </summary>
        /// <param name="selectedIds">归还单Id集合</param>
        public virtual void CancelAssetReturns(List<double> selectedIds)
        {
            var assetReturns = GetAssetReturnListByIds(selectedIds);
            if (assetReturns.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> assetReturnsIds = new List<double>();
                assetReturns.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    assetReturnsIds.Add(p.Id);
                });
                RF.Save(assetReturns);
                var nowDate = RF.Find<AssetReturn>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(assetReturnsIds, typeof(AssetReturn).FullName, ApprovalResult.Retract, nowDate, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产归还单
        /// </summary>
        /// <param name="selectedIds">归还单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void ApprovalAssetReturns(List<double> selectedIds, ApprovalResult value, string remark)
        {
            if (!selectedIds.Any())
            {
                throw new ValidationException("传参异常".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovalAssetReturnsInner(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产归还单
        /// </summary>
        /// <param name="selectedIds">归还单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        /// <param name="assetReturnList">数据组</param>
        public virtual void ApprovalAssetReturnsInner(List<double> selectedIds, ApprovalResult value, string remark, EntityList<AssetReturn> assetReturnList = null)
        {
            if (assetReturnList == null)
            {
                assetReturnList = GetAssetReturnListByIds(selectedIds);
                if (!assetReturnList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            //验证只有待审核的数据才能审核
            if (assetReturnList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var nowDate = RF.Find<AssetReturn>().GetDbTime();
            var ids = new List<double>();
            assetReturnList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            //保存成功之后添加审核记录
            RF.Save(assetReturnList);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(AssetReturn).FullName, value, nowDate, remark);

            //执行发放单审批后的逻辑
            if (status == ApprovalStatus.Audited)
            {
                var returnEquipList = assetReturnList.Where(p => p.AssetObject == Enums.AssetObject.Equipment).ToList();
                var returnFixtureList = assetReturnList.Where(p => p.AssetObject == Enums.AssetObject.Fixture).ToList();

                if (returnEquipList.Any())
                {
                    //更新设备台账的相关信息
                    UpdateAssetReturnEquipments(returnEquipList);
                }

                if (returnFixtureList.Any())
                {
                    //更新工治具的相关信息
                    UpdateAssetReturnFixtures(returnFixtureList);
                }
            }
        }

        /// <summary>
        /// 归还单审批通过后更新设备台账的相关信息
        /// </summary>
        /// <param name="assetReturnList">归还单集合</param>
        public virtual void UpdateAssetReturnEquipments(List<AssetReturn> assetReturnList)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var requisitionList = assetReturnList.Select(p => p.AssetRequisitionId).SplitContains(tempIds =>
                {
                    return Query<AssetRequisition>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(AssetRequisition.AssetRequisitionEquipmentListProperty));
                });
                foreach (var assetReturn in assetReturnList)
                {
                    var requisition = requisitionList.First(p => p.Id == assetReturn.AssetRequisitionId);

                    foreach (var assetEquip in assetReturn.AssetReturnEquipmentList)
                    {
                        var reqEquipment = requisition.AssetRequisitionEquipmentList.First(p => p.Id == assetEquip.AssetRequisitionEquipmentId);

                        if (assetEquip.ReturnType == Enums.ReturnType.Real)
                        {
                            //更新设备台账
                            var equipAccountUpdate = DB.Update<EquipAccount>()
                                .Set(p => p.WorkShopId, assetEquip.WorkshopId)
                                .Set(p => p.ResourceId, assetEquip.ResourceId)
                                .Set(p => p.InstallationLocation, assetEquip.Location)
                                .Set(p => p.AdministratorId, assetEquip.DepositaryId)
                                .Set(p => p.WarehouseId, assetEquip.AssetReturn.WarehouseId)
                                .Where(p => p.Id == assetEquip.EquipAccountId);

                            if (requisition.External)
                            {
                                equipAccountUpdate.Set(p => p.UseState, AccountUseState.Using);
                            }

                            equipAccountUpdate.Execute();

                            //更新设备立卡
                            var equipCardUpdate = DB.Update<EquipmentCard>()
                                .Set(p => p.WorkShopId, assetEquip.WorkshopId)
                                .Set(p => p.ResourceId, assetEquip.ResourceId)
                                .Set(p => p.InstallationLocation, assetEquip.Location)
                                .Set(p => p.AdministratorId, assetEquip.DepositaryId)
                                .Set(p => p.WarehouseId, assetEquip.AssetReturn.WarehouseId)
                                .Where(p => p.Code == assetEquip.EquipAccount.Code);

                            if (requisition.External)
                            {
                                equipCardUpdate.Set(p => p.AccountUseState, AccountUseState.Using);
                            }

                            equipCardUpdate.Execute();

                            //更新设备履历
                            EquipAccountResume resume = new EquipAccountResume();
                            resume.EquipAccountId = assetEquip.EquipAccountId;
                            resume.State = assetEquip.EquipAccount.State;
                            resume.ResumeType = ResumeType.AssetReturn;
                            resume.No = assetReturn.ReturnNo;
                            RF.Save(resume);

                            //实物归还时，更新对应发放单的发放清单的归还状态、归还单号、实际归还日期
                            DB.Update<AssetIssueEquipment>()
                                .Set(p => p.ReturnStatus, Enums.ReturnStatus.Done)
                                .Set(p => p.ReturnNo, assetReturn.ReturnNo)
                                .Set(p => p.ReturnDate, assetReturn.ApplyDate)
                                .Where(p => p.Id == assetEquip.AssetIssueEquipmentId)
                                .Execute();
                        }

                        //更新资产领用设备清单的归还数量和无实物归还数量
                        reqEquipment.ReturnQty += assetEquip.ReturnType == Enums.ReturnType.Real ? 1 : 0;
                        reqEquipment.NoGoodsReturnQty += assetEquip.ReturnType == Enums.ReturnType.None ? 1 : 0;
                        RF.Save(reqEquipment);
                    }

                    //更新资产领用单的归还状态
                    if (requisition.AssetRequisitionEquipmentList.Sum(p => p.IssuedQty)
                        == requisition.AssetRequisitionEquipmentList.Sum(p => p.ReturnQty) + requisition.AssetRequisitionEquipmentList.Sum(p => p.NoGoodsReturnQty))
                    {
                        requisition.ReturnStatus = Enums.ReturnStatus.Done;
                    }
                    else
                    {
                        requisition.ReturnStatus = Enums.ReturnStatus.Partial;
                    }
                    RF.Save(requisition);
                }

            }
        }

        /// <summary>
        /// 归还单审批通过后更新工治具的相关信息
        /// </summary>
        /// <param name="assetReturnList">归还单集合</param>
        public virtual void UpdateAssetReturnFixtures(List<AssetReturn> assetReturnList)
        {

            //批量查询领用单
            var requisitionList = assetReturnList.Select(p => p.AssetRequisitionId).SplitContains(tempIds =>
            {
                return Query<AssetRequisition>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(AssetRequisition.AssetRequisitionFixtureListProperty));
            });

            //批量查询归还单明细
            var assetReturnFixtureList = assetReturnList.Select(p => p.Id).SplitContains(tempIds =>
            {
                return Query<AssetReturnFixture>().Where(p => tempIds.Contains(p.AssetReturnId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(AssetReturnFixture.AssetReturnProperty));
            });

            //批量查询工治具台账
            var fixtureAccountList = assetReturnFixtureList.Select(p => p.FixtureEncodeId).SplitContains(tempIds =>
              {
                  return Query<FixtureAccount>().Where(p => tempIds.Contains(p.FixtureEncodeId)).ToList(null, new EagerLoadOptions().LoadWith(FixtureAccount.FixtureEncodeProperty));
              });

            //批量查询工治具编码保养任务项目
            var maintainTaskList = assetReturnList.SelectMany(p => p.AssetReturnFixtureList).Select(p => p.AssetRequisitionFixture.FixtureEncodeId).SplitContains(tempIds =>
            {
                return Query<FixtureEncodeMaintainProject>().Where(p => tempIds.Contains(p.FixtureEncodeId) && p.InStorageMaintain).ToList(null, new EagerLoadOptions().LoadWith(FixtureModelMaintainProject.FixtureModelProperty));
            });

            var nowDate = RF.Find<AssetReturn>().GetDbTime();
            FixtureAccount fixtureAccount = null;

            foreach (var assetReturn in assetReturnList)
            {
                var requisition = requisitionList.First(p => p.Id == assetReturn.AssetRequisitionId);

                var maintainAccountIds = new List<double>();
                foreach (var assetFixture in assetReturn.AssetReturnFixtureList)
                {
                    var reqFixture = requisition.AssetRequisitionFixtureList.First(p => p.Id == assetFixture.AssetRequisitionFixtureId);
                    var assetReturnFixture = assetReturnFixtureList.First(p => p.Id == assetFixture.Id);

                    if (assetFixture.ReturnType == Enums.ReturnType.Real)
                    {
                        bool isSnControl = assetFixture.Sn.IsNotEmpty();

                        if (isSnControl)
                        {
                            fixtureAccount = fixtureAccountList.First(p => p.Id == (double)assetReturnFixture.FixtureAccountId);

                            //实物归还且是ID管控时，更新对应发放单的发放清单的归还状态、归还单号、实际归还日期
                            DB.Update<AssetIssueFixture>()
                                .Set(p => p.ReturnStatus, Enums.ReturnStatus.Done)
                                .Set(p => p.ReturnNo, assetReturn.ReturnNo)
                                .Set(p => p.ReturnDate, assetReturn.ApplyDate)
                                .Where(p => p.Id == assetFixture.AssetIssueFixtureId)
                                .Execute();
                        }
                        else
                        {
                            fixtureAccount = fixtureAccountList.First(p => p.FixtureEncodeId == assetReturnFixture.FixtureEncodeId);
                        }

                        //若工治具编码存在入库类型的保养项目则生成保养任务单
                        var tasks = maintainTaskList.Where(p => p.FixtureEncodeId == assetReturnFixture.FixtureEncodeId).ToList();
                        bool isNeedMaintain = tasks.Any();
                        bool isHasMaintain = maintainAccountIds.Contains(fixtureAccount.Id);

                        if (isNeedMaintain && !isHasMaintain)
                        {
                            CreateMaintainTaskByAssetReturn(fixtureAccount.Id, assetReturn, assetReturnFixture, assetReturnFixtureList, tasks, nowDate);

                            maintainAccountIds.Add(fixtureAccount.Id);
                        }

                        if (!isSnControl && isHasMaintain)
                        {
                            var hasMaintainAccount = assetReturnFixtureList.First(p => p.FixtureEncodeId == assetReturnFixture.FixtureEncodeId && p.MaintainTaskId != null);
                            assetReturnFixture.MaintainTaskId = hasMaintainAccount.MaintainTaskId;
                            assetReturnFixture.MaintainTask = hasMaintainAccount.MaintainTask;
                        }

                        if (isSnControl)
                        {
                            //更新ID类工治具台账的状态
                            UpdateFixtureStateByAssetFixture(fixtureAccount.Id, assetFixture, isNeedMaintain);
                        }
                        else
                        {
                            //更新编码类工治具台账的数量
                            UpdateFixtureQtyByAssetFixture(fixtureAccount.Id, assetFixture, isNeedMaintain);
                        }
                    }

                    //更新资产领用工治清单的归还数量和无实物归还数量
                    reqFixture.ReturnQty += assetFixture.ReturnType == Enums.ReturnType.Real ? assetFixture.Qty : 0;
                    reqFixture.NoGoodsReturnQty += assetFixture.ReturnType == Enums.ReturnType.None ? assetFixture.Qty : 0;
                    RF.Save(reqFixture);
                }

                //更新资产领用单的归还状态
                if (requisition.AssetRequisitionFixtureList.Sum(p => p.IssuedQty)
                    == requisition.AssetRequisitionFixtureList.Sum(p => p.ReturnQty) + requisition.AssetRequisitionFixtureList.Sum(p => p.NoGoodsReturnQty))
                {
                    requisition.ReturnStatus = Enums.ReturnStatus.Done;
                }
                else
                {
                    requisition.ReturnStatus = Enums.ReturnStatus.Partial;
                }
                RF.Save(requisition);

                //根据归还单和归还明细生成入库单
                CreateInboundOrderByAssetReturn(assetReturn, assetReturnFixtureList.Where(p => p.AssetReturnId == assetReturn.Id && p.ReturnType == Enums.ReturnType.Real).ToList());
            }
        }

        /// <summary>
        /// 根据资产归还明细更新ID类工治具台账的状态
        /// </summary>
        /// <param name="fixtureAccountId">ID类工治具台账Id</param>
        /// <param name="assetFixture">资产归还明细</param>
        /// <param name="isNeedMaintain">是否需要保养</param>
        public virtual void UpdateFixtureStateByAssetFixture(double fixtureAccountId, AssetReturnFixture assetFixture, bool isNeedMaintain)
        {
            var updateQueryer = DB.Update<FixtureAccount>()
                                  .Set(p => p.AccountState, isNeedMaintain ? FixtureAccountState.WaitMaintain : FixtureAccountState.WaitShelf)
                                  .Set(p => p.QualityState, assetFixture.QualityStatus);

            updateQueryer.Where(p => p.Id == fixtureAccountId).Execute();
        }

        /// <summary>
        /// 根据资产归还明细更新编码类工治具台账的数量
        /// </summary>
        /// <param name="fixtureAccountId">编码类工治具台账Id</param>
        /// <param name="assetFixture">资产归还明细</param>
        /// <param name="isNeedMaintain">是否需要保养</param>
        public virtual void UpdateFixtureQtyByAssetFixture(double fixtureAccountId, AssetReturnFixture assetFixture,bool isNeedMaintain) 
        {
            var updateQueryer = DB.Update<FixtureAccount>()
                                  .Set(p => p.TotalQty, p => p.TotalQty + assetFixture.Qty);

            if (isNeedMaintain)
            {
                updateQueryer.Set(p => p.WaitMaintain, p => p.WaitMaintain + assetFixture.Qty)
                             .Set(p => p.PassQty, p => p.PassQty + assetFixture.Qty);
            }
            else 
            {
                updateQueryer.Set(p => p.WaitShelfQty, p => p.WaitShelfQty + assetFixture.Qty);

                if (assetFixture.QualityStatus == FixtureQualityState.Pass)
                {
                    updateQueryer.Set(p => p.PassQty, p => p.PassQty + assetFixture.Qty);
                }
                else
                {
                    updateQueryer.Set(p => p.NgQty, p => p.NgQty + assetFixture.Qty);
                }
            }

            updateQueryer.Where(p => p.Id == fixtureAccountId).Execute();
        }

        /// <summary>
        /// 通过资产归还单生成工治具入库单
        /// </summary>
        /// <param name="fixtureAccountId">工治具台账Id</param>
        /// <param name="assetReturn">资产归还单</param>
        /// <param name="assetReturnFixture">资产归还工治具行</param>
        /// <param name="assetReturnFixtureList">归还单工治具集合</param>
        /// <param name="tasks">保养项目</param>
        /// <param name="nowDate">当前时间</param>
        public virtual void CreateMaintainTaskByAssetReturn(double fixtureAccountId, AssetReturn assetReturn, AssetReturnFixture assetReturnFixture, IList<AssetReturnFixture> assetReturnFixtureList,IList<FixtureEncodeMaintainProject> tasks, DateTime nowDate)
        {
            int passQty = 0;
            int ngQty = 0;

            if (assetReturnFixture.Sn.IsNotEmpty())
            {
                passQty = assetReturnFixture.QualityStatus == FixtureQualityState.Pass ? assetReturnFixture.Qty : 0;
                ngQty = assetReturnFixture.QualityStatus == FixtureQualityState.Ng ? assetReturnFixture.Qty : 0;
            }
            else
            {
                var returnFixtures = assetReturnFixtureList.Where(p => p.AssetReturnId == assetReturn.Id && p.FixtureEncodeId == assetReturnFixture.FixtureEncodeId);
                passQty = returnFixtures.Where(p => p.QualityStatus == FixtureQualityState.Pass).Sum(p => p.Qty);
                ngQty = returnFixtures.Where(p => p.QualityStatus == FixtureQualityState.Ng).Sum(p => p.Qty);
            }

            var _commonController = RT.Service.Resolve<CommonController>();
            var maintainTask = new MaintainTask();
            maintainTask.No = _commonController.GetNo<MaintainTask>("保养任务编号".L10N());
            maintainTask.RelatedNo = assetReturn.ReturnNo;
            maintainTask.MaintainType = MaintainType.InStorage;
            maintainTask.FixtureAccountId = fixtureAccountId;
            maintainTask.State = MaintainState.Wait;
            maintainTask.PassQty = passQty;
            maintainTask.NgQty = ngQty;
            maintainTask.Qty = passQty + ngQty;
            maintainTask.ApplyDate = nowDate;
            maintainTask.GenerateId();

            foreach (var maintainPrj in tasks)
            {
                var detail = new MaintainTaskDetail();
                detail.MaintainTaskId = maintainTask.Id;
                detail.MaintainProjectId = maintainPrj.MaintainProjectId;
                detail.MinValue = maintainPrj.MinValue;
                detail.MaxValue = maintainPrj.MaxValue;
                maintainTask.Details.Add(detail);
            }
            RF.Save(maintainTask);

            assetReturnFixture.MaintainTaskId = maintainTask.Id;
            assetReturnFixture.MaintainTask = maintainTask;
        }

        /// <summary>
        /// 通过资产归还单生成工治具入库单
        /// </summary>
        /// <param name="assetReturn">资产归还单</param>
        /// <param name="assetReturnFixtureList">归还单工治具集合</param>
        public virtual void CreateInboundOrderByAssetReturn(AssetReturn assetReturn, IList<AssetReturnFixture> assetReturnFixtureList)
        {
            var _commonController = RT.Service.Resolve<CommonController>();
            var inboundOrderList = new EntityList<InboundOrder>();

            //ID类工治具和不保养的编码类工治具都需要按照质量状态生成入库单
            inboundOrderList.AddRange(assetReturnFixtureList.Where(p=>p.Sn.IsNotEmpty() || p.MaintainTaskId == null).GroupBy(p => new { p.FixtureEncodeId, p.QualityStatus })
                .Select(p => new InboundOrder
                {
                    No = _commonController.GetNo<InboundOrder>("入库单号".L10N()),
                    FixtureEncodeId = p.First().FixtureEncodeId,
                    QualityState = p.First().QualityStatus,
                    Qty = p.Sum(s => s.Qty),
                    InboundType = FixtureInboundType.Return,
                    InboundStatus = InboundStatus.ToBe,
                    CustomerId = assetReturn.AssetRequisition.CustomerId,
                    SupplierId = assetReturn.AssetRequisition.SupplierId,
                    WarehouseId = assetReturn.WarehouseId,
                    Proprietorship = Proprietorship.Own
                }).ToList());

            //需要保养的编码类工治具按工治具编码生成入库单
            inboundOrderList.AddRange(assetReturnFixtureList.Where(p => p.Sn.IsNullOrEmpty() && p.MaintainTaskId != null).GroupBy(p => new { p.FixtureEncodeId })
                .Select(p => new InboundOrder
                {
                    No = _commonController.GetNo<InboundOrder>("入库单号".L10N()),
                    FixtureEncodeId = p.First().FixtureEncodeId,
                    QualityState = null,
                    Qty = p.Sum(s => s.Qty),
                    InboundType = FixtureInboundType.Return,
                    InboundStatus = InboundStatus.ToBe,
                    CustomerId = assetReturn.AssetRequisition.CustomerId,
                    SupplierId = assetReturn.AssetRequisition.SupplierId,
                    WarehouseId = assetReturn.WarehouseId,
                    Proprietorship = Proprietorship.Own
                }).ToList());

            inboundOrderList.ForEach(p =>
            {
                p.GenerateId();
            });

            foreach (var inboundOrder in inboundOrderList)
            {
                var detailList = assetReturnFixtureList.Where(p => p.AssetReturnId == assetReturn.Id && p.FixtureEncodeId == inboundOrder.FixtureEncodeId).ToList();

                if (inboundOrder.QualityState != null)
                {
                    detailList = detailList.Where(p => p.QualityStatus == inboundOrder.QualityState).ToList();
                }
                else 
                {
                    var newDetail = new AssetReturnFixture();
                    newDetail.ManageMode = detailList[0].ManageMode;
                    newDetail.Qty = detailList.Sum(p => p.Qty);
                    newDetail.MaintainTaskId = detailList[0].MaintainTaskId;
                    newDetail.MaintainTask = detailList[0].MaintainTask;
                    detailList.Clear();
                    detailList.Add(newDetail);
                }

                foreach (var detail in detailList)
                {
                    if (detail.ManageMode == ManageMode.Code)
                    {
                        var inboundOrderFixtureCode = new InboundOrderFixtureCodeAccount();
                        inboundOrderFixtureCode.InboundOrderId = inboundOrder.Id;
                        inboundOrderFixtureCode.Qty = detail.Qty;
                        inboundOrder.InboundOrderFixtureCodeAccountList.Add(inboundOrderFixtureCode);
                        inboundOrder.MaintainTaskId = detail.MaintainTaskId;
                        inboundOrder.MaintainTask = detail.MaintainTask;
                    }
                    else
                    {
                        var inboundOrderFixtureId = new InboundOrderFixtureIdAccount();
                        inboundOrderFixtureId.InboundOrderId = inboundOrder.Id;
                        inboundOrderFixtureId.Qty = 1;
                        inboundOrderFixtureId.FixtureIDAccountId = (double)detail.FixtureAccountId;
                        inboundOrderFixtureId.MaintainTaskId = detail.MaintainTaskId;
                        inboundOrderFixtureId.MaintainTask = detail.MaintainTask;
                        inboundOrder.InboundOrderFixtureIdAccountList.Add(inboundOrderFixtureId);
                    }
                }
            }
            RF.Save(inboundOrderList);
        }
    }
}
