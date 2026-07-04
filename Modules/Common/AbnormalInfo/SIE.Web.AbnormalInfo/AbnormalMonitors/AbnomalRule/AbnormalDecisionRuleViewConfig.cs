using Newtonsoft.Json;
using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Web.AbnormalInfo.AbnormalMonitors.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule
{
    /// <summary>
    /// 异常判定规则视图配置
    /// </summary>
    internal class AbnormalDecisionRuleViewConfig : WebViewConfig<AbnormalDecisionRule>
    {
        /// <summary>
        /// 设置填写规则视图
        /// </summary>
        public const string WritingRuleView = "WritingRule";

        /// <summary>
        /// 数据源设置视图
        /// </summary>
        public const string WritingDataSourceView = "WritingDataSource";


        /// <summary>
        /// 指标运算视图
        /// </summary>
        public const string RuleIndicatorView = "RuleIndicator";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.RequierModels(typeof(IndicatorRuleViewModel));
            View.AssignAuthorize(typeof(IndicatorRuleViewModel));
            View.AssignAuthorize(typeof(LayerConditions));
            View.AssignAuthorize(typeof(IndicatorCondition));
            View.AssignAuthorize(typeof(AbnomalRuleWhere));
            View.DeclareExtendViewGroup(WritingRuleView, WritingDataSourceView, RuleIndicatorView);
            if (ViewGroup == WritingRuleView)
            {
                ConfigWritingRuleView();
            }
            if (ViewGroup == WritingDataSourceView)
            {
                ConfigWritingDataSourceView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Copy);
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddAnomalyMonitorsCommand).FullName);
            View.UseCommands(typeof(WritingRuleCommand).FullName);
            View.UseCommands("SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.AbnomalRuleTestCommand");//初始化按钮
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.RuleName);
            View.Property(p => p.AbnormalType).HasLabel("异常类型");
            View.Property(p => p.AbnomalSourceId).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).HasLabel("异常来源");
            View.Property(p => p.IndicatorOperation).Readonly();
            View.ChildrenProperty(p => p.IndicatorCondtionList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.WhereList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.LayerConditionsList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.TabRelationList).Show(ChildShowInWhere.Hide);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.RuleName);
            View.Property(p => p.AbnomalSource).HasLabel("异常来源");
            View.Property(p => p.UpdateDate).HasLabel("更新时间").UseDateRangeEditor(p => {p.DateRangeType = ObjectModel.DateRangeType.Week; });

        }

        /// <summary>
        /// 填写报告视图配置
        /// </summary>
        void ConfigWritingRuleView()
        {
            View.ClearCommands();
            //View.Property(p => p.DisPlaySelect).HasLabel("显示列(Select)").UseMemoEditor().ShowInDetail(columnSpan: 5).Show(ShowInWhere.All);
            View.Property(p => p.IsSQL).Show(ShowInWhere.All);
            View.ChildrenProperty(p => p.WhereList).HasLabel("数据源配置").HasOrderNo(1).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.IndicatorCondtionList).HasLabel("指标运算列表").HasOrderNo(2).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.LayerConditionsList).HasLabel("层别条件").HasOrderNo(3).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 数据源视图配置
        /// </summary>
        void ConfigWritingDataSourceView()
        {
            View.ClearCommands();
            View.Property(p => p.DisPlaySelect).HasLabel("显示列(Select)").UseMemoEditor().ShowInDetail(columnSpan: 5).Show(ShowInWhere.All);
            View.ChildrenProperty(p => p.WhereList).HasLabel("条件范围列表").HasOrderNo(1).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.IndicatorCondtionList).HasLabel("指标运算列表").HasOrderNo(2).Show(ChildShowInWhere.All);

        }

        protected override void ConfigSelectionView()
        {
            View.DisableEditing();
            View.Property(p => p.Code).Show();
            View.Property(p => p.RuleName).Show();
            View.Property(p => p.AbnormalType).HasLabel("异常类型").Show();
            View.Property(p => p.AbnomalSourceId).HasLabel("异常来源").Show();
            View.Property(p => p.IndicatorOperation).Show();
        }
    }
}