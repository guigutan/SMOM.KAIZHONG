using SIE.Domain;
using SIE.EMS.FixedAssets.Accounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.FixedAssets.Accounts.Commands
{
    /// <summary>
    /// 添加设备清单
    /// </summary>
    [JsCommand("SIE.Web.EMS.FixedAssets.Accounts.Commands.SelEquipsCommand")]
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
            List<DeviceFixedAssetsAccountInfo> deviceFixedAssetsAccountInfos = args.Data.ToJsonObject<List<DeviceFixedAssetsAccountInfo>>();
            Check.NotNullOrEmpty(deviceFixedAssetsAccountInfos, nameof(deviceFixedAssetsAccountInfos));

            foreach (var item in deviceFixedAssetsAccountInfos)
            {
                var deviceBill = new FixedAssetDeviceBill();
                deviceBill.FixedAssetsAccountId = item.SourceId;
                deviceBill.EquipAccountId = item.DevicePurId;
                savedData.Add(deviceBill);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 设备与资产台账信息
    /// </summary>
    [Serializable]
    public class DeviceFixedAssetsAccountInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 设备与人员权限Id
        /// </summary>
        public double DevicePurId { get; set; }
    }
}
