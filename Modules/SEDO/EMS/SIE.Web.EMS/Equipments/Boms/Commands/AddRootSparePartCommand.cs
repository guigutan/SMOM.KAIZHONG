using SIE.Web.Command;

namespace SIE.Web.EMS.Equipments.Boms.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AddRootSparePartCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
}
