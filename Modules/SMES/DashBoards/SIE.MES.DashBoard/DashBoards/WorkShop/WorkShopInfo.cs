using SIE.ObjectModel;

namespace SIE.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// 车间看板信息
    /// </summary>
    public class WorkShopInfo : ObservableObject
    {
        #region 生产线 Line
        /// <summary>
        /// 生产线
        /// </summary>
        private string _line;

        /// <summary>
        /// 生产线
        /// </summary>
        public string Line
        {
            get
            {
                return _line;
            }

            set
            {
                _line = value;
                OnPropertyChanged("Line");
            }
        }
        #endregion

        #region 计划量 PlanQty
        /// <summary>
        /// 计划量
        /// </summary>
        private decimal _planQty;

        /// <summary>
        /// 计划量
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

        #region 已完量 FinishQty
        /// <summary>
        /// 已完量
        /// </summary>
        private decimal _finishQty;

        /// <summary>
        /// 已完量
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

        #region 小时产量 HourOutputQty
        /// <summary>
        /// 小时产量
        /// </summary>
        private decimal _hourOutputQty;

        /// <summary>
        /// 小时产量
        /// </summary>
        public decimal HourOutputQty
        {
            get
            {
                return _hourOutputQty;
            }

            set
            {
                _hourOutputQty = value;
                OnPropertyChanged("HourOutputQty");
            }
        }
        #endregion

        #region 直通率 ThroughRate
        /// <summary>
        /// 直通率
        /// </summary>
        private decimal _throughRate;

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal ThroughRate
        {
            get
            {
                return _throughRate;
            }

            set
            {
                _throughRate = value;
                OnPropertyChanged("ThroughRate");
            }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        private string _state;

        /// <summary>
        /// 状态
        /// </summary>
        public string State
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
    }
}