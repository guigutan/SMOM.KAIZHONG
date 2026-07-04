using SIE.Resources.ShiftTypes;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.CalendarSchemes.Commands
{
    /// <summary>
    /// 查看班制
    /// </summary>
    [Command(ImageName = "FileEye", Label = "查看班制", GroupType = 20)]
    public class ShowShiftTypeCommand : ClientCommand
    {
        /// <summary>
        /// 查看日历方案
        /// </summary>
        protected override void ExecuteCore()
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.ListView, typeof(ShiftType), null);

            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var ui = new ListUITemplate(typeof(ShiftType)).CreateUI();
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
