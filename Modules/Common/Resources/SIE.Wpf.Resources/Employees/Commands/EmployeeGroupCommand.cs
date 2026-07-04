using SIE.Resources.Employees;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 维护员工组
    /// </summary>
    [Command(ImageName = "People", Label = "员工组", GroupType = CommandGroupType.Edit)]
    public class EmployeeGroupCommand : ClientCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        protected override void ExecuteCore()
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.ListView, typeof(EmployeeGroupViewConfig), null);

            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                ////var ui = AutoUI.GenerateAggtControl(typeof(EmployeeGroup));
                var ui = new ListUITemplate(typeof(EmployeeGroup)).CreateUI();
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