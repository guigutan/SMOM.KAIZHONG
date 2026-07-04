using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 任务单执行记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("任务单执行记录")]
    public class ReportOperateLog : DataEntity
    {
        #region 派工管理 DispatchTask
        /// <summary>
        /// 派工管理Id
        /// </summary>
        [Label("派工管理")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<ReportOperateLog>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Parent);

        /// <summary>
        /// 派工管理Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工管理
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<ReportOperateLog>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工管理
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 开始时间 StartTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime?> StartTimeProperty = P<ReportOperateLog>.Register(e => e.StartTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime
        {
            get { return this.GetProperty(StartTimeProperty); }
            set { this.SetProperty(StartTimeProperty, value); }
        }
        #endregion

        #region 开始操作人 StartOpter
        /// <summary>
        /// 开始操作人Id
        /// </summary>
        [Label("开始操作人")]
        public static readonly IRefIdProperty StartOpterIdProperty =
            P<ReportOperateLog>.RegisterRefId(e => e.StartOpterId, ReferenceType.Normal);

        /// <summary>
        /// 开始操作人Id
        /// </summary>
        public double? StartOpterId
        {
            get { return (double?)this.GetRefNullableId(StartOpterIdProperty); }
            set { this.SetRefNullableId(StartOpterIdProperty, value); }
        }

        /// <summary>
        /// 开始操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> StartOpterProperty =
            P<ReportOperateLog>.RegisterRef(e => e.StartOpter, StartOpterIdProperty);

        /// <summary>
        /// 开始操作人
        /// </summary>
        public Employee StartOpter
        {
            get { return this.GetRefEntity(StartOpterProperty); }
            set { this.SetRefEntity(StartOpterProperty, value); }
        }
        #endregion

        #region 结束时间 EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime?> EndTimeProperty = P<ReportOperateLog>.Register(e => e.EndTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 结束操作人 EndOpter
        /// <summary>
        /// 结束操作人Id
        /// </summary>
        [Label("结束操作人")]
        public static readonly IRefIdProperty EndOpterIdProperty =
            P<ReportOperateLog>.RegisterRefId(e => e.EndOpterId, ReferenceType.Normal);

        /// <summary>
        /// 结束操作人Id
        /// </summary>
        public double? EndOpterId
        {
            get { return (double?)this.GetRefNullableId(EndOpterIdProperty); }
            set { this.SetRefNullableId(EndOpterIdProperty, value); }
        }

        /// <summary>
        /// 结束操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EndOpterProperty =
            P<ReportOperateLog>.RegisterRef(e => e.EndOpter, EndOpterIdProperty);

        /// <summary>
        /// 结束操作人
        /// </summary>
        public Employee EndOpter
        {
            get { return this.GetRefEntity(EndOpterProperty); }
            set { this.SetRefEntity(EndOpterProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 实体设置
    /// </summary>
    public class ReportOperateLogConfig : EntityConfig<ReportOperateLog>
    {
        /// <summary>
        /// 数据源
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_DISP_TASKLOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
