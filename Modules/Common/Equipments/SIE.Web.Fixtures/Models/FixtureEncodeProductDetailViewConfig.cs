using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models
{
    /// <summary>
    /// 工治具编码（产品清单）视图配置
    /// </summary>
    internal class FixtureEncodeProductDetailViewConfig : WebViewConfig<FixtureEncodeProductDetail>
    {

        /// <summary>
        /// 显示宽度
        /// </summary>
        private const int displayCoulmnWidth = 20;

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, typeof(ImmediateDeleteCommand).FullName);
            View.UseCommands(typeof(SIE.Web.Common.Import.Commands.ImportExcelCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            View.Property(p => p.ItemId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                m.DicLinkField = keyValues;
            }).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ItemController>().GetItemList(keyword, pagingInfo);
            }).HasLabel("产品编码").ShowInList(displayCoulmnWidth*8);
            View.Property(p => p.ItemName).Readonly().HasLabel("产品名称").ShowInList(displayCoulmnWidth * 6);
            View.Property(p => p.Deck).ShowInList(displayCoulmnWidth *5);
            View.Property(p => p.ProcessSegment).HasLabel("工段").ShowInList(displayCoulmnWidth * 4);
            View.Property(p => p.DemandQuantity).DefaultValue(1).UseSpinEditor(p => { p.MinValue = 1; p.DecimalPrecision = 0; p.AllowBlank = false; }).ShowInList(displayCoulmnWidth * 3);
        }


       /// <summary>
       /// 配置导入
       /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.FixtureEncode.Code).HasLabel("工治具编码");
            View.PropertyRef(p=>p.Item.Code).HasLabel("产品编码");
            View.Property(p => p.Deck).HasLabel("工艺面");
            View.PropertyRef(p => p.ProcessSegment.Name).HasLabel("工段");
            View.Property(p => p.DemandQuantity).HasLabel("需求数量");
        }
    }
}