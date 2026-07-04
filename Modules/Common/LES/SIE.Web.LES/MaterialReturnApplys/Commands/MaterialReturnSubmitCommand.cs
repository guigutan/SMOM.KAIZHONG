using SIE.Domain;
using SIE.LES.MaterialReturnApplys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialReturnApplys.Commands
{
    /// <summary>
    /// 退料申请提交命令
    /// </summary>
    public class MaterialReturnSubmitCommand : MaterialReturnSaveCommand
    {
        /// <summary>
        /// 保存后提交
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var data = entity as MaterialReturnApply;
            RT.Service.Resolve<MaterialReturnApplyController>().SaveSubmitReturnApply(data);
        }
    }
}
