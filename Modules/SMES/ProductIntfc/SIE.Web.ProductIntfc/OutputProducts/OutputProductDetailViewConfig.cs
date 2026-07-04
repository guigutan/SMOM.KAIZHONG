using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.ProductIntfc.OutputProducts;
using SIE.Web.Items._Extentions_;
using SIE.Web.ProductIntfc.OutputProducts.Commands;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 入库单
    /// </summary>
    public class OutputProductDetailViewConfig : WebViewConfig<OutputProductDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.AddBehavior("SIE.Web.ProductIntfc.OutputProducts.OutputProductBehavior");
            View.ClearCommands(true).UseCommands(WebCommandNames.ExportXls);
            View.UseCommands("SIE.Web.ProductIntfc.OutputProducts.Commands.AddOutPutProductCommand",
                "SIE.Web.ProductIntfc.OutputProducts.Commands.EditOutPutProductCommand", "SIE.Web.ProductIntfc.OutputProducts.Commands.DeleteOutPutProductCommand", WebCommandNames.Save,typeof(ToStorageCommand).FullName);
            View.UseChildrenAsHorizontal(true);
           
            using (View.OrderProperties())
            {
                View.Property(p => p.NO).Readonly().ShowInList(150);
                
                View.Property(p => p.WorkOrderOutputProductId).UseDataSource((entity, propa, Key) => {
                    var outputProductDetail = entity as OutputProductDetail;
                    if (outputProductDetail.StorageWorkOrderId != 0)
                    {
                        return RT.Service.Resolve<OutputProductController>().GetOutputProduceList(outputProductDetail.StorageWorkOrderId);
                    }
                    return new EntityList<WorkOrderOutputProduct>();

                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.WorkOrderOutputProduct.ItemName));
                    dic.Add(nameof(e.InStorageQty), nameof(e.WorkOrderOutputProduct.Qty));
                    dic.Add(nameof(e.ItemExtProp), nameof(e.WorkOrderOutputProduct.ItemExtProp));
                    dic.Add(nameof(e.ItemExtPropName), nameof(e.WorkOrderOutputProduct.ItemExtPropName));
                    dic.Add(nameof(e.OutPutType), nameof(e.WorkOrderOutputProduct.OutputListType));
                    dic.Add(nameof(e.ItemId), nameof(e.WorkOrderOutputProduct.ItemId));
                    dic.Add(nameof(e.IsBatchCtrl), nameof(e.WorkOrderOutputProduct.IsBatchCtrl));
                    
                    m.DicLinkField = dic;
                    m.BindDisplayField = nameof(OutputProductDetail.ItemCode);
                    m.DisplayField = nameof(e.WorkOrderOutputProduct.ItemCode);
                }).Readonly(p=>p.InStorageState== InStorageState.InStorage);
                View.Property(p => p.OutPutType).Readonly().UseEnumEditor().ShowInList(150);
                View.Property(p => p.ItemName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ItemExtPropName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InStorageQty).UseItemUnitEditor().Show(ShowInWhere.All).Readonly(p => p.InStorageState == InStorageState.InStorage||p.WorkOrderOutputProductId==0||p.WorkOrderOutputProductId==null);
                View.Property(p => p.Barcode).Show(ShowInWhere.All).Readonly(p => p.InStorageState == InStorageState.InStorage);
                View.Property(p => p.Lot).Show(ShowInWhere.All).Readonly(p => p.InStorageState == InStorageState.InStorage||p.IsBatchCtrl==false|| p.IsBatchCtrl ==null);
                View.Property(p => p.Warehouse).Show(ShowInWhere.All).HasLabel("收货仓库").Readonly(p => p.InStorageState == InStorageState.InStorage);
                View.Property(p => p.Operator).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.OperatorTime).Readonly().Show(ShowInWhere.All);                
                View.Property(p => p.InStorageState).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.AsnNo).Readonly().ShowInList(150).HasLabel("ASN单号");
                View.Property(p => p.ReceiveState).Readonly().Show(ShowInWhere.All).HasLabel("接收情况");
                View.Property(p => p.ReceiveDate).Readonly().ShowInList(150);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
