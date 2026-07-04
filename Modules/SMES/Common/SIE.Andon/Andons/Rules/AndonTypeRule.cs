using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Rules
{
    /// <summary>
    /// 安灯类型维护编码唯一
    /// </summary>
    [System.ComponentModel.DisplayName("<安灯类型维护编码唯一>")]
    [System.ComponentModel.Description("<安灯类型维护编码唯一>")]
    public class AndonTypeCodeNotDuplicateRule: NotDuplicateRule<AndonType>
    {
        /// <summary>
        /// 编码唯一
        /// </summary>
        public AndonTypeCodeNotDuplicateRule()
        {
            Properties.Add(AndonType.AndonTypeCodeProperty);
            MessageBuilder = (e) =>
            {
                var andonType = e as AndonType;
                return "已经存在[安灯类型编码]是{0}的数据".L10nFormat(andonType.AndonTypeCode);
            };
        }
    }

    /// <summary>
    /// 安灯类型维护名称唯一
    /// </summary>
    [System.ComponentModel.DisplayName("<安灯类型维护名称唯一>")]
    [System.ComponentModel.Description("<安灯类型维护名称唯一>")]
    public class AndonTypeNameNotDuplicateRule : NotDuplicateRule<AndonType>
    {
        /// <summary>
        /// 名称唯一
        /// </summary>
        public AndonTypeNameNotDuplicateRule()
        {
            Properties.Add(AndonType.AndonTypeNameProperty);
            MessageBuilder = (e) => {
                var andonType = e as AndonType;
                return "已经存在[安灯类型名称]是{0}的数据".L10nFormat(andonType.AndonTypeName);

            };
        }
    }


    /// <summary>
    /// 安灯类型被引用不可删除
    /// </summary>
    [System.ComponentModel.DisplayName("<安灯类型被引用不可删除>")]
    [System.ComponentModel.Description("<安灯类型被引用不可删除>")]
    public class AndonTypeReferenceRule: NoReferencedRule<AndonType>
    {
        /// <summary>
        /// 引用不可删除
        /// </summary>
        public AndonTypeReferenceRule()
        {
            Properties.Add(Andon.AndonTypeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var andonType = o as AndonType;
                return "安灯类型{0}被安灯引用，不能被删除".L10nFormat(andonType.AndonTypeCode);

            };
        }
    }
}
