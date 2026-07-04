using SIE.Domain.Validation;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.TemporaryRepairs.Commands
{
    /// <summary>
    /// 完成命令
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
            var model = view.Current as TemporaryRepairViewModel;
            return model != null && model.CanSubmit();
        }

        /// <summary>
        /// 返修完成命令执行方法
        /// </summary>
        /// <param name="view">明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Current.CastTo<TemporaryRepairViewModel>();
            ValidateResponsibility(model);
            //model.SaveRepairRecord();
            Submit(model);
        }

        /// <summary>
        /// 提交维修结果
        /// </summary>
        /// <param name="model">维修采集视图模型</param>
        /// <returns>true提交成功，false提交失败</returns>
        protected virtual bool Submit(TemporaryRepairViewModel model)
        {
            try
            {

                model.Submit(null);
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
        private static void ValidateResponsibility(TemporaryRepairViewModel model)
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
