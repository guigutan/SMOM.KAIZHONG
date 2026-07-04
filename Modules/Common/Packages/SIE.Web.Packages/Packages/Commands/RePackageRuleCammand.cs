using SIE.Packages.Packages;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Packages.Packages.Commands
{
    /// <summary>
    /// 启用
    /// </summary>
    public class RePackageEnableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var controller = RT.Service.Resolve<RePackageController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }
            controller.EnablePackageRules(args.SelectedIds.ToList());
            return true;
        }
    }
}
