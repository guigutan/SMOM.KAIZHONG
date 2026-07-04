using SIE.Wpf.MES;

namespace SIE.Wpf.ESop
{
    /// <summary>
    /// 工作站信息 视图配置
    /// </summary>
    public class WorkstationViewConfig : WPFViewConfig<EsopWorkstation>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.User).UseLockableLookUpEditor().ShowInDetail();
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //重写父类
        }
    }
}