using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.AssetRequisitions
{
    /// <summary>
    /// 领用申请工治具清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("领用申请工治具清单")]
    public partial class AssetRequisitionFixture : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<AssetRequisitionFixture>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 申请数量 Qty
        /// <summary>
        /// 申请数量
        /// </summary>
        [Label("申请数量")]
        [MinValue(1)]
        public static readonly Property<int> QtyProperty = P<AssetRequisitionFixture>.Register(e => e.Qty);

        /// <summary>
        /// 申请数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 预估价值 EstimatedAmount
        /// <summary>
        /// 预估价值
        /// </summary>
        [Label("预估价值")]
        public static readonly Property<decimal?> EstimatedAmountProperty = P<AssetRequisitionFixture>.Register(e => e.EstimatedAmount);

        /// <summary>
        /// 预估价值
        /// </summary>
        public decimal? EstimatedAmount
        {
            get { return GetProperty(EstimatedAmountProperty); }
            set { SetProperty(EstimatedAmountProperty, value); }
        }
        #endregion

        #region 发放数量 IssuedQty
        /// <summary>
        /// 发放数量
        /// </summary>
        [Label("发放数量")]
        public static readonly Property<int> IssuedQtyProperty = P<AssetRequisitionFixture>.Register(e => e.IssuedQty);

        /// <summary>
        /// 发放数量
        /// </summary>
        public int IssuedQty
        {
            get { return GetProperty(IssuedQtyProperty); }
            set { SetProperty(IssuedQtyProperty, value); }
        }
        #endregion

        #region 实物归还数量 ReturnQty
        /// <summary>
        /// 实物归还数量
        /// </summary>
        [Label("实物归还数量")]
        public static readonly Property<int> ReturnQtyProperty = P<AssetRequisitionFixture>.Register(e => e.ReturnQty);

        /// <summary>
        /// 实物归还数量
        /// </summary>
        public int ReturnQty
        {
            get { return GetProperty(ReturnQtyProperty); }
            set { SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 无实物归还数量 NoGoodsReturnQty
        /// <summary>
        /// 无实物归还数量
        /// </summary>
        [Label("无实物归还数量")]
        public static readonly Property<int> NoGoodsReturnQtyProperty = P<AssetRequisitionFixture>.Register(e => e.NoGoodsReturnQty);

        /// <summary>
        /// 无实物归还数量
        /// </summary>
        public int NoGoodsReturnQty
        {
            get { return GetProperty(NoGoodsReturnQtyProperty); }
            set { SetProperty(NoGoodsReturnQtyProperty, value); }
        }
        #endregion

        #region 拣货数量 PickedQty
        /// <summary>
        /// 拣货数量
        /// </summary>
        [Label("拣货数量")]
        public static readonly Property<int> PickedQtyProperty = P<AssetRequisitionFixture>.Register(e => e.PickedQty);

        /// <summary>
        /// 拣货数量
        /// </summary>
        public int PickedQty
        {
            get { return GetProperty(PickedQtyProperty); }
            set { SetProperty(PickedQtyProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<AssetRequisitionFixture>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<AssetRequisitionFixture>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 资产领用 AssetRequisition
        /// <summary>
        /// 资产领用Id
        /// </summary>
        [Label("资产领用")]
        public static readonly IRefIdProperty AssetRequisitionIdProperty = P<AssetRequisitionFixture>.RegisterRefId(e => e.AssetRequisitionId, ReferenceType.Parent);

        /// <summary>
        /// 资产领用Id
        /// </summary>
        public double AssetRequisitionId
        {
            get { return (double)GetRefId(AssetRequisitionIdProperty); }
            set { SetRefId(AssetRequisitionIdProperty, value); }
        }

        /// <summary>
        /// 资产领用
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisition> AssetRequisitionProperty
            = P<AssetRequisitionFixture>.RegisterRef(e => e.AssetRequisition, AssetRequisitionIdProperty);

        /// <summary>
        /// 资产领用
        /// </summary>
        public AssetRequisition AssetRequisition
        {
            get { return GetRefEntity(AssetRequisitionProperty); }
            set { SetRefEntity(AssetRequisitionProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<AssetRequisitionFixture>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<AssetRequisitionFixture>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<AssetRequisitionFixture>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<AssetRequisitionFixture>.RegisterView(e => e.FixtureType, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
        }
        #endregion

        #region 管控方式 ManageMode
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ManageMode?> ManageModeProperty = P<AssetRequisitionFixture>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode? ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<AssetRequisitionFixture>.RegisterView(e => e.UnitName, p => p.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 发放仓库 Warehouse
        /// <summary>
        /// 发放仓库Id
        /// </summary>
        [Label("发放仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetRequisitionFixture>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发放仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发放仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetRequisitionFixture>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发放仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库存可用数 StoreUsableQty
        /// <summary>
        /// 库存可用数
        /// </summary>
        [Label("库存可用数")]
        public static readonly Property<string> StoreUsableQtyProperty = P<AssetRequisitionFixture>.Register(e => e.StoreUsableQty);

        /// <summary>
        /// 库存可用数
        /// </summary>
        public string StoreUsableQty
        {
            get { return this.GetProperty(StoreUsableQtyProperty); }
            set { this.SetProperty(StoreUsableQtyProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 领用申请工治具清单 实体配置
    /// </summary>
    internal class AssetRequisitionFixtureConfig : EntityConfig<AssetRequisitionFixture>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_REQ_FIX").MapAllProperties();
            Meta.Property(AssetRequisitionFixture.StoreUsableQtyProperty).DontMapColumn();
            Meta.Property(AssetRequisitionFixture.WarehouseIdProperty).DontMapColumn();
            Meta.Property(AssetRequisitionFixture.WarehouseProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }

        /// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
        {
            //rules.AddRule(new HandlerRule()
            //{
            //    Handler = (o, e) =>
            //    {
            //        var para = o.CastTo<AssetRequisitionFixture>();
            //        StringBuilder sb = new StringBuilder();

            //        int usableQty = para.StoreUsableQty.IsNullOrEmpty() ? 0 : int.Parse(para.StoreUsableQty);
            //        if (para.Qty > usableQty)
            //        {
            //            sb.AppendLine("【申请数量】不能大于【可用库存数】！".L10N());
            //        }

            //        e.BrokenDescription = sb.ToString();
            //    }
            //}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}