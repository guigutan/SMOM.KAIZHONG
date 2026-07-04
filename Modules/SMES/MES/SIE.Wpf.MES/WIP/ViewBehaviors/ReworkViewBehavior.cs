using SIE.MES.WIP.Reworks;
using SIE.Wpf.MES.WIP.Reworks;
using System.ComponentModel;

namespace SIE.Wpf.MES.WIP.ViewBehaviors
{
    /// <summary>
    /// 返工采集视图行为
    /// </summary>
    public class ReworkViewBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加视图行为
        /// </summary>
        protected override void OnAttach()
        {
            View.CurrentChanged += (o, e) =>
            {
                var rewkWml = View.Current as ReworkViewModel;
                SetCommandVisible(rewkWml);
                rewkWml.PropertyChanged += RewkWml_PropertyChanged;
            };
        }

        /// <summary>
        /// 返工采集视图属性变更后事件处理方法
        /// </summary>
        /// <param name="sender">事件触发者</param>
        /// <param name="e">事件参数</param>
        private void RewkWml_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReworkViewModel.ReworkOperate))
            {
                var rewkWml = sender as ReworkViewModel;
                SetCommandVisible(rewkWml);
            }
        }

        /// <summary>
        /// 设置提交按钮是否显示
        /// </summary>
        /// <param name="rewkWml">返工采集视图</param>
        private void SetCommandVisible(ReworkViewModel rewkWml)
        {
            var rewkOperate = rewkWml.ReworkOperate;
            var command = View.Commands.Find(typeof(SubmitCommand));
            if (command != null)
            {
                if (rewkOperate == ReworkOperate.PermuteUnbound)
                    command.IsVisible = true;
                else
                    command.IsVisible = false;
            }
        }
    }
}
