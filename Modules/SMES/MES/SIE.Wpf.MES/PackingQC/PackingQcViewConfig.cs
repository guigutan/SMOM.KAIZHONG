using SIE.MES.PackingQC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.PackingQC
{
    /// <summary>
    /// 装箱QC
    /// </summary>
    public class PackingQcViewConfig : WPFViewConfig<PackingQcModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).ShowInList(100).Readonly();
                View.Property(p => p.ProductLabel).ShowInList(300).Readonly();
                View.Property(p => p.PackIdent).ShowInList().Readonly();
                View.Property(p => p.Confirm).ShowInList().Readonly();
                View.Property(p => p.Item).ShowInList().Readonly();
                View.Property(p => p.ItemName).ShowInList().Readonly();
                View.Property(p => p.ProductLabel).ShowInList().Readonly();
                View.Property(p => p.Resource).ShowInList().Readonly();
            }
        }
    }
}
