using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.EMS.Purchases.SparePartReceives.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Configs;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收扫描控制器
    /// </summary>
    public partial class SparePartReceiveScanController : DomainController
    {
        /// <summary>
        /// 获取备件接收扫描信息
        /// </summary>
        /// <param name="receiveId">备件接收id</param>
        /// <returns>备件接收扫描信息</returns>
        public virtual Tuple<ReceiveScanViewModel, EntityList<SparePartReceiveDetail>, EntityList<SparePartReceiveLot>, EntityList<SparePartReceiveSn>> GetReceiveScanInfo(double receiveId)
        {
            var receive = RT.Service.Resolve<SparePartReceiveController>().GetSparePartReceiveById(receiveId);
            var details = RT.Service.Resolve<SparePartReceiveController>().GetDetailsByReceiveId(receiveId, null);
            var lots = RT.Service.Resolve<SparePartReceiveController>().GetReceiveLotInfo(receiveId, null);
            var sns = RT.Service.Resolve<SparePartReceiveController>().GetReceiveSnInfo(receiveId, null);
            var model = new ReceiveScanViewModel();
            model.ReceiveNo = receive.ReceiveNo;
            model.FactoryName = receive.FactoryName;
            model.DepartmentName = receive.DepartmentName;
            model.ReceiveType = receive.ReceiveType;
            return new Tuple<ReceiveScanViewModel, EntityList<SparePartReceiveDetail>, EntityList<SparePartReceiveLot>, EntityList<SparePartReceiveSn>>
                (model, details, lots, sns);
        }

        /// <summary>
        /// 备件接收扫描-批次
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">备件接收模型</param>
        /// <returns>扫描信息</returns>
        public virtual ReceiveScanInfo ReceiveLotExecute(string sn, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var info = new ReceiveScanInfo();
            var old = Query<SparePartReceiveLot>().Where(p => p.LotNo == sn).Count();
            if (old > 0)
            {
                info.Success = false;
                info.Message = "批次号{0}已存在，请确认".L10nFormat(sn);
                return info;
            }
            var oldLot = Query<StoreSummaryLot>().Where(p => p.BatchNumber == sn).Count();
            if (oldLot > 0)
            {
                info.Success = false;
                info.Message = "批次号{0}已存在，请确认".L10nFormat(sn);
                return info;
            }
            var lotInfo = new SparePartReceiveLot();
            lotInfo.LotNo = sn;
            lotInfo.Qty = model.LotQty;
            lotInfo.SparePartReceiveDetailId = model.SparePartReceiveDetailId;
            info.Message = "".L10N();
            info.Success = true;
            info.LotInfo = lotInfo;
            return info;
        }

        /// <summary>
        /// 获取备件序列号
        /// </summary>
        /// <param name="partOutDepotDetailId">备件出库单</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>备件序列号</returns>
        public virtual EntityList<StoreSummaryDetail> GetStoreSummaryDetails(double? partOutDepotDetailId, PagingInfo pagingInfo, string keyword)
        {
            var list = new EntityList<StoreSummaryDetail>();
            if (partOutDepotDetailId.HasValue)
            {
                var partOutDepotDetail = GetById<PartOutDepotDetail>(partOutDepotDetailId.Value);
                if (partOutDepotDetail != null)
                {
                    list = Query<StoreSummaryDetail>().Where(p => p.StoreStatus == OrdNumStoreStatus.Outsourced && p.Id == partOutDepotDetail.SeriaNoRefId).ToList();
                }
            }
            else
            {
                list = Query<StoreSummaryDetail>().Where(p => p.StoreStatus == OrdNumStoreStatus.Outsourced)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.OrderNumberCode.Contains(keyword))
                .ToList(pagingInfo);
            }
            return list;
        }


        /// <summary>
        /// 备件接收扫描-序列号
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">备件接收模型</param>
        /// <returns>扫描信息</returns>
        public virtual ReceiveScanInfo ScanSnExecute(string sn, ReceiveScanViewModel model)
        {
            var info = new ReceiveScanInfo();
            try
            {
                if (model == null)
                {
                    throw new ValidationException("数据异常".L10N());
                }
                if (model.ReceiveType == ReceiveType.Outsourced)
                {
                    var storeSummaryDetail = Query<StoreSummaryDetail>().Where(p => p.StoreStatus == OrdNumStoreStatus.Outsourced && p.OrderNumberCode == sn
                    && p.StoreSummary.SparePartId == model.SparePartId).FirstOrDefault();

                    //委外返厂校验条码
                    OutsourcedCheck(model, storeSummaryDetail, "备件序列号编码");
                }
                else
                {
                    //校验序列号编码
                    StoreCheckOrderNumber(sn);
                }
                var snInfo = new SparePartReceiveSn();
                snInfo.Sn = sn;
                snInfo.SparePartReceiveDetailId = model.SparePartReceiveDetailId;
                info.Message = "请扫描序列号编码".L10N();
                info.Success = true;
                info.SnInfo = snInfo;
                return info;
            }
            catch (ValidationException ex)
            {
                info.Success = false;
                info.Message = ex.Message;
                return info;
            }
        }

        /// <summary>
        /// 备件接收扫描-原厂序列号
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">备件接收模型</param>
        /// <returns>扫描信息</returns>
        public virtual ReceiveScanInfo ScanOriginalSnExecute(string sn, ReceiveScanViewModel model)
        {
            var info = new ReceiveScanInfo();
            try
            {
                if (model == null)
                {
                    throw new ValidationException("数据异常".L10N());
                }
                var snInfo = new SparePartReceiveSn();
                if (model.ReceiveType == ReceiveType.Outsourced)
                {
                    var storeSummaryDetail = Query<StoreSummaryDetail>().Where(p => p.StoreStatus == OrdNumStoreStatus.Outsourced && p.OriginalSn == sn
                    && p.StoreSummary.SparePartId == model.SparePartId).FirstOrDefault();

                    //委外返厂校验条码
                    OutsourcedCheck(model, storeSummaryDetail, "原厂序列号");
                    snInfo.Sn = storeSummaryDetail.OrderNumberCode;
                }
                else
                {
                    //校验原厂序列号
                    StoreCheckOriginalSn(sn);
                    snInfo.Sn = RT.Service.Resolve<SparePartController>().GetSnNumber();
                }
                snInfo.OriginalSn = sn;
                snInfo.SparePartReceiveDetailId = model.SparePartReceiveDetailId;
                info.Message = "请扫描原厂序列号".L10N();
                info.Success = true;
                info.SnInfo = snInfo;
                return info;
            }
            catch (ValidationException ex)
            {
                info.Success = false;
                info.Message = ex.Message;
                return info;
            }
        }

        /// <summary>
        /// 委外返厂校验条码
        /// </summary>
        /// <param name="model">备件接收模型</param>
        /// <param name="storeSummaryDetail">序列号明细</param>
        /// <param name="msg">报错信息</param>
        private void OutsourcedCheck(ReceiveScanViewModel model, StoreSummaryDetail storeSummaryDetail, string msg)
        {
            if (storeSummaryDetail == null)
            {
                throw new ValidationException("请扫描委外维修的{0}".L10nFormat(msg));
            }
            if (model.PartOutDepotDetailId.HasValue)
            {
                var partOutDepotDetail = GetById<PartOutDepotDetail>(model.PartOutDepotDetailId.Value);
                if (partOutDepotDetail == null)
                {
                    throw new ValidationException("备件接收扫描数据异常".L10N());
                }
                if (storeSummaryDetail.Id != partOutDepotDetail.SeriaNoRefId)
                {
                    throw new ValidationException("请扫描委外维修的{0}".L10nFormat(msg));
                }
            }
        }

        /// <summary>
        /// 备件接收扫描-序列号和原厂序列号
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">备件接收模型</param>
        /// <returns>扫描信息</returns>
        public virtual ReceiveScanInfo ScanSnAndOriginalSnExecute(string sn, ReceiveScanViewModel model)
        {
            var info = new ReceiveScanInfo();
            try
            {
                if (model == null)
                {
                    throw new ValidationException("数据异常".L10N());
                }
                if (model.FirstSn.IsNullOrWhiteSpace())
                {
                    //校验序列号编码
                    StoreCheckOrderNumber(sn);
                    info.Success = true;
                    info.IsFirstSn = true;
                    info.Message = "请扫描原厂序列号".L10N();
                    return info;
                }
                else
                {
                    //校验原厂序列号
                    StoreCheckOriginalSn(sn);
                    var snInfo = new SparePartReceiveSn();
                    snInfo.Sn = model.FirstSn;
                    snInfo.OriginalSn = sn;
                    snInfo.SparePartReceiveDetailId = model.SparePartReceiveDetailId;
                    info.Message = "请扫描序列号编码".L10N();
                    info.Success = true;
                    info.SnInfo = snInfo;
                    return info;
                }
            }
            catch (ValidationException ex)
            {
                info.Success = false;
                info.Message = ex.Message;
                return info;
            }
        }

        /// <summary>
        /// 校验序列号编码
        /// </summary>
        /// <param name="sn">序列号编码</param>
        private void StoreCheckOrderNumber(string sn)
        {
            var old = Query<StoreSummaryDetail>().Where(p => p.OrderNumberCode == sn).Count();
            if (old > 0)
            {
                throw new ValidationException("序列号编码{0}已存在于备件序列号中，请确认".L10nFormat(sn));
            }
            var oldSn = Query<SparePartReceiveSn>().Where(p => p.Sn == sn).ToList();
            if (oldSn.Any())
            {
                var acceptanceSns = Query<SparePartAcceptanceSn>().Where(p => p.Sn == sn).ToList();
                if (!acceptanceSns.Any() || acceptanceSns.Any(p => p.AcceptanceResult != SIE.Common.InspectionResult.Fail))
                {
                    throw new ValidationException("序列号编码{0}已存在于备件序列号中，请确认".L10nFormat(sn));
                }
            }
        }

        /// <summary>
        /// 校验原厂序列号
        /// </summary>
        /// <param name="sn">原厂序列号</param>
        private void StoreCheckOriginalSn(string sn)
        {
            var old = Query<StoreSummaryDetail>().Where(p => p.OriginalSn == sn).Count();
            if (old > 0)
            {
                throw new ValidationException("原厂序列号{0}已存在，请确认".L10nFormat(sn));
            }
            var oldSn = Query<SparePartReceiveSn>().Where(p => p.OriginalSn == sn).ToList();
            if (oldSn.Any())
            {
                var acceptanceSns = Query<SparePartAcceptanceSn>().Where(p => p.OriginalSn == sn).ToList();
                if (!acceptanceSns.Any() || acceptanceSns.Any(p => p.AcceptanceResult != SIE.Common.InspectionResult.Fail))
                {
                    throw new ValidationException("原厂序列号编码{0}已存在于备件序列号中，请确认".L10nFormat(sn));
                }
            }
        }

        /// <summary>
        /// 保存备件接收扫描
        /// </summary>
        /// <param name="model">备件接收模型</param>
        /// <param name="detailList">明细信息</param>
        /// <param name="lotList">批次信息</param>
        /// <param name="snList">序列号信息</param>
        public virtual void SaveReceiveScan(ReceiveScanViewModel model, List<SparePartReceiveDetail> detailList, List<SparePartReceiveLot> lotList,
            List<SparePartReceiveSn> snList)
        {
            if (model == null || detailList == null || lotList == null || snList == null)
            {
                throw new ValidationException("数据异常，保存数据为空".L10N());
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var detail in detailList)
                {
                    if (detail.ControlMethod == ControlMethod.Batch)
                    {
                        detail.RecivedQty = lotList.Where(p => p.SparePartReceiveDetailId == detail.Id).Sum(p => p.Qty);
                    }
                    if (detail.ControlMethod == ControlMethod.Sn)
                    {
                        detail.RecivedQty = snList.Count(p => p.SparePartReceiveDetailId == detail.Id);
                    }
                    detail.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(detail);
                }

                foreach (var lot in lotList)
                {
                    RF.Save(lot);
                }

                foreach (var sn in snList)
                {
                    RF.Save(sn);
                }

                var hasReceivedCount = Query<SparePartReceiveDetail>().Where(x => x.RecivedQty > 0).Count();
                if (hasReceivedCount > 0)
                {
                    DB.Update<SparePartReceive>()
                        .Set(x => x.HasReceived, true)
                        .Where(x => x.Id == model.SparePartReceiveId)
                        .Execute();
                }
                else
                {
                    DB.Update<SparePartReceive>()
                    .Set(x => x.HasReceived, false)
                    .Where(x => x.Id == model.SparePartReceiveId)
                    .Execute();
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 批次管控确定
        /// </summary>
        /// <param name="model">备件接收模型</param>
        /// <returns>返回信息</returns>
        public virtual List<SparePartReceiveLot> LotDetermine(ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var list = new List<SparePartReceiveLot>();
            for (var i = 0; i < model.LotCount; i++)
            {
                var lotInfo = new SparePartReceiveLot();
                lotInfo.LotNo = RT.Service.Resolve<SparePartController>().GetLotNumber();
                lotInfo.Qty = model.LotQty;
                lotInfo.SparePartReceiveDetailId = model.SparePartReceiveDetailId;
                list.Add(lotInfo);
            }
            return list;
        }

        /// <summary>
        /// 序列号管控确定
        /// </summary>
        /// <param name="model">备件接收模型</param>
        /// <returns>返回信息</returns>
        public virtual List<SparePartReceiveSn> SnDetermine(ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var list = new List<SparePartReceiveSn>();
            if (model.ReceiveType == ReceiveType.Outsourced)
            {
                var storeSummaryDetail = GetById<StoreSummaryDetail>(model.StoreSummaryDetailId);
                if (storeSummaryDetail == null)
                {
                    throw new ValidationException("请选择返厂的序列号编码".L10N());
                }
                var snInfo = new SparePartReceiveSn();
                snInfo.Sn = storeSummaryDetail.OrderNumberCode;
                snInfo.OriginalSn = storeSummaryDetail.OriginalSn;
                snInfo.SparePartReceiveDetailId = model.SparePartReceiveDetailId;
                list.Add(snInfo);
            }
            else
            {
                for (var i = 0; i < model.CurrentQty; i++)
                {
                    var snInfo = new SparePartReceiveSn();
                    snInfo.Sn = RT.Service.Resolve<SparePartController>().GetSnNumber();
                    snInfo.SparePartReceiveDetailId = model.SparePartReceiveDetailId;
                    list.Add(snInfo);
                }
            }
            return list;
        }
    }
}
