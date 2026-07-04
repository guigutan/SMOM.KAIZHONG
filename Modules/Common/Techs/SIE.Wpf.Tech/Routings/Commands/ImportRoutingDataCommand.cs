using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Routings;
using SIE.Tech.Routings.ImportRoutings;
using SIE.Wpf.Command;
using SIE.Wpf.Tech.Routings.ViewModels;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.Tech.Routings.Commands
{
    /// <summary>
    /// 工单 导入命令
    /// </summary>
    [Command(Label = "导入", ToolTip = "导入", Location = CommandLocation.All, GroupType = CommandGroupType.Business)]
    public class ImportRoutingDataCommand : DetailViewCommand
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
            var current = view.Data as ImportRoutingCheckViewModel;
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
            var current = view.Data as ImportRoutingCheckViewModel;
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
                        RuntingImportDataHandle importWOHandle = new RuntingImportDataHandle();
                        string strMsg = importWOHandle.ImportProcess(importFilePath, typeof(ImportRoutingHandle), (drSuccess, drFailed) =>
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
        private void ImportCompleted(DataRow[] importSuccess, DataRow[] importFailed, ImportRoutingCheckViewModel current)
        {
            EntityList<RoutingCheckDataViewModel> importList = new EntityList<RoutingCheckDataViewModel>();
            importSuccess.ForEach(dr =>
            {
                importList.Add(CreateCheckData(dr));
            });
            importFailed.ForEach(dr =>
            {
                importList.Add(CreateCheckData(dr));
            });
            CRT.MainThread.InvokeAsync(() =>
            {
                current.ImportDataViewModelList.Clear();
                current.ImportFailAmount = importList.Count(p => p.IsSuccess == false);
                current.ImportSuccessAmount = importList.Count(p => p.IsSuccess == true);
                current.ImportDataViewModelList.AddRange(importList.OrderBy(p => p.RowNum));
                current.NotifyAllPropertiesChanged();
                current.MarkSaved();
                current.ImportDataViewModelList.MarkSaved();
                current.ImportFinish();
                if (current.ImportSuccessAmount > 0)
                    RefreshData();
            });

        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            var type = typeof(EntitySavedEvent<>).MakeGenericType(typeof(Routing));
            var @event = Activator.CreateInstance(type, new Routing());
            RT.EventBus.Publish(@event);
        }

        /// <summary>
        /// 创建工艺路线视图模型
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns>工艺路线视图模型</returns>
        private RoutingCheckDataViewModel CreateCheckData(DataRow dr)
        {
            var vm = new RoutingCheckDataViewModel()
            {
                Category = dr[0].ToString(),
                RoutingName = dr[1].ToString(),
                RoutingDesc = dr[2].ToString(),
                ProcessName = dr[3].ToString(),
                StrSortOrder = dr[4].ToString(),
                StrSortOrderBack = dr[5].ToString(),
                StrResult = dr[6].ToString(),
                ResultDesc = dr[7].ToString(),
                StrCanChoose = dr[8].ToString(),
                StrIsRepeat = dr[9].ToString(),
                StrIsSku = dr[10].ToString(),
                ErrorMessage = dr[ImportDataHandle.MessageColumnName].ToString(),
                RowNum = ConvertInt(dr[ImportDataHandle.RowIndex].ToString()),
            };
            if (!dr[ImportRoutingHandle.ImportSuccess].ToString().IsNullOrEmpty())
                vm.IsSuccess = Convert.ToBoolean(dr[ImportRoutingHandle.ImportSuccess].ToString());
            return vm;
        }

        /// <summary>
        /// 整数转换
        /// </summary>
        /// <param name="str">str</param>
        /// <returns>int?</returns>
        private int ConvertInt(string str)
        {
            int rst = -1;
            int.TryParse(str, out rst);
            return rst;
        }
    }

    /// <summary>
    /// 导入处理逻辑类
    /// </summary>
    public class RuntingImportDataHandle : ImportDataHandle
    {
        /// <summary>
        /// 处理导入数据之前执行
        /// </summary>
        /// <param name="importTable">处理导入数据</param>
        /// <param name="type">处理导入数据的业务类</param>
        /// <returns>返回需要处理的导入数据</returns>
        public override DataTable ProcessImportDataPre(DataTable importTable, Type type)
        {
            //添加导入成功列,用于标识成功导入的工艺路线，界面提示导入成功数量
            importTable.Columns.Add(ImportRoutingHandle.ImportSuccess);
            return importTable;
        }
    }
}