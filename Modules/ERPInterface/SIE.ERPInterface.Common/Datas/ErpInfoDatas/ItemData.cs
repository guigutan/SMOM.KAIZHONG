using SIE.Items;
using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 物料数据
    /// </summary>
    [Serializable]
    public class ItemData : ErpInfoData
    {
        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel { get; set; }

        /// <summary>
        /// 状态(1可用 0不可用)
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo { get; set; }

        /// <summary>
        /// 图号版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 基准机型
        /// </summary>
        public string BaseModel { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string Person { get; set; }

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpPerson { get; set; }

        /// <summary>
        /// 上偏差
        /// </summary>
        public decimal UpperWeight { get; set; }

        /// <summary>
        /// 下偏差
        /// </summary>
        public decimal LowerWeight { get; set; }

        /// <summary>
        /// 最小包装数
        /// </summary>
        public decimal? MinPackingQty { get; set; }

        /// <summary>
        /// 英文描述
        /// </summary>
        public string EnglishDescription { get; set; }

        /// <summary>
        /// 物料简称
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public decimal? Length { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public decimal? Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        public decimal? Height { get; set; }
        /// <summary>
        /// 体积
        /// </summary>
        public decimal? Volume { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 采购员
        /// </summary>
        public string PurchasingAgent { get; set; }

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? Precision { get; set; }

        /// <summary>
        /// 商品条码
        /// </summary>
        public string GoodsBarcode { get; set; }

        /// <summary>
        /// 物料类型（约定0成品1原材料2半成品3其他）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode { get; set; }

        #region 库存资料
        /// <summary>
        /// 是否批次管理，是则取标准批次方案
        /// </summary>
        public bool IsBatch { get; set; }

        /// <summary>
        /// 序列号管理
        /// </summary>
        public bool IsSerialNumber { get; set; }       

        /// <summary>
        /// 保质期（天）
        /// </summary>
        public decimal? ShelfLife { get; set; }
        #endregion

        /// <summary>
        /// 保存后回写Id
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string ItemSourceType { get; set; }
    }
}
