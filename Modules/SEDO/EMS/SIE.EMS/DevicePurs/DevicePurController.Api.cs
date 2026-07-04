using SIE.Api;
using SIE.EMS.DevicePurs.ApiModels;
using SIE.EMS.Maintains.Controller;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 设备与人员权限维护控制器API
    /// </summary>
    public partial class DevicePurController : DomainController
    {
        /// <summary>
        /// 获取登录用户部门
        /// </summary>
        [ApiService("获取登录用户部门")]
        [return: ApiReturn("登录用户部门列表 List<DepartmentInfo>")]
        public virtual List<DepartmentInfo> GetUserDepartments()
        {
            //构建返回数据
            var infos = new List<DepartmentInfo>();

            //var isDepartmentMaintain = RT.Service.Resolve<MaintainController>().IsDepartmentMaintain();
            //if (!isDepartmentMaintain)
            //{
            //    return infos;
            //}

            //通过设备编码获取备件基础数据
            var deviceDepas = RT.Service.Resolve<DevicePurController>().GetLoginUserDeviceDepas();

            deviceDepas.ForEach(p =>
            {
                infos.Add(new DepartmentInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                });
            });

            return infos;
        }
    }
}
