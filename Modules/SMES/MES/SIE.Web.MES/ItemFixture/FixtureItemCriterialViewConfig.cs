using SIE.MES.ItemFixture;
using SIE.MES.ItemProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemFixture
{
    /// <summary>
    /// 工装与产品的关系实体查询
    /// </summary>
    public class FixtureItemCriterialViewConfig : WebViewConfig<FixtureItemCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FixtureUphold).ShowInList(width: 150).HasLabel("工装唯一码");
                View.Property(p => p.FixtureName).ShowInList(width: 150).HasLabel("工装物料描述");
                View.Property(p => p.Drawn).ShowInList(width: 150).HasLabel("工装图号");
                View.Property(p => p.Item).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                //View.Property(p => p.).ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.ProcessCode).ShowInList(width: 150);
            }
        }
    }
}
