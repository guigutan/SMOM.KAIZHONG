using SIE.LES.MaterialPreparations;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations.Commands
{
    /// <summary>
    /// 添加车间领料
    /// </summary>
    public class AddWorkShopPrepareCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var pre = args.Data.ToJsonObject<MaterialPreparation>();
            pre.No = RT.Service.Resolve<MaterialPreparationController>().GetMaterialPreprationNo().FirstOrDefault();
            pre.PrepareType = SIE.LES.MaterialPreparations.Enums.PrepareType.Sfmr;
            return pre;
        }
    }
}
