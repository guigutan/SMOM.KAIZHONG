using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Packages.Packages;
using SIE.Web.Common.Commands;
using SIE.Web.Packages.Packages.Commands;
using System.Collections.Generic;

namespace SIE.WPF.Packages.Packages
{
    /// <summary>
    /// 复核包装规则视图配置
    /// </summary>
    internal class RePackageRuleViewConfig : WebViewConfig<RePackageRule>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Copy);
            View.ReplaceCommands(EnableCommand.CommandName, typeof(RePackageEnableCommand).FullName);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.CustomerId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CustomerName), nameof(e.Customer.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.CustomerName).Readonly();
            View.Property(p => p.State).DefaultValue((int)State.Disable).Readonly();
            View.ChildrenProperty(p => p.DetailList).HasLabel("规则明细");
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Customer);
            View.Property(p => p.State).UseEnumEditor(p => { p.IsEnumNull = true; });
        }
    }
}