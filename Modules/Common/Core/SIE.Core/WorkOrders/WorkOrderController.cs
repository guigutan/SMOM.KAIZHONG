using SIE.Api;
using SIE.Core.Common;
using SIE.Core.WorkOrders.Models;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.WorkOrders
{
    /// <summary>
    /// 工单控制器
    /// </summary>
    [ApiName("CoreWorkOrderController")]
    public partial class WorkOrderController : DomainController
    {
        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(WorkOrderCriteria criteria)
        {
            var q = Query<WorkOrder>();
            if (criteria.No.IsNotEmpty())
                q.Where(p => p.No.Contains(criteria.No));
            if (criteria.Item.IsNotEmpty())
                q.Where(p => p.Product.Code.Contains(criteria.Item));
            if (criteria.ItemName.IsNotEmpty())
                q.Where(p => p.Product.Name.Contains(criteria.ItemName));
            if (criteria.PlanBeginDate.BeginValue.HasValue && criteria.PlanBeginDate.EndValue.HasValue)
                q.Where(p => p.PlanBeginDate > criteria.PlanBeginDate.BeginValue && p.PlanEndDate < criteria.PlanBeginDate.EndValue);
            if (criteria.ProductCode.IsNotEmpty())
                q.Where(p => p.Product.Code.Contains(criteria.ProductCode));
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单Id列表获取工单列表
        /// </summary>
        /// <param name="ids">工单Id列表</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(List<double> ids)
        {
            return Query<WorkOrder>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取工单基本信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual List<WoBaseInfo> GetWorkOrderBaseInfos(List<double> ids)
        {
            return Query<WorkOrder>().Where(p => ids.Contains(p.Id)).Select(p => new
            {
                Id = p.Id,
                No = p.No,
            }).ToList<WoBaseInfo>().ToList();
        }

        /// <summary>
        /// 获取工单集合
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="no">单号</param>
        /// <returns>工单集合</returns>
        public virtual EntityList GetWorkOrders(PagingInfo pagingInfo, string no)
        {
            var query = Query<WorkOrder>();
            if (!no.IsNullOrEmpty())
                query.Where(p => p.No.Contains(no));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单集合
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="needFinish">需要完工工单</param>
        /// <returns>工单集合</returns>
        public virtual EntityList GetWorkOrders(string keyword, PagingInfo pagingInfo, bool needFinish = false)
        {
            var query = Query<WorkOrder>();
            if (needFinish)
            {
                query.Where(p => p.State == WorkOrderState.Producing || p.State == WorkOrderState.Release || p.State == WorkOrderState.Finish);
            }
            else
            {
                query.Where(p => p.State == WorkOrderState.Producing || p.State == WorkOrderState.Release);
            }
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.No.Contains(keyword));
            query.Exists<WorkOrderBom>((x, y) => y.Where(f => f.WorkOrderId == x.Id));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取过滤的工单列表
        /// </summary>
        /// <param name="id">过滤的工单Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工单列表</returns>
        public virtual EntityList GetWorkOrderList(double id, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<WorkOrder>().Where(p => p.Id != id);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.No.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrderList(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<WorkOrder>();
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.No.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取过滤的工单列表
        /// </summary>
        /// <param name="noWoIds">过滤的工单Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<WorkOrder> GetWorkOrderList(List<double> noWoIds, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<WorkOrder>().Where(p => !noWoIds.Contains(p.Id));
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.No.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单中的产品
        /// </summary>
        /// <param name="workorderNo">工单号</param>
        /// <returns>产品列表</returns>
        public virtual List<double> GetItemIdsFromWO(string workorderNo)
        {
            var nos = workorderNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] nolist = new string[nos.Length];
            for (int i = 0; i < nos.Length; i++)
            {
                nolist[i] = string.Format("{0}{1}{0}", "'", nos[i]);
            }

            var wo = DB.Query<WorkOrder>("w");
            var entityMeta = RF.Find<WorkOrder>().EntityMeta;
            var workOrderList = wo.Where(p => p.SQL<bool>(new Data.FormattedSql(@"w.{0} in ({1})".FormatArgs(entityMeta.Property(WorkOrder.NoProperty).ColumnMeta.ColumnName, string.Join(",", nolist))))).ToList();
            var productIds = workOrderList.Select(p => p.ProductId);
            List<double> itemIds = new List<double>();
            itemIds.AddRange(productIds);
            return itemIds;
        }

        /// <summary>
        /// 依据改单编号获取工单
        /// </summary>
        /// <param name="no">工单编号</param>
        /// <returns>工单</returns>
        [ApiService("获取工单")]
        [return: ApiReturn("返回工单数据:WorkOrder")]
        public virtual WorkOrder GetWorkOrder(string no)
        {
            return Query<WorkOrder>().Where(p => p.No == no).FirstOrDefault();
        }

        /// <summary>
        /// 根据工单号列表获取工单列表
        /// </summary>
        /// <param name="nos"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrders(IList<string> nos, EagerLoadOptions elo = null)
        {
            return nos.SplitContains(temp => { 
                return Query<WorkOrder>().Where(p => temp.Contains(p.No)).ToList(null, elo); 
            });
        }

        /// <summary>
        /// 根据工单号列表获取工单Id列表
        /// </summary>
        /// <param name="nos">工单号列表</param>
        /// <returns>工单Id列表</returns>
        public virtual IList<double> GetWorkOrderByNos(IList<string> nos)
        {
            return Query<WorkOrder>().Select(p => p.Id).Where(p => nos.Contains(p.No)).ToList<double>();
        }

        /// <summary>
        /// 获取生成批次
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns>生产批次实体</returns>
        public virtual WoWipBatch GetWipBatch(double id)
        {
            return Query<WoWipBatch>().Where(p => p.WorkOrderId == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工单Bom
        /// </summary>
        /// <param name="woIdList">工单Id列表</param>
        /// <returns>工单Bom数据</returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBoms(List<double> woIdList)
        {
            var exp = woIdList.CreateContainsExpression<WorkOrderBom>("x", "WorkOrderId");
            if (exp == null)
                return new EntityList<WorkOrderBom>();
            return Query<WorkOrderBom>().Where(exp).ToList();
        }

        /// <summary>
        /// 获取工单bom信息
        /// </summary>
        /// <param name="no">工单号</param>
        /// <returns>工单bom信息列表</returns>
        public virtual IList<WorkOrderBomInfo> GetWorkOrderBomInfos(string no)
        {
            return Query<WorkOrderBom>()
                .Join<WorkOrder>((b, w) => b.WorkOrderId == w.Id && w.No == no)
                .Select(p => new WorkOrderBomInfo()
                {
                    ItemId = p.ItemId,
                    RequireQty = p.RequireQty
                }).ToList<WorkOrderBomInfo>();
        }

        /// <summary>
        /// 根据工单获取工单Bom物料
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>工单BOM物料</returns>
        public virtual EntityList<Items.Item> GetBomItems(double woId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Items.Item>().Join<WorkOrderBom>((x, y) => x.Id == y.ItemId && y.WorkOrderId == woId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单ID获取工单Bom
        /// </summary>
        /// <param name="woIds">工单Id列表</param>
        /// <param name="itemIds">物料Id列表</param>
        /// <param name="elo"></param>
        /// <returns>工单Bom数据</returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBomsByWoIds(List<double> woIds, List<double> itemIds, EagerLoadOptions elo = null)
        {
            return Query<WorkOrderBom>().Where(p => woIds.Contains(p.WorkOrderId) && itemIds.Contains(p.ItemId)).ToList(null, elo);
        }

        /// <summary>
        /// 根据工单号获取工单Bom
        /// </summary>
        /// <param name="woNos">工单号列表</param>
        /// <param name="itemIds">物料Id列表</param>
        /// <param name="elo"></param>
        /// <returns>工单Bom数据</returns>
        public virtual EntityList<WorkOrderBom> GetWorkOrderBomsByWoNos(List<string> woNos, List<double> itemIds, EagerLoadOptions elo = null)
        {
            return Query<WorkOrderBom>()
                .Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id && woNos.Contains(y.No))
                .Where(p => itemIds.Contains(p.ItemId))
                .ToList(null, elo);
        }
    }
}