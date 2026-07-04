using SIE.Domain;
using SIE.Resources.ProcessTechs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺新增命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class AddProcessTechCommand : ListAddCommand
    {
        /// <summary>
        /// 新增制程工艺
        /// </summary>
        /// <returns>制程工艺</returns>
        protected override Entity CreateNewItem()
        {
            var processTech = base.CreateNewItem() as ProcessTech;
            processTech.Code = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechNo();
            processTech.WorkingHours = 1;
            processTech.IsScheduling = true;

            return processTech;
        }
    }
}
