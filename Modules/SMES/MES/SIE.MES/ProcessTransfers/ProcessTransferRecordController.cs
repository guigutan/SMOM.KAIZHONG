using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Configs;
using SIE.MES.WIP.Products;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;

namespace SIE.MES.ProcessTransfers
{
    public class ProcessTransferRecordController : DomainController
    {
        /// <summary>
        /// 获取交接记录
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="barcodetype">条码类型</param>
        /// <param name="processId">工序</param>
        /// <param name="type">0转入1转出</param>
        /// <returns></returns>
        public virtual ProcessTransferRecord GetRecord(string barcode, TransferBarcodeType barcodetype, double processId, int? type, DateTime? wipdate)
        {
            var q = Query<ProcessTransferRecord>().Where(p => p.Barcode == barcode && p.BarcodeType == barcodetype && p.ProcessId == processId);
            if (type == 0)
                q.Where(p => p.Type == TransferType.TransferIn);
            if (type == 1)
                q.Where(p => p.Type == TransferType.TransferOut);
            if (wipdate != null)
                q.Where(p => p.CreateDate > wipdate);
            return q.OrderByDescending(p => p.CreateDate).FirstOrDefault();
        }

        public virtual void SaveRecord(TransferType transferType, double woid, double productId, List<string> barcodes, TransferBarcodeType barcodeType, double? resourceId, double processId, decimal qty)
        {
            EntityList<ProcessTransferRecord> list = new EntityList<ProcessTransferRecord>();
            foreach (var barcode in barcodes)
            {
                ProcessTransferRecord record = new ProcessTransferRecord();
                record.Type = transferType;
                record.WorkOrderId = woid;
                record.ProductId = productId;
                record.Barcode = barcode;
                record.BarcodeType = barcodeType;
                record.ResourceId = resourceId;
                record.ProcessId = processId;
                record.OperateById = RT.IdentityId;
                record.OperateTime = DateTime.Now;
                record.Qty = qty;
                list.Add(record);
            }

            RF.Save(list);
        }
        public virtual WipProductProcess GetLastWipProcess(string barcode, double processId)
        {
            var q = Query<WipProductProcess>().Where(p => p.Version.Sn == barcode && p.ProcessId == processId);
            return q.OrderByDescending(p => p.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 获取工序交接校验配置项
        /// </summary>
        /// <param name="resouseId">资源Id</param>
        /// <param name="wipResourceMove">生产资源</param>
        /// <returns>是否校验</returns>
        public virtual bool GetProcessTransferCheck(double resouseId, WipResourceMove wipResourceMove)
        {
            if (wipResourceMove == null)
            {
                wipResourceMove = RF.GetById<WipResourceMove>(resouseId);
            }

            if (wipResourceMove == null)
            {
                throw new ValidationException("找不到资源ID为{0}的产线".L10nFormat(resouseId));
            }

            var resourceWarehouse = new ResourceWarehouse()
            {
                Id = wipResourceMove.Id.ToString(),
                Code = wipResourceMove.Code,
                Name = wipResourceMove.Name
            };

            var config = ConfigService.GetConfig(new ProcessTransferCheckConfig(), resourceWarehouse);

            if (config == null)
            {
                return false;
            }

            return config.IsCheck;
        }
    }
}
