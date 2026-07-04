using SIE.Wpf.MES.WIP.Assemblys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Effects;
using DevExpress.Xpf.Grid;

namespace SIE.Wpf.MES.WIP.ViewBehaviors
{
    /// <summary>
    /// 上料采集装配明细VM行为
    /// </summary>
    public class AssemblyDetailViewBehavior : ViewBehavior
    {
        private const string expressionString = "[{0}] > [{1}]";

        /// <summary>
        /// 附加视图行为
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnAttach()
        {
            var listView = View as ListLogicalView;
            var tableView = listView.Control.View as TableView;
            
            var formatConditions = new FormatCondition();
            formatConditions.ApplyToRow = true;
            formatConditions.ValueRule = DevExpress.Xpf.Core.ConditionalFormatting.ConditionRule.Expression;
            formatConditions.Value1 = true;
            formatConditions.Expression = expressionString.FormatArgs(AssemblyDetailViewModel.DemandQtyProperty.Name, AssemblyDetailViewModel.QtyProperty.Name);
            formatConditions.Format = new DevExpress.Xpf.Core.ConditionalFormatting.Format() { Foreground = Brushes.Red };
            tableView.FormatConditions.Add(formatConditions);
        }
    }
}
