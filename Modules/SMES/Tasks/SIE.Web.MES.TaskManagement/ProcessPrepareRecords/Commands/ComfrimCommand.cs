using Newtonsoft.Json;
using SIE.Common;
using SIE.MES.ProcessPrepareRecords;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessPrepareRecordDetail = SIE.MES.TaskManagement.ProcessPrepareRecords.ProcessPrepareRecordDetail;
using ProcessPrepareRecordsController = SIE.MES.TaskManagement.ProcessPrepareRecords.ProcessPrepareRecordsController;

namespace SIE.Web.MES.TaskManagement.ProcessPrepareRecords.Commands
{
    public class ComfrimCommand : FormSaveCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var prepareRecordDetail = JsonConvert.DeserializeObject<List<ProcessPrepareRecordDetail>>(args.Data);

            //RT.Service.Resolve<ProcessPtyController>().ExecuteComfrim(prepareRecordDetail.AsEntityList());
            RT.Service.Resolve<ProcessPrepareRecordsController>().PPRExecuteComfrim(prepareRecordDetail);

            return true;
        }
    }
}
