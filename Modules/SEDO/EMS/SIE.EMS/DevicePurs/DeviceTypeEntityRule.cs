using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.DevicePurs
{
    /// <summary>
    /// 设备与人员权限-设备类型实体规则
    /// </summary>
    [DisplayName("设备与人员权限-设备类型实体规则")]
    [Description("设备与人员权限-设备类型实体规则")]
    public class DeviceTypeEntityRule : EntityRule<DeviceType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeviceTypeEntityRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">事件参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var deviceType = entity as DeviceType;

            //if (string.IsNullOrEmpty(deviceType.TypeCategory))
            //{
            //    e.BrokenDescription = "【设备类别】不能为空！";
            //}

            if (RT.Service.Resolve<DevicePurController>().IsDeviceTypeDuplicate(deviceType))
            {
                var equipTypeDesc = deviceType.EquipType != null ? deviceType.EquipType.TypeName : "空";

                var catalog = RT.Service.Resolve<CatalogController>()
                    .GetCatalog(EquipType.EquipTypeCatalogType, deviceType.TypeCategory);

                var catalogDesc = catalog != null ? catalog.Name : deviceType.TypeCategory;

                e.BrokenDescription = "[设备与人员权限维护]的[设备类型]已经存在[设备类别]是【{0}】和[设备类型]是【{1}】的数据"
                        .L10nFormat(catalogDesc, equipTypeDesc);
            }
        }
    }
}
