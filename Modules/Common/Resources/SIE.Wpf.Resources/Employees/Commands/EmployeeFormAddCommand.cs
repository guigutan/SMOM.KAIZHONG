using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.Employees;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.Employees.ViewModels;
using System;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 员工表单添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = CommandGroupType.Edit)]
    public class EmployeeFormAddCommand : FormAddCommand
    {
        /// <summary>
        /// 添加员工命令
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnNewItemCreated(Entity entity)
        {
            try
            {
                ////base.OnNewItemCreated(entity);
                ////var employeeVmdl = entity as EmployeeViewModel;
                ////employeeVmdl.Employee = new Employee();

                CRT.Workbench.Close(View["detailVmlKey"].ToString());
                var newEmployee = new Employee();
                newEmployee.GenerateId();
                string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(EmployeeViewModel), null);
                CRT.Workbench.ShowView(key, v =>
                {
                    v.Title = Meta.Label.L10N();
                    var template = new DetailsUITemplate(typeof(EmployeeViewModel));
                    var ui = template.CreateUI();
                    var detailView = ui.MainView as DetailLogicalView;
                    detailView["detailVmlKey"] = key;
                    var model = new EmployeeViewModel(newEmployee);
                    detailView.Data = model;
                    model.MarkSaved();
                    return ui;
                });
            }
            catch (Exception exc)
            {
                throw new ValidationException("员工添加失败! ".L10N() + exc.Message);
            }
        }
    }
}
