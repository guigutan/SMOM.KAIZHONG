using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线工序BOM
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺资料-产品工艺路线工序BOM")]
    public class DesignTreeRoutingProBom : DataEntity
    {
        #region 产品工艺路线设置 DesignTreeRouting
        /// <summary>
        /// 产品工艺路线设置Id
        /// </summary>
        [Label("产品工艺路线设置")]
        public static readonly IRefIdProperty DesignTreeRoutingIdProperty =
            P<DesignTreeRoutingProBom>.RegisterRefId(e => e.DesignTreeRoutingId, ReferenceType.Parent);

        /// <summary>
        /// 产品工艺路线设置Id
        /// </summary>
        public double DesignTreeRoutingId
        {
            get { return (double)this.GetRefId(DesignTreeRoutingIdProperty); }
            set { this.SetRefId(DesignTreeRoutingIdProperty, value); }
        }

        /// <summary>
        /// 产品工艺路线设置
        /// </summary>
        public static readonly RefEntityProperty<DesignTreeRouting> DesignTreeRoutingProperty =
            P<DesignTreeRoutingProBom>.RegisterRef(e => e.DesignTreeRouting, DesignTreeRoutingIdProperty);

        /// <summary>
        /// 产品工艺路线设置
        /// </summary>
        public DesignTreeRouting DesignTreeRouting
        {
            get { return this.GetRefEntity(DesignTreeRoutingProperty); }
            set { this.SetRefEntity(DesignTreeRoutingProperty, value); }
        }
        #endregion

        #region 工序 RoutingProcess
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty RoutingProcessIdProperty =
            P<DesignTreeRoutingProBom>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double RoutingProcessId
        {
            get { return (double)this.GetRefId(RoutingProcessIdProperty); }
            set { this.SetRefId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<DesignTreeRoutingDetail> RoutingProcessProperty =
            P<DesignTreeRoutingProBom>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public DesignTreeRoutingDetail RoutingProcess
        {
            get { return this.GetRefEntity(RoutingProcessProperty); }
            set { this.SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion

        #region 物料 Material
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty MaterialIdProperty =
            P<DesignTreeRoutingProBom>.RegisterRefId(e => e.MaterialId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double MaterialId
        {
            get { return (double)this.GetRefId(MaterialIdProperty); }
            set { this.SetRefId(MaterialIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> MaterialProperty =
            P<DesignTreeRoutingProBom>.RegisterRef(e => e.Material, MaterialIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Material
        {
            get { return this.GetRefEntity(MaterialProperty); }
            set { this.SetRefEntity(MaterialProperty, value); }
        }
        #endregion

        #region 物料名称 MaterialName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> MaterialNameProperty = P<DesignTreeRoutingProBom>.RegisterView(e => e.MaterialName, p => p.Material.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName
        {
            get { return this.GetProperty(MaterialNameProperty); }
        }
        #endregion

        #region 型号规格 SpecificationModel
        /// <summary>
        /// 型号规格
        /// </summary>
        [Label("型号规格")]
        public static readonly Property<string> SpecificationModelProperty = P<DesignTreeRoutingProBom>.RegisterView(e => e.SpecificationModel, p => p.Material.SpecificationModel);

        /// <summary>
        /// 型号规格
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
        }
        #endregion

        #region 单位用量 Amount
        /// <summary>
        /// 单位用量
        /// </summary>
        [Label("单位用量")]
        public static readonly Property<decimal> AmountProperty = P<DesignTreeRoutingProBom>.Register(e => e.Amount);

        /// <summary>
        /// 单位用量
        /// </summary>
        public decimal Amount
        {
            get { return this.GetProperty(AmountProperty); }
            set { this.SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<DesignTreeRoutingProBom>.RegisterView(e => e.UnitName, p => p.Material.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 工序顺序 Index
        /// <summary>
        /// 工序顺序
        /// </summary>
        [Label("工序顺序")]
        public static readonly Property<int> IndexProperty = P<DesignTreeRoutingProBom>.RegisterView(e => e.Index, p => p.RoutingProcess.Index);

        /// <summary>
        /// 工序顺序
        /// </summary>
        public int Index
        {
            get { return this.GetProperty(IndexProperty); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<DesignTreeRoutingProBom>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 不映射数据库
        #region 表头产品Id ProductId
        /// <summary>
        /// 表头产品Id
        /// </summary>
        [Label("表头产品Id")]
        public static readonly Property<double> ProductIdProperty = P<DesignTreeRoutingProBom>.Register(e => e.ProductId);

        /// <summary>
        /// 表头产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeRoutingProBomConfig : EntityConfig<DesignTreeRoutingProBom>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_ROUBOM").MapAllProperties();
            Meta.Property(DesignTreeRoutingProBom.ProductIdProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
