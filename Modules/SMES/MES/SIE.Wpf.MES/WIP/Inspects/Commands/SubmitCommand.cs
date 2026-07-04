using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Inspects.Commands
{
    /// <summary>
    /// 检验采集提交命令
    /// </summary>
    [Command(ImageName = "Check",
    Label = "提交",
    ToolTip = "提交",
    GroupType = CommandGroupType.Business)]
    public class SubmitCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var m = view.Current as InspectViewModel;
            return m != null && m.CanSubmit();
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var m = view.Current as InspectViewModel;
            m.Submit();
        }
    }

    /// <summary>
    /// 检验项目采集提交命令
    /// </summary>
    [Command(ImageName = "Check",
    Label = "提交",
    ToolTip = "提交",
    GroupType = CommandGroupType.Business)]
    public class SubmitByItemCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var m = view.Current as InspectByItemViewModel;
            return m != null && m.CanSubmit();
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var m = view.Current as InspectByItemViewModel;
            if (m.InspectionItemList.Any(p=>!p.IsNg && !p.IsOk)) 
            {
                if (!CRT.MessageService.AskQuestion("存在未录入的检验项目，是否继续提交?".L10N(), "提示".L10N()))
                {
                    return;
                }
            }
            m.Submit();
        }
    }
}