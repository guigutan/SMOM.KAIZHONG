using SIE.Wpf.Common;
using SIE.Wpf.MES.BatchWIP.Repairs;
using SIE.Wpf.MES.BatchWIP.Repairs.Commands;
using SIE.Wpf.MES.WIP.Repairs.Commands;
using System;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    /// 装配信息视图配置
    /// </summary>
    internal class ProductAssemblyDetailViewModelViewConfig : WPFViewConfig<ProductAssemblyDetailViewModel>
    {
        /// <summary>
        /// 维修信息
        /// </summary>
        public const string RepairView = "RepairView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(RepairViewModel));
            View.DeclareExtendViewGroup(RepairView);
            View.ClearCommands();

            if (ViewGroup == RepairView)
            {
                RepairListView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        void RepairListView()
        {
            View.UseCommands(typeof(ChangeItemCommand));

            using (View.OrderProperties())
            {
                View.Property(p => p.KeyItem.Process.Process.Name).HasLabel("工序").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.KeyItem.SourceCode).HasLabel("原标签").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.ChangeBarcode).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.KeyItem.Item.Code).HasLabel("物料编码").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.KeyItem.Item.Name).HasLabel("物料名称").Show(ShowInWhere.List).Readonly();
                View.Property(p => p.KeyItem.Qty).HasLabel("用料量").ShowInList(gridWidth: 80).Readonly();
                View.Property(p => p.TotalChangeQty).ShowInList(gridWidth: 80).Readonly();
                View.Property(p => p.IsChangeSn).ShowInList(gridWidth: 60).Readonly();
                View.ChildrenProperty(p => p.ChangeItemViewModelList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(columnCount: 4);

            using (View.OrderProperties())
            {
                using (View.DeclareGroup("原标签信息".L10N()))
                {
                    View.Property(p => p.KeyItem.SourceCode).HasLabel("原标签").ShowInDetail(columnSpan: 2, height: 0).Readonly();
                    View.Property(p => p.KeyItem.Qty).HasLabel("用料量").ShowInDetail(columnSpan: 2, height: 0).Readonly();

                    View.Property(p => p.KeyItem.Item.Code).HasLabel("物料编码").ShowInDetail(columnSpan: 2, height: 0).Readonly();
                    View.Property(p => p.KeyItem.Item.Name).HasLabel("物料名称").ShowInDetail(columnSpan: 2, height: 0).Readonly();

                    View.Property(p => p.TotalChangeQty).ShowInDetail(columnSpan: 2, height: 0).Readonly();
                    View.Property(p => p.HandleMethod).ShowInDetail(columnSpan: 2, height: 0);
                }

                using (View.DeclareGroup("请输入换料后标签".L10N()))
                {
                    View.Property(p => p.Barcode).HasLabel("换料条码").UseBarcodeEditor().ShowInDetail(columnSpan: 3, height: 0);
                    View.Property(p => p.ChangeQty).UseChangeQtyEditor().Show(ShowInWhere.Detail);
                }

                View.ChildrenProperty(p => p.ChangeItemViewModelList).Show(ChildShowInWhere.Detail);
            }
        }
    }
}
