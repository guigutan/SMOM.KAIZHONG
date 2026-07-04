using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.OutDepotHandovers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.OutDepotHandovers
{
    /// <summary>
    /// 备件交接单查询视图
    /// </summary>
    public class OutDepotHandoverCriteriaViewConfig : WebViewConfig<OutDepotHandoverCriteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.HandoverNo);
            View.Property(p => p.OutDepotNo);
            View.Property(p => p.HandOverStatus);
            View.Property(p => p.SparePartId)
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    m.DicLinkField = keyValues;
                }).UseDataSource((e, p, o) =>
                {
                    return RT.Service.Resolve<SparePartController>().GetSparePartByEquipModelId(p, o, null);
                });
            View.Property(p => p.SparePartName);
            View.Property(p => p.SeriaNo);
            View.Property(p => p.BatchNo);
        }
    }
}
