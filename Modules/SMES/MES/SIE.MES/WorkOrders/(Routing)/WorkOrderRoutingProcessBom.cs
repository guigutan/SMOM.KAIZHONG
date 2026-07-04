using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单工序清单BOM配置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单工序清单BOM配置")]
    public partial class WorkOrderRoutingProcessBom : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<WorkOrderRoutingProcessBom>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<WorkOrderRoutingProcessBom>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 扩展属性名称 ItemExtPropName
        /// <summary>
        /// 扩展属性名称
        /// </summary>
        [Label("扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderRoutingProcessBom>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtProp
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrderRoutingProcessBom>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 工单工序清单与BOM关系 Process
        /// <summary>
        /// 工单工序清单与BOM关系Id
        /// </summary>
        [Label("工单工序清单")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WorkOrderRoutingProcessBom>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工单工序清单与BOM关系Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工单工序清单与BOM关系
        /// </summary>
        [Label("工单工序清单")]
        public static readonly RefEntityProperty<WorkOrderRoutingProcess> ProcessProperty = P<WorkOrderRoutingProcessBom>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工单工序清单与BOM关系
        /// </summary>
        public WorkOrderRoutingProcess Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单工序清单BOM配置 实体配置
    /// </summary>
    internal class WorkOrderRoutingProcessBomConfig : EntityConfig<WorkOrderRoutingProcessBom>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_RT_PROC_BOM_CFG").MapAllProperties();
            Meta.Property(WorkOrderRoutingProcessBom.ProcessIdProperty).ColumnMeta.IgnoreFK();
            Meta.EnablePhantoms();
            Meta.Property(WorkOrderRoutingProcessBom.ProcessIdProperty).ColumnMeta.HasIndex();
        }
    }
}