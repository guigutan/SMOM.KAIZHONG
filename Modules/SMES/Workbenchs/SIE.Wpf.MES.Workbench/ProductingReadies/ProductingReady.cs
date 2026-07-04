using SIE.MES.Workbench.ProductingReadies;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.Workbench.ProductingReadies
{
    /// <summary>
    /// 产前准备
    /// </summary>
    public class ProductingReadyInfo : ObservableObject
    {
        /// <summary>
        /// 班组/线别
        /// </summary>
        public string LineName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 计划生产日期
        /// </summary>
        public DateTime PlanProductDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 生产数量
        /// </summary>
        public decimal ProductQty
        {
            get { return GetProperty<decimal>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 物料状态
        /// </summary>
        public ReadyState ItemState
        {
            get { return GetProperty<ReadyState>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 工具状态
        /// </summary>
        public ReadyState ToolState
        {
            get { return GetProperty<ReadyState>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 人员状态
        /// </summary>
        public ReadyState PersonnelState
        {
            get { return GetProperty<ReadyState>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// ESOP状态
        /// </summary>
        public ReadyState EsopState
        {
            get { return GetProperty<ReadyState>(); }
            set { SetProperty(value); }
        }
        /// <summary>
        /// 品质预防
        /// </summary>
        public ReadyState QualityState
        {
            get { return GetProperty<ReadyState>(); }
            set { SetProperty(value); }
        }

    }
}
