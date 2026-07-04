using SIE.EMS.Equipments.AlarmStates;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 报警明细查询实体视图配置
    /// </summary>
    internal class EquipAlarmRecordCriteriaViewConfig : WebViewConfig<EquipAlarmRecordCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipAccountId).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.ViewEquipAccountName), nameof(r.EquipAccount.Name));

                m.DicLinkField = keyValues;
            }).HasLabel("设备编码");
            View.Property(p => p.ViewEquipAccountName);
            View.Property(p => p.EquipModelId).HasLabel("设备型号");
            View.Property(p => p.EquipTypeId).HasLabel("设备类型");
            View.Property(p => p.AlarmLevel);
            View.Property(p => p.AlarmType);
            View.Property(p => p.AlarmState);
            View.Property(p => p.AlarmContent).HasLabel("报警主题");
            View.Property(p => p.AlarmTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show(ShowInWhere.All);
        }
    }
}
