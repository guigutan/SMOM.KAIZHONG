using SIE.LES.LesStockCounts;
using SIE.LES.LesStockCounts.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 盘点单完工视图
    /// </summary>
    public class LesProcessModeViewModelViewConfig : WebViewConfig<LesProcessModeViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LesStockCount));
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.IsDiffReview).UseCheckEditor(p => p.XType = "radio");
            View.Property(p => p.IsDiffAdjust).UseCheckEditor(p => p.XType = "radio");
            View.Property(p => p.IsNotDeal).UseCheckEditor(p => p.XType = "radio");
            View.Property(p => p.Tips).UseDisplayEditor(p =>
            {
                p.LabelWidth = 0;
            }).ShowInDetail();
        }
    }
}
