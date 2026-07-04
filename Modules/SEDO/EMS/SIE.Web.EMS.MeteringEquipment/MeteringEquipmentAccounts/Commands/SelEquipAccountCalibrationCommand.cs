using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands
{
    /// <summary>
    /// 选择计量设备台账定项目
    /// </summary>
    public class SelEquipAccountCalibrationCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return false;
            }
            var EquipModelInspectionList = args.Data.ToJsonObject<List<EquipAccountCalibration>>();
            Check.NotNullOrEmpty(EquipModelInspectionList, nameof(EquipModelInspectionList));
            if (null == EquipModelInspectionList || EquipModelInspectionList.Count == 0)
            {
                throw new ArgumentNullException("检验规程列表不能为空".L10N());
            }

            RT.Service.Resolve<MeteringEquipmentAccountController>().SaveSelEquipAccountCalibration(EquipModelInspectionList);
            return true;
        }
    }
}
