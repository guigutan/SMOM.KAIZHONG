using SIE.Domain;
using SIE.Resources.ShiftTypes;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = CommandGroupType.Edit)]
    public class ShiftAddCommand : ListAddCommand
    {
        /// <summary>
        /// 创建编辑对象
        /// </summary>
        /// <returns>班次时间</returns>
        protected override Entity CreateNewItem()
        {
            var shift = base.CreateNewItem() as Shift;
            return shift;
        }
    }
}
