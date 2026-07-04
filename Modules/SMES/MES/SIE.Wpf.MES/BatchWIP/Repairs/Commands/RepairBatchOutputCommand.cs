using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Repairs.Commands
{
    /// <summary>
    /// 批次转出命令
    /// </summary> 
    [Command(ImageName = "ArrowWithCircleDown", Label = "转出", ToolTip = "维修批次转出", GroupType = CommandGroupType.Edit)]
    public class RepairBatchOutputCommand : ListViewCommand
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view.Relations.Find("mainView")?.Data as BatchRepairViewModel;
            return view.Current != null && model != null /*&& !model.DefectList.IsDirty */&& model.CanSubmit();
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var model = view.Relations.Find("mainView")?.Data as BatchRepairViewModel;
            try
            {
                ValidateResponsibility(model);

                var inputBatch = view.Current as InputBatch;
                var upline = new BatchUplineViewModel(model, inputBatch);
                var template = new DetailsUITemplate<BatchUplineViewModel>();
                var ui = template.CreateUI();
                ui.MainView.Data = upline;

                if (model.BatchRepairDefectList.Sum(p => p.ScrapQty) >= inputBatch.RemainQty || !upline.HasNextProcess)
                {
                    StraightSubmit(model, upline);
                }
                else
                {
                    CRT.Workbench.ShowDialog(upline.ViewKey, ui.Control, w =>
                    {
                        w.Title = "上线工序".L10N();
                        w.Height = 150;
                        w.Width = 350;
                        w.Closing += (s, e) =>
                        {
                            if (w.Result == 0)
                            {
                                e.Cancel = !Submit(model, upline);
                            }
                        };
                    });
                }
            }
            catch (Exception ex)
            {
                model.ShowError(ex);
            }
        }

        /// <summary>
        /// 直接提交
        /// </summary>
        /// <param name="model">维修Model</param>
        /// <param name="upline">返修完成Model</param>
        /// <returns>提交结果</returns>
        bool Submit(BatchRepairViewModel model, BatchUplineViewModel upline)
        {
            try
            {
                var broken = upline.Validate(ValidatorActions.None);
                if (broken.Count > 0)
                {
                    throw new ValidationException(broken.ToString());
                }

                return StraightSubmit(model, upline);
            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowException(exc, "验证异常".L10N());
                return false;
            }
        }

        /// <summary>
        /// 提交维修结果
        /// </summary>
        /// <param name="model">维修采集视图模型</param>
        /// <param name="upline">返修完成视图模型</param>
        /// <returns>true提交成功，false提交失败</returns>
        protected virtual bool StraightSubmit(BatchRepairViewModel model, BatchUplineViewModel upline)
        {
            try
            {
                var obj = View.Current as InputBatch;
                model.Submit(obj, upline);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 验证维修措施，缺陷责任
        /// </summary>
        /// <param name="model">维修采集视图模型</param>
        private void ValidateResponsibility(BatchRepairViewModel model)
        {
            if (model.BatchRepairDefectList.Any(p => p.MeasureList.Count == 0))
            {
                var deftCode = model.BatchRepairDefectList.FirstOrDefault(p => p.MeasureList.Count == 0)?.WipProductDefect?.DetailList?.Select(p => p.Defect).FirstOrDefault().Code;

                throw new ValidationException("缺陷[{0}]维修措施不能为空".L10nFormat(deftCode));
            }

            if (model.BatchRepairDefectList.Any(p => p.ResponsibilityList.Count == 0))
            {
                throw new ValidationException("缺陷[{0}]缺陷责任不能为空".L10nFormat(model.BatchRepairDefectList.FirstOrDefault(p => p.ResponsibilityList.Count == 0)?.WipProductDefect?.DetailList?.Select(p => p.Defect).FirstOrDefault().Code));
            }
        }
    }
}
