using SIE.Resources.WipResources;
using SIE.Threading;
using SIE.Wpf.Command;
using SIE.Wpf.Controls.WaitProgress;
using System;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 生产资源导入命令类
    /// </summary>
    [Command(ImageName = "Refresh", Label = "刷新", ToolTip = "刷新同步数据", GroupType = 20)]
    public class WipResourceRefreshCommand : ListViewCommand
    {
        /// <summary>
        /// 生产资源刷新同步方法
        /// </summary>   
        /// <param name="view">view</param>
        public override void Execute(ListLogicalView view)
        {
            using (SIE.Diagnostics.DebugTrace.Start("ScheResourceRefresh".L10N()))
            {
                var exception = EnableScheResProgressBar();
                if (exception != string.Empty)
                {
                    CRT.MessageService.ShowMessage("同步出错。{0}".L10nFormat(exception));
                }
                else
                {
                    CRT.MessageService.ShowMessage("同步成功".L10N());
                }

                if (view.DataLoader.AnyLoaded)
                    view.DataLoader.ReloadDataAsync();
            }
        }

        /// <summary>
        /// 生产资源刷新，添加进度条
        /// </summary>       
        /// <returns>启用生产资源的异常</returns>
        private string EnableScheResProgressBar()
        {
            string exception = null;
            var win = new WaitDialog();
            win.Width = 500;
            win.ShowInTaskbar = false;
            win.Text = "正在同步非自定义资源，请稍等……".L10N();
            win.ProgressValue = new ProgressValue() { Percent = 100 };                        
            AsyncHelper.InvokeSafe((() =>
            {
                exception = RT.Service.Resolve<WipResourceController>().RunSync();
                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            }));

            win.Topmost = true;
            win.ShowDialog();

            return exception;
        }
    }
}
