using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Equipments.EquipAccounts.Commands
{
    /// <summary>
    /// 设置LOGO
    /// </summary>
    [JsCommand("SIE.Web.Equipments.EquipAccounts.Commands.SetEquipLogoCommand")]
    public class SetEquipLogoCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
                throw new ArgumentNullException("请选择一条记录操作".L10N());
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<EquipAccountController>().SetEquipLogo(selectedIds.FirstOrDefault());
            return true;
        }
    }

}
