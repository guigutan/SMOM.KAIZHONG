using SIE.EMS.Tpms;
using SIE.Web.Command;

namespace SIE.Web.EMS.Tpms.Commands
{
    /// <summary>
    /// TPM添加命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Tpms.Commands.AddRecordCommand")]
    public class AddRecordCommand : ViewCommand
    {
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<TpmRecord>();
            data.TpmNo = RT.Service.Resolve<TpmController>().GetTpmScoreNo();
            return data;
        }
    }
}
