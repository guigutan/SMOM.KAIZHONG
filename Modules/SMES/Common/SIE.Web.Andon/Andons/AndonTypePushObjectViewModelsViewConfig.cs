using SIE.Andon.Andons.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护推送对象编码视图配置
    /// </summary>
    public class AndonTypePushObjectViewModelsViewConfig : WebViewConfig<AndonTypePushObjectViewModel>
    {
       /// <summary>
       /// 推送对象选择视图字符串
       /// </summary>
        public const string PushObjectViewGroup = "PushObjectViewGroup";

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PushObjectViewGroup);
            if (ViewGroup == PushObjectViewGroup)
            {
                PushObjectView();
            }
        }

        /// <summary>
        /// 推送对象选择视图
        /// </summary>
        protected void PushObjectView()
        {
            using (View.OrderProperties())
            {
                View.DisableEditing();
                View.Property(p => p.Code).ShowInList(width: 200);
                View.Property(p => p.Name).ShowInList(width: 200);
            }
        }
    }
}
