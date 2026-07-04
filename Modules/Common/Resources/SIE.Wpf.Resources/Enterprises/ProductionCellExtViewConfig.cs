using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Resources.Enterprises;

namespace SIE.Wpf.Resources.Enterprises
{
    /// <summary>
    /// 生产单元属性配置类
    /// Create : shilei
    /// </summary>
    class ProductionCellExtViewConfig : WPFViewConfig<ProductionCellExt>
    {
        /// <summary>
        /// 生产单元属性列表视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
            ////View.UseCommands(WPFCommandNames.FormSave);
            View.HasDetailColumnsCount(10);

            using (View.OrderProperties())
            {
                View.Property(p => p.Area).UseAreaEditor(p => p.ReloadDataOnPopping = true).Show(ShowInWhere.All).ShowInDetail(columnSpan: 2);
                View.Property(p => p.BarcodeSymbol).Show(ShowInWhere.All).ShowInDetail(hideLabel: false, columnSpan: 3);
            }
        }
    }
}
