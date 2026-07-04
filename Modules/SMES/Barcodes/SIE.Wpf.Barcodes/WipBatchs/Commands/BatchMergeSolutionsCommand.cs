using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 合并规则命令
    /// </summary>
    [Command(ImageName = "Layer", Label = "合并规则", GroupType = CommandGroupType.Edit)]
    public class BatchMergeSolutionsCommand : ListViewCommand
    {
        /// <summary>
        /// 执行弹窗
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(BatchMergeSolutions), ViewConfig.ListView);
            var ui = template.CreateUI();
            ui.MainView.Data = RF.GetAll<BatchMergeSolutions>();
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                w.Title = "合并规则".L10N();
                w.MinHeight = 550;
                w.MinWidth = 750;
                w.Height = 550;
                w.Width = 750;
            });
        }
    }

    /// <summary>
    /// 设置组织层级为资源
    /// </summary>
    [Command(Label = "设为缺省")]
    public class SetDefaultCommand : ListViewCommand
    {
        /// <summary>
        /// 控制命令是否可执行的逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>只有组织层级不是资源时此命令才可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            //有且只能有一个是缺省
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<BatchMergeSolutions>().All(p => !p.IsDefault && p.PersistenceStatus != PersistenceStatus.New)
                && RT.Service.Resolve<WipBatchController>().GetDefaultSolution() == null;
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var selected = view.SelectedEntities;
            var result = CRT.MessageService.AskQuestion("确定设定当前选中的{0}行为缺省吗?".L10nFormat(selected.Count));
            if (result)
            {
                foreach (BatchMergeSolutions solution in selected.OfType<BatchMergeSolutions>())
                {
                    var rtn = RT.Service.Resolve<WipBatchController>().SetDefault(solution.Id, true);
                    solution.Clone(rtn, CloneOptions.ReadDbRow());    //克隆对象
                    solution.NotifyAllPropertiesChanged();
                    solution.MarkSaved();
                }
            }
        }
    }

    /// <summary>
    /// 设置组织层级为资源
    /// </summary>
    [Command(Label = "取消缺省")]
    public class CancelDefaultCommand : ListViewCommand
    {
        /// <summary>
        /// 控制命令是否可执行的逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>只有组织层级不是资源时此命令才可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<BatchMergeSolutions>().All(p => p.IsDefault && p.PersistenceStatus != PersistenceStatus.New);
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var selected = view.SelectedEntities;
            var result = CRT.MessageService.AskQuestion("确定取消当前选中的{0}行缺省吗?".L10nFormat(selected.Count));
            if (result)
            {
                foreach (BatchMergeSolutions solution in selected.OfType<BatchMergeSolutions>())
                {
                    var rtn = RT.Service.Resolve<WipBatchController>().SetDefault(solution.Id, false);
                    solution.Clone(rtn, CloneOptions.ReadDbRow());    //克隆对象
                    solution.NotifyAllPropertiesChanged();
                    solution.MarkSaved();
                }
            }
        }
    }
}
