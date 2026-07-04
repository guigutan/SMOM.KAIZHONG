using DocumentFormat.OpenXml.Wordprocessing;
using SIE.MetaModel.View;
using SIE.Web.MES.Andon.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Threshold
{
    internal class ThresholdViewConfig : WebViewConfig<SIE.MES.Threshold.Threshold>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(AndonUpholdSaveCommands).FullName);
            View.UseCommands(typeof(Andon.Commands.ThresholdImportCommand).FullName, typeof(Andon.Commands.ThresholdDLTemplateCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            //View.UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Process).UsePagingLookUpEditor((m, e) => {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProcessCode), nameof(e.Process.Code));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 150).HasLabel("工序名称");
                View.Property(p => p.ProcessCode).ShowInList(width: 150).Readonly();
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 300);
                View.Property(p => p.ThresholdValue).ShowInList(width: 150);
                View.Property(p => p.AlertValue).ShowInList(width: 150);
            }
        }

        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
            View.PropertyRef(p => p.Item.Code).HasLabel("产品编码");
            View.Property(p => p.ThresholdValue).ShowInList(width: 150);
            View.Property(p => p.AlertValue).ShowInList(width: 150);
        }
    }
}
