using SIE.Resources.Employees;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.Employees.ViewModels;
using System;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 员工列表添加员工命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加", GroupType = CommandGroupType.Edit, Gestures = "Ctrl+F")]
    public class EmployeeAddCommand : ListAddCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
            var newEmployee = view.CreateNewItem() as Employee;
            OnItemCreated(newEmployee);
            
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(EmployeeViewModel), null);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var template = new DetailsUITemplate(typeof(EmployeeViewModel));
                template.ModuleKey = view.ModuleKey;
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                detailView["detailVmlKey"] = key;
                var model = new EmployeeViewModel(newEmployee);
                detailView.Data = model;
                model.MarkSaved();
                ////退出时，数据已被修改且未保存时，提示用户
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