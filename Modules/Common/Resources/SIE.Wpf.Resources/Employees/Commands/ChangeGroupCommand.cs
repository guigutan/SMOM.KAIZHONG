using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.Employees.ViewModels;
using System;
using System.Linq;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 转班组命令按钮
    /// </summary>
    [Command(ImageName = "PeopleChange", Label = "转班组")]
    public class ChangeGroupCommand : ListViewCommand
    {
        /// <summary>
        /// 选择的数量、(bool)选择的实体.确定序列是否包含任何元素、当前选择对象
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Count >= 1 && view.SelectedEntities.Any() && view.Current != null;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new SearchTemplate();
            template.EntityType = typeof(WorkGroup);
            var ui = template.CreateUI();
            var listView = ui.MainView.CastTo<ListLogicalView>();

            var current = view.Current as Employee;
            double parentId = current.WorkGroupId.Value;
            var ctl = RT.Service.Resolve<EmployeeController>();
            listView.Data = ctl.GetWorkGroupsExceptId(current.WorkGroup.Name);
            var employeeList = new EntityList<Employee>();
            CRT.Workbench.ShowDialog(ui, w =>
           {
               w.Title = "选择班组".L10N();
               w.Closing += (o, e) =>
               {
                   if (w.Result == 0)
                   {
                       if (listView.SelectedEntities.Count != 1) //弹框选择的班组对象
                       {
                           CRT.MessageService.ShowMessage("一个员工只能对应一个班组".L10N());
                           e.Cancel = true;
                           return;
                       }
                       else
                       {
                           try
                           {
                               foreach (var employee in view.SelectedEntities) //遍历选择的要转班组的员工集合
                               {
                                   var selectItem = employee.CastTo<Employee>();
                                   selectItem.WorkGroupId = listView.Current.CastTo<WorkGroup>().Id; //更改该员工的班组ID
                                   selectItem.PersistenceStatus = PersistenceStatus.Modified;
                                   employeeList.Add(selectItem);
                                   selectItem.NotifyPropertyChanged(Employee.WorkGroupIdProperty);
                               }

                               RF.Save(employeeList);
                               CRT.MessageService.ShowMessage("班组转换成功".L10N());
                               view.Data = ctl.GetEmployeeByWorkGroupId(parentId);
                               view.RefreshControl();
                           }
                           catch (Exception)
                           {
                               CRT.MessageService.ShowMessage("班组转换失败".L10N());
                           }
                       }
                   }
               };
           });
        }
    }
}
