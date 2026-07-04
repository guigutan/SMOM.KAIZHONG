using SIE.EMS.Lubrications;
using SIE.MetaModel.View;

namespace SIE.WPF.EMS.Lubrications
{
    /// <summary>
    /// 润滑工时登记视图配置
    /// </summary>
    public class LubricationWorkHourViewConfig : WebViewConfig<LubricationWorkHour>
    {
        /// <summary>
        /// 查看记录
        /// </summary>
        public const string SeeView = "SeeView";


        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private const int charWidth = 20;

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SeeView);
            if (ViewGroup == SeeView)
            {
                ConfigSeeView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationWorkHourListBehavior");
            View.UseCommands("SIE.Web.EMS.Lubrications.Commands.LubricationWorkHourAddCommand", WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.StartDateTime).UseDateTimeEditor().ShowInList(width: (charWidth * 8));
                View.Property(p => p.EndDateTime).UseDateTimeEditor().ShowInList(width: (charWidth * 8));
                View.Property(p => p.Hours).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 1;
                }).ShowInList(width: (charWidth * 4)).UseSpinEditor(p=>p.MinValue=0.01);
                View.Property(p => p.ExecutorId).HasLabel("执行人").ShowInList(width: (charWidth * 6));
            }
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        public void ConfigSeeView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.StartDateTime).UseDateTimeEditor().Readonly().ShowInList(width: (charWidth * 8));
                View.Property(p => p.EndDateTime).UseDateTimeEditor().Readonly().ShowInList(width: (charWidth * 8));
                View.Property(p => p.Hours).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 1;
                }).Readonly().ShowInList(width: (charWidth * 4));
                View.Property(p => p.ExecutorId).Readonly().HasLabel("执行人").ShowInList(width: (charWidth * 6));
            }
        }
    }
}