using SIE.Common.Schdules;
using SIE.Core.Enums;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.Allocate;
using SIE.Inventory.Transactions;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Job.SyncResources
{
    /// <summary>
    /// 同步资源
    /// </summary>
    [Job("生产资源-同步资源", typeof(JobParameter))]
    public class SyncResourceJob : JobBase
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="param"></param>
        protected override void ExecuteJob(object param)
        {
            if (RT.Service.Resolve<ErpJobCloseRuleController>().ValidateInCloseTime())
                return;
         
            AddLog("{0}同步资源开始。".L10nFormat(DateTime.Now));
            try
            {
                SyncReourceContainer.SyncResource();
                AddLog("{0}同步资源结束。".L10nFormat(DateTime.Now));
            }
            catch (Exception ex)
            {
                AddLog("{1}同步资源失败：{0}。".L10nFormat(ex.Message,DateTime.Now));
            }
        }
    }
}
