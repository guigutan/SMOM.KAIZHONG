using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 一键下料视图行为
    /// </summary>
    public class UnloadAllItemBehavior : ViewBehavior
    {
        bool _isVisible = true;

        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            View.DataChanged += (o, e) =>
            {
                if (!_isVisible) return;
                var command = View.Commands.FirstOrDefault(p => p.GetType().IsSubclassOf(typeof(ListEditCommand)) || p.GetType().FullName == typeof(ListEditCommand).FullName);
                if (command != null)
                {
                    _isVisible = false;
                    command.IsVisible = false;
                }
            };
        }
    }
}