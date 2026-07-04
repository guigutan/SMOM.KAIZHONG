using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Entity;
using SIE.EMS.DevicePurs;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.DevicePurs.Commands
{
    /// <summary>
    /// 添加设备清单
    /// </summary>
    [JsCommand("SIE.Web.EMS.DevicePurs.Commands.SelBillCommand")]
    public class SelBillCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<DevicePurInfo> devicePurInfos = args.Data.ToJsonObject<List<DevicePurInfo>>();
            Check.NotNullOrEmpty(devicePurInfos, nameof(devicePurInfos));

            RT.Service.Resolve<DevicePurController>().SaveDeviceBills(devicePurInfos);

            return true;
        }
    }
}
