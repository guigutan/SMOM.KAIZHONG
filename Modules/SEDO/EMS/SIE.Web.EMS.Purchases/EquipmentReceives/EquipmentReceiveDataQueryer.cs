using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.Equipments.EquipModels;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收查询器
    /// </summary>
    public class EquipmentReceiveDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的设备接收
        /// </summary>
        /// <returns>新的设备接收</returns>
        public EquipmentReceive GetNewEquipmentReceive()
        {
            return RT.Service.Resolve<EquipmentReceiveController>().GetNewEquipmentReceive();
        }

        /// <summary>
        /// 根据采购订单行获取设备型号数据
        /// </summary>
        /// <param name="orderItemId">采购订单行</param>
        /// <returns>设备型号数据</returns>
        public EquipModel GetEquipModelInfo(double orderItemId)
        {
            return RT.Service.Resolve<EquipmentReceiveController>().GetEquipModelInfo(orderItemId);
        }

        /// <summary>
        /// 获取设备接收扫描信息
        /// </summary>
        /// <param name="receiveId">设备接收id</param>
        /// <returns>设备接收扫描信息</returns>
        public ReceiveScanViewModel GetReceiveScanInfo(double receiveId)
        {
            return RT.Service.Resolve<EquipmentReceiveController>().GetReceiveScanInfo(receiveId);
        }

        /// <summary>
        /// 设备接收扫描
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">设备接收模型</param>
        /// <returns>扫描信息</returns>
        public ReceiveExecuteInfo ReceiveExecute(string sn, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            if (model.ScanEquip)
            {
                return RT.Service.Resolve<EquipmentReceiveController>().ScanEquipExecute(sn, model);
            }
            else if (model.ScanSn)
            {
                return RT.Service.Resolve<EquipmentReceiveController>().ScanSnExecute(sn, model);
            }
            else
            {
                return RT.Service.Resolve<EquipmentReceiveController>().ScanEquipAndSnExecute(sn, model);
            }
        }

        /// <summary>
        /// 委外返厂确定
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <returns>序列号信息</returns>
        public ReceiveExecuteInfo Determine(ReceiveScanViewModel model)
        {
            return RT.Service.Resolve<EquipmentReceiveController>().Determine(model);
        }

        /// <summary>
        /// 获取序列号列表信息
        /// </summary>
        /// <param name="ReceiveId">设备接收Id</param>
        /// <returns>序列号信息</returns>
        public EntityList<ReceiveScanSnViewModel> GetEquipmentReceiveSnInfo(double ReceiveId)
        {
            return RT.Service.Resolve<EquipmentReceiveController>().GetEquipmentReceiveSnInfo(ReceiveId, null, null);
        }

        /// <summary>
        /// 非委外返厂确定
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <returns>序列号信息</returns>
        public List<ReceiveScanSnViewModel> DetermineOnQty(ReceiveScanViewModel model)
        {
            return RT.Service.Resolve<EquipmentReceiveController>().DetermineOnQty(model);
        }

        /// <summary>
        /// 保存设备接收扫描
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <param name="snList">序列号信息</param>
        public void SaveReceiveScan(ReceiveScanViewModel model, List<ReceiveScanSnViewModel> snList)
        {
            RT.Service.Resolve<EquipmentReceiveController>().SaveReceiveScan(model, snList);
        }

        /// <summary>
        /// 序列号打印
        /// </summary>
        /// <param name="barcodeIds">序列号id列表</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult ReceiveSnPrint(List<double> barcodeIds, ReceiveSnPrintViewModel printInfo)
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
                var printable = new EquipReceiveSnPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(EquipReceiveSnPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【设备接收序列号】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
                var sns = RT.Service.Resolve<EquipmentReceiveSnController>().GetEquipmentReceiveSnsByIds(barcodeIds);
                var snList = new List<ReceiveScanSnViewModel>();
                foreach (var sn in sns)
                {
                    snList.Add(new ReceiveScanSnViewModel()
                    {
                        EquipmentReceiveDetailId = sn.EquipmentReceiveDetailId,
                        ReceiveLineNo = sn.ReceiveLineNo,
                        PurchaseOrderNo = sn.PurchaseOrderNo,
                        OrderItemNo = sn.OrderItemNo,
                        EquipModelCode = sn.EquipModelCode,
                        EquipModelName = sn.EquipModelName,
                        EquipmentCode = sn.EquipmentCode,
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
        /// 序列号打印
        /// </summary>
        /// <param name="snList">序列号列表</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult DeterminePrint(List<ReceiveScanSnViewModel> snList, ReceiveSnPrintViewModel printInfo)
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
                var printable = new EquipReceiveSnPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(EquipReceiveSnPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【设备接收序列号】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
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

    }
}
