using SIE.Domain;
using System.ComponentModel;

namespace SIE.Tech.VictoryStandards
{
    /// <summary>
    /// 胜局标准提交后事件
    /// </summary>
    [DisplayName("胜局标准提交后事件")]
    [Description("胜局标准提交后-自动带出最大测试次数")]
    public class VictoryStandardDetailOnSubmitted : OnSubmitted<VictoryStandardDetail>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="entity"> 胜局标准</param>
        /// <param name="e">参数</param>
        protected override void Invoke(VictoryStandardDetail entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update || e.Action == SubmitAction.Delete)
            {
                if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
                    RT.Service.Resolve<VictoryStandardController>().ValidationStandard(entity.VictoryStandardId);
                RT.Service.Resolve<VictoryStandardController>().UpdateMaxTestQty(entity.VictoryStandardId);
            }
        }
    }
}
