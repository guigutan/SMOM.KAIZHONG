using SIE.LES.MaterialPreparations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations.ViewModels
{
    /// <summary>
    /// 车间备料选择物料视图
    /// </summary>
    public class MaterialPrepareItemViewConfig : WebViewConfig<MaterialPrepareItemViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).Readonly().ShowInList();
                View.Property(p => p.ItemName).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.ConsumeMode).Readonly().ShowInList();
            }
        }
    }

    /// <summary>
    /// 车间备料选择物料查询视图
    /// </summary>
    public class MaterialPrepareItemCriteriaViewConfig : WebViewConfig<MaterialPrepareItemCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ConsumeMode).Show();
            }
        }
    }
}
