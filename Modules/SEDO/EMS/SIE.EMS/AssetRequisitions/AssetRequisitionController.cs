using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Controller;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.AssetRequisitions
{
    /// <summary>
    /// 资产领用单控制器
    /// </summary>
    public class AssetRequisitionController : DomainController
    {
        #region 资产领用单号生成规则
        /// <summary>
        /// 获取自动生成资产领用单号No
        /// </summary>
        /// <returns>编号</returns>
        public virtual string GetNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AssetRequisition));

            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到资产领用单号生成规则,请检查规则配置".L10N());
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
            var configValue = ConfigService.GetConfig<ApprovalConfigValue>(new ApprovalConfig(), typeof(AssetRequisition));

            if (configValue == null)
                throw new ValidationException("未找到审批流配置规则,请检查规则配置".L10N());

            return configValue;
        }
        #endregion

        /// <summary>
        /// 查询资产领用单
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>资产领用列表</returns>
        public virtual EntityList<AssetRequisition> GetAssetRequisitionList(AssetRequisitionCriteria criteria)
        {
            var q = Query<AssetRequisition>();

            if (criteria.RequisitionNo.IsNotEmpty())
            {
                q.Where(p => p.RequisitionNo.Contains(criteria.RequisitionNo));
            }
            if (criteria.QureyFactoryId != null && criteria.QureyFactoryId != 0)
            {
                q.Where(p => p.FactoryId == criteria.QureyFactoryId);
            }
            if (criteria.RequisitionType != null) 
            {
                q.Where(p => p.RequisitionType == criteria.RequisitionType);
            }
            if (criteria.AssetObject != null)
            {
                q.Where(p => p.AssetObject == criteria.AssetObject);
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
            if (criteria.IssueStatus != null) 
            {
                q.Where(p => p.IssueStatus == criteria.IssueStatus);
            }
            if (criteria.EmployeeId != null && criteria.EmployeeId != 0) 
            {
                q.Where(p => p.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.IsExpired) 
            {
                q.Where(p => ((p.ReturnStatus == Enums.ReturnStatus.ToBe || p.ReturnStatus == Enums.ReturnStatus.Partial) &&
                              (p.IssueStatus == Enums.IssueStatus.Done || p.IssueStatus == Enums.IssueStatus.PartDone) && 
                               p.RequisitionType == Enums.RequisitionType.Borrow && p.RetureDate < DateTime.Now.Date));
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
        /// 获取资产领用单集合
        /// </summary>
        /// <param name="idList">领用单Id集合</param>
        /// <returns>资产领用单集合</returns>
        public virtual EntityList<AssetRequisition> GetAssetRequisitionListByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<AssetRequisition>().Where(p => ids.Contains(p.Id)).ToList();
            });

        }

        /// <summary>
        /// 查询可发放的资产领用单集合
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>资产领用单集合</returns>
        public virtual EntityList<AssetRequisition> GetAssetRequisitionsForIssue(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<AssetRequisition>().Where(p=>(p.IssueStatus == Enums.IssueStatus.ToBe || p.IssueStatus == Enums.IssueStatus.PartDone) && p.ApprovalStatus == ApprovalStatus.Audited);

            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.RequisitionNo.Contains(keyword));
            }
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询可归还的资产领用单集合
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>资产领用单集合</returns>
        public virtual EntityList<AssetRequisition> GetAssetRequisitionsForReturn(double factoryId,PagingInfo pagingInfo, string keyword)
        {
            var q = Query<AssetRequisition>().Where(p => (p.IssueStatus == Enums.IssueStatus.Done || p.IssueStatus == Enums.IssueStatus.PartDone) &&
                                                         (p.ReturnStatus == Enums.ReturnStatus.ToBe || p.ReturnStatus == Enums.ReturnStatus.Partial) &&
                                                          p.FactoryId == factoryId && p.ApprovalStatus == ApprovalStatus.Audited);

            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.RequisitionNo.Contains(keyword));
            }
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 保存和提交资产领用单
        /// </summary>
        /// <param name="assetRequisition">领用单</param>
        public virtual void SaveAndSumbitAssetRequisitions(AssetRequisition assetRequisition)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                var nowDate = RF.Find<AssetRequisition>().GetDbTime();
                var configValue = GetApprovalFlowConfigValue();

                var recordIds = new List<double>() { assetRequisition.Id };
                assetRequisition.ApprovalStatus = configValue.EnableAudit ? ApprovalStatus.PendingReview : ApprovalStatus.Audited;

                RF.Save(assetRequisition);
                
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(AssetRequisition).FullName, ApprovalResult.Submit, nowDate, "");
                
                trans.Complete();
            }
        }

        /// <summary>
        /// 提交资产领用单
        /// </summary>
        /// <param name="selectedIds">领用单Id集合</param>
        public virtual void SumbitAssetRequisitions(List<double> selectedIds)
        {
            var configValue = GetApprovalFlowConfigValue();
            var assetRequisitions = GetAssetRequisitionListByIds(selectedIds);
            if (assetRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var config = RT.Service.Resolve<EmsApprovalController>().GetApprovalConfigValue(typeof(AssetRequisition));
            var recordIds = new List<double>();
            foreach (var item in assetRequisitions)
            {
                item.ApprovalStatus = ApprovalStatus.PendingReview;
                recordIds.Add(item.Id);
            }
            var nowDate = RF.Find<AssetRequisition>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(assetRequisitions);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(AssetRequisition).FullName, ApprovalResult.Submit, nowDate, "");
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ApprovalAssetRequisitionsInner(selectedIds, ApprovalResult.Pass, "通过".L10N(), assetRequisitions);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 撤回资产领用单
        /// </summary>
        /// <param name="selectedIds">领用单Id集合</param>
        public virtual void CancelAssetRequisitions(List<double> selectedIds)
        {
            var assetRequisitions = GetAssetRequisitionListByIds(selectedIds);
            if (assetRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> assetRequisitionsIds = new List<double>();
                assetRequisitions.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    assetRequisitionsIds.Add(p.Id);
                });
                RF.Save(assetRequisitions);
                var nowDate = RF.Find<AssetRequisition>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(assetRequisitionsIds, typeof(AssetRequisition).FullName, ApprovalResult.Retract, nowDate, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产领用单
        /// </summary>
        /// <param name="selectedIds">领用单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void ApprovalAssetRequisitions(List<double> selectedIds, ApprovalResult value, string remark)
        {
            if (!selectedIds.Any())
            {
                throw new ValidationException("传参异常".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovalAssetRequisitionsInner(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核资产领用单
        /// </summary>
        /// <param name="selectedIds">领用单Id集合</param>
        /// <param name="value">审核结果</param>
        /// <param name="remark">审核意见</param>
        /// <param name="assetRequisitionList">审核意见</param>
        public virtual void ApprovalAssetRequisitionsInner(List<double> selectedIds, ApprovalResult value, string remark, EntityList<AssetRequisition> assetRequisitionList = null)
        {
            if (assetRequisitionList == null)
            {
                assetRequisitionList = GetAssetRequisitionListByIds(selectedIds);
                if (!assetRequisitionList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            //验证只有待审核的数据才能审核
            if (assetRequisitionList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var nowDate = RF.Find<AssetRequisition>().GetDbTime();
            var ids = new List<double>();
            assetRequisitionList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            //保存成功之后添加审核记录
            RF.Save(assetRequisitionList);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(AssetRequisition).FullName, value, nowDate, remark);
        }

        /// <summary>
        /// 获取工治具清单
        /// </summary>
        /// <param name="assetRequisitionId">领用单Id</param>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>工治具清单列表</returns>
        public virtual EntityList<AssetRequisitionFixture> GetAssetRequisitionFixtureList(double assetRequisitionId, double warehouseId, IList<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<AssetRequisitionFixture>().Where(p => p.AssetRequisitionId == assetRequisitionId);

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var fixtureEncodeIds = list.Select(p => p.FixtureEncodeId).ToList();

            //获取有可用库存数的工治具编码列表
            var encodeList = RT.Service.Resolve<CoreFixtureController>().GetCanUseNumByWarehouseId(warehouseId, fixtureEncodeIds);

            foreach (var item in list)
            {
                var encode = encodeList.FirstOrDefault(p => p.Id == item.FixtureEncodeId);

                if (encode != null)
                {
                    item.StoreUsableQty = encode.CanUseNum.ToString();
                }
            }

            return list;
        }

        /// <summary>
        /// 主表审核状态为已审批时不可删除
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool CheckCanExecute(double parentId)
        {
            var assetRequision = RF.GetById<AssetRequisition>(parentId);
            if (assetRequision == null)
            {
                return false;
            }
            if (assetRequision.ApprovalStatus == ApprovalStatus.Audited)
            {
                return false;
            }
            return true;
        }
    }
}
