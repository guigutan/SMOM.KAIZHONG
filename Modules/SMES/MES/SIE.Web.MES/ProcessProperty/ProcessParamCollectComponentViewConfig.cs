using SIE.MES.ProcessProperty;

namespace SIE.Web.MES.ProcessProperty
{
    /// <summary>
    /// 子件界面
    /// </summary>
    public class ProcessParamCollectComponentViewConfig : WebViewConfig<ProcessParamCollectComponent>
    {
        /// <summary>
        /// 列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.ComponentSN).ShowInList(250);
        }
    }
}
