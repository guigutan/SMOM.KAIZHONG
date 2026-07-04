using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Stations;
using SIE.Tech.Stations.ImportStationItem;
using SIE.Wpf.Command;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.Tech.Stations.Commands
{
    /// <summary>
    /// 导入命令
    /// </summary>
    [Command(Label = "导入", ToolTip = "导入", Location = CommandLocation.All, GroupType = CommandGroupType.Business)]
    public class ImportStationItemDataCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否正在导入中
        /// </summary>
        private bool isRun;

        /// <summary>
        /// 是否可执行导入逻辑
        /// </summary>
        /// <param name="view">明细视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var current = view.Data as ImportStationItemViewModel;
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
            var current = view.Data as ImportStationItemViewModel;
            current.ImportProcessMsg = "正在导入";
            if (current != null)
            {
                try
                {
                    isRun = true;
                    string importFilePath = current.ImportFilePath;
                    Task<string> task = new Task<string>(() =>
                    {
                        ImportStationItemDataHandle importItemHandle = new ImportStationItemDataHandle();
                        importItemHandle.StationId = current.StationId;
                        string strMsg = importItemHandle.ImportProcess(importFilePath, typeof(ImportStationItemHandle), (drSuccess, drFailed) =>
                        {
                            ImportCompleted(drSuccess, drFailed, current);
                        });

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
        private void ImportCompleted(DataRow[] importSuccess, DataRow[] importFailed, ImportStationItemViewModel current)
        {
            EntityList<StationItemCheckDataViewModel> errorList = new EntityList<StationItemCheckDataViewModel>();
            foreach (DataRow dr in importFailed)
            {
                StationItemCheckDataViewModel errorRow = new StationItemCheckDataViewModel();
                errorRow.ItemCode = dr[0].ToString();
                errorRow.ItemName = dr[1].ToString();
                errorRow.Warning = dr[2].ToString();
                errorRow.Capacity = dr[3].ToString();
                errorRow.ErrorMessage = dr[5].ToString();
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
            var type = typeof(EntitySavedEvent<>).MakeGenericType(typeof(Station));
            var @event = Activator.CreateInstance(type, new Station());
            RT.EventBus.Publish(@event);
        }
    }

    /// <summary>
    /// 写子类继承导入处理，给StationId赋值
    /// </summary>
    internal class ImportStationItemDataHandle : ImportDataHandle
    {
        /// <summary>
        /// 当前工位Id
        /// </summary>
        public double StationId;

        /// <summary>
        /// 处理导入数据之前执行
        /// </summary>
        /// <param name="importTable">处理导入数据</param>
        /// <param name="type">处理导入数据的业务类</param>
        /// <returns>返回需要处理的导入数据</returns>
        public override DataTable ProcessImportDataPre(DataTable importTable, Type type)
        {
            importTable.Columns.Add(ImportStationItemHandle.StationId, typeof(double));
            importTable.AsEnumerable().ForEach(p => p[ImportStationItemHandle.StationId] = StationId);
            return importTable;
        }
    }
}
