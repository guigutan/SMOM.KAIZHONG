using SIE.EMS.EquipLends;
using SIE.EMS.EquipLends.ApiModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.EquipLends.Commands
{
    /// <summary>
    /// 设备借机归还命令
    /// </summary>
    public class EquipLendReturnCommand : ViewCommand
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
            var returnRemark = args.Data.ToJsonObject<EquipLendExamineInfo>().Remark;
            var selIds = args.SelectedIds.ToList();
            RT.Service.Resolve<EquipLendController>().EquipLendReturn(returnRemark, selIds);
            return true;
        }
    }
}
