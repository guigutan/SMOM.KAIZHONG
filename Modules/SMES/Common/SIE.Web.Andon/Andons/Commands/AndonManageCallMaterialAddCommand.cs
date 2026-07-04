using SIE.Andon.Andons;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯管理添加BOM物料命令
    /// </summary>
    public class AndonManageCallMaterialAddCommand : ViewCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var itemDetail = args.Data.ToJsonObject<AndonManageCallMaterial>();
            itemDetail = RT.Service.Resolve<AndonManageController>().AddCallMaterial(itemDetail);
            return itemDetail;
        }
    }
}
