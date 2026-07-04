using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.CSM.Suppliers.Commands
{
    /// <summary>
    /// 删除命令
    /// </summary>
    [JsCommand(CommandName)]
    public class SupplierItemDeleteCommand : ViewCommand
    {
        /// <summary>
        /// 删除命令
        /// </summary>
        public const string CommandName = "SIE.Web.CSM.Suppliers.Commands.SupplierItemDeleteCommand";

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null || null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }
            var selectedSupplierItems = new List<SupplierItem>();
            foreach (var item in args.SelectedIds)
            {
                var supplierItem = RF.GetById<SupplierItem>(item);
                selectedSupplierItems.Add(supplierItem);
            }
            RT.Service.Resolve<SupplierController>().DeleteSupplierItem(selectedSupplierItems);
            return true;
        }
    }
}
