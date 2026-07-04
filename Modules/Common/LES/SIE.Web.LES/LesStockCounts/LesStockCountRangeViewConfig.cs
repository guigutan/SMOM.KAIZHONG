using SIE.Items;
using SIE.LES.LesStockCounts;
using SIE.Warehouses;

namespace SIE.Web.LES.LesStockCounts
{
    /// <summary>
    /// 盘点范围视图配置
    /// </summary>
    public class LesStockCountRangeViewConfig : WebViewConfig<LesStockCountRange>
    {
        /// <summary>
        /// 只读视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 单据状态等于创建可编辑
        /// </summary>
        private const string STATECREATECANEDIT = "单据状态等于创建可编辑";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { ReadonlyView });
            View.AssignAuthorize(typeof(LesStockCount));
            if (ViewGroup == ReadonlyView)
                ConfigReadonlyView();
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-8.5, -1.5);
            using (View.DeclareGroup("基本筛选", 8))
            {
                View.Property(p => p.Warehouses).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Warehouse).FullName;
                    p.XType = "MultiLesWhComboPopup";
                    p.LinkField = Warehouse.CodeProperty.Name;
                    p.ValueField = Warehouse.CodeProperty.Name;
                    p.DisplayField = Warehouse.CodeProperty.Name;
                    p.Editable = false;
                    p.Separator = ";";
                }).ShowInDetail(columnSpan: 4).Readonly(p => p.State != LesCountState.Create).UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; });
                View.Property(p => p.ItemCategorys).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(ItemCategory).FullName;
                    p.XType = "MultiItemCateComboPopup";
                    p.LinkField = ItemCategory.CodeProperty.Name;
                    p.ValueField = ItemCategory.CodeProperty.Name;
                    p.DisplayField = ItemCategory.CodeProperty.Name;
                    p.Editable = false;
                    p.Separator = ";";
                }).ShowInDetail(columnSpan: 4).Readonly(p => p.State != LesCountState.Create).UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; });
                View.Property(p => p.Items).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Item).FullName;
                    p.LinkField = Item.CodeProperty.Name;
                    p.ValueField = Item.CodeProperty.Name;
                    p.DisplayField = Item.CodeProperty.Name;
                    p.XType = "MultiItemComboPopup";
                    p.Editable = false;
                    p.Separator = ";";
                }).ShowInDetail(columnSpan: 4).Readonly(p => p.State != LesCountState.Create).UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; }); 
                View.Property(p => p.ConsumeMode).ShowInDetail(columnSpan: 4).Readonly(p => p.State != LesCountState.Create).UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; }); 
            }

            using (View.DeclareGroup("盘点要求", 8))
            {
                View.Property(p => p.CountDimension).ShowInDetail(columnSpan: 4, width: "50%").Readonly(p => p.State != LesCountState.Create).DefaultValue(CountDimension.Lot).UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; });
                View.Property(p => p.IsBlindCount).ShowInDetail(columnSpan: 4).Readonly(p => p.State != LesCountState.Create).UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; });
                View.Property(p => p.IsDynamicOnhand).ShowInDetail(columnSpan: 4).Readonly(p => p.State != LesCountState.Create).UseListSetting(e => { e.HelpInfo = STATECREATECANEDIT; });
            }
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.ClearCommands();
            using (View.DeclareGroup("基本筛选", 8))
            {
                View.Property(p => p.Warehouses).ShowInDetail(columnSpan: 4).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ItemCategorys).ShowInDetail(columnSpan: 4).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Items).ShowInDetail(columnSpan: 4).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ConsumeMode).ShowInDetail(columnSpan: 4).Readonly().Show(ShowInWhere.All);
            }
            using (View.DeclareGroup("盘点要求", 8))
            {
                View.Property(p => p.CountDimension).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.IsBlindCount).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.IsDynamicOnhand).Readonly().Show(ShowInWhere.All);
            }
        }
    }
}
