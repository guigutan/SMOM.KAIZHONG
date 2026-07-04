using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.ItemLine
{
    /// <summary>
    /// 产品与产线的关系
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductLineCriterial))]
    [Label("产品与产线的关系")]
    public partial class ProductLine :DataEntity
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ProductLine>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ProductLine>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工序名称 Process
        /// <summary>
        /// 工序名称Id
        /// </summary>
        [Label("工序名称")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProductLine>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序名称Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProductLine>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序名称
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产线/机台编码 WipResource
        /// <summary>
        /// 产线/机台编码Id
        /// </summary>
        [Label("产线/机台编码")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<ProductLine>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线/机台编码Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)this.GetRefId(WipResourceIdProperty); }
            set { this.SetRefId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<ProductLine>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        //#region 状态 State
        ///// <summary>
        ///// 状态
        ///// </summary>
        //[Label("状态")]
        //public static readonly Property<State> StateProperty = P<ProductLine>.Register(e => e.State);

        ///// <summary>
        ///// 状态
        ///// </summary>
        //public State State
        //{
        //    get { return this.GetProperty(StateProperty); }
        //    set { this.SetProperty(StateProperty, value); }
        //}
        //#endregion

        #region 视图属性

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<ProductLine>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<ProductLine>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProductLine>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ProductLine>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }

        #endregion

        #region 产线/机台编码 LineCode
        /// <summary>
        /// 产线/机台编码
        /// </summary>
        [Label("产线/机台编码")]
        public static readonly Property<string> LineCodeProperty = P<ProductLine>.RegisterView(e => e.LineCode, p => p.WipResource.Code);

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public string LineCode
        {
            get { return this.GetProperty(LineCodeProperty); }
        }
        #endregion


        #region 产线/机台名称 LineName
        /// <summary>
        /// 产线/机台名称
        /// </summary>
        [Label("产线/机台名称")]
        public static readonly Property<string> LineNameProperty = P<ProductLine>.RegisterView(e => e.LineName, p => p.WipResource.Name);

        /// <summary>
        /// 产线/机台名称
        /// </summary>
        public string LineName
        {
            get { return this.GetProperty(LineNameProperty); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 产品与产线的关系 实体配置
    /// </summary>
    internal class ProductLineConfig : EntityConfig<ProductLine>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    ProductLine.ItemIdProperty,
                    ProductLine.ProcessIdProperty,
                    ProductLine.WipResourceIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRODUCT_LINE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
