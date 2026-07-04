using SIE.Equipments.Abnormal;
using SIE.ObjectModel;

namespace SIE.Wpf.MES.Workbench.LineStates
{
    /// <summary>
    /// 产线状态信息
    /// </summary>
    public class LineState : ObservableObject
    {
        public double LineId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public string LineName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public ExceptionStopType State
        {
            get { return GetProperty<ExceptionStopType>(); }
            set { SetProperty(value); }
        }
    }
}
