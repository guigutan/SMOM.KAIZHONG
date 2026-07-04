using SIE.Resources.ShiftTypes;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", GroupType = CommandGroupType.Edit)]
    public class ShiftDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 删除是否可用
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view?.Current != null;
        }

        /// <summary>
        /// 执行删除命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var shiftList = view.SelectedEntities.OfType<Shift>().ToList();

            if (view.SelectedEntities.Count < 2)
            {
                view.Data.Remove(view.Current);
            }
            else
            {
                foreach (var shift in shiftList)
                {
                    view.Data.Remove(shift);
                }
            }            
        }
    }
}
