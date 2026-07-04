using SIE.Domain;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 培训记录提交了，触发检查员工认证
    /// </summary>
    [DisplayName("培训记录保存后逻辑")]
    [Description("培训记录保存后触发检查员工技能授予情况")]
    public class TrainingRecordSubmitted : OnSubmitted<TrainingRecord>
    {
        /// <summary>
        /// 提交后事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">e</param>
        protected override void Invoke(TrainingRecord entity, EntitySubmittedEventArgs e)
        {
            ////添加是否历史验证，避免技能授权后更新认证过程数据时多次重复认证
            if ((e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update) && !entity.IsHistory)
            {
                RT.Service.Resolve<SkillAuthController>().UpdateTrainingRecord(entity);
            }
        }
    }
}