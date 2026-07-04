using SIE.Domain;
using SIE.Kit.MES.CallMaterials;
using SIE.MES.WorkOrders;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.MES.CallMaterials.Commands
{
    /// <summary>
    /// 保存排序规则
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.CallMaterials.Commands.SaveSolSettingCommand")]
    public class SaveSolSettingCommand : SaveCommand
    {
        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void DoSave(EntityList data)
        {
            var solSettings = data as EntityList<SortSolutionsSetting>;
            Dictionary<string, string> SettingConditions = new Dictionary<string, string>();
            SettingConditions[RF.Find<WorkOrder>().EntityMeta.Property(WorkOrder.ActuStartDateProperty).Label] = WorkOrder.ActuStartDateProperty.Name;
            SettingConditions[RF.Find<WorkOrder>().EntityMeta.Property(WorkOrder.PlanBeginDateProperty).Label] = WorkOrder.PlanBeginDateProperty.Name;
            SettingConditions[RF.Find<WorkOrder>().EntityMeta.Property(WorkOrder.PlanQtyProperty).Label] = WorkOrder.PlanQtyProperty.Name;

            foreach (var solSetting in solSettings)
            {
                if (!string.IsNullOrWhiteSpace(solSetting.PriorityVMList))
                {
                    var prioritys = solSetting.PriorityList;
                    var dicPrioritys = prioritys.ToDictionary(p => p.Condition);
                    var priorityInfoList = solSetting.PriorityVMList.ToJsonObject<List<PriorityInfo>>();
                    var sortNameList = priorityInfoList.Select(p => p.SortName).Distinct();

                    var removePriorityList = prioritys.Where(p => !sortNameList.Contains(p.Condition));
                    if (removePriorityList.Any())
                    {
                        removePriorityList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

                    } 
                    foreach (var priorityInfo in priorityInfoList)
                    {
                        PrioritySetting prioritySet = null;
                        if (dicPrioritys.TryGetValue(priorityInfo.SortName, out prioritySet))
                        {
                            prioritySet.Priority = priorityInfo.Priority;
                            prioritySet.PersistenceStatus = PersistenceStatus.Modified;
                            prioritySet.SortMode = priorityInfo.SortMode == "升序" ? SortMode.Asc : SortMode.Desc;
                        }
                        else
                        {
                            prioritySet = new PrioritySetting()
                            {
                                Priority = priorityInfo.Priority,
                                Condition = priorityInfo.SortName,
                                PersistenceStatus = PersistenceStatus.New
                            };

                            prioritySet.SortMode = priorityInfo.SortMode == "升序" ? SortMode.Asc : SortMode.Desc;

                            string sortPropertyName = string.Empty;
                            if (SettingConditions.TryGetValue(priorityInfo.SortName, out sortPropertyName))
                                prioritySet.SortPropertyName = sortPropertyName;

                            solSetting.PriorityList.Add(prioritySet);
                        }
                    }
                }
                else
                {
                    solSetting.PriorityList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                }
            }

            base.DoSave(data);
        }
    }
}
