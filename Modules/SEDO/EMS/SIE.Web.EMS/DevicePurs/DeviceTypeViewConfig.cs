using SIE.EMS.DevicePurs;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Domain;
using SIE.Equipments;
using SIE.Equipments.EquipTypes;
using System.Collections.Generic;
using SIE.Web.EMS.DevicePurs.Commands;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 设备类型-界面
    /// </summary>
    internal class DeviceTypeViewConfig : WebViewConfig<DeviceType>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(
                typeof(SelDeviceTypeCommand).FullName
                , WebCommandNames.Delete);

            View.Property(p => p.EquipType).Readonly().UseDataSource(
                (source, pagingInfo, keyword) =>
                {
                    var sourceList = new EntityList<EquipType>();

                    var deviceType = source as DeviceType;

                    if (deviceType != null)
                    {
                        if (!string.IsNullOrEmpty(deviceType.TypeCategory))
                        {
                            sourceList = RT.Service.Resolve<CoreEquipController>()
                                .GetEquipTypes(deviceType.TypeCategory, pagingInfo, keyword);
                        }
                        else
                        {
                            sourceList = RT.Service.Resolve<CoreEquipController>()
                               .GetEquipTypes(pagingInfo, keyword);
                        }
                    }

                    return sourceList;
                }).UsePagingLookUpEditor(
                (m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipTypeName), nameof(e.EquipType.TypeName));
                    m.DicLinkField = keyValues;
                });

            View.Property(p => p.EquipTypeName).Readonly();
        }
    }
}
