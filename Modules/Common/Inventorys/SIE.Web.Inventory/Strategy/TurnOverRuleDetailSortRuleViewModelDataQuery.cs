using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Web.Data;

namespace SIE.Web.Inventory.Strategy
{
    public class TurnOverRuleDetailSortRuleViewModelDataQuery : DataQueryer
    {
        public virtual string SaveDatas(TurnOverRuleDetail detail, EntityList<TurnOverRuleDetailSortRuleViewModel> list)
        {
            return RT.Service.Resolve<TurnOverRuleDetailSortRuleViewModelController>().SaveDatas(detail, list);
        }
    }
}
