using SIE.Web.MES.BlueLable.Commands;
using SIE.Web.MES.ItemChecker.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BlueLable
{
    public class BlueLableLevelViewConfig : WebViewConfig<SIE.MES.BlueLable.BlueLableLevel>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(BlueLableLevelImportCommand).FullName, typeof(BlueLableLevelDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 300);
                View.Property(p => p.LevelName).ShowInList(width: 150);
                View.Property(p => p.CalcMethod).ShowInList(width: 150);
            }
        }
        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.ItemCode).ShowInList(width: 150);
            View.Property(p => p.LevelName).ShowInList(width: 150);
            View.Property(p => p.CalcMethod).ShowInList(width: 150);
        }
    }
}
