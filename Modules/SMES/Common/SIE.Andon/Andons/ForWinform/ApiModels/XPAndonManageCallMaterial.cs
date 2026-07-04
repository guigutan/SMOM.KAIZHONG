using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XPAndonManageCallMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 安灯管理Id
        /// </summary>
        public double AndonManageId { get; set; }
    
        /// <summary>
        /// 安灯管理
        /// </summary>
        //public AndonManage AndonManage { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        //public Item Item { get; set; }

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode? ConsumeType { get; set; }

        /// <summary>
        /// 本次备料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime TimeNeed { get; set; }

        /// <summary>
        /// 备料接收仓库Id
        /// </summary>
        public double? WareHouseId { get; set; }

        /// <summary>
        /// 备料接收仓库
        /// </summary>
        //public Warehouse WareHouse { get; set; }

        /// <summary>
        /// 备料接收库位Id
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 备料接收库位
        /// </summary>
        //public StorageLocation StorageLocation { get; set; }

        /// 备料单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 备料单行号
        /// </summary>
        public string LineNo { get; set; }

        #region 视图属性
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料消耗类型(视图属性)
        /// </summary>
        public ConsumeMode? ConsumeModeView { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double WipId { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// 手动填写
        /// </summary>
        public bool Hand { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string UpdateByName { get; set; }

        #endregion

        public static XPAndonManageCallMaterial Gen(AndonManageCallMaterial cm)
        {
            return new XPAndonManageCallMaterial()
            {
                Id = cm.Id,
                AndonManageId = cm.AndonManageId,
                //AndonManage = cm.AndonManage,
                ItemId = cm.ItemId,
                //Item = cm.Item,
                ConsumeType = cm.ConsumeType,
                Qty = cm.Qty,
                TimeNeed = cm.TimeNeed,
                WareHouseId = cm.WareHouseId,
                //WareHouse = cm.WareHouse,
                StorageLocationId = cm.StorageLocationId,
                //StorageLocation = cm.StorageLocation,
                No = cm.No,
                LineNo = cm.LineNo,
                ItemCode = cm.ItemCode,
                ItemName = cm.ItemName,
                ConsumeModeView = cm.ConsumeModeView,
                FactoryId = cm.FactoryId,
                WipId = cm.WipId,
                WorkShopId = cm.WorkShopId,
                WorkOrderId = cm.WorkOrderId,
                ProcessId = cm.ProcessId,
                WareHouseName = cm.WareHouseName,
                LocationName = cm.LocationName,
                Hand = cm.Hand,
                UpdateByName = cm.UpdateByName,
            };
        }
        public AndonManageCallMaterial ToAndonManageCallMaterial()
        {
            return new AndonManageCallMaterial()
            {
                Id = this.Id,
                AndonManageId = this.AndonManageId,
                //AndonManage = this.AndonManage,
                ItemId = this.ItemId,
                //Item = this.Item,
                ConsumeType = this.ConsumeType,
                Qty = this.Qty,
                TimeNeed = this.TimeNeed,
                WareHouseId = this.WareHouseId,
                //WareHouse = this.WareHouse,
                StorageLocationId = this.StorageLocationId,
                //StorageLocation = this.StorageLocation,
                No = this.No,
                LineNo = this.LineNo,
                //ItemCode = this.ItemCode,
                //ItemName = this.ItemName,
                //ConsumeModeView = this.ConsumeModeView,
                FactoryId = this.FactoryId,
                WipId = this.WipId,
                WorkShopId = this.WorkShopId,
                WorkOrderId = this.WorkOrderId,
                ProcessId = this.ProcessId,
                WareHouseName = this.WareHouseName,
                LocationName = this.LocationName,
                Hand = this.Hand,
                //UpdateByName = this.UpdateByName,
            };
        }
    }
}
