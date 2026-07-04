using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Tech.Processs.Commands
{
    /// <summary>
    /// 采集步骤下移
    /// </summary>
    [Command(ImageName = "ArrowLongDown",
        Label = "下移",
        ToolTip = "下移",
        GroupType = CommandGroupType.Business)]
    public class ProcessCollectStepDownCommand : ListViewCommand
    {
        /// <summary>
        /// 能否执行修改
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Data == null || view.Current == null)
                return false;
            var processCollectStepList = view.Data as EntityList<ProcessCollectStep>;
            var processCollectStep = view.Current as ProcessCollectStep;
            if (processCollectStepList != null)
            {
                var bar = processCollectStepList.OfType<ProcessCollectStep>().OrderByDescending(m => m.Step).FirstOrDefault();
                if (bar.Step == processCollectStep.Step)
                    return false;
            }

            if (view.Data.Count < 1)
                return false;
            return true;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var processCollectStepList = view.Data as EntityList<ProcessCollectStep>;
            var current = view.Current as ProcessCollectStep;
            var next = processCollectStepList.FirstOrDefault(p => p.Step == current.Step + 1);
            if (next != null)
            {
                var temp = 0;
                temp = next.Step;
                next.Step = current.Step;
                current.Step = temp;
                processCollectStepList.Remove(current);
                processCollectStepList.Insert(processCollectStepList.IndexOf(next) + 1, current);
                if (current.PersistenceStatus != PersistenceStatus.New)
                {
                    current.PersistenceStatus = PersistenceStatus.Modified;
                }
            }

            view.RefreshControl();
            view.Current = processCollectStepList.FirstOrDefault();
        }
    }

    /// <summary>
    /// 采集步骤上移
    /// </summary>
    [Command(ImageName = "ArrowLongUp",
        Label = "上移",
        ToolTip = "上移",
        GroupType = CommandGroupType.Business)]
    public class ProcessCollectStepUpCommand : ListViewCommand
    {
        /// <summary>
        /// 能否执行修改
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Data == null || view.Current == null)
                return false;
            var processCollectStepList = view.Data as EntityList<ProcessCollectStep>;
            var processCollectStep = view.Current as ProcessCollectStep;

            if (processCollectStepList != null)
            {
                var bar = processCollectStepList.OfType<ProcessCollectStep>().OrderBy(m => m.Step).FirstOrDefault();
                if (bar.Step == processCollectStep.Step)
                {
                    return false;
                }
            }

            if (view.Data.Count < 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var processCollectStepList = view.Data as EntityList<ProcessCollectStep>;
            var current = view.Current as ProcessCollectStep;
            var previous = processCollectStepList.FirstOrDefault(p => p.Step == current.Step - 1);
            if (previous != null)
            {
                var temp = 0;
                temp = previous.Step;
                previous.Step = current.Step;
                current.Step = temp;
                processCollectStepList.Remove(current);
                processCollectStepList.Insert(processCollectStepList.IndexOf(previous), current);
                if (current.PersistenceStatus != PersistenceStatus.New)
                    current.PersistenceStatus = PersistenceStatus.Modified;
            }

            view.RefreshControl();
            view.Current = current;
        }
    }

    /// <summary>
    /// 采集步骤添加
    /// </summary>
    [Command(ImageName = "AddEntity",
        Label = "添加",
        ToolTip = "添加数据",
        Gestures = "Ctrl+Shift+N",
        GroupType = CommandGroupType.Edit)]
    public class ProcessCollectStepPopupAddCommand : ListAddCommand
    {
        /// <summary>
        /// 能否执行修改
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            //维修，采集步骤只允许有一个
            var process = view.Parent.Current as Process;
            if (process == null) return false;
            if (process.Type == ProcessType.Fix && process.CollectStepList.Count == 1)
                return false;
            return true;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
        }

        /// <summary>
        /// 命令执行前（如果是行内编辑，则在视图中新增一行）
        /// </summary>
        /// <param name="editEntity">当前实体</param>
        protected override void OnEditting(Entity editEntity)
        {
            if (View.Meta.EditMode == EditMode.Inline)
            {
                var parent = View.Parent.Current as Process;
                var current = editEntity as ProcessCollectStep;

                if (current != null)
                {
                    if (parent != null && current.Step == 0 && parent.CollectStepList.Count != 0)
                    {
                        current.Step = parent.CollectStepList.Max(p => p.Step) + 1;
                    }
                    else
                    {
                        current.Step = 1;
                    }

                    View.Data.Add(editEntity);
                    View.Current = editEntity;
                    View.TrySetFocus(View.Data.Count - 1, ProcessCollectStep.BarcodeTypeProperty.Name);
                }
            }
        }
    }

    ///// <summary>
    ///// 采集步骤保存
    ///// </summary>
    //[Command(ImageName = "SaveEntity",
    //Label = "保存",
    //ToolTip = "保存数据",
    //Gestures = "Ctrl+S",
    //GroupType = CommandGroupType.Edit)]
    //public class ProcessCollectStepPopupSaveCommand : ListSaveCommand
    //{
    //    /// <summary>
    //    /// 执行命令
    //    /// </summary>
    //    /// <param name="view">当前列表逻辑视图</param>
    //    public override void Execute(ListLogicalView view)
    //    {
    //        var list = view.Data.OfType<ProcessCollectStep>();
    //        if (list.Any(p => p.BarcodeType == 0))
    //            throw new ValidationException("条码类型不能为空".L10N());
    //        //if (list.Count() == 0)
    //        //    throw new ValidationException("采集步骤不能为空".L10N());
    //        base.Execute(view);
    //    }
    //}
}