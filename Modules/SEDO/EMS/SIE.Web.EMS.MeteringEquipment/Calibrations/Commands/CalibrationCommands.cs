namespace SIE.Web.EMS.MeteringEquipment.Calibrations.Commands
{
    /// <summary>
    /// 计量设备定检命令集
    /// </summary>
    internal static class CalibrationCommands
    {
        //命令命名空间
        internal const string commandNameSpace = "SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.";

        #region 添加页

        /// <summary>
        /// 添加记录页保存按钮
        /// </summary>
        internal const string CalibrationSaveCommand = commandNameSpace + "CalibrationSaveCommand";

        #endregion


        #region 录入检验报告页
        /// <summary>
        /// 录入校验记录
        /// </summary>
        internal const string InputReportCommand = commandNameSpace + "InputReportCommand";

        /// <summary>
        /// 明细页保存(添加润滑记录)
        /// </summary>
        internal const string CalibrationDetailSaveCommand = commandNameSpace + "CalibrationDetailSaveCommand";
        /// <summary>
        /// 明细页提交(添加润滑记录)
        /// </summary>
        internal const string CalibrationDetailSubmitCommand = commandNameSpace + "CalibrationDetailSubmitCommand";




        /// <summary>
        /// 全部合格
        /// </summary>
        internal const string AllPassCommand = commandNameSpace + "AllPassCommand";

        /// <summary>
        /// 重置结果
        /// </summary>
        internal const string ResetResultCommand = commandNameSpace + "ResetResultCommand";


        #endregion


        /// <summary>
        /// 查看
        /// </summary>
        internal const string SeeReportCommand = commandNameSpace + "SeeReportCommand";

        /// <summary>
        /// 审核
        /// </summary>
        /// 
        internal const string AuditCommand = commandNameSpace + "AuditCommand";
       
    }
}
