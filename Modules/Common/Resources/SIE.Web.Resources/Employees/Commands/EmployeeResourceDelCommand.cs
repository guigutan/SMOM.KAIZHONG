using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 执行员工维护的资源删除命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.EmployeeResourceDelCommand")]
    public class EmployeeResourceDelCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">员工ID</param>
        /// <param name="scope">作用域</param>
        /// <returns></returns>

        protected override object Excute(double[] args, string scope)
        {
            //var employeeResource = RF.GetById<EmployeeResource>(Id);
            //if (employeeResource == null) return false;
            //employeeResource.PersistenceStatus = PersistenceStatus.Deleted;
            //RF.Save(employeeResource);
            var selectIds = args.ToList();
            RT.Service.Resolve<EmployeeController>().EmployeeResourceDelCommand(selectIds);
            return true;
        }
    }
}

