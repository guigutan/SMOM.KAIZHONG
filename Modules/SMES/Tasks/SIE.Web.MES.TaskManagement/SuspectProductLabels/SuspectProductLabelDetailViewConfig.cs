using SIE.MES.TaskManagement.SuspectProductLabels;

namespace SIE.Web.MES.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签明细 视图配置
    /// </summary>
    internal class SuspectProductLabelDetailViewConfig : WebViewConfig<SuspectProductLabelDetail>
    {
        protected override void ConfigListView()
        {
            View.Property(p => p.SuspectJudgeResult);
            View.Property(p => p.Qty);
            View.Property(p => p.DefectId);
            View.Property(p => p.SubBatchNo);
            View.Property(p => p.HandleById);
            View.Property(p => p.HandleDate);
        }
    }
}
