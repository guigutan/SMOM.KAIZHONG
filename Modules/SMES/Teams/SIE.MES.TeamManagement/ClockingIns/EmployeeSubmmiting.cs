using SIE.Domain;
using SIE.Resources.Employees;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工保存前需统计新旧班组在编人数
    /// </summary>
    [DisplayName("员工保存前需统计新旧班组在编人数")]
    [Description("员工保存前需统计新旧班组在编人数")]
    public class EmployeeSubmmiting : OnSubmitting<Employee>
    {
        /// <summary>
        /// 重写提交前方法
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="e">e</param>
        protected override void Invoke(Employee entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert && entity.WorkGroupId.HasValue)
            {
                var workGroup = RF.GetById<SIE.Resources.Employees.WorkGroup>(entity.WorkGroupId);
                if (workGroup == null) return;
                workGroup.ActualQty = (workGroup.ActualQty.HasValue ? workGroup.ActualQty + 1 : 1);
                RF.Save(workGroup);
                RT.Service.Resolve<ClockInController>().UpdateEmployeeClockIn(entity);
            }
            else if (e.Action == SubmitAction.Delete && entity.WorkGroupId.HasValue)
            {
                var workGroup = RF.GetById<SIE.Resources.Employees.WorkGroup>(entity.WorkGroupId);
                if (workGroup == null || workGroup.ActualQty < 1) return;
                workGroup.ActualQty--;
                RF.Save(workGroup);
            }
            else if (e.Action == SubmitAction.Update)
            {
                var oldEmployee = RF.GetById<Employee>(entity.Id);
                if (entity.WorkGroup != null)
                {
                    // 更新保存数据
                    UpdateEmployee(entity,oldEmployee);
                }
                else
                {
                    if (oldEmployee.WorkGroup == null)
                    {
                        return;
                    }
                    var oldWorkGroup = RF.GetById<WorkGroup>(oldEmployee.WorkGroupId);
                    if (oldWorkGroup == null || oldWorkGroup.ActualQty < 1) return;
                    oldWorkGroup.ActualQty--;
                    RF.Save(oldWorkGroup);
                }
            }
        }

        /// <summary>
        /// 更新保存数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="oldEmployee"></param>
        private void UpdateEmployee(Employee entity, Employee oldEmployee)
        {
            if (entity.WorkGroupId == oldEmployee.WorkGroupId && entity.EmployeeStatus == oldEmployee.EmployeeStatus)
            {
                return;
            }
            ////修改了班组，或员工状态离职改成在职，新班组或当前班组在编数量+1
            if (entity.WorkGroupId != oldEmployee.WorkGroupId || (entity.EmployeeStatus != oldEmployee.EmployeeStatus && entity.EmployeeStatus == EmployeeStatus.Job))
            {
                ////修改了班组而且员工是在职的才更改班组员工数量
                if (entity.EmployeeStatus == EmployeeStatus.Job && entity.WorkGroupId.HasValue)
                {
                    var workGroup = RF.GetById<WorkGroup>(entity.WorkGroupId);
                    if (workGroup != null)
                    {
                        workGroup.ActualQty = (workGroup.ActualQty.HasValue ? workGroup.ActualQty + 1 : 1);
                        RF.Save(workGroup);
                    }
                }
                ////只要改了班组就更改员工出勤的数据
                if (entity.WorkGroupId != oldEmployee.WorkGroupId)
                    RT.Service.Resolve<ClockInController>().UpdateEmployeeClockIn(entity);
            }
            ////仅修改了班组，或员工状态在职改成离职，旧班组在编数量-1
            if (oldEmployee.WorkGroupId.HasValue || (entity.EmployeeStatus != oldEmployee.EmployeeStatus && entity.EmployeeStatus == EmployeeStatus.UnJob))
            {
                var oldWorkGroup = RF.GetById<WorkGroup>(oldEmployee.WorkGroupId);
                if (oldWorkGroup == null || oldWorkGroup.ActualQty < 1) return;
                oldWorkGroup.ActualQty--;
                RF.Save(oldWorkGroup);
            }
        }
    }
}