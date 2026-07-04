using SIE.ObjectModel;

namespace SIE.Wpf.MES.Workbench.AndonAbnormals
{
    /// <summary>
    /// 安灯异常管理实体
    /// </summary>
    public class AndonAbnormalViewData : ObservableObject
    {
        /// <summary>
        /// 生产线Id
        /// </summary>
        public double ProductLineId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 生产线编码
        /// </summary>
        public string ProductLineCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 生产线名称
        /// </summary>
        public string ProductLineName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 呼叫时间
        /// </summary>
        public string CallTime
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 处理人Id
        /// </summary>
        public double ProcesserId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 处理人编号
        /// </summary>
        public string ProcesserCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 处理人姓名
        /// </summary>
        public string ProcesserName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 签到时间
        /// </summary>
        public string SignTime
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 处理状态
        /// </summary>
        public string ProcessStatus
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
