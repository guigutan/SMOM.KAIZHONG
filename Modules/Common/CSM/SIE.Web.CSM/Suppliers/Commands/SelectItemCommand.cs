using SIE.CSM.ItemInspCharacteristicses;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EventMessages.QMS;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.CSM.Suppliers.Commands
{
    /// <summary>
    /// 供应商选择账户按钮
    /// </summary>
    public class SelectItemCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args != null)
            {
                //获取元数据
                var meta = ClientEntities.Find("SIE.CSM.Suppliers.SupplierItem");
                var savedData = RF.Find(meta.EntityType).NewList();//新的数据集
                var supplierItemList = args.Data.ToJsonObject<List<SupplierItem>>();
                Check.NotNullOrEmpty(supplierItemList, nameof(supplierItemList));//检查空
                if (null == supplierItemList || supplierItemList.Count == 0)
                {
                    throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(supplierItemList)));
                }
                foreach (var item in supplierItemList)
                {
                    var supplierItem = new SupplierItem();
                    supplierItem.ItemId = item.ItemId;//物料
                    supplierItem.SupplierId = item.SupplierId;//供应商
                    savedData.Add(supplierItem);//保存
                }
                RF.Save(savedData);

                //供应商选择物料后往物料检验特性表插入数据
                RT.Service.Resolve<ItemInspCharacteristicsController>().SaveItemInspCharacteristics(savedData);
            }
            return true;
        }
    }
}
