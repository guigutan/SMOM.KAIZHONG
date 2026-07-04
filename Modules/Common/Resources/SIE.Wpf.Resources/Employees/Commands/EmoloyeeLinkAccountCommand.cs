using SIE.Common.Users;
using SIE.Domain.Validation;
using SIE.Resources.Employees;
using SIE.Security;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.Employees.ViewModels;
using System;
using System.Windows;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 关联账号命令
    /// </summary>
    [Command(ImageName = "LinkVariant", Label = "关联账号", ToolTip = "关联账号", GroupType = CommandGroupType.Business)]
    public class LinkUserCommand : ListViewCommand
    {
        /// <summary>
        /// //当前选择对象不为空，选择的当前对象 UserId 没有有值，即该员工没有关联用户表中的账号
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && !view.Current.CastTo<Employee>().UserId.HasValue;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
            var ui = this.GenerateSelectionUI();
            var result = this.PopupSelectionWindow(ui, view.Current.CastTo<Employee>());
            if (result == 0)
            {
                view.DataLoader.ReloadDataAsync();
            }
        }

        /// <summary>
        /// 生成选择界面的UI
        /// </summary>
        /// <returns>ControlResult</returns>
        protected ControlResult GenerateSelectionUI()
        {
            var template = new SearchTemplate();
            template.EntityType = typeof(User);
            template.ViewGroup = ViewConfig.ListView;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            if (listView.CommandsContainer != null)
                listView.CommandsContainer.Visibility = Visibility.Collapsed;
            listView.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;
            var queryView = ui.MainView.QueryView;
            if (queryView != null)
            {
                queryView.TryExecuteQuery();
            }

            return ui;
        }

        /// <summary>
        /// 子类重写此方法实现自己的弹框逻辑
        /// </summary>
        /// <param name="ui">ControlResult</param>
        /// <param name="currentEmployee">Employee</param>
        /// <returns>int</returns>
        protected int PopupSelectionWindow(ControlResult ui, Employee currentEmployee)
        {
            return CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = this.Meta.Label.L10N();
                var listView = ui.MainView as ListLogicalView;

                listView.Control.MouseDoubleClick += (o, e) =>
                {
                    w.Close(0);
                };

                //关闭窗口事件
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0 && listView.SelectedEntities.Count > 0)
                    {
                        try
                        {
                            Complete(listView, currentEmployee);
                        }
                        catch (Exception ex)
                        {
                            ex.Alert();
                            e.Cancel = true;
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 完成选择操作
        /// </summary>
        /// <param name="view">LogicalView</param>
        /// <param name="currentEmployee">Employee</param>
        protected void Complete(LogicalView view, Employee currentEmployee)
        {
            var user = view.Current as User;
            ////调用控制器里面的关联账户方法
            RT.Service.Resolve<EmployeeController>().LinkUser(currentEmployee.Id, user.Id);
        }
    }

    /// <summary>
    /// 解除关联的账号
    /// </summary>
    [Command(ImageName = "LinkVariantOff", Label = "解除关联", ToolTip = "解除关联的账号", GroupType = CommandGroupType.Business)]
    public class UnLinkUserCommand : ListViewCommand
    {
        /// <summary>
        /// 判断命令是否可执行
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && view.Current.CastTo<Employee>().UserId.HasValue;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
            var employee = view.Current.CastTo<Employee>();
            var ctl = RT.Service.Resolve<IUserService>();
            var user = ctl.GetById(employee.UserId.Value);
            if (user == null)
                throw new ValidationException("解除关联失败，用户已删除".L10N());
            if (CRT.MessageService.AskQuestion("确认解除 ({0}){1}与用户{2} 的关联？".L10nFormat(employee.Code, employee.Name, user.Code),
                "操作确认".L10N()))
            {
                RT.Service.Resolve<EmployeeController>().UnlinkUser(employee.Id);
                view.DataLoader.ReloadDataAsync();
            }
        }
    }
}
