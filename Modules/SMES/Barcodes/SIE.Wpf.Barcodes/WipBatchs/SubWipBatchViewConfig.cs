using SIE.Barcodes.WipBatchs;
using SIE.Wpf.Barcodes.WipBatchs.Commands;

namespace SIE.Wpf.Barcodes.WipBatchs
{
    /// <summary>
    /// 子生产批次视图配置
    /// </summary>
    public class SubWipBatchViewConfig : WPFViewConfig<SubWipBatch>
    {
        /// <summary>
        /// 子批次条码视图
        /// </summary>
        public static readonly string SubBatchView = "SubBatchView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SubBatchView);
            if (ViewGroup == SubBatchView)
                ConfigSubBatchView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WipBatchNo).HasLabel("批次编码").Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.BatchNo).HasLabel("子批次编码").Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.Qty).HasLabel("批次数量").Show(ShowInWhere.All);
                View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).HasLabel("满批数量").Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).HasLabel("生成时间").Show(ShowInWhere.All);
                View.Property(p => p.PrintTimes).Show(ShowInWhere.All);
                View.Property(p => p.PrintByName).HasLabel("打印人").Show(ShowInWhere.All);
                View.Property(p => p.PrintedState).HasLabel("打印状态").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsScraped).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.BatchList).HasLabel("子批次信息").Visible(false);
            }
        }

        /// <summary>
        /// 子批次条码视图
        /// </summary>
        void ConfigSubBatchView()
        {
            View.UseDefaultBehaviors();
            View.ClearCommands().UseCommands(typeof(ReprintSubWipBatchCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.WipBatchNo).HasLabel("批次编码").Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.BatchNo).HasLabel("子批次编码").Show(ShowInWhere.All).FixColumn(MetaModel.View.ColumnFixedStyle.Left);
                View.Property(p => p.Qty).HasLabel("批次数量").Show(ShowInWhere.All);
                View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).HasLabel("满批数量").Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).HasLabel("生成时间").Show(ShowInWhere.All);
                View.Property(p => p.PrintTimes).Show(ShowInWhere.All);
                View.Property(p => p.PrintByName).HasLabel("打印人").Show(ShowInWhere.All);
                View.Property(p => p.PrintedState).HasLabel("打印状态").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsScraped).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.BatchList).HasLabel("子批次信息").Visible(false);
            }
        }
    }
}