using SIE.Common.Employees;
using SIE.Core.Common.Controllers;
using SIE.DataPortal;
using System;

namespace SIE.xUnit.Core
{
    /// <summary>
    /// 固件基类
    /// </summary>
    public class FixtureBase : IDisposable
    {
        public FixtureBase()
        {
            if (RT.InvOrg == null || RT.InvOrg != 0 || RT.Principal == null || RT.IdentityId == 0)
            {
                var config = RT.Config.Get<TestContext>("Context");
                RT.InvOrg = config.InvOrg;
                var emp = RT.Service.Resolve<CommonController>().GetData<Employee>(p => p.Name == config.EmployeeName);
                RT.Principal = new DataPortalPrincipal(emp.Id, emp.UserId.Value, config.EmployeeName);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
