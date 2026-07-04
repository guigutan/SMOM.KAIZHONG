using SIE.MES.SpcFromMess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.SpcFromMess
{
    /// <summary>
    /// 统计过程控制-查询实体视图
    /// </summary>
    public class SpcFromMesCriteriaViewConfig : WebViewConfig<SpcFromMesCriteria>
    {
        /// <summary>
        /// 统计过程控制-查询实体视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.Name).ShowInList(width: 150);
                View.Property(p => p.ItemId).ShowInList(width: 150);
                View.Property(p => p.Project).ShowInList(width: 150);
                View.Property(p => p.ProductionDept).ShowInList(width: 150);
                View.Property(p => p.ResourceId).ShowInList(width: 150);
                View.Property(p => p.InspectorId).ShowInList(width: 150);
                View.Property(p => p.CreateTime).ShowInList(width: 150);
            }
        }
    }
}
