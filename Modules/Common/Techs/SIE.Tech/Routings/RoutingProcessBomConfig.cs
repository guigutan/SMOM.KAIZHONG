using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序BOM配置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序BOM配置")]
    public partial class RoutingProcessBomConfig : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<RoutingProcessBomConfig>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<RoutingProcessBomConfig>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工序清单 RoutingProcess
        /// <summary>
        /// 工序清单Id
        /// </summary>
        public static readonly IRefIdProperty RoutingProcessIdProperty = P<RoutingProcessBomConfig>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序清单Id
        /// </summary>
        public double RoutingProcessId
        {
            get { return (double)GetRefId(RoutingProcessIdProperty); }
            set { SetRefId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序清单
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty = P<RoutingProcessBomConfig>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工序清单
        /// </summary>
        public RoutingProcess RoutingProcess
        {
            get { return GetRefEntity(RoutingProcessProperty); }
            set { SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展值")]
        public static readonly Property<string> ItemExtPropProperty = P<RoutingProcessBomConfig>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion


        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<RoutingProcessBomConfig>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 工序BOM配置 实体配置
    /// </summary>
    internal class RoutingProcessBomConfigConfig : EntityConfig<RoutingProcessBomConfig>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_PROC_BOM").MapAllProperties();
            Meta.Property(RoutingProcessBomConfig.RoutingProcessIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}