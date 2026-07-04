using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.ImportWorkOrders;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WorkOrders.ViewModels;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 工单 导入命令
    /// </summary>
    [Command(Label = "导入", ToolTip = "导入", Location = CommandLocation.All, GroupType = CommandGroupType.Business)]
    public class ImportWorkOrderDataCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否正在导入中
        /// </summary>
        private bool isRun = false;

        /// <summary>
        /// 是否可执行导入逻辑
        /// </summary>
        /// <param name="view">明细视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var current = view.Data as ImportWorkOrderCheckViewModel;
            if (base.CanExecute(view) && !isRun && current != null && !string.IsNullOrEmpty(current.ImportFilePath))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 执行导入逻辑
        /// </summary>
        /// <param name="view">当前视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var current = view.Data as ImportWorkOrderCheckViewModel;
            current.ImportProcessMsg = "正在导入";
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
                        string strMsg = importWOHandle.ImportProcess(importFilePath, typeof(ImportWorkOrderHandle), (drSuccess, drFailed) =>
                        {
                            ImportCompleted(drSuccess, drFailed, current);
                        });
                        sw.Stop();
                        Debug.WriteLine(sw.Elapsed.TotalMilliseconds);

                        return strMsg;
                    });

                    task.Start();

                    Task cwt = task.ContinueWith(t =>
                    {
                        current.ImportProcessMsg = t.Result.ToString();
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
        private void ImportCompleted(DataRow[] importSuccess, DataRow[] importFailed, ImportWorkOrderCheckViewModel current)
        {
            EntityList<WorkOrderCheckDataViewModel> errorList = new EntityList<WorkOrderCheckDataViewModel>();
            foreach (DataRow dr in importFailed)
            {
                WorkOrderCheckDataViewModel errorRow = new WorkOrderCheckDataViewModel();
                errorRow.No = dr[0].ToString();
                errorRow.Product = dr[1].ToString();
                errorRow.PlanQty = dr[2].ToString();
                errorRow.Type = dr[3].ToString();
                errorRow.PlanBeginDate = dr[4].ToString();
                errorRow.PlanEndDate = dr[5].ToString();
                errorRow.WorkShop = dr[6].ToString();
                errorRow.Resource = dr[7].ToString();
                errorRow.ParentCode = dr[8].ToString();
                errorRow.ErpWorkOrder = dr[9].ToString();
                errorRow.CustomerOrderNo = dr[10].ToString();
                errorRow.SaleOrderNo = dr[11].ToString();
                errorRow.OrderQty = dr[12].ToString();
                errorRow.ErrorMessage = dr[13].ToString();
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
                    RefreshWorkOrder();
            });
        }

        /// <summary>
        /// 刷新工单数据
        /// </summary>
        void RefreshWorkOrder()
        {
            var type = typeof(EntitySavedEvent<>).MakeGenericType(typeof(WorkOrder));
            var @event = Activator.CreateInstance(type, new WorkOrder());
            RT.EventBus.Publish(@event);
        }
    }
}