using SIE.Domain;
using SIE.Security;
using SIE.Tech.Routings;
using SIE.Web.Command;
using System;

namespace SIE.Web.Tech.Routings.Commands
{
    /// <summary>
    /// 工艺路线版本删除命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.DeleteRoutingVersion")]
    [AllowAnonymous]
    public class DeleteRoutingVersion : ViewCommand
    {
        /// <summary>
        /// 工艺路线版本删除命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var versionId = Convert.ToDouble(args.Data);
            var routingVersion = RF.GetById<RoutingVersion>(versionId);
            var routingId = routingVersion.RoutingId;
            routingVersion.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(routingVersion);
           return RT.Service.Resolve<RoutingController>().GetMaxVersionNum(routingId);
        }
    }
}
