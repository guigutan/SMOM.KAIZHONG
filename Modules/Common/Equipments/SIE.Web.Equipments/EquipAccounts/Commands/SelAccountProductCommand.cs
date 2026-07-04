using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipAccounts.Commands
{
    /// <summary>
    /// 选择产品
    /// </summary>
    public class SelAccountProductCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var equipProductList = args.Data.ToJsonObject<List<EquipAccountProduct>>();
            Check.NotNullOrEmpty(equipProductList, nameof(equipProductList));
            if (equipProductList == null || equipProductList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(equipProductList)));
            }

            RT.Service.Resolve<EquipAccountController>().SaveEquipAccountProductList(equipProductList);

            return true;
        }
    }
}
