using SIE.Equipments.Configs;
using SIE.Equipments.EquipTypes;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipAccounts.Configs
{
    /// <summary>
    /// 位置列表维护条件配置值视图配置
    /// </summary>
    internal class EquipAccountLocationConfigViewConfig : WebViewConfig<EquipAccountsLocationConfigValue>
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
                dic.Add(EquipModelLocationConfigValue.EquipTypeCodeProperty.Name, EquipType.TypeCodeProperty.Name);
                dic.Add(EquipModelLocationConfigValue.EquipTypeNameProperty.Name, EquipType.TypeNameProperty.Name);
                dic.Add(EquipModelLocationConfigValue.EquipTypeIdsProperty.Name, EquipType.IdProperty.Name);
                p.DisplayField = EquipType.TypeNameProperty.Name;
                p.MutiLinkField = dic.ToJsonString();
            }).DisableSort();
            View.Property(c => c.EquipTypeIds).Show(ShowInWhere.Hide);
        }
    }
}