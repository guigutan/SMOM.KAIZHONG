
using NPOI.SS.Formula.Functions;
using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.Service;
using SIE.Web.Data;
using System;
using System.Linq;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys
{
    /// <summary>
    ///  异常规则定义数据查询类
    /// </summary>
    class AnomalyMonitorQueryer : DataQueryer
    {
        /// <summary>
        /// 异常规则定义初始化
        /// </summary>
        public virtual bool Initialization() {
            return RT.Service.Resolve<AbnormalDecisionRuleService>().Initialization();
        }

        public object GetAnomalyMonitorTree(string typeName)
        {
            return RT.Service.Resolve<CollectionService>().GetAnomalyMonitorTree(typeName);
        }

        public virtual object GetInitialzationCodes()
        {
            return RT.Service.Resolve<AbnormalDecisionRuleService>().GetInitialzationCodes();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnomalRuleId"></param>
        /// <returns></returns>
        public object AbnomalRuleTest(double abnomalRuleId)
        {
             RT.Service.Resolve<AbnormalDecisionRuleService>().AbnomalRuleMultTest(abnomalRuleId);
            return true;
        }
         
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual object RunSqlScript(string sql)
        {
            RT.Service.Resolve<CollectionService>().QueryData(sql, "");
            return true;
        }


        /// <summary>
        /// 生成默认sql
        /// </summary>
        /// <param name="abnomalRuleId"></param>
        /// <returns></returns>
        public virtual object GeneralSqlByDataSource(double abnomalRuleId)
        {
            return RT.Service.Resolve<AbnormalDecisionRuleService>().GenerenDefaultSql(abnomalRuleId);
        }


        
    }
}
