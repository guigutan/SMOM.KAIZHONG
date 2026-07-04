using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Tech.Processs;
using SIE.Resources.Employees;

namespace SIE.MES.ProcessPrepareRecords
{
    /// <summary>
    /// 产前准备记录明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产前准备记录明细")]
    public class ProcessPrepareRecordDetail : DataEntity
    {

        #region 产前准备记录 PrepareRecord
        /// <summary>
        /// 产前准备记录Id
        /// </summary>
        [Label("产前准备记录")]
        public static readonly IRefIdProperty PrepareRecordIdProperty =
            P<ProcessPrepareRecordDetail>.RegisterRefId(e => e.PrepareRecordId, ReferenceType.Parent);

        /// <summary>
        /// 产前准备记录Id
        /// </summary>
        public double PrepareRecordId
        {
            get { return (double)this.GetRefId(PrepareRecordIdProperty); }
            set { this.SetRefId(PrepareRecordIdProperty, value); }
        }

        /// <summary>
        /// 产前准备记录
        /// </summary>
        public static readonly RefEntityProperty<ProcessPrepareRecord> PrepareRecordProperty =
            P<ProcessPrepareRecordDetail>.RegisterRef(e => e.PrepareRecord, PrepareRecordIdProperty);

        /// <summary>
        /// 产前准备记录
        /// </summary>
        public ProcessPrepareRecord PrepareRecord
        {
            get { return this.GetRefEntity(PrepareRecordProperty); }
            set { this.SetRefEntity(PrepareRecordProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ProcessPrepareRecordDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<ProcessPrepareRecordDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产前项目Id PrepareProjectId
        /// <summary>
        /// 产前项目Id
        /// </summary>
        [Label("产前项目Id")]
        public static readonly Property<double?> PrepareProjectIdProperty = P<ProcessPrepareRecordDetail>.Register(e => e.PrepareProjectId);

        /// <summary>
        /// 产前项目Id
        /// </summary>
        public double? PrepareProjectId
        {
            get { return this.GetProperty(PrepareProjectIdProperty); }
            set { this.SetProperty(PrepareProjectIdProperty, value); }
        }
        #endregion

        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<ProcessPrepareRecordDetail>.Register(e => e.ProjectCode);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
            set { this.SetProperty(ProjectCodeProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<ProcessPrepareRecordDetail>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
            set { this.SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<PrepareProjectType?> ProjectTypeProperty = P<ProcessPrepareRecordDetail>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType? ProjectType
        {
            get { return this.GetProperty(ProjectTypeProperty); }
            set { this.SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 项目描述 ProjectDesc
        /// <summary>
        /// 项目描述
        /// </summary>
        [Label("项目描述")]
        [MaxLength(200)]
        public static readonly Property<string> ProjectDescProperty = P<ProcessPrepareRecordDetail>.Register(e => e.ProjectDesc);

        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProjectDesc
        {
            get { return this.GetProperty(ProjectDescProperty); }
            set { this.SetProperty(ProjectDescProperty, value); }
        }
        #endregion

        #region 计数器 Counter
        /// <summary>
        /// 计数器
        /// </summary>
        [Label("计数器")]
        public static readonly Property<int> CounterProperty = P<ProcessPrepareRecordDetail>.Register(e => e.Counter);

        /// <summary>
        /// 计数器
        /// </summary>
        public int Counter
        {
            get { return this.GetProperty(CounterProperty); }
            set { this.SetProperty(CounterProperty, value); }
        }
        #endregion

        #region 结果 Result
        /// <summary>
        /// 结果
        /// </summary>
        [Label("结果")]
        public static readonly Property<PrepareRecordDetailResult?> ResultProperty = P<ProcessPrepareRecordDetail>.Register(e => e.Result);

        /// <summary>
        /// 结果
        /// </summary>
        public PrepareRecordDetailResult? Result
        {
            get { return this.GetProperty(ResultProperty); }
            set { this.SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(2000)]
        public static readonly Property<string> RemarkProperty = P<ProcessPrepareRecordDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 确认人 Confirmer
        /// <summary>
        /// 确认人Id
        /// </summary>
        [Label("确认人")]
        public static readonly IRefIdProperty ConfirmerIdProperty =
            P<ProcessPrepareRecordDetail>.RegisterRefId(e => e.ConfirmerId, ReferenceType.Normal);

        /// <summary>
        /// 确认人Id
        /// </summary>
        public double? ConfirmerId
        {
            get { return (double?)this.GetRefNullableId(ConfirmerIdProperty); }
            set { this.SetRefNullableId(ConfirmerIdProperty, value); }
        }

        /// <summary>
        /// 确认人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ConfirmerProperty =
            P<ProcessPrepareRecordDetail>.RegisterRef(e => e.Confirmer, ConfirmerIdProperty);

        /// <summary>
        /// 确认人
        /// </summary>
        public Employee Confirmer
        {
            get { return this.GetRefEntity(ConfirmerProperty); }
            set { this.SetRefEntity(ConfirmerProperty, value); }
        }
        #endregion

        #region 确认时间 ConfirmTime
        /// <summary>
        /// 确认时间
        /// </summary>
        [Label("确认时间")]
        public static readonly Property<DateTime?> ConfirmTimeProperty = P<ProcessPrepareRecordDetail>.Register(e => e.ConfirmTime);

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmTime
        {
            get { return this.GetProperty(ConfirmTimeProperty); }
            set { this.SetProperty(ConfirmTimeProperty, value); }
        }
        #endregion

        #region 确认人名称 ConfirmerName
        /// <summary>
        /// 确认人名称
        /// </summary>
        [Label("确认人名称")]
        public static readonly Property<string> ConfirmerNameProperty = P<ProcessPrepareRecordDetail>.RegisterView(e => e.ConfirmerName, p => p.Confirmer.Name);

        /// <summary>
        /// 确认人名称
        /// </summary>
        public string ConfirmerName
        {
            get { return this.GetProperty(ConfirmerNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProcessPrepareRecordDetail>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion
    }

    internal class ProcessPrepareRecordDetailConfig : EntityConfig<ProcessPrepareRecordDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("PROCESS_PREPARE_RECORD_DTL").MapAllProperties();
            Meta.Property(ProcessPrepareRecordDetail.ProjectDescProperty).ColumnMeta.HasLength(400);
            Meta.Property(ProcessPrepareRecordDetail.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
