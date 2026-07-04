using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.ShiftTypes
{
    /// <summary>
    /// 班次休息
    /// </summary>
    [ChildEntity, Serializable]
    [Label("休息时间")]
    [DisplayMember(nameof(ShiftRest.Type))]
    public partial class ShiftRest : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 班次休息
        /// </summary>
        public ShiftRest()
        {
            BeginTime = Convert.ToDateTime("00:00");
            EndTime = Convert.ToDateTime("23:59");
        }
        #endregion

        #region 休息类型 Type
        /// <summary>
        /// 休息类型
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("休息类型")]
        public static readonly Property<string> TypeProperty = P<ShiftRest>.Register(e => e.Type);

        /// <summary>
        /// 休息类型
        /// </summary>
        public string Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginTimeProperty = P<ShiftRest>.Register(e => e.BeginTime, new PropertyMetadata<DateTime>
        {
            DateTimePart = ObjectModel.DateTimePart.Time
        });

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get { return GetProperty(BeginTimeProperty); }
            set { SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 结束时间 EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndTimeProperty = P<ShiftRest>.Register(e => e.EndTime, new PropertyMetadata<DateTime>
        {
            DateTimePart = ObjectModel.DateTimePart.Time
        });

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return GetProperty(EndTimeProperty); }
            set { SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<ShiftRest>.RegisterRefId(e => e.ShiftId, ReferenceType.Parent);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)GetRefId(ShiftIdProperty); }
            set { SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<ShiftRest>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        /// <summary>
        /// 班次休息属性变更事件
        /// </summary>
        /// <param name="e">e</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // 以下代码没什么luan用，
            ////if (this.Shift == null)
            ////{
            ////    return;
            ////}

            ////if (e.Property == BeginTimeProperty || e.Property == EndTimeProperty)
            ////{
            ////    var start = new DateTime(2018, 7, 5, this.BeginTime.Hour, this.BeginTime.Minute, 0);
            ////    var end = new DateTime(2018, 7, 5, this.EndTime.Hour, this.EndTime.Minute, 0);
            ////    var shiftRestWorkHour = (decimal)(end - start).TotalHours;

            ////    var shiftStart = new DateTime(2018, 7, 5, this.Shift.BeginTime.Hour, this.Shift.BeginTime.Minute, 0);
            ////    var shiftEnd = new DateTime(2018, 7, 5, this.Shift.EndTime.Hour, this.Shift.EndTime.Minute, 0);
            ////    var shiftWorkHour = (decimal)(shiftEnd - shiftStart).TotalHours;

            ////    if (shiftRestWorkHour < 0)
            ////    {
            ////        shiftRestWorkHour = (decimal)(end.AddDays(1) - start).TotalHours;
            ////    }

            ////    if (this.Shift.IsOverDay || shiftWorkHour < 0)
            ////    {
            ////        shiftWorkHour = (decimal)(shiftEnd.AddDays(1) - shiftStart).TotalHours;
            ////    }
            ////}
        }
    }

    /// <summary>
    /// 班次休息 实体配置
    /// </summary>
    internal class ShiftRestConfig : EntityConfig<ShiftRest>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CAL_SHIFT_REST").MapAllProperties();
            Meta.Property(ShiftRest.TypeProperty).ColumnMeta.HasLength(120);
            Meta.EnablePhantoms();
        }
    }
}