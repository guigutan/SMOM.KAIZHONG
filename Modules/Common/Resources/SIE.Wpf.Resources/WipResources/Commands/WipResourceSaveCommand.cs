using SIE.Diagnostics;
using SIE.Domain;
using SIE.Wpf.Behaviors;
using SIE.Wpf.Command;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Threading;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 排程资源保存命令类
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存排程资源", GroupType = 10)]
    public class WipResourceSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 排程资源保存
        /// </summary>
        /// <param name="view">排程资源视图</param>
        public override void Execute(ListLogicalView view)
        {
            OnSaving(view);

            SaveSchedulingResource(view);

            view.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;

            OnSaved(view);

            var behavious = view.FindBehavior<MementoViewBehavior>();

            if (behavious != null)
            {
                behavious.Manager.ClearUndoStack();
            }
        }

        /// <summary>
        /// 排程资源保存方法--添加进度条
        /// </summary>
        private void SaveSchedulingResource(ListLogicalView view)
        {

            using (DebugTrace.Start("SaveScheResource"))
            {
                Exception exception = null;

                var win = new WaitDialog();
                win.Width = 500;
                win.ShowInTaskbar = false;
                win.Text = "正在保存排程资源，请稍等……";
                win.ProgressValue = new ProgressValue()
                {
                    Percent = 100
                };

                ThreadPool.QueueUserWorkItem(oo =>
                {
                    try
                    {
                        RF.Save(view.Data);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    Action ac = () => win.DialogResult = true;
                    win.Dispatcher.BeginInvoke(ac);
                });

                win.Topmost = true;
                win.ShowDialog();

                if (exception != null)
                {
                    CRT.MessageService.ShowException(exception);
                }
            }
        }
    }
}