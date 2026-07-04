using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.CalendarSchemes
{
    /// <summary>
	/// 日历方案
	/// </summary>
	[RootEntity, Serializable]
    [CriteriaQuery]
    [Label("日历方案")]
    [DisplayMember(nameof(Name))]
    public partial class CalendarScheme : DataEntity
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public CalendarScheme()
        {
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [Required, NotDuplicate]
        [MaxLength(40)]
        public static readonly Property<string> NameProperty = P<CalendarScheme>.Register(e => e.Name);

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
        public static readonly Property<YesNo> IsDefaultProperty = P<CalendarScheme>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否缺省
        /// </summary>
        public YesNo IsDefault
        {
            get { return GetProperty(IsDefaultProperty); }
            set { SetProperty(IsDefaultProperty, value); }
        }
        #endregion

        #region 是否可用 IsEnable
        /// <summary>
        /// 是否可用
        /// </summary>
        [Label("是否可用")]
        public static readonly Property<YesNo> IsEnableProperty = P<CalendarScheme>.Register(e => e.IsEnable);

        /// <summary>
        /// 是否可用
        /// </summary>
        public YesNo IsEnable
        {
            get { return GetProperty(IsEnableProperty); }
            set { SetProperty(IsEnableProperty, value); }
        }
        #endregion

        #region 日历方案例外 Excepts
        /// <summary>
        /// 日历方案例外
        /// </summary>
        [Label("日历方案例外")]
        public static readonly ListProperty<EntityList<CalendarSchemeExcept>> ExceptsProperty = P<CalendarScheme>.RegisterList(e => e.Excepts);

        /// <summary>
        /// 日历方案例外
        /// </summary>
        public EntityList<CalendarSchemeExcept> Excepts
        {
            get { return this.GetLazyList(ExceptsProperty); }
        }
        #endregion

        #region 周方案 SchemeWeeks
        /// <summary>
        /// 周方案
        /// </summary>
        [Label("周方案")]
        public static readonly ListProperty<EntityList<CalendarSchemeWeek>> SchemeWeeksProperty = P<CalendarScheme>.RegisterList(e => e.SchemeWeeks);

        /// <summary>
        /// 周方案
        /// </summary>
        public EntityList<CalendarSchemeWeek> SchemeWeeks
        {
            get { return this.GetLazyList(SchemeWeeksProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 日历方案 实体配置
    /// </summary>
    internal class CalendarSchemeConfig : EntityConfig<CalendarScheme>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CAL_SCH").MapAllProperties();
            Meta.Property(CalendarScheme.NameProperty).ColumnMeta.HasLength(120);
            Meta.EnableEntityLog();
            Meta.EnablePhantoms();
        }
    }
}