using SIE.Domain;
using SIE.Fixtures.InboundOrders;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.InboundOrders.Commands
{

    /// <summary>
    /// 提交治工具入库
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.InboundOrders.Commands.SumbitInboundCommand")]
    public class SumbitInboundCommand : FormSaveCommand
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {

            var model = entity as InboundOrder;
            RT.Service.Resolve<InboundOrderController>().SubmitInboundOrder(model);
        }
    }
}
