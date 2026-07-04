using SIE.Web.Command;

namespace SIE.Web.EMS.Equipments.Boms.Commands
{
    /// <summary>
    /// 插入子数据命令
    /// </summary>
    public class InsertChildSparePartCommand : ViewCommand
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
