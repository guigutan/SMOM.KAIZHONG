using SIE.Domain.Validation;
using SIE.EMS.AssetReturns;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.AssetReturns.Commands
{
    /// <summary>
    /// 提交命令
    /// </summary>
    internal class SubmitAssetReturnCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> selectedIds = new List<double>(args.SelectedIds);
            if (!selectedIds.Any())
            {
                throw new ValidationException("请先选择数据!".L10N());
            }
            RT.Service.Resolve<AssetReturnController>().SumbitAssetReturns(selectedIds);
            return true;
        }
    }
}
