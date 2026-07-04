using SIE.Domain;
using SIE.Security;
using SIE.Tech.Routings;
using SIE.Web.Command;

namespace SIE.Web.Tech.Routings.Commands
{
    /// <summary>
    /// 添加工艺路线命令
    /// </summary>
    [JsCommand("SIE.Web.Tech.Routings.Commands.AddRouting")]
    [AllowAnonymous]
    public class AddRouting : ViewCommand
    {
        /// <summary>
        /// 添加工艺路线命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var routing = args.Data.ToJsonObject<Routing>();
            routing.PersistenceStatus = PersistenceStatus.New;
            RF.Save(routing);
            return new
            {
                Id = routing.Id,
                CategoryId = routing.CategoryId,
                Name = routing.Name,
                Description = routing.Description,
                DefaultVersionId = routing.DefaultVersionId,
                text = routing.Name,
                leaf = false,
            };
        }
    }
}
