using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.GlobalConfigs;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Data;
using SIE.Web.EMS.Checks.Plans.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.DataQuery
{
    /// <summary>
    /// 点检计划维护查询器
    /// </summary>
    public class CheckPlanDataQueryer : DataQueryer
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
        /// <returns>固定资产对应点检保养项目</returns>
        public EntityList<EquipAccountMaintainProject> GetProjectByAccount(double equipAccountId, int cycleType, int level)
        {
            return RT.Service.Resolve<EquipController>().GetProjectByAccount(equipAccountId, cycleType);
        }

        /// <summary>
        /// 根据台账获取设备台账点检项目
        /// </summary>
        /// <param name="equipAccountId">设备台账Id</param>
        /// <param name="cycleType">周期类型</param>
        /// <param name="level">层级</param>
        /// <returns>固定资产对应点检保养项目</returns>
        public EntityList<EquipAccountCheckProject> GetCheckProjectByAccount(double equipAccountId, int cycleType, int level)
        {
            return RT.Service.Resolve<EquipController>().GetCheckProjectsByAccount(equipAccountId, cycleType);
        }

        /// <summary>
        /// 根获取日点检计划
        /// </summary>
        /// <param name="equipId">设备台账Id</param>
        /// <param name="beginDate">点检开始日期</param>
        /// <param name="endDate">点检结束日期</param>
        /// <returns></returns>
        public List<SelDepartmentViewModel> GetCheckPlan(double equipId, DateTime beginDate, DateTime endDate)
        {
            //获取配置项点检周期类型
            var checkCycleType = RT.Service.Resolve<CheckController>().GetCheckPlanType();
            //根据设备人员权限，查询点检计划单
            var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlans(equipId, checkCycleType, beginDate, endDate);
            
            if (!list.Any())
            {
                throw new ValidationException("无点检执行单或点检确认单。".L10nFormat());
            }

            //"当前不在点检时间，点检时间为：{0} - {1}".L10nFormat(beginDate, endDate)
            //构建界面返回VM数据
            var vms = new List<SelDepartmentViewModel>();
            list.ForEach(p =>
            {
                var vm = new SelDepartmentViewModel();
                vm.CheckPlanId = p.Id;
                vm.CheckPlanNo = p.No;
                vm.DepartmentId = p.DepartmentId;
                vm.DepartmentName = p.DepartmentName;
                vm.EquipAccountId = p.EquipId;
                vm.State = (CheckExeState)p.State;
                vm.IfOpenConfirmationTab = true;
                vm.IsConfirm = p.CheckConfirm;
                vms.Add(vm);
            });

            return vms;
        }

        /// <summary>
        /// 初始化点检计划执行界面
        /// </summary>
        /// <param name="checkPlanId"></param>
        /// <param name="accountId"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public string InitExeCheckPlan(double checkPlanId, double accountId, double? departmentId)
        {
            //生成点检计划点检项目
            RT.Service.Resolve<CheckPlanController>().GeneratePlanProject(checkPlanId);

            //获取上次点检小结
            var lastCheckSummary = RT.Service.Resolve<CheckPlanController>().GetLastCheckSummary(accountId, departmentId);
            return lastCheckSummary;
        }

        /// <summary>
        /// 获取点检计划主表列信息
        /// </summary>
        /// <returns>点检计划主表列信息</returns>
        public List<CheckPlanColumn> GetCheckPlanColumns(DateTime dateTime)
        {
            return RT.Service.Resolve<CheckPlanController>().GetCheckPlanColumns(dateTime);
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

        /// <summary>
        /// 根获取日点检计划
        /// </summary>
        /// <param name="checkPlanId">点检计划ID</param> 
        /// <returns></returns>
        public List<SelDepartmentViewModel> GetCheckPlanById(double checkPlanId)
        {
            //根据设备人员权限，查询点检计划单
            var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlansDisplay(checkPlanId);

            if (!list.Any())
            {
                throw new ValidationException("当前登陆人没有当前点检单责任部门权限".L10nFormat());
            }

            //"当前不在点检时间，点检时间为：{0} - {1}".L10nFormat(beginDate, endDate)
            //构建界面返回VM数据
            var vms = new List<SelDepartmentViewModel>();
            list.ForEach(p =>
            {
                var vm = new SelDepartmentViewModel();
                vm.CheckPlanId = p.Id;
                vm.CheckPlanNo = p.No;
                vm.DepartmentId = p.DepartmentId;
                vm.DepartmentName = p.DepartmentName;
                vm.EquipAccountId = p.EquipId;
                vm.State = (CheckExeState)p.State;
                vm.IfOpenConfirmationTab = (vm.State == CheckExeState.NotConfirm || vm.State == CheckExeState.Scored);
                vm.IsConfirm = p.CheckConfirm;
                vms.Add(vm);
            });

            return vms;
        }


        /// <summary>
        /// 添加点检计划重复校验
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        public virtual AddCheckPlanResultInfo AddCheckPlanToVerifyRepeat(AddCheckPlanViewModel model)
        {
            var equipList = new List<BaseDataInfo>
            {
                RT.Service.Resolve<EquipAccountController>().GetEquipAccountBaseInfo(model.EquipAccountId)
            };
            return RT.Service.Resolve<CheckPlanController>().AddCheckPlanToVerifyRepeat(model, equipList);
        }

        /// <summary>
        /// 批次添加点检计划校验重复
        /// </summary>
        /// <param name="model">点检计划添加Model</param>
        /// <param name="equipIds">设备清单</param>
        /// <returns></returns>
        public virtual AddCheckPlanResultInfo BatchAddCheckPlanToVerifyRepeat(AddCheckPlanViewModel model, List<double> equipIds)
        {
            var equipList = new List<BaseDataInfo>();
            equipList.AddRange(RT.Service.Resolve<EquipAccountController>().GetEquipAccountBaseInfos(equipIds));
            return RT.Service.Resolve<CheckPlanController>().AddCheckPlanToVerifyRepeat(model, equipList);
        }
    }
}
