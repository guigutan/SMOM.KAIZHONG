using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.Resources.Enterprises
{
    /// <summary>
    /// 生产单元视图的查询实体配置类
    /// </summary>
    public class ProductionCellViewModelCriteriaViewConfig : WPFViewConfig<ProductionCellViewModelCriteria>
    {
        /// <summary>
        /// 生产单元视图的查询配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Shop).UseResourceWorkShopEditor().Show(ShowInWhere.All); //UseShopEditor
        }
    }
}
