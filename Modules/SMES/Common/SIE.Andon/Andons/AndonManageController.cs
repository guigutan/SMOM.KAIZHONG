using Castle.Facilities.TypedFactory.Internal;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SIE.Andon.Andons.Configs;
using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.ForWinform.ApiModels;
using SIE.Andon.Andons.IOT;
using SIE.Andon.MessageSendJob;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Organizations;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Abnormal;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.LES;
using SIE.LES.Commons;
using SIE.LES.LinesideWarehouses;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Service;
using SIE.MES.Andon;
using SIE.MES.Andon.MessageSendJob;
using SIE.MES.LineAndon;
using SIE.MES.PrepareProducts;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using SIE.Reflection;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯管理控制器
    /// </summary>
    public partial class AndonManageController : DomainController
    {
        private const string change = "{0}修改为{1}";
        private const string dataException = "安灯管理数据异常！";
        #region 安灯管理查询逻辑
        /// <summary>
        /// 安灯管理查询逻辑
        /// </summary>
        /// <param name="andonManageCriterial"></param>
        /// <returns></returns>
        public virtual EntityList<AndonManage> QueryAndonManage(AndonManageCriterial andonManageCriterial)
        {
            var query = Query<AndonManage>();
            if (!andonManageCriterial.AndonManageCode.IsNullOrEmpty())
                query.Where(p => p.AndonManageCode.Contains(andonManageCriterial.AndonManageCode));
            if (andonManageCriterial.AndonManageClass.HasValue)
                query.Where(p => p.AndonManageClass == andonManageCriterial.AndonManageClass);
            if (andonManageCriterial.AndonId != null && andonManageCriterial.AndonId != 0)
                query.Where(p => p.AndonId == andonManageCriterial.AndonId);
            if (andonManageCriterial.AndonTypeId != null && andonManageCriterial.AndonTypeId != 0)
                query.Where(p => p.AndonTypeId == andonManageCriterial.AndonTypeId);
            if (!andonManageCriterial.MulitState.IsNullOrEmpty())
            {
                var stateList = new List<int>();
                andonManageCriterial.MulitState.Split(',').ForEach(s =>
                {
                    stateList.Add(int.Parse(s));
                });
                query.Where(p => stateList.Contains((int)p.State));
            }
            if (andonManageCriterial.DepartmentId != null && andonManageCriterial.DepartmentId != 0)
                query.Where(p => p.Department == andonManageCriterial.Department.Name);
            if (andonManageCriterial.TriggerId != null && andonManageCriterial.TriggerId != 0)
                query.Where(p => p.TriggerId == andonManageCriterial.TriggerId);
            if (andonManageCriterial.HandlerId != null && andonManageCriterial.HandlerId != 0)
                query.Where(p => p.HandlerId == andonManageCriterial.HandlerId);
            if (andonManageCriterial.FactoryId != null && andonManageCriterial.FactoryId != 0)
                query.Where(p => p.FactoryId == andonManageCriterial.FactoryId);
            if (andonManageCriterial.WorkShopId != null && andonManageCriterial.WorkShopId != 0)
                query.Where(p => p.WorkShopId == andonManageCriterial.WorkShopId);
            if (andonManageCriterial.WipResourceId != null && andonManageCriterial.WipResourceId != 0)
                query.Where(p => p.WipResourceId == andonManageCriterial.WipResourceId);
            if (andonManageCriterial.CreateTime.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= andonManageCriterial.CreateTime.BeginValue.Value);
            if (andonManageCriterial.CreateTime.EndValue.HasValue)
                query.Where(p => p.CreateDate <= andonManageCriterial.CreateTime.EndValue.Value);
            if (andonManageCriterial.LineStop.HasValue)
                query.Where(p => p.LineStop == (andonManageCriterial.LineStop == YesNo.Yes));
            if (andonManageCriterial.AskMaterial.HasValue)
                query.Where(p => p.AskMaterial == (andonManageCriterial.AskMaterial == YesNo.Yes));
            query.Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EmployeeId == RT.IdentityId && p.EnterpriseId == x.FactoryId));
            var list = query.OrderBy(andonManageCriterial.OrderInfoList).ToList(andonManageCriterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            var ids = list.Select(p => p.Id).Distinct().ToList();
            var logs = ids.SplitContains(temp =>
            {
                return Query<AndonManageOperateLog>().Where(p => temp.Contains(p.AndonManageId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            foreach (var l in list)
            {
                l.HandleTime = logs.Where(p => p.OperateType == AndonManageOperateType.Handle && p.AndonManageId == l.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                l.ResponseTime = logs.Where(p => p.OperateType == AndonManageOperateType.Response && p.AndonManageId == l.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                l.CheckTime = logs.Where(p => p.OperateType == AndonManageOperateType.Check && p.AndonManageId == l.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                l.HandleHour = l.HandleTime == null || l.ResponseTime == null ? 0 : Math.Round((decimal)(l.HandleTime.Value - l.ResponseTime.Value).TotalMinutes, 2);
                l.ResponseHour = l.ResponseTime == null ? 0 : Math.Round((decimal)(l.ResponseTime.Value - l.TriggerTime).TotalMinutes, 2);
                l.CheckHour = l.CheckTime == null || l.HandleTime == null ? 0 : Math.Round((decimal)(l.CheckTime.Value - l.HandleTime.Value).TotalMinutes, 2);
                l.Duration = (l.HandleHour ?? 0) + (l.ResponseHour ?? 0) + (l.CheckHour ?? 0);
                l.Responser = logs.Where(p => p.OperateType == AndonManageOperateType.Response && p.AndonManageId == l.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperaterName;
                l.Checker = logs.Where(p => p.OperateType == AndonManageOperateType.Check && p.AndonManageId == l.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperaterName;
                l.Handler2 = logs.Where(p => p.OperateType == AndonManageOperateType.Handle && p.AndonManageId == l.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperaterName;
            }

            AndonManage total = new AndonManage();
            total.AndonManageCode = "合计";
            total.CheckHour = list.Sum(p => p.CheckHour);
            total.HandleHour = list.Sum(p => p.HandleHour);
            total.ResponseHour = list.Sum(p => p.ResponseHour);
            total.Duration = list.Sum(p => p.Duration);
            total.ActualTime = list.Sum(p => (p.ActualTime ?? 0));
            total.MarkSaved();
            list.Add(total);

            return list;
        }
        #endregion

        /// <summary>
        /// 根据Id获取安灯维护
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<Andon> GetAndonsByIds(List<double> ids)
        {
            var list = ids.SplitContains(temp =>
            {
                return Query<Andon>().Where(p => temp.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        #region 安灯经验库查询逻辑
        /// <summary>
        /// 安灯经验库查询逻辑
        /// </summary>
        /// <param name="andonExperienceCriterial"></param>
        /// <returns></returns>
        public virtual EntityList<AndonExperience> QueryAndonExperience(AndonExperienceCriterial andonExperienceCriterial)
        {
            var query = Query<AndonExperience>();
            if (!andonExperienceCriterial.AndonManageCode.IsNullOrEmpty())
                query.Where(p => p.AndonManageCode.Contains(andonExperienceCriterial.AndonManageCode));
            if (andonExperienceCriterial.AndonManageClass.HasValue)
                query.Where(p => p.AndonManageClass == andonExperienceCriterial.AndonManageClass);
            if (andonExperienceCriterial.AndonId != null && andonExperienceCriterial.AndonId != 0)
                query.Where(p => p.AndonId == andonExperienceCriterial.AndonId);
            if (andonExperienceCriterial.AndonTypeId != null && andonExperienceCriterial.AndonTypeId != 0)
                query.Where(p => p.AndonTypeId == andonExperienceCriterial.AndonTypeId);
            if (andonExperienceCriterial.State.HasValue)
                query.Where(p => p.State == andonExperienceCriterial.State);
            if (andonExperienceCriterial.DepartmentId != null && andonExperienceCriterial.DepartmentId != 0)
                query.Where(p => p.Department == andonExperienceCriterial.Department.Name);
            if (andonExperienceCriterial.TriggerId != null && andonExperienceCriterial.TriggerId != 0)
                query.Where(p => p.TriggerId == andonExperienceCriterial.TriggerId);
            if (andonExperienceCriterial.HandlerId != null && andonExperienceCriterial.HandlerId != 0)
                query.Where(p => p.HandlerId == andonExperienceCriterial.HandlerId);
            if (andonExperienceCriterial.FactoryId != null && andonExperienceCriterial.FactoryId != 0)
                query.Where(p => p.FactoryId == andonExperienceCriterial.FactoryId);
            if (andonExperienceCriterial.WorkShopId != null && andonExperienceCriterial.WorkShopId != 0)
                query.Where(p => p.WorkShopId == andonExperienceCriterial.WorkShopId);
            if (andonExperienceCriterial.WipResourceId != null && andonExperienceCriterial.WipResourceId != 0)
                query.Where(p => p.WipResourceId == andonExperienceCriterial.WipResourceId);
            if (andonExperienceCriterial.CreateTime.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= andonExperienceCriterial.CreateTime.BeginValue.Value);
            if (andonExperienceCriterial.CreateTime.EndValue.HasValue)
                query.Where(p => p.CreateDate <= andonExperienceCriterial.CreateTime.EndValue.Value);
            if (andonExperienceCriterial.LineStop.HasValue)
                query.Where(p => p.LineStop == (andonExperienceCriterial.LineStop == YesNo.Yes));
            if (andonExperienceCriterial.AskMaterial.HasValue)
                query.Where(p => p.AskMaterial == (andonExperienceCriterial.AskMaterial == YesNo.Yes));
            query.Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EmployeeId == RT.IdentityId && p.EnterpriseId == x.FactoryId));
            query.Where(p => p.ExperienceFlag);
            return query.OrderBy(andonExperienceCriterial.OrderInfoList).ToList(andonExperienceCriterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 历史安灯查询逻辑
        /// <summary>
        /// 历史安灯查询逻辑
        /// </summary>
        /// <param name="andonManageHisCriterial"></param>
        /// <returns></returns>
        public virtual EntityList<AndonManageHistory> QueryAndonHistory(AndonManageHisCriterial andonManageHisCriterial)
        {
            char[] IdentitySplit = new[] { '[', ']' };
            var query = Query<AndonManageHistory>();
            if (!andonManageHisCriterial.AndonManageCode.IsNullOrEmpty())
                query.Where(p => p.AndonManageCode.Contains(andonManageHisCriterial.AndonManageCode));
            if (andonManageHisCriterial.AndonManageClass.HasValue)
                query.Where(p => p.AndonManageClass == andonManageHisCriterial.AndonManageClass);
            if (andonManageHisCriterial.AndonId != null && andonManageHisCriterial.AndonId != 0)
                query.Where(p => p.AndonId == andonManageHisCriterial.AndonId);
            if (andonManageHisCriterial.AndonTypeId != null && andonManageHisCriterial.AndonTypeId != 0)
                query.Where(p => p.AndonTypeId == andonManageHisCriterial.AndonTypeId);
            if (andonManageHisCriterial.State.HasValue)
                query.Where(p => p.State == andonManageHisCriterial.State);
            if (andonManageHisCriterial.DepartmentId != null && andonManageHisCriterial.DepartmentId != 0)
                query.Where(p => p.Department == andonManageHisCriterial.Department.Name);
            if (andonManageHisCriterial.TriggerId != null && andonManageHisCriterial.TriggerId != 0)
                query.Where(p => p.TriggerId == andonManageHisCriterial.TriggerId);
            if (andonManageHisCriterial.HandlerId != null && andonManageHisCriterial.HandlerId != 0)
                query.Where(p => p.HandlerId == andonManageHisCriterial.HandlerId);
            if (andonManageHisCriterial.FactoryId != null && andonManageHisCriterial.FactoryId != 0)
                query.Where(p => p.FactoryId == andonManageHisCriterial.FactoryId);
            if (andonManageHisCriterial.WorkShopId != null && andonManageHisCriterial.WorkShopId != 0)
                query.Where(p => p.WorkShopId == andonManageHisCriterial.WorkShopId);
            if (andonManageHisCriterial.WipResourceId != null && andonManageHisCriterial.WipResourceId != 0)
                query.Where(p => p.WipResourceId == andonManageHisCriterial.WipResourceId);
            if (andonManageHisCriterial.CreateTime.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= andonManageHisCriterial.CreateTime.BeginValue.Value);
            if (andonManageHisCriterial.CreateTime.EndValue.HasValue)
                query.Where(p => p.CreateDate <= andonManageHisCriterial.CreateTime.EndValue.Value);
            if (andonManageHisCriterial.LineStop.HasValue)
                query.Where(p => p.LineStop == (andonManageHisCriterial.LineStop == YesNo.Yes));
            if (andonManageHisCriterial.AskMaterial.HasValue)
                query.Where(p => p.AskMaterial == (andonManageHisCriterial.AskMaterial == YesNo.Yes));
            query.Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EmployeeId == RT.IdentityId && p.EnterpriseId == x.FactoryId))
                .Exists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == x.AndonTypeId
                && p.ObjectType == Enum.AndonTypeTriggerPower.Staff
                && p.ObjectName == RT.Identity.Name.Split(IdentitySplit)[0]));
            return query.OrderBy(andonManageHisCriterial.OrderInfoList).ToList(andonManageHisCriterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 获取安灯管理事件编码
        /// <summary>
        /// 获取安灯管理事件编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetAndonManageCode()
        {
            var config = ConfigService.GetConfig(new AndonManageCodeConfig(), typeof(AndonManage));
            if (config == null || config.AndonManageCodeRule == null)
            {
                throw new ValidationException("未找到安灯管理事件编码生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.AndonManageCodeRule, 1).FirstOrDefault();
        }
        #endregion

        #region 获取启用且有触发权限的安灯类型数据
        /// <summary>
        /// 获取启用且有触发权限的安灯类型数据
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<AndonType> GetAndonTypeEnable(AndonManage andonManage, PagingInfo pagingInfo, string keyword)
        {
            //角色
            var roles = Query<Role>().Exists<UserInRole>((x, y) => y.Where(p => p.UserId == RT.Identity.UserId && p.RoleId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var roleList = new List<string>();
            roles.ForEach(temp =>
            {
                roleList.Add(temp.Code);
            });

            //用户组
            var userGroups = Query<UserGroup>().Exists<UserInUserGroup>((x, y) => y.Where(p => p.UserId == RT.Identity.UserId && p.UserGroupId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var userGroupList = new List<string>();
            userGroups.ForEach(temp =>
            {
                userGroupList.Add(temp.Code);
            });

            //部门
            var departments = Query<Organization>().Where(p => p.InvOrgId == RT.InvOrg && p.Level.Type == OrganizationType.Department).Exists<Org2Employee>((x, y) => y.Where(p => p.OrganizationId == x.Id && p.EmployeeId == RT.IdentityId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var departmentList = new List<string>();
            departments.ForEach(temp =>
            {
                departmentList.Add(temp.Code);
            });

            //班组
            var identity = RF.GetById<Employee>(RT.IdentityId);
            var team = Query<WorkGroup>().Where(p => p.Id == identity.WorkGroupId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            string workGroupCode = string.Empty;
            if (team != null)
            {
                workGroupCode = team.Code;
            }
            char[] IdentitySplit = new[] { '[', ']' };
            var query = Query<AndonType>().Where(p => p.State == State.Enable)
                .Where(p => p.AndonTypeClass == andonManage.AndonManageClass)
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.AndonTypeCode.Contains(keyword) || p.AndonTypeName.Contains(keyword))
                .Exists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == x.Id && ((p.ObjectType == Enum.AndonTypeTriggerPower.Staff && p.ObjectName == RT.Identity.Name.Split(IdentitySplit)[0])
                || (p.ObjectType == Enum.AndonTypeTriggerPower.Role && roleList.Contains(p.ObjectCode))
                || (p.ObjectType == Enum.AndonTypeTriggerPower.UserGroup && userGroupList.Contains(p.ObjectCode))
                || (p.ObjectType == Enum.AndonTypeTriggerPower.Department && departmentList.Contains(p.ObjectCode))
                || (p.ObjectType == Enum.AndonTypeTriggerPower.Team && p.ObjectCode == workGroupCode))))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取启用且安灯类型为选择的安灯数据
        /// <summary>
        /// 获取启用且安灯类型为选择的安灯数据
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Andon> GetAndonEnable(AndonManage andonManage, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Andon>().Where(p => p.State == State.Enable)
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.AndonCode.Contains(keyword) || p.AndonName.Contains(keyword))
                .Where(p => p.AndonTypeId == andonManage.AndonTypeId)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取组织架构下的部门数据
        /// <summary>
        /// 获取组织架构下的部门数据
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Organization> GetOrganizations(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Organization>()
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .Where(p => p.Level.Type == OrganizationType.Department && p.InvOrgId == RT.InvOrg)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            query.ForEach(organization =>
            {
                organization.TreePId = null;
            });
            return query;
        }
        #endregion

        #region 获取对应工厂下的车间(查询用,无工厂找所有权限工厂下的车间)
        /// <summary>
        /// 获取对应工厂下的车间(查询用,无工厂找所有权限工厂下的车间)
        /// </summary>
        /// <param name="andonManageCriterial"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetWorkShops(AndonManageCriterial andonManageCriterial, PagingInfo pagingInfo, string keyword)
        {
            if (andonManageCriterial.FactoryId == null || andonManageCriterial.FactoryId == 0)
            {
                var Factorys = Query<Enterprise>()
                    .Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.Id && p.EmployeeId == RT.IdentityId)).ToList();
                var FactoryIds = new List<double?>();
                Factorys.ForEach(item =>
                {
                    FactoryIds.Add(item.Id);
                });
                List<Enterprise> workShopList = new List<Enterprise>();
                FactoryIds.ForEach(item =>
                {
                    GetchildrenWorkShop(item, workShopList);

                });
                workShopList.ForEach(item =>
                {
                    item.TreePId = null;
                });
                EntityList<Enterprise> workShops = new EntityList<Enterprise>();
                workShops.AddRange(workShopList);
                return workShops;
            }
            else
            {
                List<Enterprise> workShopList = new List<Enterprise>();
                GetchildrenWorkShop(andonManageCriterial.FactoryId, workShopList);
                workShopList.ForEach(item =>
                {
                    item.TreePId = null;
                });
                EntityList<Enterprise> workShops = new EntityList<Enterprise>();
                workShops.AddRange(workShopList);
                return workShops;
            }
        }
        #endregion

        #region 获取对应工厂下的车间(触发用)
        /// <summary>
        /// 获取对应工厂下的车间(触发用)
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetWorkShops(AndonManage andonManage, PagingInfo pagingInfo, string keyword)
        {
            if (andonManage == null)
            {
                throw new ValidationException(dataException.L10N());
            }
            if (andonManage.FactoryId != 0)
            {
                List<Enterprise> workShopList = new List<Enterprise>();
                GetchildrenWorkShop(andonManage.FactoryId, workShopList);
                workShopList.ForEach(item =>
                {
                    item.TreePId = null;
                });
                EntityList<Enterprise> workShops = new EntityList<Enterprise>();
                workShops.AddRange(workShopList);
                return workShops;
            }
            else
            {
                return new EntityList<Enterprise>();
            }

        }
        #endregion

        #region 递归获取子节点获取车间
        /// <summary>
        /// 递归获取子节点获取车间
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="workShops"></param>
        /// <returns></returns>
        public virtual void GetchildrenWorkShop(double? parentId, List<Enterprise> workShops)
        {
            var childrenList = Query<Enterprise>().Where(x => x.TreePId == parentId && x.InvOrgId == RT.InvOrg).ToList();
            childrenList.ForEach(item =>
            {
                if (item.Level.Type == EnterpriseType.Shop)
                {
                    item.TreePId = null;
                    workShops.Add(item);
                }
                GetchildrenWorkShop(item.Id, workShops);
            });
        }
        #endregion

        #region 获取工厂车间下的生产资源(查询用)
        /// <summary>
        /// 获取工厂车间下的生产资源(查询用)
        /// </summary>
        /// <param name="andonManageCriterial"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResources(AndonManageCriterial andonManageCriterial, PagingInfo pagingInfo, string keyword)
        {
            if (andonManageCriterial.WorkShopId != null)
            {
                return Query<WipResource>()
                    .Where(p => p.WorkShopId == andonManageCriterial.WorkShopId)
                    .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else if (andonManageCriterial.FactoryId != null && andonManageCriterial.WorkShopId == null)
            {
                return Query<WipResource>()
                    .Where(p => p.FactoryId == andonManageCriterial.FactoryId)
                    .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                var Factorys = Query<Enterprise>()
                    .Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.Id && p.EmployeeId == RT.IdentityId))
                    .ToList();
                var FactoryIds = new List<double?>();
                var wipListCount = 0;
                Factorys.ForEach(item =>
                {
                    FactoryIds.Add(item.Id);
                });
                if (FactoryIds.Count != 0)
                {
                    var wipResourceList = FactoryIds.SplitContains(tempIds =>
                    {
                        var query = Query<WipResource>()
                         .Where(p => tempIds.Contains(p.FactoryId))
                         .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                        wipListCount = query.Count();
                        return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                    });
                    wipResourceList.SetTotalCount(wipListCount);
                    return wipResourceList;
                }
                return new EntityList<WipResource>();
            }
        }
        #endregion

        #region 获取工厂车间下的生产资源(触发用)
        /// <summary>
        /// 获取工厂车间下的生产资源(触发用)
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResources(AndonManage andonManage, PagingInfo pagingInfo, string keyword)
        {
            if (andonManage == null)
            {
                throw new ValidationException(dataException.L10N());
            }
            if (andonManage.WorkShopId != 0)
            {
                return Query<WipResource>()
                .Where(p => p.WorkShopId == andonManage.WorkShopId)
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return new EntityList<WipResource>();
            }

        }
        #endregion

        #region 获取产线下的工位
        /// <summary>
        /// 获取产线下的工位
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<Station> GetStations(AndonManage andonManage, PagingInfo pagingInfo, string keyword)
        {
            if (andonManage == null)
            {
                throw new ValidationException(dataException.L10N());

            }
            if (andonManage.WipResourceId != 0 && andonManage.WipResourceId != null)
            {
                return Query<Station>()
                .Where(p => p.ResourceId == andonManage.WipResourceId)
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return new EntityList<Station>();
            }
        }
        #endregion

        #region 获取选用安灯的停线和叫料
        /// <summary>
        /// 获取选用安灯的停线和叫料
        /// </summary>
        /// <param name="andonId"></param>
        /// <returns></returns>
        public virtual List<bool> GetLineStopAndAskMaterial(double andonId)
        {
            List<bool> lineStopAndAskMaterial = new List<bool>();
            if (andonId != 0)
            {
                var andon = Query<Andon>().Where(p => p.Id == andonId).FirstOrDefault();
                bool lineStop = andon.LineStop == Enum.AndonYesOrNo.Yes;
                bool askMaterial = andon.AskMaterial == Enum.AndonYesOrNo.Yes;
                lineStopAndAskMaterial.AddRange(new List<bool> { lineStop, askMaterial });
            }
            return lineStopAndAskMaterial;
        }
        #endregion

        #region 根据工位获取在制工单
        /// <summary>
        /// 根据工位获取在制工单
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetMakingWorkOrder(double stationId)
        {
            var workOrder = Query<WorkOrder>()
                .Exists<WipResourceWorkOrder>((x, y) => y.Where(p => p.StationId == stationId && p.WorkOrderId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return workOrder;
        }
        #endregion

        #region 根据资源获取在制工单
        /// <summary>
        /// 根据工位获取在制工单
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetResourceWorkOrder(double resourceId)
        {
            var workOrder = Query<WorkOrder>()
                .Exists<WipResourceWorkOrder>((x, y) => y.Where(p => p.ResourceId == resourceId && p.WorkOrderId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return workOrder;
        }
        #endregion

        #region 根据产线(生产资源)获取工单
        /// <summary>
        /// 根据产线(生产资源)获取工单
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrdersByWip(AndonManage andonManage, PagingInfo pagingInfo, string keyword)
        {
            if (andonManage == null)
            {
                throw new ValidationException(dataException.L10N());
            }
            if (andonManage.WipResourceId != 0 && andonManage.WipResourceId != null)
            {
                var query = Query<WorkOrder>()
                .Where(p => p.ResourceId == andonManage.WipResourceId)
                .Where(p => p.State != Core.WorkOrders.WorkOrderState.Finish && p.State != Core.WorkOrders.WorkOrderState.Close)
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.No.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                return query;
            }
            else
            {
                return new EntityList<WorkOrder>();
            }
        }
        #endregion

        #region 获取转派员工
        /// <summary>
        /// 获取转派员工
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetReassignEmployee(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Employee>().WhereIf(!keyword.IsNullOrEmpty(), p => p.Name.Contains(keyword) || p.Code.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取当前登录人的班组
        /// <summary>
        /// 获取当前登录人的班组
        /// </summary>
        /// <returns></returns>
        public virtual string GetLoaderWorkGroup()
        {
            var workGroupName = Query<Employee>().Where(p => p.Id == RT.IdentityId).FirstOrDefault();
            if (workGroupName.WorkGroup != null)
            {
                return workGroupName.WorkGroup.Name;
            }
            return null;
        }
        #endregion

        #region 加入经验库
        /// <summary>
        /// 加入经验库
        /// </summary>
        /// <param name="andonManage"></param>
        public virtual void AndonManageAddExp(AndonManage andonManage)
        {
            if (andonManage == null)
            {
                throw new ValidationException("安灯管理数据异常，无法加入经验库".L10N());
            }
            if (andonManage.Reason.Length == 0)
            {
                throw new ValidationException("事件原因为空，无法加入经验库".L10N());
            }
            if (andonManage.HandleMethod.Length == 0)
            {
                throw new ValidationException("处理方式为空，无法加入经验库".L10N());
            }
            if (andonManage.Measures.Length == 0)
            {
                throw new ValidationException("预防措施为空，无法加入经验库".L10N());
            }
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                var ExperienceFlag = RF.GetById<AndonManage>(andonManage.Id).ExperienceFlag;
                if (ExperienceFlag)
                {
                    throw new ValidationException("安灯{0}已加入经验库，请刷新界面".L10nFormat(andonManage.AndonManageCode));
                }
                andonManage.ExperienceFlag = true;
                RF.Save(andonManage);
                tran.Complete();
            }
        }
        #endregion

        #region 安灯管理取消命令
        /// <summary>
        /// 安灯管理取消命令
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        public virtual void AndonManageCancel(double andonManageId, AndonManageOperateType operateType, string reason, bool? isCs = null, Employee employee = null)
        {

            var andonManage = RF.GetById<AndonManage>(andonManageId);
            var andonUpholdId = Query<WipResource>().LeftJoin<AndonManage>((x, y) => x.Id == y.WipResourceId)
                                .Where<AndonManage>((x, y) => y.Id == andonManageId).ToList().FirstOrDefault().AndonUpholdId;
            if (andonManage.State != AndonManageState.Standby)
            {
                throw new ValidationException("安灯{0}状态不为[待响应]，请刷新界面".L10nFormat(andonManage.AndonManageCode));
            }
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {



                andonManage.State = Enum.AndonManageState.Cancel;
                RF.Save(andonManage);
                var andonCf = Query<AndonManage>().LeftJoin<WipResource>((x, y) => x.WipResourceId == y.Id)
                                .Where<WipResource>((x, y) => y.AndonUpholdId == andonUpholdId)
                                .Where(p => p.State == AndonManageState.Processing).ToList();
                var andond = Query<AndonManage>().LeftJoin<WipResource>((x, y) => x.WipResourceId == y.Id)
                                .Where<WipResource>((x, y) => y.AndonUpholdId == andonUpholdId)
                                .Where(p => p.State == AndonManageState.Standby).ToList();
                int sy = 1;
                if (andond.Count <= 0)
                {
                    sy = 0;
                }
                if (sy == 0)
                {
                    if (andonCf.Count > 0)
                    {
                        sy = 1;
                    }
                    var andonUpholdData = Query<MES.Andon.AndonUphold>().Where(p => p.Id == andonUpholdId).ToList().FirstOrDefault();

                    //触发iot接口关闭
                    var strToKen = IotGetToken();
                    var iotMessage = IotGetWrite(strToKen, andonUpholdData.AndonEntity, andonUpholdData.AndonOrder, 0, sy);

                    if (iotMessage != "")
                    {
                        throw new ValidationException(iotMessage.L10nFormat(andonManage.AndonManageCode));
                    }
                }
                AndonManageCommandCreateOperate(andonManageId, operateType, reason);
                //取消消息推送
                RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, false);

                //只有CS端需要推给企微
                if (isCs == true)
                {
                    //推送安灯给企微
                    SendMarkdownMessage(andonManage, operateType, isCs, employee?.Name);
                }
                tran.Complete();
            }
        }
        #endregion

        #region 安灯管理响应命令
        /// <summary>
        /// 安灯管理响应命令
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        /// /// <param name="employee"></param>
        public virtual void AndonManageResponse(double andonManageId, AndonManageOperateType operateType, string reason, double employee = -1, bool? isCs = null)
        {

            var andonManage = RF.GetById<AndonManage>(andonManageId);
            if (andonManage.State != AndonManageState.Standby)
            {
                throw new ValidationException("安灯{0}状态不为[待响应]，请刷新界面".L10nFormat(andonManage.AndonManageCode));
            }
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                andonManage.State = Enum.AndonManageState.Processing;
                andonManage.HandlerId = RT.IdentityId;
                RF.Save(andonManage);
                AndonManageCommandCreateOperate(andonManageId, operateType, reason, employee);
                //响应消息推送
                RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, false);
                //只有CS端需要推给企微
                if (isCs == true)
                {
                    var emp = RF.GetById<Employee>(employee);
                    //推送安灯给企微
                    SendMarkdownMessage(andonManage, operateType, isCs, emp?.Name);
                }
                tran.Complete();
            }
            var andonUpholdId = Query<WipResource>().LeftJoin<AndonManage>((x, y) => x.Id == y.WipResourceId)
         .Where<AndonManage>((x, y) => y.Id == andonManageId).ToList().FirstOrDefault().AndonUpholdId;
            var andonCf = Query<AndonManage>().LeftJoin<WipResource>((x, y) => x.WipResourceId == y.Id)
                  .Where<WipResource>((x, y) => y.AndonUpholdId == andonUpholdId)
                  .Where(p => p.State == AndonManageState.Standby).ToList();

            if (andonCf.Count <= 0)
            {
                var andonUpholdData = Query<MES.Andon.AndonUphold>().Where(p => p.Id == andonUpholdId).ToList().FirstOrDefault();

                //触发iot接口关闭
                var strToKen = IotGetToken();
                var iotMessage = IotGetWrite(strToKen, andonUpholdData.AndonEntity, andonUpholdData.AndonOrder, 0, 1);
                if (iotMessage != "")
                {
                    throw new ValidationException(iotMessage.L10nFormat(andonManage.AndonManageCode));
                }

            }
        }
        #endregion

        #region 安灯管理转派命令
        /// <summary>
        /// 安灯管理转派命令
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        /// <param name="reassignEmployeeId"></param>
        /// <param name="ressignAndonId"></param>
        public virtual void AndonManageReassignment(double andonManageId, AndonManageOperateType operateType, string reason, double? reassignEmployeeId, double ressignAndonId)
        {
            var andonManage = RF.GetById<AndonManage>(andonManageId);
            var ressignAndon = RF.GetById<Andon>(ressignAndonId);

            if (andonManage.State != AndonManageState.Standby && andonManage.State != AndonManageState.Processing)
            {
                throw new ValidationException("安灯{0}状态不为[待响应]或[处理中]，请刷新界面".L10nFormat(andonManage.AndonManageCode));
            }
            if (andonManage.AndonId == ressignAndonId && andonManage.HandlerId == reassignEmployeeId)
            {
                throw new ValidationException("转派失败，无数据变更".L10N());
            }
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                string andonChangeReason = string.Empty;
                var andonChange = andonManage.AndonId != ressignAndonId;
                var handlerChange = andonManage.HandlerId != reassignEmployeeId;
                if (andonChange)
                {
                    if (ressignAndon == null)
                    {
                        throw new ValidationException("转派失败，转派安灯数据有误".L10N());
                    }
                    andonChangeReason = change.L10nFormat(andonManage.Andon.AndonName, ressignAndon.AndonName);
                    andonManage.AndonId = ressignAndonId;
                }
                if (handlerChange)
                {
                    andonManage.State = Enum.AndonManageState.Standby;
                    if (reassignEmployeeId.HasValue)
                    {
                        var reassignEmployee = RF.GetById<Employee>(reassignEmployeeId);
                        if (andonManage.HandlerId.HasValue)
                        {
                            reason += change.L10nFormat(andonManage.Handler.Name, reassignEmployee.Name);
                        }
                        else
                        {
                            reason += change.L10nFormat(string.Empty, reassignEmployee.Name);
                        }
                        andonManage.HandlerId = reassignEmployeeId;
                    }
                    else
                    {
                        reason += change.L10nFormat(andonManage.Handler.Name, "空");
                        andonManage.HandlerId = null;
                    }
                }
                RF.Save(andonManage);
                //生成操作记录
                var operateTime = RF.Find<AndonManageOperateLog>().GetDbTime();
                var lastOperation = Query<AndonManageOperateLog>().Where(p => p.AndonManageId == andonManageId).OrderByDescending(p => p.OperateTime).FirstOrDefault();
                var lastOperate = lastOperation == null ? 0 : Math.Round((operateTime - lastOperation.OperateTime).TotalHours, 2);
                if (andonChange)
                {
                    var andonManageOperateLog = new AndonManageOperateLog
                    {
                        AndonManageId = andonManageId,
                        OperateTime = operateTime,
                        OperateType = AndonManageOperateType.AndonNameChange,
                        Remark = andonChangeReason,
                        LastOperate = lastOperate,
                        OperaterId = RT.IdentityId
                    };
                    RF.Save(andonManageOperateLog);
                }
                if (handlerChange)
                {
                    var andonManageOperateLog = new AndonManageOperateLog
                    {
                        AndonManageId = andonManageId,
                        OperateTime = operateTime,
                        OperateType = operateType,
                        Remark = reason,
                        LastOperate = lastOperate,
                        OperaterId = RT.IdentityId
                    };
                    RF.Save(andonManageOperateLog);
                    //转派成功消息推送
                    RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, false, true);
                }
                tran.Complete();
            }
        }
        #endregion

        #region 安灯管理处理完成命令
        /// <summary>
        /// 安灯管理处理完成命令
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        /// <param name="enventReason">事件原因</param>
        /// <param name="handleWay">处理方式</param>
        /// <param name="measure">预防措施</param>
        /// <param name="operaterId">操作人</param>
        /// <param name="isCs">是否CS端</param>
        public virtual void AndonManageHandleAsync(double andonManageId, AndonManageOperateType operateType, string reason, string enventReason = "", string handleWay = "", string measure = "", double operaterId = -1, bool? isCs = null)
        {
            var andonManage = RF.GetById<AndonManage>(andonManageId);
            var andonUpholdId = Query<WipResource>().LeftJoin<AndonManage>((x, y) => x.Id == y.WipResourceId)
                                .Where<AndonManage>((x, y) => y.Id == andonManageId).ToList().FirstOrDefault().AndonUpholdId;

            var responseDtl = andonManage.Andon.AndonResponseDetailList.Where(p => p.AndonUpholdId == andonUpholdId).OrderBy(p => p.AndonseepLevel).FirstOrDefault();
            if (responseDtl == null)
            {
                throw new ValidationException("请先维护安灯维护下面的安灯责任组，触发失败！".L10N());
            }
            //任意一个在职的
            var agD = responseDtl.AndonGroup.AndonGroupDetailList.Where(p => p.User.State == State.Enable && p.User.Employee.EmployeeStatus == Resources.EmployeeStatus.Job).FirstOrDefault();
            if (agD == null)
            {
                throw new ValidationException("安灯责任组维护基础表未维护用户，触发失败！".L10N());
            }
            andonManage.RespPersonId = agD.User.EmployeeId;

            //根据安灯明细获取A1的人员
            //var andonDesc = Query<AndonSesp>().Where(p => p.AndonId == andonManage.AndonId && p.AndonUpholdId == andonUpholdId).OrderBy(p => p.AndonLevel).FirstOrDefault();

            //if (andonDesc == null)
            //{
            //    throw new ValidationException("请先维护安灯维护下面的安灯清单，触发失败！".L10N());
            //}
            //andonManage.RespPersonId = andonDesc.EmployeeId;

            if (andonManage.State != AndonManageState.Processing)
            {
                throw new ValidationException("安灯{0}状态不为[处理中]，请刷新界面".L10nFormat(andonManage.AndonManageCode));
            }
            if (!enventReason.IsNullOrEmpty())
            {
                andonManage.Reason = enventReason;
            }
            if (!handleWay.IsNullOrEmpty())
            {
                andonManage.HandleMethod = handleWay;
            }
            if (!measure.IsNullOrEmpty())
            {
                andonManage.Measures = measure;
            }
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                andonManage.State = Enum.AndonManageState.ToAccepted;
                RF.Save(andonManage);
                AndonManageCommandCreateOperate(andonManageId, operateType, reason, operaterId);
                //处理完成消息推送
                //RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, false);

                //只有CS端需要推给企微
                if (isCs == true)
                {
                    var emp = RF.GetById<Employee>(operaterId);
                    //推送安灯给企微
                    SendMarkdownMessage(andonManage, operateType, isCs, emp?.Name);
                }
                tran.Complete();
            }

            var andonCf = Query<AndonManage>().LeftJoin<WipResource>((x, y) => x.WipResourceId == y.Id)
                  .Where<WipResource>((x, y) => y.AndonUpholdId == andonUpholdId)
                  .Where(p => p.State == AndonManageState.Processing).ToList();
            var andond = Query<AndonManage>().LeftJoin<WipResource>((x, y) => x.WipResourceId == y.Id)
                  .Where<WipResource>((x, y) => y.AndonUpholdId == andonUpholdId)
                  .Where(p => p.State == AndonManageState.Standby).ToList();
            int sy = 1;
            if (andond.Count <= 0)
            {
                sy = 0;
            }
            if (sy == 0)
            {
                if (andonCf.Count <= 0)
                {
                    var andonUpholdData = Query<MES.Andon.AndonUphold>().Where(p => p.Id == andonUpholdId).ToList().FirstOrDefault();

                    //触发iot接口关闭
                    var strToKen = IotGetToken();
                    var iotMessage = IotGetWrite(strToKen, andonUpholdData.AndonEntity, andonUpholdData.AndonOrder, sy, 0);

                    if (iotMessage != "")
                    {
                        throw new ValidationException(iotMessage.L10nFormat(andonManage.AndonManageCode));
                    }
                }
            }
        }
        #endregion

        #region 安灯管理验收命令
        /// <summary>
        /// 安灯管理验收命令
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        /// <param name="actualTime"></param>
        /// <param name="isCs">是否CS端</param>
        public virtual void AndonManageCheck(double andonManageId, AndonManageOperateType operateType, string reason, double? actualTime, bool? isCs = null, double employee = -1)
        {
            var andonManage = RF.GetById<AndonManage>(andonManageId);
            if (andonManage.State != AndonManageState.ToAccepted)
            {
                throw new ValidationException("安灯{0}状态不为[待验收]，请刷新界面".L10nFormat(andonManage.AndonManageCode));
            }
            using (var trans = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                andonManage.State = Enum.AndonManageState.Closed;
                var nowTime = RF.Find<AndonManage>().GetDbTime();
                andonManage.CloseTime = nowTime;
                andonManage.LastTime = Math.Round((nowTime - andonManage.TriggerTime).TotalHours, 1);
                andonManage.ActualTime = actualTime;
                RF.Save(andonManage);
                AndonManageCommandCreateOperate(andonManageId, operateType, reason, employee);
                //验收消息推送
                RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, false);
                //只有CS端需要推给企微
                if (isCs == true)
                {
                    var emp = RF.GetById<Employee>(employee);
                    //推送安灯给企微
                    SendMarkdownMessage(andonManage, operateType, isCs, emp?.Name);
                }
                trans.Complete();
            }
        }
        #endregion

        #region 安灯管理驳回命令
        /// <summary>
        /// 安灯管理驳回命令
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        /// <param name="isCs">是否CS端</param>
        public virtual void AndonManageReject(double andonManageId, AndonManageOperateType operateType, string reason, bool? isCs = null, double employee = -1)
        {
            var andonManage = RF.GetById<AndonManage>(andonManageId);
            if (andonManage.State != AndonManageState.ToAccepted)
            {
                throw new ValidationException("安灯{0}状态不为[待验收]，请刷新界面".L10nFormat(andonManage.AndonManageCode));
            }
            using (var trans = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                andonManage.State = Enum.AndonManageState.Processing;
                RF.Save(andonManage);
                AndonManageCommandCreateOperate(andonManageId, operateType, reason, employee);
                //驳回
                RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, true);
                //只有CS端需要推给企微
                if (isCs == true)
                {
                    var emp = RF.GetById<Employee>(employee);
                    //推送安灯给企微
                    SendMarkdownMessage(andonManage, operateType, isCs, emp?.Name);
                }
                trans.Complete();
            }
        }
        #endregion

        #region 安灯管理命令创建操作记录
        /// <summary>
        /// 安灯管理命令创建操作记录
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        public virtual void AndonManageCommandCreateOperate(double andonManageId, AndonManageOperateType operateType, string reason, double operaterId = -1)
        {
            var andonManageOperateLog = new AndonManageOperateLog();
            var operateTime = RF.Find<AndonManageOperateLog>().GetDbTime();
            andonManageOperateLog.AndonManageId = andonManageId;
            andonManageOperateLog.OperateTime = operateTime;
            andonManageOperateLog.OperateType = operateType;
            if (operaterId == -1)
                andonManageOperateLog.OperaterId = RT.IdentityId;
            else
                andonManageOperateLog.OperaterId = operaterId;


            if (!reason.IsNullOrEmpty())
            {
                andonManageOperateLog.Remark = reason;
            }
            var query = Query<AndonManageOperateLog>().Where(p => p.AndonManageId == andonManageId).OrderByDescending(p => p.OperateTime).FirstOrDefault();
            if (query == null)
            {
                andonManageOperateLog.LastOperate = 0;
            }
            else
            {
                andonManageOperateLog.LastOperate = Math.Round((operateTime - query.OperateTime).TotalHours, 2);
            }
            RF.Save(andonManageOperateLog);
        }
        #endregion

        #region 安灯管理重复触发验证
        /// <summary>
        /// 安灯管理重复触发验证
        /// </summary>
        /// <param name="andonManage"></param>
        /// <returns></returns>
        public virtual bool AndonManageRepeatTrigger(AndonManage andonManage)
        {
            var query = Query<AndonManage>()
                .Where(p => p.Id != andonManage.Id)
                .Where(p => p.AndonId == andonManage.AndonId)
                .Where(p => p.FactoryId == andonManage.FactoryId)
                .Where(p => p.WorkShopId == andonManage.WorkShopId)
                .Where(p => p.WipResourceId == andonManage.WipResourceId)
                .Where(p => p.StationId == andonManage.StationId)
                .Where(p => p.EquipAccountId == andonManage.EquipAccountId)
                .Where(p => p.WorkGroup == andonManage.WorkGroup).ToList();
            var canRepeat = query.Any(p => p.State != Enum.AndonManageState.Cancel && p.State != Enum.AndonManageState.Closed);
            return canRepeat;
        }
        #endregion

        #region 安灯管理保存生成停线记录
        /// <summary>
        /// 安灯管理保存生成停线记录
        /// </summary>
        /// <param name="andonManage"></param>
        public virtual void AndonManageCreateAbnormalCause(AndonManage andonManage)
        {
            if (andonManage == null)
            {
                throw new ValidationException("安灯管理数据异常，生成停线记录失败".L10N());
            }
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                var abnormalCause = new AbnormalCause
                {
                    Code = RT.Service.Resolve<AbnormalCauseController>().GetNewAbnormalCode(),
                    SourceType = ExceptionStopSourceType.AlertLight,
                    EquipAccountId = andonManage.EquipAccountId,
                    ResourceId = (double)andonManage.WipResourceId,
                    ExceptionStopType = ExceptionStopType.StopLine,
                    AbnormalType = andonManage.Andon.DefaultType,
                    AbnormalReason = andonManage.ProblemDesc,
                    ShopId = andonManage.WorkShopId,
                    WorkOrderId = andonManage.WorkOrderId
                };
                RF.Save(abnormalCause);
                andonManage.AbnormalCauseId = abnormalCause.Id;
                RF.Save(andonManage);
                tran.Complete();
            }
        }
        #endregion

        #region 安灯经验库移除命令
        /// <summary>
        /// 安灯经验库移除命令
        /// </summary>
        /// <param name="andonExpIds"></param>
        public virtual void AndonExpRemove(List<double> andonExpIds)
        {
            var andonExpList = andonExpIds.SplitContains(tempId =>
            {
                return Query<AndonExperience>().Where(p => tempId.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                andonExpList.ForEach(andonExp =>
                {
                    if (!andonExp.ExperienceFlag)
                    {
                        throw new ValidationException("安灯{0}已被移除经验库，请刷新界面".L10nFormat(andonExp.AndonManageCode));
                    }
                    andonExp.ExperienceFlag = false;
                });
                RF.Save(andonExpList);
                tran.Complete();
            }
        }
        #endregion

        #region 获取当前员工有权限的安灯类型
        /// <summary>
        /// 获取当前员工有权限的安灯类型
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<AndonType> GetCurrentUserAndonTypes(double? employeeId = null)
        {
            //用户ID
            double? userId = RT.Identity?.UserId;

            if (employeeId.HasValue)
            {
                var user = Query<User>().Where(x => x.EmployeeId == employeeId.Value).FirstOrDefault();
                if (user != null)
                {
                    userId = user.Id;
                }
            }

            //默认员工ID为当前用户的员工ID
            if (!employeeId.HasValue)
            {
                employeeId = RT.IdentityId;
            }


            var roles = Query<Role>().Exists<UserInRole>((x, y) => y.Where(p => p.UserId == userId && p.RoleId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var roleList = new List<string>();
            roles.ForEach(temp =>
            {
                roleList.Add(temp.Code);
            });

            //用户组
            var userGroups = Query<UserGroup>().Exists<UserInUserGroup>((x, y) => y.Where(p => p.UserId == userId && p.UserGroupId == x.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var userGroupList = new List<string>();
            userGroups.ForEach(temp =>
            {
                userGroupList.Add(temp.Code);
            });

            //部门
            var departments = Query<Organization>().Where(p => p.InvOrgId == RT.InvOrg && p.Level.Type == OrganizationType.Department)
                .Exists<Org2Employee>((x, y) => y.Where(p => p.OrganizationId == x.Id && p.EmployeeId == userId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var departmentList = new List<string>();
            departments.ForEach(temp =>
            {
                departmentList.Add(temp.Code);
            });

            //班组
            var identity = RF.GetById<Employee>(RT.IdentityId);
            var team = Query<WorkGroup>().Where(p => p.Id == identity.WorkGroupId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            string workGroupCode = string.Empty;
            if (team != null)
            {
                workGroupCode = team.Code;
            }
            char[] IdentitySplit = new[] { '[', ']' };
            var query = Query<AndonType>().Where(p => p.State == State.Enable)
                .Exists<AndonTypeTriggerPower>((x, y) => y.Where(p => p.AndonTypeId == x.Id && ((p.ObjectType == Enum.AndonTypeTriggerPower.Staff && p.ObjectName == RT.Identity.Name.Split(IdentitySplit)[0])
                || (p.ObjectType == Enum.AndonTypeTriggerPower.Role && roleList.Contains(p.ObjectCode))
                || (p.ObjectType == Enum.AndonTypeTriggerPower.UserGroup && userGroupList.Contains(p.ObjectCode))
                || (p.ObjectType == Enum.AndonTypeTriggerPower.Department && departmentList.Contains(p.ObjectCode))
                || (p.ObjectType == Enum.AndonTypeTriggerPower.Team && p.ObjectCode == workGroupCode))))
                .ToList();
            return query;
        }
        #endregion

        #region  返回Token
        /// <summary>
        /// 获取Iot的Token
        /// </summary>
        /// <returns></returns>
        public virtual string IotGetToken()
        {
            IotToken iot = new IotToken();

            TokenParams token = new TokenParams();
            TokenContext context = new TokenContext();
            TokenArgs args = new TokenArgs();
            token.context = context;
            token.args = args;
            iot.@params = token;
            var requeststr = JsonConvert.SerializeObject(iot);
            //触发iot接口关闭IotTokenData
            var url = RT.Config.Get<string>("IOT.URL");
            url += "/api/root/rpc/login";
            var responsestr = GetHttpPost(url, requeststr, "", false);
            var returnTokenData = JsonConvert.DeserializeObject<IotTokenData>(responsestr);
            if (returnTokenData.result != null)
            {
                if (returnTokenData.result.data != null)
                {
                    if (returnTokenData.result.data.token != null || returnTokenData.result.data.token != "")
                    {
                        return returnTokenData.result.data.token;
                    }
                }
            }
            return "";
        }

        #endregion

        #region 写入IOT安灯指令

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="entityId">设备标识</param>
        /// <param name="orderId">指令</param>
        /// <param name="tag1">点位1</param>
        /// <param name="tag2">点位2</param>
        /// <returns></returns>
        public virtual string IotGetWrite(string token, string entityId, string orderId, int tag1, int tag2)
        {

            IotWrite iot = new IotWrite();
            IotWriteParams param = new IotWriteParams();
            IotWriteContext context = new IotWriteContext();
            context.uid = "";
            IotWriteArgs args = new IotWriteArgs();
            args.entityId = entityId;
            args.orderId = orderId;
            IotWriteParameterMap map = new IotWriteParameterMap();
            map.Tag1 = tag1;
            map.Tag2 = tag2;
            args.parameterMap = map;
            param.args = args;
            param.context = context;
            iot.@params = param;
            var requeststr = JsonConvert.SerializeObject(iot);
            var url = RT.Config.Get<string>("IOT.URL");
            url += "/api/root/rpc/service/";
            var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.IOTWrite, requeststr, DateTime.Now, CallDirection.MesToIot, CallResult.UnSave, 1);
            try
            {
                var responsestr = GetHttpPostWithAuth(url, requeststr, token);
                erpDataInfLog.ResponseContent = responsestr;
                RF.Save(erpDataInfLog);
                var returnData = JsonConvert.DeserializeObject<IotWriteReturn>(responsestr);

                if (returnData.result != null)
                {
                    if (returnData.result.data)
                    {
                        erpDataInfLog.CallResult = CallResult.Success;
                        erpDataInfLog.EndDate = DateTime.Now;
                        erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                        RF.Save(erpDataInfLog);
                        return "";
                    }
                }
                else
                {
                    erpDataInfLog.ErrorMsg = returnData.error.message;
                    erpDataInfLog.CallResult = CallResult.Fail;
                    erpDataInfLog.EndDate = DateTime.Now;
                    erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(erpDataInfLog);
                }

                //erpDataInfLog.ErrorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;


            }
            catch (Exception ex)
            {
                erpDataInfLog.ErrorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                erpDataInfLog.CallResult = CallResult.Fail;
                erpDataInfLog.EndDate = DateTime.Now;
                erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(erpDataInfLog);
                return ex.Message;
            }
            return "";
        }
        #endregion

        #region 写入IOT工单指令

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="entityId">设备标识</param>
        /// <param name="orderId">指令</param>
        /// <param name="workOrder">工单号</param>
        /// <param name="deviceCode">设备编码</param>
        /// /// <param name="outPutNum">工单数量</param>
        /// <returns></returns>
        public virtual string IotGetWorkWrite(string token, string entityId, string orderId, string workOrder, string deviceCode, int outPutNum)
        {

            IotWorkWrite iot = new IotWorkWrite();
            IotWorkWriteParams param = new IotWorkWriteParams();
            IotWorkWriteContext context = new IotWorkWriteContext();
            context.uid = "";
            IotWorkWriteArgs args = new IotWorkWriteArgs();
            args.entityId = entityId;
            args.orderId = orderId;
            IotWorkWriteParameterMap map = new IotWorkWriteParameterMap();
            map.WorkOrder = workOrder;
            map.DeviceCode = deviceCode;
            map.OutPutNum = outPutNum;
            args.parameterMap = map;
            param.args = args;
            param.context = context;
            iot.@params = param;
            var requeststr = JsonConvert.SerializeObject(iot);
            var url = RT.Config.Get<string>("IOT.URL");
            url += "/api/root/rpc/service/";
            var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.IOTWorkWrite, requeststr, DateTime.Now, CallDirection.MesToIot, CallResult.UnSave, 1);
            try
            {
                var responsestr = GetHttpPostWithAuth(url, requeststr, token);
                erpDataInfLog.ResponseContent = responsestr;
                erpDataInfLog.TipMsg = workOrder;
                RF.Save(erpDataInfLog);
                var returnData = JsonConvert.DeserializeObject<IotWriteReturn>(responsestr);
                if (returnData.result != null)
                {
                    if (returnData.result.data)
                    {
                        erpDataInfLog.CallResult = CallResult.Success;
                        erpDataInfLog.EndDate = DateTime.Now;
                        erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                        RF.Save(erpDataInfLog);
                        return "";
                    }
                }
                else
                {
                    erpDataInfLog.ErrorMsg = returnData.error.message;
                    erpDataInfLog.CallResult = CallResult.Fail;
                    erpDataInfLog.EndDate = DateTime.Now;
                    erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(erpDataInfLog);
                    return erpDataInfLog.ErrorMsg;
                }
            }
            catch (Exception ex)
            {
                erpDataInfLog.ErrorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                erpDataInfLog.CallResult = CallResult.Fail;
                erpDataInfLog.EndDate = DateTime.Now;
                erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(erpDataInfLog);
                return ex.Message;
            }
            return "";
        }
        #endregion

        #region 写入冲压和注塑工单指令

        /// <summary>
        /// 写入冲压和注塑工单指令
        /// </summary>
        /// <param name="token"></param>
        /// <param name="entityId"></param>
        /// <param name="orderId"></param>
        /// <param name="iotPunch"></param>
        /// <param name="workOrders"></param>
        /// <returns></returns>
        public virtual string IotBatchWorkWrite(string token, string entityId, string orderId, object iotPunch, string workOrders = null)
        {
            IotPunchWorkWrite iot = new IotPunchWorkWrite();
            IotPunchWorkWriteParams param = new IotPunchWorkWriteParams();
            IotPunchWorkWriteContext context = new IotPunchWorkWriteContext();
            context.uid = "";
            IotPunchWorkWriteArgs args = new IotPunchWorkWriteArgs();
            args.entityId = entityId;
            args.orderId = orderId;
            //IotPunchData map = new IotPunchData();
            //map = iotPunch;
            args.parameterMap = iotPunch;
            param.args = args;
            param.context = context;
            iot.@params = param;
            var requeststr = JsonConvert.SerializeObject(iot);
            var url = RT.Config.Get<string>("IOT.URL");
            url += "/api/root/rpc/service/";
            var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.IOTPunchWorkWrite, requeststr, DateTime.Now, CallDirection.MesToIot, CallResult.UnSave, 1);
            string message = "";
            try
            {
                var responsestr = GetHttpPostWithAuth(url, requeststr, token);
                erpDataInfLog.ResponseContent = responsestr;
                erpDataInfLog.TipMsg = workOrders;
                RF.Save(erpDataInfLog);
                var returnData = JsonConvert.DeserializeObject<IotWriteReturn>(responsestr);
                if (returnData.result != null)
                {
                    if (returnData.result.data)
                    {
                        erpDataInfLog.CallResult = CallResult.Success;
                        erpDataInfLog.EndDate = DateTime.Now;
                        erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                        RF.Save(erpDataInfLog);
                        return "";
                    }
                }
                else
                {
                    erpDataInfLog.ErrorMsg = returnData.error.message;
                    erpDataInfLog.CallResult = CallResult.Fail;
                    erpDataInfLog.EndDate = DateTime.Now;
                    erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(erpDataInfLog);
                    return erpDataInfLog.ErrorMsg;
                }

            }
            catch (Exception ex)
            {
                erpDataInfLog.ErrorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                erpDataInfLog.CallResult = CallResult.Fail;
                erpDataInfLog.EndDate = DateTime.Now;
                erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(erpDataInfLog);
                return message;
            }
            return "";
        }

        /// <summary>
        /// 读取冲压和注塑工单指令
        /// </summary>
        /// <param name="token"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual List<MesWorkOrder> IotBatchWorkRead(string token, string entityId)
        {
            var result = new List<MesWorkOrder>();

            try
            {
                IotWorkRead iot = new IotWorkRead();
                IotWorkReadparams iotParam = new IotWorkReadparams();
                IotWorkReadArgs args = new IotWorkReadArgs();
                IotWorkReadContext context = new IotWorkReadContext();
                iotParam.context = context;
                List<EntityIdCustomPropertyIdsItem> listEntity = new List<EntityIdCustomPropertyIdsItem>();
                EntityIdCustomPropertyIdsItem entity = new EntityIdCustomPropertyIdsItem();
                List<string> list = new List<string>();
                entity.entityId = entityId;
                var tags = new List<string>() { "DeviceCode", "WorkOrder", "OutPutNum", "PutNum" };
                for (int i = 1; i <= 16; i++)
                {
                    tags.Add($"WorkOrder{i}");
                    tags.Add($"OutPutNum{i}");
                }
                entity.customPropertyId = tags;
                listEntity.Add(entity);
                args.entityIdCustomPropertyIds = listEntity;
                iotParam.args = args;
                iot.@params = iotParam;
                var requeststr = JsonConvert.SerializeObject(iot);
                var url = RT.Config.Get<string>("IOT.URL");
                url += "/api/root/rpc/service/";
                var responsestr = GetHttpPostWithAuth(url, requeststr, token);

                var returnData = JsonConvert.DeserializeObject<IotReadReturn>(responsestr);
                if (returnData.result != null)
                {
                    if (returnData.result.data.Count > 0)
                    {

                        var dic = returnData.result.data.ToDictionary(p => p.custom_property_id, p => p.value);
                        var deviceCode = dic.GetValue("DeviceCode", "");
                        var data = new MesWorkOrder()
                        {
                            WorkOrder = dic.GetValue("WorkOrder", ""),
                            OutPutNum = dic.GetValue<decimal>("OutPutNum", 0),
                            DeviceCode = deviceCode
                        };
                        result.Add(data);
                        for (int i = 1; i <= 16; i++)
                        {
                            var keyWorkOrder = $"WorkOrder{i}";
                            var keyOutPutNum = $"OutPutNum{i}";
                            var item = new MesWorkOrder()
                            {
                                WorkOrder = dic.GetValue(keyWorkOrder, ""),
                                OutPutNum = dic.GetValue<decimal>(keyOutPutNum, 0),
                                DeviceCode = deviceCode
                            };
                            result.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
            return result;
        }

        #endregion

        #region  写入IOT工单状态指令
        /// <summary>
        /// 写入IOT工单状态指令
        /// </summary>
        /// <param name="token"></param>
        /// <param name="entityId"></param>
        /// <param name="orderId"></param>
        /// <param name="stateId">状态1等于开始或恢复，2等于暂停</param>
        /// <returns></returns>
        public virtual string IotGeWoStateWrite(string token, string entityId, string orderId, int stateId)
        {
            try
            {
                IotWoStateWrite iot = new IotWoStateWrite();
                IotWoStateWriteParams param = new IotWoStateWriteParams();
                IotWoStateWriteContext context = new IotWoStateWriteContext();
                context.uid = "";
                IotWoStateWriteArgs args = new IotWoStateWriteArgs();
                args.entityId = entityId;
                args.orderId = orderId;
                IotWoStateWriteParameterMap map = new IotWoStateWriteParameterMap();
                map.WoState = stateId;
                args.parameterMap = map;
                param.args = args;
                param.context = context;
                iot.@params = param;
                var requeststr = JsonConvert.SerializeObject(iot);
                var url = RT.Config.Get<string>("IOT.URL");
                url += "/api/root/rpc/service/";
                var responsestr = GetHttpPostWithAuth(url, requeststr, token);
                var returnData = JsonConvert.DeserializeObject<IotWriteReturn>(responsestr);
                if (returnData.result != null)
                {
                    if (returnData.result.data)
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        #endregion

        #region 读取IOT工单指令
        /// <summary>
        /// 读取IOT工单指令
        /// </summary>
        /// <param name="token"></param>
        /// <param name="entityId"></param>
        public virtual MesWorkOrder IOTGetWorkRead(string token, string entityId)
        {
            MesWorkOrder mes = new MesWorkOrder();
            try
            {
                IotWorkRead iot = new IotWorkRead();
                IotWorkReadparams iotParam = new IotWorkReadparams();
                IotWorkReadArgs args = new IotWorkReadArgs();
                IotWorkReadContext context = new IotWorkReadContext();
                iotParam.context = context;
                List<EntityIdCustomPropertyIdsItem> listEntity = new List<EntityIdCustomPropertyIdsItem>();
                EntityIdCustomPropertyIdsItem entity = new EntityIdCustomPropertyIdsItem();
                List<string> list = new List<string>();
                entity.entityId = entityId;
                list.Add("WorkOrder");
                list.Add("DeviceCode");
                list.Add("OutPutNum");
                entity.customPropertyId = list;
                listEntity.Add(entity);
                args.entityIdCustomPropertyIds = listEntity;
                iotParam.args = args;
                iot.@params = iotParam;
                var requeststr = JsonConvert.SerializeObject(iot);
                var url = RT.Config.Get<string>("IOT.URL");
                url += "/api/root/rpc/service/";
                var responsestr = GetHttpPostWithAuth(url, requeststr, token);

                var returnData = JsonConvert.DeserializeObject<IotReadReturn>(responsestr);
                if (returnData.result != null)
                {
                    if (returnData.result.data.Count > 0)
                    {

                        foreach (var item in returnData.result.data)
                        {
                            if (item.custom_property_id == "WorkOrder")
                            {
                                mes.WorkOrder = item.value;
                            }
                            else if (item.custom_property_id == "OutPutNum")
                            {
                                mes.OutPutNum = decimal.Parse(item.value);
                            }
                            else if (item.custom_property_id == "DeviceCode")
                            {
                                mes.DeviceCode = item.value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
            return mes;
        }
        #endregion

        #region HttpPost

        /// <summary>
        /// Psot可以2.0
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="datastr"></param>
        /// <param name="contentType"></param>
        /// <param name="useHttp2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual string GetHttpPost(string posturl, string datastr, string contentType = "", bool useHttp2 = false)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(posturl))
                throw new ArgumentNullException(nameof(posturl), "请求URL不能为空");

            if (datastr == null)
                datastr = string.Empty;

            string ret = string.Empty;
            HttpWebRequest webReq = null;

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(datastr);

                webReq = (HttpWebRequest)WebRequest.Create(new Uri(posturl));
                webReq.Method = "POST";
                webReq.ContentType = string.IsNullOrWhiteSpace(contentType)
                    ? "application/json;charset=utf-8"
                    : contentType;

                // 设置超时时间（30秒）
                webReq.Timeout = 30000;
                webReq.ReadWriteTimeout = 30000;

                // 设置HTTP版本
                if (useHttp2)
                {
                    // 启用HTTP/2.0支持
                    webReq.ProtocolVersion = new Version(2, 0);

                    // HTTP/2.0通常需要TLS加密，这里可以添加一些TLS相关设置
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                }
                else
                {
                    // 默认使用HTTP 1.1
                    webReq.ProtocolVersion = HttpVersion.Version11;
                }

                webReq.ContentLength = byteArray.Length;

                // 使用using确保流资源释放
                using (Stream newStream = webReq.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);
                }

                // 使用using确保响应资源释放
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    // 记录实际使用的HTTP版本
                    System.Diagnostics.Debug.WriteLine($"实际使用的HTTP版本: {response.ProtocolVersion}");

                    // 检查HTTP状态码是否成功
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new HttpRequestException($"服务器返回错误状态码: {response.StatusCode}");
                    }

                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        ret = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                // 完善日志信息
                var errorMsg = $"posturl[{posturl}] content-type[{webReq?.ContentType}] HTTP版本[{webReq?.ProtocolVersion}] failed: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $", 内部异常: {ex.InnerException.Message}";
                }
                Logging.LogManager.GetLogger("error_logger").Info(Environment.NewLine + errorMsg);

                return "请求处理失败，请稍后重试. [{0}]".L10nFormat(ex.Message);
            }
            finally
            {
                // 释放资源
                if (webReq is IDisposable disposableReq)
                {
                    disposableReq.Dispose();
                }
            }

            return ret;
        }

        /// <summary>
        /// 添加Authorization请求头
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="datastr"></param>
        /// <param name="bearerToken"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual string GetHttpPostWithAuth(string posturl, string datastr, string bearerToken, string contentType = "")
        {
            // 校验入参
            if (string.IsNullOrWhiteSpace(posturl))
                throw new ArgumentNullException(nameof(posturl), "请求URL不能为空");
            if (string.IsNullOrWhiteSpace(datastr))
                throw new ArgumentNullException(nameof(datastr), "请求数据不能为空");
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentNullException(nameof(bearerToken), "Bearer令牌不能为空");

            string ret = string.Empty;
            // 设置默认Content-Type
            string actualContentType = string.IsNullOrWhiteSpace(contentType)
                ? "application/json;charset=utf-8"
                : contentType;

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(datastr);

                // 使用using自动释放HttpWebRequest资源
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(posturl);

                webReq.Method = "POST";
                webReq.ContentType = actualContentType;
                // 设置为HTTP 2.0
                webReq.ProtocolVersion = HttpVersion.Version11;
                webReq.ContentLength = byteArray.Length;
                // 设置超时时间（30秒）
                webReq.Timeout = 30000;

                // 添加Authorization请求头
                webReq.Headers.Add("Authorization", $"Bearer {bearerToken}");

                // 使用using自动释放请求流
                using (Stream newStream = webReq.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);
                }

                // 使用using自动释放响应资源
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    // 处理HTTP错误状态码（非200-299范围）
                    if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    {
                        throw new HttpRequestException($"服务器返回错误状态码: {response.StatusCode}");
                    }

                    // 使用using自动释放StreamReader
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        ret = sr.ReadToEnd();
                    }
                }


                return ret;
            }
            catch (Exception ex)
            {
                // 日志记录
                Logging.LogManager.GetLogger("error_logger").Error(
                    $"POST请求失败，URL: {posturl}, Content-Type: {actualContentType}",
                    ex
                );
                throw new InvalidOperationException("发送POST请求时发生错误", ex);
            }
        }
        #endregion

        #region 安灯管理处理完成命令
        /// <summary>
        /// 安灯管理处理完成命令
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <param name="operateType"></param>
        /// <param name="reason"></param>
        /// <param name="enventReason">事件原因</param>
        /// <param name="handleWay">处理方式</param>
        /// <param name="measure">预防措施</param>
        public virtual void AndonManageHandle(double andonManageId, AndonManageOperateType operateType, string reason, string enventReason = "", string handleWay = "", string measure = "")
        {
            var andonManage = RF.GetById<AndonManage>(andonManageId);
            if (andonManage.State != AndonManageState.Processing)
            {
                throw new ValidationException("安灯{0}状态不为[处理中]，请刷新界面".L10nFormat(andonManage.AndonManageCode));
            }
            if (!enventReason.IsNullOrEmpty())
            {
                andonManage.Reason = enventReason;
            }
            if (!handleWay.IsNullOrEmpty())
            {
                andonManage.HandleMethod = handleWay;
            }
            if (!measure.IsNullOrEmpty())
            {
                andonManage.Measures = measure;
            }
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                andonManage.State = Enum.AndonManageState.ToAccepted;
                RF.Save(andonManage);
                AndonManageCommandCreateOperate(andonManageId, operateType, reason);
                //处理完成消息推送
                RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, false);
                tran.Complete();
            }
        }
        #endregion


        #region 通用问题描述

        /// <summary>
        /// 根据安灯维护获取通用问题描述
        /// </summary>
        /// <param name="AndonId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<GeneralProbDtl> GetGeneralProbDtlsByAndonId(double AndonId,PagingInfo pagingInfo,string key)
        {
            var query = Query<GeneralProbDtl>().Where(p => p.AndonId == AndonId);
            if (!key.IsNullOrEmpty())
                query.Where(p => p.Desc.Contains(key));

            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        /// <summary>
        /// 保存安灯管理
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="andonManageCallMaterials"></param>
        public virtual void SaveAndonAndItemDetail(AndonManage andonManage, EntityList<AndonManageCallMaterial> andonManageCallMaterials)
        {
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                SaveAndonManage(andonManage);
                SaveItemDetail(andonManageCallMaterials);
                //触发完成消息推送
                RT.Service.Resolve<MessageSendController>().SendInstantMessage(andonManage, andonManage.State, false);
                tran.Complete();
            }

        }

        /// <summary>
        /// 获取安灯异常产线
        /// </summary>
        /// <param name="wipResourceIds"></param>
        /// <returns></returns>
        public virtual List<double> GetAndonManageIds(List<double> wipResourceIds)
        {
            var andonWipResourceIds = Query<AndonManage>()
                 .Where(p => wipResourceIds.Contains(p.WipResourceId))
                 .Where(p => p.State != AndonManageState.Closed && p.State != AndonManageState.Cancel)
                 .Select(p => p.WipResourceId).ToList<double>();
            return andonWipResourceIds.ToList();
        }

        /// <summary>
        /// 获取用户有权限且启用的安灯类型下启用的安灯
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns></returns>        
        public virtual EntityList<Andon> GetAndonsByEmployeeId(double employeeId)
        {
            var result = new EntityList<Andon>();

            var types = GetCurrentUserAndonTypes(employeeId);

            if (types.Any())
            {
                var andonTypeIds = types.Select(m => (double?)m.Id).ToList();
                var res = Query<Andon>().Where(y => y.State == Domain.State.Enable && andonTypeIds.Contains(y.AndonTypeId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                //内存排序
                result.AddRange(res.OrderBy(m => m.AndonClass).ThenBy(m => m.OrderNo));
            }

            return result;
        }

        /// <summary>
        /// 获取工位为工作单元选择的工位或者触发人为当前用户的安灯事件（状态为待响应、处理中、待验收）
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns></returns>
        public virtual EntityList<AndonManage> GetAndonManages(double employeeId, double resourceId)
        {
            //上面显示工位为工作单元选择的工位或者触发人为当前用户的安灯事件（状态为待响应、处理中、待验收）
            var query = Query<AndonManage>()
                .Where(x => x.State == AndonManageState.Standby
                    || x.State == AndonManageState.Processing || x.State == AndonManageState.ToAccepted)
                .Where(x => x.WipResourceId == resourceId);// x.CreateBy == employeeId || 只按照工位显示

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工位为工作单元选择的工位或者触发人为当前用户的安灯事件（状态为待响应、处理中、待验收）
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="stationId">工位Id</param>
        /// <returns></returns>
        public virtual EntityList<AndonManage> GetAndonRegions(double resourceId)
        {
            var wipResourceData = Query<WipResource>().Where(p => p.Id == resourceId).FirstOrDefault();

            //上面显示工位为工作单元选择的工位或者触发人为当前用户的安灯事件（状态为待响应、处理中、待验收）
            var query = Query<AndonManage>()
                .LeftJoin<WipResource>((x, y) => x.WipResourceId == y.Id)
                .Where<WipResource>((x, y) => (x.State == AndonManageState.Standby
                    || x.State == AndonManageState.Processing || x.State == AndonManageState.ToAccepted)
                    && y.AndonUpholdId == wipResourceData.AndonUpholdId);


            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取这个区域的设备
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccount> GetEquipAccounts(double resourceId)
        {
            var wipResourceData = Query<WipResource>().Where(p => p.Id == resourceId).FirstOrDefault();
            List<double?> eqIds = new List<double?>();
            var andonLines = Query<AndonLine>().Where(p => p.AndonUpholdId == wipResourceData.AndonUpholdId).ToList();
            eqIds.AddRange(andonLines.Select(x => x.EquipmentId).Distinct());
            return Query<EquipAccount>().Where(p => eqIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取安灯管理
        /// </summary>
        /// <param name="andonManageId">安灯管理Id</param>
        /// <returns></returns>
        public virtual AndonManage GetAndonManage(double andonManageId)
        {
            return RF.GetById<AndonManage>(andonManageId, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 计算实际影响时间
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <returns></returns>
        public virtual double ComputerActualTime(double andonManageId)
        {
            //实际影响时间：默认为【当前时间减去触发时间】
            var andonManage = RF.GetById<AndonManage>(andonManageId);
            var dateTime = RF.Find<AndonManage>().GetDbTime();

            return Math.Round((dateTime - andonManage.TriggerTime).TotalMinutes, 1);
        }

        /// <summary>
        /// 获取安灯
        /// </summary>
        /// <param name="andonId">安灯Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="wipId">资源Id</param>
        /// <param name="factoryId">工厂Id</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="isValidAndonEquipAccount">是否校验主设备</param>
        /// <returns></returns>
        public virtual AndonManage CreateAndonManage(object andonId, double stationId, double processId, double wipId, double? factoryId = null, double? workShopId = null, double? workOrderId = null, bool isValidAndonEquipAccount = true)
        {
            var dbDateTime = RF.Find<AndonManage>().GetDbTime();

            var andon = RF.GetById<Andon>(andonId, new EagerLoadOptions().LoadWithViewProperty());

            AndonManage andonManage = new AndonManage();

            andonManage.GenerateId();

            andonManage.AndonManageCode = GetAndonManageCode();
            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Standby;
            andonManage.TriggerId = RT.IdentityId;
            andonManage.TriggerTime = dbDateTime;

            andonManage.AndonManageClass = andon.AndonClass;

            if (andon.AndonTypeId.HasValue)
            {
                andonManage.AndonTypeId = andon.AndonTypeId.Value;
            }

            andonManage.AndonId = andon.Id;
            andonManage.Solution = andon.Solution;
            andonManage.FaultTime = dbDateTime;

            andonManage.LineStopFlag = andon.LineStop;
            andonManage.AskMaterialFlag = andon.AskMaterial;

            andonManage.AskMaterial = andon.AskMaterial == Enum.AndonYesOrNo.Yes;

            andonManage.LineStop = andon.LineStop == Enum.AndonYesOrNo.Yes;

            //if (stationId != 0)
            //    andonManage.StationId = stationId;

            var wip = RF.GetById<WipResource>(wipId,
                new EagerLoadOptions().LoadWithViewProperty());
            andonManage.WipResourceId = wipId;
            //andonManage.ProcessId = processId;
            if (factoryId.HasValue)
                andonManage.FactoryId = factoryId.GetValueOrDefault();
            else
            {
                if (wip.FactoryId == null)
                {
                    throw new ValidationException("生产资源【" + wip.Name + "】没有维护工厂！".L10N());
                }
                andonManage.FactoryId = (double)wip.FactoryId;
            }
            var andonLine = Query<AndonLine>().Where(p => p.MachineCode == wip.Code).ToList().FirstOrDefault();
            if (andonLine != null)
            {
                if (andonLine.Equipment == null && isValidAndonEquipAccount == true && andon.AndonClass == AndonTypeClass.Machine)
                {
                    throw new ValidationException("产线与安灯区域，机台编码【" + wip.Code + "】没有维护主设备号！".L10N());
                }
                if (andonLine.Equipment != null)
                {
                    andonManage.EquipAccount = andonLine.Equipment;
                    andonManage.EquipAccountId = andonLine.EquipmentId;
                }
                //andonManage.EquipAccount.Name = andonLine.Equipment.Name;
                //andonManage.EquipAccount.Code = andonLine.Equipment.Code;

            }
            else
            {
                throw new ValidationException("资源编码在产线与安灯区域没有维护【" + wip.Code + "】没有资源编码！".L10N());
            }

            if (workShopId.HasValue)
                andonManage.WorkShopId = workShopId.GetValueOrDefault();
            else
            {
                if (wip.WorkShopId == null)
                {
                    throw new ValidationException("生产资源【" + wip.Name + "】没有维护车间！".L10N());
                }
                andonManage.WorkShopId = (double)wip.WorkShopId;
            }

            //获取当前产线下的安灯区域
            var resData = Query<WipResource>().Where(p => p.Id == andonManage.WipResourceId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            var responseDtl = andonManage.Andon.AndonResponseDetailList.Where(p => p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonseepLevel).FirstOrDefault();
            if (responseDtl == null)
            {
                throw new ValidationException("请先维护安灯维护下面的安灯责任组，触发失败！".L10N());
            }
            //任意一个在职的
            var agD = responseDtl.AndonGroup.AndonGroupDetailList.Where(p => p.User.State == State.Enable && p.User.Employee.EmployeeStatus == Resources.EmployeeStatus.Job).FirstOrDefault();
            if (agD == null)
            {
                throw new ValidationException("安灯责任组维护基础表未维护用户，触发失败！".L10N());
            }
            andonManage.RespPersonId = agD.User.EmployeeId;

            //根据安灯明细获取A1的人员
            //var andonDesc = Query<AndonSesp>().Where(p => p.AndonId == andonManage.AndonId && p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonLevel).FirstOrDefault();

            //if (andonDesc == null)
            //{
            //    throw new ValidationException("请先维护安灯维护下面的安灯清单，触发失败！".L10N());
            //}
            //andonManage.RespPersonId = andonDesc.EmployeeId;



            if (workOrderId.HasValue)
                andonManage.WorkOrderId = workOrderId.GetValueOrDefault();
            else
            {
                var wos = GetResourceWorkOrder(wipId);

                if (wos != null && wos.Any())
                {
                    var wo = wos.FirstOrDefault();
                    if (wo != null)
                    {
                        andonManage.WorkOrderId = wo.Id;
                    }
                }
            }
            return andonManage;
        }

        /// <summary>
        /// 保存安灯管理
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="andonManageCallMaterials"></param>
        public virtual void SaveAndonAndItemDetailAsync(AndonManage andonManage, EntityList<AndonManageCallMaterial> andonManageCallMaterials)
        {
            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                SaveAndonManage(andonManage);
                //SaveItemDetail(andonManageCallMaterials);
                SendIotMessage(andonManage);
                tran.Complete();
                SendMarkdownMessage(andonManage);
            }

        }

        /// <summary>
        /// 推送IOT消息
        /// </summary>
        /// <param name="andonManage"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void SendIotMessage(AndonManage andonManage)
        {
            try
            {
                //获取当前产线下的安灯区域
                var resData = Query<WipResource>().Where(p => p.Id == andonManage.WipResourceId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                if (resData.AndonUpholdId < 0)
                {
                    throw new ValidationException("资源下没有维护安灯区域，触发失败！".L10N());
                }

                var andonUpholdData = Query<MES.Andon.AndonUphold>().Where(p => p.Id == resData.AndonUpholdId).ToList().FirstOrDefault();
                if (andonUpholdData == null)
                {
                    throw new ValidationException("该产线对应的安灯区域没有维护!".L10N());
                }
                if (!andonUpholdData.AndonEntity.IsNotEmpty())
                {
                    throw new ValidationException("安灯区域IOT实体没有维护!".L10N());
                }
                if (!andonUpholdData.AndonOrder.IsNotEmpty())
                {
                    throw new ValidationException("安灯区域IOT指令没有维护!".L10N());
                }
                //触发iot接口 打开

                var strToKen = IotGetToken();
                var iotMessage = IotGetWrite(strToKen, andonUpholdData.AndonEntity, andonUpholdData.AndonOrder, 1, 1);
                if (iotMessage != "")
                {
                    throw new ValidationException(iotMessage.L10nFormat(andonManage.AndonManageCode));
                }
            }
            catch (Exception ex)
            {

                throw new ValidationException("[{0}]IOT推送失败：{1}".L10nFormat(andonManage?.AndonCode, ex.Message));
            }


            //}
        }

        /// <summary>
        /// 推送企业微信
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="operateType">操作类型</param>
        /// <param name="isCs">是否CS端</param>
        /// <param name="specifyName">指定人</param>
        /// <returns></returns>
        public virtual void SendMarkdownMessage(AndonManage andonManage, AndonManageOperateType? operateType = null, bool? isCs = null, string specifyName = null)
        {
            if (andonManage.Id > 0)
            {
                andonManage = RF.GetById<AndonManage>(andonManage.Id,
                new EagerLoadOptions().LoadWithViewProperty());
            }
            var factoryName = RF.GetById<Enterprise>(andonManage.FactoryId,
                new EagerLoadOptions().LoadWithViewProperty());
            //发送消息人员
            List<double> employeeId = new List<double>();
            var andon = Query<Andon>().Where(m => m.Id == andonManage.AndonId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            //获取当前产线下的安灯区域
            var resData = Query<WipResource>().Where(p => p.Id == andonManage.WipResourceId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            //根据安灯明细获取A1的人员
            //var andonSesps = Query<AndonSesp>().Where(p => p.AndonId == andon.Id && p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonLevel).ToList();
            try
            {
                //if (andonSesps.Count == 0)
                //    throw new ValidationException("安灯责任清单为空".L10N());
                //string andonLevel = andonSesps.FirstOrDefault().AndonLevel;
                //andonSesps = Query<AndonSesp>().Where(p => p.AndonId == andon.Id && p.AndonUpholdId == resData.AndonUpholdId && p.AndonLevel == andonLevel).ToList();
                //employeeId.AddRange(andonSesps.Select(x => x.EmployeeId).Distinct());
                //string userIds = GetEmpCodes(employeeId);
                //string userNames = GetEmpNames(employeeId);


                var responseDtl = andon.AndonResponseDetailList.Where(p => p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonseepLevel).FirstOrDefault();
                if (responseDtl == null)
                    throw new ValidationException("安灯维护未维护安灯责任组".L10N());
                //获取安灯责任组明细
                var andonGroupsDtls = responseDtl.AndonGroup.AndonGroupDetailList;
                employeeId = andonGroupsDtls.Where(p => p.User.State == State.Enable).Select(p => p.User.EmployeeId ?? 0).Where(p => p != 0).Distinct().ToList();
                //责任人
                var responseIds = andonGroupsDtls.Where(p => p.IsResponser == true && p.User.State == State.Enable).Select(p => p.User.EmployeeId ?? 0).Where(p => p != 0).Distinct().ToList();
                //验收人
                var AcceptancerIds = andonGroupsDtls.Where(p => p.IsAcceptancer == true && p.User.State == State.Enable).Select(p => p.User.EmployeeId ?? 0).Where(p => p != 0).Distinct().ToList();
                //组员
                var groupIds = andonGroupsDtls.Where(p => (p.IsAcceptancer == null || p.IsAcceptancer == false) && (p.IsResponser == null || p.IsResponser == false) && p.User.State == State.Enable).Select(p => p.User.EmployeeId ?? 0).Where(p => p != 0).Distinct().ToList();

                //责任人
                string responseNames = GetEmpNames(responseIds);
                //验收人
                string AcceptancerNames = GetEmpNames(AcceptancerIds);
                //组员
                string groupNames = GetEmpNames(groupIds);
                //发送给全部人
                string userIds = GetEmpCodes(employeeId);

                var senders = new WeComMessageSender(
                           corpId: "wx78ac1de6dc983cb0",
                           corpSecret: "yusMO36JshZVnA9bmHuS-ceCMKVio6_Ue0OjknV_JmA",
                           agentId: 1000053);
                var employeeData = Query<Employee>().Where(p => p.Id == andonManage.RespPersonId).ToList().FirstOrDefault();

                string eqCode = "";
                if (andonManage.EquipAccount != null)
                    eqCode = andonManage.EquipAccount.Code;

                var proDesc = "";
                proDesc += andonManage.ProblemDesc;
                //当有通用问题描述的时候，要将合并起来
                //if (andonManage.GeneralProbDtlId != null && andonManage.GeneralProbDtlId > 0)
                //{
                //    if (proDesc == "")
                //        proDesc += andonManage.GeneralProbDtlDesc;
                //    else
                //        proDesc += (";" + andonManage.GeneralProbDtlDesc);
                //}
                TimeSpan timeDifference = RF.Find<AndonManage>().GetDbTime() - andonManage.FaultTime;
                var waitingTime = Math.Round((double)timeDifference.TotalMinutes, 1);

                string message = "您有一个**" + andonManage.AndonName + "**安灯待响应\n" +
                        "**安灯事件编号**：" + andonManage.AndonManageCode + "\n" +
                        "**工**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**厂**：" + factoryName.Name + "\n" +
                        "**产**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**线**：" + andonManage.WipResourceName + "\n" +
                        "**所属区域**：" + resData.AndonUphold?.AndonDesc + "\n" +
                        "**设备编码**：" + eqCode + "\n" +
                        "**问题描述**：" + proDesc + "\n" + 
                         "**异常发生时间**：" + andonManage.FaultTime + "\n" +
                         "**触**\u00A0\u00A0**发**\u00A0\u00A0**人**：" + andonManage.Trigger?.Name + "\n" +
                         "**等待时长**：" + waitingTime + "分钟\n" +
                        "**责**\u00A0\u00A0**任**\u00A0\u00A0**人**：" + responseNames + "\n" +
                        "**验**\u00A0\u00A0**收**\u00A0\u00A0**人**：" + AcceptancerNames + "\n" +
                        "**组**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**员**：" + groupNames + "\n" +
                        "请及时处理!";

                if (operateType != null && isCs == true)
                {
                    var oType = "";
                    switch (operateType)
                    {
                        case AndonManageOperateType.Response:
                            oType = "响应";
                            break;
                        case AndonManageOperateType.Handle:
                            oType = "处理";
                            break;
                        case AndonManageOperateType.Check:
                            oType = "验收";
                            break;
                        case AndonManageOperateType.Reject:
                            oType = "驳回";
                            break;
                        case AndonManageOperateType.Cancel:
                            oType = "取消";
                            break;
                    }
                    message = andonManage.AndonName + "安灯已" + (oType == "验收" ? "验收完成" : oType) + "\n" +
                        "**安灯事件编号**：" + andonManage.AndonManageCode + "\n" +
                        "**工**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**厂**：" + factoryName.Name + "\n" +
                        "**产**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**线**：" + andonManage.WipResourceName + "\n" +
                        "**所属区域**：" + resData.AndonUphold?.AndonDesc + "\n" +
                        "**设备编码**：" + eqCode + "\n" +
                        "**问题描述**：" + proDesc + "\n" +
                         "**异常发生时间**：" + andonManage.FaultTime + "\n" +
                         "**等待时长**：" + waitingTime + "分钟\n" +
                        "**责**\u00A0\u00A0**任**\u00A0\u00A0**人**：" + responseNames + "\n" +
                        "**验**\u00A0\u00A0**收**\u00A0\u00A0**人**：" + AcceptancerNames + "\n" +
                        "**组**\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0**员**：" + groupNames + "\n" +
                        "**" + oType + "人" + "**：" + specifyName;
                    //非验收、取消类型的加这个
                    if (oType != "验收" && oType != "取消")
                    {
                        message += "\n" + "请及时处理!";
                    }
                }

                senders.SendMarkdownMessageAsync
                      (userIds: userIds,//userIds"00101074"
                       content: message);//message

            }
            catch (Exception ex)
            {
                throw new ValidationException("[{0}]企业微信推送失败:{1}".L10nFormat(andonManage?.AndonCode, ex.Message));
            }
        }



        /// <summary>
        /// 根据id获取名称
        /// </summary>
        /// <param name="empids"></param>
        /// <returns></returns>
        private string GetEmpNames(List<double> empids)
        {
            string empName = "";
            foreach (var item in empids)
            {
                var EmployeeData = Query<Employee>().Where(p => p.Id == item).ToList().FirstOrDefault();
                empName += EmployeeData.Name + ",";
            }
            if (empName != "")
            {
                empName = empName.Substring(0, empName.Length - 1);
            }
            return empName;
        }

        /// <summary>
        /// 根据id获取名称
        /// </summary>
        /// <param name="empids"></param>
        /// <returns></returns>
        private string GetEmpCodes(List<double> empids)
        {
            string empCode = "";
            foreach (var item in empids)
            {
                var EmployeeData = Query<Employee>().Where(p => p.Id == item).ToList().FirstOrDefault();
                empCode += EmployeeData.Code + "|";
            }
            if (empCode != "")
            {
                empCode = empCode.Substring(0, empCode.Length - 1);
            }
            return empCode;
        }

        /// <summary>
        /// 提交安灯触发
        /// </summary>
        /// <param name="andonManage">提交的安灯数据</param>
        /// <returns></returns>
        public virtual bool SaveAndonManage(AndonManage andonManage)
        {
            var andon = Query<Andon>().Where(m => m.Id == andonManage.AndonId)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (andon == null)
            {
                throw new ValidationException("安灯数据异常，触发失败！".L10N());
            }
            if (andon.AndonClass == Enum.AndonTypeClass.Machine && !andonManage.EquipAccountId.HasValue)
            {
                throw new ValidationException("安灯大类为【机】时校验设备台账必输".L10N());
            }
            //获取当前产线下的安灯区域
            var resData = Query<WipResource>().Where(p => p.Id == andonManage.WipResourceId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (resData.AndonUpholdId < 0)
            {
                throw new ValidationException("资源下没有维护安灯区域，触发失败！".L10N());
            }
            //根据安灯明细获取A1的人员
            //var andonDesc = Query<AndonSesp>().Where(p => p.AndonId == andon.Id && p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonLevel).FirstOrDefault();

            //if (andonDesc == null)
            //{
            //    throw new ValidationException("请先维护安灯维护下面的安灯清单，触发失败！".L10N());
            //}

            var responseDtl = andonManage.Andon.AndonResponseDetailList.Where(p => p.AndonUpholdId == resData.AndonUpholdId).OrderBy(p => p.AndonseepLevel).FirstOrDefault();
            if (responseDtl == null)
            {
                throw new ValidationException("请先维护安灯维护下面的安灯责任组，触发失败！".L10N());
            }
            //任意一个在职的
            var agD = responseDtl.AndonGroup.AndonGroupDetailList.Where(p => p.User.State == State.Enable && p.User.Employee.EmployeeStatus == Resources.EmployeeStatus.Job).FirstOrDefault();
            if (agD == null)
            {
                throw new ValidationException("安灯责任组维护基础表未维护用户，触发失败！".L10N());
            }
            andonManage.RespPersonId = agD.User.EmployeeId;

            CreateAndonManage(andonManage, andon);

            //andonManage.RespPersonId = (double)andonDesc.EmployeeId;
            if (andon.RepeatTrigger && AndonManageRepeatTrigger(andonManage))
            {
                throw new ValidationException("该安灯{0}存在未关闭的事件，请确认是否重复触发".L10nFormat(andon.AndonCode));
            }

            using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
            {
                //生成停线
                if (andonManage.LineStop)
                {
                    var abnormalCause = new AbnormalCause
                    {
                        Code = RT.Service.Resolve<AbnormalCauseController>().GetNewAbnormalCode(),
                        SourceType = ExceptionStopSourceType.AlertLight,
                        EquipAccountId = andonManage.EquipAccountId,
                        ResourceId = andonManage.WipResourceId,
                        ExceptionStopType = ExceptionStopType.StopLine,
                        AbnormalType = andonManage.Andon.DefaultType,
                        AbnormalReason = andonManage.ProblemDesc,
                        ShopId = andonManage.WorkShopId,
                        WorkOrderId = andonManage.WorkOrderId
                    };
                    RF.Save(abnormalCause);
                    andonManage.AbnormalCauseId = abnormalCause.Id;
                }

                RF.Save(andonManage);

                //创建触发操作记录
                var operateTime = RF.Find<AndonManageOperateLog>().GetDbTime();
                if (operateTime < andonManage.FaultTime)
                {
                    throw new ValidationException("安灯故障发生时间不能晚于当前时间！".L10N());
                }
                var andonManageOperateLog = new AndonManageOperateLog
                {
                    AndonManageId = andonManage.Id,
                    OperateTime = operateTime,
                    OperateType = AndonManageOperateType.Add,
                    OperaterId = RT.IdentityId,
                    LastOperate = 0
                };

                RF.Save(andonManageOperateLog);


                tran.Complete();
            }

            return true;
        }

        private void CreateAndonManage(AndonManage andonManage, Andon andon)
        {
            var station = RF.GetById<Station>(andonManage.StationId,
                new EagerLoadOptions().LoadWithViewProperty());
            if (station != null)
            {
                var resource = RF.GetById<WipResource>(station.ResourceId);

                if (resource == null)
                {
                    throw new ValidationException("系统不存在该产线".L10N());
                }

                if (!resource.FactoryId.HasValue)
                {
                    throw new ValidationException("当前所选产线未维护所属工厂，请维护".L10N());
                }

                if (!resource.WorkShopId.HasValue)
                {
                    throw new ValidationException("当前所选产线未维护所属工厂，请维护".L10N());
                }
                andonManage.FactoryId = resource.FactoryId.Value;
                andonManage.WorkShopId = resource.WorkShopId.Value;
            }

            andonManage.AndonId = andon.Id;
            var identity = RF.GetById<Employee>(RT.IdentityId);
            var team = Query<WorkGroup>().Where(p => p.Id == identity.WorkGroupId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            string workGroupCode = string.Empty;
            if (team != null)
            {
                workGroupCode = team.Name;
            }
            andonManage.WorkGroup = workGroupCode;

            andonManage.Department = andon.DepartmentName;
            andonManage.AndonName = andon.AndonName;


            if (andon.AndonTypeId.HasValue)
            {
                andonManage.AndonTypeId = andon.AndonTypeId.Value;
            }
        }

        /// <summary>
        /// 根据工单工序来选择物料
        /// </summary>
        /// <param name="andonManageCallMaterial"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Item> ChoseItems(AndonManageCallMaterial andonManageCallMaterial, PagingInfo pagingInfo, string keyword)
        {
            if (andonManageCallMaterial == null)
            {
                throw new ValidationException("安灯管理物料明细子表数据异常！".L10N());
            }
            var callWorkOrderId = andonManageCallMaterial.WorkOrderId;
            var callProcessId = andonManageCallMaterial.ProcessId;
            var haveWorkOrderId = callWorkOrderId > 0 && callWorkOrderId != null;
            var haveProcessId = callProcessId > 0 && callProcessId != null;

            //测试用
            //var query1 = Query<Item>()
            //        .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
            //        .Where(p => p.ConsumeMode == ConsumeMode.Pull)
            //        .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //return query1;

            //只要工单为空,只能选拉式物料
            if (!haveWorkOrderId)
            {
                var query = Query<Item>()
                    .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                    .Where(p => p.ConsumeMode == ConsumeMode.Pull)
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                return query;
            }
            //有工单没工序，选择对应工单下的工单bom物料
            else if (!haveProcessId)
            {
                var query = Query<Item>()
                    .Exists<WorkOrderBom>((x, y) => y.Where(p => p.ItemId == x.Id && p.WorkOrderId == callWorkOrderId))
                    .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                return query;

            }
            //有工单有工序，选择对应工单下的工序bom物料
            else
            {
                var query = Query<Item>()
                    .Exists<WorkOrderProcessBom>((x, y) => y.Where(p => p.ItemId == x.Id && p.WorkOrderId == callWorkOrderId && p.ProcessId == callProcessId))
                    .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword))
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                return query;
            }
        }


        /// <summary>
        /// 添加物料根据仓库获取库位
        /// </summary>
        /// <param name="andonManageCallMaterial"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<StorageLocation> GetStorageLocations(AndonManageCallMaterial andonManageCallMaterial, PagingInfo pagingInfo, string keyword)
        {
            if (andonManageCallMaterial.WareHouseId == null || andonManageCallMaterial.WareHouseId == 0)
            {
                throw new ValidationException("请先维护仓库，再选择库位！".L10N());
            }
            var query = Query<StorageLocation>()
                .Where(p => p.WarehouseId == andonManageCallMaterial.WareHouseId)
                .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 安灯管理保存物料子表并创建备料单
        /// </summary>
        /// <param name="andonManageCallMaterials"></param>
        public virtual void SaveItemDetail(EntityList<AndonManageCallMaterial> andonManageCallMaterials)
        {
            if (andonManageCallMaterials == null)
            {
                throw new ValidationException("安灯管理物料明细子表数据异常，创建备料单失败！".L10N());
            }
            if (andonManageCallMaterials.Count != 0)
            {
                using (var tran = DB.TransactionScope(AndonEntityDataProvider.ConnectionStringName))
                {
                    //拉式物料、推式物料分离
                    var pullMaterials = andonManageCallMaterials.Where(p => p.ConsumeType == ConsumeMode.Pull).ToList();
                    var pushMaterials = andonManageCallMaterials.Where(p => p.ConsumeType == ConsumeMode.Push).ToList();
                    if (pullMaterials.Count != 0)
                    {
                        CreatePullStockOrder(pullMaterials);
                        foreach (var item in andonManageCallMaterials)
                        {
                            var targetItem = pullMaterials
                                .FirstOrDefault(p => p.ConsumeType == ConsumeMode.Pull && p.ItemId == item.ItemId);

                            if (targetItem != null)
                            {
                                item.No = targetItem.No;
                                item.LineNo = targetItem.LineNo;
                            }
                        }
                    }
                    if (pushMaterials.Count != 0)
                    {
                        CreatePushStockOrder(pushMaterials);
                        foreach (var item in andonManageCallMaterials)
                        {
                            var targetItem = pullMaterials
                                .FirstOrDefault(p => p.ConsumeType == ConsumeMode.Push && p.ItemId == item.ItemId);
                            if (targetItem != null)
                            {
                                item.No = targetItem.No;
                                item.LineNo = targetItem.LineNo;
                            }
                        }
                    }
                    RF.Save(andonManageCallMaterials);
                    tran.Complete();
                }

            }
        }

        /// <summary>
        /// 创建拉式物料备料单
        /// </summary>
        /// <param name="pullMaterials"></param>
        public virtual void CreatePullStockOrder(List<AndonManageCallMaterial> pullMaterials)
        {
            if (pullMaterials == null)
            {
                throw new ValidationException("拉式物料数据异常！".L10N());
            }
            var stockOrderBaseDate = pullMaterials.FirstOrDefault();
            var stockOrder = CreateNewStockOrder(stockOrderBaseDate, true);
            RF.Save(stockOrder);

            int lineNo = 1;
            var stockDetailList = new EntityList<StockOrderDetail>();

            //一次性取出所有待使用数据
            var woIds = pullMaterials.Where(m => m.WorkOrderId.HasValue).Select(m => m.WorkOrderId.Value).ToList();
            var processIds = pullMaterials.Where(m => m.ProcessId.HasValue).Select(m => m.ProcessId.Value).ToList();
            var woBoms = RT.Service.Resolve<WorkOrderController>().GetWorkOrderBomList(woIds);
            var woList = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(woIds);
            var processList = RT.Service.Resolve<ProcessController>().GetProcessByIds(processIds);
            var workOrderProcessBoms = RT.Service.Resolve<WorkOrderController>().GetWoProcessBomList(woIds);

            pullMaterials.ForEach(item =>
            {
                var itemWo = woList.FirstOrDefault(m => m.Id == item.WorkOrderId);
                var itemProcess = processList.FirstOrDefault(m => m.Id == item.ProcessId);
                var woTotalQty = CalculateTotalQty(item.ItemId, itemWo, itemProcess, woBoms, workOrderProcessBoms);
                var stockDetail = new StockOrderDetail
                {
                    LineNo = lineNo.ToString(),
                    StockState = StockState.Submitted,
                    ItemId = item.ItemId,
                    ProcessId = item.ProcessId,
                    Qty = item.Qty,
                    DemandTime = item.TimeNeed,
                    WarehouseId = item.WareHouseId,
                    StorageLocationId = item.StorageLocationId,
                    StockOrderId = stockOrder.Id,
                    IsManualRec = true,
                    WoTotalQty = woTotalQty,
                    ShipQty = 0,
                    ReceiveQty = 0,
                    CancelQty = 0,
                };
                item.No = stockOrder.No;
                item.LineNo = lineNo++.ToString();
                stockDetailList.Add(stockDetail);
            });
            var tosaveList = new EntityList<StockOrderDetail>();
            tosaveList.AddRange(stockDetailList.OrderBy(p => p.LineNo));
            RF.Save(tosaveList);

            AndonCreateStockPlan(stockOrder);
        }

        /// <summary>
        /// 创建推式物料备料单
        /// </summary>
        /// <param name="pushMaterials"></param>
        public virtual void CreatePushStockOrder(List<AndonManageCallMaterial> pushMaterials)
        {
            if (pushMaterials == null)
            {
                throw new ValidationException("推式物料数据异常！".L10N());
            }
            var stockOrderBaseDate = pushMaterials.FirstOrDefault();
            var stockOrder = CreateNewStockOrder(stockOrderBaseDate, false);
            RF.Save(stockOrder);
            //一次性取出所有待使用数据
            var woIds = pushMaterials.Where(m => m.WorkOrderId.HasValue).Select(m => m.WorkOrderId.Value).ToList();
            var processIds = pushMaterials.Where(m => m.ProcessId.HasValue).Select(m => m.ProcessId.Value).ToList();
            var woBoms = RT.Service.Resolve<WorkOrderController>().GetWorkOrderBomList(woIds);
            var woList = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(woIds);
            var processList = RT.Service.Resolve<ProcessController>().GetProcessByIds(processIds);
            var workOrderProcessBoms = RT.Service.Resolve<WorkOrderController>().GetWoProcessBomList(woIds);


            int lineNo = 1;
            var stockDetailList = new EntityList<StockOrderDetail>();
            pushMaterials.ForEach(item =>
            {
                var itemWo = woList.FirstOrDefault(m => m.Id == item.WorkOrderId);
                var itemProcess = processList.FirstOrDefault(m => m.Id == item.ProcessId);
                var woTotalQty = CalculateTotalQty(item.ItemId, itemWo, itemProcess, woBoms, workOrderProcessBoms);
                var stockDetail = new StockOrderDetail
                {
                    LineNo = lineNo.ToString(),
                    StockState = StockState.Submitted,
                    ItemId = item.ItemId,
                    ProcessId = item.ProcessId,
                    Qty = item.Qty,
                    DemandTime = item.TimeNeed,
                    WarehouseId = item.WareHouseId,
                    StorageLocationId = item.StorageLocationId,
                    StockOrderId = stockOrder.Id,
                    IsManualRec = true,
                    WoTotalQty = woTotalQty,
                    ShipQty = 0,
                    ReceiveQty = 0,
                    CancelQty = 0,
                };
                item.No = stockOrder.No;
                item.LineNo = lineNo++.ToString();
                stockDetailList.Add(stockDetail);
            });
            //保证行号升序显示
            var tosaveList = new EntityList<StockOrderDetail>();
            tosaveList.AddRange(stockDetailList.OrderBy(p => p.LineNo));
            RF.Save(tosaveList);

            AndonCreateStockPlan(stockOrder);
        }

        /// <summary>
        /// 生成备料单主表
        /// </summary>
        /// <param name="stockOrderBaseDate"></param>
        /// <param name="isPull">是否是拉式</param>
        /// <returns></returns>
        public virtual StockOrder CreateNewStockOrder(AndonManageCallMaterial stockOrderBaseDate, bool isPull)
        {
            if (stockOrderBaseDate == null)
            {
                throw new ValidationException("备料单基础数据异常，生成备料单失败！".L10N());
            }
            var woShopId = stockOrderBaseDate.WorkShopId;
            var wipId = stockOrderBaseDate.WipId;
            var woOrId = stockOrderBaseDate.WorkOrderId;
            var factoryId = stockOrderBaseDate.FactoryId;
            var stockOrder = new StockOrder
            {
                No = RT.Service.Resolve<StockOrderService>().GetStockOrderNo(),
                StockState = StockState.Submitted,
                StockType = isPull ? PrepareItemType.Pull : PrepareItemType.Push,
                FactoryId = factoryId,
                WorkShopId = woShopId,
                ResourceId = wipId,
                WorkOrderId = woOrId,
                BillSource = BillSource.External,
                TriggerMode = TriggerMode.ManualModel,
                DemandMode = DemandMode.ManualFillIn,
            };
            return stockOrder;
        }

        /// <summary>
        /// 安灯创建备料计划
        /// </summary>
        /// <param name="stockOrder"></param>
        private void AndonCreateStockPlan(StockOrder stockOrder)
        {
            EntityList<StockOrder> stockOrders = new EntityList<StockOrder>();
            stockOrders.Add(stockOrder);
            RT.Service.Resolve<StockOrderService>().CreateStockPlan(stockOrders);
        }

        /// <summary>
        /// 计算物料工单需求总数
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="workOrder"></param>
        /// <param name="process"></param>
        /// <param name="workOrderBomss"></param>
        /// <param name="workOrderProcessBoms"></param>
        /// <returns></returns>
        public virtual decimal CalculateTotalQty(double itemId, WorkOrder workOrder, Process process, EntityList<WorkOrderBom> workOrderBomss, EntityList<WorkOrderProcessBom> workOrderProcessBoms)
        {
            var haveWorkOrder = workOrder != null;
            var haveProcess = process != null;
            //无工单则返回0
            if (!haveWorkOrder)
            {
                return 0;
            }
            //有工单无工序找工单BOM
            else if (!haveProcess)
            {
                decimal workOrderQty = 0;
                if (workOrder != null)
                {
                    workOrderQty = workOrder.PlanQty;
                }
                var woOrderItem = workOrderBomss.FirstOrDefault(m => m.WorkOrderId == (workOrder == null ? 0 : workOrder.Id) && m.ItemId == itemId);
                decimal singleQty = 0;
                if (woOrderItem != null)
                {
                    singleQty = woOrderItem.SingleQty;
                }
                return workOrderQty * singleQty;
            }
            else
            {
                decimal workOrderQty = 0;
                if (workOrder != null)
                {
                    workOrderQty = workOrder.PlanQty;
                }
                var woProcessItem = workOrderProcessBoms.FirstOrDefault(p => p.ItemId == itemId && p.WorkOrderId == (workOrder == null ? 0 : workOrder.Id));
                decimal singleQty = 0;
                if (woProcessItem != null)
                {
                    singleQty = woProcessItem.SingleQty;
                }
                return workOrderQty * singleQty;
            }
        }

        /// <summary>
        /// 安灯管理添加BOM物料命令
        /// </summary>
        /// <param name="andonManageCallMaterual"></param>
        /// <returns></returns>
        public virtual AndonManageCallMaterial AddCallMaterial(AndonManageCallMaterial andonManageCallMaterual)
        {
            var lineSideWareHouse = Query<LinesideWarehouse>().Where(p => p.WipResouceId == andonManageCallMaterual.WipId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (lineSideWareHouse != null)
            {
                var haveStoreLo = lineSideWareHouse.StorageLocationId != 0 && lineSideWareHouse.StorageLocationId > 0;
                andonManageCallMaterual.WareHouseId = lineSideWareHouse.WarehouseId;
                andonManageCallMaterual.WareHouseName = lineSideWareHouse.Warehouse.Name;
                if (haveStoreLo)
                {
                    andonManageCallMaterual.StorageLocationId = lineSideWareHouse.StorageLocationId;
                    andonManageCallMaterual.LocationName = lineSideWareHouse.StorageLocation.Name;
                }
                andonManageCallMaterual.Hand = false;
            }
            else
            {
                andonManageCallMaterual.Hand = true;
            }
            return andonManageCallMaterual;
        }

        /// <summary>
        /// 查询人员是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual Employee EmpId(string code)
        {
            var EmpData = Query<Employee>().Where(p => p.Code == code).ToList();
            if (EmpData.Count > 0)
            {
                return EmpData.FirstOrDefault();
            }
            return null;
        }
    }
}
