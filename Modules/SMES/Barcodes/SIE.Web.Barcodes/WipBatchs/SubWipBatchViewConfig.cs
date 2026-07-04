using SIE.Barcodes.WipBatchs;
using SIE.Web.Barcodes.WipBatchs.Commands;

namespace SIE.Web.Barcodes.WipBatchs
{
    /// <summary>
    /// 子生产批次视图配置
    /// </summary>
    public class SubWipBatchViewConfig : WebViewConfig<SubWipBatch>
    {
        /// <summary>
        /// 子批次条码视图
        /// </summary>
        public const string SubBatchView = "SubBatchView";

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
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.WipBatchNo).HasLabel("批次编码").ShowInList(150).FixColumn();
                View.Property(p => p.BatchNo).HasLabel("子批次编码").ShowInList(150).FixColumn();
                View.Property(p => p.Qty).HasLabel("批次数量").Show(ShowInWhere.All);
                View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.BoxesQty).HasLabel("满批数量").Show(ShowInWhere.All);
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All);
                View.Property(p => p.PrintDate).HasLabel("生成时间").ShowInList(150);
                View.Property(p => p.PrintTimes).ShowInList(150);
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
            View.ClearCommands().UseCommand(typeof(ReprintSubWipBatchCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipBatchNo).HasLabel("批次编码").ShowInList(150).FixColumn().Readonly();
                View.Property(p => p.BatchNo).HasLabel("子批次编码").ShowInList(150).FixColumn().Readonly();
                View.Property(p => p.Qty).HasLabel("批次数量").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.BatchState).HasLabel("批次状态").UseEnumEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.BoxesQty).HasLabel("满批数量").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsMantissa).UseCheckDropDownEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PrintDate).ShowInList(150).HasLabel("生成时间").Readonly();
                View.Property(p => p.PrintTimes).ShowInList(150).Readonly();
                View.Property(p => p.PrintByName).HasLabel("打印人").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PrintedState).HasLabel("打印状态").UseEnumEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsScraped).Show(ShowInWhere.All).Readonly();
                View.ChildrenProperty(p => p.BatchList).HasLabel("子批次信息").Visible(false);
            }
        }
    }
}