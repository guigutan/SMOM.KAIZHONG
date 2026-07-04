using SIE.Resources.ShiftTypes;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 设置缺省指令
    /// </summary>
    [Command(ImageName = "FlagTriangle", Label = "设为缺省", ToolTip = "设为缺省", GroupType = 10)]
    public class SetDefaultCommand : ListViewCommand
    {
        /// <summary>
        /// 是否执行设置缺省指令
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var shiftType = view.Current as ShiftType;
            return shiftType != null && shiftType.IsDefault == YesNo.No;
        }

        /// <summary>
        /// 执行设置缺省指令
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var shiftType = view.Current as ShiftType;                
            if (CRT.MessageService.AskQuestion("是否将班制{0}设置为缺省?".L10nFormat(shiftType.Name)))
            {
                RT.Service.Resolve<ShiftTypeController>().SetDefaultShiftType(shiftType);
                view.QueryView.TryExecuteQuery();
            }
        }
    }
}