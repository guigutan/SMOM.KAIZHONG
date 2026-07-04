using NPOI.HSSF.Record;
using SIE.RedCardManagment.RedCards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.RedCardManagment.RedCards.Commands
{
    /// <summary>
    /// 红牌管理启用命令
    /// </summary>
    internal class EnableRedCardCommand : ViewCommand<ViewArgs>
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            RedCard redCard = args.Data.ToJsonObject<RedCard>();
            return RT.Service.Resolve<RedCardService>().AlterRedCardStatus(redCard.Id, RedCardState.Enable);
        }
    }
}
