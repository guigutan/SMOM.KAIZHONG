using SIE.EMS.SpecialEquipment.Models;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpecialEquipment.Commands
{
    /// <summary>
    /// 设备检验规程选择按钮(设备型号)
    /// </summary>
    public class SelModelInspectionCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var EquipModelInspectionList = args.Data.ToJsonObject<List<EquipModelRegularInspection>>();
            Check.NotNullOrEmpty(EquipModelInspectionList, nameof(EquipModelInspectionList));
            if (null == EquipModelInspectionList || EquipModelInspectionList.Count == 0)
            {
                throw new ArgumentNullException("检验规程列表不能为空".L10N());
            }
            RT.Service.Resolve<EquipModelExtensionController>().SaveSelModelInspection(EquipModelInspectionList);
            return true;
        }
    }
}
