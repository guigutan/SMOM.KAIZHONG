using SIE.CSM.Suppliers;
using SIE.CSM.Suppliers.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.CSM.Suppliers.ViewModels
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class SupplierPswViewModelViewConfig:WebViewConfig<SupplierPswViewModel>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Supplier));
            View.WithoutPaging();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.UserCode).Show();
                View.Property(p => p.PassWord).Show();
            }
        }
    }
}
