using SIE.AbnormalInfo.AbnormalInfos;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.AbnormalInfo._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.AbnormalInfoss
{
    /// <summary>
    /// 异常信息分类视图配置
    /// </summary>
    public class AbnormalInfoDefinitionViewConfig : WebViewConfig<AbnormalInfoDefinition>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.AbnormalInfo.AbnormalInfos.Commands.AddAbnormalInfoDefinitionCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.AbnormalInfo.AbnormalInfos.Commands.EditAbnormalInfoDefinitionCommand");
            View.UseCommands("SIE.Web.AbnormalInfo.AbnormalInfos.Commands.AddAbnormalInfoCategoryCommand");
            View.Property(p => p.AbnormalSource).Cascade(p => p.Code, null).Cascade(p => p.Desc, null);
            View.Property(p => p.Code).UseAbnormalCodeEditor();
            View.Property(p => p.Desc).Readonly(p => p.AbnormalSource == AbnormalSource.Alert);
            View.Property(p => p.AbnormalCategoryId).HasLabel("异常分类编码").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.AbnormalCategoryDesc), nameof(e.AbnormalCategory.Desc));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.AbnormalCategoryDesc).ShowInList(width: 200).HasLabel("异常分类描述").Readonly();
            //View.Property(p => p.DefectLevel).UseCatalogEditor(e => { e.CatalogReloadData = true; e.CatalogType = AbnormalInfoDefinition.LevelCatalog; });
            View.ChildrenProperty(p => p.SendUpgradeSetList);
            View.ChildrenProperty(p => p.HandlerList);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.AbnormalCategoryId).HasLabel("分类编码").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.AbnormalCategoryDesc), nameof(e.AbnormalCategory.Desc));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.AbnormalCategoryDesc).HasLabel("分类描述").Readonly();
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.AbnormalSource).Readonly();
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.Desc).Readonly();
            View.Property(p => p.AbnormalCategoryId).HasLabel("异常分类编码").Readonly();
            View.Property(p => p.AbnormalCategoryDesc).ShowInList(width: 200).HasLabel("异常分类描述").Readonly();
            //View.Property(p => p.DefectLevel).Readonly();
        }
    }
}
