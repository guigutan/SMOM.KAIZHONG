using SIE.Core.WorkOrders;
using SIE.Web.Items._Extentions_;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
	/// 生产批次视图配置
	/// </summary>
	internal class WoWipBatchViewConfig : WebViewConfig<WoWipBatch>
    {
        /// <summary>
        /// 查看工单视图ViewGroup
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadonlyView);

            if (ViewGroup == ReadonlyView) 
            {
                ConfigReadonlyView();
            }
        }
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.Qty).UseItemUnitEditor(e => { e.MinValue = 1; e.MaxValue = 1000000;}).DefaultValue(1).Readonly(false);
        }

        void ConfigReadonlyView()
        {
            View.ClearCommands();
            View.Property(p => p.Qty).ShowInDetail().Readonly();
        }
    }
}