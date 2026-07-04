using SIE.Domain;
using SIE.LES.MaterialPreparations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations.Commands
{
    /// <summary>
    /// 车间备料提交命令
    /// </summary>
    public class SubmitPrepareCommand : SavePrepareCommand
    {
        /// <summary>
        /// 保存后提交
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var pre = entity as MaterialPreparation;
            RT.Service.Resolve<MaterialPreparationController>().SaveSubmitPreparationOrder(pre);
        }
    }
}
