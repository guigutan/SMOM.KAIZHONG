using SIE.Domain;
using SIE.EMS.RunStandards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipRepairs.Commands
{
    /// <summary>
    /// 添加设备清单
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepairs.Commands.SelEquipsCommand")]
    public class SelEquipsCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            List<DeviceAccountInfo> deviceFixedAssetsAccountInfos = args.Data.ToJsonObject<List<DeviceAccountInfo>>();
            Check.NotNullOrEmpty(deviceFixedAssetsAccountInfos, nameof(deviceFixedAssetsAccountInfos));

            foreach (var item in deviceFixedAssetsAccountInfos)
            {
                var deviceBill = new RunStandardEquipment();
                deviceBill.RunStandardId = item.SourceId;
                deviceBill.EquipAccountId = item.DevicePurId;
                savedData.Add(deviceBill);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 设备台账信息
    /// </summary>
    [Serializable]
    public class DeviceAccountInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public double DevicePurId { get; set; }
    }
}
