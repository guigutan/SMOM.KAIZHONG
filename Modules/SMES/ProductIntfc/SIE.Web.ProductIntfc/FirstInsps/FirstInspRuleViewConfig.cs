using SIE.ProductIntfc.FirstInsps;
using SIE.Web.ProductIntfc.FirstInsps.Commands;
using System;
using System.Linq.Expressions;

namespace SIE.Web.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 首件规则视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class FirstInspRuleViewConfig : WebViewConfig<FirstInspRule>
    {
        #region 首检规则 FirstInspRule 
        /// <summary>
        /// 首检规则
        /// 首检参数是工单的默认选中并不能编辑
        /// </summary>
        public static readonly Expression<Func<FirstInspRule, bool>> FirstInspRuleProperty =
            e => e.Parameter == FirstInspParam.WorkOrder;
        #endregion

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(FirstInsp));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(FirstInsp));
            View.ClearCommands();
            View.UseCommands("SIE.Web.ProductIntfc.FirstInsps.Commands.FirstInspListCommand", typeof(ListSaveCommand).FullName);
            View.Property(p => p.Parameter).ShowInList(width: 180).Readonly();
            View.Property(p => p.IsSelect).Show().Readonly(FirstInspRuleProperty)
                        .UseListSetting(e => { e.HelpInfo = "报检参数等于工单不可编辑"; });
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
