using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品直通率设置视图配置
    /// </summary>
    internal class ProductFpySettingViewConfig : WebViewConfig<ProductFpySetting>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ProductFpySetting.ProductIdProperty);
            View.AssignAuthorize(typeof(ProductReportViewModel));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.Product).HasLabel("产品编码").UseDataSource((e, c, r) =>
            {
                var setting = e as ProductFpySetting;
                if (setting?.ProductModelFpySetting?.Model != null)
                    return RT.Service.Resolve<ItemController>().GetItems(ItemType.Material, c, r, setting.ProductModelFpySetting.ModelId);
                return new EntityList<Item>();
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                m.DicLinkField = keyValues;
            }).UseListSetting(e => { e.HelpInfo = "显示某产品机型下为原材料的物料"; });
            View.Property(p => p.ProductName).HasLabel("产品名称");
            View.Property(p => p.Desired).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.MaxValue = 100;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.Alarm).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.MaxValue = 100;
                p.DecimalPrecision = 2;
            });
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}