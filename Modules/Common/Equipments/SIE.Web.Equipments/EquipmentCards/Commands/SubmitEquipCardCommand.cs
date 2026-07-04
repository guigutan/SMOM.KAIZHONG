using SIE.Equipments.EquipmentCards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Equipments.EquipmentCards.Commands
{
    /// <summary>
    /// 设备立卡批量提交
    /// </summary>
    public class SubmitEquipCardCommand : ViewCommand
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
            RT.Service.Resolve<EquipmentCardController>().SubmitEquipmentCard(selectedIds);
            return true;
        }
    }
}
