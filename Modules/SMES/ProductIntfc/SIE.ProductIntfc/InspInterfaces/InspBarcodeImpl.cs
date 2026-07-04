using Newtonsoft.Json;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Inspection;
using SIE.EventMessages.MES.Inspection.Models;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.InspSettings;
using SIE.Utils;
using System;
using System.Collections.Generic;

namespace SIE.ProductIntfc.InspInterfaces
{
    /// <summary>
    /// 条码报检接口实现
    /// </summary>
    public class InspBarcodeImpl : IInspBarcode
    {
        /// <summary>
        /// 获取报检条码
        /// </summary>
        /// <param name="queryInfo">报检条码查询信息</param>
        /// <returns>报检条码</returns>
        public IList<InspBarcodeInfo> GetInspBarcodes(BarcodeQueryInfo queryInfo)
        {
            SaveGetInspBarcodesLog(queryInfo);
            if (!Enum.IsDefined(typeof(InspType), queryInfo.InspType))
                throw new ValidationException("检验方式不存在".L10N());
            InspType inspType = (InspType)queryInfo.InspType;
            return RT.Service.Resolve<InspRecordController>().GetInspBarcodeInfos(queryInfo.WorkOrderId, inspType);
        }

        /// <summary>
        /// 保存获取报检条码日志
        /// </summary>
        /// <param name="queryInfo">报检条码查询信息</param>
        private void SaveGetInspBarcodesLog(BarcodeQueryInfo queryInfo)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(queryInfo);
                var inputValue = "报检条码查询信息:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IInspBarcode",
                    Method = "GetInspBarcodes",
                    ControllerName = "InspBarcodeImpl",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 条码报检
        /// </summary>
        /// <param name="inspBarcodeIds">报检条码ID列表</param>
        /// <param name="inspType">报检类型 0成品 1首件 2抽检</param>
        public void BarcodeInsp(List<double> inspBarcodeIds, int inspType)
        {
            SaveBarcodeInspLog(inspBarcodeIds, inspType);

            if (inspBarcodeIds == null) 
            {
                throw new ValidationException("报检条码ID列表不能为空".L10N());
            }

            if (inspBarcodeIds.Count == 0)
            {
                return;
            }

            if (inspType == 0) 
            {
                RT.Service.Resolve<InspRecordController>().ProductInsp(inspBarcodeIds);
            }
            else 
            {
                throw new ValidationException("报检方式不支持".L10N());
            }
        }

        /// <summary>
        /// 保存条码报检日志
        /// </summary>
        /// <param name="inspBarcodeIds">报检条码ID列表</param>
        /// <param name="inspType">报检类型 0成品 1首件 2抽检</param>
        private void SaveBarcodeInspLog(List<double> inspBarcodeIds, int inspType)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(inspBarcodeIds);
                var inputValue = "报检条码ID列表:{0};报检类型:{1}".L10nFormat(strValue, EnumViewModel.EnumToLabel((InspType)inspType).L10N());
                var log = new InterfaceLog()
                {
                    Name = "IInspBarcode",
                    Method = "BarcodeInsp",
                    ControllerName = "InspBarcodeImpl",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }
    }
}