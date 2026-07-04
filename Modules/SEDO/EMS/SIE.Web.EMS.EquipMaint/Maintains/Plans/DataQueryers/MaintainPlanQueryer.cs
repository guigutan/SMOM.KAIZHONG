using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.GlobalConfigs;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Data;
using SIE.Web.EMS.EquipMaint.Maintains.Plans.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.EMS.Maintains.ApiModels;
using SIE.EMS.Checks.Plans;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers
{
    /// <summary>
    /// 保养计划查询类
    /// </summary>
    public class MaintainPlanQueryer : DataQueryer
    {
        /// <summary>
        /// 根据产线Id获取设备
        /// </summary>
        /// <param name="resourceId">产线Id</param>
        /// <returns>设备台账列表</returns>
        public EntityList<EquipAccount> GetEquipAccountByResourceId(double resourceId)
        {
            return RT.Service.Resolve<EquipController>().GetCheckPlanEquipAccountsByResourceId(resourceId, null);
        }

        /// <summary>
        /// 根据台账获取保养项目
        /// </summary>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <param name="cycleType">周期类型</param>
        /// <param name="level">层级</param>
        /// <returns>保养项目</returns>
        public EntityList<EquipAccountMaintainProject> GetProjectByAccount(double equipAccountId, int cycleType, int level)
        {
            return RT.Service.Resolve<EquipController>().GetProjectByAccount(equipAccountId, cycleType);
        }

        /// <summary>
        /// 根据台账获取保养计划
        /// </summary>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <returns>保养计划</returns>
        public List<MaintainPlan> GetMtPlanByAccount(double equipAccountId)
        {
            return RT.Service.Resolve<MaintainController>().GetMtPlanByAccount(equipAccountId).ToList();
        }

        /// <summary>
        /// 获取保养计划主表列信息
        /// </summary>
        /// <returns>保养计划主表列信息</returns>
        public List<CheckPlanColumn> GetMaintainPlanColumns()
        {
            return RT.Service.Resolve<MaintainController>().GetMaintainPlanColumns(DateTime.Now);
        }

        /// <summary>
        /// 获取保养计划信息（添加）
        /// </summary>
        /// <returns>保养计划信息</returns>
        public List<MaintainPlanRecord> GetMaintainPlanRecords(DateTime beginDate, DateTime endDate, MaintainCycleType maintainCycleType)
        {
            return RT.Service.Resolve<MaintainController>().GetMaintainPlanRecords(beginDate, endDate, maintainCycleType);
        }

        /// <summary>
        /// 获取保养计划信息（修改）
        /// </summary>
        /// <returns>保养计划信息</returns>
        public List<MaintainPlanRecord> GetEditMaintainPlanRecords(string equipAccountCode)
        {
            return RT.Service.Resolve<MaintainController>().GetEditMaintainPlanRecords(equipAccountCode);
        }

        /// <summary>
        /// 获取当前保养单执行数据
        /// </summary>
        /// <param name="maintainPlanId"></param>
        /// <returns></returns>
        public List<SelDepartmentViewModel> GetMaintainPlansById(double maintainPlanId)
        {
            //根据设备人员权限，查询保养单
            var list = RT.Service.Resolve<MaintainController>().GetMaintainPlansDisplay(maintainPlanId);

            if (!list.Any())
            {
                throw new ValidationException("当前登陆人没有当前保养单责任部门权限".L10nFormat());
            }

            //构建界面返回VM数据
            var vms = new List<SelDepartmentViewModel>();
            list.ForEach(p =>
            {
                var vm = new SelDepartmentViewModel();
                vm.MaintainPlanId = p.Id;
                vm.MaintainPlanNo = p.No;
                vm.DepartmentId = p.DepartmentId;
                vm.DepartmentName = p.DepartmentName;
                vm.EquipAccountId = p.EquipId;
                vm.State = (MaintExeState)p.State;
                vm.IfOpenConfirmationTab = (vm.State == MaintExeState.NotConfirm || vm.State == MaintExeState.Scored);
                vm.IsConfirm = p.MaintainConfirm;
                vms.Add(vm);
            });

            return vms;
        }

        /// <summary>
        /// 获取该用户在该设备上指定时间的所有保养单
        /// </summary>
        /// <param name="equipId">设备Id</param>
        /// <param name="beginDate">计划开始时间</param>
        /// <param name="endDate">计划结束时间</param>
        /// <returns></returns>
        public List<SelDepartmentViewModel> GetMaintainPlans(double equipId, DateTime beginDate, DateTime endDate)
        {
            //查询保养计划单
            var list = RT.Service.Resolve<SIE.EMS.Maintains.Controller.MaintainController>().GetMaintainPlans(equipId, beginDate, endDate);

            if (!list.Any())
            {
                throw new ValidationException("无保养执行单或保养确认单。".L10nFormat());
            }

            //构建界面返回VM数据
            var vms = new List<SelDepartmentViewModel>();
            list.ForEach(p =>
            {
                var vm = new SelDepartmentViewModel();
                vm.MaintainPlanId = p.Id;
                vm.MaintainPlanNo = p.No;
                vm.DepartmentId = p.DepartmentId;
                vm.DepartmentName = p.DepartmentName;
                vm.EquipAccountId = p.EquipId;
                vm.State = (MaintExeState)p.State;
                vm.IfOpenConfirmationTab = (vm.State == MaintExeState.NotConfirm || vm.State == MaintExeState.Scored);
                vm.IsConfirm = p.MaintainConfirm;
                vms.Add(vm);
            });

            return vms;
        }

        /// <summary>
        /// 初始化保养执行界面
        /// </summary>
        /// <param name="maintainPlanId">保养计划ID</param>        
        /// <param name="accountId">设备台账ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        public MaintainSummary InitExeMaintainPlan(double maintainPlanId, double accountId, double? departmentId)
        {
            //获取保养执行的指定开始时间和结束时间
            return RT.Service.Resolve<MaintainController>().GetMaintainPlanForExcute(maintainPlanId, accountId, departmentId);
        }
               
        /// <summary>
        ///根据Id获取保养单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetMaintainPlanById(double id)
        {
            return RT.Service.Resolve<MaintainController>().GetMaintainPlanById(id);
        }

        /// <summary>
        /// 根据设备Id获取设备
        /// </summary>
        /// <param name="id">设备Id</param>
        /// <returns></returns>
        public EquipAccount GetEquipAccountById(double id)
        {
            return RT.Service.Resolve<ElecEquipController>().GetEquipAccountById(id);
        }

        /// <summary>
        /// 工时登记默认添加一条
        /// </summary>
        /// <param name="begTime">保养计划开始时间</param>
        /// <param name="endTime">保养计划结束时间</param>
        /// <param name="employeeId">当前账户Id</param>
        /// <param name="id">保养计划Id</param>
        /// <returns></returns>
        public object AddWorkHoursRegister(DateTime begTime, DateTime endTime, double employeeId, double id)
        {
            DateTime dt1 = Convert.ToDateTime(endTime);
            DateTime dt2 = Convert.ToDateTime(begTime);
            TimeSpan ts1 = dt1.Subtract(dt2);
            var workHours = Math.Round(ts1.TotalHours, 2);
            WorkHoursRegister entity = new WorkHoursRegister()
            {
                BeginDay = begTime,
                EndDay = endTime,
                EmployeeId = employeeId,
                MaintainPlanId = id,
                WorkHours = workHours
            };
            RT.Service.Resolve<MaintainController>().AddWorkHoursRegister(entity);
            return workHours;
        }

        /// <summary>
        /// 时间计算
        /// </summary>
        /// <param name="begTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public object CalculateDate(DateTime begTime, DateTime endTime)
        {
            DateTime dt1 = Convert.ToDateTime(endTime);
            DateTime dt2 = Convert.ToDateTime(begTime);
            TimeSpan ts1 = dt1.Subtract(dt2);
            return Math.Round(ts1.TotalHours, 2);
        }

        /// <summary>
        /// 获取备件库存
        /// </summary>
        /// <param name="sparePareId">备件ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public int GetSparePartStoreQty(double sparePareId, double warehouseId)
        {
            return RT.Service.Resolve<SparePartController>().GetSparePartStoreQty(sparePareId, warehouseId);
        }

        /// <summary>
        /// 获取设备点检和保养异常PDCA管控的配置值
        /// </summary>
        /// <returns></returns>
        public bool GetPdcaConfigValue()
        {
            return RT.Service.Resolve<EmsGlobalConfigController>().GetCheckAndMaintainPdcaConfigValue();
        }
    }
}
