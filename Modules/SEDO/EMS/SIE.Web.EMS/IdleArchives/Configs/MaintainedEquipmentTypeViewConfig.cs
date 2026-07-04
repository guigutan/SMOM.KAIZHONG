using SIE.Core.Equipments;
using SIE.EMS.IdleArchives.Configs;
using System.Collections.Generic;

namespace SIE.Web.EMS.IdleArchives.Configs
{
    /// <summary>
    /// 设备编码配置值视图
    /// </summary>
    public class MaintainedEquipmentTypeViewConfig : WebViewConfig<MaintainedEquipmentTypeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.EquipTypeName).Show(ShowInWhere.List).UsePagingLookUpGridPopupEditor(p =>
            {
                p.Model = typeof(EquipType).FullName;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(MaintainedEquipmentTypeConfigValue.EquipTypeCodeProperty.Name, EquipType.TypeCodeProperty.Name);
                dic.Add(MaintainedEquipmentTypeConfigValue.EquipTypeIdsProperty.Name, EquipType.IdProperty.Name);
                p.MutiLinkField = dic.ToJsonString();
            }).DisableSort();
            View.Property(c => c.EquipTypeIds).Show(ShowInWhere.Hide);
        }
    }
}
