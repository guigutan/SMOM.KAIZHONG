using SIE.Domain;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 绩效目标控制器
    /// </summary>
    public class KpiController : DomainController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public virtual EntityList<KpiModel> GetKpiList(ModuleCategory category)
        {
            return Query<KpiModel>().Where(p => p.ModuleCategory == category).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual KpiModel GetByName(string name)
        {
            return Query<KpiModel>().Where(p => p.Name == name).FirstOrDefault();
        }
    }
}
