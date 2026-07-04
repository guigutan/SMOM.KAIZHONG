using SIE.Domain;
using SIE.Warehouses;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Warehouses.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    public class WorkAreaAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var workArea = args.Data.ToJsonObject<WorkArea>();
            workArea.Code = RT.Service.Resolve<WarehouseController>().GetWorkAreaCode();

            return workArea;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class WorkAreaDeleteCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 选择库位命令
    /// </summary>
    public class WorkAreaSelLocationCommand : ViewCommand
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
            var locationList = args.Data.ToJsonObject<List<WorkAreaLocation>>();
            Check.NotNullOrEmpty(locationList, nameof(locationList));
            if (null == locationList || locationList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(locationList)));
            }
            foreach (var item in locationList)
            {
                WorkAreaLocation workAreaLocation = new WorkAreaLocation();
                workAreaLocation.WorkAreaId = item.WorkAreaId;
                workAreaLocation.StorageLocationId = item.Id;
                savedData.Add(workAreaLocation);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 库位关系删除命令
    /// </summary>
    public class WorkAreaLocDeleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> locIdList = args.ToList();
            EntityList<WorkAreaLocation> locations = RT.Service.Resolve<WarehouseController>().GetWorkAreaLocationList(locIdList);
            locations.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

            RF.Save(locations);
            return true;
        }
    }

    /// <summary>
    /// 选择员工命令
    /// </summary>
    public class WorkAreaSelEmployeeCommand : ViewCommand
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
            var employeeList = args.Data.ToJsonObject<List<WorkAreaEmployee>>();
            Check.NotNullOrEmpty(employeeList, nameof(employeeList));
            if (null == employeeList || employeeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(employeeList)));
            }
            foreach (var item in employeeList)
            {
                WorkAreaEmployee workAreaEmployee = new WorkAreaEmployee();
                workAreaEmployee.WorkAreaId = item.WorkAreaId;
                workAreaEmployee.EmployeeId = item.Id;
                workAreaEmployee.WorkSituation = WorkSituation.OnDuty;
                savedData.Add(workAreaEmployee);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 员工关系删除命令
    /// </summary>
    public class WorkAreaEmployeeDeleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> empIdList = args.ToList();
            EntityList<WorkAreaEmployee> employees = RT.Service.Resolve<WarehouseController>().GetWorkAreaEmployeeList(empIdList);
            employees.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

            RF.Save(employees);
            return true;
        }
    }

    /// <summary>
    /// 在岗命令
    /// </summary>
    public class WorkAreaEmployeeOnDutyCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> empIdList = args.ToList();
            RT.Service.Resolve<WarehouseController>().WorkAreaEmployeeWorkSituation(empIdList, WorkSituation.OnDuty);
            return true;
        }
    }

    /// <summary>
    /// 离岗命令
    /// </summary>
    public class WorkAreaEmployeeOffDutyCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> empIdList = args.ToList();
            RT.Service.Resolve<WarehouseController>().WorkAreaEmployeeWorkSituation(empIdList, WorkSituation.OffDuty);
            return true;
        }
    }
}