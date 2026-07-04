using SIE.Domain.Validation;
using SIE.EMS.AssetTransfers;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.AssetTransfers.Commands
{
    /// <summary>
    ///调拨发货
    /// </summary>
    [JsCommand("SIE.Web.EMS.AssetTransfers.Commands.SendAssetTransfersCommand")]
    public class SendAssetTransfersCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }
           
            if (args.Data.IsNullOrEmpty())
            {
                throw new ValidationException("请先选择数据".L10N());
            }
            double selectedId = double.Parse(args.Data);
            RT.Service.Resolve<AssetTransfersController>().Send(selectedId);
            return true;
        }
    }
}
