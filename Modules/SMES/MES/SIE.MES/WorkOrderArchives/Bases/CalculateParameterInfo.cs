using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.LES.LinesideWarehouses.Models;
using SIE.MES.LoadItems;
using SIE.MES.LoadItems.Models;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Packages.ItemLabels.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrderArchives.Bases
{
    /// <summary>
    /// 参数信息类
    /// </summary>
    public class CalculateParameterInfo
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="workOrderArchiveId">工单Id</param>
        /// <param name="pagingInfo">分页信息</param>

        public CalculateParameterInfo(double workOrderArchiveId, PagingInfo pagingInfo)
        {
            WorkOrderArchiveId = workOrderArchiveId;
            Init(pagingInfo);
        }

        #region 属性

        /// <summary>
        /// 工单制造档案Id
        /// </summary>
        public double WorkOrderArchiveId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// 拉式物料Ids
        /// </summary>
        public List<double> BomPullItemIds { get; set; }

        /// <summary>
        /// 推式物料Ids
        /// </summary>
        public List<double> BomPushItemIds { get; set; }

        /// <summary>
        /// 物料标签
        /// </summary>
        public List<ItemLabelBaseData> ItemLabelList { get; set; }

        /// <summary>
        /// 上料记录
        /// </summary>
        public List<LoadItemBaseData> LoadItemList { get; set; }

        /// <summary>
        /// 工单bom
        /// </summary>
        public EntityList<WorkOrderBom> BomList { get; set; }

        /// <summary>
        /// 线边仓
        /// </summary>
        public LinesideWareBaseData LineWare { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init(PagingInfo pagingInfo)
        {
            // 工单
            WorkOrder = RF.GetById<WorkOrder>(WorkOrderArchiveId) ?? throw new ValidationException("工单不存在！".L10N());

            if (WorkOrder.ResourceId == 0 || WorkOrder.ResourceId == null)
            {
                throw new ValidationException("工单未维护产线！".L10N());
            }
            // 工单产线线边仓
            LineWare = RT.Service.Resolve<LinesideWarehouseController>().GetBaseLinesideWarehouse(WorkOrder.ResourceId.Value) ?? throw new ValidationException("工单资源未维护对应的线边仓！".L10N());


            // 工单bom
            BomList = RT.Service.Resolve<WorkOrderBomController>().GetWorkOrderBomByOrderId(pagingInfo, WorkOrderArchiveId);
            if (!BomList.Any())
            {
                throw new ValidationException("工单未维护工单BOM！".L10N());
            }
            BomPullItemIds = new List<double>();
            BomPushItemIds = new List<double>();
            // 工单bom下的拉式物料
            BomPullItemIds = BomList.Where(p => p.ItemConsumeMode == ConsumeMode.Pull).Select(p => p.ItemId).Distinct().ToList();
            // 工单bom下的推式物料
            BomPushItemIds = BomList.Where(p => p.ItemConsumeMode == ConsumeMode.Push).Select(p => p.ItemId).Distinct().ToList();

            var itemIds = new List<double>(BomPullItemIds);
            itemIds.AddRange(BomPushItemIds);
            // 物料标签表
            ItemLabelList = RT.Service.Resolve<ItemLabelController>().GetItemLabelBaseDatas(itemIds);

            // 上料记录表
            LoadItemList = RT.Service.Resolve<LoadItemController>().GetLoadItemBaseDatas(itemIds);
        }
        #endregion
    }
}
