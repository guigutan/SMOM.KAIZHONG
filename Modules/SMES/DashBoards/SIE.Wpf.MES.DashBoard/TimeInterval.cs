using MimeKit;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.DashBoard
{
    /// <summary>
    /// 时间间隔类
    /// </summary>
    [Serializable]
    public class TimeInterval : ObservableObject
    {
        /// <summary>
        /// 时间类型
        /// </summary>
        public TimePart TimePart
        {
            get { return GetProperty<TimePart>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 时间值
        /// </summary>
        public double TimeValue
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
    }
}
