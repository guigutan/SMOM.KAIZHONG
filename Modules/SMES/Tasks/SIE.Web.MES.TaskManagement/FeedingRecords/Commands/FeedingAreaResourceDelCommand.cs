using SIE.Domain;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.FeedingRecords.Commands
{
    /// <summary>
    ///删除命令
    /// </summary>
    public class FeedingAreaResourceDelCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope">作用域</param>
        /// <returns></returns>

        protected override object Excute(double[] args, string scope)
        {
            var selectIds = args.ToList();
            RT.Service.Resolve<FeedingRecordController>().DeleteFeedingAreaResources(selectIds);
            return true;
        }
    }
}

