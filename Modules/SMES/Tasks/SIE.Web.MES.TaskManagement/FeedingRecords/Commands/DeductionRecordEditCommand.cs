using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords.Commands
{
    /// <summary>
    /// 修改按钮
    /// </summary>
    public class DeductionRecordEditCommand : ViewCommand<List<DeductionRecord>>
    {
        protected override object Excute(List<DeductionRecord> args, string scope)
        {
            RT.Service.Resolve<FeedingRecordController>().EditDeductionRecord(args);

            return true;
        }
    }
}
