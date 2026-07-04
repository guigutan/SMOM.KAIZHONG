using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonMonthReports
{
   /// <summary>
   /// 安灯统计月度报表返回值
   /// </summary>
    [Serializable]
    public class AndonMonthReportInfos
    {
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<AndonMonthReportViewModel> andonMonthReportViewModels { get; set; } = new List<AndonMonthReportViewModel>();
    }
}
