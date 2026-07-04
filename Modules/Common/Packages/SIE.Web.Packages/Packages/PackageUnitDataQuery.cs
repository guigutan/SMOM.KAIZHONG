using SIE.Packages;
using SIE.Web.Data;

namespace SIE.Web.Packages.Packages
{
    /// <summary>
    /// 包装单位数据查找类
    /// </summary>
    public class PackageUnitDataQuery : DataQueryer
    {
        /// <summary>
        /// 判断是否存在主单位
        /// </summary>
        /// <returns>返回是否存在主单位</returns>
        public bool IsExistMasterUnit()
        {
            bool isExist = RT.Service.Resolve<PackageController>().IsExistsMasterUnit();
            return isExist;
        }
    }
}
