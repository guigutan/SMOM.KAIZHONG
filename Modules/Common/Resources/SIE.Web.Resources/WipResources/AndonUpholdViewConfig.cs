using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Resources.WipResources
{
    public class AndonUpholdViewConfig : WebViewConfig<AndonUphold>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonDesc).ShowInList(width: 150);
                View.Property(p => p.AndonCode).ShowInList(width: 150);
                View.Property(p => p.PlantCode).ShowInList(width: 150);
                View.Property(p => p.PlantName).ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.AndonDesc).ShowInList(width: 150);
            View.Property(p => p.AndonCode).ShowInList(width: 150);
        }
    }
}
