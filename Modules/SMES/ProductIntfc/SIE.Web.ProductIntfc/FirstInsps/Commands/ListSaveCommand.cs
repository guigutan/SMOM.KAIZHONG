using SIE.Domain;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspLogs;
using SIE.Web.Command;

namespace SIE.Web.ProductIntfc.FirstInsps.Commands
{
    /// <summary>
    /// 保存首件规则
    /// </summary>
    [JsCommand("SIE.Web.ProductIntfc.FirstInsps.Commands.ListSaveCommand")]
    public class ListSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存首件规则
        /// </summary>
        /// <param name="data">被修改的首件规则列表</param>
        protected override void DoSave(EntityList data)
        {
            var firstInspRules = data as EntityList<FirstInspRule>;
            EntityList<FirstInspRule> modifyFirstInspRules = new EntityList<FirstInspRule>();
            foreach (var firstInspRule in firstInspRules)
            {
                if (firstInspRule.Id == 0)
                    firstInspRule.PersistenceStatus = PersistenceStatus.New;
                else
                    firstInspRule.PersistenceStatus = PersistenceStatus.Modified;
                modifyFirstInspRules.Add(firstInspRule);
            }

            RT.Service.Resolve<InspLogController>().SaveFirstInspRules(modifyFirstInspRules);
        }
    }
}
