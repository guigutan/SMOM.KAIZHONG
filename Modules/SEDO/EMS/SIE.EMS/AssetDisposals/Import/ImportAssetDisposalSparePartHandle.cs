using SIE.Common.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.AssetDisposals.Import
{
    /// <summary>
    /// 导入备件回收帮助类
    /// </summary>
    public class ImportAssetDisposalSparePartHandle
    {

        /// <summary>
        /// 资产处置List
        /// </summary>
        private EntityList<AssetDisposal> AssetDisposalList { get; set; }

        /// <summary>
        /// 备件List
        /// </summary>
        private EntityList<SparePart> SparePartList { get; set; }

        /// <summary>
        /// 序列号明细List
        /// </summary>
        private EntityList<StoreSummaryDetail> StoreSummaryDetailList { get; set; }

        /// <summary>
        /// 批次明细List
        /// </summary>
        private EntityList<StoreSummaryLot> StoreSummaryList { get; set; }

        /// <summary>
        /// All仓库List
        /// </summary>
        private EntityList<Warehouse> AllWarehouseList { get; set; }
        /// <summary>
        /// 零成本仓List
        /// </summary>
        private EntityList<Warehouse> WarehouseList { get; set; }


        /// <summary>
        /// 导入的所有批次号
        /// </summary>
        private List<string> NewLotNoList { get; set; }

        /// <summary>
        /// 已保存的备件回收表中所有批次号
        /// </summary>
        private List<string> OldLotNoList { get; set; }

        /// <summary>
        /// 导入的所有序列号
        /// </summary>
        private List<string> NewSnNoList { get; set; }

        /// <summary>
        /// 已保存的备件回收表中所有序列号
        /// </summary>
        private List<string> OldSnNoList { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        List<ImportMessageResult> MessageList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImportAssetDisposalSparePartHandle()
        {
            AssetDisposalList = new EntityList<AssetDisposal>();
            SparePartList = new EntityList<SparePart>();
            MessageList = new List<ImportMessageResult>();
            StoreSummaryDetailList = new EntityList<StoreSummaryDetail>();
            StoreSummaryList = new EntityList<StoreSummaryLot>();
            AllWarehouseList = new EntityList<Warehouse>();
            WarehouseList = new EntityList<Warehouse>();
            NewLotNoList = new List<string>();
            OldLotNoList = new List<string>();
            NewSnNoList = new List<string>();
            OldSnNoList = new List<string>();
        }


        /// <summary>
        /// 导入备件回收主入口
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<ImportMessageResult> ImportEquipmentCard(IList<RowData> data)
        {
            //加载数据
            LoadData(data);
            List<ApprovalStatus> status = new List<ApprovalStatus>() { ApprovalStatus.Draft, ApprovalStatus.Reject };
            //加载数据
            foreach (var row in data)
            {
                try
                {
                    var part = row.Entity as AssetDisposalSparePart;
                    //基本验证
                    DataCheck(part, status);
                    using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                    {
                        RF.Save(part);
                        trans.Complete();
                    }
                    MessageList.Add(new ImportMessageResult { RowNum = row.RowIndex + 1, MsgType = ImportMessageType.SaveSucess, Message = "保存成功！".L10N() });
                }
                catch (Exception exc)
                {
                    MessageList.Add(new ImportMessageResult { RowNum = row.RowIndex + 1, MsgType = ImportMessageType.SaveFail, Message = exc.GetBaseException().Message });
                }
            }
            return MessageList;
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="part"></param>
        /// <param name="status"></param>
        protected void DataCheck(AssetDisposalSparePart part, List<ApprovalStatus> status)
        {
            //备件回收单上的资产处置单
            AssetDisposal asset = AssetDisposalList.FirstOrDefault(p => p.Id == part.AssetDisposalId);
            //备件回收单上的备件
            SparePart sparePart = SparePartList.FirstOrDefault(p => p.Id == part.SparePartId);
            //备件回收单上的序列号明细数据为空则不存在
            StoreSummaryDetail storeSummaryDetail = StoreSummaryDetailList.FirstOrDefault(p => p.OrderNumberCode == part.Sn);
            //备件回收单上的序列号明细数据为空则不存在
            StoreSummaryLot storeSummary = StoreSummaryList.FirstOrDefault(p => p.BatchNumber == part.LotNo);
            //备件回收单上的仓库为空则不存在
            Warehouse warehouse = AllWarehouseList.FirstOrDefault(p => p.Id == part.WarehouseId);
            Warehouse zorewarehouse = WarehouseList.FirstOrDefault(p => p.Id == part.WarehouseId);
            if (warehouse == null)
            {
                throw new ValidationException("入库仓库不存在！".L10N());
            }
            if (zorewarehouse == null)
            {
                throw new ValidationException("入库仓库【{0}】非零成本仓！".L10nFormat(warehouse.Name));
            }
            if (asset == null)
            {
                throw new ValidationException("资产处置单不存在".L10N());
            }
            if (sparePart == null)
            {
                throw new ValidationException("备件不存在".L10N());
            }

            //校验处置单的审核状态是否为【待提交、驳回】否则报错：处置单XXX审核状态不为【待提交、驳回】，不能导入
            if (!status.Contains(asset.ApprovalStatus))
            {
                throw new ValidationException("导入资产处置单的审核状态须为【待提交、驳回】".L10N());
            }

            //备件的管控方式为【批次】
            if (sparePart.ControlMethod == ControlMethod.Batch)
            {
                if (!part.LotNo.IsNotEmpty())
                {
                    throw new ValidationException("备件的管控方式为【批次】时,批次必输".L10N());
                }
                //校验批次在导入的数据中是否重复、在已保存的备件回收表中是否重复、在备件批次表中是否重复，是则报错：批次号XXX已存在
                if (NewLotNoList.Count(p => p == part.LotNo) > 1)
                {
                    throw new ValidationException("批次【{0}】在导入的数据中重复".L10nFormat(part.LotNo));
                }
                if (OldLotNoList.Any(p => p == part.LotNo))
                {
                    throw new ValidationException("批次【{0}】在已保存的备件回收表中已存在".L10nFormat(part.LotNo));
                }
                if (storeSummary != null)
                {
                    throw new ValidationException("批次【{0}】在备件批次表中已存在".L10nFormat(part.LotNo));
                }
            }
            //备件的管控方式为【序列号】
            if (sparePart.ControlMethod == ControlMethod.Sn)
            {
                //回收数量 序列号管控时，忽略该字段，直接取值为1
                part.Qty = 1;

                if (!part.Sn.IsNotEmpty())
                {
                    throw new ValidationException("备件的管控方式为【序列号】时,序列号必输".L10N());
                }

                if (NewSnNoList.Count(p => p == part.Sn) > 1)
                {
                    throw new ValidationException("序列号【{0}】在导入的数据中重复".L10nFormat(part.Sn));
                }

                if (asset.AssetDisposalSparePartList.FirstOrDefault(p => p.Sn == part.Sn) != null)
                {
                    throw new ValidationException("资产处置单【{0}】备件回收中序列号【{1}】重复".L10nFormat(asset.No, part.Sn));
                }

                //备件回收单上的序列号明细数据存在
                if (storeSummaryDetail != null)
                {
                    if (storeSummaryDetail.StoreStatus != OrdNumStoreStatus.Out)
                    {
                        throw new ValidationException("序列号【{0}】库存状态【{1}】不为【出库】,不能回收!".L10nFormat(part.Sn, storeSummaryDetail.StoreStatus.ToLabel()));
                    }
                    if (storeSummaryDetail.SparePartId != part.SparePartId)
                    {
                        throw new ValidationException("序列号【{0}】为备件编码【{1}】的序列号，与导入数据备件【{2}】的序列号【{3}】不符合!".L10nFormat(storeSummaryDetail.OrderNumberCode, storeSummaryDetail.SparePartCode, sparePart.SparePartCode, part.Sn));
                    }
                }
                else
                {
                    if (OldSnNoList.Any(p => p == part.Sn))
                    {
                        throw new ValidationException("序列号【{0}】在已保存的备件回收表中重复".L10nFormat(part.Sn));
                    }
                }
            }

            if (part.Qty <= 0)
            {
                throw new ValidationException("备件编码【{0}】的回收数量须大于0！".L10nFormat(sparePart.SparePartCode));
            }
            if (part.QualityStatus == null)
            {
                throw new ValidationException("备件编码【{0}】的质量状态不能为空！".L10nFormat(sparePart.SparePartCode));
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="data"></param>
        private void LoadData(IList<RowData> data)
        {
            List<double> assetDisposalIds = new List<double>();
            List<double> sparePartIds = new List<double>();
            List<double> warehouseIds = new List<double>();
            List<string> onewLotNoList = new List<string>();
            List<string> onewSnNoList = new List<string>();
            foreach (var row in data)
            {
                var assetDisposalSparePart = row.Entity as AssetDisposalSparePart;
                assetDisposalIds.Add(assetDisposalSparePart.AssetDisposalId);
                sparePartIds.Add(assetDisposalSparePart.SparePartId);
                onewLotNoList.Add(assetDisposalSparePart.LotNo);
                onewSnNoList.Add(assetDisposalSparePart.Sn);
                warehouseIds.Add(assetDisposalSparePart.WarehouseId);
            }
            //获取导入资产回收的资产信息
            AssetDisposalList = RT.Service.Resolve<AssetDisposalController>().GetAssetDisposalListByIds(assetDisposalIds);
            //获取导入资产回收的备件信息
            SparePartList = RT.Service.Resolve<SparePartController>().GetSpareParts(sparePartIds);
            //导入的所有批次号
            NewLotNoList.AddRange(onewLotNoList);
            NewSnNoList.AddRange(onewSnNoList);
            //批次
            StoreSummaryList = RT.Service.Resolve<StoreSummaryController>().GetStoreSummaryLots(onewLotNoList.Distinct().ToList());
            //序列号
            StoreSummaryDetailList = RT.Service.Resolve<StoreSummaryController>().GetStoreSummaryDetails(onewSnNoList.Distinct().ToList());
            //获取已保存的备件回收表中的批次号
            OldLotNoList = RT.Service.Resolve<AssetDisposalController>().GteAssetDisposalSparePartAllLotNo();
            OldSnNoList = RT.Service.Resolve<AssetDisposalController>().GteAssetDisposalSparePartAllSn();

            //获取所有仓库
            AllWarehouseList = RT.Service.Resolve<WarehouseController>().GetWarehouses(warehouseIds.Distinct().ToList());
            //获取零成本仓
            WarehouseList = RT.Service.Resolve<SparePartController>().GetZereCostWarehouses(null, null);
        }
    }
}
