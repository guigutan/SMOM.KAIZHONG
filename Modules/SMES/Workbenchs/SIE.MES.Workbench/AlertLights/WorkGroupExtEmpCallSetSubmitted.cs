using SIE.Domain;
using SIE.Resources.Employees;

namespace SIE.MES.Workbench.AlertLights
{
    /// <summary>
    /// 安灯预警员工呼叫设置保存事件
    /// </summary>
    [System.ComponentModel.DisplayName("班组保存后需要保存安灯预警员工呼叫设置")]
    [System.ComponentModel.Description("班组保存后需要保存安灯预警员工呼叫设置")]
    public class WorkGroupExtEmpCallSetSubmitted : OnSubmitted<WorkGroup>
    {
        /// <summary>
        /// OnSubmitted的事件处理方法
        /// </summary>
        /// <param name="entity">WorkGroup实体</param>
        /// <param name="e">事件参数</param>
        protected override void Invoke(WorkGroup entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update || e.Action == SubmitAction.Delete)
            {
                var empCallSets = entity.GetProperty(WorkGroupExtPrpyAlertLightCallSet.EmpCallSettingExtProperty);
                if (empCallSets != null)
                {
                    RF.Save(empCallSets);
                    empCallSets.MarkSaved();
                }
            }
        }
    }
}
