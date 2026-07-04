using SIE.MES.Andon;
using SIE.MES.Fixture;
using SIE.MetaModel.View;
using SIE.Web.MES.Andon.Commands;
using SIE.Web.MES.Fixture.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Andon
{
    /// <summary>
    /// 安灯区域视图配置
    /// </summary>
    public class AndonUpholdViewConfig : WebViewConfig<AndonUphold>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(AndonUpholdSaveCommands).FullName);
            View.UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonDesc).ShowInList(width: 150);
                View.Property(p => p.AndonCode).ShowInList(width: 150);
                View.Property(p => p.AndonEntity).ShowInList(width:150);
                View.Property(p => p.AndonOrder).ShowInList(width: 150);
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

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.AndonDesc).ShowInList(width: 150);
            View.Property(p => p.AndonCode).ShowInList(width: 150);
            View.Property(p => p.AndonEntity).ShowInList(width: 150);
            View.Property(p => p.AndonOrder).ShowInList(width: 150);
        }
    }
}
