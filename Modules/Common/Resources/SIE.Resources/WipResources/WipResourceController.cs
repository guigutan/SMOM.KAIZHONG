using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Algorithms.KZ;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Resources.WipResources;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Configs;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.Holidays;
using SIE.Resources.LineAndons;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.ShiftTypes;
using SIE.Resources.UserGroups;
using SIE.Resources.WipResources.Models;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 生产资源控制器
    /// </summary>
    public partial class WipResourceController : DomainController, IWipResources
    {
        #region 生产资源 

        /// <summary>
        /// 删除用户组中失效的资源
        /// </summary>
        public virtual void DiseffectUpdateUserGroupResource()
        {
            var userGroupResources = Query<UserGroupResource>().Join<WipResource>((x, y) => x.ResourceId == y.Id && y.ResourceState == ResourceState.Diseffect).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var dic = userGroupResources.GroupBy(p => p.UserGroupId).ToDictionary(p => p.Key, p => p.ToList());

            foreach (var d in dic)
            {
                using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
                {
                    //删除用户组对应的失效资源(同时会更新到员工中)
                    RT.Service.Resolve<UserGroupsController>().DeleteUserGroupResourceSyncEmployee(d.Value.AsEntityList());
                    
                    var ids = d.Value.Select(p => p.Id).Distinct().ToList();
                    ids.SplitDataExecute(i =>
                    {
                        DB.Delete<UserGroupResource>().Where(p => i.Contains(p.Id)).Execute();
                    });
                    tran.Complete();
                }

            }
        }

        /// <summary>
        /// 根据工作中心编码获取工作中心的产线资源
        /// </summary>
        /// <param name="workCenterCode"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResourcesByWorkCenterCode(string workCenterCode, List<double> itemIds, string key = null)
        {
            var list = Query<WipResource>().Join<AndonLine>((x, y) => x.Code == y.MachineCode).Join<AndonLine, WorkCenter>((x, y) => x.WorkCenterId == y.Id && y.Code == workCenterCode).Where(p => p.SQL<bool>($"EXISTS(SELECT 1 FROM PRODUCT_LINE WHERE PRODUCT_LINE.Wip_Resource_Id = T0.ID AND PRODUCT_LINE.IS_PHANTOM = 0 AND PRODUCT_LINE.Item_Id in ({string.Join(",", itemIds)}))")).WhereIf(!key.IsNullOrEmpty(), p => p.Code.Contains(key) || p.Name.Contains(key)).OrderByDescending(p => p.SourceType).Distinct().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var ids1 = list.Select(p => p.Id).Distinct().ToList();

            var list1 = Query<WipResource>().Join<AndonLine>((x, y) => x.Code == y.MachineCode).Join<AndonLine, WorkCenter>((x, y) => x.WorkCenterId == y.Id && y.Code == workCenterCode).Where(p => !ids1.Contains(p.Id)).OrderByDescending(p => p.SourceType).WhereIf(!key.IsNullOrEmpty(), p => p.Code.Contains(key) || p.Name.Contains(key)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            list.AddRange(list1);
            list.SetTotalCount(list.Count);
            return list;
        }

        /// <summary>
        /// 获取生产资源集合
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>生产资源集合</returns>
        public virtual EntityList<WipResource> GetSchedResources(WipResourceCriteria criteria)
        {
            var query = Query<WipResource>();
            if (!criteria.No.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.No));
            if (!criteria.Name.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.State.HasValue)
                query.Where(p => p.ResourceState == criteria.State.Value);
            if (criteria.ProcessTechTypeId.HasValue)
                query.Where(p => p.ProcessTechTypeId == criteria.ProcessTechTypeId);
            if (criteria.CalendarSchemeId.HasValue)
                query.Where(p => p.SchemeId == criteria.CalendarSchemeId);
            if (criteria.WorkShopId.HasValue)
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            if (criteria.SourceType.HasValue)
                query.Where(p => p.SourceType == criteria.SourceType);
            if (criteria.FactoryId.HasValue)
                query.Where(p => p.FactoryId == criteria.FactoryId);
            if (criteria.IsInvalid)
                query.Where(p => p.ResourceState != ResourceState.Diseffect);
            ExtensionWipResourceCrieriaCondition(query, criteria);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 扩展资源查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="criteria"></param>
        protected virtual void ExtensionWipResourceCrieriaCondition(IEntityQueryer<WipResource> query, WipResourceCriteria criteria)
        {
        }

        public virtual EntityList<WipResource> GetSchedResources(PagingInfo pagingInfo = null, string keyword = null)
        {
            var query = Query<WipResource>();
            if (keyword != null)
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取生产资源的ResourceNo
        /// </summary>
        /// <returns>ResourceNo字符串</returns>
        public virtual string GetResourceNo()
        {
            var config = ConfigService.GetConfig(new ResourceNoConfig(), typeof(WipResource));
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到资源编号配置规则，请检查规则配置".L10N());
            }

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 启用生产资源
        /// </summary>
        /// <param name="resources">生产资源</param>
        /// <param name="isRelevant">相关</param>
        public virtual void EnableWipResource(IReadOnlyList<WipResource> resources, bool isRelevant = true)
        {
            var error = new StringBuilder();
            EntityList<WipResource> processWipResources = new EntityList<WipResource>();
            if (isRelevant)
            {
                var parentResIds = resources.Where(p => p.ParentResourceId.HasValue).Select(p => p.ParentResourceId.Value).Distinct().ToList();
                parentResIds.RemoveAll(p => resources.Any(q => q.Id == p));
                if (parentResIds.Count > 0)
                    processWipResources.AddRange(GetWipResources(parentResIds));
            }

            processWipResources.AddRange(resources);
            foreach (var resource in processWipResources)
            {
                if (resource.SchemeId == 0)
                {
                    error.Append("资源[{0}]未指定日历方案。\r\n".L10nFormat(resource.Name));
                }
                else
                {
                    resource.ResourceState = ResourceState.Actived;
                    RF.Save(resource);
                }
            }
            if (error.Length > 0)
                throw new ValidationException(error.ToString());
        }

        /// <summary>
        /// 停用生产资源
        /// </summary>
        /// <param name="resources">生产资源</param>
        /// <param name="isRelevant">相关</param>
        /// <returns>提示语</returns> 
        public virtual void DisableWipResource(IReadOnlyList<WipResource> resources, bool isRelevant = true)
        {
            EntityList<WipResource> processWipResources = new EntityList<WipResource>();
            if (isRelevant)
            {
                var parentResIds = resources.Where(p => p.ParentResourceId.HasValue).Select(p => p.ParentResourceId.Value).Distinct().ToList();
                parentResIds.RemoveAll(p => resources.Any(q => q.Id == p));
                if (parentResIds.Count > 0)
                    processWipResources.AddRange(GetWipResources(parentResIds));
            }

            processWipResources.AddRange(resources);
            foreach (var resource in processWipResources)
            {
                resource.ResourceState = ResourceState.Stop;
                RF.Save(resource);
            }
        }

        /// <summary>
        /// 同步资源数据
        /// </summary>
        /// <returns>报错信息</returns>
        public virtual string RunSync()
        {
            return SyncReourceContainer.SyncResource();
        }

        /// <summary>
        /// 停用生产资源
        /// </summary>
        /// <param name="levelId">企业层级Id</param>
        /// <param name="syncType">同步来源类型</param>
        public virtual void StopSchResourse(double levelId, SyncSourceType syncType)
        {
            //取消资源，停用生产资源中的对应企业模型资源  
            var wipResources = Query<WipResource>()
                .Join<Enterprise>((w, e) => w.SourceId == e.Id && e.LevelId == levelId)
                .Where(p => p.SourceType == syncType && p.SourceId != null && p.ResourceState != ResourceState.Stop)
                .ToList();
            wipResources.ForEach(p =>
            {
                p.ResourceState = ResourceState.Diseffect;
                RF.Save(p);
            });
        }

        /// <summary>
        /// 删除自定义生产资源
        /// </summary>
        /// <param name="ids">来源ID</param>
        /// <param name="syncType">类型</param>
        public virtual void DeleteSchResourse(List<double> ids, SyncSourceType syncType)
        {
            DB.Delete<WipResource>().Where(F => F.SourceId != null && ids.Contains((double)F.SourceId) && F.SourceType == syncType).Execute();
        }

        /// <summary>
        /// 删除自定义生产资源
        /// </summary>
        /// <param name="id">来源ID</param>
        /// <param name="syncType">类型</param>
        public virtual void DeleteSchResourse(double id, SyncSourceType syncType)
        {
            DB.Delete<WipResource>().Where(F => F.SourceId == id && F.SourceType == syncType).Execute();
        }

        /// <summary>
        /// 根据来源Id，更新生产资源的状态
        /// </summary>
        /// <param name="id">来源Id</param>
        /// <param name="syncType">来源类型</param>
        /// <param name="state">来源状态</param>
        public virtual void UpdateSchResourseState(double id, SyncSourceType syncType, ResourceState state)
        {
            WipResource wipResource = Query<WipResource>().Where(p => p.SourceId == id && p.SourceType == syncType).FirstOrDefault();
            if (wipResource != null)
            {
                if (state == ResourceState.Diseffect && wipResource.ResourceState == ResourceState.Actived)
                {
                    throw new ValidationException("保存失败：企业模型对应的生产资源非停用状态".L10N());
                }
                wipResource.ResourceState = state;
                RF.Save(wipResource);
            }
        }

        #region 获取生产资源
        /// <summary>
        /// 通过资源编码获取资源
        /// </summary>
        /// <param name="resCode">资源编码</param>
        /// <returns>资源</returns>
        public virtual WipResource GetScheResourceByResCode(string resCode)
        {
            return Query<WipResource>().Where(p => p.Code == resCode).FirstOrDefault();
        }

        /// <summary>
        /// 通过名称获取生产资源
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>生产资源</returns>
        public virtual WipResource GetWipResourceByName(string name)
        {
            return Query<WipResource>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 通过编码获取生产资源
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>生产资源</returns>
        public virtual WipResource GetWipResourceByCode(string code)
        {
            return Query<WipResource>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 通过编码集合获取生产资源集合
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResourceByCodeList(List<string> codes)
        {
            return Query<WipResource>().Where(p => codes.Contains(p.Code)).ToList();
        }

        /// <summary>
        /// 通过资源Id获取生产资源
        /// </summary>
        /// <param name="resId">资源Id</param>
        /// <returns>生产资源</returns>
        public virtual WipResource GetWipResourceById(double resId)
        {
            return Query<WipResource>().Where(p => p.Id == resId).FirstOrDefault();
        }

        /// <summary>
        /// 获取资源By来源信息
        /// </summary>
        /// <param name="sourceType">来源类型</param>
        /// <param name="sourceId">来源ID</param>
        /// <returns>生产资源</returns>
        public virtual WipResource GetWipResource(double sourceId, SyncSourceType sourceType)
        {
            return Query<WipResource>().Where(p => p.SourceId == sourceId && p.SourceType == sourceType).FirstOrDefault();
        }

        /// <summary>
        /// 获取资源类型为生产线的排程资源
        /// </summary>
        /// <param name="lineId">产线</param>
        /// <returns>排程资源</returns>
        public virtual WipResource GetWipResource(double lineId)
        {
            return Query<WipResource>().Where(p => p.Id == lineId /*&& p.ResourceType == ResourceType.ProductionLine*/).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据来源类型获取生产资源
        /// </summary>
        /// <param name="sourceType">来源类型</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回生产资源</returns>
        public virtual EntityList<WipResource> GetWipResourceBySrcType(List<SyncSourceType> sourceType, PagingInfo pagingInfo, string keyword)
        {
            return GetWipResources(null, null, sourceType, pagingInfo, keyword);
        }

        /// <summary>
        /// 过滤(排除)指定状态、指定类型的生产资源
        /// </summary> 
        /// <param name="criteria">查询条件</param>
        /// <returns>生产资源集合</returns>
        public virtual EntityList<WipResource> GetWipResourcesExcludeSpecifyStateType(WipResourceCriteria criteria)
        {
            Check.NotNull(criteria, "查询条件".L10N());
            return GetSchedResources(criteria);
        }

        /// <summary>
        /// 过滤制程工艺类型的生产资源（含关键字）
        /// </summary> 
        /// <param name="processTechTypeId">制程工艺类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns>生产资源集合</returns>
        public virtual EntityList<WipResource> GetWipResourcesByProcessTechTypeId(double processTechTypeId, string keyword)
        {
            var query = Query<WipResource>();
            query.Where(p => p.ProcessTechTypeId == processTechTypeId);
            query.Where(p => p.ResourceState == ResourceState.Actived);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => (p.Name.Contains(keyword) || p.Code.Contains(keyword)));
            }
            query.Where(p => p.ResourceState != ResourceState.Diseffect);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }




        /// <summary>
        /// 获取使用状态、来源类型获取生产资源数据
        /// </summary>
        /// <param name="stateList">使用状态集合</param>
        /// <param name="srcTypeList">来源类型集合</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回生产资源数据</returns>
        public virtual EntityList<WipResource> GetWipResources(List<ResourceState> stateList, List<SyncSourceType> srcTypeList, PagingInfo pagingInfo, string keyword)
        {
            return GetWipResources(stateList, null, srcTypeList, pagingInfo, keyword);
        }

        /// <summary>
        /// 获取使用状态、来源类型获取生产资源数据
        /// </summary>
        /// <param name="stateList">使用状态集合</param>
        /// <param name="srcTypeList">来源类型集合</param>
        /// <returns>返回生产资源数据</returns>
        public virtual EntityList<WipResource> GetWipResources(List<ResourceState> stateList, List<SyncSourceType> srcTypeList)
        {
            var query = Query<WipResource>();
            if (stateList != null && stateList.Count > 0)
            {
                List<int> stateValueList = stateList.Select(p => (int)p).ToList();
                query = query.Where(p => stateValueList.Contains((int)p.ResourceState));
            }

            if (srcTypeList != null && srcTypeList.Count > 0)
            {
                List<int> srcTypeValueList = srcTypeList.Select(p => (int)p).ToList();
                query.Where(p => srcTypeValueList.Contains((int)p.SourceType));
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取生产资源
        /// </summary>
        /// <param name="stateList">使用状态列表</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="pagInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回生产资源</returns>
        public virtual EntityList<WipResource> GetWipResources(List<ResourceState> stateList, double workShopId, PagingInfo pagInfo, string keyword)
        {
            return GetWipResources(stateList, workShopId, null, pagInfo, keyword);
        }

        /// <summary>
        /// 获取生产资源
        /// </summary>
        /// <param name="stateList">使用状态集合</param>
        /// <param name="workShopId">车间</param>
        /// <param name="srcTypeList">来源类型</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回生产资源集合</returns>
        public virtual EntityList<WipResource> GetWipResources(List<ResourceState> stateList, double? workShopId, List<SyncSourceType> srcTypeList, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WipResource>();
            if (stateList != null && stateList.Count > 0)
            {
                List<int> stateValueList = stateList.Select(p => (int)p).ToList();
                query = query.Where(p => stateValueList.Contains((int)p.ResourceState));
            }

            if (workShopId != null && workShopId != 0)
                query.Where(p => p.WorkShopId == workShopId);

            if (srcTypeList != null && srcTypeList.Count > 0)
            {
                List<int> srcTypeValueList = srcTypeList.Select(p => (int)p).ToList();
                query.Where(p => srcTypeValueList.Contains((int)p.SourceType));
            }

            if (!keyword.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取员工关联的生产资源列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>生产资源列表</returns>
        public virtual EntityList<WipResource> GetWipResources(double? employeeId, PagingInfo pagingInfo = null, string keyword = "")
        {
            var query = Query<WipResource>();
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains($"%{keyword}%") || p.Name.Contains($"%{keyword}%"));
            }
            if (employeeId.HasValue)
            {
                query.Join<EmployeeResource>((x, y) => x.Id == y.ResourceId && y.EmployeeId == employeeId);
            }
            query.OrderBy(p => p.Code);
            return query
                 .ToList(pagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前库存组织的所有生产资源数据【日历方案】
        /// </summary>
        /// <returns>返回当前库存组织的所有生产资源数据</returns>
        public virtual EntityList<WipResource> GetWipResources(List<double> resIds = null)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(WipResource.SchemeProperty);
            elo.LoadWith(WipResource.WorkShopProperty);
            var query = Query<WipResource>();
            if (resIds != null)
            {
                var exp = resIds.CreateContainsExpression<WipResource>("x", "Id");
                query.Where(exp);
            }

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取生产资源（获取已启用）
        /// </summary>       
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <param name="workShopId">车间Id</param>
        /// <returns>返回生产资源</returns>
        public virtual EntityList<WipResource> GetWipResources(PagingInfo pagingInfo, string keyword, double? workShopId = null)
        {
            var query = Query<WipResource>().Where(p => p.ResourceState == ResourceState.Actived && p.SourceType == SyncSourceType.Enterprise);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (workShopId != null)
            {
                query.Where(p => p.WorkShopId == workShopId);
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 根据查询文本获取生产资源列表
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResourcesByKeyword(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WipResource>();
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取生产资源
        /// </summary>
        /// <param name="stateList">使用状态集合</param>
        /// <param name="workShopIdList">车间列表</param>
        /// <param name="srcTypeList">来源类型</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回生产资源集合</returns>
        public virtual EntityList<WipResource> GetWipResourcesByWorkShopId(List<ResourceState> stateList, List<double?> workShopIdList, List<SyncSourceType> srcTypeList, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WipResource>();
            if (stateList != null && stateList.Count > 0)
            {
                List<int> stateValueList = stateList.Select(p => (int)p).ToList();
                query = query.Where(p => stateValueList.Contains((int)p.ResourceState));
            }

            if (workShopIdList != null && workShopIdList.Any())
            {
                query.Where(p => workShopIdList.Contains(p.WorkShopId));
            }

            if (srcTypeList != null && srcTypeList.Count > 0)
            {
                List<int> srcTypeValueList = srcTypeList.Select(p => (int)p).ToList();
                query.Where(p => srcTypeValueList.Contains((int)p.SourceType));
            }

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前库存组织的所有生产资源数据【日历方案】
        /// </summary>
        /// <returns>返回当前库存组织的所有生产资源数据</returns>
        public virtual List<ResourceSchemeInfo> GetResourceSchemeInfos(List<double> resIds = null)
        {
            var query = Query<WipResource>();
            if (resIds != null)
            {
                var exp = resIds.CreateContainsExpression<WipResource>("x", "Id");
                query.Where(exp);
            }

            return query.Select(p => new { ResourceId = p.Id, SchemeId = p.SchemeId }).ToList<ResourceSchemeInfo>().ToList();
        }

        /// <summary>
        /// 根据员工ID查找关联员工所对应资源
        /// </summary>
        /// <param name="employeeId">用户ID</param>
        /// <param name="keyword">资源编码或者资源名称</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="workShopId">车间ID</param>
        /// <returns>资源列表</returns> 
        public virtual EntityList<WipResource> GetResourcesByUserId(double employeeId, string keyword, PagingInfo pagingInfo, double? workShopId = null)
        {
            var query = Query<WipResource>()
                .Join<EmployeeResource>((x, y) => x.Id == y.ResourceId && y.EmployeeId == employeeId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (workShopId.HasValue)
                query.Where(p => p.WorkShopId == workShopId.Value);

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 人员与资源是否存在关系
        /// </summary>
        /// <param name="employeeId">人员Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistEmployeeResource(double employeeId, double resourceId)
        {
            return Query<EmployeeResource>().Where(p => p.ResourceId == resourceId && p.EmployeeId == employeeId).Count() > 0;
        }

        /// <summary>
        /// 获取可用的车间
        /// </summary>
        /// <returns>返回车间数据</returns>
        public virtual EntityList<Enterprise> GetEnterprises()
        {
            return Query<Enterprise>().Exists<WipResource>((x, y) => y.Where(p => p.WorkShopId == x.Id)).ToList();
        }

        /// <summary>
        /// 判断是否使用过制程工艺类型
        /// </summary>
        /// <param name="proTechTypeId">制程工艺类型Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistsProcessTechType(double? proTechTypeId)
        {
            return Query<WipResource>().Where(p => p.ProcessTechTypeId == proTechTypeId).Count() > 0;
        }

        #endregion

        /// <summary>
        /// 获取所有已启用的生产资源列表
        /// </summary>
        /// <returns>生产资源列表</returns>
        public virtual EntityList<WipResource> GetEnableWipResourceList()
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(WipResource.FactoryProperty);
            elo.LoadWith(WipResource.WorkShopProperty);
            return Query<WipResource>()
                .Where(p => p.ResourceState == ResourceState.Actived && p.ProcessTechTypeId > 0)
                .OrderBy(p => new { p.Sequence, p.Name })
                .ToList(null, elo);
        }

        /// <summary>
        /// 通过资源Id列表获取已启用的生产资源列表
        /// </summary>
        /// <param name="resourceIds">资源Id列表</param>
        /// <returns>已启用的生产资源列表</returns>
        public virtual EntityList<WipResource> GetEnableWipResourceList(List<double> resourceIds)
        {
            return Query<WipResource>()
                .Where(p => p.ResourceState == ResourceState.Actived && resourceIds.Contains(p.Id) &&
                            p.ProcessTechTypeId > 0)
                .ToList();
        }

        /// <summary>
        /// 通过资源Id列表获取的生产资源列表
        /// </summary>
        /// <param name="resourceIds">资源Id列表</param>
        /// <returns>生产资源列表</returns>
        public virtual EntityList<WipResource> GetResourceList(List<double> resourceIds)
        {
            return Query<WipResource>().Where(p => resourceIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取生产资源-员工所属企业模型
        /// </summary>       
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回生产资源</returns>
        public virtual EntityList<WipResource> GetWipResourcesByEmp(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WipResource>().Exists<EmployeeResource>((x, y) => y.Where(e => e.ResourceId == x.Id && e.EmployeeId == RT.IdentityId))
                .Where(p => p.ResourceState != ResourceState.Diseffect && p.SourceType == SyncSourceType.Enterprise);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取启用的生产资源列表
        /// </summary>       
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>生产资源列表</returns>
        public virtual EntityList<WipResource> GetWipResourceList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WipResource>().Where(p => p.ResourceState == ResourceState.Actived);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取资源同步设置
        /// </summary>
        /// <returns>资源同步设置</returns>
        public virtual EntityList<SynWipResSetting> GetSynWipResSettings()
        {
            return RF.GetAll<SynWipResSetting>(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据生产资源ID，找到（对应的制程工艺类型，找到对应的）制程工艺ID列表
        /// </summary>
        /// <param name="resourceId">生产资源ID</param>
        /// <returns>对应可选的制程工艺ID列表</returns>
        public virtual List<double> GetProcessIdsByResourceId(double resourceId)
        {
            return Query<WipResource>().Where(p => p.Id == resourceId).Join<ProcessTechType>((w, p) => w.ProcessTechTypeId == p.Id)
                .Join<ProcessTechType, ProcessTech>((ptt, pt) => ptt.Id == pt.ProcessTechTypeId)
                .Select<ProcessTech>((w, p) => p.Id).ToList<double>().ToList();
        }

        /// <summary>
        /// 根据制程工艺ID，找到（对应的制程工艺类型，找到对应的）生产资源ID列表
        /// </summary>
        /// <param name="processId">制程工艺ID</param>
        /// <returns>对应可选的生产资源ID列表</returns>
        public virtual List<double> GetResourceIdsByProcessId(double processId)
        {
            return Query<ProcessTech>().Where(p => p.Id == processId).Join<ProcessTechType>((pt, ptt) => pt.ProcessTechTypeId == ptt.Id)
                .Join<ProcessTechType, WipResource>((p, w) => p.Id == w.ProcessTechTypeId)
                .Select<WipResource>((p, w) => w.Id).ToList<double>().ToList();
        }

        /// <summary>
        /// 根据制程工艺ID，找到（对应的制程工艺类型，找到对应的）生产资源列表【如果为0，则返回所有】
        /// </summary>
        /// <param name="processId">制程工艺ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetResourcesByProcessId(double processId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WipResource>();
            if (keyword.IsNotEmpty() && keyword == "%")
            {
                return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            if (keyword.IsNotEmpty())
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            if (processId > 0)
            {
                query.Exists<ProcessTechType>((w, p) => p.Where(p1 => p1.Id == w.ProcessTechTypeId)
                .Exists<ProcessTech>((ptt, pt) => pt.Where(pt1 => ptt.Id == pt1.ProcessTechTypeId && pt1.Id == processId)));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据生产资源ID，找到（对应的制程工艺类型，找到对应的）制程工艺列表【如果为0，则返回所有】
        /// </summary>
        /// <param name="resourceId">生产资源ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public virtual EntityList<ProcessTech> GetProcessesByResourceId(double resourceId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<ProcessTech>();
            if (keyword.IsNotEmpty() && keyword == "%")
            {
                return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            if (keyword.IsNotEmpty())
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            if (resourceId > 0)
            {
                query.Exists<ProcessTechType>((pt, p) => p.Where(p1 => p1.Id == pt.ProcessTechTypeId)
                .Exists<WipResource>((ptt, wr) => wr.Where(wr1 => ptt.Id == wr1.ProcessTechTypeId && wr1.Id == resourceId)));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion 生产资源 

        /// <summary>
        /// 根据制程工艺编码获取工段实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ProcessSegment GetProcessSegmentByProcessTechTypeId(double id)
        {
            return Query<ProcessSegment>().Join<ProcessTech>((x, y) => x.Id == y.ProcessSegmentId && y.ProcessTechTypeId == id).FirstOrDefault();
        }

        /// <summary>
        /// 获取生产资源，若工厂ID有值，则过滤
        /// </summary>
        /// <param name="factoryId">工厂ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>生产资源实体列表</returns>
        public virtual EntityList<WipResource> GetResourcesByFactoryId(double? factoryId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<WipResource>();
            if (factoryId.HasValue)
            {
                query.Where(p => p.FactoryId == factoryId);
            }
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo);
        }

        #region 计划资源工作日获取逻辑

        /// <summary>
        /// 获取资源工作日字典
        /// </summary>
        /// <param name="wipResource">生产资源</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>资源工作日字典</returns>
        public virtual Dictionary<DateTime, ShiftTypeInfo> GetShiftTypeInfo(WipResource wipResource,
            DateTime beginDate, DateTime endDate)
        {
            if (wipResource == null)
            {
                return new Dictionary<DateTime, ShiftTypeInfo>();
            }

            var calSchCt = RT.Service.Resolve<CalendarSchemeController>();
            var holidayCt = RT.Service.Resolve<HolidayController>();

            //法定假期列表
            var holidays = holidayCt.GetHolidayList(beginDate, endDate);
            var holidayOfDay = GetHolidaysByDate(holidays);

            //默认周方案
            var calendarList = calSchCt.GetCalendarSchemeWeeks(wipResource.SchemeId);

            var infos = new Dictionary<DateTime, ShiftTypeInfo>();
            var days = (endDate - beginDate).Days;
            for (int day = 0; day <= days; day++)
            {
                DateTime displayDate = beginDate.AddDays(day);
                var info = GetExpShiftTypeInfo(displayDate, calendarList);
                if (wipResource.ResourceState == ResourceState.Actived && displayDate >= DateTime.Today)
                    info.IsActived = true;
                if (holidayOfDay.Contains(displayDate))
                {
                    info.IsHoliday = holidayOfDay.Contains(displayDate);
                }

                infos.Add(displayDate, info);
            }

            return infos;
        }

        /// <summary>
        /// 获取按天的法定节假日列表
        /// </summary>
        /// <param name="holidays">法定节假日列表</param>
        /// <returns>按天的法定节假日列表</returns>
        private List<DateTime> GetHolidaysByDate(EntityList<Holiday> holidays)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (var holiday in holidays)
            {
                var days = (holiday.EndDate.Date - holiday.BeginDate.Date).Days + 1;
                for (int day = 0; day < days; day++)
                {
                    dates.Add(holiday.BeginDate.AddDays(day));
                }
            }

            return dates;
        }

        /// <summary>
        /// 获取工作日信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="calendarList">周方案列表</param>
        /// <returns>工作日信息</returns>
        private ShiftTypeInfo GetExpShiftTypeInfo(DateTime date, EntityList<CalendarSchemeWeek> calendarList)
        {
            ShiftTypeInfo info = new ShiftTypeInfo();
            ShiftType shiftType = null;

            var calSalWeek = calendarList.Where(p => p.ActiveDate.Date <= date).OrderByDescending(p => p.ActiveDate)
                .FirstOrDefault();
            shiftType = calSalWeek?.ShiftType;
            if (calSalWeek != null)
            {
                SetHolidayFlag(date, info, calSalWeek);
            }

            info.Content = shiftType == null ? string.Empty : shiftType.Name;
            info.ShiftType = shiftType;
            return info;
        }

        /// <summary>
        /// 设置是否休息日标记
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="info">班制信息</param>
        /// <param name="calSalWeek">周方案</param>
        private void SetHolidayFlag(DateTime date, ShiftTypeInfo info, CalendarSchemeWeek calSalWeek)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    info.IsHoliday = !calSalWeek.Mon;
                    break;
                case DayOfWeek.Tuesday:
                    info.IsHoliday = !calSalWeek.Tue;
                    break;
                case DayOfWeek.Wednesday:
                    info.IsHoliday = !calSalWeek.Wed;
                    break;
                case DayOfWeek.Thursday:
                    info.IsHoliday = !calSalWeek.Thu;
                    break;
                case DayOfWeek.Friday:
                    info.IsHoliday = !calSalWeek.Fri;
                    break;
                case DayOfWeek.Saturday:
                    info.IsHoliday = !calSalWeek.Sat;
                    break;
                case DayOfWeek.Sunday:
                    info.IsHoliday = !calSalWeek.Sun;
                    break;
            }
        }

        /// <summary>
        /// 获取生产资源班制信息
        /// </summary>
        /// <param name="resourceId">生产资源ID</param>
        /// <param name="date">日期</param>
        /// <returns>班制信息</returns>
        public virtual ShiftType GetWipResourceShiftType(double resourceId, DateTime date)
        {
            ShiftType shiftType = null;
            DateTime beginDate = date.Date;
            ////优先级 ：1生产资源例外，存在返回；2日历方案例外，存在返回；3、默认周方案 
            var calSchController = RT.Service.Resolve<CalendarSchemeController>();

            var resource = RF.GetById<WipResource>(resourceId);
            if (resource != null)
            {
                var calExcept = calSchController.GetCalendarSchemeExcept(resource.SchemeId, beginDate);
                shiftType = calExcept?.ShiftType;

                if (shiftType == null)
                {
                    var calSalWeek = calSchController.GetNewestCalSchWeek(resource.SchemeId);
                    if (calSalWeek == null)
                        return null;
                    var isHoliday = RT.Service.Resolve<CalendarSchemeController>().IsHoliday(date, calSalWeek);  //20200706 BUG#B0024155
                    if (!isHoliday)
                        shiftType = calSalWeek.ShiftType;
                }
            }

            return shiftType;
        }


        /// <summary>
        /// 获取生产资源班制信息
        /// </summary>
        /// <param name="wipResourceMove">生产资源</param>
        /// <param name="date">日期</param>
        /// <returns>班制信息</returns>
        public virtual double? GetWipResourceShiftTypeId(WipResourceMove wipResourceMove, DateTime date)
        {
            double? shiftTypeId = null;
            DateTime beginDate = date.Date;

            //优先级 ：1生产资源例外，存在返回；2日历方案例外，存在返回；3、默认周方案 
            var calSchController = RT.Service.Resolve<CalendarSchemeController>();

            shiftTypeId = calSchController.GetCalendarSchemeExceptShiftTypeId(wipResourceMove.SchemeId, beginDate);

            if (shiftTypeId == null)
            {
                var calSalWeek = calSchController.GetNewestCalSchWeek(wipResourceMove.SchemeId);
                if (calSalWeek == null)
                {
                    return null;
                }

                var isHoliday = RT.Service.Resolve<CalendarSchemeController>().IsHoliday(date, calSalWeek);  //20200706 BUG#B0024155
                if (!isHoliday)
                {
                    shiftTypeId = calSalWeek.ShiftTypeId;
                }
            }

            return shiftTypeId;
        }


        /// <summary>
        /// 获取生产资源班次信息
        /// </summary>
        /// <param name="resourceId">生产资源ID</param>
        /// <param name="date">当前时间</param>
        /// <param name="wipResourceMove">生产资源</param>
        /// <exception cref="ValidationException">未找到班制</exception>
        /// <exception cref="ValidationException">未找到班次</exception>
        /// <returns>班次</returns>
        public virtual Shift GetWipResourceShift(double resourceId, DateTime date, WipResourceMove wipResourceMove = null)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                if (wipResourceMove == null)
                {
                    wipResourceMove = RF.GetById<WipResourceMove>(resourceId);
                }

                var shiftTypeId = GetWipResourceShiftTypeId(wipResourceMove, date);

                if (shiftTypeId == null)
                {
                    throw new ValidationException("未找到班制".L10N());
                }
                var controller = RT.Service.Resolve<ShiftTypeController>();
                var shiftList = controller.GetShifts((double)shiftTypeId);
                var shift = controller.GetShift(shiftList, date);
                if (shift == null)
                {
                    throw new ValidationException("未找到班次".L10N());
                }
                return shift;
            }
        }

        #endregion 计划资源工作日获取逻辑

        /// <summary>
        /// 获取火山灰前处理的生产资源
        /// </summary>
        /// <returns>返回火山灰前处理的生产资源</returns>
        public virtual EntityList<WipResource> GetAshWipResources()
        {
            var query = Query<WipResource>().Join<ProcessTechType>((a, b) => a.ProcessTechTypeId == b.Id)
                .Where<ProcessTechType>((a, b) => a.ResourceState == ResourceState.Actived && (b.Code.Contains("火山灰%") || b.Name.Contains("火山灰%")));

            return query.ToList();
        }

        /// <summary>
        /// 根据生产资源id获取资源类型
        /// </summary>
        /// <param name="resourceId">生产资源id</param>
        /// <returns>资源类型</returns>
        public virtual string GetResourceTypeByResourceId(double resourceId)
        {
            return Query<WipResource>().Where(p => p.Id == resourceId).Select(p => p.ResourceType).ToList<string>().FirstOrDefault();
        }

        /// <summary>
        /// 根据生产资源id获取资源类型
        /// </summary>
        /// <param name="resourceIds">生产资源ids</param>
        /// <returns>资源类型</returns>
        public virtual EntityList<WipResource> GetResourceTypeByResourceIds(List<double> resourceIds)
        {
            return resourceIds.SplitContains(p =>
            {
                return Query<WipResource>().Where(q => p.Contains(q.Id)).ToList();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetWipResourceCodeIdDic(IEnumerable<string> codes)
        {
            List<BaseDataInfo> wipInfos = new List<BaseDataInfo>();
            codes.SplitDataExecute(temps =>
            {
                var list = Query<WipResource>().Where(p => temps.Contains(p.Code)).Select(p => new
                {
                    Code = p.Code,
                    Id = p.Id,
                }).ToList<BaseDataInfo>();
                wipInfos.AddRange(list);
            });
            return wipInfos.ToDictionary(p => p.Code, p => p.Id);
        }

        /// <summary>
        /// 根据Id获取资源名称
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string WipResourcesName(double Id)
        {
            var wipResource = RF.GetById<WipResource>(Id);
            if (wipResource == null)
                return string.Empty;
            return wipResource.Name;
        }

        /// <summary>
        /// 通过编码获取集合
        /// </summary>
        /// <param name="workshopCodes"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResourceByCodes(List<string> codes)
        {
            var list = codes.SplitContains(c =>
            {
                return Query<WipResource>().Where(p => c.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 获取任意一个资源
        /// </summary>
        /// <returns></returns>
        public virtual double GetOneWipResource()
        {
            return Query<WipResource>().ToList().FirstOrDefault().Id;
        }
    }
}