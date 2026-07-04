using SIE.MES.PackingQC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.PackingQC
{
    /// <summary>
    /// 装箱QC实体查询
    /// </summary>
    public class PackingQcCriterialViewConfig : WPFViewConfig<PackingQcCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).ShowInList();
                View.Property(p => p.PackIdent).ShowInList();
                View.Property(p => p.Confirm).ShowInList();
            }
        }
    }
}
