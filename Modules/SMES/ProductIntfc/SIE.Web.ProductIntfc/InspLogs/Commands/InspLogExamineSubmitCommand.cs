using SIE.Domain;
using SIE.ProductIntfc.InspLogs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ProductIntfc.InspLogs.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class InspLogExamineSubmitCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var log = entity as InspLog;
            RT.Service.Resolve<InspLogController>().ExamineSubmit(log);
        }
    }
}
