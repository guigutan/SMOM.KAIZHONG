using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.Items.ProductModels.Commands;
using System.Collections.Generic;

namespace SIE.Web.Items
{
    /// <summary>
    /// 机型技能视图配置
    /// </summary>
    internal class ProductModelSkillViewConfig : WebViewConfig<ProductModelSkill>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
		protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Save).UseCommands(typeof(ProductModelSkillImportCommand).FullName);
            View.RemoveCommands(WebCommandNames.Copy);
            View.Property(p => p.Skill).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.SkillCode), nameof(e.Skill.Code));
                m.DicLinkField = dic;
            }).HasLabel("技能名称").HasOrderNo(1);
            View.Property(p => p.SkillCode).Readonly().HasLabel("技能编码*").HasOrderNo(2);
           
            View.Property(p => p.DemandQty).UseSpinEditor(e => e.MinValue = 0);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

        /// <summary>
        /// 导入模板
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Skill).HasLabel("编码").ImportIndexer();
            View.Property(p => p.SkillName).HasLabel("技能名称");
            View.Property(p => p.DemandQty);
        }
    }
}
