using SIE.ObjectModel;

namespace SIE.Wpf.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// 产线物料需求看板数据实体
    /// </summary>
    public class ScoreRecordEntity : ObservableObject
    {
        /// <summary>
        /// 班组
        /// </summary>
        public string WorkGroupName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public string EmployeeName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 第一周
        /// </summary>
        public string FirstWeek
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 第二周
        /// </summary>
        public string SecondWeek
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 第三周
        /// </summary>
        public string ThirdWeek
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 第四周
        /// </summary>
        public string FourthWeek
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 第五周
        /// </summary>
        public string FifthWeek
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 月绩效评分
        /// </summary>
        public decimal MonthScore
        {
            get { return GetProperty<decimal>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 评分等级图
        /// </summary>
        public string ScorePic
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public double cellHeight
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double cellWidth
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double cellFontSize
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double headFontSize
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double imgHeight
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double imgWidth
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double imgRowWidth
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double headHeight
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
    }
}
