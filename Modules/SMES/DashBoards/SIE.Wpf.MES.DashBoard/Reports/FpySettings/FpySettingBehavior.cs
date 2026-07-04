using SIE.MES.DashBoard.Reports.FpySettings;
using System;

namespace SIE.Wpf.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 直通率设置视图行为
    /// </summary>
    public class FpySettingBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加方法
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            view.Control.CustomColumnDisplayText += (s, e) =>
            {
                if (e.Column.FieldName == FpySetting.DesiredProperty.Name || e.Column.FieldName == FpySetting.AlarmProperty.Name)
                    e.DisplayText = "{0}%".FormatArgs(e.DisplayText);
            };
        }
    }
}