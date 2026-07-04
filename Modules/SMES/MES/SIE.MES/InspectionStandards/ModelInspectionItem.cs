using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.InspectionStandards
{
    /// <summary>
    /// 检验项目
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ModelInspectionItemCriteria))]
    [Label("检验项目")]
    [DisplayMember(nameof(Name))]
    public partial class ModelInspectionItem : InspectionItemBase
    {
        #region 机型 Model
        /// <summary>
        /// 机型Id
        /// </summary>
        [Label("机型")]
        public static readonly IRefIdProperty ModelIdProperty =
            P<ModelInspectionItem>.RegisterRefId(e => e.ModelId, ReferenceType.Normal);

        /// <summary>
        /// 机型Id
        /// </summary>
        public double? ModelId
        {
            get { return (double?)this.GetRefNullableId(ModelIdProperty); }
            set { this.SetRefNullableId(ModelIdProperty, value); }
        }

        /// <summary>
        /// 机型
        /// </summary>
        public static readonly RefEntityProperty<ProductModel> ModelProperty =
            P<ModelInspectionItem>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 机型
        /// </summary>
        public ProductModel Model
        {
            get { return this.GetRefEntity(ModelProperty); }
            set { this.SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 产品 ProductItem
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductItemIdProperty =
            P<ModelInspectionItem>.RegisterRefId(e => e.ProductItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductItemId
        {
            get { return (double?)this.GetRefNullableId(ProductItemIdProperty); }
            set { this.SetRefNullableId(ProductItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductItemProperty =
            P<ModelInspectionItem>.RegisterRef(e => e.ProductItem, ProductItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item ProductItem
        {
            get { return this.GetRefEntity(ProductItemProperty); }
            set { this.SetRefEntity(ProductItemProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ModelInspectionItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ModelInspectionItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 排序 OrderNum
        /// <summary>
        /// 排序
        /// </summary>
        [Label("排序")]
        public static readonly Property<int?> OrderNumProperty = P<ModelInspectionItem>.Register(e => e.OrderNum);

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderNum
        {
            get { return this.GetProperty(OrderNumProperty); }
            set { this.SetProperty(OrderNumProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<ModelInspectionItem>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 机型名称 ModelName
        /// <summary>
        /// 机型名称
        /// </summary>
        [Label("机型")]
        public static readonly Property<string> ModelNameProperty = P<ModelInspectionItem>.RegisterView(e => e.ModelName, p => p.Model.Name);

        /// <summary>
        /// 机型名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 产品名称 ProductItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductItemNameProperty = P<ModelInspectionItem>.RegisterView(e => e.ProductItemName, p => p.ProductItem.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductItemName
        {
            get { return this.GetProperty(ProductItemNameProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<ModelInspectionItem>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 机型检验项目 实体配置
    /// </summary>
    internal class ModelInspectionItemConfig : EntityConfig<ModelInspectionItem>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_INSPECTION_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}