using DocumentFormat.OpenXml.Wordprocessing;
using SIE.MES.TaskManagement.WipProgress;
using SIE.Web.Barcodes.WipBatchs.Commands;

namespace SIE.Web.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 生产批次视图配置
    /// </summary>
    public class WipProgressWipBatchViewConfig : WebViewConfig<WipProgressWipBatch>
    {

        /// <summary>
        /// 在制品查询视图
        /// </summary>
        public const string WipProgressBatchView = "WipProgressBatchView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WipProgressViewModel));
            View.DeclareExtendViewGroup(WipProgressBatchView);
            if (ViewGroup == WipProgressBatchView)
                ConfigWipProgressBatchView();
        }

        /// <summary>
        /// 配置在制品查询视图
        /// </summary>
        protected void ConfigWipProgressBatchView()
        {
            View.AddBehavior("SIE.Web.MES.TaskManagement.WipProgress.Behaviors.WipProgressBatchBehavior");

            View.ClearCommands().UseCommands(typeof(ReprintBatchCommand).FullName);
            View.UseGridSelectionModel();
        }
    }
}