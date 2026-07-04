using SIE.Domain;
using SIE.Warehouses;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Warehouses.Commands
{
    /// <summary>
    /// 仓库添加命令
    /// </summary>
    public class WarehouseAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var warehouse = args.Data.ToJsonObject<Warehouse>();
            warehouse.Code = RT.Service.Resolve<WarehouseController>().GetWarehouseCode();

            return warehouse;
        }
    }

    /// <summary>
    /// 仓库删除命令
    /// </summary>
    public class WarehouseDeleteCommand : ViewCommand<IList<Entity>>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(IList<Entity> args, string scope)
        {
            if (args.ToList().OfType<Warehouse>().Count(p => p.PersistenceStatus == PersistenceStatus.New || p.State == State.Disable) != args.ToList().Count)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 仓库启用命令
    /// </summary>
    public class WarehouseEnableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //仓库Id列表  

            RT.Service.Resolve<WarehouseController>().EnabelWarehouses(idlist);

            return true;
        }
    }

    /// <summary>
    /// 仓库禁用命令
    /// </summary>
    public class WarehouseDisableCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //仓库Id列表  

            RT.Service.Resolve<WarehouseController>().DisableWarehouses(idlist);

            return true;
        }
    }

    /// <summary>
    /// 仓库复制新增命令
    /// </summary>
    public class WarehouseListCopyCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 更改冻结状态
    /// </summary>
    public class FrozenCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList();  //仓库Id列表  
            RT.Service.Resolve<WarehouseController>().FrozenWarehouses(idlist);

            return true;
        }
    }

    /// <summary>
    /// 仓库地址复制新增命令
    /// </summary>
    public class WarehouseAddressCopyCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 选择仓库与员工关系
    /// </summary>
    public class WarehouseEmployeeLookupCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var employeeList = args.Data.ToJsonObject<List<WarehouseEmployee>>();
            Check.NotNullOrEmpty(employeeList, nameof(employeeList));
            if (employeeList == null || employeeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(employeeList)));
            }
            foreach (var item in employeeList)
            {
                var employee = new WarehouseEmployee();
                employee.EmployeeId = item.EmployeeId;
                employee.WarehouseId = item.WarehouseId;
                savedData.Add(employee);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 选择仓库与员工关系
    /// </summary>
    public class EmployeeWarehouseCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var employeeList = args.Data.ToJsonObject<List<WarehouseEmployee>>();
            Check.NotNullOrEmpty(employeeList, nameof(employeeList));
            if (employeeList == null || employeeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(employeeList)));
            }
            foreach (var item in employeeList)
            {
                var employee = new WarehouseEmployee();
                employee.EmployeeId = item.EmployeeId;
                employee.WarehouseId = item.WarehouseId;
                savedData.Add(employee);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 选择调拨仓与员工的关系
    /// </summary>
    public class InEmployeeWarehouseCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            //var savedData = RF.Find(meta.EntityType).NewList();
            var employeeList = args.Data.ToJsonObject<List<InWarehouseEmployee>>();
            Check.NotNullOrEmpty(employeeList, nameof(employeeList));
            if (employeeList == null || employeeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(employeeList)));
            }
            RT.Service.Resolve<WarehouseController>().SaveInWareHouseEmployee(employeeList);
            
            return true;
        }
    }

    /// <summary>
    /// 同步给员工
    /// </summary>
    public class SynchronizeToEmployeesCommand : ViewCommand<SynchronizeData>
    {
        // <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(SynchronizeData args, string scope)
        {
            RT.Service.Resolve<WarehouseController>().SynchronizeToEmployees(args.EmployeeId,args.EmployeeIds,args.Type);
            return true;
        }
    }

    /// <summary>
    /// 同步提交的数据
    /// </summary>
    public class SynchronizeData
    {
        /// <summary>
        /// 员工ID(同步的数据源ID)
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<double> EmployeeIds{ get; set; }

        /// <summary>
        /// 类型 1-覆盖同步 2-追加同步
        /// </summary>
        public int Type { get; set; }
    }
}
