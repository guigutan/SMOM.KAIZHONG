using SIE.Domain;
using SIE.Kit.MES.CallMaterials;
using SIE.MES.WorkOrders;
using SIE.Resources.WipResources;
using SIE.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.MES.CallMaterials.DataQuery
{
    /// <summary>
    /// 叫料单管理查询器
    /// </summary>
    public class CallMaterialWODataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取叫料工单的物料占比
        /// </summary>
        /// <param name="callWorkOrderId">叫料工单</param>
        /// <returns>叫料工单的物料占比</returns>
        public double? GetMatchRate(CallMaterialWorkOrder callWo)
        {
            return RT.Service.Resolve<CallMaterialController>().GetMatchRate(callWo);
        }

        /// <summary>
        /// 获取自动手动设置参数
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>自动手动设置参数</returns>
        public CallMaterialSetting GetCallMaterialSetting(double resourceId)
        {
            return RT.Service.Resolve<CallMaterialController>().GetIsAutoCall(resourceId);
        }

        /// <summary>
        /// 获取工单匹配物料列表
        /// </summary>
        /// <param name="callMatchWO">工单匹配</param>
        /// <returns>工单匹配物料列表</returns>
        public CallMatchItem GetCallMatchItem(CallMatchWorkOrder callMatchWO)
        {
            return RT.Service.Resolve<CallMaterialController>().GetCallMatchItem(callMatchWO.CallWorkOrderId, callMatchWO.WorkOrderId, callMatchWO.ItemId);
        }

        /// <summary>
        /// 获取排序方案设置
        /// </summary>
        /// <returns>排序方案设置列表</returns>
        public EntityList<SortSolutionsSetting> GetSortSolutionsSettings()
        {
            return RT.Service.Resolve<CallMaterialController>().GetSortSolutionsSettings();
        }

        /// <summary>
        /// 获取排序优先级设置信息
        /// </summary>
        /// <param name="solutionId">排序解决方案Id</param>
        /// <returns>排序优先级设置信息</returns>
        public PrioritySetInfo GetPrioritySetInfo(double solutionId, List<double> solSettingIds)
        {
            PrioritySetInfo prioritySetInfo = new PrioritySetInfo();
            Dictionary<string, string> SettingConditions = new Dictionary<string, string>();
            List<PrioritySetting> prioritySetsOfSortSol;
            SettingConditions[WorkOrder.ActuStartDateProperty.Name] = RF.Find<WorkOrder>().EntityMeta.Property(WorkOrder.ActuStartDateProperty).Label;
            SettingConditions[WorkOrder.PlanBeginDateProperty.Name] = RF.Find<WorkOrder>().EntityMeta.Property(WorkOrder.PlanBeginDateProperty).Label;
            SettingConditions[WorkOrder.PlanQtyProperty.Name] = RF.Find<WorkOrder>().EntityMeta.Property(WorkOrder.PlanQtyProperty).Label;

            var prioritySettings = RT.Service.Resolve<CallMaterialController>().GetPrioritySettings(solSettingIds);
            var groupPrioritySets = prioritySettings.GroupBy(p => p.SolutionsId).ToDictionary(p => p.Key, p => p.ToList());
            if (groupPrioritySets.TryGetValue(solutionId, out prioritySetsOfSortSol))
            {
                prioritySetsOfSortSol.ForEach(p =>
                {
                    var selectedPriorityInfo = new PriorityInfo
                    {
                        SolutionSettingId = solutionId,
                        SortName = p.Condition,
                        SortMode = "降序",
                        Priority = p.Priority
                    };

                    if (p.SortMode == SortMode.Asc)
                        selectedPriorityInfo.SortMode = "升序";

                    prioritySetInfo.SelectedPriortityInfos.Add(selectedPriorityInfo);
                });
            }

            if (prioritySetsOfSortSol == null)
                prioritySetsOfSortSol = new List<PrioritySetting>();
            var selectedCons = prioritySetsOfSortSol.Select(p => p.SortPropertyName);
            SettingConditions.Where(p => !selectedCons.Contains(p.Key)).ForEach(p =>
            {
                var priorityInfo = new PriorityInfo
                {
                    SolutionSettingId = solutionId,
                    SortName = p.Value,
                    SortMode = "降序"
                };

                prioritySetInfo.SelectPriorityInfos.Add(priorityInfo);
            });

            prioritySetInfo.SolSettingInfos.AddRange(GetSolSettingInfos(groupPrioritySets));

            return prioritySetInfo;
        }

        /// <summary>
        /// 获取查询面板默认选中的生产资源
        /// </summary>
        /// <returns>生产资源</returns>
        public WipResource GetDefaultResource()
        {
            return RT.Service.Resolve<CallMaterialController>().GetDefaultResource();
        }

        /// <summary>
        /// 获取排序设置信息列表
        /// </summary>
        /// <param name="groupPrioritySets">排序优先级设置字典</param>
        /// <returns>排序设置信息列表</returns>
        public List<SolSettingInfo> GetSolSettingInfos(Dictionary<double, List<PrioritySetting>> groupPrioritySets)
        {
            List<SolSettingInfo> solSettingInfos = new List<SolSettingInfo>();
            foreach (var groupPrioritySet in groupPrioritySets)
            {
                var solSettingInfo = new SolSettingInfo()
                {
                    SolutionSettingId = groupPrioritySet.Key
                };

                foreach (var prioritySetting in groupPrioritySet.Value)
                {
                    var priorityInfo = new PriorityInfo()
                    {
                        SolutionSettingId = groupPrioritySet.Key,
                        SortName = prioritySetting.Condition,
                        SortMode = "降序",
                        Priority = prioritySetting.Priority
                    };

                    if (prioritySetting.SortMode == SortMode.Asc)
                        priorityInfo.SortMode = "升序";
                    solSettingInfo.SelectedPriortityInfos.Add(priorityInfo);
                }

                solSettingInfos.Add(solSettingInfo);
            }

            return solSettingInfos;
        }
    }
}
