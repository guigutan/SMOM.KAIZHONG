using SIE.Domain;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.Web.ERPInterface.UploadTransactionRules.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.ERPInterface.UploadTransactionRules
{
    /// <summary>
    /// 交易上传规则 视图
    /// </summary>
    public class UploadTransactionRuleViewConfig : WebViewConfig<UploadTransactionRule>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("交易上传规则").HasDelegate(UploadTransactionRule.NameProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommand(typeof(InitTransRuleCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Name).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
                View.Property(p => p.ActivationDate);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Name);
                View.Property(p => p.ActivationDate);
            }
        }
    }
}
