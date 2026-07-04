using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Routings.Technologys;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.ProductRoutings.Commands
{
    /// <summary>
    /// 产品工艺路线 工序BOM 添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", GroupType = CommandGroupType.Edit)]
    public class AddBomCommand : ListAddCommand
    {
        /// <summary>
        /// 判断是否可以添加
        /// </summary>
        /// <param name="view">当前视图对象</param>
        /// <returns>返回是否可添加</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view["Model"] as ProductRoutingViewModel;
            if (model == null || model.SelectedItem == null || model.SelectedItem.Type != ActivityType.Interaction || model.Version == null || model.Version.IsPause == YesNo.No)
                return false;
            return base.CanExecute(view);
        }

        /// <summary>
        /// 实体创建后
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            BomViewModel bomViewModel = entity as BomViewModel;
            bomViewModel.Qty = 1m;
            base.OnItemCreated(entity);
        }
    }

    /// <summary>
    /// 修改列表
    /// </summary>
    [Command(Label = "修改", ImageName = "EditEntity", Location = CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class EditBomCommand : ListEditCommand
    {
        /// <summary>
        /// 判断是否可以修改
        /// </summary>
        /// <param name="view">当前视图对象</param>
        /// <returns>返回是否可修改</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view["Model"] as ProductRoutingViewModel;
            if (model == null || model.SelectedItem == null || model.SelectedItem.Type != ActivityType.Interaction || model.Version == null || model.Version.IsPause == YesNo.No)
                return false;
            return base.CanExecute(view);
        }
    }

    /// <summary>
    /// 删除列表中选中的对象
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Location = CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class DeleteBomCommand : ListDeleteCommand
    {
        /// <summary>
        /// 判断是否可以删除
        /// </summary>
        /// <param name="view">当前视图对象</param>
        /// <returns>返回是否可删除</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view["Model"] as ProductRoutingViewModel;
            if (model == null || model.SelectedItem == null || model.SelectedItem.Type != ActivityType.Interaction || model.Version == null || model.Version.IsPause == YesNo.No)
                return false;
            return base.CanExecute(view);
        }
    }
}
