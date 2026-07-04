using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.ViewModels;
using SIE.Common.Organizations;
using SIE.Domain;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.DataQuery
{
    /// <summary>
    /// 安灯类型维护数据操作
    /// </summary>
    public class AndonTypeTriggerPowerDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取已选择的员工
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<SIE.Andon.Andons.AndonTypeTriggerPower> GetEmployeeAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetEmployeeAlternative(andonTypeId, pagingInfo);
        }

        /// <summary>
        /// 获取全部的员工数据
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<Employee> GetEmployeeAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetEmployeeAll(andonTypeId, keyword, pagingInfo);
        }

        /// <summary>
        /// 保存选择的员工
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public void SaveEmployee(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            RT.Service.Resolve<AndonTypeController>().SaveEmployee(andonTypeId, addCodes, deleteCodes);
        }

        /// <summary>
        /// 获取已选择的角色
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<SIE.Andon.Andons.AndonTypeTriggerPower> GetRoleAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetRoleAlternative(andonTypeId, pagingInfo);
        }
        /// <summary>
        /// 获取所有的角色
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<Role> GetRoleAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetRoleAll(andonTypeId, keyword, pagingInfo);
        }
        /// <summary>
        /// 保存选择的角色
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public void SaveRole(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            RT.Service.Resolve<AndonTypeController>().SaveRole(andonTypeId, addCodes, deleteCodes);
        }

        /// <summary>
        /// 获取已选择的用户组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<SIE.Andon.Andons.AndonTypeTriggerPower> GetUserGroupAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetUserGroupAlternative(andonTypeId, pagingInfo);
        }
        /// <summary>
        /// 获取全部的用户组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<UserGroup> GetUserGroupAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetUserGroupAll(andonTypeId, keyword, pagingInfo);
        }
        /// <summary>
        /// 保存选择的用户组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public void SaveUserGroup(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            RT.Service.Resolve<AndonTypeController>().SaveUserGroup(andonTypeId, addCodes, deleteCodes);
        }

        /// <summary>
        /// 获取已选择的部门
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<SIE.Andon.Andons.AndonTypeTriggerPower> GetOrganizationAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetOrganizationAlternative(andonTypeId, pagingInfo);
        }

        /// <summary>
        /// 获取组织架构所有的部门
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<Organization> GetOrganizationAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetOrganizationAll(andonTypeId, keyword, pagingInfo);
        }

        /// <summary>
        /// 保存选择的部门
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public void SaveOrganization(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            RT.Service.Resolve<AndonTypeController>().SaveOrganization(andonTypeId, addCodes , deleteCodes);
        }

        /// <summary>
        /// 获取选择的班组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<SIE.Andon.Andons.AndonTypeTriggerPower> GetWorkGroupAlternative(double andonTypeId, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetWorkGroupAlternative(andonTypeId, pagingInfo);
        }

        /// <summary>
        /// 获取全部的班组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<WorkGroup> GetWorkGroupAll(double andonTypeId, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetWorkGroupAll(andonTypeId , keyword, pagingInfo);
        }

        /// <summary>
        /// 保存选择的班组
        /// </summary>
        /// <param name="andonTypeId"></param>
        /// <param name="addCodes"></param>
        /// <param name="deleteCodes"></param>
        public void SaveWorkGroup(double andonTypeId, List<string> addCodes, List<string> deleteCodes)
        {
            RT.Service.Resolve<AndonTypeController>().SaveWorkGroup(andonTypeId, addCodes, deleteCodes);
        }

        /// <summary>
        /// 根据推送对象类型获取数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<AndonTypePushObjectViewModel> GetDataByType(SIE.Andon.Andons.Enum.PushObjectType type, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonTypeController>().GetPushObjectData(type, keyword, pagingInfo);
        }
    }
}
