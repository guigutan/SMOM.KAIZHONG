using SIE.Domain;
using SIE.ObjectModel;
using SIE.ProductIntfc.FirstInsps;
using SIE.Wpf.ProductIntfc.FirstInsps.Commands;

namespace SIE.Wpf.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 首件规则视图类
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class FirstInspRuleViewConfig : WPFViewConfig<FirstInspRule>
    {
        #region 首检规则 FirstInspRule
        /// <summary>
        /// 首检规则
        /// 首检参数是工单的默认选中并不能编辑
        /// </summary>
        [Label("自定义")]
        public static readonly Property<bool> FirstInspRuleProperty = P<FirstInspRule>.RegisterExtensionReadOnly("FirstInspRule", typeof(FirstInspRuleViewConfig),
            GetFirstInspRule, FirstInspRule.ParameterProperty);

        /// <summary>
        /// 按产品分批
        /// </summary>
        /// <param name="me">实体ItemBatchRule</param>
        /// <returns>bool</returns>
        public static bool GetFirstInspRule(FirstInspRule me)
        {
            bool flag = me.Parameter == FirstInspParam.WorkOrder;
            if (flag)
                me.IsSelect = true;
            return flag;
        }
        #endregion 

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(FirstInspListCommand), WPFCommandNames.ListSave);
            View.Property(p => p.Parameter).Show().Readonly();
            View.Property(p => p.IsSelect).Show().Readonly(FirstInspRuleProperty);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
