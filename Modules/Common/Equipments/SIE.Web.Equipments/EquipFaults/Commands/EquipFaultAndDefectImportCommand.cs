using SIE.Common.ImportHelper;
using SIE.Equipments.EquipFaults;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.Equipments.EquipFaults.Commands
{
    /// <summary>
    /// 设备故障与系统缺陷对应关系导入命令
    /// </summary> 
    public class EquipFaultAndDefectImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入完成后
        /// </summary>
        /// <returns>ImportCompleted</returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {

            };
        }

        /// <summary>
        /// 处理器
        /// </summary>
        /// <returns>类型</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportEquipFaultAndDefectHandle);
        }
    }
}