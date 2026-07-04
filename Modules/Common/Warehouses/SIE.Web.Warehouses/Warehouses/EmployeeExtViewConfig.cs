using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 员工扩展视图配置
    /// </summary>
    public class EmployeeExtViewConfig : WebViewConfig<Employee>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AttachChildrenProperty(typeof(WarehouseEmployee), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Employee>();
                if (parent == null)
                {
                    return new EntityList<WarehouseEmployee>();
                }

                var wareEmp = RT.Service.Resolve<WarehouseController>().GetWarehouseByEmpId(parent.Id, args.PagingInfo);
                return wareEmp;
            }, WarehouseEmployeeViewConfig.EmployeeSelectView).OrderNo = 1;
            View.AttachChildrenProperty(typeof(InWarehouseEmployee), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Employee>();
                if (parent == null)
                {
                    return new EntityList<InWarehouseEmployee>();
                }

                var wareEmp = RT.Service.Resolve<WarehouseController>().GetInWarehouseByEmpId(parent.Id, args.PagingInfo);
                return wareEmp;
            }, InWarehouseEmployeeViewConfig.EmployeeSelectView).OrderNo = 1;
        }
    }
}
