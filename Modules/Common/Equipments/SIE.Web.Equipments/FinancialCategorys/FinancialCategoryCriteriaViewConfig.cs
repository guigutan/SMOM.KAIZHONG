using SIE.Equipments.FinancialCategorys;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Equipments.FinancialCategorys
{

    /// <summary>
    /// 查询视图
    /// </summary>
    public class FinancialCategoryCriteriaViewConfig : WebViewConfig<FinancialCategoryCriteria>
    {
       /// <summary>
       /// 配置查询视图
       /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Desc);
            View.Property(p=>p.CreationDate).UseDateRangeEditor(p=>p.DateRangeType= ObjectModel.DateRangeType.All);
        }

        
    }
}
