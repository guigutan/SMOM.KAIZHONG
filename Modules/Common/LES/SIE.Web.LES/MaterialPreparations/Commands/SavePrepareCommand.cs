using SIE.Domain;
using SIE.LES.MaterialPreparations;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations.Commands
{
    /// <summary>
    /// 备料需求单保存命令
    /// </summary>
    public class SavePrepareCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnSaving(Entity entity)
        {
            var pre = entity as MaterialPreparation;
            RT.Service.Resolve<MaterialPreparationController>().ValidateBeforeSave(pre);
            base.OnSaving(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var pre = entity as MaterialPreparation;
            RT.Service.Resolve<MaterialPreparationController>().SavePreparationOrder(pre);
        }
    }
}
