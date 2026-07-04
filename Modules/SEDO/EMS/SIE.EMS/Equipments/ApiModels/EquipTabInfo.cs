using SIE.Core.Enums;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Equipments.ApiModels
{
    /// <summary>
    /// 设备信息
    /// </summary>
    [Serializable]
    public class EquipTabInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资产编码
        /// </summary>
        public string AssetCode { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 设备物联状态
        /// </summary>
        public string IOTState { get; set; }

        /// <summary>
        /// 设备物联状态值
        /// </summary>
        public EquipOnLineState? IOTStateValue { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public string AccountState { get; set; }

        /// <summary>
        /// 设备状态值
        /// </summary>
        public AccountState AccountStateValue {  get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
    }

    /// <summary>
    /// 设备查询信息
    /// </summary>
    [Serializable]
    public class EquipTabQueryInfo
    {
        /// <summary>
        /// 设备编码\名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 设备类型id
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public string WipResource { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState? State { get; set; }
    }

    /// <summary>
    /// 设备详细信息
    /// </summary>
    [Serializable]
    public class EquipTabDetailInfo : EquipTabInfo
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public double EquipId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModel { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipType { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 资产责任人
        /// </summary>
        public string ResPerson { get; set; }

        /// <summary>
        /// 故障次数
        /// </summary>
        public int FaultQty { get; set; }

        /// <summary>
        /// 维修次数
        /// </summary>
        public int RepairQty { get; set; }

        /// <summary>
        /// 停机次数
        /// </summary>
        public int ShutDownQty { get; set; }

        /// <summary>
        /// 报警次数
        /// </summary>
        public int WarningQty { get; set; }

        /// <summary>
        /// 点检记录显示
        /// </summary>
        public string CheckDisplay { get; set; }

        /// <summary>
        /// 保养记录显示
        /// </summary>
        public string MaintainDisplay { get; set; }

        /// <summary>
        /// 维修记录显示
        /// </summary>
        public string RepairDisplay { get; set; }

        /// <summary>
        /// 维修记录显示(20231201前端多语言优化)
        /// </summary>
        public int RepairDisplayValue {  get; set; }

        /// <summary>
        /// 报警记录显示
        /// </summary>
        public string WarningDisplay { get; set; }
    }

    /// <summary>
    /// 设备台账详细信息
    /// </summary>
    [Serializable]
    public class EquipAccountTabInfo
    {
        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode { get; set; }

        /// <summary>
        /// 类型编码
        /// </summary>
        public string EquipTypeCode { get; set; }

        /// <summary>
        /// 使用级别
        /// </summary>
        public string UseLevel { get; set; }

        /// <summary>
        /// 重点设备
        /// </summary>
        public string KeyEquip { get; set; }

        /// <summary>
        /// 虚拟设备
        /// </summary>
        public string IsVirtual { get; set; }

        /// <summary>
        /// 行业属性
        /// </summary>
        public string IndustryCategory { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public string UseState { get; set; }

        /// <summary>
        /// 资产来源
        /// </summary>
        public string Proprietorship { get; set; }

        /// <summary>
        /// 入厂日期
        /// </summary>
        public string EnterDate { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 租赁/客供单位
        /// </summary>
        public string PurchaseUnit { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 资产原值
        /// </summary>
        public string AssetOriginalValue { get; set; }

        /// <summary>
        /// 资产净值
        /// </summary>
        public string AssetNetValue { get; set; }

        /// <summary>
        /// 使用年限
        /// </summary>
        public decimal UsefulLife { get; set; }

        /// <summary>
        /// 保修期
        /// </summary>
        public string WarrantyPeriod { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string InstallationLocation { get; set; }

        /// <summary>
        /// 存储位置
        /// </summary>
        public string Location { get; set; }
    }

    /// <summary>
    /// 设备BOM信息
    /// </summary>
    [Serializable]
    public class EquipTabBomInfo
    {
        /// <summary>
        /// BOMid
        /// </summary>
        public double BomId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int SparePartQty { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int? StockQty { get; set; }

        /// <summary>
        /// 子bom信息
        /// </summary>
        public List<EquipTabBomInfo> Children { get; set; } = new List<EquipTabBomInfo>();
    }
}
