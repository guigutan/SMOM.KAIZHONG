using SIE.Domain;
using SIE.EMS.EquipLends;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipLends.Commands
{
    /// <summary>
    /// 设备借还保存命令
    /// </summary>
    public class EquipLendSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnSaving(Entity entity)
        {
            var lend = entity as EquipLendManage;
            RT.Service.Resolve<EquipLendController>().EquipLendOnSavingValidate(lend);
            base.OnSaving(entity);
        }
    }
}
