using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.MaterialReceptions.Services;
using SIE.LES.MaterialReceptions.ViewModels;
using SIE.LES.StockOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace SIE.LES.MaterialReceptions.Controllers
{
    /// <summary>
    /// BS端命令
    /// </summary>
    public partial class MaterialReceptionController : DomainController
    {
        /// <summary>
        /// 按明细添加
        /// </summary>
        /// <param name="labelNo"></param>
        /// <param name="scanType"></param>
        /// <param name="scanRecords"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReceptionAddViewModel> AddByDetailorOrder(string labelNo, int scanType, List<MaterialReceptionAddViewModel> scanRecords)
        {
            if (labelNo.IsNullOrEmpty())
            {
                throw new ValidationException("请输入正确条码!".L10N());
            }
            var scanList = RT.Service.Resolve<MaterialReceptionServices>().ViewModelToInfo(scanRecords);
            var paramters = new ScanParamters(labelNo, "", scanList, null, scanType);

            if (scanType == 1)
            {
                List<StockOrderSn> snList = RT.Service.Resolve<MaterialReceptionServices>().ScanByDetail(paramters).StockOrderSnList;
                if (!paramters.ErroMes.IsNullOrEmpty() && !paramters.Isvalidated)
                {
                    throw new ValidationException("{0}!".L10nFormat(paramters.ErroMes));
                }

                var materialReceptions = RT.Service.Resolve<MaterialReceptionServices>().GetMaterialReceptionByScanType(snList, scanType);
                if (!materialReceptions.Any())
                {
                    throw new ValidationException("扫描结果中不存在待接收数量大于0的单据或系统中不存在该条码!".L10N());
                }
                return materialReceptions;
            }
            else
            {
                paramters.StockOrderNo = labelNo;
                RT.Service.Resolve<MaterialReceptionServices>().ValidateChangedStockOrder(paramters);
                if (!paramters.ErroMes.IsNullOrEmpty() && !paramters.Isvalidated)
                {
                    throw new ValidationException("{0}!".L10nFormat(paramters.ErroMes));
                }
                if (!paramters.NewRecords.Any())
                {
                    throw new ValidationException("扫描结果中不存在待接收数量大于0的单据或系统中不存在该条码!".L10N());
                }
                return RT.Service.Resolve<MaterialReceptionServices>().InfoToViewModel(paramters.NewRecords);
            }
        }

        /// <summary>
        /// 按明细提交
        /// </summary>
        /// <param name="viewModelList"></param>
        public virtual void SubByDetail(List<MaterialReceptionAddViewModel> viewModelList)
        {
            var subScanList = RT.Service.Resolve<MaterialReceptionServices>().ViewModelToInfo(viewModelList);
            RT.Service.Resolve<MaterialReceptionServices>().Submit(subScanList);
        }


        public virtual void OneKeySubmit(EntityList<MaterialReception> materialReceptions)
        {

            var viewModelList = MaterialReceptionToInfo(materialReceptions);


            RT.Service.Resolve<MaterialReceptionServices>().Submit(viewModelList);
        }

        public virtual List<MaterialReceptionInfo> MaterialReceptionToInfo(EntityList<MaterialReception> materialReceptions)
        {
            if (materialReceptions.Count == 0)
            {
                return new List<MaterialReceptionInfo>();
            }
            var scanList = new List<MaterialReceptionInfo>();
            materialReceptions.ForEach(scanRecord =>
            {
                var info = new MaterialReceptionInfo
                {
                    Id = scanRecord.Id,
                    LineNo = scanRecord.LineNo,
                    Label = scanRecord.LabelNo,
                    BatchNo = scanRecord.LotNo,
                    ItemId = scanRecord.ItemId,
                    //ItemCode = scanRecord.ItemCode,
                    ItemName = scanRecord.ItemName,
                    ItemExtProp = scanRecord.ItemExtProp,
                    //ItemExtPropName = scanRecord.ItemExtPropName,
                    DetailState = scanRecord.State,
                    FactoryId = scanRecord.FactoryId,
                    WorkOrderId = scanRecord.WorkOrderId,
                    BillId = scanRecord.StockOrderId,
                    BillDtlId = scanRecord.StockOrderDetailId,
                    Qty = (scanRecord.ShipQty - scanRecord.Qty),
                    ShipQty = scanRecord.ShipQty,
                    ResourceId = scanRecord.ResourceId,
                    ReceiveWarehouseId = scanRecord.WarehouseId,
                    //ReceiveWarehouseName = scanRecord.WarehouseName,
                    ReceiveStorageLocationId = scanRecord.StorageLocationId,
                    //ReceiveStorageLocationName = scanRecord.StorageLocationName,
                    SoNo = scanRecord.SoNo,
                    SoLineNo = scanRecord.SoLineNo,
                    //StockState = scanRecord.StockOrderState,
                    IsManualRec = scanRecord.IsManualRec,

                    ReceiveBy = RT.IdentityId,
                    ReceiveTime = DateTime.Now
                };
                if (info.Qty < 0)
                {
                    throw new ValidationException("接收数量不能小于0！".L10N());
                }
                scanList.Add(info);
            });
            return scanList;
        }
    }
}
