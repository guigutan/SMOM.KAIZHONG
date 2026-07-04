using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.InboundOrders.Commands
{
    /// <summary>
    /// 一键设置库位
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.InboundOrders.Commands.OneKeyPassCommand")]
    public class OneKeyPassCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new System.NotImplementedException();
        }
    }
}
