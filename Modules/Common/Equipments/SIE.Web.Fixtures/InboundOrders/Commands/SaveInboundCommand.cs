using SIE.Domain;
using SIE.Fixtures.InboundOrders;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.InboundOrders.Commands
{
    /// <summary>
    /// 保存入库单
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.InboundOrders.Commands.SaveInboundCommand")]
    public class SaveInboundCommand : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {

            var model = entity as InboundOrder;
            RT.Service.Resolve<InboundOrderController>().SaveInboundOrder(model);
        }
    }
}
