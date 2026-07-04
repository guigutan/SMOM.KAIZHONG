using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.Items;
using SIE.Tech.Routings.ViewModels;
using SIE.MES.Routings.RoutingBoms.ImportBoms;
using SIE.Tech.Processs;
using SIE.Domain.Validation;
using SIE.Resources.ProcessSegments;
using SIE.Tech.Routings;
using SIE.Tech.ViewModels;
using IronPython.Modules;
using SIE.Common.Algorithm;
using SIE.MES.WIP.Runtime;
using SIE.Common;
using SIE.MES.Routings.RoutingBoms.ApiModels;

namespace SIE.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工序bom Controller
    /// </summary>
    public class RoutingBomController : DomainController
    {
        #region 工序bom处理 （迁移自ElecRoutingController）
        /// <summary>
        /// 获取工序Bom主表信息
        /// </summary>
        /// <param name="productId">产品</param>
        /// <param name="routingId">工序</param>
        /// <param name="versionId">版本</param>
        /// <param name="segmentId">工段</param>
        /// <returns></returns>
        public virtual RoutingBom GetRoutingBom(double productId, double routingId, double versionId, double? segmentId)
        {
            return Query<RoutingBom>()
                .Where(p => p.ProductId == productId && p.RoutingId == routingId && p.RoutingVersionId == versionId && p.ProcessSegmentId == segmentId)
                .ToList()
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取工序Bom主表信息
        /// </summary>
        /// <param name="productId">产品</param>
        /// <param name="routingId">工序</param>
        /// <param name="versionId">版本</param>
        /// <param name="segmentId">工段</param>
        /// <param name="projectMaintainId">项目号</param>
        /// <returns></returns>
        public virtual RoutingBom GetRoutingBom(double productId, double routingId, double versionId, double? segmentId, double? projectMaintainId)
        {
            return Query<RoutingBom>()
                .Where(p => p.ProductId == productId && p.RoutingId == routingId && p.RoutingVersionId == versionId && p.ProcessSegmentId == segmentId && p.ProjectMaintainId == projectMaintainId)
                .ToList()
                .FirstOrDefault();
        }

        /// <summary>
        /// 查询工序Bom主表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<RoutingBom> GetRoutingBoms(RoutingBomCriteria criteria)
        {
            var query = Query<RoutingBom>();

            query.WhereIf(criteria.ProductCode.IsNotEmpty(),
                p => p.Product.Code.Contains(criteria.ProductCode.Trim()));
            query.WhereIf(criteria.ProductName.IsNotEmpty(),
                    p => p.Product.Name.Contains(criteria.ProductName.Trim()));
            query.WhereIf(criteria.ProcessSegmentId.HasValue,
                p => p.ProcessSegmentId == criteria.ProcessSegmentId);

            if (!criteria.ItemCode.IsNullOrEmpty())
            {
                query.Exists<RoutingBomDetail>((a, b) => b.Where(k => k.RoutingBomId == a.Id && k.Material.Code.Contains(criteria.ItemCode)));
            }
            if (!criteria.ItemName.IsNullOrEmpty())
            {
                query.Exists<RoutingBomDetail>((a, b) => b.Where(k => k.RoutingBomId == a.Id && k.Material.Name.Contains(criteria.ItemName)));
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询工序Bom主表
        /// </summary>
        /// <param name="productIds">产品 Id 列表</param>
        /// <param name="versionIds">工艺路线版本 Id 列表</param>
        /// <returns></returns>
        public virtual EntityList<RoutingBom> GetRoutingBoms(List<double> productIds, List<double> versionIds)
        {
            return productIds.SplitContains(tempProductIds =>
            {
                return versionIds.SplitContains(tempVersionIds =>
                {
                    return Query<RoutingBom>()
                        .Where(x => tempProductIds.Contains(x.ProductId) && tempVersionIds.Contains(x.RoutingVersionId))
                        .ToList();
                });
            });
        }

        /// <summary>
        /// 获取工艺路线版本工序bom明细
        /// </summary>
        /// <param name="productId">产品</param>
        /// <param name="versionId">版本</param>
        /// <param name="segmentId">工段</param>
        /// <returns></returns>
        public virtual EntityList<RoutingBomDetail> GetRoutingBomDetails(double productId, double versionId, double? segmentId)
        {
            return Query<RoutingBomDetail>()
                .Where(p => p.RoutingBom.ProductId == productId && p.RoutingBom.RoutingVersionId == versionId
                    && p.RoutingBom.ProcessSegmentId == segmentId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工艺路线版本工序bom明细
        /// </summary>
        /// <param name="routingBomIds">产品</param> 
        /// <returns></returns>
        public virtual EntityList<RoutingBomDetail> GetRoutingBomDetails(List<double> routingBomIds)
        {
            return routingBomIds.SplitContains(tempIds =>
            {
                return Query<RoutingBomDetail>()
                    .Where(p => tempIds.Contains(p.RoutingBomId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取工序bom明细是否存在
        /// </summary>
        /// <param name="RoutingBomId">工序bom主表Id</param>
        /// <returns></returns>
        public virtual bool RoutingBomDetailsExists(double RoutingBomId)
        {
            return Query<RoutingBomDetail>()
                .Where(p => p.RoutingBomId == RoutingBomId)
                .Count() > 0;
        }

        /// <summary>
        /// 获取工序BOM物料的单机用量总和
        /// </summary>
        /// <param name="RoutingBomId"></param>
        /// <param name="itemId">物料Id</param>
        /// <param name="RoutingBomDetailId">排除当前产品ID，如果为0则不排除</param>
        /// <returns></returns>
        public virtual decimal GetRountingBomUnitQty(double RoutingBomId, double itemId, double RoutingBomDetailId)
        {
            var lst = Query<RoutingBomDetail>()
                .Where(p => p.RoutingBomId == RoutingBomId
                && p.MaterialId == itemId && p.Id != RoutingBomDetailId)
                .ToList();
            return lst.Sum(p => p.Amount);
        }

        /// <summary>
        /// 获取产品工艺路线配置的工艺路线信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns>工艺路线版本集合</returns>
        public virtual EntityList<Routing> GetRoutingByProductRouting(double productId, PagingInfo pagingInfo, string key)
        {
            var routings = Query<SIE.MES.RoutingSettings.ProductRouting>()
                .Where(p => p.ProductId == productId)
                .ToList();
            var ids = routings.Select(p => p.RoutingId).ToList();

            return Query<Routing>()
                .Where(p => ids.Contains(p.Id))
                .WhereIf(string.IsNullOrEmpty(key), p => p.Name.Contains(key))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品工艺路线配置的工段信息
        /// </summary>
        /// <param name="routingId">工艺路线ID</param>
        /// <param name="productId">产品ID</param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns>工段</returns>
        public virtual EntityList<ProcessSegment> GetProcessSegmentByProductRouting(double routingId, double productId, PagingInfo pagingInfo, string key)
        {
            var routings = Query<SIE.MES.RoutingSettings.ProductRouting>()
                .Where(p => p.RoutingId == routingId && p.ProductId == productId)
                .ToList();
            var segmentIds = routings.Select(p => p.ProcessSegmentId).ToList();

            return Query<ProcessSegment>()
                .Where(p => segmentIds.Contains(p.Id))
                .WhereIf(string.IsNullOrEmpty(key), p => p.Code.Contains(key))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存导入的工序bom数据
        /// </summary>
        /// <param name="boms"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public virtual bool SaveRoutingBomDetail(List<RoutingBomDetail> boms, List<RoutingBomImportRecord> records)
        {
            if (boms == null || records == null)
            {
                return false;
            }
            bool bRet = false;
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var ids = boms.Select(p => p.RoutingBomId).ToList();
                DB.Delete<RoutingBomDetail>().Where(p => ids.Contains(p.RoutingBomId)).Execute();
                if (boms.Any())
                {
                    RF.Save(boms.AsEntityList());
                }
                if (records.Any())
                {
                    RF.Save(records.AsEntityList());
                }

                tran.Complete();
                bRet = true;
            }
            return bRet;
        }


        /// <summary>
        /// 获取导入日志
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<RoutingBomImportRecord> GetRoutingBomDetailImportRecordList(
            double RoutingBomId,
            PagingInfo pagingInfo = null,
            IList<OrderInfo> sortInfo = null)
        {
            return Query<RoutingBomImportRecord>()
                .Where(p => p.RoutingBomId == RoutingBomId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions()
                .LoadWithViewProperty());
        }

        /// <summary>
        /// 获取导入日志
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<RoutingBomDetail> GetBomImportRecordByAttachment(
            double attachmentId,
            PagingInfo pagingInfo = null,
            IList<OrderInfo> sortInfo = null)
        {
            return Query<RoutingBomDetail>()
                .Where(p => p.AttachmentId == attachmentId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions()
                .LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工序BOM主表和版本获取工序
        /// 如果工序bom主表不存在则返回空列表
        /// </summary>
        /// <param name="routingVersionId">工艺路线版本ID</param>
        /// <returns>工序列表</returns>
        public virtual EntityList<RoutingProcess> GetRoutingProcessesExceptGroup(double routingVersionId)
        {
            return Query<RoutingProcess>()
                .Join<RoutingVersion>((x, y) => x.VersionId == y.Id)
                .Where(t => t.VersionId == routingVersionId && (t.IsGroup == null || t.IsGroup == false))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工序BOM主表和版本获取工序
        /// 如果工序bom主表不存在则返回空列表
        /// </summary>
        /// <param name="routingVersionId">工艺路线版本ID</param>
        /// <returns>工序列表</returns>
        public virtual EntityList<RoutingProcess> GetRoutingProcesses(double routingVersionId)
        {
            return Query<RoutingProcess>()
                .Join<RoutingVersion>((x, y) => x.VersionId == y.Id)
                .Where(t => t.VersionId == routingVersionId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工工步信息
        /// </summary>
        /// <param name="routingProcessId">工单工序ID</param>
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<WorkStep> GetWorkSteps(double routingProcessId)
        {
            var routingProcess = RF.GetById<RoutingProcess>(routingProcessId);
            if (routingProcess == null)
            {
                return new EntityList<WorkStep>();

            }

            return Query<WorkStep>().Where(t => t.ProcessId == routingProcess.ProcessId).OrderBy(t => t.SeqNumber).ToList();
        }

        /// <summary>
        /// 根据工序BOM主表和版本获取工序
        /// 如果工序bom主表不存在则返回空列表
        /// </summary>
        /// <param name="RoutingBomId">工序BOM主表ID</param>
        /// <param name="routingVersionId">工艺路线版本ID</param>
        /// <returns>工序列表</returns>
        public virtual EntityList<Process> GetRoutingBomProcesses(double RoutingBomId, double routingVersionId)
        {
            EntityList<Process> lst = new EntityList<Process>();
            var routing = Query<RoutingBom>().Where(t => t.Id == RoutingBomId).FirstOrDefault();
            if (routing == null)
            {
                return lst;
            }

            RoutingVersion rv = Query<RoutingVersion>().Where(t => t.Id == routingVersionId).FirstOrDefault();
            if (rv == null)
            {
                return lst;
            }

            var rpLst = Query<RoutingProcess>().Where(t => t.VersionId == rv.Id).ToList().Select(t => t.ProcessId).ToList();
            if (rpLst.Count == 0)
            {
                return lst;
            }

            return Query<Process>().Where(t => rpLst.Contains(t.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存工序bom附件
        /// </summary>
        /// <param name="base64Str">附件内容</param>
        /// <param name="cadManageId">CAD数据ID</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileSize">文件大小</param>
        /// <returns>CAD附件Id</returns>
        public virtual double SaveBomAttachment(string base64Str, double cadManageId, string fileName, double fileSize)
        {
            if (base64Str == null || fileName == null)
            {
                throw new ValidationException("文件内容或者上传文件名有误".L10N());
            }
            var name = fileName.Split('.');
            if (name.Length != 2)
            {
                throw new ValidationException("上传文件名有误".L10N());
            }
            var base64Index = base64Str.IndexOf("base64,") + 7;
            var baseStr = "";
            if (base64Str.Length > base64Index)
            {
                baseStr = base64Str.Substring(base64Index);
            }

            var attachment = new RoutingBomAttachment();
            attachment.GenerateId();
            attachment.FileName = fileName;
            attachment.FilePath = "";
            attachment.FileExtesion = "." + name[1];
            attachment.FileSize = fileSize.ToString();
            attachment.Content = Convert.FromBase64String(baseStr);
            attachment.OwnerId = cadManageId;
            RF.Save(attachment);
            return attachment.Id;
        }

        /// <summary>
        /// 工序bom是否存在指定的工步
        /// </summary>
        /// <param name="workStepId">工步Id</param>
        /// <returns>是否包含</returns>
        public virtual bool IsRoutingBomDetailHasStep(double workStepId)
        {
            var count = Query<RoutingBomDetail>()
                .Where(p => p.WorkStepId == workStepId)
                .Count();
            return count > 0;
        }

        #endregion


        #region 产品 bom 相关，注意：这里是产品bom
        /// <summary>
        /// 获取主料信息(from product bom)
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="itemId">替代料Id</param>
        /// <param name="segmentId">工段Id</param>
        /// <returns></returns>
        public virtual Item GetMainMaterial(double productId, double itemId, double? segmentId)
        {
            var pbs = Query<ProductBom>().Where(p => p.ProductId == productId).ToList();
            if (pbs.Count == 0)
                return null;
            var bomDetails = new List<ProductBomDetail>();
            foreach (var pb in pbs)
            {
                var lst = GetProductBomDetailList(pb.Id, segmentId);
                bomDetails.AddRange(lst);
            }
            var detailIds = bomDetails.Select(s => s.Id).ToList();
            var altList = GetProductBomDetailAlternativeList(detailIds);
            var altItem = altList.FirstOrDefault(t => t.ItemId == itemId);
            if (altItem != null)
            {
                var detail = bomDetails.FirstOrDefault(t => t.Id == altItem.BomDetailId);
                if (detail != null)
                {
                    return RF.GetById<Item>(detail.ItemId);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取产品bom明细
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="segmentId">工段Id</param>
        /// <returns></returns>
        public virtual BomDetailViewModel GetRoutingBomDetailViewModel(double productId, double itemId, double? segmentId)
        {
            var pbs = Query<ProductBom>().Where(p => p.ProductId == productId).ToList();
            if (pbs.Count == 0)
                return null;
            var bomDetails = new List<BomDetailViewModel>();
            foreach (var pb in pbs)
            {
                var lst = GetProductBomDetailByBomId(pb.Id, segmentId);
                bomDetails.AddRange(lst);
            }
            return bomDetails.FirstOrDefault(t => t.ItemId == itemId);
        }



        /// <summary>
        /// 获取产品bom物料信息
        /// </summary>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="productId">产品Id</param>
        /// <param name="keyword">查询条件</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <param name="isAlt">是否带出替代料</param>
        /// <returns></returns>
        public virtual EntityList<Item> GetRoutingBomItemByProductId(PagingInfo pageInfo, double productId, string keyword, double? processSegmentId = null, bool isAlt = true)
        {
            // 取得产品bom，可能有多个
            var pbs = Query<ProductBom>().Where(p => p.ProductId == productId).Select(p => p.Id).ToList<double>();
            if (pbs.Count == 0)
                return new EntityList<Item>();
            List<double> itemIds = new List<double>();
            foreach (var id in pbs)
            {
                // 取产品bom明细的物料Id
                var ids = GetProductBomDetailByBomId(id, processSegmentId, isAlt).Select(p => p.ItemId).Distinct();
                itemIds.AddRange(ids);
            }
            // 取出所有物料
            var items = Query<Item>().Where(t => itemIds.Contains(t.Id))
                .WhereIf(!string.IsNullOrWhiteSpace(keyword), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());

            return items;
        }

        /// <summary>
        /// 取产品bom明细的物料
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="deck"></param>
        /// <param name="processSegmentId"></param>
        /// <param name="isAlt"></param>
        /// <returns></returns>
        public virtual EntityList<BomDetailViewModel> GetProductBomDetailByBomId(double bomId, double? processSegmentId = null, bool isAlt = true)
        {
            EntityList<BomDetailViewModel> list = new EntityList<BomDetailViewModel>();
            var allEntityList = GetProductBomDetailList(bomId, processSegmentId);
            if (!allEntityList.Any())//找不到数据
                return new EntityList<BomDetailViewModel>();
            var entityList = new EntityList<ProductBomDetail>();

            var virtualPart = allEntityList.Where(m => m.ItemIsVirtualPart).ToList();//虚拟件
            var vitruItemBoms = RT.Service.Resolve<SIE.Items.ProductBoms.ProductBomController>().GetVitruItemBom(virtualPart);
            if (vitruItemBoms.Any())
                entityList.AddRange(vitruItemBoms);

            var bomList = allEntityList.Where(m => !m.ItemIsVirtualPart).ToList();
            if (bomList.Any())
            {
                entityList.AddRange(bomList);
            }

            var detailIds = entityList.Select(s => s.Id).ToList();
            var propertyValueList = GetProductBomDetailPropertyValueList(detailIds);
            var alternativeList = GetProductBomDetailAlternativeList(detailIds);
            foreach (var item in entityList)
            {
                var itemPropertyValue = string.Empty;
                var nowPropertyValueList = propertyValueList.Where(w => w.DetailId == item.Id).ToList();
                if (nowPropertyValueList.Any())
                {
                    var groups = nowPropertyValueList.GroupBy(p => p.Definition.Name).ToList();//与清单统一
                    string[] result = new string[groups.Count];
                    for (int i = 0; i < groups.Count; i++)
                    {
                        var values = groups[i].Select(p => p.Value);
                        result[i] = groups[i].Key + "：" + string.Join("、", values);
                    }
                    itemPropertyValue = string.Join("；", result);
                }
                list.Add(GetProducBomMainBom(item, item.UnitQty, itemPropertyValue));
                if (isAlt)
                {
                    list.AddRange(GetProductBomAltList(alternativeList, item, item.UnitQty, itemPropertyValue));
                }
            }
            return list;
        }

        /// <summary>
        /// 主料
        /// </summary>
        /// <param name="bomDetail">bom明细</param>
        /// <param name="unitQty">数量</param>
        /// <param name="itemPropertyValue">物料属性值</param>
        /// <returns>转换列表</returns>
        private BomDetailViewModel GetProducBomMainBom(ProductBomDetail bomDetail, decimal unitQty, string itemPropertyValue)
        {
            return new BomDetailViewModel()
            {
                Id = bomDetail.Id.ToString(),
                ItemId = bomDetail.ItemId,
                ItemCode = bomDetail.ItemCode,
                ItemName = bomDetail.ItemName,
                SpecificationModel = bomDetail.ItemSpecificationModel,
                UnitQty = unitQty,
                ItemUnitCode = bomDetail.ItemUnitName,
                IsAltMaterial = false,
                MainMaterialCode = "",
                ProcessSegmentId = bomDetail.ProcessSegmentId,
                ProcessSegmentName = bomDetail.ProcessSegmentName,
                IsSplit = bomDetail.IsSplit,
                BomDetailId = bomDetail.Id,
                LossRate = bomDetail.LossRate,
                ItemPropertyValue = itemPropertyValue,
            };
        }

        /// <summary>
        /// 替代料
        /// </summary>
        /// <param name="alternativeList">替代料列表</param>
        /// <param name="bomDetail">bom明细</param>
        /// <param name="unitQty">数量</param>
        /// <param name="itemPropertyValue">物料属性值</param>
        /// <returns>转换列表</returns>
        private EntityList<BomDetailViewModel> GetProductBomAltList(EntityList<ProductBomDetailAlternative> alternativeList, ProductBomDetail bomDetail, decimal unitQty, string itemPropertyValue)
        {
            var list = new EntityList<BomDetailViewModel>();
            var nowAlternativeList = alternativeList.Where(w => w.BomDetailId == bomDetail.Id).ToList();
            foreach (var alternative in nowAlternativeList)
            {
                list.Add(new BomDetailViewModel()
                {
                    Id = alternative.Id.ToString(),
                    ItemId = alternative.ItemId,
                    ItemCode = alternative.ItemCode,
                    ItemName = alternative.ItemName,
                    SpecificationModel = alternative.ItemSpecificationModel,
                    UnitQty = unitQty,
                    ItemUnitCode = alternative.ItemUnitName,
                    IsAltMaterial = true,
                    MainMaterialCode = bomDetail.ItemCode,
                    ProcessSegmentId = bomDetail.ProcessSegmentId,
                    ProcessSegmentName = bomDetail.ProcessSegmentName,
                    IsSplit = bomDetail.IsSplit,
                    BomDetailId = bomDetail.Id,
                    LossRate = bomDetail.LossRate,
                    ItemPropertyValue = itemPropertyValue,
                });
            }
            return list;
        }

        /// <summary>
        /// 根据产品BOMID找明细
        /// </summary>
        /// <param name="bomId">产品BOMID</param>
        /// <param name="processSegmentId">工段Id</param>
        /// <returns>明细列表</returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetailList(double bomId, double? processSegmentId)
        {
            var query = Query<ProductBomDetail>().Where(w => w.ProductBomId == bomId);
            if (processSegmentId != null)
                query.Where(w => w.ProcessSegmentId == processSegmentId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }



        /// <summary>
        /// 根据产品BOM明细ID找产品BOM属性值
        /// </summary>
        /// <param name="detailIds">BOM明细ID</param>
        /// <returns>BOM属性值列表</returns>
        public virtual EntityList<ProductBomDetailPropertyValue> GetProductBomDetailPropertyValueList(List<double> detailIds)
        {
            var list = new EntityList<ProductBomDetailPropertyValue>();
            for (int i = 0; i < Convert.ToInt32(Math.Ceiling(detailIds.Count / 1000.00)); i++)
            {
                var cList = detailIds.Skip(i * 1000).Take(1000).ToList();
                list.AddRange(Query<ProductBomDetailPropertyValue>().Where(w => cList.Contains(w.DetailId)).ToList());
            }
            return list;
        }

        /// <summary>
        /// 根据产品BOM明细ID找产品BOM替代料
        /// </summary>
        /// <param name="detailIds">BOM明细ID</param>
        /// <returns>BOM替代料列表</returns>
        public virtual EntityList<ProductBomDetailAlternative> GetProductBomDetailAlternativeList(List<double> detailIds)
        {
            var list = new EntityList<ProductBomDetailAlternative>();
            for (int i = 0; i < Convert.ToInt32(Math.Ceiling(detailIds.Count / 1000.00)); i++)
            {
                var cList = detailIds.Skip(i * 1000).Take(1000).ToList();
                list.AddRange(Query<ProductBomDetailAlternative>().Where(w => cList.Contains(w.BomDetailId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return list;
        }

        /// <summary>
        /// 根据产品+工艺路线+版本获取产品bom明细
        /// </summary>
        /// <param name="productIds">产品Id</param>
        /// <param name="routingIds">工艺路线Id</param>
        /// <param name="versionIds">版本Id</param>
        /// <returns></returns>
        public virtual List<RtBomDtlInfo> GetRoutingBomDetailsByVersionId(IEnumerable<double> productIds, IEnumerable<double?> routingIds, IEnumerable<double?> versionIds)
        {
            List<RtBomDtlInfo> routingBomDetails = new List<RtBomDtlInfo>();
            productIds.SplitDataExecute(tempIds1 =>
            {
                routingIds.SplitDataExecute(tempIds2 =>
                {
                    versionIds.SplitDataExecute(tempIds3 =>
                    {
                        var list = Query<RoutingBomDetail>().Join<RoutingBom>((rbd, rb) => rbd.RoutingBomId == rb.Id && tempIds1.Contains(rb.ProductId) && tempIds2.Contains(rb.RoutingId) && tempIds3.Contains(rb.RoutingVersionId))
                        .Select<RoutingBom>((rbd, rb) => new
                        {
                            MaterialId = rbd.MaterialId,
                            Amount = rbd.Amount,
                            Description = rbd.Description,
                            RoutingProcessId = rbd.RoutingProcessId,
                            ProductId = rb.ProductId,
                            RoutingId = rb.RoutingId,
                            RoutingVersionId = rb.RoutingVersionId,
                        }).ToList<RtBomDtlInfo>();
                        routingBomDetails.AddRange(list);
                    });
                });
            });

            return routingBomDetails;
        }

        /// <summary>
        /// 获取同产品、项目、工艺路线的工序bom设置
        /// </summary>
        /// <param name="productId">产品</param>
        /// <param name="projectId">项目</param>
        /// <param name="routingId">工艺路线</param>
        /// <returns></returns>
        public virtual RoutingBom GetSameRoutingBom(double productId, double projectId, double routingId)
        {
            var q = Query<RoutingBom>().Where(p => p.ProductId == productId && p.ProjectMaintainId == projectId && p.RoutingId == routingId).ToList();
            return q.FirstOrDefault();
        }
        #endregion
    }
}
