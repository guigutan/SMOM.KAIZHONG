using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Projects.Rules
{
    /// <summary>
    /// 项目参数验证删除规则
    /// </summary>
    [DisplayName("项目参数验证删除规则")]
    [Description("项目参数验证删除规则")]
    public class ProjectParamNoReRule : NoReferencedRule<ProjectParam>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ProjectParamNoReRule()
        {
            Properties.Add(ProcessStandardParamDtl.ProjectParamIdProperty);
            MessageBuilder = (o, e) =>
            {
                var param = o as ProjectParam;
                return "项目参数[{0}]存在工序需求属性组中引用，不能删除".L10nFormat(param.Code);
            };
        }
    }
}
