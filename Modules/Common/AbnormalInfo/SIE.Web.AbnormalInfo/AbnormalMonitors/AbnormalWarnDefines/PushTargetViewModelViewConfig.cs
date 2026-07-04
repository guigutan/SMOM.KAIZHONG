using SIE.AbnormalInfo.AbnormalMonitors.ViewModels;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 推送对象编码视图配置
    /// </summary>
    public class PushTargetViewModelViewConfig : WebViewConfig<PushTargetViewModel>
    {
       /// <summary>
       /// 推送对象选择视图字符串
       /// </summary>
        public const string PushTargetViewGroup = "PushTargetViewGroup";

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PushTargetViewGroup);
            if (ViewGroup == PushTargetViewGroup)
            {
                PushTargetView();
            }
        }

        /// <summary>
        /// 推送对象选择视图
        /// </summary>
        protected void PushTargetView()
        {
            using (View.OrderProperties())
            {
                View.DisableEditing();
                View.Property(p => p.TargetCode).ShowInList(width: 200);
                View.Property(p => p.TargetName).ShowInList(width: 200);
            }
        }
    }
}
