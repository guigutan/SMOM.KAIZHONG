using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 上料采集单体条码
    /// </summary>
    [ChildEntity, Serializable]
    [Label("上料采集单体条码")]
    public partial class WipProductProcessSinglelabel : DataEntity
    {
        #region 单体条码 SingleLabel
        /// <summary>
        /// 单体条码
        /// </summary>
        [Label("单体条码")]
        public static readonly Property<string> SingleLabelProperty = P<WipProductProcessSinglelabel>.Register(e => e.SingleLabel);

        /// <summary>
        /// 单体条码
        /// </summary>
        public string SingleLabel
        {
            get { return GetProperty(SingleLabelProperty); }
            set { SetProperty(SingleLabelProperty, value); }
        }
        #endregion

        #region 系统外条码 IsExternal
        /// <summary>
        /// 系统外条码
        /// </summary>
        [Label("系统外条码")]
        public static readonly Property<bool> IsExternalProperty = P<WipProductProcessSinglelabel>.Register(e => e.IsExternal);

        /// <summary>
        /// 系统外条码
        /// </summary>
        public bool IsExternal
        {
            get { return GetProperty(IsExternalProperty); }
            set { SetProperty(IsExternalProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<WipProductProcessSinglelabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<WipProductProcessSinglelabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 生产采集记录 WipProductProcess
        /// <summary>
        /// 生产采集记录Id
        /// </summary>
        public static readonly IRefIdProperty WipProductProcessIdProperty = P<WipProductProcessSinglelabel>.RegisterRefId(e => e.WipProductProcessId, ReferenceType.Parent);

        /// <summary>
        /// 生产采集记录Id
        /// </summary>
        public double WipProductProcessId
        {
            get { return (double)GetRefId(WipProductProcessIdProperty); }
            set { SetRefId(WipProductProcessIdProperty, value); }
        }

        /// <summary>
        /// 生产采集记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductProcess> WipProductProcessProperty = P<WipProductProcessSinglelabel>.RegisterRef(e => e.WipProductProcess, WipProductProcessIdProperty);

        /// <summary>
        /// 生产采集记录
        /// </summary>
        public WipProductProcess WipProductProcess
        {
            get { return GetRefEntity(WipProductProcessProperty); }
            set { SetRefEntity(WipProductProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 上料采集单体条码 实体配置
    /// </summary>
    internal class WipProductProcessSinglelabelConfig : EntityConfig<WipProductProcessSinglelabel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_SINGLE_LABEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
