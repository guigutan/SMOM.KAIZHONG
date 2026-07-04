using SIE.LES.MaterialPreparations;
using SIE.MES.WorkOrders.Models;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 一键备料命令
    /// </summary>
    public class OneKeyToPrepareCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<OneKeyInfo>();
            var type = data.Type;
            var woDic = data.WoList.ToDictionary(p => p.Id, p => p.Code);
            RT.Service.Resolve<MaterialPreparationController>().ValidateOneKeyWoInfo(woDic.Keys.ToList(), type);
            if (type == 10) // 创建发运订单
            {
                RT.Service.Resolve<MaterialPreparationController>().OneKeyCreateShippingOrder(woDic);
            }
            else // 创建备料单
            {
                RT.Service.Resolve<MaterialPreparationController>().OneKeyCreateMpOrders(woDic);
            }
            return true;
        }
    }
}
