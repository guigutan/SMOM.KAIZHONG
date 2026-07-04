using SIE.EMS.SpareParts.Criterias;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.Criterias
{
    /// <summary>
    /// 备件查询 查询库存相关
    /// </summary>
    public class SparePartByEquipModelCriteriaViewConfig: WebViewConfig<SparePartByEquipModelCriteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p=>p.EquipModel);
            View.Property(p => p.EquipModelCode);
        }
    }
}
