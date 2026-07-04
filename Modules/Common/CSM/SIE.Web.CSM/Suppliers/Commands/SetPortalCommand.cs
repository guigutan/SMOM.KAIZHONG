using SIE.CSM.Suppliers;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.CSM.Suppliers.Commands
{
    /// <summary>
    /// 设置为门户
    /// </summary>
    [JsCommand("SIE.Web.CSM.Suppliers.Commands.EnablePortalCommand")]
    public class EnablePortalCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var controller = RT.Service.Resolve<SupplierController>();
            if (args == null || null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            var result = controller.EnableSupplierPortal(args.SelectedIds.ToList());

            return result;
        }
    }

    /// <summary>
    /// 禁用门户
    /// </summary>
    [JsCommand("SIE.Web.CSM.Suppliers.Commands.DisablePortalCommand")]
    public class DisablePortalCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var controller = RT.Service.Resolve<SupplierController>();
            if (args == null || null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            controller.DisableSupplierPortal(args.SelectedIds.ToList());

            return true;
        }
    }
}
