using SIE.Common.Employees;
using SIE.Core.Common.Controllers;
using SIE.DataPortal;
namespace SIE.xUnit.Core
{
    /// <summary>
    /// 单元测试基类。使用启动固件
    /// </summary>
    public abstract class TestBase 
    {
        protected TestBase()
        {
            if (RT.InvOrg == null || RT.InvOrg != 0 || RT.Principal == null || RT.IdentityId == 0)
            {
                var config = RT.Config.Get<TestContext>("Context");
                RT.InvOrg = config.InvOrg;
                var emp = RT.Service.Resolve<CommonController>().GetData<Employee>(p => p.Name == config.EmployeeName);
                RT.Principal = new DataPortalPrincipal(emp.Id, emp.UserId.Value, config.EmployeeName);
            }
        }
    }
}
