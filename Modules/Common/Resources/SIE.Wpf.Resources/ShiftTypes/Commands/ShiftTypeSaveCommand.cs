using SIE.Diagnostics;
using SIE.Domain;
using SIE.Resources.ShiftTypes;
using SIE.Wpf.Behaviors;
using SIE.Wpf.Command;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SIE.Wpf.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 保存班制
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存数据", GroupType = CommandGroupType.Edit)]
    public class ShiftTypeSaveCommand : ListViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            //return view.Parent?.Current?.PersistenceStatus != PersistenceStatus.New && (view.Control.View.AllowEditing || view.Data.IsDirty);
            return  view.Data.IsDirty;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            using (DebugTrace.Start("UpdateShiftType".L10N()))
            {
                OnSaving(view);

                var shiftTypes = view.Data as EntityList<ShiftType>;

                //找出修改的班制ID
                var modifiedShiftTypeIds = new List<double>();
                var modifiedShiftTypes =
                    shiftTypes.Where(x => x.IsDirty
                        && (x.PersistenceStatus == PersistenceStatus.Modified
                            || x.PersistenceStatus == PersistenceStatus.Unchanged))
                    .ToList();

                if (modifiedShiftTypes != null && modifiedShiftTypes.Any())
                {
                    modifiedShiftTypeIds = modifiedShiftTypes.Select(x => x.Id).ToList();
                }

                RF.Save(shiftTypes);

                var win = new WaitDialog();
                win.Width = 500;
                win.ShowInTaskbar = false;
                win.Text = "正在保存班制资料，请稍等……".L10N();
                win.ProgressValue = new ProgressValue()
                {
                    Percent = 100
                };

                Exception exception = null;

                ThreadPool.QueueUserWorkItem(oo =>
                {
                    try
                    {
                        RT.Service.Resolve<ShiftTypeController>().SendShiftTypeModifyMessage(modifiedShiftTypeIds);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    Action ac = () => win.DialogResult = true;
                    win.Dispatcher.BeginInvoke(ac);
                });

                win.Topmost = true;

                win.ShowDialog();

                if (exception != null)
                {
                    CRT.MessageService.ShowException(exception);
                }

                view.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;

                OnSaved(view);

                var behavious = view.FindBehavior<MementoViewBehavior>();
                if (behavious != null)
                {
                    behavious.Manager.ClearUndoStack();
                }
            }
        }

        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        protected virtual void OnSaving(ListLogicalView view) { }

        /// <summary>
        /// 保存后
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        protected virtual void OnSaved(ListLogicalView view)
        {
            view.QueryView?.TryExecuteQuery();
        }
    }
}