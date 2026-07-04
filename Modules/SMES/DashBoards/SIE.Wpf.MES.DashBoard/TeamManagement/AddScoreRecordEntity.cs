using SIE.ObjectModel;

namespace SIE.Wpf.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// 看板加分项明细数据
    /// </summary>
    public class AddScoreRecordEntity : ObservableObject
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 明细
        /// </summary>
        public string DetailStr
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
