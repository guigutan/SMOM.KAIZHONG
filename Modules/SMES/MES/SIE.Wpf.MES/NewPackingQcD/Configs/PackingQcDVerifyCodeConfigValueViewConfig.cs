using SIE.MES.PackingQC.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.NewPackingQcD.Configs
{
    public class PackingQcDVerifyCodeConfigValueViewConfig : WPFViewConfig<PackingQcDVerifyCodeConfigValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.CodeLength).Show(ShowInWhere.All);
            }
        }
    }
}
