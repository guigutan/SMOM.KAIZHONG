using SIE.Common.ImportHelper;
using SIE.MES.TeamManagement.ShiftSchedules;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 班组排班导入命令
    /// </summary>
    public class ScheduleImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入完成
        /// </summary>
        /// <returns>完成事件</returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类
        /// </summary>
        /// <returns>排班导入处理类</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ScheduleImportHandle);
        }

        /// <summary>
        /// 模板
        /// </summary>
        /// <returns>示例数据</returns>
        protected override List<string> GetImportTempleData()
        {
            return new List<string>() { "A班", "PCBA-1线", "2019_1", "白班", "晚班", "白班" };
        }
    }
}