using SIE.Domain;
using SIE.MES.TeamManagement.RatedItems;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录
    /// </summary>
    [RootEntity, Serializable]
    ////[CriteriaQuery]
    [ConditionQueryType(typeof(ScoreRecordCriteria))]
    [Label("评分记录")]
    public partial class ScoreRecord : DataEntity
    {
        #region 发起时间 InitiateDate
        /// <summary>
        /// 发起时间
        /// </summary>
        [Label("发起时间")]
        public static readonly Property<DateTime> InitiateDateProperty = P<ScoreRecord>.Register(e => e.InitiateDate);

        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime InitiateDate
        {
            get { return GetProperty(InitiateDateProperty); }
            set { SetProperty(InitiateDateProperty, value); }
        }
        #endregion

        #region 发生时间 OccurDate
        /// <summary>
        /// 发生时间
        /// </summary>
        [Label("发生时间")]
        public static readonly Property<DateTime> OccurDateProperty = P<ScoreRecord>.Register(e => e.OccurDate);

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurDate
        {
            get { return GetProperty(OccurDateProperty); }
            set { SetProperty(OccurDateProperty, value); }
        }
        #endregion

        #region 项目分值 Score
        /// <summary>
        /// 项目分值
        /// </summary>
        [Label("项目分值")]
        public static readonly Property<decimal> ScoreProperty = P<ScoreRecord>.Register(e => e.Score);

        /// <summary>
        /// 项目分值
        /// </summary>
        public decimal Score
        {
            get { return GetProperty(ScoreProperty); }
            set { SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 是否有效 IsEffective
        /// <summary>
        /// 是否有效
        /// </summary>
        [Label("是否有效")]
        public static readonly Property<bool> IsEffectiveProperty = P<ScoreRecord>.Register(e => e.IsEffective);

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffective
        {
            get { return GetProperty(IsEffectiveProperty); }
            set { SetProperty(IsEffectiveProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ScoreRecord>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        [Label("附件列表")]
        public static readonly ListProperty<EntityList<ScoreAttachment>> AttachmentListProperty = P<ScoreRecord>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<ScoreAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("班组成员")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<ScoreRecord>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefId(EmployeeIdProperty); }
            set { SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<ScoreRecord>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 发起人 Initiator
        /// <summary>
        /// 发起人Id
        /// </summary>
        [Label("发起人")]
        public static readonly IRefIdProperty InitiatorIdProperty = P<ScoreRecord>.RegisterRefId(e => e.InitiatorId, ReferenceType.Normal);

        /// <summary>
        /// 发起人Id
        /// </summary>
        public double? InitiatorId
        {
            get { return (double?)this.GetRefNullableId(InitiatorIdProperty); }
            set { this.SetRefNullableId(InitiatorIdProperty, value); }
        }

        /// <summary>
        /// 发起人
        /// </summary>
        public static readonly RefEntityProperty<Employee> InitiatorProperty = P<ScoreRecord>.RegisterRef(e => e.Initiator, InitiatorIdProperty);

        /// <summary>
        /// 发起人
        /// </summary>
        public Employee Initiator
        {
            get { return GetRefEntity(InitiatorProperty); }
            set { SetRefEntity(InitiatorProperty, value); }
        }
        #endregion

        #region 申诉列表 PetitionList
        /// <summary>
        /// 申诉列表
        /// </summary>
        [Label("申诉列表")]
        public static readonly ListProperty<EntityList<PetitionRecord>> PetitionListProperty = P<ScoreRecord>.RegisterList(e => e.PetitionList);

        /// <summary>
        /// 申诉列表
        /// </summary>
        public EntityList<PetitionRecord> PetitionList
        {
            get { return this.GetLazyList(PetitionListProperty); }
        }
        #endregion

        #region 评分项目 RatedItem
        /// <summary>
        /// 评分项目Id
        /// </summary>
        [Label("评分项目")]
        public static readonly IRefIdProperty RatedItemIdProperty = P<ScoreRecord>.RegisterRefId(e => e.RatedItemId, ReferenceType.Normal);

        /// <summary>
        /// 评分项目Id
        /// </summary>
        public double RatedItemId
        {
            get { return (double)GetRefId(RatedItemIdProperty); }
            set { SetRefId(RatedItemIdProperty, value); }
        }

        /// <summary>
        /// 评分项目
        /// </summary>
        public static readonly RefEntityProperty<RatedItem> RatedItemProperty = P<ScoreRecord>.RegisterRef(e => e.RatedItem, RatedItemIdProperty);

        /// <summary>
        /// 评分项目
        /// </summary>
        public RatedItem RatedItem
        {
            get { return GetRefEntity(RatedItemProperty); }
            set { SetRefEntity(RatedItemProperty, value); }
        }
        #endregion

        #region 状态 ScoreState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("评分状态")]
        public static readonly Property<ScoreState> ScoreStateProperty = P<ScoreRecord>.Register(e => e.ScoreState);

        /// <summary>
        /// 状态
        /// </summary>
        public ScoreState ScoreState
        {
            get { return GetProperty(ScoreStateProperty); }
            set { SetProperty(ScoreStateProperty, value); }
        }
        #endregion

        #region 评分来源 ScoreSource
        /// <summary>
        /// 评分来源
        /// </summary>
        [Label("评分来源")]
        public static readonly Property<ScoreSource> ScoreSourceProperty = P<ScoreRecord>.Register(e => e.ScoreSource);

        /// <summary>
        /// 评分来源
        /// </summary>
        public ScoreSource ScoreSource
        {
            get { return this.GetProperty(ScoreSourceProperty); }
            set { this.SetProperty(ScoreSourceProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 班组成员编码 EmployeeCode
        /// <summary>
        /// 编组成员编码
        /// </summary>
        [Label("班组成员编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<ScoreRecord>.RegisterView(e => e.EmployeeCode, p => p.Employee.Code);

        /// <summary>
        /// 编组成员编码
        /// </summary>
        public string EmployeeCode
        {
            get { return this.GetProperty(EmployeeCodeProperty); }
        }
        #endregion

        #region 班组成员姓名 EmployeeName
        /// <summary>
        /// 班组成员姓名
        /// </summary>
        [Label("班组成员姓名")]
        public static readonly Property<string> EmployeeNameProperty = P<ScoreRecord>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 班组成员姓名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 发起人编码 InitiaorCode
        /// <summary>
        /// 发起人编码
        /// </summary>
        [Label("发起人编码")]
        public static readonly Property<string> InitiatorCodeProperty = P<ScoreRecord>.RegisterView(e => e.InitiatorCode, p => p.Initiator.Code);

        /// <summary>
        /// 发起人编码
        /// </summary>
        public string InitiatorCode
        {
            get { return this.GetProperty(InitiatorCodeProperty); }
        }
        #endregion

        #region 发起人姓名 InitiaorName
        /// <summary>
        /// 发起人姓名
        /// </summary>
        [Label("发起人姓名")]
        public static readonly Property<string> InitiaorNameProperty = P<ScoreRecord>.RegisterView(e => e.InitiaorName, p => p.Initiator.Name);

        /// <summary>
        /// 发起人姓名
        /// </summary>
        public string InitiaorName
        {
            get { return this.GetProperty(InitiaorNameProperty); }
        }
        #endregion

        #region 评分项目编码 RatedItemCode
        /// <summary>
        /// 评分项目编码
        /// </summary>
        [Label("评分项目编码")]
        public static readonly Property<string> RatedItemCodeProperty = P<ScoreRecord>.RegisterView(e => e.RatedItemCode, p => p.RatedItem.Code);

        /// <summary>
        /// 评分项目编码
        /// </summary>
        public string RatedItemCode
        {
            get { return this.GetProperty(RatedItemCodeProperty); }
        }
        #endregion

        #region 评分项目名称 RatedItemName
        /// <summary>
        /// 评分项目名称
        /// </summary>
        [Label("评分项目名称")]
        public static readonly Property<string> RatedItemNameProperty = P<ScoreRecord>.RegisterView(e => e.RatedItemName, p => p.RatedItem.Name);

        /// <summary>
        /// 评分项目名称
        /// </summary>
        public string RatedItemName
        {
            get { return this.GetProperty(RatedItemNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 评分记录 实体配置
    /// </summary>
    internal class ScoreRecordConfig : EntityConfig<ScoreRecord>
    {
        /// <summary>
        /// 评分记录配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WG_SCORE_RECORD").MapAllProperties();
            Meta.Property(ScoreRecord.OccurDateProperty).ColumnMeta.HasIndex();
            //Meta.EnablePhantoms();
        }
    }
}