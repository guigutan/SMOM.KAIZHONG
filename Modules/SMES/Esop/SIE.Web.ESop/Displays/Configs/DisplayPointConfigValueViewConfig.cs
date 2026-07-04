using SIE.ESop.Configs;
using SIE.ESop.Displays;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.ESop.Configs
{
    /// <summary>
    /// 文档服务器配置视图配置
    /// </summary>
    internal class DisplayPointConfigValueViewConfig : WebViewConfig<DisplayPointConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseDetail(dialogHeight: 350);
            using (View.DeclareGroup("播放设置"))
            {
                View.Property(p => p.Interval).UseSpinEditor(p => p.MinValue = 10);
                View.Property(p => p.ReTryCount).UseSpinEditor(p => p.MinValue = 0);
            }

            using (View.DeclareGroup("快捷键"))
            {
                View.Property(p => p.ModifierKeys);
                View.Property(p => p.Key);
            }
        }
    }
}