using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 批次转出命令
    /// </summary> 
    [Command(ImageName = "ArrowWithCircleDown", Label = "转出", ToolTip = "批次转出", GroupType = CommandGroupType.Edit)]
    public class BatchOutputCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            var mainModel = mainView?.Current as BatchDataCollectionViewModel;
            if (mainModel == null)
            {
                return false;
            }
            var model = view.Current as InputBatch;
            if (model == null)
            {
                return false;
            }
            if (view.SelectedEntities.OfType<InputBatch>().Count() > 1)
            {
                return false;
            }
            if (mainModel.ContainerNo.IsNotEmpty())
            {
                return false;
            }
            //return view.SelectedEntities.OfType<OutputBatch>().Count() == 1 && (mainView?.Current as BatchDataCollectionViewModel) != null;
            return base.CanExecute(view);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as BatchDataCollectionViewModel;
            try
            {
                if (vm == null)
                    throw new ValidationException("未找到批次采集主视图模型，请查看用户权限".L10N());
                vm.ClearTipsInfos();
                var input = view.Current as InputBatch;
                if (input == null)
                    throw new ArgumentNullException("转入批次不能为空".L10N());
                vm.NewBatchOutput(input);
                
            }
            catch (Exception exc)
            {
                vm.ShowError(exc);
            }
        }
    }
}