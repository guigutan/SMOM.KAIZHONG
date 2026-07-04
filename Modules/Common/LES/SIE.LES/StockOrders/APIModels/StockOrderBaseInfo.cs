using System;
using System.Collections.Generic;

namespace SIE.LES.StockOrders.APIModels
{
    /// <summary>
    /// 工厂、车间、资源基本信息
    /// </summary>
    [Serializable]
    public class BaseInfo
    {
        /// <summary>
        ///构造函数
        /// </summary>
        public BaseInfo()
        {
            ItemExtPropDataInfos = new List<ItemExtPropDataInfo>();
        }
        /// <summary>
        /// 工厂、车间、资源、仓库、库位Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工厂、车间、资源、仓库、库位编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工厂、车间、资源、仓库、库位名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 资源的车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 资源的车间编码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 资源是车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType StockType { get; set; }

        /// <summary>
        /// 数量（拉式物料默认数量）
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 是否启用扩展属性
        /// </summary>
        public bool EnableItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性列表
        /// </summary>
        public List<ItemExtPropDataInfo> ItemExtPropDataInfos { get; set; }


    }

    /// <summary>
    /// 资源下的线边仓仓库库位
    /// </summary>
    public class WareAndStorage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public WareAndStorage()
        {
            WareList = new List<BaseInfo>();
            StorageList = new List<BaseInfo>();
        }

        /// <summary>
        /// 仓库列表
        /// </summary>
        public List<BaseInfo> WareList { get; set;}

        /// <summary>
        /// 库位列表
        /// </summary>
        public List<BaseInfo> StorageList { get; set; }
    }

    /// <summary>
    /// 物料扩展属性数据
    /// </summary>
    public class ItemExtPropDataInfo
    {
        /// <summary>
        /// 物料扩展属性ID
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string DefinitionName { get; set; }

        /// <summary>
        /// 物料扩展属性值
        /// </summary>
        public string ItemExtPropValue { get; set; }
    }
    /// <summary>
    /// 创建备料单信息
    /// </summary>
    public class StockBaseInfo
    {
        /// <summary>
        /// 是否是新用户
        /// </summary>
        public bool isNew { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageId { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageName { get; set; }

        /// <summary>
        /// 产线线边仓仓库列表
        /// </summary>
        public List<BaseInfo> LineWarehouseList { get; set; }

        /// <summary>
        /// 产线线边仓库位列表
        /// </summary>
        public List<BaseInfo> LineStorageList { get; set; }

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType StockType { get; set; }

        /// <summary>
        /// 接收方式
        /// </summary>
        public StockReceiveType ReceiveType { get; set; }
    }
}
