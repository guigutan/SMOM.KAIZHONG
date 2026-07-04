using SIE.Items;

namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工艺路线 视图配置
    /// </summary>
    public class ProcessBomViewConfig : WebViewConfig<Item>
    {
        /// <summary>
        /// 工序bom视图配置
        /// </summary>
        public const string ProcessBomViewGroup = "ProcessBomConfigView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ProcessBomViewGroup);
            if (ViewGroup == ProcessBomViewGroup)
            {
                ProcessBomConfigView();
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected void ProcessBomConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("物料编码").Show();
                View.Property(p => p.Name).HasLabel("物料名称").Show();
                ////工艺路线中工序BOM不显示创建人等信息
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }


    }
}