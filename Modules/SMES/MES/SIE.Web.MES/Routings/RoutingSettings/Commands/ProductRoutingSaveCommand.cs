using SIE.Domain;
using SIE.MES.RoutingSettings;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Routings.RoutingSettings.Commands
{
    /// <summary>
    /// 产品工艺路线设置保存命令
    /// </summary>
    public class ProductRoutingSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            var list = data as EntityList<ProductRouting>;
            RT.Service.Resolve<RoutingSettingController>().SavingProductRouting(list);
        }
    }
}
