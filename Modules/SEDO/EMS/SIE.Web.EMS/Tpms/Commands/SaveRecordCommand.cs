using SIE.EMS.Tpms;
using SIE.EMS.Tpms.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.Tpms.Commands
{
    /// <summary>
    /// 保存TPM操作记录
    /// </summary>
    [JsCommand("SIE.Web.EMS.Tpms.Commands.SaveRecordCommand")]
    public class SaveRecordCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行保存操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var recordInfo = new TpmRecordInfo();
            recordInfo.ErrMsg = string.Empty;
            var addRecordInfo = args.Data.ToJsonObject<AddRecordInfo>();
            try
            {
                recordInfo.ErrMsg = RT.Service.Resolve<TpmController>().SaveTpmRecordInfo(addRecordInfo);
            }
            catch (Exception ex)
            {
                recordInfo.ErrMsg = ex.Message;
            }

            return recordInfo;
        }
    }
}
