using SIE.MES.ItemEquipAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount
{
    /// <summary>
    /// 模具与产品的关系实体查询
    /// </summary>
    public class EquipAccountItemCriterialViewConfig : WebViewConfig<EquipAccountItemCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.EquipAccount).UseDataSource((o, e, r) =>
                //{
                //    return RT.Service.Resolve<EquipAccountItemController>().GetEquipAccounts(e,r);
                //}).ShowInList(width: 150);
                View.Property(p => p.EquipAccountCode).ShowInList(width: 150);
                View.Property(p => p.EquipAccountName).ShowInList(width: 150);
                View.Property(p => p.Drawn).ShowInList(width: 150);
                View.Property(p => p.Item).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.OldItem).ShowInList(width: 150);
                View.Property(p => p.ItemMtart).ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.ProcessCode).ShowInList(width: 150);
                View.Property(p => p.UniqueCode).ShowInList(width: 150);
            }
        }
    }
}
