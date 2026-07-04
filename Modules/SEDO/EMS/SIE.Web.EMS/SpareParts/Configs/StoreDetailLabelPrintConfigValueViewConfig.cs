using SIE.EMS.AssetDisposals;
using SIE.EMS.SpareParts.Configs;
using SIE.EMS.SpareParts.Printables;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.Configs
{
    /// <summary>
    ///  备件入库单号视图配置
    /// </summary>
    public class StoreDetailLabelPrintConfigValueViewConfig : WebViewConfig<StoreDetailLabelPrintConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.LotPrintTemplateId).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<AssetDisposalController>().GetPrintTemplatesByType(typeof(StoreDetailLotPrintable).GetQualifiedName(), p, r);
            });
            View.Property(p => p.SnPrintTempldateId).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<AssetDisposalController>().GetPrintTemplatesByType(typeof(StoreDetailSnPrintable).GetQualifiedName(), p, r);
            });
        }
    }
}
