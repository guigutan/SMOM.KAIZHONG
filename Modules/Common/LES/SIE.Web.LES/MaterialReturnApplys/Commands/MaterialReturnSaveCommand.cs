using SIE.Domain;
using SIE.LES.MaterialReturnApplys;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialReturnApplys.Commands
{
    /// <summary>
    /// 退料申请保存命令
    /// </summary>
    public class MaterialReturnSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(Entity data)
        {
            var apply = data as MaterialReturnApply;
            RT.Service.Resolve<MaterialReturnApplyController>().ValidateBeforeSave(apply);
            base.OnSaving(data);
        }
    }
}
