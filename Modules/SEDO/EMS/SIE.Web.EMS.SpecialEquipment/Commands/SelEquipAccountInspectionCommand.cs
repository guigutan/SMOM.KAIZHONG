using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;


namespace SIE.Web.EMS.SpecialEquipment.Commands
{
    /// <summary>
    /// 特种设备台账选择检验规则
    /// </summary>
    public class SelEquipAccountInspectionCommand : ViewCommand
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
            var EquipModelInspectionList = args.Data.ToJsonObject<List<EquipAccountRegularInspection>>();
            Check.NotNullOrEmpty(EquipModelInspectionList, nameof(EquipModelInspectionList));
            if (null == EquipModelInspectionList || EquipModelInspectionList.Count == 0)
            {
                throw new ArgumentNullException("检验规程列表不能为空".L10N());
            }

            RT.Service.Resolve<SpecialEquipAccountController>().SaveSelEquipAccountRegularInspection(EquipModelInspectionList);
            return true;
        }
    }
}
