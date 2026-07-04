using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.Service;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.DataQuerys
{

    /// <summary>
    /// 
    /// </summary>
    public class AbnormalDefineDataQuery : DataQueryer
    {
        /// <summary>
        /// 生成编码
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateCode()
        {
            return RT.Service.Resolve<AbnormalDefineService>().GenerateCode();
        }

    }
}
