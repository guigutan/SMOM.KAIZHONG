using SIE.MES.ProcessProperty;

namespace SIE.Web.MES.ProcessProperty
{
    /// <summary>
    /// 工艺参数界面
    /// </summary>
    public class ProcessParamCollectParamViewConfig : WebViewConfig<ProcessParamCollectParam>
    {
        /// <summary>
        /// 列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.ParamName).ShowInList(150);
            View.Property(p => p.ParamValue).ShowInList(150);
            View.Property(p => p.Unit).ShowInList(150);
        }
    }
}
