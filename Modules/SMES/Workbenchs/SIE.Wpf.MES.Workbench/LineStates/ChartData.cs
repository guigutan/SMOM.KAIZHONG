using SIE.ObjectModel;

namespace SIE.Wpf.MES.Workbench.LineStates
{
    /// <summary>
    /// 产线状态--圆环图数据类
    /// </summary>
    public class ChartData : ObservableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 值
        /// </summary>
        public decimal Value
        {
            get { return GetProperty<decimal>(); }
            set { SetProperty(value); }
        }
    }
}
