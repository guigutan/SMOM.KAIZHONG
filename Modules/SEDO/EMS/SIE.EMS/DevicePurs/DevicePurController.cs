using SIE.Common.Configs;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.EMS.DevicePurs.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.EMS.Common.Entity;
using SIE.Core.Common;
using SIE.EMS.DevicePurs.ApiModels;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 设备与人员权限维护控制器
    /// </summary>
    public partial class DevicePurController : DomainController
    {
        /// <summary>
        /// 获取所有的用户组
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<UserGroup> GetUserGroups(PagingInfo pagingInfo, string keyword)
        {
            var list = Query<UserGroup>();
            if (!keyword.IsNullOrEmpty())
            {
                list.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return list.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<User> GetUsers(PagingInfo pagingInfo, string keyword)
        {
            var list = Query<User>();
            if (!keyword.IsNullOrEmpty())
            {
                list.Where(p => p.Code.Contains(keyword) || p.Employee.Name.Contains(keyword));
            }
            return list.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备与人员权限
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>备与人员权限列表</returns>
        public virtual EntityList<DevicePur> GetDevicePurs(DevicePurCriteria criteria)
        {
            var query = Query<DevicePur>();
            if (!string.IsNullOrEmpty(criteria.UserGroupCode))
            {
                query.Where(p => p.UserGroup.Code.Contains(criteria.UserGroupCode));
            }
            if (!string.IsNullOrEmpty(criteria.UserGroupName))
            {
                query.Where(p => p.UserGroup.Name.Contains("%" + criteria.UserGroupName + "%"));
            }
            if (!string.IsNullOrEmpty(criteria.EmployeeCode))
            {
                query.Where(p => p.User.Code.Contains(criteria.EmployeeCode));
            }
            if (!string.IsNullOrEmpty(criteria.EmployeeName))
            {
                query.Where(p => p.User.Employee.Name.Contains("%" + criteria.EmployeeName + "%"));
            }
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前登录用户及所在用户组，在人员权限的部门集合
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetLoginUserDeviceDepas()
        {
            var q = Query<Enterprise>();
            q.Exists<DeviceDepa>((x, y) => y.Join<DevicePur>((a, b) => a.DevicePurId == b.Id)
                .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((a, b, c) => x.Id == a.EnterpriseId && (c.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)));

            return q.ToList();
        }

        /// <summary>
        /// 根据设备ID，获取有设备/备件维修权限的员工列表
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="isEquipRepair"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetDevicePurRepairEmployees(bool isEquipRepair = true)
        {
            var q = Query<Employee>().As("x");
            if (isEquipRepair)
            {
                q.Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                     .Where<UserInUserGroup>((a, b) => (a.UserId == x.UserId || b.UserId == x.UserId) && a.EquipMaintain));
            }
            return q.ToList();
        }

        /// <summary>
        /// 获取是否启用设备权限管理
        /// </summary>
        /// <returns></returns>
        public virtual YesNo GetEnableDevicePermissions()
        {
            //获取是否启用设备权限的配置项
            YesNo enableDevicePermissions;
            var configValue = ConfigService.GetConfig(new EnableDevicePermissionsConfig(), typeof(DevicePur));
            if (configValue == null || configValue.EnableDevicePermissions == null)
            {
                enableDevicePermissions = new EnableDevicePermissionsConfig().DefaultValue.EnableDevicePermissions.Value;
            }
            else
            {
                enableDevicePermissions = configValue.EnableDevicePermissions.Value;
            }
            return enableDevicePermissions;
        }

        /// <summary>
        /// 根据设备ID，获取有设备维修权限的员工列表
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetDevicePurRepairs(double equipId, string keyword, PagingInfo pagingInfo)
        {
            var q = Query<Employee>().As("x");
            q.Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<UserInUserGroup>((a, b) => (a.UserId == x.UserId || b.UserId == x.UserId) && a.EquipMaintain));
            if (keyword.IsNotEmpty())
            {
                q.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            var list = q.ToList(pagingInfo);
            return list;
        }

        /// <summary>
        /// 获取有设备维修权限的员工列表（备件）
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetSparePartDevicePurRepairs(string keyword, PagingInfo pagingInfo)
        {
            //查询出设备与人员权限维护的用户关联且开始设备维修权限的所有员工
            var q = Query<Employee>();
            q.Exists<DevicePur>((x, y) => y.Where(p => p.UserId == x.UserId && p.EquipMaintain));

            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            var list = q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }


        /// <summary>
        /// 根据当前用户获取 权限
        /// </summary>
        /// <returns></returns>
        public virtual DevicePur GetDevicePurByNowUserId()
        {
            return DB.Query<DevicePur>().Where(p => p.UserId == RT.Identity.UserId).FirstOrDefault();
        }

        /// <summary>
        /// 获取人员权限设备清单
        /// </summary>
        /// <param name="devicePurId"></param>
        /// <param name="orderInfoList"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<DeviceBill> GetDeviceBills(double devicePurId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo, string keyword)
        {
            var q = Query<DeviceBill>();
            q.Where(p => p.DevicePurId == devicePurId);
            q.WhereIf(keyword.IsNotEmpty(), p => p.EquipAccount.Code.Contains(keyword) || p.EquipAccount.Name.Contains(keyword));
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取用户的责任部门列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetDutyDepartments(double userId)
        {
            var q = Query<DeviceDepa>()
                 .LeftJoin<DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => b.UserId == userId || c.UserId == userId);
            var deviceDepaList = q.ToList();

            return deviceDepaList.Select(x => x.EnterpriseId).Distinct().SplitContains(tempIds =>
            {
                return Query<Enterprise>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取用户的责任部门列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual List<double> GetDutyDepartmentIds(double userId)
        {
            List<double> dptIds = new List<double>();
            var q = Query<DeviceDepa>()
                 .LeftJoin<DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => b.UserId == userId || c.UserId == userId);
            var deviceDepaList = q.ToList();
            if (!deviceDepaList.Any())
            {
                return new List<double>();
            }
            deviceDepaList.Select(x => x.EnterpriseId).Distinct().SplitDataExecute(tempIds =>
            {
                var list = Query<Enterprise>().Where(p => tempIds.Contains(p.Id)).Select(p => new {p.Id}).ToList<double>().ToList();
                dptIds.AddRange(list);
            });
            return dptIds;
        }

        /// <summary>
        /// 指定用户可以对哪些部门的点检单进行点检确认
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual List<DurDeptInfo> GetDepartmentsForConfirmCheck(double userId)
        {
            var q = Query<DeviceDepa>()
                 .LeftJoin<DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == userId || c.UserId == userId));
            var deviceDepaList = q.Select<DevicePur>((dd, dp) => new
            {
                DeptId = dd.EnterpriseId,
                IsConfirm = dp.CheckConfirm,
            }).ToList<DurDeptInfo>().ToList();

            return deviceDepaList;
        }

        /// <summary>
        /// 指定用户可以对哪些部门的保养单进行保养确认
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual List<DurDeptInfo> GetDepartmentsForConfirmMaintain(double userId)
        {
            var q = Query<DeviceDepa>()
                 .LeftJoin<DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == userId || c.UserId == userId));
            var deviceDepaList = q.Select<DevicePur>((dd, dp) => new
            {
                DeptId = dd.EnterpriseId,
                IsConfirm = dp.MaintainConfirm,
            }).ToList<DurDeptInfo>().ToList();

            return deviceDepaList;
        }

        /// <summary>
        /// 根据用户ID和部门ID，获取有权限的预算部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        public virtual DeviceBudgetDepartment GetBudgetDepartment(double userId, double departmentId)
        {
            //检查用户预算部门权限
            var deviceBudgetDepartment = Query<DeviceBudgetDepartment>()
                 .LeftJoin<DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == userId || c.UserId == userId))
                 .Where(x => x.EnterpriseId == departmentId)
                 .FirstOrDefault(new EagerLoadOptions().LoadWith(DeviceBudgetDepartment.EnterpriseProperty));

            return deviceBudgetDepartment;
        }

        /// <summary>
        /// 根据用户ID和部门名称获取有权限的部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="factoryId">工厂ID</param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetUserBudgetDepartments(double userId, PagingInfo pagingInfo, string keyword,
            double? factoryId = null)
        {
            //检查用户预算部门权限
            var query = Query<Enterprise>()
                .Join<DeviceBudgetDepartment>((x, y) => x.Id == y.EnterpriseId)
                .Join<DeviceBudgetDepartment, DevicePur>((a, b) => a.DevicePurId == b.Id)
                .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == userId || c.UserId == userId))
                .WhereIf(keyword.IsNotEmpty(), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));

            if (factoryId != null)
            {
                query.Where(x => x.TreePId == factoryId);
            }

            var enterprises = query
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return enterprises;
        }

        /// <summary>
        /// 根据用户ID和部门名称或编码过滤用户有权限的业务部门列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="factoryId">工厂ID</param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetUserBussinessDepartments(double userId, PagingInfo pagingInfo, string keyword,
            double? factoryId = null)
        {
            //检查用户预算部门权限
            var query = Query<Enterprise>()
                .Join<DeviceUseDepartment>((x, y) => x.Id == y.EnterpriseId)
                .Join<DeviceUseDepartment, DevicePur>((a, b) => a.DevicePurId == b.Id)
                .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == userId || c.UserId == userId))
                .WhereIf(keyword.IsNotEmpty(), x => x.Code.Contains(keyword) || x.Name.Contains(keyword));

            if (factoryId != null)
            {
                query.Where(x => x.TreePId == factoryId);
            }

            var enterprises = query
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return enterprises;
        }

        /// <summary>
        /// 根据用户ID获取有权限的采购对象
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>采购对象</returns>
        public virtual IList<PurchaseObjectType> GetUserPurchaseObjects(double userId)
        {
            var list = Query<DevicePurchaseObjectType>()
                .Join<DevicePur>((x, y) => x.DevicePurId == y.Id)
                .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == userId || c.UserId == userId))
                .Select(p => p.PurchaseObjectType).ToList<PurchaseObjectType>();
            return list;
        }

        /// <summary>
        /// 设备与人员权限维护-设备类型-设备类别和设备类型是否已经存在
        /// </summary>
        /// <param name="deviceType">设备类型</param>
        /// <returns></returns>
        public virtual bool IsDeviceTypeDuplicate(DeviceType deviceType)
        {
            var query = Query<DeviceType>()
                .Where(x => x.Id != deviceType.Id
                    && x.DevicePurId == deviceType.DevicePurId
                    && x.TypeCategory == deviceType.TypeCategory
                    && x.EquipTypeId == deviceType.EquipTypeId);

            return query.Count() > 0;
        }

        /// <summary>
        /// 保存设备与人员权限的设备清单
        /// </summary>
        /// <param name="devicePurInfos">设备清单信息列表</param>
        public virtual void SaveDeviceBills(List<DevicePurInfo> devicePurInfos)
        {
            if (devicePurInfos == null)
            {
                return;
            }
            var equipAccountIds = devicePurInfos.Select(x => x.DevicePurId).ToList();
            var devicePurId = devicePurInfos.First().SourceId;

            var deviceBills = equipAccountIds.SplitContains((tmpIds) =>
            {
                return Query<DeviceBill>()
                    .Where(p => p.DevicePurId == devicePurId && tmpIds.Contains(p.EquipAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            var existsEquipAccountIds = deviceBills.Select(x => x.EquipAccountId).Distinct().ToList();
            var dictionaryOfExistsEquipAccountIds = existsEquipAccountIds.ToDictionary(x => x);

            var savedData = new EntityList<DeviceBill>();

            foreach (var item in devicePurInfos)
            {
                if (dictionaryOfExistsEquipAccountIds.ContainsKey(item.DevicePurId))
                {
                    continue;
                }

                var deviceBill = new DeviceBill();
                deviceBill.DevicePurId = item.SourceId;
                deviceBill.EquipAccountId = item.DevicePurId;
                savedData.Add(deviceBill);
            }

            if (savedData.Any())
            {
                RF.Save(savedData);
            }
        }
    }
}
