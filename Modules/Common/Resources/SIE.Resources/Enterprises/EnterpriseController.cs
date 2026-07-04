using DocumentFormat.OpenXml.Office2010.ExcelAc;
using NPOI.SS.Formula.Functions;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ObjectModel;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 企业模型控制器
    /// </summary>
    public partial class EnterpriseController : DomainController
    {
        #region 企业模型 
        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterprises(EnterpriseType type)
        {
            return Query<Enterprise>()
                .Where(p => p.Level.Type == type && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg))
                .ToList();
        }

        /// <summary>
        /// 获取企业模型列表（编码、名称查询）
        /// 企业类型为空获取全部类型企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterprises(EnterpriseType? type, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>();
            if (type != null)
            {
                query.Where(p => p.Level.Type == type);
            }
            query.Where(p => p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterprises(EnterpriseType type, string name, string code)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            var query = Query<Enterprise>()
                .Where(p => p.Level.Type == type)
                .Where(x => x.InvOrgId == 0 || x.InvOrgId == AppRuntime.InvOrg);
            if (name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(name));
            }

            if (code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(code));
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <returns>企业列表（当前库存组织和0库存组织)</returns>
        public virtual EntityList<Enterprise> GetEnterprises()
        {
            return Query<Enterprise>().Where(p => p.InvOrgId == RT.InvOrg || p.InvOrgId == 0).ToList();
        }

        /// <summary>
        /// 获取企业模型列表
        /// </summary>
        /// <param name="parentId">父企业模型ID</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterprises(double parentId)
        {
            return Query<Enterprise>().Where(p => p.TreePId == parentId).ToList();
        }

        /// <summary>
        /// 获取企业模型集合
        /// </summary>
        /// <param name="query">标准查询实体</param>
        /// <returns>企业模型集合</returns>
        public virtual EntityList<Enterprise> GetEnterprises(CriteriaQuery query)
        {
            var queryData = Query<Enterprise>().Where(p => p.InvOrgId == 0 || p.InvOrgId == RT.InvOrg);
            if (query.Criteria != null)
                queryData = queryData.Where(query.Criteria);
            else
            {
                if (queryData.Count() >= 100)
                {
                    queryData = queryData.Where(x => x.TreePId == null || x.TreePId == x.Id);
                }
            }
            return queryData.OrderBy(query.OrderInfoList).ToList(query.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据企业编码和企业类型获得企业模型
        /// </summary>
        /// <param name="code">企业编码</param>
        /// <param name="enterpriseType">企业类型</param>
        /// <returns>企业模型</returns>
        public virtual Enterprise GetEnterprises(string code, EnterpriseType? enterpriseType)
        {
            var query = Query<Enterprise>().Where(p => p.Code == code);
            if (enterpriseType.HasValue)
                query.Join<EnterpriseLevel>((x, y) => x.LevelId == y.Id).Where<EnterpriseLevel>((x, y) => y.Type == enterpriseType.Value);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 根据编码/名称查询企业模型（当前库存组织和通用库存组织）
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns></returns>
        public virtual Enterprise GetEnterprises(Enterprise enterprise)
        {
            return Query<Enterprise>().Where(p => p.InvOrgId == 0).Where(p => p.Code == enterprise.Code || p.Name == enterprise.Name).FirstOrDefault();
        }



        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>企业模型</returns>
        public virtual EntityList<Enterprise> GetEnterprises(List<string> codes)
        {
            var exp = codes.CreateContainsExpression<Enterprise>("x", "Code");
            if (exp == null)
                return new EntityList<Enterprise>();
            return Query<Enterprise>().Where(exp).Where(p => p.InvOrgId == AppRuntime.InvOrg).ToList();
        }

        /// <summary>
        /// 根据Id获取企业模型的数据
        /// </summary>
        /// <param name="idList">Id集合</param>
        /// <returns>返回企业模型的数据</returns>
        public virtual EntityList<Enterprise> GetEnterprises(List<double> idList, EagerLoadOptions elo = null)
        {
            var exp = idList.CreateContainsExpression<Enterprise>("x", "Id");
            if (exp == null)
                return new EntityList<Enterprise>();
            return Query<Enterprise>().Where(exp).ToList(null, elo);
        }


        /// <summary>
        /// 根据编码/名称查询企业模型（通用库存组织）
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns></returns>
        public virtual Enterprise GetInvOrgEnterprises(Enterprise enterprise)
        {
            var queryer = Query<Enterprise>().Where(p => p.InvOrgId == AppRuntime.InvOrg).Where(p => p.Code == enterprise.Code || p.Name == enterprise.Name);
            if (enterprise.Id > 0)
            {
                queryer.Where(x => x.Id != enterprise.Id);
            }
            return queryer.FirstOrDefault();
        }

        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="codes">编码</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterpriseByEmployee(EnterpriseType type, List<string> codes)
        {
            var query = Query<Enterprise>()
                .Where(p => p.Level.Type == type)
                .Where(x => x.InvOrgId == 0 || x.InvOrgId == AppRuntime.InvOrg);

            if (codes.Count > 0)
            {
                query.Exists<WipResource>((x, y) => y.Where(p => p.WorkShopId != null && p.WorkShopId == x.Id && codes.Contains(p.Code)));
            }

            return query.ToList();
        }

        /// <summary>
        /// 根据员工ID获取所属车间
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns>车间列表</returns>
        public virtual EntityList<Enterprise> GetEnterpriseByEmployee(double employeeId)
        {
            return Query<Enterprise>()
                  .Join<WipResource>((e, w) => e.Id == w.WorkShopId)
                  .Join<WipResource, EmployeeResource>((w, er) => w.Id == er.ResourceId && er.EmployeeId == employeeId)
                  .Distinct()
                  .ToList();
        }

        /// <summary>
        /// 获取企业模型列表（编码、名称查询）-（勾选了资源）
        /// 企业类型为空获取全部类型企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterpriseShops(EnterpriseType? type, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>();
            if (type != null)
            {
                query.Where(p => p.Level.Type == type);
            }

            query.Where(p => p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg);
            query.Where(p => p.IsResource);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterprisesByType(EnterpriseType type)
        {
            var q = Query<Enterprise>();
            q.Where(p => p.Level.Type == type && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg));
            return q.ToList();
        }

        /// <summary>
        /// 企业模型是否有子级
        /// </summary>
        /// <param name="id">当前企业模型ID</param>
        /// <returns>true/false</returns>
        public virtual bool EnterpriseHasChild(double? id)
        {
            var query = Query<Enterprise>().Where(p => p.TreePId == id);
            return query.ToList().Any();
        }

        /// <summary>
        /// 企业层级是否包含企业模型
        /// </summary>
        /// <param name="levelId">组织层级ID</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>true/false</returns>
        public virtual bool LevelHasResource(double levelId)
        {
            if (levelId <= 0)
            {
                throw new ArgumentNullException(nameof(levelId));
            }

            return Query<Enterprise>()
                .Where(p => p.LevelId == levelId && p.InvOrgId == RT.InvOrg)
                .Count() > 0;
        }

        /// <summary>
        /// 查询资源数量
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <returns>资源数量</returns>
        public virtual int CountEnterprise(string code, string name)
        {
            var query = Query<Enterprise>();
            if (code.IsNotEmpty())
            {
                query.Where(o => o.Code == code);
            }

            if (name.IsNotEmpty())
            {
                query.Where(o => o.Name == name);
            }

            query.Where(e => e.InvOrgId == AppRuntime.InvOrg || e.InvOrgId == 0);
            return query.Count();
        }

        /// <summary>
        /// 根据产线匹配车间
        /// </summary>
        /// <param name="lineIds">产线Id集合</param>
        /// <returns>对应关系数据字典</returns>
        public virtual Dictionary<double, Enterprise> GetLineToShopDics(List<double> lineIds)
        {
            EntityList<WipResource> lineList = new EntityList<WipResource>();
            for (int i = 0; i <= lineIds.Count / 1000; i++)
            {
                var filterLineIds = lineIds.Skip(i * 1000).Take(1000);
                //资源类型(zenglihua:拿掉自定义类型)
                lineList.AddRange(Query<WipResource>().Where(p => filterLineIds.Contains(p.Id) && (p.SourceType == SyncSourceType.Enterprise)).ToList());
            }

            EntityList<Enterprise> shopList = new EntityList<Enterprise>();
            var shopIds = lineList.Select(p => p.WorkShopId).Distinct().ToList();
            for (int i = 0; i <= shopIds.Count / 1000; i++)
            {
                shopList.AddRange(Query<Enterprise>().Where(p => shopIds.Contains(p.Id) && p.Level.Type == EnterpriseType.Shop).ToList());
            }

            var dics = new Dictionary<double, Enterprise>();
            lineList.ForEach(p =>
            {
                dics[p.Id] = shopList.FirstOrDefault(t => t.Id == p.WorkShopId);
            });

            return dics;
        }

        #region 递归获取企业模型数据
        /// <summary>
        /// 根据parentId获取企业模型下一层级的子数据
        /// </summary>
        /// <param name="parentIdList">parentId集合</param>
        /// <returns>返回下一层级的子数据</returns>
        public virtual EntityList<Enterprise> GetSubEnterprises(List<double?> parentIdList)
        {
            return Query<Enterprise>().Where(p => parentIdList.Contains(p.TreePId)).ToList();
        }

        /// <summary>
        /// 获取指定企业模型（可以多个）的所有子节点数据
        /// </summary>
        /// <param name="enterpriseList">指定的企业模型（可以多个）</param>
        /// <param name="parentCountList">对应每一组企业模型需要查找子节点的个数</param>
        /// <returns>返回指定企业模型（可以多个）的所有子节点数据</returns>
        public virtual List<EntityList<Enterprise>> GetALLSubEnterprises(List<EntityList<Enterprise>> enterpriseList, List<int> parentCountList)
        {
            List<double?> parentIdList = new List<double?>();
            int i = 0;

            // 每一组企业模型需要查找子节点的Id增加到parentIdList
            for (i = 0; i < enterpriseList.Count; i++)
            {
                EntityList<Enterprise> groupEnterprise = enterpriseList[i];
                int parentCount = parentCountList[i];
                for (int addIndex = groupEnterprise.Count - parentCount; addIndex < groupEnterprise.Count; addIndex++)
                {
                    if (!parentIdList.Contains(groupEnterprise[addIndex].Id))
                        parentIdList.Add(groupEnterprise[addIndex].Id);
                }
            }

            if (parentIdList.Count == 0) return enterpriseList;

            List<int> newparentCountList = new List<int>();
            EntityList<Enterprise> childEnterprises = GetSubEnterprises(parentIdList);
            if (childEnterprises.Count > 0)
            {
                for (i = 0; i < enterpriseList.Count; i++)
                {
                    EntityList<Enterprise> newChildData = new EntityList<Enterprise>();
                    EntityList<Enterprise> groupEnterprise = enterpriseList[i];
                    int parentCount = parentCountList[i];
                    for (int addIndex = groupEnterprise.Count - parentCount; addIndex < groupEnterprise.Count; addIndex++)
                    {
                        for (int childIndex = 0; childIndex < childEnterprises.Count;)
                        {
                            if (groupEnterprise[addIndex].Id == childEnterprises[childIndex].TreePId)
                            {
                                newChildData.Add(childEnterprises[childIndex]);
                                childEnterprises.RemoveAt(childIndex);
                            }
                            else
                            {
                                childIndex++;
                            }
                        }
                    }

                    groupEnterprise.AddRange(newChildData);
                    newparentCountList.Add(newChildData.Count);
                }

                GetALLSubEnterprises(enterpriseList, newparentCountList);
            }

            return enterpriseList;
        }

        /// <summary>
        /// 递归获取上级企业模型
        /// </summary>
        /// <param name="allEnterpriseIds">所有企业模型ID列表</param>
        /// <param name="newEnterpriseIds">新的企业模型ID列表</param>
        public virtual void GetAllParentEnerprise(List<double> allEnterpriseIds, List<double> newEnterpriseIds)
        {
            allEnterpriseIds.AddRange(newEnterpriseIds);

            EntityList<Enterprise> newEnterpriseList = GetEnterprises(newEnterpriseIds);

            List<double> newParentEnterpriseIdList = (from Enterprise p in newEnterpriseList
                                                      where p.TreePId != null && !allEnterpriseIds.Contains(p.TreePId.Value)
                                                      select p.TreePId.Value).ToList();
            if (newParentEnterpriseIdList.Count > 0)
            {
                GetAllParentEnerprise(allEnterpriseIds, newParentEnterpriseIdList);
            }
        }
        #endregion


        /// <summary>
        /// 根据企业层级获取对应企业模型的数据
        /// </summary>
        /// <param name="levelId">企业层级Id</param>
        /// <returns>返回企业模型数据</returns>
        public virtual EntityList<Enterprise> GetEnterpriseFromLevel(double levelId)
        {
            return Query<Enterprise>().Where(p => p.LevelId == levelId).ToList();
        }

        /// <summary>
        /// 根据企业层级获取对应企业模型的数据
        /// </summary>
        /// <param name="levelId">企业层级Id</param>
        /// <returns>返回企业模型数据</returns>
        public virtual EntityList<Enterprise> GetEnterpriseFromLevels(double levelId)
        {
            return Query<Enterprise>().Where(p => p.LevelId == levelId).ToList();
        }

        /// <summary>
        /// 根据企业层级获取对应企业模型的数据
        /// </summary>
        /// <param name="levelIdList">企业层级Id集合</param>
        /// <returns>返回企业模型数据</returns>
        public virtual bool IsExistRourceEnterprise(List<double?> levelIdList)
        {
            return Query<Enterprise>().Where(p => levelIdList.Contains(p.LevelId) && p.IsResource).Count() > 0;
        }

        /// <summary>
        /// 获取是资源的企业模型
        /// </summary>
        /// <returns>企业模型</returns>
        public virtual EntityList<Enterprise> GetEnterprisesIsRes()
        {
            var q = Query<Enterprise>();
            q.Where(p => (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg));
            return q.ToList();
        }

        /// <summary>
        /// 同步企业模型到资源
        /// </summary>       
        /// <returns>报错信息</returns>
        public virtual string SyncEnterprise()
        {
            StringBuilder sb = new StringBuilder();
            EntityList<Enterprise> enterpriseList = GetEnterprisesIsRes();
            List<Enterprise> itemList = enterpriseList.Where(p => p.IsResource).ToList();
            // KEY:企业模型ID，value：企业模型
            Dictionary<double, Enterprise> enterpriseDic = enterpriseList.ToDictionary(p => p.Id);
            var srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.Enterprise }, null, string.Empty);
            if (itemList.Count > 0)
            {
                foreach (var item in itemList)
                {
                    try
                    {
                        WipResource src = srlist.FirstOrDefault(p => p.Code == item.Code);
                        if (src == null)
                        {
                            src = new WipResource();
                            src.Code = item.Code;
                            src.Name = item.Name;
                            src.WorkShopId = GetWorkShop(item, enterpriseDic, EnterpriseType.Shop);
                            src.FactoryId = GetWorkShop(item, enterpriseDic, EnterpriseType.Plant);
                            src.AutomationType = AutomationType.FullAutomatic;
                            src.ResourceState = ResourceState.Actived;
                            src.SourceType = SyncSourceType.Enterprise;
                            src.SourceId = item.Id;
                            src.Scheme = RT.Service.Resolve<CalendarSchemeController>().GetDefaultCalendar();
                            src.Qty = 1;
                            src.TaktTime = 1;
                        }
                        else
                        {
                            src.Code = item.Code;
                            src.Name = item.Name;
                            src.SourceId = item.Id;

                            var workShopId = GetWorkShop(item, enterpriseDic, EnterpriseType.Shop);
                            if (src.WorkShopId != workShopId) src.WorkShopId = workShopId;
                            var facotryId = GetWorkShop(item, enterpriseDic, EnterpriseType.Plant);
                            if (src.FactoryId != facotryId) src.FactoryId = facotryId;
                            if (src.ResourceState == ResourceState.Diseffect)
                            {
                                src.ResourceState = ResourceState.Actived;
                            }
                        }

                        if (src.PersistenceStatus != PersistenceStatus.Unchanged)
                        {
                            RF.Save(src);
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine("同步企业模型{0}失败：{1}".L10nFormat(item.Code, ex.Message));
                    }
                }
            }

            srlist = RT.Service.Resolve<WipResourceController>().GetWipResourceBySrcType(new List<SyncSourceType>() { SyncSourceType.Enterprise }, null, string.Empty);
            if (srlist.Count > 0)
            {
                var codes = srlist.Select(p => p.Code).Distinct().ToList();
                var itemListCodes = itemList.Select(p => p.Code).Distinct().ToList();

                codes = codes.Except(itemListCodes).ToList();
                //把已经不存在的企业模型变成失效状态
                DB.Update<WipResource>().Set(p => p.ResourceState, ResourceState.Diseffect).Where(p => codes.Contains(p.Code) && p.SourceType == SyncSourceType.Enterprise).Execute();
            }


            return sb.ToString();
        }

        /// <summary>
        /// 递归获取企业模型的所属车间
        /// </summary>
        /// <param name="enter">企业模型</param>
        /// <param name="enterpriseDic">企业模型集合</param>
        /// <param name="type">企业类型</param>
        /// <returns>返回指定类型的ID</returns>
        public virtual double? GetWorkShop(Enterprise enter, Dictionary<double, Enterprise> enterpriseDic, EnterpriseType type)
        {
            if (enter.Level.Type == type)
            {
                return enter.Id;
            }

            if (enter.TreePId == null)
                return null;

            Enterprise preEnterprise = null;
            if (enterpriseDic.TryGetValue(enter.TreePId.Value, out preEnterprise))
            {
                return GetWorkShop(preEnterprise, enterpriseDic, type);
            }

            return null;
        }

        /// <summary>
        /// 获取生产资源的车间集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <param name="factoryId">工厂Id</param>
        /// <returns>车间集合</returns>
        public virtual EntityList<Enterprise> GetResourceWorkShops(PagingInfo pagingInfo, string keyword, double? factoryId = null)
        {
            var query = Query<Enterprise>().Join<WipResource>((x, y) => x.Id == y.WorkShopId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (factoryId != null)
                query.Where<WipResource>((x, y) => y.FactoryId == factoryId);
            return query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工厂下的车间
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetDepartmentWorkShops(double? factoryId, PagingInfo pagingInfo, string keyword)
        {
            if (factoryId == null || factoryId == 0)
            {
                return new EntityList<Enterprise>();
            }
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Shop && p.InvOrgId == RT.InvOrg);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            var enterpriseIds = new List<double>();
            this.GetEnterpriseTreeUnderIds(factoryId.Value, enterpriseIds);
            var list = enterpriseIds.SplitContains(tempIds =>
            {
                return query.Where(p => enterpriseIds.Contains(p.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            });
            list.ForEach(p => p.TreePId = null);
            return list;
        }

        /// <summary>
        /// 获取车间集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <param name="upId">上级ID，父级以上ID</param>
        /// <returns>车间集合</returns>
        public virtual EntityList<Enterprise> GetSynResources(PagingInfo pagingInfo, string keyword, double? upId)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Line && p.InvOrgId == RT.InvOrg);

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            if (upId.HasValue)
            {
                //查询upId以下层级的企业模型数据的ID
                var enterpriseIds = new List<double>();
                this.GetEnterpriseTreeUnderIds(upId.Value, enterpriseIds);

                var list = enterpriseIds.SplitContains(tempIds =>
                {
                    return query.Where(x => tempIds.Contains(x.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                });

                list.ForEach(x => x.TreePId = null);

                return list;
            }
            else
            {
                var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

                list.ForEach(x => x.TreePId = null);

                return list;
            }
        }

        /// <summary>
        /// 获取车间集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <param name="upId">上级ID，父级以上ID</param>
        /// <returns>车间集合</returns>
        public virtual EntityList<Enterprise> GetWorkShops(PagingInfo pagingInfo, string keyword, double? upId)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Shop && p.InvOrgId == RT.InvOrg);

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            if (upId.HasValue)
            {
                //查询upId以下层级的企业模型数据的ID
                var enterpriseIds = new List<double>();
                this.GetEnterpriseTreeUnderIds(upId.Value, enterpriseIds);

                var list = enterpriseIds.SplitContains(tempIds =>
                {
                    return query.Where(x => tempIds.Contains(x.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                });

                list.ForEach(x => x.TreePId = null);

                return list;
            }
            else
            {
                var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

                list.ForEach(x => x.TreePId = null);

                return list;
            }
        }

        /// <summary>
        /// 根据名称或编号获取车间id列表
        /// </summary>
        /// <param name="nameOrCode"></param>
        /// <returns></returns>
        public virtual List<double> GetShopIdsByNameOrCode(string nameOrCode)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Shop && p.InvOrgId == RT.InvOrg);
            if (nameOrCode.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(nameOrCode) || p.Code.Contains(nameOrCode));
            }
            return query.Select(p => p.Id).ToList<double>().ToList();
        }



        #region 递归获取企业模型数据
        /// <summary>
        /// 根据parentId获取企业模型下一层级的子数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>返回下一层级的子数据</returns>
        public virtual EntityList<Enterprise> GetSubEnterprise(double? parentId)
        {
            return Query<Enterprise>().Where(x => x.TreePId == parentId).Where(p => p.InvOrgId == RT.InvOrg || p.InvOrgId == 0)
               .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工厂Id获取下属所有的车间
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="allWorkShop"></param>
        /// <returns></returns>
        public virtual List<Enterprise> GetWorkShopByFactoryId(double? parentId, List<Enterprise> allWorkShop)
        {
            if (allWorkShop == null)
            {
                return new List<Enterprise>();
            }
            EntityList<Enterprise> childEnterprises = GetSubEnterprise(parentId);

            foreach (var item in childEnterprises)
            {
                if (item.Level.Type == EnterpriseType.Shop)
                {
                    allWorkShop.Add(item);
                }
                GetWorkShopByFactoryId(item.Id, allWorkShop);
            }
            return allWorkShop;
        }


        /// <summary>
        /// 获取全部的企业模型数据
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetEnterpriseAll()
        {
            return Query<Enterprise>()
                .Where(x => x.InvOrgId == 0 || x.InvOrgId == RT.InvOrg)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        #endregion

        /// <summary>
        /// 获取工厂Id与编码名称字典。
        /// </summary>
        /// <param name="factoryIds">工厂ID列表</param>
        /// <returns>key：工厂ID；value：工厂编码、工厂名称</returns>
        public virtual Dictionary<double, Tuple<string, string>> GetFactoryIdCodeAndNameDic(List<double> factoryIds)
        {
            List<EnterpriseInfo> infos = new List<EnterpriseInfo>();
            factoryIds.SplitDataExecute(ids =>
            {
                var query = Query<Enterprise>()
                    .Where(p => p.Level.Type == EnterpriseType.Plant && ids.Contains(p.Id))
                    .Select(p => new
                    {
                        WorkShopId = p.Id,
                        WorkShopCode = p.Code,
                        WorkShopName = p.Name
                    }).ToList<EnterpriseInfo>().ToList();
                infos.AddRange(query);
            });
            return infos.GroupBy(p => p.WorkShopId).ToDictionary(p => p.Key, p => new Tuple<string, string>(p.First().WorkShopCode, p.First().WorkShopName));
        }

        /// <summary>
        /// 获取职员所属的工厂集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工厂集合</returns>
        public virtual EntityList<Enterprise> GetEmployeeFactoriesList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>()
                .Join<EmployeeEnterprise>((x, y) => x.Id == y.EnterpriseId)
               // .Where<EmployeeEnterprise>((x, y) => y.EmployeeId == RT.IdentityId)
                .Where(p => p.Level.Type == EnterpriseType.Plant);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取职员所属的工厂集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>工厂集合</returns>
        public virtual EntityList<Enterprise> GetFactoriesList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>()
                .Where(p => p.Level.Type == EnterpriseType.Plant && p.InvOrgId == RT.InvOrg);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工厂名称获取当前库存组织的工厂对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual Enterprise GetFactoryByCode(string code)
        {
            return Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Plant && p.InvOrgId == RT.InvOrg)
                .Where(m => m.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 根据工厂名称获取当前库存组织的工厂对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Enterprise GetFactoryByName(string name)
        {
            return Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Plant && p.InvOrgId == RT.InvOrg)
                .Where(m => m.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 根据工厂名称获取当前库存组织的工厂对象
        /// </summary>
        /// <param name="cds"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetFactoryByCode(List<string> cds)
        {
            return cds.SplitContains(tempCds =>
            {
                return Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Plant && p.InvOrgId == RT.InvOrg)
                .Where(m => tempCds.Contains(m.Code)).ToList();
            });
        }

        /// <summary>
        /// 获取职员所属的工厂集合
        /// </summary>
        /// <param name="code"></param>
        /// <param name="enterpriseType"></param>
        /// <returns></returns>
        public virtual Enterprise GetEmployeeFactories(string code, EnterpriseType? enterpriseType)
        {
            var query = Query<Enterprise>()
                .Join<EmployeeEnterprise>((x, y) => x.Id == y.EnterpriseId)
                .Where<EmployeeEnterprise>((x, y) => y.EmployeeId == RT.IdentityId)
                .Where(p => p.Level.Type == enterpriseType);
            if (!code.IsNullOrEmpty())
                query.Where(p => p.Code == code);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取职员所属的工厂Id
        /// </summary>
        /// <param name="factoryName">工厂名称</param>
        /// <returns>工厂集合</returns>
        public virtual double? GetFactoryIdByName(string factoryName)
        {
            var query = Query<Enterprise>()
                .Join<EmployeeEnterprise>((x, y) => x.Id == y.EnterpriseId)
                .Where<EmployeeEnterprise>((x, y) => y.EmployeeId == RT.IdentityId)
                .Where(p => p.Level.Type == EnterpriseType.Plant && p.Name == factoryName);
            return query.FirstOrDefault()?.Id;
        }

        /// <summary>
        /// 获取职员所属的工厂Id
        /// </summary>
        /// <param name="factoryCode">工厂编码</param>
        /// <returns>工厂集合</returns>
        public virtual double? GetFactoryIdByCode(string factoryCode)
        {
            var query = Query<Enterprise>()
                .Join<EmployeeEnterprise>((x, y) => x.Id == y.EnterpriseId)
                .Where<EmployeeEnterprise>((x, y) => y.EmployeeId == RT.IdentityId)
                .Where(p => p.Level.Type == EnterpriseType.Plant && p.Code == factoryCode);
            return query.FirstOrDefault()?.Id;
        }

        /// <summary>
        /// 递归遍历企业模型树子ID
        /// </summary>
        /// <param name="enterpriseId">最上级ID</param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual void GetEnterpriseTreeUnderIds(double enterpriseId, List<double> ids)
        {
            ids.Add(enterpriseId);
            var list = Query<Enterprise>().Where(p => p.TreePId == enterpriseId && p.InvOrgId == RT.InvOrg).ToList();
            foreach (var item in list)
                GetEnterpriseTreeUnderIds(item.Id, ids);
        }

        /// <summary>
        /// 获取产线集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pId">父ID</param>
        /// <returns>车间集合</returns>
        public virtual EntityList<Enterprise> GetLines(PagingInfo pagingInfo, string keyword, double? pId)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Line && p.InvOrgId == RT.InvOrg);
            if (pId.HasValue)
                query.Where(p => p.TreePId == pId);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取生产资源的部门集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>部门集合</returns>
        public virtual EntityList<Enterprise> GetResourceDepartments(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>()
                .Join<EnterpriseLevel>((x, y) => x.LevelId == y.Id && y.Type == EnterpriseType.Department)
                .Join<WipResource>((m, n) => m.Id == n.SourceId && n.SourceType == SyncSourceType.Enterprise && n.ResourceState == ResourceState.Actived);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="code">部门编码</param>
        /// <returns>部门</returns>
        public virtual Enterprise GetDepartmentByCode(string code)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg && p.Code == code);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="code">部门编码</param>
        /// <returns>部门</returns>
        public virtual EntityList<Enterprise> GetDepartmentByCode(List<string> codes)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg && codes.Contains(p.Code));
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取部门集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>部门集合</returns>
        public virtual EntityList<Enterprise> GetDepartments(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取部门集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>部门集合</returns>
        public virtual EntityList<Enterprise> GetDepartmentsNoTree(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            var list = query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            foreach(var i in list)
            {
                i.TreePId = null;
            }
            return list;
        }

        /// <summary>
        /// 获取部门集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <returns>部门集合</returns>
        public virtual List<BaseDataInfo> GetDepartmentsBaseInfo(string keyword)
        {
            var query = Query<Enterprise>()
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg)
                .Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                }).ToList<BaseDataInfo>().ToList();
            return query;
        }

        /// <summary>
        /// 获取工厂下的部门集合
        /// </summary>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="keyword">关键字</param>
        /// <param name="factoryId">工厂id</param>
        /// <returns>部门集合</returns>
        public virtual EntityList<Enterprise> GetDepartments(PagingInfo pagingInfo, string keyword, double? factoryId)
        {
            if (factoryId == null || factoryId == 0)
            {
                throw new ValidationException("请先维护工厂！".L10N());
            }
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (factoryId != null)
            {
                query.Where(p => p.TreePId == factoryId);
            }
            return query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前工厂下所有的部门
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetAllDepartmentWithFactory(double? factoryId, PagingInfo pagingInfo, string keyword)
        {
            if (factoryId == null || factoryId == 0)
            {
                return new EntityList<Enterprise>();
            }
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            var enterpriseIds = new List<double>();
            this.GetEnterpriseTreeUnderIds(factoryId.Value, enterpriseIds);
            var list = enterpriseIds.SplitContains(tempIds =>
            {
                return query.Where(p => enterpriseIds.Contains(p.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            });
            list.ForEach(p => p.TreePId = null);
            return list;
        }

        /// <summary>
        /// 获取当前工厂下所有的部门,如果工厂为空返回所有的部门数据
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetAllDepartmentsWithFactoryOrAll(double? factoryId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg);
            if (factoryId == null || factoryId == 0)
            {
                var list = query.WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                    .Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var item in list)
                {
                    item.TreePId = null;
                }
                return list;
            }

            else
            {
                if (keyword.IsNotEmpty())
                {
                    query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                }
                var enterpriseIds = new List<double>();
                this.GetEnterpriseTreeUnderIds(factoryId.Value, enterpriseIds);
                var list = enterpriseIds.SplitContains(tempIds =>
                {
                    return query.Where(p => enterpriseIds.Contains(p.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                });
                list.ForEach(p => p.TreePId = null);
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetDepartmentsWithParent(PagingInfo pagingInfo, string keyword)
        {
            var departments = GetDepartments(pagingInfo, keyword);

            if (departments == null || departments.Count <= 0)
                return new EntityList<Enterprise>();

            List<double> allEnterpriseIds = new List<double>();
            List<double> newEnterpriseIds = departments.Select(x => x.Id).ToList();

            GetAllParentEnerprise(allEnterpriseIds, newEnterpriseIds);

            return GetEnterprises(allEnterpriseIds);
        }
        /// <summary>
        /// 企业层级是否包含企业模型
        /// </summary>
        /// <param name="enterId">组织层级ID</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>true/false</returns>
        public virtual bool EnterpriseIsResource(double? enterId)
        {
            return Query<Enterprise>()
                .Where(p => p.IsResource && p.Id == enterId)
                .Count() > 0;
        }

        /// <summary>
        /// 根据ID获取企业模型
        /// </summary>
        /// <param name="enterId">企业模型ID</param>
        /// <returns>企业模型</returns>
        public virtual Enterprise GetEnterpriseById(double enterId)
        {
            return Query<Enterprise>().Where(p => p.Id == enterId).FirstOrDefault();
        }

        /// <summary>
        /// 根据ID获取企业模型
        /// </summary>
        /// <param name="enterIds">企业模型ID</param>
        /// <returns>企业模型</returns>
        public virtual EntityList<Enterprise> GetEnterpriseByIds(List<double> enterIds)
        {
            return enterIds.SplitContains(temps => { return Query<Enterprise>().Where(p => temps.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()); });
        }

        /// <summary>
        /// 根据产线ID列表  获取产线
        /// </summary>
        /// <param name="lineIds">产线Id列表</param>
        /// <returns>产线清单</returns>
        public virtual EntityList<Enterprise> GetLinesByIds(List<double> lineIds)
        {
            return Query<Enterprise>().Where(p => lineIds.Contains(p.Id) && p.Level.Type == EnterpriseType.Line).ToList();
        }

        /// <summary>
        /// 根据车间ID列表  获取车间
        /// </summary>
        /// <param name="workshopIds">车间Id列表</param>
        /// <returns>车间清单</returns>
        public virtual EntityList<Enterprise> GetWorkShopByIds(List<double> workshopIds)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(WipResource.FactoryProperty);
            // elo.LoadWith(WipResource.WorkShopProperty);
            //return Query<WipResource>()
            //    .Where(p => p.ResourceState == ResourceState.Actived && p.ProcessTechTypeId > 0)
            //    .OrderBy(p => new { p.Sequence, p.Name })
            //    .ToList(null, elo);

            return Query<Enterprise>().Where(p => workshopIds.Contains(p.Id) && p.Level.Type == EnterpriseType.Shop).ToList(null, elo);
        }

        /// <summary>
        /// 通过编码集合获取车间集合
        /// </summary>
        /// <param name="workshopCodes"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetWorkShopByCodes(List<string> workshopCodes)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(WipResource.FactoryProperty);

            return Query<Enterprise>().Where(p => workshopCodes.Contains(p.Code) && p.Level.Type == EnterpriseType.Shop).ToList(null, elo);
        }


        /// <summary>
        /// 根据用户ID查找关联员工所对应资源
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="code">资源编码</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>资源列表</returns> 
        public virtual EntityList<Enterprise> GetLineByUserId(double userId, string code, PagingInfo pagingInfo)
        {
            var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByUserId(userId);
            if (employee == null)
                return new EntityList<Enterprise>();
            return Query<Enterprise>().Join<EmployeeResource>((x, y) => x.Id == y.ResourceId
               && y.EmployeeId == employee.Id
               && x.Level.Type == EnterpriseType.Line)
                .Where(p => p.Code.Contains(code))
                .ToList(pagingInfo);
        }

        /// <summary>
        /// 根据员工ID获取部门
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>员工部门，未找到员工部门抛异常</returns>
        public virtual Enterprise GetEmployeeDepartment(double employeeId)
        {
            var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeById(employeeId);
            return ValidateEmplyeeDepartment(employee);
        }

        /// <summary>
        /// 验证员工部门信息
        /// </summary>
        /// <param name="employee">员工ID</param>
        /// <returns>部门</returns>
        private Enterprise ValidateEmplyeeDepartment(Employees.Employee employee)
        {
            if (employee == null)
                throw new EntityNotFoundException("未找到员工".L10N());
            if (employee.WorkGroup == null)
                throw new ValidationException("请在班组中维护员工和部门的关系".L10N());
            var department = employee.WorkGroup.Department;
            if (department == null)
                throw new ValidationException("请在班组中维护部门的信息".L10N());
            return department;
        }

        /// <summary>
        /// 获取工厂下的车间
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <param name="type">企业类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>车间列表</returns>
        public virtual EntityList<Enterprise> GetEnterprisesNew(double? factoryId, EnterpriseType? type, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>();
            if (type != null)
            {
                query.Where(p => p.Level.Type == type && p.TreePId == factoryId);
            }

            query.Where(p => p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取对应企业类型的工厂信息/车间信息
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="context">编码/名称</param>
        /// <returns>返回工厂信息/车间信息</returns>
        public virtual Enterprise GetEnterprisesImport(EnterpriseType? type, string context)
        {
            return Query<Enterprise>()
                .Where(p => p.Level.Type == type && (p.Name == context || p.Code == context) && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg))
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取对应工厂下的车间信息
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <param name="context">编码/名称</param>
        /// <returns>返回车间信息</returns>
        public virtual Enterprise GetWorkShopInfos(double? factoryId, string context)
        {
            return Query<Enterprise>()
                .Where(p => p.TreePId == factoryId && (p.Code == context || p.Name == context) && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg))
                .FirstOrDefault();
        }

        /// <summary>
        /// 根据产线id获取工厂
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual Enterprise GetFactoryByWipId(double wipId)
        {
            Enterprise enterprise = new Enterprise();
            var wip = RF.GetById<WipResource>(wipId);
            if (wip != null)
            {
                enterprise = RF.GetById<Enterprise>(wip.FactoryId);
            }
            return enterprise;
        }

        /// <summary>
        /// 获取企业模型列表（编码、名称查询）（根据员工权限筛选）
        /// 企业类型为空获取全部类型企业模型
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>企业模型列表</returns>
        public virtual EntityList<Enterprise> GetEnterprisesConsideringEmployee(EnterpriseType? type, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Enterprise>().Join<EmployeeEnterprise>((en, em) => em.EmployeeId == RT.IdentityId && en.Id == em.EnterpriseId);
            if (type != null)
            {
                query.Where(p => p.Level.Type == type);
            }
            query.Where(p => p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据名称获取对应企业模型
        /// </summary>
        /// <param name="names">名称</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetEnterprisesByNames(List<string> names, EnterpriseType? type)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            names.SplitDataExecute(temps =>
            {
                var list = Query<Enterprise>()
                .WhereIf(type != null, p => p.Level.Type == type)
                .Where(p => temps.Contains(p.Name) && p.InvOrgId == RT.InvOrg)
                .Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                }).ToList<BaseDataInfo>().ToList();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos;
        }
        #endregion

        #region 企业层级 
        /// <summary>
        /// 企业层级是否有子级
        /// </summary>
        /// <param name="id">当前企业层级ID</param>
        /// <returns>true/false</returns>
        public virtual bool EnterpriseLevelHasChild(double id)
        {
            return Query<EnterpriseLevel>().Where(p => p.TreePId == id).Count() > 0;
        }

        /// <summary>
        /// 设置企业层级是否资源
        /// </summary>
        /// <param name="id">企业层级ID</param>
        /// <param name="isResource">是否资源</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <exception cref="EntityNotFoundException">找不到实体</exception>
        /// <returns>企业层级</returns>
        public virtual EnterpriseLevel SetResource(double id, bool isResource)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var level = RF.GetById<EnterpriseLevel>(id);
            if (level == null)
            {
                throw new EntityNotFoundException(typeof(EnterpriseLevel), id);
            }

            level.IsResource = isResource;
            RF.Save(level);
            return level;
        }

        /// <summary>
        /// 根据编码/名称查询企业层级（当前库存组织和通用库存组织）
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual EnterpriseLevel GetEnterpriseLevel(EnterpriseLevel level)
        {
            var queryer = Query<EnterpriseLevel>().Where(p => p.InvOrgId == AppRuntime.InvOrg).Where(p => p.Code == level.Code || p.Name == level.Name);
            if (level.Id > 0)
            {
                queryer.Where(x => x.Id != level.Id);
            }
            return queryer.FirstOrDefault();
        }

        /// <summary>
        /// 获取企业层级
        /// Create : shilei
        /// </summary>
        /// <param name="type">企业层级类型</param>
        /// <param name="invOrgId">库存组织</param>
        /// <returns>EnterpriseLevel</returns>
        public virtual EnterpriseLevel GetEnterpriseLevel(EnterpriseType? type, int? invOrgId)
        {
            var level = Query<EnterpriseLevel>().Where(p => p.Type == type && (p.InvOrgId == 0 || p.InvOrgId == invOrgId)).FirstOrDefault();
            return level;
        }

        /// <summary>
        /// 获取企业层级
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">下拉查询条件</param>
        /// <param name="levelId">父企业层级ID</param>
        /// <returns>企业层级列表</returns>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        public virtual EntityList<EnterpriseLevel> GetEnterpriseLevelsByParentId(PagingInfo pagingInfo, string keyword, double levelId)
        {
            if (levelId <= 0)
            {
                throw new ArgumentNullException(nameof(levelId));
            }

            var query = Query<EnterpriseLevel>();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.Where(p => p.TreePId == levelId && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg)).ToList(pagingInfo);
        }


        /// <summary>
        /// 获取企业模型
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">下拉查询条件</param>
        /// <param name="parentId">父企业层级ID</param>
        /// <param name="type">下级企业模型的类型</param>
        /// <returns>企业层级列表</returns>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        public virtual EntityList<Enterprise> GetEnterpriseByParentId(PagingInfo pagingInfo, string keyword, double? parentId, EnterpriseType? type)
        {
            if (parentId == null)
            {
                return new EntityList<Enterprise>();
            }
            var query = Query<Enterprise>();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            if (type != null)
            {
                query.Where(p => p.Level.Type == type);
            }
            return query.Where(p => p.TreePId == parentId).ToList(pagingInfo);
        }


        /// <summary>
        /// 查询企业层级数量
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <returns>企业层级数量</returns>
        public virtual int CountEnterpriseLevel(string code, string name)
        {
            var query = Query<EnterpriseLevel>();
            if (code.IsNotEmpty())
            {
                query.Where(o => o.Code == code);
            }

            if (name.IsNotEmpty())
            {
                query.Where(o => o.Name == name);
            }

            query.Where(o => o.Name == name && o.Code == code && (o.InvOrgId == AppRuntime.InvOrg || o.InvOrgId == 0));
            return query.Count();
        }

        /// <summary>
        /// 获取企业层级集合
        /// </summary>
        /// <param name="query">标准查询实体</param>
        /// <returns>企业层级集合</returns>
        public virtual EntityList<EnterpriseLevel> GetEnterpriseLevels(CriteriaQuery query)
        {
            return Query<EnterpriseLevel>().Where(p => p.InvOrgId == 0 || p.InvOrgId == RT.InvOrg).Where(query.Criteria)
                .OrderBy(query.OrderInfoList).ToList(query.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取企业层级
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>企业层级</returns>
        public virtual EntityList<EnterpriseLevel> GetEnterpriseLevels(List<string> codes)
        {
            return Query<EnterpriseLevel>().Where(p => codes.Contains(p.Code)).ToList();
        }

        #endregion

        #region 资源 
        /// <summary>
        /// 获取资源的企业模型
        /// </summary>
        /// <param name="pagingInfo">pagingInfo</param>
        /// <param name="keyword">keyword</param>
        /// <param name="parentId">parentId</param>
        /// <returns>资源列表</returns>
        public virtual EntityList<Resource> GetResources(PagingInfo pagingInfo, string keyword, double parentId = 0)
        {
            var query = Query<Resource>()
                .Where(p => p.InvOrgId == AppRuntime.InvOrg || p.InvOrgId == 0);
            if (parentId > 0)
                query.Where(p => p.TreePId == parentId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>资源列表</returns>
        public virtual EntityList<Resource> GetResources(EnterpriseType type, PagingInfo pagingInfo)
        {
            var q = Query<Resource>()
                .Where(p => p.Level.Type == type && p.Level.IsResource && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg));
            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取资源集合
        /// </summary>
        /// <param name="query">标准查询实体</param>
        /// <returns>资源集合</returns>
        public virtual EntityList<Resource> GetResources(CriteriaQuery query)
        {
            return Query<Resource>()
                .Where(p => p.InvOrgId == 0 || p.InvOrgId == RT.InvOrg)
                .Where(query.Criteria)
                .Where(p => p.Level.IsResource)
                .OrderBy(query.OrderInfoList)
                .ToList(query.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据资源编码获取资源模型
        /// </summary>
        /// <param name="code">资源编码</param>
        /// <returns>资源模型</returns>
        public virtual WipResource GetResources(string code)
        {
            return Query<WipResource>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="type">企业类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>资源列表</returns>
        public virtual EntityList<Resource> GetResources(EnterpriseType type, PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Resource>();
            q.Where(p => p.Level.Type == type && (p.InvOrgId == 0 || p.InvOrgId == AppRuntime.InvOrg));
            q.WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual WipResource GetEquipment(string code)
        {
            return Query<WipResource>().Where(p => p.Code == code && p.SourceType == SyncSourceType.Equipment).FirstOrDefault();
        }

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<WipResource> QueryEquipment(PagingInfo page, string key, double? factoryId)
        {
            var q = Query<WipResource>();
            if (!string.IsNullOrWhiteSpace(key))
                q.Where(t => t.Name.Contains(key) || t.Code.Contains(key));
            if (factoryId != null)
                q.Where(t => t.FactoryId == factoryId);
            return q.ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 递归获取资源ID集合
        /// </summary>
        /// <param name="resourceId">父资源ID</param>
        /// <returns>资源ID集合</returns>
        public virtual IEnumerable<double> GetChildResourceId(double resourceId)
        {
            yield return resourceId;
            var child = Query<Resource>().Where(p => p.TreePId == resourceId).ToList();
            foreach (Resource resource in child)
                GetChildResourceId(resource.Id);
        }

        /// <summary>
        /// 根据资源名称获取资源
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>资源</returns>
        public virtual Resource GetResourceByName(string name)
        {
            return Query<Resource>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 得到资源Id的父组织的所有产线资源
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <returns>父组织的所有产线资源</returns>
        public virtual EntityList<Resource> GetLineResources(double resourceId)
        {
            var curResource = Query<Resource>().Where(p => p.Id == resourceId).FirstOrDefault();
            if (curResource == null)
                throw new ArgumentException(nameof(curResource));
            var resources = Query<Resource>().Where(p => p.TreePId == curResource.TreePId && p.Level.Type == EnterpriseType.Line).ToList();
            return resources;
        }

        /// <summary>
        /// 获取所有产线资源
        /// </summary>
        /// <returns>所有产线资源</returns>
        public virtual EntityList<Resource> GetAllLineResouuces()
        {
            var resources = Query<Resource>().Where(p => p.Level.Type == EnterpriseType.Line).ToList();
            return resources;
        }

        /// <summary>
        /// 通过主工厂编码获得副工厂编码
        /// </summary>
        /// <param name="doucenterpriseCodes">主工厂编码</param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetAssistantInvOrg(List<string> doucenterpriseCodes)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                return Query<Enterprise>().Where(p => p.InvOrgId != AppRuntime.InvOrg.Value && doucenterpriseCodes.Contains(p.Code)).ToList();
            }
        }


        /// <summary>
        /// 通过Id列表获取企业的name和其对应id的字典
        /// </summary>
        /// <param name="Ids">企业Id列表</param>
        public virtual Dictionary<double, string> GetEnterpriseIdAndNameDic(List<double> Ids)
        {
            return Query<Enterprise>()
                .Where(p => Ids.Contains(p.Id))
                .ToList().GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.First().Name);
        }
        #endregion
    }
}