using SIE.WorkBenchCommon.Workbench.KPI;
using System;
using System.Linq;

namespace SIE.Web.WorkBenchCommon.Workbench.KPI.DataQueryer
{
    /// <summary>
    /// 
    /// </summary>
    public class QuotaTargetSettingDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取指标名称字典
        /// </summary>
        /// <param name="code">指标分类</param>
        /// <returns>作业指导书列表</returns>
        public Object GetQuotaTargetSettingNameDic(string code)
        {
            var dic = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetSettingNameDic(code);
            var dd = dic.ToList();
            return dd;
        }
    }

}
