using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// 工单信息
    /// </summary>
    public class WorkOrderInfo : ObservableObject
    {

        #region 工单编号 No
        /// <summary>
        /// 工单编号
        /// </summary>
        private string _no;

        /// <summary>
        /// 工单编号
        /// </summary>
        public string No
        {
            get
            {
                return _no;
            }

            set
            {
                _no = value;
                OnPropertyChanged("No");
            }
        }
        #endregion

        #region 工单类型 WorkOrderType
        /// <summary>
        /// 工单类型
        /// </summary>
        private string _workOrderType;

        /// <summary>
        /// 工单类型
        /// </summary>
        public string WorkOrderType
        {
            get
            {
                return _workOrderType;
            }

            set
            {
                _workOrderType = value;
                OnPropertyChanged("WorkOrderType");
            }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        private string _productCode;

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get
            {
                return _productCode;
            }

            set
            {
                _productCode = value;
                OnPropertyChanged("ProductCode");
            }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        private DateTime _planEndDate;

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get
            {
                return _planEndDate;
            }

            set
            {
                _planEndDate = value;
                OnPropertyChanged("PlanEndDate");
            }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        private Core.WorkOrders.WorkOrderState? _state;

        /// <summary>
        /// 工单状态
        /// </summary>
        public Core.WorkOrders.WorkOrderState? State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }
        #endregion

        #region 是否正在生产中 IsProducing
        /// <summary>
        /// 是否正在生产中
        /// </summary>
        private bool _isProducing;

        /// <summary>
        /// 是否正在生产中
        /// </summary>
        public bool IsProducing
        {
            get
            {
                return _isProducing;
            }

            set
            {
                _isProducing = value;
                OnPropertyChanged("IsProducing");
            }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        private decimal _planQty;

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get
            {
                return _planQty;
            }

            set
            {
                _planQty = value;
                OnPropertyChanged("PlanQty");
            }
        }
        #endregion

        #region 累计完成数 FinishQty
        /// <summary>
        /// 累计完成数
        /// </summary>
        private decimal _finishQty;

        /// <summary>
        /// 累计完成数
        /// </summary>
        public decimal FinishQty
        {
            get
            {
                return _finishQty;
            }

            set
            {
                _finishQty = value;
                OnPropertyChanged("FinishQty");
            }
        }
        #endregion

        #region 完成率 CompletionRate
        /// <summary>
        /// 完成率
        /// </summary>
        private decimal _completionRate;

        /// <summary>
        /// 完成率
        /// </summary>
        public decimal CompletionRate
        {
            get
            {
                return _completionRate;
            }

            set
            {
                _completionRate = value;
                OnPropertyChanged("CompletionRate");
            }
        }
        #endregion

        #region 超期时间(天) DelayDay
        /// <summary>
        /// 超期时间(天)
        /// </summary>
        private double _delayDay;

        /// <summary>
        /// 超期时间(天)
        /// </summary>
        public double DelayDay
        {
            get
            {
                return _delayDay;
            }

            set
            {
                _delayDay = value;
                OnPropertyChanged("DelayDay");
            }
        }
        #endregion

        #region 产线Id ResourceId
        /// <summary>
        /// 产线Id
        /// </summary>
        private double _resourceId;

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId
        {
            get
            {
                return _resourceId;
            }

            set
            {
                _resourceId = value;
                OnPropertyChanged("ResourceId");
            }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        private string _resourceName;

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get
            {
                return _resourceName;
            }

            set
            {
                _resourceName = value;
                OnPropertyChanged("ResourceName");
            }
        }
        #endregion
    }
}
