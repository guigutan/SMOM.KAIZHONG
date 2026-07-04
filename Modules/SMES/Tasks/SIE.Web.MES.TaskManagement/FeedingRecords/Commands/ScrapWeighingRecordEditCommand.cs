using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords.Commands
{
    public class ScrapWeighingRecordEditCommand : ViewCommand<List<ScrapWeighingRecord>>
    {
        protected override object Excute(List<ScrapWeighingRecord> args, string scope)
        {
            RT.Service.Resolve<FeedingRecordController>().EditScrapWeighingRecord(args);
            return true;
        }
    }
}
