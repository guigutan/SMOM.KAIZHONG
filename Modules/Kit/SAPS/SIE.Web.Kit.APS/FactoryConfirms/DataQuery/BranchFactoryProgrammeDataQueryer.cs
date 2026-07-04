using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Web.Data;

namespace SIE.Web.Kit.APS.FactoryConfirms.DataQuery
{
    /// <summary>
    /// 查询分厂方案
    /// </summary>
    public class BranchFactoryProgrammeDataQueryer : DataQueryer
    {
        /// <summary>
        /// 查询分厂方案
        /// </summary>
        /// <returns></returns>
        public EntityList<BranchFactoryProgramme> ExpandBranchFactory()
        {
            var ctrl = RT.Service.Resolve<BranchFactoryProgrammeController>();
            return ctrl.GetBranchFactoryProgrammeAll();
        }
    }
}
