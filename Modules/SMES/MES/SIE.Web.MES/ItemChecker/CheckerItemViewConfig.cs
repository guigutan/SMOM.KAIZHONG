using SIE.MES.ItemChecker;
using SIE.MetaModel.View;
using SIE.Web.Common.Commands;
using SIE.Web.MES.ItemChecker.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemChecker
{
    /// <summary>
    /// 检具与产品的关系视图配置
    /// </summary>
    public class CheckerItemViewConfig : WebViewConfig<CheckerItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, typeof(CheckerItemEditCommand).FullName, WebCommandNames.Delete, typeof(CheckerItemSaveCommand).FullName);
            //View.ReplaceCommands(EnableCommand.CommandName, typeof(CheckerItemEnableCommand).FullName);
            //View.ReplaceCommands(DisableCommand.CommandName, typeof(CheckerItemDisableCommand).FullName);
            View.UseCommands(typeof(CheckerItemImportCommand).FullName, typeof(CheckerItemDLTemplateCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.CheckerUphold).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.CheckerName), nameof(e.CheckerUphold.CheckerName));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.CheckerName).Readonly().ShowInList(width: 300);
                View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 300);
                View.Property(p => p.Process).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProcessCode), nameof(e.Process.Code));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200);
                View.Property(p => p.ProcessCode).Readonly().ShowInList(width: 150);
                View.Property(p => p.DrawingNo).Readonly().ShowInList(width: 150);
            }
        }


        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.CheckerUphold.CheckerCode).HasLabel("检具编码");
            View.PropertyRef(p => p.Item.Code).HasLabel("产品编码");
            View.PropertyRef(p => p.Process.Code).HasLabel("标准文本码");
            View.Property(p => p.DrawingNo).Show().HasLabel("图号");
        }
    }
}
