using System;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Wpf.Command;

namespace SIE.Wpf.CSM.Suppliers.Commonds
{
    /// <summary>
    /// 供应商复制添加
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "复制添加", GroupType = CommandGroupType.Edit)]
    public class CopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>返回结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var supplier = view.Current as Supplier;
            return supplier != null && base.CanExecute(view);
        }

        /// <summary>
        /// 实体创建后执行克隆操作
        /// </summary>
        /// <param name="entity">供应商</param>
        protected override void OnItemCreated(Entity entity)
        {
            var newSupplier = entity as Supplier;
            var oldSupplier = View.Current as Supplier;
            newSupplier.Code = oldSupplier.Code + "-复制".L10N();
            newSupplier.Name = oldSupplier.Name;
            newSupplier.Description = oldSupplier.Description;
            newSupplier.Type = oldSupplier.Type;
            newSupplier.State = oldSupplier.State;
            newSupplier.ShortName = oldSupplier.ShortName;
            newSupplier.Region = oldSupplier.Region;
            newSupplier.DutyParagraph = oldSupplier.DutyParagraph;
            newSupplier.Contacts = oldSupplier.Contacts;
            newSupplier.ContactNumber = oldSupplier.ContactNumber;
            newSupplier.ContactAddress = oldSupplier.ContactAddress;
            newSupplier.EMail = oldSupplier.EMail;
            newSupplier.ZipCode = oldSupplier.ZipCode;
            newSupplier.Remark = oldSupplier.Remark;
            newSupplier.IsPortal = oldSupplier.IsPortal;
            newSupplier.SourceType = oldSupplier.SourceType;

            foreach (var suppliseAddress in oldSupplier.AddressList)
            {
                var address = new SupplierAddress();
                address.Clone(suppliseAddress, new CloneOptions(CloneActions.NormalProperties));
                address.CreateBy = RT.IdentityId;
                address.UpdateBy = RT.IdentityId;
                address.CreateDate = DateTime.Now;
                address.UpdateDate = DateTime.Now;
                newSupplier.AddressList.Add(address);
            }

            foreach (var item in oldSupplier.ItemList)
            {
                newSupplier.ItemList.Add(item);
            }

            //var oldSupplierRequire = RT.Service.Resolve<SupplierExtController>().GetSupplierRequire(oldSupplier.Id);
            //if(oldSupplier!=null){

            //    SupplierRequire newSupplierRequire = new SupplierRequire();

            //    newSupplierRequire.SupplierId = newSupplier.Id;
            //    newSupplierRequire.Supplier = newSupplier;

            //    SupplierRequire newSupplierRequire1 = newSupplier.GetProperty(SupplierRequireDetailProperty.SupplierRequireProperty);
            //    newSupplierRequire1.Clone(oldSupplierRequire, new CloneOptions(CloneActions.NormalProperties));
            //}
        }
    }
}
