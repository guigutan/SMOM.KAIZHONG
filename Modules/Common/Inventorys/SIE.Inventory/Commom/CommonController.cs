using SIE.Api;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Items;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class CommonController : DomainController
    {
        /// <summary>
        /// 获取物料扩展属性
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>物料扩展属性</returns>
        [ApiService("获取扩展属性")]
        [return: ApiReturn("返回扩展属性列表。参数类型：List<ExtensionDataData>")]
        public virtual List<ExtensionData> GetOtherItemExtPropsSelList([ApiParameter("物料id")] double itemId)
        {
            List<ExtensionData> list = new List<ExtensionData>();
            var item = RF.GetById<Item>(itemId);
            if (item.EnableExtendProperty)
            {
                var itemPropValueList = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId);
                var definitionList = itemPropValueList.GroupBy(p => new { Name = p.DefinitionName, Id = p.DefinitionId }).Distinct().ToList();
                definitionList.OrderBy(p=>p.Key.Id).ForEach(p =>
                {
                    var data = new ExtensionData();
                    data.name = p.Key.Name;
                    data.id = p.Key.Id;
                    data.valList = p.Select(q => q.Value).ToList();
                    list.Add(data);
                });
            }
            return list;
        }

        /// <summary>
        /// 根据类型获取订单类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual OrderType GetBillOrderType(string type)
        {
            OrderType orderType = OrderType.PurchaseIn;
            switch (type)
            {
                case "SIE.WMS.Receipt.PurAsn":
                case "SIE.WMS.Receipt.Asn":
                    orderType = OrderType.PurchaseIn;
                    break;
                case "SIE.WMS.Receipt.FinAsn":
                    orderType = OrderType.Finished;
                    break;
                case "SIE.WMS.Receipt.PartAsn":
                    orderType = OrderType.PartedIn;
                    break;
                case "SIE.WMS.Receipt.RetAsn":
                    orderType = OrderType.MaterialReturn;
                    break;
                case "SIE.WMS.Receipt.OutRetAsn":
                    orderType = OrderType.OutMaterialReturn;
                    break;
                case "SIE.WMS.Receipt.SaleRetAsn":
                    orderType = OrderType.SaleReturn;
                    break;
                case "SIE.WMS.Receipt.CustAsn":
                    orderType = OrderType.CustomerIn;
                    break;
                case "SIE.WMS.Receipt.VmiAsn":
                    orderType = OrderType.VMIIN;
                    break;
                case "SIE.WMS.Receipt.OtherAsn":
                    orderType = OrderType.OtherIn;
                    break;
                case "SIE.WMS.Shipment.SaleOutSO":
                case "SIE.WMS.Shipment.ShippingOrder":
                    orderType = OrderType.SaleOut;
                    break;
                case "SIE.WMS.Shipment.WorkFeedSO":
                    orderType = OrderType.WorkFeed;
                    break;
                case "SIE.WMS.Shipment.WoFinishReturn":
                    orderType = OrderType.WoFinishReturn;
                    break;
               
                case "SIE.WMS.Shipment.OutWorkFeedSO":
                    orderType = OrderType.OutWorkFeed;
                    break;
                case "SIE.WMS.Shipment.OutWorkFeedUseSO":
                    orderType = OrderType.OutWorkFeedUse;
                    break;
                case "SIE.WMS.Shipment.OutAllotReturnSO":
                    orderType = OrderType.OutAllotReturn;
                    break;
                case "SIE.WMS.Shipment.OtherOutSO":
                    orderType = OrderType.OtherOut;
                    break;
                case "SIE.WMS.Shipment.WhTransferOutSO":
                    orderType = OrderType.WhTransferOut;
                    break;
                case "SIE.WMS.Shipment.SupplierReturnSO":
                    orderType = OrderType.SupplierReturn;
                    break;
                case "SIE.WMS.Receipt.WhTranferInAsn":
                    orderType = OrderType.WhTransferIn;
                    break;
                case "SIE.WMS.Receipt.WhCrossInAsn":
                    orderType = OrderType.CrossOrgTransferIn;
                    break;
                default:
                    break;
            }

            return orderType;
        }
    }
}
