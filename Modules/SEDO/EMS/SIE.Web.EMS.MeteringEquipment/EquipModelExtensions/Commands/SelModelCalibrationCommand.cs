using SIE.EMS.MeteringEquipment.EquipModelExtensions;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.EquipModelExtensions.Commands
{
    /// <summary>
    /// 计量校验规程(选择)按钮
    /// </summary>
    public class SelModelCalibrationCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var EquipModelCalibrationList = args.Data.ToJsonObject<List<EquipModelCalibration>>();
            Check.NotNullOrEmpty(EquipModelCalibrationList, nameof(EquipModelCalibrationList));
            if (null == EquipModelCalibrationList || EquipModelCalibrationList.Count == 0)
            {
                throw new ArgumentNullException("检验规程列表不能为空".L10N());
            }
            RT.Service.Resolve<EquipModelExtensionController>().SaveSelModelCalibration(EquipModelCalibrationList);
            return true;
        }
    }
}
