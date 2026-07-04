using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductModels.ImportProductModels;
using SIE.Wpf.Command;
using SIE.Wpf.Items.ProductModels.ViewModels;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SIE.Wpf.Items.ProductModels.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    [Command(Label = "导入", ToolTip = "导入", GroupType = CommandGroupType.Business)]
    public class ImportProductModelDataCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否正在导入中
        /// </summary>
        private bool isRun ;

        /// <summary>
        /// 命令能否执行
        /// </summary>
        /// <param name="view">明细视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var current = view.Data as ImportProductModelCheckViewModel;
            if (base.CanExecute(view) && !isRun && current != null && !string.IsNullOrEmpty(current.ImportFilePath))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">明细视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var current = view.Data as ImportProductModelCheckViewModel;
            current.ImportProcessMsg = "正在导入...".L10N();
            if (current != null)
            {
                try
                {
                    isRun = true;
                    string importFilePath = current.ImportFilePath;
                    Task<string> task = new Task<string>(() =>
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        ImportDataHandle importWOHandle = new ImportDataHandle();
                        string strMsg = importWOHandle.ImportProcess(importFilePath, typeof(ImportProductModelHandel), (drSuccess, drFailed) =>
                        {
                            ImportCompleted(drSuccess, drFailed, current);
                        });
                        sw.Stop();
                        Debug.WriteLine(sw.Elapsed.TotalMilliseconds);

                        return strMsg;
                    });

                    task.Start();

                    task.ContinueWith(t =>
                    {
                        current.ImportProcessMsg = t.Result;
                        isRun = false;
                    });
                }
                catch (Exception exc)
                {
                    current.ImportProcessMsg = "异常消息:" + exc.Message + "。详细信息:" + exc.StackTrace;
                    isRun = false;
                }
            }
        }

        /// <summary>
        /// 导入完成触发
        /// </summary>
        /// <param name="importSuccess">导入成功数据集</param>
        /// <param name="importFailed">导入失败数据集</param>
        /// <param name="current">当前界面数据对象</param>
        private void ImportCompleted(DataRow[] importSuccess, DataRow[] importFailed, ImportProductModelCheckViewModel current)
        {
            EntityList<ProductModelCheckDataViewModel> errorList = new EntityList<ProductModelCheckDataViewModel>();
            foreach (DataRow dr in importFailed)
            {
                ProductModelCheckDataViewModel errorRow = new ProductModelCheckDataViewModel();
                errorRow.Code = dr[0].ToString();
                errorRow.Name = dr[1].ToString();
                errorRow.WorkingHours = dr[2].ToString();
                errorRow.SendingHours = dr[3].ToString();
                errorRow.Resource = dr[4].ToString();
                errorRow.LineWorkingHours = dr[5].ToString();
                errorRow.ErrorMessage = dr[6].ToString();
                errorList.Add(errorRow);
            }

            CRT.MainThread.InvokeAsync(() =>
            {
                current.ImportDataViewModelList.Clear();
                current.ImportFailAmount = importFailed.Length;
                current.ImportSuccessAmount = importSuccess.Length;
                current.ImportDataViewModelList.AddRange(errorList);
                current.NotifyAllPropertiesChanged();
                if (importSuccess.Length > 0)
                    RefreshData();
            });
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            var type = typeof(EntitySavedEvent<>).MakeGenericType(typeof(ProductModel));
            var @event = Activator.CreateInstance(type, new ProductModel());
            RT.EventBus.Publish(@event);
        }
    }
}
