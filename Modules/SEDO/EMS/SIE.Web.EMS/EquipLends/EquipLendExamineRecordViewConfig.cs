using SIE.EMS.EquipLends;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipLends
{
    /// <summary>
    /// 设备借还审核记录视图配置
    /// </summary>
    public class EquipLendExamineRecordViewConfig : WebViewConfig<EquipLendExamineRecord>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ExamineType).Readonly().ShowInList();
                View.Property(p => p.ExamineResult).Readonly().ShowInList();
                View.Property(p => p.Employee).Readonly().ShowInList();
                View.Property(p => p.ExamineDate).Readonly().ShowInList();
                View.Property(p => p.Remark).Readonly().ShowInList(width: 200);
            }
        }
    }
}
