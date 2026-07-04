using SIE.Data;
using SIE.Domain;
using SIE.MES.WIP.PackRecombine.Logs;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.PackRecombine.Relations
{
    /// <summary>
    /// 包装关系查询控制器
    /// </summary>
    public partial class PackingRelationQueryController : DomainController
    {
        /// <summary>
        /// 根据包装操作日志查询体获取包装操作日志列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<RecombineLog> GetRecombineLogs(RecombineLogCriteria criteria)
        {
            var query = Query<RecombineLog>();
            if (criteria.PackageNo.IsNotEmpty())
                query.Where(p => p.PackageNo.Contains(criteria.PackageNo));
            if (criteria.ParentNo.IsNotEmpty())
                query.Where(p => p.ParentNo.Contains(criteria.ParentNo));
            if (criteria.ScanMode.HasValue)
                query.Where(p => p.ScanMode == criteria.ScanMode);
            if (criteria.IsBatch.HasValue)
            {
                if (criteria.IsBatch == YesNo.Yes)
                    query.Where(p => p.IsBatch);
                else
                    query.Where(p => !p.IsBatch);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取异常停线列表
        /// </summary>
        /// <param name="criteria">异常停线查询实体</param>
        /// <returns>异常停线信息</returns>
        public virtual EntityList<PackingRelationQuery> GetPackingRelationQuerys(PackingRelationQueryCriteria criteria)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var packRelationList = new EntityList<PackingRelationQuery>();

                //按查询条件获取所有根节点
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria));
                }
                var query = Query<PackingRelationQuery>();
                if (criteria.PackageNo.IsNotEmpty())
                {
                    query.Where(p => p.PackageNo.Contains(criteria.PackageNo));
                }
                if (criteria.WorkOrderNo.IsNotEmpty())
                {
                    query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
                }
                if (criteria.Product != null)
                {
                    query.Where(p => p.WorkOrder.Product.Id == criteria.ProductId);
                }
                if (criteria.Resource != null)
                {
                    query.Where(p => p.WorkOrder.ResourceId == criteria.ResourceId);
                }
                if (criteria.Process != null)
                {
                    query.Where(p => p.ProcessId == criteria.ProcessId);
                }
                //if (criteria.IsPacked.HasValue)
                //{
                //    query.Where(p => p.IsPacked == (criteria.IsPacked == YesNo.Yes));
                //}
                if (criteria.IsPacked.HasValue)
                {
                    if (criteria.IsPacked == YesNo.Yes)
                    {
                        query.Where(p => p.PackageNo != null || p.PackageNo != string.Empty || p.PackageNo != "");

                    }
                    else
                    {
                        query.Where(p => p.PackageNo == null || p.PackageNo == string.Empty || p.PackageNo == "");

                    }
                }
                if (criteria.PackedDate.BeginValue.HasValue)
                {
                    query.Where(p => p.PackedDate >= criteria.PackedDate.BeginValue);
                }
                if (criteria.PackedDate.EndValue.HasValue)
                {
                    query.Where(p => p.PackedDate <= criteria.PackedDate.EndValue);
                }
                if (criteria.Sn.IsNotEmpty())
                {
                    query.Exists<ItemLabel>((a, b) => b.Where(c => c.RelationId == a.Id && c.Label == criteria.Sn));
                }
                var rootIdList = query.Select(p => p.RootId).Distinct().ToList<double>();

                //根据根节点获取所有包装关系
                if (rootIdList.Count > 0)
                {
                    for (int i = 0; i < Math.Ceiling((double)rootIdList.Count / 1000); i++)
                    {
                        var queryRsult = Query<PackingRelationQuery>().Where(p => rootIdList.Skip(i * 1000).Take(1000).Contains(p.RootId));
                        packRelationList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
                    }

                    //根据查询到的包装关系补充ItemLabel
                    var relationIds = packRelationList.Select(p => p.Id).Distinct().ToList();
                    var itemLabelList = RT.Service.Resolve<ItemLabelController>().GetItemLabelByRelationId(relationIds);
                    var relationMaxId = relationIds.Max();
                    foreach (var label in itemLabelList)
                    {
                        var relation = packRelationList.FirstOrDefault(p => p.Id == label.RelationId);
                        var labelRelation = CreatePackingRelation(relation, label);
                        relationMaxId = relationMaxId + 1;
                        labelRelation.Id = relationMaxId;
                        packRelationList.Add(labelRelation);
                    }
                }
                packRelationList.MarkSaved();
                return packRelationList;
            }
        }

        /// <summary>
        /// 获取所有包装关系列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>包装关系列表</returns>
        public virtual EntityList<PackingRelationQuery> GetAllPackRelations(PackingRelationQueryCriteria criteria)
        {
            return Query<PackingRelationQuery>().OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 创建物料标签的包装关系
        /// </summary>
        /// <param name="relation">包装关系</param>
        /// <param name="label">物料标签</param>
        /// <returns>物料标签的包装关系</returns>
        private PackingRelationQuery CreatePackingRelation(PackingRelationQuery relation, ItemLabel label)
        {
            return new PackingRelationQuery
            {
                TreePId = relation.Id,
                RootId = relation.RootId,
                PackageNo = label.Label,
                ItemQty = label.Qty,
                WorkOrderId = label.WorkOrderId.Value,
                WorkOrderNo = label.WorkOrderNo,
                ProductId = relation.ProductId,
                ProductName = relation.ProductName,
                ResourceId = relation.ResourceId,
                ResourceName = relation.ResourceName,
                ProcessId = relation.ProcessId,
                ProcessName = relation.ProcessName,
                StationId = relation.StationId,
                StationName = relation.StationName,
                PackedDate = label.CreateDate
            };
        }

    }
}
