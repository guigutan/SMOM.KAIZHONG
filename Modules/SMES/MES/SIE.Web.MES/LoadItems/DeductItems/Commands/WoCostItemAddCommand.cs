using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.LoadItems.DeductItems.Commands
{
    /// <summary>
    /// 工单耗用单添加命令
    /// </summary>
    public class WoCostItemAddCommand : ViewCommand
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
            var woCostItem = args.Data.ToJsonObject<WoCostItem>();
            var dateTime = RF.Find<WoCostItem>().GetDbTime();
            woCostItem.CostNo = RT.Service.Resolve<WoCostItemController>().GetCostNoRule(1).FirstOrDefault();
            woCostItem.State = SIE.MES.LoadItems.Enum.WoCostItemState.ToSubmit;
            woCostItem.CreateBy = RT.IdentityId;            
            woCostItem.CreateDate = dateTime;
            return woCostItem;
        }
    }
}
