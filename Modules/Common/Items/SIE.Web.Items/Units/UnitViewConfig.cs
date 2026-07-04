using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Items.Units.Commands;

namespace SIE.Web.Items.Units
{
    /// <summary>
    /// 单位视图配置
    /// </summary>
    class UnitViewConfig : WebViewConfig<Unit>
    {
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Save, typeof(SaveUnitCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Delete, typeof(DeleteUnitCommand).FullName);
            View.UseCommands(typeof(InitUnitCommand).FullName);
            View.UseImportCommands();
            View.Property(p => p.Code).UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; })
                .Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.Name).Readonly(p => p.PersistenceStatus != PersistenceStatus.New && p.UnitSource == UnitSource.BaseUnit);
            View.Property(p => p.Type).Readonly(p => p.PersistenceStatus != PersistenceStatus.New && p.UnitSource == UnitSource.BaseUnit).UseListSetting(e => { e.HelpInfo = "单位快码类型(UNIT_TYPE)"; })
                .UseCatalogEditor(p => { p.CatalogReloadData = true; p.CatalogType = Unit.CatalogType; });
            View.Property(p => p.Precision).DefaultValue(0).UseSpinEditor(p => { p.AllowDecimals = false; p.DecimalPrecision = 0; p.Step = 1; });
            View.Property(p => p.UnitSource).Readonly();
            View.Property(p => p.TradeType);
        }

        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type).UseListSetting(e => { e.HelpInfo = "单位快码类型(UNIT_TYPE)"; })
                .UseCatalogEditor(p => { p.CatalogReloadData = true; p.CatalogType = Unit.CatalogType; });
            View.Property(p => p.Precision);
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type).UseCatalogEditor(p =>
            {
                p.CatalogReloadData = true;
                p.CatalogType = Unit.CatalogType;
            })
                .UseListSetting(e => { e.HelpInfo = "单位快码类型(UNIT_TYPE)"; });
        }
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.Precision);
            View.Property(p => p.TradeType);
        }
    }
}
