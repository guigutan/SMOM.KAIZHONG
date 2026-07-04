using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.EMS.RunStandards;
using SIE.Equipments.Configs;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.RunStandards
{
   /// <summary>
   ///数据查询器
   /// </summary>
    public class RunStandardsDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取运行定标
        /// </summary>
        /// <returns></returns>
        public RunStandard GetRunStandard()
        {
            return RT.Service.Resolve<RunStandardsController>().GetRunStandard();
        }

        /// <summary>
        /// 获取是否启用审核功能
        /// </summary>
        /// <returns></returns>
        public bool GetEnableApproval()
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(RunStandard));
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config.EnableAudit;
        }
    }
}
