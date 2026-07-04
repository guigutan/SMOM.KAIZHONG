using SIE.EMS.Equipments.AlarmStates;

namespace SIE.Web.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 报警统计查询实体视图配置
    /// </summary>
    internal class AlarmCountCriteriaViewConfig : WebViewConfig<AlarmCountCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipAccountId).HasLabel("设备编码");
            View.Property(p => p.EquipModelId).HasLabel("设备型号");
            View.Property(p => p.EquipTypeId).HasLabel("设备类型");
            View.Property(p => p.AlarmTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show(ShowInWhere.All);
        }
    }
}
