using SIE.MES.LoadItems;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.MES.LoadItems.DeductItems.Commands
{
    /// <summary>
    /// 补扣命令
    /// </summary>
    public class WoCostItemSupplementCommand : ViewCommand
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
            //var deductItem = args.Data.ToJsonObject<WoCostItem>();
            var deductItemIds = args.SelectedIds.ToList();
            return RT.Service.Resolve<WoCostItemController>().SupDeduct(deductItemIds);
        }
    }
}
