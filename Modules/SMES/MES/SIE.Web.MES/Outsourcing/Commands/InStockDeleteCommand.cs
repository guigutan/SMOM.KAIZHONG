using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 删除在制品委外出库
    /// </summary>
    public class InStockDeleteCommand : DeleteCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            RT.Service.Resolve<OutsourcingController>().DeleteInStock(args.SelectedIds);

            return true;
        }
    }
}
