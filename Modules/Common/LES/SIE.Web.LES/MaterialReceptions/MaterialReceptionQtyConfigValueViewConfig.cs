using SIE.LES.MaterialReceptions.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.MaterialReceptions
{
    /// <summary>
    /// 物料接收配置项视图配置
    /// </summary>
    public class MaterialReceptionQtyConfigValueViewConfig : WebViewConfig<MaterialReceptionQtyConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderQty).Show(ShowInWhere.All);
            }
        }
    }
}
