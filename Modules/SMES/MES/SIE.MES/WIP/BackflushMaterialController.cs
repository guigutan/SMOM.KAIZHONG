using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LoadItems;
using SIE.MES.LoadItems.Enum;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 非工序BOM物料倒扣
    /// </summary>
    public class BackflushMaterialController : DomainController
    {
        /// <summary>
        /// 产品完工,倒扣非工序BOM物料 (产品完工下线时进行工单BOM反冲物料倒扣)
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <param name="resourceId">生产资源</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>         
        /// <param name="product">生产采集运行时产品</param>
        public virtual void BackflushMaterialByFinsh(string barcode, double? resourceId, double? processId, double? stationId, product product)
        {
            var executor = new BackflushMaterialExecutor();
            var woCostItems = executor.CreateDeductItems(barcode, resourceId, processId, stationId, product, true);
            if (woCostItems.Count > 0)
                ExecuteBackflushMaterialAsync(woCostItems);
        }

        /// <summary>
        /// 工序完工,倒扣非工序BOM物料 (每个工序采集过站完成后进行工序BOM反冲物料倒扣)
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="product"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void BackflushMaterialByProcess(string barcode, double? resourceId, double? processId, double? stationId, product product)
        {
            var executor = new BackflushMaterialExecutor();
            var woCostItems = executor.CreateDeductItems(barcode, resourceId, processId, stationId, product, false);
            if (woCostItems.Count > 0)
                ExecuteBackflushMaterialAsync(woCostItems);
        }

        /// <summary>
        /// 创建倒扣料工单耗用单
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="factoryId"></param>
        /// <param name="workOrderId"></param>
        /// <param name="productQty"></param>
        /// <param name="workOrderBoms"></param>
        /// <param name="retrospectType"></param>
        /// <param name="submitTime"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual EntityList<WoCostItem> CreateDeductItems(string barcode, double? resourceId,
            double? processId, double? stationId, double factoryId, double workOrderId, decimal productQty,
            IList<WorkOrderBom> workOrderBoms, RetrospectType retrospectType, DateTime? submitTime = null)
        {
            var executor = new BackflushMaterialExecutor();
            return executor.CreateDeductItems(barcode, resourceId, processId, stationId, factoryId, workOrderId, productQty, workOrderBoms, retrospectType, submitTime);
        }

        /// <summary>
        /// 倒扣料记录补扣
        /// </summary>
        /// <param name="deductItemIds">倒扣料记录ID</param>
        /// <exception cref="ValidationException"></exception>
        public virtual WoCostItemDeductResult ReBackflushMaterial(List<double> deductItemIds)
        {
            // 多选
            var deductItemList = deductItemIds.SplitContains(tempIds =>
            {
                return Query<WoCostItem>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            if (!deductItemList.Any())
            {
                throw new ValidationException("物料倒扣记录找不到".L10N());

            }
            if (deductItemList.Any(p => p.State != LoadItems.Enum.WoCostItemState.FailSubmit && p.State != LoadItems.Enum.WoCostItemState.ToSubmit))
            {
                throw new ValidationException("扣料状态是【提交失败】或【待提交】的才允许点击".L10N());
            }

            using (DataAuth.DataAuths.LoadAll())
            {
                return ExecuteBackflushMaterial(deductItemList);
            }
        }

        /// <summary>
        /// 执行扣料逻辑 (异步)
        /// </summary>
        /// <param name="deductItems">倒扣料记录</param>
        /// <param name="milliseconds">延时执行时间(默认3000毫秒)</param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ExecuteBackflushMaterialAsync(EntityList<WoCostItem> deductItems, int milliseconds = 3000, double? dispatchTaskId = null)
        {
            var invOrg = RT.InvOrg;
            AsyncHelper.InvokeSafe(async () =>
            {
                await Task.Delay(milliseconds);
                var executor = new BackflushMaterialExecutor(deductItems);
                if (dispatchTaskId != null)
                    executor.InputDispatchTaskId(dispatchTaskId.Value);
                executor.ExecuteDeductItems();
            });
        }

        /// <summary>
        /// 执行扣料逻辑
        /// </summary>
        /// <param name="deductItems">倒扣料记录</param>
        /// <exception cref="ValidationException"></exception>
        public virtual WoCostItemDeductResult ExecuteBackflushMaterial(EntityList<WoCostItem> deductItems)
        {
            var executor = new BackflushMaterialExecutor(deductItems);
            return executor.ExecuteDeductItems();
        }

        /// <summary>
        /// 自动执行倒扣料 ([待提交]或者[失败],类型为[物料倒扣]的数据)
        /// 
        /// </summary>
        public virtual WoCostItemDeductResult AutoBackflushMaterial()
        {
            var deductItems = DB.Query<WoCostItem>()
                .Where(x => (x.State == WoCostItemState.FailSubmit || x.State == WoCostItemState.ToSubmit) && x.RecordType == WoCostItemType.DeductItem)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty()); ;

            using (DataAuth.DataAuths.LoadAll())
            {
                return ExecuteBackflushMaterial(deductItems);
            }
        }

    }
}
