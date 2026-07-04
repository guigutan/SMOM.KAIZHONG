using SIE.Domain;
using SIE.ERPInterface.Smom.InventoryControl;
using SIE.Web.Command;

namespace SIE.Web.ERPInterface.InventoryControl.Commands
{
    /// <summary>
    /// 库存对照表设置按钮
    /// </summary>
    public class InventoryControlSettingCommand : ViewCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //var setting = RT.Service.Resolve<StatisticsController>().GetInventoryControlSetting();
            var data = args.Data.ToJsonObject<InventoryControlSetting>();
            RF.Save(data);
            return true;
        }
    }
}
