using SIE.RedCardManagment.RedCards;
using SIE.Web.Command;

namespace SIE.Web.RedCardManagment.RedCards.Commands
{
    /// <summary>
    /// 信息追溯命令
    /// </summary>
    public class SyncRedCardReelsCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            double redCardId = args.Data.ToJsonObject<double>();
            int syncCount = RT.Service.Resolve<RedCardService>().SyncRedCardTraceable(redCardId);
            return syncCount;
        }
    }
}
