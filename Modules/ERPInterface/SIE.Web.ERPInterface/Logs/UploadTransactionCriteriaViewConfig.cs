using SIE.Domain;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.Logs
{
    public class UploadTransactionCriteriaViewConfig : WebViewConfig<UploadTransactionCriteria>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderType).UseEnumEditor("KZ").Show() ;
                View.Property(p => p.TransactionCode).Show();
                View.Property(p => p.TransactionType).UseEnumEditor("KZ").Show();
                View.Property(p => p.State).UseEnumMutilEditor(x => x.EnumType = typeof(ProcessState)).DefaultValue("2,3").Show(ShowInWhere.Detail);
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.Bismt).Show();
                View.Property(p => p.WoNo).Show();
                //            View.Property(p => p.WorkShopId).Show(ShowInWhere.Detail).UseDataSource((source, pagingInfo, keyword) =>
                //{
                //    var entity = source as UploadTransactionCriteria;
                //    var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                //    if (workshop == null || workshop.Count <= 0)
                //        return new EntityList<Enterprise>();
                //    workshop.ForEach(p => p.TreePId = null);
                //    return workshop;
                //});
                View.Property(p => p.WorkShopCode).Show();
                View.Property(p => p.LotCode).Show();
                View.Property(p => p.ProcessCode).Show();
                View.Property(p => p.Mblnr).Show();
                View.Property(p => p.Mjahr).Show();
                View.Property(p => p.ProcessMessage).Show();
                View.Property(p => p.Zuid).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; }).Show();//
            }
        }
    }
}
