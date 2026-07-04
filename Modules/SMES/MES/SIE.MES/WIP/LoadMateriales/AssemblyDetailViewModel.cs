using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.LoadMateriales
{
    /// <summary>
    /// 装配明细VM
    /// </summary>
    [RootEntity, Serializable]
    public class AssemblyDetailViewModel : ViewModel
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary> 
        public static readonly IRefIdProperty ItemIdProperty =
            P<AssemblyDetailViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<AssemblyDetailViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 需求数量 DemandQty
        /// <summary>
        /// 需求数量
        /// </summary>
        public static readonly Property<decimal> DemandQtyProperty = P<AssemblyDetailViewModel>.Register(e => e.DemandQty);

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal DemandQty
        {
            get { return this.GetProperty(DemandQtyProperty); }
            set { this.SetProperty(DemandQtyProperty, value); }
        }
        #endregion

        #region 已扫数量 Qty
        /// <summary>
        /// 已扫数量
        /// </summary>
        public static readonly Property<decimal> QtyProperty = P<AssemblyDetailViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 已扫数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 物料标签 ItemLabel
        /// <summary>
        /// 物料标签
        /// </summary>
        public static readonly Property<string> ItemLabelProperty = P<AssemblyDetailViewModel>.Register(e => e.ItemLabel);

        /// <summary>
        /// 物料标签
        /// </summary>
        public string ItemLabel
        {
            get { return this.GetProperty(ItemLabelProperty); }
            set { this.SetProperty(ItemLabelProperty, value); }
        }
        #endregion

        #region 物料标签剩余数量 RemainQty
        /// <summary>
        /// 物料标签剩余数量
        /// </summary>
        [Label("物料标签剩余数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<AssemblyDetailViewModel>.Register(e => e.RemainQty);

        /// <summary>
        /// 物料标签剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 替代料 AltItemList
        /// <summary>
        /// 替代料
        /// </summary> 
        public static readonly ListProperty<EntityList<AltItemViewModel>> AltItemListProperty = P<AssemblyDetailViewModel>.RegisterList(e => e.AltItemList);

        /// <summary>
        /// 替代料
        /// </summary>
        public EntityList<AltItemViewModel> AltItemList
        {
            get { return this.GetLazyList(AltItemListProperty); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<AssemblyDetailViewModel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 替代组合分组 AlterGroup
        /// <summary>
        /// 替代组合分组
        /// </summary>
        [Label("替代组合分组")]
        public static readonly Property<string> AlterGroupProperty = P<AssemblyDetailViewModel>.Register(e => e.AlterGroup);

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get { return this.GetProperty(AlterGroupProperty); }
            set { this.SetProperty(AlterGroupProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 替代料
    /// </summary>
    [ChildEntity]
    [Label("替代料")]
    public class AltItemViewModel : ViewModel
    {
        #region 装配清单 AssemblyDetail
        /// <summary>
        /// 装配清单Id
        /// </summary> 
        public static readonly IRefIdProperty AssemblyDetailIdProperty =
            P<AltItemViewModel>.RegisterRefId(e => e.AssemblyDetailId, ReferenceType.Parent);

        /// <summary>
        /// 装配清单Id
        /// </summary>
        public double AssemblyDetailId
        {
            get { return (double)this.GetRefId(AssemblyDetailIdProperty); }
            set { this.SetRefId(AssemblyDetailIdProperty, value); }
        }

        /// <summary>
        /// 装配清单
        /// </summary>
        public static readonly RefEntityProperty<AssemblyDetailViewModel> AssemblyDetailProperty =
            P<AltItemViewModel>.RegisterRef(e => e.AssemblyDetail, AssemblyDetailIdProperty);

        /// <summary>
        /// 装配清单
        /// </summary>
        public AssemblyDetailViewModel AssemblyDetail
        {
            get { return this.GetRefEntity(AssemblyDetailProperty); }
            set { this.SetRefEntity(AssemblyDetailProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary> 
        public static readonly IRefIdProperty ItemIdProperty =
            P<AltItemViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<AltItemViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<AltItemViewModel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 替代组合分组 AlterGroup
        /// <summary>
        /// 替代组合分组
        /// </summary>
        [Label("替代组合分组")]
        public static readonly Property<string> AlterGroupProperty = P<AltItemViewModel>.Register(e => e.AlterGroup);

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get { return this.GetProperty(AlterGroupProperty); }
            set { this.SetProperty(AlterGroupProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 装配明细ViewModel视图配置
    /// </summary>
    [CompiledPropertyDeclarer]

    public class AssemblyDetailViewModelViewConfig : WPFViewConfig<AssemblyDetailViewModel>
    {
        /// <summary>
        /// 批次上料装配清单视图
        /// </summary>
        public const string BatchAssemblyDetailView = "BatchAssemblyDetailView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("装配清单");
            View.DeclareExtendViewGroup(BatchAssemblyDetailView);
            if (ViewGroup == BatchAssemblyDetailView)
                ConfigBatchAssemblyDetailView();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.AssignAuthorize(typeof(AssemblyViewModel));
            //View.AddBehavior(typeof(AssemblyDetailViewBehavior));
            using (View.OrderProperties())
            {
                View.Property(p => p.Item.Code).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.Item.Name).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.ItemExtProp).Show(ShowInWhere.All);
                View.Property(p => p.AlterGroup).Show(ShowInWhere.All);
                View.Property(p => p.DemandQty).HasLabel("需求数量").Show(ShowInWhere.All);
                View.Property(p => p.Qty).HasLabel("已扫数量").Show(ShowInWhere.All);
                View.Property(p => p.ItemLabel).HasLabel("物料标签").Show(ShowInWhere.All);
                View.Property(p => p.RemainQty).HasLabel("物料标签剩余数量").Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.AltItemList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //View.AssignAuthorize(typeof(AssemblyViewModel));
            //View.AddBehavior(typeof(AssemblyDetailViewBehavior));
            using (View.OrderProperties())
            {
                View.Property(p => p.Item.Code).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.Item.Name).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.DemandQty).HasLabel("需求数量").Show(ShowInWhere.All);
                View.Property(p => p.Qty).HasLabel("已扫数量").Show(ShowInWhere.All);
                View.Property(p => p.ItemLabel).HasLabel("物料标签").Show(ShowInWhere.All);
                View.Property(p => p.RemainQty).HasLabel("物料标签剩余数量").Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.AltItemList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 批次上料装配清单视图
        /// </summary>
        void ConfigBatchAssemblyDetailView()
        {
            //View.AssignAuthorize(typeof(BatchAssemblyViewModel));
            //View.AddBehavior(typeof(AssemblyDetailViewBehavior));
            using (View.OrderProperties())
            {
                View.Property(p => p.Item.Code).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.Item.Name).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.ItemExtProp).Show(ShowInWhere.All);
                View.Property(p => p.AlterGroup).Show(ShowInWhere.All);
                View.Property(p => p.DemandQty).HasLabel("需求数量").Show(ShowInWhere.All);                
                View.Property(p => p.Qty).HasLabel("批次需求数量").Show(ShowInWhere.All);
                View.Property(p => p.ItemLabel).HasLabel("物料标签").Show(ShowInWhere.All);
                View.Property(p => p.RemainQty).HasLabel("物料标签剩余数量").Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.AltItemList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}