using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ShiftTypes;
using System;

namespace SIE.Resources.CalendarSchemes
{
    /// <summary>
    /// 周方案
    /// </summary>
    [RootEntity, Serializable]
    [Label("周方案")]
    public partial class CalendarSchemeWeek : DataEntity
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public CalendarSchemeWeek()
        {
            ActiveDate = DateTime.Today.AddDays(1);
        }

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [Required]
        [MaxLength(40)]
        public static readonly Property<string> NameProperty = P<CalendarSchemeWeek>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 预设启用日期 ActiveDate
        /// <summary>
        /// 预设启用日期
        /// </summary>
        [Label("预设启用日期")]
        public static readonly Property<DateTime> ActiveDateProperty = P<CalendarSchemeWeek>.Register(e => e.ActiveDate);

        /// <summary>
        /// 预设启用日期
        /// </summary>
        public DateTime ActiveDate
        {
            get { return GetProperty(ActiveDateProperty); }
            set { SetProperty(ActiveDateProperty, value); }
        }
        #endregion

        #region 日历方案 Scheme
        /// <summary>
        /// 日历方案Id
        /// </summary>
        [Label("日历方案")]
        public static readonly IRefIdProperty SchemeIdProperty = P<CalendarSchemeWeek>.RegisterRefId(e => e.SchemeId, ReferenceType.Parent);

        /// <summary>
        /// 日历方案Id
        /// </summary>
        public double SchemeId
        {
            get { return (double)GetRefId(SchemeIdProperty); }
            set { SetRefId(SchemeIdProperty, value); }
        }

        /// <summary>
        /// 日历方案
        /// </summary>
        public static readonly RefEntityProperty<CalendarScheme> SchemeProperty = P<CalendarSchemeWeek>.RegisterRef(e => e.Scheme, SchemeIdProperty);

        /// <summary>
        /// 日历方案
        /// </summary>
        public CalendarScheme Scheme
        {
            get { return GetRefEntity(SchemeProperty); }
            set { SetRefEntity(SchemeProperty, value); }
        }
        #endregion

        #region 星期一 Mon
        /// <summary>
        /// 星期一
        /// </summary>
        [Label("星期一")]
        public static readonly Property<bool> MonProperty = P<CalendarSchemeWeek>.Register(e => e.Mon);

        /// <summary>
        /// 星期一
        /// </summary>
        public bool Mon
        {
            get { return GetProperty(MonProperty); }
            set { SetProperty(MonProperty, value); }
        }
        #endregion

        #region 星期二 Tue
        /// <summary>
        /// 星期二
        /// </summary>
        [Label("星期二")]
        public static readonly Property<bool> TueProperty = P<CalendarSchemeWeek>.Register(e => e.Tue);

        /// <summary>
        /// 星期二
        /// </summary>
        public bool Tue
        {
            get { return GetProperty(TueProperty); }
            set { SetProperty(TueProperty, value); }
        }
        #endregion

        #region 星期三 Wed
        /// <summary>
        /// 星期三
        /// </summary>
        [Label("星期三")]
        public static readonly Property<bool> WedProperty = P<CalendarSchemeWeek>.Register(e => e.Wed);

        /// <summary>
        /// 星期三
        /// </summary>
        public bool Wed
        {
            get { return GetProperty(WedProperty); }
            set { SetProperty(WedProperty, value); }
        }
        #endregion

        #region 星期四 Thu
        /// <summary>
        /// 星期四
        /// </summary>
        [Label("星期四")]
        public static readonly Property<bool> ThuProperty = P<CalendarSchemeWeek>.Register(e => e.Thu);

        /// <summary>
        /// 星期四
        /// </summary>
        public bool Thu
        {
            get { return GetProperty(ThuProperty); }
            set { SetProperty(ThuProperty, value); }
        }
        #endregion

        #region 星期五 Fri
        /// <summary>
        /// 星期五
        /// </summary>
        [Label("星期五")]
        public static readonly Property<bool> FriProperty = P<CalendarSchemeWeek>.Register(e => e.Fri);

        /// <summary>
        /// 星期五
        /// </summary>
        public bool Fri
        {
            get { return GetProperty(FriProperty); }
            set { SetProperty(FriProperty, value); }
        }
        #endregion

        #region 星期六 Sat
        /// <summary>
        /// 星期六
        /// </summary>
        [Label("星期六")]
        public static readonly Property<bool> SatProperty = P<CalendarSchemeWeek>.Register(e => e.Sat);

        /// <summary>
        /// 星期六
        /// </summary>
        public bool Sat
        {
            get { return GetProperty(SatProperty); }
            set { SetProperty(SatProperty, value); }
        }
        #endregion

        #region 星期日 Sun
        /// <summary>
        /// 星期日
        /// </summary>
        [Label("星期日")]
        public static readonly Property<bool> SunProperty = P<CalendarSchemeWeek>.Register(e => e.Sun);

        /// <summary>
        /// 星期日
        /// </summary>
        public bool Sun
        {
            get { return GetProperty(SunProperty); }
            set { SetProperty(SunProperty, value); }
        }
        #endregion

        #region 班制 ShiftType
        /// <summary>
        /// 班制Id
        /// </summary>
        [Label("班制")]
        public static readonly IRefIdProperty ShiftTypeIdProperty = P<CalendarSchemeWeek>.RegisterRefId(e => e.ShiftTypeId, ReferenceType.Normal);

        /// <summary>
        /// 班制Id
        /// </summary>
        public double ShiftTypeId
        {
            get { return (double)GetRefId(ShiftTypeIdProperty); }
            set { SetRefId(ShiftTypeIdProperty, value); }
        }

        /// <summary>
        /// 班制
        /// </summary>
        public static readonly RefEntityProperty<ShiftType> ShiftTypeProperty = P<CalendarSchemeWeek>.RegisterRef(e => e.ShiftType, ShiftTypeIdProperty);

        /// <summary>
        /// 班制
        /// </summary>
        public ShiftType ShiftType
        {
            get { return GetRefEntity(ShiftTypeProperty); }
            set { SetRefEntity(ShiftTypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 周方案 实体配置
    /// </summary>
    internal class CalendarSchemeWeekConfig : EntityConfig<CalendarSchemeWeek>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CAL_SCH_WEEK").MapAllProperties();
            Meta.Property(CalendarSchemeWeek.NameProperty).ColumnMeta.HasLength(120);
            Meta.EnablePhantoms();
        }
    }
}
