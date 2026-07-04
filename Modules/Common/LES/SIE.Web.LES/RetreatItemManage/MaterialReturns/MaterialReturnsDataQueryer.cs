using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.LES.RetreatItemManage;
using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.Packages.ItemLabels;
using SIE.Resources;
using SIE.Resources.WipResources;
using SIE.Web.Data;
using System.Linq;
using System;

namespace SIE.Web.LES.RetreatItemManage.MaterialReturns
{
    /// <summary>
    /// 查询器
    /// </summary>
    public class MaterialReturnsDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建新的退料
        /// </summary>
        /// <returns></returns>
        public virtual MaterialReturn GetNewMaterialReturns()
        {
            return new MaterialReturn()
            {
                EmployeeId = RT.IdentityId,
                NO = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialCode()
            };

        }


        /// <summary>
        /// 获取查询的标签数据
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturn> RetrunSearch(string keyWord)
        {
            EntityList<MaterialReturn> materialReturns = RT.Service.Resolve<MaterialReturnController>().GetSearch(keyWord);
            return materialReturns;
        }
    }
}
