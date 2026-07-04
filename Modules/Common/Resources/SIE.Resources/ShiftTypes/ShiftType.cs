using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.ShiftTypes
{
    /// <summary>
    /// 班制
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("班制")]
    [DisplayMember(nameof(ShiftType.Name))]
    public partial class ShiftType : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("班制编码")]
        public static readonly Property<string> CodeProperty = P<ShiftType>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("班制名称")]
        public static readonly Property<string> NameProperty = P<ShiftType>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 是否缺省 IsDefault
        /// <summary>
        /// 是否缺省
        /// </summary>
        [Label("是否缺省")]
        public static readonly Property<YesNo> IsDefaultProperty = P<ShiftType>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否缺省
        /// </summary>
        public YesNo IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 是否休息日 IsWeekend
        /// <summary>
        /// 是否休息日
        /// </summary>
        [Label("是否休息日")]
        public static readonly Property<YesNo> IsWeekendProperty = P<ShiftType>.Register(e => e.IsWeekend);

        /// <summary>
        /// 是否休息日
        /// </summary>
        public YesNo IsWeekend
        {
            get { return GetProperty(IsWeekendProperty); }
            set { SetProperty(IsWeekendProperty, value); }
        }
        #endregion

        #region 班次列表 ShiftList
        /// <summary>
        /// 班次列表
        /// </summary>
        public static readonly ListProperty<EntityList<Shift>> ShiftListProperty = P<ShiftType>.RegisterList(e => e.ShiftList);

        /// <summary>
        /// 班次列表
        /// </summary>
        public EntityList<Shift> ShiftList
        {
            get { return this.GetLazyList(ShiftListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 班制 实体配置
    /// </summary>
    internal class ShiftTypeConfig : EntityConfig<ShiftType>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CAL_SHIFT_TYPE").MapAllProperties();
            Meta.Property(ShiftType.CodeProperty).ColumnMeta.HasLength(120);
            Meta.Property(ShiftType.NameProperty).ColumnMeta.HasLength(120);
            Meta.EnablePhantoms();
        }
    }
}