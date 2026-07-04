using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechs.ImportProcessTech;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.ProcessTechs.ViewModels;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SIE.Wpf.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺 导入命令
    /// </summary>
    [Command(Label = "导入", ToolTip = "导入", Location = CommandLocation.All, GroupType = CommandGroupType.Business)]
    internal class ImportProcessTechViewModelCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否正在导入中
        /// </summary>
        private bool isRun;

        /// <summary>
        /// 判断逻辑是否可执行
        /// </summary>
        /// <param name="view">当前视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var model = view.Data as ImportProcessTechViewModel;
            if (base.CanExecute(view) && !isRun && model != null && !string.IsNullOrEmpty(model.ImportFilePath))
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
            var model = view.Data as ImportProcessTechViewModel;
            model.ImportProcessMsg = "正在导入...";
            if (model != null)
            {
                try
                {
                    isRun = true;
                    string importFilePath = model.ImportFilePath;
                    Task<string> task = new Task<string>(() =>
                    {
                        ImportDataHandle importCtgInspHandle = new ImportDataHandle();
                        string strMsg = importCtgInspHandle.ImportProcess(importFilePath, typeof(ImportProcessTechHandle), (drSuccess, drFailed) =>
                        {
                            ImportCompleted(drSuccess, drFailed, model);
                        });
                        return strMsg;
                    });
                    task.Start();

                   task.ContinueWith(t =>
                    {
                        model.ImportProcessMsg = t.Result;
                        isRun = false;
                    });
                }
                catch (Exception ex)
                {
                    model.ImportProcessMsg = "异常消息:" + ex.Message + "。详细信息:" + ex.StackTrace;
                    isRun = false;
                }
            }
        }

        /// <summary>
        /// 导入完成触发
        /// </summary>
        /// <param name="success">导入成功数据集</param>
        /// <param name="failed">导入失败数据集</param>
        /// <param name="model">当前界面数据对象</param>
        private void ImportCompleted(DataRow[] success, DataRow[] failed, ImportProcessTechViewModel model)
        {
            EntityList<ImportProcessTechDetailViewModel> errorList = new EntityList<ImportProcessTechDetailViewModel>();
            foreach (DataRow dr in failed)
            {
                ImportProcessTechDetailViewModel errorRow = new ImportProcessTechDetailViewModel();
                
                errorRow.Code = dr[0].ToString();
                errorRow.Name = dr[1].ToString();
                errorRow.ProcessTechType = dr[2].ToString();
                errorRow.ProcessSegment = dr[3].ToString();
                errorRow.IsBottleneck = dr[4].ToString();
                errorRow.TransferTime = dr[5].ToString();
                errorRow.SAM = dr[6].ToString();
                errorRow.WorkingHours = dr[7].ToString();
                errorRow.ErrorMessage = dr[8].ToString();
                errorList.Add(errorRow);
            }

            CRT.MainThread.InvokeAsync(() =>
            {
                model.ImportDataViewModelList.Clear();
                model.ImportFailAmount = failed.Length;
                model.ImportSuccessAmount = success.Length;
                model.ImportDataViewModelList.AddRange(errorList);
                model.NotifyAllPropertiesChanged();
                if (success.Length > 0)
                    RefreshData();
            }); 
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            var type = typeof(EntitySavedEvent<>).MakeGenericType(typeof(ProcessTech));
            var @event = Activator.CreateInstance(type, new ProcessTech());
            RT.EventBus.Publish(@event);
        }
    }
}
