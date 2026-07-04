using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 历史安灯查询视图
    /// </summary>
    [QueryEntity, Serializable]
    [Label("历史安灯查询视图")]
    public class AndonManageHisCriterial : AndonManageCriterial
    {
        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonManageController>().QueryAndonHistory(this);
        }
    }


}
