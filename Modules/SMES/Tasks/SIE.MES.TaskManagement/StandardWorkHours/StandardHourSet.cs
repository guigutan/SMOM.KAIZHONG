using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
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
    /// 产品标准工时维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StandardHourSetCriteria))]
    [Label("产品标准工时维护")]
    public class StandardHourSet : DataEntity
    {
        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<StandardHourSet>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

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
            P<StandardHourSet>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

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
            P<StandardHourSet>.RegisterRefId(e => e.ProductModelId, ReferenceType.Normal);

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
            P<StandardHourSet>.RegisterRef(e => e.ProductModel, ProductModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel ProductModel
        {
            get { return this.GetRefEntity(ProductModelProperty); }
            set { this.SetRefEntity(ProductModelProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<StandardHourSet>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<StandardHourSet>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<StandardHourSet>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<StandardHourSet>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序标准工时(分) StandardMin
        /// <summary>
        /// 工序标准工时(分)
        /// </summary>
        [Label("工序标准工时(分)")]
        public static readonly Property<decimal> StandardMinProperty = P<StandardHourSet>.Register(e => e.StandardMin);

        /// <summary>
        /// 工序标准工时(分)
        /// </summary>
        public decimal StandardMin
        {
            get { return this.GetProperty(StandardMinProperty); }
            set { this.SetProperty(StandardMinProperty, value); }
        }
        #endregion

        #region 附加合计工时(分) AttachMin
        /// <summary>
        /// 附加合计工时(分)
        /// </summary>
        [Label("附加合计工时(分)")]
        public static readonly Property<decimal?> AttachMinProperty = P<StandardHourSet>.Register(e => e.AttachMin);

        /// <summary>
        /// 附加合计工时(分)
        /// </summary>
        public decimal? AttachMin
        {
            get { return this.GetProperty(AttachMinProperty); }
            set { this.SetProperty(AttachMinProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<StandardHourSet>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<StandardHourSet>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 型号规格 SpecificationModel
        /// <summary>
        /// 型号规格
        /// </summary>
        [Label("型号规格")]
        public static readonly Property<string> SpecificationModelProperty = P<StandardHourSet>.RegisterView(e => e.SpecificationModel, p => p.Product.SpecificationModel);

        /// <summary>
        /// 型号规格
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class StandardHourSetConfig : EntityConfig<StandardHourSet>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_STANDARD_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
