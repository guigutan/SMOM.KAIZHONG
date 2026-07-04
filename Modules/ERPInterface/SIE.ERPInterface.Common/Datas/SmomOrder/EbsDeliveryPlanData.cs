using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 发货计划分析数据
    /// </summary>
    [Serializable]
    public class EbsDeliveryPlanData : EbsOrderBaseData
    {
        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal RequireQty { get; set; }
         
        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 指定项目号
        /// </summary>
        public string ProjectNo
        {
            get;
            set;
        }

        /// <summary>
        /// 指定任务号
        /// </summary>
        public string TaskNo
        {
            get;
            set;
        }

        /// <summary>
        /// 指定批次号
        /// </summary>
        public string LotCode
        {
            get;
            set;
        }

        /// <summary>
        /// 指定生产批次
        /// </summary>
        public string ProductBatch
        {
            get;
            set;
        }
                
        /// <summary>
        /// 单据类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierCode { get; set; }

         
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        
        /// <summary>
        /// 明细来源Id
        /// </summary>
        public string ErpSplitFromDetailId { get; set; }

        /// <summary>
        /// ERP单位名称，可能是辅助单位，需要转换成主单位
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 预计发货日期，回传给ERP
        /// </summary>
        public string ScheduleShipDate { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 行状态(0可发货1已关闭2取消)
        /// </summary>
        public int LineState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 拆分来源行的状态，0可发货 1不可发货
        /// </summary>
        public int? ErpSplitFromState { get; set; }

        /// <summary>
        /// ERP行号
        /// </summary>
        public string ErpLineNo { get; set; }
    }
}
