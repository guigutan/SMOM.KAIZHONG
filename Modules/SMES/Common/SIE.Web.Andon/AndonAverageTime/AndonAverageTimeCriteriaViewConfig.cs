using SIE.Andon.AndonAverageTime;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.AndonAverageTime
{
    /// <summary>
    /// 查询视图
    /// </summary>
    public class AndonAverageTimeCriteriaViewConfig : WebViewConfig<AndonAverageTimeViewModelCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.UseClientOrder();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ByUser).HasLabel("基于人员");

            View.Property(p => p.CreateTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
        }

    }
}