using Irony;
using Org.BouncyCastle.Asn1.X509;
using SIE.Common.Users;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.EventMessages.Tech.Processs;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using User = SIE.Rbac.Users.User;

namespace SIE.Resources.UserGroups
{
    public class UserGroupsController : DomainController
    {
        /// <summary>
        /// 创建操作日志
        /// </summary>
        /// <param name="userGroup"></param>
        /// <param name="type"></param>
        /// <param name="state"></param>
        /// <param name="operateData"></param>
        public virtual void CreateUserGroupLog(UserGroup userGroup, UserGroupLogType type, UserGroupLogState state,string operateData)
        {
            UserGroupLog userGroupLog = new UserGroupLog();

            userGroupLog.PersistenceStatus = PersistenceStatus.New;
            userGroupLog.UserGroupId = userGroup.Id;
            userGroupLog.UserGroup = userGroup.Name + $"[{userGroup.Code}]";
            userGroupLog.Type = type;
            userGroupLog.State = state;
            userGroupLog.OperateData = operateData;
            RF.Save(userGroupLog);
        }

        /// <summary>
        /// 删除用户组与工厂关系，同步删除用户与工厂关系
        /// </summary>
        /// <param name="userGroupEnterprises"></param>
        public virtual void DeleteUserGroupEnterpriseSyncEmployee(EntityList<UserGroupEnterprise> userGroupEnterprises)
        {
            var userGroupId = userGroupEnterprises.FirstOrDefault().UserGroupId;
            //根据资源ID + 用户组Id获取员工对应的资源，然后删掉
            var enterpriseIds = userGroupEnterprises.Select(p => p.EnterpriseId).Distinct().ToList();

            var employeeEnterprises = Query<EmployeeEnterprise>().Join<Employee>((x, y) => x.EmployeeId == y.Id).Join<Employee, User>((x, y) => x.Id == y.EmployeeId).Join<User, UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupId).Where(p => enterpriseIds.Contains(p.EnterpriseId)).ToList(null,new EagerLoadOptions().LoadWithViewProperty());
            var employeeEnterpriseIds = employeeEnterprises.Select(p => p.Id).Distinct().ToList();
            var employeeIds = employeeEnterprises.Select(p => p.EmployeeId).Distinct().ToList();

            var notEmployeeEnterpriseIds = Query<EmployeeEnterprise>().Exists<UserGroupEnterprise>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.EmployeeId == x.EmployeeId).Where(p => p.EnterpriseId == x.EnterpriseId && enterpriseIds.Contains(p.EnterpriseId) && p.UserGroupId != userGroupId)).Where(p => employeeIds.Contains(p.EmployeeId)).Select(p => p.Id).ToList<double>().ToList();

            employeeEnterpriseIds = employeeEnterpriseIds.Except(notEmployeeEnterpriseIds).ToList();

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                if (employeeEnterpriseIds.Count > 0)
                    DB.Delete<EmployeeEnterprise>().Where(p => employeeEnterpriseIds.Contains(p.Id)).Execute();
                tran.Complete();
            }
        }


        /// <summary>
        /// 删除用户组与资源关系，同步删除用户与资源关系
        /// </summary>
        /// <param name="userGroupResources"></param>
        public virtual void DeleteUserGroupResourceSyncEmployee(EntityList<UserGroupResource> userGroupResources)
        {
            var userGroupId = userGroupResources.FirstOrDefault().UserGroupId;
            //根据资源ID + 用户组Id获取员工对应的资源，然后删掉
            var resourceIds = userGroupResources.Select(p => p.ResourceId).Distinct().ToList();

            var employeeResources = Query<EmployeeResource>().Join<Employee>((x, y) => x.EmployeeId == y.Id).Join<Employee, User>((x, y) => x.Id == y.EmployeeId).Join<User, UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupId).Where(p => resourceIds.Contains(p.ResourceId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var employeeResourceIds = employeeResources.Select(p => p.Id).Distinct().ToList();
            var employeeIds = employeeResources.Select(p => p.EmployeeId).Distinct().ToList();

            var notEmployeeResourceIds = Query<EmployeeResource>().Exists<UserGroupResource>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.EmployeeId == x.EmployeeId).Where(p => p.ResourceId == x.ResourceId && resourceIds.Contains(p.ResourceId) && p.UserGroupId != userGroupId)).Where(p => employeeIds.Contains(p.EmployeeId)).Select(p => p.Id).ToList<double>().ToList();

            employeeResourceIds = employeeResourceIds.Except(notEmployeeResourceIds).ToList();

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                if (employeeResourceIds.Count > 0)
                {
                    employeeResourceIds.SplitDataExecute(ids =>
                    {
                        DB.Delete<EmployeeResource>().Where(p => ids.Contains(p.Id)).Execute();
                    });
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 删除用户的时候同步员工资源、工厂、工序
        /// </summary>
        /// <param name="userInUserGroup"></param>
        public virtual void DeleteSyncEmployee(UserInUserGroup userInUserGroup)
        {
            //删除员工与资源
            var employeeResources = Query<EmployeeResource>().Join<User>((x, y) => x.EmployeeId == y.EmployeeId && y.Id == userInUserGroup.UserId).Join<UserGroupResource>((x, y) => x.ResourceId == y.ResourceId && y.UserGroupId == userInUserGroup.UserGroupId).ToList();

            var employeeResourceIds = employeeResources.Select(p => p.Id).Distinct().ToList();
            var employeeIds = employeeResources.Select(p => p.EmployeeId).Distinct().ToList();
            //找出想要删除的资源
            var resourceIds = Query<UserGroupResource>().Where(p => p.UserGroupId == userInUserGroup.UserGroupId).Select(p => p.ResourceId).Distinct().ToList<double>().ToList();
            //找出存在在其他用户组中的资源
            var notEmployeeResourceIds = resourceIds.SplitContains(rsid =>
            {
                return employeeIds.SplitContains(eids =>
                {
                    return Query<EmployeeResource>().Exists<UserGroupResource>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.EmployeeId == x.EmployeeId).Where(p => p.ResourceId == x.ResourceId && rsid.Contains(p.ResourceId) && p.UserGroupId != userInUserGroup.UserGroupId)).Where(p => eids.Contains(p.EmployeeId)).ToList();
                });
            }).Select(p => p.Id).Distinct().ToList();
            //存在与employeeResourceIds，不存在与notEmployeeResourceIds的，就是需要删除的
            employeeResourceIds = employeeResourceIds.Except(notEmployeeResourceIds).ToList();

            //删除员工与工厂
            var employeeEnterprises = Query<EmployeeEnterprise>().Join<User>((x, y) => x.EmployeeId == y.EmployeeId && y.Id == userInUserGroup.UserId).Join<UserGroupEnterprise>((x, y) => x.EnterpriseId == y.EnterpriseId && y.UserGroupId == userInUserGroup.UserGroupId).ToList();
            
            var employeeEnterpriseIds = employeeEnterprises.Select(p => p.Id).Distinct().ToList();
            employeeIds = employeeEnterprises.Select(p => p.EmployeeId).Distinct().ToList();
            //找出需要删除的工厂
            var enterpriseIds = Query<UserGroupEnterprise>().Where(p => p.UserGroupId == userInUserGroup.UserGroupId).Select(p => p.EnterpriseId).Distinct().ToList<double>().ToList();
            //查看需要删除的工厂，是否在其他用户组中存在
            var notEmployeeEnterpriseIds =
                enterpriseIds.SplitContains(epids =>
                {
                    return employeeIds.SplitContains(eids =>
                    {
                        return Query<EmployeeEnterprise>().Exists<UserGroupEnterprise>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.EmployeeId == x.EmployeeId).Where(p => p.EnterpriseId == x.EnterpriseId && epids.Contains(p.EnterpriseId) && p.UserGroupId != userInUserGroup.UserGroupId)).Where(p => eids.Contains(p.EmployeeId)).ToList();
                    });
                }).Select(p => p.Id).Distinct().ToList();
            //存在与employeeEnterpriseIds，不存在与notEmployeeEnterpriseIds的，就是需要删除的
            employeeEnterpriseIds = employeeEnterpriseIds.Except(notEmployeeEnterpriseIds).ToList();

            //删除用户与库存组织
            var userInInvOrgs = Query<UserInInvOrg>().Join<UserGroupInvOrg>((x, y) => x.InvOrgId == y.InvId && y.UserGroupId == userInUserGroup.UserGroupId).Where(p => p.UserId == userInUserGroup.UserId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var userInInvOrgIds = userInInvOrgs.Select(p => p.Id).Distinct().ToList();
            var invIds = userInInvOrgs.Select(p => p.InvOrgId).Distinct().ToList();
            var userIds = userInInvOrgs.Select(p => p.UserId).Distinct().ToList();

            var notuserInInvOrgIds =
                userIds.SplitContains(uids =>
                {
                    return Query<UserInInvOrg>().Exists<UserGroupInvOrg>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.Id == x.UserId).Where(p => p.InvId == x.InvOrgId && invIds.Contains(p.InvId) && p.UserGroupId != userInUserGroup.UserGroupId)).Where(p => uids.Contains(p.UserId)).ToList();
                }).Select(p => p.Id).Distinct().ToList();

            userInInvOrgIds = userInInvOrgIds.Except(notuserInInvOrgIds).ToList();


            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                if (employeeResourceIds.Count > 0)
                    DB.Delete<EmployeeResource>().Where(p => employeeResourceIds.Contains(p.Id)).Execute();
                if (employeeEnterpriseIds.Count > 0)
                    DB.Delete<EmployeeEnterprise>().Where(p => employeeEnterpriseIds.Contains(p.Id)).Execute();
                if (userInInvOrgIds.Count > 0)
                    DB.Delete<UserInInvOrg>().Where(p => userInInvOrgIds.Contains(p.Id)).Execute();
                //删除员工与工序
                RT.Service.Resolve<IProcess>().DeleteUserInUserGroupSyncEmployeeProcess(userInUserGroup.UserId, userInUserGroup.UserGroupId);

                tran.Complete();
            }
        }

        /// <summary>
        /// 同步用户组
        /// </summary>
        /// <param name="userInUserGroup"></param>
        public virtual void InsertSyncEmployee(UserInUserGroup userInUserGroup)
        {
            //找出员工资源明细不存在的资源数据
            var userGroupResources = Query<UserGroupResource>().Where(p => p.UserGroupId == userInUserGroup.UserGroupId).NotExists<EmployeeResource>((x, y) => y.Join<User>((x1, y1) => x1.EmployeeId == y1.EmployeeId && y1.Id == userInUserGroup.UserId).Where(p => p.ResourceId == x.ResourceId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //找出员工工厂明细不存在的工厂数据
            var userGroupEnterprises = Query<UserGroupEnterprise>().Where(p => p.UserGroupId == userInUserGroup.UserGroupId).NotExists<EmployeeEnterprise>((x, y) => y.Join<User>((x1, y1) => x1.EmployeeId == y1.EmployeeId && y1.Id == userInUserGroup.UserId).Where(p => p.EnterpriseId == x.EnterpriseId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //找出用户库存组织不存在的用户组与库存组织数据
            var userGroupInvOrgs = Query<UserGroupInvOrg>().Where(p => p.UserGroupId == userInUserGroup.UserGroupId).NotExists<UserInInvOrg>((x, y) => y.Where(p => p.InvOrgId == x.InvId && p.UserId == userInUserGroup.UserId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                //同步工厂数据
                RT.Service.Resolve<IProcess>().InsertUserInUserGroupSyncEmployeeProcess(userInUserGroup.Id);

                //创建员工资源关系
                foreach (var userGroupResource in userGroupResources)
                {
                    EmployeeResource employeeResource = new EmployeeResource();
                    employeeResource.ResourceId = userGroupResource.ResourceId;
                    employeeResource.EmployeeId = userInUserGroup.User.EmployeeId.Value;
                    RF.Save(employeeResource);
                }
                //创建员工与工厂关系
                foreach (var userGroupEnterprise in userGroupEnterprises)
                {
                    EmployeeEnterprise employeeEnterprise = new EmployeeEnterprise();
                    employeeEnterprise.EnterpriseId = userGroupEnterprise.EnterpriseId;
                    employeeEnterprise.EmployeeId = userInUserGroup.User.EmployeeId.Value;
                    RF.Save(employeeEnterprise);
                }
                //创建用户与库存组织关系
                foreach (var userGroupInvOrg in userGroupInvOrgs)
                {
                    UserInInvOrg newUserInInvOrg = new UserInInvOrg();
                    newUserInInvOrg.InvOrgId = userGroupInvOrg.InvId;
                    newUserInInvOrg.UserId = userInUserGroup.UserId;
                    newUserInInvOrg.IsInternal = false;
                    newUserInInvOrg.PersistenceStatus = PersistenceStatus.New;
                    RF.Save(newUserInInvOrg);
                }

                tran.Complete();
            }
        }

        #region 用户组与库存组织


        /// <summary>
        /// 删除用户组与库存组织关系，同步删除用户与库存组织关系
        /// </summary>
        /// <param name="userGroupResources"></param>
        public virtual void DeleteUserGroupInvOrgSyncUser(EntityList<UserGroupInvOrg> userGroupInvOrgs)
        {
            var userGroupId = userGroupInvOrgs.FirstOrDefault().UserGroupId;

            //获取需要操作的用户
            var users = Query<User>().Join<UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupInvOrgs.FirstOrDefault().UserGroupId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var userIds = users.Select(p => p.Id).Distinct().ToList();
            //根据资源ID + 用户组Id获取员工对应的库存组织，然后删掉
            var invOrgIds = userGroupInvOrgs.Select(p => p.InvId).Distinct().ToList();

            //获取用户与库存组织关系
            var userInInvOrgs = userIds.SplitContains(ids =>
            {
                return Query<UserInInvOrg>().Where(p => ids.Contains(p.UserId) && invOrgIds.Contains(p.InvOrgId)).ToList();
            });

            var userInInvOrgIds = userInInvOrgs.Select(p => p.Id).Distinct().ToList();
            var invIds = userGroupInvOrgs.Select(p => p.InvId).Distinct().ToList();

            var notuserInInvOrgIds = Query<UserInInvOrg>().Exists<UserGroupInvOrg>((x, y) => y.Join<UserInUserGroup>((x1, y1) => x1.UserGroupId == y1.UserGroupId).Join<UserInUserGroup, User>((x1, y1) => x1.UserId == y1.Id && y1.Id == x.UserId).Where(p => p.InvId == x.InvOrgId && invIds.Contains(p.InvId) && p.UserGroupId != userGroupId)).Where(p => userIds.Contains(p.UserId)).Select(p => p.Id).ToList<double>().ToList();

            userInInvOrgIds = userInInvOrgIds.Except(notuserInInvOrgIds).ToList();


            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                if (userInInvOrgs.Count > 0)
                {
                    userInInvOrgs.Where(p => userInInvOrgIds.Contains(p.Id)).ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                    RF.Save(userInInvOrgs);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存库存组织
        /// </summary>
        /// <param name="userGroupInvOrgs"></param>
        public virtual void SaveUserGroupInvOrg(List<UserGroupInvOrg> userGroupInvOrgs)
        {
            //获取需要操作的用户
            var users = Query<User>().Join<UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupInvOrgs.FirstOrDefault().UserGroupId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var userIds = users.Select(p => p.Id).Distinct().ToList();
            //获取用户与库存组织关系
            var userInInvOrgs = userIds.SplitContains(ids =>
            {
                return Query<UserInInvOrg>().Where(p => ids.Contains(p.UserId)).ToList();
            });

            EntityList<UserGroupInvOrg> newUserGroupInvOrgs = new EntityList<UserGroupInvOrg>();
            foreach (var userGroupInvOrg in userGroupInvOrgs)
            {
                UserGroupInvOrg newUserGroupInvOrg = new UserGroupInvOrg();
                newUserGroupInvOrg.UserGroupId = userGroupInvOrg.UserGroupId;
                newUserGroupInvOrg.InvId = userGroupInvOrg.InvId;
                newUserGroupInvOrg.PersistenceStatus = PersistenceStatus.New;
                newUserGroupInvOrgs.Add(newUserGroupInvOrg);
            }

            EntityList<UserInInvOrg> newUserInInvOrgs = new EntityList<UserInInvOrg>();
            foreach (var user in users)
            {
                foreach (var userGroupInvOrg in userGroupInvOrgs)
                {
                    //存在就跳过
                    if (userInInvOrgs.Any(p => p.UserId == user.Id && p.InvOrgId == userGroupInvOrg.InvId))
                        continue;

                    UserInInvOrg newUserInInvOrg = new UserInInvOrg();
                    newUserInInvOrg.InvOrgId = userGroupInvOrg.InvId;
                    newUserInInvOrg.UserId = user.Id;
                    newUserInInvOrg.IsInternal = false;
                    newUserInInvOrg.PersistenceStatus = PersistenceStatus.New;
                    newUserInInvOrgs.Add(newUserInInvOrg);
                }
            }

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                if (newUserGroupInvOrgs.Count > 0)
                    RF.Save(newUserGroupInvOrgs);
                if (newUserInInvOrgs.Count > 0)
                    RF.Save(newUserInInvOrgs);
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据用户组Id获取用户组与库存组织关系
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<UserGroupInvOrg> GetUserGroupInvOrgsByUserGroupId(double UserGroupId, PagingInfo pagingInfo)
        {
            var List = Query<UserGroupInvOrg>().Where(p => p.UserGroupId == UserGroupId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return List;
        }

        #endregion

        #region 用户组与资源

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGroupResourceList"></param>
        public virtual void SaveUserGroupResource(List<UserGroupResource> userGroupResourceList)
        {
            EntityList<UserGroupResource> savedData = new EntityList<UserGroupResource>();
            foreach (var item in userGroupResourceList)
            {
                var userGroupResource = new UserGroupResource();
                userGroupResource.ResourceId = item.ResourceId;
                userGroupResource.UserGroupId = item.UserGroupId;
                savedData.Add(userGroupResource);
            }

            //根据用户组 ，找出需要添加的员工
            var userGroupId = savedData.FirstOrDefault().UserGroupId;
            var employees = DB.Query<Employee>().Join<User>((x, y) => x.Id == y.EmployeeId).Join<User, UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupId).ToList();

            //找出现在每个员工都拥有的资源，后面可用于判断是否已经存在了，防止重复添加
            var employeeIds = employees.Select(p => p.Id).Distinct().ToList();
            //当查询数据查过50000条的时候就会报错
            var employeeResources = RT.Service.Resolve<CommonController>().CriteriaDatas<EmployeeResource>(p => employeeIds.Contains(p.EmployeeId));

            EntityList<EmployeeResource> ees = new EntityList<EmployeeResource>();

            //开始循环判断是否 该员工是否存在该工厂
            foreach (var employee in employees)
            {
                foreach (var sd in savedData)
                {
                    if (employeeResources.Any(p => p.EmployeeId == employee.Id && p.ResourceId == sd.ResourceId))
                    { }
                    else
                    {
                        EmployeeResource ee = new EmployeeResource();
                        ee.PersistenceStatus = PersistenceStatus.New;
                        ee.EmployeeId = employee.Id;
                        ee.ResourceId = sd.ResourceId;
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
        /// 根据用户组Id获取用户组与资源关系
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<UserGroupResource> GetUserGroupResourcesByUserGroupId(double UserGroupId,PagingInfo pagingInfo)
        {
            var list = Query<UserGroupResource>().Where(p => p.UserGroupId == UserGroupId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        #region 用户组与工厂


        /// <summary>
        /// 保存员工与工厂关系
        /// </summary>
        /// <param name="employeeEnterpriseList">员工与工厂关系</param>
        public virtual void SaveUserGroupEnterprise(List<UserGroupEnterprise> userGroupEnterpriseList)
        {
            EntityList<UserGroupEnterprise> savedData = new EntityList<UserGroupEnterprise>();
            Check.NotNullOrEmpty(userGroupEnterpriseList, nameof(userGroupEnterpriseList));

            if (null == userGroupEnterpriseList || userGroupEnterpriseList.Count == 0)
                throw new ArgumentNullException(nameof(userGroupEnterpriseList));

            foreach (var item in userGroupEnterpriseList)
            {
                var userGroupEnterprise = new UserGroupEnterprise();
                userGroupEnterprise.EnterpriseId = item.EnterpriseId;
                userGroupEnterprise.UserGroupId = item.UserGroupId;
                savedData.Add(userGroupEnterprise);
            }

            //根据用户组 ，找出需要添加的员工
            var userGroupId = savedData.FirstOrDefault().UserGroupId;
            var employees = Query<Employee>().Join<User>((x, y) => x.Id == y.EmployeeId).Join<User, UserInUserGroup>((x, y) => x.Id == y.UserId && y.UserGroupId == userGroupId).ToList();
            //找出现在每个员工都拥有的工厂，后面可用于判断是否已经存在了，防止重复添加
            var employeeIds = employees.Select(p => p.Id).Distinct().ToList();
            var employeeEnterprises = Query<EmployeeEnterprise>().Where(p => employeeIds.Contains(p.EmployeeId)).ToList();

            EntityList<EmployeeEnterprise> ees = new EntityList<EmployeeEnterprise>();

            //开始循环判断是否 该员工是否存在该工厂
            foreach (var employee in employees)
            {
                foreach (var sd in savedData)
                {
                    if (employeeEnterprises.Any(p => p.EmployeeId == employee.Id && p.EnterpriseId == sd.EnterpriseId))
                    { }
                    else
                    {
                        EmployeeEnterprise ee = new EmployeeEnterprise();
                        ee.PersistenceStatus = PersistenceStatus.New;
                        ee.EmployeeId = employee.Id;
                        ee.EnterpriseId = sd.EnterpriseId;
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
        /// 根据用户组Id获取用户组与工厂关系
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<UserGroupEnterprise> GetUserGroupEnterprisesByUserGroupId(double UserGroupId, PagingInfo pagingInfo)
        {
            var List = Query<UserGroupEnterprise>().Where(p => p.UserGroupId == UserGroupId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return List;
        }

        #endregion

    }
}
