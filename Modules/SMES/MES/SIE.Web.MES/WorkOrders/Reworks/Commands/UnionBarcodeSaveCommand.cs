using SIE.Common;
using SIE.Domain;
using SIE.EventMessages.MES.Barcodes;
using SIE.MES.WIP.Reworks;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;
using SIE.Web.Command;
using SIE.Web.MES.WorkOrders.Reworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 条码关联保存方法
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.Reworks.UnionBarcodeSaveCommand")]
    public class UnionBarcodeSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行保存
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var reworkEntity = args.Data.ToJsonObject<ReworkEntity>();
            var barcodeList = reworkEntity.BarcodeList.Where(b => b.CodeState == CodeState.NotAssociated).AsEntityList();
            
            barcodeList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
            });
            var keyItemList = GetKeyItemList(reworkEntity, reworkEntity.BarcodeList);
            RT.Service.Resolve<ReworkController>().UnionBarcode(reworkEntity.WorkOrderId, keyItemList, barcodeList);
            return true;
        }

        /// <summary>
        /// 根据关联的条码获取关键件列表
        /// </summary>
        /// <param name="reworkEntity">条码关联实体</param>
        /// <param name="barcodeList">关联条码列表</param>
        /// <returns>条码获取关键件列表</returns>
        private EntityList<KeyItemUnboundConfig> GetKeyItemList(ReworkEntity reworkEntity, List<UnionBarcode> barcodeList)
        {
            var keyItemList = new EntityList<KeyItemUnboundConfig>();
            var oldKeyItems = RT.Service.Resolve<ReworkController>().GetTaskKeyItemUnboundConfigs(reworkEntity.WorkOrderId);

            foreach (var item in reworkEntity.KeyItemList.Where(p => p.IsUnbound))
            {
                if (!oldKeyItems.Any(p => p.ItemId == item.ItemId && p.OldWorkOrderId == item.OldWorkOrderId))
                {
                    var keyItem = new KeyItemUnboundConfig
                    {
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ItemId = item.ItemId,
                        IsUnbound = true,
                        WorkOrderId = item.WorkOrderId,
                        UnitId = item.UnitId,
                        UnitName = item.UnitName,
                        SingleQty = item.SingleQty,
                        OldWorkOrderId = item.OldWorkOrderId
                    };
                    keyItem.GenerateId();
                    keyItemList.Add(keyItem);
                }
            }

            return keyItemList;
        }
    }
}
