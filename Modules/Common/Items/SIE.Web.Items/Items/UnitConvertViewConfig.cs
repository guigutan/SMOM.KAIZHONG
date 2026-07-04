using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Web.Items.Items.Commands;
using System.Collections.Generic;

namespace SIE.Web.Items.Items
{
    /// <summary>
    /// 单位转换视图配置
    /// </summary>
    public class UnitConvertViewConfig : WebViewConfig<UnitConvert>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(UnitConvert));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Items.Scripts.ItemUnitBehavior");
            View.InlineEdit().UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Save, typeof(SaveUnitConvertCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Delete, typeof(DeleteUnitConvertCommand).FullName);
            View.UseCommands(typeof(ImportUnitConvertCommand).FullName, typeof(SetDefaultUnitConvertCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).UsePagingLookUpEditor((p, m) =>
                {
                    var keyValues = new Dictionary<string, string>
                        {
                            { nameof(m.ItemName), nameof(m.Item.Name) },
                            { nameof(m.ItemUnitName), nameof(m.Item.UnitName) },
                            { nameof(m.MainUnitId), nameof(m.Item.UnitId) },
                        };
                    p.DicLinkField = keyValues;
                }).Readonly(p => p.IsBaseUnit);
                View.Property(p => p.ItemName).Readonly();
                View.Property(p => p.ItemUnitName).HasLabel("主单位*").Readonly();
                View.Property(p => p.UnitId).UseDataSource((o, c, r) =>
                {
                    var criteria = o as UnitConvert;
                    return RT.Service.Resolve<ItemController>().GetUnitList(criteria.MainUnitId, r, c);
                }).Readonly(p => p.IsBaseUnit);
                View.Property(p => p.Numerator).UseSpinEditor(p => { p.AllowDecimals = false; p.DecimalPrecision = 0; p.Step = 1; }).Readonly(p => p.IsBaseUnit).UseListSetting(e => { e.HelpInfo = "主单位数量×分子=辅助单位数量×分母"; });
                View.Property(p => p.Denominator).UseSpinEditor(p => { p.AllowDecimals = false; p.DecimalPrecision = 0; p.Step = 1; }).Readonly(p => p.IsBaseUnit);
                View.Property(p => p.IsDefault).Readonly();
                View.Property(p => p.UnitSource).DefaultValue(UnitSource.Manaul).Readonly();
                View.Property(p => p.ChangeDesc).Readonly();
                View.Property(p => p.ErpNumerator).Readonly().UseListSetting(e => { e.HelpInfo = "下载ERP资料的原始分子"; });
                View.Property(p => p.ErpDenominator).Readonly().UseListSetting(e => { e.HelpInfo = "下载ERP资料的原始分母"; });
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.UnitId);
            View.Property(p => p.UnitSource);
        }
    }
}
