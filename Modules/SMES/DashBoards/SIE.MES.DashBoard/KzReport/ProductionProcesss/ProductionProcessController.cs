using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.ProductionProcesss
{
    public partial class ProductionProcessController:DomainController
    {
        /// <summary>
        /// 获取产能工序
        /// </summary>
        /// <param name="productLine"></param>
        /// <param name="plantCode"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public virtual EntityList<ProductionProcess> GetProductionProcesses(string productLine, string plantCode, string processCode)
        {
            var query = Query<ProductionProcess>();
            query.WhereIf(!productLine.IsNullOrEmpty(),p=>p.ProductLine == productLine);
            query.WhereIf(!plantCode.IsNullOrEmpty(), p => p.PlantCode == plantCode);
            query.WhereIf(!processCode.IsNullOrEmpty(), p => p.ProcessCode == processCode);
            return query.ToList();
        }
    }
}
