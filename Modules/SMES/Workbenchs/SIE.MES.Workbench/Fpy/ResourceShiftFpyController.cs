using SIE.Domain;
using SIE.MES.Statistics.Fpy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Workbench.Fpy
{
    public class ResourceShiftFpyController : DomainController
    {
        public virtual List<ResourceShiftFpy> GetResourceShiftFpyList(double resourceId, double shiftId, DateTime beginDate, DateTime endDate)
        {
            var list = new List<ProcessFpyStatistics>();
            var q = Query<ProcessFpyStatistics>().Where(
               p => p.InvOrgId == RT.InvOrg &&
                p.ResourceId == resourceId && p.ShiftId == shiftId && p.ShiftDate >= beginDate && p.ShiftDate <= endDate
            );
            list.AddRange(q.ToList());
            var list1 = list.GroupBy(x => new { x.ResourceId, x.ResourceName, x.ShiftDate, x.ShiftId, x.ShiftName, x.ProcessId, x.ProcessName }).Select(
                g => new ProcessFpyStatistics
                {
                    ShiftId = g.Key.ShiftId,
                    ShiftName = g.Key.ShiftName,
                    ProcessId = g.Key.ProcessId,
                    ProcessName = g.Key.ProcessName,
                    PassQty = g.Sum(p => p.PassQty),
                    FailedQty = g.Sum(p => p.FailedQty),
                    InputQty = g.Sum(p => p.InputQty)
                }
            ).ToList();

            List<ResourceShiftFpy> listResourceFpy = new List<ResourceShiftFpy>();
            list1.ForEach(e =>
            {
                ResourceShiftFpy i = new ResourceShiftFpy();
                i.ResourceId = e.ResourceId;
                i.ShiftId = e.ShiftId;
                i.ShiftDate = e.ShiftDate;
                i.ProcessId = e.ProcessId;
                i.ProcessName = e.ProcessName;
                i.Fpy = e.PassQty / e.InputQty;
                listResourceFpy.Add(i);
            });
            return listResourceFpy;
        }
    }
}
