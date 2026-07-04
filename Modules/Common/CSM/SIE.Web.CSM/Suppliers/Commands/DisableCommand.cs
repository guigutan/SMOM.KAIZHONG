using SIE.CSM.Suppliers;
using SIE.Web.Command;
using System;

namespace SIE.Web.CSM.Suppliers.Commands
{
    /// <summary>
    /// 禁用
    /// </summary>
    public class DisableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var controller = RT.Service.Resolve<SupplierController>();
            if (args == null || null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }
            foreach (var item in args.SelectedIds)
            {
                controller.DisableSupplier(item);
            }
            return true;
        }
    }
}
