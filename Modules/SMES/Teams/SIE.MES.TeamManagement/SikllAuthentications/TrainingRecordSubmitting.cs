using SIE.Domain;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 培训记录保存前触发换算培训时长
    /// </summary>
    [DisplayName("培训记录保存前逻辑")]
    [Description("培训记录保存前触发换算培训时长")]
    public class TrainingRecordSubmitting : OnSubmitting<TrainingRecord>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="entity">培训记录</param>
        /// <param name="e">参数</param>
        protected override void Invoke(TrainingRecord entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                var ctl = RT.Service.Resolve<SkillAuthController>();
                decimal duration = 0;
                if (entity.BeginDate.HasValue && entity.EndDate.HasValue)
                    duration = ctl.GetHourDiff(entity.BeginDate.Value, entity.EndDate.Value);
                entity.Duration = duration;
            }
        }
    }
}