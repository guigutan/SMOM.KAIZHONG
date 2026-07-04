using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using SIE.Domain.Validation;
using SIE.MES.WIP.Reworks;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// 返工采集提交命令
    /// </summary>
    [Command(ImageName = "Check", Label = "提交", ToolTip = "提交", GroupType = CommandGroupType.Edit)]
    public class SubmitCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var m = view.Current as ReworkViewModel;
            return m != null && m.CanSubmit();
        }

        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var m = view.Current as ReworkViewModel;
            if (m != null)
            {
                m.Submit();
            }
        }
    }
}
