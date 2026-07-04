using SIE.Domain.Validation;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Repairs.Commands
{
    /// <summary>
    /// 返修完成命令
    /// </summary> 
    [Command(ImageName = "SettingFinish", Label = "维修完成", ToolTip = "维修完成", GroupType = 10)]
    public class SubmitCommand : DetailViewCommand
    {
        /// <summary>
        /// 判断返修完成命令能否执行
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        /// <returns>true可点击，false不可点击</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var model = view.Current as RepairViewModel;
            return model != null && model.CanSubmit() && model.CanRepairComplete();
        }

        /// <summary>
        /// 返修完成命令执行方法
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Current.CastTo<RepairViewModel>();
            ValidateResponsibility(model);
            var upline = new UplineViewModel(model);
            var template = new DetailsUITemplate(typeof(UplineViewModel), ViewConfig.DetailsView, view.ModuleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = upline;
            CRT.Workbench.ShowDialog(ui, w =>
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

        /// <summary>
        /// 提交维修结果
        /// </summary>
        /// <param name="model">维修采集视图模型</param>
        /// <param name="upline">返修完成视图模型</param>
        /// <returns>true提交成功，false提交失败</returns>
        protected virtual bool Submit(RepairViewModel model, UplineViewModel upline)
        {
            var broken = upline.Validate(ValidatorActions.None);
            if (broken.Count > 0)
            {
                CRT.MessageService.ShowInstantMessage(broken.ToString(), "提示".L10N(), 5);
                return false;
            }

            try
            {
                if (upline.UplineProcess == null)
                {
                    CRT.MessageService.ShowInstantMessage("请选择上线工序".L10N(), "提示".L10N(), 5);
                    return false;
                }

                model.Submit(upline.UplineProcess.RoutingProcessId);
                return true;
            }
            catch (Exception exc)
            {
                exc.Alert();
                return false;
            }
        }

        /// <summary>
        /// 验证维修措施，缺陷责任
        /// </summary>
        /// <param name="model">维修采集视图模型</param>
        private static void ValidateResponsibility(RepairViewModel model)
        {
            if (model.RepairDefectList.Any(p => p.MeasureList.Count == 0))
            {
                throw new ValidationException("缺陷[{0}]维修措施不能为空".L10nFormat(model.RepairDefectList.FirstOrDefault(p => p.MeasureList.Count == 0)?.WipProductDefect?.Defect?.Code));
            }

            if (model.RepairDefectList.Any(p => p.ResponsibilityList.Count == 0))
            {
                throw new ValidationException("缺陷[{0}]缺陷责任不能为空".L10nFormat(model.RepairDefectList.FirstOrDefault(p => p.ResponsibilityList.Count == 0)?.WipProductDefect?.Defect?.Code));
            }
        }
    }
}
