using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Core.WorkOrders;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Equipments.Abnormal
{
    /// <summary>
    /// 异常停线控制器
    /// </summary>
    public partial class AbnormalCauseController : DomainController
    {
        /// <summary>
        /// 获取异常停线列表
        /// </summary>
        /// <param name="criteria">异常停线查询实体</param>
        /// <returns>异常停线信息</returns>
        public virtual EntityList<AbnormalCause> GetAbnormalCauses(AbnormalCauseCriteria criteria)
        {
            var q = Query<AbnormalCause>();
            if (criteria.Shop != null)
                q.Where(p => p.ShopId == criteria.ShopId);
            if (criteria.Resource != null)
                q.Where(p => p.ResourceId == criteria.ResourceId);
            if (criteria.WorkOrder != null)
                q.Where(p => p.WorkOrderId == criteria.WorkOrderId);
            if (criteria.Product != null)
                q.Where(p => p.ProductId == criteria.ProductId);
            if (criteria.AbnormalType.IsNotEmpty())
                q.Where(p => p.AbnormalType == criteria.AbnormalType);
            if (criteria.SourceType != null)
                q.Where(p => p.SourceType == criteria.SourceType);
            if (criteria.ExceptionStopType != null)
                q.Where(p => p.ExceptionStopType == criteria.ExceptionStopType);
            if (criteria.EquipAccountId != null)
                q.Where(p => p.EquipAccountId == criteria.EquipAccountId);
            if (criteria.AlertManageId != null)
                q.Where(p => p.AlerterManageId == criteria.AlertManageId);
            if (criteria.AlerterId != null)
                q.Where(p => p.AlerterId == criteria.AlerterId);
            if (criteria.BeginDate.BeginValue.HasValue)
                q.Where(p => p.BeginDate >= criteria.BeginDate.BeginValue);
            if (criteria.BeginDate.EndValue.HasValue)
                q.Where(p => p.BeginDate <= criteria.BeginDate.EndValue);
            if (criteria.ProcessId.HasValue)
                q.Where(p => p.ProcessId == criteria.ProcessId);
            if (criteria.ProcessSegmentId.HasValue)
                q.Where(p => p.ProcessSegmentId == criteria.ProcessSegmentId);
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取异常停线集合
        /// </summary>
        /// <param name="exceptionStopType">异常停线类别</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="itemId">产品Id</param>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>异常停线集合</returns>
        public virtual EntityList<AbnormalCause> GetAbnormalCauses(ExceptionStopType? exceptionStopType = null,
            double? resourceId = null, double? itemId = null, double? workOrderId = null)
        {
            var querys = Query<AbnormalCause>();
            if (exceptionStopType != null)
                querys.Where(x => x.ExceptionStopType == exceptionStopType);
            if (resourceId != null)
                querys.Where(x => x.ResourceId == resourceId);
            if (itemId != null)
                querys.Where(x => x.ProductId == itemId);
            if (workOrderId != null)
                querys.Where(x => x.WorkOrderId == workOrderId);
            return querys.ToList();
        }

        /// <summary>
        /// 根据产线查异常停线
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns>异常停线列表</returns>
        public virtual EntityList<AbnormalCause> GetAbnormalCauseList(double? resourceId)
        {
            return Query<AbnormalCause>().Where(p => p.ResourceId == resourceId && p.BeginDate <= DateTime.Now && p.EndDate == null).ToList();
        }

        /// <summary>
        /// 获取同一产线/设备，重叠停线时间的数量
        /// </summary>
        /// <param name="cause">异常停线</param>
        /// <returns>异常停线时间列表</returns>
        public virtual EntityList<AbnormalCause> GetAbnormalCauseDate(AbnormalCause cause)
        {
            var query = Query<AbnormalCause>().Where(p => p.Id != cause.Id);
            query.WhereIf(cause.ResourceId.HasValue, p => p.ResourceId == cause.ResourceId);
            query.WhereIf(cause.EquipAccountId.HasValue, p => p.EquipAccountId == cause.EquipAccountId);
            return query.ToList();
        }

        /// <summary>
        /// 根据引用判断是否可以删除企业模型和设备模型
        /// </summary>
        /// <param name="id">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>true,false</returns>
        public virtual bool IsHasUsedResourse(double id, SyncSourceType sourceType)
        {
            //根据企业模型或设备中获取资源ID
            //判断该资源ID是否有被工单使用
            var res = AppRuntime.Service.Resolve<WipResourceController>().GetWipResource(id, sourceType);
            if (res == null) return true;
            return Query<AbnormalCause>().Where(p => p.ResourceId == res.Id).FirstOrDefault() == null;
        }

        /// <summary>
        /// 获取所有产线的异常停线信息
        /// </summary>
        /// <returns>异常停线集合</returns>
        /// <param name="dateTime">日期</param>
        public virtual EntityList<AbnormalCause> GetAllLineAbnormalCause(DateTime dateTime)
        { ////DateTime.Now
            var qrys = Query<AbnormalCause>().Where(p => p.BeginDate <= dateTime && (p.EndDate == null || p.EndDate >= dateTime)).ToList();
            return qrys;
        }

        /// <summary>
        /// 保存预警推送的停线管理
        /// </summary>
        /// <param name="abnormalCauses"></param>
        internal void SaveAlertAbnormalCauses(EntityList<AbnormalCause> abnormalCauses)
        {
            if (abnormalCauses.IsNotEmpty())
            {
                foreach (var abnormal in abnormalCauses)
                {
                    if (abnormal.Code.IsNullOrEmpty())
                        abnormal.Code = GetNewAbnormalCode();
                }

                RepositoryFactory.Save(abnormalCauses);
            }
        }

        /// <summary>
        /// 判断异常停线是否引用指定的生产资源
        /// </summary>
        /// <param name="wipResourceId">生产资源Id</param>
        /// <returns>bool: false--工单未引用生产资源；true--工单已引用生产资源</returns>
        public virtual bool AbnormalCauseHasUsedWipResource(double wipResourceId)
        {
            var abnormalCause = Query<AbnormalCause>().Where(x => x.ResourceId == wipResourceId).FirstOrDefault();
            if (abnormalCause == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 生成新的停线管理编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetNewAbnormalCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(AbnormalCause));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到停线管理编码生成规则,请检查规则配置".L10N());

            return AppRuntime.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 恢复停线(手工)
        /// </summary>
        /// <param name="abnormalId">停线管理ID</param>
        /// <param name="reason">原因</param>
        public virtual void RestoreAbnormalCauseManual(double abnormalId, string reason)
        {
            if (reason.IsNullOrEmpty())
                throw new ValidationException("恢复原因不能为空。".L10N());
            var abnormal = RF.GetById<AbnormalCause>(abnormalId);
            Check.AssertNotNull(abnormal, "停线管理信息不存在.".L10N());
            if (abnormal.ExceptionStopType == ExceptionStopType.Normal)
                throw new ValidationException("停线管理信息已是正常，不需恢复。".L10N());
            abnormal.ExceptionStopType = ExceptionStopType.Normal;
            abnormal.EndDate = DateTime.Now;
            abnormal.StateDescription = "手工恢复".L10N();
            abnormal.RestoreReason = reason;
            abnormal.RestorerId = AppRuntime.IdentityId;
            RepositoryFactory.Save(abnormal);
        }

        /// <summary>
        /// 恢复停线(自动)
        /// </summary>
        /// <param name="lineIdList">产线ID集合</param>
        /// <param name="equipIdList">设备ID集合</param>
        public virtual void RestoreAbnormalCauseAuto(List<double> lineIdList, List<double> equipIdList)
        {
            List<double?> lineIds = null, equipIds = null;
            if (lineIdList.IsNotEmpty())
                lineIds = lineIdList.ConvertAll(p => (double?)p).ToList();
            if (equipIdList.IsNotEmpty())
                equipIds = equipIdList.ConvertAll(p => (double?)p).ToList();
            var q = Query<AbnormalCause>().Where(p => p.ExceptionStopType == ExceptionStopType.StopLine);
            if (lineIds.IsNotEmpty() && equipIds.IsNotEmpty())
                q.Where(p => lineIds.Contains(p.ResourceId) || equipIds.Contains(p.EquipAccountId));
            else if (lineIds.IsNotEmpty())
                q.Where(p => lineIds.Contains(p.ResourceId));
            else if (equipIds.IsNotEmpty())
                q.Where(p => equipIds.Contains(p.EquipAccountId));
            else
                return;
            var abnormals = q.ToList();
            if (abnormals.IsNotEmpty())
            {
                foreach (var abnormal in abnormals)
                {
                    Check.AssertNotNull(abnormal, "停线管理信息不存在.".L10N());
                    if (abnormal.ExceptionStopType == ExceptionStopType.Normal)
                        throw new ValidationException("停线管理信息已是正常，不需恢复。".L10N());
                    abnormal.ExceptionStopType = ExceptionStopType.Normal;
                    abnormal.EndDate = DateTime.Now;
                    abnormal.StateDescription = "自动恢复".L10N();
                }
                RepositoryFactory.Save(abnormals);
            }
        }

        /// <summary>
        /// 通过资源获取工单
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">下拉查询条件</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns>根据产品获取未完成和非关闭的工单</returns>
        public virtual EntityList<SIE.Equipments.WorkOrders.WorkOrder> GetWorkOrders(PagingInfo pagingInfo, string keyword, double resourceId)
        {
            var query = Query<SIE.Equipments.WorkOrders.WorkOrder>();
            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.No.Contains(keyword));
            query.Where(p => p.ResourceId == resourceId && p.State != WorkOrderState.Close && p.State != WorkOrderState.Finish);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 不合格审核生成停线单
        /// </summary>
        /// <param name="bill"></param>
        public virtual void CreateAbnormalCauseFromFailedAudit(AbnormalCause bill)
        {
            bill.GenerateId();
            bill.Code = this.GetNewAbnormalCode();

            RF.Save(bill);
        }
    }
}