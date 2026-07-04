using SIE.LES.MaterialReturnApplys;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialReturnApplys
{
    /// <summary>
    /// 选择退料申请明细视图配置
    /// </summary>
    public class MaterialReturnApplyDetailSelectViewConfig : WebViewConfig<MaterialReturnApplyDetailSelect>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).Readonly().ShowInList();
                View.Property(p => p.ItemName).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.ReDetailQuality).Readonly().ShowInList();
                View.Property(p => p.CtrlMode).Readonly().ShowInList();
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList();
                View.Property(p => p.Label).Readonly().ShowInList(width: 150);
                View.Property(p => p.Lot).Readonly().ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }

    /// <summary>
    /// 查询视图
    /// </summary>
    public class MaterialReturnApplyDtlSelCriteriaViewConfig : WebViewConfig<MaterialReturnApplyDtlSelCriteria>
    {
        /// <summary>
        /// 查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExecuteQuery);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).Show();
            }
        }
    }
}
