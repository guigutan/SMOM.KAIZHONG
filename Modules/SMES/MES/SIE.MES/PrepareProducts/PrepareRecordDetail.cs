using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备记录子表
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产前准备记录子表")]
   
    public class PrepareRecordDetail : DataEntity
    {
        #region 产前准备记录 PrepareRecord
        /// <summary>
        /// 产前准备记录Id
        /// </summary>
        [Label("产前准备记录")]
        public static readonly IRefIdProperty PrepareRecordIdProperty =
            P<PrepareRecordDetail>.RegisterRefId(e => e.PrepareRecordId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<PrepareRecord> PrepareRecordProperty =
            P<PrepareRecordDetail>.RegisterRef(e => e.PrepareRecord, PrepareRecordIdProperty);

        /// <summary>
        /// 产前准备记录
        /// </summary>
        public PrepareRecord PrepareRecord
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
            P<PrepareRecordDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<PrepareRecordDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly Property<double?> PrepareProjectIdProperty = P<PrepareRecordDetail>.Register(e => e.PrepareProjectId);

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
        public static readonly Property<string> ProjectCodeProperty = P<PrepareRecordDetail>.Register(e => e.ProjectCode);

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
        public static readonly Property<string> ProjectNameProperty = P<PrepareRecordDetail>.Register(e => e.ProjectName);

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
        public static readonly Property<PrepareProjectType?> ProjectTypeProperty = P<PrepareRecordDetail>.Register(e => e.ProjectType);

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
        public static readonly Property<string> ProjectDescProperty = P<PrepareRecordDetail>.Register(e => e.ProjectDesc);

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
        public static readonly Property<int> CounterProperty = P<PrepareRecordDetail>.Register(e => e.Counter);

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
        public static readonly Property<PrepareRecordDetailResult?> ResultProperty = P<PrepareRecordDetail>.Register(e => e.Result);

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
        public static readonly Property<string> RemarkProperty = P<PrepareRecordDetail>.Register(e => e.Remark);

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
            P<PrepareRecordDetail>.RegisterRefId(e => e.ConfirmerId, ReferenceType.Normal);

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
            P<PrepareRecordDetail>.RegisterRef(e => e.Confirmer, ConfirmerIdProperty);

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
        public static readonly Property<DateTime?> ConfirmTimeProperty = P<PrepareRecordDetail>.Register(e => e.ConfirmTime);

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
        public static readonly Property<string> ConfirmerNameProperty = P<PrepareRecordDetail>.RegisterView(e => e.ConfirmerName, p => p.Confirmer.Name);

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
        public static readonly Property<string> ProcessNameProperty = P<PrepareRecordDetail>.RegisterView(e => e.ProcessName, p => p.Process.Name);

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

    /// <summary>
    /// 实体元配置
    /// </summary>
    public class PrepareRecordDetailConfig : EntityConfig<PrepareRecordDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRERECORD").MapAllProperties();
            Meta.Property(PrepareRecordDetail.ProjectDescProperty).ColumnMeta.HasLength(400);
            Meta.Property(PrepareRecordDetail.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }

}
