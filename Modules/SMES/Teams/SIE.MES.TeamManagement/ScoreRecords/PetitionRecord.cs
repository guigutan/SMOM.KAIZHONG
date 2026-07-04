using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 申诉记录
    /// </summary>
    [ChildEntity, Serializable]
    ////[CriteriaQuery]
    [Label("申诉记录")]
    public partial class PetitionRecord : DataEntity
    {
        #region 行号 RowIndex
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> RowIndexProperty = P<PetitionRecord>.Register(e => e.RowIndex);

        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex
        {
            get { return this.GetProperty(RowIndexProperty); }
            set { this.SetProperty(RowIndexProperty, value); }
        }
        #endregion

        #region 申诉时间 PetitionDate
        /// <summary>
        /// 申诉时间
        /// </summary>
        [Label("申诉时间")]
        public static readonly Property<DateTime> PetitionDateProperty = P<PetitionRecord>.Register(e => e.PetitionDate);

        /// <summary>
        /// 申诉时间
        /// </summary>
        public DateTime PetitionDate
        {
            get { return GetProperty(PetitionDateProperty); }
            set { SetProperty(PetitionDateProperty, value); }
        }
        #endregion

        #region 申诉说明 PetitionRemark
        /// <summary>
        /// 申诉说明
        /// </summary>
        [Label("申诉说明")]
        public static readonly Property<string> PetitionRemarkProperty = P<PetitionRecord>.Register(e => e.PetitionRemark);

        /// <summary>
        /// 申诉说明
        /// </summary>
        public string PetitionRemark
        {
            get { return GetProperty(PetitionRemarkProperty); }
            set { SetProperty(PetitionRemarkProperty, value); }
        }
        #endregion

        #region 处理时间 ProcessDate
        /// <summary>
        /// 处理时间
        /// </summary>
        [Label("处理时间")]
        public static readonly Property<DateTime?> ProcessDateProperty = P<PetitionRecord>.Register(e => e.ProcessDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessDate
        {
            get { return GetProperty(ProcessDateProperty); }
            set { SetProperty(ProcessDateProperty, value); }
        }
        #endregion

        #region 处理结果 ProcessResult
        /// <summary>
        /// 处理结果
        /// </summary>
        [Label("处理结果")]
        public static readonly Property<string> ProcessResultProperty = P<PetitionRecord>.Register(e => e.ProcessResult);

        /// <summary>
        /// 处理结果
        /// </summary>
        public string ProcessResult
        {
            get { return GetProperty(ProcessResultProperty); }
            set { SetProperty(ProcessResultProperty, value); }
        }
        #endregion

        #region 处理方式 ProcessMode
        /// <summary>
        /// 处理方式
        /// </summary>
        [Label("申述处理方式")]
        public static readonly Property<StateProcessMode?> ProcessModeProperty = P<PetitionRecord>.Register(e => e.ProcessMode);

        /// <summary>
        /// 处理方式
        /// </summary>
        public StateProcessMode? ProcessMode
        {
            get { return GetProperty(ProcessModeProperty); }
            set { SetProperty(ProcessModeProperty, value); }
        }
        #endregion

        #region 处理人 Handler
        /// <summary>
        /// 处理人Id
        /// </summary>
        public static readonly IRefIdProperty HandlerIdProperty = P<PetitionRecord>.RegisterRefId(e => e.HandlerId, ReferenceType.Normal);

        /// <summary>
        /// 处理人Id
        /// </summary>
        public double? HandlerId
        {
            get { return (double?)GetRefNullableId(HandlerIdProperty); }
            set { SetRefNullableId(HandlerIdProperty, value); }
        }

        /// <summary>
        /// 处理人
        /// </summary>
        public static readonly RefEntityProperty<Employee> HandlerProperty = P<PetitionRecord>.RegisterRef(e => e.Handler, HandlerIdProperty);

        /// <summary>
        /// 处理人
        /// </summary>
        public Employee Handler
        {
            get { return GetRefEntity(HandlerProperty); }
            set { SetRefEntity(HandlerProperty, value); }
        }
        #endregion

        #region 申述人 Petitioner
        /// <summary>
        /// 申述人Id
        /// </summary>
        public static readonly IRefIdProperty PetitionerIdProperty = P<PetitionRecord>.RegisterRefId(e => e.PetitionerId, ReferenceType.Normal);

        /// <summary>
        /// 申述人Id
        /// </summary>
        public double PetitionerId
        {
            get { return (double)GetRefId(PetitionerIdProperty); }
            set { SetRefId(PetitionerIdProperty, value); }
        }

        /// <summary>
        /// 申述人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PetitionerProperty = P<PetitionRecord>.RegisterRef(e => e.Petitioner, PetitionerIdProperty);

        /// <summary>
        /// 申述人
        /// </summary>
        public Employee Petitioner
        {
            get { return GetRefEntity(PetitionerProperty); }
            set { SetRefEntity(PetitionerProperty, value); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<PetitionAttachment>> AttachmentListProperty = P<PetitionRecord>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<PetitionAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 处理记录列表 ProcessList
        /// <summary>
        /// 处理记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<ProcessRecord>> ProcessListProperty = P<PetitionRecord>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 处理记录列表
        /// </summary>
        public EntityList<ProcessRecord> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 评分记录 ScoreRecord
        /// <summary>
        /// 评分记录Id
        /// </summary>
        [Label("评分记录")]
        public static readonly IRefIdProperty ScoreRecordIdProperty = P<PetitionRecord>.RegisterRefId(e => e.ScoreRecordId, ReferenceType.Parent);

        /// <summary>
        /// 评分记录Id
        /// </summary>
        public double ScoreRecordId
        {
            get { return (double)GetRefId(ScoreRecordIdProperty); }
            set { SetRefId(ScoreRecordIdProperty, value); }
        }

        /// <summary>
        /// 评分记录
        /// </summary>
        public static readonly RefEntityProperty<ScoreRecord> ScoreRecordProperty = P<PetitionRecord>.RegisterRef(e => e.ScoreRecord, ScoreRecordIdProperty);

        /// <summary>
        /// 评分记录
        /// </summary>
        public ScoreRecord ScoreRecord
        {
            get { return GetRefEntity(ScoreRecordProperty); }
            set { SetRefEntity(ScoreRecordProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 处理人编码 HandlerCode
        /// <summary>
        /// 处理人编码
        /// </summary>
        [Label("处理人编码")]
        public static readonly Property<string> HandlerCodeProperty = P<PetitionRecord>.RegisterView(e => e.HandlerCode, p => p.Handler.Code);

        /// <summary>
        /// 处理人编码
        /// </summary>
        public string HandlerCode
        {
            get { return this.GetProperty(HandlerCodeProperty); }
        }
        #endregion

        #region 处理人名称 HandlerName
        /// <summary>
        /// 处理人名称
        /// </summary>
        [Label("处理人名称")]
        public static readonly Property<string> HandlerNameProperty = P<PetitionRecord>.RegisterView(e => e.HandlerName, p => p.Handler.Name);

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string HandlerName
        {
            get { return this.GetProperty(HandlerNameProperty); }
        }
        #endregion

        #region 申诉人编码 PetitionerCode
        /// <summary>
        /// 申诉人编码
        /// </summary>
        [Label("申诉人编码")]
        public static readonly Property<string> PetitionerCodeProperty = P<PetitionRecord>.RegisterView(e => e.PetitionerCode, p => p.Petitioner.Code);

        /// <summary>
        /// 申诉人编码
        /// </summary>
        public string PetitionerCode
        {
            get { return this.GetProperty(PetitionerCodeProperty); }
        }
        #endregion

        #region 申诉人名称 PetitionerName
        /// <summary>
        /// 申诉人名称
        /// </summary>
        [Label("申诉人名称")]
        public static readonly Property<string> PetitionerNameProperty = P<PetitionRecord>.RegisterView(e => e.PetitionerName, p => p.Petitioner.Name);

        /// <summary>
        /// 申诉人名称
        /// </summary>
        public string PetitionerName
        {
            get { return this.GetProperty(PetitionerNameProperty); }
        }
        #endregion        
        #endregion
    }

    /// <summary>
    /// 申诉记录 实体配置
    /// </summary>
    internal class PetitionRecordConfig : EntityConfig<PetitionRecord>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WG_PET_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}