using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Controllers;
using SIE.Core.ProjectMaintains;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Configs;
using SIE.Items.ProductBoms;
using SIE.Items.ProductBoms.Models;
using SIE.MES.ProjectDesigns.ApiModels;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MES.ProjectDesigns.Enums;
using SIE.MES.Projects;
using SIE.MES.Projects.Enums;
using SIE.MES.Routings.RoutingBoms;
using SIE.MES.Routings.RoutingSettings;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Runtime;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SIE.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计控制器
    /// </summary>
    public class ProjectDesignController : DomainController
    {
        #region 查询
        /// <summary>
        /// 查询实体查询
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<ProjectDesign> QueryProjectDesign(ProjectDesignCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<ProjectDesign>();
            }
            var q = Query<ProjectDesign>();
            if (criteria.ProjectMaintainId != null && criteria.ProjectMaintainId != 0)
            {
                q.Where(p => p.ProjectMaintainId == criteria.ProjectMaintainId);
            }
            if (criteria.ProductId != null && criteria.ProductId != 0)
            {
                q.Where(p => p.ProductId == criteria.ProductId);
            }
            if (criteria.SaleOrderNo.IsNotEmpty())
            {
                q.Where(p => p.SaleOrderNo.Contains(criteria.SaleOrderNo));
            }
            if (criteria.State.HasValue)
            {
                q.Where(p => p.State == criteria.State.Value);
            }
            if (criteria.ExamineStatus.HasValue)
            {
                q.Where(p => p.ExamineStatus == criteria.ExamineStatus.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询当前项目号设计下是否有基本属性
        /// </summary>
        /// <param name="designId"></param>
        /// <returns></returns>
        public virtual bool HasBasicProperty(double designId)
        {
            return Query<DesignBasicProperty>().Where(p => p.ProjectDesignId == designId).Count() > 0;
        }

        /// <summary>
        /// 附加子加载基本属性
        /// </summary>
        /// <param name="proDesignId">需求设计Id</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<DesignBasicProperty> GetDesignBasicProperties(double proDesignId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<DesignBasicProperty>().Where(p => p.ProjectDesignId == proDesignId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 项目号需求设计-保存重复校验获取数据库数据
        /// </summary>
        /// <param name="editIds">编辑数据Ids</param>
        /// <param name="propertys">属性</param>
        /// <returns></returns>
        public virtual EntityList<DesignBasicProperty> GetDesignBasicProperties(IEnumerable<double> editIds, IEnumerable<string> propertys)
        {
            EntityList<DesignBasicProperty> designBasicProperties = new EntityList<DesignBasicProperty>();
            propertys.SplitDataExecute(tempIds2 =>
            {
                editIds.SplitDataExecute(tempIds1 =>
                {
                    var list = Query<DesignBasicProperty>().Where(p => !tempIds1.Contains(p.Id) && propertys.Contains(p.BasicProperty)).ToList();
                    designBasicProperties.AddRange(list);
                });
                        
            });
            return designBasicProperties;
        }

        /// <summary>
        /// 附加子加载工艺资料
        /// </summary>
        /// <param name="proDesignId">需求设计Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<DesignProductTree> GetDesignProductTrees(double proDesignId, PagingInfo pagingInfo)
        {
            return Query<DesignProductTree>().Where(p => p.ProjectDesignId == proDesignId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存校验获取层级产品判断是否同层级同产品
        /// </summary>
        /// <param name="projectDesignId">项目号需求设计</param>
        /// <param name="levels">层级</param>
        /// <param name="productIds">产品Ids</param>
        /// <param name="editIds">编辑ids</param>
        /// <returns></returns>
        public virtual EntityList<DesignProductTree> GetDesignProductTrees(double projectDesignId, IEnumerable<int> levels, IEnumerable<double> productIds, IEnumerable<double> editIds)
        {
            EntityList<DesignProductTree> designs = new EntityList<DesignProductTree>();
            productIds.SplitDataExecute(tempIds1 => 
            {
                editIds.SplitDataExecute(tempIds2 =>
                {
                    var list = Query<DesignProductTree>().Where(p => p.ProjectDesignId == projectDesignId && !tempIds2.Contains(p.Id) && tempIds1.Contains(p.ProductId) && levels.Contains(p.TreeLevel)).ToList();
                    designs.AddRange(list);
                });
            });
            return designs;
        }

        /// <summary>
        /// 获取需求设计产品id
        /// </summary>
        /// <param name="designId">需求设计Id</param>
        /// <returns></returns>
        public virtual double GetDesignProductId(double designId)
        {
            return Query<ProjectDesign>().Where(p => p.Id == designId).Select(p => new { p.ProductId }).ToList<double>().FirstOrDefault();
        }

        /// <summary>
        /// 判断数据库中工艺资料已存在层级1
        /// </summary>
        /// <param name="designId">需求设计Id</param>
        /// <param name="editId">编辑工艺资料Id</param>
        /// <returns></returns>
        public virtual bool HasLevelOne(double designId, double? editId)
        {
            return Query<DesignProductTree>().Where(p => p.ProjectDesignId == designId && p.TreeLevel == 1)
                .WhereIf(editId != null, p => p.Id != editId).Count() > 0;
        }

        /// <summary>
        /// 页签加载工艺资料产品Bom信息
        /// </summary>
        /// <param name="projectDesign">工艺资料Id</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeBom> GetDesignTreeBomDetails(double projectDesign, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<DesignTreeBom>().Where(p => p.ProjectDesignId == projectDesign).OrderBy(p => p.TreeLevel).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询工艺资料产品Bom信息
        /// </summary>
        /// <param name="selIds">选择产品bom行Ids</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeBom> GetDesignTreeBoms(IEnumerable<double> selIds)
        {
            return selIds.SplitContains(tempIds => { return Query<DesignTreeBom>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()); });
        }

        /// <summary>
        /// 导入产品Bom明细根据项目编码、项目产品编码、bom编码获取产品Bom key: 项目编码+项目产品编码+bom编码 value:bomId
        /// </summary>
        /// <param name="projectCodes">项目编码</param>
        /// <param name="projectProductCodes">项目产品编码</param>
        /// <param name="bomCodes">bom编码</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetDesignTreeBoms(IEnumerable<string> projectCodes, IEnumerable<string> projectProductCodes, IEnumerable<string> bomCodes)
        {
            List<TreeBomImpInfo> treeBomImpInfos = new List<TreeBomImpInfo>();
            projectCodes.SplitDataExecute(projects =>
            {
                projectProductCodes.SplitDataExecute(products =>
                {
                    bomCodes.SplitDataExecute(boms =>
                    {
                        var list = Query<DesignTreeBom>().Join<ProjectDesign>((dtb, pd) => dtb.ProjectDesignId == pd.Id)
                        .LeftJoin<ProjectDesign, ProjectMaintain>((pd, pm) => pd.ProjectMaintainId == pm.Id)
                        .Where<ProjectMaintain>((dtb, pm) => projects.Contains(pm.Code))
                        .LeftJoin<ProjectDesign, Item>((pd, i) => pd.ProductId == i.Id)
                        .Where<Item>((dtb, i) => products.Contains(i.Code))
                        .Where(dtb => boms.Contains(dtb.BomCode))
                        .Select<ProjectDesign, ProjectMaintain, Item>((dtb, pd, pm, i) => new
                        {
                            Id = dtb.Id,
                            ProjectMaintainCode = pm.Code,
                            ProjectProductCode = i.Code,
                            BomCode = dtb.BomCode,
                        }).ToList<TreeBomImpInfo>();
                        treeBomImpInfos.AddRange(list);
                    });
                });
            });

            return treeBomImpInfos.ToDictionary(p => p.ProjectMaintainCode + "@" + p.ProjectProductCode + "@" + p.BomCode, p => p.Id);
        }

        /// <summary>
        /// 页签加载工艺资料产品工艺路线信息
        /// </summary>
        /// <param name="productDesignId">工艺资料Id</param>
        /// <param name="orderInfos">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRouting> GetDesignTreeRoutings(double productDesignId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            return Query<DesignTreeRouting>().Where(p => p.ProjectDesignId == productDesignId).OrderBy(p => p.TreeLevel).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品工艺路线设置导入信息 key:项目号+项目产品编码+层级+层级产品编码 value:产品工艺路线设置Id
        /// </summary>
        /// <param name="projectCodes">项目号</param>
        /// <param name="projectProductCodes">项目产品编码</param>
        /// <param name="levels">层级</param>
        /// <param name="levelProductCodes">层级产品编码</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetDesignTreeRoutings(IEnumerable<string> projectCodes, IEnumerable<string> projectProductCodes, IEnumerable<int> levels, IEnumerable<string> levelProductCodes)
        {
            List<TreeRoutingImpInfo> treeRoutingImpInfos = new List<TreeRoutingImpInfo>();
            projectCodes.SplitDataExecute(projects =>
            {
                projectProductCodes.SplitDataExecute(products =>
                {
                    levelProductCodes.SplitDataExecute(lproducts =>
                    {
                        var list = Query<DesignTreeRouting>().Join<ProjectDesign>((dtr, pd) => dtr.ProjectDesignId == pd.Id)
                        .LeftJoin<ProjectDesign, ProjectMaintain>((pd, pm) => pd.ProjectMaintainId == pm.Id)
                        .Where<ProjectMaintain>((dtb, pm) => projects.Contains(pm.Code))
                        .LeftJoin<ProjectDesign, Item>("i", (pd, i) => pd.ProductId == i.Id)
                        .Where<Item>((dtb, i) => products.Contains(i.Code))
                        .LeftJoin<DesignTreeRouting, Item>("ii", (dtr, ii) => dtr.ProductId == ii.Id)
                        .Where<Item>((dtr, ii) => levels.Contains(dtr.TreeLevel) && lproducts.Contains(ii.Code))
                        .Select<ProjectDesign, ProjectMaintain, Item, Item>((dtr, pd, pm, i, ii) => new
                        {
                            Id = dtr.Id,
                            ProjectMaintainCode = pm.Code,
                            ProjectProductCode = i.Code,
                            Level = dtr.TreeLevel,
                            ProductCode = ii.Code,
                        }).ToList<TreeRoutingImpInfo>();
                        treeRoutingImpInfos.AddRange(list);
                    });
                });
            });
            return treeRoutingImpInfos.ToDictionary(p => p.ProjectMaintainCode + "@" + p.ProjectProductCode + "@" + p.Level + "@" + p.ProductCode, p => p.Id);
        }

        /// <summary>
        /// 根据Ids获取工艺资料产品工艺路线设置
        /// </summary>
        /// <param name="ids">工艺资料产品工艺路线设置Ids</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRouting> GetDesignTreeRoutings(IEnumerable<double> ids)
        {
            return ids.SplitContains(tempIds =>
            {
                return Query<DesignTreeRouting>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 页签加载文档信息
        /// </summary>
        /// <param name="productDesignId">项目号需求设计Id</param>
        /// <param name="orderInfos">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeDocument> GetDesignTreeDocuments(double productDesignId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            return Query<DesignTreeDocument>().Where(p => p.ProjectDesignId == productDesignId).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 文档信息数量
        /// </summary>
        /// <param name="productDesignId">项目号需求设计Id</param>
        /// <returns></returns>
        public virtual int GetDesignTreeDocuments(double productDesignId)
        {
            return Query<DesignTreeDocument>().Where(p => p.ProjectDesignId == productDesignId).Count();
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public virtual List<string> GetBomVersions(int count)
        {
            var versions = new List<string>();
            var config = ConfigService.GetConfig(new ProductBomVersionConfig(), typeof(ProductBom));
            if (config.Version != null)
            {
                var list = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.VersionId.Value, count);
                versions.AddRange(list);
            }
            return versions;
        }

        /// <summary>
        /// 获取工艺资料产品Bom
        /// </summary>
        /// <param name="codes">编码</param>
        /// <param name="names">名称</param>
        /// <param name="editIds">编辑数据</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeBom> GetDesignTreeBomByCodes(IEnumerable<string> codes, IEnumerable<string> names, IEnumerable<double> editIds)
        {
            return Query<DesignTreeBom>().Where(p => (codes.Contains(p.BomCode) || names.Contains(p.BomName)) && !editIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取产品Bom
        /// </summary>
        /// <param name="bomId">产品bomId</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeBom> GetDesignTreeBomById(double bomId)
        {
            return Query<DesignTreeBom>().Where(p => p.Id == bomId).ToList();
        }

        /// <summary>
        /// 获取产品bom明细
        /// </summary>
        /// <param name="treeBomIds">产品bomIds</param>
        /// <param name="editIds">编辑明细</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeBomDetail> GetDesignTreeBomDetails(IEnumerable<double> treeBomIds, IEnumerable<double> editIds)
        {
            return Query<DesignTreeBomDetail>().Where(p => treeBomIds.Contains(p.DesignTreeBomId) && !editIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取产品bom明细
        /// </summary>
        /// <param name="treeBomIds">产品bomIds</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeBomDetail> GetDesignTreeBomDetails(IEnumerable<double> treeBomIds)
        {
            return treeBomIds.SplitContains(tempIds => { return Query<DesignTreeBomDetail>().Where(p => treeBomIds.Contains(p.DesignTreeBomId)).ToList(); });
        }

        /// <summary>
        /// 获取项目号需求设计Id
        /// </summary>
        /// <param name="treeId">工艺资料Id</param>
        /// <returns></returns>
        public virtual double GetDesignDetailIdByTreeId(double treeId)
        {
            return Query<DesignProductTree>().Where(p => p.Id == treeId).Select(p => new { p.ProjectDesignId }).ToList<double>().FirstOrDefault();
        }

        /// <summary>
        /// 判断产品工艺路线设置类型时间范围是否有重叠
        /// </summary>
        /// <param name="projectDesignId">项目号需求设计Id</param>
        /// <param name="editIds">工艺资料产品工艺路线编辑数据</param>
        /// <param name="types">类型</param>
        /// <param name="productIds">产品Ids</param>
        /// <param name="routingIds">工艺路线ids</param>
        /// <param name="typeTimeRangeDic">类型字典(key:类型,value:最小时间和最大时间)</param>
        /// <returns></returns>
        public virtual bool CountTypeRangeIsCross(double projectDesignId, IEnumerable<double> editIds, IEnumerable<WorkOrderType?> types, IEnumerable<double> productIds, IEnumerable<double?> routingIds, Dictionary<string, TreeRoutingInfo> typeTimeRangeDic)
        {
            bool isCross = false;
            // 查询分组，对每个类型进行groupby查询出对应的min(start)和max(end)
            List<TreeRoutingInfo> queryList = new List<TreeRoutingInfo>();

            productIds.SplitDataExecute(tempIds1 =>
            {
                routingIds.SplitDataExecute(tempIds2 =>
                {
                    editIds.SplitDataExecute(tempIds3 =>
                    {
                        var q = Query<DesignTreeRouting>()
                        .Where(p => !tempIds3.Contains(p.Id) && p.ProjectDesignId == projectDesignId && p.OrderType != null && types.Contains(p.OrderType) && tempIds1.Contains(p.ProductId) && p.RoutingId != null && tempIds2.Contains(p.RoutingId))
                        .GroupBy(p => new { p.OrderType, p.ProductId, p.RoutingId })
                        .Select(p => new { 
                            Type = p.OrderType,
                            ProductId = p.ProductId,
                            RoutingId = p.RoutingId,
                            StartDate = p.StartDate.MIN(),
                            EndDate = p.EndDate.MAX() 
                        })
                        .ToList<TreeRoutingInfo>();
                    });
                });
            });
            var queryDic = queryList.ToDictionary(p => "@" + p.Type + p.ProductId + p.RoutingId, p => new TreeRoutingInfo { StartDate = p.StartDate, EndDate = p.EndDate });
            foreach (var key in typeTimeRangeDic.Keys)
            {
                if (queryDic.TryGetValue(key, out var treeRoutingInfo))
                {
                    var typeTime = typeTimeRangeDic[key];
                    if (treeRoutingInfo.EndDate >= typeTime.StartDate && treeRoutingInfo.StartDate <= typeTime.EndDate)
                    {
                        isCross = true;
                        break;
                    }
                }
            }
            return isCross;
        }

        /// <summary>
        /// 获取当前版本的工序清单
        /// </summary>
        /// <param name="designTreeRoutingId">产品工艺路线设置Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRoutingDetail> GetDesignTreeRoutingDetails(double designTreeRoutingId, PagingInfo pagingInfo, string keyword)
        {
            List<ProcessType?> processTypes = new List<ProcessType?> { ProcessType.Assembly, ProcessType.Packing, ProcessType.BatchAssembly, ProcessType.BatchPacking };
            var list = Query<DesignTreeRoutingDetail>()
                .Where(dr => dr.DesignTreeRoutingId == designTreeRoutingId && processTypes.Contains(dr.Process.Type)).WhereIf(keyword.IsNotEmpty(), dr => dr.Process.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取项目号产品工艺路线设置明细
        /// </summary>
        /// <param name="editIds">产品工艺路线设置明细编辑数据</param>
        /// <param name="designRoutingIds">产品工艺路线设置Ids</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRoutingDetail> GetDesignTreeRoutingDetails(IEnumerable<double> editIds, IEnumerable<double> designRoutingIds)
        {
            EntityList<DesignTreeRoutingDetail> designTreeRoutingDetails = new EntityList<DesignTreeRoutingDetail>();
            if (editIds.Any())
            {
                editIds.SplitDataExecute(tempIds1 =>
                {
                    designRoutingIds.SplitDataExecute(tempIds2 =>
                    {
                        var q = Query<DesignTreeRoutingDetail>().Where(p => tempIds2.Contains(p.DesignTreeRoutingId) && !tempIds1.Contains(p.Id)).ToList();
                        designTreeRoutingDetails.AddRange(q);
                    });
                });
            }
            else
            {
                designRoutingIds.SplitDataExecute(tempIds2 =>
                {
                    var q = Query<DesignTreeRoutingDetail>().Where(p => tempIds2.Contains(p.DesignTreeRoutingId)).ToList();
                    designTreeRoutingDetails.AddRange(q);
                });
            }
            return designTreeRoutingDetails;
        }

        /// <summary>
        /// 获取当前版本的工序清单
        /// </summary>
        /// <param name="designTreeRoutingId">产品工艺路线设置Id</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRoutingDetail> GetSortDesignTreeRoutingDetails(double designTreeRoutingId)
        {
            var list = Query<DesignTreeRoutingDetail>().Where(p => p.DesignTreeRoutingId == designTreeRoutingId).OrderBy(p => p.Index).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取版本对应工序清单的工序Ids
        /// </summary>
        /// <param name="designTreeRoutingIds">产品工艺路线设置Ids</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRoutingDetail> GetDesignTreeRoutingDetailProcess(IEnumerable<double> designTreeRoutingIds)
        {
            return designTreeRoutingIds.SplitContains(tempIds => {
                return Query<DesignTreeRoutingDetail>()
                .Where(p => tempIds.Contains(p.DesignTreeRoutingId))
                .OrderBy(p => p.Index)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取产品工艺路线设置工序清单导入信息 key:工序名称+顺序 value:id
        /// </summary>
        /// <param name="designTreeRoutingIds">产品工艺路线设置Ids</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetDesignTreeRoutingDetailProcessDic(IEnumerable<double> designTreeRoutingIds)
        {
            List<TreeRoutingDtlImpInfo> dtlImpInfos = new List<TreeRoutingDtlImpInfo>();

            designTreeRoutingIds.SplitDataExecute(tempIds => {
                var list = Query<DesignTreeRoutingDetail>()
                .Join<DesignTreeRouting>((dtrd, dtr) => dtrd.DesignTreeRoutingId == dtr.Id && tempIds.Contains(dtr.Id))
                .LeftJoin<Process>((dtrd, p) => dtrd.ProcessId == p.Id)
                .Select<Process>((dtrd, p) => new
                {
                    Id = dtrd.Id,
                    Name = p.Name,
                    Index = dtrd.Index,
                })
                .ToList<TreeRoutingDtlImpInfo>();
                dtlImpInfos.AddRange(list);
            });
            return dtlImpInfos.ToDictionary(p => p.Name + "@" + p.Index, p => p.Id);
        }

        /// <summary>
        /// 获取拥有工序清单明细的Id
        /// </summary>
        /// <param name="designTreeRoutingIds">产品工艺路线设置Ids</param>
        /// <returns></returns>
        public virtual List<double> GetHasRoutingDetailIds(IEnumerable<double> designTreeRoutingIds)
        {
            List<double> ids = new List<double>();
            designTreeRoutingIds.SplitDataExecute(tempIds =>
            {
                var list = Query<DesignTreeRoutingDetail>().Where(p => tempIds.Contains(p.DesignTreeRoutingId)).Select(p => new { p.DesignTreeRoutingId }).ToList<double>();
                ids.AddRange(list);
            });
            return ids;
        }

        /// <summary>
        /// 页签加载工序清单明细
        /// </summary>
        /// <param name="designTreeRoutingId">产品工艺路线设置Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRoutingDetail> GetDesignTreeRoutingDetails(double designTreeRoutingId, PagingInfo pagingInfo)
        {
            return Query<DesignTreeRoutingDetail>().Where(p => p.DesignTreeRoutingId == designTreeRoutingId).OrderBy(p => p.Index).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品工艺路线设置获取工序BOM清单
        /// </summary>
        /// <param name="designTreeRoutingId">产品工艺路线设置Id</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRoutingProBom> GetDesignTreeRoutingProBoms(double designTreeRoutingId)
        {
            return Query<DesignTreeRoutingProBom>().Where(p => p.DesignTreeRoutingId == designTreeRoutingId).ToList();
        }

        /// <summary>
        /// 页签加载工序参数
        /// </summary>
        /// <param name="designTreeRoutingId">产品工艺路线设置Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeRoutingParamer> GetDesignTreeRoutingParamers(double designTreeRoutingId, PagingInfo pagingInfo)
        {
            return Query<DesignTreeRoutingParamer>().Where(p => p.DesignTreeRoutingId == designTreeRoutingId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 项目号需求设计-保存重复校验获取数据库数据
        /// </summary>
        /// <param name="editIds">编辑数据</param>
        /// <param name="projectIds">项目Ids</param>
        /// <param name="productIds">产品Ids</param>
        /// <returns></returns>
        public virtual EntityList<ProjectDesign> GetProjectDesigns(IEnumerable<double> editIds, IEnumerable<double> projectIds, IEnumerable<double> productIds)
        {
            EntityList<ProjectDesign> projectDesigns = new EntityList<ProjectDesign>();
            productIds.SplitDataExecute(tempIds1 =>
            {
                projectIds.SplitDataExecute(tempIds2 =>
                {
                    var list = Query<ProjectDesign>()
                    .Where(p => !editIds.Contains(p.Id) && tempIds1.Contains(p.ProductId) && tempIds2.Contains(p.ProjectMaintainId))
                    .Select(p => new { Product_Id = p.ProductId, Project_Maintain_Id = p.ProjectMaintainId })
                    .ToList();
                    projectDesigns.AddRange(list);
                });
            });
            return projectDesigns;
        }

        /// <summary>
        /// 判断选中的项目号设计是否都已填充 工艺资料和产品Bom
        /// </summary>
        /// <param name="selIds">选中项目号需求Ids</param>
        /// <returns></returns>
        public virtual bool CheckDesignIsFillInForComplete(IEnumerable<double> selIds)
        {
            var count = 0;
            selIds.SplitDataExecute(tempIds =>
            {
                count += Query<ProjectDesign>().Where(p => tempIds.Contains(p.Id)).Where(p => p.RoutingInfo != ChildInfoStatus.HasFilled || p.BomInfo != ChildInfoStatus.HasFilled).Count();
            });
            return count <= 0;
        }

        /// <summary>
        /// 判断选中的项目号设计是否都已填充 工艺资料和产品BOM且审核状态为待审核
        /// </summary>
        /// <param name="selIds">选中项目号需求Ids</param>
        /// <returns></returns>
        public virtual bool CheckDesignIsFillInForExamine(IEnumerable<double> selIds)
        {
            var count = 0;
            selIds.SplitDataExecute(tempIds =>
            {
                count += Query<ProjectDesign>().Where(p => tempIds.Contains(p.Id)).Where(p => p.RoutingInfo != ChildInfoStatus.HasFilled || p.BomInfo != ChildInfoStatus.HasFilled || p.ExamineStatus != ExamineStatus.UnExamine).Count();
            });
            return count <= 0;
        }

        /// <summary>
        /// 判断选中的项目号设计是否为已审核
        /// </summary>
        /// <param name="selIds">选中项目号需求Ids</param>
        /// <returns></returns>
        public virtual bool CheckdesignIsExamine(IEnumerable<double> selIds)
        {
            var count = 0;
            selIds.SplitDataExecute(tempIds =>
            {
                count += Query<ProjectDesign>().Where(p => tempIds.Contains(p.Id)).Where(p => p.ExamineStatus != ExamineStatus.Examined).Count();
            });
            return count <= 0;
        }

        /// <summary>
        /// 判断选中的项目号设计是否为禁用且为已审核
        /// </summary>
        /// <param name="selIds">选中项目号需求Ids</param>
        /// <returns></returns>
        public virtual bool CheckdesignIsDisable(IEnumerable<double> selIds)
        {
            var count = 0;
            selIds.SplitDataExecute(tempIds =>
            {
                count += Query<ProjectDesign>().Where(p => tempIds.Contains(p.Id)).Where(p => p.State != State.Disable || p.ExamineStatus != ExamineStatus.Examined).Count();
            });
            return count <= 0;
        }

        /// <summary>
        /// 判断选中的项目号设计是否为启用
        /// </summary>
        /// <param name="selIds">选中项目号需求Ids</param>
        /// <returns></returns>
        public virtual bool CheckdesignIsEnable(IEnumerable<double> selIds)
        {
            var count = 0;
            selIds.SplitDataExecute(tempIds =>
            {
                count += Query<ProjectDesign>().Where(p => tempIds.Contains(p.Id)).Where(p => p.State != State.Enable).Count();
            });
            return count <= 0;
        }
        #endregion

        #region 业务逻辑

        /// <summary>
        /// 创建操作日志
        /// </summary>
        /// <param name="proDesignId">项目号需求设计Id</param>
        /// <param name="operatePoint">操作节点</param>
        /// <param name="operateTime">操作时间</param>
        /// <param name="otherRemark">其他说明</param>
        /// <returns></returns>
        private ProjectDesignLog CreateProjectDesignLog(double proDesignId, OperatePoint operatePoint, DateTime operateTime, string otherRemark)
        {
            return new ProjectDesignLog
            {
                ProjectDesignId = proDesignId,
                OperatePoint = operatePoint,
                OperateTime = operateTime,
                OtherRemark = otherRemark,
                OperaterId = RT.IdentityId,
            };
        }

        /// <summary>
        /// 项目号需求设计重复校验
        /// </summary>
        /// <param name="projectDesigns"></param>
        public virtual void ValidateUnique(EntityList<ProjectDesign> projectDesigns)
        {
            string repeatStr = "项目号+产品编码唯一".L10N();
            var repeatHash = new HashSet<string>();
            if (projectDesigns.Any(p => !repeatHash.Add(p.ProjectMaintainId + "@" + p.ProductId)))
            {
                throw new ValidationException(repeatStr);
            }
            var editIds = projectDesigns.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id);
            var dbList = GetProjectDesigns(editIds, projectDesigns.Select(p => p.ProjectMaintainId), projectDesigns.Select(p => p.ProductId));
            if (dbList.Any(p => !repeatHash.Add(p.ProjectMaintainId + "@" + p.ProductId)))
            {
                throw new ValidationException(repeatStr);
            }
        }

        /// <summary>
        /// 项目号需求设计
        /// </summary>
        /// <param name="projectDesigns"></param>
        public virtual void ValidateBeforeSaving(EntityList<ProjectDesign> projectDesigns)
        {
            if (!projectDesigns.Any()) { return; }

            // 必填校验

            // 重复校验
            ValidateUnique(projectDesigns);
        }

        /// <summary>
        /// 项目需求设计保存
        /// </summary>
        /// <param name="projectDesigns"></param>
        public virtual void DesignSave(EntityList<ProjectDesign> projectDesigns)
        {
            // 操作日志
            EntityList<ProjectDesignLog> logs = new EntityList<ProjectDesignLog>();
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            foreach (var projectDesign in projectDesigns)
            {
                var point = projectDesign.PersistenceStatus == PersistenceStatus.New ? OperatePoint.Create : OperatePoint.Edit;
                logs.Add(new ProjectDesignLog
                {
                    ProjectDesign = projectDesign,
                    OperatePoint = point,
                    OperaterId = RT.IdentityId,
                    OperateTime = dbDate,
                    OtherRemark = point == OperatePoint.Edit ? "修改内容".L10N() : string.Empty,
                });
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(projectDesigns);
                foreach (var log in logs)
                {
                    log.ProjectDesignId = log.ProjectDesign.Id;
                }
                RT.Service.Resolve<CommonController>().BatchInsertSave(logs);
                tran.Complete();
            }
        }

        /// <summary>
        /// 项目号需求设计-基础属性重复校验
        /// </summary>
        /// <param name="designBasicProperties">项目号需求设计保存命令</param>
        protected virtual void ValidateUnique(EntityList<DesignBasicProperty> designBasicProperties)
        {
            var proHashSet = new HashSet<string>();
            if (designBasicProperties.Any(p => !proHashSet.Add(p.ProjectDesignId + "@" + p.BasicProperty)))
            {
                throw new ValidationException("属性不能重复".L10N());
            }
            var editIds = designBasicProperties.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id);
            var dbList = GetDesignBasicProperties(editIds, designBasicProperties.Select(p => p.BasicProperty));
            if (dbList.Any(p => !proHashSet.Add(p.ProjectDesignId + "@" + p.BasicProperty)))
            {
                throw new ValidationException("属性不能重复".L10N());
            }
        }

        /// <summary>
        /// 项目号需求设计-基础属性必填校验
        /// </summary>
        /// <param name="designBasicProperties"></param>
        protected virtual void ValidateRequire(EntityList<DesignBasicProperty> designBasicProperties)
        {
            if (designBasicProperties.Any(p => p.BasicProperty.IsNullOrEmpty()))
            {
                throw new ValidationException("基础属性必填".L10N());
            }
            if (designBasicProperties.Any(p => p.BasicProValue.IsNullOrEmpty()))
            {
                throw new ValidationException("基础属性值必填".L10N());
            }
        }

        /// <summary>
        /// 项目号需求设计-基础属性保存前校验
        /// </summary>
        /// <param name="designBasicProperties"></param>
        public virtual void ValidateBeforeSaving(EntityList<DesignBasicProperty> designBasicProperties)
        {
            if (!designBasicProperties.Any())
            {
                return;
            }
            // 必填校验
            ValidateRequire(designBasicProperties);
            // 重复校验
            ValidateUnique(designBasicProperties);
        }

        /// <summary>
        /// 项目号需求设计-基本属性保存
        /// </summary>
        /// <param name="designBasicProperties">保存数据</param>
        public virtual void DesignBasicPropertySave(EntityList<DesignBasicProperty> designBasicProperties)
        {
            // 主表
            double? designId = designBasicProperties.FirstOrDefault()?.ProjectDesignId;
            // 操作日志
            EntityList<ProjectDesignLog> logs = new EntityList<ProjectDesignLog>();
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();

            // 删除节点
            foreach (var d in designBasicProperties.DeletedList)
            {
                var pro = d as DesignBasicProperty;
                designId = pro.ProjectDesignId;
                logs.Add(CreateProjectDesignLog(pro.ProjectDesignId, OperatePoint.BasicDelete, dbDate, "删除属性[{0}]".L10nFormat(pro.BasicProperty)));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(designBasicProperties);
                RT.Service.Resolve<CommonController>().BatchInsertSave(logs);
                if (designId != null)
                {
                    var proInfoStatus = HasBasicProperty(designId.Value) ? ChildInfoStatus.HasFilled : ChildInfoStatus.UnFill;
                    DB.Update<ProjectDesign>().Set(p => p.BaseInfo, proInfoStatus).Where(p => p.Id == designId.Value).Execute();
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="baseData">树首层</param>
        /// <returns></returns>
        public virtual void InitProductTrees(ProTreeInfo baseData)
        {
            EntityList<DesignProductTree> trees = new EntityList<DesignProductTree>();
            int level = 1;
            // 首层为当前产品
            var root = new DesignProductTree
            {
                ProjectDesignId = baseData.ProjectDesignDetailId,
                ProductId = baseData.ProductId,
                TreeLevel = level++,
                TreePId = null,
            };
            root.GenerateId();
            trees.Add(root);
            // 标准Bom
            var bomList = RT.Service.Resolve<ProductBomController>().GetProductBomDetailsByProductId(baseData.ProductId);
            if (bomList.Any())
            {
                foreach (var bom in bomList)
                {
                    trees.Add(new DesignProductTree
                    {
                        ProjectDesignId = baseData.ProjectDesignDetailId,
                        ProductId = bom.Id,
                        TreeLevel = level,
                        TreePId = root.Id,
                    });
                }
            }
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 新增一笔操作日志
            var log = CreateProjectDesignLog(baseData.ProjectDesignDetailId, OperatePoint.PTreeInit, dbDate, string.Empty);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 删除原有
                DB.Delete<DesignProductTree>().Where(p => p.ProjectDesignId == baseData.ProjectDesignDetailId).Execute();
                // 覆盖新数据
                RT.Service.Resolve<CommonController>().BatchInsertSave(trees);
                // 保存日志
                RT.Service.Resolve<CommonController>().BatchInsertSave(new EntityList<ProjectDesignLog> { log });
                tran.Complete();
            }
        }

        /// <summary>
        /// 工艺资料保存前校验
        /// </summary>
        /// <param name="treeList">保存数据</param>
        public virtual void ValidateBeforeSaving(EntityList<DesignProductTree> treeList)
        {
            if (!treeList.Any()) return;
            // 需求设计Id
            var designDetailId = treeList.First().ProjectDesignId;
            var editOne = treeList.Where(p => p.PersistenceStatus == PersistenceStatus.Modified && p.TreeLevel == 1).FirstOrDefault();
            var designProductId = GetDesignProductId(designDetailId);
            var hasOne = HasLevelOne(designDetailId, editOne?.Id);
            
            var count = 0;
            foreach (var tree in treeList)
            {
                if (tree.TreeLevel != 1)
                {
                    continue;
                }
                else
                {
                    count++;
                    if (tree.ProductId != designProductId)
                    {
                        throw new ValidationException("层级1的产品必须为当前项目号需求设计的产品".L10N());
                    }
                    if (hasOne)
                    {
                        throw new ValidationException("项目号需求设计最多存在1个层级1".L10N());
                    }
                }
                if (count > 1)
                {
                    throw new ValidationException("项目号需求设计最多存在1个层级1".L10N());
                }
            }

            HashSet<string> levelProductId = new HashSet<string>();
            if (treeList.Any(p => !levelProductId.Add(p.TreeLevel + "@" + p.ProductId)))
            {
                throw new ValidationException("同一层级下的产品不能重复".L10N());
            }
            var dbList = GetDesignProductTrees(treeList.First().ProjectDesignId, treeList.Select(p => p.TreeLevel), treeList.Select(p => p.ProductId), treeList.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id));
            if (dbList.Any(p => !levelProductId.Add(p.TreeLevel + "@" + p.ProductId)))
            {
                throw new ValidationException("同一层级下的产品不能重复".L10N());
            }
        }

        /// <summary>
        /// 工艺资料-产品Bom必填校验
        /// </summary>
        /// <param name="treeBoms">保存数据</param>
        public virtual void ValidateRequire(EntityList<DesignTreeBom> treeBoms)
        {
            if (treeBoms.Any(p => p.BomCode.IsNullOrEmpty()))
            {
                throw new ValidationException("Bom编码必填".L10N());
            }
            if (treeBoms.Any(p => p.BomName.IsNullOrEmpty()))
            {
                throw new ValidationException("Bom名称必填".L10N());
            }
        }

        /// <summary>
        /// 工艺资料-产品Bom非重复校验
        /// </summary>
        /// <param name="treeBoms">保存数据</param>
        public virtual void ValidateUnique(EntityList<DesignTreeBom> treeBoms)
        {
            var codeRepeat = "Bom编码唯一".L10N();
            var nameRepeat = "Bom名称唯一".L10N();
            var codeHash = new HashSet<string>();
            if (treeBoms.Any(p => !codeHash.Add(p.ProjectDesignId + "@" + p.BomCode)))
            {
                throw new ValidationException(codeRepeat);
            }
            var nameHash = new HashSet<string>();
            if (treeBoms.Any(p => !nameHash.Add(p.ProjectDesignId + "@" + p.BomName)))
            {
                throw new ValidationException(nameRepeat);
            }
            var dbList = GetDesignTreeBomByCodes(treeBoms.Select(p => p.BomCode), treeBoms.Select(p => p.BomName), treeBoms.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id));
            if (dbList.Any(p => !codeHash.Add(p.ProjectDesignId + "@" + p.BomCode)))
            {
                throw new ValidationException(codeRepeat);
            }
            if (dbList.Any(p => !nameHash.Add(p.ProjectDesignId + "@" + p.BomName)))
            {
                throw new ValidationException(nameRepeat);
            }
        }

        /// <summary>
        /// 验证明细
        /// </summary>
        /// <param name="treeBoms">保存数据</param>
        public virtual void ValidateDetail(EntityList<DesignTreeBom> treeBoms)
        {
            var details = treeBoms.SelectMany(p => p.DetailList).ToList();
            var dbDetailList = GetDesignTreeBomDetails(treeBoms.Select(p => p.Id).Distinct(), details.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id));
            if (details.Any(p => p.UnitQty <= 0))
            {
                throw new ValidationException("单位耗用量必须大于0".L10N());
            }
            var idHash = new HashSet<string>();
            var treeBomsDic = treeBoms.ToDictionary(p => p.Id, p => p.ProductId);
            foreach (var detail in details)
            {
                if (!idHash.Add(detail.DesignTreeBomId + "@" + detail.ItemId))
                {
                    throw new ValidationException("产品Bom明细物料不能重复".L10N());
                }
                if (treeBomsDic.TryGetValue(detail.DesignTreeBomId, out var productId))
                {
                    if (productId == detail.ItemId)
                    {
                        throw new ValidationException("产品Bom明细物料不能与主表产品相同".L10N());
                    }
                }
            }
        }

        /// <summary>
        /// 工艺资料-产品Bom保存前校验
        /// </summary>
        /// <param name="treeBoms">保存数据</param>
        public virtual void ValidateBeforeSaving(EntityList<DesignTreeBom> treeBoms)
        {
            if (!treeBoms.Any()) return;

            // 必填校验
            ValidateRequire(treeBoms);
            // 重复校验
            ValidateUnique(treeBoms);
            // 子表校验
            ValidateDetail(treeBoms);
        }

        /// <summary>
        /// 工艺资料产品bom引用标准Bom
        /// </summary>
        /// <param name="selIds">项目号需求设计产品bom选择行的Ids</param>
        public virtual void InitDesignTreeBoms(IEnumerable<double> selIds)
        {
            // 查询产品bom表
            var treeBomList = GetDesignTreeBoms(selIds);
            if (!treeBomList.Any())
            {
                throw new ValidationException("当前选择的数据不存在，请刷新".L10N());
            }

            // 所有产品Ids
            var productIds = treeBomList.Select(p => p.ProductId).Distinct();

            // 查询产品bom明细信息
            var bomDtlList = RT.Service.Resolve<ProductBomController>().GetDefaultProductBomDtls(productIds);
            // 分组产品信息（一个产品只有一个默认版本）
            var bomDtlLookUp = bomDtlList.ToLookup(p => p.ProductId);

            Dictionary<double, ProBomDtlInfo> treeBomUpdateDic = new Dictionary<double, ProBomDtlInfo>();
            EntityList<DesignTreeBomDetail> designTreeBomDetails = new EntityList<DesignTreeBomDetail>();
            foreach (var treeBom in treeBomList)
            {
                if (!bomDtlLookUp[treeBom.ProductId].Any())
                {
                    throw new ValidationException("当前产品[{0}]未维护产品Bom或明细".L10nFormat(treeBom.ProductCode));
                }
                else
                {
                    var detailList = bomDtlLookUp[treeBom.ProductId];
                    // 加入一条数据更新产品bom表头编码、名称、版本
                    var first = detailList.First();
                    treeBomUpdateDic.Add(treeBom.Id, new ProBomDtlInfo { BomCode = first.BomCode, BomName = first.BomName, Version = first.Version });
                    foreach (var dtl in detailList)
                    {
                        DesignTreeBomDetail designTreeBomDetail = new DesignTreeBomDetail
                        {
                            DesignTreeBomId = treeBom.Id,
                            ItemId = dtl.ItemId,
                            UnitQty = dtl.UnitQty,
                            LossRate = dtl.LossRate,
                            IsRecoilItem = dtl.IsRecoilItem ?? false,
                        };
                        designTreeBomDetails.Add(designTreeBomDetail);
                    }
                }
            }

            var dbTime = RF.Find<ProjectDesign>().GetDbTime();
            // 创建日志
            var log = CreateProjectDesignLog(treeBomList.First().ProjectDesignId, OperatePoint.PTreeBomInit, dbTime, "引用标准BOM资料".L10N());

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var ctl = RT.Service.Resolve<CommonController>();
                // 更新产品Bom表头的信息
                foreach (var key in treeBomUpdateDic.Keys)
                {
                    var value = treeBomUpdateDic[key];
                    DB.Update<DesignTreeBom>().Set(p => p.BomCode, value.BomCode).Set(p => p.BomName, value.BomName).Set(p => p.Version, value.Version).Where(p => p.Id == key).Execute();
                    // 清空明细
                    DB.Delete<DesignTreeBomDetail>().Where(p => p.DesignTreeBomId == key).Execute();
                }
                // 插入新明细数据
                ctl.BatchInsertSave(designTreeBomDetails);
                ctl.BatchInsertSave(new EntityList<ProjectDesignLog> { log });
                tran.Complete();
            }
        }

        /// <summary>
        /// 产品Bom版本更新数据处理
        /// </summary>
        /// <param name="treeBomList">选择的产品Bom</param>
        /// <param name="projectMaintainId">项目id</param>
        /// <param name="bomIdDic">产品Bom字典(产品Id-BomId)</param>
        /// <param name="treeBomDtlLookUp">产品Bom明细分组</param>
        /// <param name="newVersions">版本号列表</param>
        /// <param name="insertSaveProBoms">新增产品Bom保存列表</param>
        /// <param name="insertSaveProBomDtls">新增产品Bom明细保存列表</param>
        /// <param name="updateProBomDic">更新产品Bom字典(产品BomId-版本)</param>
        /// <param name="updateProjectProBomDic">更新项目号需求设计产品Bom字典(产品BomId-项目号产品BomId)</param>
        /// <param name="updateSaveProBomDtls">更新产品Bom明细</param>
        /// <exception cref="ValidationException"></exception>
        private void CreateUpdateDesignTreeBomData(EntityList<DesignTreeBom> treeBomList, double projectMaintainId, Dictionary<double, double> bomIdDic, ILookup<double, DesignTreeBomDetail> treeBomDtlLookUp, List<string> newVersions,
            EntityList<ProductBom> insertSaveProBoms, EntityList<ProductBomDetail> insertSaveProBomDtls, Dictionary<double, string> updateProBomDic, Dictionary<double, string> updateProjectProBomDic, EntityList<ProductBomDetail> updateSaveProBomDtls)
        {
            int versionIndex = 0;
            foreach (var treebom in treeBomList)
            {
                // 判断是否已存在产品+项目产品Bom
                if (bomIdDic.TryGetValue(treebom.ProductId, out var bomId))
                {
                    var version = newVersions[versionIndex++];
                    // 更新产品bom版本
                    updateProBomDic.Add(bomId, version);
                    // 更新项目号需求设计产品bom版本
                    updateProjectProBomDic.Add(treebom.Id, version);

                    // 生成产品bom明细
                    var bomDtls = treeBomDtlLookUp[treebom.Id];
                    if (!bomDtls.Any())
                    {
                        throw new ValidationException("当前产品[{0}]未维护产品bom明细".L10nFormat(treebom.ProductCode));
                    }
                    foreach (var bomDtl in bomDtls)
                    {
                        ProductBomDetail productBomDetail = new ProductBomDetail
                        {
                            ProductBomId = bomId,
                            ItemId = bomDtl.ItemId,
                            UnitQty = bomDtl.UnitQty,
                            LossRate = bomDtl.LossRate,
                            IsRecoilItem = bomDtl.IsRecoilItem,
                        };
                        updateSaveProBomDtls.Add(productBomDetail);
                    }
                }
                // 否则创建一条新的
                else
                {
                    var version = newVersions[versionIndex++];
                    ProductBom productBom = new ProductBom
                    {
                        Code = treebom.BomCode + version,
                        Name = treebom.BomName + version,
                        ProductId = treebom.ProductId,
                        ProjectMaintainId = projectMaintainId,
                        SourceType = SIE.Common.SourceType.Internal,
                        Version = version,
                    };
                    insertSaveProBoms.Add(productBom);
                    // 生成产品bom明细
                    var bomDtls = treeBomDtlLookUp[treebom.Id];
                    // 更新项目号需求设计产品bom版本
                    updateProjectProBomDic.Add(treebom.Id, version);
                    if (!bomDtls.Any())
                    {
                        throw new ValidationException("当前产品[{0}]未维护产品bom明细".L10nFormat(treebom.ProductCode));
                    }
                    foreach (var bomDtl in bomDtls)
                    {
                        ProductBomDetail productBomDetail = new ProductBomDetail
                        {
                            ProductBom = productBom,
                            ItemId = bomDtl.ItemId,
                            UnitQty = bomDtl.UnitQty,
                            LossRate = bomDtl.LossRate,
                            IsRecoilItem = bomDtl.IsRecoilItem,
                        };
                        insertSaveProBomDtls.Add(productBomDetail);
                    }

                }
            }
        }

        /// <summary>
        /// 工艺资料-产品BOM更新版本
        /// </summary>
        /// <param name="selIds">选择产品bom的Ids</param>
        /// <param name="projectMaintainId">项目Id</param>
        public virtual void UpdateDesignTreeBom(IEnumerable<double> selIds, double projectMaintainId)
        {
            // 查询产品bom表
            var treeBomList = GetDesignTreeBoms(selIds);
            if (!treeBomList.Any())
            {
                throw new ValidationException("当前选择的数据不存在，请刷新".L10N());
            }
            // 必填验证
            ValidateRequire(treeBomList);
            // 产品Ids
            var productIds = treeBomList.Select(p => p.ProductId).Distinct();
            // 明细
            var treeBomDtlList = GetDesignTreeBomDetails(selIds);
            // 分组
            var treeBomDtlLookUp = treeBomDtlList.ToLookup(p => p.DesignTreeBomId);

            // 查询是否存在产品+项目号的数据
            var bomIdDic = RT.Service.Resolve<ProductBomController>().GetProductBomDicByProjectMaintain(productIds, projectMaintainId);

            // 更新版本号
            var newVersions = RT.Service.Resolve<ProjectDesignController>().GetBomVersions(selIds.Count());

            // 插入保存产品bom
            EntityList<ProductBom> insertSaveProBoms = new EntityList<ProductBom>();
            // 插入保存产品bom明细
            EntityList<ProductBomDetail> insertSaveProBomDtls = new EntityList<ProductBomDetail>();
            // 更新保存产品Bom版本(key:产品bomId, value:版本)
            Dictionary<double, string> updateProBomDic = new Dictionary<double, string>();
            // 更新保存产品bom明细
            EntityList<ProductBomDetail> updateSaveProBomDtls = new EntityList<ProductBomDetail>();
            // 更新项目号需求设计产品Bom版本(key:项目号需求设计产品bomId, value:版本号)
            Dictionary<double, string> updateProjectProBomDic = new Dictionary<double, string>();
            // 数据处理
            CreateUpdateDesignTreeBomData(treeBomList, projectMaintainId, bomIdDic, treeBomDtlLookUp, newVersions, insertSaveProBoms, insertSaveProBomDtls, updateProBomDic, updateProjectProBomDic, updateSaveProBomDtls);

            // 生成一笔日志
            var dbTime = RF.Find<ProjectDesign>().GetDbTime();
            var log = CreateProjectDesignLog(treeBomList.First().ProjectDesignId, OperatePoint.PTreeBomUpdate, dbTime, "引用标准BOM资料".L10N());

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var ctl = RT.Service.Resolve<CommonController>();
                // 新增产品Bom、明细
                ctl.BatchInsertSave(insertSaveProBoms);
                insertSaveProBomDtls.ForEach(i => { i.ProductBomId = i.ProductBom.Id; });
                ctl.BatchInsertSave(insertSaveProBomDtls);
                // 更新产品Bom版本、新增明细
                foreach (var key in updateProBomDic.Keys)
                {
                    // 更新产品bom版本
                    DB.Update<ProductBom>().Set(p => p.Version, updateProBomDic[key]).Where(p => p.Id == key).Execute();
                    // 删除原明细
                    DB.Delete<ProductBomDetail>().Where(p => p.ProductBomId == key).Execute();
                }
                // 更新项目号需求设计产品bom版本
                foreach (var key in updateProjectProBomDic.Keys)
                {
                    DB.Update<DesignTreeBom>().Set(p => p.Version, updateProjectProBomDic[key]).Set(p => p.HasUp, true).Where(p => p.Id == key).Execute();
                }
                ctl.BatchInsertSave(updateSaveProBomDtls);

                // 更新日志
                ctl.BatchInsertSave(new EntityList<ProjectDesignLog> { log });
                // 更新项目号需求设计产品Bom
                DB.Update<ProjectDesign>().Set(p => p.BomInfo, ChildInfoStatus.HasFilled).Where(p => p.Id == treeBomList.First().ProjectDesignId).Execute();

                tran.Complete();
            }
        }

        /// <summary>
        /// 工艺资料-产品Bom保存前校验
        /// </summary>
        /// <param name="treeRoutings">保存数据</param>
        public virtual void ValidateBeforeSaving(EntityList<DesignTreeRouting> treeRoutings)
        {
            if (!treeRoutings.Any())
            {
                return;
            }

            // 必填校验
            ValidateRequire(treeRoutings);
            // 重复校验
            ValidateUnique(treeRoutings);

            // 校验明细
            var bomDetailList = treeRoutings.SelectMany(p => p.RoutingDetailList).AsEntityList();
            EntityList<DesignTreeRoutingDetail> delBomDetailList = new EntityList<DesignTreeRoutingDetail>();
            foreach (var tree in treeRoutings.SelectMany(p => p.RoutingDetailList.DeletedList))
            {
                var detail = tree as DesignTreeRoutingDetail;
                delBomDetailList.Add(detail);
            }
            ValidateBeforeSaving(bomDetailList, delBomDetailList);
        }

        /// <summary>
        /// 工艺资料-产品Bom保存
        /// </summary>
        /// <param name="treeRoutings">保存数据</param>
        public virtual void SaveRouting(EntityList<DesignTreeRouting> treeRoutings)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(treeRoutings);
                var saveIds = treeRoutings.Select(p => p.Id).ToList();
                // 获取拥有工序清单明细的Ids
                var hasDetailIds = GetHasRoutingDetailIds(saveIds);
                var notHasDetailIds = saveIds.Except(hasDetailIds).ToList();
                // 更新维护工序清单标识符
                hasDetailIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<DesignTreeRouting>().Set(p => p.HasRoutingDetail, true).Where(p => tempIds.Contains(p.Id)).Execute();
                });
                notHasDetailIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<DesignTreeRouting>().Set(p => p.HasRoutingDetail, false).Where(p => tempIds.Contains(p.Id)).Execute();
                });
                tran.Complete();
            }
        }

        /// <summary>
        /// 工艺资料-产品工艺路线必填校验
        /// </summary>
        /// <param name="treeRoutings">保存数据</param>
        public virtual void ValidateRequire(EntityList<DesignTreeRouting> treeRoutings)
        {
            if (treeRoutings.Any(p => p.OrderType == null))
            {
                throw new ValidationException("类型不能为空".L10N());
            }
            if (treeRoutings.Any(p => p.RoutingId == null))
            {
                throw new ValidationException("工艺路线不能为空".L10N());
            }
            if (treeRoutings.Any(p => p.StartDate == null))
            {
                throw new ValidationException("开始日期不能为空".L10N());
            }
            if (treeRoutings.Any(p => p.EndDate == null))
            {
                throw new ValidationException("结束日期不能为空".L10N());
            }
            if (treeRoutings.Any(p => p.EndDate != null && p.EndDate.Value < p.StartDate))
            {
                throw new ValidationException("结束日期不能早于开始日期".L10N());
            }
        }

        /// <summary>
        /// 工艺资料-产品工艺路线非重复校验
        /// </summary>
        /// <param name="treeRoutings">保存数据</param>
        public virtual void ValidateUnique(EntityList<DesignTreeRouting> treeRoutings)
        {
            // 项目号需求设计
            var parentId = treeRoutings.First().ProjectDesignId;
            // 先排序再判断时间交叉
            var sortlist = treeRoutings.OrderBy(p => p.StartDate).ToList();
            var editIds = sortlist.Where(p => p.PersistenceStatus == PersistenceStatus.Modified).Select(p => p.Id);

            // 同一类型时间段不能重复
            var repeatDic = new Dictionary<string, TreeRoutingInfo>();
            // 记录类型的最小时间和最大时间
            var typeTimeRangeDic = new Dictionary<string, TreeRoutingInfo>();
            foreach (var i in sortlist)
            {
                var key = "@" + i.OrderType + i.ProductId + i.RoutingId;
                if (!repeatDic.TryGetValue(key, out TreeRoutingInfo treeRoutingInfo))
                {
                    repeatDic.Add(key, new TreeRoutingInfo { StartDate = i.StartDate, EndDate = i.EndDate });
                    typeTimeRangeDic.Add(key, new TreeRoutingInfo { StartDate = i.StartDate, EndDate = i.EndDate });
                }
                else
                {
                    if (i.StartDate <= treeRoutingInfo.EndDate)
                    {
                        throw new ValidationException("同一类型、产品下的产品工艺路线设置时间范围不能重叠".L10N());
                    }
                    else
                    {
                        // 更新
                        repeatDic[key] = new TreeRoutingInfo { StartDate = i.StartDate, EndDate = i.EndDate };
                        typeTimeRangeDic[key].EndDate = i.EndDate;
                    }
                }
            }
            if (CountTypeRangeIsCross(parentId, editIds, treeRoutings.Select(p => p.OrderType), treeRoutings.Select(p => p.ProductId), treeRoutings.Select(p => p.RoutingId), typeTimeRangeDic))
            {
                throw new ValidationException("同一类型、产品下的产品工艺路线设置时间范围不能重叠".L10N());
            }
        }

        /// <summary>
        /// 工艺资料-文档上传保存前校验
        /// </summary>
        /// <param name="documents">保存数据</param>
        public virtual void ValidateRequire(EntityList<DesignTreeDocument> documents)
        {
            if (documents.Any(p => p.DocCode.IsNullOrEmpty()))
            {
                throw new ValidationException("文档编码不能为空".L10N());
            }
            if (documents.Any(p => p.DocName.IsNullOrEmpty()))
            {
                throw new ValidationException("文档名称不能为空".L10N());
            }
            if (documents.Any(p => p.DocVer.IsNullOrEmpty()))
            {
                throw new ValidationException("文档版本不能为空".L10N());
            }
            if (documents.Any(p => p.DocType.IsNullOrEmpty()))
            {
                throw new ValidationException("文档类型不能为空".L10N());
            }
        }

        /// <summary>
        /// 工艺资料-文档上传保存前校验
        /// </summary>
        /// <param name="documents">保存数据</param>
        public virtual void ValidateBeforeSaving(EntityList<DesignTreeDocument> documents)
        {
            if (!documents.Any()) return;

            // 必填校验
            ValidateRequire(documents);
        }

        /// <summary>
        /// 保存工艺资料文档上传
        /// </summary>
        /// <param name="documents">保存数据</param>
        public virtual void DesignTreeSave(EntityList<DesignTreeDocument> documents)
        {
            double? designDetailId = documents.FirstOrDefault()?.ProjectDesignId;
            if (designDetailId == null)
            {
                var del = documents.DeletedList.FirstOrDefault() as DesignTreeDocument;
                designDetailId = del?.ProjectDesignId;
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(documents);

                if (designDetailId != null)
                {
                    var status = GetDesignTreeDocuments(designDetailId.Value) > 0 ? ChildInfoStatus.HasFilled : ChildInfoStatus.UnFill;
                    // 更新项目号需求设计字段
                    DB.Update<ProjectDesignDetail>().Set(p => p.AttachInfo, status).Where(p => p.Id == designDetailId).Execute();
                }
                
                tran.Complete();
            }
        }


        /// <summary>
        /// 判断文件是否为图片或pdf
        /// </summary>
        /// <param name="fileExt">文件扩展名</param>
        public virtual void CheckDocType(string fileExt)
        {
            var extStrList = new List<string>() { ".png" , ".jpg", ".bmp", ".gif", ".webp"
                , ".avif" , ".apng" , ".jfif" , ".jpeg" , ".tif", ".pdf" };
            if (!extStrList.Contains(fileExt))
            {
                throw new ValidationException("只允许上传图片或PDF文件".L10N());
            }
        }

        /// <summary>
        /// 工艺资料-产品工艺路线明细保存前校验
        /// </summary>
        /// <param name="details">保存数据</param>
        /// <param name="delDetails">删除数据</param>
        public virtual void ValidateBeforeSaving(EntityList<DesignTreeRoutingDetail> details, EntityList<DesignTreeRoutingDetail> delDetails)
        {
            if (!details.Any() && !delDetails.Any())
            {
                return;
            }
            // 不允许出现同顺序同工序
            var indexProcessHash = new HashSet<string>();
            if (details.Any(p => !indexProcessHash.Add(p.DesignTreeRoutingId + "@" + p.Index + "@" + p.ProcessId)))
            {
                throw new ValidationException("产品工艺路线设置明细同一个工序顺序不能相同".L10N());
            }

            var dbList = GetDesignTreeRoutingDetails(details.Where(p => p.PersistenceStatus != PersistenceStatus.New).Select(p => p.Id), details.Select(p => p.DesignTreeRoutingId).Distinct());
            if (dbList.Any(p => !indexProcessHash.Add(p.DesignTreeRoutingId + "@" + p.Index + "@" + p.ProcessId)))
            {
                throw new ValidationException("产品工艺路线设置明细同一个工序顺序不能相同".L10N());
            }

            // 删除校验-工序清单引用
            if (delDetails.Any())
            {
                var delIds = delDetails.Select(p => p.Id);
                var count = Query<DesignTreeRoutingProBom>().Where(p => delIds.Contains(p.RoutingProcessId)).Count();
                if (count > 0)
                {
                    throw new ValidationException("存在工序被当前产品工序BOM清单中引用,无法删除".L10N());
                }
            }

        }

        /// <summary>
        /// 工艺资料产品bom引用标准Bom
        /// </summary>
        /// <param name="selIds">选择产品工艺路线数据Ids</param>
        public virtual void InitDesignTreeRouting(IEnumerable<double> selIds)
        {
            // 查询产品工艺路线设置数据
            var routingList = GetDesignTreeRoutings(selIds);
            if (!routingList.Any())
            {
                throw new ValidationException("当前选择的数据不存在，请刷新".L10N());
            }

            // 产品Ids
            var productIds = routingList.Select(p => p.ProductId);
            // 获取当前产品对应的产品工艺路线设置
            var productRoutingList = RT.Service.Resolve<ProductRoutingController>().GetNowProductRoutingsByProductIds(productIds);

            // 获取这些工艺路线版本的工序清单
            var versionIds = productRoutingList.Where(p => p.RoutingVersionId != null).Select(p => (double)p.RoutingVersionId).ToList();
            // 获取这些版本的工艺路线清单
            var versionProcessList = RT.Service.Resolve<ProductRoutingController>().GetRoutingProcessesByVersionIds(versionIds);

            // 更新产品工艺路线信息key数据id-value产品工艺路线设置信息
            Dictionary<double, TreeRoutingInfo> updateTRDic = new Dictionary<double, TreeRoutingInfo>();
            // 明细
            EntityList<DesignTreeRoutingDetail> designTreeRoutingDetails = new EntityList<DesignTreeRoutingDetail>();

            foreach (var routing in routingList)
            {
                // 获取最后一个版本为标准
                var productRouting = productRoutingList.FirstOrDefault(p => p.ProductId == routing.ProductId);
                if (productRouting == null)
                {
                    throw new ValidationException("当前产品[{0}]未维护当前时间的产品工艺路线设置".L10nFormat(routing.ProductCode));
                }
                // 更新信息
                updateTRDic.Add(routing.Id, new TreeRoutingInfo { Type = productRouting.OrderType, RoutingId = productRouting.RoutingId, RoutingVersionId = productRouting.RoutingVersionId, StartDate = productRouting.StartDate, EndDate = productRouting.EndDate });
                // 获取工艺路线工序清单明细
                var processList = versionProcessList.Where(p => p.VersionId == productRouting.RoutingVersionId).ToList();
                foreach (var process in processList)
                {
                    DesignTreeRoutingDetail designTreeRoutingDetail = new DesignTreeRoutingDetail
                    {
                        DesignTreeRoutingId = routing.Id,
                        Index = process.Index,
                        ProcessId = process.ProcessId.Value,
                        ProcessName = process.Name,
                        IsOptional = process.IsOptional,
                        Outsourcing = process.Outsourcing,
                        IsGenerateTask = process.IsGenerateTask,
                        IsRequirementTask= process.IsRequirementTask,
                    };
                    designTreeRoutingDetails.Add(designTreeRoutingDetail);
                }
            }

            // 生成一笔日志
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 新增一笔操作日志
            var log = CreateProjectDesignLog(routingList.First().ProjectDesignId, OperatePoint.PTreeRoutingInit, dbDate, "引用工艺路线".L10N());

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var ctl = RT.Service.Resolve<CommonController>();
                // 更新产品工艺路线设置信息
                foreach (var key in updateTRDic.Keys)
                {
                    var value = updateTRDic[key];
                    DB.Update<DesignTreeRouting>().Set(p => p.OrderType, value.Type).Set(p => p.RoutingId, value.RoutingId).Set(p => p.RoutingVersionId, value.RoutingVersionId).Set(p => p.StartDate, value.StartDate).Set(p => p.EndDate, value.EndDate).Where(p => p.Id == key).Execute();
                    // 删除原本工序清单明细
                    DB.Delete<DesignTreeRoutingDetail>().Where(p => p.DesignTreeRoutingId == key).Execute();
                    // 清空工序bom
                    DB.Delete<DesignTreeRoutingProBom>().Where(p => p.DesignTreeRoutingId == key).Execute();
                    // 清空参数明细
                    DB.Delete<DesignTreeRoutingParamer>().Where(p => p.DesignTreeRoutingId == key).Execute();
                }
                ctl.BatchInsertSave(designTreeRoutingDetails);
                ctl.BatchInsertSave(new EntityList<ProjectDesignLog> { log });
                tran.Complete();
            }
        }

        /// <summary>
        /// 工艺BOM生成
        /// </summary>
        /// <param name="treeRoutingIds">产品工艺路线设置Ids</param>
        public virtual void InitRoutingBomDetail(IEnumerable<double> treeRoutingIds)
        {
            // 查询产品工艺路线设置数据
            var routingList = GetDesignTreeRoutings(treeRoutingIds);
            if (!routingList.Any())
            {
                throw new ValidationException("当前选择的数据不存在，请刷新".L10N());
            }
            ValidateRequire(routingList);
            // 根据产品+工艺路线+版本获取标准工序Bom明细
            var routingBomDetailList = RT.Service.Resolve<RoutingBomController>().GetRoutingBomDetailsByVersionId(routingList.Select(p => p.ProductId).Distinct(), routingList.Select(p => p.RoutingId).Distinct(), routingList.Select(p => p.RoutingVersionId).Distinct());

            List<ProcessType?> processTypes = new List<ProcessType?> { ProcessType.Assembly, ProcessType.Packing, ProcessType.BatchAssembly, ProcessType.BatchPacking };

            // 获取其当前的项目号需求设计工序清单（工序+顺序）
            var routingDetailProcessses = GetDesignTreeRoutingDetailProcess(routingList.Select(p => p.Id));
            // 获取版本号获取工艺路线工序清单（装配、包装）
            var routingDetailIProcesses = routingDetailProcessses.Where(p => processTypes.Contains(p.ProcessType)).ToList();
            var routingProcessIds = routingDetailProcessses.Select(p => p.ProcessId).Distinct().ToList();
            // 获取当前版本的标准工序清单（装配、包装）
            var routingProcessList = RT.Service.Resolve<RoutingSettingController>().GetRoutingProcesses(routingList.Select(p => p.RoutingVersionId).Distinct(), processTypes).ToList();
            // 根据产品+工序查询标准工序参数
            var processParamList = RT.Service.Resolve<ProjectParamController>().GetProcessStandardParamDtlsByProcess(routingList.Select(p => p.ProductId).Distinct(), routingProcessIds);

            // 生成的工序BOM
            EntityList<DesignTreeRoutingProBom> designTreeBomDetails = new EntityList<DesignTreeRoutingProBom>();
            // 生成的工序参数
            EntityList<DesignTreeRoutingParamer> designTreeRoutingParamers = new EntityList<DesignTreeRoutingParamer>();

            foreach (var routing in routingList)
            {
                // 标准工序bom（装配、包装）
                var standardProcessList = routingProcessList.Where(p => p.VersionId == routing.RoutingVersionId).ToList();
                // 项目号工序清单
                var projectProcessList = routingDetailProcessses.Where(p => p.DesignTreeRoutingId == routing.Id).ToList();
                // 项目号装配包装工序
                var assAndPkgProcessList = projectProcessList.Where(p => processTypes.Contains(p.ProcessType)).ToList();
                
                // 项目号工序bom明细
                var projectBomDetailList = routingBomDetailList.Where(p => p.ProductId == routing.ProductId && p.RoutingId == routing.RoutingId && p.RoutingVersionId == routing.RoutingVersionId);
                foreach (var bomDetail in projectBomDetailList)
                {
                    // 按顺序匹配到工序清单
                    var i = standardProcessList.FindIndex(p => p.Id == bomDetail.RoutingProcessId);
                    DesignTreeRoutingProBom bom = new DesignTreeRoutingProBom
                    {
                        DesignTreeRoutingId = routing.Id,
                        RoutingProcessId = assAndPkgProcessList[i].Id,
                        MaterialId = bomDetail.MaterialId,
                        Amount = bomDetail.Amount,
                        Remark = bomDetail.Description,
                    };
                    designTreeBomDetails.Add(bom);
                }
                // 标准工序参数
                var standardParamList = processParamList.Where(p => p.ProductId == routing.ProductId && projectProcessList.Select(x => x.ProcessId).Contains(p.ProcessId));
                foreach (var processParam in standardParamList)
                {
                    DesignTreeRoutingParamer paramer = new DesignTreeRoutingParamer
                    {
                        DesignTreeRoutingId = routing.Id,
                        ProjectParamId = processParam.ProjectParamId,
                        ProcessStDtlValueType = processParam.ProcessStDtlValueType,
                        ProcessId = processParam.ProcessId,
                        Unit = processParam.Unit,
                        SingleValue = processParam.SingleValue,
                        RangeMaxValue = processParam.RangeMaxValue,
                        RangeMinValue = processParam.RangeMinValue,
                    };
                    designTreeRoutingParamers.Add(paramer);
                }
            }

            // 生成一笔日志
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 新增一笔操作日志
            var log = CreateProjectDesignLog(routingList.First().ProjectDesignId, OperatePoint.PTreeRoutingInit, dbDate, "工艺BOM生成".L10N());

            if (designTreeBomDetails.Any() || designTreeRoutingParamers.Any())
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    var ctl = RT.Service.Resolve<CommonController>();
                    treeRoutingIds.SplitDataExecute(tempIds =>
                    {
                        // 清空原本数据
                        DB.Delete<DesignTreeRoutingProBom>().Where(p => tempIds.Contains(p.DesignTreeRoutingId)).Execute();
                        DB.Delete<DesignTreeRoutingParamer>().Where(p => tempIds.Contains(p.DesignTreeRoutingId)).Execute();
                    });
                    // 更新插入
                    ctl.BatchInsertSave(designTreeBomDetails);
                    ctl.BatchInsertSave(designTreeRoutingParamers);
                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 校验工艺路线工序清单规则
        /// 0.首工序不能为包装或维修
        /// 1.检验工序后面必须为维修工序
        /// 2.维修工序后面不能为维修工序
        /// 3.包装工序只能在最后一个
        /// </summary>
        /// <param name="details">工序清单明细</param>
        private void ValidateProcessDetailRules(EntityList<DesignTreeRoutingDetail> details)
        {
            if (!details.Any())
            {
                throw new ValidationException("当前工艺路线版本未维护工序清单明细".L10N());
            }

            // 3.包装工序只能在最后一个
            if (details.Any(p => (p.ProcessType == Tech.Processs.ProcessType.Packing || p.ProcessType == Tech.Processs.ProcessType.BatchPacking) && p.Index != details.Last().Index))
            {
                throw new ValidationException("包装工序顺序号必须为最后一个".L10N());
            }

            var singleTypes = RT.Service.Resolve<CommonController>().GetEnumByCategory<ProcessType>("Single");
            if (!(details.All(p => singleTypes.Contains((ProcessType)p.ProcessType)) || details.All(p => !singleTypes.Contains((ProcessType)p.ProcessType))))
            {
                throw new ValidationException("工序类型必须都为单体工序或都为批次工序".L10N());
            }

            // 1.检验工序后面必须为维修工序
            // 2.维修工序后面不能为维修工序
            for (int i = 0; i < details.Count - 1; i++)
            {
                var isPqc = details[i].ProcessType == ProcessType.Pqc || details[i].ProcessType == ProcessType.BatchPqc;
                var nextIsFix = details[i + 1].ProcessType == ProcessType.Fix || details[i + 1].ProcessType == ProcessType.BatchFix;
                var isFix = details[i].ProcessType == ProcessType.Fix || details[i].ProcessType == ProcessType.BatchFix;
                var isPack = details[i].ProcessType == ProcessType.Packing || details[i].ProcessType == ProcessType.BatchPacking;
                if (i == 0 && (isFix || isPack))
                {
                    throw new ValidationException("首工序不能是维修工序或包装工序".L10N());
                }
                if (isPqc && !nextIsFix)
                {
                    throw new ValidationException("检验工序下一工序必须是维修工序".L10N());
                }
                if (isFix && nextIsFix)
                {
                    throw new ValidationException("维修工序下一工序必须不能是维修工序".L10N());
                }
            }
        }

        /// <summary>
        /// 获取工序参数表
        /// </summary>
        /// <param name="processIds">工序Ids</param>
        private EntityList<ProcessParameter> GetProcessParameter(IEnumerable<double> processIds)
        {
            return RT.Service.Resolve<ProcessController>().GetProcessParameterByProcessId(processIds);
        }

        /// <summary>
        /// 组装工艺路线导入数据
        /// </summary>
        /// <param name="routingId">产品工艺路线Id</param>
        /// <param name="details">工艺路线工序清单</param>
        /// <param name="processParamerList">工艺路线工序参数</param>
        /// <returns></returns>
        private RoutingImportSaveViewModel GetRoutingImportSaveViewModel(double routingId, EntityList<DesignTreeRoutingDetail> details, EntityList<ProcessParameter> processParamerList)
        {
            var singleTypes = RT.Service.Resolve<CommonController>().GetEnumByCategory<ProcessType>("Single");
            RoutingImportSaveViewModel importViewModel = new RoutingImportSaveViewModel();
            importViewModel.RoutingId = routingId;
            importViewModel.IsPass = true;
            importViewModel.RowNum = 0;

            var i = 1;
            var maxIndex = details.Count;
            foreach (var detail in details)
            {

                ProcessViewModel process = new ProcessViewModel();
                process.IsBatch = !singleTypes.Contains((ProcessType)detail.ProcessType);
                process.ProcessId = detail.ProcessId;
                process.ProcessType = detail.ProcessType.Value;
                process.ProcessName = detail.ProcessName;
                process.IsOutsourcing = detail.Outsourcing;
                process.IsGenerateTask = detail.IsGenerateTask;
                process.IsRequirementTask = detail.IsRequirementTask;
                process.CanChoose = detail.IsOptional;
                process.SortOrder = i;

                // 检验工序需要多增一条记录
                if (detail.ProcessType == ProcessType.Pqc || detail.ProcessType == ProcessType.BatchPqc)
                {
                    var paramer = processParamerList.FirstOrDefault(p => p.ProcessId == detail.ProcessId && p.Type == ResultTypeForDesign.Fail);
                    if (paramer == null)
                    {
                        throw new ValidationException("工序[{0}]不存在[{1}]工序参数".L10nFormat(detail.ProcessName, ResultTypeForDesign.Fail.ToLabel()));
                    }
                    process.ParameterId = paramer.Id;
                    process.ResultDesc = paramer.Description;
                    process.Script = paramer.Script;
                    // 默认失败进入维修工序
                    process.Result = paramer.Type;
                    process.ResultDesc = paramer.Type.ToLabel();
                    process.SortOrderBack = i + 1 > maxIndex ? 0 : i + 1;


                    // 额外创建一条成功
                    var successParamer = processParamerList.FirstOrDefault(p => p.ProcessId == detail.ProcessId && p.Type == ResultTypeForDesign.Pass);
                    if (paramer == null)
                    {
                        throw new ValidationException("工序[{0}]不存在[{1}]工序参数".L10nFormat(detail.ProcessName, ResultTypeForDesign.Pass.ToLabel()));
                    }
                    ProcessViewModel successPqcProcess = new ProcessViewModel();
                    successPqcProcess.IsBatch = !singleTypes.Contains((ProcessType)detail.ProcessType);
                    successPqcProcess.ProcessId = detail.ProcessId;
                    successPqcProcess.ParameterId = successParamer.Id;
                    successPqcProcess.ResultDesc = successParamer.Description;
                    successPqcProcess.Script = successParamer.Script;
                    successPqcProcess.ProcessType = detail.ProcessType.Value;
                    successPqcProcess.ProcessName = detail.ProcessName;
                    successPqcProcess.IsOutsourcing = detail.Outsourcing;
                    successPqcProcess.CanChoose = detail.IsOptional;
                    successPqcProcess.SortOrder = i;
                    successPqcProcess.Result = successParamer.Type;
                    successPqcProcess.ResultDesc = successParamer.Type.ToLabel();
                    successPqcProcess.SortOrderBack = i + 2 > maxIndex ? 0 : i + 2;
                    importViewModel.ProcessDetailModelList.Add(successPqcProcess);
                }
                else if (detail.ProcessType == ProcessType.Fix || detail.ProcessType == ProcessType.BatchFix)
                {
                    var fixParamer = processParamerList.FirstOrDefault(p => p.ProcessId == detail.ProcessId && p.Type == ResultTypeForDesign.Pass);
                    if (fixParamer == null)
                    {
                        throw new ValidationException("工序[{0}]不存在[{1}]工序参数".L10nFormat(detail.ProcessName, ResultTypeForDesign.Pass.ToLabel()));
                    }
                    process.ParameterId = fixParamer.Id;
                    process.ResultDesc = fixParamer.Description;
                    process.Script = fixParamer.Script;
                    // 默认通过返回检验工序
                    process.Result = fixParamer.Type;
                    process.ResultDesc = fixParamer.Type.ToLabel();
                    process.SortOrderBack = i - 1;
                }
                else
                {
                    var normalParamer = processParamerList.FirstOrDefault(p => p.ProcessId == detail.ProcessId && (p.Type == ResultTypeForDesign.Any || p.Type == ResultTypeForDesign.Pass));
                    if (normalParamer == null)
                    {
                        throw new ValidationException("工序[{0}]不存在[{1}]或[{2}]工序参数".L10nFormat(detail.ProcessName, ResultTypeForDesign.Any.ToLabel(), ResultTypeForDesign.Pass.ToLabel()));
                    }
                    process.ParameterId = normalParamer.Id;
                    process.ResultDesc = normalParamer.Description;
                    process.Script = normalParamer.Script;
                    // 默认失败进入维修工序
                    process.Result = normalParamer.Type;
                    process.ResultDesc = normalParamer.Type.ToLabel();
                    process.SortOrderBack = i + 1 > maxIndex ? 0 : i + 1;
                }
                importViewModel.ProcessDetailModelList.Add(process);
                i++;
            }
            // 存在包装工序需要在首工序创建sku
            if (importViewModel.ProcessDetailModelList.Last().ProcessType == ProcessType.Packing || importViewModel.ProcessDetailModelList.Last().ProcessType == ProcessType.BatchPacking)
            {
                importViewModel.ProcessDetailModelList.First().IsCreateSku = true;
            }

            return importViewModel;
        }

        /// <summary>
        /// 版本更新校验
        /// </summary>
        /// <param name="routing">产品工艺路线设置</param>
        /// <param name="treeBomInfo">项目信息</param>
        private void UpdateVersionValidate(DesignTreeRouting routing, TreeBomInfo treeBomInfo)
        {
            var count = Query<ProductRouting>().Where(p => p.Id != routing.Id && p.ProductId == routing.ProductId && p.OrderType == routing.OrderType && p.ProjectMaintainId == treeBomInfo.ProjectMaintainId && p.StartDate <= routing.EndDate && p.EndDate >= routing.StartDate).Count();
            if (count > 0)
            {
                throw new ValidationException("同一产品、工单类型、项目号、时间范围内产品工艺路线设置只能有一条".L10N());
            }
        }

        /// <summary>
        /// 工艺资料-产品工艺路线版本更新
        /// </summary>
        /// <param name="treeBomInfo"></param>
        public virtual void TreeRoutingUpDateVersion(TreeBomInfo treeBomInfo)
        {
            // 查询工艺路线设置
            var routing = RF.GetById<DesignTreeRouting>(treeBomInfo.Id);
            // 产品工艺路线设置必填校验
            ValidateRequire(new EntityList<DesignTreeRouting> { routing });
            // 查询工序清单明细
            var processDetailList = GetSortDesignTreeRoutingDetails(routing.Id);
            // 校验工序清单合理性
            ValidateProcessDetailRules(processDetailList);
            // 创建工艺路线导入数据
            var processIds = processDetailList.Select(p => p.ProcessId).ToList();
            var processParamerList = GetProcessParameter(processIds);
            var importData = GetRoutingImportSaveViewModel(routing.RoutingId.Value, processDetailList, processParamerList);

            ProductRouting productRouting;
            // 判断是否已经创建
            productRouting = RT.Service.Resolve<ProductRoutingController>().GetSameProductRouting(routing.OrderType.Value, routing.ProductId, routing.RoutingId.Value, treeBomInfo.ProjectMaintainId, routing.StartDate.Value, routing.EndDate);
            // 否则创建新的产品工艺路线设置
            if (productRouting == null)
            {
                // 产品工艺路线设置更新校验主键是否重复
                UpdateVersionValidate(routing, treeBomInfo);
                // 创建产品工艺路线设置
                productRouting = new ProductRouting
                {
                    StartDate = routing.StartDate.Value,
                    EndDate = routing.EndDate,
                    OrderType = routing.OrderType.Value,
                    RoutingId = routing.RoutingId,
                    ProductId = routing.ProductId,
                    ProjectMaintainId = treeBomInfo.ProjectMaintainId,
                };
            }

            // 查询工序bom清单
            var processBomList = GetDesignTreeRoutingProBoms(routing.Id);
            RoutingBom processBom;
            // 判断是否已创建
            processBom = RT.Service.Resolve<RoutingBomController>().GetSameRoutingBom(routing.ProductId, treeBomInfo.ProjectMaintainId, routing.RoutingId.Value);
            // 否则创建新的工序bom设置
            if (processBom == null)
            {
                // 创建工序bom设置
                processBom = new RoutingBom
                {
                    ProductId = routing.ProductId,
                    ProjectMaintainId = treeBomInfo.ProjectMaintainId,
                    RoutingId = routing.RoutingId.Value,
                };
                processBom.GenerateId();
            }

            EntityList<RoutingBomDetail> routingBomDetails = new EntityList<RoutingBomDetail>();
            // 创建工序bom设置明细
            foreach (var bom in processBomList)
            {
                var detail = new RoutingBomDetail
                {
                    RoutingBomId = processBom.Id,
                    MaterialId = bom.MaterialId,
                    Amount = bom.Amount,
                    RoutingProcessId = bom.RoutingProcessId, // 暂时记下项目号工序bom对应项目号工序清单Id
                };
                routingBomDetails.Add(detail);
            }


            // 创建日志
            // 生成一笔日志
            var dbTime = RF.Find<ProjectDesign>().GetDbTime();
            var log = CreateProjectDesignLog(treeBomInfo.DesignDetailId, OperatePoint.PTreeRoutingInit, dbTime, "更新工艺路线版本及工艺BOM信息".L10N());

            var saveCtl = RT.Service.Resolve<CommonController>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 创建工艺路线
                var versionId = RT.Service.Resolve<RoutingController>().ImportRoutingThenRelease(importData, processParamerList);
                productRouting.RoutingVersionId = versionId;
                processBom.RoutingVersionId = versionId;
                // 获取版本号获取工艺路线工序清单（装配、包装）
                List<ProcessType?> processTypes = new List<ProcessType?> { ProcessType.Assembly, ProcessType.Packing, ProcessType.BatchAssembly, ProcessType.BatchPacking };
                // 获取生成的工艺路线工序清单
                var routingProcessList = RT.Service.Resolve<RoutingSettingController>().GetRoutingProcesses(versionId, processTypes);
                // 匹配原始数据
                var processDetailIList = processDetailList.Where(p => processTypes.Contains(p.ProcessType)).ToList();
                routingBomDetails.ForEach(detail =>
                {
                    var i = processDetailIList.FindIndex(p => p.Id == detail.RoutingProcessId);
                    detail.RoutingProcessId = routingProcessList[i].Id;
                });

                // 更新产品工艺路线版本号
                DB.Update<DesignTreeRouting>().Set(p => p.RoutingVersionId, versionId).Set(p => p.HasUp, true).Where(p => p.Id == treeBomInfo.Id).Execute();
                // 产品工艺路线设置
                if (productRouting.PersistenceStatus == PersistenceStatus.New)
                {
                    // 保存产品工艺路线设置
                    saveCtl.BatchInsertSave(new EntityList<ProductRouting> { productRouting });
                }
                else
                {
                    // 更新产品工艺路线设置的版本
                    DB.Update<ProductRouting>().Set(p => p.RoutingVersionId, productRouting.RoutingVersionId).Where(p => p.Id == productRouting.Id).Execute();
                }
                // 工序BOM设置
                if (processBom.PersistenceStatus == PersistenceStatus.New)
                {
                    // 保存工序BOM设置
                    saveCtl.BatchInsertSave(new EntityList<RoutingBom> { processBom });
                    // 保存新的工序BOM明细
                    saveCtl.BatchInsertSave(routingBomDetails);
                }
                else
                {
                    // 更新工序bom版本
                    DB.Update<RoutingBom>().Set(p => p.RoutingVersionId, processBom.RoutingVersionId).Where(p => p.Id == processBom.Id).Execute();
                    // 清空原本工序bom明细
                    DB.Delete<RoutingBomDetail>().Where(p => p.RoutingBomId == processBom.Id).Execute();
                    // 保存新的工序BOM明细
                    saveCtl.BatchInsertSave(routingBomDetails);
                }


                // 保存日志
                saveCtl.BatchInsertSave(new EntityList<ProjectDesignLog> { log });
                // 更新工艺资料为已补充
                DB.Update<ProjectDesign>().Set(p => p.RoutingInfo, ChildInfoStatus.HasFilled).Where(p => p.Id == treeBomInfo.DesignDetailId).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 项目号需求设计-设计完成命令
        /// </summary>
        /// <param name="selIds">选中项目号设计的Ids</param>
        public virtual void DesignComplete(IEnumerable<double> selIds)
        {
            if (!selIds.Any())
            {
                return;
            }
            // 校验项目号设计工艺资料和产品bom均已设计
            if (!CheckDesignIsFillInForComplete(selIds))
            {
                throw new ValidationException("工艺资料或产品BOM未设计，不允许设计完成".L10N());
            }

            EntityList<ProjectDesignLog> logs = new EntityList<ProjectDesignLog>();
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 创建日志
            foreach (var id in selIds)
            {
                logs.Add(CreateProjectDesignLog(id, OperatePoint.DesignComplete, dbDate, string.Empty));
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 更新状态启用
                selIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProjectDesign>().Set(p => p.State, State.Enable).Where(p => tempIds.Contains(p.Id)).Execute();
                });
                // 创建操作日志
                RT.Service.Resolve<CommonController>().BatchInsertSave(logs);
                tran.Complete();
            }
        }

        /// <summary>
        /// 项目号需求设计-审核命令
        /// </summary>
        /// <param name="selIds">选中项目号设计的Ids</param>
        public virtual void DesignExamine(IEnumerable<double> selIds)
        {
            if (!selIds.Any())
            {
                return;
            }

            if (!CheckDesignIsFillInForExamine(selIds))
            {
                throw new ValidationException("工艺资料、产品BOM未设计，或审核状态不为未审核".L10N());
            }

            EntityList<ProjectDesignLog> logs = new EntityList<ProjectDesignLog>();
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 创建日志
            foreach (var id in selIds)
            {
                logs.Add(CreateProjectDesignLog(id, OperatePoint.DesignExamine, dbDate, string.Empty));
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 更新状态启用、审核状态、审核人、审核时间
                selIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProjectDesign>()
                    .Set(p => p.State, State.Enable)
                    .Set(p => p.ExamineStatus, ExamineStatus.Examined)
                    .Set(p => p.ExaminerId, RT.IdentityId)
                    .Set(p => p.ExamineDate, dbDate)
                    .Where(p => tempIds.Contains(p.Id)).Execute();
                });
                // 创建操作日志
                RT.Service.Resolve<CommonController>().BatchInsertSave(logs);
                tran.Complete();
            }
        }

        /// <summary>
        /// 项目号需求设计-反审命令
        /// </summary>
        /// <param name="selIds">选中项目号设计的Ids</param>
        public virtual void DesignAgainstExamine(IEnumerable<double> selIds)
        {
            if (!selIds.Any())
            {
                return;
            }
            // 判断当前选中的项目号需求状态均为已审核
            if (!CheckdesignIsExamine(selIds))
            {
                throw new ValidationException("项目号需求设计未审核，不允许进行反审".L10N());
            }

            EntityList<ProjectDesignLog> logs = new EntityList<ProjectDesignLog>();
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 创建日志
            foreach (var id in selIds)
            {
                logs.Add(CreateProjectDesignLog(id, OperatePoint.DesignAgainstExamine, dbDate, string.Empty));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 更新状态启用、审核状态、审核人、审核时间
                selIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProjectDesign>()
                    .Set(p => p.State, State.Disable)
                    .Set(p => p.ExamineStatus, ExamineStatus.UnExamine)
                    .Set(p => p.ExaminerId, p => null)
                    .Set(p => p.ExamineDate, p => null)
                    .Where(p => tempIds.Contains(p.Id)).Execute();
                });
                // 创建操作日志
                RT.Service.Resolve<CommonController>().BatchInsertSave(logs);
                tran.Complete();
            }
        }

        /// <summary>
        /// 项目号需求设计-启用命令
        /// </summary>
        /// <param name="selIds">选中项目号设计的Ids</param>
        public virtual void DesignEnable(IEnumerable<double> selIds)
        {
            if (!selIds.Any())
            {
                return;
            }
            // 判断当前选中的项目号需求状态均为禁用
            if (!CheckdesignIsDisable(selIds))
            {
                throw new ValidationException("项目号需求设计审核状态不为已审核或状态不为禁用，不允许启用".L10N());
            }

            EntityList<ProjectDesignLog> logs = new EntityList<ProjectDesignLog>();
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 创建日志
            foreach (var id in selIds)
            {
                logs.Add(CreateProjectDesignLog(id, OperatePoint.Enable, dbDate, string.Empty));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 更新状态启用、审核状态、审核人、审核时间
                selIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProjectDesign>()
                    .Set(p => p.State, State.Enable)
                    .Where(p => tempIds.Contains(p.Id)).Execute();
                });
                // 创建操作日志
                RT.Service.Resolve<CommonController>().BatchInsertSave(logs);
                tran.Complete();
            }
        }

        /// <summary>
        /// 项目号需求设计-禁用命令
        /// </summary>
        /// <param name="selIds">选中项目号设计的Ids</param>
        public virtual void DesignDisable(IEnumerable<double> selIds)
        {
            if (!selIds.Any())
            {
                return;
            }
            // 判断当前选中的项目号需求状态均为禁用
            if (!CheckdesignIsEnable(selIds))
            {
                throw new ValidationException("项目号需求设计状态不为启用，不允许禁用".L10N());
            }

            EntityList<ProjectDesignLog> logs = new EntityList<ProjectDesignLog>();
            var dbDate = RF.Find<ProjectDesign>().GetDbTime();
            // 创建日志
            foreach (var id in selIds)
            {
                logs.Add(CreateProjectDesignLog(id, OperatePoint.Disable, dbDate, string.Empty));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 更新状态启用、审核状态、审核人、审核时间
                selIds.SplitDataExecute(tempIds =>
                {
                    DB.Update<ProjectDesign>()
                    .Set(p => p.State, State.Disable)
                    .Where(p => tempIds.Contains(p.Id)).Execute();
                });
                // 创建操作日志
                RT.Service.Resolve<CommonController>().BatchInsertSave(logs);
                tran.Complete();
            }
        }
        #endregion
    }
}
