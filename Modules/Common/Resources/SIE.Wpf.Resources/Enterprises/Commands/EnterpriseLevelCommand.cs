using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Wpf.Command;
using System;
using System.Diagnostics;

namespace SIE.Wpf.Resources.Enterprises.Commands
{
    /// <summary>
    /// 添加子节点命名
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加子", Location = MetaModel.View.CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class AddLevelChildCommand : TreeAddChildCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return base.CanExecute(view) && view.Current != null;
        }

        /// <summary>
        /// 实体创建后更改其中某字段的值
        /// </summary>
        /// <param name="entity">企业层级</param>
        protected override void OnItemCreated(Entity entity)
        {
            var enterprise = entity as EnterpriseLevel;
            Debug.WriteLine("企业架构父ID{0}", enterprise.TreePId);
            enterprise.InvOrgId = AppRuntime.InvOrg;
            enterprise.PropertyChanged += Enterprise_PropertyChanged;
        }

        /// <summary>
        /// 企业层级类型属性变更
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">属性变更事件参数</param>
        private void Enterprise_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var level = sender as EnterpriseLevel;
            if (e.PropertyName == nameof(level.Type))
            {
                if(level.Code.IsNullOrWhiteSpace())
                    level.Code = level.Type?.ToString();
            }
        }
    }

    /// <summary>
    /// 删除企业层级命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", Location = CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class DeleteLevelCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可以删除
        /// </summary>
        public bool? CanDelete { get; set; }

        /// <summary>
        /// 控制命令是否可以执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>企业层级没有子企业层级时可以删除</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (CanDelete.HasValue)
            {
                return CanDelete.Value;
            }

            var current = view.Current as EnterpriseLevel;
            if (view.Current != null && current != null)
            {
                var resutl = !RT.Service.Resolve<EnterpriseController>().EnterpriseLevelHasChild(current.Id);
                CanDelete = resutl;
                return CanDelete.Value;
            }

            return false;
        }
    }
}
