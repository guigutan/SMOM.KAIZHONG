namespace SIE.Web.EMS.Lubrications.Commands
{
    /// <summary>
    /// 润滑项目按钮集合
    /// </summary>
    internal static class LubricationCommands
    {
        //命令命名空间
        internal const string commandNameSpace = "SIE.Web.EMS.Lubrications.Commands.";
        /// <summary>
        /// 添加保存
        /// </summary>
        internal const string LubricationSaveCommand = commandNameSpace + "LubricationSaveCommand";

        /// <summary>
        /// 添加记录
        /// </summary>
        internal const string AddReportCommand = commandNameSpace + "AddReportCommand";
       
        /// <summary>
        /// 主页提交
        /// </summary>
        internal const string LubricationSubmitCommand = commandNameSpace + "LubricationSubmitCommand";

        /// <summary>
        /// 审核
        /// </summary>
        /// 
        internal const string LubricationApprovalCommand = commandNameSpace + "LubricationApprovalCommand";

        /// <summary>
        /// 删除
        /// </summary>
        /// 
        internal const string LubricationDeleteCommand = commandNameSpace + "LubricationDeleteCommand";
        

        /// <summary>
        /// 查看
        /// </summary>
        internal const string SeeReportCommand = commandNameSpace + "SeeReportCommand";



        /// <summary>
        /// 明细页保存(添加润滑记录)
        /// </summary>
        internal const string LubricationDetailSaveCommand = commandNameSpace + "LubricationDetailSaveCommand";
        /// <summary>
        /// 明细页提交(添加润滑记录)
        /// </summary>
        internal const string LubricationDetailSubmitCommand = commandNameSpace + "LubricationDetailSubmitCommand";
        /// <summary>
        ///  明细页润滑项目一键润滑(添加润滑记录)
        /// </summary>
        internal const string LubricationOnekeyCommand = commandNameSpace + "LubricationOnekeyCommand";
    }
}
