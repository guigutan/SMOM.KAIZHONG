using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Barcodes.Barcodes.Enums;

namespace SIE.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("条码工序指派查询实体")]
    public class BarcodeProcessCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BarcodeProcessCriteria()
        {
            CrtTime = new DateRange { DateRangeType = DateRangeType.Today };
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BarcodeProcessCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 条码号 Sn
        /// <summary>
        /// 条码号
        /// </summary>
        [Label("条码号")]
        public static readonly Property<string> SnProperty = P<BarcodeProcessCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 指派状态 AssignState
        /// <summary>
        /// 指派状态
        /// </summary>
        [Label("指派状态")]
        public static readonly Property<AssignState?> AssignStateProperty = P<BarcodeProcessCriteria>.Register(e => e.AssignState);

        /// <summary>
        /// 指派状态
        /// </summary>
        public AssignState? AssignState
        {
            get { return this.GetProperty(AssignStateProperty); }
            set { this.SetProperty(AssignStateProperty, value); }
        }
        #endregion

        #region 创建时间 CrtTime
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CrtTimeProperty = P<BarcodeProcessCriteria>.Register(e => e.CrtTime);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CrtTime
        {
            get { return this.GetProperty(CrtTimeProperty); }
            set { this.SetProperty(CrtTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BarcodeProcessController>().QueryBarcodeProcess(this);
        }
    }
}
