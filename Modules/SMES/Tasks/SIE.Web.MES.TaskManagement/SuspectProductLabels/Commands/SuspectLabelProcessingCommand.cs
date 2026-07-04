using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.TaskManagement.SuspectProductLabels.ApiModels;
using SIE.MES.TaskManagement.SuspectProductLabels.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SuspectProductLabels.Commands
{
    public class SuspectLabelProcessingCommand : ViewCommand<SuspectLabelProcessingCommandData>
    {
        protected override object Excute(SuspectLabelProcessingCommandData data, string scope)
        {
            try
            {
                RT.Service.Resolve<SuspectProductLabelController>().SuspectProductLabelProcessing(data);
            }
            catch(Exception ex)
            {
                return ex.GetBaseException()?.Message;
            }
            return "";
        }
    }
}
