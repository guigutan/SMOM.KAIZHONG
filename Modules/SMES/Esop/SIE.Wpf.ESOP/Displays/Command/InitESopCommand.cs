using SIE.Domain;
using SIE.ESop.Displays;
using SIE.Wpf.Command;

namespace SIE.Wpf.ESop.Displays.Command
{
    /// <summary>
    /// 初始化ESOP
    /// </summary>
    [Command(ImageName = "PlaylistPlay", Label = "初始化ESOP", ToolTip = "初始化ESOP", Gestures = "Ctrl+Shift+I", GroupType = CommandGroupType.Edit)]
    public class InitESopCommand :ListSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            RT.Service.Resolve<DisplayPointController>().InitEsop();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return true;
        }
    }
}