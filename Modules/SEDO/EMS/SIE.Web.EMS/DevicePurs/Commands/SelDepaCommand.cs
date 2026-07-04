using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Entity;
using SIE.EMS.DevicePurs;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.Resources.Enterprises;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.DevicePurs.Commands
{
    /// <summary>
    /// 添加设备清单
    /// </summary>
    [JsCommand("SIE.Web.EMS.DevicePurs.Commands.SelDepaCommand")]
    public class SelDepaCommand : ViewCommand
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
            List<DevicePurInfo> devicePurInfos = args.Data.ToJsonObject<List<DevicePurInfo>>();
            Check.NotNullOrEmpty(devicePurInfos, nameof(devicePurInfos));

            foreach (var item in devicePurInfos)
            {
                var deviceDepa = new DeviceDepa();
                deviceDepa.DevicePurId = item.SourceId;
                deviceDepa.EnterpriseId = item.DevicePurId;

                savedData.Add(deviceDepa);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
