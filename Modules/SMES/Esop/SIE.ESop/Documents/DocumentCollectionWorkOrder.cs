using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档集与工单关系
    /// </summary>
    [Serializable, ChildEntity]
    [Label("文档集与工单关系")]
    [DisplayMember(nameof(DocumentCollectionWorkOrder.Id))]
    public class DocumentCollectionWorkOrder : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<DocumentCollectionWorkOrder>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<DocumentCollectionWorkOrder>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 文档集 Collection
        /// <summary>
        /// 文档集Id
        /// </summary>
        [Label("文档集")]
        public static readonly IRefIdProperty CollectionIdProperty =
            P<DocumentCollectionWorkOrder>.RegisterRefId(e => e.CollectionId, ReferenceType.Parent);

        /// <summary>
        /// 文档集Id
        /// </summary>
        public double CollectionId
        {
            get { return (double)this.GetRefId(CollectionIdProperty); }
            set { this.SetRefId(CollectionIdProperty, value); }
        }

        /// <summary>
        /// 文档集
        /// </summary>
        public static readonly RefEntityProperty<DocumentCollection> CollectionProperty =
            P<DocumentCollectionWorkOrder>.RegisterRef(e => e.Collection, CollectionIdProperty);

        /// <summary>
        /// 文档集
        /// </summary>
        public DocumentCollection Collection
        {
            get { return this.GetRefEntity(CollectionProperty); }
            set { this.SetRefEntity(CollectionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 文档与适用工单对应关系 配置
    /// </summary>
    internal class DocumentCollectionWorkOrderConfig : EntityConfig<DocumentCollectionWorkOrder>
    {
        /// <summary>
        /// 数据库映射配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ESOP_DOC_SET_WO").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}