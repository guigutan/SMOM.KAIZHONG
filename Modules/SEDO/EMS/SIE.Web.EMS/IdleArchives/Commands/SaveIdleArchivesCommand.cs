using SIE.Domain;
using SIE.EMS.IdleArchives;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.IdleArchives.Commands
{
    /// <summary>
    /// 保存入库单
    /// </summary>
    [JsCommand("SIE.Web.EMS.IdleArchives.Commands.SaveIdleArchivesCommand")]
    public class SaveIdleArchivesCommand : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var model = entity as IdleArchive;
            RT.Service.Resolve<IdleArchivesController>().SaveIdleArchive(model);
        }
    }
}
