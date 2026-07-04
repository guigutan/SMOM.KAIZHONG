using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys
{

    /// <summary>
    /// 
    /// </summary>
    public class AbnormalMonitorTaskDataQuery : DataQueryer
    {
        /// <summary>
        /// 生成编码
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateCode()
        {
            return RT.Service.Resolve<AbnormalMonitorTaskService>().GenerateCode();
        }

        /// <summary>
        /// 获取扩展视图数据
        /// </summary>
        /// <param name="abnormalTaskId"></param>
        /// <returns></returns>
        public virtual AbnormalMonitorTask GetExtentionViewData(double abnormalTaskId)
        {
            return  RT.Service.Resolve<AbnormalMonitorTaskService>().GetExtentionViewData(abnormalTaskId);
        } 

        
    }
}
