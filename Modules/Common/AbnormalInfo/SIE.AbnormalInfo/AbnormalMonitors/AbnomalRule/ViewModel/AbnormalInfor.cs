using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	public partial class AbnormalMonitorTab : ViewModel
    {


        #region 数据列字段 ColumnName
        /// <summary>
        /// 数据列字段
        /// </summary>
        [Label("数据列字段")]
        public static readonly Property<string> ColumnNameProperty = P<AbnormalMonitorTab>.Register(e => e.ColumnName);

        /// <summary>
        /// 数据列字段
        /// </summary>
        public string ColumnName
        {
            get { return GetProperty(ColumnNameProperty); }
            set { SetProperty(ColumnNameProperty, value); }
        }
        #endregion

        #region 字段备注 Comments
        /// <summary>
        /// 字段备注
        /// </summary>
        [Label("字段备注")]
        public static readonly Property<string> CommentsProperty = P<AbnormalMonitorTab>.Register(e => e.Comments);

        /// <summary>
        /// 字段备注
        /// </summary>
        public string Comments
        {
            get { return GetProperty(CommentsProperty); }
            set { SetProperty(CommentsProperty, value); }
        }
        #endregion
    }
}