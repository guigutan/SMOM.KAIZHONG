using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackingQC.Commands
{
    public class ReportUloadCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<PackingQc>();
            var qc = RF.GetById<PackingQc>(entity.Id);
            var isAutoFeeding = qc.PackingDetailList.Any(p => p.ProductLabel.Contains("*"));
            var log = RT.Service.Resolve<PackingQcController>().SubmitData(entity, isAutoFeeding);
            if (log == "")
            {
                return true;
            }
            else
            {
                return log;
            }
        }
    }
}
