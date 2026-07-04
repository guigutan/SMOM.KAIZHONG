using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 下料缺陷
    /// </summary>
    [ChildEntity, Serializable]
    [Label("下料缺陷")]
    public partial class UnloadItemDefect : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnloadItemDefect()
        {
            this.Qty = 0;
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<UnloadItemDefect>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 不良 Defect
        /// <summary>
        /// 不良Id
        /// </summary>
        [Required]
        [Label("不良Id")]
        public static readonly IRefIdProperty DefectIdProperty = P<UnloadItemDefect>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 不良Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 不良
        /// </summary>
        [Label("不良")]
        public static readonly RefEntityProperty<Defect> DefectProperty = P<UnloadItemDefect>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 不良
        /// </summary>
        public Defect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 不良列表 UnloadItem
        /// <summary>
        /// 不良列表Id
        /// </summary>
        [Label("不良列表Id")]
        public static readonly IRefIdProperty UnloadItemIdProperty = P<UnloadItemDefect>.RegisterRefId(e => e.UnloadItemId, ReferenceType.Parent);

        /// <summary>
        /// 不良列表Id
        /// </summary>
        public double UnloadItemId
        {
            get { return (double)GetRefId(UnloadItemIdProperty); }
            set { SetRefId(UnloadItemIdProperty, value); }
        }

        /// <summary>
        /// 不良列表
        /// </summary>
        [Label("不良列表")]
        public static readonly RefEntityProperty<UnloadItem> UnloadItemProperty = P<UnloadItemDefect>.RegisterRef(e => e.UnloadItem, UnloadItemIdProperty);

        /// <summary>
        /// 不良列表
        /// </summary>
        public UnloadItem UnloadItem
        {
            get { return GetRefEntity(UnloadItemProperty); }
            set { SetRefEntity(UnloadItemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 下料缺陷 实体配置
    /// </summary>
    internal class UnloadItemDefectConfig : EntityConfig<UnloadItemDefect>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_UNLOAD_ITEM_DEF").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}