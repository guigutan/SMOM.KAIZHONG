using SIE.MES.ItemFixture;
using SIE.MetaModel.View;
using SIE.Web.Common.Commands;
using SIE.Web.MES.ItemFixture.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemFixture
{
    /// <summary>
    /// 工装与产品的关系视图配置
    /// </summary>
    public class FixtureItemViewConfig : WebViewConfig<FixtureItem>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, typeof(FixtureItemEditCommand).FullName, WebCommandNames.Delete, typeof(FixtureItemSaveCommand).FullName);
            //View.ReplaceCommands(EnableCommand.CommandName, typeof(FixtureItemEnableCommand).FullName);
            //View.ReplaceCommands(DisableCommand.CommandName, typeof(FixtureItemDisableCommand).FullName);
            View.UseCommands(typeof(FixtureItemImportCommand).FullName, typeof(FixtureItemDLTemplateCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p=>p.FixtureUphold).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.FixtureName), nameof(e.FixtureUphold.FixtureName));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 200).HasLabel("工装唯一码");
                View.Property(p => p.FixtureName).Readonly().ShowInList(width: 300).HasLabel("工装物料描述");
                View.Property(p => p.Drawn).Readonly().ShowInList(width: 300);
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
            }
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.FixtureUphold.FixtureCode).HasLabel("工装唯一码");
            View.PropertyRef(p => p.Item.Code).HasLabel("产品编码");
            View.PropertyRef(p => p.Process.Code).HasLabel("标准文本码");
        }
    }
}
