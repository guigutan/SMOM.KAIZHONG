using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ShiftTypes;
using System;

namespace SIE.Resources.CalendarSchemes
{
    /// <summary>
    /// 例外日历方案
    /// </summary>
    [RootEntity, Serializable]
    [Label("例外日历方案")]
    public partial class CalendarSchemeExcept : DataEntity
    {
        #region 日历日期 CalendarDay
        /// <summary>
        /// 日历日期
        /// </summary>
        [Label("日历日期")]
        public static readonly Property<DateTime> CalendarDayProperty = P<CalendarSchemeExcept>.Register(e => e.CalendarDay);

        /// <summary>
        /// 日历日期
        /// </summary>
        public DateTime CalendarDay
        {
            get { return GetProperty(CalendarDayProperty); }
            set { SetProperty(CalendarDayProperty, value); }
        }
        #endregion

        #region 班制 ShiftType
        /// <summary>
        /// 班制Id
        /// </summary>
        [Label("班制")]
        public static readonly IRefIdProperty ShiftTypeIdProperty = P<CalendarSchemeExcept>.RegisterRefId(e => e.ShiftTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ShiftType> ShiftTypeProperty = P<CalendarSchemeExcept>.RegisterRef(e => e.ShiftType, ShiftTypeIdProperty);

        /// <summary>
        /// 班制
        /// </summary>
        public ShiftType ShiftType
        {
            get { return GetRefEntity(ShiftTypeProperty); }
            set { SetRefEntity(ShiftTypeProperty, value); }
        }
        #endregion

        #region 日历方案 Scheme
        /// <summary>
        /// 日历方案Id
        /// </summary>
        [Label("日历方案")]
        public static readonly IRefIdProperty SchemeIdProperty = P<CalendarSchemeExcept>.RegisterRefId(e => e.SchemeId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<CalendarScheme> SchemeProperty = P<CalendarSchemeExcept>.RegisterRef(e => e.Scheme, SchemeIdProperty);

        /// <summary>
        /// 日历方案
        /// </summary>
        public CalendarScheme Scheme
        {
            get { return GetRefEntity(SchemeProperty); }
            set { SetRefEntity(SchemeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 例外日历方案 实体配置
    /// </summary>
    internal class CalendarSchemeExceptConfig : EntityConfig<CalendarSchemeExcept>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CAL_SCH_EX").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}