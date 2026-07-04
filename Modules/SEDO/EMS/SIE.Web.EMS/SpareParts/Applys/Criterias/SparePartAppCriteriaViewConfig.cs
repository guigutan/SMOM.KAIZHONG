using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Criterias;
using SIE.Equipments.EquipModels;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.Applys.Criterias
{
    /// <summary>
    /// 配置视图
    /// </summary>
    public class SparePartAppCriteriaViewConfig : WebViewConfig<SparePartAppCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.FromNo);
            View.Property(p => p.FromType);
            View.Property(p => p.AuditState);
            View.Property(p => p.EquipAccountCode);
            View.Property(p => p.EquipModel).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
            });
            View.Property(p => p.SparePart)
                .UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<SparePartController>().GetSpareParts(pagingInfo, keyword);
                })
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    m.DicLinkField = keyValues;
                    m.BindDisplayField = SparePartAppCriteria.SparePartCodeProperty.Name;
                    m.DisplayField = SparePartAppCriteria.SparePartCodeProperty.Name;
                });
            View.Property(p => p.SparePartName);
            View.Property(p => p.CreateDate)
                .UseDateRangeEditor(e => { e.DateRangeType = ObjectModel.DateRangeType.All; });
        }
    }
}
