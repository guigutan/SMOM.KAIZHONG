using SIE.MES.LoadItems;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.LoadItems.DeductItems.Commands
{
    /// <summary>
    /// 强制关闭命令
    /// </summary>
    public class WoCostItemCloseCommand : ViewCommand
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
            var deductItem = args.Data.ToJsonObject<WoCostItem>();
            RT.Service.Resolve<WoCostItemController>().Close(deductItem.Id);
            return true;
        }
    }
}
