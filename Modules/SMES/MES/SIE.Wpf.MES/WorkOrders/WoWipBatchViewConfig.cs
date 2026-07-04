using SIE.Core.WorkOrders;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
	/// 生产批次视图配置
	/// </summary>
	internal class WoWipBatchViewConfig : WPFViewConfig<WoWipBatch>
    {
        /// <summary>
        /// 查看工单视图ViewGroup
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadonlyView);
            switch (ViewGroup)
            {
                case ReadonlyView:
                    ConfigReadonlyView();
                    break;
            }
        }
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.Qty).UseSpinEditor(e => { e.MinValue = 1; e.MaxValue = 1000000; });
        }

        void ConfigReadonlyView()
        {
            View.ClearCommands();
            View.Property(p => p.Qty).ShowInDetail().Readonly();
        }
    }
}