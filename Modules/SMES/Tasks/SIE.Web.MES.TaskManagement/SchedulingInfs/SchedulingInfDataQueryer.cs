using SIE.Domain;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs
{
    public class SchedulingInfDataQueryer : DataQueryer
    {
        public object GetSchedulingInfValues(List<double> ids)
        {
            var list = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfValuesBySchedulingInfId(ids);
            foreach (var l in list)
            {
                l.DateStr = l.Date.ToString("yyyyMMdd");
            }
            var dates = list.OrderByDescending(p => p.Date).Select(p => p.Date.ToString("yyyyMMdd")).Distinct().ToList();

            return new
            {
                dates = dates,
                list = list
            };
        }
    }
}
