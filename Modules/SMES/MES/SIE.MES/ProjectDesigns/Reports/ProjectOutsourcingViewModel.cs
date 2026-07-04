using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号关联工序委外需求单
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号关联工序委外需求单")]
    public class ProjectOutsourcingViewModel : ViewModel
    {
        #region 委外需求单号 No
        /// <summary>
        /// 委外需求单号
        /// </summary>
        [Label("委外需求单号")]
        public static readonly Property<string> NoProperty = P<ProjectOutsourcingViewModel>.Register(e => e.No);

        /// <summary>
        /// 委外需求单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<ProjectOutsourcingViewModel>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProjectOutsourcingViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProjectOutsourcingViewModel>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 委外状态 OutsourcingState
        /// <summary>
        /// 委外状态
        /// </summary>
        [Label("委外状态")]
        public static readonly Property<OutsourcingState> OutsourcingStateProperty = P<ProjectOutsourcingViewModel>.Register(e => e.OutsourcingState);

        /// <summary>
        /// 委外状态
        /// </summary>
        public OutsourcingState OutsourcingState
        {
            get { return this.GetProperty(OutsourcingStateProperty); }
            set { this.SetProperty(OutsourcingStateProperty, value); }
        }
        #endregion

        #region 需求数量 RequestQty
        /// <summary>
        /// 需求数量
        /// </summary>
        [Label("需求数量")]
        public static readonly Property<decimal> RequestQtyProperty = P<ProjectOutsourcingViewModel>.Register(e => e.RequestQty);

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal RequestQty
        {
            get { return this.GetProperty(RequestQtyProperty); }
            set { this.SetProperty(RequestQtyProperty, value); }
        }
        #endregion

        #region 出库数 OutboundQty
        /// <summary>
        /// 出库数
        /// </summary>
        [Label("出库数")]
        public static readonly Property<decimal> OutboundQtyProperty = P<ProjectOutsourcingViewModel>.Register(e => e.OutboundQty);

        /// <summary>
        /// 出库数
        /// </summary>
        public decimal OutboundQty
        {
            get { return this.GetProperty(OutboundQtyProperty); }
            set { this.SetProperty(OutboundQtyProperty, value); }
        }
        #endregion

        #region 入库数 WarehousingQty
        /// <summary>
        /// 入库数
        /// </summary>
        [Label("入库数")]
        public static readonly Property<decimal> WarehousingQtyProperty = P<ProjectOutsourcingViewModel>.Register(e => e.WarehousingQty);

        /// <summary>
        /// 入库数
        /// </summary>
        public decimal WarehousingQty
        {
            get { return this.GetProperty(WarehousingQtyProperty); }
            set { this.SetProperty(WarehousingQtyProperty, value); }
        }
        #endregion

        #region 起始工序 BeginProcess
        /// <summary>
        /// 起始工序
        /// </summary>
        [Label("起始工序")]
        public static readonly Property<string> BeginProcessProperty = P<ProjectOutsourcingViewModel>.Register(e => e.BeginProcess);

        /// <summary>
        /// 起始工序
        /// </summary>
        public string BeginProcess
        {
            get { return this.GetProperty(BeginProcessProperty); }
            set { this.SetProperty(BeginProcessProperty, value); }
        }
        #endregion

        #region 结束工序 EndProcess
        /// <summary>
        /// 结束工序
        /// </summary>
        [Label("结束工序")]
        public static readonly Property<string> EndProcessProperty = P<ProjectOutsourcingViewModel>.Register(e => e.EndProcess);

        /// <summary>
        /// 结束工序
        /// </summary>
        public string EndProcess
        {
            get { return this.GetProperty(EndProcessProperty); }
            set { this.SetProperty(EndProcessProperty, value); }
        }
        #endregion

    }
}
