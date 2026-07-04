using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.EMS.IdleArchives;
using SIE.Equipments.Configs;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.IdleArchives
{
    /// <summary>
    /// 数据查询器
    /// </summary>
    public class IdleArchivesDataQueryer: DataQueryer
    {
        /// <summary>
        /// 获取闲置封存
        /// </summary>
        /// <returns></returns>
        public IdleArchive GetIdleArchive()
        {
           return RT.Service.Resolve<IdleArchivesController>().GetIdleArchive();
        }

       /// <summary>
       /// 获取是否启用审核功能
       /// </summary>
       /// <returns></returns>
        public bool GetEnableApproval()
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(),typeof(IdleArchive));
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config.EnableAudit;
        }
    }
}
