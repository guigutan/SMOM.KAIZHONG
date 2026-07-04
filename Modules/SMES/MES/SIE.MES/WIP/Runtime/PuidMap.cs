using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.MetaModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WIP.Runtime
{
    /// <summary>
    /// 采集运行时Puid与条码关联关系
    /// </summary>
    [RootEntity, Serializable]
    public class PuidMap : StringEntity
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public static readonly Property<string> PuidProperty = P<PuidMap>.Register(e => e.Puid);

        /// <summary>
        /// 产品ID
        /// </summary>
        public string Puid
        {
            get { return this.GetProperty(PuidProperty); }
            set { this.SetProperty(PuidProperty, value); }
        }

        /// <summary>
        /// 生产条码
        /// </summary>
        public static readonly Property<string> BarcodeProperty = P<PuidMap>.Register(e => e.Barcode);

        /// <summary>
        /// 生产条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }

        /// <summary>
        /// 条码类型
        /// </summary>
        public static readonly Property<BarcodeType> BarcodeTypeProperty = P<PuidMap>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get { return this.GetProperty(BarcodeTypeProperty); }
            set { this.SetProperty(BarcodeTypeProperty, value); }
        }
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class PuidMappingtInfoConfig : EntityConfig<PuidMap>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_RT_PUID_MAP").MapAllProperties();
            Meta.Property(PuidMap.PuidProperty).ColumnMeta.HasIndex();
            Meta.Property(PuidMap.BarcodeProperty).ColumnMeta.HasIndex();
        }
    }
}
