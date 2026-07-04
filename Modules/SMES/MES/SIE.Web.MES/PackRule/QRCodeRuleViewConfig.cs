using SIE.MES.PackRule;
using SIE.MetaModel.View;
using SIE.Web.MES.ItemLine.Commands;
using SIE.Web.MES.PackRule.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackRule
{
    /// <summary>
    /// 二维码规则表视图配置
    /// </summary>
    internal class QRCodeRuleViewConfig : WebViewConfig<QRCodeRule>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            //View.HasDelegate(QRCodeRule.NameProperty);
            //View.UseDefaultBehaviors();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseDefaultCommands();
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add,WebCommandNames.Delete, typeof(QRCodeRuleSaveCommands).FullName);
            View.Property(p => p.RuleNumber).ShowInList(width: 80);
            View.Property(p => p.RuleNumberDesc).ShowInList(width: 80);
            View.Property(p => p.CustomPnStartDigit).ShowInList(width: 140);
            View.Property(p => p.CustomPnEndDigit).ShowInList(width: 140);
            View.Property(p => p.VersionNumberStartDigit).ShowInList(width: 140);
            View.Property(p => p.VersionNumberEndDigit).ShowInList(width: 140);
            View.Property(p => p.SerialNumberStartDigit).ShowInList(width: 120);
            View.Property(p => p.SerialNumberEndDigit).ShowInList(width: 120);
            View.Property(p => p.TotalDigit).ShowInList(width: 120);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.RuleNumber);
            View.Property(p => p.RuleNumberDesc);
            View.Property(p => p.CustomPnStartDigit);
            View.Property(p => p.CustomPnEndDigit);
            View.Property(p => p.VersionNumberStartDigit);
            View.Property(p => p.VersionNumberEndDigit);
            View.Property(p => p.SerialNumberStartDigit);
            View.Property(p => p.SerialNumberEndDigit);
            View.Property(p => p.TotalDigit);
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
            View.Property(p => p.RuleNumber);
            View.Property(p => p.RuleNumberDesc);
        }
    }
}
