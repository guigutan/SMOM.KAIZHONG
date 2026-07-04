using SIE.LES.RetreatItemManage;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands
{
    /// <summary>
    ///提交命令
    /// </summary>
    public class SubmitReturnCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }
            List<double> ids = new List<double>();
            foreach (var id in args.SelectedIds)
            {
                ids.Add(id);
            }
            RT.Service.Resolve<MaterialReturnController>().Submit(ids);
            return true;
        }
    }
}
