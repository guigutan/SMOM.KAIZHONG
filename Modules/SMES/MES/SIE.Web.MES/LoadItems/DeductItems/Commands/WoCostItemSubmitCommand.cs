using SIE.MES.LoadItems;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.LoadItems.DeductItems.Commands
{
    /// <summary>
    /// 工单耗用单提交命令
    /// </summary>
    public class WoCostItemSubmitCommand : ViewCommand
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
            var data = args.Data.ToJsonObject<WoCostItem>();
            RT.Service.Resolve<WoCostItemController>().SubmitCost(data);
            return true;
        }
    }
}
