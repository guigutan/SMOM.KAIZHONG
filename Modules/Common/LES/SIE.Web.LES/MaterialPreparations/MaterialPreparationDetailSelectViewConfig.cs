using SIE.LES.MaterialPreparations;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单明细选择视图
    /// </summary>
    public class MaterialPreparationDetailSelectViewConfig : WebViewConfig<MaterialPreparationDetailSelect>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly().ShowInList();
                View.Property(p => p.ItemCode).Readonly().ShowInList();
                View.Property(p => p.ItemName).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList();
                View.Property(p => p.ItemConsumeMode).Readonly().ShowInList();
                View.Property(p => p.BomNeedQty).Readonly().ShowInList();
                View.Property(p => p.CanPrepareQty).Readonly().ShowInList();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }

    /// <summary>
    /// 备料需求单选择明细查询视图
    /// </summary>
    public class MaterialPreparationDtlSelCriteriaViewConfig : WebViewConfig<MaterialPreparationDtlSelCriteria>
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
