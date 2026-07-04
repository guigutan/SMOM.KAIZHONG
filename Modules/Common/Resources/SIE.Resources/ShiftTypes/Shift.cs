using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.ShiftTypes
{
    /// <summary>
    /// 班次
    /// </summary>
    [ChildEntity, Serializable]
    [Label("班次")]
    [DisplayMember(nameof(Shift.Name))]
    public partial class Shift : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 班次
        /// </summary>
        public Shift()
        {
            BeginTime = Convert.ToDateTime("00:00");
            EndTime = Convert.ToDateTime("23:59");
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("班次编码")]
        public static readonly Property<string> CodeProperty = P<Shift>.Register(e => e.Code);

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
        [MaxLength(40)]
        [Label("班次名称")]
        public static readonly Property<string> NameProperty = P<Shift>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> BeginTimeProperty = P<Shift>.Register(e => e.BeginTime, new PropertyMetadata<DateTime>
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
        public static readonly Property<DateTime> EndTimeProperty = P<Shift>.Register(e => e.EndTime, new PropertyMetadata<DateTime>
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

        #region 是否跨日 IsOverDay
        /// <summary>
        /// 是否跨日
        /// </summary>
        [Label("是否跨日")]
        public static readonly Property<bool> IsOverDayProperty = P<Shift>.Register(e => e.IsOverDay);

        /// <summary>
        /// 是否跨日
        /// </summary>
        public bool IsOverDay
        {
            get { return GetProperty(IsOverDayProperty); }
            set { SetProperty(IsOverDayProperty, value); }
        }
        #endregion

        #region 班次休息列表 ShiftRestList
        /// <summary>
        /// 班次休息列表
        /// </summary>
        public static readonly ListProperty<EntityList<ShiftRest>> ShiftRestListProperty = P<Shift>.RegisterList(e => e.ShiftRestList);

        /// <summary>
        /// 班次休息列表
        /// </summary>
        public EntityList<ShiftRest> ShiftRestList
        {
            get { return this.GetLazyList(ShiftRestListProperty); }
        }
        #endregion

        #region 班制 ShiftType
        /// <summary>
        /// 班制Id
        /// </summary>
        [Label("班制")]
        public static readonly IRefIdProperty ShiftTypeIdProperty = P<Shift>.RegisterRefId(e => e.ShiftTypeId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<ShiftType> ShiftTypeProperty = P<Shift>.RegisterRef(e => e.ShiftType, ShiftTypeIdProperty);

        /// <summary>
        /// 班制
        /// </summary>
        public ShiftType ShiftType
        {
            get { return GetRefEntity(ShiftTypeProperty); }
            set { SetRefEntity(ShiftTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 班次属性变更事件
        /// </summary>
        /// <param name="e">e</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (this.ShiftType == null)
            {
                return;
            }

            if (e.Property == BeginTimeProperty || e.Property == EndTimeProperty)
            {
                var start = new DateTime(2018, 7, 5, this.BeginTime.Hour, this.BeginTime.Minute, 0);
                var end = new DateTime(2018, 7, 5, this.EndTime.Hour, this.EndTime.Minute, 0);

                if (end < start)
                {
                    if (!this.IsOverDay)
                    {
                        this.IsOverDay = true;
                    }
                }
                else
                {
                    if (this.IsOverDay)
                    {
                        this.IsOverDay = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 班次 实体配置
    /// </summary>
    internal class ShiftConfig : EntityConfig<Shift>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CAL_SHIFT").MapAllProperties();
            Meta.Property(Shift.CodeProperty).ColumnMeta.HasLength(120);
            Meta.Property(Shift.NameProperty).ColumnMeta.HasLength(120);
            Meta.EnablePhantoms();
        }
    }
}