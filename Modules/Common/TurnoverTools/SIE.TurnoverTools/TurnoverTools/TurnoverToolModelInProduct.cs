using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 产品容量
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品容量")]
    public partial class TurnoverToolModelInProduct : DataEntity
    {
        #region 容量 Capacity
        /// <summary>
        /// 容量
        /// </summary>
        [MinValue(1)]
        [Label("容量")]
        public static readonly Property<int> CapacityProperty = P<TurnoverToolModelInProduct>.Register(e => e.Capacity);

        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity
        {
            get { return GetProperty(CapacityProperty); }
            set { SetProperty(CapacityProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        public static readonly IRefIdProperty ProductIdProperty = P<TurnoverToolModelInProduct>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<TurnoverToolModelInProduct>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 周转工具型号 Model
        /// <summary>
        /// 周转工具型号Id
        /// </summary>
        public static readonly IRefIdProperty ModelIdProperty = P<TurnoverToolModelInProduct>.RegisterRefId(e => e.ModelId, ReferenceType.Parent);

        /// <summary>
        /// 周转工具型号Id
        /// </summary>
        public double ModelId
        {
            get { return (double)GetRefId(ModelIdProperty); }
            set { SetRefId(ModelIdProperty, value); }
        }

        /// <summary>
        /// 周转工具型号
        /// </summary>
        public static readonly RefEntityProperty<TurnoverToolModel> ModelProperty = P<TurnoverToolModelInProduct>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 周转工具型号
        /// </summary>
        public TurnoverToolModel Model
        {
            get { return GetRefEntity(ModelProperty); }
            set { SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 物料编码
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ProductCodeProperty = P<TurnoverToolModelInProduct>.RegisterView(e => e.ProductCode, p => p.Product.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ProductCode
        {

            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 物料名称 ProductName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ProductNameProperty = P<TurnoverToolModelInProduct>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName
        {

            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 产品容量 实体配置
    /// </summary>
    internal class TurnoverToolModelInProductConfig : EntityConfig<TurnoverToolModelInProduct>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_TURN_TM_PROD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}