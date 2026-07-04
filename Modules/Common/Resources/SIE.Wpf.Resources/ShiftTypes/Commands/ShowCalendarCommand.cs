using System;
using SIE.Wpf.Command;
using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Resources.CalendarSchemes.Templates;

namespace SIE.Wpf.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 查看日历方案
    /// </summary>
    [Command(ImageName = "FileEye", Label = "查看日历方案", GroupType = 20)]
    public class ShowCalendarCommand : ClientCommand
    {
        /// <summary>
        /// 查看日历方案
        /// </summary>
        protected override void ExecuteCore()
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.ListView, typeof(CalendarScheme), null);

            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var ui = new CalendarSchemeTemplate().CreateUI();
                var query = ui.MainView.QueryView;
                if (query != null)
                {
                    query.TryExecuteQuery();
                }

                v.Closing += (o, e) =>
                {
                    if (ui.MainView.Data.IsDirty)
                    {
                        if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                };
                return ui;
            });
        }
    }
}
