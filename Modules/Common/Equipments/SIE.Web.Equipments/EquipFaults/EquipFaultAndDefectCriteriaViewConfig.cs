using SIE;
using SIE.Defects;
using SIE.Domain;
using SIE.Equipments.EquipFaults;

namespace SIE.Web.Equipments.EquipFaults
{
    /// <summary>
    /// 设备故障与系统缺陷对应关系查询实体视图配置
    /// </summary>
    internal class EquipFaultAndDefectCriteriaViewConfig : WebViewConfig<EquipFaultAndDefectCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipBadCode);
            View.Property(p => p.EquipModelId);
            View.Property(p => p.EquipModelName);
            View.Property(p => p.DefectCategoryId).Cascade(p => p.Defect, null);
            View.Property(p => p.DefectId).UseDataSource((e, pagingInfo, keyword) =>
            {
                var criteria = e as EquipFaultAndDefectCriteria;
                if (criteria != null && criteria.DefectCategoryId.HasValue)
                    return AppRuntime.Service.Resolve<DefectController>().GetDefects(criteria.DefectCategoryId.Value, keyword, pagingInfo);
                return new EntityList<Defect>();
            }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}