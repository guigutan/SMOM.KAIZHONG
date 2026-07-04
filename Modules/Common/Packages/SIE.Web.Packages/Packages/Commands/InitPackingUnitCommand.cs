using SIE.Packages;
using SIE.Web.Command;

namespace SIE.Web.Packages.Packages.Commands
{
    #region 功能初始化
    /// <summary>
    /// 功能初始化
    /// </summary>
    [JsCommand("SIE.Web.Packages.Packages.Commands.InitPackingUnitCommand")]
    class InitPackingUnitCommand : ViewCommand
    {
        /// <summary>
        /// 事务初始化按钮
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope">view</param>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<PackageController>().InitPackingUnit();
            return true;
        }
    }
    #endregion

    /// <summary>
    /// 包装单位删除
    /// </summary>
    [JsCommand("SIE.Web.Packages.Packages.Commands.PackingUnitDeleteCommand")]
    public class PackingUnitDeleteCommand : DeleteCommand
    {
    }
}
