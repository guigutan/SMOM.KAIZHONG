using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Tech.Processs.Commands
{
    /// <summary>
    /// 工序技能命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Processs.Commands.ProcessSkillCommand")]
    public class ProcessSkillCommand : ViewCommand
    {
        /// <summary>
        /// 工序技能命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var skillList = args.Data.ToJsonObject<List<ProcessSkill>>();
            Check.NotNullOrEmpty(skillList, nameof(skillList));
            if (skillList == null || skillList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(skillList)));
            }

            foreach (var item in skillList)
            {
                var skill = new ProcessSkill();
                skill.SkillId = item.SkillId;
                skill.ProcessId = item.ProcessId;
                savedData.Add(skill);
            }

            RF.Save(savedData);
            return true;
        }
    }
}