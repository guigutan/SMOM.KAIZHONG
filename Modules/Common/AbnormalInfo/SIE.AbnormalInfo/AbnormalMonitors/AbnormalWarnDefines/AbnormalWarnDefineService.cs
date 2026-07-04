using MimeKit;
using SIE.AbnormalInfo.AbnormalMonitors.ViewModels;
using SIE.AbnormalInfo.Common;
using SIE.Common.Alert;
using SIE.Common.Sender;
using SIE.Core.Common;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIE.Core.Common;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常定义Service
    /// </summary>
    public class AbnormalWarnDefineService : DomainService
    {
        private readonly AbnormalWarnDefineDao _abnormalWarnDefineDao;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalWarnDefineDao"></param>
        public AbnormalWarnDefineService(AbnormalWarnDefineDao abnormalWarnDefineDao)
        {
            _abnormalWarnDefineDao = abnormalWarnDefineDao;
        }

        #region 查询

        /// <summary>
        /// 查询异常预警定义
        /// </summary>
        /// <param name="criteria">异常预警定义查询实体</param>
        /// <returns>查询异常预警定义列表</returns>
        public virtual EntityList<AbnormalWarnDefine> GetAbnormalWarnDefines(AbnormalWarnDefineCriteria criteria)
        {
            var list = _abnormalWarnDefineDao.GetAbnormalWarnDefines(criteria);
            this.SetEmployeeNames(list);
            return list;
        }

        /// <summary>
        /// 设置多选工单号
        /// </summary>
        /// <param name="list"></param>
        private void SetEmployeeNames(EntityList<AbnormalWarnDefine> list)
        {
            var billsWithWorkOrders = list.Where(p => p.JoinEmployeeIds.IsNotEmpty());
            if (!billsWithWorkOrders.Any()) return;
            var strIds = billsWithWorkOrders.SelectMany(p => p.JoinEmployeeIds.Split(',')).Distinct().ToList();
            if (strIds?.Count > 0)
            {
                var employeeIds = strIds.ConvertAll<double>(p => Convert.ToDouble(p));
                var employees = RT.Service.Resolve<EmployeeController>().GetEmployeeByIds(employeeIds);
                if (employees?.Count > 0)
                {
                    foreach (var bill in billsWithWorkOrders)
                    {
                        if (bill.JoinEmployeeIds.IsNullOrEmpty()) continue;
                        var tempIds = bill.JoinEmployeeIds.Split(',');
                        if (tempIds.IsNullOrEmpty()) continue;
                        bill.JoinEmployeeNames = String.Join(StringCommon.SplitStr, employees.Where(p => tempIds.Contains(p.Id.ToString())).Select(p => p.Name));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual AbnormalWarnDefine GetAbnormalWarnDefine(string name)
        {
            return _abnormalWarnDefineDao.Get(name);
        }

        #endregion

        #region 验证

        /// <summary>
        /// 验证推送升级机制重复
        /// </summary>
        /// <param name="warnDefine"></param>
        /// <returns></returns>
        public virtual bool PushUpgradeRuleDuplicate(AbnormalWarnDefine warnDefine)
        {
            warnDefine.GetAllChildData<AbnormalWarnDefine,PushUpgradeRule>();

            var upgradeRuleList = warnDefine.UpgradeRuleList;
            bool result = upgradeRuleList.ToList().GroupBy(c => new { c.AbnormalNode, c.Time, c.UnitType, c.PusherId }).Any(g => g.Count() > 1);

            return result;
        }

        /// <summary>
        /// 验证推送对象重复
        /// </summary>
        /// <param name="upgradeRule"></param>
        /// <returns></returns>
        public virtual bool PushUpgradeRuleDuplicate(PushUpgradeRule upgradeRule)
        {
            upgradeRule.GetAllChildData<PushUpgradeRule, PushTarget>();

            var targetList = upgradeRule.TargetList;
            bool result = targetList.ToList().GroupBy(c => new { c.TargetCode, c.TargetId, c.TargetName, c.TargetType }).Any(g => g.Count() > 1);

            return result;
        }

        #endregion
    }
}
