using SIE.Domain;
using SIE.MES.ItemEquipAccount;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount.Commands
{
    /// <summary>
    /// 模具与产品关系的保存命令
    /// </summary>
    public class EquipAccountItemEditCommand : SaveCommand
    {
        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            base.DoSave(data);
        }
    }
}
