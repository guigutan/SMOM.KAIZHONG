using SIE.Common.InvOrg;
using SIE.Items.Units;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 单位初始化控制器
    /// </summary>
    public class UnitInitController : IInvOrgInit
    {
        /// <summary>
        /// 新建组织初始化单位数据
        /// </summary>         
        public virtual void InvOrgInitData()
        {
            //单位初始化
            if (!RT.Service.Resolve<UnitsController>().CheckInitUnitList())
            {
                RT.Service.Resolve<UnitsController>().InsertUnitList();
            }
            RT.Service.Resolve<PackageController>().InitPackingUnit();
        }
    }
}
