using SIE.Domain.Validation;
using System;

namespace SIE.Tech.VictoryStandards
{
    /// <summary>
    /// 胜局标准:【胜制方案编码】+【胜局标准】不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("胜局标准非重验证")]
    [System.ComponentModel.Description("胜局标准:【胜制方案编码】+【胜局标准】不能重复")]
    public class VictoryStandardDetailNotDuplicateRule : NotDuplicateRule<VictoryStandardDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VictoryStandardDetailNotDuplicateRule()
        {
            Properties.Add(VictoryStandardDetail.VictoryStandardIdProperty);
            Properties.Add(VictoryStandardDetail.StandardProperty);
            MessageBuilder = (e) => { return "胜局标准:【胜制方案编码】+【胜局标准】不能重复".L10N(); };
        }
    }
}
