using SIE.MES.Workbench;
using SIE.MES.Workbench.AlertLights;
using SIE.MES.Workbench.AndonAbnormals;
using SIE.MetaModel.View;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Workbench.AndonAbnormals.Commands
{
    /// <summary>
    /// 安灯异常签到时间
    /// </summary>
    [Command(ImageName = "EditEntity", Label = "签到", ToolTip = "修改数据", Gestures = "Ctrl+Shift+E", Location = CommandLocation.All, GroupType = 10)]
    public class AndonSignCommand : ListEditCommand
    {
        /// <summary>
        /// 签到按钮是否可以执行
        /// </summary>
        /// <param name="view">安灯异常视图集合</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            //return base.CanExecute(view);
            var entity = view.Current as AndonAbnormal; ////&& entity.Status == true
            return entity != null && entity.ProcessStatus == ProcessStatusType.Waitting;
        }

        /// <summary>
        /// 签到命令方法
        /// </summary>
        /// <param name="view">安灯异常视图集合</param>
        public override void Execute(ListLogicalView view)
        {
            var empId = RT.IdentityId;
            var entity = view.Current as AndonAbnormal;
            RT.Service.Resolve<AlertLightsController>().SignAndonAbnormal(entity.Id, empId);
            if (view.DataLoader.AnyLoaded)
                view.DataLoader.ReloadDataAsync();
        }
    }
}
