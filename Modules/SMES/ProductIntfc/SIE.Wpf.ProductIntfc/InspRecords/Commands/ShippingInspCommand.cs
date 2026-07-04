using SIE.Common;
using SIE.ProductIntfc.InspRecords;
using SIE.Wpf.Command;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Linq;
using System.Threading;

namespace SIE.Wpf.ProductIntfc.InspRecords.Commands
{
    /// <summary>
    /// 成品报检命令
    /// </summary>
    [Command(Label = "报检", ImageName = "TextRelease", ToolTip = "报检", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class ShippingInspCommand : ListViewCommand
    {
        /// <summary>
        /// 判断成品报检命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Count > 0;
        }

        /// <summary>
        /// 成品报检命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            Exception exc = null;
            var barcodes = view.SelectedEntities.OfType<InspBarcode>().AsEntityList();
            var barcodeIds = barcodes.Select(p => p.Id).Distinct().ToList();
            var win = new WaitDialog();
            win.Width = 500;
            win.ShowInTaskbar = false;
            win.Text = "成品报检中，请稍等…";
            win.ProgressValue = new ProgressValue() { Percent = 100 };
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    RT.Service.Resolve<InspRecordController>().ProductInsp(barcodeIds);
                }
                catch (Exception ex)
                {
                    exc = ex;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });
            win.Topmost = true;
            win.ShowDialog();
            if (exc != null)
                exc.Alert();
            else
                CRT.MessageService.ShowMessage("报检成功".L10N(), "提示".L10N());
            view.Data.Clear();
            if (view.DataLoader.AnyLoaded)
                view.DataLoader.ReloadDataAsync();
        }
    }
}