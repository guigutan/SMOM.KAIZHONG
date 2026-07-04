using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Wpf.Command;
using System.Diagnostics;
using System.Linq;

namespace SIE.Wpf.Resources.Enterprises.Commands
{
    /// <summary>
    /// 添加子节点命名
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加子", Location = MetaModel.View.CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class AddChildCommand : TreeAddChildCommand
    {
        /// <summary>
        /// 实体创建后更改其中某字段的值
        /// </summary>
        /// <param name="entity">企业模型</param>
        protected override void OnItemCreated(Entity entity)
        {
            var enterprise = entity as Enterprise;
            Debug.WriteLine("企业架构父ID{0}", enterprise.TreePId);
            enterprise.InvOrgId = AppRuntime.InvOrg;
        }
    }

    /// <summary>
    /// 删除企业模型命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", Location = CommandLocation.All, ToolTip = "删除数据", Gestures = "Delete", GroupType = CommandGroupType.Edit)]
    public class DeleteEnterpriseCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可以删除
        /// </summary>
        public bool? CanDelete { get; set; }

        /// <summary>
        /// 控制命令是否可以执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>企业模型没有子企业模型的时候可以删除</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view == null)
            {
                return false;
            }
            if (CanDelete.HasValue)
            {
                return CanDelete.Value;
            }

            var current = view.Current as Enterprise;
            if (view.Current != null && current != null && current.Level?.Type != EnterpriseType.Group)
            {
                var resutl = !RT.Service.Resolve<EnterpriseController>().EnterpriseHasChild(current.Id);
                CanDelete = resutl;
                return CanDelete.Value;
            }
            CanDelete = false;
            return CanDelete.Value;
        }
    }


    /// <summary>
    /// 保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存", GroupType = CommandGroupType.Edit)]
    public class EnterpriseSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 保存执行
        /// </summary>
        /// <param name="view"></param>
        public override void Execute(ListLogicalView view)
        {
            if(view == null)
            {
                return;
            }
            var deleteList = view.Data.DeletedList.OfType<Enterprise>().Select(p => p.Id).ToList();
            if (deleteList.Count > 0)
            {
                RT.Service.Resolve<WipResourceController>().DeleteSchResourse(deleteList, SyncSourceType.Enterprise);
            }
        }
    }
}
