using SIE.MetaModel.View;
using SIE.Resources.ProcessTechTypes;

namespace SIE.Web.Resources.ProcessTechTypes
{
    /// <summary>
    /// 制程工艺类型
    /// </summary>
    public class ProcessTechTypeViewConfig : WebViewConfig<ProcessTechType>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.AlgorithmMarking);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.AlgorithmMarking);
            }
        }
    }
}
