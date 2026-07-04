using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Tech.Routings;
using SIE.Web.Command;
using System;

namespace SIE.Web.Tech.Routings.Commands
{
    /// <summary>
    /// 编辑工艺路线命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.EditRouting")]
    [AllowAnonymous]
    public class EditRouting : ViewCommand
    {
        /// <summary>
        /// 编辑工艺路线命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var edit_routing = args.Data.ToJsonObject<Routing>();
            var routing = RF.GetById<Routing>(edit_routing.Id);
            if (routing == null)
                throw new ValidationException("获取工节路线[{0}]失败".L10nFormat(edit_routing.Name));
            routing.Name = edit_routing.Name;
            routing.Description = edit_routing.Description;
            routing.PersistenceStatus = PersistenceStatus.Modified;
            RF.Save(routing);
            return true;
        }
    }
}