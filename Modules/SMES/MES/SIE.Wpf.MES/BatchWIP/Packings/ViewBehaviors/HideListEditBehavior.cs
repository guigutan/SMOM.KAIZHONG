using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Packings.ViewBehaviors
{
    /// <summary>
    /// 隐藏编辑命令行为
    /// </summary>
    public class HideListEditBehavior : ViewBehavior
    {
        /// <summary>
        /// 初始化执行方法
        /// 隐藏编辑命令
        /// </summary>
        protected override void OnAttach()
        {
            View.DataChanged += (o, e) =>
              {
                  var command = View.Commands.FirstOrDefault(p => p.GetType().IsSubclassOf(typeof(ListEditCommand)) || p.GetType().FullName == typeof(ListEditCommand).FullName);
                  if (command != null)
                      command.IsVisible = false;
              };
        }
    }
}
