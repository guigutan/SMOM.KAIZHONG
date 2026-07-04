using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    public class InfDataLogCriteriaViewConfig : WebViewConfig<InfDataLogCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.InfType).UseEnumEditor(p => {
                    p.Editable = true;
                    p.AllowBlank = true;
                    p.XType = "EnumFilterEditor";
                }).Show(ShowInWhere.Detail);
                View.Property(p => p.CallDirection).Show(ShowInWhere.Detail);
                View.Property(p => p.CallResult).Show(ShowInWhere.Detail);
                View.Property(p => p.Remark).Show(ShowInWhere.Detail);
                View.Property(p => p.TipMsg).Show(ShowInWhere.Detail);
                View.Property(p => p.ErrorMsg).Show(ShowInWhere.Detail);
                View.Property(p => p.RequestContent).Show(ShowInWhere.Detail);
                View.Property(p => p.ResponseContent).Show(ShowInWhere.Detail);
                View.Property(p => p.BeginDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/M/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Today;
                });
            }
        }
    }
}
