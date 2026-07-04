using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BlueLable
{
    /// <summary>
    /// 蓝标视图查询
    /// </summary>
    public class BlueLableViewConfig : WebViewConfig<SIE.MES.BlueLable.BlueLable>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(AndonUpholdSaveCommands).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLableBox).ShowInList(width: 150);
                View.Property(p => p.ProductionNo).ShowInList(width: 150);
                View.Property(p => p.PackageNum).ShowInList(width: 150);
                View.Property(p => p.Item).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.StorageLocation).ShowInList(width: 150);
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.IsPack).Readonly().ShowInList();
                View.Property(p => p.ExternalIdent).ShowInList(width: 150);
                View.Property(p => p.Factory).ShowInList(width: 150);
                View.Property(p => p.CreateDeleteident).ShowInList(width: 150);
            }
        }
    }
}
