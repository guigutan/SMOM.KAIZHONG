using SIE.LES.MaterialPreparations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单取消记录视图配置
    /// </summary>
    public class MaterialPreCancelRecordViewConfig : WebViewConfig<MaterialPreCancelRecord>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.MpNo).ShowInList();
                View.Property(p => p.ItemCode).ShowInList();
                View.Property(p => p.ItemExtPropName).ShowInList();
                View.Property(p => p.CancelQty).ShowInList();
            }
        }
    }
}
