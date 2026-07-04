using SIE.CSM.Suppliers;
using SIE.Wpf.Command;
using System;
using SIE.Domain;

namespace SIE.Wpf.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商地址默认按钮
    /// </summary>
    [Command(ImageName = "FlagTriangle", Label = "设为缺省", ToolTip = "设为缺省", GroupType = CommandGroupType.Business)]
    public class SupplierAddressDefaultCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">逻辑视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            SupplierAddress supplierAddress = view.Current as SupplierAddress;
            return supplierAddress != null && view.HasSelectedEntities && supplierAddress.PersistenceStatus == Domain.PersistenceStatus.Unchanged;
        }

        /// <summary>
        /// 设置默认逻辑
        /// </summary>
        /// <param name="view">逻辑视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            //if (!CRT.MessageService.AskQuestion("是否设置当前选择地址为默认地址？".L10N()))
            //{
            //    return;
            //}
            SupplierAddress supplierAddress = view.Current as SupplierAddress;
            RT.Service.Resolve<SupplierController>().SetDefaultAddress(supplierAddress.SupplierId.Value, supplierAddress.Id);
            if (supplierAddress.IsDefault)
            {
                CRT.MessageService.ShowInstantMessage("取消默认地址成功！".L10N(), "提示".L10N(), 3);
            }
            else
            {
                CRT.MessageService.ShowInstantMessage("设置默认地址成功！".L10N(), "提示".L10N(), 3);
            }

            view.Parent.QueryView.TryExecuteQuery();
        }
    }

    /// <summary>
    /// 复制添加供应商地址
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "复制添加", GroupType = CommandGroupType.Edit)]
    public class AddressCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var supplierAddress = view.Current as SupplierAddress;
            return supplierAddress != null && base.CanExecute(view);
        }

        /// <summary>
        /// 实体创建后执行克隆操作
        /// </summary>
        /// <param name="entity">供应商地址</param>
        protected override void OnItemCreated(Entity entity)
        {
            var address = entity as SupplierAddress;
            var supplier = View.Current as SupplierAddress;
            address.Clone(supplier, new CloneOptions(CloneActions.NormalProperties));
            address.IsDefault = false;
            address.CreateBy = RT.IdentityId;
            address.UpdateBy = RT.IdentityId;
            address.CreateDate = DateTime.Now;
            address.UpdateDate = DateTime.Now;
        }
    }
}