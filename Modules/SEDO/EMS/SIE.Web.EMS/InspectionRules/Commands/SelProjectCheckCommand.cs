using SIE.EMS.Common.Entity;
using SIE.EMS.InspectionRules;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIE.Web.EMS.InspectionRules.Commands
{
    /// <summary>
    /// 添加点检项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.InspectionRules.Commands.SelProjectCheckCommand")]
    public class SelProjectCheckCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var checkProjectInfos = args.Data.ToJsonObject<List<CheckProjectInfo>>();
            Check.NotNullOrEmpty(checkProjectInfos, nameof(checkProjectInfos));
            if (null == checkProjectInfos || checkProjectInfos.Count == 0)
            {
                throw new ValidationException("点检项目列表不能为空".L10N());
            }
            RT.Service.Resolve<InspectionRuleController>().SaveSelProjectCheck(checkProjectInfos);
            return true;
        }
    }
}
