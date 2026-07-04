using SIE.Items;
using SIE.MES.Validitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Validitys
{
    /// <summary>
    /// 有效期标准维护查询视图
    /// </summary>
    public class ValidityStandardCriteriaViewConfig : WebViewConfig<ValidityStandardCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Item).UseDataSource((s, p, k) =>
            {
                return RT.Service.Resolve<ItemController>().GetItemDatas(p, k);
            }).Show();
            View.Property(p => p.ItemType).Show();
            View.Property(p => p.Effective).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; p.DateFormat = "Y-m-d"; }).Show();
            View.Property(p => p.Expiration).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; p.DateFormat = "Y-m-d"; }).Show();
        }
    }
}
