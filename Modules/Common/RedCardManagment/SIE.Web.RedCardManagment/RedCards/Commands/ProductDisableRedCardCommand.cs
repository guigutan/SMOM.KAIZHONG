using SIE.RedCardManagment.RedCards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SIE.Web.RedCardManagment.RedCards.Commands
{
    /// <summary>
    /// 关联产品禁用命令
    /// </summary>
    internal class ProductDisableRedCardCommand : ViewCommand<ViewArgs>
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            string dataString = args.Data;
            double[] ids = JsonSerializer.Deserialize<double[]>(dataString);
            return RT.Service.Resolve<RedCardService>().SetProductRedCardStatus(ids, RedCardState.Disable);
        }
    }
}
