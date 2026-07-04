using SIE.Domain;
using System;
using System.Linq;

namespace SIE.Core.Logs
{
    /// <summary>
    /// 接口控制器
    /// </summary>
    public partial class InterfaceLogController : DomainController
    {
        /// <summary>
        /// 清空某天以前的接口日志数据
        /// </summary>
        /// <param name="day">提前天数</param>
        public virtual void ClearInterfaceLog(int day)
        {
            var now = RF.Find<InterfaceLog>().GetDbTime();
            var logs = GetInterfaceLogs(now, day);
            logs.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(logs);
        }

        /// <summary>
        /// 获取某时间点之前的接口日志列表
        /// </summary>
        /// <param name="now">当前时间</param>
        /// <param name="day">提前天数</param>
        /// <returns>接口日志列表</returns>
        public virtual EntityList<InterfaceLog> GetInterfaceLogs(DateTime now, int day)
        {
            return Query<InterfaceLog>().Where(p => p.CreateDate < now.AddDays(-day)).ToList();
        }
    }
}
