using Castle.MicroKernel.Registration;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Tech.Processs;
using SIE.Items;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Resources;
using SIE.Resources.Employees;
using SIE.Resources.Skills;
using SIE.Resources.UserGroups;
using SIE.Tech.Processs.Models;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Employee = SIE.Resources.Employees.Employee;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序控制器
    /// </summary>
    public class ProcessController : DomainController, IProcess
    {

        #region 用户组与工序

        public virtual void SelectSaveProcessUserGroup(List<ProcessUserGroup> processUserGroupList)
        {
            EntityList<ProcessUserGroup> savedData = new EntityList<ProcessUserGroup>();

            foreach (var item in processUserGroupList)
            {
                var processUserGroup = new ProcessUserGroup();
                processUserGroup.UserGroupId = item.UserGroupId;
                processUserGroup.ProcessId = item.ProcessId;
                savedData.Add(processUserGroup);
            }

            //根据用户组 ，找出需要添加的员工
            var userGroupId = savedData.FirstOrDefault().UserGroupId;
            var employees = DB.Query<Employee>().Join<User>((x, y) => x.Id == y.EmployeeId).Join<User, UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupId).ToList();

            //找出现在每个员工都拥有的资源，后面可用于判断是否已经存在了，防止重复添加
            var employeeIds = employees.Select(p => p.Id).Distinct().ToList();
            var processEmployees = Query<ProcessEmployee>().Where(p => employeeIds.Contains((double)p.EmployeeId)).ToList();

            EntityList<ProcessEmployee> ees = new EntityList<ProcessEmployee>();

            //开始循环判断是否 该员工是否存在该工厂
            foreach (var employee in employees)
            {
                foreach (var sd in savedData)
                {
                    if (processEmployees.Any(p => p.EmployeeId == employee.Id && p.ProcessId == sd.ProcessId))
                    { }
                    else
                    {
                        ProcessEmployee ee = new ProcessEmployee();
                        ee.PersistenceStatus = PersistenceStatus.New;
                        ee.EmployeeId = employee.Id;
                        ee.ProcessId = sd.ProcessId;
                        ees.Add(ee);
                    }
                }
            }


            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                RF.Save(savedData);

                if (ees.Count > 0)
                    RF.Save(ees);

                tran.Complete();
            }
        }

        /// <summary>
        /// 删除用户组与工序关系，同步删除用户与工序关系
        /// </summary>
        /// <param name="processUserGroups"></param>
        public virtual void DeleteProcessUserGroupSyncEmployee(EntityList<ProcessUserGroup> processUserGroups)
        {
            var userGroupId = processUserGroups.FirstOrDefault().UserGroupId;
            //根据资源ID + 用户组Id获取员工对应的资源，然后删掉
            var processIds = processUserGroups.Select(p => p.ProcessId).Distinct().ToList();
            var processEmployees = Query<ProcessEmployee>().Join<Employee>((x, y) => x.EmployeeId == y.Id).Join<Employee, User>((x, y) => x.Id == y.EmployeeId).Join<User, UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupId).Where(p => processIds.Contains(p.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var processEmployeeIds = processEmployees.Select(p => p.Id).Distinct().ToList();
            var employeeIds = processEmployees.Select(p => p.EmployeeId).Distinct().ToList();
            //找出其他用户组存在的相同工序
            var notprocessEmployeeIds = Query<ProcessEmployee>().Exists<ProcessUserGroup>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.EmployeeId == x.EmployeeId).Where(p => p.ProcessId == x.ProcessId && processIds.Contains(p.ProcessId) && p.UserGroupId != userGroupId)).Where(p => employeeIds.Contains(p.EmployeeId)).Select(p => p.Id).ToList<double>().ToList();
            //找出存在processEmployeeIds中，但是不存在notprocessEmployeeIds中的数据，然后删除
            processEmployeeIds = processEmployeeIds.Except(notprocessEmployeeIds).ToList();

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                if (processEmployeeIds.Count > 0)
                    DB.Delete<ProcessEmployee>().Where(p => processEmployeeIds.Contains(p.Id)).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 用户组删除用户，同步到员工
        /// </summary>
        /// <param name="UserInUserGroupId"></param>
        public virtual void DeleteUserInUserGroupSyncEmployeeProcess(double userId,double userGroupId)
        {
            //删除员工与资源
            var processEmployees = Query<ProcessEmployee>().Join<User>((x, y) => x.EmployeeId == y.EmployeeId && y.Id == userId).Join<ProcessUserGroup>((x, y) => x.ProcessId == y.ProcessId && y.UserGroupId == userGroupId).ToList();

            var processEmployeeIds = processEmployees.Select(p => p.Id).Distinct().ToList();
            var employeeIds = processEmployees.Select(p => p.EmployeeId).Distinct().ToList();

            var processIds = Query<ProcessUserGroup>().Where(p => p.UserGroupId == userGroupId).Select(p => p.ProcessId).Distinct().ToList<double?>().ToList();

            //找出其他用户组存在的相同工序
            var notprocessEmployeeIds =
                processIds.SplitContains(pids =>
                {
                    return employeeIds.SplitContains(eids =>
                    {
                        return Query<ProcessEmployee>().Exists<ProcessUserGroup>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.EmployeeId == x.EmployeeId).Where(p => p.ProcessId == x.ProcessId && pids.Contains(p.ProcessId) && p.UserGroupId != userGroupId)).Where(p => eids.Contains(p.EmployeeId)).ToList();
                    });
                }).Select(p => p.Id).Distinct().ToList();
            //找出存在processEmployeeIds中，但是不存在notprocessEmployeeIds中的数据，然后删除
            processEmployeeIds = processEmployeeIds.Except(notprocessEmployeeIds).ToList();

            if (processEmployeeIds.Count > 0)
                DB.Delete<ProcessEmployee>().Where(p => processEmployeeIds.Contains(p.Id)).Execute();

        }

        /// <summary>
        /// 用户组新增用户，同步到员工
        /// </summary>
        /// <param name="UserInUserGroupId"></param>
        public virtual void InsertUserInUserGroupSyncEmployeeProcess(double UserInUserGroupId)
        {
            var userInUserGroup = RF.GetById<UserInUserGroup>(UserInUserGroupId, new EagerLoadOptions().LoadWithViewProperty());
            //找出员工工序明细不存在的工厂数据
            var processUserGroups = Query<ProcessUserGroup>().Where(p => p.UserGroupId == userInUserGroup.UserGroupId).NotExists<ProcessEmployee>((x, y) => y.Join<User>((x1, y1) => x1.EmployeeId == y1.EmployeeId && y1.Id == userInUserGroup.UserId).Where(p => p.ProcessId == x.ProcessId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var processUserGroup in processUserGroups)
            {
                ProcessEmployee processEmployee = new ProcessEmployee();
                processEmployee.ProcessId = processUserGroup.ProcessId;
                processEmployee.EmployeeId = userInUserGroup.User.EmployeeId;
                RF.Save(processEmployee);
            }
        }

        /// <summary>
        /// 根据用户组Id获取用户组与工序关系
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <returns></returns>
        public virtual EntityList<ProcessUserGroup> GetProcessUserGroupsByUserGroupId(double UserGroupId,PagingInfo pagingInfo)
        {
            var list = Query<ProcessUserGroup>().Where(p => p.UserGroupId == UserGroupId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        /// <summary>
        /// 获取工序
        /// </summary>
        /// <param name="name">工序名称</param>
        /// <returns>工序</returns>
        public virtual Process GetProcess(string name)
        {
            return Query<Process>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 根据编码获取工序
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcessesList(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<Process>().Where(p => c.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }
        /// <summary>
        /// 获取多个库存组织下的工序
        /// </summary>
        /// <param name="invOrgs"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual Dictionary<string, EntityList<Process>> GetProcessesListByInvOrg(List<string> invOrgs,List<string> codes)
        {
            Dictionary<string, EntityList<Process>> pairs = new Dictionary<string, EntityList<Process>>();
            var invOrgList = Query<InvOrg>().Where(p => invOrgs.Contains(p.ExternalId)).ToList();
            var invOrgCodes = invOrgList.Select(p => p.Code).ToList();
            var currInvOrg = RT.InvOrg;
            foreach (var item in invOrgList)
            {
                RT.InvOrg = item.Code;
                var list = codes.SplitContains(c =>
                {
                        return Query<Process>().Where(p => c.Contains(p.Code)).ToList();
                });
                pairs.Add(item.ExternalId.ToString(), list);
            }
            RT.InvOrg = currInvOrg;
            return pairs;
        }

        /// <summary>
        /// 查询工序列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcessList(ProcessCriteria criteria)
        {
            var query = Query<Process>();
            if (criteria.Code.IsNotEmpty()) {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.Type.HasValue)
            {
                query.Where(p => p.Type==criteria.Type.Value);
            }
            if (criteria.ProductFamilyId.HasValue)
            {
                query.Where(p => p.ProductFamilyId == criteria.ProductFamilyId);
            }
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取所有工序
        /// </summary> 
        /// <returns>所有工序</returns>
        public virtual EntityList<Process> GetProcess()
        {
            return Query<Process>().ToList();
        }

        /// <summary>
        /// 获取工序
        /// </summary>
        /// <param name="code">工序编码</param>
        /// <returns>工序</returns>
        public virtual Process GetProcessByCode(string code)
        {
            return Query<Process>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 根据工序id获取工序参数总数
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <returns>工序参数总数</returns>
        public virtual int GetProcessParameterList(double processId)
        {
            return Query<ProcessParameter>().Where(p => p.ProcessId == processId).Count();
        }

        /// <summary>
        /// 获取工序列表
        /// </summary>
        /// <param name="processIds">工序id集合</param>
        /// <param name="loadViewProperty">是否加视图属性</param>
        /// <exception cref="ArgumentNullException">工序Id列表不能为空</exception>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcessByIds(List<double> processIds, bool loadViewProperty = true)
        {
            Check.NotNull(processIds, "工序Id列表不能为空".L10N());
            return processIds.SplitContains(tempIds =>
            {
                if (loadViewProperty)
                {
                    return Query<Process>()
                        .Where(p => tempIds.Contains(p.Id))
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                }

                return Query<Process>()
                    .Where(p => tempIds.Contains(p.Id))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取工序名称集合
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        public virtual List<ProcessIdName> GetProcessIdNames(List<double> processIds)
        {
            List<ProcessIdName> processIdNames = new List<ProcessIdName>();
            processIds.SplitDataExecute(tempIds =>
            {
                var list = Query<Process>().Where(p => tempIds.Contains(p.Id)).Select(p => new
                {
                    ProcessId = p.Id,
                    ProcessName = p.Name,
                }).ToList<ProcessIdName>();
                processIdNames.AddRange(list);
            });
            return processIdNames;
        }

        /// <summary>
        /// 用产品小族获取工序
        /// </summary>
        /// <param name="categoryId">产品小族ID</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcessListByCategory(double categoryId, string keyWord, PagingInfo pagingInfo)
        {
            var query = Query<Process>().Where(p => p.ProductFamilyId == categoryId);
            if (!keyWord.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(keyWord));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 用产品小族获取工序
        /// </summary>
        /// <param name="categoryId">产品小族ID</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcessListByProductFamilyCategory(double categoryId,
            string keyWord, PagingInfo pagingInfo)
        {
            var query = Query<Process>()
                .Join<ProductFamily>((x, y) => x.ProductFamilyId == y.Id)
                .Where<ProductFamily>((x, y) => y.CategoryId == categoryId);

            if (!keyWord.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(keyWord));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过工序类型获取工序
        /// </summary>
        /// <param name="ProcessTypes">工序类型</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <exception cref="ArgumentNullException">工序类型列表不能为空</exception>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcessByType(List<int> ProcessTypes, string keyWord, PagingInfo pagingInfo)
        {
            Check.NotNull(ProcessTypes, "工序类型列表不能为空".L10N());
            return Query<Process>().Where(p => ProcessTypes.Contains((int)p.Type)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过工序类型获取工序
        /// </summary>
        /// <param name="ProcessTypes">工序类型</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <exception cref="ArgumentNullException">工序类型列表不能为空</exception>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcessBy(string invCode, string keyWord, PagingInfo pagingInfo)
        {
            var invOrgCurr = RT.InvOrg;
            var invOrg = Query<InvOrg>().Where(p => p.ExternalId == invCode).FirstOrDefault();
            if(invOrg==null)
                throw new ValidationException("库存组织不正确,请确定【{0}】！！！;".L10nFormat(invCode));
            RT.InvOrg = Convert.ToInt32(invOrg.Code);
            var entityList = Query<Process>().Where(p => p.Code.Contains(keyWord)||p.Name.Contains(keyWord))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            RT.InvOrg = invOrgCurr;
            return entityList;
        }

        /// <summary>
        /// 获取工序列表
        /// </summary>       
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcessList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Process>();
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(keyword)||p.Code.Contains(keyword));
            return query.ToList(pagingInfo,new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取员工与工序
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="pagingInfo"></param>
        /// <returns>员工与工序列表</returns>
        public virtual EntityList<ProcessEmployee> GetProcessEmployees(double employeeId, PagingInfo pagingInfo=null)
        {
            return Query<ProcessEmployee>().Where(p => p.EmployeeId == employeeId).ToList(pagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过员工Id列表获取工序与员工列表
        /// </summary>
        /// <param name="employeeIds">员工Id列表</param>
        /// <exception cref="ArgumentNullException">员工Id列表不能为空</exception>
        /// <returns>工序与员工列表</returns>
        public virtual EntityList<ProcessEmployee> GetProcessEmployees(List<double> employeeIds)
        {
            Check.NotNull(employeeIds, "员工Id列表不能为空".L10N());
            var ids = employeeIds.Cast<double?>().ToList();
            return Query<ProcessEmployee>().Where(p => ids.Contains(p.EmployeeId)).ToList(null, new EagerLoadOptions().LoadWith(ProcessEmployee.ProcessProperty));
        }

        /// <summary>
        /// 通过员工ID取员工关联工序列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>工序列表</returns>
        [ApiService("通过员工ID取员工关联工序列表")]
        [return: ApiReturn("工序列表. 参数类型: EntityList<Process>")]
        public virtual EntityList<Process> GetProcessByEmployeeId([ApiParameter("员工ID")] double employeeId)
        {
            return Query<Process>()
                .Join<ProcessEmployee>((x, y) => x.Id == y.ProcessId && y.EmployeeId == employeeId)
                .ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过员工ID取员工关联工序列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>工序列表</returns>
        [ApiService("通过员工ID取员工关联工序列表")]
        [return: ApiReturn("工序列表. 参数类型: EntityList<Process>")]
        public virtual EntityList<Process> GetProcessListByEmployeeId([ApiParameter("员工ID")] double employeeId)
        {
            var processList = Query<Process>()
                .Join<ProcessEmployee>((x, y) => x.Id == y.ProcessId && y.EmployeeId == employeeId)
                .ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());

            if (processList.Count <= 0)
                processList = GetProcessList(null, null);

            return processList;
        }

        /// <summary>
        /// 获取当前登录员工的工序
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Process> GetEmployeeProcessList(PagingInfo pagingInfo, string keyword)
        {
            return Query<Process>()
                .Join<ProcessEmployee>((x, y) => y.ProcessId == x.Id && y.EmployeeId == RT.IdentityId)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 判断员工是否有工序权限
        /// </summary>
        /// <param name="employeeId">用户ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>bool</returns>
        public virtual bool EmployeeHasProcess(double employeeId, double processId)
        {
            var q = Query<ProcessEmployee>();
            q.Where(p => p.EmployeeId == employeeId && p.ProcessId == processId);
            return q.Count() > 0;
        }

        /// <summary>
        /// 获取包装单位与工序
        /// </summary>
        /// <param name="packingUnitId">包装单位ID</param>
        /// <returns>包装单位与工序列表</returns>
        public virtual EntityList<ProcessPackingUnit> GetProcessPackingUnitsByUnitId(double packingUnitId)
        {
            return Query<ProcessPackingUnit>().Where(p => p.PackageUnitId == packingUnitId).ToList();
        }

        /// <summary>
        /// 获取包装单位与工序
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <returns>包装单位与工序列表</returns>
        public virtual EntityList<ProcessPackingUnit> GetProcessPackingUnitsByProcessId(double processId)
        {
            return Query<ProcessPackingUnit>().Where(p => p.ProcessId == processId).ToList();
        }

        /// <summary>
        /// 获取包装单位与工序
        /// </summary>
        /// <param name="processIds">工序ID列表</param>
        /// <returns>包装单位与工序列表</returns>
        public virtual EntityList<ProcessPackingUnit> GetProcessPackingUnits(List<double> processIds)
        {
            return processIds.SplitContains(tempIds =>
            {
                return Query<ProcessPackingUnit>().Where(p => tempIds.Contains(p.ProcessId)).ToList();
            });
        }

        /// <summary>
        /// 判断工序是否有包装权限
        /// </summary>
        /// <param name="packingUnitId">包装单位ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>bool</returns>
        public virtual bool PackingUnitHasProcess(double packingUnitId, double processId)
        {
            var q = Query<ProcessPackingUnit>();
            q.Where(p => p.ProcessId == processId);
            return q.Count() == 0 || q.Where(f => f.PackageUnitId == packingUnitId).Count() > 0;
        }

        /// <summary>
        /// 判断工序是否有包装权限
        /// </summary>
        /// <param name="packingUnitId">包装单位ID</param>
        /// <param name="process">工序</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool PackingUnitHasProcess(double packingUnitId, Process process)
        {
            return !process.ProcessPackingUnitList.Any()
                || process.ProcessPackingUnitList.Any(f => f.PackageUnitId == packingUnitId);
        }

        /// <summary>
        /// 根据登陆用户获取工序列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="name">工序名称</param>
        /// <param name="types">工序类型</param>
        /// <exception cref="ArgumentNullException">工序类型列表不能为空</exception>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcesssByUserId(double employeeId, string name, List<ProcessType> types)
        {
            Check.NotNull(types, "工序类型列表不能为空".L10N());
            var processTypes = types.Select(e => (int)e).ToList();
            var query = Query<Process>().Exists<ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == employeeId ));
            if (name.IsNotEmpty())
                query.Where(p => p.Name.Contains(name));
            if(processTypes.Count> 0)
                query.Where(p => processTypes.Contains((int)p.Type));
            return query.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcesssByTypes(double employeeId, string name, List<int> types)
        {
            Check.NotNull(types, "工序类型列表不能为空".L10N());
            var query = Query<Process>().Exists<ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == employeeId && types.Contains((int)a.Type)));
            if (name.IsNotEmpty())
                query.Where(p => p.Name.Contains(name));
            return query.ToList();
        }

        /// <summary>
        /// 根据登陆用户获取工序列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="name"></param>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetProcesssByUserId(double employeeId,string name)
        {
            var query = Query<Process>().Exists<ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == employeeId));
            if (name.IsNotEmpty())
                query.Where(p => p.Name.Contains(name));
            return query.ToList();
        }

        /// <summary>
        /// 获取点检工位列表
        /// </summary>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetCheckStations()
        {
            return Query<Station>().OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// 获取产线下所有工位列表
        /// </summary>
        /// <param name="resourceId">产线ID</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetResourceStations(double resourceId)
        {
            return Query<Station>().Where(p => p.ResourceId == resourceId).ToList();
        }

        /// <summary>
        /// 根据产线和工序获取工位列表
        /// </summary>
        /// <param name="reourceId">产线ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStationsByResourceId(double reourceId, double processId, PagingInfo pagingInfo = null)
        {
            return Query<Station>().Join<StationProcess>((s, p) => s.Id == p.StationId && p.ProcessId == processId).Where(p => p.ResourceId == reourceId).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取工位列表
        /// </summary> 
        /// <param name="resourceId">产线Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> GetStationList(double? resourceId, double? processId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Station>();
            if (processId.HasValue)
                query.Join<StationProcess>((s, p) => s.Id == p.StationId && p.ProcessId == processId);
            if (resourceId.HasValue)
                query.Where(p => p.ResourceId == resourceId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取工序技能集合
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <returns>工序技能集合</returns>
        public virtual EntityList<ProcessSkill> GetProcessSkillList(double processId)
        {
            return Query<ProcessSkill>().Where(p => p.ProcessId == processId && p.IsCheck).ToList();
        }

        /// <summary>
        /// 通过工序Id列表获取技能列表
        /// </summary>
        /// <param name="processIds">工序Id列表</param>
        /// <returns>技能列表</returns>
        public virtual EntityList<ProcessSkill> GetProcessSkills(List<double> processIds)
        {
            return Query<ProcessSkill>().Where(p => processIds.Contains(p.ProcessId)&& p.IsCheck).ToList(null, new EagerLoadOptions().LoadWith(ProcessSkill.SkillProperty));
        }

        /// <summary>
        /// 通过工序Id获取采集步骤列表
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <returns>采集步骤列表</returns>
        public virtual EntityList<ProcessCollectStep> GetProcessCollectSteps(double processId)
        {
            return Query<ProcessCollectStep>().Where(p => p.ProcessId == processId).ToList();
        }

        /// <summary>
        /// 获取界面实际的工序采集步骤列表
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="modifyCollectSteps">修改的工序采集步骤列表</param>
        /// <returns>工序采集步骤列表</returns>
        public virtual EntityList<ProcessCollectStep> GetProcessCollectSteps(double processId, EntityList<ProcessCollectStep> modifyCollectSteps)
        {
            EntityList<ProcessCollectStep> processCollectSteps = new EntityList<ProcessCollectStep>();
            var deletedCollectSteps = modifyCollectSteps.DeletedList.OfType<ProcessCollectStep>().ToList();
            var oldCollectSteps = RT.Service.Resolve<ProcessController>().GetProcessCollectSteps(processId);
            processCollectSteps.AddRange(modifyCollectSteps);
            foreach (var oldCollectStep in oldCollectSteps)
            {
                if (deletedCollectSteps.Any(p => p.Id == oldCollectStep.Id)) continue;
                var newCollectStep = processCollectSteps.FirstOrDefault(p => p.Id == oldCollectStep.Id && p.Id != 0);
                if (newCollectStep == null)
                    processCollectSteps.Add(oldCollectStep);
            }

            return processCollectSteps;
        }

        /// <summary>
        /// 通过工序Id获取工序参数列表
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <returns></returns>
        public virtual EntityList<ProcessParameter> GetProcessParameters(double processId)
        {
            return Query<ProcessParameter>().Where(p => p.ProcessId == processId).ToList();
        }

        /// <summary>
        /// 获取界面实际的工序参数列表
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="modifyParameters">修改的工序参数列表</param>
        /// <returns>实际的工序参数列表</returns>
        public virtual EntityList<ProcessParameter> GetProcessParameters(double processId, EntityList<ProcessParameter> modifyParameters)
        {
            EntityList<ProcessParameter> processParameters = new EntityList<ProcessParameter>();
            var oldProcessParameters = RT.Service.Resolve<ProcessController>().GetProcessParameters(processId);
            var deletedParameters = modifyParameters.DeletedList.OfType<ProcessParameter>().ToList();
            processParameters.AddRange(modifyParameters);
            foreach (var oldProcessParameter in oldProcessParameters)
            {
                if (deletedParameters.Any(p => p.Id == oldProcessParameter.Id)) continue;
                var newParameter = processParameters.FirstOrDefault(p => p.Id == oldProcessParameter.Id && p.Id != 0);
                if (newParameter == null)
                    processParameters.Add(oldProcessParameter);
            }

            return processParameters;
        }

        /// <summary>
        /// 根据工序ID获取工序参数
        /// </summary>
        /// <param name="processIds">工序ID集合</param>
        /// <returns>工序参数列表</returns>
        public virtual EntityList<ProcessParameter> GetProcessParameterByProcessId(double[] processIds)
        {
            return Query<ProcessParameter>().Where(p => processIds.Contains(p.ProcessId)).ToList();
        }

        /// <summary>
        /// 根据工序ID获取工序参数
        /// </summary>
        /// <param name="processIds">工序ID集合</param>
        /// <returns>工序参数列表</returns>
        public virtual EntityList<ProcessParameter> GetProcessParameterByProcessId(IEnumerable<double> processIds)
        {
            return processIds.SplitContains(tempIds => { return Query<ProcessParameter>().Where(p => tempIds.Contains(p.ProcessId)).ToList(); });
        }

        /// <summary>
        /// 根据工序Id集合获取工序参数
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>

        public virtual Dictionary<double, List<ProcessParameter>> GetProcessParameterByProcessIds(List<double> processIds)
        {
            var resultList = processIds.SplitContains(ids =>
            {
                return Query<ProcessParameter>().Where(p => ids.Contains(p.ProcessId)).ToList();
            });
            return resultList.GroupBy(p => p.ProcessId).ToDictionary(p => p.Key, p => p.ToList());
        }

        /// <summary>
        /// 判断员工是否具有工序所要求的技能
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <param name="empId">员工Id</param>
        /// <returns>bool</returns>
        public virtual bool IsEmpHasProcessSkill(double processId, double empId)
        {
            var needSkillList = GetProcessSkillList(processId);
            if (needSkillList.Count > 0)
            {
                var empSkillList = RT.Service.Resolve<SkillController>().GetEmployeeSkill(empId);
                var empSkillListIds = empSkillList.Select(p => p.SkillId).ToList();
                if (needSkillList.Any(p => !empSkillListIds.Contains(p.SkillId)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取工序缺陷信息
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<Defect> GetProcessDefects(double processId)
        {
            return Query<Defect>().Join<ProcessDefect>((d, p) => d.Id == p.DefectId && p.ProcessId == processId).ToList();
        }

        /// <summary>
        /// 获取工工步信息
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="keyWord">工序ID</param>
        /// <param name="pagingInfo">工序ID</param>
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<WorkStep> GetWorkSteps(double processId, string keyWord = null, PagingInfo pagingInfo = null)
        {
            return Query<WorkStep>()
                .Where(t => t.ProcessId == processId)
                .WhereIf(keyWord.IsNotEmpty(), p => p.Code.Contains(keyWord) || p.Name.Contains(keyWord))
                .OrderBy(t => t.SeqNumber)
                .ToList(pagingInfo);
        }

        /// <summary>
        /// 获取工工步信息
        /// </summary>
        /// <param name="processIds">工序ID列表</param>
        /// <returns>工步列表</returns>
        public virtual EntityList<WorkStep> GetWorkSteps(List<double> processIds)
        {
            return processIds.SplitContains(tempIds =>
                {
                    return Query<WorkStep>().Where(t => tempIds.Contains(t.ProcessId)).ToList();
                });
        }

        /// <summary>
        /// 获取工工步信息
        /// </summary>
        /// <param name="workStepIds">工步ID列表</param>
        /// <returns>工步列表</returns>
        public virtual EntityList<WorkStep> GetWorkStepsByIds(List<double> workStepIds)
        {
            return workStepIds.SplitContains(tempIds =>
            {
                return Query<WorkStep>().Where(t => tempIds.Contains(t.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取工工步信息
        /// </summary>
        /// <param name="code">工步编码</param>
        /// <param name="name">工步名称</param>
        /// <returns>缺陷代码列表</returns>
        public virtual bool IsWorkStepExists(string code, string name)
        {
            return Query<WorkStep>().Where(t => t.Code == code && t.Name == name).ToList().Any();
        }

        /// <summary>
        /// 判断工序是否启用入站控制
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual bool? ProcessIsMoveIn(double processId)
        {
            var process = RF.GetById<Process>(processId);
            return process.EnableMoveInControl;
        }

        /// <summary>
        /// 根据工序名称获取工序名称-Id字典
        /// </summary>
        /// <param name="processNames">工序名称</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetProcessNameIdDic(IEnumerable<string> processNames)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            processNames.SplitDataExecute(temps =>
            {
                var list = Query<Process>().Where(p => temps.Contains(p.Name)).Select(p => new
                {
                    Name = p.Name,
                    Id = p.Id,
                }).ToList<BaseDataInfo>();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos.ToDictionary(p => p.Name, p => p.Id);
        }

        public virtual Process GetPgProcess(string name)
        {
            return Query<Process>().Where(p => p.Name == name).FirstOrDefault();
        }
    }
}