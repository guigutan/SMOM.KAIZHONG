using SIE.EMS.Equipments.Accounts.Criterias;
using SIE.Equipments.EquipModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Equipments.Accounts.Criterias
{
    /// <summary>
    /// 设备类型查找台账查询视图配置
    /// </summary>
    public class AccountByModleTypeCriteriaViewConfig:WebViewConfig<AccountByModleTypeCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p=>p.EquipModel).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
            }).Readonly();
        }
    }
}
