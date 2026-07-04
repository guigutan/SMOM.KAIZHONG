using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.EMS.Purchases.SparePartReceives.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收查询器
    /// </summary>
    public class SparePartReceiveDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的备件接收
        /// </summary>
        /// <returns>新的备件接收</returns>
        public SparePartReceive GetNewSparePartReceive()
        {
            return RT.Service.Resolve<SparePartReceiveController>().GetNewSparePartReceive();
        }

        /// <summary>
        /// 根据采购订单行获取备件数据
        /// </summary>
        /// <param name="orderItemId">采购订单行</param>
        /// <returns>备件数据</returns>
        public SparePart GetSparePartInfo(double orderItemId)
        {
            return RT.Service.Resolve<SparePartReceiveController>().GetSparePartInfo(orderItemId);
        }

        /// <summary>
        /// 一键接收
        /// </summary>
        /// <param name="receiveId">接收id</param>
        public void OnekeyReceive(double receiveId)
        {
            RT.Service.Resolve<SparePartReceiveController>().OnekeyReceive(receiveId);
        }

        /// <summary>
        /// 批次打印
        /// </summary>
        /// <param name="barcodeIds">条码id列表</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult LotPrint(List<double> barcodeIds, SIE.EMS.Purchases.EquipmentReceives.ReceiveSnPrintViewModel printInfo)
        {
            if (printInfo == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var prtResult = new QRCodePrintResult();
            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(printInfo.TemplateId);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new SparePartLotSnPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(SparePartLotSnPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【备件接收条码】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
                var sns = RT.Service.Resolve<SparePartReceiveController>().GetSparePartReceiveLotsByIds(barcodeIds);
                var snList = new List<SparePartLotSnViewModel>();
                foreach (var sn in sns)
                {
                    snList.Add(new SparePartLotSnViewModel()
                    {
                        ReceiveLineNo = sn.LineNo,
                        PurchaseOrderNo = sn.PurchaseOrderNo,
                        OrderItemNo = sn.PurchaseOrderItemLineNo,
                        SparePartCode = sn.SparePartCode,
                        SparePartName = sn.SparePartName,
                        LotNo = sn.LotNo,
                        Qty = sn.Qty
                    });
                }
                prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return snList;
                });
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }

        /// <summary>
        /// 序列号打印
        /// </summary>
        /// <param name="barcodeIds">条码id列表</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult SnPrint(List<double> barcodeIds, SIE.EMS.Purchases.EquipmentReceives.ReceiveSnPrintViewModel printInfo)
        {
            if (printInfo == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var prtResult = new QRCodePrintResult();
            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(printInfo.TemplateId);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new SparePartLotSnPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(SparePartLotSnPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【备件接收条码】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
                var sns = RT.Service.Resolve<SparePartReceiveController>().GetSparePartReceiveSnsByIds(barcodeIds);
                var snList = new List<SparePartLotSnViewModel>();
                foreach (var sn in sns)
                {
                    snList.Add(new SparePartLotSnViewModel()
                    {
                        ReceiveLineNo = sn.LineNo,
                        PurchaseOrderNo = sn.PurchaseOrderNo,
                        OrderItemNo = sn.PurchaseOrderItemLineNo,
                        SparePartCode = sn.SparePartCode,
                        SparePartName = sn.SparePartName,
                        Sn = sn.Sn,
                        OriginalSn = sn.OriginalSn
                    });
                }
                prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return snList;
                });
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }

        /// <summary>
        /// 获取备件接收扫描信息
        /// </summary>
        /// <param name="receiveId">备件接收id</param>
        /// <returns>备件接收扫描信息</returns>
        public Tuple<ReceiveScanViewModel, EntityList<SparePartReceiveDetail>, EntityList<SparePartReceiveLot>, EntityList<SparePartReceiveSn>> GetReceiveScanInfo(double receiveId)
        {
            return RT.Service.Resolve<SparePartReceiveScanController>().GetReceiveScanInfo(receiveId);
        }

        /// <summary>
        /// 备件接收扫描-批次
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">备件接收模型</param>
        /// <returns>扫描信息</returns>
        public ReceiveScanInfo ReceiveLotExecute(string sn, ReceiveScanViewModel model)
        {
            return RT.Service.Resolve<SparePartReceiveScanController>().ReceiveLotExecute(sn, model);
        }

        /// <summary>
        /// 备件接收扫描-序列号
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">备件接收模型</param>
        /// <returns>扫描信息</returns>
        public ReceiveScanInfo ReceiveSnExecute(string sn, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            if (model.ScanEquip)
            {
                return RT.Service.Resolve<SparePartReceiveScanController>().ScanSnExecute(sn, model);
            }
            else if (model.ScanSn)
            {
                return RT.Service.Resolve<SparePartReceiveScanController>().ScanOriginalSnExecute(sn, model);
            }
            else
            {
                return RT.Service.Resolve<SparePartReceiveScanController>().ScanSnAndOriginalSnExecute(sn, model);
            }
        }

        /// <summary>
        /// 保存备件接收扫描
        /// </summary>
        /// <param name="model">备件接收模型</param>
        /// <param name="detailList">明细信息</param>
        /// <param name="lotList">批次信息</param>
        /// <param name="snList">序列号信息</param>
        public void SaveReceiveScan(ReceiveScanViewModel model, List<SparePartReceiveDetail> detailList, List<SparePartReceiveLot> lotList, List<SparePartReceiveSn> snList)
        {
            RT.Service.Resolve<SparePartReceiveScanController>().SaveReceiveScan(model, detailList, lotList, snList);
        }

        /// <summary>
        /// 批次管控确定
        /// </summary>
        /// <param name="model">备件接收模型</param>
        /// <returns>返回信息</returns>
        public List<SparePartReceiveLot> LotDetermine(ReceiveScanViewModel model)
        {
            return RT.Service.Resolve<SparePartReceiveScanController>().LotDetermine(model);
        }

        /// <summary>
        /// 序列号管控确定
        /// </summary>
        /// <param name="model">备件接收模型</param>
        /// <returns>返回信息</returns>
        public List<SparePartReceiveSn> SnDetermine(ReceiveScanViewModel model)
        {
            return RT.Service.Resolve<SparePartReceiveScanController>().SnDetermine(model);
        }

        /// <summary>
        /// 批次序列号打印
        /// </summary>
        /// <param name="lotList">批次列表</param>
        /// <param name="snList">序列号列表</param>
        /// <param name="model">备件接收模型</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult DeterminePrint(List<SparePartReceiveLot> lotList, List<SparePartReceiveSn> snList, ReceiveScanViewModel model,
            SIE.EMS.Purchases.EquipmentReceives.ReceiveSnPrintViewModel printInfo)
        {
            if (model == null || printInfo == null || lotList == null || snList == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var prtResult = new QRCodePrintResult();
            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(printInfo.TemplateId);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new SparePartLotSnPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(SparePartLotSnPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【备件接收条码】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
                var snModels = new List<SparePartLotSnViewModel>();
                if (model.ControlMethod == ControlMethod.Batch)
                {
                    foreach (var sn in lotList)
                    {
                        snModels.Add(new SparePartLotSnViewModel()
                        {
                            ReceiveLineNo = sn.LineNo,
                            PurchaseOrderNo = sn.PurchaseOrderNo,
                            OrderItemNo = sn.PurchaseOrderItemLineNo,
                            SparePartCode = sn.SparePartCode,
                            SparePartName = sn.SparePartName,
                            LotNo = sn.LotNo,
                            Qty = sn.Qty
                        });
                    }
                }
                else
                {
                    foreach (var sn in snList)
                    {
                        snModels.Add(new SparePartLotSnViewModel()
                        {
                            ReceiveLineNo = sn.LineNo,
                            PurchaseOrderNo = sn.PurchaseOrderNo,
                            OrderItemNo = sn.PurchaseOrderItemLineNo,
                            SparePartCode = sn.SparePartCode,
                            SparePartName = sn.SparePartName,
                            Sn = sn.Sn,
                            OriginalSn = sn.OriginalSn
                        });
                    }
                }
                prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return snModels;
                });
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }
    }
}
