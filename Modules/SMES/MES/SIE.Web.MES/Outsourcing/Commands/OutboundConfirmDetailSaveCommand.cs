using SIE.Domain;
using SIE.EventMessages.ErpCommon;
using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 发货确认保存
    /// </summary>
    public class OutboundConfirmDetailSaveCommand : SaveCommand
    {
        protected override void OnSaved(EntityList data)
        {
            base.OnSaved(data);
            //更新事务上传数量
            foreach (OutboundConfirmDetail entity in data)
            {
                RT.Service.Resolve<IUploadLogControllercs>().UpdateOutboundConfirmTransaction(entity.Zuid, entity.Qty);
            }
        }
    }
}
