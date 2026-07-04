using SIE.CSM.Suppliers;
using SIE.Domain;
using System.Linq;

namespace SIE.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 禁用供应商时更新供方状态
    /// </summary>
    [System.ComponentModel.DisplayName("禁用供应商时更新供方状态")]
    [System.ComponentModel.Description("禁用供应商时更新物料检验特性表中的供方状态")]
    public class UpdateSupplierState : OnSubmitted<Supplier>
    {
        /// <summary>
        /// 禁用供应商时，同步更新物料检验特性表中的相应数据的供方状态
        /// </summary>
        /// <param name="entity">供应商</param>
        /// <param name="e">提交事件参数</param>
        protected override void Invoke(Supplier entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Update)
            {
                var result = RT.Service.Resolve<ItemInspCharacteristicsController>().GetInspCharacteristicsList(entity.Id);
                if (result == null) return;
                var enableList = result.Where(p => p.SupplierState == State.Enable).ToList();
                var disableList = result.Where(p => p.SupplierState == State.Disable).ToList();
                if (enableList.Count > 0 && entity.State == State.Disable)
                {
                    foreach (var item in enableList)
                    {
                        item.SupplierState = State.Disable;
                        RF.Save(item);
                    }
                }

                if (disableList.Count > 0 && entity.State == State.Enable)
                {
                    foreach (var item in disableList)
                    {
                        var supplierItem = RT.Service.Resolve<SupplierController>().GetSupplierItem(item.SupplierId, item.ItemId);
                        if (supplierItem != null)
                        {
                            item.SupplierState = State.Enable;
                            RF.Save(item);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 删除供应商所选择的物料时更新物料检验特性表的供方状态
    /// </summary>
    [System.ComponentModel.DisplayName("删除供应商选择的物料时更新供方状态")]
    [System.ComponentModel.Description("删除供应商选择的物料时更新物料检验特性表中的供方状态")]
    public class UpdateItemInspCharacterSupplierState : OnSubmitted<SupplierItem>
    {
        /// <summary>
        /// 删除供应商与物料关系表中的记录时，同步更新物料检验特性表中相应数据的供方状态
        /// </summary>
        /// <param name="entity">供应商与物料关系表</param>
        /// <param name="e">提交事件参数</param>
        protected override void Invoke(SupplierItem entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Delete)
            {
                if (entity == null) return;
                var result = RT.Service.Resolve<ItemInspCharacteristicsController>().GetInspCharacteristics(entity.SupplierId, entity.ItemId);
                if (result == null) return;
                result.SupplierState = State.Disable;
                RF.Save(result);
            }
        }
    }
}