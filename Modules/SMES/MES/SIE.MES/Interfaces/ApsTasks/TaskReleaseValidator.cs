using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.Items;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 工单下达验证
    /// </summary>
    public class TaskReleaseValidator
    {

        /// <summary>
        /// 计划任务关联
        /// </summary>
        private readonly EntityList<TaskUnion> taskUnions;

        /// <summary>
        /// 生产资源列表
        /// </summary>
        private EntityList<WipResource> wipResources { get; }

        /// <summary>
        /// 车间列表
        /// </summary>
        public EntityList<Enterprise> WorkShops { get; }

        /// <summary>
        /// 要下达的产品（物料）清单
        /// </summary>
        public EntityList<Item> Products { get; }

        /// <summary>
        /// 计划任务关联明细
        /// </summary>
        private readonly EntityList<TaskUnionDetail> taskUnionDetails;
        private readonly DateTime curDateTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_releasePlanDatas"></param>
        public TaskReleaseValidator(IReadOnlyList<ReleasePlanData> _releasePlanDatas)
        {
            //获取计划任务关联工单
            var planTaskIds = _releasePlanDatas.Select(x => x.PlanTaskId).Distinct().ToList();
            taskUnions = RT.Service.Resolve<ApsMesTaskController>().GetTaskUnions(planTaskIds);

            var wipResourceIds = _releasePlanDatas.Select(x => x.WipResourceId).Distinct().ToList();
            wipResources = RT.Service.Resolve<WipResourceController>().GetResourceList(wipResourceIds);

            var workShopIds = _releasePlanDatas.Select(x => x.WorkShopId).Distinct().ToList();
            WorkShops = RT.Service.Resolve<EnterpriseController>().GetWorkShopByIds(workShopIds);

            var itemIds = _releasePlanDatas.SelectMany(x => x.Details).Select(x => x.ItemId).Distinct().ToList();
            Products = RT.Service.Resolve<ItemController>().GetItemList(itemIds);

            var detailIds = _releasePlanDatas.SelectMany(x => x.Details).Select(x => x.DetailId).Distinct().ToList();
            taskUnionDetails = RT.Service.Resolve<ApsMesTaskController>().GetTaskUnionDetails(detailIds);

            curDateTime = RF.Find<TaskUnion>().GetDbTime();
        }

        /// <summary>
        /// 验证下达计划数据--验证失败时创建失败的下达结果
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanResult">下达结果</param>
        public void ValidataReleasePlanData(ReleasePlanData curReleasePlanData, ReleasePlanResult curReleasePlanResult)
        {
            if (curReleasePlanData == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanData));
            }

            ValidataReleasePlanDataMain(curReleasePlanData, curReleasePlanResult);
            ValidataReleasePlanDataDetails(curReleasePlanData.Details, curReleasePlanResult);
        }

        /// <summary>
        /// 验证下达计划主数据
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanResult">下达结果</param>
        private void ValidataReleasePlanDataMain(ReleasePlanData curReleasePlanData, ReleasePlanResult curReleasePlanResult)
        {
            var cursbMsg = new StringBuilder();
            if (curReleasePlanData.PlanTaskId.IsNullOrWhiteSpace())
            {
                cursbMsg.Append("计划任务ID不能为空".L10N());
            }
            if (curReleasePlanData.PlanNo.IsNullOrWhiteSpace())
            {
                cursbMsg.Append("计划单号不能为空".L10N());
            }

            if (taskUnions.Any(x => x.PlanTaskId == curReleasePlanData.PlanTaskId) && curReleasePlanResult.Details.Count()>0)
            {
                cursbMsg.Append("计划任务ID[{0}]已存在下达信息!".L10nFormat(curReleasePlanData.PlanTaskId));
            }
            if (curReleasePlanData.WipResourceId <= 0)
            {
                cursbMsg.Append("生产资源Id不能<=0".L10N());
            }

            if (!wipResources.Any(x => x.Id == curReleasePlanData.WipResourceId))
            {
                cursbMsg.Append("生产资源ID[{0}]对应的实体不存在!".L10nFormat(curReleasePlanData.WipResourceId));
            }

            if (curReleasePlanData.WorkShopId <= 0)
            {
                cursbMsg.Append("车间ID不能<=0".L10N());
            }

            if (!WorkShops.Any(x => x.Id == curReleasePlanData.WorkShopId))
            {
                cursbMsg.Append("车间ID[{0}]对应的实体不存在!".L10nFormat(curReleasePlanData.WorkShopId));
            }
            if (curReleasePlanData.MouldId != null && curReleasePlanData.MouldId <= 0)
            {
                cursbMsg.Append("模具ID不能<=0".L10N());
            }

            if (curReleasePlanData.MouldBarId != null && curReleasePlanData.MouldBarId <= 0)
            {
                cursbMsg.Append("模具条码ID不能<=0".L10N());
            }

            if (!curReleasePlanData.Details.Any())
            {
                cursbMsg.Append("下达计划明细数据不存在!".L10N());
            }

            if (cursbMsg.Length > 0)
            {
                TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, cursbMsg.ToString());
            }
        }

        /// <summary>
        /// 验证下达计划主数据
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanResult">下达结果</param>
        private void EbsValidataReleasePlanDataMain(ReleasePlanData curReleasePlanData, ReleasePlanResult curReleasePlanResult)
        {
            var cursbMsg = new StringBuilder();
            if (curReleasePlanData.PlanTaskId.IsNullOrWhiteSpace())
            {
                cursbMsg.Append("计划任务ID不能为空".L10N());
            }
            if (curReleasePlanData.PlanNo.IsNullOrWhiteSpace())
            {
                cursbMsg.Append("计划单号不能为空".L10N());
            }

            //if (taskUnions.Any(x => x.PlanTaskId == curReleasePlanData.PlanTaskId))
            //{
            //    cursbMsg.Append("计划任务ID[{0}]已存在下达信息!".L10nFormat(curReleasePlanData.PlanTaskId));
            //}
            if (!curReleasePlanData.WithOutEnterprise)
            {
                if (curReleasePlanData.WipResourceId <= 0)
                {
                    cursbMsg.Append("生产资源Id不能<=0".L10N());
                }

                if (!wipResources.Any(x => x.Id == curReleasePlanData.WipResourceId))
                {
                    cursbMsg.Append("生产资源ID[{0}]对应的实体不存在!".L10nFormat(curReleasePlanData.WipResourceId));
                }

                if (curReleasePlanData.WorkShopId <= 0)
                {
                    cursbMsg.Append("车间ID不能<=0".L10N());
                }

                if (!WorkShops.Any(x => x.Id == curReleasePlanData.WorkShopId))
                {
                    cursbMsg.Append("车间ID[{0}]对应的实体不存在!".L10nFormat(curReleasePlanData.WorkShopId));
                }
            }
            if (curReleasePlanData.MouldId != null && curReleasePlanData.MouldId <= 0)
            {
                cursbMsg.Append("模具ID不能<=0".L10N());
            }

            if (curReleasePlanData.MouldBarId != null && curReleasePlanData.MouldBarId <= 0)
            {
                cursbMsg.Append("模具条码ID不能<=0".L10N());
            }

            if (!curReleasePlanData.Details.Any())
            {
                cursbMsg.Append("下达计划明细数据不存在!".L10N());
            }

            if (cursbMsg.Length > 0)
            {
                TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, cursbMsg.ToString());
            }
        }

        /// <summary>
        /// 验证下达计划明细数据
        /// </summary>
        /// <param name="details">下达计划明细数据集合</param>
        /// <param name="curReleasePlanResult">下达结果</param>        
        private void ValidataReleasePlanDataDetails(List<ReleasePlanDetail> details,
            ReleasePlanResult curReleasePlanResult)
        {

            foreach (var planDetail in details)
            {
                var cursbMsg = new StringBuilder();
                if (planDetail.DetailId.IsNullOrWhiteSpace())
                {
                    cursbMsg.Append("下达计划明细ID不能为空!".L10N());
                }

                if (taskUnionDetails.Any(x => x.DetailId == planDetail.DetailId))
                {
                    cursbMsg.Append("明细ID[{0}]已存在下达信息!".L10nFormat(planDetail.DetailId));
                }

                if (planDetail.CustomerId != null && planDetail.CustomerId <= 0)
                {
                    cursbMsg.Append("下达计划明细的客户ID不能<=0!".L10N());
                }

                if (planDetail.ItemId <= 0)
                {
                    cursbMsg.Append("下达计划明细的物料ID不能<=0".L10N());
                }

                if (!Products.Any(x => x.Id == planDetail.ItemId))
                {
                    cursbMsg.Append("物料ID[{0}]对应的实体不存在!".L10nFormat(planDetail.ItemId));
                }

                if (planDetail.ProcessTechOrderCode.IsNullOrWhiteSpace())
                {
                    cursbMsg.Append("工艺单编号不能为空!".L10N());
                }

                if (planDetail.ProductionOrderCode.IsNullOrWhiteSpace())
                {
                    cursbMsg.Append("生产订单编号不能为空!".L10N());
                }

                if (planDetail.PlanAmount < 1)
                {
                    cursbMsg.Append("数量[{0}]不合法!".L10nFormat(planDetail.PlanAmount));
                }

                if (planDetail.PlanStartTime < curDateTime)
                {
                    cursbMsg.Append("计划开始时间[{0}]不能小于当前时间[{1}]"
                        .L10nFormat(planDetail.PlanStartTime, curDateTime));
                }

                if (planDetail.PlanEndTime <= planDetail.PlanStartTime)
                {
                    cursbMsg.Append("计划结束时间[{0}]不能小于计划开始时间[{1}]"
                        .L10nFormat(planDetail.PlanEndTime, planDetail.PlanStartTime));
                }

                if (planDetail.Proportion <= 0)
                {
                    cursbMsg.Append("与主物料投入数量比例[{0}]不能<=0".L10nFormat(planDetail.Proportion));
                }

                if (cursbMsg.Length > 0)
                {
                    var curDetailResult = TaskReleaseHelper.CreateReleaseDetailResult(planDetail.DetailId,
                        planDetail.ProcessTechOrderCode, cursbMsg.ToString(), string.Empty);
                    curReleasePlanResult.Details.Add(curDetailResult);
                    TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, string.Empty);
                }
            }
        }

        /// <summary>
        /// 验证下达计划明细数据
        /// </summary>
        /// <param name="details">下达计划明细数据集合</param>
        /// <param name="curReleasePlanResult">下达结果</param>        
        private void EbsValidataReleasePlanDataDetails(List<ReleasePlanDetail> details,
            ReleasePlanResult curReleasePlanResult)
        {

            foreach (var planDetail in details)
            {
                var cursbMsg = new StringBuilder();
                if (planDetail.DetailId.IsNullOrWhiteSpace())
                {
                    cursbMsg.Append("下达计划明细ID不能为空!".L10N());
                }

                //if (taskUnionDetails.Any(x => x.DetailId == planDetail.DetailId))
                //{
                //    cursbMsg.Append("明细ID[{0}]已存在下达信息!".L10nFormat(planDetail.DetailId));
                //}

                if (planDetail.CustomerId != null && planDetail.CustomerId <= 0)
                {
                    cursbMsg.Append("下达计划明细的客户ID不能<=0!".L10N());
                }

                if (planDetail.ItemId <= 0)
                {
                    cursbMsg.Append("下达计划明细的物料ID不能<=0".L10N());
                }

                if (!Products.Any(x => x.Id == planDetail.ItemId))
                {
                    cursbMsg.Append("物料ID[{0}]对应的实体不存在!".L10nFormat(planDetail.ItemId));
                }

                if (planDetail.ProcessTechOrderCode.IsNullOrWhiteSpace())
                {
                    cursbMsg.Append("工艺单编号不能为空!".L10N());
                }

                if (planDetail.ProductionOrderCode.IsNullOrWhiteSpace())
                {
                    cursbMsg.Append("生产订单编号不能为空!".L10N());
                }

                if (planDetail.PlanAmount < 1)
                {
                    cursbMsg.Append("数量[{0}]不合法!".L10nFormat(planDetail.PlanAmount));
                }

                //if (planDetail.PlanStartTime < curDateTime)
                //{
                //    cursbMsg.Append("计划开始时间[{0}]不能小于当前时间[{1}]"
                //        .L10nFormat(planDetail.PlanStartTime, curDateTime));
                //}

                if (planDetail.PlanEndTime <= planDetail.PlanStartTime)
                {
                    cursbMsg.Append("计划结束时间[{0}]不能小于计划开始时间[{1}]"
                        .L10nFormat(planDetail.PlanEndTime, planDetail.PlanStartTime));
                }

                if (planDetail.Proportion <= 0)
                {
                    cursbMsg.Append("与主物料投入数量比例[{0}]不能<=0".L10nFormat(planDetail.Proportion));
                }

                if (cursbMsg.Length > 0)
                {
                    var curDetailResult = TaskReleaseHelper.CreateReleaseDetailResult(planDetail.DetailId,
                        planDetail.ProcessTechOrderCode, cursbMsg.ToString(), string.Empty);
                    curReleasePlanResult.Details.Add(curDetailResult);
                    TaskReleaseHelper.SetReleasePlanMainResult(curReleasePlanResult, false, string.Empty);
                }
            }
        }

        /// <summary>
        /// 获取生产资源
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public WipResource GetWipResource(double resourceId)
        {
            return wipResources.FirstOrDefault(x => x.Id == resourceId);
        }

        /// <summary>
        /// 验证下达计划数据--验证失败时创建失败的下达结果
        /// </summary>
        /// <param name="curReleasePlanData">下达计划数据</param>
        /// <param name="curReleasePlanResult">下达结果</param>
        public void EbsValidataReleasePlanData(ReleasePlanData curReleasePlanData, ReleasePlanResult curReleasePlanResult)
        {
            if (curReleasePlanData == null)
            {
                throw new ArgumentNullException(nameof(curReleasePlanData));
            }

            EbsValidataReleasePlanDataMain(curReleasePlanData, curReleasePlanResult);
            EbsValidataReleasePlanDataDetails(curReleasePlanData.Details, curReleasePlanResult);
        }
    }
}
