using SIE.Domain;
using SIE.EMS.IdleArchives;
using SIE.EMS.RunStandards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.RunStandards.Commands
{
    /// <summary>
    /// 保存入库单
    /// </summary>
    [JsCommand("SIE.Web.EMS.RunStandards.Commands.SaveRunStandardsCommand")]
    public class SaveRunStandardsCommand : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var model = entity as RunStandard;
            RT.Service.Resolve<RunStandardsController>().SaveRunStandard(model);
        }
    }
}
