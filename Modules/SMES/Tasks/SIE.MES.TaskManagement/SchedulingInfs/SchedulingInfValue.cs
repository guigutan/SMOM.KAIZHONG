using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    public class SchedulingInfValue : DataEntity
    {
        #region 排程中间表 SchedulingInf
        /// <summary>
        /// 排程中间表Id
        /// </summary>
        [Label("排程中间表")]
        public static readonly IRefIdProperty SchedulingInfIdProperty =
            P<SchedulingInfValue>.RegisterRefId(e => e.SchedulingInfId, ReferenceType.Normal);

        /// <summary>
        /// 排程中间表Id
        /// </summary>
        public double SchedulingInfId
        {
            get { return (double)this.GetRefId(SchedulingInfIdProperty); }
            set { this.SetRefId(SchedulingInfIdProperty, value); }
        }

        /// <summary>
        /// 排程中间表
        /// </summary>
        public static readonly RefEntityProperty<SchedulingInf> SchedulingInfProperty =
            P<SchedulingInfValue>.RegisterRef(e => e.SchedulingInf, SchedulingInfIdProperty);

        /// <summary>
        /// 排程中间表
        /// </summary>
        public SchedulingInf SchedulingInf
        {
            get { return this.GetRefEntity(SchedulingInfProperty); }
            set { this.SetRefEntity(SchedulingInfProperty, value); }
        }
        #endregion

        #region 日期 Date
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime> DateProperty = P<SchedulingInfValue>.Register(e => e.Date);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion

        #region 派工管理1 DispatchTask1
        /// <summary>
        /// 派工管理1Id
        /// </summary>
        [Label("派工管理1")]
        public static readonly IRefIdProperty DispatchTask1IdProperty =
            P<SchedulingInfValue>.RegisterRefId(e => e.DispatchTask1Id, ReferenceType.Normal);

        /// <summary>
        /// 派工管理1Id
        /// </summary>
        public double? DispatchTask1Id
        {
            get { return (double?)this.GetRefNullableId(DispatchTask1IdProperty); }
            set { this.SetRefNullableId(DispatchTask1IdProperty, value); }
        }

        /// <summary>
        /// 派工管理1
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTask1Property =
            P<SchedulingInfValue>.RegisterRef(e => e.DispatchTask1, DispatchTask1IdProperty);

        /// <summary>
        /// 派工管理1
        /// </summary>
        public DispatchTask DispatchTask1
        {
            get { return this.GetRefEntity(DispatchTask1Property); }
            set { this.SetRefEntity(DispatchTask1Property, value); }
        }
        #endregion

        #region 派工管理2 DispatchTask2
        /// <summary>
        /// 派工管理2Id
        /// </summary>
        [Label("派工管理2")]
        public static readonly IRefIdProperty DispatchTask2IdProperty =
            P<SchedulingInfValue>.RegisterRefId(e => e.DispatchTask2Id, ReferenceType.Normal);

        /// <summary>
        /// 派工管理2Id
        /// </summary>
        public double? DispatchTask2Id
        {
            get { return (double?)this.GetRefNullableId(DispatchTask2IdProperty); }
            set { this.SetRefNullableId(DispatchTask2IdProperty, value); }
        }

        /// <summary>
        /// 派工管理2
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTask2Property =
            P<SchedulingInfValue>.RegisterRef(e => e.DispatchTask2, DispatchTask2IdProperty);

        /// <summary>
        /// 派工管理2
        /// </summary>
        public DispatchTask DispatchTask2
        {
            get { return this.GetRefEntity(DispatchTask2Property); }
            set { this.SetRefEntity(DispatchTask2Property, value); }
        }
        #endregion

        #region Value1(白班) Value1
        /// <summary>
        /// Value1(白班)
        /// </summary>
        [Label("Value1")]
        public static readonly Property<decimal?> Value1Property = P<SchedulingInfValue>.Register(e => e.Value1);

        /// <summary>
        /// Value1(白班)
        /// </summary>
        public decimal? Value1
        {
            get { return this.GetProperty(Value1Property); }
            set { this.SetProperty(Value1Property, value); }
        }
        #endregion

        #region Value2(晚班) Value2
        /// <summary>
        /// Value2(晚班)
        /// </summary>
        [Label("Value2")]
        public static readonly Property<decimal?> Value2Property = P<SchedulingInfValue>.Register(e => e.Value2);

        /// <summary>
        /// Value2(晚班)
        /// </summary>
        public decimal? Value2
        {
            get { return this.GetProperty(Value2Property); }
            set { this.SetProperty(Value2Property, value); }
        }
        #endregion

        #region 导入时间1 ImportTime1
        /// <summary>
        /// 导入时间1
        /// </summary>
        [Label("导入时间1")]
        public static readonly Property<DateTime?> ImportTime1Property = P<SchedulingInfValue>.Register(e => e.ImportTime1);

        /// <summary>
        /// 导入时间1
        /// </summary>
        public DateTime? ImportTime1
        {
            get { return this.GetProperty(ImportTime1Property); }
            set { this.SetProperty(ImportTime1Property, value); }
        }
        #endregion

        #region 导入时间2 ImportTime2
        /// <summary>
        /// 导入时间2
        /// </summary>
        [Label("导入时间2")]
        public static readonly Property<DateTime?> ImportTime2Property = P<SchedulingInfValue>.Register(e => e.ImportTime2);

        /// <summary>
        /// 导入时间2
        /// </summary>
        public DateTime? ImportTime2
        {
            get { return this.GetProperty(ImportTime2Property); }
            set { this.SetProperty(ImportTime2Property, value); }
        }
        #endregion

        #region 视图属性

        #region 任务单1状态 TaskStatus1
        /// <summary>
        /// 任务单1状态
        /// </summary>
        [Label("任务单1状态")]
        public static readonly Property<DispatchTaskStatus?> TaskStatus1Property = P<SchedulingInfValue>.RegisterView(e => e.TaskStatus1,p=>p.DispatchTask1.TaskStatus);

        /// <summary>
        /// 任务单1状态
        /// </summary>
        public DispatchTaskStatus? TaskStatus1
        {
            get { return this.GetProperty(TaskStatus1Property); }
        }
        #endregion

        #region 任务单2状态 TaskStatus2
        /// <summary>
        /// 任务单2状态
        /// </summary>
        [Label("任务单2状态")]
        public static readonly Property<DispatchTaskStatus?> TaskStatus2Property = P<SchedulingInfValue>.RegisterView(e => e.TaskStatus2, p => p.DispatchTask2.TaskStatus);

        /// <summary>
        /// 任务单2状态
        /// </summary>
        public DispatchTaskStatus? TaskStatus2
        {
            get { return this.GetProperty(TaskStatus2Property); }
        }
        #endregion

        #endregion

        #region 不映射数据库

        #region 日期string DateStr
        /// <summary>
        /// 日期string
        /// </summary>
        [Label("日期string")]
        public static readonly Property<string> DateStrProperty = P<SchedulingInfValue>.Register(e => e.DateStr);

        /// <summary>
        /// 日期string
        /// </summary>
        public string DateStr
        {
            get { return this.GetProperty(DateStrProperty); }
            set { this.SetProperty(DateStrProperty, value); }
        }
        #endregion

        #endregion
    }

    internal class SchedulingInfValueConfig : EntityConfig<SchedulingInfValue>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("SCHEDULING_INF_VALUE").MapAllProperties();
            Meta.Property(SchedulingInfValue.DateStrProperty).DontMapColumn();
        }
    }
}
