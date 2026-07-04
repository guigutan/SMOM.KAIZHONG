using SIE.TurnoverTools.TurnoverTools;
using SIE.Packages.Boxs;
using SIE.Web.Common;
using SIE.Web.Elec.MES.TurnoverTools.Commands;
using System.Collections.Generic;
using SIE.CSM.Customers;

namespace SIE.Web.Elec.MES.TurnoverTools
{
    /// <summary>
    /// 周转工具型号视图配置
    /// </summary>
    internal class TurnoverToolModelViewConfig : WebViewConfig<TurnoverToolModel>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(TurnoverToolModelImportCommand).FullName, "SIE.Web.Elec.MES.TurnoverTools.Commands.SetModelDedicatedCommand", "SIE.Web.Elec.MES.TurnoverTools.Commands.CancelModelDedicatedCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ToolType).UseCatalogEditor(e =>
            {
                e.CatalogType = TurnoverBox.BoxTypeCatalog;
                e.CatalogReloadData = true;
            }).UseListSetting(e => { e.HelpInfo = "周转工具型号快码“BOX_TYPE”"; });
            View.Property(p => p.DefaultCapacity).UseSpinEditor(p => { p.AllowDecimals = false; p.MinValue = 0; });
            View.Property(p => p.Customer).UseDataSource((o, c, r) =>
            {
                return RT.Service.Resolve<CustomerController>().GetEnableCustomers(c, r);
            }).HasLabel("客户").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CustomerName), nameof(e.Customer.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.CustomerName).Readonly();
            View.Property(p => p.IsDedicated).Readonly();
            View.Property(p => p.Length).UseSpinEditor(p => { p.DecimalPrecision = 2; p.Step = 1; p.MinValue = 0; });
            View.Property(p => p.Width).UseSpinEditor(p => { p.DecimalPrecision = 2; p.Step = 1; p.MinValue = 0; });
            View.Property(p => p.Height).UseSpinEditor(p => { p.DecimalPrecision = 2; p.Step = 1; p.MinValue = 0; });
            View.Property(p => p.Supplier).HasLabel("供应商").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.SupplierName).Readonly();
            View.ChildrenProperty(p => p.ProductList).HasLabel("产品容量");
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ToolType).UseCatalogEditor(e => { e.CatalogType = TurnoverBox.BoxTypeCatalog; e.CatalogReloadData = true; });
            View.Property(p => p.Customer).HasLabel("客户编码");
            View.Property(p => p.Supplier).HasLabel("供应商编码");
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置默认视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ToolType).UseCatalogEditor(e => { e.CatalogType = TurnoverBox.BoxTypeCatalog; e.CatalogReloadData = true; });
        }
    }
}