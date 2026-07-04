using SIE.Packages;
using SIE.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Packages.Packages
{
    /// <summary>
    /// 包装规则查询器
    /// </summary>
    public class PackageRuleDataQuery : DataQueryer
    {
        /// <summary>
        /// 设置产品机型
        /// </summary>
        /// <param name="selPackageRuleList">productModel</param>
        /// <returns>true</returns>
        public bool SetItemPackageRule(List<SelectPackageRule> selPackageRuleList)
        {
            var ctl = RT.Service.Resolve<PackageController>();
            if (selPackageRuleList.Count > 0)
            {
                List<double> packageRuleIdList = selPackageRuleList.Select(p => p.PackageRuleId).ToList();
                var packageRuleList = ctl.GetPackageRules(packageRuleIdList).ToList();
                ctl.CreateItemPackageRule(packageRuleList, selPackageRuleList.Select(p => p.ItemId).FirstOrDefault());
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 选择包装规则数据
    /// </summary>
    public class SelectPackageRule
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 包装规则Id
        /// </summary>
        public double PackageRuleId { get; set; }
    }
}
