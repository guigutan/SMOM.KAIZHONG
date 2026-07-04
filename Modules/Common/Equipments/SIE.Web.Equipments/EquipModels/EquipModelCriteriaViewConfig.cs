using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;

namespace SIE.Web.Equipments.EquipModels
{
    /// <summary>
    /// 设备型号维护视图配置
    /// </summary>
    internal class EquipModelCriteriaViewConfig : WebViewConfig<EquipModelCriteria>
    {
        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.EquipTypeId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipTypeController>().GetEquipTypes(pagingInfo, keyword);
                }).HasLabel("设备类型").Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
