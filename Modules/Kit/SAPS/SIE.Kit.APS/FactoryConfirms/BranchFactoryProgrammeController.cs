using SIE.Domain;
using System.Linq;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 分厂方案控制器
    /// </summary>
    public class BranchFactoryProgrammeController : DomainController
    {
        /// <summary>
        /// 获得所有分厂方案数据
        /// </summary>
        /// <returns>获得所有分工方案列表</returns>
        public virtual EntityList<BranchFactoryProgramme> GetBranchFactoryProgrammeAll()
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(BranchFactoryProgramme.BranchFactiryProgrammeDetailListProperty);
            return Query<BranchFactoryProgramme>().ToList(null, elo);
        }

        /// <summary>
        /// 判断分配方案是否有明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int CountBranchFactoryProgrammeDetail(double id)
        {
            return Query<BranchFactoryProgrammeDetail>().Where(x => x.BranchFactoryProgrammeId == id).Count();
        }
    }
}
