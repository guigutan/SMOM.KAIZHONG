using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 设备润滑PDA首页统计
    /// </summary>
    public partial class LubricationController : DomainController
    {
        /// <summary>
        /// 获取润滑单对应状态的单据统计数据
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public virtual List<LubricationStatus> GetLubPDAHomeInfo(List<LubricationStatus> states)
        {
            // 获取权限部门
            var deptIds = RT.Service.Resolve<DevicePurController>().GetDutyDepartmentIds(RT.Identity.UserId).Cast<double?>();

            // 查询状态
            var queryStates = Query<Lubrication>().Where(p => states.Contains(p.LubricationStatus) && (deptIds.Contains(p.DepartmentId) || p.DepartmentId == null))
                .Select(p => new { p.LubricationStatus }).ToList<LubricationStatus>().ToList();
            return queryStates;
        }
    }
}
