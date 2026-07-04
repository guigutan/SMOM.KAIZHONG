using SIE.Domain;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Utils;
using SIE.Wpf;
using SIE.Wpf.Behaviors;
using SIE.Wpf.Command;
using System;
using System.Diagnostics;
using System.Linq;

namespace SIE.Wpf.Resources.Enterprises.Commands
{
    /// <summary>
    /// 生产单元Save命令类
    /// </summary>
    [Command(Label = "保存", ImageName = "SaveEntity", GroupType = CommandGroupType.Edit)]
    public class SaveProductCellCommand : ListSaveCommand
    {
        /// <summary>
        /// 生产单元Save的执行方法
        /// </summary>
        /// <param name="view">列表视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            OnSaving(view);
            view.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;

            var ctl = RT.Service.Resolve<EnterpriseController>();
            var productBomList = ctl.SaveProductCells(view.Data as EntityList<ProductionCellViewModel>);

            OnSaved(view);
            var behavious = view.FindBehavior<MementoViewBehavior>();
            if (behavious != null)
            {
                behavious.Manager.ClearUndoStack();
            }

            //view.Data.MarkSaved();
        }

        /// <summary>
        /// 生产单元Save后的执行操作
        /// </summary>
        /// <param name="view">列表视图对象</param>
        protected override void OnSaved(ListLogicalView view)
        {
            /*var ctl = RT.Service.Resolve<EnterpriseController>();
            var productBomList = ctl.SaveProductCells(view.Data as EntityList<ProductionCellViewModel>);*/

            CRT.MessageService.ShowInstantMessage("生产单元保存成功".L10N(), "保存提示", 3);
        }
    }

    #region shilei 注释--删除自定义方法
    /* [Command(ImageName ="DeleteEntity", Label ="删除", Location =CommandLocation.All, ToolTip ="删除数据", Gestures ="Delete", GroupType = CommandGroupType.Edit)]
    public class DeleteProductCellCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可以删除
        /// </summary>
        public bool? CanDelete;

        /// <summary>
        /// 控制命令是否可以执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>企业模型没有子企业模型的时候可以删除</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (CanDelete.HasValue)
            {
                return CanDelete.Value;
            }
            
            if (view!=null && view.Current != null)
            {
                var current = (view.Current as ProductionCellViewModel).ProductionCell;
                if (current != null)
                { 
                    //&& current.LevelId != RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Group).FirstOrDefault().LevelId
                    var resutl = !RT.Service.Resolve<EnterpriseController>().EnterpriseHasChild(current.Id);
                    CanDelete = resutl;
                }
                return CanDelete != null ? CanDelete.Value : false;
            }

            return false;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (CRT.MessageService.AskQuestion(string.Format("你确认删除选择的{0}条数据吗？", view.SelectedEntities.Count)) != true)
                return;

            ////var selectedItems = view.SelectedEntities.OfType<ProductionCellViewModel>().ToList();
            var selectedItems = view.Data as EntityList<ProductionCellViewModel>;
            var ctl = RT.Service.Resolve<EnterpriseController>();
            var criteria = new ProductionCellViewModelCriteria();
            var list = ctl.DeleteProductCells(selectedItems);

            view.Data = list;
            view.Data.MarkSaved();
            view.RefreshControl();
        }
    } */
    #endregion shilei 注释--删除自定义方法
}