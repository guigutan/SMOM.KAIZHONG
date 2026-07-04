using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 周转箱型号
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("周转箱型号")]
    [DisplayMember(nameof(Code))]
    public partial class TurnoverBoxModel : SIE.Core.Boxs.TurnoverBoxModel
    {
        #region 长度(cm) Length
        /// <summary>
        /// 长度(cm)
        /// </summary>
        [Label("长度(cm)")]
        public static readonly Property<decimal?> LengthProperty = P<TurnoverBoxModel>.Register(e => e.Length);

        /// <summary>
        /// 长度(cm)
        /// </summary>
        public decimal? Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽(cm) Width
        /// <summary>
        /// 宽(cm)
        /// </summary>
        [Label("宽(cm)")]
        public static readonly Property<decimal?> WidthProperty = P<TurnoverBoxModel>.Register(e => e.Width);

        /// <summary>
        /// 宽(cm)
        /// </summary>
        public decimal? Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高(cm) Height
        /// <summary>
        /// 高(cm)
        /// </summary>
        [Label("高(cm)")]
        public static readonly Property<decimal?> HeightProperty = P<TurnoverBoxModel>.Register(e => e.Height);

        /// <summary>
        /// 高(cm)
        /// </summary>
        public decimal? Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 周转工具型号 实体配置
    /// </summary>
    internal class TurnoverBoxModelConfig : EntityConfig<TurnoverBoxModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CNT_TURNOVER_BOX_MODEL").MapAllProperties();
            Meta.Property(TurnoverBoxModel.CodeProperty).ColumnMeta.HasLength(60);
            Meta.Property(TurnoverBoxModel.NameProperty).ColumnMeta.HasLength(160);
            Meta.EnablePhantoms();
        }
    }
}