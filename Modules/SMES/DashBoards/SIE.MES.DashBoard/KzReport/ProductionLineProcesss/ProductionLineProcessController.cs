using Microsoft.Scripting.Utils;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.ProductionLineProcesss
{
    public class ProductionLineProcessController : DomainController
    {

        /// <summary>
        /// 获取库存组织下的工序编码
        /// </summary>
        /// <param name="productLine"></param>
        /// <param name="plantName"></param>
        /// <returns></returns>
        public virtual List<DictionaryData> GetInvCodeProcessCode(string productLine, string plantName,List<string> processCodes)
        {
            var entityList = Query<ProductionLineProcess>()
                .WhereIf(!productLine.IsNullOrEmpty(), p => p.ProductLine == productLine)
                .WhereIf(!plantName.IsNullOrEmpty(), p => p.PlantName == plantName)
                .WhereIf(!(processCodes == null|| processCodes.Count==0), p => processCodes.Contains(p.ProcessCode)).ToList();
            var invCodes = entityList.Select(p => p.InventoryCode).Distinct().ToList();
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            foreach (var item in invCodes)
            {
                //RT.Service.Resolve<KzLoginController>().SetInvOrgByExternalId(item);
                var invCodeProcessCodes = entityList.Where(p => p.InventoryCode == item).Select(p => p.ProcessCode).ToList<string>();
                dic.Add(item, invCodeProcessCodes);
            }
            return dic.Select(p => new DictionaryData() { DicKey = p.Key, DicValue = p.Value }).ToList<DictionaryData>();
        }


    }
}
