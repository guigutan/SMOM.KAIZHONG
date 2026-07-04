using SIE.AbnormalInfo.AbnormalMonitors.ViewModels;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.Web.Data;

namespace SIE.AbnormalInfo.AbnormalMonitors.DataQuerys
{
    /// <summary>
    /// 安灯类型维护数据操作
    /// </summary>
    public class PushTargetTriggerPowerDataQuery : DataQueryer
    {
        /// <summary>
        /// 根据推送对象类型获取数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<PushTargetViewModel> GetDataByType(PushTargetEnum type, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<PushTargetController>().GetPushTargetData(type, keyword, pagingInfo);
        }
    }
}
