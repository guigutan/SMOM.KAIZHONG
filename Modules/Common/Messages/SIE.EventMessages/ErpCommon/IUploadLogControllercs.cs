using SIE.EventMessages.ErpCommon.Datas;
using SIE.EventMessages.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.ErpCommon
{
    [Services.Service(FallbackType = typeof(DefaultIUploadLogControllercs))]
    public interface IUploadLogControllercs
    {
        /// <summary>
        /// 更新余料称重记录的数量
        /// </summary>
        /// <param name="ids"></param>
        void EditScrapWeighingRecordQty(List<double> ids);

        /// <summary>
        /// 更新扣料记录的数量
        /// </summary>
        /// <param name="ids"></param>
        void EditDeductionRecordQty(List<double> ids);

        /// <summary>
        /// 更新标签事务上传状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        void UpdateWipBatchCreateTransaction(double id, string msg);

        /// <summary>
        /// 批次标签创建事务上传
        /// </summary>
        /// <param name="datas"></param>
        void SyncWipBatchCreateTransaction(List<SyncWipBatchData> datas);

        /// <summary>
        /// 委外需求单接口更新事务上传
        /// </summary>
        /// <param name="dtlIds"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        void RequestCreateTransaction(List<double> dtlIds, string msg, int type);

        /// <summary>
        /// 委外需求单创建事务上传
        /// </summary>
        void OutsourcingRequestCreateTransaction(List<OutReqCreateTransactionData> datas);

        /// <summary>
        /// 委外需求单收货提交报工
        /// </summary>
        /// <param name="datas"></param>
        void ProcessingInStockReportTransaction(List<ProcessingInStockReportTranData> datas);

        /// <summary>
        /// 发货确认提交SAP
        /// </summary>
        /// <param name="datas"></param>
        void OutboundConfirmTransaction(List<OutboundConfirmTransactionData> datas);

        /// <summary>
        /// 更新发货确认事务上传s
        /// </summary>
        /// <param name="zuid"></param>
        /// <param name="qty"></param>
        void UpdateOutboundConfirmTransaction(string zuid, decimal qty);

    }

    public class DefaultIUploadLogControllercs : IUploadLogControllercs
    {

        /// <summary>
        /// 更新余料称重记录的数量
        /// </summary>
        /// <param name="ids"></param>
        public void EditScrapWeighingRecordQty(List<double> ids)
        { 
            
        }

        /// <summary>
        /// 更新扣料记录的数量
        /// </summary>
        /// <param name="ids"></param>
        public void EditDeductionRecordQty(List<double> ids)
        { 
            
        }


        /// <summary>
        /// 更新发货确认事务上传s
        /// </summary>
        /// <param name="zuid"></param>
        /// <param name="qty"></param>
        public void UpdateOutboundConfirmTransaction(string zuid, decimal qty)
        {
            return;
        }

        /// <summary>
        /// 发货确认提交SAP
        /// </summary>
        /// <param name="datas"></param>
        public void OutboundConfirmTransaction(List<OutboundConfirmTransactionData> datas)
        {
            return;
        }

        /// <summary>
        /// 委外需求单收货提交报工
        /// </summary>
        /// <param name="datas"></param>
        public void ProcessingInStockReportTransaction(List<ProcessingInStockReportTranData> datas)
        {
            return;
        }

        /// <summary>
        /// 更新标签事务上传状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        public void UpdateWipBatchCreateTransaction(double id, string msg)
        { 
            
        }

        /// <summary>
        /// 批次标签创建事务上传
        /// </summary>
        /// <param name="datas"></param>
        public void SyncWipBatchCreateTransaction(List<SyncWipBatchData> datas)
        { 
        }

        /// <summary>
        /// 委外需求单接口更新事务上传
        /// </summary>
        /// <param name="dtlIds"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public void RequestCreateTransaction(List<double> dtlIds, string msg, int type)
        { }

        /// <summary>
        /// 委外需求单创建事务上传
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OutsourcingRequestCreateTransaction(List<OutReqCreateTransactionData> datas)
        {
            throw new NotImplementedException();
        }
    }
}
