using SIE.Resources.Employees;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.Employees.ViewModels;
using System;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 创建员工保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存", GroupType = CommandGroupType.Edit, Gestures = "Ctrl+S")]
    internal class EmployeeSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 判断命令是否可执行
        /// </summary>
        /// <param name="view">DetailLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            ////return true/*view.Data.IsDirty*/;
            return view.Data.IsDirty;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">DetailLogicalView</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Data as EmployeeViewModel;
            model.Employee.Code = model.Code;
            model.Employee.Name = model.Name;
            model.Employee.Email = model.Email;
            model.Employee.Phone = model.Phone;
            model.Employee.Remark = model.EmployeeRemark;
            model.AcceptChanges();
            ////创建员工
            // model.Employee = RT.Service.Resolve<EmployeeController>().CreateEmployee(model.Employee, model.Employee.User);
            model.Employee = RT.Service.Resolve<EmployeeController>().SaveEmployee(model.Employee, model.CreateAccount, model.AuthenticateMode, model.UserCode);
            model.MarkSaved();
            CRT.MessageService.ShowMessage("保存成功".L10N(), "操作提示".L10N());
            ////CRT.Workbench.Close(view["detailVmlKey"].ToString());
        }
    }
}
