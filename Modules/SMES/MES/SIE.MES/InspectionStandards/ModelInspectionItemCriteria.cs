using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.InspectionStandards
{
    /// <summary>
    /// 检验项目查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("检验项目查询实体")]
    public class ModelInspectionItemCriteria: Criteria
    {
        #region 检验项目 Name
        /// <summary>
        /// 检验项目
        /// </summary>
        [Label("检验项目")]
        public static readonly Property<string> NameProperty = P<ModelInspectionItemCriteria>.Register(e => e.Name);

        /// <summary>
        /// 检验项目
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 机型 Model
        /// <summary>
        /// 机型Id
        /// </summary>
        [Label("机型")]
        public static readonly IRefIdProperty ModelIdProperty =
            P<ModelInspectionItemCriteria>.RegisterRefId(e => e.ModelId, ReferenceType.Normal);

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
            P<ModelInspectionItemCriteria>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 机型
        /// </summary>
        public ProductModel Model
        {
            get { return this.GetRefEntity(ModelProperty); }
            set { this.SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ModelInspectionItemCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<ModelInspectionItemCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品 ProductItem
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductItemIdProperty =
            P<ModelInspectionItemCriteria>.RegisterRefId(e => e.ProductItemId, ReferenceType.Normal);

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
            P<ModelInspectionItemCriteria>.RegisterRef(e => e.ProductItem, ProductItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item ProductItem
        {
            get { return this.GetRefEntity(ProductItemProperty); }
            set { this.SetRefEntity(ProductItemProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ModelInspectionItemController>().GetModelInspectionItems(this);
        }
    }

}
