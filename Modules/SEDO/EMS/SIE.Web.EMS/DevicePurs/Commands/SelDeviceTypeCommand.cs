using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.DevicePurs.Commands
{
    /// <summary>
    /// 选择设备类型
    /// </summary>
    [JsCommand("SIE.Web.EMS.DevicePurs.Commands.SelDeviceTypeCommand")]
    public class SelDeviceTypeCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = args.Data.ToJsonObject<List<DeviceType>>();
            if (list == null || list.Count == 0)
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(list)));
            var savedData = new EntityList<DeviceType>();
            foreach (var item in list)
            {
                var deviceType = new DeviceType();
                deviceType.DevicePurId = item.DevicePurId;
                deviceType.EquipTypeId = item.EquipTypeId;
                savedData.Add(deviceType);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
