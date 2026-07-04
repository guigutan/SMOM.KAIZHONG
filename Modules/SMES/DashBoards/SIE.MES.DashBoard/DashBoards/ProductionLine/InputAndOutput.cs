using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// 班次日统计
    /// </summary>
    public class InputAndOutput : ObservableObject
    {
        /// <summary>
        /// 采集日期
        /// </summary>
        private DateTime _collectDate;

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime CollectDate
        {
            get
            {
                return _collectDate;
            }

            set
            {
                _collectDate = value;
                OnPropertyChanged("CollectDate");
            }
        }

        /// <summary>
        /// 时间段
        /// </summary>
        private string _timeBetween;

        /// <summary>
        /// 时间段
        /// </summary>
        public string TimeBetween
        {
            get
            {
                return _timeBetween;
            }

            set
            {
                _timeBetween = value;
                OnPropertyChanged("TimeBetween");
            }
        }

        #region 目标产能 PlanQty
        /// <summary>
        /// 目标产能
        /// </summary>
        private decimal _planQty;

        /// <summary>
        /// 目标产能
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

        #region 投入 OnlineQty
        /// <summary>
        /// 投入
        /// </summary>
        private decimal? _onlineQty;

        /// <summary>
        /// 投入
        /// </summary>
        public decimal? OnlineQty
        {
            get
            {
                return _onlineQty ?? 0;
            }

            set
            {
                _onlineQty = value;
                OnPropertyChanged("OnlineQty");
            }
        }
        #endregion

        #region 产出 OfflineQty
        /// <summary>
        /// 产出
        /// </summary>
        private decimal? _offlineQty;

        /// <summary>
        /// 产出
        /// </summary>
        public decimal? OfflineQty
        {
            get
            {
                return _offlineQty ?? 0;
            }

            set
            {
                _offlineQty = value;
                OnPropertyChanged("OfflineQty");
            }
        }
        #endregion

        #region 不良数 NgQty
        /// <summary>
        /// 不良数
        /// </summary>
        private decimal _ngqty;

        /// <summary>
        /// 不良数
        /// </summary>
        public decimal NgQty
        {
            get
            {
                return _ngqty;
            }

            set
            {
                _ngqty = value;
                OnPropertyChanged("NgQty");
            }
        }
        #endregion

        #region 达成率 CompletionRate 
        /// <summary>
        /// 达成率
        /// </summary>
        private double _completionRate;

        /// <summary>
        /// 达成率
        /// </summary>
        public double CompletionRate
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

        #region 直通率 RolledYield
        /// <summary>
        /// 直通率
        /// </summary>
        private double _rolledYield;

        /// <summary>
        /// 直通率
        /// </summary>
        public double RolledYield
        {
            get
            {
                return _rolledYield;
            }

            set
            {
                _rolledYield = value;
                OnPropertyChanged("RolledYield");
            }
        }
        #endregion

        #region 达成率背景颜色 CRateBackgroundStr  
        /// <summary>
        /// 达成率背景颜色
        /// </summary>
        private string completionRateBackgroundStr;

        /// <summary>
        /// 达成率背景颜色
        /// </summary>
        public string CRateBackgroundStr
        {
            get
            {
                return completionRateBackgroundStr;
            }

            set
            {
                completionRateBackgroundStr = value;
                OnPropertyChanged("CRateBackgroundStr");
            }
        }
        #endregion

        #region 直通率背景颜色 RolledYieldBackgroundStr
        /// <summary>
        /// 直通率背景颜色
        /// </summary>
        private string _rolledYieldBackgroundStr;

        /// <summary>
        /// 直通率背景颜色
        /// </summary>
        public string RolledYieldBackgroundStr
        {
            get
            {
                return _rolledYieldBackgroundStr;
            }

            set
            {
                _rolledYieldBackgroundStr = value;
                OnPropertyChanged("RolledYieldBackgroundStr");
            }
        }
        #endregion
    }
}
