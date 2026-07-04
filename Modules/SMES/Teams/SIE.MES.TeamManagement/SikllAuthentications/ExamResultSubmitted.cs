using SIE.Domain;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 考试结果提交了，触发检查员工认证
    /// </summary>
    [DisplayName("考试结果保存后逻辑")]
    [Description("考试结果保存后触发检查员工技能授予情况")]
    public class ExamResultSubmitted : OnSubmitted<ExamResult>
    {
        /// <summary>
        /// 提交后事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Invoke(ExamResult entity, EntitySubmittedEventArgs e)
        {
            ////添加是否历史验证，避免技能授权后更新认证过程数据时多次重复认证
            if ((e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update) && !entity.IsHistory)
            {
                RT.Service.Resolve<SkillAuthController>().UpdateExamResult(entity);
            }
        }
    }
}