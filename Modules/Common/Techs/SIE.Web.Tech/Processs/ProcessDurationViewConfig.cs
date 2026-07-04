using DevExpress.DataProcessing;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.Common.Import.Commands;
using System.Collections.Generic;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序加工时长视图配置
    /// </summary>
    public class ProcessDurationViewConfig : WebViewConfig<ProcessDuration>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(ImportExcelCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.Process).HasLabel("工序");
            View.Property(p => p.Product).UsePagingLookUpEditor((e, o) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(o.ProductName), nameof(o.Product.Name));
                e.DicLinkField = keyValues;
            }).ShowInList(150).HasLabel("产品编码");
            View.Property(p => p.ProductName).ShowInList(250);
            View.Property(p => p.Durations).ShowInList(150);
        }

        protected override void ConfigImportView()
        {
            View.Property(p => p.Process).ImportIndexer(true).HasLabel("工序*");
            View.Property(p => p.Product).ImportIndexer(true).HasLabel("产品编码*");
            View.Property(p => p.Durations).UseSpinEditor(m => m.MinValue = 0).HasLabel("加工时长(小时)");
        }
    }
}