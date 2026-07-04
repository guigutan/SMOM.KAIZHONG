using SIE.ObjectModel;
using System.Windows.Media.Imaging;

namespace SIE.Wpf.MES.Workbench.EmployeeMarks
{
    /// <summary>
    /// 个人评分视图数据
    /// </summary>
    public class EmployeeMarkViewData : ObservableObject
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        private string _employeeCode = string.Empty;

        /// <summary>
        /// 员工名称
        /// </summary>
        private string _employeeName = string.Empty;

        /// <summary>
        /// 当前评分
        /// </summary>
        private string _curScore = string.Empty;

        /// <summary>
        /// 当前排名
        /// </summary>
        private string _curRank = string.Empty;

        /// <summary>
        /// 奖杯Map资源
        /// </summary>
        private BitmapImage _imgCup;

        /// <summary>
        /// 员工编码
        /// </summary>
        public string EmployeeCode
        {
            get { return _employeeCode; }
            set { _employeeCode = value; }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }

        /// <summary>
        /// 当前评分
        /// </summary>
        public string CurScore
        {
            get { return _curScore; }
            set { _curScore = value; }
        }

        /// <summary>
        /// 当前排名
        /// </summary>
        public string CurRank
        {
            get { return _curRank; }
            set { _curRank = value; }
        }

        /// <summary>
        /// 奖杯Map资源
        /// </summary>
        public BitmapImage ImgCup
        {
            get { return _imgCup; }
            set { _imgCup = value; }
        }
    }
}
