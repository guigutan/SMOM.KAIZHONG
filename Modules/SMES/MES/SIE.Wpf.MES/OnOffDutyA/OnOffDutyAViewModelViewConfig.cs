using SIE.Wpf.Common;
using SIE.Wpf.MES.OnOffDutyA.Commands;
using System;

namespace SIE.Wpf.MES.OnOffDutyA
{
    /// <summary>
    /// 维修采集视图配置
    /// </summary>
    internal class OnOffDutyAViewModelViewConfig : WPFViewConfig<OnOffDutyAViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {

            View.AssignAuthorize(typeof(OnOffDutyAViewModel));
            View.ClearCommands();
            View.UseCommands(typeof(OnOffDutyAInputCommand));

            View.UseDetail(columnCount: 3);

            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty))
                {
                    View.Property(p => p.Tips).UseEditor("TipsEditor").ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                    View.Property(p => p.Error).UseEditor("ErrorEditor").ShowInDetail(columnSpan: 3, height: 0, hideLabel: true);
                }

                using (View.DeclareGroup("扫描信息".L10N()))
                {
                    View.Property(p => p.Barcode).UseBarcodeEditor().ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
                    View.Property(p => p.IsOnDuty).UseBoolSwitchEditor(e => e.DisplayName = new string[] { "上岗", "下岗" }).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                }
            }
        }
    }
}