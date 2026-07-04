using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StandardWorkHours
{
    /// <summary>
    /// 产品标准工时维护-查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产品标准工时维护-查询实体")]
    public class StandardHourSetCriteria : Criteria
    {
        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<StandardHourSetCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<StandardHourSetCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 产品机型 ProductModel
        /// <summary>
        /// 产品机型Id
        /// </summary>
        [Label("产品机型")]
        public static readonly IRefIdProperty ProductModelIdProperty =
            P<StandardHourSetCriteria>.RegisterRefId(e => e.ProductModelId, ReferenceType.Normal);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double? ProductModelId
        {
            get { return (double?)this.GetRefNullableId(ProductModelIdProperty); }
            set { this.SetRefNullableId(ProductModelIdProperty, value); }
        }

        /// <summary>
        /// 产品机型
        /// </summary>
        public static readonly RefEntityProperty<ProductModel> ProductModelProperty =
            P<StandardHourSetCriteria>.RegisterRef(e => e.ProductModel, ProductModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel ProductModel
        {
            get { return this.GetRefEntity(ProductModelProperty); }
            set { this.SetRefEntity(ProductModelProperty, value); }
        }
        #endregion

        #region 产品编码 Product
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<StandardHourSetCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品编码Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<StandardHourSetCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品编码
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<StandardHourSetCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<StandardHourSetCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<StandardHourSetCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<StandardHourSetCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StandardHourSetController>().QueryStandardHourSets(this);
        }
    }
}
