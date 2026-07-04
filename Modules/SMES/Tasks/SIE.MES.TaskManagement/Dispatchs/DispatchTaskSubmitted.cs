using SIE.Domain;
using SIE.MES.TaskManagement.SchedulingInfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs
{
    public class DispatchTaskSubmitted : OnSubmitted<DispatchTask>
    {
        protected override void Invoke(DispatchTask entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Delete)
            {
                if (entity != null && entity.SourceType == SourceType.SchedulingInf)
                {
                    RT.Service.Resolve<SchedulingInfController>().UpdateSchedulingInfValueTaskId(entity.Id);

                }
            }
        }
    }
}
