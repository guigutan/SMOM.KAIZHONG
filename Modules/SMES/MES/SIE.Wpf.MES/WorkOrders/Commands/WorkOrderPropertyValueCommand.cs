using SIE.Domain;
using SIE.Items.ViewModels;
using SIE.MES.WorkOrders;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 添加工单属性值
    /// </summary>
    [Command(Label = "添加", ImageName = "AddEntity", GroupType = CommandGroupType.Edit)]
    public class AddWoPropertyValueCommand : ListAddCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Parent?.Current as WorkOrder;
            return view.CanAddItem() && parent != null && parent.Product != null;
        }

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <returns>创建的实体</returns>
        protected override Entity CreateNewItem()
        {
            var property = base.CreateNewItem() as PropertyValueViewModel;
            var parent = View.Parent.Current as WorkOrder;
            property.Type = parent.GetType();
            property.ParentId = parent.Id;
            if (parent.Product != null)
                property.ItemId = parent.ProductId;

            return property;
        }
    }

    /// <summary>
    /// 添加工单BOM属性值
    /// </summary>
    [Command(Label = "添加", ImageName = "AddEntity", GroupType = CommandGroupType.Edit)]
    public class AddWoBomPropertyValueCommand : ListAddCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Parent?.Current as WorkOrderBom;
            return view.CanAddItem() && parent != null && parent.Item != null;
        }

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <returns>创建的新实体</returns>
        protected override Entity CreateNewItem()
        {
            var property = base.CreateNewItem() as PropertyValueViewModel;
            var parent = View.Parent.Current as WorkOrderBom;
            property.Type = parent.GetType();
            property.ParentId = parent.Id;
            if (parent.Item != null)
                property.ItemId = parent.ItemId;

            return property;
        }
    }

    /// <summary>
    /// 添加工单BOM属性值
    /// </summary>
    [Command(Label = "添加", ImageName = "AddEntity", GroupType = CommandGroupType.Edit)]
    public class AddWoProcessBomPropertyValueCommand : ListAddCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var parent = view.Parent?.Current as WorkOrderProcessBom;
            return view.CanAddItem() && parent != null && parent.Item != null;
        }

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <returns>创建后的实体</returns>
        protected override Entity CreateNewItem()
        {
            var property = base.CreateNewItem() as PropertyValueViewModel;
            var parent = View.Parent.Current as WorkOrderProcessBom;
            property.Type = parent.GetType();
            property.ParentId = parent.Id;
            if (parent.Item != null)
                property.ItemId = parent.ItemId;

            return property;
        }
    }
}
