using SIE.Domain;
using SIE.Items;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Engrave
{
    /// <summary>
    ///刻码标签
    /// </summary>
    [RootEntity, Serializable]
    [Label("刻码数据")]
    [ConditionQueryType(typeof(EngraveLabelCriteria))]
    public partial class EngraveLabel : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EngraveLabel()
        {
        }

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Required]
        [Label("标签号")]
        public static readonly Property<string> BatchNoProperty = P<EngraveLabel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<EngraveLabel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<EngraveLabel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<EngraveLabel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<EngraveLabel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<EngraveLabel>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

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
            P<EngraveLabel>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<EngraveLabel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region SN明细列表 EngraveSnList
        /// <summary>
        /// SN明细列表
        /// </summary>
        [Label("SN明细列表")]
        public static readonly ListProperty<EntityList<EngraveSn>> EngraveSnListProperty = P<EngraveLabel>.RegisterList(e => e.EngraveSnList);

        /// <summary>
        /// SN明细列表
        /// </summary>
        public EntityList<EngraveSn> EngraveSnList
        {
            get { return this.GetLazyList(EngraveSnListProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class EngraveLabelEntityConfig : EntityConfig<EngraveLabel>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("ENGRAVE_LABEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
