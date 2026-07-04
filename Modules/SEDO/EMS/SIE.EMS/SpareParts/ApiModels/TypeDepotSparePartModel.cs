using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件查询信息
    /// </summary>
    [Serializable]
    public class TypeDepotQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 备件类型id
        /// </summary>
        public double? TypeId { get; set; }

        /// <summary>
        /// 备件仓库id
        /// </summary>
        public double? DepotId { get; set; }
    }

    /// <summary>
    /// 备件数据
    /// </summary>
    [Serializable]
    public class TypeDepotSparePartData
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 备件列表
        /// </summary>
        public List<TypeDepotSparePartInfo> Data { get; set; } = new List<TypeDepotSparePartInfo>();
    }

    /// <summary>
    /// 备件信息
    /// </summary>
    [Serializable]
    public class TypeDepotSparePartInfo
    {
        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SparePartSpecification { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 库存数
        /// </summary>
        public int StoreQty { get; set; }

        /// <summary>
        /// Base64图片
        /// </summary>
        public string PhotoBase64 { get; set; }

        /// <summary>
        /// 是否低于安全库存
        /// </summary>
        public bool IsLowerThan { get; set; }
    }

    /// <summary>
    /// 备件详细信息
    /// </summary>
    [Serializable]
    public class SparePartDetailInfo
    {
        /// <summary>
        /// 备件ID
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SparePartSpecification { get; set; }

        /// <summary>
        /// 类型编码
        /// </summary> 
        public string SparePartTypeCode { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 备件类别
        /// </summary>
        public string PartType { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipType { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModel { get; set; }

        /// <summary>
        /// 原厂料号
        /// </summary>
        public string OriginalItemCode { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 安全库存
        /// </summary>
        public int? SafeStock { get; set; }

        /// <summary>
        /// 库存数
        /// </summary>
        public int StoreQty { get; set; }

        /// <summary>
        /// 是否低于安全库存
        /// </summary>
        public bool IsLowerThan { get; set; }

        /// <summary>
        /// 更换周期
        /// </summary>
        public int? LifeTime { get; set; }

        /// <summary>
        /// 可用时间
        /// </summary>
        public int? UseTime { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 以旧换新
        /// </summary>
        public string IsReplacement { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 序列号管控
        /// </summary>
        public string IsSeqNoCharge { get; set; }

        /// <summary>
        /// Base64图片列表
        /// </summary>
        public List<string> PhotoBase64List { get; set; } = new List<string>();

        /// <summary>
        /// 备件库存列表
        /// </summary>
        public List<SparePartDetailDepotInfo> Depots { get; set; } = new List<SparePartDetailDepotInfo>();
    }

    /// <summary>
    /// 备件库存信息
    /// </summary>
    [Serializable]
    public class SparePartDetailDepotInfo
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string DepotCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string DepotName { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// 可用数
        /// </summary>
        public int? GoodNumber { get; set; }

        /// <summary>
        /// 不良数
        /// </summary>
        public int? RotNumber { get; set; }
    }
}
