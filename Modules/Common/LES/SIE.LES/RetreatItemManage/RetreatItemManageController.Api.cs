using SIE.Api;
using SIE.Domain.Validation;
using SIE.LES.RetreatItemManage.APIModel;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.Domain;
using SIE.Resources.WipResources;
using SIE.EventMessages.MES.WorkOrders;
using SIE.LES.Interfaces;
using SIE.LES.Interfaces.Datas;
using SIE.Core.Labels;
using ItemLabel = SIE.Packages.ItemLabels.ItemLabel;
using Castle.MicroKernel;
using System.Reflection.Emit;
using SIE.Common.Catalogs;
using SIE.LES.RetreatItemManage.MaterialReturns;
using Castle.Components.DictionaryAdapter;
using SIE.Core.ApiModels;
using SIE.Items;
using SIE.Resources.Employees;
using SIE.Warehouses;

namespace SIE.LES.RetreatItemManage
{
    /// <summary>
    ///退料管理API
    /// </summary>
    public class RetreatItemManageController : DomainController
    {
        /// <summary>
        /// 获取待退料物料标签信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [ApiService("获取待退料物料标签信息")]
        [return: ApiReturn("待退料物料标签信息 ScanItemLabelInfo")]
        public virtual List<ScanItemLabelInfo> GetScanItemLabelInfo([ApiParameter("物料标签信息")] string barcode)
        {
            if (barcode.IsNullOrEmpty())
            {
                throw new ValidationException("请扫描物料标签".L10N());
            }
            var itemLabelList = Query<SIE.Packages.ItemLabels.ItemLabel>().Where(n => n.Label == barcode && (n.Qty > 0 || n.NgQty > 0)
            &&n.SourceType!= LabelSource.Import
            ).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (itemLabelList.Count <= 0)
            {
                throw new ValidationException("系统中不存在扫描标签或标签来源类型为外部录入".L10N());
            }
            //if (itemLabelList.Any(p => p.SourceType != LabelSource.Receive))
            //{
            //    throw new ValidationException("存在物料标签条码信息类型不为【物料接收】，请更换标签".L10N());
            //}
            var materialReturns = RT.Service.Resolve<MaterialReturnController>().GetSearch(barcode);
            List<ScanItemLabelInfo> scanItemLabelInfoList = new List<ScanItemLabelInfo>();
            materialReturns.ForEach(materialReturn =>
            {
                var scanItemLabelInfo = new ScanItemLabelInfo
                {
                    No = materialReturn.NO,
                    retireType = materialReturn.ReturnType,
                    retireTypeLabel = materialReturn.ReturnType.ToLabel(),
                    States = materialReturn.ReturnState,
                    BatchNo = materialReturn.BatchNO,
                    ReWorkOrderId = materialReturn.WorkOrderId,
                    ReWorkOrderNo = materialReturn.WorkOrder != null ? materialReturn.WorkOrder.No : string.Empty,
                    WarehouseId = materialReturn.ReturnWarehouseId,
                    WarehouseName = materialReturn.ReturnWarehouse != null ? materialReturn.ReturnWarehouse.Name : string.Empty,
                    StorageLocationId = materialReturn.ReturnWarehouseLocationId,
                    StorageLocationName = materialReturn.ReturnWarehouseLocation != null ? materialReturn.ReturnWarehouseLocation.Name : string.Empty,
                    NowQty = materialReturn.AlreadyQty,
                    BadQty = materialReturn.BadQty,
                    ItemId = materialReturn.ItemId,
                    ItemCode = materialReturn.ItemCode,
                    ItemName = materialReturn.ItemName,
                    LabelId = materialReturn.LabelId,
                    Label = materialReturn.Label,
                    InputQty = materialReturn.Qty,
                    ReturnReason = materialReturn.ReturnReason,
                    ReturnReasonDesc = materialReturn.ReturnReasonDesc,
                    FactoryId = materialReturn.FactoryId,
                    FactoryName = materialReturn.Factory.Name,
                    WipId = materialReturn.WipResourceId,
                    WipName = materialReturn.WipResource != null? materialReturn.WipResource.Name:string.Empty,
                    IsSerial = materialReturn.IsSerial ?? false,
                };
                var res= RT.Service.Resolve<ItemController>().GetItemUnitPrecisions(materialReturn.ItemId);
                if (res != null)
                {
                    scanItemLabelInfo.unitPrecsion = res.unitPrecsion;
                    scanItemLabelInfo.carry = res.carry;
                }

                scanItemLabelInfoList.Add(scanItemLabelInfo);
            });
            return scanItemLabelInfoList;

        }

        /// <summary>
        /// 获取退料原因信息
        /// </summary>
        [ApiService("获取退料原因信息")]
        [return: ApiReturn("退料原因信息")]
        public virtual List<BaseDataInfo> ChooseReturnReason()
        {
            var catalist = RT.Service.Resolve<CatalogController>().GetCatalogList(MaterialReturn.ReasonMaterialReturn);
            if (catalist == null)
            {
                throw new ValidationException("退料原因快码异常！".L10N());
            }
            List<BaseDataInfo> returnReasonList = new List<BaseDataInfo>();
            catalist.ForEach(cata =>
            {
                returnReasonList.Add(new BaseDataInfo
                {
                    Id = cata.Id,
                    Code = cata.Code,
                    Name = cata.Name,
                });
            });
            return returnReasonList;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="scanItemLabelInfo"></param>
        /// <returns></returns>
        [ApiService("提交")]
        [return: ApiReturn("")]
        public virtual void SubmitItemLabelInfo([ApiParameter("扫描列表")] List<ScanItemLabelInfo> scanItemLabelInfo)
        {
            var isMust = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialMustReturnReason();
            var emptyReason = false;
            if (!scanItemLabelInfo.Any())
            {
                throw new ValidationException("退料数量不能为0！".L10N());
            }
            if (scanItemLabelInfo.Any(p => p.ReturnReason.IsNullOrEmpty()))
            {
                emptyReason = true;
            }
            if (isMust && emptyReason)
            {
                throw new ValidationException("存在退料信息退料原因未填写！".L10N());
            }
            if (scanItemLabelInfo.Any(p => p.InputQty < 0))
            {
                throw new ValidationException("退料数量不能小于0！".L10N());
            }
            if (scanItemLabelInfo.Any(p => p.InputQty > p.NowQty))
            {
                throw new ValidationException("退料数量不能大于现有数量！".L10N());
            }

            EntityList<MaterialReturn> materialReturns = new EntityList<MaterialReturn>();
            scanItemLabelInfo.ForEach(info =>
            {
                var entityCur = CreateMaterialReturn(info);
                if (entityCur.Id == 0)
                {
                    entityCur.PersistenceStatus = PersistenceStatus.New;
                }
                materialReturns.Add(entityCur);
            });
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                RT.Service.Resolve<MaterialReturnController>().ApiHandleSubmit(materialReturns);
                CreateMaterialWithdrawals(scanItemLabelInfo);
                tran.Complete();
            }
        }

        /// <summary>
        /// API转化为MaterialReturn实体
        /// </summary>
        /// <param name="scanItemLabelInfo"></param>
        /// <returns></returns>
        private MaterialReturn CreateMaterialReturn(ScanItemLabelInfo scanItemLabelInfo)
        {
            var materialReturn = new MaterialReturn
            {
                NO = scanItemLabelInfo.No,
                ReturnType = scanItemLabelInfo.retireType,
                ItemId = scanItemLabelInfo.ItemId,
                ItemCode = scanItemLabelInfo.ItemCode,
                ItemName = scanItemLabelInfo.ItemName,
                LabelId = scanItemLabelInfo.LabelId,
                Label = scanItemLabelInfo.Label,
                BatchNO = scanItemLabelInfo.BatchNo,
                Qty = scanItemLabelInfo.InputQty,
                BadQty = (int)scanItemLabelInfo.BadQty,
                AlreadyQty = (int)scanItemLabelInfo.NowQty,
                ReturnReason = scanItemLabelInfo.ReturnReason,
                ReturnReasonDesc = scanItemLabelInfo.ReturnReasonDesc,
                WorkOrderId = scanItemLabelInfo.ReWorkOrderId,
                FactoryId = scanItemLabelInfo.FactoryId,
                WipResourceId = scanItemLabelInfo.WipId,
                ReturnWarehouseId = scanItemLabelInfo.WarehouseId,
                ReturnWarehouseLocationId = scanItemLabelInfo.StorageLocationId,
                IsSerial = scanItemLabelInfo.IsSerial,
            };
            return materialReturn;
        }

        /// <summary>
        /// 一键退料
        /// </summary>
        /// <param name="scanItemLabelInfos"></param>
        [ApiService("一键退料")]
        [return: ApiReturn("")]
        public virtual void SubmitWithdrawalableItemLabelInfos(List<ScanItemLabelInfo> scanItemLabelInfos)
        {
            if (!scanItemLabelInfos.Any())
            {
                throw new ValidationException("请扫描物料标签".L10N());
            }
            var itemLabelIds = scanItemLabelInfos.Select(m => m.Id).ToList();

            var itemLabels = Query<ItemLabel>().Where(m => itemLabelIds.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //校验物料标签
            var returnSnDatas = new List<ReturnSnData>();
            itemLabels.ForEach(itemLabel =>
            {
                if (itemLabel.SourceType != LabelSource.Receive)
                {
                    throw new ValidationException("物料标签【{0}】类型不为【物料接收】，请更换标签".L10nFormat(itemLabel.Label));
                }

                if (itemLabel.Qty <= 0)
                {
                    throw new ValidationException("扫描的标签【{0}】剩余数量小于等于0，请更换标签".L10nFormat(itemLabel.Label));
                }
                var postItemData = new ReturnSnData()
                {
                    Qty = itemLabel.Qty,
                    Sn = itemLabel.Label,
                    SourceStorageLocationId = itemLabel.StorageLocationId,
                    SourceWarehouseCode = itemLabel.WarehouseCode,
                    SourceWarehouseId = itemLabel.WarehouseId,
                    //IsFail = !itemLabel.IsQualified,
                    WoNo = itemLabel.WorkOrderNo,
                };
                if (itemLabel.WorkOrderId.HasValue)//工单有值获取产线
                {
                    var resource = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(itemLabel.WorkOrderId.Value);
                    if (resource != null)
                    {
                        postItemData.WorkShopId = resource.WorkShopId;
                    }
                }

                returnSnDatas.Add(postItemData);
                itemLabel.Qty = 0;
                itemLabel.WarehouseId = null;
                itemLabel.StorageLocationId = null;
            });

            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                //todo 调用wms 退料接口 
                RT.Service.Resolve<StockOrderController>().ReturnSnUpdate(returnSnDatas, "");

                CreateMaterialWithdrawals(scanItemLabelInfos);
                RF.Save(itemLabels);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建退料记录
        /// </summary>
        /// <param name="withdrawalableItemLabelInfos"></param>
        private void CreateMaterialWithdrawals(List<ScanItemLabelInfo> withdrawalableItemLabelInfos)
        {
            var nowDate = RF.Find<MaterialWithdrawalRecord>().GetDbTime();
            var wipResourceIds = withdrawalableItemLabelInfos.Where(p => p.ResourceId != null).Select(p => (double)p.ResourceId).ToList();
            var wipResources = RT.Service.Resolve<WipResourceController>().GetResourceTypeByResourceIds(wipResourceIds);
            EntityList<MaterialWithdrawalRecord> callMaterialWithdrawals = new EntityList<MaterialWithdrawalRecord>();
            foreach (var curWithdrawalable in withdrawalableItemLabelInfos)
            {
                var curCallMaterialWithdrawal = new MaterialWithdrawalRecord();
                curCallMaterialWithdrawal.ItemLabel = curWithdrawalable.Label;
                curCallMaterialWithdrawal.RemainQty = curWithdrawalable.InputQty;
                curCallMaterialWithdrawal.WithdrawalQty = curWithdrawalable.InputQty;
                curCallMaterialWithdrawal.BatchNo = curWithdrawalable.BatchNo;
                curCallMaterialWithdrawal.WithdrawalDate = nowDate;
                curCallMaterialWithdrawal.WithdrawalById = RT.IdentityId;
                curCallMaterialWithdrawal.ItemId = curWithdrawalable.ItemId;
                if (curWithdrawalable.ResourceId != 0)
                {
                    curCallMaterialWithdrawal.ResourceId = curWithdrawalable.ResourceId;
                }
                if (curWithdrawalable.WorkOrderId != 0)
                {
                    curCallMaterialWithdrawal.WorkOrderId = curWithdrawalable.WorkOrderId;
                }
                curCallMaterialWithdrawal.WorkShopId = wipResources.FirstOrDefault(p => p.Id == curWithdrawalable.ResourceId)?.WorkShopId;
                callMaterialWithdrawals.Add(curCallMaterialWithdrawal);
            }

            RF.Save(callMaterialWithdrawals);
        }

        /// <summary>
        /// 获取已退料物料标签信息
        /// </summary>
        /// <param name="label"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [ApiService("获取已退料物料标签信息")]
        [return: ApiReturn("已退料物料标签信息.  参数类型: List<ScanItemLabelInfo>")]
        public virtual List<ScanItemLabelInfo> GetWithdrawaledItemLabelInfos([ApiParameter("标签号")] string label = null, [ApiParameter("退料日期")] string date = null)
        {
            if (string.IsNullOrEmpty(label) && string.IsNullOrEmpty(date))
                throw new ValidationException("标签号和退料日期不能同时为空".L10nFormat());

            DateTime? dataTime = null;
            if (!string.IsNullOrEmpty(date))
                dataTime = DateTime.Parse(date);
            var withdrawaledlInfos = GetWithdrawaledInfos(label, dataTime);
            if (withdrawaledlInfos == null || withdrawaledlInfos.Count == 0)
                throw new ValidationException("物料标签: {0} 无已退料信息!".L10nFormat(label));
            return withdrawaledlInfos;
        }

        /// <summary>
        /// 获取退料信息
        /// </summary>
        /// <param name="label"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual List<ScanItemLabelInfo> GetWithdrawaledInfos(string label = null, DateTime? date = null)
        {
            List<ScanItemLabelInfo> withdrawaledlInfos = null;
            var materialWithdrawals = GetMaterialWithdrawals(label, date, null);
            if (materialWithdrawals != null && materialWithdrawals.Count > 0)
            {
                withdrawaledlInfos = CreateWithdrawaledInfos(materialWithdrawals);
            }

            return withdrawaledlInfos;
        }

        private List<ScanItemLabelInfo> CreateWithdrawaledInfos(EntityList<MaterialReturn> materialReturns)
        {
            List<ScanItemLabelInfo> withdrawaledItemLabelInfos = new List<ScanItemLabelInfo>();
            foreach (var materialReturn in materialReturns)
            {
                var curWithdrawaledInfo = new ScanItemLabelInfo {
                    No = materialReturn.NO,
                    retireType = materialReturn.ReturnType,
                    retireTypeLabel = materialReturn.ReturnType.ToLabel(),
                    States = materialReturn.ReturnState,
                    BatchNo = materialReturn.BatchNO,
                    ReWorkOrderId = materialReturn.WorkOrderId,
                    ReWorkOrderNo = materialReturn.WorkOrder != null ? materialReturn.WorkOrder.No : string.Empty,
                    WarehouseId = materialReturn.ReturnWarehouseId,
                    WarehouseName = materialReturn.ReturnWarehouse != null ? materialReturn.ReturnWarehouse.Name : string.Empty,
                    StorageLocationId = materialReturn.ReturnWarehouseLocationId,
                    StorageLocationName = materialReturn.ReturnWarehouseLocation != null ? materialReturn.ReturnWarehouseLocation.Name : string.Empty,
                    NowQty = materialReturn.Qty,
                    ItemId = materialReturn.ItemId,
                    ItemCode = materialReturn.ItemCode,
                    ItemName = materialReturn.ItemName,
                    LabelId = materialReturn.LabelId,
                    Label = materialReturn.Label,
                    InputQty = 0,
                    ReturnReason = materialReturn.ReturnReason,
                    ReturnReasonDesc = materialReturn.ReturnReasonDesc,
                    FactoryId = materialReturn.FactoryId,
                    FactoryName = materialReturn.Factory.Name,
                    WipId = materialReturn.WipResourceId,
                    WipName = materialReturn.WipResource != null ? materialReturn.WipResource.Name : string.Empty,
                    SubName = materialReturn.Employee != null ? materialReturn.Employee.Name : string.Empty,
                    SubTime = materialReturn.SubmitDate.Value.ToString(),
                };
                withdrawaledItemLabelInfos.Add(curWithdrawaledInfo);
            }

            return withdrawaledItemLabelInfos;
        }

        /// <summary>
        /// 获取物料退料
        /// </summary>
        /// <param name="label"></param>
        /// <param name="curDateTime"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturn> GetMaterialWithdrawals(string label = null, DateTime? curDateTime = null, double? resourceId = null)
        {
            var querys = Query<MaterialReturn>();
            if (!string.IsNullOrEmpty(label))
            {
                querys.Where(x => x.Label == label);
            }
            if (curDateTime != null)
            {
                var curDate = ((DateTime)curDateTime).Date;
                querys.Where(x => x.SubmitDate >= curDate && x.SubmitDate < curDate.AddDays(1));
            }

            if (resourceId != null)
            {
                querys.Where(x => x.WipResourceId == resourceId);
            }
            return querys.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
