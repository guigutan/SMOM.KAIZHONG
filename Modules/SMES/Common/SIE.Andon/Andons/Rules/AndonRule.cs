using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Rules
{
    /// <summary>
    /// 安灯维护编码唯一验证
    /// </summary>
    [System.ComponentModel.DisplayName("<安灯维护编码唯一>")]
    [System.ComponentModel.Description("<安灯维护编码唯一>")]
    public class AndonCodeNotDuplicateRule : NotDuplicateRule<Andon>
    {
        /// <summary>
        /// 编码唯一
        /// </summary>
        public AndonCodeNotDuplicateRule()
        {
            Properties.Add(Andon.AndonCodeProperty);
            MessageBuilder = (e) =>
            {
                var andon = e as Andon;
                return "已经存在[安灯类型编码]是{0}的数据".L10nFormat(andon.AndonCode);
            };
        }
    }


    /// <summary>
    /// 安灯维护名称唯一
    /// </summary>
    [System.ComponentModel.DisplayName("<安灯维护名称唯一>")]
    [System.ComponentModel.Description("<安灯维护名称唯一>")]
    public class AndonNameNotDuplicateRule : NotDuplicateRule<Andon>
    {
        /// <summary>
        /// 名称唯一
        /// </summary>
        public AndonNameNotDuplicateRule()
        {
            Properties.Add(Andon.AndonNameProperty);
            MessageBuilder = (e) => {
                var andon = e as Andon;
                return "已经存在[安灯类型名称]是{0}的数据".L10nFormat(andon.AndonName);

            };
        }
    }
}
