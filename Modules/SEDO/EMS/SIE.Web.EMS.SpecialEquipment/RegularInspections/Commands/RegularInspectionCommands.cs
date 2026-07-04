namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands
{
    /// <summary>
    /// 特种设备定检命令集合
    /// </summary>
    internal static class RegularInspectionCommands
    {
        //命令命名空间
        internal const string commandNameSpace = "SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.";
        /// <summary>
        /// 录入校验记录
        /// </summary>
        internal const string WritingReportCommand = commandNameSpace + "WritingReportCommand";
        /// <summary>
        /// 查看校验报告
        /// </summary>
        internal const string ViewReportCommand = commandNameSpace + "ViewReportCommand";

        /// <summary>
        /// 审核
        /// </summary>
        internal const string AuditCommand = commandNameSpace + "AuditCommand";

        /// <summary>
        /// 添加数据列
        /// </summary>
        internal const string AddColumnCommand = commandNameSpace + "AddColumnCommand";

        /// <summary>
        /// 删除数据列
        /// </summary>
        internal const string DeleteColumnCommand = commandNameSpace + "DeleteColumnCommand";

        /// <summary>
        /// 全部合格
        /// </summary>
        internal const string AllPassCommand = commandNameSpace + "AllPassCommand";

        /// <summary>
        /// 重置结果
        /// </summary>
        internal const string ResetResultCommand = commandNameSpace + "ResetResultCommand";

    }
}
