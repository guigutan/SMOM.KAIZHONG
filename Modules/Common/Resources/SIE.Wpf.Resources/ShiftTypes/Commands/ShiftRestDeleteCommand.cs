using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", GroupType = CommandGroupType.Edit)]
    public class ShiftRestDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 执行删除命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            ////var shift = view.Data.Parent as Shift;
            
            ////var shiftStart = new DateTime(2018, 7, 5, shift.BeginTime.Hour, shift.BeginTime.Minute, 0);
            ////var shiftEnd = new DateTime(2018, 7, 5, shift.EndTime.Hour, shift.EndTime.Minute, 0);
            ////var shiftWorkHour = (decimal)(shiftEnd - shiftStart).TotalHours;
            
            ////if (shiftWorkHour < 0)
            ////{
            ////    shiftWorkHour = (decimal)(shiftEnd.AddDays(1) - shiftStart).TotalHours;
            ////}
            base.Execute(view);
        }
    }
}
