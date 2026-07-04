using SIE.Wpf.MES.ConnectorPackings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ConnectorPacking
{
    /// <summary>
    /// 装箱QC
    /// </summary>
    public class ConnectorSnPackingViewConfig : WPFViewConfig<ConnectorSnPackingModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).ShowInList(100).Readonly();
                View.Property(p => p.BatchLabel).ShowInList(150).Readonly();
                View.Property(p => p.ProductLabel).ShowInList(300).Readonly();
                View.Property(p => p.PackIdent).ShowInList().Readonly();
                View.Property(p => p.Confirm).ShowInList().Readonly();
                View.Property(p => p.Item).ShowInList().Readonly();
                View.Property(p => p.ItemName).ShowInList(300).Readonly();
                View.Property(p => p.PackingNum).ShowInList().Readonly();
                View.Property(p => p.Resource).ShowInList().Readonly();
            }
        }
    }
}
