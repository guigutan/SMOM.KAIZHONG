using SIE.Domain;
using SIE.Items;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using SIE.Wpf.MES.ProductRoutings.Commands;
using System;

namespace SIE.Wpf.MES.ProductRoutings
{
    /// <summary>
    /// 产品生产BOM ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序BOM")]
    public class BomViewModel : ViewModel
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BomViewModel()
        {
            Qty = 1;
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<BomViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<BomViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<BomViewModel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量"), MinValue(0.000000000001)]
        public static readonly Property<decimal> QtyProperty = P<BomViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 是否扣料 IsBuckleMaterial
        /// <summary>
        /// 是否扣料
        /// </summary>
        [Label("是否扣料")]
        public static readonly Property<bool> IsBuckleMaterialProperty = P<BomViewModel>.Register(e => e.IsBuckleMaterial);

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool IsBuckleMaterial
        {
            get { return this.GetProperty(IsBuckleMaterialProperty); }
            set { this.SetProperty(IsBuckleMaterialProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品生产BOM视图配置
    /// </summary>
    internal class BomViewModelViewModelViewConfig : WPFViewConfig<BomViewModel>
    {
        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WipProductRouting), typeof(BatchWipProductRouting));
            View.InlineEdit();
            View.UseCommands(typeof(AddBomCommand), typeof(EditBomCommand), typeof(DeleteBomCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.Item.Name).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.Qty).Show(ShowInWhere.All);
                View.Property(p => p.IsBuckleMaterial).Show(ShowInWhere.All);
            }
        }
    }
}