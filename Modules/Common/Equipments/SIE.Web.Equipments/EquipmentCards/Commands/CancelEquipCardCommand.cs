using SIE.Domain.Validation;
using SIE.Equipments.EquipmentCards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipmentCards.Commands
{
    /// <summary>
    /// 设备立卡撤回
    /// </summary>
    public class CancelEquipCardCommand : ViewCommand
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
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ValidationException("撤回预算变更数据参数不能为空".L10N());
            }
            List<double> selectedIds = new List<double>(args.SelectedIds);
            RT.Service.Resolve<EquipmentCardController>().CancelEquipmentCard(selectedIds);
            return true;
        }
    }
}
