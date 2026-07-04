using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Inventory.Strategy.Commands
{
    public class TurnOverRuleDetailAddCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<TurnOverRuleDetail>();
            var turnOverRule = RF.GetById<TurnOverRule>(data.TurnOverRuleId);
            if (turnOverRule != null)
            {
                data.LineNo = turnOverRule.DetailList.Count == 0 ? 1 : turnOverRule.DetailList.Max(p =>
                {
                    return p.LineNo + 1;
                });
            }
            return data;
        }
    }
}
