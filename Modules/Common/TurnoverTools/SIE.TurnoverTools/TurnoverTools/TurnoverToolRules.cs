using SIE.Domain.Validation;
using System.ComponentModel;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 周转工具型号删除验证规则，被周转工具引用的不能删除
    /// </summary>
    [DisplayName("周转工具型号删除验证规则")]
    [Description("被周转工具引用的不能删除")]
    public class TurnoverToolRefModelRule : NoReferencedRule<TurnoverToolModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TurnoverToolRefModelRule()
        {
            Properties.Add(TurnoverTool.ModelIdProperty);
        }
    }
}
