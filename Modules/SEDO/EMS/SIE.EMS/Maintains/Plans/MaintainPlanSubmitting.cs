using SIE.Domain;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养计划维护保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("保养计划维护保存前事件")]
    [System.ComponentModel.Description("保养计划维护保存前事件")]
    public class MaintainPlanSubmitting : OnSubmitting<MaintainPlan>
    {
        /// <summary>
        /// 保养计划维护保存前事件
        /// </summary>
        /// <param name="entity">客户地址</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(MaintainPlan entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                //新增时，根据选择的保养类型信息，更新保养类型
                if (string.IsNullOrEmpty(entity.MaintainTypeInfoId))
                {
                    entity.MaintainType = MaintainType.Week;
                }
                else
                {
                    entity.MaintainType = (MaintainType)(int.Parse(entity.MaintainTypeInfoId));
                }
            }
        }
    }
}
