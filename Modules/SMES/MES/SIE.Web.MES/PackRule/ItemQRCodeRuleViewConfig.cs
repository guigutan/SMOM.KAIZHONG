using SIE.MES.PackRule;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.MES.PackRule
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class ItemQRCodeRuleViewConfig : WebViewConfig<ItemQRCodeRule>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            /*            View.HasDelegate(ItemQRCodeRule.NameProperty);
                        View.UseDefaultBehaviors();*/
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseDefaultCommands();
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.ItemId).UsePagingLookUpEditor((c, p) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(p.Code), nameof(p.Item.Code));
                dic.Add(nameof(p.Name), nameof(p.Item.Name));
                dic.Add(nameof(p.ShortDescription), nameof(p.Item.ShortDescription));
                c.DicLinkField = dic;
            }).HasLabel("物料编码").ShowInList(width: 160);
            View.Property(p => p.Name).ShowInList(width: 300); ;
            View.Property(p => p.ShortDescription).ShowInList(width: 120); ;
            View.Property(p => p.CustomPn).ShowInList(width: 120);
            View.Property(p => p.VersionNumber).ShowInList(width: 120);
            View.Property(p => p.QRCodeRuleId).UsePagingLookUpEditor((c, p) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(p.RuleNumber), nameof(p.QRCodeRule.RuleNumber));
                dic.Add(nameof(p.RuleNumberDesc), nameof(p.QRCodeRule.RuleNumberDesc));
                c.DicLinkField = dic;
            }).HasLabel("规则编号").ShowInList(width: 120);
            View.Property(p => p.RuleNumberDesc).ShowInList(width: 120);
            View.Property(p => p.IsQualityStatus).ShowInList(width: 120);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ShortDescription);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.RuleNumber);
            View.Property(p => p.RuleNumberDesc);
            View.Property(p => p.CustomPn);
            View.Property(p => p.VersionNumber);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
        }
    }


}
