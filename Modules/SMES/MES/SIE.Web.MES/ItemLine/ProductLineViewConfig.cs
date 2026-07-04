using SIE.MES.ItemLine;
using SIE.MES.MtartProcessLookups;
using SIE.MetaModel.View;
using SIE.Web.Common.Commands;
using SIE.Web.MES.ItemLine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemLine
{
    /// <summary>
    /// 产品与产线的关系视图配置
    /// </summary>
    public class ProductLineViewConfig:WebViewConfig<ProductLine>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //WebCommandNames.Edit
            View.UseCommands(WebCommandNames.Add,typeof(ProductLineEditCommands).FullName , WebCommandNames.Delete,typeof(ProductLineSaveCommands).FullName);
            //View.ReplaceCommands(EnableCommand.CommandName, typeof(ProductLineEnableCommand).FullName);
            //View.ReplaceCommands(DisableCommand.CommandName, typeof(ProductLineDisableCommand).FullName);
            View.UseCommands(typeof(ProductLineImportCommand).FullName, typeof(ProductLineDLTemplateCommand).FullName, typeof(ProductLineImportItemCommand).FullName, typeof(ProductLineImportWipResourceCommand).FullName, typeof(ProductLineImportProcessCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200).HasLabel("产品编码");
                View.Property(p => p.ItemName).Readonly().ShowInList(width:300);
                View.Property(p => p.WipResource).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.LineName), nameof(e.WipResource.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.LineName).Readonly().ShowInList(width: 150);
                //View.Property(p => p.Process).UsePagingLookUpEditor((m, e) =>
                //{
                //    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                //    keyValues.Add(nameof(e.ProcessName), nameof(e.Process.Name));
                //    m.DicLinkField = keyValues;
                //}).ShowInList(width: 200).HasLabel("工序编码");
                View.Property(p => p.ProcessId).Show().UsePagingLookUpEditor((x, y) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(y.ProcessName), nameof(y.Process.Name));
                    dic.Add("ProcessId_Display", nameof(y.Process.Code));
                    x.DisplayField = nameof(y.Process.Code);
                    x.BindDisplayField = nameof(ProductLine.ProcessCode);
                    x.DicLinkField = dic;
                }).HasLabel("工序编码");
                View.Property(p => p.ProcessName).Readonly().ShowInList(width: 150);
                //View.Property(p => p.State).Readonly().ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Item.Code).HasLabel("产品编码");
            View.PropertyRef(p => p.WipResource.Code).HasLabel("产线/机台编码");
            View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
        }
    }
}
