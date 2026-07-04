
using Newtonsoft.Json;
using SIE.AbnormalInfo.AbnormalMonitors.AbnomalRule.Dao;
using SIE.AbnormalInfo.AbnormalMonitors.Dao;
using SIE.AbnormalInfo.AbnormalMonitors.SimpleCalculator;
using SIE.AbnormalInfo.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.Service
{
    /// <summary>
    /// 异常来源控制类
    /// </summary>
    public class AbnormalSourceService : DomainService
    {
        private readonly AbnormalSourceDao _abnormalSourceDao;

        /// <summary>
        /// AbnormalSourceService
        /// </summary>
        public AbnormalSourceService(AbnormalSourceDao abnormalSourceDao)
        {
            _abnormalSourceDao = abnormalSourceDao;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public virtual AbnormalSource GetAbnormalSource(string monitorName, double metadataId)
        {
            var source = _abnormalSourceDao.FindMany(c => c.MonitorName == monitorName && c.AbnormalEntityMetadataId == metadataId,null,new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();
            return source;
        }



    }

}
