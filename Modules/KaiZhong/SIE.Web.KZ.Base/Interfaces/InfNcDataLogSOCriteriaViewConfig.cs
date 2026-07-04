using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    public class InfNcDataLogSOCriteriaViewConfig : WebViewConfig<InfNcDataLogSOCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.InfType).ShowInList(width: 150);
                /*                View.Property(p => p.InfCode).ShowInList(width: 150);
                                View.Property(p => p.OperationType).ShowInList(width: 150);*/
                View.Property(p => p.WO);
                View.Property(p => p.DataJsons);
                //View.Property(p => p.CallResult).DefaultValue(CallResult.UnSave);
                View.Property(p => p.CallResult)
                    .UseDropDownEditor(() =>
                    {
                        return new Dictionary<CallResult, string>
                        {
                            { CallResult.UnSave, "失败" },
                            { CallResult.Success, "成功" }
                        };
                    }).DefaultValue(CallResult.UnSave);
                View.Property(p => p.BeginDate).Show(ShowInWhere.Detail).UseDateRangeEditor(p =>
                {
                    p.Format = "Y/M/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Today;
                });
                //View.Property(p => p.GroupGuid);
                /*                View.Property(p => p.DataJsons);
                                View.Property(p => p.ErrorMsg);
                                View.Property(p => p.GroupGuid);*/
            }
        }
    }
}
