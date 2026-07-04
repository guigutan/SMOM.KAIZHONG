using SIE.Domain;
using SIE.EMS.AssetIssues;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AssetReturns
{
    /// <summary>
    /// 归还工治具清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工治具清单")]
    public partial class AssetReturnFixture : DataEntity
    {
        #region 资产归还单 AssetReturn
        /// <summary>
        /// 资产归还单Id
        /// </summary>
        [Label("资产归还单")]
        public static readonly IRefIdProperty AssetReturnIdProperty = P<AssetReturnFixture>.RegisterRefId(e => e.AssetReturnId, ReferenceType.Parent);

        /// <summary>
        /// 资产归还单Id
        /// </summary>
        public double AssetReturnId
        {
            get { return (double)GetRefId(AssetReturnIdProperty); }
            set { SetRefId(AssetReturnIdProperty, value); }
        }

        /// <summary>
        /// 资产归还单
        /// </summary>
        public static readonly RefEntityProperty<AssetReturn> AssetReturnProperty = P<AssetReturnFixture>.RegisterRef(e => e.AssetReturn, AssetReturnIdProperty);

        /// <summary>
        /// 资产归还单
        /// </summary>
        public AssetReturn AssetReturn
        {
            get { return GetRefEntity(AssetReturnProperty); }
            set { SetRefEntity(AssetReturnProperty, value); }
        }
        #endregion

        #region ID类工治具台账 FixtureAccount
        /// <summary>
        /// ID类工治具台账Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty FixtureAccountIdProperty =
            P<AssetReturnFixture>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

        /// <summary>
        /// ID类工治具台账Id
        /// </summary>
        public double? FixtureAccountId
        {
            get { return (double?)this.GetRefNullableId(FixtureAccountIdProperty); }
            set { this.SetRefNullableId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// ID类工治具台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureIDAccount> FixtureAccountProperty =
            P<AssetReturnFixture>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// ID类工治具台账
        /// </summary>
        public FixtureIDAccount FixtureAccount
        {
            get { return this.GetRefEntity(FixtureAccountProperty); }
            set { this.SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<AssetReturnFixture>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 归还类型 ReturnType
        /// <summary>
        /// 归还类型
        /// </summary>
        [Label("归还类型")]
        public static readonly Property<ReturnType?> ReturnTypeProperty = P<AssetReturnFixture>.Register(e => e.ReturnType);

        /// <summary>
        /// 归还类型
        /// </summary>
        public ReturnType? ReturnType
        {
            get { return GetProperty(ReturnTypeProperty); }
            set { SetProperty(ReturnTypeProperty, value); }
        }
        #endregion

        #region 归还数量 Qty
        /// <summary>
        /// 归还数量
        /// </summary>
        [Label("归还数量")]
        public static readonly Property<int> QtyProperty = P<AssetReturnFixture>.Register(e => e.Qty);

        /// <summary>
        /// 归还数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<FixtureQualityState> QualityStatusProperty = P<AssetReturnFixture>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState QualityStatus
        {
            get { return GetProperty(QualityStatusProperty); }
            set { SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 领用工治具明细 AssetRequisitionFixture
        /// <summary>
        /// 领用工治具明细Id
        /// </summary>
        [Label("领用工治具明细")]
        public static readonly IRefIdProperty AssetRequisitionFixtureIdProperty = P<AssetReturnFixture>.RegisterRefId(e => e.AssetRequisitionFixtureId, ReferenceType.Normal);

        /// <summary>
        /// 领用工治具明细Id
        /// </summary>
        public double AssetRequisitionFixtureId
        {
            get { return (double)GetRefId(AssetRequisitionFixtureIdProperty); }
            set { SetRefId(AssetRequisitionFixtureIdProperty, value); }
        }

        /// <summary>
        /// 领用工治具明细
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisitionFixture> AssetRequisitionFixtureProperty = P<AssetReturnFixture>.RegisterRef(e => e.AssetRequisitionFixture, AssetRequisitionFixtureIdProperty);

        /// <summary>
        /// 领用工治具明细
        /// </summary>
        public AssetRequisitionFixture AssetRequisitionFixture
        {
            get { return GetRefEntity(AssetRequisitionFixtureProperty); }
            set { SetRefEntity(AssetRequisitionFixtureProperty, value); }
        }
        #endregion

        #region 发放工治具明细 AssetIssueFixture
        /// <summary>
        /// 发放工治具明细Id
        /// </summary>
        [Label("发放工治具明细")]
        public static readonly IRefIdProperty AssetIssueFixtureIdProperty =
            P<AssetReturnFixture>.RegisterRefId(e => e.AssetIssueFixtureId, ReferenceType.Normal);

        /// <summary>
        /// 发放工治具明细Id
        /// </summary>
        public double? AssetIssueFixtureId
        {
            get { return (double?)this.GetRefNullableId(AssetIssueFixtureIdProperty); }
            set { this.SetRefNullableId(AssetIssueFixtureIdProperty, value); }
        }

        /// <summary>
        /// 发放工治具明细
        /// </summary>
        public static readonly RefEntityProperty<AssetIssueFixture> AssetIssueFixtureProperty =
            P<AssetReturnFixture>.RegisterRef(e => e.AssetIssueFixture, AssetIssueFixtureIdProperty);

        /// <summary>
        /// 发放工治具明细
        /// </summary>
        public AssetIssueFixture AssetIssueFixture
        {
            get { return this.GetRefEntity(AssetIssueFixtureProperty); }
            set { this.SetRefEntity(AssetIssueFixtureProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 借用行号 LineNo
        /// <summary>
        /// 借用行号
        /// </summary>
        [Label("借用行号")]
        public static readonly Property<string> LineNoProperty = P<AssetReturnFixture>.RegisterView(e => e.LineNo, p => p.AssetRequisitionFixture.LineNo);

        /// <summary>
        /// 借用行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 工治具编码Id FixtureEncodeId
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码Id")]
        public static readonly Property<double> FixtureEncodeIdProperty = P<AssetReturnFixture>.RegisterView(e => e.FixtureEncodeId, p => p.AssetRequisitionFixture.FixtureEncodeId);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return this.GetProperty(FixtureEncodeIdProperty); }
            set { this.SetProperty(FixtureEncodeIdProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeProperty = P<AssetReturnFixture>.RegisterView(e => e.FixtureEncode, p => p.AssetRequisitionFixture.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncode
        {
            get { return this.GetProperty(FixtureEncodeProperty); }
            set { this.SetProperty(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 工治具型号Id FixtureModelId
        /// <summary>
        /// 工治具型号Id
        /// </summary>
        [Label("工治具型号Id")]
        public static readonly Property<double> FixtureModelIdProperty = P<AssetReturnFixture>.RegisterView(e => e.FixtureModelId, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModelId);

        /// <summary>
        /// 工治具型号Id
        /// </summary>
        public double FixtureModelId
        {
            get { return this.GetProperty(FixtureModelIdProperty); }
            set { this.SetProperty(FixtureModelIdProperty, value); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<AssetReturnFixture>.RegisterView(e => e.ModelCode, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<AssetReturnFixture>.RegisterView(e => e.ModelName, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<AssetReturnFixture>.RegisterView(e => e.FixtureType, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
            set { this.SetProperty(FixtureTypeProperty, value); }
        }
        #endregion

        #region 管控方式 ManageMode
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<AssetReturnFixture>.RegisterView(e => e.ManageMode, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set { this.SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<AssetReturnFixture>.RegisterView(e => e.UnitName, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion
        #endregion

        #region 不映射数据库的属性

        #region 未归还数量 NotReturnQty
        /// <summary>
        /// 未归还数量
        /// </summary>
        [Label("未归还数量")]
        public static readonly Property<int> NotReturnQtyProperty = P<AssetReturnFixture>.Register(e => e.NotReturnQty);

        /// <summary>
        /// 未归还数量
        /// </summary>
        public int NotReturnQty
        {
            get { return this.GetProperty(NotReturnQtyProperty); }
            set { this.SetProperty(NotReturnQtyProperty, value); }
        }
        #endregion

        #region 是否已选明细行 IsSelected
        /// <summary>
        /// 是否已选明细行
        /// </summary>
        [Label("是否已选明细行")]
        public static readonly Property<bool> IsSelectedProperty = P<AssetReturnFixture>.Register(e => e.IsSelected);

        /// <summary>
        /// 是否已选明细行
        /// </summary>
        public bool IsSelected
        {
            get { return this.GetProperty(IsSelectedProperty); }
            set { this.SetProperty(IsSelectedProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<AssetReturnFixture>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 归还单号 ReturnNo
        /// <summary>
        /// 归还单号
        /// </summary>
        [Label("归还单号")]
        public static readonly Property<string> ReturnNoProperty = P<AssetReturnFixture>.Register(e => e.ReturnNo);

        /// <summary>
        /// 归还单号
        /// </summary>
        public string ReturnNo
        {
            get { return GetProperty(ReturnNoProperty); }
            set { SetProperty(ReturnNoProperty, value); }
        }
        #endregion

        #region 保养任务 MaintainTask
        /// <summary>
        /// 保养任务Id
        /// </summary>
        [Label("保养任务")]
        public static readonly IRefIdProperty MaintainTaskIdProperty =
            P<AssetReturnFixture>.RegisterRefId(e => e.MaintainTaskId, ReferenceType.Normal);

        /// <summary>
        /// 保养任务Id
        /// </summary>
        public double? MaintainTaskId
        {
            get { return (double?)this.GetRefNullableId(MaintainTaskIdProperty); }
            set { this.SetRefNullableId(MaintainTaskIdProperty, value); }
        }

        /// <summary>
        /// 保养任务
        /// </summary>
        public static readonly RefEntityProperty<MaintainTask> MaintainTaskProperty =
            P<AssetReturnFixture>.RegisterRef(e => e.MaintainTask, MaintainTaskIdProperty);

        /// <summary>
        /// 保养任务
        /// </summary>
        public MaintainTask MaintainTask
        {
            get { return this.GetRefEntity(MaintainTaskProperty); }
            set { this.SetRefEntity(MaintainTaskProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 归还工治具清单 实体配置
    /// </summary>
    internal class AssetReturnFixtureConfig : EntityConfig<AssetReturnFixture>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_RETURN_FIX").MapAllProperties();
            Meta.Property(AssetReturnFixture.NotReturnQtyProperty).DontMapColumn();
            Meta.Property(AssetReturnFixture.IsSelectedProperty).DontMapColumn();
            Meta.Property(AssetReturnFixture.ApprovalStatusProperty).DontMapColumn();
            Meta.Property(AssetReturnFixture.ReturnNoProperty).DontMapColumn();
            Meta.Property(AssetReturnFixture.MaintainTaskIdProperty).DontMapColumn();
            Meta.Property(AssetReturnFixture.MaintainTaskProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}