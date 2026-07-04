using SIE.Domain;
using SIE.EventMessages.MES.WIP;
using SIE.Security;
using SIE.Tech.Processs.Scripts;
using SIE.Web.Data;

namespace SIE.Web.Tech.DataQuery
{
    [AllowAnonymous]
    public class ProcessConditionDataQuery : DataQueryer
    {

        public object GetConditionItems()
        {
            return RT.Service.Resolve<IProcessConditionService>().GetProcessConditionItems();
        }

        public object GetScriptConditions()
        {
            var items = RF.GetAll<ScriptCondition>();
            return items;
        }
    }
}
