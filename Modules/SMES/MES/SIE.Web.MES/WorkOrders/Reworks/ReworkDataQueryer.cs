using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.WIP.Reworks;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;
using SIE.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码数据处理器
    /// </summary>
    public class ReworkDataQueryer : DataQueryer
    {
        /// <summary>
        /// 逻辑处理
        /// </summary>
        readonly WoUnionBarcode ctl = new WoUnionBarcode();

        /// <summary>
        /// 获取已关联数量
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>已关联数量</returns>
        public WorkOrderUnionBarcode GetUnionBarcodeCount(double workOrderId)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId);
            if (wo == null) return null;
            WorkOrderUnionBarcode union = new WorkOrderUnionBarcode(wo);
            union.RelevancyQty = RT.Service.Resolve<ReworkController>().GetUnionBarcodeCount(workOrderId);
            return union;
        }

        /// <summary>
        /// 选择条码处理
        /// </summary>
        /// <param name="woBarcode">当前实体</param>
        /// <param name="UnionBarcode">关联条码列表</param>
        /// <param name="KeyItemUnboundConfig">关键件配置列表</param>
        /// <param name="barcodes">条码</param>
        /// <returns>结果实体</returns>
        public ReworkEntity GetUnionBarcodes(WorkOrderUnionBarcode woBarcode, List<UnionBarcode> UnionBarcode, List<KeyItemUnboundConfig> KeyItemUnboundConfig, string[] barcodes)
        {
            woBarcode.BarcodeList.AddRange(UnionBarcode);
            woBarcode.KeyItemList.AddRange(KeyItemUnboundConfig);
            var str = ctl.UnionBarcodes(barcodes, woBarcode);
            return ResReworkEntity(woBarcode, str.ToString());
        }

        /// <summary>
        /// 扫描条码处理
        /// </summary>
        /// <param name="woBarcode">当前实体</param>
        /// <param name="UnionBarcode">关联条码列表</param>
        /// <param name="KeyItemUnboundConfig">关键件配置列表</param>
        /// <param name="barcode">条码</param>
        /// <returns>结果实体</returns>
        public ReworkEntity GetUnionBarcode(WorkOrderUnionBarcode woBarcode, List<UnionBarcode> UnionBarcode, 
            List<KeyItemUnboundConfig> KeyItemUnboundConfig, string barcode)
        {
            //条码
            var barcodes = RT.Service.Resolve<BarcodeController>().GetBarcodesBySns(new List<string> { barcode });

            //已关联
            var unionBarcodes = RT.Service.Resolve<ReworkController>().GetUnionBarcodesBySnList(new List<string> { barcode });

            var workOrderIds = barcodes.Where(x => x.WorkOrderId != null)
                .Select(x => x.WorkOrderId.Value)
                .Distinct()
                .ToList();

            //工序BOM列表
            var workOrderProcessBoms = RT.Service.Resolve<WorkOrderController>().GetWoProcessBomList(workOrderIds);

            woBarcode.BarcodeList.AddRange(UnionBarcode);

            woBarcode.KeyItemList.AddRange(KeyItemUnboundConfig);
            ctl.AddUnionBarcode(barcode, woBarcode, barcodes, unionBarcodes, workOrderProcessBoms);
            return ResReworkEntity(woBarcode);
        }

        /// <summary>
        /// 移除条码处理
        /// </summary>
        /// <param name="woBarcode">当前实体</param>
        /// <param name="UnionBarcode">关联条码列表</param>
        /// <param name="KeyItemUnboundConfig">关键件配置列表</param>
        /// <param name="RemoveUnionBarcodes">移除条码列表</param>
        /// <returns>结果实体</returns>
        public ReworkEntity RemoveUnionBarcodes(WorkOrderUnionBarcode woBarcode, List<UnionBarcode> UnionBarcode, List<KeyItemUnboundConfig> KeyItemUnboundConfig, string[] RemoveUnionBarcodes)
        {
            //仅取已关联条码或不在本次移除的未关联条码
            var effectBarcode = UnionBarcode.Where(p => p.CodeState == CodeState.Associated || !RemoveUnionBarcodes.Contains(p.OriginalBarcode)).ToList();
            woBarcode.BarcodeList.AddRange(effectBarcode);
            woBarcode.KeyItemList.AddRange(KeyItemUnboundConfig);
            ctl.UpdateKeyItemConfigs(woBarcode);
            return ResReworkEntity(woBarcode);
        }

        /// <summary>
        /// 重构返回数据实体
        /// </summary>
        /// <param name="woBarcode">返工单实体</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据实体</returns>
        private ReworkEntity ResReworkEntity(WorkOrderUnionBarcode woBarcode, string error = null)
        {
            ReworkEntity rework = new ReworkEntity();
            rework.ScanQty = woBarcode.ScanQty;
            rework.BarcodeList = new List<UnionBarcode>();
            rework.KeyItemList = new List<KeyItemUnboundConfig>();
            rework.BarcodeList.AddRange(woBarcode.BarcodeList);
            rework.KeyItemList.AddRange(woBarcode.KeyItemList);
            rework.Error = error;
            return rework;
        }

        /// <summary>
        /// 初始化获取数据
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="woNo">工单编号</param>
        /// <returns>重构返回数据实体</returns>
        public ReworkEntity GetUnionBarcodeData(double woId, string woNo)
        {
            ReworkEntity rst = new ReworkEntity();
            var barcodes = RT.Service.Resolve<ReworkController>().GetUnionBarcodes(woNo);
            rst.BarcodeList = new List<UnionBarcode>();
            rst.BarcodeList.AddRange(barcodes);
            var keys = RT.Service.Resolve<ReworkController>().GetTaskKeyItemUnboundConfigs(woId);
            keys.ForEach(p =>
            {
                p.ItemCode = p.Item.Code;
                p.ItemName = p.Item.Name;
                p.UnitName = p.Unit?.Name;
            });
            rst.KeyItemList = new List<KeyItemUnboundConfig>();
            rst.KeyItemList.AddRange(keys);
            return rst;
        }
    }

    /// <summary>
    /// 返回数据实体数据存储
    /// </summary>
    public class ReworkEntity
    {
        /// <summary>
        /// 当前扫描数
        /// </summary>
        public int ScanQty
        {
            get; set;
        }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get; set;
        }

        /// <summary>
        /// 关联条码列表
        /// </summary>
        public List<UnionBarcode> BarcodeList
        {
            get; set;
        }

        /// <summary>
        /// 关键件解绑配置列表
        /// </summary>
        public List<KeyItemUnboundConfig> KeyItemList
        {
            get; set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get; set;
        }
    }
}