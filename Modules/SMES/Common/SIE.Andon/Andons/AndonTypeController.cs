using Org.BouncyCastle.Crypto.Operators;
using SIE.Andon.Andons.Configs;
using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.ViewModels;
using SIE.Api;
using SIE.Common.Configs;
using SIE.Common.Organizations;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护控制器
    /// </summary>
    public class AndonTypeController : DomainController
    {
        #region 安灯类型维护查询方法
        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <param name="andonTypeCriterial"></param>
        /// <returns></returns>
        public virtual EntityList<AndonType> GetAndonTypes(AndonTypeCriterial andonTypeCriterial)
        {
            var query = Query<AndonType>();
            if (!andonTypeCriterial.AndonTypeCode.IsNullOrEmpty())
                query.Where(p => p.AndonTypeCode.Contains(andonTypeCriterial.AndonTypeCode));
            if (!andonTypeCriterial.AndonTypeName.IsNullOrEmpty())
                query.Where(p => p.AndonTypeName.Contains(andonTypeCriterial.AndonTypeName));
            if (andonTypeCriterial.AndonTypeClass.HasValue)
                query.Where(p => p.AndonTypeClass == andonTypeCriterial.AndonTypeClass);
            if (andonTypeCriterial.State.HasValue)
                query.Where(p => p.State == andonTypeCriterial.State);
            if (andonTypeCriterial.CreateTime.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= andonTypeCriterial.CreateTime.BeginValue);
            if (andonTypeCriterial.CreateTime.EndValue.HasValue)
                query.Where(p => p.CreateDate <= andonTypeCriterial.CreateTime.EndValue);
            return query.OrderBy(andonTypeCriterial.OrderInfoList).ToList(andonTypeCriterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 启用安灯类型维护
        /// <summary>
        /// 启用安灯类型维护
        /// </summary>
        /// <returns></returns>
        public virtual void EnableAndonType(List<double> andonIds)
        {
            var andonList = andonIds.SplitContains(tempIds =>
            {
                return Query<AndonType>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            andonList.ForEach(p =>
            {
                p.State = State.Enable;
            });
            RF.Save(andonList);
        }
        #endregion

        #region 禁用安灯维护类型
        /// <summary>
        /// 禁用安灯维护类型
        /// </summary>
        /// <param name="andonIds"></param>
        public virtual void DisableAndonType(List<double> andonIds)
        {
            var andonList = andonIds.SplitContains(tempIds =>
            {
                return Query<AndonType>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            andonList.ForEach(p =>
            {
                p.State = State.Disable;
            });
            RF.Save(andonList);
        }
        #endregion

        #region 获取已选择的员工
        /// <summary>
        /// 获取已选择的员工
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<AndonTypeTriggerPower> GetEmployeeAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            var query = Query<AndonTypeTriggerPower>()
                .Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Staff).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取所有员工(或用于查找)
        /// <summary>
        /// 获取所有员工(或用于查找)
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetEmployeeAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Employee>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .NotExists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Staff && p.ObjectCode == x.Code));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 保存选择的员工
        /// <summary>
        /// 保存选择的员工
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public virtual void SaveEmployee(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            EntityList<AndonTypeTriggerPower> deleteAlterList, addAlterList = new EntityList<AndonTypeTriggerPower>();
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                if (deleteCodes.Count != 0)
                {
                    deleteAlterList = deleteCodes.SplitContains(dCodes =>
                    {
                        return Query<AndonTypeTriggerPower>().Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Staff && dCodes.Contains(p.ObjectCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    });
                    deleteAlterList.ForEach(alter =>
                    {
                        alter.PersistenceStatus = PersistenceStatus.Deleted;
                    });
                    RF.Save(deleteAlterList);
                }
                if (addCodes.Count != 0)
                {
                    var employeeList = addCodes.SplitContains(aCodes =>
                    {
                        return Query<Employee>().Where(p => aCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    });
                    employeeList.ForEach(employee =>
                    {
                        var addAlter = new AndonTypeTriggerPower();
                        addAlter.AndonTypeId = andonTypeId;
                        addAlter.ObjectType = Enum.AndonTypeTriggerPower.Staff;
                        addAlter.ObjectCode = employee.Code;
                        addAlter.ObjectName = employee.Name;
                        addAlterList.Add(addAlter);
                    });
                    RF.Save(addAlterList);
                }
                tran.Complete();
            }
        }
        #endregion

        #region 获取已选择的角色
        /// <summary>
        /// 获取已选择的角色
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<AndonTypeTriggerPower> GetRoleAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            var query = Query<AndonTypeTriggerPower>()
                .Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Role).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取所有角色
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Role> GetRoleAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Role>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .NotExists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Role && p.ObjectCode == x.Code));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 保存选择的角色
        /// <summary>
        /// 保存选择的角色
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public virtual void SaveRole(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            EntityList<AndonTypeTriggerPower> deleteAlterList, addAlterList = new EntityList<AndonTypeTriggerPower>();
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                if (deleteCodes.Count != 0)
                {
                    deleteAlterList = deleteCodes.SplitContains(dCodes =>
                    {
                        return Query<AndonTypeTriggerPower>().Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Role && dCodes.Contains(p.ObjectCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    });
                    deleteAlterList.ForEach(alter =>
                    {
                        alter.PersistenceStatus = PersistenceStatus.Deleted;
                    });
                    RF.Save(deleteAlterList);
                }
                if (addCodes.Count != 0)
                {
                    var roleList = Query<Role>().Where(p => addCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    roleList.ForEach(role =>
                    {
                        var addAlter = new AndonTypeTriggerPower();
                        addAlter.AndonTypeId = andonTypeId;
                        addAlter.ObjectType = Enum.AndonTypeTriggerPower.Role;
                        addAlter.ObjectCode = role.Code;
                        addAlter.ObjectName = role.Name;
                        addAlterList.Add(addAlter);
                    });
                    RF.Save(addAlterList);
                }
                tran.Complete();
            }
        }
        #endregion

        #region 获取已选择的用户组
        /// <summary>
        /// 获取已选择的用户组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<AndonTypeTriggerPower> GetUserGroupAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            var query = Query<AndonTypeTriggerPower>()
                .Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.UserGroup).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取全部用户组
        /// <summary>
        /// 获取全部用户组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<UserGroup> GetUserGroupAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<UserGroup>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .NotExists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.UserGroup && p.ObjectCode == x.Code)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;

        }
        #endregion

        #region 保存选择的用户组
        /// <summary>
        /// 保存选择的用户组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public virtual void SaveUserGroup(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            EntityList<AndonTypeTriggerPower> deleteAlterList, addAlterList = new EntityList<AndonTypeTriggerPower>();
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                if (deleteCodes.Count != 0)
                {
                    deleteAlterList = deleteCodes.SplitContains(dCodes =>
                    {
                        return Query<AndonTypeTriggerPower>().Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.UserGroup && dCodes.Contains(p.ObjectCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    });
                    deleteAlterList.ForEach(alter =>
                    {
                        alter.PersistenceStatus = PersistenceStatus.Deleted;
                    });
                    RF.Save(deleteAlterList);
                }
                if (addCodes.Count != 0)
                {
                    var userGroupList = Query<UserGroup>().Where(p => addCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    userGroupList.ForEach(userGroup =>
                    {
                        var addAlter = new AndonTypeTriggerPower();
                        addAlter.AndonTypeId = andonTypeId;
                        addAlter.ObjectType = Enum.AndonTypeTriggerPower.UserGroup;
                        addAlter.ObjectCode = userGroup.Code;
                        addAlter.ObjectName = userGroup.Name;
                        addAlterList.Add(addAlter);
                    });
                    RF.Save(addAlterList);
                }
                tran.Complete();
            }
        }
        #endregion

        #region 获取已选择的部门
        /// <summary>
        /// 获取已选择的部门
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<AndonTypeTriggerPower> GetOrganizationAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            var query = Query<AndonTypeTriggerPower>()
                .Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Department).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取全部部门
        /// <summary>
        /// 获取全部部门
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Organization> GetOrganizationAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Organization>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .Where(p => p.Level.Type == OrganizationType.Department && p.InvOrgId == RT.InvOrg)
                .NotExists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Department && p.ObjectCode == x.Code)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            query.ForEach(organization =>
            {
                organization.TreePId = null;
            });
            return query;
        }
        #endregion

        #region 保存选择的部门
        /// <summary>
        /// 保存选择的部门
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public virtual void SaveOrganization(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            EntityList<AndonTypeTriggerPower> deleteAlterList, addAlterList = new EntityList<AndonTypeTriggerPower>();
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                if (deleteCodes.Count != 0)
                {
                    deleteAlterList = deleteCodes.SplitContains(dCodes =>
                    {
                        return Query<AndonTypeTriggerPower>().Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Department && dCodes.Contains(p.ObjectCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    });
                    deleteAlterList.ForEach(deleteAlter =>
                    {
                        deleteAlter.PersistenceStatus = PersistenceStatus.Deleted;
                    });
                    RF.Save(deleteAlterList);
                }
                if (addCodes.Count != 0)
                {
                    var organizationList = Query<Organization>().Where(p => addCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    organizationList.ForEach(organization =>
                    {
                        var addAlter = new AndonTypeTriggerPower();
                        addAlter.AndonTypeId = andonTypeId;
                        addAlter.ObjectType = Enum.AndonTypeTriggerPower.Department;
                        addAlter.ObjectCode = organization.Code;
                        addAlter.ObjectName = organization.Name;
                        addAlterList.Add(addAlter);
                    });
                    RF.Save(addAlterList);
                }
                tran.Complete();
            }
        }
        #endregion

        #region 获取已选择的班组
        /// <summary>
        /// 获取已选择的班组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<AndonTypeTriggerPower> GetWorkGroupAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            var query = Query<AndonTypeTriggerPower>()
                .Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Team).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取所有的班组
        /// <summary>
        /// 获取所有的班组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WorkGroup> GetWorkGroupAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<WorkGroup>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .NotExists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Team && p.ObjectCode == x.Code))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 保存选择的班组
        /// <summary>
        /// 保存选择的班组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public virtual void SaveWorkGroup(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            EntityList<AndonTypeTriggerPower> deleteAlterList, addAlterList = new EntityList<AndonTypeTriggerPower>();
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                if (deleteCodes.Count != 0)
                {
                    deleteAlterList = deleteCodes.SplitContains(dCodes =>
                    {
                        return Query<AndonTypeTriggerPower>().Where(p => p.AndonTypeId == andonTypeId && p.ObjectType == Enum.AndonTypeTriggerPower.Team && dCodes.Contains(p.ObjectCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    });
                    deleteAlterList.ForEach(deleteAlter =>
                    {
                        deleteAlter.PersistenceStatus = PersistenceStatus.Deleted;
                    });
                    RF.Save(deleteAlterList);
                }
                if (addCodes.Count != 0)
                {
                    var workGroupList = Query<WorkGroup>().Where(p => addCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    workGroupList.ForEach(workGroup =>
                    {
                        var addAlter = new AndonTypeTriggerPower();
                        addAlter.AndonTypeId = andonTypeId;
                        addAlter.ObjectType = Enum.AndonTypeTriggerPower.Team;
                        addAlter.ObjectCode = workGroup.Code;
                        addAlter.ObjectName = workGroup.Name;
                        addAlterList.Add(addAlter);
                    });
                    RF.Save(addAlterList);
                }
                tran.Complete();
            }
        }
        #endregion

        #region 安灯类型维护消息推送子表推送模块默认值
        /// <summary>
        /// 获取安灯类型维护消息推送子表推送模块默认值
        /// </summary>
        /// <returns></returns>
        public virtual PushPlug GetAndonTypeConfigPushPlug()
        {
            var config = ConfigService.GetConfig(new AndonTypePushPlugConfig(), typeof(AndonType));
            return config.PushPugDefault;
        }
        #endregion

        #region 安灯维护消息推送子表推送模块默认值
        /// <summary>
        /// 获取安灯维护消息推送子表推送模块默认值
        /// </summary>
        /// <returns></returns>
        public virtual PushPlug GetAndonConfigPushPlug()
        {
            var config = ConfigService.GetConfig(new AndonPushPlugConfig(), typeof(Andon));
            return config.PushPugDefault;
        }
        #endregion

        #region 推送对象获取数据
        /// <summary>
        /// 推送对象获取数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<AndonTypePushObjectViewModel> GetPushObjectData(Enum.PushObjectType type, string keyword, PagingInfo pagingInfo)
        {
            if (type == Enum.PushObjectType.Staff)
            {
                var query = base.Query<Employee>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<AndonTypePushObjectViewModel> pushObjectList = new EntityList<AndonTypePushObjectViewModel>();
                query.ForEach(item =>
                {
                    var pushObject = new AndonTypePushObjectViewModel();
                    pushObject.Id = item.Code;
                    pushObject.Code = item.Code;
                    pushObject.Name = item.Name;
                    pushObjectList.Add(pushObject);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else if (type == Enum.PushObjectType.Role)
            {
                var query = base.Query<Role>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<AndonTypePushObjectViewModel> pushObjectList = new EntityList<AndonTypePushObjectViewModel>();
                query.ForEach(item =>
                {
                    var pushObject = new AndonTypePushObjectViewModel();
                    pushObject.Id = item.Code;
                    pushObject.Code = item.Code;
                    pushObject.Name = item.Name;
                    pushObjectList.Add(pushObject);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else if (type == Enum.PushObjectType.UserGroup)
            {
                var query = base.Query<UserGroup>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<AndonTypePushObjectViewModel> pushObjectList = new EntityList<AndonTypePushObjectViewModel>();
                query.ForEach(item =>
                {
                    var pushObject = new AndonTypePushObjectViewModel();
                    pushObject.Id = item.Code;
                    pushObject.Code = item.Code;
                    pushObject.Name = item.Name;
                    pushObjectList.Add(pushObject);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else if (type == Enum.PushObjectType.Department)
            {
                var query = base.Query<Organization>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).Where(p => p.Level.Type == OrganizationType.Department && p.InvOrgId == RT.InvOrg).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                EntityList<AndonTypePushObjectViewModel> pushObjectList = new EntityList<AndonTypePushObjectViewModel>();
                query.ForEach(item =>
                {
                    var pushObject = new AndonTypePushObjectViewModel();
                    pushObject.Id = item.Code;
                    pushObject.Code = item.Code;
                    pushObject.Name = item.Name;
                    pushObjectList.Add(pushObject);
                });
                pushObjectList.SetTotalCount(query.TotalCount);
                return pushObjectList;
            }
            else
            {
                return new EntityList<AndonTypePushObjectViewModel>();
            }
        }
        #endregion

        #region 获取推送对象列表
        public virtual EntityList<AndonTypePushObject> GetAndonTypePushObjects(double andonTypeMessageSendId, PagingInfo pagingInfo)
        {
            var query = Query<AndonTypePushObject>().Where(p => p.MessageSendId == andonTypeMessageSendId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion
    }
}
